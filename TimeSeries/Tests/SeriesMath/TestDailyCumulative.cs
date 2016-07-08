using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Reclamation.TimeSeries.Tests.SeriesMath
{
    [TestFixture]
    class TestDailyCumulative
    {
        /// <summary>
        /// Using incremental value 1 each day
        /// create cumulative value beginning october 1
        /// </summary>
        [Test]
        public void WaterYearCumulative()
        {
            var t = new DateTime(2015, 9, 12);// start extra to test 'priming'
            var t2 = new DateTime(2015, 10, 31);

            Series s = new Series("", TimeInterval.Daily);
            Series cu = new Series("", TimeInterval.Daily);
            
            while (t <= t2)
            {
                s.Add(t,1);
                t = t.AddDays(1).Date;
            }
            var rval = Math.DailyWaterYearRunningTotal(s, cu);
            rval.RemoveMissing();
            Assert.AreEqual(31, rval.Count);
           // rval.WriteToConsole();

        }

        [Test]
        public void CalendarYearCumulative()
        {
            var t = new DateTime(2015, 12, 12);
            var t2 = new DateTime(2016, 2, 28);

            Series s = new Series("", TimeInterval.Daily);
            Series cu = new Series("", TimeInterval.Daily);

            while (t <= t2)
            {
                s.Add(t, 1);
                t = t.AddDays(1).Date;
            }
            var rval = Math.DailyCalendarYearRunningTotal(s, cu);
            rval.RemoveMissing();
            Assert.AreEqual(59, rval.Count);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void ExistingDailyCumulativeBug()
        {
            var t = new DateTime(2015, 12, 12);
            var t2 = new DateTime(2016, 2, 28);

            Series s = new Series("", TimeInterval.Daily);
            Series cu = new Series("", TimeInterval.Daily);

            while (t <= t2)
            {
                s.Add(t, 1);
                if( t > DateTime.Parse("2016-2-1"))
                    cu.Add(t, 1000);

                t = t.AddDays(1).Date;
            }
            var rval = Math.DailyCalendarYearRunningTotal(s, cu);
            rval.RemoveMissing();
            Assert.AreEqual(27, rval.Count);
            Assert.AreEqual(1026, rval["2016-02-28"].Value);
        }

        [Test]
        public void PrimeCalculationCumulative()
        {
            var t = new DateTime(2015, 10, 1);
            var t2 = new DateTime(2015, 10, 5);

            Series s = new Series("", TimeInterval.Daily);
            Series cu = new Series("", TimeInterval.Daily);

            cu.Add(t, 100);
            cu.Add(t.AddDays(3), 5000); // this should be ignored.

            while (t <= t2)
            {
                s.Add(t, 1);
                t = t.AddDays(1).Date;
            }
            var rval = Math.DailyCalendarYearRunningTotal(s, cu);

            Assert.AreEqual(100, rval[0].Value);
            Assert.AreEqual(101, rval[1].Value);
            Assert.AreEqual(102, rval[2].Value);
            Assert.AreEqual(103, rval[3].Value);
            Assert.AreEqual(104, rval[4].Value);

        }

    }
}
