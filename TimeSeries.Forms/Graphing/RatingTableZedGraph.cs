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
    public partial class RatingTableZedGraph : UserControl
    {
        string title = "";
        string subTitle = "";

        public RatingTableZedGraph()
        {
            InitializeComponent();
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
