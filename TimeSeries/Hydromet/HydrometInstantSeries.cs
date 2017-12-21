using System;
using System.Data;
using System.Text.RegularExpressions;
using Reclamation.Core;
using System.IO;
using System.Configuration;
using System.Globalization;
namespace Reclamation.TimeSeries.Hydromet
{
    /// <summary>
    /// Hydromet dayfiles data 
    /// </summary>
    public class HydrometInstantSeries : Reclamation.TimeSeries.Series
    {
        static string[] GoodDataFlags = new string[] { "", " ", "e" };
        private string pcode;
        private string cbtt;
        private static bool s_keepflaggedData = false;// flagged data

        private static int s_fillGapMinutes = 0;

        public string Cbtt
        {
            get { return this.cbtt; }
        }
        public string Pcode
        {
            get { return this.pcode; }
        }

        public override bool IsBadData(string flag)
        {
            return Array.IndexOf(GoodDataFlags, flag) < 0; // did not find good flag
        }
        /// <summary>
        /// Interval in minutes should be 0, 5, 15, or 60
        /// Zero indicates no filling will be done.
        /// </summary>
        public static int FillInterval
        {
            get { return HydrometInstantSeries.s_fillGapMinutes; }
            set { HydrometInstantSeries.s_fillGapMinutes = value; }
        }

        public static bool KeepFlaggedData
        {
            get { return s_keepflaggedData; }
            set { s_keepflaggedData = value; }
        }
        HydrometHost server;


        public HydrometInstantSeries(string cbtt, string pcode, HydrometHost server)
        {
            Init(cbtt, pcode, server);
        }

        public HydrometInstantSeries(string cbtt, string pcode)
        {
            Init(cbtt, pcode, HydrometHost.PN);
        }
        public HydrometInstantSeries(TimeSeriesDatabase db, TimeSeriesDatabaseDataSet.SeriesCatalogRow sr)
            : base(db, sr)
        {
            HydrometInfoUtility.ParseConnectionString(ConnectionString, out server, out cbtt, out pcode);
        }

        private void Init(string cbtt, string pcode, HydrometHost server)
        {
            this.server = server;
            this.cbtt = cbtt;
            this.pcode = pcode;
            this.Table.TableName = TimeSeriesName.GetTableName(server.ToString(), TimeSeries.TimeInterval.Irregular,
                cbtt, pcode);
            Parameter = pcode;
            Units = HydrometInfoUtility.LookupDayfileUnits(pcode);
            Name = HydrometInfoUtility.LookupSiteDescription(cbtt) + " " + Units;
            Source = "Hydromet";
            Provider = "HydrometInstantSeries";
            this.SiteID = HydrometInfoUtility.LookupSiteDescription(cbtt);
            this.TimeInterval = TimeSeries.TimeInterval.Irregular;
            ReadOnly = true;
            this.Appearance.LegendText = cbtt + " " + pcode;
            ConnectionString = "server=" + server
            + ";cbtt=" + cbtt + ";pcode=" + pcode;// +";LastUpdate=" + DateTime.Now.ToString(DateTimeFormat_Instantaneous);
        }


        protected override void ReadCore()
        {
            if (m_db == null)
                ReadCore(DateTime.Now.AddDays(-7).Date, DateTime.Now);
            else
                ReadCore(TimeSeriesDatabase.MinDateTime, TimeSeriesDatabase.MaxDateTime);
        }

        /// <summary>
        /// Reads hydromet 15 minute data 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        protected override void ReadCore(DateTime t1, DateTime t2)
        {

            if (m_db == null || HydrometInfoUtility.WebOnly)
            {
                ReadAndParse(t1, t2);
            }
            else
            {
                if (HydrometInfoUtility.AutoUpdate)
                {
                    base.UpdateCore(t1, t2, true);
                }
                base.ReadCore(t1, t2);
            }

            if (FillInterval != 0)
            {
                NormalizeInstant(MinDateTime, MaxDateTime, FillInterval);
            }

        }

        private static HydrometDataCache s_cache;

        public static HydrometDataCache Cache // in-memory cache for high performance 
        {
            get
            {
                if (s_cache == null)
                {
                    s_cache = new HydrometDataCache();
                }
                return s_cache;
            }
            set
            {
                s_cache = value;
            }
        }

