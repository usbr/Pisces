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
    [Route("inventory/")]
    public class InventoryController : Controller
    {
        /// <summary>
        /// Retrieve inventory of data.
        /// /inventory?site=STDO&ui=true&interval=instant
        /// </summary>
        [HttpGet()]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public ContentResult Get()
        {
            var query = Request.QueryString.Value;
            if (query.IndexOf("?") == 0)
                query = query.Substring(1);


            var db = Database.GetTimeSeriesDatabase();
            var w = new InventoryReport(db, query);
            var x = w.Run();
            return Content(x, "text/html");
        }


    }
}
