using System;
using NUnit.Framework;
using Reclamation.TimeSeries;
using System.IO;
using Reclamation.Core;
using Reclamation.TimeSeries.Import;

namespace Pisces.NunitTests.Import
{
    [TestFixture]
    public class TestBulkImport
    {

        [Test]
        [Ignore("Until test data paths can be reliably resolved on Linux")]
        public void ImportDirectory()
        {
            var dir = Path.Combine(TestData.DataPath, "Scenarios", "dir_import");
            var tmp = @"c:\temp\import_dir.pdb";
            if (File.Exists(tmp))
                File.Delete(tmp);

            var svr = new SQLiteServer(tmp);
            var db = new TimeSeriesDatabase(svr);
            BulkImportDirectory.Import(db, dir, "*.csv",
  @".+dir_import\\(?<scenario>[-a-z_0-9]+)-(?<siteid>[a-z0-9]+)-biascorrected_streamflow");

            Assert.IsTrue(db.GetSeriesCatalog().Rows.Count == 3);

            svr.CloseAllConnections();
            File.Delete(tmp);
        }
    }
}
