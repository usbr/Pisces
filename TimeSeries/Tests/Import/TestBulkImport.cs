using System;
using NUnit.Framework;
using Reclamation.TimeSeries;
using System.IO;
using Reclamation.Core;
using Reclamation.TimeSeries.Import;

namespace Pisces.NunitTests.Import
{
    /// <summary>
    /// Test bulk import of scenario data
    /// </summary>
    [TestFixture]
    public class TestBulkImport
    {

        [Test]
        public void ImportDirectory()
        {
            var dir = Path.Combine(TestData.DataPath, "Scenarios", "dir_import");
            var tmp = FileUtility.GetTempFileName(".pdb");
            if (File.Exists(tmp))
                File.Delete(tmp);

            var svr = new SQLiteServer(tmp);
            var db = new TimeSeriesDatabase(svr);
            
            BulkImportDirectory.Import(db, dir, "*.csv",
  @".+dir_import\"+ Path.DirectorySeparatorChar+"(?<scenario>[-a-z_0-9]+)-(?<siteid>[a-z0-9]+)-biascorrected_streamflow");

            var sc = db.GetSeriesCatalog();
            Assert.IsTrue(sc.Rows.Count == 3,"found "+sc.Rows.Count+" rows in catalog. expected 3");

            svr.CloseAllConnections();
            File.Delete(tmp);
        }
    }
}
