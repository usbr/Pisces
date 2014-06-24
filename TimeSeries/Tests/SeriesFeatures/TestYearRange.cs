using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;
using Reclamation.Core;

namespace Pisces.NunitTests.SeriesFeatures
{
    [TestFixture]
    public class TestYearRange
    {

        [Test]
        public void WaterYear2000()
        {
            YearRange wy = new YearRange(2000, 10);

            Assert.AreEqual(2000, wy.Year, "water year");
            Assert.AreEqual(wy.BeginningMonth, 10, " should be october");
            
            Assert.AreEqual(1999,wy.DateTime1.Year);
            Assert.AreEqual(10,wy.DateTime1.Month);
            Assert.AreEqual(1,wy.DateTime1.Day);

            Assert.AreEqual(2000,wy.DateTime2.Year);
            Assert.AreEqual(9,wy.DateTime2.Month);
            Assert.AreEqual(30,wy.DateTime2.Day);


        }

        [Test]
        public void CalendarYear2000()
        {
            YearRange wy = new YearRange(2000, 1);

            Assert.AreEqual(2000, wy.Year, "water year");
            Assert.AreEqual(wy.BeginningMonth, 1, " should be january");

            Assert.AreEqual(2000, wy.DateTime1.Year);
            Assert.AreEqual(1, wy.DateTime1.Month);
            Assert.AreEqual(1, wy.DateTime1.Day);

            Assert.AreEqual(2000, wy.DateTime2.Year);
            Assert.AreEqual(12, wy.DateTime2.Month);
            Assert.AreEqual(31, wy.DateTime2.Day);


        }

        [Test]
        public void Contains()
        {
            MonthDayRange mdr = new MonthDayRange(9,1,1,1);

            Assert.IsFalse(mdr.ValidBeginningMonth(1));

        }

    }
}
