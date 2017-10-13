using Reclamation.TimeSeries.Parser;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reclamation.TimeSeries
{
    public static partial class Math
    {

        //// we need this overloaded version of EstimateDailyFromMonthly without the optional parameter because we use reflection

        //public static Series EstimateDailyFromMonthly(Series daily, Series monthly)
        //{
        //    return EstimateDailyFromMonthly(daily, monthly, false);
        //}

        //[FunctionAttribute("Replaces data above or below a user specified maximum and minimum, by interpolating from nearest ‘good’ data.",
        //"SmoothingInterpolateOutliers(series,min,max)")]
        //public static Series SmoothingInterpolateOutliers(Series s, double min, double max)
        //{
        //    return Estimation.Smoothing.SmoothingInterpolateOutliers(s, min, max);
        //}


        ///// <summary>
        ///// Estimates daily data 
        ///// </summary>
        ///// <param name="observed"></param>
        ///// <param name="monthly"></param>
        ///// <param name="merge"></param>
        ///// <returns></returns>
        //[FunctionAttribute("Estimates Daily data based on monthly and partial daily.  This was designed for estimating missing diversion data.  This is performed by using the time pattern from a Summary Hydrograph and scaling data to ensure the monthly volume matches.",
        // "EstimateDailyFromMonthly(daily_cfs,monthly_acre_feet,bool merge)")]
        //public static Series EstimateDailyFromMonthly(Series observed, Series monthly, bool merge = false)
        //{
        //    MonthlyToDailyConversion c = new MonthlyToDailyConversion(observed, monthly);

        //    var rval = c.ConvertToDaily();
        //    if (merge)
        //        return Merge(observed, rval);

        //    return rval;

        ////}
        ///// <summary>
        /////  Estimates Daily data based on monthly and partial daily.  This was designed for estimating missing diversion data.  This is performed by using the time pattern from a Summary Hydrograph and scaling data to ensure the monthly volume matches.
        ///// </summary>
        ///// <param name="observed">Series of daily data (cfs)</param>
        ///// <param name="monthly">Series of monthly data (acre-feet)</param>
        ///// <param name="merge">when true any available observed data is used for the estimate</param>
        ///// <param name="medianOnly">Use a single median (50%) exceedence level instead of every 2% between 5% and 95%</param>
        ///// <param name="setMissingToZero">applies to observed daily data.  When setMissingToZero is true missing data is set to zero. Otherwise missing data is ignored</param>
        ///// <returns></returns>
        //[FunctionAttribute("Estimates Daily data based on monthly and partial daily.  This was designed for estimating missing diversion data.  This is performed by using the time pattern from a Summary Hydrograph and scaling data to ensure the monthly volume matches.",
        //"EstimateDailyFromMonthly(daily_cfs,monthly_acre_feet,bool merge = false,bool medianOnly=false, bool setMissingToZero=true)")]
        //public static Series EstimateDailyFromMonthly(Series observed, Series monthly, bool merge = false, bool medianOnly = false, bool setMissingToZero = true)
        //{
        //    MonthlyToDailyConversion c = new MonthlyToDailyConversion(observed, monthly);
        //    c.FillMissingWithZero = setMissingToZero;
        //    c.MedianOnly = medianOnly;

        //    var rval = c.ConvertToDaily();
        //    if (merge)
        //        return Merge(observed, rval);

        //    return rval;

        //}


        [FunctionAttribute("Sums a list of Series.  Missing data is replaced with a zero.",
      "SumSetMissingToZero(series1,series2,...)")]
        public static Series SumSetMissingToZero(params Series[] items)
        {
            var rval = new Series();
            if (items.Length > 0)
                rval = items[0].Copy();

            SeriesList list = new SeriesList();
            list.AddRange(items);

            rval = Math.FillMissingWithZero(rval, list.MinDateTime, list.MaxDateTime);
            for (int i = 1; i < items.Length; i++)
            {
                rval = rval + Math.FillMissingWithZero(items[i], list.MinDateTime, list.MaxDateTime);
            }

            return rval;
        }

        public static Series Quality(Series s, double lowLimit, double highLimit)
        {
            Series rval = s.Copy();
            for (int i = 0; i < rval.Count; i++)
            {
                Point pt = rval[i];

                if (!pt.IsMissing)
                {
                    //pt.Value += d;
                    rval[i] = pt;
                }
            }
            ///Here, we're going to take the bad data and make it good data
            return rval;
        }



        /// <summary>
        /// Method to interpolate an estimated series based on a time-step ratio of the sum of an observed series
        /// </summary>
        /// <param name="sReal">Series of observed values</param>
        /// <param name="sEst">Series of estimated values</param>
        /// <param name="t1">DateTime to start interpolation</param>
        /// <param name="t2">DateTime to end interpolation</param>
        /// <returns></returns>
        [FunctionAttribute("Estimates a series between pairs of date ranges based on the shape of an observed series while preserving flow balance.",
        "InterpolateWithStyle(Observed Data, Data to be Overwritten, Date1A [Format: \"2000-01-31\"] , Date1B, Date2A, Date2B, ... )")]
        public static Series InterpolateWithStyle(Series sReal, Series sEst, params string[] dates)
        {
            var dateArray = getDates(dates);
            Series sOut = sEst.Copy();
            for (int i = 0; i < dateArray.Count; i = i + 2)
            {
                DateTime t1 = dateArray[i];
                DateTime t2 = dateArray[i + 1];
                Series sOuti = InterpolateWithStyleMain(sReal, sOut, t1, t2);
                sOut = Reclamation.TimeSeries.Math.Merge(sOuti, sOut);

                double sum1 = sEst.Values.Sum();
                double sum2 = sOut.Values.Sum();
                Console.WriteLine("Date Range " + t1.ToShortDateString() + " - " + t2.ToShortDateString() + " Processed.");
                Console.WriteLine("Flow Balance Check || Original Flow Sum:" + sum1 + "    Interpolated Flow Sum: " + sum2);
            }
            // Label interpolated series
            sOut.Name = sEst.Name + "_Interp";
            sOut.Units = sEst.Units;
            sOut.Provider = "Series";
            return sOut;
        }

        /// <summary>
        /// Parse date strings and check that they come in pairs
        /// </summary>
        /// <param name="dates"></param>
        /// <returns></returns>
        private static List<DateTime> getDates(string[] dates)
        {
            var dateArray = new List<DateTime>();
            if (dates.Length % 2 == 0) //Even number of date strings
            {
                for (int i = 0; i < dates.Length; i++)
                { dateArray.Add(DateTime.Parse(dates[i])); }
            }
            else //Odd number of date strings
            { throw new Exception("There are an odd number of dates specified. Use date-pairs."); }

            return dateArray;
        }

        /// <summary>
        /// Main Calculation for Interpolate with Style
        /// </summary>
        /// <param name="sReal"></param>
        /// <param name="sEst"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        private static Series InterpolateWithStyleMain(Series sReal, Series sEst, DateTime t1, DateTime t2)
        {
            // Get subsets of data for interpolation
            Series sRealTemp, sEstTemp;
            // Old workaround due to the difference in column names between Pisces and Series()
            //try
            //{ sRealTemp = sReal.Subset(string.Format("{0} >= '{1}' AND {2} <= '{3}'", "[datetime]", t1, "[datetime]", t2)); }
            //catch
            //{ sRealTemp = sReal.Subset(string.Format("{0} >= '{1}' AND {2} <= '{3}'", "[Date]", t1, "[Date]", t2)); }
            //try
            //{ sEstTemp = sEst.Subset(string.Format("{0} >= '{1}' AND {2} <= '{3}'", "[datetime]", t1, "[datetime]", t2)); }
            //catch
            //{ sEstTemp = sEst.Subset(string.Format("{0} >= '{1}' AND {2} <= '{3}'", "[Date]", t1, "[Date]", t2)); }
            sRealTemp = sReal.Subset(t1, t2);
            sEstTemp = sEst.Subset(t1, t2);

            // Get sum of values from data subset
            double sRealTempSum = sRealTemp.Values.Sum();
            double sEstTempSum = sEstTemp.Values.Sum();
            // Get source subset incremental ratios
            Series sRealTempRatio = sRealTemp / sRealTempSum;
            // Produce series with interpolated values based on source subset ratio
            Series sEstInterpQ = sRealTempRatio * sEstTempSum;
            // Merge interpolated series with original estimated series
            Series sEstInterpQOut = Reclamation.TimeSeries.Math.Merge(sEstInterpQ, sEst);

            return sEstInterpQOut;
        }


        ///// <summary>
        ///// Method to interpolate missing values within a Series via Multiple Linear Regression
        ///// </summary>
        ///// <param name="fitTolerance"></param>
        ///// <param name="s"></param>
        ///// <returns></returns>
        //[FunctionAttribute("Performs a Multiple Linear Regression using different combinations of the input interpolator Series " +
        //    "and assigns the best fit as the interpolated value so long as the input fit tolerance is met.",
        //    "MLRInterpolation(fitTolerance = value between 0.0 & 1.0, Series-0 to be interpolated, Series-1 used for interpolation, " +
        //    "Series-2 used for interpolation, Series-3, Series-4, ...")]
        //public static Series MLRInterpolationPisces(double fitTolerance, params Series[] s)
        //{
        //    SeriesList sList = new SeriesList();
        //    foreach (var item in s)
        //    { sList.Add(item); }
        //    var sOut = Reclamation.TimeSeries.Estimation.MultipleLinearRegression.MlrInterpolation(sList,
        //        new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, fitTolerance);

        //    return sOut.EstimatedSeries;
        //}

    }



}
