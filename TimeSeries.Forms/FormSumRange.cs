using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Forms
{
    public partial class FormSumRange : Form
    {
        public FormSumRange()
        {
            InitializeComponent();
        }
        public string SeriesName
        {
            get { return this.textBoxSiteName.Text; }
        }

        public MonthDayRange MonthDayRange
        {
            get { return this.monthDayRangePicker1.MonthDayRange; }
        }

	
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBoxSiteName.Text = openFileDialog1.FileName;
            }
        }
    }
}