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

                var s = Reclamation.TimeSeries.Math.HydrometDaily(cbtt, pcode);

                var t1 = new DateTime(wy1 - 1, 10, 1);
                var t2 = new DateTime(wy2, 9, 30);

                s.Read(t1, t2);

                if (AF.Checked)
                {
                    Reclamation.TimeSeries.Math.Multiply(s, 1.98347);
                    s.Units = "Acre-Feet";
                }

                var rval = Reclamation.TimeSeries.Math.AnnualSum(s, range, 10);//Convert.ToInt16(Range1.Text.Substring(0, 2)));

                rval.Appearance.LegendText = cbtt.ToUpper() + " " + pcode.ToUpper() + " Sum in Range";
                
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

    }
}
