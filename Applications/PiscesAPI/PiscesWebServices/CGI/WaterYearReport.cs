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
        TimeRange r;
        string siteid, parameter;
        public WaterYearReport(TimeSeriesDatabase db, 
            string siteid, string parameter,
            int start=0, int end=0,string format = "usgs-html")
        {
            this.db = db;
            this.siteid = siteid;
            this.parameter = parameter;
            this.format = format;
            Validation(siteid, parameter);

            if (start <= 0)
            { // default start
                start = DateTime.Now.Date.Year;

                if (DateTime.Now.Date.Month >= 10)
                    start = DateTime.Now.Date.Year + 1;
            }
            if( end <=0)
            {
                end = DateTime.Now.Date.Year;
            }

            //creates a water year time range between the selected dates y1, y2
            r = new TimeRange(
                     WaterYear.BeginningOfWaterYear(new DateTime(start, 1, 1)),
                     WaterYear.EndOfWaterYear(new DateTime(end, 1, 1)));
        }



        internal string Run()
        {

            if (Array.IndexOf(supportedFormats, format) < 0)
                StopWithError("Error: invalid format " + format);

            if ( format == "usgs-html")
                 return PrintHtmlReport(r, siteid, parameter);

            if(format == "csv-analysis")
            {
                return PrintAnalysis(siteid,parameter);
            }

            return "";
        }

        /// <summary>
        /// prints csv table in this format.
        /// DateTime,current year, previous, average
        /// 10/1/2017, 123.34,   69.0,   77.7
        /// 10/2/2017, 120.0,   67.0,   77.3
        /// </summary>
        private string PrintAnalysis( string siteID, string parameter)
        {
            var years = new List<int>();
            var current = DateTime.Now.Date.WaterYear();
            var prev = current - 1;
            years.Add(current);
            years.Add(prev);
            DateTime startOf30YearAvearge = new DateTime(1980, 10, 1);

            var x = new SeriesList();

            var s = db.GetSeriesFromTableName("daily_" + siteID + "_" + parameter);
            if( s == null)
            {
                return "Error:  no data found: " + siteID + "/" + parameter;
            }
            x.Add(s);
            var result = PiscesAnalysis.WaterYears(x, years.ToArray(), true, 10, true,startOf30YearAvearge);
            var tbl = result.ToDataTable(true);
            // Console.WriteLine("<pre/>");
            StringBuilder sb = new StringBuilder();
            sb.Append("DateTime,Current Year,Previous Year,Average");
            
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                var o = tbl.Rows[i];
                var str = ((DateTime)o[0]).ToString("yyyy/MM/dd");
                sb.AppendLine();
                sb.Append(str+","+o[1].ToString()+","+o[2].ToString()+","+o[3].ToString());
            }
            return sb.ToString();
        }

        private string PrintHtmlReport(TimeRange r, string siteID, string parameter)
        {
            var s = db.GetSeriesFromTableName("daily_" + siteID + "_" + parameter);
            if (s == null)
            {
                return "Error:  no data found: " + siteID + "/" + parameter;
            }

            var startYear = r.StartDate.Year;
            var endYear = r.EndDate.Year;
            DateTime t1 = r.StartDate;

            StringBuilder sb = new StringBuilder();
            for (int i = startYear; i < endYear; i++)
            {
                s.Read(t1, t1.AddMonths(12));
                DataTable wyTable = Usgs.WaterYearTable(s);
                var header = GetHeader(i + 1, siteID, parameter);
                var html = DataTableOutput.ToHTML(wyTable, true, "", header);
                sb.Append(html);
                sb.AppendLine();
                t1 = t1.AddMonths(12);
            }

            return sb.ToString();
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

        
        private static void StopWithError(string message)
        {
            Console.WriteLine("Error: " + message);
            throw new Exception(message);
        }


    }
}