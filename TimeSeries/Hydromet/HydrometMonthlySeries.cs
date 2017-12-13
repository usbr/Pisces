using Reclamation.Core;
using System;
using System.Data;

namespace Reclamation.TimeSeries.Hydromet
{

    public enum TimePostion { FirstOfMonth, MidMonth, EndOfMonth, Automatic };

    public class HydrometMonthlySeries: Series
    {

        /*        Fc is the final adopted forecast
                  Fcn is the raw multiple linear regression forecast
         * 
        These are the forecasts that were produced AT the time, using whatever 
         * forecast equation that was in play, and not necessarily what the current 
         * forecast equation(s) would come up with.  However, we’ve done a forecast regeneration, 
         * which back generated forecasts using both MLR and the new principal components 
         * analysis (PCA) forecasts.   Those codes would be “HIST” for the mlr, and PCA.
         * 
         * Ted Day
         */

        public string Cbtt
        {
            get { return this.m_cbtt; }
        }
        public string Pcode
        {
            get { return this.m_pcode; }
        }

        string m_cbtt;
        string m_pcode;
        HydrometHost server = HydrometHost.PN;
        public HydrometMonthlySeries(string cbtt, string pcode, HydrometHost server=HydrometHost.PN)
        {
            this.TimeInterval = TimeSeries.TimeInterval.Monthly;
            this.Units = "";
            ReadOnly = true;
            this.m_cbtt = cbtt;
            this.m_pcode = pcode;
            this.Table.TableName = TimeSeriesName.GetTableName(server.ToString(), TimeSeries.TimeInterval.Monthly,
                cbtt, pcode);
            this.Name = FindName(cbtt);
            this.Parameter = pcode;
            this.HasFlags = true;
            this.Units = LookupUnits(pcode);
            this.Appearance.LegendText = cbtt + " " + pcode;
            this.server = server;
            Provider = "HydrometMonthlySeries";
            ConnectionString = "server=" + server.ToString()
            + ";cbtt=" + cbtt + ";pcode=" + pcode + ";LastUpdate=" + DateTime.Now.ToString(DateTimeFormatInstantaneous);

            this.Table.TableName = "monthly_" + cbtt + "_" + pcode;
        }

        public HydrometMonthlySeries(TimeSeriesDatabase db, Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow sr)
            : base( db, sr)
        {
            string str = ConnectionStringToken("server");
            if( str.Trim() != "")
               server = (HydrometHost)Enum.Parse(typeof(HydrometHost), str);

            m_pcode = ConnectionStringToken("pcode");
            m_cbtt = ConnectionStringToken("cbtt");

        }

        protected override Series CreateFromConnectionString()
        {
            string str = ConnectionStringToken("server");
            HydrometHost svr = HydrometHost.PN;
            if( str.Trim() != "")
                 svr = (HydrometHost)Enum.Parse(typeof(HydrometHost), str);

            HydrometMonthlySeries s = new HydrometMonthlySeries(
            ConnectionStringToken("cbtt"), ConnectionStringToken("pcode"),svr );

            return s;
        }

        protected override void ReadCore()
        {
            if (m_db != null)
                ReadCore(TimeSeriesDatabase.MinDateTime, TimeSeriesDatabase.MaxDateTime);
            else
                ReadCore(new DateTime(1850, 1, 1), DateTime.Now.Date.AddDays(-1));
        }

        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            Clear();

            if (m_db == null || HydrometInfoUtility.WebOnly)
            {
                ReadMpollFromHydromet(t1, t2);
            }
            else
            {
                if (HydrometInfoUtility.AutoUpdate)
                {
                    if (t2 >= DateTime.Now.Date && t2.Year < 6189)
                    { // don't waste time looking to the future
                        t2 = DateTime.Now.AddDays(-1).Date;
                    }
                    base.UpdateCore(t1, t2, true);
                }
                base.ReadCore(t1, t2);
            }
            
          

       }

      

        public static bool ConvertToAcreFeet = true;
        public static HydrometDataCache Cache ; // in-memory cache used for forecasting

       // public bool FromCache = false;


