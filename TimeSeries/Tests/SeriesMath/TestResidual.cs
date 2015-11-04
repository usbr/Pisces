using System;
using System.Data;
using System.IO;
using NUnit.Framework;
using Reclamation.TimeSeries;
using Math = Reclamation.TimeSeries.Math;
using Reclamation.TimeSeries.Excel;

namespace Pisces.NunitTests.SeriesMath
{
    [TestFixture]
    public class TestResidual
    {
        [Test]
        public void HeiseResidualTest()
        {
            var fn = Path.Combine(TestData.DataPath, "ResidualCalcs.xls");

            ExcelDataReaderSeries heii_qu = new ExcelDataReaderSeries(fn, "unregulation", "Date", "HEII_QU", "cfs");
            heii_qu.Read();

            var heii_resid = Math.ResidualOctToJul(heii_qu);
            heii_resid.WriteToConsole();

            var expected = new ExcelDataReaderSeries(fn, "unregulation", "Date", "HEII_RESID", "cfs");
            expected.Read();

            var diff = Math.Sum(heii_resid - expected);
           
            Assert.IsTrue(System.Math.Abs(diff) < 0.01);
        }


    }
}
