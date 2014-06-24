using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;

namespace Pisces.NunitTests.SeriesFeatures
{
    [TestFixture]
    public class TestLookupIndex
    {

        [Test]
        public void LookupIndex()
        {
            
            Series s = new Series();
            s.Add("2006-01-01", 0);
            s.Add("2006-01-02", 1);
            s.Add("2006-01-03", 2);
            s.Add("2006-01-04", 3);
            s.Add("2006-01-05", 4);

            int i = s.LookupIndex(DateTime.Parse("2006-01-03 12:22 AM"));

            Assert.AreEqual(3, i);

            i = s.LookupIndex(DateTime.Parse("2006-01-03 12:22 AM"),true);

            Assert.AreEqual(2, i);

        
        }

    }
}
