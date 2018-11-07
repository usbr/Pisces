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
    [Route("instant/")]
    public class InstantController : Controller
    {
        /// <summary>
        /// Retrieve instant time (typically 15 minute) series data
        /// </summary>
        [HttpGet()]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public ContentResult Get()
        {
            var query = Request.QueryString.Value;
            if (query.IndexOf("?") == 0)
                query = query.Substring(1);

            var db = Database.GetTimeSeriesDatabase();
            var w = new WebTimeSeriesWriter(db, Reclamation.TimeSeries.TimeInterval.Irregular, query);
            var x = w.Run(Response);
            return Content(x, Response.ContentType);
        }



        /// <summary>
        /// Retrieve instant time (typically 15 minute) series data
        /// </summary>
        [HttpPost()]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public ContentResult Post()
        {
            var sr = new System.IO.StreamReader(Request.Body);
            var body = sr.ReadToEnd();
            sr.Close();

            if (body == "")
                throw new Exception("no data posted");
            var db = Database.GetTimeSeriesDatabase();
            var w = new WebTimeSeriesWriter(db, Reclamation.TimeSeries.TimeInterval.Irregular, body);
            var x = w.Run(Response);
            return Content(x, Response.ContentType);
        }

    }
}
