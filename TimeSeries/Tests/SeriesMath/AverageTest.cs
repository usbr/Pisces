using NUnit.Framework;
using Reclamation.TimeSeries;
using System;
using System.IO;
using Math = Reclamation.TimeSeries.Math;

namespace Pisces.NunitTests.SeriesMath
{
    /// <summary>
    /// Summary description for DailyAverageTest.
    /// </summary>
    [TestFixture]
	public class AverageTest
	{
    public static void RunTests()
    {
      AverageTest d = new AverageTest();
      d.AverageForDay();
	    d.SingleDayAverageBanks();
	    d.MultipleDayAverageBanks();
    }

    [Test]
    public void AverageWithFlags()
    {

        Series s = new Series();

        s.Add(DateTime.Parse("1/1/2013 12:30"), -55,"-");
        s.Add(DateTime.Parse("1/1/2013 13:00"), 1);
        s.Add(DateTime.Parse("1/1/2013 13:15"), 1);
        s.Add(DateTime.Parse("1/1/2013 14:01"), 500,"+");
        var avg = Math.DailyAverage(s,2);
        avg.WriteToConsole();
        Assert.AreEqual(1, avg.Count);
        Assert.AreEqual(1, avg[0].Value,0.01);

    }

    [Test]
    public void HourlyAverage()
    {

        Series s = new Series();

        s.Add("1/1/2013 12:30", 55);
        s.Add("1/1/2013 13:00", 1);
        s.Add("1/1/2013 13:15", 1);
        s.Add("1/1/2013 14:01", 500);
        var avg = Math.Average(s, TimeInterval.Hourly);
        avg.WriteToConsole();

        Assert.AreEqual(55, avg["1/1/2013 12:00:00"].Value);
        Assert.AreEqual(1, avg["1/1/2013 13:00:00"].Value);
        Assert.AreEqual(500, avg["1/1/2013 14:00:00"].Value);

    }
    [Test]
		public void SingleDayAverageBanks()
		{
			Console.WriteLine("SingleDayAverageBanks Test:  banks lake average water level may 17, 2005 (raw scada data)");
			Series input = TestData.Banks;
			DateTime date = new DateTime(2005,5,17);
			Point point = Math.TimeWeightedAverageForDay(input,date);
			Console.WriteLine("Average from raw scada = "+point.Value);
      Assert.AreEqual(1568.5,point.Value,0.1);
		}

    [Test]
		public  void MultipleDayAverageBanks()
		{
			Console.WriteLine("MultipleDayAverageBanks Test");
			Series input = TestData.Banks;
			
			Series ts = Math.TimeWeightedDailyAverage(input);
			/*
			 type of series is PeriodAver
units:feet
5/17/2005, 1568.52804869882
5/18/2005, 1568.38618500667
5/19/2005, 1568.12503620462
5/20/2005, 1567.96182203232
5/21/2005, 1568.07234682992
5/22/2005, 1568.56578340813
5/23/2005, 1568.78610831966
5/24/2005, 1568.68114810308
			 * */
			double[] goodValues = {1568.52804869882,
									  1568.38618500667,
									  1568.12503620462,
									  1567.96182203232,
									  1568.07234682992,
									  1568.56578340813,
									  1568.78610831966,
									  1568.68114810308};
			DateTime date = new DateTime(2005,5,17);
			for(int i=0; i<ts.Count; i++)
			{
				double val = ts.Lookup(date);
        Assert.AreEqual(goodValues[i],val,0.1);
				date = date.AddDays(1);
			}
		}

    [Test]
    public void SimpleAverageForDay()
    {
      Series input = TestData.Simple1Day;
      input.WriteToConsole();
      Series avg = Math.DailyAverage(input);
      avg.WriteToConsole();
      Assert.AreEqual(1, avg.Count);
      Assert.AreEqual(52.5,avg[0].Value,0.0001);
        
    }

    [Test]
    public void SimpleAverageForDayOnly12Points()
    {
        Series input = new Series();

        DateTime t = DateTime.Now.Date.AddMinutes(15);
        for (int i = 0; i < 12; i++)
        {
            input.Add(t, i * 10);

            t = t.AddMinutes(2);
        }
        input.WriteToConsole();
        Series avg = Math.DailyAverage(input,96);
        avg.WriteToConsole();
        Assert.AreEqual(1, avg.Count);
        Assert.IsTrue(avg[0].IsMissing);

    }

    [Test]
    public void AverageForDay()
    {
      Series input = TestData.Simple1Day;
      input.WriteToConsole();
      Point pt = Math.TimeWeightedAverageForDay(input,input[0].DateTime);

      Assert.AreEqual(72.291667,pt.Value,0.0001);

//--call cbp.Soi_DailyAverage('Records','value','2000-01-01','2025-01-01',0,null,0,0)
//-- result = 72.291667
    }


