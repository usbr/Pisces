using System;
using System.Collections.Generic;
using Reclamation.Core;
using System.IO;
using System.Data;
using System.Net;
using System.Text.RegularExpressions;
using System.Linq;
using System.Windows.Forms;
namespace Reclamation.TimeSeries.Hydromet
{
    public enum HydrometDataBase { Dayfiles, Archives, MPoll };
    public enum HydrometHost { PN, Yakima, GreatPlains };

    /// <summary>
    /// Common hydromet helper functions
    /// </summary>
    public static class HydrometInfoUtility
    {

        public static bool AutoUpdate = false;
        public static bool WebOnly = false;
        public static bool WebCaching = false;

        

        private static bool CbttOnly(string query)
        {
            string[] pairs = query.Split(',');
            if (pairs.Length == 1)
            {
                var tokens = pairs[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 1)
                    return true;
            }

            return false;
        }


        private static string[] GetParameters(string cbtt, HydrometDataBase db)
        {
            if( db == HydrometDataBase.Archives)
                return HydrometInfoUtility.ArchiveParameters(cbtt);
            if( db == HydrometDataBase.Dayfiles)
                return DayfileParameters(cbtt);
            if (db == HydrometDataBase.MPoll)
                return MpollParameters(cbtt);

            return new string[] { };

        }

        
        /// <summary>
        /// Expand simplified query
        /// BOII MX,MN,MM
        /// is translated to
        /// BOII MX, BOII MN, BOII MM
        /// 
        /// GREY
        /// is translated to 
        /// GREY GH, GREY Q
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string ExpandQuery(string query,HydrometDataBase db)
        {
            var rval = new List<string>();


            string title = "";
            var idx = query.IndexOf("#");
            if (idx >= 0)
            {
               title = query.Substring(idx );
               query = query.Substring(0, idx );
            }

            if (CbttOnly(query))
            {
                string[] pcodes = GetParameters(query, db);
                if (pcodes.Length > 0)
                {
                    query = query + " " + String.Join(",", pcodes);
                 //   return query;
                }
            }

            string[] pairs = query.Split(',');
            string cbtt = "";
            var pc = "";
            for (int i = 0; i < pairs.Length; i++)
            {
                string s = pairs[i];
                s = s.Replace(": ", ":");
                var tokens = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (i == 0 && tokens.Length != 2)
                    {
                        Logger.WriteLine("query syntax Error: first pair must have cbtt and pcode");
                        return query;
                    }

                    if (tokens.Length == 2)
                    {
                        cbtt = tokens[0];
                        pc = tokens[1];
                    }
                    else if (tokens.Length == 1)
                    {
                        pc = tokens[0];
                    }
                    else if (tokens.Length == 0)
                        continue;

                rval.Add(cbtt + " " + pc);

            }
            string r = String.Join(",", rval.ToArray());
            if( r.Trim() != query.Trim())
               Logger.WriteLine("query expanded to '" + r + "'");
            return r+title;
        }


        public static HydrometHost HydrometServerFromPreferences()
        {
            HydrometHost svr = HydrometInfoUtility.HydrometServerFromString(
    UserPreference.Lookup("HydrometServer"));
            return svr;
        }
        private static HydrometHost HydrometServerFromString(string server)
        {
            if (server == HydrometHost.PN.ToString()) return HydrometHost.PN;
            if (server == HydrometHost.Yakima.ToString()) return HydrometHost.Yakima;
            if (server == HydrometHost.GreatPlains.ToString()) return HydrometHost.GreatPlains;

            return HydrometHost.PN;
        }


        internal static void ParseConnectionString(string connectionString, out HydrometHost svr, out string cbtt, out string pcode)
        {
            string str = ConnectionStringUtility.GetToken(connectionString, "server","");
            svr = (HydrometHost)Enum.Parse(typeof(HydrometHost), str);
            cbtt = ConnectionStringUtility.GetToken(connectionString, "cbtt","");
            pcode = ConnectionStringUtility.GetToken(connectionString, "pcode","");
        }

        

