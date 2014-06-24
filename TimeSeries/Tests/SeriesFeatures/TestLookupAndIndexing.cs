using NUnit.Framework;
using Reclamation.Core;
using Reclamation.TimeSeries;

namespace Pisces.NunitTests.SeriesFeatures
{
	/// <summary>
	/// Summary description for TestLookupAndIndexing.
	/// </summary>
	[TestFixture]
	public class TestLookupAndIndexing
	{
		public TestLookupAndIndexing()
		{
		}


    [Test]
    public void LookupSimple2Day()
    {
    Series s = TestData.Simple2Day;
      System.DateTime t  = System.DateTime.Now;

      Point pt = new Point(t,1234.5678);
      s.Insert(pt);
      Assert.IsTrue(s.Count == TestData.Simple2Day.Count+1);
      s.WriteToConsole();
      System.Console.WriteLine("count = "+s.Count);

      double lookup = s.Lookup(t);

    }
    
    [Test]
    public void LookupEveryOne()
    {
      Series input = TestData.Banks;
      Series ts = Math.TimeWeightedDailyAverage(input);
      System.DateTime date = new System.DateTime(2005,5,17);
      for(int i=0; i<ts.Count; i++)
      {
        double val = ts.Lookup(date);
        date = date.AddDays(1);
      }
    }

    [Test]
    public void IndexOfTest()
    {

      Series ts = TestData.LindCouleeWW1DailyAverageStage2004;
      Performance p = new Performance();
      for(int i=0; i<ts.Count; i++)
      {
        System.DateTime d = ts[i].DateTime ;
        Assert.IsTrue(ts.IndexOf(d) == i);
      }
      p.Report("done with IndexOfTest()");
    }

    [Test]
    public void LookupPerfTest()
    {

      
      Series ts = TestData.LindCouleeWW1InstantanousStage2004;
      Performance p = new Performance();
      for(int i=0; i<ts.Count; i++)
      {
        System.DateTime d = ts[i].DateTime ;
        Assert.IsTrue(ts.LookupIndex(d)==i);
      }
      Assert.IsTrue(p.ElapsedSeconds<5," Test was to slow! it took "+p.ElapsedSeconds + " seconds");// measured 0.5 seconds on average.
      // before BinaryLookupIndex() was  219.4375 seconds elapsed.
      p.Report("done with LookupPerfTest()");
    }

	}
}
