using System;
using System.Linq;
using NUnit.Framework;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System.IO;
using HydrometServer;

namespace Pisces.NunitTests.SeriesMath
{
    [TestFixture]
    class TestMidnightCalculation
    {
        static void Main(string[] argList)
        {
            var x = new TestMidnightCalculation();
            x.wrdo();
        }


        TimeSeriesDatabase db;
        public TestMidnightCalculation()
        {
            string fn = FileUtility.GetTempFileNameInDirectory(@"c:\temp",".pdb","midnight");
            Console.WriteLine(fn);
            var svr = new SQLiteServer(fn);
            db = new TimeSeriesDatabase(svr, Reclamation.TimeSeries.Parser.LookupOption.TableName);
        }


        /// <summary>
        /// This test imports data over 3 days, but only includes a single full day
        /// of data.
        ///  10/6/2014 --- partial day, begins 12:15 
        ///  10/7/2014 -- full day 
        ///  10/8/2014 -- partial day  ends 12:00
        /// </summary>
        [Test]
        public void wrdo()
        {
            //Logger.EnableLogger();
            FileUtility.CleanTempPath();
            var dir = FileUtility.GetTempPath();
            var testDir = Path.Combine(Globals.TestDataPath, "wrdo");
            FileUtility.CopyFiles(testDir, dir,"*.*",true,true);


            var c = new Series("daily_wrdo_mm"); //needed to 
            //c.Expression = "DailyAverage(instant_wrdo_obm,92)";
            c.TimeInterval = TimeInterval.Daily;
            c.Add("10-1-2014",43.27); 
            c.Add("10-2-2014",48.53);
            c.Add("10-3-2014",52.33);
            c.Add("10-4-2014",54.63);
            c.Add("10-5-2014",53.97);
            c.Add("10-6-2014",55.22);
            c.Add("10-7-2014", 55.12);

            db.AddSeries(c);

            c = new CalculationSeries("daily_wrdo_mn");
            c.Expression = "DailyMin(instant_wrdo_obn)";
            c.TimeInterval = TimeInterval.Daily;
            db.AddSeries(c);

            c = new CalculationSeries("daily_wrdo_pc");
            c.Expression = "DailyMidnight(instant_wrdo_pc)";
            c.TimeInterval = TimeInterval.Daily;
            c.Add("10-5-2014", 8);
            db.AddSeries(c);


            c = new CalculationSeries("daily_wrdo_pp");
            c.Expression = "daily_wrdo_pc-daily_wrdo_pc[t-1]";
            c.TimeInterval = TimeInterval.Daily;
            c.Add("10-5-2014", 0);
            db.AddSeries(c);

            c = new CalculationSeries("daily_wrdo_pu");
            c.TimeSeriesDatabase = db;
            c.Expression = "DailyWaterYearRunningTotal(daily_wrdo_pp,daily_wrdo_pu)";
            c.TimeInterval = TimeInterval.Daily;

            c.Add("10-1-2014", 0);
            c.Add("10-2-2014", 0);
            c.Add("10-3-2014", 0);
            c.Add("10-4-2014", 0);
            c.Add("10-5-2014", 0);
            c.Add("10-6-2014", 0);
            db.AddSeries(c);
            c = db.GetSeriesFromTableName("daily_wrdo_pu");

            c.Properties.Set("DaysBack", "7");
            c.Properties.Save();
            c = new CalculationSeries("daily_wrdo_mx");
            c.Expression = "DailyMax(instant_wrdo_obx)";
            c.TimeInterval = TimeInterval.Daily;
            db.AddSeries(c);

            c = new CalculationSeries("daily_wrdo_ym");
            c.Expression = "DailyAverage(instant_wrdo_tp,92)";
            c.TimeInterval = TimeInterval.Daily;
            db.AddSeries(c);

            c = new CalculationSeries("daily_wrdo_wr");
            c.Expression = "DailySum(instant_wrdo_ws,92)/4.0";
            c.TimeInterval = TimeInterval.Daily;
            db.AddSeries(c);

            c = new CalculationSeries("daily_wrdo_sr");
            c.Expression = "DailySum(instant_wrdo_si,92)/4.0";
            c.TimeInterval = TimeInterval.Daily;
            db.AddSeries(c);


            c = new CalculationSeries("daily_wrdo_et");
            c.SiteID = "wrdo"; // needed fore expression pre-processor with  %site% 
            c.Expression = "DailyEtKimberlyPenman(daily_%site%_mm,daily_%site%_mn,daily_%site%_mx,daily_%site%_ym,daily_%site%_wr,daily_%site%_sr,42.0125,1243.58)";
            c.TimeInterval = TimeInterval.Daily;
            db.AddSeries(c);





           //Reclamation.TimeSeries.Parser.SeriesExpressionParser.Debug = true;
            FileImporter fi = new FileImporter(db);
            fi.Import(dir, computeDailyOnMidnight: true);


            var s = db.GetSeriesFromTableName("daily_wrdo_ym");
            s.Read();
            Assert.IsTrue(s.Count > 0, " DailyAverage(instant_wrdo_tp,92) failed!");

            s = db.GetSeriesFromTableName("daily_wrdo_et");
            s.Read();
            Assert.AreEqual(s["10-7-2014"].Value,0.16,0.01,"Error with ET");


            s = db.GetSeriesFromTableName("daily_wrdo_pu");
            s.Read();
            Assert.AreEqual(s["10-7-2014"].Value, 0, 0.02, "Error with pu");


        }

       

    }
}
