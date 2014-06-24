using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries.Usgs;
using Reclamation.Core;
using System.Diagnostics;

namespace Reclamation.TimeSeries.Forms.ImportForms
{
    public partial class ImportOWRD : Form
    {
        public ImportOWRD()
        {
            InitializeComponent();

            this.timeSelectorBeginEnd1.T1 = DateTime.Now.AddDays(-10);
            this.timeSelectorBeginEnd1.T2 = DateTime.Now.AddDays(-1);
        }

        public bool IncludeProvisional
        {
            get { return this.checkBoxIncludeProvisional.Checked; }
        }
        public string SiteID
        {
            get { return this.textBoxSiteNumber.Text; }
        }
       
        public string[] SiteIDs
        {
            get { return this.textBoxSiteNumber.Text.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries); }
        }

        public DateTime T2
        {
            get { return this.timeSelectorBeginEnd1.T2; }
        }

        public DateTime T1
        {
            get { return timeSelectorBeginEnd1.T1; }
        }


        public Owrd.OwrdSeries.OwrdDataSet OwrdDataSet
        {
            get
            {
                if (radioButtonMeanFlow.Checked)
                    return Owrd.OwrdSeries.OwrdDataSet.MDF;
                if (radioButtonInstantStage.Checked)
                    return Owrd.OwrdSeries.OwrdDataSet.Instantaneous_Stage;
                if (radioButtonInstantFlow.Checked)
                    return Owrd.OwrdSeries.OwrdDataSet.Instantaneous_Flow;


                // DEFAULT.
                return Owrd.OwrdSeries.OwrdDataSet.MDF;
            }
        }

        private void linkLabelUSGSInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            Process.Start("http://apps.wrd.state.or.us/apps/sw/hydro_near_real_time/");

        }

    }
}
