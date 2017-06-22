using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Reclamation.TimeSeries;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using Reclamation.Core;
using System.Net;
using System.Text;

namespace PiscesWebServices.CGI
{
    /// <summary>
    ///  returns results from web query to timeseries data in pisces.

    /// "https://localhost/pn-bin/instant.pl?list=boii ob,boii obx&start=2016-04-15&end=2016-04-20"
    /// "https://localhost/pn-bin/instant.pl?list=bewo ob,bewo pc&start=2016-04-15&end=2016-04-20&format=zrxp"
    /// "https://lrgs1/pn-bin/daily?list=jck fb, amf fb&start=2016-04-15&end=2016-04-20"
    /// "https://localhost/pn-bin/daily.pl?site=luc&start=2016-04-01&end=2016-04-20"
    ///  https://localhost/pn-bin/daily.pl?parameter=CRSM%20ET,COVM%20ET,RDBM%20ET,DRLM%20ET,SIGM%20ET,CRSM%20SR,COVM%20SR,RDBM%20SR,DRLM%20SR,SIGM%20SR,CRSM%20WR,COVM%20WR,RDBM%20WR,DRLM%20WR,SIGM%20WR,CRSM%20WG,COVM%20WG,RDBM%20WG,DRLM%20WG,SIGM%20WG,CRSM%20MN,COVM%20MN,RDBM%20MN,DRLM%20MN&syer=2017&format=html&header=false
    /// options :  
    ///      back=12  (12 hours for instant, 12 days for daily)
    ///      print_hourly=true (print hourly data)
    /// 
    /// Legacy Test Samples
    /// https://localhost/pn-bin/instant.pl?station=ABEI&year=2016&month=1&day=1&year=2016&month=1&day=1&pcode=OB&pcode=OBX&pcode=OBM&pcode=TU&print_hourly=1
    /// https://localhost/pn-bin/instant.pl?station=BOII&year=2016&month=1&day=1&year=2016&month=1&day=1&pcode=OB&pcode=OBX&pcode=OBN&pcode=TU
    /// https://localhost/pn-bin/instant.pl?station=ABEI&year=2016&month=1&day=1&year=2016&month=1&day=1&pcode=OB&pcode=OBX&pcode=OBM&pcode=OBN&pcode=TUX&print_hourly=true
    /// </summary>
    public partial class WebTimeSeriesWriter
    {
        TimeSeriesDatabase db;
        DateTime start = DateTime.Now.AddDays(-1).Date;
        DateTime end = DateTime.Now.Date;
        Formatter m_formatter;
        string m_query = "";
        NameValueCollection m_collection;


        string[] supportedFormats = new string[] {"csv", // csv with headers
                                                "html", // basic html
                                                "zrxp", // wiski zxrp (kisters)
                                                "1", // legacy tab separated.
                                                "2" // legacy csv
                                                };




        public WebTimeSeriesWriter(TimeSeriesDatabase db, TimeInterval interval, string query = "")
        {
            this.db = db;
            m_query = query;
            InitFormatter(interval);
        }

