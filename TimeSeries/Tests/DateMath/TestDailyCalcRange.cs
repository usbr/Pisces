using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;
using Reclamation.Core;
using System.IO;
using Reclamation.TimeSeries.DataLogger;

namespace Pisces.NunitTests.DateMath
{
    [TestFixture]
    public class TestDailyCalcRange
    {

        [Test]
        public void MALI_DailyAverageMidnight()
        {
            string fn = Path.Combine(TestData.DataPath, "MALI_2016_08_16_2315.dat");
            LoggerNetFile lf = new LoggerNetFile(fn);

            SeriesList list = lf.ToSeries();
            TimeRange tr;
            bool valid = TimeSeriesImporter.TryGetDailyTimeRange(list, out tr, new DateTime(2016,8,17,0,10,0));

            Assert.IsTrue(valid);
            Assert.AreEqual( DateTime.Parse("2016-08-16"),tr.StartDate);
            Assert.AreEqual( DateTime.Parse("2016-08-16").EndOfDay(),tr.EndDate);
        }

        [Test]
        public void MALI_TodayFilesNoCalc()
        {
            string fn = CreateFutureFile();
            var contents = File.ReadAllText(fn);
            contents = contents.Replace("2016-08-16", DateTime.Now.ToString("yyyy-MM-dd"));
            
            File.WriteAllText(fn, contents);

            LoggerNetFile lf = new LoggerNetFile(fn);

            SeriesList list = lf.ToSeries();
            TimeRange tr;
            bool valid = TimeSeriesImporter.TryGetDailyTimeRange(list, out tr,DateTime.Now);

            Assert.IsFalse(valid);
        }

        private string CreateFutureFile()
        {
            var fn = Path.Combine(FileUtility.GetTempPath(), "MALI_XYZ.dat");
            File.WriteAllLines(fn,
                new string[]{
"\"TOACI1\",\"MALI\",\"Min15\",",
"\"TMSTAMP\",\"RECNBR\",\"BV\",\"OB\",\"OBM\",\"OBX\",\"OBN\",\"TU\",\"TUX\",\"TUN\",\"EA\",\"TP\",\"WS\",\"WSH\",\"WD\",\"WDS\",\"WG\",\"PC\",\"SQ\",\"UI\",\"SI\",\"ETo\",\"ETr\",\"SW\",\"SV\"",
"\"2016-08-16 06:15:00\",3408,12.41,49.24,49.27,49.72,48.93,72.1,73.98,69.68,0.861,40.68,1.658,1.658,62.59,11.82,3.726,3.268,5042,4876,0,0.003072904,0.005296957,72.73,72.23",
"\"2016-08-16 06:30:00\",3409,12.39,49.12,49.13,49.42,48.87,72.94,79.53,69.1,0.866,40.83,0.986,0.986,43.29,24.73,2.85,3.268,5042,4877,0.021,0.001928358,0.003254686,72.6,72.17",
"\"2016-08-16 06:45:00\",3410,12.39,47.59,48,49.12,47.35,75.26,78.31,71.34,0.857,40.56,1.923,1.923,290.8,35.24,4.165,3.268,5042,4878,0.262,0.0034616,0.005698343,72.51,72.11",
"\"2016-08-16 07:00:00\",3411,12.38,48.69,48.11,48.87,47.47,76.52,78.85,72.69,0.875,41.09,2.936,2.936,325.1,26.1,3.946,3.268,5042,4878,0.89,0.00554588,0.008715499,72.39,72.05"
        });

            return fn;
        }
    }
}
