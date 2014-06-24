using System;
using NUnit.Framework;
using Reclamation.TimeSeries;

namespace Pisces.NunitTests.SeriesFeatures
{
	/// <summary>
	/// Summary description for TestMoveTo.
	/// </summary>
	[TestFixture]
	public class TestMoveTo
	{
		public TestMoveTo()
		{
		}

    /// <summary>
    /// Move selection of points to new value.
    /// </summary>
    [Test]
    public void SimpleMoveTo()
    {
      Series ts = TestData.Simple2Day;
      Series original = TestData.Simple2Day;
      Selection sel = new Selection(ts[0].DateTime,ts[ts.Count-1].DateTime,0,50);
      Reclamation.TimeSeries.Math.MoveTo(ts,sel,123.456);

      Console.WriteLine("after move to");
      ts.WriteToConsole(true);
      Console.WriteLine("original");
      original.WriteToConsole(true);
      for(int i=0; i<ts.Count; i++)
      {
        if(original[i].Value >=0 && original[i].Value <=50)
        {
        Assert.AreEqual(123.456,ts[i].Value,0.0001);
          Assert.AreEqual("Edited",ts[i].Flag);
        }
        else
        {
         Assert.AreEqual("",ts[i].Flag);
         Assert.AreEqual(original[i].Value,ts[i].Value,0.0001);
        }
      }
    }

	}
}
