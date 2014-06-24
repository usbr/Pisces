using System;
using NUnit.Framework;
//using TimeSeries.Tests;
using Reclamation.TimeSeries;
namespace Pisces.NunitTests.SeriesFeatures
{
	/// <summary>
	/// Summary description for TestOffset.
	/// </summary>
	[TestFixture]
	public class TestOffset
	{
		public TestOffset()
		{
		}

    [Test]
    public void OffsetAfterDeletion()
    {
      string fn = TestData.DataPath+"\\el68d_export.csv";
      TextSeries s =new TextSeries(fn);
      s.Read();
      int count = s.Count;
      Assert.AreEqual(s.Count,1145,"Test Data has been modified....expected 1145 records. found "+s.Count);
      // delete middle half of data first.
      int idx = s.Count/4;

      Selection sel =new Selection(s[idx].DateTime,s[s.Count-idx-1].DateTime,-1000,1000);
      Console.WriteLine("about to delete selection : "+sel.ToString());
      s.Delete(sel);
      
      Assert.IsTrue(count > s.Count,"some data should be missing ");

      double[] values = new double[s.Count];
      for(int i=0; i<values.Length; i++)
      {
        values[i] = s[i].Value; // copy data before offset.
      }
      // select all data
       sel =new Selection(s[0].DateTime,s[s.Count-1].DateTime,-1000,1000);
      Reclamation.TimeSeries.Math.Offset(s,sel,System.Math.PI);
      for(int i=0; i<values.Length; i++)
      {
        Assert.AreEqual(values[i]+System.Math.PI,s[i].Value,0.000001,"offset failed");
      }

      
    }

    [Test]
    public void Simple()
    {
      Series ts = TestData.Simple2Day;
      Selection sel = new Selection(ts[0].DateTime,ts[ts.Count-1].DateTime,0,50);
      Reclamation.TimeSeries.Math.Offset(ts,sel,50);
      for(int i=0; i<ts.Count; i++)
      {
        Assert.IsTrue(System.Math.Abs(ts[i].Value - 100) <0.0001,"expected all values to be 100");
      }
    }
	}
}
