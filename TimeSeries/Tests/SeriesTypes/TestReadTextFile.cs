using System;
using System.IO;
using NUnit.Framework;
using Reclamation.TimeSeries;
namespace Pisces.NunitTests.SeriesTypes
{
	/// <summary>
	/// Summary description for ReadTextFile.
	/// </summary>
	[TestFixture]
	public class ReadTextFile
	{
		public ReadTextFile()
		{
		}


        [Test]
        public void DailyIntervalDetection()
        {
            string fn = TestData.DataPath + @"\El686_2004DailyAverageStage.csv";
            TextSeries s = new TextSeries(fn);
            s.Read();
            Assert.AreEqual(TimeInterval.Daily, s.TimeInterval);

        }
            

        /// <summary>
        /// Example                      yymmdd   hhmm     value
        /// PE16.4 157653-96F   USLEV    070101   0000     00008.20 
        /// PE16.4 157653-96F   USLEV    070101   0010     00008.20 
        /// </summary>
        [Test]
        public void StevensLogger()
        {
            string fn = TestData.DataPath + "\\StevensLogger.txt";
            TextSeries s = new TextSeries(fn);
            s.Read();

            Assert.AreEqual(13, s.Count);

            Assert.AreEqual(7.93, s["2007-12-31 22:40"].Value);
            Assert.AreEqual(1.51, s["2007-8-27 08:40"].Value);

            Assert.AreEqual(TimeInterval.Irregular, s.TimeInterval);

        }


        [Test]
        public void BelowDeadwoodDam()
        {
            string path = TestData.DataPath + "\\";
            string fn1 = path + "below Deadwood Dam.csv";
            Console.WriteLine("reading " + fn1);
            TextSeries s = new TextSeries(fn1);
            s.Read();
            Console.WriteLine("skipped the following\n" + s.Messages.ToString());
            //s.WriteToConsole();
            Assert.AreEqual(32000, s.Count);

        }

        [Test]
        public void mmDDyy()
        {
            string path = TestData.DataPath + "\\";
            string fn1 = path + "mmddyy.csv";
            Console.WriteLine("reading " + fn1);
            TextSeries s = new TextSeries(fn1);
            s.Read();
            Console.WriteLine("skipped the following\n" + s.Messages.ToString());
            s.WriteToConsole();
            Assert.AreEqual(5, s.Count);

        }

        [Test]
        public void ExcelCSV()
        {
            string path = TestData.DataPath + "\\";
            string fn1 = path + "mmddyyyyhhmmAMPM.txt";
            Console.WriteLine("reading " + fn1);
            TextSeries s = new TextSeries(fn1);
            s.Read();
            Console.WriteLine("skipped the following\n" + s.Messages.ToString());

            Assert.AreEqual(9,s.Count);
        }

    [Test]
    public void TextFileHydromet()
    {
      
      string path = TestData.DataPath+"\\";
      string fn1 = path + "LuckyPeakWaterLevel.txt";
      Console.WriteLine("reading "+ fn1);
      TextSeries s = new TextSeries(fn1);
      s.Read();
      Assert.IsTrue(s.Count >0);
      DateTime t1 = Convert.ToDateTime("10/13/2004");
      double v = s.Lookup(t1);
      Assert.IsTrue(System.Math.Abs(v-2907.2) <0.01 ,"expected 2907.2. got "+v);

      // save to text file..
      string fn = Path.GetTempFileName();
      //fn = TestData.OutputPath+"\\"+fn;
      s.WriteCsv(fn);

      TextSeries s1 = new TextSeries(fn);
      s1.Read();
      Assert.IsTrue(s.Count == s1.Count);
      v = s1.Lookup(t1);
      Assert.IsTrue(System.Math.Abs(v-2907.2) <0.01 ,"expected 2907.2. got "+v);

      File.Delete(fn);
      Console.WriteLine("finished TextFileHydromet");
    }


    [Test]
    public void TextFileDigitizedChart()
    {
      string fn = TestData.DataPath +"\\el68d_DigitizedChart.txt";
      TextSeries s = new TextSeries(fn);
      s.Read();
      //s.Save(TestData.DataPath +"\\el68d_DigitizedChart2.txt");
      //1999/01/02 12:40:11,   4.969
      DateTime t1 = Convert.ToDateTime("1999/01/02 12:40:11");
      Assert.IsTrue(System.Math.Abs(s.Lookup(t1) - 4.969) <0.001);

    }

    /// <summary>
    /// 1999/01/01 12:45:19,   5.056
    /// 1999/01/20 13:00:14,   5.061
    /// # with fractional seconds
    /// 1999/01/20 13:00:14.001,   5.062
    /// </summary>
    [Test]
    public void csv_yyyymmdd()
    {// comma separated
      string fn = TestData.DataPath +"\\csv_yyyymmdd.txt";
      yyyymmdd(fn);
    }

    [Test]
    public void space_yyyymmdd()
    {// space separated
      string fn = TestData.DataPath +"\\space_yyyymmdd.txt";
      yyyymmdd(fn);
    }

    private void yyyymmdd(string filename)
    {
      DateTime[] dates = {
                           new DateTime(1999,1, 1,12,45,19,0),
                           new DateTime(1999,1,20,13,0,14,0),
                           new DateTime(1999,1,20,13,0,14,1),
      };
      double[] values = {5.056,5.061,5.062};
    
      TextSeries s = new TextSeries(filename);
      s.Read();
      s.WriteToConsole();
      Assert.IsTrue(s.Count == dates.Length,"should have 3 dates we have "+s.Count);
      for(int i=0;i<dates.Length; i++)
      {
        Assert.IsTrue(s[i].DateTime == dates[i]);
        Assert.IsTrue(System.Math.Abs(s[i].Value- values[i])<0.000001);
      }

    }
    /// <summary>
    ///2/1/2005 13:02,3
    ///02/01/2005 13:02:4,5
    /// 3/1/2005 13:03,4.1
    ///4/1/2005 2:00:01,5
    /// </summary>
    [Test]
    public void Csv_mmddyyyy()
    {// comma separated
      string fn = TestData.DataPath +"\\csv_mmddyyyy.txt";
      mmddyyyy(fn);
    }
    [Test]
    public void space_mmddyyyy()
    {// space separated.
      string fn = TestData.DataPath +"\\space_mmddyyyy.txt";
      mmddyyyy(fn);
    }
   

    private void mmddyyyy(string filename)
    {
      DateTime[] dates = {
                           new DateTime(2005,2,1,13,2,0,0),
                           new DateTime(2005,2,1,13,2,4,0),
                           new DateTime(2005,3,1,13,3,0,0),
                           new DateTime(2005,4,1,2,0,1,0)
                         };
      double[] values = {3,5,4.1,5};

      TextSeries s = new TextSeries(filename);
      s.Read();
      s.WriteToConsole();
      Assert.IsTrue(s.Count == dates.Length,"should have 4 dates we have "+s.Count);
      for(int i=0;i<dates.Length; i++)
      {
        Assert.IsTrue(s[i].DateTime == dates[i]);
        Assert.IsTrue(System.Math.Abs(s[i].Value- values[i])<0.000001);
      }
    }




	}
}
