using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Graphing;

namespace HydrometTools.Stats
{
    public partial class VolumeInRange : UserControl
    {
        GraphExplorerView view;
        public VolumeInRange()
        {
            InitializeComponent();

            var zg = new TimeSeriesZedGraph();
            view = new GraphExplorerView(zg);

            view.Parent = panel1;
            view.Dock = DockStyle.Fill;
            view.BringToFront();

            this.textBoxRankedYear.Text = DateTime.Now.WaterYear().ToString();
        }

        private void buttonGo_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            Application.DoEvents();
            try
            {
                var range = new MonthDayRange(Convert.ToInt16(Range1.Text.Split('/')[0]), Convert.ToInt16(Range1.Text.Split('/')[1]),
                    Convert.ToInt16(Range2.Text.Split('/')[0]), Convert.ToInt16(Range2.Text.Split('/')[1]));

                int wy1 = Convert.ToInt32(textBoxYear.Text);
                int wy2 = Convert.ToInt32(textBoxEndYear.Text);

                string cbtt = textBoxCbtt.Text;
                string pcode = textBoxPcode.Text;

                var s = Reclamation.TimeSeries.Math.HydrometDaily(cbtt, pcode);

                var t1 = new DateTime(wy1 - 1, 10, 1);
                var t2 = new DateTime(wy2, 9, 30);

                s.Read(t1, t2);

                if (AF.Checked)
                {
                    Reclamation.TimeSeries.Math.Multiply(s, 1.98347);
                    s.Units = "Acre-Feet";
                }

                var rval = Reclamation.TimeSeries.Math.AnnualSum(s, range, 10);

                rval.Appearance.LegendText = cbtt.ToUpper() + " " + pcode.ToUpper() + " Sum in Range";

                if (checkBoxOldSChool.Checked)
                {
                    var s2 = Reclamation.TimeSeries.Math.HydrometDaily(cbtt, pcode);
                    var t12 = new DateTime(Convert.ToInt32(textBoxRankedYear.Text) -1, 10, 1);
                    var t22 = new DateTime(Convert.ToInt32(textBoxRankedYear.Text), 9, 30);
                    s2.Read(t12, t22);
                    if (AF.Checked)
                    {
                        Reclamation.TimeSeries.Math.Multiply(s2, 1.98347);
                        s2.Units = "Acre-Feet";
                    }
                    var rval2 = Reclamation.TimeSeries.Math.AnnualSum(s2, range, 10);
                    OldSchoolReport.Display(rval, "DAILY VALUES SUMMATION  - Volume in Acre-feet", "Volume " + s.Units,
                      cbtt, pcode, range, wy1, wy2, rval2);
                }

                var list = new SeriesList();
                list.Add(rval);
                view.SeriesList = list;
                view.Draw();

            }
            finally
            {
                Cursor = Cursors.Default;

            }
        }

        private void checkBoxOldSChool_CheckedChanged(object sender, EventArgs e)
        {
            textBoxRankedYear.Enabled = !textBoxRankedYear.Enabled;
        }
    }
}
