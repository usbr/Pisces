using System;
using System.Collections.Generic;
using System.Text;

namespace Reclamation.TimeSeries
{
    public static class Extensions
    {

        public static int WaterYear(this DateTime t)
        {

            if (t.Month > 9)
                return t.Year + 1;
            return t.Year;
        }

        public static DateTime EndOfMonth(this DateTime t)
        {
            return new DateTime(t.Year, t.Month, DateTime.DaysInMonth(t.Year, t.Month));
        }

        public static DateTime FirstOfMonth(this DateTime t)
        {
            return new DateTime(t.Year, t.Month, 1);
        }

        public static DateTime MidMonth(this DateTime t)
        {
            return new DateTime(t.Year, t.Month, 16);
        }
        public static bool IsMidnight(this DateTime t)
        {
            return (t.Hour == 0 && t.Minute == 0 && t.Second == 0);
        }
       
    }
}
