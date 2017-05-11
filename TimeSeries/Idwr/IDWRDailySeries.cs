using System;
using System.Data;
using System.Collections.Generic;
using Reclamation.Core;
using System.Windows.Forms;
using Reclamation.TimeSeries;
using Newtonsoft.Json;
using RestSharp;

namespace Reclamation.TimeSeries.IDWR
{
    public class IDWRDailySeries : Series
    {

        static string idwrAPI = @"https://research.idwr.idaho.gov/apps/Shared/WaterServices/Accounting";
        static RestClient idwrClient = new RestClient(idwrAPI);

        ///<summary>
        ///SCRIPT TO TEST IDWR SERIES GENERATION and UPDATING
        ///</summary>
        ///<param name="args"></param>
        public void Main(string[] args)
        {

            //string fn = @"C:\temp\IDWR_UpperSnake_DB.pdb";
            //var db = new TimeSeriesDatabase(new SQLiteServer(fn),false);
            //string station = "13080000";
            //var s = db.GetSeriesFromName("IDWR"+station);
            //s.Read(DateTime.Parse("1/1/2009"), DateTime.Parse("12/31/2009"));
            ////s.IDWRUpdate(2011);
            //Console.WriteLine(s);

            //var a = Utilities.IdwrApiQueryRiverList();
            //var riverSys = a[0];
            //var b = IdwrApiQueryRiverSites(riverSys.River);
            //var riverSite = b[0];
            //var c = IdwrApiQuerySiteInfo(riverSite.SiteID);
            //var d = IdwrApiQuerySiteYears(riverSite.SiteID);

        }

        
        string station;
        string parameter;


        public IDWRDailySeries(string station, string parameter)
        {
            this.station = station;
            this.parameter = parameter;
            TimeInterval = TimeSeries.TimeInterval.Daily;
            switch (parameter)
            {
                case "FB": case "GH":
                    Units = "feet";
                    break;
                case "QD":
                    Units = "cfs";
                    break;
                case "AF":
                    Units = "acre-feet";
                    break;
            }
            Name = station + "_" + parameter;            
        }

        /// <summary>
        /// Reads IDWR Series from pdb Database.
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            if (m_db != null)
            {
                base.ReadCore(t1, t2);
            }
            else
            {
                Add(
                //IDWRWebDownload(station, t1, t2));
                IdwrApiDownload(station, parameter, t1, t2));
            }            
        }


        /// <summary>
        /// Updates IDWR SDF Database with downloaded IDWR values.
        /// Relies on an existing Pisces DB located at the path below.
        /// </summary>
        /// <param name="args"></param>
        //public void IDWRUpdate(int year)
        //{
        //    // Opens Pisces Connection and reads Pisces DB.
        //    Reclamation.Core.SqlServerCompact pDB = new SqlServerCompact
        //        (@"C:\Documents and Settings\jrocha\Desktop\IDWR_UpperSnake_DB.sdf");
        //    TimeSeriesDatabase DB = new TimeSeriesDatabase(pDB);

        //    // Gets Pisces table and series.
        //    string tableName = "IDWR" + station;
        //    var seriesPisces = DB.GetSeriesFromName(tableName);
        //    seriesPisces.Read();

        //    // Gets IDWR series.
        //    string calendarType = "C";
        //    Series seriesIDWR = IDWRWebDownload(station, year, year, calendarType);

        //    // Updates Pisces series with new IDWR data.
        //    int countPisces = seriesPisces.Count;
        //    int countIDWR = seriesIDWR.Count;
        //    for (int i = 1; i <= countIDWR; i++)
        //    {
        //        if (seriesPisces[countPisces - 1].DateTime < seriesIDWR[countIDWR - i].DateTime)
        //        {
        //            seriesPisces.Add(seriesIDWR[countIDWR - i]);
        //        }
        //    }

        //    // Updates Pisces values with modified IDWR values.
        //    countPisces = seriesPisces.Count;
        //    for (int i = 1; i <= countIDWR; i++)
        //    {
        //        if (!seriesPisces[countPisces - i].Value.Equals(seriesIDWR[countIDWR - i].Value))
        //        {
        //            seriesPisces[countPisces - i] = seriesIDWR[countIDWR - i];
        //        }
        //    }
        //    int dbID = 1;//seriesPisces.SiteDataTypeID;
        //    DB.SaveTimeSeriesTable(dbID, seriesPisces, DatabaseSaveOptions.Save);
        //}
        
        private static List<TsData> IdwrApiQuerySiteData(RiverSite riverSite, string yearList)
        {
            // Working API Call
            //https://research.idwr.idaho.gov/apps/Shared/WaterServices/Accounting/history?siteid=10055500&yearlist=2016,2015&yeartype=CY&f=json

            return IdwrApiQuerySiteData(riverSite.SiteID, yearList);
        }

