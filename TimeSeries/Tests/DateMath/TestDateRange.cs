using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;
using Reclamation.Core;

namespace Pisces.NunitTests.DateMath
{
    [TestFixture]
    public class TestDateRange
    {


        [Test]
        public void BadRange()
        {
            try
            {
                MonthDayRange range = new MonthDayRange(12, 3, 2, 2);
                DateRange dr = new DateRange(range, 2001, 1);
            }
            catch(Exception e)
            {
                Assert.IsTrue(e is ArgumentOutOfRangeException);
            }
        }



        [Test]
        public void CalendarYear()
        {
            MonthDayRange rng = new MonthDayRange(1, 1, 12, 31);
            DateRange dr = new DateRange(rng, 2001, 1);

            DateTime t1 = new DateTime(2001, 1, 1);
            DateTime t2 = new DateTime(2001, 12, 31).AddHours(23).AddMinutes(59).AddSeconds(59);

            Assert.AreEqual(365,dr.Count);
            Assert.AreEqual(t1, dr.DateTime1);
            Assert.AreEqual(t2, dr.DateTime2);
        }

        [Test]
        public void WaterYear()
        {
            MonthDayRange rng = new MonthDayRange(10, 1, 9, 30);
            DateRange dr = new DateRange(rng, 2001, 10);

            DateTime t1 = new DateTime(2000, 10, 1);
            DateTime t2 = new DateTime(2001, 9, 30).AddHours(23).AddMinutes(59).AddSeconds(59);

            Assert.AreEqual(365,dr.Count);
            Assert.AreEqual(t1, dr.DateTime1);
            Assert.AreEqual(t2, dr.DateTime2);
        }


        [Test]
        public void AccuralYear()
        {
            MonthDayRange rng = new MonthDayRange(11, 1, 10, 31);
            DateRange dr = new DateRange(rng, 2004, 11);
            
            DateTime t1 = new DateTime(2003,11,1);
            DateTime t2 = new DateTime(2004, 10, 31).AddHours(23).AddMinutes(59).AddSeconds(59);
            
            Assert.AreEqual(366,dr.Count, "leap year");
            Assert.AreEqual(t1, dr.DateTime1);
            Assert.AreEqual(t2, dr.DateTime2);
        }
    }
}
