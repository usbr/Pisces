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
    /// "http://www.usbr.gov/pn-bin/instant.pl?list=boii ob,boii obx&start=2016-04-15&end=2016-04-20"
    /// "http://lrgs1/pn-bin/daily?list=jck fb, amf fb&start=2016-04-15&end=2016-04-20"
    /// 
    /// options :  
    ///      back=12  (12 hours for instant, 12 days for daily)
    ///      print_hourly=true (print hourly data)
    /// 
    /// Legacy Test Samples
    /// http://www.usbr.gov/pn-bin/instant.pl?station=ABEI&year=2016&month=1&day=1&year=2016&month=1&day=1&pcode=OB&pcode=OBX&pcode=OBM&pcode=TU&print_hourly=1
    /// http://www.usbr.gov/pn-bin/instant.pl?station=BOII&year=2016&month=1&day=1&year=2016&month=1&day=1&pcode=OB&pcode=OBX&pcode=OBN&pcode=TU
    /// http://www.usbr.gov/pn-bin/instant.pl?station=ABEI&year=2016&month=1&day=1&year=2016&month=1&day=1&pcode=OB&pcode=OBX&pcode=OBM&pcode=OBN&pcode=TUX&print_hourly=true
    /// </summary>
    public partial class WebTimeSeriesWriter
    {
        TimeSeriesDatabase db;
        DateTime start = DateTime.Now.AddDays(-1).Date;
        DateTime end  = DateTime.Now.Date;
        Formatter m_formatter ;
        string m_query = "";
        NameValueCollection m_collection;
       

        string[] supportedFormats =new string[] {"csv", // csv with headers
                                                "html", // basic html

                                                "2" // legacy csv
                                                }; 
       

       

        public WebTimeSeriesWriter(TimeSeriesDatabase db, TimeInterval interval, string query="")
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
               StopWithError ("Error: Invalid query");
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
            else if (format == "2")
            {
                m_formatter = new LegacyCsvFormatter(interval, m_printFlags);
            }
            else if( format == "html")
            {
                m_formatter = new HtmlFormatter(interval, m_printFlags, printHeader);
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

        public void Run( string outputFile="")
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
               HydrometWebUtility.PrintHydrometTrailer();

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

            return Regex.IsMatch(query,"[^A-Za-z0-9=&%+-]");
        }


        /// <summary>
        /// Gets the queried series and generates simple text output
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private void WriteSeries(SeriesList list)
        {
            
            m_formatter.WriteSeriesHeader(list);

            int maxDaysInMemory = 30;

            if (m_formatter.Interval == TimeInterval.Daily)
                maxDaysInMemory = 3650; // 10 years

            if (m_formatter.Interval == TimeInterval.Monthly)
                maxDaysInMemory = 36500;

            TimeRange timeRange = new TimeRange(start, end, maxDaysInMemory);

            foreach (TimeRange item in timeRange.List())
            {
                var tbl = Read(list, item.StartDate, item.EndDate); // 0.0 seconds windows/linux
                var interval = m_formatter.Interval;
            
                PrintDataTable(list, tbl, m_formatter, interval);
            }
            m_formatter.WriteSeriesTrailer();
            
        }

        private DataTable Read(SeriesList list, DateTime t1, DateTime t2)
        {
            var sql = CreateSQL(list, t1, t2);
            var tbl = db.Server.Table("tbl", sql);
            return tbl;
        }


        /// <summary>
        /// Create a SQL command that performs UNION of multiple series
        /// so that can be queried in one round-trip to the server.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        private string CreateSQL(SeriesList list, DateTime t1, DateTime t2)
        {
            Logger.WriteLine("CreateSQL");
            Logger.WriteLine("list of " + list.Count + " series");
            var sql = "";
            for (int i = 0; i < list.Count; i++)
            {
                string tableName = list[i].Table.TableName;
                if (!db.Server.TableExists(tableName))
                {
                   // if( m_printFlags)
                      sql += "SELECT '" + tableName + "' as tablename , current_timestamp as datetime, -998877.0 as value, '' as flag where 0=1 ";
                    //else
                      //sql += "SELECT '" + tableName + "' as tablename , current_timestamp as datetime, -998877.0 as value where 0=1 ";
                }
                else
                {
                    //if(m_printFlags)
                       sql += "SELECT '" + tableName + "' as tablename, datetime,value,flag FROM " + tableName;
                    //else
                      //  sql += "SELECT '" + tableName + "' as tablename, datetime,value FROM " + tableName;

                    if (t1 != TimeSeriesDatabase.MinDateTime || t2 != TimeSeriesDatabase.MaxDateTime)
                    {
                        sql += " WHERE datetime >= " + db.Server.PortableDateString(t1, TimeSeriesDatabase.dateTimeFormat)
                            + " AND "
                            + " datetime <= " + db.Server.PortableDateString(t2, TimeSeriesDatabase.dateTimeFormat);
                    }
                }
                if (i != list.Count - 1)
                    sql += " UNION ALL \n";
            }

            sql += " \norder by datetime,tablename ";

            return sql;
        }

        

        private SeriesList CreateSeriesList()
        {
            var interval = m_formatter.Interval;
            TimeSeriesName[] names = GetTimeSeriesName(m_collection, interval);

            var tableNames = (from n in names select n.GetTableName()).ToArray();

            var sc = db.GetSeriesCatalog("tablename in ('" + String.Join("','", tableNames) + "')");

            SeriesList sList = new SeriesList();
            foreach (var tn in names)
            {
                Series s = new Series();

                s.TimeInterval = interval;
                if (sc.Select("tablename = '" + tn.GetTableName() + "'").Length == 1)
                {
                    s = db.GetSeriesFromTableName(tn.GetTableName());
                }
                s.Table.TableName = tn.GetTableName();
                sList.Add(s);
            }
            return sList;
        }

        /// <summary>
        /// Print DataTable composed of tablename,datetime,value[,flag]
        /// with columns for each tablename
        /// </summary>
        /// <param name="list"></param>
        /// <param name="table"></param>
        private static void PrintDataTable(SeriesList list, DataTable table, 
            Formatter fmt, TimeInterval interval)
        {
            var t0 = "";

            if (table.Rows.Count > 0)
                t0 = fmt.FormatDate(table.Rows[0][1]);

            var vals = new string[list.Count];
            var flags = new string[list.Count];
            var dict = new Dictionary<string, int>();
            for (int i = 0; i < list.Count; i++)
            {
                dict.Add(list[i].Table.TableName, i);
            }

            string t="";
            bool printThisRow = false;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];
               
                t = fmt.FormatDate(row[1]);

               if( t!= t0)
                {
                   if (printThisRow)
                    fmt.PrintRow(t0,vals,flags);
                    vals = new string[list.Count];
                    flags = new string[list.Count];
                    t0 = t;
                }

                vals[dict[row[0].ToString()]] =  fmt.FormatNumber(row[2]);
                flags[dict[row[0].ToString()]] = fmt.FormatFlag(row[3]);

                DateTime date = Convert.ToDateTime(row[1]);
                bool topOfHour = date.Minute == 0;
                printThisRow = fmt.HourlyOnly == false || (fmt.HourlyOnly && topOfHour);

            }
            if (printThisRow)
                fmt.PrintRow(t, vals, flags);
        }

       


        

        private static TimeSeriesName[] GetTimeSeriesName(NameValueCollection query, TimeInterval interval)
        {
            List<TimeSeriesName> rval = new List<TimeSeriesName>();

            var sites = HydrometWebUtility.GetParameter(query,"list");

            Logger.WriteLine("GetTimeSeriesName()");
            Logger.WriteLine(query.ToString());
            
            var siteCodePairs = sites.Split(',');

            foreach (var item in siteCodePairs)
            {
                var tokens = item.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 2)
                {
                    TimeSeriesName tn = new TimeSeriesName(tokens[0] + "_" + tokens[1] , interval);
                    rval.Add(tn);
                }
            }
            return rval.ToArray();
        }
       


    }
}