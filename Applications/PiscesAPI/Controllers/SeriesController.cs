using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PiscesAPI.Models;

namespace PiscesAPI.Controllers
{
    [Route("[controller]")]
    public class TimeSeriesController : Controller
    {
        /// <summary>
        /// List all Series data objects
        /// </summary>
        /// <remarks>Long description for this API endpoint goes here...</remarks>
        /// <response code="200">TS fetched</response>
        /// <response code="400">TS have missing/invalid values</response>
        /// <response code="500">Oops! Can't fetch TS right now</response>
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public OkObjectResult Get()
        {
            var seriesProcessor = new DataAccessLayer.SeriesRepository();
            return Ok(seriesProcessor.GetSeries());
        }

        /// <summary>
        /// Retrieve Series object by id
        /// </summary>
        /// <remarks>Long description for this API endpoint goes here...</remarks>
        /// <response code="200">TS fetched</response>
        /// <response code="400">TS has missing/invalid values</response>
        /// <response code="500">Oops! Can't fetch your TS right now</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public OkObjectResult Get(string id)
        {
            var seriesProcessor = new DataAccessLayer.SeriesRepository();
            return Ok(seriesProcessor.GetSeries(id));
        }

        /// <summary>
        /// Adds or modifies a specific Series data object by unique id
        /// </summary>
        /// <remarks>Long description for this API endpoint goes here...</remarks>
        /// <response code="200">TS(s) created</response>
        /// <response code="400">TS(s) has missing/invalid values</response>
        /// <response code="500">Oops! Can't create your TS(s) right now</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public OkObjectResult Post([FromBody]List<SiteModel.PiscesSite> input)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deletes a specific Series data object by unique id
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