        public static Series Read(string cbtt, string pcode, DateTime t1, DateTime t2,
            TimeInterval interval, HydrometHost hydrometServer)
        {
            Series s = new Series();
           
            if (interval == TimeInterval.Daily)
            {
                s = HydrometDailySeries.Read(cbtt, pcode, t1, t2, hydrometServer);
            }
            else if (interval == TimeInterval.Irregular)
            {
                s = HydrometInstantSeries.Read(cbtt, pcode, t1, t2, hydrometServer);
            }
            else if (interval == TimeInterval.Monthly)
            {
                s = new HydrometMonthlySeries(cbtt, pcode,hydrometServer);
                s.Read(t1, t2);
            }
            else
            {
                throw new ArgumentException("Undefined TimeInterval", interval.ToString());
            }
            
            return s;
        }


         
        public static string LookupNessid(string cbtt)
        {
            var rows = Site.Select("SITE='" + cbtt + "'");
            if (rows.Length == 1)
                return rows[0]["NESSID"].ToString();
            return "";
        }


        public static string LookupAltID(string cbtt)
        {

            var rows = Site.Select("SITE='" + cbtt + "'");

            if (rows.Length == 1)
                return rows[0]["ALTID"].ToString();
            return "";
        }


        public static string LookupDayfileUnits(string pcode)
        {
            pcode = pcode.ToUpper();
            string sql = "pcode = '" + pcode + "'";

            DataRow[] rows = InstantPcodes.Select(sql);

            if (rows.Length > 0)
            {
                return rows[0]["units"].ToString();
            }

            return pcode;
        }



        static DataTable s_group;
        private static DataTable Group
        {
            get
            {
                string filename = "group.csv";

                if (HydrometInfoUtility.HydrometServerFromPreferences() == HydrometHost.GreatPlains)
                    filename = "group_gp.csv";// to do... get file from GP.


                if (s_group == null || s_group.TableName != filename )
                {
                    s_group = new CsvFile(FileUtility.GetFileReference(filename), CsvFile.FieldTypes.AllText);
                    s_group.TableName = filename;
                }

                return s_group;
            }
        }

        static DataTable s_site;
        public static DataTable Site
        {
            get
            {
                string filename = "site.csv";
                if (HydrometInfoUtility.HydrometServerFromPreferences() == HydrometHost.GreatPlains)
                    filename = "site_gp.csv";

                if (s_site == null || s_site.TableName != filename)
                {
                    string fn = FileUtility.GetFileReference(filename);
                    Logger.WriteLine("Reading " + fn);
                    s_site = new CsvFile(fn, CsvFile.FieldTypes.AllText);
                    s_site.TableName = filename;
                }

                return s_site;
            }
        }
        static DataTable s_pcode;
        public static DataTable Pcode
        {
            get
            {
                string filename = "pcode.csv";
                if (HydrometInfoUtility.HydrometServerFromPreferences() == HydrometHost.GreatPlains)
                    filename = "pcode_gp.csv";

                if (s_pcode == null || s_pcode.TableName != filename)
                {
                   // Performance p = new Performance();
                    var fn = FileUtility.GetFileReference(filename);
                    s_pcode = new CsvFile(fn, CsvFile.FieldTypes.AllText);
                    s_pcode.TableName = filename;
                    //p.Report("done reading pcode.csv",true);
                    // baseline
                    // 5.531073 seconds elapsed. done reading pcode.csv
                    // using CsvFile.FieldTypes.AllText
                    // 0.2499472 seconds elapsed. done reading pcode.csv
                    // changed to compiled Regex in CsvFile 
                    // 0.2031146 seconds elapsed. done reading pcode.csv
                }

                return s_pcode;
            }
        }

