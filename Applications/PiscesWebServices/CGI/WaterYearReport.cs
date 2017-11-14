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
using Reclamation.TimeSeries.Reports;

namespace PiscesWebServices.CGI
{
    /// <summary>
    /// wyreport?site=ABEI&parameter=PP&start=2016[&end=2016]
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

            Validation(siteID, parameter);

            var s = new HydrometDailySeries(siteID, parameter, HydrometHost.PNLinux);
            var startYear = r.StartDate.Year;
            var endYear = r.EndDate.Year;
            DateTime t1 = r.StartDate;

                for (int i = startYear; i < endYear; i++)
                {
                    s.Read(t1, t1.AddMonths(12));
                    DataTable wyTable = Usgs.WaterYearTable(s);
                    var header = GetHeader(i + 1, siteID,parameter);
                    var html = DataTableOutput.ToHTML(wyTable, true, "",header);
                    Console.WriteLine(html);
                    t1 = t1.AddMonths(12);
                }
        }

        private void Validation(string siteID, string parameter)
        {
            if( parameter == "" || Regex.IsMatch(parameter,"[^_a-z0-1A-Z]"))
            {
                StopWithError("invalid parameter ");
            }
            if (siteID == "" || Regex.IsMatch(siteID, "[^_a-z0-1A-Z]"))
            {
                StopWithError("invalid parameter ");
            }
        }

        private string GetHeader(int year, string siteID, string parameter)
        {
            string siteDescription = db.GetSiteDescription(siteID);
            string parameterDescription = db.GetParameterDescription(parameter, TimeInterval.Daily);

            return "<thead align=\"center\"><tr><td colspan=\"13\"><b>Station  "+ siteDescription +"<br />" +
                    parameterDescription + "<br />" +
                    "Report for Water Year " + year + "<br />"+
                    "Bureau of Reclamation Hydromet/AgriMet System<br /></b>"+
                    "Provisional Data, Subject to Change</thead>";

        }

        

       


        private static TimeRange GetDateRange(NameValueCollection collection)
        {
            var start = DateTime.Now.Date.Year.ToString();

            if (DateTime.Now.Date.Month >= 10)
                start = (DateTime.Now.Date.Year + 1).ToString();

            if (collection.AllKeys.Contains("start"))
            {
                start = collection["start"];
            }
            var end = start;
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
            Help.PrintWaterYearHelp();
            Console.WriteLine("Error: " + message);
            throw new Exception(message);
        }


    }
}