using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
using Reclamation.TimeSeries.RatingTables;
using Reclamation.TimeSeries.Forms.Graphing;

namespace Reclamation.TimeSeries.Forms.RatingTables
{
    public partial class RatingTableZedGraph : UserControl, IRatingTableGraph
    {
        string title = "";
        string subTitle = "";

        GraphPane mypane;
        public RatingTableZedGraph()
        {
            InitializeComponent();
            mypane = chart1.GraphPane;

            SetGridsON();

        }

        
        private void SetupGrid(ZedGraph.MinorGrid g, Color c, double dashPercent=0.5)
        {
            g.IsVisible = true;
            g.Color = c;
            g.DashOff = 5*(float)(1.0- dashPercent ); 
            g.DashOn = 5*(float)(dashPercent);
        }

        private void SetGridsON( )
        {
            var c = System.Drawing.Color.DarkGreen;
            double pct = .80;
            SetupGrid(mypane.XAxis.MajorGrid, c, pct);
            SetupGrid(mypane.XAxis.MinorGrid, c, pct);

            SetupGrid(mypane.YAxis.MajorGrid, c, pct);
            SetupGrid(mypane.YAxis.MinorGrid, c, pct);
        }


        public void DrawDemo()
        {
            PointPairList points = new PointPairList();
            points.Add(28.43, 856);
	
points.Add(28.16,719);
points.Add(28.00,675);
points.Add(28.31,820);
points.Add(28.91,1160);
points.Add(28.53,937);
points.Add(29.23,1350);
points.Add(30.05,1960);
points.Add(34.45,8450);
points.Add(31.57,3740);
points.Add(31.03,3150);
points.Add(31.40,3690);
points.Add(31.74,4280);
points.Add(28.68,1036);
points.Add(28.20,767);

chart1.GraphPane.AddCurve("YRWW", points, Color.Green);

RefreshGraph();
        }

        /// <summary>
        /// remove all series from the graph.
        /// </summary>
        public void Clear()
        {
            title = "";
            subTitle = "";
            
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

        private void SetupAxis(MeasurementList list)
        {
            //mypane.XAxis.Scale.Min = list.MinDischarge;
            //mypane.XAxis.Scale.Min = 0;
            //mypane.XAxis.Scale.Max = list.MaxDischarge;
            mypane.XAxis.Scale.IsUseTenPower = false;
            mypane.XAxis.Scale.Mag = 0;
            mypane.XAxis.Scale.Format = "#,#";
            mypane.XAxis.Scale.MinGrace = 0.05;
            mypane.AxisChange();           
            mypane.XAxis.Title.Text = "Flow (cfs)";
            mypane.AxisChange();

            //mypane.YAxis.Type = AxisType.Log;
            //mypane.AxisChange();
            //mypane.YAxis.MajorGrid.IsVisible = true;
            //mypane.AxisChange();
            //mypane.YAxis.Scale.Min = list.MinStage ;
            //mypane.YAxis.Scale.Max = list.MaxStage;
            mypane.YAxis.Scale.IsUseTenPower = false;
            mypane.YAxis.Scale.Mag = 0;
            mypane.YAxis.Scale.Format = "#,#";
            mypane.YAxis.Scale.MinGrace = 0.05;
            mypane.YAxis.Title.Text = "Stage (feet)";
            mypane.AxisChange();
        }

        string YAxis_ScaleFormatEvent(GraphPane pane, Axis axis, double val, int index)
        {
            return val.ToString("F0");
        }

        public void Draw(BasicMeasurement[] measurements)
        {
            chart1.GraphPane.CurveList.Clear();
            if (measurements.Length == 0)
                return;
            MeasurementList list = new MeasurementList(measurements);
            Title = list.Text;
            chart1.GraphPane.Title.Text = Title;
            SetupAxis(list);
            

            PointPairList points = new PointPairList();

            for (int i = 0; i < measurements.Length; i++)
            {
                points.Add(measurements[i].MeasurementRow.discharge,
                    measurements[i].MeasurementRow.stage);
            }

            if (measurements.Length > 0)
            {
                AddRegressionLines(measurements, points);

            }

            RefreshGraph();
        }

        private void AddRegressionLines(BasicMeasurement[] measurements, PointPairList points)
        {
            var c = chart1.GraphPane.AddCurve(measurements[0].MeasurementRow.siteid, points, Color.Green, SymbolType.Circle);
            c.Line.IsVisible = false;
            c.Symbol.Fill = new Fill(Color.Green);
            c.Symbol.IsVisible = true;

            // TEST REGRESSION CODE
            double[] xData = new double[measurements.Length];
            double[] yData = new double[measurements.Length];
            for (int i = 0; i < points.Count; i++)
            {
                xData[i] = points[i].Y;
                yData[i] = points[i].X;
            }

            // Linear Regression Line
            var fitPoints = Reclamation.TimeSeries.Estimation.Regression.SimpleRegression(xData, yData);
            PointPairList fitPointsArray = new PointPairList();
            for (int i = 0; i < fitPoints.Length; i++)
            {
                fitPointsArray.Add(fitPoints[i].Item2, fitPoints[i].Item1);
            }
            var f = chart1.GraphPane.AddCurve("Best Linear Fit Line", fitPointsArray, Color.Black, SymbolType.None);
            f.Line.IsVisible = true;
            f.Symbol.IsVisible = false;

            // Log base-10 regression line
            var logPoints = Reclamation.TimeSeries.Estimation.Regression.SimpleRegression(xData, yData, 1, true, 10);
            PointPairList logPointsArray = new PointPairList();
            for (int i = 0; i < logPoints.Length; i++)
            {
                logPointsArray.Add(logPoints[i].Item2, logPoints[i].Item1);
            }
            var l = chart1.GraphPane.AddCurve("Best Log Fit Line", logPointsArray, Color.Red, SymbolType.None);
            l.Line.IsVisible = true;
            l.Symbol.IsVisible = false;


            if (xData.Length > 2)
            {             // Piecewise Linear Regression Line with 1 break at the flow average value
                var pcwsePoints = Reclamation.TimeSeries.Estimation.Regression.PiecewiseLinearRegression(xData, yData,
                                    new double[] { yData.Average() });
                PointPairList pcwsePointsArray = new PointPairList();
                for (int i = 0; i < logPoints.Length; i++)
                {
                    pcwsePointsArray.Add(pcwsePoints[i].Item2, pcwsePoints[i].Item1);
                }
                var p = chart1.GraphPane.AddCurve("Piecewise Line: 1 break", pcwsePointsArray, Color.Blue, SymbolType.None);
                p.Line.IsVisible = true;
                p.Symbol.IsVisible = false;
            }
        }

        private void toolStripButtonSettings_Click(object sender, EventArgs e)
        {
            var uc  = new RatingTableZedGraphOptions(chart1.GraphPane);
            uc.Parent = chart1;
            uc.Dock = DockStyle.Fill;
            uc.BringToFront();
            
        }
    }
}
