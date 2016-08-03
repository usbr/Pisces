using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Estimation
{
    public class Regression
    {

        public static Tuple<double, double>[] SimpleLinearRegression(double[] xDataIn, double[] yDataIn, 
            bool logTransform = false, int logBase = 10)
        {
            double[] xData, yData;
            // Do a log transform on flow data
            if (logTransform)
            {
                var xTemp = new List<double>();
                var yTemp = new List<double>();
                for (int i = 0; i < xDataIn.Count(); i++)
                {
                    if (yDataIn[i] > 0)
                    {
                        yTemp.Add(System.Math.Log(yDataIn[i], logBase));
                        xTemp.Add(xDataIn[i]);
                    }
                }
                xData = xTemp.ToArray();
                yData = yTemp.ToArray();
            }
            else
            {
                xData = xDataIn;
                yData = yDataIn;
            }

            // produces an equation of the form Y = mX + b
            Tuple<double, double> coeffs = MathNet.Numerics.Fit.Line(xData, yData);
            double b = coeffs.Item1;
            double m = coeffs.Item2;

            int xDiff = Convert.ToInt32(xData.Max() - xData.Min());
            int xCount = xData.Count();
            int evalCount = xCount * 5;
            double xStep = 1.0 * xDiff / evalCount;

            Tuple<double, double>[] modelVals = new Tuple<double, double>[evalCount];
            for (int i = 0; i < evalCount; i++)
            {
                var xVal = xData.Min() + (xStep * i);
                var yVal = MathNet.Numerics.Evaluate.Polynomial(xVal, new double[] { b, m });
                if (logTransform)
                {
                    modelVals[i] = new Tuple<double, double>(xVal,System.Math.Pow(logBase,yVal));
                }
                else
                {
                    modelVals[i] = new Tuple<double, double>(xVal, yVal);
                }
            }

            return modelVals;
        }
        
    }
}
