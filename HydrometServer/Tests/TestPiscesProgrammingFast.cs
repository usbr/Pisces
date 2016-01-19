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
    class TestPiscesProgrammingFast
    {

        /// <summary>
        /// This example creates series entries, but does not include series data
        /// This method is fast because the series catalog is managed directly
        /// and only saved once.  However, no timeseries data is stored.
        /// you must right click update to download the data.
        ///  see the follwing example AddSeriesDirectly for a 'eaiser'
        ///  but slower way of managing the database with timeseries data included.
        /// </summary>
        [Test]
        public void ManageSeriesCatalogDirectly()
        {
            Logger.EnableLogger();

            var filename = @"c:\temp\test.pdb";
            if (File.Exists(filename))
                File.Delete(filename);

            Console.WriteLine(filename);
            var server = new SQLiteServer(filename);
            var db = new TimeSeriesDatabase(server);

            var seriesCatalog = db.GetSeriesCatalog();
            var siteCatalog = db.GetSiteCatalog();

            // create a folder for each month
            for (int i = 1; i <= 12; i++)
            {
                var t = new DateTime(2015, i, 1);
                seriesCatalog.GetOrCreateFolder("Months", t.ToString("MMMM"));
            }

            // Add USGS series (Boise River) to the January Folder
            Series s = new UsgsDailyValueSeries("13206000", UsgsDailyParameter.DailyMeanDischarge);
            s.SiteID = "usgs_13206000";
            var januaryIndex = seriesCatalog.GetOrCreateFolder("Months", "January");
            seriesCatalog.AddSeriesCatalogRow(s, seriesCatalog.NextID(), januaryIndex,"usgs_boiseriver_flow");

            // Add Hydromet series to the February Folder
            s = new HydrometDailySeries("bhr", "af", HydrometHost.GreatPlains);
            s.Name = "gphyd_bhr_af";
            s.SiteID = "gphyd_bhr";
            var feb = seriesCatalog.GetOrCreateFolder("Months", "February");
            seriesCatalog.AddSeriesCatalogRow(s, seriesCatalog.NextID(), feb, "usgs_boiseriver_flow");
            
           
            // Add Site information

            siteCatalog.AddsitecatalogRow("usgs_13206000", "BOISE RIVER AT GLENWOOD BRIDGE NR BOISE ID", "ID");
            siteCatalog.AddsitecatalogRow("gphyd_bhr", "Big Horn Reservoir","MT");

            server.SaveTable(seriesCatalog);


            s = db.GetSeriesFromName("gphyd_bhr_af");
            s.Read();
            Console.WriteLine(s.Count);

            // Add CSV file 


            db.Inventory();
        }


    }
}
