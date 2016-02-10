using System;
using System.Data;
using Reclamation.Core;
using Reclamation.TimeSeries.Parser;
using System.Linq;

namespace Reclamation.TimeSeries
{
    [Flags]
    public enum StatisticalMethods
    {
        None  = 0,
        Max   = 1,
        Min   = 2,
        Mean  = 4,
        Sum   = 8,
        Count = 16,
        Average =32
    }

    
    /// <summary>
    /// The Math class contains static functions 
    /// that operate on time series data.
    /// </summary>
    public static partial class Math
    {


        public static void ConvertCelciusToF(Series s)
        {
            // convert nrcs C to F
            for (int j = 0; j < s.Count; j++)
            {
                Point p = s[j];
                p.Value = p.Value * 9.0 / 5.0 + 32.0;
                s[j] = p;
            }
        }

        /// <summary>
        /// Converts series to specified units
        /// </summary>
        /// <param name="units"></param>
        public static void ConvertUnits(Series s, string units)
        {
            if (s.Count == 0)
                return;

            if (s.Units == "degrees C" && units == "degrees F")
            {
                for (int i = 0; i < s.Count; i++)
                {
                    Point p = s[i];
                    if (p.IsMissing)
                        continue;
                    p.Value = p.Value * 9.0 / 5.0 + 32.0;
                    s[i] = p;
                }
                s.Units = "degrees F";
                return;
            }

            else if (s.Units.ToLower() == "degrees f" && units.ToLower() == "degrees c")
            {
                for (int i = 0; i < s.Count; i++)
                {
                    Point p = s[i];
                    if (p.IsMissing)
                        continue;
                    p.Value = (p.Value - 32.0) * 5.0 / 9.0;
                    s[i] = p;
                }
                s.Units = "degrees C";
                return;
            }
            else
                if (s.Units == "acre-feet" && units == "cfs")
                {
                    if (s.TimeInterval == TimeInterval.Daily)
                    {
                        Math.Multiply(s, ConvertUnitsFactor(s.TimeInterval, s.Units, units, DateTime.Now));
                        s.Units = "cfs";
                        return;
                    }
                    else
                        if (s.TimeInterval == TimeInterval.Monthly)
                        {
                            for (int i = 0; i < s.Count; i++)
                            {
                                Point pt = s[i];
                                pt = pt * ConvertUnitsFactor(s.TimeInterval, s.Units, units, pt.DateTime);
                                s[i] = pt;

                            }
                            s.Units = "cfs";
                            return;
                        }
                }

            throw new NotImplementedException("cannot convert " + s.TimeInterval.ToString() + " " + s.Units + " to " + units);
        }

       

        public static double ConvertUnitsFactor(TimeInterval type, string fromUnits, string toUnits, DateTime date)
        {
            double rval = 1.0;
            if (fromUnits == "acre-feet" && toUnits == "cfs")
            {
                if (type == TimeInterval.Daily)
                {
                    rval = 1.0 / 1.98347;
                }
                else if (type == TimeInterval.Monthly)
                {
                    int days = DateTime.DaysInMonth(date.Year, date.Month);
                    rval = 1.0 / (1.98347 * days);
                }
                else
                {
                    throw new NotImplementedException("cannot convert " + type.ToString() + " " + fromUnits + " to " + toUnits);
                }
            }
            return rval;
        }


        public static Series Subset(Series s, int month, int day)
        {
            Series rval = s.Clone();
            int sz = s.Count;
            for (int i = 0; i < sz; i++)
            {
                Point pt = s[i];
                if (pt.DateTime.Month == month && pt.DateTime.Day == day)
                {
                    rval.Add(pt);
                }
            }
            return rval;
        }

        /// <summary>
        /// Returns new series that includes data between two points
        /// </summary>
        public static Series Subset(Series s, DateTime t1, DateTime t2)
        {
            Series rval = s.Clone();
            int sz = s.Count;
            for (int i = 0; i < sz; i++)
            {
                Point pt = s[i];
                if (pt.DateTime >= t1 && pt.DateTime <= t2)
                {
                    rval.Add(pt);
                }
            }
            return rval;
        }

        /// <summary>
        /// Returns new series with dates included in a DateRange
        /// </summary>
        public static Series Subset(Series s, MonthDayRange range)
        {
            Series rval = s.Clone();
            int sz = s.Count;
            for (int i = 0; i < sz; i++)
            {
                Point pt = s[i];
                if (range.Contains(pt.DateTime))
                {
                    rval.Add(pt);
                }
            }
            return rval;
        }

