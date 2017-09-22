using NUnit.Framework;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System;
using System.IO;

namespace Pisces.NunitTests.SeriesMath
{
    [TestFixture]
   public class TestMovingAverage
    {

       /// <summary>
       /// there is not data on 7/9/2004 
       /// therefore any moving average that 'touches' this date
       /// should be missing.  Don't compute the average
       /// </summary>
       [Test]
       public void SevenDayMovingInstantSparse()
       {

          // string fn = Path.Combine(TestData.DataPath, "temp example 7 day max.xls");
          // var s = new ExcelDataReaderSeries(fn, "sparse", "C", "D");
            var s = new TextSeries(Path.Combine(TestData.DataPath, "temp_sparse.csv"));
           s.Read();
           Series s2 = Reclamation.TimeSeries.Math.SevenDADMAX(s);
            //Series expected = new ExcelDataReaderSeries(fn, "7dadmax", "A", "B");
            TextSeries expected = new TextSeries(Path.Combine(TestData.DataPath, "7dadmax.csv"));
           expected.Read();

           Assert.AreEqual(expected[0].DateTime.Date, s2[0].DateTime.Date);
           Assert.AreEqual(PointFlag.Missing, s2[0].Flag); //
           
       }



       [Test]
       public void SevenDayMovingInstant()
       {
            //string fn = Path.Combine(TestData.DataPath, "temp example 7 day max.xls");
            //var s = new ExcelDataReaderSeries(fn, "457373", "C", "D");
            var s = new TextSeries(Path.Combine(TestData.DataPath, "457373.csv"));

            s.Read();

           Series s2 = Reclamation.TimeSeries.Math.SevenDADMAX(s);

            TextSeries expected = new TextSeries(Path.Combine(TestData.DataPath, "7dadmax.csv"));
            expected.Read();

            for (int i = 0; i < expected.Count; i++)
           {
               Assert.AreEqual(expected[i].Value, s2[i].Value, 0.001);    
           }
           
         
       }

       [Test]
       public void SevenDayMovingDaily()
       {
           string fn = Path.Combine(TestData.DataPath, "SpecificationTestData.csv");
            var csv = new CsvFile(fn);
          DataTableSeries s = new DataTableSeries(csv, TimeInterval.Daily, "Date", "JulianDay");

            Series expected = new DataTableSeries(csv,TimeInterval.Daily, "Date", "SevenDayMovingAverage");
         s.Read();
         expected.Read();

        Series s2 = Reclamation.TimeSeries.Math.SevenDayMovingAverage(s);
        Assert.AreEqual(2798, s2.Count);
        expected.RemoveMissing();
        Assert.AreEqual(expected.Count, s2.Count);

        Series diff = expected - s2;

        double d = Reclamation.TimeSeries.Math.Sum(diff);
        Assert.AreEqual(0, d, 0.001);

       }

       [Test]
       public void Basic()
       {
           Reclamation.TimeSeries.Series s = new Series();
           DateTime t = DateTime.Now.Date;
           for (int i = 1; i < 10; i++)
           {
               s.Add(t, i);
               t = t.AddHours(1);
           }
           var ma = Reclamation.TimeSeries.Math.MovingAvearge(s, 2);
           //s.WriteToConsole();
           //ma.WriteToConsole();
           /*
            2007-05-22 00:00:00.00	1.00
2007-05-22 01:00:00.00	2.00
2007-05-22 02:00:00.00	3.00
2007-05-22 03:00:00.00	4.00
2007-05-22 04:00:00.00	5.00
2007-05-22 05:00:00.00	6.00
2007-05-22 06:00:00.00	7.00
2007-05-22 07:00:00.00	8.00
2007-05-22 08:00:00.00	9.00
Name:  2 hour average
ScenarioName 
SeriesType: Irregular
units:
2007-05-22 02:00:00.00	2.00
2007-05-22 03:00:00.00	3.00
2007-05-22 04:00:00.00	4.00
2007-05-22 05:00:00.00	5.00
2007-05-22 06:00:00.00	6.00
2007-05-22 07:00:00.00	7.00
2007-05-22 08:00:00.00	8.00
            */
           Assert.AreEqual(7, ma.Count);

           Assert.AreEqual(2, ma[0].Value,"2 hr moving avg");
           Assert.AreEqual(3, ma[1].Value, "2 hr moving avg");
           Assert.AreEqual(4, ma[2].Value, "2 hr moving avg");
           Assert.AreEqual(5, ma[3].Value, "2 hr moving avg");
           Assert.AreEqual(6, ma[4].Value, "2 hr moving avg");
           Assert.AreEqual(7, ma[5].Value, "2 hr moving avg");
           Assert.AreEqual(8, ma[6].Value, "2 hr moving avg");

       }

       
    }
}
