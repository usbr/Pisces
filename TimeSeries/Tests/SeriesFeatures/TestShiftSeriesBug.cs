using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;
using Reclamation.Core;
using System;

namespace Pisces.NunitTests.SeriesFeatures
{
    [TestFixture]
    public class TestShiftSeriesBug
    {

        [Test]
        public void Test1()
        {
            Series s = new Series();

            DateTime t = new DateTime(2011, 2, 28);
            DateTime t2 = new DateTime(2011, 12, 31);


            do
            {
                s.Add(t, t.Day);
                t = t.AddDays(1);
            } while (t <= t2);

           var s2 = Reclamation.TimeSeries.Math.ShiftToYear(s, 2008);
            var dec31 = s2[s2.Count -1];

            Assert.AreEqual(2016, dec31.DateTime.Year);
            s2.WriteToConsole();

        }
    }
}

