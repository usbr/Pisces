using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Analysis
{
    public class SummaryHydrographAnalysis : BaseAnalysis 
    {
        public SummaryHydrographAnalysis(PiscesSettings explorer):base(explorer)
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
                YearRange yearRng = new YearRange(Explorer.PlotYear, Explorer.BeginningMonth);
                DateTime t1 = yearRng.DateTime1;
                DateTime t2 = yearRng.DateTime2;

                Series s = Math.Subset(list[0], t1, t2);
                s.Appearance.LegendText = yearRng.Year.ToString();
                view.Messages.Add(yearRng.Year.ToString() + " included as separate series ");
                myList.Add(s);
                myList.Add(list.SummaryHydrograph(Explorer.ExceedanceLevels, t1,
                    Explorer.PlotMax, Explorer.PlotMin, Explorer.PlotAvg,true));//,true));
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
