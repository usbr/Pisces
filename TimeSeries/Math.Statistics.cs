using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Reclamation.Core;
using Reclamation.TimeSeries.Parser;

namespace Reclamation.TimeSeries
{

  

    public static partial class Math
    {

        public static double Sum(Series a)
        {
            return Sum(a, false);
        }
        public static double Sum(Series a, bool ignoreNegatives)
        {
            int sz = a.Count;
            double rval = 0.0;
            for (int i = 0; i < sz; i++)
            {
                Point pt = a[i];

                if (pt.Flag != PointFlag.Missing && pt.Value != Point.MissingValueFlag)
                {
                    if (ignoreNegatives && pt.Value < 0)
                    {
                        continue;
                    }

                    rval += pt.Value;
                }
            }
            return rval;
        }


        /// <summary>
        /// Count number of non-missing points
        /// </summary>
        public static int Count(Series a)
        {
            int rval = 0;
            int sz = a.Count;
            for (int i = 0; i < sz; i++)
            {
                if (!a[i].IsMissing)
                {
                    rval++;
                }
            }
            return rval;
        }

        /// <summary>
        /// find point with lowest value
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Point MinPoint(Series a)
        {
            int sz = a.Count;
            Point rval = Point.Missing;
            double low = double.MaxValue;
            for (int i = 0; i < sz; i++)
            {
                Point pt = a[i];
                if (!pt.IsMissing)
                {
                    if (pt.Value < low)
                    {
                        rval = pt;
                        low = pt.Value;
                    }
                }
                else if (rval.IsMissing )
                {// if all data is missing 
                 // we at least return a valid date time
                 // if Count >0
                    rval.DateTime = pt.DateTime;
                }
            }
            return rval;
        }
        /// <summary>
        /// find point with maximum value
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Point MaxPoint(Series a)
        {
            int sz = a.Count;
            Point rval = Point.Missing;
            double hi = double.MinValue;
            for (int i = 0; i < sz; i++)
            {
                Point pt = a[i];
                if (!pt.IsMissing)
                {
                    if (pt.Value > hi)
                    {
                        rval = pt;
                        hi = pt.Value;
                    }
                }
                else if (rval.IsMissing)
                {// if all data is missing 
                    // we at least return a valid date time
                    // if Count >0
                    rval.DateTime = pt.DateTime;
                }
            }
            return rval;
        }



        /// <summary>
        /// computes the Max value 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [FunctionAttribute("series","Max(series1,double)")]
        public static Series Max(Series s,double value)
        {
            Series rval = s.Copy();
            for (int i = 0; i < s.Count; i++)
            {
                Point pt = s[i];
                if (pt.IsMissing)
                {
                    continue;
                }

                pt.Value = System.Math.Max(pt.Value,value);
                rval[i] = pt;
            }
            return rval;
        }


        /// <summary>
        /// computes the Min value 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [FunctionAttribute("series", "Min(series1,double)")]
        public static Series Min(Series s, double value)
        {
            Series rval = s.Copy();
            for (int i = 0; i < s.Count; i++)
            {
                Point pt = s[i];
                if (pt.IsMissing)
                {
                    continue;
                }

                pt.Value = System.Math.Min(pt.Value, value);
                rval[i] = pt;
            }
            return rval;
        }


        /// <summary>
        /// computes the absolute value for points
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [FunctionAttribute("Computes a absolute value for each data point in the series",
         "Abs(series1)")]
        public static Series Abs(Series s)
        {
            Series rval = s.Copy();
            for (int i = 0; i < s.Count; i++)
            {
                Point pt = s[i];
                if (pt.IsMissing)
                {
                    continue;
                }

                pt.Value = System.Math.Abs(pt.Value);
                rval[i] = pt;
            }
            return rval;
        }

        /// <summary>
        /// computes avearge of all values in series.
        /// mising values are not included in the average.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double AverageOfSeries(Series s)
        {
            double sum = 0;
            int sz = s.Count;
            int counter = 0;
            for (int i = 0; i < sz; i++)
            {
                Point pt = s[i];
                if (!pt.IsMissing)
                {
                    sum += pt.Value;
                    counter++;
                }
            }

            if (counter == 0)
            {
             //   throw new Exception("Error: The Average value cannot be computed, since there is no data");
                return Point.MissingValueFlag;
            }
            double avg = sum / counter;
            return avg;
        }

        /// <summary>
        /// get time weighted average for a Date.
        /// must have data before and after midnight (or exactly midnight)
        /// Will not extrapolate.
        /// Missing data must be cleaned out before running this routine.
        /// </summary>
        public static Point TimeWeightedAverageForDay(Series s, DateTime dateTime)
        {
            DateTime midnight = dateTime.Date; // go to midnight to be sure.
            DateTime noon = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, 12, 0, 0, 0);
            int indexMidnight = s.LookupIndex(midnight);// check that we have midnight on the other end?
            int index2 = s.LookupIndex(midnight.AddDays(1));
            if (indexMidnight < 0 || index2 <0 ) //|| !s.WithinRange(t))
            {
               // throw new Exception("cannot find the date for an average");
                return new Point(noon, Point.MissingValueFlag, PointFlag.Missing);
            }

            if( s[indexMidnight].IsMissing)
                return new Point(noon, Point.MissingValueFlag, PointFlag.Missing);


            double val = 0;
            DateTime date;
            DateTime prevDate = midnight;

            

            TimeSpan distance = new TimeSpan( Convert.ToInt64(System.Math.Abs(s[indexMidnight].DateTime.Ticks-midnight.Ticks)));
            
            

