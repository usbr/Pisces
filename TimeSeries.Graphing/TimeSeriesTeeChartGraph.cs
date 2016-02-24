using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;


namespace Reclamation.TimeSeries.Graphing
{

    public partial class TimeSeriesTeeChartGraph : UserControl, Reclamation.TimeSeries.Graphing.ITimeSeriesGraph
    {
        double _missingDataValue;
        SeriesList seriesList;
        string title = "";
        string subTitle = "";
        private Steema.TeeChart.Tools.DragPoint dragPoint1;

      //  Steema.TeeChart.Tools.RectangleTool rectTool;

        public TimeSeriesTeeChartGraph()
        {
            InitializeComponent();
            seriesList = new SeriesList();
            analysisType = AnalysisType.TimeSeries;
            _missingDataValue = Point.MissingValueFlag;
            dragPoint1 = new Steema.TeeChart.Tools.DragPoint();
            this.dragPoint1.Style = Steema.TeeChart.Tools.DragPointStyles.Y;
            this.tChart1.Tools.Add(this.dragPoint1);
            this.dragPoint1.Drag += new Steema.TeeChart.Tools.DragPointEventHandler(dragPoint1_Drag);
            //rectTool = new Steema.TeeChart.Tools.RectangleTool(tChart1.Chart);
        }

       


        public double MissingDataValue
        {
            get { return _missingDataValue; }
            set { _missingDataValue = value; }
        }
        public SeriesList Series
        {
            get { return this.seriesList; }
            set
            {
                this.seriesList = value;
            }
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

        public void Add(Series s)
        {
            seriesList.Add(s);
        }

       public string SubTitle
        {
            get
            {
                return this.subTitle;
            }
            set
            {
                this.subTitle = value;
            }
        }
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
                
            }
        }

        private void UpdateTitle()
        {
            tChart1.Text = title + "\n" + subTitle;
        }

        private AnalysisType analysisType;
        public AnalysisType AnalysisType
        {
            set { this.analysisType = value;}
        }

       
        private void toolStripButtonEditGraph_Click(object sender, EventArgs e)
        {
            Steema.TeeChart.Editor ed = new Steema.TeeChart.Editor(this.tChart1);
            ed.Title = "Pisces Editor";
            ed.ShowModal();

            // save colors to user settings.
            for (int i = 0; i < tChart1.Series.Count; i++)
            {
                Properties.Settings.Default.SetColor(i, tChart1.Series[i].Color);
            Steema.TeeChart.Styles.Line line = tChart1.Series[i] as  Steema.TeeChart.Styles.Line;
                if(line != null)
                {
                    Properties.Settings.Default.SetSeriesWidth(i, line.LinePen.Width);
                }
                
            }
        }

        bool m_multiLeftAxis = false;

        public bool MultiLeftAxis
        {
            get { return m_multiLeftAxis; }
            set { m_multiLeftAxis = value; }
        }


        public void Draw(bool undoZoom)
        {
            TChartDataLoader loader = new TChartDataLoader(this.tChart1);
            toolStripComboBoxDragPoints.Items.Clear();
            toolStripComboBoxDragPoints.Text = "";
            dragPoint1.Active = false;
            toolStripComboBoxDragPoints.Enabled = false;
            switch (analysisType)
            {
                case AnalysisType.TimeSeries:

                    loader.DrawTimeSeries(seriesList, title, subTitle,undoZoom,m_multiLeftAxis);
                    for (int i = 0; i < tChart1.Series.Count; i++)
                    {
                        toolStripComboBoxDragPoints.Items.Add(tChart1[i].ToString());  
                    }
                    
                    toolStripComboBoxDragPoints.Enabled = !seriesList.ReadOnly;
                    

                    break;
                case AnalysisType.Exceedance:
                    loader.DrawSorted(seriesList, title, subTitle,"Percent Exceedance");
                    break;
                case AnalysisType.Probability:
                    loader.DrawSorted(seriesList, title, subTitle,"Percent");
                    break;
                case AnalysisType.WaterYears:
                    loader.DrawWaterYears(seriesList, title, subTitle,m_multiLeftAxis);
                    break;
                case AnalysisType.SummaryHydrograph:
                    loader.DrawWaterYears(seriesList, title, subTitle);
                    break;
                case AnalysisType.Correlation:
                    if (seriesList.Count == 2)
                    {
                        loader.DrawCorrelation(seriesList[0], seriesList[1], title, subTitle);
                    }
                    else
                    {
                        loader.Clear();
                    }
                    break;
                case AnalysisType.MonthlySummary :
                    loader.DrawTimeSeries(seriesList, title, subTitle,undoZoom,m_multiLeftAxis);
                    break;
                case AnalysisType.MovingAverage :
                    loader.DrawTimeSeries(seriesList, title, subTitle,undoZoom);
                    break;
                case AnalysisType.TraceAnalysis:
                    loader.DrawTimeSeries(seriesList, title, subTitle, undoZoom, m_multiLeftAxis);
                    break;
                default:
                    loader.Clear();
                    break;
            }
        }

