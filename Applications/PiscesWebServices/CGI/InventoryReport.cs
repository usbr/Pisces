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
    /// inventory?site=ABEI&interval=daily[&ui=true|false]
    /// </summary>
    public partial class InventoryReport
    {
        private TimeSeriesDatabase db;
        private string query;

        public InventoryReport(TimeSeriesDatabase db, string payload)
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

            if (query == "")
                StopWithError("invalid query");

            var collection = HttpUtility.ParseQueryString(query);

            var siteID = "";
            if (collection.AllKeys.Contains("site"))
            {
                siteID = collection["site"];
            }

            if( siteID == "" || Regex.IsMatch(siteID,"[^a-zA-Z0-1_]"))
            {
                StopWithError("invalid or missing site");
            }

            TimeInterval interval;
            bool ui = LookupUI(collection);
            SetInterval(collection, out interval);

            PrintInventory(siteID, interval,ui);
            

        }

        private bool LookupUI(NameValueCollection collection)
        {
           return collection.AllKeys.Contains("ui")
               && collection["ui"].ToLower() == "true";
        }

        private static void SetInterval(NameValueCollection collection, out TimeInterval interval)
        {
            interval = TimeInterval.Daily;
            if (collection.AllKeys.Contains("interval"))
            {
                if (collection["interval"].ToLower() == "daily")
                {
                    interval = TimeInterval.Daily;
                }
                else
                    if (collection["interval"].ToLower() == "instant")
                    {

                    }
                    else
                    {
                        StopWithError("Error: bad or missing interval");
                    }
            }
        }

        private void PrintInventory(string siteID, TimeInterval interval, bool ui)
        {
            var parms = db.GetParameters(siteID, interval);
            var desc = db.GetSiteDescription(siteID);

            WriteLine("<!DOCTYPE html>");
            WriteLine("<html>");

            

            DataTable tbl = new DataTable();
            tbl.Columns.Add("parameter");
            tbl.Columns.Add("available records");
            tbl.Columns.Add("description");
            int firstYear = DateTime.Now.Year;
            int minYr=0, maxYr=0;
            int min=0, max=0;

            for (int i = 0; i < parms.Length; i++)
            {
                string por = db.GetPeriodOfRecord(siteID, parms[i], interval,out minYr,out maxYr);
                if( i == 0)
                {
                   min = minYr;
                   max = maxYr;
                }
                if (minYr < min)
                    min = minYr;
                if (maxYr > max)
                    max = maxYr;

                //Console.WriteLine("por = "+por);
                if (ui)
                {
                    var cb = " <input type=\"checkbox\" name=\"pcode\" value=\"" + parms[i] + "\" id=\"" + parms[i] + "\">"+parms[i].ToUpper();
                    tbl.Rows.Add(cb, db.GetParameterDescription(parms[i], interval), por);
                }
                else
                    tbl.Rows.Add(parms[i], db.GetParameterDescription(parms[i], interval), por);
            }

            if (ui)
            {
                WriteLine("<form name=\"Form\" action=\"/pn-bin/daily.pl\" method=\"get\" >");
                WriteLine("<input name=station type=\"hidden\" value=\"" + siteID + "\">");
                WriteLine("<input name=format type=\"hidden\" value=\"html\">");

                WriteTimeSelector(siteID, min,max);
                
            }
            var s = DataTableOutput.ToHTML(tbl,false,desc);
            WriteLine(s);

            WriteLine("<p><input type=\"submit\" value=\"Retrieve Daily Data\">");

            if( ui)
              WriteLine("</form>");
            //WriteLine("<\\html>");
        }

        private void WriteTimeSelector(string siteID, int firstYear,int lastYear)
        {
            DateTime now = DateTime.Now.Date.AddDays(-30);
        WriteLine("<p>Start:");
        WriteLine("&nbsp;&nbsp;&nbsp;&nbsp;Year:");
        WriteLine(HtmlElement.SelectIntRange("year",firstYear,lastYear,now.Year));

        WriteLine("</select>&nbsp;&nbsp;&nbsp;Month:");
        WriteLine(HtmlElement.SelectMonth("month",now.Month));

        WriteLine("&nbsp;&nbsp;&nbsp;Day:");
        WriteLine(HtmlElement.SelectIntRange("day",1,31,now.Day));


        now = DateTime.Now.Date.AddDays(-1);

       WriteLine("<p>End:");
       WriteLine("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Year:");
       WriteLine(HtmlElement.SelectIntRange("year",firstYear,lastYear,now.Year));

       WriteLine("&nbsp;&nbsp;&nbsp;Month:");
       WriteLine(HtmlElement.SelectMonth("month",now.Month));

      WriteLine("&nbsp;&nbsp;&nbsp;Day:");
      WriteLine(HtmlElement.SelectIntRange("day",1,31,now.Day));
      
        }

        private void WriteLine(string s)
        {
            Console.WriteLine(s);
        }


        private static void StopWithError(string message)
        {
            Console.WriteLine("Error: " + message);
            throw new Exception(message);
        }


    }
}