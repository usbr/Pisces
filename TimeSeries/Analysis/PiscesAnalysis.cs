using System;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Analysis
{
    public class PiscesAnalysis
    {

        /// <summary>
        /// Creates a list of water year based data all aligned to year 2000
        /// to allow comparison.
        /// </summary>
        /// <param name="list">intput series</param>
        /// <param name="years">water years</param>
        /// <param name="avg30yr">when true also includes 30 year average. </param>
        /// <param name="beginningMonth">series starting month number</param>
        /// <returns></returns>
        public static SeriesList WaterYears(SeriesList list, int[] years, bool avg30yr, 
            int beginningMonth, bool alwaysShiftTo2000,DateTime? startOf30YearAvearge = null)
        {

            SeriesList wySeries = new SeriesList();
            for (int j = 0; j < list.Count; j++)
            {
                for (int i = 0; i < years.Length; i++)
                {
                    YearRange yr = new YearRange(years[i], beginningMonth);
                    Series s = list[j];
                    s.Clear();
                    s.Read(yr.DateTime1, yr.DateTime2);

                    Logger.WriteLine("Read() " + yr.ToString() + " count = " + s.Count);

                    foreach (string msg in s.Messages)
                    {
                        Logger.WriteLine(msg);
                    }
                    if (s.Count > 0 && s.CountMissing() != s.Count)
                    {
                        Series s2 = TimeSeries.Math.ShiftToYear(s, 2000);
                        if (years.Length == 1 && !alwaysShiftTo2000 && !avg30yr)
                        {
                            s2 = s;
                        }
                        if (list.HasMultipleSites)
                            s2.Appearance.LegendText = years[i].ToString() + "   " + list[j].Name;
                        else
                            s2.Appearance.LegendText = years[i].ToString();
                        wySeries.Add(s2);
                    }
                    else
                    {
                        Logger.WriteLine("year :" + years[i] + "skipping series with no data " + s.Name + " " + s.Parameter);
                    }

                }
                if (avg30yr)
                {
                    DateTime start = DateTime.Now.Date.AddYears(-30);
                    if (startOf30YearAvearge.HasValue)
                        start = startOf30YearAvearge.Value;

                    DateTime end = start.AddYears(30);
                    list[j].Read(start,end);
                    Series s30 = Math.MultiYearDailyAverage( list[j], beginningMonth);
                    if (s30.Count > 0)
                        wySeries.Add(s30);
                }
            }
            wySeries.Type = SeriesListType.WaterYears;
            if (wySeries.Count > 1)
            {
                wySeries.DateFormat = "MM/dd";
            }

            return wySeries;
        }

    }
}
