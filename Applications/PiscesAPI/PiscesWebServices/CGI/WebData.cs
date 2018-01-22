using Reclamation.Core;
using Reclamation.TimeSeries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiscesWebServices.CGI
{
    /// <summary>
    /// Data access layer with SQL commands to read multiple series in a single query.
    /// </summary>
    class WebData
    {
        TimeSeriesDatabase m_db;
        public WebData(TimeSeriesDatabase db)
        {
            m_db = db;
        }

        /// <summary>
        /// Create a single datatable by reading from multiple tables
        /// </summary>
        /// <param name="list"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="interval"></param>
        /// <param name="orderByDate"></param>
        /// <returns></returns>
         public DataTable Read(SeriesList list, DateTime t1, DateTime t2,
        TimeInterval interval, bool orderByDate = true)
        {
            var sql = CreateSQL(list, t1, t2, interval, orderByDate);
            if (sql == "")
                return new DataTable();

            var tbl = m_db.Server.Table("tbl", sql);
            return tbl;
        }


        /*
         *   ***************** NOTE: ******************
         *   SELECT   'daily_karl_test' as tablename,a.datetime, value,flag 
         *   FROM  ( Select datetime from generate_series
         *   ( '2016-07-23'::timestamp 
         *   , '2016-08-03 23:59:59.996'::timestamp
         *   , '1 day'::interval) datetime ) a
         *
         *   left join daily_karl_test b on a.datetime = b.datetime
         *
         *   WHERE  a.datetime >= '2016-07-23 00:00:00.000' AND  a.datetime <= '2016-08-03 23:59:59.996' 
         *
         *   UNION ALL 
         *   SELECT 'daily_hrmo_etos' as tablename, datetime,value,flag FROM daily_hrmo_etos 
         *   WHERE datetime >= '2016-07-23 00:00:00.000' AND  datetime <= '2016-08-03 23:59:59.996' 
         *   order by datetime,tablename 
         *
         * */
        /// <summary>
        /// Create a SQL command that performs UNION of multiple series
        /// so that can be queried in one round-trip to the server.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        private string CreateSQL(SeriesList list, DateTime t1, DateTime t2, TimeInterval interval, bool orderByDate = true)
        {
            if (list.Count == 0)
                return "";

            string tableName = list[0].Table.TableName;
            Logger.WriteLine("CreateSQL");
            Logger.WriteLine("list of " + list.Count + " series");
            int startIndex = 0;
            var sql = "";

            if ( ( interval == TimeInterval.Daily || interval == TimeInterval.Monthly)
                
                && m_db.Server is PostgreSQL)
            {
                startIndex = 1; // take care of first table with join to enumerate all dates in range
                var pgInterval = "1 day";
                if (interval == TimeInterval.Monthly)
                    pgInterval = "1 month";
                sql = TableWithMissingDates(t1, t2, tableName,pgInterval);
                if (list.Count > 1)
                    sql += "\n UNION ALL \n";
            }

            sql += BuildUnionSQL(list, t1, t2, startIndex);
            if (orderByDate)
                sql += " \norder by datetime,tablename ";
            else
                sql += " \norder by tablename,datetime ";

            return sql;
        }

        /// <summary>
        /// Returns daily table with dates even when data is missing
        /// table ( tablename, datetime, value, flag)
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private string TableWithMissingDates(DateTime t1, DateTime t2, string tableName,string pgInterval="1day")
        {
            string st1 = t1.ToString("yyyy-MM-dd");
            string st2 = t2.ToString("yyyy-MM-dd") + " 23:59:59.996";
            string sql = "";

            if (m_db.Server.TableExists(tableName))
            {
                sql = "SELECT   '" + tableName + "' as tablename,a.datetime, value,flag "
                  + " FROM  ( Select datetime from generate_series"
                  + "( '" + st1 + "'::timestamp , '" + st2 + "'::timestamp , '"+pgInterval+"'::interval) datetime ) a ";
                sql += @" left join " + tableName + "  b on a.datetime = b.datetime "
                    + " WHERE  a.datetime >= '" + st1
                    + "' AND    a.datetime <= '" + st2 + "'";
            }
            else
            {
                sql = "SELECT   '" + tableName + "' as tablename, datetime, null as value  , '' as flag "
                  + " FROM  ( Select datetime from generate_series"
                  + "( '" + st1 + "'::timestamp , '" + st2 + "'::timestamp , '"+pgInterval+"'::interval) datetime ) a ";

            }
            return sql;
        }

        private string BuildUnionSQL(SeriesList list, DateTime t1, DateTime t2, int startIndex)
        {
            string sql = "";
            for (int i = startIndex; i < list.Count; i++)
            {
                var tableName = list[i].Table.TableName;

                if (!m_db.Server.TableExists(tableName))
                {
                    sql += " \nSELECT '" + tableName + "' as tablename , current_timestamp as datetime, -998877.0 as value, '' as flag where 0=1 ";
                }
                else
                {
                    sql += " \nSELECT '" + tableName + "' as tablename, datetime,value,flag FROM " + tableName;

                    sql += DateWhereClause(t1, t2);
                }
                if (i != list.Count - 1)
                    sql += " UNION ALL \n";
            }
            return sql;
        }

        private string DateWhereClause(DateTime t1, DateTime t2)
        {
            string rval = "";
            if (t1 != TimeSeriesDatabase.MinDateTime || t2 != TimeSeriesDatabase.MaxDateTime)
            {
                rval = " \nWHERE datetime >= " + m_db.Server.PortableDateString(t1, TimeSeriesDatabase.dateTimeFormat)
                    + " \nAND "
                    + " \ndatetime <= " + m_db.Server.PortableDateString(t2, TimeSeriesDatabase.dateTimeFormat);
            }
            return rval;
        }

    }
}
