using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace Reclamation.TimeSeries.Forms.Graphing
{
    public partial class TemperatureProfileZedGraph : UserControl
    {
        public TemperatureProfileZedGraph()
        {
            InitializeComponent();
        }




        public void DrawDemo()
        {
            PointPairList points = new PointPairList();
            points.Add(28.43, 856);

            points.Add(28.16, 719);
            points.Add(28.00, 675);
            points.Add(28.31, 820);
            points.Add(28.91, 1160);
            points.Add(28.53, 937);
            points.Add(29.23, 1350);
            points.Add(30.05, 1960);
            points.Add(34.45, 8450);
            points.Add(31.57, 3740);
            points.Add(31.03, 3150);
            points.Add(31.40, 3690);
            points.Add(31.74, 4280);
            points.Add(28.68, 1036);
            points.Add(28.20, 767);

            chart1.GraphPane.AddCurve("YRWW", points, Color.Green);

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
