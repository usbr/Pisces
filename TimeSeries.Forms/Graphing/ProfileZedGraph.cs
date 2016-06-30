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

    /// <summary>
    /// ZedGraph wrapper for profile plots such as:
    /// water temperature with depth
    /// nutrients with depth
    /// dissolved oxygen with depth
    /// </summary>
    public partial class ProfileZedGraph : UserControl
    {
        public ProfileZedGraph()
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

            chart1.GraphPane.AddCurve("temp profile", points, Color.Green);

            RefreshGraph();
        }

        public Image GetImage()
        {
            return chart1.GetImage();
        }

        private void RefreshGraph()
        {
            using (Graphics g = CreateGraphics())
            {
                chart1.GraphPane.AxisChange(g);
            }
            chart1.Refresh();
        }

        public string YAxisText
        {
            set
            {
                chart1.GraphPane.YAxis.Title.Text = value; 
            }
        }
        public string XAxisText
        {
            set
            {
                chart1.GraphPane.XAxis.Title.Text = value;
            }
        }

        public double YAxisMinScale
        {
            set {
                chart1.GraphPane.YAxis.Scale.Min = value;
            }
        }
             public double YAxisMaxScale
        {
            set {
                chart1.GraphPane.YAxis.Scale.Max = value;
            }
        }

             public double XAxisMinScale
             {
                 set
                 {
                     chart1.GraphPane.XAxis.Scale.Min = value;
                 }
             }
             public double XAxisMaxScale
             {
                 set
                 {
                     chart1.GraphPane.XAxis.Scale.Max = value;
                 }
             }

        public string Title
             {
                 set { chart1.GraphPane.Title.Text = value; }
             }


        List<string> staticLabels = new List<string>();
        List<double> staticValues = new List<double>();
        List<Color> staticColors = new List<Color>();
        List<SymbolType> staticSymbols = new List<SymbolType>();
        public void AddStaticLine(double value, string legend, Color c,SymbolType sym )
        {
            staticLabels.Add(legend);
            staticValues.Add(value);
            staticColors.Add(c);
            staticSymbols.Add(sym);
        }


        /// <summary>
        /// Plots profile data in the table
        /// firs column is sensor values, second column is sensor depth
        /// </summary>
        /// <param name="profileTable"></param>
        public void DrawProfile(DataTable profileTable, string profileLegend, 
            double waterSurface=-1, string waterSurfaceLegend="" )
        {
            chart1.GraphPane.CurveList.Clear();
           var xs = chart1.GraphPane.XAxis.Scale;
           
            PointPairList points = new PointPairList();
            for (int i = 0; i < profileTable.Rows.Count; i++)
            {
                points.Add(Convert.ToDouble(profileTable.Rows[i][0]), Convert.ToDouble(profileTable.Rows[i][1]));
            }

            if( waterSurface >0)
            {
                PointPairList ws = new PointPairList();
                ws.Add(xs.Min, waterSurface);
                ws.Add(xs.Max, waterSurface);
                chart1.GraphPane.AddCurve(waterSurfaceLegend, ws, Color.Blue,SymbolType.None);
            }

            chart1.GraphPane.AddCurve(profileLegend, points, Color.Green,SymbolType.Square);

            for (int i = 0; i < staticColors.Count; i++)
            {
                PointPairList p = new PointPairList();
                p.Add(xs.Min, staticValues[i]);
                p.Add(xs.Min/2+xs.Max/2, staticValues[i]);
                p.Add(xs.Max, staticValues[i]);
                chart1.GraphPane.AddCurve(staticLabels[i], p, staticColors[i],staticSymbols[i]);
            }

            RefreshGraph();

        }
    }
}
