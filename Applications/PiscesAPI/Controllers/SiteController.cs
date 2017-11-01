using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PiscesAPI.Models;

namespace PiscesAPI.Controllers
{
    [Route("[controller]")]
    public class SiteController : Controller
    {
        /// <summary>
        /// Retrieves all sites
        /// </summary>
        /// <remarks>Long description for this API endpoint goes here...</remarks>
        /// <response code="200">Sites fetched</response>
        /// <response code="400">Sites have missing/invalid values</response>
        /// <response code="500">Oops! Can't fetch sites right now</response>
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public OkObjectResult Get()
        {
            var siteProcessor = new DataAccessLayer.SiteRepository();
            return Ok(siteProcessor.GetSites());
        }

        /// <summary>
        /// Retrieves a specific site by id
        /// </summary>
        /// <remarks>Long description for this API endpoint goes here...</remarks>
        /// <response code="200">Site fetched</response>
        /// <response code="400">Site has missing/invalid values</response>
        /// <response code="500">Oops! Can't fetch your site right now</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public OkObjectResult Get(string id)
        {
            var siteProcessor = new DataAccessLayer.SiteRepository();
            return Ok(siteProcessor.GetSites(id));
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
        public OkObjectResult Post([FromBody]string value)
        {
            return Ok("Not Implemented");
        }

        /// <summary>
        /// Deletes a specific site by unique id
        /// </summary>
        /// <remarks>Long description for this API endpoint goes here...</remarks>
        /// <response code="200">Site(s) deleted</response>
        /// <response code="400">Site(s) has missing/invalid values</response>
        /// <response code="500">Oops! Can't delete your site(s) right now</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public OkObjectResult Delete(int id)
        {
            return Ok("Not Implemented");
        }
    }
}
