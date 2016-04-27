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

namespace PiscesWebServices
{
    /// <summary>
    ///  returns results from web query to timeseries data in pisces.
    /// "http://www.usbr.gov/pn-bin/instant.pl?list=boii ob,boii obx&start=2016-04-15&end=2016-04-20"
    /// 
    /// options :  
    ///      back=12  (12 hours for instant, 12 days for daily)
    ///      print_hourly=true (print hourly data)
    /// 
    /// Legacy Test Samples
    /// http://www.usbr.gov/pn-bin/instant.pl?station=ABEI&year=2016&month=1&day=1&year=2016&month=1&day=1&pcode=OB&pcode=OBX&pcode=OBM&pcode=TU&print_hourly=1
    /// http://www.usbr.gov/pn-bin/instant.pl?station=BOII&year=2016&month=1&day=1&year=2016&month=1&day=1&pcode=OB&pcode=OBX&pcode=OBN&pcode=TU

    /// </summary>
    public class CsvTimeSeriesWriter
    {
        TimeSeriesDatabase db;
        DateTime start = DateTime.Now.AddDays(-1).Date;
        DateTime end  = DateTime.Now.Date;
        string format = "csv"; // csv, tab, html 
        bool print_hourly = false;
        TimeInterval interval = TimeInterval.Irregular;

        public CsvTimeSeriesWriter(TimeSeriesDatabase db)
        {
            this.db = db;
        }

