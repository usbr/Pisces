using System;
using NUnit.Framework;
using Reclamation.TimeSeries;
using System.IO;
using Reclamation.Core;

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

  

	}
}
