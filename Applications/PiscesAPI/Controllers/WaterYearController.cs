using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PiscesAPI.Models;
using PiscesWebServices.CGI;
using Reclamation.Core;

namespace PiscesAPI.Controllers
{
    [Route("wyreport/")]
    public class WaterYearController : Controller
    {

        /// <summary>
        /// Retrieve Water Year Report
        /// wyreport?site=pal&parameter=af&format=csv-analysis
        /// http://lrgs1.pn.usbr.gov/pn-bin/wyreport?site=abei&parameter=pp&start=2012&end=2012&format=usgs-html
        /// 
        /// </summary>
        [HttpGet()]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public ContentResult Get()
        {
            int start = GetInt(Request, "start", 0);
            int end = GetInt(Request, "end", 0);
            var site = GetStr(Request, "site");
            var parameter = GetStr(Request, "parameter");
            var format = GetStr(Request, "format", "usgs-html");
            var db = Database.GetTimeSeriesDatabase();


            WaterYearReport r = new WaterYearReport(db, site, parameter, start, end, format);
            var x = r.Run();
            Response.ContentType = "text/plain";
            if (format == "usgs-html")
                Response.ContentType = "Content-type: text/html";
            if (format == "csv-analysis")
                Response.ContentType = "Content-type: text/csv";

            //var pre = "";
            //if (format == "usgs-html")
            //    pre = "<html>;"
           return Content(x);
        }

        private string GetStr(HttpRequest request, string key, string defaultValue = "")
        {
            if (request.Query.ContainsKey(key))
                return request.Query[key];
            return defaultValue;
        }
        private int GetInt(HttpRequest request, string key, int defaultValue = 0)
        {
            if (request.Query.ContainsKey(key))
            {
                int rval = defaultValue;
                if (int.TryParse(request.Query[key], out rval))
                    return rval;
            }
            return defaultValue;
        }

        ///// <summary>
        ///// Retrieve Water Year Report
        ///// </summary>
        ///// <param name="site"> siteid i.e. 'low'</param>
        ///// <param name="parameter">parameter i.e. 'qd'</param>
        ///// <param name="start">first water year</param>
        ///// <param name="end">ending water year</param>
        ///// <param name="format">output format: csv-analysis|usgs-html</param>
        //[HttpGet("{site},{parameter}")]
        //[ProducesResponseType(typeof(string), 200)]
        //[ProducesResponseType(typeof(string), 400)]
        //[ProducesResponseType(typeof(void), 500)]
        //public ContentResult Get(string site, string parameter, 
        //    int start=0, int end = 0,string format = "usgs-html")
        //{
        //    //"/pn-bin/wyreport.pl?site="+site+"&parameter="+pc+"&format=csv-analysis",
        //    var db = Database.GetTimeSeriesDatabase();
        //    WaterYearReport r = new WaterYearReport(db, site, parameter, start, end, format);
        //    var x = r.Run();
        //    Response.ContentType = "text/plain";
        //    if (format == "usgs-html")
        //        Response.ContentType = "Content-type: text/html";
        //    if (format == "csv-analysis")
        //            Response.ContentType = "Content-type: text/csv";

        //    return Content(x);//,Response.ContentType);
        //}
    }
}
