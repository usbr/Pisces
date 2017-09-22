using System;
using System.Data;
using Reclamation.Core;
using Reclamation.TimeSeries.Analysis;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
namespace Reclamation.TimeSeries
{
    public enum AnalysisType
    {
        TimeSeries, Probability,
        Exceedance, WaterYears, SummaryHydrograph,
        Correlation, MonthlySummary, MovingAverage, TraceAnalysis
    };

    /// <summary>
    /// PiscesEngine performs analysis and displays results 
    /// to IExplorerView
    /// </summary>
    public class PiscesEngine
    {

        private bool m_thirtyYearAverage;

        public bool ThirtyYearAverage
        {
            get { return m_thirtyYearAverage; }
            set { m_thirtyYearAverage = value; }
        }

        private bool m_multiYearMonthlyAggregate;
        public bool MultiYearMonthlyAggregate
        {
            get { return m_multiYearMonthlyAggregate; }
            set { m_multiYearMonthlyAggregate = value; }
        }

        private TimeSeriesDatabase m_db;

        public PiscesSettings Settings
        {
            get { return m_db.Settings; }
        }
        

        public PiscesEngine(IExplorerView view,string fileName)
        {
            this.m_view = view;
            Open(fileName);
        }


        public void Open(string path)
        {
            var svr = new SQLiteServer(path);
            Connect(svr);
        }

        public void Connect(BasicDBServer svr)
        {
            m_db = new TimeSeriesDatabase(svr,false);
            Defaults(m_db);
        }


        public TimeSeriesDatabase Database
        {
            get { return m_db; }
        }

        bool m_subtractFromBaseline;

        public bool SubtractFromBaseline
        {
            get { return m_subtractFromBaseline; }
            set { m_subtractFromBaseline = value; }
        }

        /// <summary>
        /// When Subtract from Baseline is checked  include baseline series in output
        /// </summary>
        public bool IncludeBaseline { get; set; }

        /// <summary>
        /// When Subtract from Baseline is checked  include selected series in output
        /// </summary>
        public bool IncludeSelected { get; set; }

        /// <summary>
        /// Merge single year(or shorter) traces into a single series.
        /// </summary>
        public bool MergeSelected { get; set; }

        IExplorerView m_view;

        public IExplorerView View
        {
            get { return m_view; }
            set
            {
                m_view = value;
            }
        }

        private MonthDayRange _monthDayRange;

        public MonthDayRange MonthDayRange
        {
            get { return _monthDayRange; }
            set { _monthDayRange = value; }
        }

        private StatisticalMethods aggregate;

        public StatisticalMethods StatisticalMethods
        {
            get { return aggregate; }
            set { aggregate = value; }
        }
        private int WaterYearBeginningMonth;

        public int BeginningMonth
        {
            get { return WaterYearBeginningMonth; }
            set { WaterYearBeginningMonth = value; }
        }
        public int[] WaterYears;
        public int[] ExceedanceLevels;
        public bool AlsoPlotYear;
        public bool AlsoPlotTrace;
        public bool traceExceedanceAnalysis;
        public bool traceAggregationAnalysis;
        public bool sumCYRadio;
        public bool sumWYRadio;
        public bool sumCustomRangeRadio;
        public int[] PlotYear;
        public string PlotTrace;
        public bool PlotMinTrace;
        public bool PlotMaxTrace;
        public bool PlotAvgTrace;
        public bool PlotMax;
        public bool PlotMin;
        public bool PlotAvg;

        // [Obsolete()]
        //public bool HasTraces; // modeling studies for each year.

        public AnalysisType SelectedAnalysisType;

        private Series[] m_selectedSeries;// { get; set; }

        private SeriesList previousSelectedSeries = new SeriesList();

        bool m_undoZoom = true;

        public bool UndoZoom
        {
            get { return m_undoZoom; }
        }

