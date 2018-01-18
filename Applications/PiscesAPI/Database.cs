using Reclamation.Core;
using Reclamation.TimeSeries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PiscesAPI
{
    public static class Database
    {
        /// <summary>
        /// Method to connect to the DB
        /// </summary>
        /// <returns></returns>
        public static IDbConnection Connect()
        {
            IDbConnection db = null;
            if (Startup.PiscesAPIDatabase == "mysql")
            {
                db = new MySql.Data.MySqlClient.MySqlConnection(Startup.ApiConnectionString);
            }
            else if (Startup.PiscesAPIDatabase == "postgresql")
            {
                db = new Npgsql.NpgsqlConnection(Startup.ApiConnectionString);
            }

            if (db == null)
            {
                Console.WriteLine("Error... Database is not defined.");
                Console.WriteLine("ApiConnectionString='"+Startup.ApiConnectionString+"'");
                Console.WriteLine("PiscesAPIDatabase='"+ Startup.PiscesAPIDatabase + "'");
            }
            return db;
        }

        public static TimeSeriesDatabase GetTimeSeriesDatabase()
        {
            var svr = new PostgreSQL(Startup.ApiConnectionString);
            var db = new TimeSeriesDatabase(svr);
            return db;
        }
    }
}
