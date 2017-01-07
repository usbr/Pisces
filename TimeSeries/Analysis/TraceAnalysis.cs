using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Analysis
{
    public class TraceAnalysis : BaseAnalysis
    {
        public TraceAnalysis(PiscesEngine explorer):base(explorer)
        {
            m_analysisType = AnalysisType.TraceAnalysis;
            m_name = "Trace Analysis";
            Description =
                "This analysis option allows users to comparatively and/or statistically analyze "
                + " a dataset that contains several traces/scenarios which usually result from "
                + " iterative model runs with varying model inputs, assumptions, and conditions."
                + " \n\n"
                + "Trace analysis will not run if the selected number of traces < 10."
                + " \n";
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

            // This seems to be common between all the analysis options
            if (Explorer.SelectedSeries.Length == 1 && Explorer.MergeSelected)
            { // merge single Year Traces.
                list.RemoveMissing();
                var s = list.MergeYearlyScenarios();
                list = new SeriesList();
                list.Add(s);
            }
            view.Messages.Add(list.MissingRecordsMessage);
            list.RemoveMissing();

            // Initialize the output container
            SeriesList traceAnalysisList = new SeriesList();
            
            // Get exceedance curves 
            if (Explorer.traceExceedanceAnalysis)
            {
                traceAnalysisList = getTraceExceedances(list,
                    Explorer.ExceedanceLevels, Explorer.AlsoPlotTrace,
                    Explorer.PlotTrace, Explorer.PlotMinTrace,
                    Explorer.PlotAvgTrace, Explorer.PlotMaxTrace); 
            }

            // Get aggregated values
            if (Explorer.traceAggregationAnalysis)
            {
                string sumType = "";
                if (Explorer.sumCYRadio)
                { sumType = "CY"; }
                else if (Explorer.sumWYRadio)
                { sumType = "WY"; }
                else if (Explorer.sumCustomRangeRadio)
                { sumType = "XX"; }
                else
                { }
                traceAnalysisList = getTraceSums(list, sumType);
            }

            // [JR] Add other analysis/report building options here...

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
        private SeriesList getTraceExceedances(SeriesList sListIn, int[] excLevels, bool xtraTraceCheck, string xtraTrace,
            bool plotMinTrace, bool plotAvgTrace, bool plotMaxTrace)
        {
            SeriesList traceAnalysisList = new SeriesList();
            
            // Define the index numbers from the serieslist wrt the selected exceedance level
            List<int> sExcIdxs = new List<int>();
            foreach (var item in excLevels)
            {
                var sNew = new Series();
                sNew.TimeInterval = sListIn[0].TimeInterval;
                sNew.Units = sListIn[0].Units;
                sNew.ScenarioName = item + "%Exceedance";
                traceAnalysisList.Add(sNew);
                int excIdx;
                if (item > 50)
                { excIdx = Convert.ToInt16(System.Math.Ceiling(sListIn.Count * (100.0 - Convert.ToDouble(item)) / 100.0)); }
                else
                { excIdx = Convert.ToInt16(System.Math.Floor(sListIn.Count * (100.0 - Convert.ToDouble(item)) / 100.0)); }
                sExcIdxs.Add(excIdx);
            }

            // Add min trace if selected
            if (plotMinTrace)
            {
                var sNew = new Series();
                sNew.TimeInterval = sListIn[0].TimeInterval;
                sNew.Units = sListIn[0].Units;
                sNew.ScenarioName = "Min";
                traceAnalysisList.Add(sNew); 
                sExcIdxs.Add(0);
            }

            // Add max trace if selected
            if (plotMaxTrace)
            {
                var sNew = new Series();
                sNew.TimeInterval = sListIn[0].TimeInterval;
                sNew.Units = sListIn[0].Units;
                sNew.ScenarioName = "Max";
                traceAnalysisList.Add(sNew);
                sExcIdxs.Add(sListIn.Count - 1);
            }

            // Define average trace container
            var sAvg = new Series();
            sAvg.TimeInterval = sListIn[0].TimeInterval;
            sAvg.Units = sListIn[0].Units;
            sAvg.ScenarioName = "Avg"; 
            
            // Populate the output serieslist with the exceddance curves
            var dTab = sListIn.ToDataTable(true);
            for (int i = 0; i < dTab.Rows.Count; i++)
            {
                var dRow = dTab.Rows[i];
                DateTime t = DateTime.Parse(dRow[0].ToString());
                var values = dRow.ItemArray;
                // Put the ith timestep values in a C# List and sort by ascending
                var valList = new List<double>();
                var valSum = 0.0;
                for (int j = 1; j < values.Length; j++)
                { 
                    valList.Add(Convert.ToDouble(values[j].ToString()));
                    valSum += Convert.ToDouble(values[j].ToString());
                }
                valList.Sort();
                // Grab the index corresponding to the selected exceedance level and populate the output serieslist
                for (int j = 0; j < sExcIdxs.Count; j++)
                { traceAnalysisList[j].Add(t, valList[sExcIdxs[j]],"interpolated"); }
                // Populate the average trace series
                if (plotAvgTrace)
                { sAvg.Add(t, valSum / valList.Count, "interpolated"); }
            }

            // Add average trace if selected
            if (plotAvgTrace)
            { traceAnalysisList.Add(sAvg); }

            // Add an extra reference trace if defined
            if (xtraTraceCheck)
            {
                //xtraTrace contains the run name "Name"
                var scenarioTable = Explorer.Database.GetSelectedScenarios();
                var selectedScenarioRow = scenarioTable.Select("[Name] = '" + xtraTrace + "'")[0];
                int selectedIdx = scenarioTable.Rows.IndexOf(selectedScenarioRow);
                //scenariosTable.Rows.IndexOf(
                if (xtraTrace == "")
                { throw new Exception("Select an additional trace that is between 1 and the total number of traces"); }
                else
                { traceAnalysisList.Add(sListIn[selectedIdx]); }
            }

            return traceAnalysisList;
        }


        /// <summary>
        /// Build a SeriesList with custom aggregation
        /// </summary>
        /// <param name="sListIn"></param>
        /// <param name="sumType"></param>
        /// <returns></returns>
        private SeriesList getTraceSums(SeriesList sListIn, string aggType)
        {
            SeriesList traceAnalysisList = new SeriesList();
            
            foreach (var s in sListIn)
            {
                var sNew = new Series();
                if (aggType == "CY")
                {
                    sNew = Reclamation.TimeSeries.Math.AnnualSum(s,
                        new MonthDayRange(1, 1, 12, 31), 1);
                }
                else if (aggType == "WY")
                {
                    sNew = Reclamation.TimeSeries.Math.AnnualSum(s,
                        new MonthDayRange(10, 1, 9, 30), 10);
                }
                else if (aggType == "XX")
                {
                    sNew = Reclamation.TimeSeries.Math.AnnualSum(s,
                        Explorer.MonthDayRange, Explorer.MonthDayRange.Month1);
                }
                else
                { view.Messages.Add(""); }
                sNew.TimeInterval = s.TimeInterval;
                sNew.Units = s.Units;
                traceAnalysisList.Add(sNew);
            }
            return traceAnalysisList;
        }

    }
}
