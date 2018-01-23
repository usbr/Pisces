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
        /// Retrieve daily TS data
        /// </summary>
        /// <param name="query">query such as 'list=jck' or list ='jck af,pal af'</param>
        [HttpGet()]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public string Get(string query)
        {
            var db = Database.GetTimeSeriesDatabase();
            var w = new WebTimeSeriesWriter(db, Reclamation.TimeSeries.TimeInterval.Irregular, query);
            var x = w.Run(Response);
            return x;
        }

        /// <summary>
        /// Retrieve instant Time Series data
        /// </summary>
        /// <param name="parameter">site parameter combination: 'pdto q'</param>
        /// 
        /// <param name="smnth">starting month</param>
        [HttpGet()]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
         ContentResult Get(string parameter, int syer, int smnth)
        {
            
            var db = Database.GetTimeSeriesDatabase();
            var w = new WebTimeSeriesWriter(db, Reclamation.TimeSeries.TimeInterval.Irregular, parameter);
            var x = w.Run(Response);
            return Content(x);
        }
        //
        //instant? parameter = PDTO % 20Q&syer=2018&smnth=1&sdy=16&eyer=2018&emnth=1&edy=23&format=2


        /// <summary>
        /// Retrieve daily TS data
        /// </summary>
        [HttpPost()]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public string Post()
        {
            var sr = new System.IO.StreamReader(Request.Body);
            var body = sr.ReadToEnd();
            sr.Close();

            if (body == "")
                throw new Exception("no data posted");
            var db = Database.GetTimeSeriesDatabase();
            var w = new WebTimeSeriesWriter(db, Reclamation.TimeSeries.TimeInterval.Daily, body);
            var x = w.Run(Response);
            return x;

        }

    }
}
