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
            x.ImportDecodesWithMissingGageHeight();
            x.ImportDecodesAndProcessWithFlagLimits();
        }
        public TestRatingTableDependency()
        {
        }

        /// <summary>
        /// This test imports satellite decoded data for a site lapo
        /// lapo has a rating tables setup for flow calculations
        /// lapo has quality limits set for both gage height (gh) and flow (q)
        /// assertions check if proper flags are set based on the limits
        /// </summary>
        [Test]
        public void ImportDecodesAndProcessWithFlagLimits()
        {
            Logger.EnableLogger();
            FileUtility.CleanTempPath();
            var fn1 = FileUtility.GetTempFileName(".pdb");
            Console.WriteLine(fn1);
            var svr = new SQLiteServer(fn1);
            var db = new TimeSeriesDatabase(svr, Reclamation.TimeSeries.Parser.LookupOption.TableName,false);
         
            var tmpDir = CopyTestDecodesFileToTempDirectory("decodes_lapo.txt");
            
           var rtlapo = CreateTempRatingTable("lapo.csv", new double[] {3.50,3.54,3.55,5.54 },
                                             new double[] {1,2,3,10 });
            // set limits  gh: low=3.53, high 3.6,  rate of change/hour 1
           Quality q = new Quality(db);
           q.SaveLimits("instant_lapo_gh", 3.6, 3.53, 1.0);
           q.SaveLimits("instant_lapo_q", 5, 1.1, 0);

           var site = db.GetSiteCatalog();
           site.AddsitecatalogRow("lapo", "", "OR");
           db.Server.SaveTable(site);
            var c = new CalculationSeries("instant_lapo_q");
            c.SiteID = "lapo";
            c.Expression = "FileRatingTable(%site%_gh,\""+rtlapo+"\")";
            db.AddSeries(c);

            //SeriesExpressionParser.Debug = true;
            FileImporter import = new FileImporter(db);
            import.Import(tmpDir,computeDependencies:true,searchPattern:"*.txt");
            db.Inventory();


            var s = db.GetSeriesFromTableName("instant_lapo_gh");
            var expectedFlags = new string[] { "", "", "", "+", "", "", "", "-" };
            for (int i = 0; i < s.Count; i++)
            {
                  Assert.AreEqual(expectedFlags[i], s[i].Flag, " flag not expected ");
            }

            s = db.GetSeriesFromTableName("instant_lapo_q");
            s.Read();
            Assert.IsTrue(s.Count > 0, "No flow data computed lapo");
            s.WriteToConsole(true);
             // computed flows should be: 2 2 2 10 2 2 1
            expectedFlags = new string[]{"","","","+","","","","-"}; //q>=1 and q<= 5
            for (int i = 0; i < s.Count; i++)
            {
                 Assert.AreEqual(expectedFlags[i], s[i].Flag.Trim()," Flag check on Flow (Q) "); 
            }

            SeriesExpressionParser.Debug = false;
        }

        private string CreateTempRatingTable(string filename, double min, double max, Func<double,double> f,
            double increment = 0.01)
        {
            var x = new System.Collections.Generic.List<double>();
            var y = new System.Collections.Generic.List<double>();
            double x1 = min;
            do
            {
                x.Add(x1);
                y.Add(f(x1));
                x1 += increment;
            } while (x1 <=max);
            return CreateTempRatingTable(filename, x.ToArray(), y.ToArray());
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

        /// <summary>
        /// missing gage height data is input with magic value 998877 (hydromet convention)
        /// flow calculations should not use this missing value to compute missing flow
        /// </summary>
        [Test]
        public void ImportDecodesWithMissingGageHeight()
        {
            FileUtility.CleanTempPath();
            var fn1 = FileUtility.GetTempFileName(".pdb");
            Console.WriteLine(fn1);
            var svr = new SQLiteServer(fn1);
            var db = new TimeSeriesDatabase(svr, Reclamation.TimeSeries.Parser.LookupOption.TableName,false);
            Logger.EnableLogger();
            
            var tmpDir = CopyTestDecodesFileToTempDirectory("decodes_mabo_missing_gh.txt");
            var ratingTableFileName =CreateTempRatingTable("mabo.csv", 2.37, 2.8, x => (x*10));
            var c = new CalculationSeries("instant_mabo_q");
            c.Expression = "FileRatingTable(mabo_gh,\""+ratingTableFileName+"\")";
            db.AddSeries(c);

            FileImporter import = new FileImporter(db);
            import.Import(tmpDir, computeDependencies: true,searchPattern:"*.txt");
            db.Inventory();

            var s = db.GetSeriesFromTableName("instant_mabo_q");
           
            s.Read();
            Assert.IsTrue(s.CountMissing() == 0);
            Assert.IsTrue(s.Count > 0, "No flow data computed");
        }


        internal static string CopyTestDecodesFileToTempDirectory(string fileName,string subdir="decodes")
        {
         
            var fn = Path.Combine(Path.Combine(Globals.TestDataPath, subdir), fileName);

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
