//using System;
using System.Data;
using NUnit.Framework;
using Reclamation.TimeSeries;
using DateTime = System.DateTime;
using Reclamation.Core;
using System.IO;

namespace Pisces.NunitTests.SeriesMath
{
	/// <summary>
	/// Summary description for TestInterpolation.
	/// </summary>
	[TestFixture]
	public class TestInterpolation
	{
		public TestInterpolation()
		{
		}

    [Test]
    public void BasicTest()
    {
      Series s = TestData.Simple2Day;
      DateTime t = new DateTime(2004,12,2,12,0,30);
      double interp = Math.Interpolate(s,t);
      Assert.AreEqual(75,interp,0.001);

    }

    [Test]
    public void InterpolateHourly()
    {
        Series s = new Series();
        s.Add("01/01/05 00:46:12.0",6.41);
        s.Add("01/01/05 01:46:12.0",6.41);
        s.Add("01/01/05 02:46:12.0",6.25);
        s.Add("01/01/05 03:46:12.0", 6.25);

        var hourly = Reclamation.TimeSeries.Math.InterpolateHourly(s);
        hourly.WriteToConsole();
        Assert.AreEqual(hourly[0].DateTime.Hour, 1);
    }

        [Test]
        public void DataTableInterpolationSimple()
        {
            DataTable t = new DataTable();
            t.Columns.Add("x", typeof(double));
            t.Columns.Add("y", typeof(double));

            t.Rows.Add(1, 1);
            t.Rows.Add(2, 2);
            t.Rows.Add(4, 4);


            int idx;
            double x = 3;
            double y = Reclamation.TimeSeries.Math.Interpolate(t, x, "x", "y", out idx);

            Assert.AreEqual(3, y,"interpolated value");
            Assert.AreEqual(2, idx,"nearest index");
        }

        [Test]
        public void DataTableInterpolationFirstValue()
        {
            DataTable t = new DataTable();
            t.Columns.Add("x", typeof(double));
            t.Columns.Add("y", typeof(double));

            t.Rows.Add(1, 1);
            t.Rows.Add(2, 2);
            t.Rows.Add(4, 4);


            int idx;
            double x = 1;
            double y = Math.Interpolate(t, x, "x", "y", out idx);

            Assert.AreEqual(1, y, "interpolated value");
            Assert.AreEqual(0, idx, "nearest index");
        }
        [Test]
        public void DataTableInterpolationLastValue()
        {
            DataTable t = new DataTable();
            t.Columns.Add("x", typeof(double));
            t.Columns.Add("y", typeof(double));

            t.Rows.Add(1, 1);
            t.Rows.Add(2, 2);
            t.Rows.Add(4, 4);


            int idx;
            double x = 4;
            double y = Math.Interpolate(t, x, "x", "y", out idx);

            Assert.AreEqual(4, y, "interpolated value");
            Assert.AreEqual(2, idx, "nearest index");
        }

        [Test]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void DataTableInterpolationWithError1()
        {
            DataTable t = new DataTable();
            t.Columns.Add("x", typeof(double));
            t.Columns.Add("y", typeof(double));

            t.Rows.Add(1, 1);
            t.Rows.Add(2, 2);

            int idx;
            double x = 3;
            double y = Math.Interpolate(t, x, "x", "y", out idx);
        }


        [Test]
        [ExpectedException(typeof(System.ArgumentException))]
        public void DataTableInterpolationWithError2()
        {
            DataTable t = new DataTable();

            double x = 3;
            double y = Reclamation.TimeSeries.Math.Interpolate(t, x, "x", "y");
        }

        [Test]
        public void InterpolateWithStyle()
        {
            string zipFile = Path.Combine(Globals.TestDataPath, "UofIDisaggregationTest.zip");
            var path = FileUtility.GetTempFileName(".pdb");
            ZipFile.UnzipFile(zipFile, path);

            Reclamation.Core.SQLiteServer pDB = new Reclamation.Core.SQLiteServer(path);
            TimeSeriesDatabase DB = new TimeSeriesDatabase(pDB,false);
            Series sReal = DB.GetSeriesFromName("SS_Day_Mean"); sReal.Read();
            Series sEst = DB.GetSeriesFromName("TS_Mon_Mean"); sEst.Read();

            Series sOut = Reclamation.TimeSeries.Math.InterpolateWithStyle(sReal, sEst, "2001-3-1", "2003-12-1");
            
            // Can't get the Series.Values.Sum() extension... workaround below.
            double sum1=0, sum2=0;
            for (int i = 0; i < sEst.Count; i++)
            {
                sum1 = sum1 + sEst[i].Value;
                sum2 = sum2 + sOut[i].Value;
            }
            sum1 = System.Math.Round(sum1, 2);
            sum2 = System.Math.Round(sum2, 2);
            Assert.AreEqual(sum1, sum2);
        }
	}
}