            if (distance.Days >= 1)
            {
                return new Point(noon, Point.MissingValueFlag, PointFlag.Missing);
            }

            double prevVal = Interpolate(s, midnight, false, indexMidnight); // get midnight value.

            double seconds;
            double incValue = 0;
            double totalSeconds = 0;
            double average = 0;

            long ticks;
            TimeSpan ts;
            while (indexMidnight < s.Count)
            {
                // integrate through the day.
                Point point = s[indexMidnight];
                date = point.DateTime;
                val = point.Value;

                if (!point.IsMissing)
                {
                    //  Console.WriteLine(index+", "+date.ToString("yyyy-MM-dd HH:mm:ss")+", "+val);
                    if (date.Date > midnight)
                    {
                        break;
                    }

                    ticks = date.Ticks - prevDate.Ticks;
                    ts = new TimeSpan(ticks);

                    seconds = ts.TotalSeconds;
                    totalSeconds += seconds;
                    incValue = (prevVal + val) * 0.5 * seconds;
                    //Console.WriteLine("seconds = "+seconds);
                    average += incValue;

                    prevDate = date;
                    prevVal = val;
                }
                else
                {
                    Console.WriteLine("Warning: encounted data flagged as missing ");
                }
                indexMidnight++;
            }

            // interpolate midnight at other end..

            date = midnight.AddDays(1);
            //Console.WriteLine(date.ToLongDateString()+" "+date.ToLongTimeString());
            val = Interpolate(s, date, false, indexMidnight);
            ticks = date.Ticks - prevDate.Ticks;
            ts = new TimeSpan(ticks);
            seconds = ts.TotalSeconds;
            //Console.WriteLine("seconds = "+seconds);
            totalSeconds += seconds;
            incValue = (prevVal + val) * 0.5 * seconds;
            average += incValue;

            // total Seconds should be one day
            if (System.Math.Abs(totalSeconds - 86400) > 0.1)
            {
                throw new Exception("error  wrong number of seconds for a day " + totalSeconds.ToString());
            }
            average = average / totalSeconds;
           
            return new Point(noon, average,PointFlag.Computed);
        }



        /// <summary>
        ///
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        [Function("Computes a 7 Day moving average, by looking 3 days forward and back",
            "SevenDayMovingAverage(series1)")]
        public static Series SevenDayMovingAverage(Series s)
        {
            // TO DO:  performance is poor for large data set.
            // could create 'normalized' data set first so we could index our way through
            // the data without the Math.Subset() call

            if (s.TimeInterval != TimeInterval.Daily)
                throw new ArgumentException("TimeInterval must be Daily.  Try to first create a daily average before the 7 day moving average");

            Series rval = new Series();
            Series.CopyAttributes(s, rval);
            if( s.Count == 0)
                return rval;
            DateTime tMax = s.MaxDateTime.Date.AddHours(23).AddMinutes(59).AddSeconds(59.9);
            DateTime t = s[0].DateTime.Date;
            t = t.AddDays(3);

            DateTime t1 = t.AddDays(-3);
            DateTime t2 = t.AddDays(3).AddHours(23).AddMinutes(59).AddSeconds(59.9);
            while (t2 <= tMax)
            {
               DateRange rng = new DateRange(t1,t2);
               Series filtered = Math.Subset(s, rng);
               double avg = AverageOfSeries(filtered);
               filtered.RemoveMissing();
               if (filtered.Count != 7) // require 7 days of data
                  rval.AddMissing(t);
               else
               if (avg == Point.MissingValueFlag)
                   rval.AddMissing(t);
               else
                   rval.Add(t.Date, avg, PointFlag.Computed);

               t = t.AddDays(1);
               t1 = t1.AddDays(1);
               t2 = t2.AddDays(1);
            }



            return rval;
            
        }
        /// <summary>
        /// Create a new series using observed data, and if necessary use
        /// estimated data to fill gaps
        /// </summary>
        /// <param name="observed">partial series of 'real' data</param>
        /// <param name="estimated">full series of estimated data</param>
        /// <returns></returns>
        [FunctionAttribute("Merges two series",
            "Merge(observed,estimated)")]
        public static Series Merge(Series observed, Series estimated)
        {
            Series rval = observed.Copy();
            if (estimated.HasFlags && !rval.HasFlags)
                rval.HasFlags = true;

            rval.RemoveMissing();

            for (int i = 0; i < estimated.Count; i++)
            {
                Point est = estimated[i];

                if (rval.IndexOf(est.DateTime) < 0  ) // outside range or no entry
                {
                    rval.Add(est);
                }
            }

            return rval;
        }

        [FunctionAttribute("Computes the maximum value each day and then computes a seven day moving average",
        "SevenDADMAX(series1)")]
        public static Series SevenDADMAX(Series s)
        {
            Series max = Math.DailyMax(s);
            return SevenDayMovingAverage(max);
        }
        

        [FunctionAttribute("Used to condense data (5 minute for example) to hourly. The hourly filter returns all values at the top of the hour (where the minute = 0).", "HourlyFilter(series1)")]
        public static Series HourlyFilter(Series s)
        {
            Series rval = s.Clone();

            rval.TimeInterval = TimeInterval.Hourly;
            for (int i = 0; i < s.Count; i++)
            {
                var pt = s[i];
                if (pt.DateTime.Minute == 0)
                    rval.Add(pt);
            }
            return rval;
        }

        [FunctionAttribute("Fills in missing daily or monthly data with zeros.",
         "FillMissingWithZero(series1)")]
        public static Series FillMissingWithZero(Series s)
        {
            Series rval = s.Clone();
            if( s.Count >0)
                rval = Math.FillMissingWithZero(s, s.MinDateTime, s.MaxDateTime);

            return rval;
        }


