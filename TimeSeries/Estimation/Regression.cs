using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Estimation
{
    public class Regression
    {
        /// <summary>
        /// Perform a simple linear regression 
        /// </summary>
        /// <param name="xDataIn"></param>
        /// <param name="yDataIn"></param>
        /// <param name="logTransform"></param>
        /// <param name="logBase"></param>
        /// <returns></returns>
        public static Tuple<double, double>[] SimpleRegression(double[] xDataIn, double[] yDataIn,
            int order = 1, bool logTransform = false, int logBase = 10)
        {
            // initialize data containers
            double[] xData, yData;
            var xTemp = new List<double>();
            var yTemp = new List<double>();
            // build regression dataset
            for (int i = 0; i < xDataIn.Count(); i++)
            {
                if (yDataIn[i] > 0) // exclude points with zero flows
                {
                    xTemp.Add(xDataIn[i]);
                    if (logTransform) // do a log transform on flow data
                    { yTemp.Add(System.Math.Log(yDataIn[i], logBase)); }
                    else // keep data as is
                    { yTemp.Add(yDataIn[i]); }
                }
            }
            xData = xTemp.ToArray();
            yData = yTemp.ToArray();
            // perform simple regression
            Func<double, double> fxn = MathNet.Numerics.Fit.PolynomialFunc(xData, yData, order);
            // build regession model evaluation points
            double xDiff = xData.Max() - xData.Min();
            int xCount = xData.Count() * 3;// point resolution 3X greater than the available number of data points
            double xStep = 1.0 * xDiff / xCount;
            // evaluate regression model
            Tuple<double, double>[] modelVals = new Tuple<double, double>[xCount+1];
            for (int i = 0; i <= xCount; i++)
            {
                var xVal = xData.Min() + (xStep * i);
                var yVal = fxn(xVal); // MathNet.Numerics.Evaluate.Polynomial(xVal, new double[] { b, m });
                if (logTransform)
                { modelVals[i] = new Tuple<double, double>(xVal, System.Math.Pow(logBase, yVal)); }
                else
                { modelVals[i] = new Tuple<double, double>(xVal, yVal); }
            }
            return modelVals;
        }
        

        public static Tuple<double, double>[] PiecewiseLinearRegression(double[] xDataIn, double[] yDataIn, 
            double[] yBreakpoints)
        {
            var output = new List<Tuple<double, double>>();
            double yMin = yDataIn.Min();
            double yMax = yDataIn.Max();
            // iterate through the breakpoints
            for (int i = 0; i <= yBreakpoints.Count(); i++)
            {
                List<double> yData = new List<double>();
                List<double> xData = new List<double>();
                double yLow, yHigh;
                // set low value for data subsetting
                if (i == 0) { yLow = yMin; }
                else { yLow = yBreakpoints[i - 1]; }
                // set high value for data subsetting
                if (i == yBreakpoints.Count()) { yHigh = yMax; }
                else { yHigh = yBreakpoints[i]; }
                // build data subsets
                for (int j = 0; j < yDataIn.Count(); j++)
                {
                    if (yDataIn[j] >= yLow && yDataIn[j] <= yHigh)
                    {
                        xData.Add(xDataIn[j]);
                        yData.Add(yDataIn[j]);
                    }
                }
                // build fit line
                output.AddRange(SimpleRegression(xData.ToArray(), yData.ToArray()));
            }
            return output.ToArray();
        }
    }
}
