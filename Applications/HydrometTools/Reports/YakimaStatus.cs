using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.Core;
using System.IO;
using Reclamation.TimeSeries.Reports;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries;

namespace HydrometTools.Reports
{
    public partial class YakimaStatus : UserControl
    {
        public YakimaStatus()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            textBoxStatus.Text = "Working..";
            Application.DoEvents();
            Cursor = Cursors.WaitCursor;
            try
            {
                var fn = Path.Combine(
                    FileUtility.GetExecutableDirectory(), "YakimaStatusTemplate.txt");

                var r = new YakimaStatusReport();

                var t = this.dateTimePicker1.Value.Date.AddHours((int)numericUpDown1.Value);
                if( !checkBox30yravg.Checked)
                {
                    textBoxStatus.Text = r.Create(t);
                }
                else
                {
                    int y1 = Convert.ToInt32(textBoxYear1.Text);
                    int y2 = Convert.ToInt32(textBoxYear2.Text);

                    textBoxStatus.Text = r.Create(t,y1,y2);
                }

            }
            finally
            {
                Cursor = Cursors.Default;
            }

        }

        private void linkLabelWebStatus_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.usbr.gov/pn/hydromet/yakima/yakstats.html");
        }

         
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (this.textBoxStatus.Text.Trim() == "")
                return;
            
            Database.UpdateYakimaStatusReport(this.dateTimePicker1.Value.Date,
                this.textBoxStatus.Text);
            
        }

        private void linkLabelOpenReports_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var f = new ViewReportsForm();
            f.ShowDialog();

        }

        
    }
}
