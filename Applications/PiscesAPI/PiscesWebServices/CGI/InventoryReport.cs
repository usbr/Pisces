using Reclamation.Core;
using Reclamation.TimeSeries;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace PiscesWebServices.CGI
{
    /// <summary>
    /// inventory?site=ABEI&interval=daily[&ui=true|false][&format=csv|html]&por=false|true
    /// 
    /// site = cbtt  (required)
    /// interval = daily or instant time step (default = 'daily')
    /// ui = true to show html user interface for selecting parameters (default = 'false')
    /// format = html or csv (default = 'html')
    /// por = include period of record   (default = 'true')
    /// 
    /// </summary>
    public partial class InventoryReport
    {
        private TimeSeriesDatabase db;
        private string query;
        StringBuilder m_sb = new StringBuilder();

        public InventoryReport(TimeSeriesDatabase db, string payload)
        {
            this.db = db;
            this.query = payload;
        }


        internal string Run()
        {
            if (query == "")
            {
                query = HydrometWebUtility.GetQuery();
            }

            if (query == "")
                StopWithError("invalid query");

            query = System.Uri.UnescapeDataString(query);
            Logger.WriteLine("query='"+query+"'");
            var collection = HttpUtility.ParseQueryString(query);

            var siteID = "";
            if (collection.AllKeys.Contains("site"))
            {
                siteID = collection["site"].ToLower();
            }

            if( siteID == "" || Regex.IsMatch(siteID,"[^a-zA-Z0-9_=]"))
            {
                StopWithError("invalid query");
            }

            bool ui = LookupUI(collection);
            var interval = GetInterval(collection);

            string format = LookupFormat(collection);

            if (format == "html")
            {
                PrintHtmlInventory(siteID, interval, ui);
            }
            else if (format == "csv")
            {
                //PrintCsvInve
            }

            return m_sb.ToString();

        }

        private string LookupFormat(NameValueCollection collection)
        {
            var rval = "html";
            if(collection.AllKeys.Contains("format"))
            {
                rval = collection["format"];
            }

            return rval;
        }

        private bool LookupUI(NameValueCollection collection)
        {
           return collection.AllKeys.Contains("ui")
               && collection["ui"].ToLower() == "true";
        }

        private TimeInterval GetInterval(NameValueCollection collection)
        {
            var interval = TimeInterval.Daily;
            if (collection.AllKeys.Contains("interval"))
            {
                if (collection["interval"].ToLower() == "daily")
                {
                    interval = TimeInterval.Daily;
                }
                else
                    if (collection["interval"].ToLower() == "instant")
                    {
                    interval = TimeInterval.Irregular;
                    }
                    else
                    {
                        StopWithError("Error: bad or missing interval");
                    }
            }
            return interval;
        }

        private void PrintHtmlInventory(string siteID, TimeInterval interval, bool ui)
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
                string cgiTag = "";
                if (CgiUtility.IsRemoteRequest())
                    cgiTag = ".pl";
                if( interval == TimeInterval.Daily)
                    WriteLine("<form name=\"Form\" action=\"/pn-bin/daily"+cgiTag+"\" method=\"get\" >");
                else
                    WriteLine("<form name=\"Form\" action=\"/pn-bin/instant"+cgiTag+"\" method=\"get\" >");

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
            m_sb.AppendLine(s);
        }


        private  void StopWithError(string message)
        {
            m_sb.AppendLine("Error: " + message);
            m_sb.AppendLine(Help.PrintInventory());
            throw new Exception(message);
        }


    }
}