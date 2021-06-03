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
    public partial class ImportUsgsData : Form
    {
        public ImportUsgsData()
        {
            InitializeComponent();

            this.timeSelectorBeginEnd1.T1 = DateTime.Now.AddDays(-10);
            this.timeSelectorBeginEnd1.T2 = DateTime.Now.AddDays(-1);
        }


        public string SiteID
        {
            get { return this.textBoxSiteNumber.Text; }
        }
        public bool IsRealTime 
        {
            get
            {
                return radioButtonRtFlow.Checked
                    || radioButtonRtGageHt.Checked
                    || this.radioButtonResElevation.Checked;
            }
        }

        public bool IsGroundWaterLevel
        {
            get { return this.radioButtonGwLevels.Checked; }
        }

        public bool IsGroundWaterDepth
        {
            get { return this.radioButtonGwDepths.Checked; }
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


        public Enum SelectedParameter
        {
            get
            {
                if (IsRealTime)
                {
                    if (radioButtonRtGageHt.Checked)
                        return UsgsRealTimeParameter.GageHeight;
                    else if (radioButtonResElevation.Checked)
                        return UsgsRealTimeParameter.ReservoirElevation;
                    else
                        return UsgsRealTimeParameter.Discharge;
                }
                else
                {
                    if (radioButtonMeanFlow.Checked)
                        return UsgsDailyParameter.DailyMeanDischarge;
                    if (radioButtonTempMax.Checked)
                        return UsgsDailyParameter.DailyMaxTemperature;
                    if (radioButtonTempMean.Checked)
                        return UsgsDailyParameter.DailyMeanTemperature;
                    if (radioButtonTempMin.Checked)
                        return UsgsDailyParameter.DailyMinTemperature;
                    else
                        return null;
                }
            }
        }

        private void linkLabelUSGSInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (textBoxSiteNumber.Text.Trim() != "")
            {
                try
                {
                    string url = "https://waterservices.usgs.gov/nwis/site/?format=rdb&sites=" + textBoxSiteNumber.Text.Trim();

                    var data = Reclamation.Core.Web.GetPage(url);
                    UsgsRDBFile rdb = new UsgsRDBFile(data);
                    DataTable tbl = DataTableUtility.Transpose(rdb);
                    var dlg = new Reclamation.Core.TableViewer(tbl);
                    dlg.ShowDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
            else
            {
                Process.Start("http://waterdata.usgs.gov/nwis/");
            }
           
        }

    }
}
