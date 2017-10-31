using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PiscesAPI.Controllers
{
    [Route("api/[controller]")]
    public class SampleEndPointsController : Controller
    {
        /// <summary>
        /// Retrieves all products
        /// </summary>
        /// <remarks>Long description for this API endpoint goes here...</remarks>
        /// <response code="200">Products fetched</response>
        /// <response code="400">Products have missing/invalid values</response>
        /// <response code="500">Oops! Can't fetch products right now</response>
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// Retrieves a specific product by unique id
        /// </summary>
        /// <remarks>Long description for this API endpoint goes here...</remarks>
        /// <response code="200">Product fetched</response>
        /// <response code="400">Product has missing/invalid values</response>
        /// <response code="500">Oops! Can't fetch your product right now</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// Adds or modifies a specific product by unique id
        /// </summary>
        /// <remarks>Long description for this API endpoint goes here...</remarks>
        /// <response code="200">Product(s) created</response>
        /// <response code="400">Product(s) has missing/invalid values</response>
        /// <response code="500">Oops! Can't create your product(s) right now</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        /// Deletes a specific product by unique id
        /// </summary>
        /// <remarks>Long description for this API endpoint goes here...</remarks>
        /// <response code="200">Product(s) deleted</response>
        /// <response code="400">Product(s) has missing/invalid values</response>
        /// <response code="500">Oops! Can't delete your product(s) right now</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public void Delete(int id)
        {
        }
    }
}