        //giant inventory to store instant,daily,monthly site and pcode information
        public static DataTable HydrometInventory
        {

            get
            {
                DataTable instant = InstantInventory;
                DataTable daily = DailyInventory;
                DataTable monthly = MonthlyInventory;
                DataTable site = Site;
                DataTable tbl = new DataTable();
                tbl.Columns.Add("DataType");
                tbl.Columns.Add("cbtt");
                tbl.Columns.Add("cbttDescr");
                tbl.Columns.Add("pcode");
                tbl.Columns.Add("pcodeDescr");
                tbl.Columns.Add("years");
                tbl.Columns.Add("units");

                for (int i = 0; i < site.Rows.Count; i++)
                {
                    string DataType = "";
                    string cbtt = "";
                    string cbttDescr = "";
                    string pcode = "";
                    string pcodeDescr = "";
                    string years = "";
                    string units = "";

                    DataType = "instant";
                    cbtt = site.Rows[i]["SITE"].ToString();
                    cbttDescr = site.Rows[i]["DESCR"].ToString();

                    var row = instant.Select("cbtt ='" + cbtt + "'");
                    for (int j = 0; j < row.Count(); j++)
                    {
                        pcode = row[j]["pcode"].ToString();
                        pcodeDescr = row[j]["descr"].ToString();
                        years = row[j]["years"].ToString();
                        units = row[j]["units"].ToString();
                        tbl.Rows.Add(DataType, cbtt, cbttDescr, pcode, pcodeDescr, years, units);
                    }

                    row = daily.Select("cbtt ='" + cbtt + "'");
                    DataType = "daily";
                    for (int k = 0; k < row.Count(); k++)
                    {
                        pcode = row[k]["pcode"].ToString();
                        pcodeDescr = row[k]["descr"].ToString();
                        years = row[k]["years"].ToString();
                        units = row[k]["units"].ToString();
                        tbl.Rows.Add(DataType, cbtt, cbttDescr, pcode, pcodeDescr, years, units);
                    }

                    row = monthly.Select("cbtt ='" + cbtt + "'");
                    DataType = "monthly";
                    for (int m = 0; m < row.Count(); m++)
                    {
                        pcode = row[m]["pcode"].ToString();
                        pcodeDescr = row[m]["descr"].ToString();
                        years = row[m]["years"].ToString();
                        units = row[m]["units"].ToString();
                        tbl.Rows.Add(DataType, cbtt, cbttDescr, pcode, pcodeDescr, years, units);
                    }



                }

                return tbl;
            }
        }

        //Added InstantData inventory for use by search program
        //This is a table of the cbtt,pcode,years,description of pcode,units
        private static DataTable m_instantPcodes;

        public static DataTable InstantPcodes
        {
            get {
                string filename = "instant_pcode.csv";

                if (m_instantPcodes == null || m_instantPcodes.TableName != filename)
                {
                    var fn = FileUtility.GetFileReference(filename);
                    m_instantPcodes = new CsvFile(fn, CsvFile.FieldTypes.AllText);
                    m_instantPcodes.TableName = filename;

                }
                
                return m_instantPcodes; 
            }
        }
        // Get instant data into a nice table
        private static DataTable InstantInventory
        {
            get
            {

                DataTable tbl = new DataTable();
                tbl.Columns.Add("cbtt");
                tbl.Columns.Add("pcode");
                tbl.Columns.Add("years");
                tbl.Columns.Add("descr");
                tbl.Columns.Add("units");

                for (int i = 0; i < Pcode.Rows.Count; i++)
                {
                    if (Pcode.Rows[i]["PCODE"].ToString().Contains(" "))
                    {
                        string var = Pcode.Rows[i]["PCODE"].ToString();
                        string cbtt = var.Substring(0, 8).Trim();
                        string pcode = var.Substring(8).Trim();
                        string years = "";
                        string descr = Pcode.Rows[i]["DESCR"].ToString();
                        // add logic to find the units of the instant series
                        string units = "";
                        for (int j = 0; j < InstantPcodes.Rows.Count; j++)
                        {
                            if (pcode == InstantPcodes.Rows[j]["pcode"].ToString())
                            {
                                descr = InstantPcodes.Rows[j]["descr"].ToString();
                                units = InstantPcodes.Rows[j]["units"].ToString();
                            }
                        }

                        tbl.Rows.Add(cbtt, pcode, years, descr, units);
                    }

                }

                return tbl;
            }
        }


        public static PeriodOfRecord ArchivePeriodOfRecord(string cbtt, string pcode)
        {
            if (ArcInventory != null)
            {
                string key = " " + cbtt.PadRight(12) + pcode;
                int idx = ArcInventory.IndexOf(key.ToUpper());
                if (idx >= 0)
                {
                    /*
 AADI        QJ      : 1961-1976, 1978-1998
 AADI        QJX     : 1989-1991
 ABE         TG      : 2007
 ABEI        EJ      : 1991-1994
                     */
                    string s = ArcInventory[idx];
                    var tokens = s.Split(',', ' ', '-');
                    var list = new List<int>();
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        int val;
                        if (int.TryParse(tokens[i], out val))
                        {
                            list.Add(val);
                        }
                    }
                    if (list.Count > 0)
                    {
                        int min = list.Min();
                        int max = list.Max();
                        DateTime t1 = new DateTime(min-1,10, 1);
                        DateTime t2 = new DateTime(max,9, 30);
                        return new PeriodOfRecord(t1, t2, 0);
                    }
                }
            }

