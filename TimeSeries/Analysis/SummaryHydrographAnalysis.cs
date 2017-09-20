using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Analysis
{
    public class SummaryHydrographAnalysis : BaseAnalysis 
    {
        public SummaryHydrographAnalysis(PiscesEngine explorer):base(explorer)
        {
            m_analysisType = AnalysisType.SummaryHydrograph;
            m_name = "Summary Hydrograph";
            Description = 
                "A summary hydrograph is a statistical hydrograph.\n"
                +"Consider using a monthly summary if you don’t need a statistic for each day in the year\n"
                +"\n\nNote: February 29th is not included.";
        }

        public override IExplorerView Run()
        {
            Logger.WriteLine("SummaryHydrographAnalysis.Run()");
            SeriesList list = Explorer.CreateSelectedSeries();

            ReadSeriesList(list);

            
            if (Explorer.SelectedSeries.Length == 1 && Explorer.MergeSelected)
            { // merge single Year Traces.
                list.RemoveMissing();
                var s = list.MergeYearlyScenarios();
                list = new SeriesList();
                list.Add(s);
            }

            view.Messages.Add(list.MissingRecordsMessage);

            string title = list.Text.TitleText();
            string subTitle = list.MissingRecordsMessage;



            SeriesList myList = new SeriesList();
            list.RemoveMissing();

            if (Explorer.AlsoPlotYear && list.Count == 1)
            {
                int[] yearsToPlot = Explorer.PlotYear;
                int xtraYearCount = 0;
                DateTime tSumHyd1 = DateTime.Now;
                DateTime tSumHyd2 = DateTime.Now;
                foreach (var year in yearsToPlot)
                {
                    YearRange yearRng = new YearRange(year, Explorer.BeginningMonth);
                    DateTime t1 = yearRng.DateTime1;
                    DateTime t2 = yearRng.DateTime2;
                    Series s = Math.Subset(list[0], t1, t2);

                    if (xtraYearCount == 0)//first series
                    {
                        s.Appearance.LegendText = yearRng.Year.ToString();
                        view.Messages.Add(yearRng.Year.ToString() + " included as separate series ");
                        myList.Add(s);
                        if (yearsToPlot.Length == 1)
                        {
                            myList.Add(list.SummaryHydrograph(Explorer.ExceedanceLevels, t1, Explorer.PlotMax, Explorer.PlotMin, Explorer.PlotAvg, true));
                        }
                        else
                        {
                            myList.Add(list.SummaryHydrograph(new int[] { }, t1, false, false, false, true));
                        }
                        tSumHyd1 = t1;
                        tSumHyd2 = t2;
                    }
                    else//every series
                    {
                        Series sDummy = new Series();
                        foreach (Point pt in s)
                        {
                            if (pt.DateTime.Month != 2 && pt.DateTime.Day != 29) //sigh... leap days...
                            {
                                sDummy.Add(pt.DateTime.AddYears(tSumHyd1.Year - t1.Year), pt.Value);
                            }
                        }
                        sDummy.Appearance.LegendText = yearRng.Year.ToString();
                        view.Messages.Add(yearRng.Year.ToString() + " included as separate series ");
                        myList.Add(sDummy);
                        if (xtraYearCount == yearsToPlot.Length - 1)//last series
                        {
                            myList.Add(list.SummaryHydrograph(Explorer.ExceedanceLevels, tSumHyd1, Explorer.PlotMax, Explorer.PlotMin, Explorer.PlotAvg, true));
                        }
                        else
                        {
                            myList.Add(list.SummaryHydrograph(new int[] { }, tSumHyd1, false, false, false, true));
                        }
                    }
                    xtraYearCount++;
                }
            }
            else
            {
                DateTime t = new DateTime(DateTime.Now.Year, Explorer.BeginningMonth, 1);
                myList = list.SummaryHydrograph(Explorer.ExceedanceLevels, t,
                    Explorer.PlotMax, Explorer.PlotMin, Explorer.PlotAvg,true);//,true);
            }
            
            Explorer.WriteProgressMessage("drawing graph", 80);
            view.Title = title;
            view.SubTitle = subTitle;
            view.SeriesList = myList;
            view.DataTable = myList.ToDataTable(true);
            //view.Draw();
            return view;

        }
    }
}
