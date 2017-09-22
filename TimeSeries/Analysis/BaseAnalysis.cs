using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Analysis
{
    /// <summary>
    /// To add a new analysis type inherit from BaseAnalysis
    /// </summary>
    public abstract class BaseAnalysis
    {


        private PiscesEngine m_explorer;

        protected PiscesEngine Explorer
        {
            get { return m_explorer; }
            set { m_explorer = value; }
        }
        protected TimeSeries.IExplorerView view;
        protected string m_name;
        private string m_description;
        private object m_userInterface;

        public object UserInterface
        {
            get { return m_userInterface; }
            set { m_userInterface = value; }
        }
        
        protected AnalysisType m_analysisType;

        protected BaseAnalysis(PiscesEngine explorer)
        {
            m_explorer = explorer;
            view = explorer.View;
        }
        public string Name
        {
            get { return m_name; }
            //set { m_Name = value; }
        }

        public AnalysisType AnalysisType
        {
            get { return m_analysisType; }
            set { m_analysisType = value; }
        }

        public IExplorerSettings ExplorerSettings
        {
            get {
                IExplorerSettings i = UserInterface as IExplorerSettings;
                return i;
            }
           // set { m_explorerSettings = value; }
        }

        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        public abstract IExplorerView Run();


        /// <summary>
        /// read based on period of record options
        /// series with no data after reading are removed
        /// from the list.
        /// </summary>
        protected void ReadSeriesList(SeriesList list)
        {
            Logger.WriteLine("reading data for " + list.Count + " items ");
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Messages.Clear();
            }

            //list.Read(TimeWindow);

            if (Explorer.TimeWindow.WindowType == TimeWindowType.FullPeriodOfRecord)
            {
                list.Read();
            }
            else
            {
                list.Read(Explorer.TimeWindow.T1, Explorer.TimeWindow.T2);
            }
            return;
        }


        /// <summary>
        /// Used by Exceedance and Probability
        /// </summary>
        /// <param name="sortType"></param>
        protected void SortAndRank(RankType sortType)
        {

          //  Logger.WriteLine("Settings.HasTraces: " + Explorer.HasTraces);
            SeriesList list = Explorer.CreateSelectedSeries();

            ReadSeriesList(list);
            list.RemoveMissing();
            if (Explorer.SelectedSeries.Length == 1 && Explorer.MergeSelected )
            { // merge single Year Traces.
                var s = list.MergeYearlyScenarios();
                list = new SeriesList();
                list.Add(s);
            }
            Explorer.WriteProgressMessage("SubSet using Month Range " + Explorer.MonthDayRange.ToString(), 10);
            SeriesList myList = list.Subset(Explorer.MonthDayRange);

            Performance p = new Performance();
            myList = myList.AggregateAndSubset(Explorer.StatisticalMethods,
                                   Explorer.MonthDayRange, Explorer.BeginningMonth);//, Explorer.HasTraces);
            p.Report("kt");

            //if (Explorer.HasTraces && Explorer.StatisticalMethods != StatisticalMethods.None)
            //{
            //    // combine each 'year/scenario' into a single series 
            //    myList = myList.Merge();
            //}
            SeriesList proababilityList = new SeriesList();
            foreach (Series s in myList)
            {
                s.Appearance.Style = Styles.Line;
                proababilityList.Add(Math.Sort(s, sortType));
            }

            Explorer.WriteProgressMessage("drawing graph", 86);

            view.Title = Explorer.MonthDayRange.ToString() + " " + list.Text.TitleText();

            proababilityList.Type = SeriesListType.Sorted;
            view.SubTitle = list.MissingRecordsMessage;
            
            view.SeriesList = proababilityList;
        }
    }
}
