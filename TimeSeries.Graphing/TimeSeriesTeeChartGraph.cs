using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Reclamation.TimeSeries.Forms.Graphing;

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
        Steema.TeeChart.Tools.Annotation annotation1;

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
            annotation1 = new Steema.TeeChart.Tools.Annotation(tChart1.Chart);
            annotation1.Position = Steema.TeeChart.Tools.AnnotationPositions.LeftTop;
            annotation1.Shape.Shadow.Visible = false;
            annotation1.Shape.Pen.Visible = false;
            //annotation1. .forma.Callout = Steema.TeeChart.Tools.AnnotationCallout.
            annotation1.Active = true;

            this.toolStripComboBoxZoomType.SelectedIndex = 0;
        }

        void SetupTChartNearestPointTool()
        {
            this.tChart1.Tools.Clear();

            if (annotation1.Active)
            {
                // set tool-tip pop-up for the FC-Ops added series
                for (int i = 0; i < this.tChart1.Series.Count; i++)
                {

                    Steema.TeeChart.Tools.NearestPoint nearestPoint2 = new Steema.TeeChart.Tools.NearestPoint(this.tChart1[i]);
                    nearestPoint2.Direction = Steema.TeeChart.Tools.NearestPointDirection.Horizontal;
                    nearestPoint2.Pen.Color = this.tChart1[i].Color;
                    nearestPoint2.Brush.Color = this.tChart1[i].Color;
                    nearestPoint2.Size = 5;
                    nearestPoint2.Style = Steema.TeeChart.Tools.NearestPointStyles.Circle;
                    nearestPoint2.DrawLine = false;
                    this.tChart1.Tools.Add(nearestPoint2);

                    this.tChart1.Series[i].GetSeriesMark += Form1_GetSeriesMark;
                }

                // Add point tooltips
                Steema.TeeChart.Tools.MarksTip marksTip1 = new Steema.TeeChart.Tools.MarksTip(this.tChart1.Chart);
                marksTip1.Style = Steema.TeeChart.Styles.MarksStyles.XY;
                marksTip1.Active = true;
                marksTip1.MouseDelay = 0;
                marksTip1.HideDelay = 999999;
                marksTip1.MouseAction = Steema.TeeChart.Tools.MarksTipMouseAction.Move;
                marksTip1.BackColor = Color.LightSteelBlue;
                marksTip1.ForeColor = Color.Black;
                //this.tChart1.Tools.Add(marksTip1);
            }
        }

        void Form1_GetSeriesMark(Steema.TeeChart.Styles.Series series, Steema.TeeChart.Styles.GetSeriesMarkEventArgs e)
        {
            var t = DateTime.FromOADate(Convert.ToDouble(series.XValues[e.ValueIndex].ToString()));
            var val = Convert.ToDouble(series.YValues[e.ValueIndex].ToString()).ToString("#,###,###.##");

            int yr = 0;
            if (int.TryParse(series.Title, out yr))
            {
                DateTime strDate = DateTime.Parse(t.ToString("MM/dd/") + series.Title);
                if (shiftAnnotationDate && t.Month >= 10)
                { // get proper date from water year
                    yr--;
                    strDate = DateTime.Parse(t.ToString("MM/dd/" + yr));
                }
                e.MarkText = "Series: " + series.Title + "\r\nDate-Time: " + strDate.ToString("MMM-d-yyyy") + "\r\nValue: " + val;
            }
            else
            {
                e.MarkText = "Series: " + series.Title + "\r\nDate-Time: " + t.ToString("MMM-d-yyyy") + "\r\nValue: " + val;
            }
        }

        public bool AnnotationOnMouseMove
        {
            get
            {
                return annotation1.Active;
            }
            set
            {
                annotation1.Active = value;
            }
        }

        private bool shiftAnnotationDate = true;
        public bool AnnotationDateShift
        {
            get
            {
                return shiftAnnotationDate;
            }
            set
            {
                shiftAnnotationDate = value;
            }
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

        public event EventHandler AfterEditGraph;

        private void toolStripButtonEditGraph_Click(object sender, EventArgs e)
        {
            Steema.TeeChart.Editor ed = new Steema.TeeChart.Editor(this.tChart1);
            ed.Title = "Graph Editor";
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

            if( AfterEditGraph != null)
            {
                var a = AfterEditGraph;
                a(this, EventArgs.Empty);
            }
        }

        bool m_multiLeftAxis = false;

        public bool MultiLeftAxis
        {
            get { return m_multiLeftAxis; }
            set { m_multiLeftAxis = value; }
        }

        bool m_monthlySummaryMultiYear = false;

        public bool MonthlySummaryMultiYear
        {
            get { return m_monthlySummaryMultiYear; }
            set { m_monthlySummaryMultiYear = value; }
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
                        loader.DrawCorrelation(seriesList[0], seriesList[1], title, subTitle);
                    else
                        loader.Clear();
                    break;
                case AnalysisType.MonthlySummary :
                    loader.DrawTimeSeries(seriesList, title, subTitle,undoZoom,m_multiLeftAxis,m_monthlySummaryMultiYear);
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

            SetupTChartNearestPointTool();
        }

        private void tChart1_ClickLegend(object sender, MouseEventArgs e)
        {
            Steema.TeeChart.Chart myChart = (Steema.TeeChart.Chart)sender;
            int idx = myChart.Legend.Clicked(e.X, e.Y);
            modifySeriesAppearance(idx, e);
        }

        private void tChart1_ClickSeries(object sender, Steema.TeeChart.Styles.Series s, int valueIndex, MouseEventArgs e)
        {
            int idx = this.tChart1.Series.IndexOf(s);
            modifySeriesAppearance(idx, e);            
            Reclamation.Core.Logger.WriteLine(s.Title);
        }
        
        public static Color[] fillColors = { Color.LightGray, Color.White, Color.LightSalmon, Color.LightGreen, Color.LightSkyBlue, Color.LightGoldenrodYellow};
        public static Color[] lineColors = { Color.Red, Color.Green, Color.Blue, Color.Orange, Color.Purple, Color.Gray };

        private static int colorCounter = 0;
        private static int selIdx = 0;
        private void modifySeriesAppearance(int idx, MouseEventArgs e)
        {
            switch (e.Button)
            {
                // Cycle through line widths
                case MouseButtons.Left:
                    if (this.tChart1.Series[idx] is Steema.TeeChart.Styles.Line)
                    {
                        int currWidth = (this.tChart1.Series[idx] as Steema.TeeChart.Styles.Line).LinePen.Width;
                        int newWidth = currWidth + 1;
                        if (newWidth > 5)
                        {
                            newWidth = 1;
                        }
                        (this.tChart1.Series[idx] as Steema.TeeChart.Styles.Line).LinePen.Width = newWidth;
                    }
                    break;

                // Cycle through line styles
                case MouseButtons.Right:
                    var lineStyle = new List<System.Drawing.Drawing2D.DashStyle> { System.Drawing.Drawing2D.DashStyle.Dash, System.Drawing.Drawing2D.DashStyle.DashDot ,
                    System.Drawing.Drawing2D.DashStyle.DashDotDot, System.Drawing.Drawing2D.DashStyle.Dot, System.Drawing.Drawing2D.DashStyle.Solid};
                    if (this.tChart1.Series[idx] is Steema.TeeChart.Styles.Line)
                    {
                        int lineStyleIdx = lineStyle.IndexOf((this.tChart1.Series[idx] as Steema.TeeChart.Styles.Line).LinePen.Style) + 1;
                        if (lineStyleIdx == lineStyle.Count)
                        {
                            lineStyleIdx = 0;
                        }
                        (this.tChart1.Series[idx] as Steema.TeeChart.Styles.Line).LinePen.Style = lineStyle[lineStyleIdx];
                    }
                    break;

                // Nothing yet
                case MouseButtons.Middle:
                    break;

                // Add area shading to line
                case MouseButtons.XButton1:
                    if (this.tChart1.Series[idx] is Steema.TeeChart.Styles.Line)
                    {
                        var s = this.tChart1.Series[idx];
                        Steema.TeeChart.Styles.Area areaChart1 = new Steema.TeeChart.Styles.Area();
                        areaChart1.DataSource = s;
                        areaChart1.AreaLines.Visible = false;
                        //areaChart1.Opacity = 0;
                        //areaChart1.Transparency = 100;
                        areaChart1.AreaBrush.Solid = true;
                        areaChart1.AreaBrush.Transparency = 100;
                        areaChart1.Legend.Visible = false;
                        tChart1.Chart.Series.Add(areaChart1);
                        tChart1.Series.MoveTo(this.tChart1.Series[this.tChart1.Series.Count - 1], 0);
                    }
                    break;

                // Cycle through colors
                case MouseButtons.XButton2:
                    if (idx != selIdx)
                    {
                        selIdx = idx;
                        colorCounter = 0;
                    }
                    if (this.tChart1.Series[idx] is Steema.TeeChart.Styles.Line)
                    {
                        this.tChart1.Series[idx].Color = lineColors[colorCounter];
                    }
                    if (this.tChart1.Series[idx] is Steema.TeeChart.Styles.Area)
                    {
                        this.tChart1.Series[idx].Color = fillColors[colorCounter];
                    }
                    colorCounter++;
                    if (colorCounter >= lineColors.Count())
                    {
                        colorCounter = 0;
                    }
                    break;
            }
        }

        private void toolStripButtonUndoZoom_Click(object sender, EventArgs e)
        {
            this.tChart1.Zoom.Undo();
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
            tChart1.Printer.Landscape = true;
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
                tChart1.Zoom.Direction = Steema.TeeChart.ZoomDirections.None;
            }
            else
            {
                tChart1.Zoom.Direction = Steema.TeeChart.ZoomDirections.Both;
            }
        }

        /// <summary>
        /// Gets and sets Color and line width, based on series title.
        /// used to keep water year colors consistent based on title
        /// </summary>
        public GraphSettings GraphSettings
        {
            get
            {
                GraphSettings gs = new GraphSettings();
                for (int i = 0; i < tChart1.Series.Count; i++)
                {
                    var s = tChart1.Series[i];
                    int width = 1;
                    if (s is Steema.TeeChart.Styles.Line)
                    {
                        var p = s as Steema.TeeChart.Styles.Line;
                        width = p.LinePen.Width;
                    }
                    gs.Add(s.Title, s.Color, width);
                }
                return gs;
            }
            set
            {
                var gs = value;
                for (int i = 0; i < tChart1.Series.Count; i++)
                {
                    var s = tChart1.Series[i];

                    if( !gs.Contains(s.Title))
                        continue;

                    SeriesSettings settings = gs.Get(s.Title);
                    s.Color = settings.Color;
                    if (s is Steema.TeeChart.Styles.Line)
                    {
                        var p = s as Steema.TeeChart.Styles.Line;
                        p.LinePen.Width = settings.Width;
                    }
                }
            }
        }

        private void toolStripComboBoxZoomType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.toolStripComboBoxZoomType.Text)
            {
                case "Zoom Horizontal":
                    this.tChart1.Zoom.Direction = Steema.TeeChart.ZoomDirections.Horizontal;
                    this.tChart1.Panning.Allow = Steema.TeeChart.ScrollModes.Horizontal;
                    break;
                case "Zoom Vertical":
                    this.tChart1.Zoom.Direction = Steema.TeeChart.ZoomDirections.Vertical;
                    this.tChart1.Panning.Allow = Steema.TeeChart.ScrollModes.Vertical;
                    break;
                default:
                    this.tChart1.Zoom.Direction = Steema.TeeChart.ZoomDirections.Both;
                    this.tChart1.Panning.Allow = Steema.TeeChart.ScrollModes.Both;
                    break;
            }
        }
    }
}

