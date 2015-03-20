using System;
using System.Data;
using Reclamation.Core;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using Reclamation.TimeSeries.Parser;

namespace Reclamation.TimeSeries
{
    public partial class TimeSeriesDatabase
    {

        public static TimeSeriesDatabase InitDatabase(Arguments args)
        {

            if (args.Contains("database"))
            {
                if (File.Exists(args["database"]))
                {
                    SQLiteServer svr = new SQLiteServer(args["database"]);
                    var db = new TimeSeriesDatabase(svr, LookupOption.TableName);
                    return db;
                }

                int idx = args["database"].IndexOf(":");
                if (idx > 0)
                {// postgresql

                }
                throw new NotImplementedException("Please use app.config file");
            }
            else if (ConfigurationManager.AppSettings["PostgresDatabase"] != null)
            {// use config file (postgresql is default)
                Logger.WriteLine("using postgresql");
                var dbname = ConfigurationManager.AppSettings["PostgresDatabase"];
                var svr = PostgreSQL.GetPostgresServer(dbname);
                Console.WriteLine(svr.ConnectionString);
                var db = new TimeSeriesDatabase(svr, LookupOption.TableName);
                db.Parser.RecursiveCalculations = false;
                Logger.WriteLine("database initilized..");
                return db;
            }
            else if (ConfigurationManager.AppSettings["MySqlDatabase"] != null)
            {// use config file (postgresql is default)
                Logger.WriteLine("using mysql");
                var dbname = ConfigurationManager.AppSettings["MySqlDatabase"];
                var server = ConfigurationManager.AppSettings["MySqlServer"];
                var user = ConfigurationManager.AppSettings["MySqlUser"];
                var svr = MySqlServer.GetMySqlServer(server, dbname, user);
                //Console.WriteLine(svr.ConnectionString);
                var db = new TimeSeriesDatabase(svr, LookupOption.TableName);
                db.Parser.RecursiveCalculations = false;
                Logger.WriteLine("database initilized..");
                return db;
            }

            throw new NotImplementedException("Please use app.config file to configure database");
        }

    }
}