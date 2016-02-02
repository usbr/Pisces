using System;
using NUnit.Framework;
using Reclamation.TimeSeries;
using System.IO;
using Reclamation.Core;

namespace Pisces.NunitTests.Import
{
	[TestFixture]
	public class TestImportRioGrande
    {
        [Test]
		public void TestArreyCanal()
		{
            var zfn  = Path.Combine(TestData.DataPath, "AC_FLOW.zip");
            var fn = FileUtility.GetTempFileName(".xls");
            ZipFile.UnzipFile(zfn, fn);
            Console.WriteLine(fn);

            var s = Reclamation.TimeSeries.Import.ImportRioGrandeExcel.ImportSpreadsheet(fn);
            Console.WriteLine(s.Count);

            Console.WriteLine(s[s.Count-1].ToString());
            Assert.AreEqual(289, s["2010-8-12"].Value, 0.01);

		}

  

	}
}
