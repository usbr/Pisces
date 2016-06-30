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
            var fnUnix = FileUtility.GetTempFileName(".pdb");
            var fnString = FileUtility.GetTempFileName(".pdb");

            File.Delete(fnUnix);
            File.Delete(fnString);
            int size = 5;
            var server1 = new SQLiteServer(fnUnix);
            var db1 = new TimeSeriesDatabase(server1,false);
            db1.UnixDateTime = true;
            db1.AddSeries(CreateSeries(size));
            ReadSeries(db1,size);

            var server = new SQLiteServer(fnString);
            var db = new TimeSeriesDatabase(server,false);
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
