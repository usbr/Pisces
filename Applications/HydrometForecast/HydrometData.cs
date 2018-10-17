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


        public static VariableResolver GetVariableResolver()
        {
           return new HydrometVariableResolver();
        }

        public static void SetupHydrometData(int StartYear, int EndYear, string[] cbttPcodeList, 
            DateTime t, bool cacheAverage, bool allYears = false)
        {
            if (UseSQLite())
            {
                return;
            }
            else
            {
                var cache = new HydrometDataCache();
                if (allYears)
                {
                    cache.Add(cbttPcodeList, new DateTime(StartYear - 1, 1, 1), new DateTime(EndYear, 12, 31));
                }
                else
                {
                    Logger.WriteLine("Caching Hydromet Data for a year ");
                    cache.Add(cbttPcodeList, new DateTime(t.Year - 1, 1, 1), new DateTime(t.Year, 12, 31), s_server);
                }
                //if (cacheAverage) // get average values (special water year 9999 )
                //{
                //    // TO DO... change pcode to average pcode....
                //    var avgCodes = new List<string>();
                //    for (int i = 0; i < cbttPcodeList.Length; i++)
                //    {
                //        string[] tokens = cbttPcodeList[i].Split();
                //        var pc = HydrometMonthlySeries.LookupAveargePcode(tokens[1]);
                //        if( pc.Trim() != "")
                //           avgCodes.Add(tokens[0] + " " + pc);
                //    }

                //    Logger.WriteLine("Caching Hydromet Data for average years");
                //    cache.Add(avgCodes.ToArray(),
                //                       new DateTime(9998, 10, 1),
                //                       new DateTime(9999, 9, 30), HydrometHost.PN);

                //}
                HydrometMonthlySeries.Cache = cache;

                // Anderson forecast without cache  8 seconds.
                //                     with  cache  4 seconds
            }
        }
    }
}
