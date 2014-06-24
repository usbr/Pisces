using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Reclamation.TimeSeries;

namespace Pisces.NunitTests.SeriesFeatures
{
    [TestFixture]
    public class TestFillMissingData
    {
        [Test]
        public void MissingData()
        {
            var s = new Series();
            s.TimeInterval = TimeInterval.Daily;
            s.Add("1/1/2006", 20);
            s.Add("1/2/2006", 20);
            s.Add("1/3/2006", 20);
            s.Add("1/4/2006", 20);
            s.Add("1/5/2006", 20);
            s.Add("1/6/2006", 20);
            // s.Add("1/7/2006", -40);
            s.Add("1/8/2006", 10);
            s.Add("1/9/2006", 50);
            s.Add("1/10/2006", 20);
            s.Add("1/11/2006", 20);
            s.Add("1/12/2006", Point.MissingValueFlag);
            s.Add("1/13/2006", 20);
            s.Add("1/14/2006", 20);
            s.Add("1/15/2006", 20);

            
            s = Reclamation.TimeSeries.Math.FillMissingWithZero(s, DateTime.Parse("12/31/2005"), 
                DateTime.Parse("1/16/2006"));// add extra day on beginning and end
            s.WriteToConsole();
            Assert.AreEqual(17, s.Count);
            Assert.AreEqual(0.0, s["1/7/2006"].Value, 0.0001);
            Assert.AreEqual(0.0, s["1/12/2006"].Value, 0.0001);
           
        }
    }
}