        [FunctionAttribute("Creates Hourly avearge",
      "HourlyAverage(series)")]
         public static Series HourlyAverage(Series s)
         {
             return Average(s , TimeInterval.Hourly);
         }



        [FunctionAttribute("Creates Hourly data by interpolating at 1 hour increment",
      "InterpolateHourly(series)")]
        public static Series InterpolateHourly(Series s)
        {
            Series rval = s.Clone();
            s.RemoveMissing();

            if (s.Count == 0)
                return rval;

            DateTime t = s.MinDateTime;
            DateTime hr = new DateTime(t.Year, t.Month, t.Day, t.Hour, 0, 0);
            if (hr < t)
            {
                t = hr.AddHours(1);
            }
            else
            {
                t = hr;
            }

            DateTime t2 = s.MaxDateTime;
            t2 = new DateTime(t2.Year, t2.Month, t2.Day, t2.Hour, 0, 0);

            while (t <= t2)
            {
                double val = Math.Interpolate(s, t);
                rval.Add(t, val, "interpolated");
                t = t.AddHours(1);

            }

            return rval;
        }


         public static Series MovingAvearge(Series s, double hours) // spelling typo.... keep for legacy
         {
             return MovingAverage(s, hours);
         }

        [FunctionAttribute("Computes a Moving average based on the number of hours specified",
            "MovingAverage(series1,hours)")]
        public static Series MovingAverage(Series s, double hours)
         {
            Series rval = new Series();

            Series.CopyAttributes(s, rval);
            rval.Name += " " + hours + " hour average";
            if (s.Count == 0)
                return rval;

            int startIndex = s.LookupIndex(s.MinDateTime.AddHours(hours));
            if (startIndex < 0)
                return rval;
            for (int i = startIndex; i < s.Count; i++)
            {
                Point pt = s[i];
                DateTime t1 = pt.DateTime.AddHours(-hours);
                DateTime t2 = pt.DateTime;
                DateRange range = new DateRange(t1, t2);
                Series subset = Math.Subset(s, range);
                double avg = Math.AverageOfSeries(subset);
                pt.Value = avg;
                rval.Add(pt);

            }
            return rval;
         }



       

        public static Series AnnualSum(Series s,
            MonthDayRange range, int beginningMonth)
        {
            return AnnualSum(s, range, beginningMonth,false);
        }


        public static Series AnnualAverage(Series s, MonthDayRange range, 
            int beginningMonth)
        {
            if (s.Count == 0)
            {
                return new Series();
            }

            DateTime t1 = s[0].DateTime;
            DateTime t2 = s[s.Count - 1].DateTime;

            YearRange wy1 = new YearRange(t1, beginningMonth);
            YearRange wy2 = new YearRange(t2, beginningMonth);

            Series rval = s.Clone();
            rval.HasFlags = true;
            rval.TimeInterval = TimeInterval.Yearly;
            rval.Appearance.Style = Styles.Point;

            for (int wy = wy1.Year; wy <= wy2.Year; wy++)
            {
                YearRange yr = new YearRange(wy, beginningMonth);
                DateRange dr = new DateRange(range, yr.Year, yr.BeginningMonth);

                Series subset = Math.Subset(s, dr);

                int removeCount = subset.RemoveMissing();
                if (removeCount > 0)
                {
                    rval.Messages.Add("Removed " + removeCount + " points from " + s.Name);
                }
                double avg = Math.AverageOfSeries(subset);
                string flag = "";
                if (subset.Count != dr.Count)
                {
                    int missingCount = dr.Count - subset.Count;
                    flag = missingCount + " missing";
                }

                 rval.Add(yr.DateTime2, avg, flag);
            }

            return rval;
        }
        /// <summary>
        /// find sum for each year.
        /// </summary>
        public static Series AnnualSum(Series s, 
            MonthDayRange range,int beginningMonth, bool hasTraces)
        {
            if (s.Count == 0)
            {
                return new Series();
            }

            DateTime t1 = s[0].DateTime;
            DateTime t2 = s[s.Count - 1].DateTime;

            YearRange wy1 = new YearRange(t1, beginningMonth);
            YearRange wy2 = new YearRange(t2, beginningMonth);

            Series rval = s.Clone();
            rval.HasFlags = true;
            rval.TimeInterval = TimeInterval.Yearly;
            rval.Appearance.Style = Styles.Point;

            for (int wy = wy1.Year; wy <= wy2.Year; wy++)
            {
                YearRange yr = new YearRange(wy, beginningMonth);
                DateRange dr = new DateRange(range, yr.Year, yr.BeginningMonth);

                Series subset = Math.Subset(s,dr);
                
                int removeCount = subset.RemoveMissing();
                if (removeCount > 0)
                {
                    rval.Messages.Add("Removed " + removeCount + " points from " + s.Name);
                }
                double sum = Math.Sum(subset);
                string flag = "";
                if( subset.Count != dr.Count)
                { 
                    int missingCount = dr.Count-subset.Count;
                    flag = missingCount +" missing";
                }
                if (hasTraces)
                {
                    if (rval.Count == 0)
                    {
                        rval.Add(yr.DateTime2, sum, flag);
                    }
                    else
                    {
                        Point pt = rval[0];
                        pt.Value += sum;
                        rval[0] = pt;
                    }
                }
                else
                {
                    rval.Add(yr.DateTime2, sum, flag);
                }
            }

            return rval;
        }

