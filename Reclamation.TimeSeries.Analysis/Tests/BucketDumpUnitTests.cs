using NUnit.Framework;
using System;
using System.IO;

namespace Reclamation.TimeSeries.Analysis.Tests
{
    [TestFixture]
    public class BucketDumpUnitTests
    {
        SeriesProvider provider = new SeriesProvider();

        /// <summary>
        /// Make sure each test has correct working directory before it's run so that
        /// Reclamation.Core methods can find files in the right relative path.
        /// </summary>
        [SetUp]
        public void InitWorkingDir()
        {
            var dir = Path.GetDirectoryName(typeof(BucketDumpUnitTests).Assembly.Location);
            Environment.CurrentDirectory = dir;
        }

        /// <summary>
        /// Test precipitation dump at Warm Springs rain guage (WARO)
        /// Dump occures 2/28/2017 11:15
        /// daily precipitation (pp) would go very negative, but should be zero
        /// </summary>
        [Test]
        public void TestPrecipBucketDump_WARO()
        {
            var s = provider.FetchInstantFromAPI("waro", "pc", DateTime.Parse("2017-01-10"), DateTime.Parse("2017-10-12"));
            var expectedSeries = provider.FetchInstantFromAPI("waro", "pc", DateTime.Parse("2017-01-10"), DateTime.Parse("2017-10-12"));

            // Ultimately, the end goal of the project is to be able to correct for daily incremental precipitaion
            var dailyData = Recipes.DailyIncrementalPrecipFromInstant(s);

            var diff = dailyData - expectedSeries;
            var sumDiff = Math.Sum(diff);
            Assert.IsTrue(System.Math.Abs(sumDiff) < 0.01, "Computed daily incremental precipitation is different than expected");
        }

        /// <summary>
        /// Test ManualBucketDump algorithm against data set with bucket dump
        /// </summary>
        [Test]
        public void TestManualBucketDump()
        {
            var url = "https://github.com/usbr/pisces-example-data/blob/master/data-issues/precipitation/precipitation_gauge_manual_dump.csv";
            var s = provider.FetchFromCSV(url);

            var obj = new ManualBucketDump();
            Assert.IsTrue(obj.Analyze(s), "ManualBucketDump.Analyze failed with: " + url);
        }

    }
}