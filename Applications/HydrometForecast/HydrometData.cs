using Reclamation.Core;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HydrometForecast
{
    /// <summary>
    /// Manages connection to Hydromet Data
    /// Data can be either from Hydromet server or local sqlite database file
    /// </summary>
    public class HydrometData
    {

        public static string FileName = ""; // when empty use hydromet server
        static HydrometData()
        {
            HydrometMonthlySeries.ConvertToAcreFeet = false;
        }

        private static TimeSeriesDatabase DB()
        {
            var cs = "Data Source=" + FileName + ";Read Only=True;";
            SQLiteServer svr = new SQLiteServer(FileName);
            TimeSeriesDatabase db = new TimeSeriesDatabase(svr);
            return db;
        }

        private static bool UseSQLite()
        {
            return FileName != "";
        }

        public static Point ReadAverageValue(string cbtt, string pcode, DateTime date)
        {
            int y1 = 9998;
            int y2 = 9999;
            var t1 = new DateTime(y1, 10, 1);
            var t2 = new DateTime(y2, 9, 30);

            string avgPcode = HydrometMonthlySeries.LookupAveargePcode(pcode);

            if (avgPcode == "")
                return new Point(date, Point.MissingValueFlag, PointFlag.Missing);

            Series avg = GetSeries(cbtt, avgPcode);
            
            avg.Read(t1, t2);
            Logger.WriteLine("Reading Average value for " + cbtt + "/" + pcode);
            Logger.WriteLine(avg.ToString());

            var day = date.Day;
            DateTime t;
            if (date.Month == 2 && day == 29) // leap year
                t = new DateTime(y2, date.Month, 28);
            else
                t = new DateTime(y2, date.Month, date.Day);

            if (date.Month > 9)
                t = new DateTime(y1, date.Month, date.Day);

            var pt = new Point(date, Point.MissingValueFlag);
            pt.Value = avg[t].Value;
            pt.Flag = PointFlag.Estimated;
            Logger.WriteLine("using estimated data " + cbtt + " " + pcode + " " + pt.DateTime.ToString("yyyy MMM"));
            return pt;
        }

        /// <summary>
        /// Sums runoff between month1 and month2 from 30 year average
        /// Only valid between january and september
        /// </summary>
        /// <param name="cbtt"></param>
        /// <param name="pcode"></param>
        /// <param name="month1"></param>
        /// <param name="month2"></param>
        /// <returns></returns>
        public static double Sum30YearRunoff(string cbtt, string pcode, int month1, int month2)
        {
            if (month1 > 9 || month1 > month2)
                throw new ArgumentOutOfRangeException("30 year average only works between january and september");

            DateTime t1 = new DateTime(8110, month1, 1);
            DateTime t2 = new DateTime(8110, month2, DateTime.DaysInMonth(8110, month2));
            var s = GetSeries(cbtt, pcode); //new HydrometMonthlySeries(cbtt, pcode);
            s.Read(t1, t2);
            return Reclamation.TimeSeries.Math.Sum(s);
        }

        public static Series GetSeries(string cbtt, string pcode)
        {
            var rval = new Series();
            if (UseSQLite())
            {
                string tableName = cbtt + "_" + pcode;

                if (Regex.IsMatch(tableName, "^[0-9]")) // table name starting with number is not allowed
                {
                    tableName = "_" + tableName; // append with underscore
                }

                rval = DB().GetSeriesFromTableName(tableName.ToLower());
            }
            else
            {
                rval = new HydrometMonthlySeries(cbtt, pcode);
            }
            Logger.WriteLine("Reading data for cbtt ='" + cbtt + "' pcode = '" + pcode+"'");
            
            return rval;
        }

        public static VariableResolver GetVariableResolver()
        {
            if (UseSQLite())
            {
                return new VariableResolver(DB(), LookupOption.TableName);
            }
            else{
            return new HydrometVariableResolver();
            }
        }

        public static void SetupHydrometData(int StartYear,
                                        int EndYear,
            string[] cbttPcodeList, DateTime t, bool cacheAverage, bool allYears = false)
        {

            if (UseSQLite())
                return;


            var cache = new HydrometDataCache();
            if (allYears)
            {
                cache.Add(cbttPcodeList,
                                 new DateTime(StartYear - 1, 1, 1),
                                 new DateTime(EndYear, 12, 31));
            }
            else
            {
                Logger.WriteLine("Caching Hydromet Data for a year ");
                cache.Add(cbttPcodeList,
                                     new DateTime(t.Year - 1, 1, 1),
                                     new DateTime(t.Year, 12, 31));
            }
            if (cacheAverage) // get average values (special water year 9999 )
            {
                // TO DO... change pcode to average pcode....
                var avgCodes = new List<string>();
                for (int i = 0; i < cbttPcodeList.Length; i++)
                {
                    string[] tokens = cbttPcodeList[i].Split();
                    var pc = HydrometMonthlySeries.LookupAveargePcode(tokens[1]);
                    if( pc.Trim() != "")
                       avgCodes.Add(tokens[0] + " " + pc);
                }

                Logger.WriteLine("Caching Hydromet Data for average years");
                cache.Add(avgCodes.ToArray(),
                                   new DateTime(9998, 10, 1),
                                   new DateTime(9999, 9, 30));

            }
            HydrometMonthlySeries.Cache = cache;
            
            // Anderson forecast without cache  8 seconds.
            //                     with  cache  4 seconds
        }
    }
}
