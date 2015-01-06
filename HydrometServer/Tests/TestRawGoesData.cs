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
            
            var fn1 = FileUtility.GetTempFileNameInDirectory(@"c:\temp", ".pdb", "rawgoes");
            Console.WriteLine(fn1);
            var svr = new SQLiteServer(fn1);
            var db = new TimeSeriesDatabase(svr, Reclamation.TimeSeries.Parser.LookupOption.TableName);
            Logger.EnableLogger();
            FileUtility.CleanTempPath();
            var tmpDir =  TestRatingTableDependency.CopyTestDecodesFileToTempDirectory("raw_20150106-1132.txt");

            FileImporter import = new FileImporter(db);
            import.Import(tmpDir, RouteOptions.None);

        }

        
       

    }
}
