using System.IO;
using NUnit.Framework;
using Reclamation.Core;
using Reclamation.TimeSeries;

namespace Pisces.NunitTests.SeriesMath
{
    [TestFixture]
    public class TestInterpolate2D
    {
        [Test]
        public void Interpolate2D()
        {
            var tbl = TestData.RirieQ();

            // sanity check, no iterpolation needed
            double q1 = Reclamation.TimeSeries.Math.Interpoloate2D(tbl, 5023, 0.5);
            Assert.AreEqual(55.0, System.Math.Round(q1, 1)); 

            // random check
            double q2 = Reclamation.TimeSeries.Math.Interpoloate2D(tbl, 5101.86, 0.01);
            double q3 = Reclamation.TimeSeries.Math.Interpoloate2D(tbl, 5101.86, 0.26);
            Assert.AreEqual(59.57, System.Math.Round(q2 + q3, 2));
            double q4 = Reclamation.TimeSeries.Math.Interpoloate2D(tbl, 5101.89, 0.01);
            double q5 = Reclamation.TimeSeries.Math.Interpoloate2D(tbl, 5101.89, 0.26);
            Assert.AreEqual(59.58, System.Math.Round(q4 + q5, 2));

            // bounds check, higher gate opening than in table, currently 
            // allowed but maybe we don't want to allow this, gets max flow
            // at elevation with max gate opening
            double q6 = Reclamation.TimeSeries.Math.Interpoloate2D(tbl, 5023, 8);
            Assert.AreEqual(924.0, System.Math.Round(q6, 1));

            // bounds check, higher forebay than in table, invalid
            double q7 = Reclamation.TimeSeries.Math.Interpoloate2D(tbl, 5119.1, 0.5);
            Assert.AreEqual(Point.MissingValueFlag, System.Math.Round(q7, 1));

            // max flow at max elevation and max opening
            double q8 = Reclamation.TimeSeries.Math.Interpoloate2D(tbl, 5119, 7);
            Assert.AreEqual(2125.0, System.Math.Round(q8, 1));
        }


        [Test]
        public void Interpolate2DWithDatabase()
        {
            Logger.EnableLogger();
            var fn = FileUtility.GetTempFileName(".pdb");
            File.Delete(fn);

            SQLiteServer svr = new SQLiteServer(fn);
            var db = new TimeSeriesDatabase(svr,false );

            var c = new CalculationSeries("rir_q");
            var path = Path.Combine(TestData.DataPath, "rating_tables");
            path = Path.Combine(path, "rir_q.txt");

            c.Expression = "FileLookupInterpolate2D(rir_fb, rir_ra, \"" + path + "\")" +
                " + FileLookupInterpolate2D(rir_fb, rir_rb, \"" + path + "\")";
            c.TimeInterval = TimeInterval.Irregular;
            db.AddSeries(c);

            var fb = new Series("rir_fb");
            fb.TimeInterval = TimeInterval.Irregular;
            db.AddSeries(fb);
            fb.Add("6-1-2011", 5110.99);
            fb.Add("6-2-2011", 5111.31);
            fb.Add("6-3-2011", 5111.71);
            fb.Add("6-4-2011", 5112.09);

            var ra = new Series("rir_ra");
            ra.TimeInterval = TimeInterval.Irregular;
            ra.Add("6-1-2011", 2.1);
            ra.Add("6-2-2011", 1.29);
            ra.Add("6-3-2011", 1.29);
            ra.Add("6-4-2011", 1.29);
            db.AddSeries(ra);

            var rb = new Series("rir_rb");
            rb.TimeInterval = TimeInterval.Irregular;
            rb.Add("6-1-2011", 2.1);
            rb.Add("6-2-2011", 1.28);
            rb.Add("6-3-2011", 1.28);
            rb.Add("6-4-2011", 1.28);
            db.AddSeries(rb);

            TimeSeriesImporter ti = new TimeSeriesImporter(db);
            ti.Import(fb, computeDependencies: true);// this should force a calculation...

            var q = db.GetSeriesFromTableName("rir_q");
            Assert.NotNull(q, "Series not created");

            q.Read();

            /* 
             * Flows from Hydromet
             * 6-1-2011, 1009.87
             * 6-2-2011, 602.24
             * 6-3-2011, 603.32
             * 6-4-2011, 604.34
             */

            Assert.AreEqual(4, q.Count);
            Assert.AreEqual(1009.87, System.Math.Round(q[0].Value, 2));
            Assert.AreEqual(603.32, System.Math.Round(q[2].Value, 2));
        }


        [Test]
        public void TestInterp2dIslandParkWithDatabase()
        {
            Logger.EnableLogger();
            var fn = FileUtility.GetTempFileName(".pdb");
            File.Delete(fn);

            SQLiteServer svr = new SQLiteServer(fn);
            var db = new TimeSeriesDatabase(svr, false);

            var c = new CalculationSeries("isl_q");
            var path = Path.Combine(TestData.DataPath, "rating_tables");
            var f1 = Path.Combine(path, "isl_q_single.txt");
            var f2 = Path.Combine(path, "isl_q_both.txt");

            var fb = new Series("isl_fb");
            fb.Add("2017-09-19 10:45", 6299.90);
            fb.Add("2017-09-19 11:00", 6302.0);

            var ra = new Series("isl_ra");
            ra.Add("2017-09-19 10:45", 0.12);
            ra.Add("2017-09-19 11:00", 2.5);

            var rb = new Series("isl_rb");
            rb.Add("2017-09-19 10:45", 0.1);
            rb.Add("2017-09-19 11:00", 0);

            var x = Math.FileLookupInterpolate2DIslandPark(fb, ra, rb, f1, f2);

            x.WriteToConsole();

            Assert.AreEqual(51.66, x[0].Value, 0.01);
            Assert.AreEqual(619.0, x[1].Value, 0.01);

        }
    }
}
