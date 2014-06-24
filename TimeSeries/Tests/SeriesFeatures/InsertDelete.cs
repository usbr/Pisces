using System;
using NUnit.Framework;
using Reclamation.TimeSeries;

namespace Pisces.NunitTests.SeriesFeatures
{
	/// <summary>
	///  Basic tests inserting points and other series.
	/// </summary>
	[TestFixture]
	public class InsertDelete
	{
		public InsertDelete()
		{
		}

    [Test]
    public void InsertPoints()
    {
      Series s = TestData.Simple2Day;
      Point pt = s[2];
      double val = pt.Value;
      pt.Value = val*2;
      s.Insert(pt,true); // should overwrite in this case..
      Assert.IsTrue(s.Messages.Count == 1, "s.Messages.Count  = " + s.Messages.Count+ " s.Count = "+s.Count);
      Assert.AreEqual(val*2,s[2].Value,0.00001,"Insert with overwrite failed");
      
      pt = s[5];
      val = pt.Value;
      double val2 =val;
      pt.Value = val *2;
      s.Insert(pt);

      Assert.AreEqual(val, s[5].Value,0.0001 ,"insert should have skipped this point");

      int count = s.Count;
      DateTime t  = DateTime.Now;
      pt = new Point(t,1234.5678);
      s.Insert(pt);
      Assert.IsTrue(s.Count == count+1);
      double lookup = s.Lookup(t);

      Assert.AreEqual( 1234.5678,lookup,0.00001,"inserted point not found");
      
      Assert.IsTrue(s.Messages.Count == 2,"expected two messages from two abnormal inserts count is "+s.Messages.Count);
      foreach (string str in s.Messages )
      {
        Console.WriteLine(str);
      }
    }

    [Test]
    public void InsertSeries()
    {
      Series banks = TestData.Banks;
      banks.Insert(TestData.Simple2Day);
      Assert.AreEqual(banks.Count, TestData.Banks.Count + TestData.Simple2Day.Count);
    }

        [Test]
        public void InsertEmptySeriesNoFlags()
        {
            Series banks = TestData.Banks;
            Series s = new Series();
            s.Table.Columns.Remove("flag");
            banks.Table.Columns.Remove("flag");
            banks.Table.AcceptChanges();
            s.Table.AcceptChanges();
            Assert.AreEqual(false, banks.HasFlags, "HasFlags should be false");
            banks.Insert(s);
           Assert.IsTrue(banks.Count == TestData.Banks.Count);
        }

        [Test]
        public void InsertEmptySeriesNoFlagsReverse()
        {
            Series banks = TestData.Banks;
            Series s = new Series();
            s.Table.Columns.Remove("flag");
            banks.Table.Columns.Remove("flag");
            banks.Table.AcceptChanges();
            s.Table.AcceptChanges();
            s.Insert(banks);
            Assert.IsTrue(banks.Count == TestData.Banks.Count);
        }
        [Test]
        public void InsertEmptySeries()
        {
            Series banks = TestData.Banks;
            Series s = new Series();
            banks.Insert(s);
            Assert.IsTrue(banks.Count == TestData.Banks.Count);
        }

        [Test]
        public void SinglePointInsertEmptySeries()
        {

            Series s = new Series();
            s.Add(DateTime.Parse("2005-11-15"), 40.0);
            s.Insert(TestData.Empty);

            Assert.IsTrue(s.Count == 1);
        }

    [Test]
    public void InsertOverlapping()
    {
      Series s = TestData.Simple1Day;
      
      Console.WriteLine("Simple1Day");
      s.WriteToConsole();
      Console.WriteLine("Simple2Day");
      TestData.Simple2Day.WriteToConsole();
      
      s.Insert(TestData.Simple2Day);
      Console.WriteLine(s.Messages.ToString());
        Console.WriteLine("Insert 2Day into 1Day");
      s.WriteToConsole();
      Console.WriteLine(s.Messages);
      Assert.IsTrue(s.Count == TestData.Simple1Day.Count+1);
    }

    [Test]
    public void ModifyRow()
    {
      Series s  = TestData.Simple1Day;
      
      Point pt = s[0];
      pt.Value = 51;
      pt.Flag = "Edited"; // user control of flag
      s[0] = pt;

      Assert.IsTrue(s[0].Flag == "Edited");
      Assert.AreEqual(s[0].Value,51,0.00001);
    }



