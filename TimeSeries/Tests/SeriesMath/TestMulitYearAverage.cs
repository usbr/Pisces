using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using Reclamation.TimeSeries;

namespace Pisces.NunitTests.SeriesMath
{
    /// <summary>
    /// Summary description for TestThrityYearAverage
    /// </summary>
    [TestFixture]
    public class TestMulitYearAverage
    {
        public TestMulitYearAverage()
        {
        
        }



        [Test]
        public void MultiYearAverageTestEntiatRiver()
        {
            Series s = TestData.EntiatRiver;

            Assert.AreEqual(3642, s.Count);
            Series avg = Reclamation.TimeSeries.Math.MultiYearDailyAverage(s, 10);

            Assert.AreEqual(new DateTime(2000, 10, 1), avg[0].DateTime);
            Assert.IsTrue(avg.Count <= 366);
            avg.WriteToConsole();
            Assert.AreEqual(159.6, avg["1-1-2001"].Value, 0.01);
            
        }
    }
}
