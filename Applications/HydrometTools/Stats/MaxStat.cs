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
    public partial class MaxStat : UserControl
    {
        GraphExplorerView view;
        public MaxStat()
        {
            InitializeComponent();

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
                var range = new MonthDayRange(Convert.ToInt16(Range1.Text.Substring(0, 2)), Convert.ToInt16(Range1.Text.Substring(3, 2)),
                    Convert.ToInt16(Range2.Text.Substring(0, 2)), Convert.ToInt16(Range2.Text.Substring(3, 2)));

                int wy1 = Convert.ToInt32(textBoxYear.Text);
                int wy2 = Convert.ToInt32(textBoxEndYear.Text);

                string cbtt = textBoxCbtt.Text;
                string pcode = textBoxPcode.Text;

                //var s = Reclamation.TimeSeries.Math.HydrometDaily(cbtt, pcode);

                var t1 = new DateTime(wy1 - 1, 10, 1);
                var t2 = new DateTime(wy2, 9, 30);
                HydrometHost svr = HydrometInfoUtility.HydrometServerFromPreferences();

                var s = HydrometDailySeries.Read(cbtt, pcode, t1, t2, svr);
                
                var rvalMax = Reclamation.TimeSeries.Math.AnnualMax(s, range, Convert.ToInt16(Range1.Text.Substring(0, 2)));
                var rvalMin = Reclamation.TimeSeries.Math.AnnualMin(s, range, Convert.ToInt16(Range1.Text.Substring(0, 2)));

                rvalMax.Appearance.LegendText = cbtt.ToUpper() + " " + pcode.ToUpper() + " Maximum in Range";
                rvalMin.Appearance.LegendText = cbtt.ToUpper() + " " + pcode.ToUpper() + " Minimum in Range";

                var list = new SeriesList();

                list.Add(rvalMax);
                list.Add(rvalMin);

                view.SeriesList = list;
                view.Draw();

            }
            finally
            {
                Cursor = Cursors.Default;

            }
        }
    }
}
