using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;
using Math = Reclamation.TimeSeries.Math;

namespace Pisces.NunitTests.SeriesFeatures
{
    [TestFixture]
    public class TestShiftToYear
    {

        [Test]
        public void EntiatRiver()
        {
            Series s = new Series();

            DateTime t = new DateTime(2006, 5, 24);

            for (int i = 0; i < 365; i++)
            {
                s.Add(t, i);
                t = t.AddDays(1);
            }
            Series w = Math.ShiftToYear(s, 2000);

            Assert.AreEqual(2000, w[0].DateTime.Year);
            Assert.AreEqual(5,w[0].DateTime.Month);
            Assert.AreEqual(24,w[0].DateTime.Day);

        }

        [Test]
        public void Feb29Bug()
        {
            Series s = new Series();
            s.Add("1-1-1921", 0);
            s.Add("1921-06-25", 364);
            var shifted = Reclamation.TimeSeries.Math.ShiftToYear(s, 2000);

            Assert.AreEqual(25, shifted[1].DateTime.Day);
            shifted.WriteToConsole();

        }
    }
}
