using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;

namespace Pisces.NunitTests.SeriesMath
{
    [TestFixture]
   public class TestWaterYear
    {
       [Test]
       public void Basic()
       {
           YearRange wy1 = new YearRange(DateTime.Parse("2006-10-1"), 10);
           YearRange wy2 = new YearRange(DateTime.Parse("2006-10-1"), 1);
           YearRange wy3 = new YearRange(DateTime.Parse("2006-10-1"), 11);
           Assert.AreEqual(2007, wy1.Year);

           Assert.AreEqual(2006, wy2.Year);
           Assert.AreEqual(2006, wy3.Year);
       }



       
    }
}
