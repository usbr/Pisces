using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Reclamation.TimeSeries;

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

        /// <summary>
        /// This report provides access to historical AgriMet archive weather data in a USGS 
        /// style water year report format with 12 monthly columns. Statistics are computed 
        /// at the bottom of each monthly column (max, min, avg, and total). Note: Not all 
        /// parameters are collected at each station. See AgriMet Weather Station Instrumentation 
        /// for the specific data parameters collected at each AgriMet station.
        /// </summary>
        /// <param name="s"></param> series for data table
        /// <returns></returns>
        public static DataTable WaterYearTable(Series s)
        {
            DateTime startDate = DateTime.Now;
            if (s.Count > 0)
            {
                startDate = s[0].DateTime;
            }
            //one water year table
            DataTable rval = new DataTable();
            DateTime tblDate = startDate;
            rval.Columns.Add("Day");
           
            for (int i = 0; i < 12; i++)
            {
                rval.Columns.Add(tblDate.ToString("MMM yyyy"));
                tblDate = tblDate.AddMonths(1);
            }
            //create empty tbl for the data
            for (int i = 1; i <= 31; i++)
            {
                var row = rval.NewRow();
                rval.Rows.Add(row);
                row[0] = i;
                for (int j = 1; j <= 12; j++)
                {
                    row[j] = "---";
                }
            }
            //names for the math at the bottom of the table
            String[] row_name = {"Total","Ave","Max","Min"};
            int nameIndex = 0;
            int rowTotalIndex = 32; 
            int lastRowIndex = 35;
            //add rows for total ave max min
            for (int i = rowTotalIndex; i <= lastRowIndex; i++)
            {
                var row = rval.NewRow();
                rval.Rows.Add(row);
                row[0] = row_name[nameIndex++];
                // fill colomuns with dashes accross the table
                for (int j = 1; j <= 12; j++)
                {
                    row[j] = "---";
                }
            }
            //put data into table 
            var month = 10;
            for (int j = 1; j <= 12; j++)
            {
                DateTime datatblDate;
                int days;
                if (month >= 1 && month < 10)
                {
                    
                    datatblDate = new DateTime(startDate.Year + 1, month, 1);
                    days = DateTime.DaysInMonth(datatblDate.Year, datatblDate.Month);
                }
                else
                {
                    datatblDate = new DateTime(startDate.Year, month, 1);
                    days = DateTime.DaysInMonth(datatblDate.Year, datatblDate.Month);
                }
                
                for (int i = 0; i < days; i++)
                {
                    if (s.IndexOf(datatblDate) >= 0)
                    {
                        rval.Rows[i][j] = s[datatblDate].Value.ToString("F2");
                    }
                    datatblDate = datatblDate.AddDays(1);
                }
                month++;
                if (month > 12)
                {
                    month = 1;
                }
            }

            //inserts the sum ave max min rows of the water year table report
            month = 10;
            var indexTotal = 31;
            var indexAve = 32;
            var indexMax = 33;
            var indexMin = 34;
            var index = indexTotal;
           
            for (int j = 1; j <= 12; j++)
            {
                var monthCol = Math.Subset(s, new int[] { month });
                if (index == indexTotal) 
                {
                    var sum = Math.Sum(monthCol);
                    rval.Rows[indexTotal][j] = sum;
                    index++;
                }
                if (index == indexAve)
                {
                    var ave = Math.AverageOfSeries(monthCol).ToString("F2");
                    rval.Rows[indexAve][j] = ave;
                    index++;
                }
                if (index == indexMax)
                {
                    var max = Math.MaxValue(monthCol);
                    rval.Rows[indexMax][j] = max;
                    index++;
                }
                if (index == indexMin)
                {
                    var max = Math.MinValue(monthCol);
                    rval.Rows[indexMin][j] = max;
                }
                index = indexTotal;
                month++;
                if (month > 12)
                {
                    month = 1;
                }
            }

            return rval;
        }
    }
}