        public Series[] SelectedSeries
        {
            get { return m_selectedSeries; }
            set
            {
                m_undoZoom = true;
                CheckIfZoomLevelShouldRemain(value);

                m_selectedSeries = value;

                for (int i = 0; i < previousSelectedSeries.Count; i++)
                {
                    previousSelectedSeries[i].Clear();
                }

                previousSelectedSeries.Clear();
                for (int i = 0; i < value.Length; i++)
                {
                    previousSelectedSeries.Add(value[i]);
                }
            }
        }

        private void CheckIfZoomLevelShouldRemain(Series[] value)
        {
            for (int i = 0; i < value.Length; i++)
            {
                if (previousSelectedSeries.Contains(value[i]))
                {
                    m_undoZoom = false;
                    break;
                }

            }
        }

        public TimeWindow TimeWindow;

        public bool AllowCustomScenarioTable = false;

        public event CustomScenarioEventHandler OnCustomizeScenarioTable;

        public void CustomizeScenarioTable(ScenarioTableEventArgs ea)
        {
            if (OnCustomizeScenarioTable != null)
            {
                OnCustomizeScenarioTable(this, ea);
            }
        }

        AnalysisCollection m_analysisList;

        public AnalysisCollection AnalysisList
        {
            get { return m_analysisList; }
            //set { m_analysisList = value; }
        }

        public BaseAnalysis SelectedTimeSeriesAnalysis
        {
            get { return AnalysisList[SelectedAnalysisType]; }
        }

        private void Defaults(TimeSeriesDatabase db)
        {
            //  m_siteCatalog = sc;
            m_db = db;
            //ScenarioNames = m_db.Scenario;

            m_thirtyYearAverage = false;
            m_selectedSeries = new Series[] { };
            m_analysisList = new AnalysisCollection(this);
            m_multiYearMonthlyAggregate = true;
//            HasTraces = false;
            _monthDayRange = new MonthDayRange(10, 1, 9, 30);
            this.StatisticalMethods = StatisticalMethods.None;
            WaterYearBeginningMonth = 10;// october
            WaterYears = new int[] { DateTime.Now.Year - 5, DateTime.Now.Year - 4, DateTime.Now.Year - 3, DateTime.Now.Year - 2, DateTime.Now.Year - 1, DateTime.Now.Year };
            ExceedanceLevels = new int[] { 10, 50, 90 };
            AlsoPlotYear = false;
            AlsoPlotTrace = false;
            PlotYear = new int[] { DateTime.Now.Year };
            PlotTrace = "";
            PlotMin = false;
            PlotAvg = false;
            PlotMax = false;
            traceExceedanceAnalysis = true;
            traceAggregationAnalysis = false;
            sumCYRadio = true;
            sumWYRadio = false;
            sumCustomRangeRadio = false;
            PlotMax = false;
            PlotMin = false;
            PlotAvg = false;
            TimeWindow = new TimeWindow();
            SelectedAnalysisType = AnalysisType.TimeSeries;
            //SelectedScenarios = new string[] { };
            m_subtractFromBaseline = false;
        }