            return new PeriodOfRecord(new DateTime(1980, 1, 1), DateTime.Now.Date.AddDays(-1), 0);
        }

        private static TextFile s_archiveInventory;

        private static TextFile ArcInventory
        {
            get
            {
                if (s_archiveInventory == null)
                {
                    string fn = FileUtility.GetFileReference("boise_arc.dat");

                    if (File.Exists(fn))
                    {
                        s_archiveInventory = new TextFile(fn);
                    }
                }

                return s_archiveInventory;
            }
        }



        private static DataTable DailyPcodesTable
        {
            get
            {
                string filename = "daily_pcode.csv";
                if (m_dailypcodes == null || m_dailypcodes.TableName != filename)
                {
                    var fn = FileUtility.GetFileReference(filename);
                    m_dailypcodes = new CsvFile(fn, CsvFile.FieldTypes.AllText);
                    m_dailypcodes.TableName = filename;

                }
                return m_dailypcodes;
            }
        }

        private static DataTable m_dailypcodes;

        public static string LookupDailyParameterName(string pcode)
        {
            var rows =DailyPcodesTable.Select("pcode = '" + pcode + "'", "");
            if (rows.Length > 0)
                return rows[0]["descr"].ToString();
            return "";
        }

        private static DataTable DailyInventory
        {
            get{

                DataTable tbl = new DataTable();
                tbl.Columns.Add("cbtt");
                tbl.Columns.Add("pcode");
                tbl.Columns.Add("years");
                tbl.Columns.Add("descr");
                tbl.Columns.Add("units");

                for (int i = 1; i < ArcInventory.Length; i++)
			    {
                    if (ArcInventory[i].Trim().Length < 21)
                        continue;
                    string cbtt = ArcInventory[i].Substring(0, 13).Trim();
                    string pcode = ArcInventory[i].Substring(13, 8).Trim();
                    string years = ArcInventory[i].Substring(21);
                    string descr = "";
                    string units = "";
                    for (int j = 0; j < DailyPcodesTable.Rows.Count; j++)
                    {
                        if (pcode == DailyPcodesTable.Rows[j]["pcode"].ToString())
                        {
                            descr = DailyPcodesTable.Rows[j]["descr"].ToString();
                            units = DailyPcodesTable.Rows[j]["units"].ToString();
                        }
                    }

                    years = years.Replace(":","").Trim();
                    tbl.Rows.Add(cbtt,pcode,years,descr,units);
			    }

                return tbl;
            }
        }


        public static bool ArchiveParameterExists(string cbtt, string pcode)
        {
         
            string key = " " +cbtt.PadRight(12) + pcode;
            int idx = ArcInventory.IndexOf(key);
            return idx >= 0;
        }

        public static string LookupMcfPcodeDescription(string cbtt, string pcode)
        {
            string s = cbtt.PadRight(8) + pcode;
            DataRow[] rows = Pcode.Select("pcode='" + s + "'");
            if (rows.Length == 0)
                return "";

            return rows[0]["DESCR"].ToString();
        }


        public static bool ArchiverEnabled(string cbtt, string pcode)
        {
            return LookupParameterSwitch(cbtt, pcode, ParameterSwitch.ACTIVE)
                &&
                LookupParameterSwitch(cbtt, pcode, ParameterSwitch.ACMSW);
        }

        public enum ParameterSwitch { ACTIVE, SHEFSW, QCSW, ALMSW, ACMSW};
        public static bool LookupParameterSwitch(string cbtt, string pcode, ParameterSwitch p)
        {
            string s = cbtt.PadRight(8).ToUpper() + pcode;
            DataRow[] rows = Pcode.Select("pcode = '"+s+"'");
            if (rows.Length == 1)
            {
                s = rows[0][p.ToString()].ToString();
                if (s.Trim() == "1")
                    return true;
            }

            return false;
        }
        public static string[] DayfileParameters(string cbtt)
        {
            List<string> rval = new List<string>();
            string s = cbtt.PadRight(8).ToUpper();
            DataRow[] rows = Pcode.Select("pcode like '" + s + "%'");

            for (int i = 0; i < rows.Length; i++)
            {
                string a = rows[i]["pcode"].ToString();
                rval.Add(a.Substring(8).Trim());
            }

            return rval.ToArray();
        }