        public static Series AnnualMin(Series s, MonthDayRange range, int beginningMonth)
        {
            return AnnualMin(s, range, beginningMonth, false);
        }
        /// <summary>
        /// find minimum value in each year
        /// </summary>
        public static Series AnnualMin(Series s, MonthDayRange range ,int beginningMonth, bool hasTraces)
        {
            if (s.Count == 0)
            {
                return new Series();
            }

            DateTime t1 = s[0].DateTime;
            DateTime t2 = s[s.Count - 1].DateTime;

            YearRange wy1 = new YearRange(t1, beginningMonth);
            YearRange wy2 = new YearRange(t2, beginningMonth);

            Series rval = s.Clone();
            rval.TimeInterval = TimeInterval.Yearly;
            rval.Appearance.Style = Styles.Point;

            for (int wy = wy1.Year; wy <= wy2.Year; wy++)
            {
                YearRange yr = new YearRange(wy, beginningMonth);
                DateRange dr = new DateRange(range, yr.Year, yr.BeginningMonth);

                Series subset = TimeSeries.Math.Subset(s, dr);

                if (subset.Count > 0)
                {
                    Point pt = MinPoint(subset);
                    if (pt.IsMissing)
                    {
                        if (hasTraces)
                        {
                            if (rval.Count == 0)
                            {
                                rval.AddMissing(pt.DateTime);
                            }
                        }
                        else
                        {
                          rval.AddMissing(pt.DateTime);
                        }
                    }
                    else
                    {
                        if (hasTraces)
                        {
                            if (rval.Count == 0)
                            {
                                rval.Add(pt);
                            }
                            else
                            {
                                if (pt.Value < rval[0].Value)
                                {
                                    rval[0] = pt;
                                }
                            }
                        }
                        else
                        {
                            if (rval.IndexOf(pt.DateTime) >= 0)
                            {
                                Console.WriteLine("Error point allready here");
                                rval.WriteToConsole();
                                Console.WriteLine("about to crash!");
                            }
                         rval.Add(pt);
                        }
                    }

                }
            }

            return rval;
        }

        public static Series AnnualMax(Series s, MonthDayRange range, int beginningMonth)
        {
            return AnnualMax(s, range, beginningMonth, false);
        }
       
        /// <summary>
        /// Compute Annual Max
        /// </summary>
        public static Series AnnualMax(Series s, MonthDayRange range, int beginningMonth,bool hasTraces)
        {
            if (s.Count == 0)
            {
                return new Series();
            }

            DateTime t1 = s[0].DateTime;
            DateTime t2 = s[s.Count - 1].DateTime;

            YearRange wy1 = new YearRange(t1, beginningMonth);
            YearRange wy2 = new YearRange(t2, beginningMonth);

            Series rval = s.Clone();
            rval.TimeInterval = TimeInterval.Yearly;
            rval.Appearance.Style = Styles.Point;

            for (int wy = wy1.Year; wy <= wy2.Year; wy++)
            {
                YearRange yr = new YearRange(wy, beginningMonth);
                DateRange dr = new DateRange(range, yr.Year, yr.BeginningMonth);

                Series subset = TimeSeries.Math.Subset(s, dr);

                //Series subset = TimeSeries.Math.Subset(s, yr.DateTime1, yr.DateTime2);
                if (subset.Count > 0)
                {
                    Point pt = MaxPoint(subset);
                    pt.Flag = wy.ToString();
                    if (pt.IsMissing)
                    {
                        if (hasTraces)
                        {
                            if (rval.Count == 0)
                            {
                                rval.AddMissing(pt.DateTime);
                            }
                        }
                        else
                        {
                            rval.AddMissing(pt.DateTime);
                        }
                    }
                    else
                    {
                        if (hasTraces)
                        {
                            if (rval.Count == 0)
                            {
                                rval.Add(pt);
                            }
                            else
                            {
                                if (pt.Value > rval[0].Value)
                                {
                                    rval[0] = pt;
                                }
                            }
                        }
                        else
                        {
                            rval.Add(pt);
                        }
                    }
                }
            }
            return rval;
        }




        public static Series AggregateAndSubset(StatisticalMethods type, Series s, MonthDayRange range, int beginningMonth)
        {

            if (!range.ValidBeginningMonth(beginningMonth))
            {
                throw new ArgumentOutOfRangeException(
                    "Please check the date range you entered. It needs to be consistent\n"
                    + "with the type of year: i.e. 'water year', 'calendar year', or custom year\n"
                    + "your range is "
                    + range.ToString() + " and your beginning month is " + beginningMonth);
            }
            switch (type)
            {
                case StatisticalMethods.None:
                    return Subset( s,range);
                case StatisticalMethods.Max:
                    return AnnualMax(s, range,beginningMonth);
                case StatisticalMethods.Min:
                    return AnnualMin(s, range,beginningMonth);
                case StatisticalMethods.Sum:
                    return AnnualSum(s, range,beginningMonth);
                case StatisticalMethods.Average:
                    return AnnualAverage(s, range, beginningMonth);
                default:
                    throw new NotSupportedException("the Aggregate type " + type.ToString() + " is not supported");
            }
        }

        
        /// <summary>
        /// Sorts data from highest value to lowest.
        /// The date column order is lost, and a Percent column is added.
        /// </summary>
        public static Series Sort(Series s, RankType type)
        {
            Series rval = s.Copy();
            rval.Table.Columns.Add("Percent", typeof(double));

            // sort by value descending
            string sort = "[" + rval.Table.Columns[1].ColumnName + "] desc";
            rval.Table.DefaultView.Sort = sort;

            rval.Table.DefaultView.ApplyDefaultSort = true;

            int sz = rval.Count;
            if (type == RankType.Weibul)
            {
                for (int i = 0; i < sz; i++)
                {
                    DataRowView row = rval.Table.DefaultView[i];
                    row["Percent"] = ((double)(i + 1.0) / (sz + 1.0)) * 100.0; //Weibul
                }
            }
            else
            if (type == RankType.Proabability)
            {
                for (int i = 0; i < sz; i++)
                {
                    DataRowView row = rval.Table.DefaultView[i];
                    row["Percent"] = ((double)(i + 1.0) / (sz )) * 100.0; 
                }
            }
            return rval;
        }

