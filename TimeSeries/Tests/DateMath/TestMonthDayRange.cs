using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.TimeSeries;
using NUnit.Framework;
using Reclamation.Core;

namespace Pisces.NunitTests.DateMath
{
    [TestFixture]
    public class TestMonthDayRange
    {


        public TestMonthDayRange()
        {
        }


        [Test]
        public void CountPointsInRange()
        {
            MonthDayRange range = new MonthDayRange(1, 1, 2, 29);
            DateRange dr = new DateRange(range, 2000, 1);

            Assert.AreEqual(dr.Count, 60);

            dr = new DateRange(range, 2001, 1);

            Assert.AreEqual(dr.Count, 59);
        }

        [Test]
        public void WaterYear()
        {
            MonthDayRange r = new MonthDayRange(10, 1, 9, 30);
            DateTime d = new DateTime(2000, 1, 1);

            Assert.IsTrue(r.Contains(d));
        }

        [Test]
        public void LeapYear()
        {
            MonthDayRange dr = new MonthDayRange(1, 1, 2, 29);
            DateTime d = new DateTime(2000,2,29);
            Assert.IsTrue(dr.Contains(d),"leap year failed");

            d = new DateTime(2001, 3, 1);
            Assert.IsFalse(dr.Contains(d) );
            

        }

        [Test]
        public void FullYear()
        {
            // this range includes full year by wrap-around
            MonthDayRange dr = new MonthDayRange(6, 15, 6, 14);

            Series s = TestData.SouthForkBoise;
            Console.WriteLine(s.Count);
            for (int i = 0; i < s.Count; i++)
            {
                Assert.IsTrue(dr.Contains(s[i].DateTime));
            }
        }

        [Test]
        public void Testing()
        {
            MonthDayRange dr = new MonthDayRange(6, 15, 6, 23);

            DateTime d = new DateTime(2006,1,30);
            Assert.IsFalse(dr.Contains(d));
            d = new DateTime(2005, 6, 15);
            Assert.IsTrue(dr.Contains(d));

            d = new DateTime(2005, 6, 23);
            Assert.IsTrue(dr.Contains(d));

            d = new DateTime(2005, 6, 30);
            Assert.IsFalse(dr.Contains(d));

            d = new DateTime(2005, 7, 1);
            Assert.IsFalse(dr.Contains(d));

        }

        [Test]
        public void WaterAndLeapYear()
        {
            MonthDayRange dr = new MonthDayRange(10, 1, 9, 30);
            Assert.IsTrue(dr.Contains(new DateTime(1948,2,29)));
        }

        [Test]
        public void TimeSeries()
        {
            Series s = TestData.SouthForkBoise;
            MonthDayRange dr = new MonthDayRange(10, 1, 9, 30);
            MonthDayRange d2 = new MonthDayRange(11, 1, 1, 31);
            for (int i = 0; i < s.Count; i++)
            {
                DateTime t = s[i].DateTime;
                Assert.IsTrue(dr.Contains(t),t.ToString());

                bool inRange2 = (t.Month > 10 || t.Month == 1);
                Assert.AreEqual(inRange2, d2.Contains(t));
            }
        }

      
    }

}
