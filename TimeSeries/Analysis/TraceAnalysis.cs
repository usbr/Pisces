using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Analysis
{
    public class TraceAnalysis : BaseAnalysis
    {
        public TraceAnalysis(PiscesSettings explorer):base(explorer)
        {
            m_analysisType = AnalysisType.TraceAnalysis;
            m_name = "Trace Analysis";
            Description =
                "This anlysis looks into the different relationships/statistical properties of a \n"
                + "dataset that result from modeling different traces/scenarios through a model.\n";
        }

        public override IExplorerView Run()
        {
            Logger.WriteLine("TraceAnalysis.Run()");
            SeriesList list = Explorer.CreateSelectedSeries();

            ReadSeriesList(list);
            string title = list.Text.TitleText();
            string subTitle = list.MissingRecordsMessage;

            // [JR] don't perform trace analysis if trace count < 10...
            if (list.Count < 10)
            {
                view.Messages.Add("Trace exceedance analysis is not available if trace count < 10");
                view.Title = title;
                view.SubTitle = subTitle;
                view.SeriesList = list;
                view.DataTable = list.ToDataTable(true);
                return view;
            }

            if (Explorer.SelectedSeries.Length == 1 && Explorer.MergeSelected)
            { // merge single Year Traces.
                list.RemoveMissing();
                var s = list.MergeYearlyScenarios();
                list = new SeriesList();
                list.Add(s);
            }

            view.Messages.Add(list.MissingRecordsMessage);

            SeriesList traceAnalysisList = new SeriesList();
            list.RemoveMissing();

            // [JR] add function here that allows users to do the exceedance values using the entire 
            // dataset as a whole...
            if (Explorer.traceExceedanceAnalysis)
            {
                traceAnalysisList = getTraceExceedances(list, Explorer.ExceedanceLevels, Explorer.AlsoPlotTrace, Explorer.PlotTrace);
            }

            // [JR] add other anlayses here
            if (Explorer.traceAggregationAnalysis)
            {
                string sumType = "";
                if (Explorer.sumCYRadio)
                { sumType = "CY"; }
                else if (Explorer.sumWYRadio)
                { sumType = "WY"; }
                else
                { }
                traceAnalysisList = getTraceSums(list, sumType);
            }

            Explorer.WriteProgressMessage("drawing graph", 80);
            view.Title = title;
            view.SubTitle = subTitle;
            view.SeriesList = traceAnalysisList;
            view.DataTable = traceAnalysisList.ToDataTable(true);
            //view.Draw();
            return view;

        }


        /// <summary>
        /// Build a SeriesList with the trace exceedances
        /// </summary>
        /// <param name="sListIn"></param>
        /// <param name="excLevels"></param>
        /// <param name="xtraTraceCheck"></param>
        /// <param name="xtraTrace"></param>
        /// <returns></returns>
        private SeriesList getTraceExceedances(SeriesList sListIn, int[] excLevels, bool xtraTraceCheck, int xtraTrace)
        {
            SeriesList traceAnalysisList = new SeriesList();
            
            List<int> sExcIdxs = new List<int>();
            foreach (var item in excLevels)
            { 
                traceAnalysisList.Add(new Series(item + "%Exceedance"));
                int excIdx = Convert.ToInt16(System.Math.Ceiling(sListIn.Count * (100.0 - Convert.ToDouble(item)) / 100.0));
                sExcIdxs.Add(excIdx);
            }

            var dTab = sListIn.ToDataTable(true);
            for (int i = 0; i < dTab.Rows.Count; i++)
            {
                var dRow = dTab.Rows[i];
                DateTime t = DateTime.Parse(dRow[0].ToString());
                var values = dRow.ItemArray;
                var valList = new List<double>();
                for (int j = 1; j < values.Length; j++)
                { valList.Add(Convert.ToDouble(values[j].ToString())); }
                valList.Sort();
                for (int j = 0; j < sExcIdxs.Count; j++)
                { traceAnalysisList[j].Add(t, valList[sExcIdxs[j]],"interpolated"); }

            }

            if (xtraTraceCheck)
            {
                if (xtraTrace < 1 || xtraTrace > sListIn.Count)
                {
                    view.Messages.Add("Select an additional trace that is between 1 and the total number of traces");
                    return sListIn;
                }
                else
                { traceAnalysisList.Add(sListIn[xtraTrace]); }
            }

            return traceAnalysisList;
        }


        /// <summary>
        /// Build a SeriesList with custom aggregation
        /// </summary>
        /// <param name="sListIn"></param>
        /// <param name="sumType"></param>
        /// <returns></returns>
        private SeriesList getTraceSums(SeriesList sListIn, string sumType)
        {
            SeriesList traceAnalysisList = new SeriesList();

            foreach (var s in sListIn)
            {
                if (sumType == "CY")
                {
                    traceAnalysisList.Add(Reclamation.TimeSeries.Math.AnnualSum(s,
                        new MonthDayRange(1, 1, 12, 31), 1));
                }
                else if (sumType == "WY")
                {
                    traceAnalysisList.Add(Reclamation.TimeSeries.Math.AnnualSum(s,
                        new MonthDayRange(10, 1, 9, 30), 10));
                }
                else
                { view.Messages.Add(""); }
            }
            return traceAnalysisList;
        }

    }
}
