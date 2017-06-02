using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace HydrometForecast
{
    public partial class RegressionResults : Form
    {
        public RegressionResults()
        {
            InitializeComponent();
        }

        public string[] Output
        {
            get { return this.textBoxOutput.Lines; }
            set { 
                this.textBoxOutput.Lines = value;
                textBoxOutput.ReadOnly = true;
            }
        }


        public event EventHandler<EventArgs> CompareToHistoryClicked;

        private void linkLabelCompareHistoryOut_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            if (CompareToHistoryClicked != null)
            {
                CompareToHistoryClicked(this, EventArgs.Empty);
            }
        }
       
        public string DataFile { get; set; }

        public string CoeficientsExisting
        {
            set
            {
                this.labelExisting.Text = value;
            }
        }

        public string CoefficientsComputed
        {

            set
            {
                this.labelComputed.Text = value;
            }
        }

       
    }
}
