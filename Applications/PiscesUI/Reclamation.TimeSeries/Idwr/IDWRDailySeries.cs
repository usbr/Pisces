using Newtonsoft.Json;
using Reclamation.Core;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Reclamation.TimeSeries.IDWR
{
    /// <summary>
    /// IDWR Daily Data from API web service
    /// </summary>
    public class IDWRDailySeries : Series
    {
        static string idwrAPI = @"https://research.idwr.idaho.gov/apps/Shared/WaterServices/Accounting";
        public static RestClient idwrClient = new RestClient(idwrAPI);
        string station;
        string parameter;


        public IDWRDailySeries(string station, string parameter = "QD")
        {
            this.station = station;
            this.parameter = parameter;
            TimeInterval = TimeInterval.Daily;
            switch (parameter)
            {
                case "FB":
                case "GH":
                    Units = "feet";
                    break;
                case "QD":
                    Units = "cfs";
                    break;
                case "AF":
                    Units = "acre-feet";
                    break;
            }
            Name = parameter + "_" + station;
            Table.TableName = "IdwrDaily" + parameter.ToString() + "_" + station;
            ConnectionString = "Source=IDWR;SiteID=" + station.ToString() + ";Parameter=" + parameter.ToString();
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
                if (t1 == TimeSeriesDatabase.MinDateTime
                    || t2 == TimeSeriesDatabase.MaxDateTime)
                {
                    // assume we want all available data
                    var availYears = Utilities.IdwrApiQuerySiteYears(station);
                    var yearList = string.Join(",", availYears);
                    Add(IdwrApiDownload(station, parameter, yearList));
                }
                else
                {
                    Add(IdwrApiDownload(station, parameter, t1, t2));
                }
            }
        }


        protected override Series CreateFromConnectionString()
        {
            IDWRDailySeries s = new IDWRDailySeries(
               ConnectionStringUtility.GetToken(ConnectionString, "SiteID", ""),
               ConnectionStringUtility.GetToken(ConnectionString, "Parameter", ""));
            return s;
        }

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


        /// <summary>
        /// Method to call the data download API, get the JSON reponse, and convert to Series()
        /// </summary>
        /// <param name="station"></param>
        /// <param name="parameter"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        private static Series IdwrApiDownload(string station, string parameter,
            DateTime t1, DateTime t2)
        {
            var rval = new Series();

            var years = Enumerable.Range(t1.Year, t2.Year - t1.Year + 1);
            var yearlist = string.Join(",", years);

            var jsonResponse = new List<TsData>();
            try
            {
                jsonResponse = IdwrApiQuerySiteData(station, yearlist);
            }
            catch
            {
                return rval;
            }

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
                        rval.AddMissing(t);
                    }
                    else
                    {
                        rval.Add(item.Date, Convert.ToDouble(value));
                    }
                }
            }

            return rval;
        }

        /// <summary>
        /// Method to call the data download API, get the JSON reponse, and convert to Series()
        /// </summary>
        /// <param name="station"></param>
        /// <param name="parameter"></param>
        /// <param name="yearlist"></param>
        /// <returns></returns>
        private static Series IdwrApiDownload(string station, string parameter,
            string yearlist)
        {
            var rval = new Series();

            var jsonResponse = new List<TsData>();
            try
            {
                jsonResponse = IdwrApiQuerySiteData(station, yearlist);
            }
            catch
            {
                return rval;
            }

            foreach (var item in jsonResponse)
            {
                var t = DateTime.Parse(item.Date);
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
                    rval.AddMissing(t);
                }
                else
                {
                    rval.Add(item.Date, Convert.ToDouble(value));
                }
            }

            return rval;
        }
    }

    public class Utilities
    {
        internal static List<RiverItems> IdwrApiQueryRiverList()
        {
            // Working API Call
            //https://research.idwr.idaho.gov/apps/Shared/WaterServices/Accounting/RiverSystems

            var request = new RestRequest("RiverSystems/", Method.GET);
            IRestResponse restResponse = IDWRDailySeries.idwrClient.Execute(request);
            return JsonConvert.DeserializeObject<List<RiverItems>>(restResponse.Content);
        }


        internal static List<RiverSite> IdwrApiQueryRiverSites(string riverItem)
        {
            // Working API Call
            //https://research.idwr.idaho.gov/apps/Shared/WaterServices/Accounting/SitesByRiver?river=BAR

            var request = new RestRequest("SitesByRiver?", Method.GET);
            request.AddParameter("river", riverItem);
            IRestResponse restResponse = IDWRDailySeries.idwrClient.Execute(request);
            return JsonConvert.DeserializeObject<List<RiverSite>>(restResponse.Content);
        }

        internal static List<SiteInfo> IdwrApiQuerySiteInfo(string riverSite)
        {
            // Working API Call
            //https://research.idwr.idaho.gov/apps/Shared/WaterServices/Accounting/SiteDetails?sitelist=10039500

            var request = new RestRequest("SiteDetails?", Method.GET);
            request.AddParameter("sitelist", riverSite);
            IRestResponse restResponse = IDWRDailySeries.idwrClient.Execute(request);
            return JsonConvert.DeserializeObject<List<SiteInfo>>(restResponse.Content);
        }

        internal static List<string> IdwrApiQuerySiteYears(string riverSite)
        {
            // Working API Call
            //https://research.idwr.idaho.gov/apps/Shared/WaterServices/Accounting/HistoryAvailableYearsBySiteId?siteid=10039500&yeartype=CY&f=json

            var request = new RestRequest("HistoryAvailableYearsBySiteId?", Method.GET);
            request.AddParameter("siteid", riverSite);
            request.AddParameter("yeartype", "CY");
            request.AddParameter("f", "json");
            IRestResponse restResponse = IDWRDailySeries.idwrClient.Execute(request);
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
            dRow["Years"] = string.Join(",", sYears);
            dTab.Rows.Add(dRow);
            return dTab;
        }
    }
}