        private void ReadMpollFromHydromet(DateTime t1, DateTime t2)
        {
            string key = m_cbtt + " " + m_pcode;
            int tableIndex = -1;
            if (Cache != null 
                && (tableIndex = Cache.TableIndex(key, TimeSeries.TimeInterval.Monthly, t1, t2)) >=0 ) 
            {
                    int idx = Cache.DataSet.Tables[tableIndex].Columns.IndexOf(key);
                    if (idx < 0)
                        throw new Exception("oops");
                        //Console.WriteLine("Cache hit for: " + key);
                        ReadFromTable(Cache.DataSet.Tables[tableIndex], idx, idx + 1, t1, t2);
                        if (ConvertToAcreFeet)
                            ConvertFromThousandAcreFeetToAcreFeet(this);
                     //   FromCache = true;
                
            }
            else
            {
                var tbl = HydrometDataUtility.MPollTable(this.server, key, t1, t2);
                ReadFromTable(tbl, 1, 2, t1, t2);
                if (ConvertToAcreFeet)
                ConvertFromThousandAcreFeetToAcreFeet(this);
            }
        }

 

        //public TimePostion TimePostion = TimePostion.EndOfMonth;
        public TimePostion TimePostion = TimePostion.Automatic;

        internal void ReadFromTable(DataTable tbl,int dataIndex, int flagIndex, DateTime t1, DateTime t2)
        {
            this.Clear();
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                string flag = tbl.Rows[i][flagIndex].ToString();

                DateTime t = Convert.ToDateTime(tbl.Rows[i][0]).FirstOfMonth();
                if (t.EndOfMonth() < t1 || t.FirstOfMonth() > t2)
                    continue;
                if (TimePostion == TimeSeries.Hydromet.TimePostion.EndOfMonth)
                    t = t.EndOfMonth(); 
                else if (TimePostion == TimeSeries.Hydromet.TimePostion.FirstOfMonth)
                    t = t.FirstOfMonth();
                else if( TimePostion == TimeSeries.Hydromet.TimePostion.MidMonth)
                      t = t.MidMonth();
                else if (TimePostion == TimeSeries.Hydromet.TimePostion.Automatic)
                {
                    string colName = tbl.Columns[dataIndex].ColumnName;
                    var tokens = colName.Trim().ToUpper().Split(' ');
                    if (tokens.Length == 2)
                    {
                        var pcode = tokens[1];
                        if (pcode == "SE" || pcode == "FC")
                            t = t.FirstOfMonth();
                    }
                    else
                    {
                        t = t.EndOfMonth();
                    }
                }

                if (Convert.IsDBNull(tbl.Rows[i][dataIndex]))
                {
                    AddMissing(t);
                }
                else
                {
                    double d = Convert.ToDouble(tbl.Rows[i][dataIndex]);
                    Add(t, d, flag);
                }
            }
        }

         static void ConvertFromThousandAcreFeetToAcreFeet(Series s)
        {
            if (s.Units == "1000 acre-feet")
            {
                for (int i = 0; i < s.Count; i++)
                {
                    Point p = s[i];
                    if (!p.IsMissing)
                    {
                        p.Value = p.Value * 1000.0;
                        s[i] = p;
                    }
                }

                s.Units = "acre-feet";
            }
        }
        public static void ConvertFromAcreFeetToThousandAcreFeet(Series s)
        {
            if (s.Units == "acre-feet")
            {
                for (int i = 0; i < s.Count; i++)
                {
                    Point p = s[i];
                    if (!p.IsMissing)
                    {
                        p.Value = p.Value / 1000.0;
                        s[i] = p;
                    }
                }

                s.Units = "1000 acre-feet";
            }
        }

        public static string LookupUnits(string pcode)
        {
            pcode = pcode.ToUpper();

            string sql = "pcode = '" + pcode + "'";
            DataRow[] rows = MpollUnits.Select(sql);

            if (rows.Length > 0)
            {
                return rows[0]["units"].ToString();
            }

            return pcode;
        }
        private static DataTable _unitsTable;
        private static DataTable MpollUnits
        {
            get
            {
                if (_unitsTable == null)
                {
                    _unitsTable = CreateUnitsTable();
                }
                return _unitsTable;
            }
        }


        public static bool IsPermanentMark(string flag)
        {
            if (flag.Trim().Length <= 0)
                return false;

            var flagList = Flags;

            for (int i = 0; i < flagList.Length; i++)
			{
                if (flagList[i][0] == flag[0])
                {
                    if (flagList[i].IndexOf("*") >= 0)
                        return true;
                }
			}

            return false;
        }
        public static string[] Flags
        {
            get
            {
                var flags = new string[] {
            "A - RFC estimate",
            "B - Estimate by USBR",
            "C - Estimate by USBR correlation",
            "D - Published by Water District *",
            "E - Est by data collection agency",
            "F - Computed from permanent data *",
            "G - Published by USGS *",
            "H - Hydromet system",
            "I - Irrigation district",
            "L - Published by State agency *",
            "M - USBR mid-month forecast est",
            "N - Nacke's data summary of 1977",
            "O - Project office",
            "P - Project History *",
            "R - Annual Water Supply Report",
            "S - Published by SCS *",
            "T - Preliminary from data agency",
            "V - Loaded directly from ARCHIVES",
            "W - Published by NWS*",
            "Y - Yakima Project final *",
            "Z - 2nd Source HYDRODATA, SCS-CFS",
            "E - Forecast period through June",
            "Y - Through July",
            "A - Through August",
            "S - Through September"};
                return flags;
            }
        }

        static DataTable CreateUnitsTable()
        {
            DataTable tbl = new DataTable("mpoll_units");
            tbl.Columns.Add("pcode");
            tbl.Columns.Add("units");

            tbl.Rows.Add("AC", "acres");
            tbl.Rows.Add("AF", "1000 acre-feet");
            tbl.Rows.Add("AFA", "1000 acre-feet");
            tbl.Rows.Add("AFM", "1000 acre-feet");
            tbl.Rows.Add("AP1", "feet");
            tbl.Rows.Add("AP2", "feet");
            tbl.Rows.Add("AP3", "feet");
            tbl.Rows.Add("AP4", "feet");
            tbl.Rows.Add("ARC", "feet");
            tbl.Rows.Add("CC", "cfs");
            tbl.Rows.Add("CI", "cfs");
            tbl.Rows.Add("CL", "cfs");
            tbl.Rows.Add("CM", "cfs");
            tbl.Rows.Add("CO", "cfs");
            tbl.Rows.Add("CR1", "feet");
            tbl.Rows.Add("CR2", "feet");
            tbl.Rows.Add("CR3", "feet");
            tbl.Rows.Add("CR4", "feet");
            tbl.Rows.Add("CS", "1000 acre-feet");
            tbl.Rows.Add("CU", "cfs");
            tbl.Rows.Add("E95", "1000 acre-feet");
            tbl.Rows.Add("ECC", "feet");
            tbl.Rows.Add("ECM", "feet");
            tbl.Rows.Add("FB", "feet");
            tbl.Rows.Add("FC", "1000 acre-feet");
            tbl.Rows.Add("FCH", "1000 acre-feet");
            tbl.Rows.Add("FCL", "1000 acre-feet");
            tbl.Rows.Add("FCM", "1000 acre-feet");
            tbl.Rows.Add("FCN", "1000 acre-feet");
            tbl.Rows.Add("FCP", "1000 acre-feet");
            tbl.Rows.Add("FDA", "1000 acre-feet");
            tbl.Rows.Add("FDJ", "1000 acre-feet");
            tbl.Rows.Add("FJJ", "1000 acre-feet");
            tbl.Rows.Add("FJY", "1000 acre-feet");
            tbl.Rows.Add("FNY", "1000 acre-feet");
            tbl.Rows.Add("FOJ", "1000 acre-feet");
            tbl.Rows.Add("FOS", "1000 acre-feet");
            tbl.Rows.Add("FOY", "1000 acre-feet");
            tbl.Rows.Add("GRO", "deg/date");
            tbl.Rows.Add("IM", "1000 acre-feet");
            tbl.Rows.Add("IMA", "1000 acre-feet");
            tbl.Rows.Add("LEC", "feet");
            tbl.Rows.Add("MM", "deg f");
            tbl.Rows.Add("MN", "deg f");
            tbl.Rows.Add("MRC", "feet");
            tbl.Rows.Add("MX", "deg f");
            tbl.Rows.Add("OLK", "");
            tbl.Rows.Add("OM", "1000 acre-feet");
            tbl.Rows.Add("OMA", "1000 acre-feet");
            tbl.Rows.Add("PDR", "cfs");
            tbl.Rows.Add("PM", "inches");
            tbl.Rows.Add("PMA", "inches");
            tbl.Rows.Add("PP", "inches");
            tbl.Rows.Add("PV", "inches");
            tbl.Rows.Add("QA", "1000 acre-feet");
            tbl.Rows.Add("QC", "1000 acre-feet");
            tbl.Rows.Add("QL", "1000 acre-feet");
            tbl.Rows.Add("QM", "1000 acre-feet");
            tbl.Rows.Add("QU", "1000 acre-feet");
            tbl.Rows.Add("QUA", "1000 acre-feet");
            tbl.Rows.Add("RF", "1000 acre-feet");
            tbl.Rows.Add("SD", "inches");
            tbl.Rows.Add("SE", "inches");
            tbl.Rows.Add("SEM", "inches");
            tbl.Rows.Add("SG", "inches");
            tbl.Rows.Add("SGM", "inches");
            tbl.Rows.Add("SK", "inches");
            tbl.Rows.Add("SM", "inches");
            tbl.Rows.Add("SMA", "inches");
            tbl.Rows.Add("SP", "inches");
            tbl.Rows.Add("SPM", "inches");
            tbl.Rows.Add("SS", "inches");
            tbl.Rows.Add("SSM", "inches");
            tbl.Rows.Add("SU", "inches");
            tbl.Rows.Add("SUA", "inches");
            tbl.Rows.Add("SUE", "inches");
            tbl.Rows.Add("SUM", "inches");
            tbl.Rows.Add("VEA", "feet");
            tbl.Rows.Add("VEC", "feet");
            tbl.Rows.Add("VEE", "feet");
            tbl.Rows.Add("VEF", "feet");
            tbl.Rows.Add("VEJ", "feet");
            tbl.Rows.Add("VEM", "feet");
            tbl.Rows.Add("VEY", "feet");
            tbl.Rows.Add("VG", "gwh");
            tbl.Rows.Add("VMA", "feet");
            tbl.Rows.Add("VME", "feet");
            tbl.Rows.Add("VMF", "feet");
            tbl.Rows.Add("VMJ", "feet");
            tbl.Rows.Add("VMM", "feet");
            tbl.Rows.Add("VMY", "feet");
            tbl.Rows.Add("WA", "miles");
            tbl.Rows.Add("YS", "1000 acre-feet");


            //tbl.WriteXml(@"c:\temp\mpoll_units.xml", XmlWriteMode.WriteSchema);
            return tbl;
        }

        /// <summary>
        /// Returns longer descriptive name from a cbtt short name
        /// </summary>
        /// <returns></returns>
        private static string FindName(string cbtt)
        {
            if (MpollTextFileCatalog == null)
                return "";

            //int idx = MpollTextFileCatalog.IndexOfRegex("^"+cbtt.ToUpper()+"\\s");
            int idx = MpollTextFileCatalog.IndexBeginningWith(cbtt.ToUpper() + " ",0);
            if (idx < 0)
                return "";

            var len = MpollTextFileCatalog[idx].Length;
            if (idx >= 0 && len>19)
                return MpollTextFileCatalog[idx].Substring(19,System.Math.Min(len-20,50));
            return "";
        }

        private static TextFile s_mpollCbtt;
        

        private static TextFile MpollTextFileCatalog
        {
            get
            {
                if (s_mpollCbtt == null)
                {
                    string fn = FileUtility.GetFileReference("mpoll.cbt");
                    s_mpollCbtt = new TextFile(fn);
                }

                return s_mpollCbtt;
            }
        }

        //public string[] MpollSiteNames
        //{

        //    get
        //    {
        //        var rval = new List<string>();
        //        foreach (var item in MpollTextFileCatalog.FileData)
        //        {
        //            if (item.Length >= 4)
        //            {
        //                rval.Add(item.Substring(0, 4).Trim());
        //            }
        //        }
        //        return rval.ToArray();
        //    }
        //}


        static string[] standardPcodes = { "PM", "SE", "SU", "QU", "QM" };
        static string[] averagePcodes = { "PMA", "SEA", "SUA", "QU", "QMA" };
        /// <summary>
        /// Based on 'normal' pcode lookup coresponding average pcode
        /// </summary>
        /// <param name="pcode"></param>
        /// <returns></returns>
        public static string LookupAveargePcode(string pcode)
        {
            int idx = Array.IndexOf(standardPcodes, pcode);
            if (idx < 0)
                return "";


            return averagePcodes[idx];

        }

       
    }
}
