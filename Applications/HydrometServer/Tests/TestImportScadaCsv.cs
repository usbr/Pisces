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
    /// Import Hydromet DMS3 output from DECODES
    /// </summary>
    [TestFixture]
    class TestImportScadaCsv
    {

        public TestImportScadaCsv()
        {
        }

        public bool AnyErrors;

        [Test]
        public void ImportParameterWithUnderscore()
        {
            Logger.OnLogEvent += Logger_OnLogEvent;
            var fn1 = FileUtility.GetTempFileName(".pdb");
            Console.WriteLine(fn1);
            var svr = new SQLiteServer(fn1);
            var db = new TimeSeriesDatabase(svr, Reclamation.TimeSeries.Parser.LookupOption.TableName,false);

            var tmpDir = TestRatingTableDependency.CopyTestDecodesFileToTempDirectory("curdata_2018-1-2.csv","efo");

            FileImporter import = new FileImporter(db);
            import.Import(tmpDir, computeDependencies: true, searchPattern: "*.txt");
            var sc = db.GetSeriesCatalog();
            Assert.AreEqual(86, sc.Rows.Count);
            db.Inventory();
            //Assert.IsFalse(anyErrors);
        }

        void Logger_OnLogEvent(object sender, StatusEventArgs e)
        {
            Console.WriteLine(e.Message);
            if (e.Message.IndexOf("Error:") >= 0)
                AnyErrors = true;
        }

       

    }
}
