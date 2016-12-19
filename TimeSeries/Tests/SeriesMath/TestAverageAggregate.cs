using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;
using Reclamation.Core;
using System.IO;

namespace Pisces.NunitTests.SeriesMath
{
    [TestFixture]
    public class TestAverageAggregate
    {


        public TestAverageAggregate()
        {

        }


        [Test]
        public void AprilAverageHrsi()
        {

            Series s = new TextSeries(Path.Combine(TestData.DataPath, "hrsiDailyModsim.csv"));
            s.Read();
            Assert.AreEqual(5418, s.Count,"reading file");

            double[] expected ={
            11133.3,
8613.8,
14385.9,
10328.0,
9689.5,
12839.8,
10611.5,
13445.1,
13052.1,
10152.5,
12536.3,
7573.6,
11985.8,
10534.4,
9936.5,
12047.7,
13240.2,
10389.1,
13993.5,
10028.9,
9384.8,
12515.8,
14188.1,
7887.3,
9998.4,
13970.7,
7438.4,
10280.7,
9050.2,
10602.3,
10659.1,
7180.2,
11815.6,
14941.5,
11761.7,
10830.9,
11825.9,
11968.4,
9618.7,
9729.7,
10152.0,
14670.9,
12391.5,
16330.4,
14960.8,
14946.8,
16466.4,
10045.9,
17344.9,
10593.5,
15629.1,
10727.6,
11894.0,
11303.9,
10471.4,
10555.0,
15624.5,
13681.5,
14531.3,
10780.0,
13061.1,
11222.4,
14146.1};

            MonthDayRange range = new MonthDayRange(4, 1, 4, 30);
            Series s2 = Reclamation.TimeSeries.Math.AggregateAndSubset(StatisticalMethods.Average, s, range, 10);

            Assert.AreEqual(63, s2.Count);

            Assert.AreEqual(1944, s2[0].DateTime.Year);

            for (int i = 0; i < s2.Count; i++)
            {
                Assert.AreEqual(expected[i], s2[i].Value, 0.1);
            }


        }
    }
}
