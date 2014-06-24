using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;

namespace Pisces.NunitTests.SeriesFeatures
{
    [TestFixture]
    public class TestSubset
    {
        [Test]
        public void SubsetByMonth()
        {
            // skip october (10th month)
            int[] months = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 11, 12 };

            string path = TestData.DataPath + "\\";
            string fn1 = path + "LuckyPeakWaterLevel.txt";
            Console.WriteLine("reading " + fn1);
            TextSeries s = new TextSeries(fn1);
            s.Read();

            DateTime d = DateTime.Parse("10/1/2004");
           
            Console.WriteLine(d);
            //10/1/2004	2926.91

            Assert.AreEqual(127,s.IndexOf(d),"test data [DateTime] has changed?");
            Assert.AreEqual(2926.91, s.Lookup(d), "test data [Value] has changed?");
            Series s2 = Reclamation.TimeSeries.Math.Subset(s, months);
            
           Assert.AreEqual( -1,  s2.IndexOf(d),"October 1, 2004 was not removed");


        }



        [Test]
        public void SubsetByMonthSimpleData()
        {
            for (int year = 2000; year <= 2012; year++)
            {

                DateTime t = new DateTime(year, 1, 1);
                int[] months = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

                int expectedDays = 365;
                if (DateTime.IsLeapYear(year))
                {
                    expectedDays = 366;
                }

                Series s = TestData.SimpleDailyData(t, expectedDays, months);
                Assert.AreEqual(expectedDays, s.Count, "Should have " + expectedDays + " values");

                months = new int[] { 10, 11, 12 };
                Series s2 = Reclamation.TimeSeries.Math.Subset(s, months);

                Assert.AreEqual(31 + 30 + 31, s2.Count, "Oct Nov Dec should be 92 days");

                for (int m = 1; m <= 12; m++)
                {
                    s2 = (Reclamation.TimeSeries.Math.Subset(s, new int[] { m }));
                    Assert.AreEqual(DateTime.DaysInMonth(t.Year, m) * m, Reclamation.TimeSeries.Math.Sum(s2), "Filtering by Month = " + m + " s2.Count = " + s2.Count + " s.Count = " + s.Count);

                }
            }
        }


    }
}
