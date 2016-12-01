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
		public void TestArreyCanal()
		{
            var fn  = Path.Combine(TestData.DataPath, "ac_flow.xls");
            Console.WriteLine(fn);
            var s = Reclamation.TimeSeries.Import.ImportMultiSheetDailySeriesExcel.ImportSpreadsheet(fn);
            Console.WriteLine(s.Count);
            Console.WriteLine(s[s.Count-1].ToString());
            Assert.AreEqual(289, s["2010-8-12"].Value, 0.01);
		}


        [Test]
        public void ImportDirectory()
        {
          var dir = Path.Combine(TestData.DataPath,"Scenarios","dir_import");
          var tmp = @"c:\temp\import_dir.pdb";
          if (File.Exists(tmp))
              File.Delete(tmp);

          var svr = new SQLiteServer(tmp);
          var db = new TimeSeriesDatabase(svr);
          BulkImportDirectory.Import(db,dir, "*.csv", 
@".+dir_import\\(?<scenario>[-a-z_0-9]+)-(?<siteid>[a-z0-9]+)-biascorrected_streamflow");

          Assert.IsTrue(db.GetSeriesCatalog().Rows.Count == 3);

          svr.CloseAllConnections();
          File.Delete(tmp);
        }
  

	}
}
