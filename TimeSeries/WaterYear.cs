using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.Core;

namespace Reclamation.TimeSeries
{
    public class WaterYear
    {

        public static int CurrentWaterYear()
        {
            var t = DateTime.Now.Date;
            if (t.Month >= 10)
                return t.Year - 1;
            return t.Year;
        }

        public static DateTime BeginningOfWaterYear(DateTime t)
        {
            if (t.Month >= 10)
                return new DateTime(t.Year, 10, 1);
            return new DateTime(t.Year-1, 10, 1);
        }

        public static DateTime EndOfWaterYear(DateTime t)
        {
            if (t.Month >= 10)
                return new DateTime(t.Year+1, 9, 30);
            return new DateTime(t.Year, 9, 30);
        }

        /// <summary>
        /// Returns a range of water years from user input
        /// such as  '1997 2001 2012-2013'
        /// </summary>
        /// <param name="input">input range of water years</param>
        /// <returns>array of water years</returns>
        public static int[] WaterYearsFromRange(string input)
        {
            List<int> years = new List<int>();


            string[] tokens = input.Trim().Split(' ');
            foreach (string s in tokens)
            {
                if (s.IndexOf("-") > 0)
                {
                    int yr1 = int.Parse(s.Split('-')[0]);
                    int yr2 = int.Parse(s.Split('-')[1]);
                    if (yr1 > yr2)
                        Logger.WriteLine("Error with range of dates. starting year is greater than ending year");

                    for (int y = yr1; y <= yr2; y++)
                    {
                        years.Add(y);
                    }
                }
                else
                {
                    int yr = 0;
                    if (int.TryParse(s, out yr))
                    {
                        years.Add(yr);
                    }
                }
            }


            return years.ToArray();
        }

        /// <summary>
        /// Returns a range of water years from user input
        /// such as  '1997 2001 2012-2013'
        /// </summary>
        /// <param name="input">input range of water years</param>
        /// <returns>2-d array of water year ranges</returns>
        public static int[][] WaterYearRangeFromRange(string input)
        {
            List<int[]> years = new List<int[]>();


            string[] tokens = input.Trim().Split(' ');
            foreach (string s in tokens)
            {
                if (s.IndexOf("-") > 0)
                {
                    int yr1 = int.Parse(s.Split('-')[0]);
                    int yr2 = int.Parse(s.Split('-')[1]);
                    if (yr1 > yr2)
                        Logger.WriteLine("Error with range of dates. starting year is greater than ending year");

                    years.Add(new int[] { yr1, yr2 });
                }
                else
                {
                    int yr = 0;
                    if (int.TryParse(s, out yr))
                    {
                        years.Add(new int[] { yr,yr});
                    }
                }
            }


            return years.ToArray();
        }
    }
}
