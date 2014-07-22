using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Reclamation.Core;
using Reclamation.TimeSeries.Parser;
using Reclamation.TimeSeries.Estimation;

namespace Reclamation.TimeSeries
{
    public static partial class Math
    {
        /// <summary>
        /// Entry point for disaggregating monthly streamflow using UofI's method
        /// </summary>
        /// <param name="daily">Observed daily data (Mean cfs)</param>
        /// <param name="TSmonth">Observed monthly data to be disaggregated (cfs or acre-feet)</param>
        /// <returns></returns>
        [FunctionAttribute("Disaggregates monthly data to daily cfs values.",
        "UofIStreamflowDisaggregation(DailySeries,MonthlySeries,bool smooth = true,bool merge = false)")]
        public static Series UofIStreamflowDisaggregation(Series daily, Series monthly, bool smooth = true,
            bool merge = false)
        {
            // Check TimeIntervals of input series
            if (daily.TimeInterval != TimeInterval.Daily)
            { 
                throw new ArgumentException("Invalid time interval for series: " + daily.Name); 
            }
            if (monthly.TimeInterval != TimeInterval.Monthly)
            { 
                throw new ArgumentException("Invalid time interval for series: " + monthly.Name); 
            }

            // Assume daily data is "observed", remove flags, in MergeCheckMassBalance we're
            // looking for data that has a Computed flag to check mass balance
            Series tsdaily = ClearFlagsKeepMissingFlag(daily);
            
            // Disaggregation
            Series s = RMSEInterp(tsdaily, monthly);
            s.Name = "Disaggregated_" + monthly.Name;
            s.Units = "cfs";
            s.TimeInterval = TimeInterval.Daily;
            
            // Spline Interpolation
            if (smooth)
            {
                SplineInterpUofI(s);
            }
            
            // Merge disaggregated series with the input daily series
            if (merge)
            {
                MergeCheckMassBalance(tsdaily, s);
            }

            // If dissagregated data ends before daily, append daily, if merging
            if (merge && s.MaxDateTime < tsdaily.MaxDateTime)
            {
                Append(tsdaily, s);
            }

            return s;
        }

        


        
        /// <summary>
        /// Main Streamflow Disaggregation script
        /// Follows procedures laid out by UofI's A.Acharya and Dr.Ryu
        /// </summary>
        /// <param name="daily">daily series (cfs)</param>
        /// <param name="monthly">monthly series (cfs or acre-feet)</param>
        /// <returns></returns>
        public static Series RMSEInterp(Series daily, Series monthly)
        {
            // Generates the monthly totals for the RMS calculations in cu.ft/month
            Series SSmonthTot = MonthSum(MonthlyAverage(daily));
            Series TSmonthTot = MonthSum(ConvertAcreFeetToCfs(monthly));

            // Builds a Series to keep track of the corresponding year with the minimum RMSe
            Series TSrms = RMSEGenerateMatchList(SSmonthTot, TSmonthTot);

            // New series for the estimated daily value
            Series TSdaily = new Series();

            // Loop to calculate the daily estimate
            for (int i = 0; i < TSrms.Count; i++)
            {
                int targetYear = Convert.ToInt16(TSrms[i].Value);
                int sourceYear = TSrms[i].DateTime.Year;
                
                // Leap Years are fun! Catch leap/non-leap year mismatches.
                // If the target is a leap year, this leaves 2-29 empty
                DateTime tLookup;
                if (TSrms[i].DateTime.Month == 2 && (DateTime.IsLeapYear(targetYear) ^ DateTime.IsLeapYear(sourceYear)))
                    tLookup = new DateTime(targetYear, TSrms[i].DateTime.Month, 28);
                else
                    tLookup = new DateTime(targetYear, TSrms[i].DateTime.Month, TSrms[i].DateTime.Day); 

                // Calculates daily ratio of the monthly total for the SS
                double SSmatchMonthly = SSmonthTot.Lookup(tLookup);
                DateTime tStart = new DateTime(targetYear, TSrms[i].DateTime.Month, 1);
                Series SSmatchDaily = TimeSeries.Math.FillMissingWithZero(daily.Subset(tStart,tLookup));
                Series SSratioTemp = SSmatchDaily / SSmatchMonthly;

                // Catches NaN values if the SS monthly data is zero and leap day mass balance problems
                Series SSratio = new Series();
                double leapDayRatio = 0.0;
                for (int x = 0; x < SSratioTemp.Count; x++)
                {
                    Point ptX = SSratioTemp[x];
                    if (ptX.Value.ToString() == "NaN" || SSmatchMonthly == 0.0)
                    {
                        SSratio.Add(ptX.DateTime, 0.0);
                        continue;
                    }
                    // Catches TS leap years and ensures that mass balance is preserved with Feb-29th
                    if (DateTime.IsLeapYear(sourceYear) && !DateTime.IsLeapYear(targetYear) &&
                        SSratio.MinDateTime.Month == 2)
                    {
                        leapDayRatio = leapDayRatio + (ptX.Value / 28.0);
                        SSratio.Add(ptX.DateTime, ptX.Value - (ptX.Value / 28.0));
                    }
                    else if (!DateTime.IsLeapYear(sourceYear) && DateTime.IsLeapYear(targetYear) &&
                        SSratio.MinDateTime.Month == 2)
                    {
                        leapDayRatio = daily[new DateTime(SSratioTemp.MaxDateTime.Year, 2, 29)].Value / SSmatchMonthly;
                        SSratio.Add(ptX.DateTime, ptX.Value + (leapDayRatio / 27.0));
                    }
                    else
                    {
                        SSratio.Add(ptX);
                    }
                }

                // Calculates the estimated daily for the TS given the monthly total and SS ratio
                TSdaily = RMSEGenerateDaily(TSdaily, TSmonthTot.Lookup(TSrms[i].DateTime.Date), SSmatchDaily, SSratio,
                    targetYear, sourceYear, leapDayRatio);
            }

            return TSdaily;
        }


