using System;
using Reclamation.Core;
using System.IO;
using System.Reflection;
using System.Data;
namespace Reclamation.TimeSeries.Nrcs
{
    /// <summary>
    /// Karl Tarbet January 2011
    /// Reads SNOTEL data from nrcs web site.
    /// 
    /// I talked with Maggie Dunklee:  503-414-3049.
    /// Dunklee, Maggie - Portland, OR [maggie.dunklee@por.usda.gov]
    /// see NWCC_Web_Report_Scripting.txt in same directory as this file.
    /// aslo see example test.bat in same directory
    /// </summary>
    public class NrcsSnotelSeries: Series
    {

        string siteNumber, parameterName;
        string url =      "http://www.wcc.nrcs.usda.gov/nwcc/view?intervalType=Historic&report=STAND&timeseries=Daily&format=copy&sitenum=679&year=2011&month=WY";

        public static bool AutoUpdate = true;
        


        public NrcsSnotelSeries(string siteNumber, string parameterName)
        {
            this.siteNumber = siteNumber;
            this.parameterName = parameterName;
            this.TimeInterval = TimeInterval.Daily;
            int idx = parameterName.IndexOf("(");
            int idx2 = parameterName.IndexOf(")");
            if (idx >= 0 && idx2 >= 0 && idx2 > idx)
                Units = parameterName.Substring(idx + 1, idx2 - idx-1);

            if (Units == "degC")
            {
                Units = "degrees C";
            }
            if (Units == "in")
            {
                Units = "inches";
            }
            
            Name = LookupName(siteNumber)+ " "+parameterName;
            this.Table.TableName = Name.Replace(" ", "_");
            base.SiteID = Name;
           // State = LookupState(siteNumber);
            Provider = "NrcsSnotelSeries"; 
            ConnectionString = "SiteNumber="+siteNumber+";ParameterName="+parameterName;
        }

       public NrcsSnotelSeries(TimeSeriesDatabase db, TimeSeriesDatabaseDataSet.SeriesCatalogRow sr)
            : base(db, sr)
        {
            siteNumber =  ConnectionStringUtility.GetToken(ConnectionString, "SiteNumber", "");
            parameterName = ConnectionStringUtility.GetToken(ConnectionString, "ParameterName", "");
            //State = LookupState(siteNumber);
        }


       protected override Series CreateFromConnectionString()
       {

           NrcsSnotelSeries s = new NrcsSnotelSeries(
           ConnectionStringUtility.GetToken(ConnectionString, "SiteNumber", ""),
           ConnectionStringUtility.GetToken(ConnectionString, "ParameterName", ""));

           return s;
       }


        static DataTable s_snotelSites;

        public static DataTable SnotelSites
        {

            get
            {

                var fn = FileUtility.GetFileReference("snotel_site_list2.csv");


                if (File.Exists(fn) && s_snotelSites == null)
                {
                    s_snotelSites = new CsvFile(fn, CsvFile.FieldTypes.AllText);
                }
                return s_snotelSites;
            }
        }

        /// <summary>
        /// Returns number based on station id 
        /// </summary>
        /// <param name="stationID">example:49L10S</param>
        /// <returns></returns>
        public static string LookupSiteID(string stationID)
        {

            if (SnotelSites != null)
            {
                string sql = "[StationID] = '" + stationID + "'";
                DataRow[] rows = SnotelSites.Select(sql);
                if (rows.Length == 1)
                {
                    return rows[0]["SiteID"].ToString();
                }
            }
            return "";
        }


        private string LookupState(string siteNumber)
        {
            if (SnotelSites != null)
            {
                string sql = "[SiteID] = '" + siteNumber + "'";
                DataRow[] rows = SnotelSites.Select(sql);
                if (rows.Length == 1)
                {
                    return rows[0]["State"].ToString();
                }
            }
            return "";
        }

        static string LookupName(string siteNumber)
        {

            if (SnotelSites != null)
            {
                string sql = "[SiteID] = '" + siteNumber + "'";
                DataRow[] rows = SnotelSites.Select(sql);
                if (rows.Length == 1)
                {
                    return rows[0]["SiteName"].ToString();
                }
            }
            return "Snotel Site "+siteNumber;

        }

        static DataTable s_inventory;


        public override PeriodOfRecord GetPeriodOfRecord()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            string fn = Path.Combine(path, "nwcc_inventory.csv");



            if (File.Exists(fn) && s_inventory == null)
            {
                s_inventory = new CsvFile(fn, CsvFile.FieldTypes.AllText);
            }
            if( s_inventory != null)
            {
                string sql = "[station id] = '" + siteNumber + "'";
                DataRow[] rows = s_inventory.Select(sql);
                if (rows.Length == 1)
                {
                    DateTime t1, t2;

                    if( DateTime.TryParse(rows[0]["start_date"].ToString(),out t1) 
                        && DateTime.TryParse(rows[0]["end_date"].ToString(),out t2) )
                    {
                        if (t2 > DateTime.Now.Date)
                            t2 = DateTime.Now.Date;

                        return new PeriodOfRecord(t1,t2,0);
                    }
                }
            }

            return new PeriodOfRecord(DateTime.Now.AddYears(-10), DateTime.Now,0);

        } 
        
        //public static DateRange GetPeriod

