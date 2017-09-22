using NUnit.Framework;
using Reclamation.TimeSeries;
using System.IO;
using Math = Reclamation.TimeSeries.Math;

namespace Pisces.NunitTests.SeriesMath
{
    [TestFixture]
    public class TestResidual
    {
        [Test]
        public void HeiseResidualTest()
        {
            //ExcelDataReaderSeries heii_qu = new ExcelDataReaderSeries(fn, "unregulation", "Date", "HEII_QU", "cfs");
            var heii_qu = new TextSeries(Path.Combine(TestData.DataPath,"heii_qu.csv"));
            heii_qu.Read();

            var heii_resid = Math.WaterYearResidual(heii_qu,7);
            heii_resid.WriteToConsole();

            var expected = new TextSeries(Path.Combine(TestData.DataPath, "heii_residual.csv"));
            //var expected = new ExcelDataReaderSeries(fn, "unregulation", "Date", "HEII_RESID", "cfs");
            expected.Read();

            var diff = Math.Sum(heii_resid - expected);
           
            Assert.IsTrue(System.Math.Abs(diff) < 0.01);
        }


    }
}
