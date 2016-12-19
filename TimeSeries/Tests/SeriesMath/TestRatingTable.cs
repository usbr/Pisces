using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;
using System.IO;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.Core;
using Reclamation.TimeSeries.Owrd;
using System.Configuration;

namespace Pisces.NunitTests.SeriesMath
{
    [TestFixture]
   public class TestRatingTable
    {
        [Test]
        public void ReservoirContentsWithDatabase()
        {
            Logger.EnableLogger();
            var fn = FileUtility.GetTempFileName(".pdb");
            System.IO.File.Delete(fn);

            SQLiteServer svr = new SQLiteServer(fn);
            var db = new TimeSeriesDatabase(svr, Reclamation.TimeSeries.Parser.LookupOption.TableName,false);

            var c = new CalculationSeries("instant_karl_af");
            var path = Path.Combine(Globals.TestDataPath, "rating_tables");
            path = Path.Combine(path, "karl_af.txt");

            c.Expression = "FileRatingTable(instant_karl_fb,\""+path+"\")";
            c.TimeInterval = TimeInterval.Irregular;
            db.AddSeries(c);

            var fb = new Series("instant_karl_fb");
            fb.TimeInterval = TimeInterval.Irregular;
            db.AddSeries(fb);
            fb.Add("1-1-2013", 1);
            fb.Add("1-2-2013", 2);
            fb.Add("1-3-2013", 3);
            fb.Add("1-4-2013", 4);
            TimeSeriesImporter ti = new TimeSeriesImporter(db);
            ti.Import(fb,computeDependencies:true);// this should force a calculation...

            var af = db.GetSeriesFromTableName("instant_karl_af");
            Assert.NotNull(af, "Series not created");
           
            af.Read();

            Assert.AreEqual(4, af.Count);
            Assert.AreEqual(300, af[2].Value);

        }



        [Test]
        public void OwrdRatingTable()
        {
            OwrdRatingTables rt = new OwrdRatingTables("14030000");
            
            //TimeSeriesDatabaseDataSet.RatingTableDataTable.
           // var x = rt.Lookup(1281.95);
           // Assert.AreEqual(4543763, x, .01);
            //var s = new HydrometInstantSeries("gcl", "fb");

            //TestRatingTable 
        }

       public void HydrometRatingTable()
       {

           //var rt = new TimeSeriesDatabaseDataSet.RatingTableDataTable();
          // rt.ReadFromFile(Path.Combine(TestData.DataPath,"rating");
           var rt = HydrometInfoUtility.GetRatingTable("gcl", "af","");
           //TimeSeriesDatabaseDataSet.RatingTableDataTable.
           var x = rt.Lookup(1281.95);
           Assert.AreEqual(4543763, x, .01);
           //var s = new HydrometInstantSeries("gcl", "fb");

           //TestRatingTable 
       }

       [Test]
       public void FileRatingTable()
       {

           Series s = new Series();
           s.Add(DateTime.Now.Date, 1281.95);

           var path = Path.Combine(Globals.TestDataPath, "rating_tables", "gcl_af.txt");
           var af = TimeSeriesDatabaseDataSet.RatingTableDataTable.ComputeSeries(s, path);


           var x = af[0].Value;
           Assert.AreEqual(4543763, x, .01);
       }

    }
}
