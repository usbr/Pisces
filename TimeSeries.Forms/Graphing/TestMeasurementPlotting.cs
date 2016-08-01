using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms.Graphing
{
    public partial class TestMeasurementPlotting : Form
    {
        public TestMeasurementPlotting()
        {
            InitializeComponent();
            Draw();
        }
        public void Draw()
        {
            var f = @"C:\Users\KTarbet\Documents\project\Pisces\PiscesTestData\data\rating_tables\yak.pdb";
            var svr = new SQLiteServer(f);
            var db = new TimeSeriesDatabase(svr);

            var m = db.Hydrography.GetMeasurements("YRWW");

            ratingTableZedGraph1.Draw(m.ToArray());
        }
    }
}
