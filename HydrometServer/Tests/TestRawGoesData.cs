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
    class TestRawGoesData
    {

        static void Main(string[] argList)
        {
            var x = new TestRawGoesData();
            x.ProcessRawFile();
        }

        [Test]
        public void ProcessRawFile()
        {
            FileUtility.CleanTempPath();
            Performance perf = new Performance();
            var fn1 = FileUtility.GetTempFileName(".pdb");
            Console.WriteLine(fn1);
            var svr = new SQLiteServer(fn1);
            var db = new TimeSeriesDatabase(svr, Reclamation.TimeSeries.Parser.LookupOption.TableName);
            Logger.EnableLogger();
            
            var tmpDir = TestRatingTableDependency.CopyTestDecodesFileToTempDirectory("bigi.txt");

            FileImporter import = new FileImporter(db);
            import.Import(tmpDir, RouteOptions.Outgoing,searchPattern:"*.txt");
            //Assert.IsTrue(db.GetSeriesCatalog().Count>1);
            perf.Report();// 5.4 seconds 
        }

        
       

    }
}
