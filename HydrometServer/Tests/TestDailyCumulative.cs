using System;
using System.Linq;
using NUnit.Framework;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System.IO;
using HydrometServer;

namespace Pisces.NunitTests.SeriesMath
{
    [TestFixture]
    class TestDailyCumulative
    {


        /// <summary>
        /// call DailyWaterYearRunningTotal with 
        /// inputs of different lengths.
        /// </summary>
        [Test]
        public void wrdo_pu_bug()
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

            DateTime t1 = new DateTime(2014,10,3);
            DateTime t2 = new DateTime(2014,10,7);


            var x = Reclamation.TimeSeries.Math.DailyWaterYearRunningTotal(pp, pu);

            x.WriteToConsole();
            Console.WriteLine(x["2014-10-7"]);
            Assert.AreEqual(0,x["2014-10-7"].Value,"PU ");

        }



       

    }
}