        /*
         example output from Math.Sort( SortType.Weibul)
         * 
         * Date          Value    Percent
1999-06-17 00:00:00.00	4810.00	0.0274499039253363
1997-05-16 00:00:00.00	4410.00	0.0548998078506725
1997-05-17 00:00:00.00	4410.00	0.0823497117760088
1999-06-18 00:00:00.00	4380.00	0.109799615701345
1997-05-15 00:00:00.00	4320.00	0.137249519626681
1997-06-01 00:00:00.00	4250.00	0.164699423552018
        ...
2000-08-18 00:00:00.00	206.00	49.9313752401867
2000-08-20 00:00:00.00	206.00	49.958825144112
2002-03-27 00:00:00.00	206.00	49.9862750480373
2003-11-11 00:00:00.00	206.00	50.0137249519627
2005-04-11 00:00:00.00	206.00	50.041174855888
1997-01-21 00:00:00.00	205.00	50.0686247598133
1997-02-12 00:00:00.00	205.00	50.0960746637387
1997-09-09 00:00:00.00	205.00	50.123524567664
        ...
2005-09-28 00:00:00.00	67.00	99.7804007685973
2000-12-13 00:00:00.00	66.00	99.8078506725226
2005-09-08 00:00:00.00	66.00	99.835300576448
2005-09-29 00:00:00.00	66.00	99.8627504803733
2005-09-09 00:00:00.00	65.00	99.8902003842987
2001-02-07 00:00:00.00	60.00	99.917650288224
2005-01-05 00:00:00.00	60.00	99.9451001921493
2000-12-12 00:00:00.00	58.00	99.9725500960747

         */




        /// <summary>
        /// SummaryHydrograph computes a a statistical hydrograph similar to USGS
        /// 'daily streamflow statistics' except SummarySeries is based 
        /// on user specified exceedance level for each day 
        /// instead of mean daily average
        /// </summary>
        /// <param name="beginningDate">align starting with this date </param>
        public static SeriesList SummaryHydrograph(Series sIn, int[] exceedanceLevels, 
            DateTime beginningDate, bool max, bool min, bool avg, bool removeEmptySeries)
        {
            // I'm thinking daily data, but this 
            // may also work for monthly if the day of month is consistent.

            // JR - If series is monthly, change dates such that the values are assigned 
            // to the beginning of each month. This is a work-around to having the leap days
            // being thrown out for dates with an end-of-month convention or for series with
            // irregular observations dates per month
            Series s = new Series();
            if (sIn.TimeInterval == TimeInterval.Monthly)
            {
                s = sIn.Clone();
                foreach (var row in sIn)
                {
                    s.Add(new DateTime(row.DateTime.Year, row.DateTime.Month, 1,
                        0, 0, 0), row.Value, row.Flag);
                }
            }
            else
            { s = sIn; }

            SeriesList eList = new SeriesList();
            Series smax = null;
            Series smin = null;
            Series savg = null;
            if (max)
            {
                smax = CreateSummarySeries(s, "max");
            }
            if (min)
            {
                smin = CreateSummarySeries(s, "min");
            }
            if (avg)
            {
                savg = CreateSummarySeries(s, "avg");
            }


            for (int i = 0; i < exceedanceLevels.Length; i++)
            {
                string txt = exceedanceLevels[i].ToString() + "%";
                Series s1 = CreateSummarySeries(s, txt);
                eList.Add(s1);
            }

            SeriesList subsetsByDay = SubsetEachDay(s,beginningDate);
            DateTime t = beginningDate.AddDays(-1);
            for (int si = 0; si < subsetsByDay.Count; si++)
            {
                Series dailySeries = subsetsByDay[si];

                // Handles time-series data to return an hourly time-series for an hourly dataset
                if (s.TimeInterval == TimeInterval.Hourly) 
                { t = t.AddHours(1); }
                else { t = t.AddDays(1); }
                
                if (dailySeries.Count == 0
                    || t.Day == 29 && t.Month == 2) // skip feburary 29
                {
                    continue;
                }

                Series sorted = Math.Sort(dailySeries, RankType.Weibul);
                //sorted.RemoveMissing();
                if (sorted.Count > 0)
                {
                    if (max)
                        AddToSeriesAtDate(smax, sorted[0], t);
                    if (min)
                        AddToSeriesAtDate(smin, sorted[sorted.Count - 1], t);
                    if (avg)
                    {
                        double avgSorted = Math.AverageOfSeries(sorted);
                        savg.Add(t, avgSorted);
                    }
                }
               
                for (int i = 0; i < exceedanceLevels.Length; i++)
                {
                  Point pt =  InterpolateExceedanceLevel( sorted.Table, exceedanceLevels[i]);
                  AddToSeriesAtDate(eList[i], pt, t);
                }
            } 

            SeriesList rval = new SeriesList();
            if (max)
            {
                rval.Add(smax);
            }
            foreach (Series e in eList)
            {
                if (removeEmptySeries && e.CountMissing() == e.Count)
                {
                    Logger.WriteLine("empty series in Summary Hydrograph " + e.Name);
                }
                else
                {
                    rval.Add(e);
                }
            }

            if (min)
            {
                rval.Add(smin);
            }

            if (avg)
            {
                rval.Add(savg);
            }

            if (s.TimeInterval == TimeInterval.Monthly)
            {
                rval.RemoveMissing();
            }
            return rval;
        }

