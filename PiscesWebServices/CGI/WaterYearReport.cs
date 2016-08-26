using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Reclamation.TimeSeries;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using Reclamation.Core;
using System.Net;
using System.Text;
using Reclamation.TimeSeries.Hydromet;

namespace PiscesWebServices.CGI
{
    /// <summary>
    /// http://www.usbr.gov/pn-bin/yearrpt.pl?site=ABEI&parameter=PP&START=2016&END=2016
    /// </summary>
    public partial class WaterYearReport
    {
        private TimeSeriesDatabase db;
        private string query;

        public WaterYearReport(TimeSeriesDatabase db, string payload)
        {
            this.db = db;
            this.query = payload;
        }


        internal void Run()
        {
            Console.Write("Content-type: text/html\n\n");
            if (query == "")
            {
                query = HydrometWebUtility.GetQuery();
            }


            var collection = HttpUtility.ParseQueryString(query);

            TimeRange r = GetDateRange(collection);

            var siteID = "";
            if (collection.AllKeys.Contains("site"))
            {
                siteID = collection["site"];
            }

            var parameter = "";
            if (collection.AllKeys.Contains("parameter"))
            {
                parameter = collection["parameter"];
            }

            var s = new HydrometDailySeries(siteID, parameter);
            var startYear = r.StartDate.Year;
            var endYear = r.EndDate.Year;
            DateTime t1 = r.StartDate;

                for (int i = startYear; i < endYear; i++)
                {
                    s.Read(t1, t1.AddMonths(12));
                    DataTable wyTable = Reports.WaterYearTable(s);
                    var header = GetHeader(i, siteID,parameter);
                    Console.WriteLine(header);
                    var html = DataTableOutput.ToHTML(wyTable,true, "Water Year Report",header);
                    Console.WriteLine(html);
                    t1 = t1.AddMonths(12);
                }
        }

        private string GetHeader(int year, string siteID, string parameter)
        {
            string siteDescription = GetSiteDescription(siteID);
            string parameterDescription = GetParameterDescription(parameter, TimeInterval.Daily);

            return "";
//            @"<thead align="center"><tr><td colspan="13"><b>Station  ABEI - ABERDEEN IDAHO WEATHER STATION<br /
//Parameter PP - Precipitation, 24 Hour Total, Inches<br />
//Report for Water Year 2014<br />
//Bureau of Reclamation AgriMet System<br /></b>
//Provisional Data, Subject to Change</thead>";
        }

        private string GetSiteDescription(string siteID)
        {
             var sc = db.GetSiteCatalog("siteid = '" + siteID + "'");
             if (sc.Count != 0)
                 return sc[0].description;
             return "";
        }

        private string GetParameterDescription(string parameterCode, TimeInterval interval)
        {
            string id = parameterCode + "." + interval.ToString();
            var pc = db.GetParameterCatalog("id = '"+id+"'");
            if (pc.Count != 0)
                return pc[0].name;
            return "";
        }


        private static TimeRange GetDateRange(NameValueCollection collection)
        {
            var start = "";
            if (collection.AllKeys.Contains("start"))
            {
                start = collection["start"];
            }
            var end = "";
            if (collection.AllKeys.Contains("end"))
            {
                end = collection["end"];
            }

            int y1 = 0, y2 = 0;

            if (!int.TryParse(start, out y1) || !int.TryParse(end, out y2))
            {
                StopWithError("Error with year range");
            }
            //creates a water year time range between the selected dates y1, y2
            var rval = new TimeRange(
                     WaterYear.BeginningOfWaterYear(new DateTime(y1, 1, 1)),
                     WaterYear.EndOfWaterYear(new DateTime(y2, 1, 1)));
            return rval;
        }

        private static void StopWithError(string message)
        {
            Console.WriteLine("Error: " + message);
            throw new Exception(message);
        }


    }
}