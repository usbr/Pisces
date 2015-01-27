using System;
using System.Linq;
using NUnit.Framework;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System.IO;
using HydrometServer;
using Reclamation.TimeSeries.Parser;

namespace Pisces.NunitTests.SeriesMath
{
    /// <summary>
    /// Test that flows are calculated when gage heights are imported.
    /// Test that low, high and rate of change flags are set on both
    /// gage height and dependent flow calculations.
    /// </summary>
    [TestFixture]
    class TestRatingTableDependency
    {

        static void Main(string[] argList)
        {
            var x = new TestRatingTableDependency();
            x.ImportDecodesFiles();
        }
        public TestRatingTableDependency()
        {
        }

        [Test]
        public void ImportDecodesFiles()
        {
            var fn1 = FileUtility.GetTempFileNameInDirectory(@"c:\temp", ".pdb", "ratings");
            Console.WriteLine(fn1);
            var svr = new SQLiteServer(fn1);
            var db = new TimeSeriesDatabase(svr, Reclamation.TimeSeries.Parser.LookupOption.TableName);
         
            Logger.EnableLogger();
            FileUtility.CleanTempPath();
            var tmpDir = CopyTestDecodesFileToTempDirectory("decodes_dms3.txt");
           var lapofn = CopyTestDecodesFileToTempDirectory("decodes_lapo.txt");

            
           var rtlapo = CreateTempRatingTable("lapo.csv", new double[] {3.50,3.54,5.54 },
                                             new double[] {1,2,10 });
            // set limits  gh: low=3.53, high 3.6,  rate of change/hour 1
           Quality q = new Quality(db);
           q.SaveLimits("instant_lapo_gh", 3.6, 3.53, 1.0);

           var rthcdi = CreateTempRatingTable("hcdi.csv", new double[] { 65.21, 65.22, 65.23, 65.24, 65.25 },
                                                         new double[] { 100,200,300,400,500});

           var site = db.GetSiteCatalog();
           site.AddsitecatalogRow("lapo", "", "OR");
           site.AddsitecatalogRow("hcdi", "", "OR");
           db.Server.SaveTable(site);
            var c = new CalculationSeries("instant_hcdi_q");
            c.Expression = "FileRatingTable(hcdi_gh,\""+rthcdi+"\")";
            db.AddSeries(c);
            c = new CalculationSeries("instant_lapo_q");
            c.SiteID = "lapo";
            c.Expression = "FileRatingTable(%site%_gh,\""+rtlapo+"\")";
            db.AddSeries(c);



            db.Inventory();
            SeriesExpressionParser.Debug = true;
            FileImporter import = new FileImporter(db);
            import.Import(tmpDir,RouteOptions.None,computeDependencies:true);
            db.Inventory();

            var s = db.GetSeriesFromTableName("instant_hcdi_q");
            s.Read();
            Assert.IsTrue(s.Count > 0, "No flow data computed hcdi");

            s = db.GetSeriesFromTableName("instant_lapo_q");
            s.Read();
            var expectedFlags = new string[]{"","","","+","","","","-"};
            for (int i = 0; i < s.Count; i++)
            {
                s[i].Flag
            }

            Assert.IsTrue(s.Count > 0, "No flow data computed lapo");


            SeriesExpressionParser.Debug = false;
            
        }

        private string CreateTempRatingTable(string filename, double[] gh, double[] q)
        {
            var tmpDir = FileUtility.GetTempPath();
            var path = Path.Combine(tmpDir, filename);
            StreamWriter sw = new StreamWriter(path, false);
            sw.WriteLine("gh,q");
            for (int i = 0; i < gh.Length; i++)
            {
                sw.WriteLine(gh[i].ToString("F2")+","+ q[i].ToString("F2"));
            }
            sw.Close();
            return path;
        }

        [Test]
        public void ImportDecodesWithMissingGageHeight()
        {

            var fn1 = FileUtility.GetTempFileNameInDirectory(@"c:\temp", ".pdb", "mabo");
            Console.WriteLine(fn1);
            var svr = new SQLiteServer(fn1);
            var db = new TimeSeriesDatabase(svr, Reclamation.TimeSeries.Parser.LookupOption.TableName);
            Logger.EnableLogger();
            FileUtility.CleanTempPath();
            var tmpDir = CopyTestDecodesFileToTempDirectory("decodes_mabo_missing_gh.txt");

            var c = new CalculationSeries("instant_mabo_q");
            c.Expression = "FileRatingTable(mabo_gh,\"mabo.csv\")";
            db.AddSeries(c);

            FileImporter import = new FileImporter(db);
            import.Import(tmpDir, RouteOptions.Outgoing, computeDependencies: true);
            db.Inventory();

            var s = db.GetSeriesFromTableName("instant_mabo_q");

            s.Read();

            Assert.IsTrue(s.Count > 0, "No flow data computed");
        }


        internal static string CopyTestDecodesFileToTempDirectory(string fileName)
        {
         
            var fn = Path.Combine(Path.Combine(Globals.TestDataPath, "decodes"), fileName);

            var tmpDir = FileUtility.GetTempPath();
            
            var tmpFile = Path.GetFileNameWithoutExtension(fileName);
            tmpFile += DateTime.Now.Ticks.ToString() + ".txt";
            tmpFile = Path.Combine(tmpDir, tmpFile);
            //C:\Users\ktarbet\AppData\Local\Temp\2\Reclamation\NUnit\decodes_dms3635491482158432877.txt
            //C:\Users\ktarbet\AppData\Local\Temp\2\Reclamation\NUnit
            Console.WriteLine(tmpFile);
            Console.WriteLine("copy "+fn +" to "+tmpFile);
            File.Copy(fn, tmpFile, true);

            File.SetCreationTime(tmpFile, File.GetCreationTime(fn));
            File.SetLastAccessTime(tmpFile, File.GetLastAccessTime(fn));
            File.SetLastWriteTime(tmpFile, File.GetLastWriteTime(fn));

            return tmpDir;
        }

       

    }
}
