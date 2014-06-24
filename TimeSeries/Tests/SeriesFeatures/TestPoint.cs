using System;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;

namespace Pisces.NunitTests.SeriesFeatures
{
    [TestFixture]
    public class TestPoint
    {


        [Test]
        public void TestMultiplyByScalar()
        {
            Point pt = new Point(DateTime.Now, 5);

            Point pt2 = pt * 2;

            Assert.AreEqual(10, pt2.Value);
        }
    }
}
