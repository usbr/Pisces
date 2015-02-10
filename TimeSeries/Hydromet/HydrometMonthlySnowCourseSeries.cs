using Reclamation.TimeSeries.Nrcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Hydromet
{

    /// <summary>
    /// Monthly Series that when Calculated reads NRCS daily snow data
    /// and stores in appropirate place in the monthly database
    /// 
    /// </summary>
    class HydrometMonthlySnowCourseSeries:CalculationSeries
    {
        string m_triplet;
        public HydrometMonthlySnowCourseSeries(string triplet)
        {
            m_triplet = triplet;
        }

        public override void Calculate(DateTime t1, DateTime t2)
        {
            var s = new SnotelSeries(m_triplet, SnotelParameterCodes.WTEQ);
            var rval = new Series("snow_" + m_triplet.Replace(":", "_"));
            rval.TimeInterval = TimeSeries.TimeInterval.Monthly;

            var t1a = t1.FirstOfMonth().AddMonths(-1);
            var t2a = t2.FirstOfMonth().AddMonths(1);
            s.Read(t1a, t2a);
            for (int i = 0; i < s.Count; i++)
            {
                Point pt = SnapPointToMonth(s[i]);
            }

            rval.Trim(t1, t2);
        } 

        /// <summary>
        /// readings in days > 20 moved to next month
        /// readings in days < 10 are moved to first of month
        /// readings in days 10 to 20 are considered mid-month
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private Point SnapPointToMonth(Point point)
        {
            if (point.DateTime.Day > 20)
                return new Point(point.DateTime.FirstOfMonth().AddMonths(1),point.Value,"");
            else
            if( point.DateTime.Day < 10)
                return new Point(point.DateTime.FirstOfMonth(),point.Value,"");
            else
            if( point.DateTime.Day >=10 || point.DateTime.Day <=20) 
                return new Point(point.DateTime.FirstOfMonth(),point.Value,"M"); // mid-month
            else
                throw new InvalidOperationException("point error in SnapPointToMonth "+point.ToString());
    }
}