        private static string SiteLookup(string cbtt, string columnName)
        {
            if (Site.Rows.Count == 0)
                return "";

            DataRow[] rows = Site.Select("site='" + cbtt + "'");
            if (rows.Length == 0)
                return "";
            return rows[0][columnName].ToString();
        }

        public static string LookupSiteDescription(string cbtt)
        {
            return SiteLookup(cbtt, "DESCR");
        }
        public static string LookupAcl(string cbtt)
        {
            return SiteLookup(cbtt, "ACL");
        }

        public static string LookupState(string cbtt)
        {
            return SiteLookup(cbtt, "STATE");
        }

        /// <summary>
        /// Used by  SnowGG.
        /// </summary>
        /// <param name="cbtt"></param>
        /// <returns></returns>
        public static string LookupElevation(string cbtt)
        {
            return SiteLookup(cbtt, "ELEV");
        }

        public static string LookupLatitude(string cbtt)
        {
            return SiteLookup(cbtt, "LAT");
        }

        public static string LookupLongitude(string cbtt)
        {
            return SiteLookup(cbtt, "LONG");
        }

        public static string LookupGroupDescription(string cbtt)
        {
            string grp = SiteLookup(cbtt, "GRP");

            if (grp.Trim() == "")
                return "";

           var rows = Group.Select("Number='" + grp + "'");
            if( rows.Length == 0)
                return "";

            return rows[0]["Name"].ToString();
        }
        

      

        

        static DataTable CreateDayFileUnitsTable()
        {
            DataTable tbl = new DataTable("dayfile_units");
            tbl.Columns.Add("pcode");
            tbl.Columns.Add("units");

            tbl.Rows.Add("AF", "acre-feet");
            tbl.Rows.Add("BH", "mmHg");
            tbl.Rows.Add("FB", "feet");
            tbl.Rows.Add("FB2", "feet");
            tbl.Rows.Add("GH", "feet");
            tbl.Rows.Add("NT", "mmHg");
            tbl.Rows.Add("PC", "inches");
            tbl.Rows.Add("PE", "inches");
            tbl.Rows.Add("PP", "inches");
            tbl.Rows.Add("PU", "inches");
            tbl.Rows.Add("PX", "inches");
            tbl.Rows.Add("Q", "cfs");
            tbl.Rows.Add("QC", "cfs");
            tbl.Rows.Add("QH", "cfs");
            tbl.Rows.Add("QD", "cfs");
            tbl.Rows.Add("QJ", "cfs");
            tbl.Rows.Add("QT", "cfs");
            tbl.Rows.Add("QU", "cfs");
            tbl.Rows.Add("QX", "cfs");
            tbl.Rows.Add("SO", "inches");
            tbl.Rows.Add("SE", "inches");
            tbl.Rows.Add("SP", "inches");
            tbl.Rows.Add("SR", "Langleys");
            tbl.Rows.Add("TA", "%");
            tbl.Rows.Add("UA", "mph");
            tbl.Rows.Add("UD", "Degrees Azimuth");
            tbl.Rows.Add("OB", "Degrees F");
            tbl.Rows.Add("WK", "Degrees F");
            tbl.Rows.Add("WI", "Degrees F");
            tbl.Rows.Add("WF", "Degrees F"); 
            tbl.Rows.Add("WF2", "Degrees F"); 
            tbl.Rows.Add("WM", "Degrees Celsius");
            tbl.Rows.Add("WN", "Degrees Celsius");
            tbl.Rows.Add("WR", "miles/day");
            tbl.Rows.Add("WY", "Degrees Celsius");
            tbl.Rows.Add("WZ", "Degrees F");
            tbl.Rows.Add("YR", "%");

            // tbl.WriteXml(@"c:\temp\archive_units.xml", XmlWriteMode.WriteSchema);
            return tbl;

        }
        //LookupDailyUnits
        public static string LookupDailyUnits(string pcode)
        {

            var rows = DailyPcodesTable.Select("pcode = '" + pcode + "'", "");
            if (rows.Length > 0)
                return rows[0]["units"].ToString();
            return "";
        }



