using System;
using System.Data;
using NUnit.Framework;
using Reclamation.TimeSeries;

namespace Pisces.NunitTests.SeriesFeatures
{
	/// <summary>
	/// Summary description for SeriesListTest.
	/// </summary>
  [TestFixture]
  public class TestSeriesList
  {
    public TestSeriesList()
    {
    }


    /// <summary>
    /// Create a DataTable from a list with 1 time Series.
    /// </summary>
    [Test]
    public void CompositeTable1()
    {
      SeriesList list = new SeriesList();
      Series s1 = TestData.Simple1Day;
      list.Add(s1);

      DataTable t2 = list.ToDataTable(false) ;
      Assert.IsTrue(t2.Equals(list[0].Table),"expected original table to be retured. not a copy");

      Assert.IsTrue(t2.Rows.Count == TestData.Simple1Day.Count,"must have same number of rows if only 1 table");

      Series s = new Series(t2,"",TimeInterval.Irregular);

      for(int i=0; i<s.Count; i++)
      {
       Assert.IsTrue(System.Math.Abs(s[i].Value - s1[i].Value) <0.0000000001);
       Assert.IsTrue(s[i].DateTime == s1[i].DateTime);
      }
   }

      // sorting by units .. list.sort() seems strange..
      // only used by GigaSoftTimeSeriesGraph... depricated
      //[Test]
      //public void TestSortingByUnits()
      //{
      //   // SeriesList list = new SeriesList();
      //   // Series s1 = TestData.Simple1Day;
      //   // s1.Units = "cfs";
      //   // list.Add(s1);

      //   // Series s2 = s1.Copy();
      //   // s2.Units = "acre feet";
      //   // list.Add(s2);

      //   // Series s3 = s1.Copy();
      //   // s3.Units = "cfs";
      //   // list.Add(s3);

      //   // Series s4 = s1.Copy();
      //   // s4.Units = "acre feet";
      //   // list.Add(s4);

      //   // Series s5 = s1.Copy();
      //   // s5.Units = "feet";
      //   // list.Add(s5);

      //   // list.Sort();

      //   //string[] expectedOrder =  {"acre feet","acre feet","cfs","cfs","feet"};

      //   //for (int i = 0; i < expectedOrder.Length; i++)
      //   //{
      //   //    Assert.AreEqual(expectedOrder[i], list[i].Units);
      //   //}
      //}

    /// <summary>
    /// Create a DataTable from a list with 2 time Series.
    /// </summary>
    [Test]
    public void CompositeTable2()
    {
      SeriesList list = new SeriesList();
      list.Add(TestData.Simple1Day);
      list.Add(TestData.Simple2Day);

      DataTable t2 = list.ToDataTable(list.Count>1) ;

      Assert.IsTrue(t2.Rows.Count == 6);

    }


    //[Test]
    //private void PerfTest()
    //{
    //  // very slow... 4 minutes +
    //  Series s  = Tests.TestData.LindCouleeWW1InstantanousStage2004;
    //  Series s2  = Tests.TestData.LindCouleeWW1InstantanousStage2004;

    //  SeriesList list = new SeriesList();
    //  list.Add(s);
    //  list.Add(s2);
      
    //  DataTable c = list.CompositeTable;

    //}


	}
}