        private void InitFormatter(TimeInterval interval)
        {
            if (m_query == "")
            {
                m_query = HydrometWebUtility.GetQuery();
            }
         
            Logger.WriteLine("Raw query: = '" + m_query + "'");

            if (m_query == "")
            {
                StopWithError("Error: Invalid query");
            }

            m_query = LegacyTranslation(m_query, interval);

            if (!ValidQuery(m_query))
            {
                StopWithError("Error: Invalid query");
            }

            m_collection = HttpUtility.ParseQueryString(m_query);
            if (!HydrometWebUtility.GetDateRange(m_collection, interval, out start, out end))
            {
                StopWithError("Error: Invalid dates");
            }


            string format = "2";
            if (m_collection.AllKeys.Contains("format"))
                format = m_collection["format"];

            // because of history daily defaults flags= false;
            // no flags (the old daily database did not have flags )
            bool m_printFlags = interval == TimeInterval.Hourly || interval == TimeInterval.Irregular;

            if (m_collection.AllKeys.Contains("flags"))
            {
                m_printFlags = m_collection["flags"] == "true";
            }

            bool printHeader = true;
            if (m_collection.AllKeys.Contains("header"))
            {
                printHeader = m_collection["header"] == "true";
            }


            if (Array.IndexOf(supportedFormats, format) < 0)
                StopWithError("Error: invalid format " + format);

            if (format == "csv")
                m_formatter = new CsvFormatter(interval, m_printFlags);
                else if( format == "zrxp")
            {
                m_formatter = new WiskiFormatter(interval);
            }
            else if (format == "2")
            {
                m_formatter = new LegacyCsvFormatter(interval, m_printFlags);
            }
            else if (format == "1")
            {
                m_formatter = new LegacyCsvFormatter(interval, m_printFlags, "\t");
            }
            else if (format == "html")
            {
                bool printDescription = false;
                if (m_collection.AllKeys.Contains("description"))
                {
                    printDescription = m_collection["description"] == "true";
                }
                m_formatter = new HtmlFormatter(interval, m_printFlags, printHeader,printDescription);
            }

            else
                m_formatter = new LegacyCsvFormatter(interval, m_printFlags);

            if (m_collection.AllKeys.Contains("print_hourly"))
                m_formatter.HourlyOnly = m_collection["print_hourly"] == "true";


        }
        private void StopWithError(string msg)
        {
            Logger.WriteLine(msg);
            HydrometWebUtility.PrintHydrometTrailer(msg);
            throw new Exception(msg);
        }

        public void Run(string outputFile = "")
        {
            StreamWriter sw = null;
            if (outputFile != "")
            {
                sw = new StreamWriter(outputFile);
                Console.SetOut(sw);
            }
            Console.Write("Content-type: text/html\n\n");

            try
            {
                SeriesList list = CreateSeriesList();

                if (list.Count == 0)
                {
                    StopWithError("Error: list of series is empty");
                }

                WriteSeries(list);
            }
            finally
            {
                if (sw != null)
                    sw.Close();

            }
            //catch (Exception e)
            //{
            //    Logger.WriteLine(e.Message);
            //  Console.WriteLine("Error: Data");	
            //}

            StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
        }


        private static bool ValidQuery(string query)
        {
            if (query == "")
                return false;
            if (query.Length > 9000)
                return false;

            return Regex.IsMatch(query, "[^A-Za-z0-9=&%+-]");
        }


