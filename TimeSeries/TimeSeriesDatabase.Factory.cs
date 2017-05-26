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

        public static TimeSeriesDatabase InitDatabase(Arguments args, bool readOnly=false)
        {

            if (args.Contains("sqlite"))
            {
                SQLiteServer svr = new SQLiteServer(args["sqlite"]);
                Logger.WriteLine("Using SQLite " + args["sqlite"]);
                Console.WriteLine(args["sqlite"]);
                var db = new TimeSeriesDatabase(svr, LookupOption.TableName,readOnly);
                return db;
            }
            else if (ConfigurationManager.AppSettings["PostgresDatabase"] != null)
            {// use config file (postgresql is default)
                Logger.WriteLine("using postgresql");
                var dbname = ConfigurationManager.AppSettings["PostgresDatabase"];
                var user = ConfigurationManager.AppSettings["PostgresUser"];
                if( user == null)
                    user = "";
                var svr = PostgreSQL.GetPostgresServer(dbname,userName:user);
                var db = new TimeSeriesDatabase(svr, LookupOption.TableName,readOnly);
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
                string pass = "";
                if( ConfigurationManager.AppSettings["MySqlPassword"] != null)
                   pass = ConfigurationManager.AppSettings["MySqlPassword"];
                var svr = MySqlServer.GetMySqlServer(server, dbname, user,pass);
                var db = new TimeSeriesDatabase(svr, LookupOption.TableName,readOnly);
                db.Parser.RecursiveCalculations = false;
                Logger.WriteLine("database initilized..");
                return db;
            }

            throw new NotImplementedException("Please use app.config file to configure database");
        }

    }
}