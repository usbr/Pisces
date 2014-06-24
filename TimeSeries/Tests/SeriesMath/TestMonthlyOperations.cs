using NUnit.Framework;
using Reclamation.TimeSeries;
using System;

namespace Pisces.NunitTests.SeriesMath
{
    [TestFixture]
    public class TestMonthlyAverage
    {

       
        [Test]
        public void MonthlySumAndAverage()
        {
            Series s = new Series();
            DateTime t = DateTime.Parse("01-01-2000");
            while (t.Month == 1)
            {
                s.Add(t, t.Day);
                t = t.AddDays(1);
            }

            var m = Reclamation.TimeSeries.Math.MonthlySum(s);
            Assert.AreEqual(1, m.Count);
            Assert.AreEqual(496, m[0].Value, .001);

            m = Reclamation.TimeSeries.Math.MonthlyAverage(s);
            Assert.AreEqual(1, m.Count);
            Assert.AreEqual(16, m[0].Value, .001);
           

        }
    }
}