        //public static string[] ParameterList = {
        //                               "WTEQ.I-1 (in)", // SE
        //                               "PREC.I-1 (in)",  // PC
        //                               //"TOBS.I-1 (degC)", // OB
        //                               "TMAX.D-1 (degC)", // MX
        //                               "TMIN.D-1 (degC)", // MN
        //                               "TAVG.D-1 (degC)", // MM
        //                               "SNWD.I-1 (in)"};  // SD
       // static string[] unitsList = { "inches", "inches", "degrees C", "degrees C", "degrees C", "inches","inches"};


        protected override void ReadCore()
        {
          var por = GetPeriodOfRecord();
          ReadCore(por.T1, por.T2);
        }

        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            Clear();
            if (m_db == null)
            {
                ReadFromWeb(t1, t2);
            }
            else
            {
                if (NrcsSnotelSeries.AutoUpdate)
                {
                    if (t2 >= DateTime.Now.Date.AddDays(1) )
                    { // don't waste time looking to the future
                        // snotel includes today
                        t2 = DateTime.Now.Date;
                    }
                    base.UpdateCore(t1, t2, true);
                }
                base.ReadCore(t1, t2);

            }
            
        }

       
        private void ReadFromWeb( DateTime t1,  DateTime t2)
        {
            if (t2 < t1)
            {
                Logger.WriteLine("Warning invalid date range");
                return;
            }
            int wy1 = t1.WaterYear();
            int wy2 = t2.WaterYear();



            for (int wy = wy1; wy <=wy2 ; wy++)
            {
                string address = url.Replace("sitenum=679", "sitenum=" + siteNumber);
                address = address.Replace("year=2011", "year=" + wy);
                var lines = Web.GetPage(address,true);
                ProcessPage(lines,t1,t2);
            }

        }

        private void ProcessPage(string[] lines, DateTime t1, DateTime t2)
        {
            /*
Wed Jan 19 06:57:29 PST 2011  NRCS National Water and Climate Center - Provisional Data - subject to revision
Site Id,Date,Time (),WTEQ.I-1 (in) ,PREC.I-1 (in) ,TOBS.I-1 (degC) ,TMAX.D-1 (degC) ,TMIN.D-1 (degC) ,TAVG.D-1 (degC) ,
679,2000-10-01,,     0.0,     0.0,     7.6,     9.1,     7.6,     8.5,
679,2000-10-02,,     0.0,     0.4,     0.0,     9.2,     0.0,     4.4,
...

679,2001-09-30,,     0.0,    76.4,    10.0,    15.4,     4.2,     9.2,
679,2001-09-30,23:59,     0.2,-99.9,    13.3,    20.4,     9.4,    14.0,

             * 
             * 
             */
            int idxTime = 2; // index to time stamp (ignore the 23:59) entry..
                              
            TextFile tf = new TextFile(lines);
            int idx = tf.IndexOf("Site Id");
            if (idx < 0)
                return;
            // find column index for parameter
            string[] tokens = CsvFile.ParseCSV(tf[idx]);
            int idxData = Array.IndexOf(tokens, parameterName);

            if (idxData < 0)
                return;

            for (int i = idx+1; i < tf.Length; i++)
            {
                if (tf[i].Trim() == "")
                    continue;

                string[] data = CsvFile.ParseCSV(tf[i]);
                if (data[idxData].Trim() == "")
                    continue;

                if (data[idxTime].IndexOf("23:59") >= 0)
                    continue;

                DateTime t;
                if (!DateTime.TryParse(data[1], out t))
                {
                    Logger.WriteLine("Skippling data '"+tf[i]+"'");
                    continue;
                }
                /* FROM NRCS:
                * Daily sensors report a summary value for the previous day.  
                * Hourly sensors report a summary value for the previous hour.  
                * Instantaneous sensors are included with both Daily and Hourly sensor selections 
                 */
                // Hydromet convention:
                // midnight values 00:00 are reported previous days daily value
                // daily snotel is reported previous day
                // so we move back 1 day for both cases.

                // Just using NRCS data as it is to avoid confusion.
                //if (AdjustDates)
                //{
                //    t = t.AddDays(-1);
                //}

                if (t < t1 || t > t2)
                    continue;

                double value;
                if (!double.TryParse(data[idxData], out value))
                {
                    Logger.WriteLine("Skippling data '" + tf[i] + "'");
                }
                // missing values are -99.9
                if (System.Math.Abs(value + 99.9) < 0.1)
                    AddMissing(t);
                else
                  Add(t, value);
            }
        }



        static string[] SnotelParameters = {
                                       "WTEQ.I-1 (in)", // SE
                                       "WTEQ.I-1 (in)", // SE2  for billings
                                       "PREC.I-1 (in)",
                                       //"TOBS.I-1 (degC)",
                                       "TMAX.D-1 (degC)",
                                       "TMIN.D-1 (degC)",
                                       "TAVG.D-1 (degC)",
                                       "SNWD.I-1 (in)"};
        public static string SnotelParameterFromHydrometPcode(string pcode)
        {
            string[] parmCode = { "SE", "SE2", "PC", "MX", "MN", "MM", "SD" };
            
            int idx = Array.IndexOf(parmCode, pcode.ToUpper());
            if (idx >= 0)
                return SnotelParameters[idx];
            return "";
        }

        
    }
}
