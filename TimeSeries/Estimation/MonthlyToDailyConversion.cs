using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Parser;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Estimation
{
    public class MonthlyToDailyConversion
    {

        Series daily;
        Series monthly;

        public MonthlyToDailyConversion(Series observedDaily, Series monthly )
        {
            this.daily = observedDaily.Copy();
            this.monthly = monthly;
            MedianOnly = false;
            FillMissingWithZero = true;
        }

        internal bool MedianOnly { get; set; }

        internal bool FillMissingWithZero { get; set; }

        internal Series ConvertToDaily()
        {
            Series estimatedDaily = new Series();
            estimatedDaily.HasFlags = true;
            estimatedDaily.TimeInterval = TimeInterval.Daily;


            if (FillMissingWithZero)
            {
                daily = Math.FillMissingWithZero(daily, daily.MinDateTime, daily.MaxDateTime);
            }
            else
            {
                daily.RemoveMissing();
            }
            //daily.RemoveMissing();

            //int[] levels = {5,10,20,30,40,50,60,70,80,90,95};
            //int[] levels = {10,20,30,40,50,60,70,80,90};

            List<int> levels = new List<int>();

            if (MedianOnly)
            {
                levels.Add(50);
            }
            else
            {
                for (int i = 5; i <= 95; i += 2)
                {
                    levels.Add(i);
                }
            }

            var sHydrograph = Math.SummaryHydrograph(daily, levels.ToArray(), new DateTime(2008, 1, 1), false, false, false, false);//, false);
            var summaryHydrographTable = sHydrograph.ToDataTable(true);

            for (int i = 1; i < summaryHydrographTable.Columns.Count; i++)
            {
                summaryHydrographTable.Columns[i].ColumnName = levels[i - 1].ToString();
            }

            //DataTableOutput.Write(summaryHydrographTable, @"c:\temp\junk.csv", false);


            SeriesList monthlySum = new SeriesList();
            for (int i = 0; i < sHydrograph.Count; i++)
            {
                Series sum = Math.MonthlyValues(sHydrograph[i], Math.Sum);
                sum.Name = levels[i].ToString();
                monthlySum.Add(sum);
            }

            var monthlyExceedanceSums = monthlySum.ToDataTable(true);
            if (monthlySum.Count == 1 && levels.Count == 1)
                monthlyExceedanceSums.Columns[1].ColumnName = levels[0].ToString();

            var monthlyTable = monthly.Table;

            DateTime t = monthly.MinDateTime;
            DateTime t2 = monthly.MaxDateTime;
            t2 = new DateTime(t2.Year, t2.Month, DateTime.DaysInMonth(t2.Year, t2.Month));


            while (t < t2)
            {
                var tm = new DateTime(t.Year, t.Month, 1);
                if (monthly.IndexOf(tm) < 0)
                {
                    estimatedDaily.AddMissing(t);
                }
                else
                {
                    double mv = monthly[tm].Value;
                    double mvcfsdays = mv / 1.98347;
                    double exceedanceValue = 0;
                    int exceedancePercent = LookupExceedance(monthlyExceedanceSums, t, mvcfsdays, out exceedanceValue);
                    double ratio = 0;
                    if (exceedanceValue != 0)
                        ratio = mvcfsdays / exceedanceValue;
                    else
                        ratio = 0;

                    double shcfs = LookupSummaryHydrograph(summaryHydrographTable, t, exceedancePercent);

                    estimatedDaily.Add(t, shcfs * ratio,"scaled with "+exceedancePercent+"%");
                }
                t = t.AddDays(1);
            }

            VerifyWithMonthlyVolume(monthly, estimatedDaily);
          //  SmoothSpikes(monthly, daily, estimatedDaily);

            return estimatedDaily;
        }


        /// <summary>
        /// Revert to using simple conversion of monthly data when estimated
        /// data exceededs the maximum observed by month * 1.5
        /// </summary>
        /// <param name="monthly"></param>
        /// <param name="daily"></param>
        /// <param name="estimatedDaily"></param>
        private static void SmoothSpikes(Series monthly, Series daily, Series estimatedDaily)
        {

            // create a list of maximums by month
            List<double> maxByMonth = new List<double>();
            maxByMonth.Add(0);// create pad at index = 0. so index = month
            for (int m = 1; m <= 12; m++)
            {
              var subset =  Math.Subset(daily, new int[] { m });
              maxByMonth.Add(Math.MaxPoint(subset).Value);
            }


            for (int i = 0; i < estimatedDaily.Count; i++)
            {
                var pt = estimatedDaily[i];
                if (!pt.IsMissing && pt.Value > maxByMonth[pt.DateTime.Month] * 1.5 )
                {
                    // lookup monthly value
                    DateTime t = new DateTime(pt.DateTime.Year, pt.DateTime.Month, 1);
                    if (monthly.IndexOf(t) < 0)
                        continue;
                    
                    double avgFlow = monthly[t].Value / 1.98347 / DateTime.DaysInMonth(t.Year, t.Month);
                    SetDailyValueForMonth(estimatedDaily, t.Year, t.Month, avgFlow,"smoothing");
                }
            }

        }
        /// <summary>
        /// verify if the monthly data agrees with estimated daily
        /// adjust if we are not within 10%
        /// adjust using simple conversion of monthly
        /// </summary>
        /// <param name="monthly"></param>
        /// <param name="estimatedDaily">Series to be adjusted to match monthly</param>
        private static void VerifyWithMonthlyVolume(Series monthly, Series estimatedDaily)
        {

            var est_af = estimatedDaily.Copy();
            Math.Multiply(est_af, 1.98347);
            var m_af = Math.MonthlySum(est_af);
            var diff = (m_af - monthly) / m_af;
            diff = Math.Abs(diff);
            for (int i = 0; i < diff.Count; i++)
            {
                if (!monthly[i].IsMissing && diff[i].Value > .1) // 10% error
                { // proabaly zero value in exceedance lookup.
                    // use monthly value instead of estimated value.
                    DateTime t = monthly[i].DateTime;
                    double avgFlow = monthly[i].Value / 1.98347 / DateTime.DaysInMonth(t.Year, t.Month);
                    SetDailyValueForMonth(estimatedDaily, t.Year,t.Month, avgFlow,"monthly avg flow");
                }
            }
        }

        private static void SetDailyValueForMonth(Series estimatedDaily, int year, int month, double avgFlow, string flag)
        {
            DateTime t1 = new DateTime(year,month,1);
            DateTime t2 = new DateTime(year,month,DateTime.DaysInMonth(year,month));
            int startIndex = estimatedDaily.IndexOf(t1);
            if (startIndex < 0)
                startIndex = 0;
            for (int j = startIndex; j < estimatedDaily.Count; j++)
            {
                var pt = estimatedDaily[j];
                if (pt.DateTime > t2)
                    break;
                if (pt.DateTime.Year == year && pt.DateTime.Month == month)
                {
                    pt.Value = avgFlow;
                    pt.Flag = flag;
                    estimatedDaily[j] = pt;
                }
            }
        }

      


        public static double LookupSummaryHydrograph(DataTable summaryHydrographTable, DateTime t, int exceedancePercent)
        {


            for (int i = 0; i < summaryHydrographTable.Rows.Count; i++)
            {
                DateTime d = Convert.ToDateTime(summaryHydrographTable.Rows[i][0]);
                if (t.Month == 2 && t.Day == 29)
                    t = new DateTime(t.Year, 2, 28);

                if (d.Month == t.Month && d.Day == t.Day)
                {
                    string colName = exceedancePercent.ToString();
                    var colIndex = summaryHydrographTable.Columns.IndexOf(colName);

                    if (colIndex < 0)
                        Console.WriteLine("oops");


                    bool alternateLevelUsed = false;
                    while (summaryHydrographTable.Rows[i][colIndex] == DBNull.Value)
                    {
                        alternateLevelUsed = true;
                        // try the next closest level until a non-null is found.
                        if (exceedancePercent >= 50)
                        {
                            colIndex--;
                        }
                        else
                        {
                            colIndex++;
                        }

                        if (colIndex < 0 || colIndex >= summaryHydrographTable.Columns.Count)
                        {
                            throw new IndexOutOfRangeException("Error: Could not find exceedance percent " + exceedancePercent);
                        }
                    }
                    if( alternateLevelUsed )
                    Logger.WriteLine("Warning: excedance level " + exceedancePercent + " is null. value estimated instead");
                    

                    return Convert.ToDouble(summaryHydrographTable.Rows[i][colIndex]);
                }
            }



            //t = new DateTime(2008, t.Month, t.Day);
            //string sql = "Date = '" + t.ToString("yyyy-MM-dd") + "'";
            //var rows = summaryHydrographTable.Select(sql);
            //if (rows.Length == 1)
            //{
            //    double val = Convert.ToDouble(rows[0][" "+exceedancePercent.ToString() + "%"]);
            //}

            throw new Exception("Error: Date not found " + t.ToString());

        }

        public static int LookupExceedance(DataTable monthlyTable, DateTime t, double mvcfsdays, out double exceedanceValue)
        {
            int rval = Convert.ToInt32(monthlyTable.Columns[monthlyTable.Columns.Count - 1].ColumnName); // 95
            exceedanceValue = 0;

            for (int i = 0; i < monthlyTable.Rows.Count; i++)
            {
                DateTime d = Convert.ToDateTime(monthlyTable.Rows[i]["Datetime"]);
                if (d.Month == t.Month)
                {
                    for (int j = 1; j < monthlyTable.Columns.Count; j++)
                    {
                        object o = monthlyTable.Rows[i][j];
                        if (o == DBNull.Value)
                            continue;

                        double val = Convert.ToDouble(o);
                        exceedanceValue = val;

                        if (mvcfsdays >= val)
                        {
                            rval = Convert.ToInt32(monthlyTable.Columns[j].ColumnName);
                            return rval;
                        }
                    }
                }
            }

            return rval;
        }
    }
}
