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
            var conx = Startup.ApiConnectionString;
            if (Startup.PiscesAPIDatabase == "mysql")
            {
                db = new MySql.Data.MySqlClient.MySqlConnection(conx);
            }
            else if (Startup.PiscesAPIDatabase == "postgresql")
            {
                db = new Npgsql.NpgsqlConnection(conx);
            }
            return db;
        }
    }
}