        [Test]
        public void DeleteFromBeginning()
        {
            Series s = TestData.Banks;

            int count = s.Count;
            Console.WriteLine("original count = " + count);
            s.RemoveAt(0);
            Console.WriteLine("series.Count() = " + s.Count);
            //Console.WriteLine("table.rows.count = "+s.Table.Rows.Count);
            //Console.WriteLine("Table.DefaultView.Count = "+s.Table.DefaultView.Count);
            Assert.IsTrue(s.Count == count - 1, "original count = " + count + " now it is " + s.Count);
            for (int i = 0; i < s.Count; i++)
            {
                double d = s[i].Value;
                Console.Write(d + " ");
            }
        }
        [Test]
        public void Delete()
    {
    Series s = TestData.Banks;
      //s.Table.AcceptChanges();
    
     int count = s.Count;
     Console.WriteLine("original count = "+count);
     s.RemoveAt(4);
    Console.WriteLine("series.Count() = "+s.Count);
      //Console.WriteLine("table.rows.count = "+s.Table.Rows.Count);
    //Console.WriteLine("Table.DefaultView.Count = "+s.Table.DefaultView.Count);
      Assert.IsTrue(s.Count == count -1,"original count = "+count+" now it is "+s.Count);
    }

    [Test]
    public void DeleteSelection()
    {
      Series s = TestData.Simple1Day;
      int count = s.Count;
      DateTime t1 = s[0].DateTime;
      DateTime t2 = s[1].DateTime;//.AddSeconds(2);
       Console.WriteLine("t1 = "+t1.ToString(Series.DateTimeFormatInstantaneous));
       Console.WriteLine("t2 = "+t2.ToString(Series.DateTimeFormatInstantaneous));

      Console.WriteLine("before delete "+count);
      s.WriteToConsole();


      Selection sel  = new Selection(t1,t2,0,5000);
      s.Delete(sel);
      
      Console.WriteLine("after delete "+s.Count);
      s.WriteToConsole();
      Assert.IsTrue(s.Count == count -2);

      for (int i = 0; i < s.Count; i++)
      {
          double d = s[i].Value ;
      }
    }
    /// <summary>
    /// Test selected from real data example
    /// that found a bug.
    /// </summary>
    [Test]
    public void DeleteSelectionInEL68D()
    {
      //2005-02-02 06:53:52.331,98.4335632324219
      //2005-02-04 09:24:53.233,98.4335632324219

      string fn = TestData.DataPath+"\\el68d_export.csv";
      TextSeries s =new TextSeries(fn);
      s.Read();
      Assert.AreEqual(s.Count,1145,"Test Data has been modified....expected 1145 records. found "+s.Count);
      DateTime delete1 = Convert.ToDateTime("2005-02-02 06:53:52.331");
      DateTime delete2 = Convert.ToDateTime("2005-02-04 09:24:53.233");
      Assert.AreEqual(186,s.IndexOf(delete1),"test data has been modified could not find "+delete1.ToShortDateString()+" "+delete1.ToLongTimeString());
      Assert.AreEqual(490,s.IndexOf(delete2),"test data has been modified could not find "+delete2.ToShortDateString()+" "+delete2.ToLongTimeString());
      DateTime t1 = new DateTime(2005,2,2);
      DateTime t2 = new DateTime(2005,2,4,10,30,0,0);
      Selection sel = new Selection(t1,t2,30,200);
      
      s.Delete(sel);// should delete two records in selection.
      Assert.AreEqual(s.Count,1143,"expected 1143 records. found "+s.Count);

      
      Assert.AreEqual(-1,s.IndexOf(delete1),"delete1 point was not deleted ");
      Assert.AreEqual(-1,s.IndexOf(delete2),"delete2 point was not deleted ");
    }

    [Test]
    public void DeepCopy()
    {
      string fn = TestData.DataPath+"\\el68d_export.csv";
      TextSeries s =new TextSeries(fn);
      s.Read();
      s.Name = "First";
      Series s2 = s.Copy();
      s.Name ="Second";
      Console.WriteLine("s.Name = "+s.Name);
      Console.WriteLine("s2.Name = "+s2.Name);
      Assert.IsTrue(s2.Name == "First");
      Assert.IsTrue(s.Name == "Second");



    }


    [Test]
    public void WithingRange()
    {
      Series ts = TestData.SimpleEnding2300;
      DateTime t1 = ts[0].DateTime;
      DateTime t2 = ts[ts.Count-1].DateTime;
      
      Assert.IsFalse(ts.WithinRange(t2.AddMilliseconds(1)));
      Assert.IsTrue(ts.WithinRange(t1));
      Assert.IsTrue(ts.WithinRange(t2));


    }

    [Test]
    public void BoundedBy()
    {
      Series ts = TestData.Simple2Day;
      DateTime t1 = ts[0].DateTime;
      DateTime t2 = ts[ts.Count-2].DateTime;

      Selection sel = new Selection(t1,t2,0,50);
      
      for(int i=0; i<ts.Count; i++)
      {
        if(ts[i].Value >=0 && ts[i].Value <=50
          && ts[i].DateTime >= t1 && ts[i].DateTime <= t2)
        {
          Assert.IsTrue(ts[i].BoundedBy(sel));
        }
        else
        {
          Assert.IsFalse(ts[i].BoundedBy(sel));
        }
      }

    }
  

	}
}
