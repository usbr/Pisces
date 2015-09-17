using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms.ImportForms
{
    public partial class AddSeries : Form
    {
        public AddSeries()
        {
            InitializeComponent();
            this.comboBoxTimeInterval.SelectedValue = TimeInterval.Irregular.ToString();
        }

        public string SeriesName { get { return this.textBoxName.Text; } }

        public string Units { get { return this.comboBoxUnits.Text; } }

        public TimeInterval TimeInterval
        {
            get
            {
                return TimeSeriesDatabase.TimeIntervalFromString(this.comboBoxTimeInterval.Text);
            }
        }



        public string TableName {
            get { return this.textBoxTableName.Text; }

        }

        private void comboBoxTimeInterval_SelectedIndexChanged(object sender, EventArgs e)
        {
            //update table name.
            string tn = this.TimeInterval.ToString().ToLower() + "_" + this.textBoxName.Text.ToLower();
            tn = tn.Replace("irregular", "instant");
            this.textBoxTableName.Text = tn;

        }
    }

}
