using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;
using Reclamation.Core;

namespace Pisces.NunitTests.SeriesFeatures
{
    [TestFixture]
    public class TestTimeRange
    {


        [Test]
        public void TestTimeRangeOutput()
        {
            var t1 = new DateTime(1999, 1, 1);
            var t2 = new DateTime(2001, 1, 1);
            TimeRange testTime = new TimeRange(t1, t2);
            foreach (TimeRange item in testTime.List())
            {
                
                Console.WriteLine(item.StartDate+", " +item.EndDate);
            }
        }



    }
}
