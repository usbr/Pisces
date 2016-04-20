using Reclamation.Core;
using Reclamation.TimeSeries.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries
{
    public partial class Math
    {
        [FunctionAttribute("Calculates remaining runoff for every day to end of endMonth, provide inflow series and integer month",
     "WaterYearResidual(inflow,endMonth)")]
        public static Series WaterYearResidual(Series inflow, int endMonth)
        {
            Series rval = new Series();
            rval.TimeInterval = TimeInterval.Daily;

            if (inflow.Count < 0)
                return rval;
            var y1 = inflow[0].DateTime.WaterYear();
            var y2 = inflow[inflow.Count-1].DateTime.WaterYear();

            for (int wy = y1; wy <= y2; wy++)
            {
                DateTime t1 = new DateTime(wy - 1, 10, 1);
                int cy = wy; // calendar year
                if (endMonth > 9)
                    cy = wy - 1;
                DateTime t2 = new DateTime(wy, endMonth, DateTime.DaysInMonth(cy,endMonth));
                var singleYear = Math.Subset(inflow, t1, t2);
                var t = t1;
                while (t  <= t2)
                {
                    var sub = Math.Subset(singleYear, t, t2);
                    var val = Math.Sum(sub);
                    rval.Add(t, val);
                    t = t.AddDays(1);
                }
            }

            return rval;
        }


        /// <summary>
        /// Sums input series daily cumulative with reset to zero on October 1.
        /// </summary>
        /// <param name="incremental"></param>
        /// <returns></returns>
        [FunctionAttribute("Daily accumulation beginning October 1",
      "DailyWaterYearRunningTotal(incremental,cumulative)")]
        public static Series DailyWaterYearRunningTotal(Series incremental,Series cumulative)
        {
            int resetMonth = 10;
            int resetDay = 1;
            return DailyCumulative(incremental, cumulative, resetMonth, resetDay);
        }

        /// <summary>
        /// Sums input series daily cumulative with reset to zero on Januar 1.
        /// </summary>
        /// <param name="incremental"></param>
        /// <returns></returns>
        [FunctionAttribute("Daily accumulation beginning January 1",
      "DailyCalendarYearRunningTotal(incremental,cumulative)")]
        public static Series DailyCalendarYearRunningTotal(Series incremental, Series cumulative)
        {
            int resetMonth = 1;
            int resetDay = 1;
            return DailyCumulative(incremental, cumulative, resetMonth, resetDay);
        }




        private static Series DailyCumulative(Series incremental, Series cumulative, int resetMonth, int resetDay)
        {
            Series rval = new Series();
            rval.TimeInterval = TimeInterval.Daily;
            if (incremental.Count == 0 || cumulative.Count == 0
                || cumulative[0].IsMissing)
            {
                Console.WriteLine("Mising data: ");
                Console.WriteLine("incremental");
                incremental.WriteToConsole();
                Console.WriteLine("cumulative");
                cumulative.WriteToConsole();
                return rval;
            }


            double sum = cumulative[0].Value;
            bool missing = false;
            rval.Add(cumulative[0]);
            for (int i = 1; i < incremental.Count; i++)
            {
                var t = incremental[i].DateTime;
                if (t.Month == resetMonth && t.Day == resetDay)
                    sum = 0.0;

                if (!missing && !incremental[i].IsMissing)
                {
                    sum += incremental[i].Value;
                    rval.Add(incremental[i].DateTime, sum);
                }
                else
                {
                    rval.AddMissing(incremental[i].DateTime);
                    Console.WriteLine("Missing data: incremental: " + incremental[i].ToString());
                    missing = true;
                }

            }

            return rval;
        }


        [FunctionAttribute("Daily Change between Midnight Values",
       "DailyChange(series1)")]
        public static Series DailyChange(Series s)
        {
            Series rval = new Series();
            rval.TimeInterval = TimeInterval.Daily;
            if (s.Count == 0)
                return new Series();

            var t = s.MinDateTime.Date;

            while (t <= s.MaxDateTime)
            {

                int idx1 = s.IndexOf(t.Date); // midnight
                int idx2 = s.IndexOf(t.AddDays(1).Date); // midnight

                if (idx1 >= 0 && idx2 >= 0 && !s[idx1].IsMissing && !s[idx2].IsMissing)
                {
                    double diff = s[idx2].Value - s[idx1].Value;
                    rval.Add(t, diff);
                }
                else
                {
                    rval.AddMissing(t);
                }

                t = t.AddDays(1).Date;
            }


            return rval;
        }


        [FunctionAttribute("Midnight Values",
        "DailyMidnight(series1)")]
        public static Series DailyMidnight(Series s)
        {
            Series rval = new Series();
            rval.TimeInterval = TimeInterval.Daily;
            if (s.Count == 0)
                return rval;

            var t = s.MinDateTime.Date;

            while (t <= s.MaxDateTime)
            {
                int idx = s.IndexOf(t.AddDays(1).Date); // midnight
                if (idx >= 0)
                    rval.Add(t,s[idx].Value);
                else
                    rval.AddMissing(t);

                t = t.AddDays(1).Date;
            }


            return rval;
        }


        [FunctionAttribute("Growing Degree Days",
        "DailyGrowingDegree(sMax, sMin,tmax,tbase)")]
        public static Series DailyGrowingDegree(Series sMax, Series sMin, double tmax, double tbase)
        {
            var max = DailySubsetCalculatorPoint(sMax, TimeSeries.Math.MaxPoint);
            var min = DailySubsetCalculatorPoint(sMin, TimeSeries.Math.MinPoint);
            Series s = GrowDeg(max, min, tmax, tbase);

            return s;
        }


        /// <summary>
        /// Returns the growing degree day value.
        /// </summary>
        internal static Series GrowDeg(Series sMax, Series sMin, double tmax, double tBase)
        {
            for (int i = 0; i < sMax.Count; i++)
            {
                if (!sMax[i].IsMissing &&  sMax[i].Value > tmax)
                    sMax[i] = new Point(sMax[i].DateTime, tmax);
            }
            for (int i = 0; i < sMin.Count; i++)
            {
                if (!sMin[i].IsMissing &&  sMin[i].Value < tBase)
                    sMin[i] = new Point(sMin[i].DateTime, tBase);
            } 

            var rval =  ((sMax + sMin) / 2.0) - tBase;
            for (int i = 0; i < rval.Count; i++)
            {
                var pt = rval[i];
                if (pt.IsMissing)
                    continue;
                if (pt.Value < 0)
                {
                    pt.Value = 0;
                    rval[i] = pt;
                }
            }
            return rval;

            //return System.Math.Round(val, 2);
        }


        [FunctionAttribute("Sums values for each day. ",
          "DailySum(series1)")]
        public static Series DailyAzimuth(Series source)
        {
            return DailySubsetCalculatorValue(source, AzimuthResultant);
        }

        /// <summary>
        /// Calculates resultant azimuth using vector addition.
        /// JR: This calculation only outputs the average vector direction without taking the magnitude (wind speed)
        ///     into account. Do we need to do a weighted vector using the wind speed?
        /// </summary>
        public static double AzimuthResultant(Series WDData)
        {
            int count = WDData.Count;
            double xRes = 0.0, yRes = 0.0;
            int i = 0;
            double val = 0;
            double cosTerm = 0;
            double sinTerm = 0;

            /*
             Convention is as shown here for the vector calculations. Orientation below is such that the calculations below 
             make sense from a mathematical (not a directional) perspective.
                                E (deg = 90)
                                |
                                |
                  (180) S ______|______ N (0)
                                |
                                |
                                |
                                W (270)

            */
            while (i < count)
            {
                if (WDData.Values[i] == 0)
                {
                    cosTerm = 1.0;
                    sinTerm = 0.0;
                }
                else
                {
                    cosTerm = System.Math.Cos(WDData.Values[i] * System.Math.PI / 180.0);
                    sinTerm = System.Math.Sin(WDData.Values[i] * System.Math.PI / 180.0);
                }
                xRes = xRes + cosTerm;
                yRes = yRes + sinTerm;
                i++;
            }
            double magnitude = System.Math.Sqrt(System.Math.Pow(xRes, 2.0) +
                System.Math.Pow(yRes, 2.0));
            double xVect = xRes / magnitude;
            double yVect = yRes / magnitude;
            if (xRes > 0)
            {
                if (yRes >= 0)
                {
                    val = (System.Math.Abs(System.Math.Atan(yVect / xVect)) *
                      180 / System.Math.PI);
                }
                else
                {
                    val = 360.0 - (System.Math.Abs(System.Math.Atan(yVect / xVect)) *
                      180 / System.Math.PI);
                }
            }
            else
            {
                if (yRes >= 0)
                {
                    val = 180.0 - (System.Math.Abs(System.Math.Atan(yVect / xVect)) *
                      180.0 / System.Math.PI);
                }
                else
                {
                    val = 180.0 + (System.Math.Abs(System.Math.Atan(yVect / xVect)) *
                      180.0 / System.Math.PI);
                }
            }

            return System.Math.Round(val, 2);
        }
        /// <summary>
        /// Creates a list of Series , one subset for each day
        /// used for math functions that need subset by day
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static Series DailySubsetCalculatorPoint(Series s, Func<Series, Point> func)
        {
            Series rval = new Series();
            rval.TimeInterval = TimeInterval.Daily;

            DateTime t;
            if (s.Count > 0)
            {
                t = s[0].DateTime.Date; // midnight
            }
            else
            {
                return rval;
            }

            while (t < s.MaxDateTime)
            {
                var subset = Math.Subset(s, new DateRange(t, t.AddDays(1)), false);
                subset.RemoveMissing();
                if (subset.Count > 0)
                {
                    Point pt = func(subset);
                    rval.Add(t, pt.Value, PointFlag.Computed);
                }
                t = t.AddDays(1).Date;
            }
            return rval;
        }



        /// <summary>
        /// Creates a list of Series , one subset for each day
        /// used for math functions that need subset by day
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static Series DailySubsetCalculatorValue(Series s, Func<Series,double> func, 
            int requiredNumberOfPoints = 0)
        {
            Series rval = new Series();
            rval.TimeInterval = TimeInterval.Daily;

            DateTime t;
            if (s.Count > 0)
            {
                t = s[0].DateTime.Date; // midnight
            }
            else
            {
                return rval; 
            }
            
            while (t < s.MaxDateTime)
            {
              var subset = Math.Subset(s, new DateRange(t, t.AddDays(1)), false);
              subset.RemoveMissing(true);

              if (subset.Count < requiredNumberOfPoints)
              {
                  Logger.WriteLine("Error: not enough data points "+s.Name);
                  Logger.WriteLine("expected "+requiredNumberOfPoints+" points, but had only "+subset.Count);
                  rval.AddMissing(t);
              }
              else
              if (subset.Count > 0)
              {
                  double d = func(subset);
                  rval.Add(t, d, PointFlag.Computed);
              }
              t = t.AddDays(1).Date;
            }
            return rval;
        }

        /// <summary>
        /// Daily sum of values.
        /// </summary>
        /// <returns></returns>
        [FunctionAttribute("Sums values for each day. ",
           "DailySum(series1,requiredNumberOfPoints)")]
        public static Series DailySum(Series source, int requiredNumberOfPoints)
        {
            return DailySubsetCalculatorValue(source, Math.Sum,requiredNumberOfPoints);
        }


        /// <summary>
        /// Simple daily average.  no minimum number of points 
        /// </summary>
        /// <returns></returns>
        [FunctionAttribute("Computes a daily average. No minimum number of points required per day",
           "DailyAverage(series1)")]
        public static Series DailyAverage(Series s)
        {
            return DailyValues(s, Math.SimpleAverageForDay,0);
        }

        /// <summary>
        /// Computes a daily average with required number of points.
        /// </summary>
        /// <returns></returns>
        [FunctionAttribute("Computes a daily average with required number of points.",
           "DailyAverage(series1,requiredNumberOfPoints)")]
        public static Series DailyAverage(Series s, int requiredNumberOfPoints)
        {
            return DailyValues(s, Math.SimpleAverageForDay,requiredNumberOfPoints);
        }



        /// <summary>
        /// Calls a function for each day returning a Daily Series
        /// Uses with calculations that can also perform subset of Series by day
        /// </summary>
        /// <param name="s"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        private static Series DailyValues(Series s, Func<Series,DateTime,int,Point> fun, int requiredNumberOfPoints )
        {
            //Console.WriteLine("Calling Generic DailyValues with "+fun.ToString());
            Series rval = new Series(s.Units, TimeInterval.Daily);
            rval.HasFlags = true;
            DateTime t;
            if (s.Count > 0)
            {
                t = s[0].DateTime.Date; // midnight
            }
            else
                return rval;

            while (t < s.MaxDateTime)
            {
                Point pt = fun(s,t,requiredNumberOfPoints);
                rval.Add(pt);
                t = t.AddDays(1);
            }

            return rval;
        }
       

        private static Point SimpleAverageForDay(Series s, DateTime t, int requiredNumberOfPoints)
        {

            t = t.Date;
            DateTime tomorrow = t.AddDays(1);
            int index = s.LookupIndex(t);
            if (index < 0) //|| !s.WithinRange(t))
            {
                // throw new Exception("cannot find the date for an average");
                return new Point(t, Point.MissingValueFlag, PointFlag.Missing);
            }
            Point rval = new Point(t, 0);
            int count = 0;
            double val = 0;
            while (index < s.Count)
            {
                // integrate through the day.
                Point point = s[index];
                if (point.DateTime >= tomorrow)
                {
                    break;
                }
                if (!point.IsMissing && !point.FlaggedBad)
                {
                    val += point.Value;
                    count++;
                }
                index++;
            }

            if (count == 0)
                return new Point(t.Date, Point.MissingValueFlag, PointFlag.Missing);
            else if( count <requiredNumberOfPoints)
                return new Point(t.Date, Point.MissingValueFlag, PointFlag.Missing);
            else
                return new Point(t.Date, val / count, PointFlag.Computed);
        }

        public static Series DailyAverage(Series s, DateTime t1, DateTime t2)
        {
            // remove missing values
            s.RemoveMissing(true);
            if (s.Count == 0)
            {
                return new Series(s.Units, TimeInterval.Daily);
            }

            DateTime time = t1;

            Series rval = new Series(s.Units, TimeInterval.Daily);
            rval.HasFlags = true;
            rval.Name = "Daily Average " + s.Name;
            while (time <= t2)
            {// Console.WriteLine("Avg("+time.ToShortDateString()+")");
                rval.Add(TimeWeightedAverageForDay(s, time));
                time = time.AddDays(1);
            }
            return rval;
        }


        [FunctionAttribute("Computes a Time weightedd daily average, by interpolating midnight values and performing numerical integration",
        "TimeWeightedDailyAverage(series1)")]
        /// <summary>
        /// Time weighted daily average
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Series TimeWeightedDailyAverage(Series s)
        {
            if (s.Count == 0)
                return new Series();

            DateTime t1 = s[0].DateTime.Date;

            if (t1 != s[0].DateTime)
            {// not exactly midnight.. start at next day.
                t1 = t1.AddDays(1);
            }
            DateTime t2 = s[s.Count - 1].DateTime.Date;
            if (t2 != s[s.Count - 1].DateTime)
            {
                t2 = t2.AddDays(-1);
            }
            return DailyAverage(s, t1, t2);
        }


        [FunctionAttribute("Computes a daily maximum.  The input data should have a interval smaller than daily",
         "DailyMax(series1)")]
        public static Series DailyMax(Series source)
        {
            Series rval = source.Clone(); 
            source.RemoveMissing(true);
            if (source.Count > 0)
            {
                Point max = source[0];
                max.DateTime = max.DateTime.Date; // force to 00:00
                for (int i = 0; i < source.Count; i++)
                {
                    Point a = source[i];
                    a.DateTime = a.DateTime.Date;
                    if (max.DateTime.Date != a.DateTime.Date)
                    {
                        rval.Add(max);
                        max = a;
                    }
                    else if (a.Value > max.Value)
                    {
                        max.Value = a.Value;
                    }

                    if (i == source.Count - 1)
                    {
                        rval.Add(max);
                    }
                }
            }
            rval.TimeInterval = TimeInterval.Daily;
            return rval;
        }
        [FunctionAttribute("Computes a daily minimum.  The input data should have a interval smaller than daily",
         "DailyMin(series1)")]
        public static Series DailyMin(Series source)
        {

           // return DailySubsetCalculator(source, Math.Min);

            Series rval = source.Clone();
            source.RemoveMissing(true);
            if (source.Count > 0)
            {
                Point min = source[0];
                min.DateTime = min.DateTime.Date;
                for (int i = 0; i < source.Count; i++)
                {
                    Point a = source[i];
                    a.DateTime = a.DateTime.Date;
                    if (min.DateTime.Date != a.DateTime.Date)
                    {
                        rval.Add(min);
                        min = a;
                    }
                    else if (a.Value < min.Value)
                    {
                        min.Value = a.Value;
                    }

                    if (i == source.Count - 1)
                    {
                        rval.Add(min);
                    }
                }
            }
            rval.TimeInterval = TimeInterval.Daily;
            return rval;

        }


    }
}