        internal SeriesList CreateSelectedSeries()
        {
            Logger.WriteLine("Begin CreateSelectedSeries()");
            
            TimeSeries.SeriesList rval = new TimeSeries.SeriesList();
            var selScenarios = m_db.GetSelectedScenarios();
            Logger.WriteLine("There are " + m_selectedSeries.Length + " series selected");
            Logger.WriteLine("There are "+selScenarios.Rows.Count+" scenarios selected");

            for (int i = 0; i < m_selectedSeries.Length; i++)
            {
                Series s = m_selectedSeries[i];
                Logger.WriteLine("Series ["+i+"] " + s.Name);
                Logger.WriteLine(" ScenarioName: "+s.ScenarioName);

                if (s.ScenarioName == "" || (selScenarios.Rows.Count == 0 && !IncludeBaseline ))
                {
                    Logger.WriteLine("Not using Scenario. Either blank scenario name, or no scenarios (unless baseline selected)");
                    rval.Add(s);
                }
                else if (selScenarios.Rows.Count == 0 && IncludeBaseline)
                {
                    // baseline only.
                    Logger.WriteLine("Not using Scenario. baseline only");
                    var baseline = s.CreateBaseline();
                    if (!baseline.SiteID.Contains("reference"))
                        baseline.SiteID += " - reference";
                    rval.Add(baseline);
                }
                else// Using Scenarios.
                {
                    Logger.WriteLine("Using scenarios");
                    Series baseline = null;
                    foreach (var sn in selScenarios)
                    {
                        
                        Series scenario = s.CreateScenario(sn);
                        Logger.WriteLine("Scenario: " + scenario.ScenarioName);
                        if (baseline == null && (IncludeBaseline || SubtractFromBaseline))
                            baseline = s.CreateBaseline();

                        if (SubtractFromBaseline)
                        {
                            SubtractSeries impact = new SubtractSeries(scenario, baseline);
                            impact.Name = scenario.Name + " - " + baseline.Name;
                            impact.Appearance.LegendText = "(" + scenario.Appearance.LegendText
                                  + ") - (" + baseline.Appearance.LegendText + ")";
                            impact.SiteID = baseline.SiteID;

                            rval.Add(impact);

                            if (IncludeSelected)
                                rval.Add(scenario);
                        }
                        else
                        {
                            rval.Add(scenario);
                        }
                    }

                    if (IncludeBaseline && baseline != null)
                        rval.Add(baseline);
                }
            }

            Logger.WriteLine("Finish CreateSelectedSeries()");
            return rval;

        }


        //private Series LookupBaseline(SeriesList list, string baseline, string name)
        //{
        //    foreach (Series s in list)
        //    {
        //        if (s.ScenarioName == baseline
        //            && s.Name == name)
        //            return s;
        //    }

        //    throw new Exception("could not find Scenario named '" + baseline + "' and series named '" + name + "'");
        //}

        /// <summary>
        /// Run Selected Analysis which reads time series data
        /// </summary>
        /// <returns></returns>
        public void Run()
        {
            View.MultipleYAxis = m_db.Settings.ReadBoolean("MultipleYAxis", false);


            if (m_db.Settings.ReadBoolean("VerboseLogging", false))
                Logger.EnableLogger(true);

            View.Messages.Clear();
            View.AnalysisType = SelectedAnalysisType;
            View.MonthlySummaryMultiYear = this.MultiYearMonthlyAggregate;

            View.UndoZoom = this.UndoZoom;
            Logger.WriteLine(SelectedAnalysisType + ".Run()");
            SelectedTimeSeriesAnalysis.Run();

            if (View.SeriesList.Count == 0)
            {
                View.Title = "No data avaliable for the item and the time range selected.";
                View.SubTitle = "Try selecting another item or changing the period of record in the Analysis menu";

            }
            Logger.WriteLine("Ready");
        }

        public void WriteProgressMessage(string message, int percent)
        {
            Logger.WriteLine(message);
            if (OnProgress != null)
            {
                OnProgress(this, new ProgressEventArgs(message, percent));
            }
        }
        private void FireOnProgress(string message, int percentComplete)
        {
            Logger.WriteLine(message);
            if (OnProgress != null)
            {
                OnProgress(this, new ProgressEventArgs(message, percentComplete));
            }

        }

        public event ProgressEventHandler OnProgress;

        private bool m_24hr = true;

        public bool PlotMoving24HourAverage
        {
            get { return m_24hr; }
            set { m_24hr = value; }
        }
        private bool m_120hr = false;

        public bool PlotMoving120HourAverage
        {
            get { return m_120hr; }
            set { m_120hr = value; }
        }
        private bool m_raw = true;

        public bool PlotRaw
        {
            get { return m_raw; }
            set { m_raw = value; }
        }
    }
}