        /// <summary>
        /// This builds a series with the dates from the target series and values that represent the year wherein
        /// the source series has the minimum moving 3-month RMSE
        /// </summary>
        /// <param name="SSmonthTot">Source monthly series</param>
        /// <param name="TSmonthTot">Target monthly series</param>
        /// <returns></returns>
        private static Series RMSEGenerateMatchList(Series SSmonthTot, Series TSmonthTot)
        {
            // Series to keep track of the corresponding year with the minimum RMSe
            Series TSrms = new Series();

            // For loop to generate corresponding month-year from Source with minimum 3 month RMSe
            for (int targetYear = TSmonthTot.MinDateTime.Year; targetYear <= TSmonthTot.MaxDateTime.Year; targetYear++)
            {
                for (int ithMonth = 1; ithMonth <= 12; ithMonth++)// i is the month for calculation evaluation
                {
                    // only estimate data for dates which are in the TS
                    DateTime tTarg = new DateTime(targetYear, ithMonth, DateTime.DaysInMonth(targetYear, ithMonth));
                    
                    if (tTarg >= TSmonthTot.MinDateTime && tTarg <= TSmonthTot.MaxDateTime)
                    {
                        var RMSList = new List<double>();
                        var RMSYear = new List<int>();
                        for (int sourceYear = SSmonthTot.MinDateTime.Year; sourceYear < SSmonthTot.MaxDateTime.Year; sourceYear++)
                        {
                            DateTime tSour = new DateTime(sourceYear, ithMonth, DateTime.DaysInMonth(sourceYear, ithMonth));
                            if (tSour < SSmonthTot.MinDateTime) // makes sure that empty dailies before the daily record starts are not used
                            {
                                sourceYear++;
                                tSour = new DateTime(sourceYear, ithMonth, DateTime.DaysInMonth(sourceYear, ithMonth)); 
                            }
                            // RMSe calculation
                            double mon1 = getRMS(SSmonthTot.Lookup(tSour.AddMonths(-1)), TSmonthTot.Lookup(tTarg.AddMonths(-1)));
                            double mon2 = getRMS(SSmonthTot.Lookup(tSour.AddMonths(0)), TSmonthTot.Lookup(tTarg.AddMonths(0)));
                            // Sets mon3 value for the last month since there is no value for t.AddMonths(1)
                            double mon3;
                            if (tTarg == TSmonthTot.MaxDateTime)
                            { mon3 = 0.0; }
                            else
                            { mon3 = getRMS(SSmonthTot.Lookup(tSour.AddMonths(1)), TSmonthTot.Lookup(tTarg.AddMonths(1))); }
                            double RMSi = System.Math.Sqrt((mon1 + mon2 + mon3) / 3);
                            RMSList.Add(RMSi);
                            RMSYear.Add(sourceYear);
                        }
                        var RMSmin = RMSList.Min();
                        var RMSminYear = RMSYear[RMSList.IndexOf(RMSmin)];
                        TSrms.Add(tTarg, Convert.ToDouble(RMSminYear));
                    }
                }
            }
            return TSrms;
        }


