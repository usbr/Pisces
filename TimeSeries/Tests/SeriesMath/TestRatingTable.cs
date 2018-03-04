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
        private string path;

        public TestRatingTable()
        {
            if (LinuxUtility.IsLinux())
            {
                path = "/var/tmp/PiscesTestData";
            } else
            {
                path = Globals.TestDataPath;
            }
        }

        [Test]
        public void ReservoirContentsWithDatabase()
        {
            Logger.EnableLogger();
            var fn = FileUtility.GetTempFileName(".pdb");
            System.IO.File.Delete(fn);

            SQLiteServer svr = new SQLiteServer(fn);
            var db = new TimeSeriesDatabase(svr, Reclamation.TimeSeries.Parser.LookupOption.TableName,false);

            var c = new CalculationSeries("instant_karl_af");
            var path = Path.Combine(this.path, "rating_tables");
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

           var path = Path.Combine(this.path, "rating_tables", "gcl_af.txt");
           var af = TimeSeriesDatabaseDataSet.RatingTableDataTable.ComputeSeries(s, path);


           var x = af[0].Value;
           Assert.AreEqual(4543763, x, .01);
        }


      [Test]
       public void FileRatingTableLogInterpolateETCW()
       {
           Series s = new Series();
           DateTime t = DateTime.Now.Date;
           var gh = new double[] { 1.63, 1.65,1.53,1.5,1.92, -.2, 10};
           var q1 = new double[] { 108.34,	110.25,	98.93,	95.08,	137.0, 0, double.NaN};
           for (int i = 0; i < gh.Length; i++)
           {
               s.Add(t, gh[i] -0.02);
               t = t.AddHours(1);
           }

           var path = Path.Combine(this.path, "rating_tables", "etcw_qc.txt");
           var q = TimeSeriesDatabaseDataSet.RatingTableDataTable.ComputeSeries(s, path, InterpolationMethod.LogLog);

           Check(gh, q1, q);
       }

       private static void Check(double[] gh, double[] q1, Series q)
       {
           Assert.AreEqual(gh.Length, q.Count);
           for (int i = 0; i < q.Count; i++)
           {

               if (double.IsNaN(q1[i]))
               {
                   Assert.IsTrue(q[i].IsMissing == true);
               }
               else
               {
                   Assert.AreEqual(q1[i], q[i].Value, 0.01);
               }
           }

           q.WriteToConsole();
       }


        /// <summary>
        /// DAYFILES > g bicw
        ///1 BICW JUN 19 08:30 # GH             69.59 # Q              55.65
        ///                   # HJ              0.08
        ///   FileRatingTable(smci_ch+LookupShift(smci_ch) ,"smci.csv")
        /// </summary>
        [Test]
        public void FileRatingTableInterpolateBICW()
        {
            Series s = new Series();
            DateTime t = new DateTime(2017, 5, 3);
            var gh = new double[] { 69.61, 69.59, 1 };
            var q1 = new double[] {57.55, 55.65, double.NaN };
            for (int i = 0; i < gh.Length; i++)
            {
                s.Add(t, gh[i]+0.08);
                t = t.AddHours(1);
            }

            var path = Path.Combine(this.path, "rating_tables", "bicw_q.txt");
            var q = TimeSeriesDatabaseDataSet.RatingTableDataTable.ComputeSeries(s, path, InterpolationMethod.Linear);

            Check(gh, q1, q);
        }

        [Test]
       public void FileRatingTableInterpolate()
       {
            Series s = new Series();
           DateTime t = new DateTime(2017,5,3);
           var ch = new double[]{1.5, 1.55,1.6 ,1.65,4  ,5.6,5.9,10};
           var qc = new double[]{double.NaN, double.NaN , 10.0,10.5,87.0,213.0,double.NaN,double.NaN};
           for (int i = 0; i < ch.Length; i++)
			{
			 s.Add(t,ch[i]);
             t= t.AddHours(1);
			}

           var path = Path.Combine(this.path, "rating_tables", "lvno.csv");
           var q = TimeSeriesDatabaseDataSet.RatingTableDataTable.ComputeSeries(s, path, InterpolationMethod.Linear);

           Check(ch, qc, q);
       }



    }
}