        public static Series Subset(Series s, DateRange  dateRange, bool includeEndDateTimes=true)
        {
            string dateColumnName = s.Table.Columns[0].ColumnName;
            Series rval = s.Clone();
            var sql = "";
            if (includeEndDateTimes)
            {
                sql = "[" + dateColumnName + "] >= '" + dateRange.DateTime1 + "'"
                               + " and "
                               + "[" + dateColumnName + "] <= '" + dateRange.DateTime2 + "'"
                               + " ";

            }
            else
            {
                sql = "[" + dateColumnName + "] >= '" + dateRange.DateTime1 + "'"
                              + " and "
                              + "[" + dateColumnName + "] < '" + dateRange.DateTime2 + "'"
                              + " ";
            }

            
            rval = s.Subset(sql);

            return rval;
        }



        /// <summary>
        /// returns subset of series based on months
        /// </summary>
        public static Series Subset(Series s, int[] months)
        {
            Series rval = s.Clone();
            int sz = s.Count;
            for (int i = 0; i < sz; i++)
            {
                Point pt = s[i];
                if (Array.IndexOf(months, pt.DateTime.Month) >= 0)
                {
                    rval.Add(pt);
                }
            }
            return rval;
        }

        /// <summary>
        /// Adds Series a + b
        /// </summary>
        /// <returns></returns>
        internal static Series Add(Series a, Series b)
        {
            return Add(a, b, false);
        }

        internal static Series Add(Series s, double d)
        {
            Series rval = s.Copy();
            for (int i = 0; i < rval.Count; i++)
            {
                Point pt = rval[i];

                if (!pt.IsMissing)
                {
                    pt.Value += d;
                    rval[i] = pt;
                }
            }
            return rval;
        }

        /// <summary>
        /// Subtracts Series a - b
        /// </summary>
        /// <returns></returns>
        internal static Series Subtract(Series a, Series b)
        {
            return Add(a, b, true);
        }

        /// <summary>
        /// s - d
        /// </summary>
        /// <param name="s"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        internal static Series Subtract(Series s, double d)
        {
            Series rval = s.Copy();
            for (int i = 0; i < rval.Count; i++)
            {
                Point pt = rval[i];

                if (!pt.IsMissing)
                {
                    pt.Value -= d;
                    rval[i] = pt;
                }
            }
            return rval;
        }
        /// <summary>
        /// Copies from series b into series a.
        /// Series a is used as a template, each point
        /// in Series a is added to the corresponding point
        /// in series b.
        /// if subtract is true then subtract a - b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="subtract"></param>
        /// <returns></returns>
        private static Series Add(Series a, Series b, bool subtract)
        {
            Series rval = new Series();
            //Series.CopyAttributes(a, rval);
            if (a.TimeInterval != b.TimeInterval)
            {
                throw new InvalidOperationException(a.TimeInterval.ToString() + " and " + b.TimeInterval.ToString() + " are not compatable for add or subtract");
            }
            if (a.Units.ToLower() != b.Units.ToLower())
            {
                Logger.WriteLine("Warning:  '"+a.Units +"' and '"+b.Units +"' are not compatible for the add or subtract calculation");
                //throw new InvalidOperationException("'"+a.Units +"' and '"+b.Units +"' are not compatible for the add or subtract calculation");
            }

            int sign = 1;
            if (subtract)
            {
                sign = -1;
            }
            rval.TimeInterval = a.TimeInterval;
            rval.Units = a.Units;
            for (int i = 0; i < a.Count; i++)
            {
                Point newPoint;
                Point ptA = a[i];
                int idx = b.IndexOf(ptA.DateTime);
                if (
                   idx >= 0 && (!ptA.IsMissing)
                  )
                {
                    Point ptB = b[idx];
                    if (ptB.IsMissing)
                    {
                        newPoint = new Point(ptA.DateTime, Point.MissingValueFlag, PointFlag.Missing);
                    }
                    else
                    {
                        newPoint = new Point(ptA.DateTime, ptA.Value + sign * ptB.Value,PointFlag.Computed);
                    }
                }
                else
                { // missing data.
                    newPoint = new Point(ptA.DateTime, Point.MissingValueFlag, PointFlag.Missing);
                }
                rval.Add(newPoint);
            }

            return rval;
        }




