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

namespace PiscesWebServices
{
    public class CsvTimeSeriesWriter
    {
        TimeSeriesDatabase db;
        public CsvTimeSeriesWriter(TimeSeriesDatabase db)
        {
            this.db = db;
        }

        //http://pnhyd0.pn.usbr.gov/~dataaccess/webarccsv.com?parameter=BOII%20PC,ODSW%20wr,&syer=2012&smnth=1&sdy=1&eyer=2012&emnth=1&edy=10&format=3
        //string srchStr = "http://pnhyd0.pn.usbr.gov/~dataaccess/webarccsv.com?parameter=BOII PC,ODSW wr,&syer=2012&smnth=1&sdy=1&eyer=2012&emnth=12&edy=30&format=3";

        public void Run(TimeInterval interval, string query = "", string outputFile="")
        {
            StreamWriter sw = null;
            if (outputFile != "")
            {
                sw = new StreamWriter(outputFile);
                Console.SetOut(sw);
            }
             Console.Write("Content-type: text/html\n\n"
                
                );

             WebUtility.PrintHydrometHeader();
             
          // try 
	        {
               
                if (query == "")
                    query = WebUtility.GetQuery();
                else
                {
                  //  query = WebUtility.SanitizeQuery(query);
                   // query = System.Web.HttpUtility.UrlEncode(query);
                }
                query = HttpUtility.HtmlDecode(query);

                if (!ValidQuery(query))
                {
                    WebUtility.PrintHydrometTrailer("Error: Invalid query");
                    return;
                }

            var queryCollection =  HttpUtility.ParseQueryString(query);
            DateTime t1;
            DateTime t2;
            if (!WebUtility.GetDateRange(queryCollection, interval,out t1, out t2))
            {
                Console.WriteLine("Error: Invalid dates");
                return;
            }


            SeriesList list = CreateSeriesList(queryCollection, TimeInterval.Irregular);

            WriteCsv(list, TimeInterval.Irregular,t1,t2);

           }
        //catch (Exception e)
        //{
        //    Logger.WriteLine(e.Message);
        //  Console.WriteLine("Error: Data");	
        //}
            WebUtility.PrintHydrometTrailer();

            if (sw != null)
                sw.Close();

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
        private void WriteCsv(SeriesList list, TimeInterval interval, DateTime t1, DateTime t2)
        {
            Console.WriteLine("BEGIN DATA");
            WriteSeriesHeader(list, interval);

            int maxDaysInMemory = 30;

            // maxDaysIhn memory
            //   maxdays      list.Read()    REad()
            //   10
            //   

            var t = t1;
            Performance p = new Performance();
            while (t<t2)
            {
                var t3 = t.AddDays(maxDaysInMemory).EndOfDay();  

                if (t3 > t2) 
                    t3 = t2;

                var tbl = Read(list, t, t3); // 0.0 seconds windows/linux
                PrintDataTable( list,tbl);
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
        private string CreateSQL(SeriesList list, DateTime t1, DateTime t2)
        {
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

        private void WriteSeriesHeader(SeriesList list, TimeInterval interval)
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

        private SeriesList CreateSeriesList(NameValueCollection query, TimeInterval interval)
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
        private static void PrintDataTable(SeriesList list, DataTable table)
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
            for (int i = 0; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];
                t = FormatDate(row[1]);

               if( t!= t0)
                {
                    PrintRow(t0,vals,flags);
                    vals = new string[list.Count];
                    flags = new string[list.Count];
                    t0 = t;
                }

                vals[dict[row[0].ToString()]] =  FormatNumber(row[2]);
                flags[dict[row[0].ToString()]] = FormatFlag(row[3]);
            }
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

            var sites = WebUtility.GetParameter(query,"parameter");

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