        /// <summary>
        /// Create a series for each day in a year starting at t1
        /// Used for SummaryHydrographs
        /// </summary>
        private static SeriesList SubsetEachDay(Series s,
            DateTime t1)
        {
            SeriesList rval = new SeriesList();
            Dictionary<string, Series> dict = 
                         new Dictionary<string, Series>();
            DateTime t = t1;
            DateTime t2 = t1.AddYears(1).AddDays(-1);
            string key = "";

            // Handles time-series data to return an hourly time-series for an hourly dataset
            if (s.TimeInterval == TimeInterval.Hourly)
            {
                do
                {
                    key = t.ToString("MM-dd hh-tt");
                    Series mySeries = s.Clone();
                    rval.Add(mySeries);
                    dict.Add(key, mySeries);
                    t = t.AddHours(1);
                } while (t <= t2);

                for (int i = 0; i < s.Count; i++)
                {
                    key = s[i].DateTime.ToString("MM-dd hh-tt");
                    if (!dict.ContainsKey(key))
                    {
                        Console.WriteLine("missing key " + key + " " + s[i]);
                    }
                    else
                    {
                        dict[key].Add(s[i]);
                    }
                }
            }
            else
            {
                do
                {
                    key = t.ToString("MM-dd");
                    Series mySeries = s.Clone();
                    rval.Add(mySeries);
                    dict.Add(key, mySeries);
                    t = t.AddDays(1);
                } while (t <= t2);

                for (int i = 0; i < s.Count; i++)
                {
                    key = s[i].DateTime.ToString("MM-dd");
                    if (!dict.ContainsKey(key))
                    {
                        Console.WriteLine("missing key " + key + " " + s[i]);
                    }
                    else
                    {
                        dict[key].Add(s[i]);
                    }
                }
            }
            //// remove feb 29 
            //key = "02-29";
            //if (dict.ContainsKey(key))
            //{
            //    Series sfeb29 = dict[key];
            //    dict.Remove(key);
            //    rval.Remove(sfeb29);
            //}
            return rval;
        }

        private static void AddToSeriesAtDate(Series s, Point pt, DateTime t)
        {
            Point pt2 = new Point(t, pt.Value, pt.Flag);
            s.Add(pt2);
        }

        private static Series CreateSummarySeries(Series template, string legendText)
        {
            Series s1 = template.Clone();
            s1.Appearance.LegendText = template.Appearance.LegendText+" "+ legendText;
            s1.Provider = template.Provider;
            s1.Name = template.Name;
            return s1;
        }

        /// <summary>
        /// Interpolate a Point in the series at the percent specified.
        /// If extrapolation is necessary a missing value point will be returned.
        /// The nearest date is recored as part of the point
        /// </summary>
        private static Point InterpolateExceedanceLevel(DataTable  tbl,double percent)
        {
            Point pt = new Point(DateTime.MinValue, Point.MissingValueFlag, PointFlag.Missing,percent);

            int sz = tbl.Rows.Count;
            if (sz > 0)
            {
                double min = Convert.ToDouble(tbl.DefaultView[0]["percent"]);
                double max = Convert.ToDouble(tbl.DefaultView[sz-1]["percent"]);

                if (percent >= min && percent <= max)
                {
                    int idx = -1;
                    double interp = Interpolate(tbl, percent, "percent", tbl.Columns[1].ColumnName,out idx);

                     DateTime date = Convert.ToDateTime(tbl.DefaultView[idx][0]);
                     pt = new Point(date, interp, "Interpolated",percent);
                }
            }
                

            return pt;
        }


        public static double Interpolate(DataTable tbl, double xValue,
                                   string xColumnName,
                                   string yColumnName
                                   )
        {
            int idx;
            return Interpolate(tbl,xValue,xColumnName,yColumnName,out idx);
        }
        /// <summary>
        /// Linearly Interpolates y value from a DataTable 
        /// sorted based on your x values.
        /// </summary>
        /// <param name="tbl"></param>
        /// <param name="x_value">interpolate at this x value</param>
        /// <param name="xColumnName">name of column that contains x values</param>
        /// <param name="yColumnName">name of column that contains y values</param>
        /// <param name="nearestIndex">index to row nearest to x_value in your DataTable </param>
        /// <returns></returns>
        public static double Interpolate(DataTable tbl, double x_value,
                                    string xColumnName ,
                                    string yColumnName,
                                    out int nearestIndex
                                    )
        {
            nearestIndex = -1;
            if (tbl.Rows.Count == 0)
            {
                throw new ArgumentException("Interpolate can not work with an empty DataTable");
            }

            DataView rows = tbl.DefaultView;
            
            int n = rows.Count;

            double currentX = Convert.ToDouble(rows[0][xColumnName]);
            double previousX = currentX;
            double maxX = Convert.ToDouble( rows[n-1][xColumnName]);

            if (x_value == currentX) // first value in table matches.
            {
                nearestIndex = 0;
                return Convert.ToDouble(tbl.Rows[0][yColumnName]);
            }

            if (    x_value > maxX || x_value < currentX )
            {
                string msg = "Cannot interpolate " + xColumnName + "=" + x_value + " it is out of the range of the input DataTable";
                Console.WriteLine(msg);
                //cbp.Utility.Write(tbl);
                throw new ArgumentOutOfRangeException(msg);
            }
            int x_pos = 0;

            do
            {
                x_pos++;
                if (x_pos >= n || previousX > currentX)
                {
                    string msg = "Interpolate failed!  is your DataTable sorted on the " + xColumnName + " column";
                    msg += "\n x_value = " + x_value + " x_pos = " + x_pos;
                    Console.WriteLine(msg);
                    //cbp.Utility.Write(tbl);
                    throw new InvalidOperationException(msg);
                }
                previousX = currentX;
                currentX = Convert.ToDouble(rows[x_pos][xColumnName]);
            }
            while (x_value > currentX );

            if (x_value == currentX)
            {
                nearestIndex = x_pos;
                return Convert.ToDouble(rows[x_pos][yColumnName]);
            }

            double percent = (x_value - previousX) / (currentX - previousX);
            if (percent >= 0.5)
            {
                nearestIndex = x_pos;
            }
            else
            {
                nearestIndex = x_pos -1;
            }

            double y = Convert.ToDouble(rows[x_pos][yColumnName]);
            double ym1 = Convert.ToDouble(rows[x_pos-1][yColumnName]);
 
            if ( Point.IsMissingValue( y) || Point.IsMissingValue( ym1))
            {
                return Point.MissingValueFlag;
            }

            return ((1.0 - percent) * ym1 + percent * y);

        }

