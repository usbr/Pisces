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
    public partial class Monthly : UserControl
    {
        GraphExplorerView view;
        public Monthly()
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
                int wy1 = Convert.ToInt32(textBoxYear.Text);
                int wy2 = Convert.ToInt32(textBoxEndYear.Text);

                string cbtt = textBoxCbtt.Text;
                string pcode = textBoxPcode.Text;

                var s = Reclamation.TimeSeries.Math.HydrometDaily(cbtt, pcode);

                var t1 = new DateTime(wy1 - 1, 10, 1);
                var t2 = new DateTime(wy2, 9, 30);

                s.Read(t1, t2);

                var list = new SeriesList();

                if (checkBoxTotal.Checked)
                {
                    var rval = Reclamation.TimeSeries.Math.MonthlySum(s);
                    rval.Units = "CFS";
                    list.Add(rval);
                }

                if (checkBoxTotalAF.Checked)
                {
                    var tmp = s.Copy();
                    Reclamation.TimeSeries.Math.Multiply(tmp, 1.98347);
                    var rval = Reclamation.TimeSeries.Math.MonthlySum(tmp);
                    rval.Units = "Acre-Feet";
                    list.Add(rval);
                }

                if (checkBoxAverage.Checked)
                {
                    var rval = Reclamation.TimeSeries.Math.MonthlyAverage(s);
                    rval.Units = "CFS";
                    list.Add(rval);
                }

                if (checkBoxAverageAF.Checked)
                {
                    var tmp = s.Copy();
                    Reclamation.TimeSeries.Math.Multiply(tmp, 1.98347);
                    var rval = Reclamation.TimeSeries.Math.MonthlyAverage(tmp);
                    rval.Units = "Acre-Feet";
                    list.Add(rval);
                }

                if (checkBoxChange.Checked)
                {
                    var start = Reclamation.TimeSeries.Math.StartOfMonth(s);
                    var end = Reclamation.TimeSeries.Math.EndOfMonth(s);
                    var rval = end - start;// Reclamation.TimeSeries.Math.Add(end, start, true);
                    list.Add(rval);
                }

                if (checkBoxMax.Checked)
                {
                    var rval = Reclamation.TimeSeries.Math.MonthlyMax(s);
                    list.Add(rval);
                }

                if (checkBoxMin.Checked)
                {
                    var rval = Reclamation.TimeSeries.Math.MonthlyMin(s);
                    list.Add(rval);
                }

                if (checkBoxFirstMonth.Checked)
                {
                    var rval = Reclamation.TimeSeries.Math.StartOfMonth(s);
                    list.Add(rval);
                }

                if (checkBoxEndMonth.Checked)
                {
                    var rval = Reclamation.TimeSeries.Math.EndOfMonth(s);
                    list.Add(rval);
                }

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
