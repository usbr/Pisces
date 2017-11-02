using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reclamation.TimeSeries;
using System.IO;
using System.Data;

namespace HydrometTools.Stats
{
    class OldSchoolReport
    {

        internal static void Display(Series series1, string title, string option,
            string cbtt, string pcode, MonthDayRange range, 
            int wy1, int wy2)
        {
            var s = series1.Copy();
            var report = new List<string>();
            report.Add("DAILY VALUES SUMMATION PROCESS DATE: "+DateTime.Now.ToShortDateString());
            report.Add("");
            report.Add("Station name        "+cbtt.ToUpper().PadRight(8)+" Begin and end year  "+wy1+"-"+wy2);
            report.Add("Parameter code      "+pcode.ToUpper().PadRight(8)+" Begin and end date "+ range.MMMDD1+" - "+range.MMMDD2);
            report.Add("Option "+option);
            report.Add("");
            report.Add("BY YEAR                RANKED");
            report.Add("------------------------------------------");
            report.Add("YEAR  SUMMATION  MISS  YEAR SUMMATION");
            report.Add("");
            int offset = report.Count;
            for (int i = 0; i < s.Count; i++)
            {
                Point pt = s[i];
                string byYear = pt.DateTime.WaterYear()+"  "+pt.Value.ToString("f2")+" "+pt.Flag;
                report.Add(byYear);

            }

            s.Table.DefaultView.Sort = "value desc";
            for (int i = 0; i < s.Count; i++)
            {
                DataRowView row = s.Table.DefaultView[i];
                string sorted = Convert.ToDateTime(row[0]).WaterYear() + " " + Convert.ToDouble(row[1]).ToString("f2");
                report[i+offset] = report[i+offset] + "      " + sorted;

            }
            report.Add("");
            report.Add("Average of  " + s.Count + " years:  " + Reclamation.TimeSeries.Math.AverageOfSeries(s).ToString("f0"));
            var fn = FileUtility.GetTempFileName("check.txt");
            File.WriteAllLines(fn, report.ToArray());

            System.Diagnostics.Process.Start(fn);

        }
    }
}
