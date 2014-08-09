using NUnit.Framework;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System;
using System.IO;

namespace Pisces.NunitTests.Database
{
    [TestFixture]
    public class TestSqLiteUnixTime
    {


        [Test]
        public void TimeStampStorage()
        {
            File.Delete(@"c:\temp\unixtime.db");
            File.Delete(@"c:\temp\stringtime.db");
            int size = 5;
            var server1 = new SQLiteServer(@"Data Source=c:\temp\unixtime.db;datetimeformat=UnixEpoch;");
            var db1 = new TimeSeriesDatabase(server1);
            db1.AddSeries(CreateSeries(size));
            ReadSeries(db1,size);

            var server = new SQLiteServer(@"C:\temp\stringtime.db");
            var db = new TimeSeriesDatabase(server);
            db.AddSeries(CreateSeries(size));
            ReadSeries(db,size);


        }

        private static void ReadSeries(TimeSeriesDatabase db,int size)
        {
            var s = db.GetSeriesFromName("test_series");
            s.Read();
            Assert.AreEqual(size, s.Count);
            if(s.Count < 10)
              s.WriteToConsole();
        }

        private static Series CreateSeries(int size)
        {
            Series s = new Series("test_series");
            DateTime t = DateTime.Now.Date;
            for (int i = 0; i < size; i++)
            {
                s.Add(t, i);
                t = t.AddDays(1);
            }
            return s;
        }


    }
}
