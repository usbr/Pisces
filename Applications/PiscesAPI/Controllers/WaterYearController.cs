using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        /// </summary>
        /// <param name="site"> siteid i.e. 'low'</param>
        /// <param name="parameter">parameter i.e. 'qd'</param>
        /// <param name="start">first water year</param>
        /// <param name="end">ending water year</param>
        /// <param name="format">output format: csv-analysis|usgs-html</param>
        [HttpGet()]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public ContentResult Get(string site, string parameter, 
            int start=0, int end = 0,string format = "usgs-html")
        {
            //"/pn-bin/wyreport.pl?site="+site+"&parameter="+pc+"&format=csv-analysis",
            var db = Database.GetTimeSeriesDatabase();
            WaterYearReport r = new WaterYearReport(db, site, parameter, start, end, format);
            var x = r.Run();
            Response.ContentType = "text/plain";
            if (format == "usgs-html")
                Response.ContentType = "Content-type: text/html";
            if (format == "csv-analysis")
                    Response.ContentType = "Content-type: text/csv";

            return Content(x);//,Response.ContentType);
        }
    }
}
