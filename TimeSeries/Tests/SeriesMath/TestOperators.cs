using System;
using NUnit.Framework;
using Reclamation.TimeSeries;

namespace Pisces.NunitTests.SeriesMath
{
  /// <summary>
  /// Test +,- operators
  /// </summary>
  [TestFixture]
	public class TestOperators
	{


    /// <summary>
    /// test adding two time series together.
    /// </summary>
    [Test]
    public void SimpleAddition()
    {
      Series s1 = TestData.EL68dDailyAverageStage2004;
      Series s2 = TestData.EL68dDailyAverageStage2004;
      Series s3 = s1 + s2;

      Assert.AreEqual(s1.Count,s3.Count);
      for(int i=0; i<s1.Count; i++)
      {
      Assert.AreEqual(s1[i].Value*2,s3[i].Value,0.00001);
      }
    }
    /// <summary>
    /// Test subtracting two time series
    /// </summary>
    [Test]
    public void SimpleSubtraction()
    {
      Series s1 = TestData.EL68dDailyAverageStage2004;
      Series s2 = TestData.EL68dDailyAverageStage2004;
      Series s3 = s1 - s2;

      Assert.AreEqual(s1.Count,s3.Count);
      for(int i=0; i<s1.Count; i++)
      {
        Assert.AreEqual(0,s3[i].Value,0.00001);
      }
      //s3.WriteToConsole();
    }

    /// <summary>
    /// Test invalid addition
    /// </summary>
    [Test]
    [ExpectedException(typeof(InvalidOperationException))]
    public void InvalidAddition()
    {
    Series s1 = new Series(null,"cfs",TimeInterval.Daily);
    Series s2 = new Series(null,"cfs",TimeInterval.Monthly);

      Series s = s1 + s2;
    }
	}
}