        /// <summary>
        /// Gets the queried series and generates simple text output
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private void WriteSeries(SeriesList list)
        {

            m_formatter.WriteSeriesHeader(list);

            int daysStored = 30;

            if (m_formatter.Interval == TimeInterval.Daily)
                daysStored = 3650; // 10 years

            if (m_formatter.Interval == TimeInterval.Monthly)
                daysStored = 36500;

            TimeRange timeRange = new TimeRange(start, end);

            foreach (TimeRange item in timeRange.Split(daysStored))
            {
                var interval = m_formatter.Interval;
                var tbl = Read(list, item.StartDate, item.EndDate, interval,m_formatter.OrderByDate);
                m_formatter.PrintDataTable(list, tbl);
            }
            m_formatter.WriteSeriesTrailer();

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
        private DataTable Read(SeriesList list, DateTime t1, DateTime t2,
        TimeInterval interval, bool orderByDate=true)
        {
            var sql = CreateSQL(list, t1, t2, interval,orderByDate);
            if (sql == "")
                return new DataTable();

            var tbl = db.Server.Table("tbl", sql);
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
        private string CreateSQL(SeriesList list, DateTime t1, DateTime t2, TimeInterval interval, bool orderByDate=true)
        {
            if (list.Count == 0)
                return "";

            string tableName = list[0].Table.TableName;
            Logger.WriteLine("CreateSQL");
            Logger.WriteLine("list of " + list.Count + " series");
            int startIndex = 0;
            var sql = "";

            if (interval == TimeInterval.Daily)
            {
                startIndex = 1; // take care of first table with join to enumerate all dates in range
                sql = JoinFirstTableWithDatesBetween(t1, t2, tableName);
                if (list.Count > 1)
                    sql += "\n UNION ALL \n";
            }

            sql += BuildUnionSQL(list, t1, t2, startIndex);
            if( orderByDate)
                sql += " \norder by datetime,tablename ";
            else
                sql += " \norder by tablename,datetime ";

            return sql;
        }

        private string JoinFirstTableWithDatesBetween(DateTime t1, DateTime t2, string tableName)
        {
            string st1 = t1.ToString("yyyy-MM-dd");
            string st2 = t2.ToString("yyyy-MM-dd") + " 23:59:59.996";
            string sql = "";

            if (db.Server.TableExists(tableName))
            {
                sql = "SELECT   '" + tableName + "' as tablename,a.datetime, value,flag "
                  + " FROM  ( Select datetime from generate_series"
                  + "( '" + st1 + "'::timestamp , '" + st2 + "'::timestamp , '1 day'::interval) datetime ) a ";
                sql += @" left join " + tableName + "  b on a.datetime = b.datetime "
                    + " WHERE  a.datetime >= '" + st1
                    + "' AND    a.datetime <= '" + st2 + "'";
            }
            else
            {
                sql = "SELECT   '" + tableName + "' as tablename, datetime, null as value  , '' as flag "
                  + " FROM  ( Select datetime from generate_series"
                  + "( '" + st1 + "'::timestamp , '" + st2 + "'::timestamp , '1 day'::interval) datetime ) a ";

            }
            return sql;
        }

        private string BuildUnionSQL(SeriesList list, DateTime t1, DateTime t2, int startIndex)
        {
            string sql = "";
            for (int i = startIndex; i < list.Count; i++)
            {
                var tableName = list[i].Table.TableName;

                if (!db.Server.TableExists(tableName))
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
                rval = " \nWHERE datetime >= " + db.Server.PortableDateString(t1, TimeSeriesDatabase.dateTimeFormat)
                    + " \nAND "
                    + " \ndatetime <= " + db.Server.PortableDateString(t2, TimeSeriesDatabase.dateTimeFormat);
            }
            return rval;
        }

        private SeriesList CreateSeriesList()
        {
            var interval = m_formatter.Interval;
            TimeSeriesName[] names = GetTimeSeriesName(m_collection, interval,db);

            var tableNames = (from n in names select n.GetTableName()).ToArray();

            var sc = db.GetSeriesCatalog("tablename in ('" + String.Join("','", tableNames) + "')");

            SeriesList sList = new SeriesList();
            foreach (var tn in names)
            {
                Series s = new Series();

                s.TimeInterval = interval;
                Logger.WriteLine("tablename: "+tn.GetTableName());
                if (sc.Select("tablename = '" + tn.GetTableName() + "'").Length == 1)
                {
                    s = db.GetSeriesFromTableName(tn.GetTableName());
                }
                s.Table.TableName = tn.GetTableName();
                sList.Add(s);
            }
            return sList;
        }


        private static TimeSeriesName[] GetTimeSeriesName(NameValueCollection query, TimeInterval interval,TimeSeriesDatabase db)
        {
            List<TimeSeriesName> rval = new List<TimeSeriesName>();

            

            var sites = HydrometWebUtility.GetParameter(query, "list");

            Logger.WriteLine("GetTimeSeriesName()");
            Logger.WriteLine(query.ToString());

            var siteCodePairs = sites.Split(',');

            foreach (var item in siteCodePairs)
            {
                var tokens = item.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 2)
                {
                    TimeSeriesName tn = new TimeSeriesName(tokens[0] + "_" + tokens[1], interval);
                    rval.Add(tn);
                }
                else if( tokens.Length ==1 )
                {//just the site return a list of all parameters
                    var parms = db.GetParameters(tokens[0].Trim(), interval,false);
                    for (int i = 0; i < parms.Length; i++)
                    {
                        TimeSeriesName tn = new TimeSeriesName(tokens[0] + "_" + parms[i], interval);
                        rval.Add(tn);
                    }

                }
            }
           
            return rval.ToArray();
        }



    }
}