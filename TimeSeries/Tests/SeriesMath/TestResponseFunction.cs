using NUnit.Framework;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System;
using System.IO;
using Math = Reclamation.TimeSeries.Math;

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
            string fn = Path.Combine(TestData.DataPath, "SimpleResponseFunction.csv");
            var csv = new CsvFile(fn);
            var diversion = new DataTableSeries(csv, TimeInterval.Daily, "Date", "Input");
            diversion.Read();
            var expected = new DataTableSeries(csv, TimeInterval.Daily, "Date", "ExpectedResponse");
            expected.Read();

            var responseFunction = new DataTableSeries(csv,  TimeInterval.Daily, "Date", "ResponseFunction");
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
