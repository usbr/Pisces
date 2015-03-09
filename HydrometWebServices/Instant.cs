using System;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace PiscesWebServices
{
    public class Instant
    {

        public static void Run(string query = "")
        {
            Console.Write("Content-Type: text/html\n\n");
            Console.Write("<pre>");
          // try 
	        {

                if (query == "")
                    query = WebUtility.GetQuery();
                else
                {
                  //  query = WebUtility.SanitizeQuery(query);
                }

              
            if( !ValidQuery(query))
                Console.WriteLine("Error: Invalid query");

            var queryCollection =  HttpUtility.ParseQueryString(query);

            foreach (String s in queryCollection.AllKeys)
            {
                Console.WriteLine(s + " - " + queryCollection[s]);
            }

            WebArcCSV(queryCollection, TimeInterval.Irregular);

           }
        //catch (Exception e)
        //{
        //    Logger.WriteLine(e.Message);
        //  Console.WriteLine("Error: Data");	
        //}
            Console.Write("</pre>");
        }

        private static bool ValidQuery(string query)
        {
            if (query == "")
                return false;

            return Regex.IsMatch(query,"[^A-Za-z0-9=&%+-/_]");
        }

        private static TimeSeriesDatabase ConnectPiscesServer()
        {
            var svr = PostgreSQL.GetPostgresServer("timeseries");
            var pDB = new TimeSeriesDatabase(svr);
            return pDB;
        }


        /// <summary>
        /// Gets the queried series and generates simple text output
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private static void WebArcCSV(NameValueCollection query, TimeInterval interval)
        {
            // Connect to Pisces Server
            var db = ConnectPiscesServer();
            // USBR  WEBARCCSV Search String
            //http://pnhyd0.pn.usbr.gov/~dataaccess/webarccsv.com?parameter=BOII%20PC,ODSW%20wr,&syer=2012&smnth=1&sdy=1&eyer=2012&emnth=1&edy=10&format=3
            //string srchStr = "http://pnhyd0.pn.usbr.gov/~dataaccess/webarccsv.com?parameter=BOII PC,ODSW wr,&syer=2012&smnth=1&sdy=1&eyer=2012&emnth=12&edy=30&format=3";

            DateTime t1;
            DateTime t2;
            if (!WebUtility.GetDateRange(query, out t1, out t2))
            {
                Console.WriteLine("Error: Invalid dates");
                return ;
            }

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
                    s.Read(t1, t2);
                }
                s.Table.TableName = tn.GetTableName();
                sList.Add(s);
            }

            WebUtility.PrintHeader();
             Console.WriteLine("");
             Console.WriteLine("BEGIN DATA");
            string headLine = "                  DATE, ";
            headLine += String.Join(",", tableNames);
            Console.WriteLine(headLine);
            // Generate body
            bool hasFlags = true;
            var sTable = sList.ToDataTable(!hasFlags);
            PrintDataTable(hasFlags, sTable);

            Console.WriteLine("END DATA");

        }

        private static void PrintDataTable(bool hasFlags, System.Data.DataTable sTable)
        {
            for (int i = 0; i < sTable.Rows.Count; i++)
            {
                string s = "";
                for (int j = 0; j < sTable.Columns.Count; j++)
                {
                    var o = sTable.Rows[i][j];
                    if (j == 0)
                    {
                        s += FormatDate(o);
                    }
                    else
                    {
                        if (!hasFlags || j % 2 == 0)
                        {
                            s += FormatNumber(o);
                        }
                        else
                        {
                            s += FormatFlag(o);
                        }
                    }
                }
                Console.WriteLine(s);
            }
        }

        private static string FormatDate( object o)
        {
            var rval = "";
            var t = Convert.ToDateTime(o);
            rval = t.ToString("G").PadLeft(22) + ", ";
            return rval;
        }

        private static string FormatFlag( object o)
        {
            var rval = "";
            if (o == DBNull.Value)
                rval = "".PadLeft(6) + ", ";
            else
                rval = o.ToString().PadLeft(6) + ", ";
            return rval;
        }

        private static string FormatNumber(object o)
        {
            var rval = "";
            if (o == DBNull.Value)
                rval = "".PadLeft(12) + ", ";
            else
                rval = Convert.ToDouble(o).ToString("F02").PadLeft(12) + ", ";
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