        /// <summary>
        /// This function returns the RMS error between 2 values.
        /// </summary>
        /// <param name="sourceVal"></param>
        /// <param name="targetVal"></param>
        /// <returns></returns>
        private static double getRMS(double sourceVal, double targetVal)
        {
            // Assign a ridiculously large value to penalize missing or zeroed data from the source series
            // this is a hack that prevents the selection of a daily series with no values when there is data
            // for the monthly series
            if (sourceVal == 0.0 || sourceVal == Point.MissingValueFlag)
            {
                sourceVal = -999999999999999.99;
            }
            return System.Math.Pow(sourceVal - targetVal, 2);
        }


        /// <summary>
        /// This builds the daily disaggregated Series target monthly values and the ratio of the source daily values 
        /// on a monthly basis
        /// </summary>
        /// <param name="targetMonthlyValue">Target monthly values</param>
        /// <param name="SSmatchDaily">Source daily values, we get the month and day values from this series</param>
        /// <param name="SSratio">The source daily ratio based on the source monthly sum</param>
        /// <param name="targetYear">Year to be disaggregated from the target series</param>
        /// <param name="sourceYear">Year to be used as a basis for disaggregation from the source series</param>
        /// <param name="leapDayRatio">Incremental value to +/- based on leap year mismatches</param>
        /// <returns></returns>
        private static Series RMSEGenerateDaily(Series TSdaily, double targetMonthlyValue, Series SSmatchDaily, Series SSratio,
            int targetYear, int sourceYear, double leapDayRatio)
        {
            // Calculates the estimated daily for the TS given the monthly total and SS ratio
            Series dailyTemp = SSratio * targetMonthlyValue;
            for (int j = 0; j < SSmatchDaily.Count; j++)
            {
                DateTime t = new DateTime(sourceYear, SSmatchDaily[j].DateTime.Month, SSmatchDaily[j].DateTime.Day);
                TSdaily.Add(t, dailyTemp[j].Value, PointFlag.Computed);
                // if there is a leap year mismatch, add Feb 29th
                if (t.Month == 2 && t.Day == 28 && DateTime.IsLeapYear(t.Year) && !DateTime.IsLeapYear(targetYear))
                {
                    t = new DateTime(sourceYear, SSmatchDaily[j].DateTime.Month, 29);
                    TSdaily.Add(t, leapDayRatio * targetMonthlyValue, PointFlag.Computed);
                }
            }
            return TSdaily;
        }



        /// <summary>
        /// Finds month-end transitions where the difference is past a particular threshold and calls the 
        /// Spline Interpolation procedure
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static void SplineInterpUofI(Series s)
        {
            Series sInterp = new Series();
            double threshold = 0.10;

            // Loop to identify EOM dates that require interpolation
            // Threshold and ratio equation taken from UofI's disaggregation method procedure
            for (DateTime t = s.MinDateTime; t < s.MaxDateTime.AddMonths(-1); t = t.AddMonths(1))
            // Magic number so last month's entry doesn't get evaluated.
            {
                t = new DateTime(t.Year, t.Month, DateTime.DaysInMonth(t.Year, t.Month));
                int eomIDX = s.IndexOf(t);
                double diffRatio = (s[eomIDX].Value - s[eomIDX + 1].Value) / s[eomIDX].Value;

                // Build series of interpolated data points
                if (System.Math.Abs(diffRatio) > threshold)
                {
                    Series sInterpTemp = s.Subset(t.AddDays(-15), t.AddDays(15));
                    sInterp = SplineCalc(sInterpTemp);
                    for (int i = 0; i < sInterp.Count; i++)
                    {
                        Point pt = sInterp[i];
                        s[pt.DateTime] = pt;
                    }
                }
            }
        }



