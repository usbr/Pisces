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
 
    }
}
