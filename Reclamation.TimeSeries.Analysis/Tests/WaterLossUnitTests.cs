using NUnit.Framework;
using System;
using System.IO;

namespace Reclamation.TimeSeries.Analysis.Tests
{
    [TestFixture]
    public class WaterLossUnitTests
    {
        SeriesProvider provider = new SeriesProvider();

        /// <summary>
        /// Make sure each test has correct working directory before it's run so that
        /// Reclamation.Core methods can find files in the right relative path.
        /// </summary>
        [SetUp]
        public void InitWorkingDir()
        {
            var dir = Path.GetDirectoryName(typeof(WaterLossUnitTests).Assembly.Location);
            Environment.CurrentDirectory = dir;
        }

        /// <summary>
        /// This test should demonstrate the WaterLossCheck method does not detect the false positive
        /// on steady increase data.
        /// </summary>
        [Test]
        public void TestWaterLossSprinkler()
        {
            // Test gradual water loss against sprinkler (Loss? Sprinkler?) (Expected return value of false).
            var url = "https://github.com/usbr/pisces-example-data/blob/master/data-issues/sprinkler/daily-sprinkler.csv";
            var s = provider.FetchFromCSV(url);

            // Run the test
            Assert.IsFalse(Recipes.WaterLossCheck(s), "WaterLossCheck failed on bucket dump.");
        }

    }
}