        [FunctionAttribute("Computes a average by increment of 7 days.  If your data is instantaneous a daily average will first be computed",
         "WeeklyAverageSimple(series1)")]
        /// <summary>
        /// Computes Weekly average. The first average is the 7th day in the series,
        /// and then each 7th day.
        /// </summary>
        /// <param name="daily"></param>
        /// <returns></returns>
        public static Series WeeklyAverageSimple(Series s)
        {

            Series daily = s;
            if (s.TimeInterval == TimeInterval.Irregular || s.TimeInterval == TimeInterval.Hourly) 
               daily = Math.DailyAverage(s);

            if (daily.TimeInterval != TimeInterval.Daily)
                throw new ArgumentException("TimeInterval must be Daily for WeeklyAverage calculation");
            Series weekly = daily.Clone();
            weekly.Name = "Weekly average of " + daily.Name;
            weekly.TimeInterval = TimeInterval.Weekly;

            daily.RemoveMissing();
            if (daily.Count == 0)
                return weekly;

            DateTime t = daily[0].DateTime;

            while (t < daily.MaxDateTime)
            {
                weekly.Add(SumWeek(daily, t));
                t = t.AddDays(7);
            }
            return weekly;
        }

        private static Point SumWeek(Series s, DateTime t)
        {
            Point rval = new Point();
            DateTime t2 = t.Date.AddDays(6).AddHours(23).AddMinutes(59).AddSeconds(59.9);

            Series week = Math.Subset(s,new DateRange(t,t2));
            //Series week = Math.Subset(s, new DateRange(t, t.AddDays(7)));
            Logger.WriteLine("subset for week has "+week.Count + " points ");
            rval.DateTime = t2;
            rval.Value = Math.AverageOfSeries(week);
            rval.Flag = PointFlag.Computed;

            return rval;
        }

                
        /// <summary>
        /// Creates a Series which contains the number of days in the month
        /// for each point in the series.
        /// Example use is to convert from average flow to volume
        /// </summary>
        /// <returns></returns>
        [FunctionAttribute("Creates a Series which contains the number of days in the month",
            "DaysInMonth(series)")]
        public static Series DaysInMonth(Series s)
        {
            Series rval = s.Clone();
            for (int i = 0; i < s.Count; i++)
            {
                DateTime t = s[i].DateTime;
                rval.Add(t, DateTime.DaysInMonth(t.Year, t.Month));
            }
            return rval;
        }

        [FunctionAttribute("Computes a monthly average across all years. result is 12 values.", "MonthlySummaryForPeriodOfRecord(daily)")]
        public static Series MonthlySummaryForPeriodOfRecord(Series daily)
        {

            Series s = new Series();
            s.TimeInterval = TimeInterval.Monthly;

            for (int m = 1; m <=12; m++)
			{
                DateTime t = new DateTime(2000, m, 1);
			 var x = Math.Subset(daily,new int[]{m});
             var avg = Math.AverageOfSeries(x);
             s.Add(t, avg);
			}


            return s;
        }

         [FunctionAttribute("Computes an average by month","MonthlyAverage(daily)")]
        public static Series MonthlyAverage(Series daily)
        {
         //  return Math.Average(daily, TimeInterval.Monthly);
           return MonthlyValues(daily, Math.AverageOfSeries);
        }

         [FunctionAttribute("Computes a monthly sum", "MonthlySum(daily)")]
         public static Series MonthlySum(Series daily)
         {
             return MonthlyValues(daily, Math.Sum);
         }

         //[FunctionAttribute("Computes monthly value from Hydromet", "HydrometMonthlyCalculator(monthlyCbtt,monthlyPcode)")]
         //public static Series HydrometMonthlyCalculator(string monthlyCbtt, string monthlyPcode)
         //{
         //    return new Reclamation.TimeSeries.Hydromet.HydrometMonthlyCalculator(monthlyCbtt, monthlyPcode);
         //}

