using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PiscesAPI.Controllers
{
    [Route("sample/[controller]")]
    public class xxSampleEndPointsController : Controller
    {
        /// <summary>
        /// Hydromet CGI Response
        /// </summary>
        /// <remarks>Test for outputting ASCII reponses</remarks>
        /// <response code="200">Products fetched</response>
        /// <response code="400">Products have missing/invalid values</response>
        /// <response code="500">Oops! Can't fetch products right now</response>
        [HttpGet("daily.pl")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public string Get([Bind(Prefix = "station")] string station, [Bind(Prefix = "pcode")] string pcode,
            [Bind(Prefix = "t1")] DateTime t1, [Bind(Prefix = "t2")] DateTime t2,
            [Bind(Prefix = "format")] string format)
        {
            string response = "<HTML><HEAD><TITLE>Hydromet/AgriMet Data Access</title></head><BODY BGCOLOR=#FFFFFF><p><PRE><B>USBR Pacific Northwest RegionHydromet/AgriMet Data Access</B><BR>Although the US Bureau of Reclamation makes efforts to maintain the accuracyof data found in the Hydromet system databases, the data is largely unverifiedand should be considered preliminary and subject to change.  Data and servicesare provided with the express understanding that the United States Governmentmakes no warranties, expressed or implied, concerning the accuracy, complete-ness, usability or suitability for any particular purpose of the informationor data obtained by access to this computer system, and the United Statesshall be under no liability whatsoever to any individual or group entity byreason of any use made thereof. </PRE><p><PRE>BEGIN DATADATE      ,   ACAO QD  12/07/2017,        8.0112/08/2017,        8.8912/09/2017,        8.2812/10/2017,        8.6112/11/2017,        8.6712/12/2017,        7.3012/13/2017,        8.8212/14/2017,        8.1412/15/2017,       10.8212/16/2017,       13.3112/17/2017,       11.8812/18/2017,       14.4412/19/2017,       20.0012/20/2017,        9.2712/21/2017,        9.2712/22/2017,        9.5612/23/2017,       11.4912/24/2017,       17.9012/25/2017,       24.3012/26/2017,       26.1412/27/2017,       13.8912/28/2017,        0.0012/29/2017,        0.0112/30/2017,        0.1412/31/2017,        0.0001/01/2018,        0.0001/02/2018,        0.0001/03/2018,        0.0001/04/2018,        0.0001/05/2018,        8.35END DATA</pre></body></html>";
            return response;
        }

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