        /// <summary>
        /// Compute a new Time Series based on input series s
        /// and polynomial equation.
        /// 
        /// Returned series has a time range specified by t1 and t2.
        /// Points outside the range of the polynomial equation (min,max)
        /// are flaged as Point.MissingValueFlag
        /// </summary>
        /// <param name="s">input time Series</param>
        /// <param name="polyList">list of polynomial equation</param>
        /// <param name="t1">starting time for new series</param>
        /// <param name="t2">ending time for new series</param>
        /// <returns>Time Series</returns>
        public static Series Polynomial(Series s, PolynomialEquation[] polyList,
          DateTime t1, DateTime t2)
        {
            int sz = s.Count;
            Series rval = s.Clone();
            for (int i = 0; i < sz; i++)
            {
                Point point = s[i];
                if (point.BoundedBy(t1, t2))
                {
                    Point newPt = new Point(point.DateTime, Point.MissingValueFlag, PointFlag.Missing);
                    for (int p = 0; p < polyList.Length; p++)
                    {
                        PolynomialEquation poly = polyList[p];
                        if (point.BoundedBy(t1, t2, poly.Min, poly.Max))
                        {
                            double val = poly.Eval(point.Value);
                            if (val < 0)
                            {

                                Console.WriteLine("What we have a negative number?? " + point + " eval = " + poly.Eval(point.Value));
                                // throw new Exception();
                            }
                            newPt = new Point(point.DateTime, val, PointFlag.Computed);
                            break;
                        }
                    }
                    rval.Add(newPt);
                }
            }
            return rval;
        }

        public static Series Polynomial(Series s, PolynomialEquation[] polyList,
          DateTime[] polyDateList1, DateTime[] polyDateList2,
          DateTime t1, DateTime t2)
        {
            int sz = s.Count;
            Series rval = s.Clone();
            for (int i = 0; i < sz; i++)
            {
                Point point = s[i];
                if (point.BoundedBy(t1, t2))
                {
                    Point newPt = new Point(point.DateTime, Point.MissingValueFlag, PointFlag.Missing);
                    for (int p = 0; p < polyList.Length; p++)
                    {
                        PolynomialEquation poly = polyList[p];

                        if (point.BoundedBy(polyDateList1[p], polyDateList2[p], poly.Min, poly.Max))
                        {

                            double val = poly.Eval(point.Value);
                            if (val < 0)
                            {
                                Console.WriteLine("What we have a negative number?? " + point + " eval = " + poly.Eval(point.Value));
                                // throw new Exception();
                            }
                            newPt = new Point(point.DateTime, val, PointFlag.Computed);
                            break;
                        }
                    }
                    rval.Add(newPt);
                }
            }
            return rval;
        }
        /// <summary>
        /// Compute a new Time Series based on input series s
        /// and polynomial equation.
        /// 
        /// Returned series has a time range specified by t1 and t2.
        /// Points outside the range of the polynomial equation (min,max)
        /// are flaged as Point.MissingValueFlag
        /// </summary>
        /// <param name="s">input time Series</param>
        /// <param name="poly">polynomial equation</param>
        /// <param name="t1">starting time for new series</param>
        /// <param name="t2">ending time for new series</param>
        /// <returns>Time Series</returns>
        public static Series Polynomial(Series s, PolynomialEquation poly,
          DateTime t1, DateTime t2)
        {
            int sz = s.Count;
            Series rval = s.Clone();
            for (int i = 0; i < sz; i++)
            {
                Point point = s[i];

                if (point.BoundedBy(t1, t2))
                {
                    Point newPt;
                    if (point.BoundedBy(t1, t2, poly.Min, poly.Max))
                    {
                        newPt = new Point(point.DateTime, poly.Eval(point.Value), PointFlag.Computed);
                    }
                    else
                    {
                        newPt = new Point(point.DateTime, Point.MissingValueFlag, PointFlag.Missing);
                    }
                    rval.Add(newPt);
                }
            }

            return rval;
        }


