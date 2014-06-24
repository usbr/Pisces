using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Excel;
using Math = Reclamation.TimeSeries.Math;
using Reclamation.Core;

namespace Pisces.NunitTests.SeriesFeatures
{
    [TestFixture]
    public class TestPeriodicSeries
    {

        [Test]
        public void PeriodicSeriesCalendar()
        {

            string fn = TestData.DataPath + "\\Periodic.xlsx";
            //var xls = new SpreadsheetGearExcel(fn);
            var wb = ExcelUtility.GetWorkbookReference(fn);
            var tbl = wb.Tables["Heise"]; //xls.GetTimeSeriesTable("Heise");

            PeriodicSeries s = new PeriodicSeries(tbl);

            DateTime t = new DateTime(1997,4,30);


            double space = s.Interpolate2D(t, 4500000);
            Console.WriteLine(space);
            Assert.AreEqual(1510909, space,0.1);
            space = s.Interpolate2D(DateTime.Parse("1/1/2001"), 500000); // test first row.
            Assert.AreEqual(200000, space,0.1);
            space = s.Interpolate2D(DateTime.Parse("7/31/1965"), 5); // test last row.
            Assert.AreEqual(1, space, 0.1);

            Console.WriteLine(s.Count);
        }

        [Test]
        public void PeriodicSeriesBeginInNovember()
        {

            string fn = TestData.DataPath + "\\Periodic.xlsx";
            var xls = ExcelUtility.GetWorkbookReference(fn);

            //var tbl = xls.GetTimeSeriesTable("BeginInNovember");
            var tbl = xls.Tables["BeginInNovember"];

            PeriodicSeries s = new PeriodicSeries(tbl);

            DateTime t = new DateTime(1997, 4, 30);


            double space = s.Interpolate2D(t, 150);
            space = s.Interpolate2D(DateTime.Parse("11/1/2001"), 150); // test first row.
            Assert.AreEqual(1.5, space, 0.0001);
            space = s.Interpolate2D(DateTime.Parse("5/15/2001"), 150); // test last row.
            Assert.AreEqual(197.5, space, 0.0001);
            space = s.Interpolate(DateTime.Parse("11/3/2001"));
            Assert.AreEqual(3, space,.001);
            space = s.Interpolate2D(DateTime.Parse("5/5/1965"),150);
            Assert.AreEqual(187.5, space,0.0001);

        }
    }
}
