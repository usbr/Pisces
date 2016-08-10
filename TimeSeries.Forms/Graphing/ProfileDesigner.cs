using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries.Profile;
using System.IO;

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

        bool loaded = false;
        private void UpdateChart()
        {

            if (!loaded)
            {
                LoadSeriesData();
                loaded = true;
                loaded = true;
                if (this.textBoxStatic1Value.Text != "")
                    chart1.AddStaticLine(Convert.ToDouble(textBoxStatic1Value.Text),
                        textBoxStatic1Label.Text, Color.Brown,ZedGraph.SymbolType.Triangle);

                if (this.textBoxStatic2Value.Text != "")
                    chart1.AddStaticLine(Convert.ToDouble(textBoxStatic2Value.Text),
                        textBoxStatic2Label.Text, Color.CadetBlue,ZedGraph.SymbolType.Circle);

            }
            this.chart1.XAxisText = this.textBoxXlabel.Text;
            chart1.YAxisText = this.textboxYlabel.Text;
            chart1.Title = this.textBoxTitle.Text;

            chart1.XAxisMinScale = Convert.ToDouble(this.textBoxXmin.Text);
            chart1.XAxisMaxScale = Convert.ToDouble(this.textBoxXmax.Text);

            chart1.YAxisMinScale = Convert.ToDouble(this.textBoxYmin.Text);
            chart1.YAxisMaxScale = Convert.ToDouble(this.textBoxYmax.Text);

            var tbl = wDB.GetPlotData(timeSelector1.T1.Date);
            var ws = wDB.GetWaterSurface(timeSelector1.T1.Date);

            chart1.DrawProfile(tbl, timeSelector1.T1.Date.ToShortDateString(),ws,"water surface");
        }




        FileStream gif;
        BumpKit.GifEncoder encoder;

        private void buttonSave_Click(object sender, EventArgs e)
        {
            timer1.Interval = 1000 / Convert.ToInt32(this.textBoxspeed.Text);
          
            timer1.Enabled = true;
            var fn = "c:\\temp\\temp.gif";// textBoxOutputFile.Text.Trim();
            if ( fn == "")
                return;

            if (File.Exists(fn))
                File.Delete(fn);

            gif = File.OpenWrite(fn);
            encoder = new BumpKit.GifEncoder(gif);
            encoder.FrameDelay = new TimeSpan(0, 0, 0, 0, timer1.Interval);

        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            gif.Close();
            encoder = null;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            encoder.AddFrame(chart1.GetImage());
            timeSelector1.T1 = timeSelector1.T1.AddDays(1).Date;
            UpdateChart();

            if (this.timeSelector1.T1 > timeSelector1.T2)
            {
                timer1.Enabled = false;
                gif.Close();
                encoder = null;
            }
        }
       
        private void buttonStep_Click(object sender, EventArgs e)
        {
            timeSelector1.T1 = timeSelector1.T1.AddDays(1).Date;
            UpdateChart();
        }

        private void buttonStop_Click_1(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            loaded = false;
        }
        
        


     
    }
}