        /// <summary>
        /// Interpolates between two dates.
        /// </summary>
        /// <param name="t">date to interpolate at</param>
        /// <param name="t1">date below t</param>
        /// <param name="t2">date greater than t</param>
        /// <param name="y1">y value at t1</param>
        /// <param name="y2">y value at t2</param>
        /// <returns></returns>
        public static double Interpolate(DateTime t, DateTime t1, DateTime t2, double y1, double y2)
        {
            if (t < t1 || t > t2)
            {
                throw new ArgumentOutOfRangeException("interpolate function cannot extrapolate");
            }
            //-- ported to c# from sql from visual basic. which was ported from C++ (karl tarbet) */
            double totalDistance = t2.Ticks - t1.Ticks; //datediff( second, @date0,@date1);
            double distance = t.Ticks - t1.Ticks;//dateDiff(second,@date0,@datex)
            double percent = distance / totalDistance;
            double result = (1.0 - percent) * y1 + percent * y2;

            return result;
        }

        /// <summary>
        /// Interpolate value if there is not an excact match
        /// for t in the series
        /// </summary>
        /// <returns></returns>
        public static double Interpolate(Series s, DateTime t)
        {
            return Interpolate(s, t, false, -1);
        }

        /// <summary>
        /// Interpolate value if there is not an excact match
        /// for t in the series
        /// </summary>
        /// <returns></returns>
        public static double Interpolate(Series s, DateTime t, bool AllowExtrapolate)
        {
            return Interpolate(s, t, AllowExtrapolate, -1);
        }
        /// <summary>
        /// Interpolate value at specified time.
        /// </summary>
        /// <param name="s">Series containing values to interpolate from</param>
        /// <param name="t">DateTime to interpolate at</param>
        /// <param name="AllowExtrapolate">Allow extrapolation outside Series s</param>
        /// <param name="index">index to t or next index in series</param>
        /// <returns></returns>
        public static double Interpolate(Series s, DateTime t, bool AllowExtrapolate, int index)
        {
            if (!s.WithinRange(t) && !AllowExtrapolate)
            {
                string msg = "can not interpolate outside range (" + s.MinDateTime + " --> " + s.MaxDateTime
                  + ")\n  requested date is " + t;

                throw new ArgumentOutOfRangeException(msg);
            }
            int currentIndex = 0;
            if (index >= 0) // use input index to save lookup.
            {
                currentIndex = index;
            }
            else
            {
                currentIndex = s.LookupIndex(t);
            }

            int i = currentIndex;
            if (i < 0)
            {  // must allow [i-1]
                // this should not happen since we called WithinRange() above
                throw new Exception("cannot interpolate with outside array bounds.  error#2");
            }

            Point point = s[i];

            // first test for exact match.
            if (point.DateTime == t)
            {
                return point.Value;
            }


            Point pointM1 = s[i - 1];

            if (point.IsMissing || pointM1.IsMissing)
            {
                throw new Exception("Error: cannot interpolate when point is marked as missing");
            }

            if (point.DateTime >= t)
            {
                if (point.DateTime == t)
                {
                    return point.Value;
                }
                // interpolate.
                double y2 = System.Convert.ToDouble(point.Value);
                double y1 = System.Convert.ToDouble(pointM1.Value);
                DateTime date1 = System.Convert.ToDateTime(pointM1.DateTime);
                double rval = Interpolate(t, date1, point.DateTime, y1, y2);
                return rval;
            }

            if (true)
            {
                Console.WriteLine("Error:  cannot interpolate");
                throw new Exception("cannot interpolate error #3 did not find proper time. ");
            }
            //	return 0;
        }

        /// <summary>
        /// moves all values in selection to specified value.
        /// can be useful in two part editing.
        /// first: move many outliers to a known datum (using MoveTo)
        /// second: manually move points one by one (using point move feature of Series Graph)
        /// </summary>
        public static void MoveTo(Series s, Selection selected, double newValue)
        {
            if (s.ReadOnly)
            {
                return;
            }
            for (int i = 0; i < s.Count; i++)
            {
                if (s[i].BoundedBy(selected))
                {
                    Console.WriteLine("moving ");

                    Point pt = s[i];
                    Console.WriteLine(pt);
                    pt.Value = newValue;
                    pt.Flag = "Edited";
                    Console.WriteLine("to ");
                    Console.WriteLine(pt);
                    s[i] = pt;
                }
            }

        }
        /// <summary>
        /// Applies vertical offset to data in selection
        /// </summary>
        public static void Offset(Series s, Selection selected, double verticalOffset)
        {
            if (s.ReadOnly)
            {
                return;
            }
            for (int i = 0; i < s.Count; i++)
            {
                if (s[i].BoundedBy(selected))
                {
                    Point pt = s[i];
                    pt.Value = pt.Value + verticalOffset;
                    pt.Flag = "Edited";
                    s[i] = pt;
                }
            }
        }

