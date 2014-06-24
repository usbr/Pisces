using System;
using System.Collections.Generic;
using System.Text;

namespace Reclamation.TimeSeries
{
    public class SeriesRange
    {
        int idx1;
        int idx2;
        int idxMidpoint;
        double midPointValue;
        double sum;
        Series s;
        public SeriesRange(Series s, DateTime t1, DateTime t2)
        {
            this.s = s;
            idx1 = s.IndexOf(t1);
            idx2 = s.IndexOf(t2);
            idxMidpoint = idx1 + (idx2 - idx1) / 2;

            if (idx1 < 0 || idx2 < 0)
            {
                throw new ArgumentOutOfRangeException("t1 = " + t1.ToLongDateString() + " t2 = " + t2.ToLongDateString());
            }
            sum = 0;

            for (int i = idx1; i <= idx2; i++)
            {
                sum += s[i].Value;
            }
        }

        public int Count
        {
            get { return idx2 - idx1 + 1; }
        }

        public double SumOriginal
        {
            get { return sum; }
        }

        public double MidPointValue
        {
            get { return midPointValue; }
        }

        /// <summary>
        /// Smooths out a range of data preserving the total in the range.
        /// data becomes linear between end points that connect to a common mid-point
        /// (the end points of the range are not modified)
        /// </summary>
        public void SmoothPreservingSum()
        {
            if (Count < 4)
            {
                throw new Exception("Error: a larger range is needed for smoothing");
            }
            
            Console.WriteLine("total sum = " + sum);
            midPointValue = (s[idx1].Value + s[idx2].Value) / 2.0; // initial guess

            int counter = 0;
            double delta = 0.001;
            double F = ComputeSum(midPointValue) - sum;
            Console.WriteLine("Sum(" + midPointValue + ") = " + ComputeSum(midPointValue));
            Console.WriteLine("F = " + F);
            do // newton method.
            {
                double F2 = ComputeSum(midPointValue + delta) - sum;
                double slope = (F - F2) / delta;
                midPointValue += F / slope;
                counter++;
                F =  ComputeSum(midPointValue) - sum;
                Console.WriteLine("Sum(" + midPointValue + ") = " + ComputeSum(midPointValue));
            }
            while (counter < 20 && System.Math.Abs(F) > 0.1);

            Console.WriteLine("midPointValue = " + midPointValue);
            Console.WriteLine("Sum(" + midPointValue + ") = " + ComputeSum(midPointValue));
            Console.WriteLine("F = " + F);
            if (System.Math.Abs(F)  > 0.1)
            {
                throw new Exception("Error: could not converge");
            }

            double[] vals;
            ComputeSum(midPointValue, out vals);

            for (int i = 1; i < Count-1; i++)
            {
                Point pt = s[idx1 + i];
                pt.Flag = PointFlag.Edited;
                pt.Value = vals[i];
                s[idx1 + i] = pt;
             
            }

        }

        public double ComputeSum(double midPointValue)
        {
            double[] v;
            var d =  ComputeSum(midPointValue, out v);
            return d;
        }
        /// <summary>
        /// Computes the sum of a smoothed range
        /// </summary>
        /// <param name="midPointValue">value at the mid-point</param>
        internal double ComputeSum(double midPointValue, out double[] newValues)
        {
            double rval = s[idx1].Value;
            newValues = new double[Count];
            newValues[0] = s[idx1].Value;

            int nLeft = idxMidpoint - idx1 - 1; // number of estimated points to left of the midpoint
            if (nLeft >= 1)
            {
                double delta = midPointValue - s[idx1].Value;
                for (int i = 1; i <= nLeft; i++)
                {
                    newValues[i] = s[idx1].Value +i * delta / (nLeft + 1);
                    rval += newValues[i];
                }
            }
            rval += midPointValue;
            newValues[nLeft+1] = midPointValue;

            int nRight = idx2 - idxMidpoint - 1; // number of estimated points right of the midpoint

            if (nRight >= 1)
            {
                double delta = s[idx2].Value - midPointValue;
                for (int i = 1; i <= nRight; i++)
                {
                    newValues[nLeft+1 +i] = midPointValue +i * delta / (nRight + 1);
                    rval += newValues[nLeft+1 + i];
                }
            }

            rval += s[idx2].Value;

            newValues[Count-1] = s[idx2].Value;
            return rval;

        }
    }
}
