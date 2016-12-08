using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.RatingTables;

namespace Reclamation.TimeSeries.Graphing
{
    public partial class GraphExplorerView : UserControl, IExplorerView 
    {
        private List<string> messages;
        ITimeSeriesGraph timeSeriesGraph1;

        UserControl uc;


        public GraphExplorerView()
        {
            var uc = new TimeSeriesZedGraph();
            InitControl(uc);
        }
        public GraphExplorerView(ITimeSeriesGraph timeSeriesGraph)
        {
            InitControl(timeSeriesGraph);
        }

        private void InitControl(ITimeSeriesGraph timeSeriesGraph)
        {
            uc = timeSeriesGraph as UserControl;
            this.timeSeriesGraph1 = timeSeriesGraph;
            InitializeComponent();
            messages = new List<string>();

            uc.Parent = tabPage1;
            uc.Dock = DockStyle.Fill;
        }


        #region IExplorerView Members

        public List<string> Messages
        {
            get { return messages; }
            set { this.messages = value; }
        }

        public bool MultipleYAxis
        {
            set { this.timeSeriesGraph1.MultiLeftAxis = value; }
        }

        private bool m_undoZoom;

        public bool UndoZoom
        {
            get { return m_undoZoom; }
            set { m_undoZoom = value; }
        }


        public void Clear()
        {
            this.timeSeriesGraph1.Clear();
            this.timeSeriesTableView1.Clear();
        }

        public SeriesList SeriesList
        {
            set {
                this.timeSeriesGraph1.Series = value;
                timeSeriesTableView1.SeriesList = SeriesList;
              }
            get { return this.timeSeriesGraph1.Series; }
        }

        public string Title
        {
            set { this.timeSeriesGraph1.Title = value; }
        }

        public string SubTitle
        {
            set { this.timeSeriesGraph1.SubTitle = value; }
        }

        private DataTable _dataTable;
        public DataTable DataTable
        {
            set { _dataTable = value; }// this.timeSeriesTableView1.DataTable = value; }
            get { return _dataTable; }// return this.timeSeriesTableView1.DataTable; }
        }

        public AnalysisType AnalysisType
        {
            set { this.timeSeriesGraph1.AnalysisType = value; }
        }

        public bool MonthlySummaryMultiYear
        {
            set { this.timeSeriesGraph1.MonthlySummaryMultiYear = value; }
        }

        public void Draw()
        {
            this.timeSeriesGraph1.Draw(this.UndoZoom);
            this.timeSeriesTableView1.Draw();
        }


        #endregion

    }
}
