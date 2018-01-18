using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PiscesAPI.Models;
using PiscesWebServices.CGI;

namespace PiscesAPI.Controllers
{
    [Route("daily/")]
    public class DailyDataController : Controller
    {
        /// <summary>
        /// Retrieve daily TS data
        /// </summary>
        [HttpGet("{query}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public string Get(string query)
        {
            var db = Database.GetTimeSeriesDatabase();
            var w = new WebTimeSeriesWriter(db, Reclamation.TimeSeries.TimeInterval.Daily, query);
            var fn = Reclamation.Core.FileUtility.GetTempFileName(".web1");
            w.Run(fn);
            var x = "Content-type: text/html\n\n";
            x += "\n < HTML >< HEAD >< TITLE" + System.IO.File.ReadAllText(fn);

            System.IO.File.Delete(fn);

            return x;
            //return Ok(url).ToString();
            //var seriesdataProcessor = new DataAccessLayer.SeriesDataRepository();
            //return Ok(seriesdataProcessor.GetSeriesData(tstable, t1, t2));

        }

    }
}
