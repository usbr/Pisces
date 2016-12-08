using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Graphing
{
    public partial class TimeSeriesZedGraph : UserControl,ITimeSeriesGraph
    {
        double _missingDataValue;
        SeriesList seriesList;
        string title = "";
        string subTitle = "";

        public TimeSeriesZedGraph()
        {
            InitializeComponent();
            seriesList = new SeriesList();
        }


        public double MissingDataValue
        {
            get { return _missingDataValue; }
            set { _missingDataValue = value; }
        }
        public SeriesList Series
        {
            get { return this.seriesList; }
            set { this.seriesList = value; }
        }


        private AnalysisType analysisType;
        public AnalysisType AnalysisType
        {
            set { this.analysisType = value; }
        }

        /// <summary>
        /// remove all series from the graph.
        /// </summary>
        public void Clear()
        {
            seriesList.Clear();
            title = "";
            subTitle = "";
            Draw(true);
        }

        public void Draw(bool undoZoom)
        {
            ZedGraphDataLoader loader = new ZedGraphDataLoader(chart1);
            
            switch (analysisType)
            {
                case AnalysisType.TimeSeries:
                    loader.DrawTimeSeries(seriesList, title, subTitle, undoZoom, m_multiLeftAxis);
                    break;
                case AnalysisType.Exceedance:
                    loader.DrawSorted(seriesList, title, subTitle, "Percent Exceedance");
                    break;
                case AnalysisType.Probability:
                    loader.DrawSorted(seriesList, title, subTitle, "Percent");
                    break;
                case AnalysisType.WaterYears:
                    loader.DrawWaterYears(seriesList, title, subTitle, m_multiLeftAxis);
                    break;
                case AnalysisType.SummaryHydrograph:
                    loader.DrawWaterYears(seriesList, title, subTitle);
                    break;
                case AnalysisType.Correlation:
                    if (seriesList.Count == 2)
                        loader.DrawCorrelation(seriesList[0], seriesList[1], title, subTitle);
                    else
                        loader.Clear();
                    break;
                case AnalysisType.MonthlySummary:
                    loader.DrawTimeSeries(seriesList, title, subTitle, undoZoom, m_multiLeftAxis, m_monthlySummaryMultiYear);
                    break;
                case AnalysisType.MovingAverage:
                    loader.DrawTimeSeries(seriesList, title, subTitle, undoZoom);
                    break;
                case AnalysisType.TraceAnalysis:
                    loader.DrawTimeSeries(seriesList, title, subTitle, undoZoom, m_multiLeftAxis);
                    break;
                default:
                    loader.Clear();
                    break;
            }


        }



        bool m_multiLeftAxis = false;

        public bool MultiLeftAxis
        {
            get { return m_multiLeftAxis; }
            set { m_multiLeftAxis = value; }
        }

        bool m_monthlySummaryMultiYear = true;
        public bool MonthlySummaryMultiYear 
        {
            get { return m_monthlySummaryMultiYear; }
            set { m_monthlySummaryMultiYear = value; }
        }


        public void Add(Series s)
        {
            seriesList.Add(s);
        }

        public string SubTitle
        {
            get { return this.subTitle; }
            set { this.subTitle = value; }
        }
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            chart1.SaveAs();
        }

        private void toolStripButtonPrint_Click(object sender, EventArgs e)
        {
            chart1.DoPrintPreview();
        }

        private void toolStripButtonUndoZoom_Click(object sender, EventArgs e)
        {
            chart1.ZoomOutAll(chart1.GraphPane);
            RefreshGraph();   
        }

        private void toolStripButtonZoomOut_Click(object sender, EventArgs e)
        {
            var pane = chart1.GraphPane;
            var centerPt = new PointF(chart1.Size.Width / 2, chart1.Size.Height / 2);
            double zoomFraction = (1 + 1.0 * chart1.ZoomStepFraction);
            chart1.ZoomPane(pane, zoomFraction, centerPt, false);
            pane.ZoomStack.Add(new ZedGraph.ZoomState(pane, ZedGraph.ZoomState.StateType.Zoom));
            RefreshGraph();   
        }

        private void toolStripButtonZoomIn_Click(object sender, EventArgs e)
        {
            var pane = chart1.GraphPane;
            var centerPt = new PointF(chart1.Size.Width / 2, chart1.Size.Height / 2);
            double zoomFraction = (1 + -1.0 * chart1.ZoomStepFraction);
            chart1.ZoomPane(pane, zoomFraction, centerPt, false);
            pane.ZoomStack.Add(new ZedGraph.ZoomState(pane, ZedGraph.ZoomState.StateType.Zoom));
            RefreshGraph();   
        }

        private void RefreshGraph()
        {
            using (Graphics g = CreateGraphics())
            {
                chart1.GraphPane.AxisChange(g);
            }
            chart1.Refresh();
        }
    }
}
