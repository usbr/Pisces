using System;
using NUnit.Framework;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System.IO;
using Reclamation.TimeSeries.Usgs;
using Reclamation.TimeSeries.Hydromet;

namespace Pisces.NunitTests.SeriesMath
{
    /// <summary>
    /// Example of programming with Pisces.
    /// 
    /// This example creates a Pisces database with a folder for each 
    /// month.  In the first two folders a series is created.
    /// </summary>
    [TestFixture]
    class TestPiscesProgrammingEasy
    {


        /// <summary>
        /// This example uses Series object and Database object to
        /// get the same results as the previous example.  However,
        /// this example also loads timeseries data.
        /// </summary>
        [Test]
        public void AddSeriesDirectly()
        {
            Logger.EnableLogger();

            var filename = FileUtility.GetTempFileNameInDirectory(@"c:\temp\", ".pdb");
            if (File.Exists(filename))
                File.Delete(filename);

            Console.WriteLine(filename);
            var server = new SQLiteServer(filename);
            var db = new TimeSeriesDatabase(server,false);

            // create a folder for each month
            for (int i = 1; i <= 12; i++)
            {
                var t = new DateTime(2015, i, 1);
                db.AddFolder("Months", t.ToString("MMMM"));
            }

            // Add USGS series (Boise River) to the January Folder
            Series s = new UsgsDailyValueSeries("13206000", UsgsDailyParameter.DailyMeanDischarge);
            s.SiteID = "13206000";
            var folder = db.GetOrCreateFolder("Months","January");
            s.Read(DateTime.Parse("2015-01-01"), DateTime.Parse("2015-01-10"));
            db.AddSeries(s, folder);
            // Add Hydromet series to the February Folder
            s = new HydrometDailySeries("bhr", "af", HydrometHost.GreatPlains);
            s.Name = "gphyd_bhr_af";
            s.SiteID = "gphyd_bhr";
            var feb = db.GetOrCreateFolder("Months", "February");
            db.AddSeries(s, feb);

            var csvFileName = FileUtility.GetTempFileName(".csv");

            File.WriteAllLines(csvFileName, new string[]
                {
                    "Date,value",
                    "1-1-2001, 12.34"
                });
            // Add Csv file data to March Folder.
            s = new TextSeries(csvFileName);
            s.Read();// read data.  Use Read(t1,t2) to limit by dates
            s.SiteID = "test";
            s.Units = "cfs";
            s.Table.TableName = "test_river"; // table name needs to be unique
            db.AddSeries(s, db.GetOrCreateFolder("Months", "March"));

            s = db.GetSeriesFromName("gphyd_bhr_af");
            s.Read();
            Console.WriteLine(s.Count);

            // Add CSV file 


            db.Inventory();
        }

    }
}
