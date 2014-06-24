using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Analysis
{
    public class CorrelationAnalysis:BaseAnalysis 
    {

        public CorrelationAnalysis(PiscesSettings explorer):base(explorer)
        {
            m_analysisType = AnalysisType.Correlation;
            m_name = "Correlation";
            Description = "Correlation between two items. You must selected exactly two items";
        }

        public override IExplorerView Run()
        {
            SeriesList list = Explorer.CreateSelectedSeries();

            if (list.Count != 2)
            {
                string msg = "Correlation Graph requires exactly two series, there are " + list.Count + " series selected";
                view.Messages.Add(msg);
                Logger.WriteLine(msg);
                view.SeriesList.Clear();
                view.DataTable = new DataTable();
                return view;
            }

            ReadSeriesList(list);

            string title = list.Text.TitleText();
            string subTitle = list.MissingRecordsMessage;

            list.RemoveMissing();

            SeriesList myList = list.Subset(Explorer.MonthDayRange);

            myList = myList.AggregateAndSubset(Explorer.StatisticalMethods, Explorer.MonthDayRange, Explorer.BeginningMonth);

            view.Title = title;
            view.SubTitle = subTitle;
            view.SeriesList = myList;
            return view;
        }
    }
}
