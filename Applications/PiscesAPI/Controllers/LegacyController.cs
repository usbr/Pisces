//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using PiscesAPI.Models;
//using PiscesWebServices.CGI;
//using Reclamation.Core;

//namespace PiscesAPI.Controllers
//{
//    [Route("/[controller]")]
//    public class LegacyController : Controller
//    {

//        /// <summary>
//        /// Retrieve daily TS data
//        /// </summary>
//        /// <param name="interval">'daily' or 'instant' </param>
//        /// <param name="custom_list">name of custom list for sites: idwr_accounting|idwr</param>
//        /// <param name="list">list of sites: 'jck' or 'jck af,pal af'</param>
//        /// <param name="start">starting date: 2018-12-25</param>
//        /// <param name="end">ending date: 2018-12-30</param>
//        /// <param name="back">number of days back</param>
//        /// <param name="parameter">site and parameter: 'jck af'</param>
//        /// <param name="syer">start year</param>
//        /// <param name="smnth">start month</param>
//        /// <param name="sdy">start day</param>
//        /// <param name="eyer">start year</param>
//        /// <param name="emnth">start month</param>
//        /// <param name="edy">start day</param>
//        /// <param name="print_hourly">set print_hourly=true for hourly only (only applies to instant)</param>
//        /// <param name="flags">set print_flags=true to output flagged data</param>
//        /// <param name="format">output format: csv|html</param>
//        [HttpGet()]
//        [ProducesResponseType(typeof(string), 200)]
//        [ProducesResponseType(typeof(string), 400)]
//        [ProducesResponseType(typeof(void), 500)]
//        public ContentResult Get(
//            string interval,
//            string custom_list ,
//            string list,
//            string start , string end , int back ,
//            string parameter,
//            int syer, int smnth, int sdy,
//            int eyer, int emnth, int edy,
//            string print_hourly, 
//            string flags,
//            string format = "2")
//        {

//        // pn-bin/instant?station=ABEI&year=2018&month=1&day=1&year=2018&month=1&day=30&pcode=SI&format=1
//            var db = Database.GetTimeSeriesDatabase();
//            var w = new WebTimeSeriesWriter(db,
//                 interval,
//             custom_list,
//             list,
//             start,  end,  back,
//             parameter,
//             syer,  smnth,  sdy,
//             eyer,  emnth,  edy,
//             print_hourly,
//             flags,
//             format);

//            var x = w.Run(Response);
//            return Content(x, Response.ContentType);
//        }

//        /// <summary>
//        /// Retrieve daily TS data
//        /// </summary>
//        [HttpPost()]
//        [ProducesResponseType(typeof(string), 200)]
//        [ProducesResponseType(typeof(string), 400)]
//        [ProducesResponseType(typeof(void), 500)]
//        public ContentResult Post()
//        {
//            var sr = new System.IO.StreamReader(Request.Body);
//            var body = sr.ReadToEnd();
//            sr.Close();

//            if (body == "")
//                throw new Exception("no data posted");
//            var db = Database.GetTimeSeriesDatabase();
//            var w = new WebTimeSeriesWriter(db, Reclamation.TimeSeries.TimeInterval.Daily, body);
//            var x =w.Run(Response);
//            return Content(x, Response.ContentType);
//        }

//    }
//}
