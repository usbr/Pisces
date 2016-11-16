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
          BasicDBServer svr = new SQLiteServer(@"c:\temp\import_dir.pdb");
          TimeSeriesDatabase db = new TimeSeriesDatabase(svr);
          BulkImportDirectory.Import(db,dir, "*.csv", 
@"C:\\TEMP\\UWdata\\AllData\\(?<scenario>[-a-z_0-9]+)-(?<siteid>[a-z0-9]+)-biascorrected_streamflow-provisional_0\.5\.csv");
             
        }
  

	}
}
