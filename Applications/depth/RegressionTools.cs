using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depth
{
    public class RegressionTools
    {
        /// <summary>
        /// Perform a simple linear regression 
        /// </summary>
        /// <param name="xDataIn"></param>
        /// <param name="yDataIn"></param>
        /// <param name="logTransform"></param>
        /// <param name="logBase"></param>
        /// <returns></returns>
        public static IEquation SimpleRegression(double[] xDataIn, double[] yDataIn,
            int order = 1, bool logTransform = false, int logBase = 10)
        {
            // initialize data containers
            var xTemp = new List<double>();
            var yTemp = new List<double>();
            // build regression dataset
            for (int i = 0; i < xDataIn.Count(); i++)
            {
                if (yDataIn[i] > 0) // exclude points with zero flows
                {
                    
                    if (logTransform) // do a log transform on flow data
                    {
                        xTemp.Add(Math.Log(xDataIn[i], logBase));
                        yTemp.Add(Math.Log(yDataIn[i], logBase));
                    }
                    else // keep data as is
                    {
                        xTemp.Add(xDataIn[i]);
                        yTemp.Add(yDataIn[i]);
                    }
                }
            }
            var xData = xTemp.ToArray();
            var yData = yTemp.ToArray();
            var fxn = MathNet.Numerics.Fit.Line(xData, yData);
            // build regession model evaluation points
            double xDiff = xData.Max() - xData.Min();
            int xCount = xData.Count() * 3;// point resolution 3X greater than the available number of data points
            double xStep = 1.0 * xDiff / xCount;
            // evaluate regression model
            Tuple<double, double>[] modelVals = new Tuple<double, double>[xCount + 1];
            LogEquation eq = new LogEquation(fxn.Item2, fxn.Item1);
            for (int i = 0; i <= xCount; i++)
            {
                var xVal = xData.Min() + (xStep * i);
                var yVal = eq.Eval(xVal);

                if (logTransform)
                { modelVals[i] = new Tuple<double, double>(System.Math.Pow(logBase,xVal), System.Math.Pow(logBase, yVal)); }
                else
                { modelVals[i] = new Tuple<double, double>(xVal, yVal); }
            }

           return eq;
        }

    }
}
