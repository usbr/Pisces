using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.Core;
using System.Data;
using Reclamation.TimeSeries.Parser;
using Reclamation.TimeSeries.Estimation;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Reclamation.TimeSeries
{
    public static partial class Math
    {

        // we need this overloaded version of EstimateDailyFromMonthly without the optional parameter because we use reflection

        public static Series EstimateDailyFromMonthly(Series daily, Series monthly)
        {
            return EstimateDailyFromMonthly(daily, monthly, false);
        }

        [FunctionAttribute("Replaces data above or below a user specified maximum and minimum, by interpolating from nearest ‘good’ data.",
        "SmoothingInterpolateOutliers(series,min,max)")]
        public static Series SmoothingInterpolateOutliers(Series s, double min, double max)
        {
            return Estimation.Smoothing.SmoothingInterpolateOutliers(s, min, max);
        }


        /// <summary>
        /// Estimates daily data 
        /// </summary>
        /// <param name="observed"></param>
        /// <param name="monthly"></param>
        /// <param name="merge"></param>
        /// <returns></returns>
        [FunctionAttribute("Estimates Daily data based on monthly and partial daily.  This was designed for estimating missing diversion data.  This is performed by using the time pattern from a Summary Hydrograph and scaling data to ensure the monthly volume matches.",
         "EstimateDailyFromMonthly(daily_cfs,monthly_acre_feet,bool merge)")]
        public static Series EstimateDailyFromMonthly(Series observed, Series monthly, bool merge = false)
        {
            MonthlyToDailyConversion c = new MonthlyToDailyConversion(observed, monthly);

            var rval = c.ConvertToDaily();
            if (merge)
                return Merge(observed, rval);

            return rval;

        }
        /// <summary>
        ///  Estimates Daily data based on monthly and partial daily.  This was designed for estimating missing diversion data.  This is performed by using the time pattern from a Summary Hydrograph and scaling data to ensure the monthly volume matches.
        /// </summary>
        /// <param name="observed">Series of daily data (cfs)</param>
        /// <param name="monthly">Series of monthly data (acre-feet)</param>
        /// <param name="merge">when true any available observed data is used for the estimate</param>
        /// <param name="medianOnly">Use a single median (50%) exceedence level instead of every 2% between 5% and 95%</param>
        /// <param name="setMissingToZero">applies to observed daily data.  When setMissingToZero is true missing data is set to zero. Otherwise missing data is ignored</param>
        /// <returns></returns>
        [FunctionAttribute("Estimates Daily data based on monthly and partial daily.  This was designed for estimating missing diversion data.  This is performed by using the time pattern from a Summary Hydrograph and scaling data to ensure the monthly volume matches.",
        "EstimateDailyFromMonthly(daily_cfs,monthly_acre_feet,bool merge = false,bool medianOnly=false, bool setMissingToZero=true)")]
        public static Series EstimateDailyFromMonthly(Series observed, Series monthly, bool merge = false, bool medianOnly = false, bool setMissingToZero = true)
        {
            MonthlyToDailyConversion c = new MonthlyToDailyConversion(observed, monthly);
            c.FillMissingWithZero = setMissingToZero;
            c.MedianOnly = medianOnly;

            var rval = c.ConvertToDaily();
            if (merge)
                return Merge(observed, rval);

            return rval;

        }


        [FunctionAttribute("Sums a list of Series.  Missing data is replaced with a zero.",
      "SumSetMissingToZero(series1,series2,...)")]
        public static Series SumSetMissingToZero(Series[] items)
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

        
        /// <summary>
        /// MLR Interpolation Report
        /// Look for '[JR]' in this method to find the code regions that could use a fix or more testing...
        /// </summary>
        /// <param name="sInputs"></param>
        /// <param name="yearStart"></param>
        /// <param name="yearEnd"></param>
        /// <param name="months"></param>
        /// <param name="fitTolerance"></param>
        /// <param name="DB"></param>
        /// <param name="waterYear"></param>
        public static Series MlrInterpolation(List<string> sInputs, int yearStart, int yearEnd,
            int[] months, double fitTolerance, TimeSeriesDatabase DB, bool waterYear = true)
        {
            // Define data read dates
            DateTime tStart, tEnd;
            if (waterYear)
            { tStart = new DateTime(yearStart, 10, 1); tEnd = new DateTime(yearEnd, 9, DateTime.DaysInMonth(yearEnd, 9)); }
            else
            { tStart = new DateTime(yearStart, 1, 1); tEnd = new DateTime(yearEnd, 12, 31); }

            // Populate SeriesLists
            var sList = new SeriesList();
            var sListFill = new SeriesList();
            foreach (var item in sInputs)
            { sList.Add(DB.GetSeriesFromName(item)); sListFill.Add(DB.GetSeriesFromName(item)); }
            sList.Read(tStart, tEnd); sListFill.Read(tStart, tEnd);

            // [JR] This method relies on missing data flags to find missing data; needs to be tested for manually imported Series 
            string missingFlag = "";
            foreach (var sItem in sList)
            {
                if (sItem.TimeInterval == TimeInterval.Hourly || sItem.TimeInterval == TimeInterval.Daily || sItem.TimeInterval == TimeInterval.Irregular)
                { missingFlag = "m"; }
                else if (sItem.TimeInterval == TimeInterval.Monthly)
                { missingFlag = "(null)"; }
                else
                { throw new Exception("Series Time Interval not defined for " + sItem.Name); }
            }

            // Get dates to be filled with interpolated values
            var fillDates = new List<DateTime>();
            var missing = sList[0].Subset(String.Format("[flag] = '{0}'", missingFlag));
            foreach (var item in missing)
            { fillDates.Add(item.DateTime); }

            // Delete common dates where at least 1 data point is missing for any of the input series
            // This is done because the MLR routine does not support missing data. Missing data causes
            // data misalignments and throws off the regression... This section also deletes data for 
            // months that are not tagged in the input
            for (int i = sList[0].Count - 1; i >= 0; i--) //start from the bottom of the list to bypass indexing problems
            {
                for (int j = 0; j < sList.Count; j++)
                {
                    Point jthPt = sList[j][i];
                    if (jthPt.Flag == missingFlag || !months.Contains(jthPt.DateTime.Month))
                    {
                        for (int k = 0; k < sList.Count; k++) //delete this date from all Series in the list
                        { sList[k].RemoveAt(i); }
                        break;
                    }
                }
            }
            
            // Initialize MLR report and populate header
            List<string> mlrOut = new List<string>();
            mlrOut.Add("");
            mlrOut.Add("MLR Output\t\t\t\t\tRun Date: "+DateTime.Now);
            mlrOut.Add("Estimated Series: " + sInputs[0]);
            var sEstimators = "";
            for (int i = 1; i < sInputs.Count; i++)
            { sEstimators = sEstimators + sInputs[i] + ", "; }
            mlrOut.Add("Estimator Series: " + sEstimators.Remove(sEstimators.Length - 2));
            mlrOut.Add("Regression Date Range: " + tStart.Date + " - " + tEnd.Date);
            var monEstimators = "";
            foreach (var item in months)
            { monEstimators = monEstimators + item + ", "; }
            mlrOut.Add("Months Used: " + monEstimators.Remove(monEstimators.Length - 2));
            mlrOut.Add("");
            mlrOut.Add("------------------------------------------------------------------------------------");

            // Initialize output SeriesList
            var sOutList = new SeriesList();
            
            // Loop through each SeriesList combination for MLR
            for (int k = 1; k <= sList.Count - 1; k++)
            {
                AllPossibleCombination combinationData = new AllPossibleCombination(sList.Count - 1, k); //uses StackOverflow Class for combinations
                var combinationList = combinationData.GetCombinations();
                // Loop through each combination in the list and run MLR
                foreach (var combo in combinationList)
                {
                    // Build MLR method inputs
                    // xData is the different Series values that will be used to generate the MLR equation, all index > 0 in the SeriesList. Matrix format
                    // yData is the target Series values that is the target for MLR, index = 0 of the SeriesList. Vector format
                    double[][] xData = new double[sList[0].Count][];
                    double[] yData = new double[sList[0].Count];
                    // Loop through the dates to populate the xData and the yData
                    for (int i = 0; i < sList[0].Count; i++)
                    {
                        var jthRow = new List<double>();
                        // Loop through each Series in SeriesList
                        for (int j = 0; j < combo.Count(); j++)
                        { jthRow.Add(sList[combo[j]][i].Value); }
                        xData[i] = jthRow.ToArray();
                        yData[i] = sList[0][i].Value;
                    }

                    // MLR via Math.Net.Numerics
                    double[] mlrCoeffs = MathNet.Numerics.LinearRegression.MultipleRegression.QR(xData, yData, true); //this is more stable than the method below
                    //double[] p2 = MathNet.Numerics.Fit.MultiDim(xData, yData, true); //this method is faster but less stable

                    // Evaluate fit
                    Series sModeled = sList[0].Clone();
                    // Equations are of the form y = x1(s1) + x2(s2) + ... + xN the loop handles the inner part of the equation if it exists x2(s2) + ...
                    //          while the lines before and after the loop handles the first and last terms x1(s1) and xN respectively
                    sModeled = sList[combo[0]] * mlrCoeffs[1];
                    for (int i = 2; i < mlrCoeffs.Count(); i++)
                    { sModeled = sModeled + sList[combo[i - 1]] * mlrCoeffs[i]; } //magic number -1 is used so the correct corresponding Series is used with the correct mlr-coefficient
                    sModeled = sModeled + mlrCoeffs[0];
                    var rVal = MathNet.Numerics.GoodnessOfFit.R(sModeled.Values, sList[0].Values);//this is the statistic reported by the FORTRAN code
                    var rSqd = MathNet.Numerics.GoodnessOfFit.RSquared(sModeled.Values, sList[0].Values); //this is the R-squared for model fit

                    // Fill missing dates and generate a SeriesList for final Series output
                    var sOut = new Series(); //initialize Series to be added to output SeriesList
                    foreach (var fillT in fillDates)
                    {
                        double fillVal;
                        try
                        {
                            fillVal = sListFill[combo[0]][fillT].Value * mlrCoeffs[1];
                            for (int i = 2; i < mlrCoeffs.Count(); i++)
                            { fillVal = fillVal + sListFill[combo[i - 1]][fillT].Value * mlrCoeffs[i]; }
                            fillVal = fillVal + mlrCoeffs[0];
                            if (fillVal < 0.0)
                            { sOut.Add(fillT, -99.99, "NoDataForInterpolation"); }
                            else
                            { sOut.Add(fillT, fillVal, rVal.ToString("F05")); } //[JR] this assigns the R value as the flag, can be switched to R-Squared...
                        }
                        catch
                        { sOut.Add(fillT, -99.99, "NoDataForInterpolation"); }
                    }
                    // Add the output Series to a SeriesList
                    sOutList.Add(sOut);

                    // Populate report
                    mlrOut.Add("");
                    string equationString = "MLR Equation: " + sInputs[0] + " = ";
                    for (int ithCoeff = 1; ithCoeff < mlrCoeffs.Count(); ithCoeff++)
                    { equationString = equationString + mlrCoeffs[ithCoeff].ToString("F04") + "(" + sInputs[combo[ithCoeff - 1]] + ") + "; }
                    equationString = equationString + mlrCoeffs[0].ToString("F04");
                    mlrOut.Add(equationString);
                    mlrOut.Add("Correlation Coefficient = " + rVal.ToString("F04"));
                    mlrOut.Add("R-Squared Coefficient = " + rSqd.ToString("F04"));
                    mlrOut.Add("MLR Estimates: ");
                    foreach (var item in sOut)
                    { mlrOut.Add("\t\t" + item.ToString(true)); }
                    mlrOut.Add("");
                    mlrOut.Add("------------------------------------------------------------------------------------");
                }
            }

            // Generate MLR report
            TextFile tf = new TextFile(mlrOut.ToArray());
            var fn = FileUtility.GetTempFileName(".txt");
            tf.SaveAs(fn);
            System.Diagnostics.Process.Start(fn);

            // Generate output Series
            var sOutFinal = sListFill[0].Copy();
            // Rmove the Points to be filled in the original input Series
            for (int i = fillDates.Count - 1; i >= 0; i--)
            { sOutFinal.RemoveAt(sOutFinal.IndexOf(fillDates[i])); }
            // Find the best fit out of all the estimated values
            // Loops through the dates
            foreach (var sRow in sOutList[0]) 
            {
                DateTime estT = sRow.DateTime;
                List<double> flagItems = new List<double>();//container for flag values
                List<double> valItems = new List<double>();//container for estiamted values
                // Loops through each estimate
                for (int i = 0; i < sOutList.Count; i++) 
                {
                    Point estPt = sOutList[i][estT];
                    valItems.Add(estPt.Value);
                    if (estPt.Value < 0.0) //add 0 correlation value if the estimated value < 0
                    { flagItems.Add(0.0); }
                    else
                    { flagItems.Add(Convert.ToDouble(estPt.Flag)); }
                }
                var maxFit = flagItems.Max();
                var bestFitVal = valItems[flagItems.IndexOf(maxFit)];
                if (maxFit >= fitTolerance) //add the value if it exceeds the specified tolerance
                { sOutFinal.Add(estT, bestFitVal, "E"); }
                else //add missing since there is no acceptable estimate to fill this missing value
                { sOutFinal.AddMissing(estT); }
            }
            return sOutFinal;
        }



    }

    /// <summary>
    /// StackOverflow class to generate combination lists for use with MLR interpolation
    /// Source: http://stackoverflow.com/questions/548402/list-all-possible-combinations-of-k-integers-between-1-n-n-choose-k
    /// Downloaded and tested 18JULY2014 - JR
    /// </summary>
    public class AllPossibleCombination
    {
        // Initialize required variables
        int n, k;
        int[] indices;
        List<int[]> combinations = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="n_"></param>
        /// <param name="k_"></param>
        public AllPossibleCombination(int n_, int k_)
        {
            if (n_ <= 0)
            { throw new ArgumentException("n_ must be in N+"); }
            if (k_ <= 0)
            { throw new ArgumentException("k_ must be in N+"); }
            if (k_ > n_)
            { throw new ArgumentException("k_ can be at most n_"); }

            n = n_;
            k = k_;
            indices = new int[k];
            indices[0] = 1;
        }

        /// <summary>
        /// Returns all possible k combination of 0..n-1
        /// </summary>
        /// <returns></returns>
        public List<int[]> GetCombinations()
        {
            if (combinations == null)
            {
                combinations = new List<int[]>();
                Iterate(0);
            }
            return combinations;
        }

        /// <summary>
        /// Generates the combination list
        /// </summary>
        /// <param name="ii"></param>
        private void Iterate(int ii)
        {
            // Initialize
            if (ii > 0)
            { indices[ii] = indices[ii - 1] + 1; }

            for (; indices[ii] <= (n - k + ii + 1); indices[ii]++)
            {
                if (ii < k - 1)
                { Iterate(ii + 1); }
                else
                {
                    int[] combination = new int[k];
                    indices.CopyTo(combination, 0);
                    combinations.Add(combination);
                }
            }
        }
    }


}
