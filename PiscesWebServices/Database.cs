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
            string sql = "select  tablename, s.parameter, name,description, units, timeinterval,server, t1 as start, t2 as end, count from view_seriescatalog s left join ref_parameter p on s.parameter=p.parameter where siteid = '" + siteid + "' order by timeinterval";
            return s_db.Server.Table("a", sql);

 
        }

        internal static DataTable GetSiteProperties(dynamic siteid)
        {
            var sql = "select siteid,name, value from siteproperties where siteid = '"+siteid+"'";
            return s_db.Server.Table("a", sql);
        }

        internal static DataTable GetSeries()
        {
            string sql = "select  tablename, s.parameter, name,description, units, timeinterval,server, t1 as start, t2 as end, count from view_seriescatalog s left join ref_parameter p on s.parameter=p.parameter  where isfolder = 0 order by timeinterval ";
            return s_db.Server.Table("a", sql);
   
        }

        internal static DataTable GetSeriesData(string tablename, DateTime t1, DateTime t2)
        {
            var sql = "";
            if (!s_db.Server.TableExists(tablename))
            {
                DataTable tbl = new DataTable();
                tbl.Columns.Add("DateTime",typeof(DateTime));
                tbl.Columns.Add("value");
                tbl.Columns.Add("flag");
                return tbl;
            }
            else
            {
                sql += "SELECT  datetime,value,flag FROM " + tablename;
                sql += " WHERE datetime >= " + s_db.Server.PortableDateString(t1, TimeSeriesDatabase.dateTimeFormat)
                        + " AND "
                        + " datetime <= " + s_db.Server.PortableDateString(t2, TimeSeriesDatabase.dateTimeFormat);
                }
            return s_db.Server.Table(tablename, sql);
        }  
        
    }
}