        private void tChart1_ClickSeries(object sender, Steema.TeeChart.Styles.Series s, int valueIndex, MouseEventArgs e)
        {
            int idx = this.tChart1.Series.IndexOf(s);
           // MessageBox.Show(s.Title);
            Reclamation.Core.Logger.WriteLine(s.Title);
        }

        private void toolStripButtonUndoZoom_Click(object sender, EventArgs e)
        {
            //this.tChart1.Zoom.Undo();
            this.tChart1.Zoom.Undo();//.getZoom().undo();
            //this.tChart1.Zoom.ZoomPercent(99);
        }

        private void toolStripButtonZoomOut_Click(object sender, EventArgs e)
        {
            this.tChart1.Zoom.ZoomPercent(90);
        }

        private void toolStripButtonZoomIn_Click(object sender, EventArgs e)
        {
            this.tChart1.Zoom.ZoomPercent(110);
        }

        private void toolStripButtonPrin_Click(object sender, EventArgs e)
        {
            tChart1.Printer.Preview();
        }

        private void toolStripComboBoxDragPoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboBoxDragPoints.SelectedIndex >= 0)
            {
                dragPoint1.Series = tChart1[toolStripComboBoxDragPoints.SelectedIndex];
                dragPoint1.Active = true;
            }
            else
                dragPoint1.Active = false;

            Console.WriteLine("dragPoint1.Active =" + dragPoint1.Active);
        }

       

       
        private void tChart1_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Mouse Down");
            pointDrag = false;
        }

        private void tChart1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Console.WriteLine("Mouse Up");
            if (pointDrag)
            {
                //timeSeriesSpreadsheet1.SetCellValue(prevRowIndex, prevColIndex, newvalue);
                prevRowIndex = -1;
            }
            //if (GraphDrawNeeded)
            //{
            //    GraphDrawNeeded = false;
            //    Graph();
            //}
        }

        int prevColIndex = -1;
        int prevRowIndex = -1;
        double newvalue = 0;
        bool pointDrag = false;

        void dragPoint1_Drag(Steema.TeeChart.Tools.DragPoint sender, int Index)
        {

            Console.WriteLine("dragPoint1_Drag");
            int seriesIndex = toolStripComboBoxDragPoints.SelectedIndex;
            if (seriesIndex > 0)
            {
                newvalue = tChart1[seriesIndex].YValues[Index];
                newvalue = System.Math.Round(newvalue, 3);
                tChart1[seriesIndex].YValues[Index] = newvalue;

                int colIndex = seriesIndex;
                //if (m_db == HydrometDataBase.Dayfiles)
                //    colIndex = seriesIndex * 2 - 1;

                prevRowIndex = Index;
                prevColIndex = colIndex;
                pointDrag = true;
                // GraphDrawNeeded = true;
            }
            else
            {
                pointDrag = false;
                newvalue = -998877;
                prevColIndex = -1;
                prevRowIndex = -1;
            }
        }

        private void toolStripButtonSelect_Click(object sender, EventArgs e)
        {
            if (toolStripButtonSelect.Checked)
            {
                tChart1.Zoom.Allow = false;
            }
            else
            {
                tChart1.Zoom.Allow = true;
            }
        }

        
    }
}