        /// <summary>
        /// Shift the series in time, keeping the month and day 
        /// </summary>
        private static Series Shift(Series s, TimeSpan ts)
        {
            Series rval = s.Clone();
            for (int i = 0; i < s.Count; i++)
            {
                Point pt = s[i];
                pt.Flag = PointFlag.Edited;
                
                var t2 = pt.DateTime.Add(ts);

                if (!DateTime.IsLeapYear(t2.Year) 
                    && pt.DateTime.Month == 2 
                    && pt.DateTime.Day == 29)
                {
                    ts = ts.Add(TimeSpan.FromDays(-1));            
                    continue;
                }

                if (!DateTime.IsLeapYear(pt.DateTime.Year)
                    && t2.Month == 2
                    && t2.Day == 29)
                {
                    Point Feb28 = s[i - 1];                    
                    Feb28.DateTime = t2;
                    rval.Add(Feb28);
                    ts = ts.Add(TimeSpan.FromDays(1));
                    t2 = pt.DateTime.Add(ts);
                }


                if (pt.DateTime.Day != t2.Day
                    || pt.DateTime.Month != t2.Month)
                {
                    throw new System.ArgumentException("Error month and day should always be the same");

                }

                pt.DateTime = t2;
                rval.Add(pt);
            }
            return rval;
        }

        /// <summary>
        /// Shift the series in time 
        /// for Daily series timeOffset represents days
        /// for Monthly series timeOffset represntes months
        /// </summary>
        public static Series Shift(Series s, int timeOffset)
        {
            
            Series rval = s.Clone();
            if (timeOffset == 0)
                return s.Copy();

            for (int i = 0; i < s.Count; i++)
            {
                Point pt = s[i];
                pt.Flag = PointFlag.Edited;
                if (s.TimeInterval == TimeInterval.Daily)
            {
                    pt.DateTime = pt.DateTime.AddDays(timeOffset);
            }
                else if (s.TimeInterval == TimeInterval.Monthly)
                {// maintain end of month or first of month
                    bool eom = pt.DateTime.EndOfMonth() == pt.DateTime;
                    bool fom = pt.DateTime.FirstOfMonth() == pt.DateTime;

                    pt.DateTime = pt.DateTime.AddMonths(timeOffset);

                    if (eom)
                        pt.DateTime = pt.DateTime.EndOfMonth();

                    if (fom)
                        pt.DateTime = pt.DateTime.FirstOfMonth();
                }
                else
                {
                    throw new NotImplementedException("time offset not implemented for "+s.TimeInterval);
                }
                
                rval.Add(pt);
            }
            return rval;
        }


        /// <summary>
        /// usefull for comparing different periods of time.  
        /// Often the period of time is one year.
        /// Modelers may model different years as scenarios.
        /// </summary>
        public static Series ShiftToYear(Series s, int year)
        {
            Series wySeries = s.Clone();
            if (s.Count != 0)
            {
                int day = s[0].DateTime.Day;
                int month = s[0].DateTime.Month;
                DateTime t1;
                if (day == 29 && month == 2 && !DateTime.IsLeapYear(year))
                {
                    day = 28;
                }
                t1 = new DateTime(year, month, day);
                
                TimeSpan ts = new TimeSpan(t1.Ticks - s[0].DateTime.Ticks);

                if (s.TimeInterval == TimeInterval.Monthly)
                {
                    int yearOffset = year - s[0].DateTime.Year;
                    for (int i = 0; i < s.Count; i++)
                    {
                        DateTime t = s[i].DateTime;
                        if (t.Month == 2 && t.Day == 29 && !DateTime.IsLeapYear(t.Year + yearOffset))
                            t = t.AddDays(-1);

                        DateTime newDate = new DateTime(t.Year+yearOffset, t.Month, t.Day);
                        wySeries.Add(newDate, s[i].Value, s[i].Flag);
                    }
                }
                else
                {
                    wySeries = Math.Shift(s, ts);
                    
                }
               // wySeries.Table.AcceptChanges();
                Series.CopyAttributes(s, wySeries);
            }
            return wySeries;
        }

        

