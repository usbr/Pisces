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
           CopyTestDecodesFileToTempDirectory("decodes_lapo.txt");


            var c = new CalculationSeries("instant_hcdi_q");
            c.Expression = "FileRatingTable(hcdi_gh,\"hcdi.csv\")";
            db.AddSeries(c);
            c = new CalculationSeries("instant_lapo_q");
            c.SiteID = "lapo";
            c.Expression = "FileRatingTable(%site%_gh,\"%site%.csv\")";
            db.AddSeries(c);



            db.Inventory();

            FileImporter import = new FileImporter(db);
            import.Import(tmpDir,RouteOptions.None,computeDependencies:true);
            db.Inventory();

            var s = db.GetSeriesFromTableName("instant_hcdi_q");

            s.Read();

            Assert.IsTrue(s.Count > 0,"No flow data computed");
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
