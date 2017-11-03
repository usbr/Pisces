using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PiscesAPI.Models;

namespace PiscesAPI.Controllers
{
    [Route("[controller]")]
    public class TimeSeriesDataController : Controller
    {
        /// <summary>
        /// Retrieve TS data by table name
        /// </summary>
        /// <remarks>Long description for this API endpoint goes here...</remarks>
        /// <response code="200">TS fetched</response>
        /// <response code="400">TS has missing/invalid values</response>
        /// <response code="500">Oops! Can't fetch your TS right now</response>
        [HttpGet("{seriestable}/{t1}/{t2}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public OkObjectResult Get(string seriestable, DateTime t1, DateTime t2)
        {
            var seriesdataProcessor = new DataAccessLayer.SeriesDataRepository();
            return Ok(seriesdataProcessor.GetSeriesData(seriestable, t1, t2));
        }

        /// <summary>
        /// Retrieve TS data by Series object
        /// </summary>
        /// <remarks>Long description for this API endpoint goes here...</remarks>
        /// <response code="200">TS(s) created</response>
        /// <response code="400">TS(s) has missing/invalid values</response>
        /// <response code="500">Oops! Can't create your TS(s) right now</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public OkObjectResult Post([FromBody]List<SeriesModel.PiscesSeries> series, DateTime t1, DateTime t2)
        {
            var seriesdataProcessor = new DataAccessLayer.SeriesDataRepository();
            return Ok(seriesdataProcessor.GetSeriesData(series[0], t1, t2));
        }

        /// <summary>
        /// Deletes TS data points
        /// </summary>
        /// <remarks>Long description for this API endpoint goes here...</remarks>
        /// <response code="200">TS(s) deleted</response>
        /// <response code="400">TS(s) has missing/invalid values</response>
        /// <response code="500">Oops! Can't delete your TS(s) right now</response>
        [HttpDelete]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public OkObjectResult Delete([FromBody]List<SiteModel.PiscesSite> input)
        {
            throw new NotImplementedException();
        }
    }
}