    [Test]
    public void SevenDay()
    {
        string fn = Path.Combine(TestData.DataPath, "sevendayavg.csv");
            //        var s = new ExcelDataReaderSeries(fn, "Sheet1", "A", "B");
            var s = new TextSeries(fn);
            s.Read();
        Assert.AreEqual(2738, s.Count);
        var s7 = Math.WeeklyAverageSimple(s);

        s7.WriteToConsole();
        Assert.AreEqual(DateTime.Parse("2004-02-12 23:59:59.9"), s7[0].DateTime);
        Assert.AreEqual(2.17, s7[0].Value, 0.01);
        Assert.AreEqual(101.32, s7[1].Value, 0.01);
    }



    [Test]
    public void WeeklyAverageFromInstant()
    {
        string fn = Path.Combine(TestData.DataPath, "El686_2004InstantaniousStage.csv");
        TextSeries s = new TextSeries(fn);
        Weekly(s);
    }
     

    [Test]
    public void WeeklyAverageFromDaily()
    {
        string fn = Path.Combine(TestData.DataPath, "El686_2004DailyAverageStage.csv");
        TextSeries s = new TextSeries(fn);

        Weekly(s);
        
    }

    private static void Weekly(TextSeries s)
    {

        s.Read(DateTime.Parse("2/6/2004"), DateTime.Parse("12/31/2004"));

        double mf = Point.MissingValueFlag;
        Point.MissingValueFlag = -9999;
        s.RemoveMissing();
        Point.MissingValueFlag = mf;

        s.TimeInterval = TimeInterval.Daily;
        var weekly = Math.WeeklyAverageSimple(s);
        Assert.AreEqual(TimeInterval.Weekly, weekly.TimeInterval);

        Assert.AreEqual(DateTime.Parse("2/12/2004"), weekly[0].DateTime.Date);
        Assert.AreEqual(2.172, weekly[0].Value, 0.01);
    }


    


    [Test]
    public void PartialDay()
    {
        string fn = Path.Combine(TestData.DataPath, "wilson.csv");
            //        var s = new ExcelDataReaderSeries(fn, "wilson", "A", "B");
            var s = new TextSeries(fn);
        s.Read();

        Series avg = Math.TimeWeightedDailyAverage(s);
    }


    /// <summary>
    /// Test that extrapolation will not 
    /// happen without permission.
    /// </summary>
    [Test]
    public void Site68OnefullDay()
    {
      Series s = TestData.Site68OneFullDayInstantaneous;

      Series avg = Math.TimeWeightedDailyAverage(s);
      avg.WriteToConsole();
      Assert.AreEqual(1,avg.Count); // should have 1 daily average

      
    }
    
    /// <summary>
    /// daily average from database should match 
    /// daily average in Calculator class
    /// </summary>
    [Test]
    public void LindCoulee2004()
    {
     
     Series s = TestData.LindCouleeWW1InstantanousStage2004;
     //Point pt =  Math.Calculator.AverageForDay(s,DateTime.Parse("2004-12-20"));
     Series avg = Math.TimeWeightedDailyAverage(s);
     // Console.WriteLine("avg");
      //avg.WriteToConsole();
      Console.WriteLine(avg[0].DateTime.ToString("yyyy-MM-dd HH:mm:ss.ffff"));

    Console.WriteLine("Math.Calculator.DailyAverage(s).Count = "+avg.Count);

    Series dbAverage = TestData.LindCouleeWW1DailyAverageStage2004;
    Console.WriteLine("TestData.LindCouleeWW1DailyAverageStage2004.Count = "+dbAverage.Count);


      Series diff = avg - dbAverage;
      SeriesList list = new SeriesList();
      list.Add(avg);
      list.Add(dbAverage);
      list.Add(diff);
      list.WriteToConsole();

      Console.WriteLine("summing difference");
      double d = Math.Sum(diff);
      Assert.AreEqual(0,d,0.1); // actual is about 0.05
      Console.WriteLine("sum of differences = "+d);
      Console.WriteLine("sum of daily "+Math.Sum(avg));
      Assert.AreEqual(dbAverage.Count-1,avg.Count);
      for(int i=0;i<avg.Count; i++)
      {
       // database has one (missing) value at beginning we skip that in comparison
        Assert.AreEqual(dbAverage[i+1].ToString(),avg[i].ToString());
        Assert.AreEqual(dbAverage[i+1].Value,avg[i].Value,0.0001);
        Assert.AreEqual(dbAverage[i+1].DateTime.Ticks , avg[i].DateTime.Ticks,"on line "+i);
      }

    }

    [Test]
    public void DailyAverageTicksBug()
    {
      Series s = TestData.LindCouleeWW1InstantanousStage2004;
      DateTime date = DateTime.Parse("2004-01-02");
      Point pt = Math.TimeWeightedAverageForDay(s,date);
      Console.WriteLine(pt);
      Assert.AreEqual(0,pt.DateTime.Millisecond,"error #1");
    }


	}
}
