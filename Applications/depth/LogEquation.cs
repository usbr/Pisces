using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Depth
{
    public class LogEquation : IEquation
    {
        double m, b;
        public LogEquation(double slope, double intercept)
        {
            m = slope;
            b = intercept;
        }

        public double EvalTransform(double x)
        {
            var yVal = b + m * Math.Log10(x);
            return Math.Pow(10,yVal);
        }
        public double Eval(double x)
        {
            return b + m * x;
        }

        public string Name
        {
            get
            {
                return "log10(y) = " + b.ToString("F5") + " + "+m.ToString("F5")+" * log(x)";
            }
        }

    }
}
