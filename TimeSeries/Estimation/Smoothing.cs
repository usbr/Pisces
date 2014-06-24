using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.Core;
using Reclamation.TimeSeries.Parser;

namespace Reclamation.TimeSeries.Estimation
{
    public static class Smoothing
    {

        public static Series SmoothingInterpolateOutliers(Series s, double min, double max)
        {

            Series rval = s.Copy();
            var indexToBadPoints = new List<int>();
            for (int i = 0; i < rval.Count; i++)
            {
                var pt = rval[i];

                if (pt.Value > max || pt.Value < min)
                {
                    var pt1 = FindGoodPoint(rval, i-1, -1,min,max);
                    var pt2 = FindGoodPoint(rval, i+1, 1,min,max);

                    if (!pt1.IsMissing && !pt2.IsMissing)
                    {
                       pt.Value = Math.Interpolate(pt.DateTime, pt1.DateTime, pt2.DateTime, pt1.Value, pt2.Value);
                       pt.Flag = PointFlag.Interpolated;
                       rval[i] = pt;
                    }
                    else
                    {
                        Logger.WriteLine("Warning: could not find boundary point to interpolate");
                    }
                 
                }
            }

            return rval;
        }

        /// <summary>
        /// find closest good point, in specified direction, starting at specified index
        /// </summary>
        /// <param name="s">series</param>
        /// <param name="i">index to begin search</param>
        /// <param name="increment">increment, and direction to look.  should be either +1 or -1</param>
        /// <param name="min">minimum criteria</param>
        /// <param name="max">maximum criteria</param>
        /// <returns></returns>
        private static Point FindGoodPoint(Series s, int i, int increment, double min,double max)
        {
            var rval = Point.Missing;

            int j = i;
            while( j<s.Count && j>0)
            {
                var pt = s[j];
                if (!pt.IsMissing && pt.Value <= max  && pt.Value >= min)
                    return pt;
            
              j+= increment;
            }
            return rval;
        }

    }
}
