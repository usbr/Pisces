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
using Reclamation.TimeSeries.Analysis;

namespace PiscesWebServices.CGI
{
    /// <summary>
    /// wyreport?site=ABEI&parameter=PP&start=2016[&end=2016]
    /// </summary>
    public partial class WaterYearReport
    {
        private TimeSeriesDatabase db;
        private string query;
        private string format = "usgs-html";
        string[] supportedFormats = new string[] {"csv-analysis", // csv with previous year, and 30 year average
                                                "usgs-html" // usgs style in html
                                                };

        public WaterYearReport(TimeSeriesDatabase db, string payload)
        {
            this.db = db;
            this.query = payload;
        }


        internal void Run()
        {
         
            if (query == "")
            {
                query = HydrometWebUtility.GetQuery();
            }
            query = System.Uri.UnescapeDataString(query);

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

            if (collection.AllKeys.Contains("format"))
            {
                format = collection["format"];
            }

            if (Array.IndexOf(supportedFormats, format) < 0)
                StopWithError("Error: invalid format " + format);

            if ( format == "usgs-html")
                 PrintHtmlReport(r, siteID, parameter);

            if(format == "csv-analysis")
            {
                PrintAnalysis(r,siteID,parameter);
            }
        }

        /// <summary>
        /// prints csv table in this format.
        /// DateTime,current year, previous, average
        /// 10/1/2017, 123.34,   69.0,   77.7
        /// 10/2/2017, 120.0,   67.0,   77.3
        /// </summary>
        private void PrintAnalysis(TimeRange r, string siteID, string parameter)
        {
            Console.Write("Content-type: text/csv\n\n");
            var years = new List<int>();
            var current = DateTime.Now.Date.WaterYear();
            var prev = current - 1;
            years.Add(current);
            years.Add(prev);
            DateTime startOf30YearAvearge = HydrometDataUtility.T1Thirty;

            var x = new SeriesList();

            Series s = new HydrometDailySeries(siteID, parameter, HydrometHost.PNLinux);
            x.Add(s);
            var result = PiscesAnalysis.WaterYears(x, years.ToArray(), true, 10, true,startOf30YearAvearge);
            var tbl = result.ToDataTable(true);
           // Console.WriteLine("<pre/>");
            Console.WriteLine("DateTime,Current Year,Previous Year,Average");
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                var o = tbl.Rows[i];
                var str = ((DateTime)o[0]).ToString("yyyy/MM/dd");
                Console.WriteLine(str+","+o[1].ToString()+","+o[2].ToString()+","+o[3].ToString());   
            }
            
        }

        private void PrintHtmlReport(TimeRange r, string siteID, string parameter)
        {
            Console.Write("Content-type: text/html\n\n");
            var s = new HydrometDailySeries(siteID, parameter, HydrometHost.PNLinux);
            var startYear = r.StartDate.Year;
            var endYear = r.EndDate.Year;
            DateTime t1 = r.StartDate;

            for (int i = startYear; i < endYear; i++)
            {
                s.Read(t1, t1.AddMonths(12));
                DataTable wyTable = Usgs.WaterYearTable(s);
                var header = GetHeader(i + 1, siteID, parameter);
                var html = DataTableOutput.ToHTML(wyTable, true, "", header);
                Console.WriteLine(html);
                t1 = t1.AddMonths(12);
            }
        }

        private void Validation(string siteID, string parameter)
        {
            if( parameter == "" || Regex.IsMatch(parameter,"[^_a-z0-9A-Z]"))
            {
                StopWithError("invalid parameter ");
            }
            if (siteID == "" || Regex.IsMatch(siteID, "[^_a-z0-9A-Z]"))
            {
                StopWithError("invalid site ");
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
            else
            {
                start = DateTime.Now.WaterYear().ToString();
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
            Console.Write("Content-type: text/html\n\n");
            Help.PrintWaterYear();
            Console.WriteLine("Error: " + message);
            throw new Exception(message);
        }


    }
}