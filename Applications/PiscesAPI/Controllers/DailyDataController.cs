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
    [Route("daily/")]
    public class DailyDataController : Controller
    {
        /// <summary>
        /// Retrieve daily TS data
        /// </summary>
        /// <param name="query">query such as 'list=jck' or list ='jck af,pal af'</param>
        [HttpGet("{query}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public string Get(string query)
        {
            var db = Database.GetTimeSeriesDatabase();
            var w = new WebTimeSeriesWriter(db, Reclamation.TimeSeries.TimeInterval.Daily, query);

            string fn = System.IO.Path.GetTempPath();
            fn = System.IO.Path.Combine(fn, Guid.NewGuid() + ".hydromet-web");
            w.Run(fn);
            var x = System.IO.File.ReadAllText(fn);

            System.IO.File.Delete(fn);
            SetResponseType(w);
            return x;
        }

        private void SetResponseType(WebTimeSeriesWriter w)
        {
            if( w.Format == "csv")
                Response.ContentType = "Content-type: text/csv\nContent-Disposition: attachment; filename=hydromet.csv\n\n";
            else
            Response.ContentType = "text/html";
        }

        /// <summary>
        /// Retrieve daily TS data
        /// </summary>
        [HttpPost()]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public string Post()
        {
             var p = new Performance();
           
            var sr = new System.IO.StreamReader(Request.Body);
            var body = sr.ReadToEnd();
            sr.Close();

            if (body == "")
                throw new Exception("no data posted");
            var db = Database.GetTimeSeriesDatabase();
            var w = new WebTimeSeriesWriter(db, Reclamation.TimeSeries.TimeInterval.Daily, body);

            string fn = System.IO.Path.GetTempPath();
            fn = System.IO.Path.Combine(fn, Guid.NewGuid() + ".hydromet-web");
            w.Run(fn);
            var x = System.IO.File.ReadAllText(fn);

            System.IO.File.Delete(fn);

            p.Report();
            Response.ContentType = "karl";
            return x;
            //return Ok(url).ToString();
            //var seriesdataProcessor = new DataAccessLayer.SeriesDataRepository();
            //return Ok(seriesdataProcessor.GetSeriesData(tstable, t1, t2));

        }

    }
}
