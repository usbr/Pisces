using NUnit.Framework;
using Reclamation.TimeSeries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pisces.NunitTests.SeriesMath
{
    [TestFixture]
    class TestET
    {
        
        private static Series Simple(DateTime t, double val)
        {
            var rval = new Series();
            rval.TimeInterval = TimeInterval.Daily;
            rval.Add(t, val);
            return rval;
        }

        private static Series MM(DateTime t)
        {
            var rval = new Series();
            rval.TimeInterval = TimeInterval.Daily;
            rval.Add(t.AddDays(-3),    66.39);
            rval.Add(t.AddDays(-2),    69.41);
            rval.Add(t.AddDays(-1),    71.38);
            rval.Add(t, 66.64);
            return rval;
        }

        [Test]
        public void EstimateDaily()
        {
           DateTime t = new DateTime(2014, 7, 28);
           Series avgTemp =  MM(t);
           Series minTemp = Simple(t,54.68);
           Series maxTemp = Simple(t,84.9);
           Series dewTemp = Simple(t, -102.68);
           Series wind = Simple(t,111.74);
           Series solar = Simple(t ,332.96);

           var pt = Reclamation.TimeSeries.Hydromet.KimberlyPenmanEtSeries.Calculate(t, avgTemp, minTemp, maxTemp, dewTemp, wind, solar, 39.68527, 1797.41);
           Console.WriteLine(pt.ToString());
           Assert.IsTrue(pt.IsMissing);
           //var pt = Reclamation.TimeSeries.Hydromet.KimberlyPenmanEtSeries.Calculate(t, avgTemp,minTemp,maxTemp,dewTemp,wind,solar, 39.68527, 1797.41);

        }
    }
}