        private static List<TsData> IdwrApiQuerySiteData(string siteID, string yearList)
        {
            // Working API Call
            //https://research.idwr.idaho.gov/apps/Shared/WaterServices/Accounting/history?siteid=10055500&yearlist=2016,2015&yeartype=CY&f=json

            var request = new RestRequest("history?", Method.GET);
            request.AddParameter("siteid", siteID);
            request.AddParameter("yearlist", yearList);
            request.AddParameter("yeartype", "CY");
            request.AddParameter("f", "json");
            IRestResponse restResponse = idwrClient.Execute(request);
            return JsonConvert.DeserializeObject<List<TsData>>(restResponse.Content);
        }


        private static Series IdwrApiDownload(string station, string parameter, DateTime t1, DateTime t2)
        {
            var s = new Series();
            string yearList = "";
            for (int year = t1.Year; year <= t2.Year; year++)
            {
                yearList = year + ",";
            }
            yearList = yearList.Trim(',');
            var jsonResponse = IdwrApiQuerySiteData(station, yearList);

            foreach (var item in jsonResponse)
            {
                var t = DateTime.Parse(item.Date);
                if (t >= t1 && t <= t2)
                {
                    string value = "";
                    switch (parameter)
                    {
                        case ("GH"):
                            value = item.GH;
                            break;
                        case ("FB"):
                            value = item.FB;
                            break;
                        case ("AF"):
                            value = item.AF;
                            break;
                        case ("QD"):
                            value = item.QD;
                            break;
                        default:
                            value = "NaN";
                            break;
                    }
                    if (value == "NaN")
                    {
                        s.AddMissing(t);
                    }
                    else
                    {
                        s.Add(item.Date, Convert.ToDouble(value));
                    }
                }
            }

            return s;
        }


        /// <summary>
        /// Downloads IDWR series from IDWR website and returns a series.
        /// </summary>
        /// <param name="station">IDWR station number</param>
        /// <returns></returns>
        private static Series IDWRWebDownload(string station, DateTime t1, DateTime t2  )
        {
        
        //  W: Water Year, I: Irrigation Year, C: Calendar Year
            string calendarType = "C";

            int year1 = t1.Year;
            int year2  =t2.Year;

            // Produces URL for data download. Data download string researched from this website:
            //http://maps.idwr.idaho.gov/qWRAccounting/WRA_Select.aspx
            string urlDate = station + "." + year1;
            year1++;
            while (year1 <= year2)
            {
                urlDate = urlDate + "," + station + "." + year1;
                year1++;
            }
            string url = "https://maps.idwr.idaho.gov/qWRAccounting/WRA_Download.aspx?req="
                + urlDate + "&datatype=HST&calendartype=" + calendarType + "&file=CSV";
            var data = Web.GetPage(url);
            if (data[0] == "")
            { throw new ArgumentException("Unexpected IDWR Database Error: Try Again Later"); }

            // Populates a Hydromet Series with IDWR data.
            DateTime t; double value; var series = new Series();
            int j = 1;
            int count = data.Length;
            while (j < count - 3)  // Outputs series with raw data without consideration for missing data points.
            {
                //"Site,Date,Value,Title"
                var s = Reclamation.Core.CsvFile.ParseCSV(data[j]);
                if (s.Length < 3)
                    continue;

                // s[0] contains site number for all rows. Duplicates.
                if (!DateTime.TryParse(s[1], out t))
                {
                    Logger.WriteLine("invalid date " + s[1]);
                    continue;
                }
                t = DateTime.Parse(s[1]);

                if (!double.TryParse(s[2], out value))
                {
                    Logger.WriteLine("invalid value " + s[2]);
                    continue;
                }
                
                // s[3] contains 'value' units for all rows. Duplicates.
                if( t >= t1 && t <= t2)
                  series.Add(t, value);
                j++;
            }
            series.TimeInterval = TimeInterval.Daily;
            series = Math.FillMissingWithZero(series);
            return series;
        }


