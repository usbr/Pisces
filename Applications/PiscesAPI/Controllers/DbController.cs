using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PiscesAPI.Models;
using System.Data;

namespace PiscesAPI.Controllers
{
    [Route("[controller]")]
    public class DatabaseConnectionController : Controller
    {
        /// <summary>
        /// Retrieves connected database
        /// </summary>
        /// <remarks>Long description for this API endpoint goes here...</remarks>
        [HttpGet]
        public OkObjectResult Get()
        {
            var conx = Startup.ApiConnectionString;
            var conxItems = conx.Split(';');
            var outList = new List<string>();
            for (int i = 0;i<3;i++)
            {
                outList.Add(conxItems[i]);
            }
            return Ok(outList);
        }

        /// <summary>
        /// Method to connect to the DB
        /// </summary>
        /// <returns></returns>
        public static IDbConnection Connect()
        {
            IDbConnection db = null;
            var conx = Startup.ApiConnectionString;
            if (Startup.PiscesAPIDatabase == "mysql")
            {
                db = new MySql.Data.MySqlClient.MySqlConnection(conx);
            }
            else if( Startup.PiscesAPIDatabase == "postgresql")
            {
                db = new Npgsql.NpgsqlConnection(conx);
            }
            return db;
        }
        
    }
}
