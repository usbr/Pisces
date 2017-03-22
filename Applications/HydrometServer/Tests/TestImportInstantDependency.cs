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
    class TestImportInstantDependency
    {

        static void Main(string[] argList)
        {
            var x = new TestImportInstantDependency();
            x.aga();
        }


        TimeSeriesDatabase db;
        public TestImportInstantDependency()
        {
           // FileUtility.CleanTempPath();
            string fn = FileUtility.GetTempFileName(".pdbaga");
            Console.WriteLine(fn);
            var svr = new SQLiteServer(fn);
            db = new TimeSeriesDatabase(svr, Reclamation.TimeSeries.Parser.LookupOption.TableName,false);
        }


        public void AddEquationSeries(string name, string equation, string interval="instant")
        {
            var f =db.GetOrCreateFolder("hydromet","aga",interval);
            TimeSeriesName tn = new TimeSeriesName(name, interval);

            var c = new CalculationSeries(name);
            c.Table.TableName = tn.GetTableName();
            c.Expression = equation;
            c.TimeInterval = tn.GetTimeInterval();
            db.AddSeries(c,f);
        }

        /// <summary>
        /// Imports data that requires second level dependency calculation
        /// aga_fb = (raw data)
        /// aga_af = FileRatingTable(aga_fb,"aga_af.csv")
        /// aga_qr = FileRatingTable(aga_gh,"aga_qr.csv")
        /// aga_qs = FileRatingTable(aga_fb,"aga_qs.csv")
        /// aga_q = aga_qs + aga_qr
        /// </summary>
        [Test, Category("Internal")]
        public void aga()
        {
            Logger.EnableLogger();

            var decodes = Path.Combine(Globals.TestDataPath, "decodes");
            
            
            var dir = FileUtility.GetTempPath();

            File.Copy(Path.Combine(decodes,"instant_aga.txt"),
                Path.Combine(dir, "instant_aga.txt"));

            File.Copy(Path.Combine(decodes, "instant_aga_midnight.txt"),
                Path.Combine(dir, "instant_aga_midnight.txt"));

            var rt = Path.Combine(Globals.TestDataPath, "rating_tables");

            AddEquationSeries("aga_q", "aga_qs + aga_qr");
            AddEquationSeries("aga_af", "FileRatingTable(aga_fb, \""+Path.Combine(rt,"aga_af.csv")+"\")");
            AddEquationSeries("aga_qr", "FileRatingTable(aga_gh, \""+Path.Combine(rt,"aga_qr.csv")+"\")");
            AddEquationSeries("aga_qs", "FileRatingTable(aga_fb, \""+Path.Combine(rt,"aga_qs.csv")+"\")");
            

            AddEquationSeries("aga_af", "DailyMidnight(instant_aga_af)", "daily");

            FileImporter fi = new FileImporter(db);
            fi.Import(dir,RouteOptions.None,true,true,"*.txt");


            var s = db.GetSeriesFromTableName("instant_aga_q");
            s.Read();
            s.WriteToConsole();
            Assert.IsTrue(s.Count > 0, s.Name);

            s = db.GetSeriesFromTableName("daily_aga_af");
            s.Read();
            s.WriteToConsole();


        }

       

    }
}
