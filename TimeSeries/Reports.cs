using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries
{
    public static class Reports
    {


        public static string[] UsgsMonthlyTextReport(Series s, bool showFlags, double scale = 1.0)
        {
            List<string> report = new List<string>();

            if (s.TimeInterval != TimeInterval.Monthly)
            {
                report.Add("Error:  "+s.Name + " is not Monthly.");
                return report.ToArray();
            }

            report.Add("");
            report.Add("");
            report.Add("");
            report.Add("");
            report.Add("");
            report.Add("                                                    UNITED STATES");
            report.Add("                                             DEPARTMENT OF THE INTERIOR");
            report.Add("                                                BUREAU OF RECLAMATION");
            report.Add("");
            
            report.Add(s.Name + " "+s.Parameter +" "+ s.Units);
            report.Add("");
            
            report.Add("Year      Oct      Nov      Dec      Jan      Feb      Mar      Apr      May      Jun      Jul      Aug      Sep       Total");
            report.Add("");


            if (s.Count == 0)
                return report.ToArray();

            int wy1 = s[0].DateTime.WaterYear();
            int wy2 = s[s.Count - 1].DateTime.WaterYear();

            for (int wy = wy1; wy <= wy2; wy++)
            {
                string line = " " + wy.ToString() + " ";
                DateTime t1 = new DateTime(wy - 1, 10, 1);
                DateTime t2 = new DateTime(wy, 9, 1);
                DateTime t = t1;
                do
                {
                    line += FormatValue(s, t, .001,showFlags);

                    t = t.AddMonths(1).FirstOfMonth();
                } while (t <= t2);

                var singleYear = Math.Subset(s, new DateRange(t1, t2)); // total
                if (singleYear.CountMissing() > 0)
                    line += "      -   ";
                else
                    line += " " + (Math.Sum(singleYear)*.001).ToString("F2").PadLeft(9);

                report.Add(line);

            }

            // All October
            var oct = Math.Subset(s,new int[] {10});

            var sumOct = Math.Sum(oct);
            //var avgOct = Math.Average(oct, TimeInterval.Monthly);
           double avgOct = Math.AverageOfSeries(oct);




            return report.ToArray();
        }

        /// <summary>
        /// Formats value at specified Date
        /// </summary>
        private static string FormatValue(Series s, DateTime date, double scale, bool showFlag)
        {
            int idx = s.IndexOf(date);
            if (idx < 0 || s[idx].IsMissing)
            {
                return "    -    ".PadLeft(9);
            }
            else
            {
                if (showFlag)
                {
                    return (s[idx].Value * scale).ToString("F2").PadLeft(8)+s[idx].Flag;
                }
                else
                {
                    return (s[idx].Value * scale).ToString("F2").PadLeft(9);
                }
            }
        }



        public static void DisplayLines(string[] lines)
        {
            var fn = FileUtility.GetTempFileName(".txt");
            File.WriteAllLines(fn, lines.ToArray());
            Process.Start(fn);
        }
    }
}
