using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Graphing;

namespace HydrometTools.Stats
{
    public partial class MultiYearAverage : UserControl
    {
        GraphExplorerView view;

        public MultiYearAverage()
        {
            InitializeComponent();
            int yr = DateTime.Now.Year;
            if (DateTime.Now.Month > 9)
                yr += 1;
            this.textBoxCompareSingleYear.Text = yr.ToString();

            var zg = new TimeSeriesZedGraph();
            view = new GraphExplorerView(zg);

            view.Parent = panel1;
            view.Dock = DockStyle.Fill;
            view.BringToFront();
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            try
            {
                HydrometHost svr = HydrometInfoUtility.HydrometServerFromPreferences();

                int yr = 2000;
                if (multi.cbtt.Trim() != "")
                {

                    var s = HydrometDailySeries.GetMultiYearAverage(multi.cbtt,multi.pcode,svr,multi.T1,multi.T2);
                    var list = new SeriesList();
                    list.Add(s);
                    view.SeriesList = list;
                    view.Draw();
                    Application.DoEvents();
                    // comparison period
                    if (compare.cbtt.Trim() != "")
                    {
                        var s2 = HydrometDailySeries.GetMultiYearAverage(compare.cbtt, compare.pcode, svr, compare.T1, compare.T2);
                        list.Add(s2);
                    }
                    if (textBoxCompareSingleYear.Text.Trim() != ""
    && textBoxSingleYearCbtt.Text.Trim() != ""
    && int.TryParse(this.textBoxCompareSingleYear.Text, out yr))
                    {

                        yr = Convert.ToInt32(textBoxCompareSingleYear.Text);
                        DateTime t1 = new DateTime(yr - 1, 10, 1);
                        DateTime t2 = new DateTime(yr, 9, 30);

                        Series s3 = HydrometDailySeries.Read(textBoxSingleYearCbtt.Text, textBoxSingleYearPcode.Text, t1, t2, svr);
                        s3 = Reclamation.TimeSeries.Math.ShiftToYear(s3, 2000);
                        s3.Appearance.LegendText = yr.ToString();
                        list.Add(s3);
                    }

                    view.SeriesList = list;
                    view.Draw();
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
 
        }

        private void textBoxYear_TextChanged(object sender, EventArgs e)
        {
            UpdateWy();
        }
        private void UpdateWy()
        {
            //int yr;
            //if (int.TryParse(this.textBoxYear.Text, out yr))
            //{
            //    this.textBoxEndYear.Text = (yr + 29).ToString();
            //}
            //if (int.TryParse(this.textBoxYearCompare.Text, out yr))
            //{
            //    this.textBoxEndYearCompare.Text = (yr + 29).ToString();
            //}



        }
    }
}