        /// <summary>
        /// Spline Interpolation using a math/stats assembly from http://www.alglib.net/download.php
        /// Interpolation method mimics UofI disaggregation method of taking a 31 day window and interpolating
        /// the values in index numbers 11 - 21
        /// Lots of magic numbers...
        /// </summary>
        private static Series SplineCalc(Series sOrig)
        {
            // Magic numbers to assign index values to the data points
            double[] X = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };
            // Magic numbers for values to be overwritten with interpolated values
            double[] evalpts = new double[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21 };

            // Generates values from the input series corresponding with 'X'
            List<double> tempVals = new List<double>();
            for (int i = 0; i < 10; i++) { tempVals.Add(sOrig[i].Value); }
            for (int i = 21; i < 31; i++) { tempVals.Add(sOrig[i].Value); }
            double[] Y = tempVals.ToArray();

            //This Cubic Spline replicates the Excel output used by UofI's A.Acharya and Dr.Ryu.
            alglib.spline1dinterpolant C = new alglib.spline1dinterpolant();
            var results = new List<double>();
            for (int i = 0; i < evalpts.Length; i++)
            {
                double evalpt = evalpts[i];
                alglib.spline1dbuildcubic(X, Y, out C);
                double s = alglib.spline1dcalc(C, evalpt);
                results.Add(s);
            }

            // Loop to populate interpolated series.
            Series sNewInterp = new Series();
            for (int j = 0; j < sOrig.Count; j++)
            {
                if (j >= 10 && j <= 20)
                {   // Assigns interpolated value to the new series so long as the interpolated value is > 0
                    if (results[j - 10] > 0)
                    { sNewInterp.Add(sOrig[j].DateTime, results[j - 10],sOrig[j].Flag); }
                    else { sNewInterp.Add(sOrig[j]); }
                }
                else { sNewInterp.Add(sOrig[j]); }
            }
            return sNewInterp;
        }



        /// <summary>
        /// Calculates the monthly average for a monthly series
        /// </summary>
        /// <param name="sMonth">series of monthly data</param>
        /// <returns></returns>
        private static Series MonthSum(Series sMonth)
        {
            if (sMonth.TimeInterval != TimeInterval.Monthly)
                throw new ArgumentException("requires monthly data");

            Series sMonthTot = new Series();
            for (int i = 0; i < sMonth.Count; i++)
            {
                Point pt = sMonth[i];
                int numDays = DateTime.DaysInMonth(pt.DateTime.Year, pt.DateTime.Month);
                DateTime t = new DateTime(pt.DateTime.Year, pt.DateTime.Month, numDays);
                
                // Accounts for missing values by setting them to zero
                if (pt.IsMissing) //[JR] hack to exclude missing months that are coming out of the MonthlyAverage() call...
                { sMonthTot.Add(t, 0.0); }
                else
                { sMonthTot.Add(t, pt.Value * numDays); }
            }

            return sMonthTot;
        }



        /// <summary>
        /// Converts acre-feet to cfs 
        /// </summary>
        /// <param name="s">Series to convert units</param>
        private static Series ConvertAcreFeetToCfs(Series s)
        {
            Series rval = new Series();
            Series.CopyAttributes(s, rval);
            if (s.Units == "cfs")
            {
                return s;
            }
            else if (s.Units.ToLower() == "acre-feet" || s.Units.ToLower() == "a-f")
            {
                for (int i = 0; i < s.Count; i++)
                {
                    Point p = s[i];
                    if (s.TimeInterval == TimeInterval.Daily)
                    {
                        p.Value /= 1.98347;
                    }
                    else if (s.TimeInterval == TimeInterval.Monthly)
                    {
                        int numDays = DateTime.DaysInMonth(p.DateTime.Year, p.DateTime.Month);
                        p.Value /= (numDays * 1.98347);
                    }
                    else
                    {
                        string msg = "series '" + s.Name + "' undefined or unsupported time interval, only daily or monthly";
                        Logger.WriteLine(msg);
                        throw new NotImplementedException(msg);
                    }
                    rval.Add(p.DateTime, p.Value);
                }
            }
            else
            {
                string msg = "unknown units '" + s.Units + "' only acre-feet or a-f";
                Logger.WriteLine(msg);
                throw new NotImplementedException(msg);
            }
            rval.Units = "cfs";
            return rval;
        }



