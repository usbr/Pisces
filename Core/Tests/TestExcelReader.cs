using System;
using System.Data;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Reclamation.Core;
namespace Reclamation.Core.Tests
{
    [TestFixture]
    public class TestExcelReader
    {

        [Test]
        public void XLSX()
        {
            var fn = Path.Combine(Globals.TestDataPath, "CalculationTests.xlsx");
            Console.WriteLine(fn);
            var tbl = ExcelUtility.Read(fn, 0);
            Console.WriteLine(tbl.Rows.Count);
        }

    }
}