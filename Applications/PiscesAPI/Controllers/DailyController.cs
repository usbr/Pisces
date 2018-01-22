﻿using System;
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
    public class DailyController : Controller
    {
        /// <summary>
        /// Retrieve daily TS data
        /// </summary>
        /// <param name="query">query such as 'list=jck' or list ='jck af,pal af'</param>
        [HttpGet("{query}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public ContentResult Get(string query)
        {
            var db = Database.GetTimeSeriesDatabase();
            var w = new WebTimeSeriesWriter(db, Reclamation.TimeSeries.TimeInterval.Daily, query);
            var x = w.Run(Response);
            return Content(x, Response.ContentType);
        }

        /// <summary>
        /// Retrieve daily TS data
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
            var w = new WebTimeSeriesWriter(db, Reclamation.TimeSeries.TimeInterval.Daily, body);
            var x =w.Run(Response);
            return Content(x, Response.ContentType);
        }

    }
}