        public static TimeSeriesDatabaseDataSet.RatingTableDataTable GetRatingTable(string cbtt, string pcode)
        {
            // yakima ?
            //http://www.usbr.gov/pn-bin/yak/expandrtf.pl?site=kee&pcode=af&form=col

            string url = "http://www.usbr.gov/pn-bin/expandrtf.pl?site=pali&pcode=q&form=col";

            url = url.Replace("site=pali", "site=" + cbtt.Trim());
            url = url.Replace("pcode=q", "pcode=" + pcode.Trim());

            string[] data = Web.GetPage(url);
            TextFile tf = new TextFile(data);

            /*
             <table border="1" summary="Expanded Rating Table">	<tr align="center">
<th width="80px"><b>ft</b></th>
<th width="80px"><b>cfs</b></th>
</tr>
             * 
             * Table Edit Date: 18-SEP-2014 02:22
<tr align="right"><td>2440.01</td><td>-704.00</td></tr>
<tr align="right"><td>2440.02</td><td>-704.00</td></tr>
<tr align="right"><td>2440.03</td><td>-704.00</td></tr>
<tr align="right"><td>2440.04</td><td>-704.00</td></tr>
             */


            TimeSeriesDatabaseDataSet.RatingTableDataTable t = new TimeSeriesDatabaseDataSet.RatingTableDataTable();
            int idx = tf.IndexOfBothRegex("<th.*</th>", "<th.*</th>");
            if (idx >= 0)
            {
                t.XUnits = Regex.Match(tf[idx],     @"<th.*<b>(?<x>\w*)</b>").Groups[1].Value;
                t.YUnits = Regex.Match(tf[idx + 1], @"<th.*<b>(?<x>\w*)</b>").Groups[1].Value;
            }
            //<br>Table Edit Date: 18-SEP-2014 02:22<br>Today's Date: 23-SEP-2014 06:54
            var idxDate = tf.IndexOfRegex("Table Edit Date:.*<br>");
            if( idxDate>=0)
            {
                t.EditDate = Regex.Match(tf[idxDate], "Table Edit Date:(?<date>.*)<br>").Groups[1].Value;
            }

            
            Regex re = new Regex(@"<tr.*?>(?<x>[\d\.\-\+]{1,12})</td><td>(?<y>[\d\.\-\+]{1,12})</td>");
            for (int i = idx+2; i < tf.Length; i++)
            {
                var m = re.Match(tf[i]);
                if (m.Success)
                {
                    double x,y;
                    if (double.TryParse(m.Groups["x"].Value, out x)
                        )
                    {
                        if (double.TryParse(m.Groups["y"].Value, out y)
                            && System.Math.Abs(998877 - y) > 0.01
                            )
                        {
                            t.AddRatingTableRow(x, y);
                        }
                    }
                }
            }
            t.Name = "Hydromet " + cbtt + " " + pcode;
            idx = tf.IndexOfRegex("<b>Station .+<br />");
            if (idx >= 0)
                t.Name = Regex.Match(tf[idx], "<b>Station (.+)<br />").Groups[1].Value;

            return t;
        }


        public static string[] LookupMonthlyInventory(string cbtt)
        {
            List<string> rval = new List<string>();
            var tbl = DataTableUtility.Select(MonthlyInventory, "Cbtt='" + cbtt + "'", "cbtt,pcode");

            var pcodeList = DataTableUtility.StringList(DataTableUtility.SelectDistinct(tbl, "pcode"), "", "pcode");

            foreach (var pc in pcodeList)
            {
                string line = cbtt.PadRight(12) + " " + pc.PadRight(9);
                var rows = tbl.Select("Cbtt='" + cbtt + "' and pcode='" + pc + "'");
                for (int i = 0; i < rows.Length; i++)
                {
                    line += rows[i]["years"].ToString() + " ";
                }
                rval.Add(line);
            }

            return rval.ToArray();
        }

        private static DataTable s_mpollInventory;

        private static DataTable Monthlykey;

