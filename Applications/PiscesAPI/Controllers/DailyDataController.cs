using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PiscesAPI.Models;

namespace PiscesAPI.Controllers
{
    [Route("daily/")]
    public class DailyDataController : Controller
    {
        /// <summary>
        /// Retrieve daily TS data
        /// </summary>
        [HttpGet("{url}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public string Get(string url)
        {
            var x = "Content-type: text/html\n\n";
            return x+"\n < HTML >< HEAD >< TITLE";
            //return Ok(url).ToString();
            //var seriesdataProcessor = new DataAccessLayer.SeriesDataRepository();
            //return Ok(seriesdataProcessor.GetSeriesData(tstable, t1, t2));
        }

    }
}
