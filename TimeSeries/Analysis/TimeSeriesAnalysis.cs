using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Analysis
{
    /// <summary>
    /// Basic Time Series Analysis
    /// this is a base class for Exceedance,Summary Hydrograph, etc..
    /// </summary>
    public class TimeSeriesAnalysis:BaseAnalysis
    {

        public TimeSeriesAnalysis(PiscesEngine explorer):base(explorer)
        {
            m_analysisType = AnalysisType.TimeSeries;
            Explorer = explorer;
            this.view = explorer.View;
            m_name = "Time Series";
            Description = "This option will graph a raw or basic summary of your data.";
        }
        
        

        public override IExplorerView Run()
        {
            SeriesList list = Explorer.CreateSelectedSeries();

            ReadSeriesList(list);
            if (Explorer.SelectedSeries.Length == 1 && Explorer.MergeSelected)
            { // merge single Year Traces.
                var s = list.MergeYearlyScenarios();
                list = new SeriesList();
                list.Add(s);
            }

            SeriesList myList = list;
            if (Explorer.StatisticalMethods != StatisticalMethods.None)
            {
                myList = list.AggregateAndSubset(Explorer.StatisticalMethods,
                    Explorer.MonthDayRange, Explorer.BeginningMonth);
            }

            Logger.WriteLine("Drawing Graph");

            if (myList.Count == 1 && myList[0].TimeInterval == TimeInterval.Monthly)
            {
                myList.DateFormat = "MMM-yyyy";
            }
            view.SeriesList = myList;
            string title = list.Text.TitleText();
            if (Explorer.SubtractFromBaseline)
                title = "Subtract Reference \n" + title;
            view.Title = title;
            view.SubTitle = list.MissingRecordsMessage;
            //view.DataTable = myList.CompositeTable;
            return view;
        }




        

       


    }
}
