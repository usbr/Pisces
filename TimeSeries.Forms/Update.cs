using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public enum SeriesProcess { Update, Calculate, Duplicate };
    public partial class Update : Form
    {
        SeriesProcess process;
        public Update(DateTime t1, DateTime t2, SeriesProcess process)
        {
            InitializeComponent();
            this.timeSelectorBeginEnd1.T1 = t1;
            this.timeSelectorBeginEnd1.T2 = t2;
            this.process = process;
            if (process == SeriesProcess.Update)
            {
                Text = "Update Selected Series";
                textBoxInfo.Text = "Click OK to update the selected series.  Pisces will reload data from the original source.  Existing data will be overwritten.";
                checkBoxFullPeriod.Hide();
            }
            if (process == SeriesProcess.Calculate)
            {
                Text = "Calculate Selected Series";
                textBoxInfo.Text = "Click OK to calculate the selected series.  Pisces will recompute based on the math expression.  Existing data will be overwritten.";
                checkBoxFullPeriod.Checked = true;
                EnableDates(this, EventArgs.Empty);
            }
            if (process == SeriesProcess.Duplicate)
            {
                Text = "Duplicate Selected Series";
                textBoxInfo.Text = "Click OK to duplicate the selected series.  Pisces will either reload data from the original source or recompute based on the math expression.  ";
                checkBoxFullPeriod.Checked = true;
                EnableDates(this, EventArgs.Empty);
            }
        }


        public DateTime T1
        {
            get { return timeSelectorBeginEnd1.T1; }
        }
        public DateTime T2
        {
            get { return timeSelectorBeginEnd1.T2;}
        }

        public bool FullPeriodOfRecord
        {
            get { return checkBoxFullPeriod.Checked; }
        }

        private void EnableDates(object sender, EventArgs e)
        {
            this.timeSelectorBeginEnd1.Enabled = true;

            if (this.checkBoxFullPeriod.Checked)
                this.timeSelectorBeginEnd1.Enabled = false;
        }
    }
}
