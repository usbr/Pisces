using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace Depth
{
    public partial class RatingTableChart : UserControl
    {
        ZedGraphControl zedGraphControl1;
        GraphPane chart1;
        public RatingTableChart()
        {
            InitializeComponent();
            CreateChartControl();
         
        }
        
        public void Draw(RatingTable input)
        {
            ChartProcessor cpu = new ChartProcessor(input, chart1);
            cpu.AddPoints();
            cpu.SetupAxis();
            cpu.AddLogRegressionLine();

            zedGraphControl1.ZoomOutAll(zedGraphControl1.GraphPane);
           
            RefreshGraph();
        }


        private void RefreshGraph()
        {
            using (Graphics g = CreateGraphics())
            {
                chart1.AxisChange(g);
            }
            zedGraphControl1.Refresh();
        }
        

       


        private void CreateChartControl()
        {
            zedGraphControl1 = new ZedGraphControl();
            zedGraphControl1.Parent = this;
            this.zedGraphControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControl1.Location = new System.Drawing.Point(0, 0);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(502, 462);
            this.zedGraphControl1.TabIndex = 0;
            this.zedGraphControl1.UseExtendedPrintDialog = true;
            chart1 = zedGraphControl1.GraphPane;
        }
    }
}