        /// <summary>
        ///  multiply each point in series by factor
        /// </summary>
        /// <param name="s"></param>
        /// <param name="factor"></param>
        public static void Multiply(Series s, double factor)
        {
            for (int i = 0; i < s.Count; i++)
            {

                Point pt = s[i];
                if (pt.IsMissing)
                {
                    continue;
                }
                pt.Value = pt.Value * factor;
                pt.Flag = "Edited";
                s[i] = pt;
            }
        }

        /// <summary>
        /// multiply series point by point with another series
        /// can be used when one series is a percentage
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Series Multiply(Series a, Series b)
        {
            return MultiplyOrDivide(a, b, false);
        }

        public static Series Divide(Series a, Series b)
        {
            return MultiplyOrDivide(a, b, true);
        }



         private static Series MultiplyOrDivide(Series a, Series b, bool division=false)
             {
            Series rval = new Series();
            //Series.CopyAttributes(a, rval);
            if (a.TimeInterval != b.TimeInterval)
            {
                throw new InvalidOperationException(a.TimeInterval.ToString() + " and " + b.TimeInterval.ToString() + " are not compatable for add or subtract");
            }

            rval.TimeInterval = a.TimeInterval;
            rval.Units = a.Units;
            for (int i = 0; i < a.Count; i++)
            {
                Point newPoint;
                Point ptA = a[i];
                int idx = b.IndexOf(ptA.DateTime);
                if (
                   idx >= 0 && (!ptA.IsMissing)
                  )
                {
                    Point ptB = b[idx];
                    if (ptB.IsMissing)
                    {
                        newPoint = new Point(ptA.DateTime, Point.MissingValueFlag, PointFlag.Missing);
                    }
                    else
                    {
                        if (division)
                        {
                            if( ptA.Value == 0)
                                newPoint = new Point(ptA.DateTime, Point.MissingValueFlag, PointFlag.Missing);
                            else
                            newPoint = new Point(ptA.DateTime, ptA.Value / ptB.Value, PointFlag.Computed);
                        }
                        else
                        {
                            newPoint = new Point(ptA.DateTime, ptA.Value * ptB.Value, PointFlag.Computed);
                        }
                    }
                }
                else
                { // missing data.
                    newPoint = new Point(ptA.DateTime, Point.MissingValueFlag, PointFlag.Missing);
                }
                rval.Add(newPoint);
            }

            return rval;

        }

        /// <summary>
        /// convert each double value to an integer
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Series Round(Series s)
        {
            Series rval = s.Clone();
            for (int i = 0; i < s.Count; i++)
            {
                Point pt = s[i];
                pt.Value = System.Math.Round(pt.Value);
                rval.Add(pt);
            }
            return rval;
        }




        /// <summary>
        /// Routes input series using lag coefficients
        /// </summary>
        /// <param name="s"></param>
        /// <param name="lagCoefficients"></param>
        public static Series RoutingWithLags(Series s, double[] lagCoefficients)
        {
            double[] result = new double[s.Count];

            Series rval = s.Copy();
            int sz = s.Count;
            Math.Multiply(rval, 0);
            Series.CopyAttributes(s, rval);

            for (int i = 0; i < s.Count; i++)
            {
                double v = s[i].Value;

                for (int j = 0; j < lagCoefficients.Length && i+j <sz; j++)
                {
                    result[i + j] += v * lagCoefficients[j];
                }    
            }

            // put results back into Series format.
            rval.Values = result;
            return rval;
        }



        /// <summary>
        /// Fills in missing data in the series with zeros.
        /// adds rows if necessary
        /// </summary>
        /// <param name="series"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        public static Series FillMissingWithZero(Series s, DateTime t1, DateTime t2)
        {
            Series rval = s.Copy();
            Logger.WriteLine("FillMissingWithZero("+rval.Name+")");
            Logger.WriteLine("before fill s.Count=" + rval.Count);
            if (rval.TimeInterval != TimeInterval.Daily && rval.TimeInterval != TimeInterval.Monthly
                && rval.TimeInterval != TimeInterval.Hourly)
            {
                Logger.WriteLine(rval.TimeInterval.ToString() + " not supported in FillMissingWithZero");
                return rval;
            }

            if (t2 <= t1)
                return rval;

            DateTime t = t1;
            while (t <=t2)
            {
                int idx = rval.IndexOf(t);
                if (idx < 0)
                {
                    rval.Add(t, 0);
                }
                else
                {
                    var pt = rval[idx];
                    if (pt.IsMissing)
                    {
                        pt.Value = 0;
                        pt.Flag = PointFlag.Estimated;
                        rval[idx] = pt;
                    }
                }

                t = rval.IncremetDate(t);
            }
            Logger.WriteLine("after fill s.Count=" + rval.Count);
            return rval;
        }


