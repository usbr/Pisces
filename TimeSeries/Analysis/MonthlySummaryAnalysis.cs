using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Analysis
{
    public class MonthlySummaryAnalysis : BaseAnalysis
    {
        /// <summary>
        /// A monthly summary of daily data 
        /// displays the min,max,mean, and count for each month.
        /// </summary>
        /// <param name="explorer"></param>
        public MonthlySummaryAnalysis(PiscesSettings explorer)
            : base(explorer)
        {
            m_analysisType = AnalysisType.MonthlySummary;
            m_name = "Monthly Summary";
            Description =
     "A monthly summary of daily data.  Either multi years or condensed to 12 values.\n"
      +"Displays the min,max,mean, and count" ;
        }

        //private SeriesList list;
        //SeriesList summaryList;
        public override IExplorerView Run()
        {
            Logger.WriteLine("MonthlySummary.Run()");
            var list = Explorer.CreateSelectedSeries();

            ReadSeriesList(list);

            DataTable summaryTable = MonthlySummary(list, Explorer.StatisticalMethods,Explorer.MultiYearMonthlyAggregate);

            
            var summaryList = CreateSummaryList(list,summaryTable, Explorer.StatisticalMethods);

            view.SeriesList = summaryList;

            view.Title = "Monthly Summary " + list.Text.TitleText();
            view.SubTitle = summaryList.MissingRecordsMessage;
            view.DataTable = summaryTable;
            
            return view;
        }


        

        private static SeriesList CreateSummaryList(SeriesList list ,DataTable summaryTable, StatisticalMethods sm)
        {
            var summaryList = new SeriesList();

            int tableIndex = 1;
            for (int seriesIndex = 0; seriesIndex < list.Count; seriesIndex++)
            {
                if ((sm & StatisticalMethods.Min) == StatisticalMethods.Min)
                    summaryList.Add(CreateSeries(list,summaryTable, seriesIndex, tableIndex++));

                if ((sm & StatisticalMethods.Max) == StatisticalMethods.Max)
                    summaryList.Add(CreateSeries(list,summaryTable, seriesIndex, tableIndex++));

                if ((sm & StatisticalMethods.Mean) == StatisticalMethods.Mean)
                    summaryList.Add(CreateSeries(list,summaryTable, seriesIndex, tableIndex++));

                if ((sm & StatisticalMethods.Count) == StatisticalMethods.Count)
                    summaryList.Add(CreateSeries(list,summaryTable, seriesIndex, tableIndex++));

                if ((sm & StatisticalMethods.Sum) == StatisticalMethods.Sum)
                    summaryList.Add(CreateSeries(list,summaryTable, seriesIndex, tableIndex++));
            }

            return summaryList;
        }

        private static Series CreateSeries(SeriesList list, DataTable summaryTable, int seriesIndex, int tableIndex)
        {
            Series s = Series.SeriesFromTable(summaryTable, 0, tableIndex);
            //Series.CopyAttributes(list[seriesIndex], s);
            s.Units = list[seriesIndex].Units;
            s.Parameter = list[seriesIndex].Parameter;
            s.TimeInterval = TimeInterval.Monthly;
            //summaryList.Add(s);
            return s;
        }


        //public static Series MonthlySeries(Series daily, StatisticalMethods sm)
        //{
        //    SeriesList lst = new SeriesList();
        //    lst.Add(daily);
        //    DataTable tbl = MonthlySummary(lst, sm);
        //    return CreateSeries(lst, tbl, 0, 1);
        //}

        /// <summary>
        /// Compute monthly statisitcs
        /// </summary>
        static DataTable MonthlySummary(SeriesList list, StatisticalMethods sm, bool multiYear )
        {
            DataTable tbl = CreateMonthlySummaryTable(list,sm,multiYear);

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                DateTime t = Convert.ToDateTime(tbl.Rows[i][0]);
                DateRange dr = new DateRange(
                  new DateTime(t.Year, t.Month, 1),
                  new DateTime(t.Year, t.Month,
                   DateTime.DaysInMonth(t.Year, t.Month), 23, 59, 59));

                int[] months = new int[]{t.Month};


                for (int j = 0; j < list.Count; j++)
                {
                    Series subset;
                    if( multiYear)
                      subset = Math.Subset(list[j], dr);
                    else
                      subset = Math.Subset(list[j], months);

                    if( (sm & StatisticalMethods.Min) == StatisticalMethods.Min )
                         tbl.Rows[i][ m_minColumnIndex[j]] = Math.MinPoint(subset).Value;

                    if( (sm & StatisticalMethods.Max) == StatisticalMethods.Max )
                         tbl.Rows[i][ m_maxColumnIndex[j]] = Math.MaxPoint(subset).Value;

                     if ((sm & StatisticalMethods.Mean) == StatisticalMethods.Mean)
                         tbl.Rows[i][m_meanColumnIndex[j]] = Math.AverageOfSeries(subset);

                     if ((sm & StatisticalMethods.Count ) == StatisticalMethods.Count)
                         tbl.Rows[i][m_countColumnIndex[j]] = Math.Count(subset);

                     if ((sm & StatisticalMethods.Sum) == StatisticalMethods.Sum)
                         tbl.Rows[i][m_sumColumnIndex[j]] = Math.Sum(subset);
                }
            }

            return tbl;
        }

       static List<int> m_minColumnIndex = new List<int>();
       static List<int> m_maxColumnIndex = new List<int>();
       static List<int> m_meanColumnIndex = new List<int>();
       static List<int> m_sumColumnIndex = new List<int>();
       static List<int> m_countColumnIndex = new List<int>();
        


        /// <summary>
        /// Creates a table with place holders for each series and statisitcal method as a column
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sm"></param>
        /// <returns></returns>
       private static DataTable CreateMonthlySummaryTable(SeriesList list, StatisticalMethods sm, bool multiYear)
        {

            DateTime t1 = list.MinDateTime;
            DateTime t2 = list.MaxDateTime;

            m_minColumnIndex.Clear();
            m_maxColumnIndex.Clear();
            m_meanColumnIndex.Clear();
            m_sumColumnIndex.Clear();
            m_countColumnIndex.Clear();
        

            
            DataTable tbl = new DataTable("Summary");
            tbl.Columns.Add("DateTime", typeof(DateTime));
            var  textHelp = new SeriesListText(list);
            for (int i = 0; i < list.Count; i++)
            {
                // string s = list[i].Appearance.LegendText;
                var s = textHelp.Text[i];

                if ((sm & StatisticalMethods.Min) == StatisticalMethods.Min)
                {
                    tbl.Columns.Add("Min " + s, typeof(double));
                    m_minColumnIndex.Add( tbl.Columns.Count - 1);
                }

                if ( (sm & StatisticalMethods.Max) == StatisticalMethods.Max)
                {
                    tbl.Columns.Add("Max " + s, typeof(double));
                    m_maxColumnIndex.Add(tbl.Columns.Count - 1);
                }
                if ( (sm & StatisticalMethods.Mean) == StatisticalMethods.Mean)
                {
                    tbl.Columns.Add("Mean " + s, typeof(double));
                    m_meanColumnIndex.Add(tbl.Columns.Count - 1);
                }
                if (( sm & StatisticalMethods.Sum )== StatisticalMethods.Sum)
                {
                    tbl.Columns.Add("Sum " + s, typeof(double));
                    m_sumColumnIndex.Add(tbl.Columns.Count - 1);
                }
                if ((sm  & StatisticalMethods.Count) == StatisticalMethods.Count)
                {
                    tbl.Columns.Add("Count " + s, typeof(int));
                    m_countColumnIndex.Add(tbl.Columns.Count - 1);
                }
            }

            InsertDates(t1, t2, tbl, multiYear );
            return tbl;
        }

        /// <summary>
        /// Inserts rows with "DateTime" column populated with dates 
        /// (incremented monthly) in the range [t1,  t2 )
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="tbl"></param>
       private static void InsertDates(DateTime t1, DateTime t2, DataTable tbl, bool multiYear)
        {
            DateTime t = new DateTime(t1.Year, t1.Month, 1);
            while (t < t2)
            {
                DateRange dr = new DateRange(
                                  new DateTime(t.Year, t.Month, 1),
                                  new DateTime(t.Year, t.Month,
                                   DateTime.DaysInMonth(t.Year, t.Month), 23, 59, 59)
                                   );
                DataRow row = tbl.NewRow();
                row["DateTime"] = dr.DateTime2;
                tbl.Rows.Add(row);
                t = t.AddMonths(1);
                if (!multiYear && t.Month == t1.Month)
                    break;
            }
        }
    }
}