        public void Run(TimeInterval interval, string query = "", string outputFile="")
        {
           
            StreamWriter sw = null;
            if (outputFile != "")
            {
                sw = new StreamWriter(outputFile);
                Console.SetOut(sw);
            }
             Console.Write("Content-type: text/html\n\n");
             HydrometWebUtility.PrintHydrometHeader();
             
           try 
             {

                 if (query == "")
                 {  //get query from web request.
                     query = HydrometWebUtility.GetQuery();
                 }
                 Logger.WriteLine("Raw query: = '" + query + "'");

                 //query = HttpUtility.UrlDecode(query);
                 //Logger.WriteLine("decodeed: = '"+query+"'");

                 if (query == "")
                 {
                     HydrometWebUtility.PrintHydrometTrailer("Error: Invalid query");
                     return;
                 }

                 query = LegacyTranslation(query);

                 if (!ValidQuery(query))
                 {
                     HydrometWebUtility.PrintHydrometTrailer("Error: Invalid query");
                     return;
                 }

                 var queryCollection = HttpUtility.ParseQueryString(query);
                 if (!HydrometWebUtility.GetDateRange(queryCollection, interval, out start,out end))
                 {
                     Console.WriteLine("Error: Invalid dates");
                     return;
                 }


                 if (queryCollection.AllKeys.Contains("print_hourly"))
                     print_hourly = queryCollection["print_hourly"] == "true";

                 SeriesList list = CreateSeriesList(queryCollection);

                 if (list.Count == 0)
                 {
                     Logger.WriteLine("Error: list of series is empty");
                     HydrometWebUtility.PrintHydrometTrailer("Error: list of series is empty");
                     return;
                 }

                 WriteCsv(list);

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

        private static string LegacyTranslation(string query)
        {

            var rval = query;

            //http://www.usbr.gov/pn-bin/webarccsv.pl?station=cedc&pcode=mx&pcode=mn&back=10&format=2
            //station=boii&year=2016&month=4&day=21&year=2016&month=4&day=21&pcode=OB&pcode=OBX&pcode=OBN&pcode=TU

            if (query.IndexOf("station=") >= 0 && query.IndexOf("pcode") >= 0)
            {
                rval = LegacyStationQuery(query);
            }
            else if( query.IndexOf("parameter") >=0 )
            {
                rval = rval.Replace("parameter", "list");
            }

            Logger.WriteLine(rval);
            return rval;

        }

        private static string LegacyStationQuery(string query)
        {
            string rval = "";
            var c = HttpUtility.ParseQueryString(query);

            var pcodes = c["pcode"].Split(',');
            var cbtt = c["station"];
            var back = "";
            var start = "";
            var end = "";
            var keys = c.AllKeys;
            if (keys.Contains("back"))
            {
                back = c["back"];
            }
            else if (keys.Contains("year") && keys.Contains("month") && keys.Contains("day"))
            {

                var years = c["year"].Split(',');
                var months = c["month"].Split(',');
                var days = c["day"].Split(',');

                start = years[0] + "-" + months[0] + "-" + days[0];
                end = years[1] + "-" + months[1] + "-" + days[1];
            }
            rval = "list=";
            //rval = rval.Replace("station=" + cbtt + "", "list=");
            for (int i = 0; i < pcodes.Length; i++)
            {
                var pc = pcodes[i];
                rval += cbtt + " " + pc;
                if (i != pcodes.Length - 1)
                    rval += ",";
            }
            rval += "&start=" + start + "&end=" + end;
            if( c.AllKeys.Contains("print_hourly") )
                rval += "&print_hourly=true";

            return rval.ToLower();
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
        private void WriteCsv(SeriesList list)
        {
            Console.WriteLine("BEGIN DATA");
            WriteSeriesHeader(list);

            int maxDaysInMemory = 30;

            // maxDaysIhn memory
            //   maxdays      list.Read()    REad()
            //   10
            //   
            var t2 = end.EndOfDay();
            var t = start;
            Performance p = new Performance();
            while (t<t2)
            {
                var t3 = t.AddDays(maxDaysInMemory).EndOfDay();  

                if (t3 > t2) 
                    t3 = t2;

                var tbl = Read(list, t, t3); // 0.0 seconds windows/linux
                PrintDataTable( list,tbl,print_hourly);
                t = t3.NextDay();
            } 

            Console.WriteLine("END DATA");
           // p.Report("done");
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
                    continue;
                }

                sql += "SELECT '" + tableName + "' as tablename, datetime,value,flag FROM " + tableName;
                if (t1 != TimeSeriesDatabase.MinDateTime || t2 != TimeSeriesDatabase.MaxDateTime)
                {
                    sql += " WHERE datetime >= " + db.Server.PortableDateString(t1, TimeSeriesDatabase.dateTimeFormat)
                        + " AND "
                        + " datetime <= " + db.Server.PortableDateString(t2, TimeSeriesDatabase.dateTimeFormat);
                }

                if (i != list.Count - 1)
                    sql += " UNION ALL \n";
            }

            sql += " \norder by datetime,tablename ";

            return sql;
        }

        private void WriteSeriesHeader(SeriesList list)
        {
            //string headLine = "DATE, ";
            var headLine = "DATE       TIME ";
            foreach (var item in list)
            {
                TimeSeriesName tn = new TimeSeriesName(item.Table.TableName);
                headLine += ",  "+tn.siteid.PadRight(8) + "" + tn.pcode.PadRight(8) ;
            }
            headLine = headLine.ToUpper();
            Console.WriteLine(headLine);
        }

        private SeriesList CreateSeriesList(NameValueCollection query)
        {
            TimeSeriesName[] names = GetTimeSeriesName(query);

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
        private static void PrintDataTable(SeriesList list, DataTable table, bool printHourly)
        {
            var t0 = "";

            if (table.Rows.Count > 0)
                t0 = FormatDate(table.Rows[0][1]);

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
               
                t = FormatDate(row[1]);

               if( t!= t0)
                {
                   if (printThisRow)
                    PrintRow(t0,vals,flags);
                    vals = new string[list.Count];
                    flags = new string[list.Count];
                    t0 = t;
                }

                vals[dict[row[0].ToString()]] =  FormatNumber(row[2]);
                flags[dict[row[0].ToString()]] = FormatFlag(row[3]);

                DateTime date = Convert.ToDateTime(row[1]);
                bool topOfHour = date.Minute == 0;
                printThisRow = printHourly == false || (printHourly && topOfHour);

            }
            if (printThisRow)
            PrintRow(t, vals, flags);
        }

        private static void PrintRow(string t0, string[] vals, string[] flags)
        {
            var  s = t0+ ",";
            for (int i = 0; i < vals.Length; i++)
            {
                s += vals[i] + flags[i];
                if (i != vals.Length - 1)
                    s += ",";
            }
            Console.WriteLine(s);

        }

        /// <summary>
        /// format like this: 04/01/2015 18:00
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static string FormatDate( object o)
        {
            var rval = "";
            var t = Convert.ToDateTime(o);
            rval = t.ToString("MM/dd/yyyy HH:mm");
            return rval;
        }

        private static string FormatFlag( object o)
        {
            if (o == DBNull.Value)
                return "";
            else
                return o.ToString();

        }

        private static string FormatNumber(object o)
        {
            var rval = "";
            if (o == DBNull.Value || o.ToString() == "")
                rval = "";//.PadLeft(11);
            else
                rval = Convert.ToDouble(o).ToString("F02").PadLeft(11) ;
            return rval;
        }

        private static TimeSeriesName[] GetTimeSeriesName(NameValueCollection query)
        {
            List<TimeSeriesName> rval = new List<TimeSeriesName>();

            var sites = HydrometWebUtility.GetParameter(query,"list");

            Logger.WriteLine("GetTimeSeriesName()");
            Logger.WriteLine(query.ToString());
            
            var siteCodePairs = sites.Split(',');

            foreach (var item in siteCodePairs)
            {
                var tokens = item.Split(' ');
                if (tokens.Length == 2)
                {
                    TimeSeriesName tn = new TimeSeriesName(tokens[0] + "_" + tokens[1], TimeInterval.Irregular);
                    rval.Add(tn);
                }
            }
            return rval.ToArray();
        }
       


    }
}