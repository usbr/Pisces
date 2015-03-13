using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Reclamation.TimeSeries;
using System.Linq;
using System.Web;

namespace PiscesWebServices
{
    public class CsvWriter
    {
        TimeSeriesDatabase db;
        public CsvWriter(TimeSeriesDatabase db)
        {
            this.db = db;
        }

        public void Run(string query = "")
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
                   // query = System.Web.HttpUtility.UrlEncode(query);
                }
                query = HttpUtility.HtmlDecode(query);

                if (!ValidQuery(query))
                {
                    Console.WriteLine("Error: Invalid query");
                    Console.Write("</pre>");
                    return;
                }

            var queryCollection =  HttpUtility.ParseQueryString(query);

            foreach (String s in queryCollection.AllKeys)
            {
                Console.WriteLine(s + " - " + queryCollection[s]);
            }

            WriteCsv(queryCollection, TimeInterval.Irregular);

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

            return Regex.IsMatch(query,"[^A-Za-z0-9=&%+-]");
        }


        /// <summary>
        /// Gets the queried series and generates simple text output
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private void WriteCsv(NameValueCollection query, TimeInterval interval)
        {
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
            bool hasFlags = true;

            int maxDaysInMemory = 1;
            var t = t1;
           
            while(t<t2)
            {
                var t3 = t.AddDays(maxDaysInMemory).EndOfDay();  

                if (t3 > t2) 
                    t3 = t2;
                sList.Read(t, t3);
                Console.WriteLine("block: "+t.ToString()+" " + t3.ToString());
                var sTable = sList.ToDataTable(!hasFlags);
                PrintDataTable(hasFlags, sTable);

                t = t3.NextDay();
            } 

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
            if (o == DBNull.Value || o.ToString() == "")
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