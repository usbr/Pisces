using Reclamation.Core;
using Reclamation.TimeSeries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace PiscesWebServices
{
    /// <summary>
    /// Manage Connection to Pisces Database for webservices
    /// </summary>
    static class Database
    {
        private static TimeSeriesDatabase s_db;
         public static void InitDB(string[] args)
        {
            s_db = TimeSeriesDatabase.InitDatabase(new Arguments(args));
        }

        public static TimeSeriesDatabase DB()
        {
            return s_db;
        }

        public static DataTable Sites
        {
            get
            {
                return s_db.Server.Table("sitecatalog", 
                    "select siteid, description,state,latitude,longitude, type from sitecatalog");
            }
        }

        internal static DataTable GetParameters(string siteid)
        {
            // list of instant, daily, etc..
            //select  s.siteid, s.parameter, description, units, timeinterval from seriescatalog s join ref_parameter p on s.parameter=p.parameter where siteid = 'Billy_Chinook'
            string sql = "select  s.siteid, s.parameter, name,description, units, timeinterval from seriescatalog s left join ref_parameter p on s.parameter=p.parameter where siteid = '" + siteid + "' order by timeinterval";
            return s_db.Server.Table("a", sql);

 
        }
    }
}