        /// <summary>
        /// Parses text file containing all of IDWR Upper Snake data and returns a series.
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        private static Series IDWRSeriesDatabase(string station)
        {
            // Read Raw Data.
            TextFile RawData = new TextFile
                (@"C:\Documents and Settings\jrocha\Desktop\SnakeDiversionData.txt");
            int count = RawData.Length;
            var ParsedData = new List<string>();
            for (int i = 1; i < count; i++)
            {
                ParsedData.AddRange(RawData[i].Split('\t'));
            }
            int ParsedDataCount = ParsedData.Count;

            Series s = new Series();
            int k = 0;
            while (k < ParsedDataCount - 4)
            {
                if (station == ParsedData[k])
                {
                    DateTime t = DateTime.Parse(ParsedData[k + 2]);
                    if (ParsedData[k + 3] == "")
                    { ParsedData[k + 3] = "0.00"; }
                    double val = Convert.ToDouble(ParsedData[k + 3]);
                    s.Add(t, val);
                }
                k = k + 4;
            }
            return s;
        }


    }

    public class Utilities
    {
        static string idwrAPI = @"https://research.idwr.idaho.gov/apps/Shared/WaterServices/Accounting";
        static RestClient idwrClient = new RestClient(idwrAPI);

        private static List<RiverItems> IdwrApiQueryRiverList()
        {
            // Working API Call
            //https://research.idwr.idaho.gov/apps/Shared/WaterServices/Accounting/RiverSystems

            var request = new RestRequest("RiverSystems/", Method.GET);
            IRestResponse restResponse = idwrClient.Execute(request);
            return JsonConvert.DeserializeObject<List<RiverItems>>(restResponse.Content);
        }


        private static List<RiverSite> IdwrApiQueryRiverSites(string riverItem)
        {
            // Working API Call
            //https://research.idwr.idaho.gov/apps/Shared/WaterServices/Accounting/SitesByRiver?river=BAR

            var request = new RestRequest("SitesByRiver?", Method.GET);
            request.AddParameter("river", riverItem);
            IRestResponse restResponse = idwrClient.Execute(request);
            return JsonConvert.DeserializeObject<List<RiverSite>>(restResponse.Content);
        }

        private static List<SiteInfo> IdwrApiQuerySiteInfo(string riverSite)
        {
            // Working API Call
            //https://research.idwr.idaho.gov/apps/Shared/WaterServices/Accounting/SiteDetails?sitelist=10039500

            var request = new RestRequest("SiteDetails?", Method.GET);
            request.AddParameter("sitelist", riverSite);
            IRestResponse restResponse = idwrClient.Execute(request);
            return JsonConvert.DeserializeObject<List<SiteInfo>>(restResponse.Content);
        }

        private static List<string> IdwrApiQuerySiteYears(string riverSite)
        {
            // Working API Call
            //https://research.idwr.idaho.gov/apps/Shared/WaterServices/Accounting/HistoryAvailableYearsBySiteId?siteid=10039500&yeartype=CY&f=json

            var request = new RestRequest("HistoryAvailableYearsBySiteId?", Method.GET);
            request.AddParameter("siteid", riverSite);
            request.AddParameter("yeartype", "CY");
            request.AddParameter("f", "json");
            IRestResponse restResponse = idwrClient.Execute(request);
            return JsonConvert.DeserializeObject<List<string>>(restResponse.Content);
        }


        /// <summary>
        /// Get all IDWR River Systems from Web API
        /// </summary>
        /// <returns></returns>
        public static DataTable GetIdwrRiverSystems()
        {
            var dTab = new DataTable();
            var data = IdwrApiQueryRiverList();
            dTab.Columns.Add("River", typeof(string));
            dTab.Columns.Add("Name", typeof(string));
            dTab.Columns.Add("WD", typeof(string));
            foreach (var item in data)
            {
                var dRow = dTab.NewRow();
                dRow["River"] = item.River;
                dRow["Name"] = item.Name;
                dRow["WD"] = item.WD;
                dTab.Rows.Add(dRow);
            }
            return dTab;
        }


        /// <summary>
        /// Get IDWR Sites from Web API given IDWR River System
        /// </summary>
        /// <param name="riverItem"></param>
        /// <returns></returns>
        public static DataTable GetIdwrRiverSites(string riverItem)
        {
            var dTab = new DataTable();
            var data = IdwrApiQueryRiverSites(riverItem);
            dTab.Columns.Add("SiteID", typeof(string));
            dTab.Columns.Add("SiteType", typeof(string));
            dTab.Columns.Add("SiteName", typeof(string));
            dTab.Columns.Add("HSTCount", typeof(string));
            dTab.Columns.Add("ALCCount", typeof(string));
            foreach (var item in data)
            {
                var dRow = dTab.NewRow();
                dRow["SiteID"] = item.SiteID;
                dRow["SiteType"] = item.SiteType;
                dRow["SiteName"] = item.SiteName;
                dRow["HSTCount"] = item.HSTCount;
                dRow["ALCCount"] = item.ALCCount;
                dTab.Rows.Add(dRow);
            }
            return dTab;
        }


        /// <summary>
        /// Get IDWR Site Info from Web API given IDWR Site ID
        /// </summary>
        /// <param name="siteID"></param>
        /// <returns></returns>
        public static DataTable GetIdwrSiteInfo(string siteID)
        {
            var dTab = new DataTable();
            dTab.Columns.Add("SiteID", typeof(string));
            dTab.Columns.Add("SiteType", typeof(string));
            dTab.Columns.Add("StationName", typeof(string));
            dTab.Columns.Add("FullName", typeof(string));
            dTab.Columns.Add("Years", typeof(string));
            var sInfo = IdwrApiQuerySiteInfo(siteID);
            var sYears = IdwrApiQuerySiteYears(siteID);
            var dRow = dTab.NewRow();
            dRow["SiteID"] = sInfo[0].SiteId;
            dRow["SiteType"] = sInfo[0].SiteType;
            dRow["StationName"] = sInfo[0].StationName;
            dRow["FullName"] = sInfo[0].FullName;
            var yearStr = "";
            foreach (var year in sYears)
            {
                yearStr += year + ",";
            }
            dRow["Years"] = yearStr.Trim(',');
            dTab.Rows.Add(dRow);
            return dTab;
        }
    }
}
