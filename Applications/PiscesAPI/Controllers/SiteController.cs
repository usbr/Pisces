using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PiscesAPI.Models;

namespace PiscesAPI.Controllers
{
    [Route("sites/")]
    public class SiteController : Controller
    {
        /// <summary>
        /// Retrieves sites, or specific site by siteid
        /// </summary>
        /// <remarks> Retrieves sites, or specific site by siteid=mysite </remarks>
        /// <response code="200">Site fetched</response>
        /// <response code="400">Site has missing/invalid values</response>
        /// <response code="500">Oops! Can't fetch your site right now</response>
        [HttpGet()]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public OkObjectResult Get(string siteid="")
        {
            var siteProcessor = new DataAccessLayer.SiteRepository();
            return Ok(siteProcessor.GetSites(siteid, siteid != ""));
        }

        /// <summary>
        /// Adds or modifies a specific site by unique id
        /// </summary>
        /// <remarks>Long description for this API endpoint goes here...</remarks>
        /// <response code="200">Site(s) created</response>
        /// <response code="400">Site(s) has missing/invalid values</response>
        /// <response code="500">Oops! Can't create your site(s) right now</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public OkObjectResult Post([FromBody]List<SiteModel.PiscesSite> input)
        {
            var siteProcessor = new DataAccessLayer.SiteRepository();
            var rval = siteProcessor.AddOrUpdateSites(input);
            return Ok(rval);
        }

        /// <summary>
        /// Deletes a specific site by unique id
        /// </summary>
        /// <remarks>Long description for this API endpoint goes here...</remarks>
        /// <response code="200">Site(s) deleted</response>
        /// <response code="400">Site(s) has missing/invalid values</response>
        /// <response code="500">Oops! Can't delete your site(s) right now</response>
        [HttpDelete]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public OkObjectResult Delete([FromBody]List<SiteModel.PiscesSite> input)
        {
            var siteProcessor = new DataAccessLayer.SiteRepository();
            var rval = siteProcessor.DeleteSites(input);
            return Ok(rval);
        }
    }
}
