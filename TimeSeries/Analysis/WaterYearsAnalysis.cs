using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Analysis
{
    public class WaterYearsAnalysis : BaseAnalysis
    {
        public WaterYearsAnalysis(PiscesSettings explorer) : base(explorer)
        {
            m_analysisType = AnalysisType.WaterYears;
            m_name = "Water Years";
            Description = "This option allows comparing different water or calendar years";
        }

        
        public override IExplorerView Run()
        {

            Logger.WriteLine("WaterYearsAnalysis.Run()");
            SeriesList list = Explorer.CreateSelectedSeries();
            // Note: we do not call ReadSeriesList(list)

            SeriesList wySeries = PiscesAnalysis.WaterYears(list,Explorer.WaterYears,Explorer.ThirtyYearAverage, Explorer.BeginningMonth);


            

            Explorer.WriteProgressMessage("drawing graph", 80);
            view.Title = list.Text.TitleText();
            view.SubTitle = list.MissingRecordsMessage;
            view.SeriesList = wySeries;
            view.DataTable = wySeries.ToDataTable(true);
            //view.Draw();
            return view;

        }

    }
}
