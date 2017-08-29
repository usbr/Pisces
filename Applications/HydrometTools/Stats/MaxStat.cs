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
                MonthDayRange range = monthDayRangePicker1.MonthDayRange;



                string cbtt = multiWaterYearSelector1.cbtt;
                string pcode = multiWaterYearSelector1.pcode;

                //var s = Reclamation.TimeSeries.Math.HydrometDaily(cbtt, pcode);

                var t1 = multiWaterYearSelector1.T1;
                var t2 = multiWaterYearSelector1.T2;
                HydrometHost svr = HydrometInfoUtility.HydrometServerFromPreferences();

                var s = HydrometDailySeries.Read(cbtt, pcode, t1, t2, svr);
                
                var rvalMax = Reclamation.TimeSeries.Math.AnnualMax(s, range, range.Month1);
                var rvalMin = Reclamation.TimeSeries.Math.AnnualMin(s, range,range.Month1 );

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
