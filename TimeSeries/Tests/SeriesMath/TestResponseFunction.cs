using System;
using NUnit.Framework;
using Reclamation.TimeSeries;
using Math = Reclamation.TimeSeries.Math;
using Reclamation.TimeSeries.Excel;
using System.IO;
namespace Pisces.NunitTests.SeriesMath
{
    /// <summary>
    /// Summary description for TestResponseFunction
    /// </summary>
    [TestFixture]
    public class TestResponseFunction
    {
        public TestResponseFunction()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        [Test]
        public void TestMethod1()
        {
            string fn = Path.Combine(TestData.DataPath, "SimpleResponseFunction.xls");

            var diversion = new ExcelDataReaderSeries(fn, "Sheet1", "Date", "Input");
            diversion.Read();
            var expected = new ExcelDataReaderSeries(fn, "Sheet1", "Date", "ExpectedResponse");
            expected.Read();

            var responseFunction = new ExcelDataReaderSeries(fn, "Sheet1", "Date", "ResponseFunction");
            responseFunction.Read();

            Console.WriteLine(" responseFunction has " + responseFunction.Count + " points");
            Console.WriteLine("diversion has " + diversion.Count);

            Series lag = Math.RoutingWithLags(diversion, responseFunction.Values);
            Assert.AreEqual(7, lag.Count);

            lag.WriteToConsole();
            if (lag.Count != expected.Count)
                Console.WriteLine("Error: expected count not the same as lag count");

            double sumDiff = System.Math.Abs(Math.Sum(expected - lag));

            Console.WriteLine("Sum of difference = " + sumDiff);
            Assert.AreEqual(0,sumDiff,0.0000001);

        }
    }
}