        internal static Series Pow(Series s, double p)
        {

            Series rval = s.Clone();
            Series.CopyAttributes(s, rval);

            for (int i = 0; i < s.Count; i++)
            {
                Point pt = s[i];

                if (pt.IsMissing)
                {
                    rval.AddMissing(pt.DateTime);
                }
                else
                {
                    double x = System.Math.Pow(pt.Value, p);
                    rval.Add(pt.DateTime, x);
                }
            }
            return rval;
        }

        /// <summary>
        /// Function that averages a series based on a defined Time-Interval
        /// </summary>
        /// <param name="s">Input Series</param>
        /// <param name="tInterval">Averaging Time-Interval</param>
        /// <returns></returns>
        public static Series Average(Series s, TimeInterval tInterval)
        {
            Series rval = s.Clone();
            if (s.Count == 0)
                return rval;

            // Define starting date of averaging process
            DateTime t = new DateTime(s[0].DateTime.Year, s[0].DateTime.Month, s[0].DateTime.Day,
                s[0].DateTime.Hour, 0, 0);

            // Find which averaging process to accomplish
            if (tInterval == TimeInterval.Daily)
            {
                // Define series time-interval
                rval.TimeInterval = TimeInterval.Daily;
                // Loop through the dates
                while (t < s[s.Count - 1].DateTime.Date)
                {
                    // Get the series subset based on the averaging time-interval
                    Series sTemp = s.Subset(t, t.AddDays(1));
                    // Average the values of the subset
                    DoAverage(rval, t, sTemp);
                    // Increment DateTime by averaging time-interval
                    t = t.AddDays(1);
                }
            }
            // Ditto on the other processes below
            else if (tInterval == TimeInterval.Monthly)
            {
                rval.TimeInterval = TimeInterval.Monthly;
                while (t < s[s.Count - 1].DateTime.Date)
                {
                    Series sTemp = s.Subset(t, t.AddMonths(1));
                    DoAverage(rval, t, sTemp);
                    t = t.AddMonths(1);
                }
            }
            else if (tInterval == TimeInterval.Hourly)
            {
                rval.TimeInterval = TimeInterval.Hourly;
                while (t < s.MaxDateTime)
                {
                    
                    Series sTemp = Math.Subset(s, new DateRange(t, t.AddHours(1)), false);
                    DoAverage(rval, t, sTemp);
                    t = t.AddHours(1);

                }
            }
            else
            { throw new Exception("Time Interval " + tInterval.ToString() + " not supported!"); }

            return rval;
        }

        private static void DoAverage(Series sOut, DateTime t, Series sTemp)
        {
            if (sTemp.Count == 0)
                sOut.AddMissing(t);
            else
            {
                double avgVal = sTemp.Values.Average();
                sOut.Add(t, avgVal);
            }
        }

       /// <summary>
       /// Compares series 1 to series2
       /// </summary>
        internal static Series Compare(Series series1, Series series2, Func<double,double,double> f)
        {
            Series rval = series1.Copy();
            for (int i = 0; i < rval.Count; i++)
            {
                Point pt = rval[i];
                var idx2 =  series2.IndexOf(pt.DateTime);
                var missing = new Point(pt.DateTime, Point.MissingValueFlag, PointFlag.Missing);
                if (pt.IsMissing || idx2 < 0)
                {
                    pt = missing;
                }
                else if (idx2 >= 0 && series2[idx2].IsMissing)
                {
                    pt = missing;
                }
                else
                {
                    pt.Value = f(pt.Value,series2[idx2].Value);
                }

                rval[i] = pt;
            }
            return rval;
        }

        internal static Series Compare(Series series, double p, Func<double,double,double> f)
        {
            Series rval = series.Copy();
            for (int i = 0; i < rval.Count; i++)
            {
                Point pt = rval[i];
                var missing = new Point(pt.DateTime, Point.MissingValueFlag, PointFlag.Missing);
                if (pt.IsMissing  || p == Point.MissingValueFlag)
                {
                    pt = missing;
                }
                else
                {
                  pt.Value = f(pt.Value,p);          
                }

                rval[i] = pt;
            }
            return rval;
        }
    }
}
