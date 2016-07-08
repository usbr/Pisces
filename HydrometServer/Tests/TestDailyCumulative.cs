using System;
using System.Linq;
using NUnit.Framework;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System.IO;
using HydrometServer;
using Math = Reclamation.TimeSeries.Math;

namespace Pisces.NunitTests.SeriesMath
{
    [TestFixture]
    class TestDailyCumulative
    {

        [Test]
        public void WrongInterval()
        {
            Series pp = new Series();
            pp.TimeInterval = TimeInterval.Irregular;
            pp.Add(DateTime.Now, DateTime.Now.Ticks);

            Series pu = new Series();
            pu.TimeInterval = TimeInterval.Irregular;
            pu.Add(DateTime.Now, DateTime.Now.Ticks);

            var ex = Assert.Throws<ArgumentException>(
                () =>Math.DailyWaterYearRunningTotal(pp, pu));
            Assert.AreEqual("Error: arguments must both have daily interval", ex.Message);

        }

        [Test]
        public void Empty()
        {

          Series pp = new Series();
          pp.TimeInterval = TimeInterval.Daily;

          Series pu = new Series();
          pu.TimeInterval = TimeInterval.Daily;
          pu.Add(DateTime.Now, System.Math.PI);

          var x =  Math.DailyWaterYearRunningTotal(pp, pu);

          Assert.AreEqual(0, x.Count);

        }

        [Test]
        public void EqualLengths()
        {
            var pu = new Series("daily_wrdo_pu");
            pu.TimeInterval = TimeInterval.Daily;
            pu.Add("10-1-2014", 0);
            pu.Add("10-2-2014", 1);
            pu.Add("10-3-2014", 2);
            pu.Add("10-4-2014", 3);
            pu.Add("10-5-2014", 4);
            pu.Add("10-6-2014", 5);

            var pp = new Series("daily_wrdo_pp");
            pp.TimeInterval = TimeInterval.Daily;
            pp.Add("10-1-2014", 0);
            pp.Add("10-2-2014", 1);
            pp.Add("10-3-2014", 1);
            pp.Add("10-4-2014", 1);
            pp.Add("10-5-2014", 1);
            pp.Add("10-6-2014", 1);

            var puNew = Reclamation.TimeSeries.Math.DailyWaterYearRunningTotal(pp, pu);

            for (int i = 0; i < pu.Count; i++)
            {
                Assert.AreEqual(puNew[i].Value, pu[i].Value);
            }

        }

        [Test]
        public void ExtraIncremental()
        {
            var pu = new Series("daily_wrdo_pu");
            pu.TimeInterval = TimeInterval.Daily;
           
            pu.Add("10-3-2014", 5);

            var pp = new Series("daily_wrdo_pp");
            pp.TimeInterval = TimeInterval.Daily;
            pu.Add("10-1-2014", 1);
            pu.Add("10-2-2014", 1);
            pu.Add("10-3-2014", 1); // 5
            pu.Add("10-4-2014", 1); // 6
            pp.Add("10-5-2014", 1); // 7
            pp.Add("10-6-2014", 1); // 8


            var x = Reclamation.TimeSeries.Math.DailyWaterYearRunningTotal(pp, pu);

            Console.WriteLine(x["2014-10-6"]);
            Assert.AreEqual(8, x["2014-10-6"].Value, "PU ");
        }

        /// <summary>
        /// call DailyWaterYearRunningTotal with 
        /// inputs of different lengths.
        /// </summary>
        [Test]
        public void ExtraCumulative()
        {
            var pu = new Series("daily_wrdo_pu");
            pu.TimeInterval = TimeInterval.Daily;
            pu.Add("10-1-2014", 0);
            pu.Add("10-2-2014", 0);
            pu.Add("10-3-2014", 0);
            pu.Add("10-4-2014", 0);
            pu.Add("10-5-2014", 0);
            pu.Add("10-6-2014", 0);

            var pp = new Series("daily_wrdo_pp");
            pp.TimeInterval = TimeInterval.Daily;
            pp.Add("10-5-2014", 0);
            pp.Add("10-6-2014", 0);


            var x = Reclamation.TimeSeries.Math.DailyWaterYearRunningTotal(pp, pu);

            x.WriteToConsole();
            Console.WriteLine(x["2014-10-7"]);
            Assert.AreEqual(0,x["2014-10-7"].Value,"PU ");

        }



       

    }
}