        /// <summary>
        /// Merges the disaggregated series with the observed daily series and scales the disaggregated values
        /// to maintain mass balance. 
        /// </summary>
        /// <param name="observed"></param>
        /// <param name="estimated"></param>
        /// <returns></returns>
        internal static void MergeCheckMassBalance(Series observed, Series estimated)
        {
            // Merge the observed and disaggregated series
            Series merged = Merge(observed, estimated);

            // Get monthly sums from the original disaggregated and merged series along with their difference
            Series disaggMonthlyVol = Reclamation.TimeSeries.Math.MonthlySum(estimated);
            Series mergedMonthlyVol = Reclamation.TimeSeries.Math.MonthlySum(merged);
            Series diffVol = disaggMonthlyVol - mergedMonthlyVol;
            diffVol = TimeSeries.Math.FillMissingWithZero(diffVol);

            // Adjust estimated values to maintain mass balance
            foreach (var item in merged)
            {
                if (item.Flag == PointFlag.Computed) //handles calculated points
                {
                    // Get the monthly MB error
                    double mbError = diffVol.Lookup(new DateTime(item.DateTime.Year, item.DateTime.Month, 1));

                    // Gets the count of calculated points for the month
                    Series sEstCount = merged.Subset(string.Format("{0} >= '{1}' AND {2} <= '{3}' AND {4} = {5}",
                        "[datetime]", new DateTime(item.DateTime.Year, item.DateTime.Month, 1), "[datetime]",
                        new DateTime(item.DateTime.Year, item.DateTime.Month, DateTime.DaysInMonth(item.DateTime.Year,
                            item.DateTime.Month)), "[flag]", "'" + PointFlag.Computed + "'"));
                    int estCount = sEstCount.Count;

                    if (mbError != 0.0 && estCount > 0) //MB error, adjust calculated points
                    {
                        Point p = estimated[item.DateTime];

                        double mbAdjust = mbError / estCount;
                        p.Value = item.Value + mbAdjust;
                        estimated[item.DateTime] = p;
                    }
                }
                else if (estimated.IndexOf(item.DateTime) < 0)
                {
                    //add observed data to estimated
                    estimated.Add(merged[item.DateTime]);
                }
                else
                {
                    //replace estimated data with observed
                    estimated[item.DateTime] = merged[item.DateTime];
                }
            }
        }


        /// <summary>
        /// Append data to end of series
        /// </summary>
        /// <param name="fromSeries">series with data to append</param>
        /// <param name="toSeries">series to append data to</param>
        /// <returns></returns>
        //[FunctionAttribute("Append series1 to the end of series2",
        //    "Append(series1,series2)")]
        private static void Append(Series fromSeries, Series toSeries)
        {
            DateTime t1 = toSeries.MaxDateTime;
            DateTime t2 = fromSeries.MaxDateTime;

            if (t2 <= t1 || fromSeries.TimeInterval != toSeries.TimeInterval)
            {
                Logger.WriteLine("no data to append or invalid time intervals");
                return;
            }

            int sIndex = fromSeries.IndexOf(t1) + 1;
            if (sIndex <= 0)
            {
                Logger.WriteLine(toSeries.Name + " start append date not found in " + fromSeries.Name);
                return;
            }

            for (int i = sIndex; i < fromSeries.Count; i++)
            {
                Point p = fromSeries[i];
                if (toSeries.IndexOf(p.DateTime) < 0)
                {
                    toSeries.Add(p);
                }
            }
        }


        private static Series ClearFlagsKeepMissingFlag(Series s)
        {
            Series rval = new Series();
            for (int i = 0; i < s.Count; i++)
            {
                Point p = s[i];
                if (p.Flag != PointFlag.None && p.Flag != PointFlag.Missing)
                {
                    p.Flag = PointFlag.None;
                }
                rval.Add(p);
            }
            return rval;
        }

    }
}
