using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Reclamation.TimeSeries
{

    /// <summary>
    /// Index tool to manage lookups of dates in a Periodic Series
    /// including those based on water years
    /// </summary>
    public class JulianIndex
    {

        int[,] m_lookupTable = new int[12, 31];

        /// <summary>
        /// creates internal index table with Index
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="ignoreFeb29">in leap years use same index for feb 28th and feb 29th</param>
        public JulianIndex(DateTime t1, DateTime t2, bool ignoreFeb29=false)
        {
            Init(t1, t2,ignoreFeb29);
        }

        public int LookupJulianDay(DateTime t)
        {
            return m_lookupTable[t.Month-1, t.Day-1];
        }

        private void Init(DateTime t1, DateTime t2, bool igoneFeb29)
        {
            TimeSpan ts = t2.Subtract(t1);
            if (ts.Days > 366)
                throw new Exception("Error: PeriodicSeries represents more than a year");
            DateTime t = t1;
            int index = 1;
            while (t <= t2)
            {
                m_lookupTable[t.Month - 1, t.Day - 1] = index;

                if (igoneFeb29 && t.Month == 2 && t.Day == 28 && DateTime.IsLeapYear(t.Year))
                    index += 0;
                else
                    index++;

                t = t.AddDays(1);
            }

        }
    }
}
