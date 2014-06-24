using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;

namespace Pisces.NunitTests.SeriesFeatures
{
    [TestFixture]
    public class TestMissingRecordCount
    {
        [Test]
        public void CountFlags()
        {
            DateTime t = DateTime.Now.Date;
            Series s = new Series();
            s.AddMissing(t);
            t = t.AddDays(1);
            s.AddMissing(t);
            t = t.AddDays(1);
            s.Add(t, 123);

            Assert.AreEqual(3, s.Count);
            Assert.AreEqual(2, s.CountMissing());
        }
    }
}
