using Reclamation.Core;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries.Parser;
using System;
using Math = Reclamation.TimeSeries.Math;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HydrometForecast
{
    /// <summary>
    /// Manages connection to Hydromet Data
    /// Data can be either from Hydromet server or local sqlite database file
    /// </summary>
    public class HydrometData
    {
        public static HydrometHost s_server = HydrometHost.PNLinux;

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
            //int y1 = 9998;
            //int y2 = 9999;
            //var t1 = new DateTime(y1, 10, 1);
            //var t2 = new DateTime(y2, 9, 30);

            //string avgPcode = HydrometMonthlySeries.LookupAveargePcode(pcode);

            //if (avgPcode == "")
            //    return new Point(date, Point.MissingValueFlag, PointFlag.Missing);

            //Series avg = GetSeries(cbtt, avgPcode, HydrometHost.PN);
            
            //avg.Read(t1, t2);
            //Logger.WriteLine("Reading Average value for " + cbtt + "/" + pcode);
            //Logger.WriteLine(avg.ToString());

            //var day = date.Day;
            //DateTime t;
            //if (date.Month == 2 && day == 29) // leap year
            //    t = new DateTime(y2, date.Month, 28);
            //else
            //    t = new DateTime(y2, date.Month, date.Day);

            //if (date.Month > 9)
            //    t = new DateTime(y1, date.Month, date.Day);

            //var pt = new Point(date, Point.MissingValueFlag);
            //pt.Value = avg[t].Value;
            //pt.Flag = PointFlag.Estimated;
            //Logger.WriteLine("using estimated data " + cbtt + " " + pcode + " " + pt.DateTime.ToString("yyyy MMM"));

            // new method...
            var computedAvg = AverageValue(cbtt, pcode, date.Month, date.Month,true);
            var pt2 = new Point(date, computedAvg, PointFlag.Estimated);
            //var diff = pt.Value - computedAvg;
            //if( System.Math.Abs(diff)/pt.Value*100 > 3)
            //{
            //    string msg = cbtt + "," + pcode + "," + pt.Value + "," + pt2.Value;
            //    System.IO.File.AppendAllText(@"C:\temp\avg_9999_compare.csv", msg + "\n");
            //    Console.WriteLine(msg);
            //}

            Logger.WriteLine("using estimated data " + cbtt + " " + pcode + " " + pt2.DateTime.ToString("yyyy MMM"));

            return pt2;
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

            //DateTime t1 = new DateTime(8110, month1, 1);
            //DateTime t2 = new DateTime(8110, month2, DateTime.DaysInMonth(8110, month2));
            //var s = GetSeries(cbtt, pcode, HydrometHost.PN); //new HydrometMonthlySeries(cbtt, pcode);
            //s.Read(t1, t2);

            //var rval = Math.Sum(s);
            //// also try average on the fly with new server.. (compare)

            var computed = AverageValue(cbtt, pcode, month1, month2,true);
        //    var diff = computed - rval;

          //  string msg = cbtt + " " + pcode + ",  static/8110, " + rval + ", computed,  " + computed + " diff ," + diff;
            //System.IO.File.AppendAllText(@"C:\temp\static_8110_compare.csv",msg+"\n");
           // Console.WriteLine(msg);
            //return rval;
            return computed;
        }

        private static double AverageValue(string cbtt, string pcode, int month1, int month2, bool limitTo30yearPeriod=false)
        {

            var t1 = new DateTime(1900, 10, 1);
            var t2 = DateTime.Now.Date.AddMonths(-1);

            if (limitTo30yearPeriod)
            {
                t1 = new DateTime(1980, 10, 1);
                t2 = new DateTime(2010, 9, 30);
            }

            var s2 = GetSeries(cbtt, pcode, HydrometHost.PNLinux);
            s2.Read(t1, t2);
            MonthDayRange rng = new MonthDayRange(month1, 1, month2, 1);
            var runoff2 = Math.AggregateAndSubset(StatisticalMethods.Sum, s2, rng, 10);
            //compute monthly average here...
            var rval = Math.Sum(runoff2) / runoff2.Count;

            return rval;
        }

        public static Series GetSeries(string cbtt, string pcode, HydrometHost host)
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
                rval = new HydrometMonthlySeries(cbtt, pcode, host);
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
                                     new DateTime(t.Year, 12, 31), s_server);
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
                                   new DateTime(9999, 9, 30), HydrometHost.PN);

            }
            HydrometMonthlySeries.Cache = cache;
            
            // Anderson forecast without cache  8 seconds.
            //                     with  cache  4 seconds
        }
    }
}