        private void ReadAndParse(DateTime t1, DateTime t2)
        {
            Clear();
            string key = cbtt + " " + pcode;
            int tableIndex = -1;
            if (Cache != null
                && (tableIndex = Cache.TableIndex(key, TimeSeries.TimeInterval.Irregular, t1, t2)) >= 0)
            {
                int idx = Cache.DataSet.Tables[tableIndex].Columns.IndexOf(key);
                if (idx < 0)
                    throw new Exception("oops");
                //Console.WriteLine("Cache hit for: " + key);
                ReadFromTable(Cache.DataSet.Tables[tableIndex], idx,idx+1, t1, t2);
            }
            else
            {
                ReadFromWeb(t1, t2);
            }
            
        }

        internal void ReadFromTable(DataTable tbl, int dataIndex, int flagIndex, DateTime t1, DateTime t2)
        {
            this.Clear();
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                DateTime t = Convert.ToDateTime(tbl.Rows[i][0]);
                if (t < t1 || t > t2)
                    continue;

                string flag = tbl.Rows[i][flagIndex].ToString();

                if (!KeepFlaggedData &&
                                 Array.IndexOf(GoodDataFlags, flag) < 0)
                {
                //    Logger.WriteLine("flagged data skipped '" + tf[i] + "'");
                    continue;
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

        private void ReadFromWeb(DateTime t1, DateTime t2)
        {
            string query = "";

            query = ReclamationURL.GetUrlToDataCgi(server, TimeSeries.TimeInterval.Irregular);

            query += "?parameter=" + cbtt + " " + pcode;
            query += "&syer=" + t1.Year;
            query += "&smnth=" + t1.Month;
            query += "&sdy=" + t1.Day;
            query += "&eyer=" + t2.Year;
            query += "&emnth=" + t2.Month;
            query += "&edy=" + t2.Day;


            string[] data = Web.GetPage(query, HydrometInfoUtility.WebCaching);
            Read(data);

            Messages.Add(query);
            Messages.Add("Returned " + Count + " records ");
        }

        private void Read(string[] data)
        {
            TextFile tf = new TextFile(data);
            int idx1 = tf.IndexOf("DATE");
            /*
             
12/13/2007 09:30,   3345.82-
12/13/2007 09:45,   3345.82-
12/13/2007 10:00,   3345.82-
12/13/2007 10:15,   3345.83-
12/13/2007 10:30,   3345.83-
12/13/2007 10:45,   3345.83-
12/13/2007 11:00,   3345.83-
12/13/2007 11:15,   3345.84-
END DATA
             * */
            if (idx1 < 0)
            {
                // TimeSeries.Series series = new TimeSeries.Series(); // empty
                Messages.Add("no data found");
                return;
            }

            Regex re = new Regex(
    @"(?<date>\d{2}/\d{2}/\d{4}\s{1}\d{2}\:\d{2})[,]?\s*(?<value>[+-]?[0-9\.]+)(?<flag>.{0,1})", RegexOptions.Compiled);
            DataTable table = this.Table;

            table.Columns[1].ColumnName = cbtt + " " + pcode;

            this.TimeInterval = Reclamation.TimeSeries.TimeInterval.Irregular;
            ReadOnly = true;

            for (int i = idx1 + 1; i < tf.Length; i++)
            {
                Match m = re.Match(tf[i]);
                if (m.Success)
                {
                    DateTime date = DateTime.MinValue;
                    if (!DateTime.TryParse(m.Groups[1].Value, out date))
                    {
                        //oops
                        string msg = "Skippling line '" + tf[i] + "' could not parse date ";
                        Console.WriteLine(msg);
                        Messages.Add(msg);
                        continue;
                    }

                    double val = -999;
                    string strValue = m.Groups[2].Value;
                    if (Double.TryParse(strValue, out val))
                    {
                        if (IndexOf(date) >= 0)
                        {
                            string msg = "Skippling line '" + tf[i] + "' this data was allready entered";
                            Messages.Add(msg);
                            Console.WriteLine(msg);
                        }
                        else
                        {
                            string flag = "";
                            if (m.Groups.Count > 3)
                                flag = m.Groups[3].Value;

                            if (!KeepFlaggedData &&
                                  Array.IndexOf(GoodDataFlags, flag) < 0)
                            {
                                Logger.WriteLine("flagged data skipped '" + tf[i] + "'");
                                continue;
                            }

                            if (System.Math.Abs(val - 998877) < 0.0001)
                                AddMissing(date);
                            else
                                Add(date, val, flag);
                        }
                    }
                    else
                    {
                        string msg = "Error: could not convert '" + strValue + "' to a number";
                        Console.WriteLine(msg);
                        Messages.Add(msg);
                        // bad record... 
                    }
                }
                else
                {
                    Messages.Add("Error parsing " + tf[i]);
                }
            }
        }


        /// <summary>
        /// Creates series and reads data from hydromet web site.
        /// </summary>
        public static HydrometInstantSeries Read(string cbtt, string pcode,
                DateTime t1, DateTime t2, HydrometHost server)
        {
            HydrometInstantSeries s = new HydrometInstantSeries(cbtt, pcode, server);
            s.Read(t1, t2);
            return s;
        }

        protected override Series CreateFromConnectionString()
        {
            HydrometHost svr;
            string scbtt, spcode;
            HydrometInfoUtility.ParseConnectionString(ConnectionString, out svr, out scbtt, out spcode);

            HydrometInstantSeries s = new HydrometInstantSeries(scbtt, spcode, svr);

            return s;
        }

        public static void WriteToHydrometFile(Series s, string cbtt, string pcode, string user, string filename, bool append = false)
        {
            if (s.Count == 0)
                return;

            bool oldFile = File.Exists(filename);

            StreamWriter output = new StreamWriter(filename,append);
            //Console.WriteLine("Writing "+cbtt+"/"+pcode+" to "+filename+" "+s.Count+" records" );
            if (!oldFile)
            {
                output.WriteLine("yyyyMMMdd hhmm cbtt     PC        NewValue   OldValue   Flag User:" + user);
            }

            double missing = 998877.0;
            for (int i = 0; i < s.Count; i++)
            {
                Point pt = s[i];

                var flagCode = DayFiles.FlagCode(pt.Flag);
                if (pt.IsMissing)
                {
                    flagCode = DayFiles.FlagCode("m");
                    pt.Value = missing;
                }

                string str = pt.DateTime.ToString("yyyyMMMdd HHmm").ToUpper()
                              + " " + cbtt.ToUpper().PadRight(8)
                              + " " + pcode.ToUpper().Trim().PadRight(9)
                              + " " + FormatValue(pt.Value)
                              + " " + missing.ToString("F2").PadRight(10)
                              + " " + flagCode.ToString().PadRight(3);
                output.WriteLine(str);
            }


            output.Close();
        }

        private static string FormatValue(double val)
        {
            if(System.Math.Abs( val) <1.0)
                return val.ToString("F4").PadRight(10);
            else
            return val.ToString("F2").PadRight(10);
        }


        public static bool IsValidDMS3(TextFile tf)
        {
            var header = @"yyyyMMMdd hhmm cbtt\s+PC\s+NewValue\s+OldValue\s+Flag";

            if (tf.Length > 0 &&  Regex.IsMatch(tf[0],header) )
                return true;

            DateTime t;
            if (tf.Length > 0 && DateTime.TryParseExact(tf[0].Substring(0,14).Trim(), "yyyyMMMdd HHmm", new CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, out t))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// converts DMS3 formated data for 'dayflag.exe' prorgram into SeriesList
        /// each series is named  instant_cbtt_pcode,
        /// for example  instant_jck_fb
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static SeriesList HydrometDMS3DataToSeriesList(TextFile tf)
        {
            var rval = new SeriesList();

            for (int i = 1; i < tf.Length; i++) // skip first row (header)
			{
                if( tf[i].Length <59)
                {
                    Console.WriteLine("Skipping invalid line: "+tf[i]);
                    continue;
                }
                var strDate = tf[i].Substring(0,14);

                DateTime t;
                if (!DateTime.TryParseExact(strDate, "yyyyMMMdd HHmm", new CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, out t))
                {
                    Console.WriteLine("Bad Date, Skipping line: "+tf[i]);
                    continue;
                }
                string cbtt = tf[i].Substring(15, 8).Trim();
                string pcode = tf[i].Substring(24, 9).Trim();
                string strValue = tf[i].Substring(34,10);
                string strFlagCode = tf[i].Substring(56, 3);
                double val =0;

                if( !double.TryParse(strValue,out val))
                {
                    Console.WriteLine("Error parsing double "+strValue);
                    continue;
                }

                string name = "instant_" + cbtt + "_" + pcode;
                name = name.ToLower();
                var idx = rval.IndexOfTableName(name);
                Series s;
                if (idx >= 0)
                    s = rval[idx];
                else
                {
                    s = new Series();
                    s.SiteID = cbtt;
                    s.Parameter = pcode;
                    s.Name = cbtt + "_" + pcode;
                    s.Name = s.Name.ToLower();
                    s.Table.TableName = name;
                    rval.Add(s);
                }

                string flag = DayFiles.FlagFromCode(strFlagCode);
                if (s.IndexOf(t) < 0)
                {
                    s.Add(t, val, flag);
                }
                else
                {
                    Logger.WriteLine(s.SiteID+":"+s.Parameter+ "skipped duplicate datetime "+t.ToString() );
                }

			}

            return rval;
        }

        
    }
}
