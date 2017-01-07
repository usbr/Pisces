using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Analysis
{
    public class MovingAverageAnalysis : BaseAnalysis
    {
        public MovingAverageAnalysis(PiscesEngine explorer)
            : base(explorer)
        {
            m_analysisType = AnalysisType.MovingAverage;
            m_name = "Moving Average";
            Description = "Moving average";
        }

        public override IExplorerView Run()
        {
            Logger.WriteLine("MovingAverageAnalysis.Run()");
            SeriesList list = Explorer.CreateSelectedSeries();

            ReadSeriesList(list);
            view.Messages.Add(list.MissingRecordsMessage);

            
            SeriesList myList = new SeriesList();

            for (int i = 0; i < list.Count; i++)
            {
                if (Explorer.PlotRaw)
                {
                    myList.Add(list[i]);
                }
                if (Explorer.PlotMoving24HourAverage)
                {
                    Series s24 = Math.MovingAvearge(list[i], 24);
                    myList.Add(s24);
                }
                if (Explorer.PlotMoving120HourAverage)
                {
                    Series s120 = Math.MovingAvearge(list[i], 120);
                    myList.Add(s120);
                }
            }
            view.Title = "Moving Average\n" + list.Text.TitleText();
            view.SubTitle = list.MissingRecordsMessage;

            view.SeriesList = myList;
            view.DataTable = myList.ToDataTable(true);
            return view;
        }


    }
}
