using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries.Profile;

namespace Reclamation.TimeSeries.Forms.Graphing
{
    public partial class ProfileDesigner : Form
    {
        TimeSeriesDatabase m_db;

        public ProfileDesigner()
        {
            InitializeComponent();
        }
        public ProfileDesigner(TimeSeriesDatabase db)
        {
            InitializeComponent();
            wDB = new WaterProfileDatabase(db);
            m_db = db;
        }

        WaterProfileDatabase wDB;

       

        private void LoadSeriesData()
        {

            var sensorSeriesNames = new List<string>();
            var depthSeriesNames = new List<string>();
            var errorMessage = new List<string>();

            try
            {
                for (int i = 0; i < textBoxSeries.Lines.Length; i++)
                {
                    var line = textBoxSeries.Lines[i];
                    var tokens = line.Trim().Split(
                        new char[] { ',', ' ','\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (tokens.Length != 2)
                    {
                        errorMessage.Add("syntax error: " + line);
                    }
                    else
                    {
                        sensorSeriesNames.Add(tokens[0]);
                        depthSeriesNames.Add(tokens[1]);
                    }
                }

                wDB.LoadSeries(sensorSeriesNames.ToArray(), depthSeriesNames.ToArray(),textBoxWaterSurface.Text);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
 

        private void ProfileDesigner_Load(object sender, EventArgs e)
        {

        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            LoadSeriesData();
            UpdateChart();
        }

        private void UpdateChart()
        {

            this.chart1.XAxisText = this.textBoxXlabel.Text;
            chart1.YAxisText = this.textboxYlabel.Text;
            chart1.Title = this.textBoxTitle.Text;

            chart1.XAxisMinScale = Convert.ToDouble(this.textBoxXmin.Text);
            chart1.XAxisMaxScale = Convert.ToDouble(this.textBoxXmax.Text);

            chart1.YAxisMinScale = Convert.ToDouble(this.textBoxYmin.Text);
            chart1.YAxisMaxScale = Convert.ToDouble(this.textBoxYmax.Text);

            var tbl = wDB.GetPlotData(timeSelectorBeginEnd1.T1.Date);
            var ws = wDB.GetWaterSurface(timeSelectorBeginEnd1.T1.Date);

            chart1.DrawProfile(tbl, timeSelectorBeginEnd1.T1.Date.ToShortDateString(),ws,"water surface");
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            timeSelectorBeginEnd1.T1 = timeSelectorBeginEnd1.T1.AddDays(1).Date;
            UpdateChart();
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            timeSelectorBeginEnd1.T1 = timeSelectorBeginEnd1.T1.AddDays(-1).Date;
            UpdateChart();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timeSelectorBeginEnd1.T1 = timeSelectorBeginEnd1.T1.AddDays(1).Date;
            UpdateChart();
        }


     
    }
}
