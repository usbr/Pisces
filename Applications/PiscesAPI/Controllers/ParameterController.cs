using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PiscesAPI.Models;

namespace PiscesAPI.Controllers
{
    [Route("parameters/")]
    public class ParameterController : Controller
    {
        /// <summary>
        /// Retrieves all parameters
        /// </summary>
        /// <remarks>Long description for this API endpoint goes here...</remarks>
        /// <response code="200">Parameters fetched</response>
        /// <response code="400">Parameters have missing/invalid values</response>
        /// <response code="500">Oops! Can't fetch parameters right now</response>
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public OkObjectResult Get()
        {
            var paramProcessor = new DataAccessLayer.ParameterRepository();
            return Ok(paramProcessor.GetParameters());
        }

        /// <summary>
        /// Retrieves a specific parameter by id
        /// </summary>
        /// <remarks>Long description for this API endpoint goes here...</remarks>
        /// <response code="200">Parameters fetched</response>
        /// <response code="400">Parameters has missing/invalid values</response>
        /// <response code="500">Oops! Can't fetch your parameter right now</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public OkObjectResult Get(string id)
        {
            var paramProcessor = new DataAccessLayer.ParameterRepository();
            return Ok(paramProcessor.GetParameters(id));
        }

        /// <summary>
        /// Adds or modifies a specific parameter by unique id
        /// </summary>
        /// <remarks>Long description for this API endpoint goes here...</remarks>
        /// <response code="200">Parameter(s) created</response>
        /// <response code="400">Parameter(s) has missing/invalid values</response>
        /// <response code="500">Oops! Can't create your parameter(s) right now</response>
        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public OkObjectResult Post([FromBody]List<ParameterModel.PiscesParameter> input)
        {
            var paramProcessor = new DataAccessLayer.ParameterRepository();
            var rval = paramProcessor.AddOrUpdateParameters(input);
            return Ok(rval);
        }

        /// <summary>
        /// Deletes a specific parameter by unique id
        /// </summary>
        /// <remarks>Long description for this API endpoint goes here...</remarks>
        /// <response code="200">Parameter(s) deleted</response>
        /// <response code="400">Parameter(s) has missing/invalid values</response>
        /// <response code="500">Oops! Can't delete your parameter(s) right now</response>
        [HttpDelete]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public OkObjectResult Delete([FromBody]List<ParameterModel.PiscesParameter> input)
        {
            var paramProcessor = new DataAccessLayer.ParameterRepository();
            var rval = paramProcessor.DeleteParameters(input);
            return Ok(rval);
        }
    }
}
