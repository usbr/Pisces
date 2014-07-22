using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Estimation
{
    class MultipleLinearRegression
    {
        /// <summary>
        /// MLR Interpolation Report
        /// Look for '[JR]' in this method to find the code regions that could use a fix or more testing...
        /// </summary>
        /// <param name="sInputs"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="months"></param>
        /// <param name="fitTolerance"></param>
        /// <param name="waterYear"></param>
        public static MultipleLinearRegressionResults MlrInterpolation(SeriesList sList, DateTime t1, DateTime t2,
            int[] months, double fitTolerance, bool fillSelectedMonths = false)
        {
            // KT if there is not enough data (for example only 1 pont ) need to ignore that data set?

            MultipleLinearRegressionResults rval = new MultipleLinearRegressionResults();
            // Populate SeriesLists
            var sListFill = new SeriesList();
            foreach (var item in sList)
            {
                sListFill.Add(item.Copy());
            }

            // Get dates to be filled with interpolated values
            var missing = sList[0].GetMissing();
            if (fillSelectedMonths) //overwrites the 'missing' variable with another Series that only contains the selected dates in the input
            {
                Series missingSubset = new Series();
                foreach (var row in missing)
                {
                    if (months.Contains(row.DateTime.Month))
                    { missingSubset.Add(row); }
                }
                missing = missingSubset;
            }
            
            // Delete common dates where at least 1 data point is missing for any of the input series
            // This is done because the MLR routine does not support missing data. Missing data causes
            // data misalignments and throws off the regression... This section also deletes data for 
            // months that are not tagged in the input
            for (int i = sList[0].Count - 1; i >= 0; i--) //start from the bottom of the list to bypass indexing problems
            {
                for (int j = 0; j < sList.Count; j++)
                {
                    Point jthPt = sList[j][i];
                    if (jthPt.IsMissing || !months.Contains(jthPt.DateTime.Month))
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
            mlrOut.Add("MLR Output\t\t\t\t\tRun Date: " + DateTime.Now);
            mlrOut.Add("Estimated Series: " + sList[0].Name);
            var sEstimators = "";
            for (int i = 1; i < sList.Count; i++)
            { sEstimators = sEstimators + sList[i].Name + ", "; }
            mlrOut.Add("Estimator Series: " + sEstimators.Remove(sEstimators.Length - 2));
            mlrOut.Add("Regression Date Range: " + t1.Date + " - " + t2.Date);
            var monEstimators = "";
            foreach (var item in months)
            { monEstimators = monEstimators + item + ", "; }
            mlrOut.Add("Months Used: " + monEstimators.Remove(monEstimators.Length - 2));
            mlrOut.Add("");
            mlrOut.Add("====================================================================================");

            
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
                    foreach (var fillT in missing)
                    {
                        double fillVal;
                        try
                        {
                            // This evaluates the equation generated during the MLR estimation. Same equation-code format as above
                            fillVal = sListFill[combo[0]][fillT.DateTime].Value * mlrCoeffs[1];
                            for (int i = 2; i < mlrCoeffs.Count(); i++)
                            { fillVal = fillVal + sListFill[combo[i - 1]][fillT.DateTime].Value * mlrCoeffs[i]; }
                            fillVal = fillVal + mlrCoeffs[0];
                            if (fillVal < 0.0)
                            { sOut.Add(fillT.DateTime, Point.MissingValueFlag, "NoDataForInterpolation"); }
                            else
                            { sOut.Add(fillT.DateTime, fillVal, rVal.ToString("F05")); } //[JR] this assigns the R value as the flag, can be switched to R-Squared...
                        }
                        catch
                        { sOut.Add(fillT.DateTime, Point.MissingValueFlag, "NoDataForInterpolation"); }
                    }
                    // Add the output Series to a SeriesList
                    sOutList.Add(sOut);

                    // Populate report
                    mlrOut.Add("");
                    string equationString = "MLR Equation: " + sList[0].Name + " = ";
                    for (int ithCoeff = 1; ithCoeff < mlrCoeffs.Count(); ithCoeff++)
                    {
                        equationString = equationString + mlrCoeffs[ithCoeff].ToString("F04") + "("
                          + sList[combo[ithCoeff - 1]].Name + ") + ";
                    }
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
            //TextFile tf = new TextFile(mlrOut.ToArray());
            //var fn = FileUtility.GetTempFileName(".txt");
            //tf.SaveAs(fn);
            //System.Diagnostics.Process.Start(fn);
            rval.Report = mlrOut.ToArray();

            // Generate output Series
            var sOutFinal = sListFill[0].Copy();
            // Rmove the Points to be filled in the original input Series
            for (int i = missing.Count - 1; i >= 0; i--)
            { sOutFinal.RemoveAt(sOutFinal.IndexOf(missing[i].DateTime)); }
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
                    if (estPt.Value < 0.0) //add 0 correlation value if the estimated value < 0, [JR] this prevents the use of this routine to estimate negative values...
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
            //return sOutFinal;

            rval.EstimatedSeries = sOutFinal;
            return rval;
        }



    }
}
