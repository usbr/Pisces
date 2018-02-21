using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;

namespace HydrometForecast
{
    /// <summary>
    /// Estimates
    /// </summary>
    internal static class MonthlyEstimation
    {

        private static List<Series> RemoveCalculationSeries(SeriesList list)
        {
            var rval = new List<Series>();

            foreach (var item in list)
            {
                if (!(item is CalculationSeries))
                {
                    rval.Add(item);
                }
            }
            return rval;
        }

        /// <summary>
        /// Estimates future data using averages stored in the monthly database
        /// used by AntecedentRunoff Term and Precipitation Term
        /// </summary>
        /// <param name="list"></param>
        /// <param name="forecastDate"></param>
        internal static void EstimateFutureWithAverage(SeriesList list, DateTime forecastDate, double estimationScaleFactor)
        {

            // filter out CalculationSeries
            var subList = RemoveCalculationSeries(list);

            foreach (var s in subList)
            {
                for (int dateIndex = 0; dateIndex < s.Count; dateIndex++)
                {
                    var mp = s[dateIndex];
                    if (mp.DateTime.FirstOfMonth() >= forecastDate.FirstOfMonth()) //... BETTER VERSION????  needed for mid month 
                    //if (mp.DateTime >= forecastDate)
                    {
                        string cbtt = s.ConnectionStringToken("cbtt");
                        string pcode = s.ConnectionStringToken("pcode");
                        var avg = HydrometMonthlySeries.ReadAverageValue(cbtt, pcode, mp.DateTime);

                        if (!avg.IsMissing)
                        {
                            if ( forecastDate.Day !=1 &&  forecastDate.Month == mp.DateTime.Month
                                && !mp.IsMissing && mp.Flag == "M")// mid-month forecast estimate using partial month 
                            {
                                double numDays = DateTime.DaysInMonth(avg.DateTime.Year, avg.DateTime.Month);    
                                double proration = (numDays - forecastDate.Day) / numDays;
                                avg.Value = avg.Value * proration + mp.Value;
                                s[dateIndex] = avg;
                            }
                            else // use average value (with scale factor)
                            {
                                avg.Value = avg.Value * estimationScaleFactor;
                                s[dateIndex] = avg;
                                Console.WriteLine("using estimated data " + cbtt + " " + pcode + " " + s[dateIndex].DateTime.ToString("yyyy MMM"));
                            }
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Estimates missing data by computing the percent
        /// of normal of surrounding data (by month).
        /// if all stations are missing use the average
        /// </summary>
        /// <param name="forecastDate"></param>
        internal static void EstimateMissingByGroup(SeriesList list, DateTime forecastDate)
        {
            // filter series list to remove calculation Series
            var subList = RemoveCalculationSeries(list);

            if (subList.Count == 0)
                return;

            for (int dateIndex = 0; dateIndex < subList[0].Count; dateIndex++)
            {
                double percentOfNormal = ComputePercentOfNormal(subList, dateIndex);
                foreach (var s in subList)
                {
                    string cbtt = s.ConnectionStringToken("cbtt");
                    string pcode = s.ConnectionStringToken("pcode");
                    if (s[dateIndex].IsMissing && s[dateIndex].DateTime <= forecastDate.EndOfMonth())
                    {
                        var pt = HydrometMonthlySeries.ReadAverageValue(cbtt, pcode, s[dateIndex].DateTime);
                        if (!pt.IsMissing)
                        {
                            pt.Value = pt.Value * percentOfNormal;
                            s[dateIndex] = pt;
                            Console.WriteLine("using estimated data " + cbtt + " " + pcode + " " + s[dateIndex].DateTime.ToString("yyyy MMM"));
                        }
                    }
                }
            }
        }

        private static double ComputePercentOfNormal(List<Series> subList, int dateIndex)
        {
            int missingCount = CountMissing(subList, dateIndex);

            //    nothing missing  or, all missing
            if (missingCount == 0 || missingCount == subList.Count)
                return 1.0;

            double sum = 0;
            double sumAvg = 0;
            //int count = 0;

            for (int i = 0; i < subList.Count; i++)
            {
                var s = subList[i];
                string cbtt = s.ConnectionStringToken("cbtt");
                string pcode = s.ConnectionStringToken("pcode");
                if (!s[dateIndex].IsMissing)
                {
                    var avg = HydrometMonthlySeries.ReadAverageValue(cbtt, pcode, s[dateIndex].DateTime).Value;
                    sumAvg += avg;
                    sum += s[dateIndex].Value;

                    //count++;
                }
            }


            return sum / sumAvg;
        }

        private static int CountMissing(List<Series> list, int t)
        {
            int rval = 0;
            foreach (var item in list)
            {
                if (item[t].IsMissing)
                    rval++;
            }

            return rval;
        }

    }
}
