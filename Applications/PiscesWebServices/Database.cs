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
            s_db = TimeSeriesDatabase.InitDatabase(new Arguments(args),true);
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

        internal static DataTable GetView(string viewName)
        {
            string sql = "select * from " + viewName;
            return s_db.Server.Table("a", sql);
        }

        internal static DataTable GetSiteByType(string typeName)
        {
            string sql = "select type, siteid, description,state,latitude,longitude from sitecatalog " +
                "where type = '" + typeName + "'";
            return s_db.Server.Table("a", sql);
        }

        internal static DataTable GetSiteByRegion(string regionName)
        {
            string sql = "select agency_region, siteid, description,state,latitude,longitude from sitecatalog " +
                "where agency_region = '" + regionName + "'";
            return s_db.Server.Table("a", sql);
        }

        internal static DataTable GetTableProperties()
        {
            string sql = "select seriescatalog.tablename, sitecatalog.description, seriescatalog.name, seriescatalog.units " +
                "from seriescatalog inner join sitecatalog on seriescatalog.siteid = sitecatalog.siteid order by sitecatalog.description";
            return s_db.Server.Table("a", sql);
        }

        internal static DataTable GetParameters(string siteid)
        {
            // list of instant, daily, etc..
            //select  s.siteid, s.parameter, description, units, timeinterval from seriescatalog s join ref_parameter p on s.parameter=p.parameter where siteid = 'Billy_Chinook'
            string sql = "select  s.tablename, s.parameter, s.name, s.units, s.timeinterval, s.statistic, s.server, s.t1 as start, s.t2 as end, count from view_seriescatalog s left join parametercatalog p on s.parameter=p.name where s.siteid = '" + siteid + "' order by timeinterval";
            return s_db.Server.Table("a", sql);
        }

        internal static DataTable GetSiteProperties(dynamic siteid)
        {
            var sql = "select siteid,name, value from siteproperties where siteid = '"+siteid+"'";
            return s_db.Server.Table("a", sql);
        }

        internal static DataTable GetSeries()
        {
            string sql = "select  tablename, s.parameter, s.name, s.units, s.timeinterval, s.statistic, s.server, s.t1 as start, s.t2 as end, count from view_seriescatalog s left join parametercatalog p on s.parameter=p.name  where isfolder = 0 order by s.name ";
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
