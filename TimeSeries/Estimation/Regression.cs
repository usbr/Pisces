using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Estimation
{
    public class Regression
    {

        public static Tuple<double, double>[] SimpleLinearRegression(double[] xData, double[] yData)
        {
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
                modelVals[i] = new Tuple<double, double>(xVal, MathNet.Numerics.Evaluate.Polynomial(xVal, new double[] {b, m}));
            }

            return modelVals;
        }
        
    }
}
