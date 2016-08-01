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
    public partial class RatingTableZedGraphOptions : UserControl
    {
        GraphPane m_chart;
        public RatingTableZedGraphOptions(GraphPane chart)
        {
            m_chart = chart;
            InitializeComponent();

            this.checkBoxXAxisIsLog.Checked = m_chart.XAxis.Type == AxisType.Log;
            this.checkBoxYAxisIsLog.Checked = m_chart.YAxis.Type == AxisType.Log;

        }
        public RatingTableZedGraphOptions()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBoxXAxisIsLog.Checked)
                m_chart.XAxis.Type = AxisType.Log;
            else
                m_chart.XAxis.Type = AxisType.Linear;

            if (checkBoxYAxisIsLog.Checked)
            {
                m_chart.YAxis.Type = AxisType.Log;
                m_chart.AxisChange();
                m_chart.YAxis.Scale.IsUseTenPower = true;
            }
            else
            {
                m_chart.YAxis.Type = AxisType.Linear;
                m_chart.YAxis.Scale.IsUseTenPower = false;
                m_chart.AxisChange();
                m_chart.YAxis.Scale.IsUseTenPower = false;
            }

            m_chart.AxisChange();

            if (this.Parent != null)
            {
                this.Parent.BringToFront();
                this.Parent = null;
            }

        }

        
    }
}
