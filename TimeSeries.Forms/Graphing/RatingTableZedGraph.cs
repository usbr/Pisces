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

namespace Reclamation.TimeSeries.Graphing
{
    public partial class RatingTableZedGraph : UserControl
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

        
        private void SetGridsON( )
        {
            
            mypane.XAxis.MinorGrid.IsVisible = true;
            mypane.XAxis.MajorGrid.IsVisible = true;
            mypane.XAxis.MajorGrid.Color = System.Drawing.Color.DarkGreen;
            
            mypane.XAxis.MajorGrid.DashOff = 2.0f;
            mypane.XAxis.MajorGrid.DashOn = 2.0f;
            mypane.XAxis.MinorGrid.DashOn = 4;
            mypane.XAxis.MinorGrid.DashOff = 1;

            mypane.YAxis.MinorGrid.IsVisible = true;
            mypane.YAxis.MajorGrid.IsVisible = true;
            mypane.YAxis.MajorGrid.Color = System.Drawing.Color.DarkGreen;

            mypane.YAxis.MajorGrid.DashOff = 2.0f;
            mypane.YAxis.MajorGrid.DashOn = 2.0f;
            mypane.YAxis.MinorGrid.DashOn = 4;
            mypane.YAxis.MinorGrid.DashOff = 1;
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
            //mypane.XAxis.Type = AxisType.Log;
           // mypane.AxisChange();
           // mypane.XAxis.MajorGrid.IsVisible = true;
            //mypane.AxisChange();
            mypane.XAxis.Scale.IsUseTenPower = false;
            mypane.XAxis.Scale.Mag = 0;
            mypane.XAxis.Scale.Format = "#,#";
            mypane.AxisChange();
            mypane.XAxis.Scale.Min = list.MinDischarge;
            mypane.XAxis.Scale.Max = list.MaxDischarge;
            mypane.XAxis.Title.Text = "Flow (cfs)";
            mypane.AxisChange();

            //mypane.YAxis.Type = AxisType.Log;
            //mypane.YAxis.Scale.IsUseTenPower = false;
            //mypane.AxisChange();
            //mypane.YAxis.ScaleFormatEvent += YAxis_ScaleFormatEvent;
            //mypane.AxisChange();
            mypane.YAxis.Scale.Min = list.MinStage ;
            mypane.YAxis.Scale.Max = list.MaxStage;
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
              var c = chart1.GraphPane.AddCurve(measurements[0].MeasurementRow.siteid, points, Color.Green, SymbolType.Circle);
              c.Line.IsVisible = false;
              c.Symbol.Fill = new Fill(Color.Green);
              c.Symbol.IsVisible = true;

            }

            RefreshGraph();
        }
    }
}