        /// <summary>
        /// Returns the last value in each month.
        /// </summary>
        /// <param name="daily">daily data</param>
        /// <returns></returns>
        [FunctionAttribute(" Returns the last value in each month.", "EndOfMonth(daily)")]
         public static Series EndOfMonth(Series daily)
         {
             var rval = new Series();
             rval.TimeInterval = TimeInterval.Monthly;
             if (daily.Count == 0)
                 return rval;

             DateTime t = daily.MinDateTime.EndOfMonth();

             while (t <= daily.MaxDateTime)
             {
                 int idx = daily.IndexOf(t);
                 if (idx >= 0)
                 {
                     var pt = daily[idx];
                     rval.Add(pt.DateTime.FirstOfMonth(),pt.Value,pt.Flag);
                 }
                 else
                 {
                     rval.AddMissing(t.FirstOfMonth());
                 }

                 t = t.AddMonths(1).EndOfMonth();
             }

             return rval;
         }

         /// <summary>
         /// 
         /// </summary>
         /// <param name="daily"></param>
         /// <returns></returns>
         internal static Series StartOfMonth(Series daily)
         {
             var rval = new Series();
             if (daily.Count == 0)
                 return rval;

             DateTime t = daily.MinDateTime.FirstOfMonth(); 

             while (t <= daily.MaxDateTime)
             {
                 int idx = daily.IndexOf(t);
                 if (idx >= 0)
                     rval.Add(daily[idx]);
                 else
                     rval.AddMissing(t);

                 t = t.AddMonths(1).FirstOfMonth();
             }

             return rval;
         }

        /// <summary>
        /// Computes monthly values
        /// </summary>
        /// <param name="daily"></param>
        /// <param name="operation"></param>
        /// <returns></returns>
        internal static Series MonthlyValues(Series daily, Func<Series,double> operation)
        {
            
            Series monthly = new Series();
            monthly.TimeInterval = TimeInterval.Monthly;

            if (daily.Count == 0)
                return monthly;

            if (daily.TimeInterval == TimeInterval.Monthly)
                return daily;

            DateTime t = daily.MinDateTime.Date.FirstOfMonth();

            DateTime t2 = daily.MaxDateTime.EndOfMonth();
            while (t <= t2)
            {
                DateRange dr = new DateRange(t.FirstOfMonth(),
                                  new DateTime(t.Year, t.Month,
                                   DateTime.DaysInMonth(t.Year, t.Month), 23, 59, 59)
                                   );


                Series subset = Math.Subset(daily, dr);
                int missing = dr.Count - subset.Count;
 
                if( missing == 0)
                    missing = subset.CountMissing();

                string flag = "";
                double val = Point.MissingValueFlag;
                if (missing > 0)
                {
                    flag = missing + " records missing";
                }
                else
                {
                    val = operation(subset);
                    flag = PointFlag.Computed;
                }

                monthly.Add(t.FirstOfMonth(), val, flag);


                t = t.AddMonths(1).EndOfMonth();
                
            }

            return monthly;
        }




        /// <summary>
        /// computes a single year series that represents the average
        /// for each day in the range between t1 and t2
        /// typically used to compute a 30 year average
        /// Required:  Daily Data
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="beginningMonth"></param>
        /// <returns></returns>
        public static Series MultiYearDailyAverage(Series s, int beginningMonth)
        {

            if (s.TimeInterval != TimeInterval.Daily)
            {
                Logger.WriteLine("MultiYearDailyAverage requires daily data ");
                return new Series();
            }

            Series rval = s.Clone();

            s.RemoveMissing();
            int yrs = rval.Count / 365;


            rval.Provider = "Series";
            rval.HasFlags = true;

            rval.Table.Columns.Add("Count", typeof(int));
            rval.Table.Columns.Add("sum", typeof(double));
            rval.Appearance.LegendText = yrs + " year avg of " + rval.Name;

            YearRange yr = new YearRange(2000, beginningMonth);
            var dateIndex = new List<string>();
            DateTime t = yr.DateTime1;
            while (t <= yr.DateTime2)
            {// fill in empty table with just dates and zeros
                if (t.Month == 2 && t.Day == 29)
                {
                    t = t.AddDays(1);
                    continue; // skip feb 29 so average looks better.
                }
                DataRow r = rval.Table.NewRow();
                r[0] = t;
                r[1] = Point.MissingValueFlag;
                r[2] = PointFlag.Missing;
                r["Count"] = 0;
                r["sum"] = 0;
                rval.Table.Rows.Add(r);
                dateIndex.Add(t.ToString("MMdd"));
                t = t.AddDays(1);
            }


            // process each point 
            for (int i = 0; i < s.Count; i++)
            {
                Point pt = s[i];
                t = pt.DateTime;
                if (t.Month == 2 && t.Day == 29)
                    continue; // skip feb 29 so average looks better.
                if (pt.IsMissing)
                    continue;

                string key = pt.DateTime.ToString("MMdd");
                int idx = dateIndex.IndexOf(key);

                rval.Table.Rows[idx]["Count"] = Convert.ToInt32(rval.Table.Rows[idx]["Count"]) + 1;
                rval.Table.Rows[idx]["Sum"] = Convert.ToDouble(rval.Table.Rows[idx]["Sum"]) + pt.Value;
            }

            // compute average

            for (int i = 0; i < rval.Count; i++)
            {

                int count = Convert.ToInt32(rval.Table.Rows[i]["Count"]);
                if (count == 0)
                    continue;
                double val = Convert.ToDouble(rval.Table.Rows[i]["Sum"]);

                rval.Table.Rows[i][1] = val / count;
                rval.Table.Rows[i][2] = PointFlag.Computed;
            }

            return Math.ShiftToYear(rval, 2000);
        }
        
    }
}
