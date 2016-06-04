using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

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
            mypane.XAxis.Type = AxisType.Log;
            mypane.AxisChange();
            mypane.XAxis.MajorGrid.IsVisible = true;

            SetGridsON();

            //mypane.XAxis.Scale.Min = 1;
            //mypane.XAxis.Scale.Max = 10000;
            mypane.XAxis.Title.Text = "Flow (cfs)";

            mypane.YAxis.Type = AxisType.Log;
            //mypane.YAxis.Scale.Min = 1;
            //mypane.YAxis.Scale.Max = 10000;
            mypane.YAxis.Title.Text = "Stage (feet)";
            mypane.AxisChange();
        }

        private void SetGridsON( )
        {
            
            mypane.XAxis.MinorGrid.IsVisible = true;
            mypane.XAxis.MajorGrid.IsVisible = true;
            mypane.XAxis.MajorGrid.Color = System.Drawing.Color.DarkGreen;
            mypane.XAxis.MajorGrid.DashOff = 2.0f;
            mypane.XAxis.MajorGrid.DashOn = 2.0f;
            mypane.XAxis.MinorGrid.DashOn = 4;

            mypane.YAxis.MinorGrid.IsVisible = true;
            mypane.YAxis.MajorGrid.IsVisible = true;
            mypane.YAxis.MajorGrid.Color = System.Drawing.Color.DarkGreen;
            mypane.YAxis.MajorGrid.DashOff = 2.0f;
            mypane.YAxis.MajorGrid.DashOn = 2.0f;
            mypane.YAxis.MinorGrid.DashOn = 4;
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
    }
}