        public static DataTable MonthlyInventory
        {
            get
            {
                string filename = "monthly_pcode.csv";

                if (Monthlykey == null || Monthlykey.TableName != filename)
                {
                    var fn = FileUtility.GetFileReference(filename);
                    Monthlykey = new CsvFile(fn, CsvFile.FieldTypes.AllText);
                    Monthlykey.TableName = filename;

                }

                if (s_mpollInventory == null)
                {
                    string fn = FileUtility.GetFileReference("mpoll_inventory.txt");

                    if (File.Exists(fn))
                    {
                        var tf = new TextFile(fn);
                         s_mpollInventory = new DataTable();
                        s_mpollInventory.Columns.Add("cbtt");
                        s_mpollInventory.Columns.Add("pcode");
                        s_mpollInventory.Columns.Add("years");
                        //added two more columns
                        s_mpollInventory.Columns.Add("descr");
                        s_mpollInventory.Columns.Add("units");

                        string cbtt = "";
                        string pcode = "";
                        DataRow newRow = s_mpollInventory.NewRow();
                        for (int i = 0; i < tf.Length; i++)
                        {
                            string line = tf[i];

                            if (line.IndexOf("   Station     Parm Code    Years")>=0)
                                continue;
                            if( line.IndexOf(" ------------")>=0)
                                continue;

                            if (line.Trim().Length < 27)
                                continue;

                            var test = line.Substring(0, 14).Trim();
                            if (test.Length > 0)
                            {
                                cbtt = test;
                            }

                            test = line.Substring(14, 11).Trim();
                            if (test != "")
                            {
                                pcode = test;
                            }

                          

                            var years = line.Substring(26).Trim();

                              if (test == "" )//append years wrapped on multiple lines.
                            {
                                newRow["years"] = newRow["years"].ToString() + " " + years;
                                continue;
                            }


                           newRow = s_mpollInventory.NewRow();
                            newRow["cbtt"] = cbtt;
                            newRow["pcode"] = pcode;
                            newRow["years"] = years;
                            //added two columns and using table to get units
                            string descr = "";
                            string units = "";
                            for (int j = 0; j < Monthlykey.Rows.Count; j++)
                            {
                                if (pcode == Monthlykey.Rows[j]["pcode"].ToString())
                                {
                                    descr = Monthlykey.Rows[j]["descr"].ToString();
                                    units = Monthlykey.Rows[j]["units"].ToString();
                                }
                            }
                            newRow["descr"] = descr;
                            newRow["units"] = units;

                            s_mpollInventory.Rows.Add(newRow);
                        }


                    }
                }

                return s_mpollInventory;
            }
        }

        private static string[] MpollParameters(string cbtt)
        {
            List<string> rval = new List<string>();
            var tbl = MonthlyInventory;

            var rows = tbl.Select("cbtt = '"+cbtt+"'");

            for (int i = 0; i < rows.Length; i++)
            {
                rval.Add(rows[i]["pcode"].ToString());   
            }


            return rval.Distinct().ToArray();
        }

       
        public static string[] ArchiveParameters(string cbtt)
        {

            //if (AgriMet.AgriMetLegacy.IsAgrimetSite(cbtt))
            //{

            //}

            string key = " " + cbtt.PadRight(12).ToUpper();
            var rval = new List<string>();
            TextFile tf = ArcInventory;
            for (int i = 0; i < tf.Length; i++)
            {
                string line = tf[i];
                if (line.IndexOf(key) == 0)
                {
                    // HACK ... need to update boise_arc.dat
                    // and latest water year (2010) in this case
                    // try the last few years... need to update this file
                    //
                    int yr = DateTime.Now.Year;
                    if (  
                         line.Substring(21).IndexOf((yr+1).ToString()) >= 0
                        || line.Substring(21).IndexOf(yr.ToString()) >= 0
                        || line.Substring(21).IndexOf((yr-1).ToString()) >= 0
                        || line.Substring(21).IndexOf((yr - 2).ToString()) >= 0
                        || line.Substring(21).IndexOf((yr - 3).ToString()) >= 0
                        )
                    {
                        string s = line.Substring(13, 7);
                       if( rval.IndexOf(s.ToUpper()) <0)
                          rval.Add(s);
                    }
                }
            }
            return rval.ToArray();
        }

       
        //public static void SaveToArchives(Series s)
        //{
        //   var sl = new SeriesList();
        //    sl.Add(s);
        //    SaveToArchives(sl);
        //}
    }
}
