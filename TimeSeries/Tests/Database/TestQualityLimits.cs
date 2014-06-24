using System;
using NUnit.Framework;
using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries;

namespace Pisces.NunitTests.Database
{
    [TestFixture]
    public class TestQualityLimits
    {

        [Test]
        public void TestSQLite()
        {
            var fn = @"c:\temp\test_quality.db";
            System.IO.File.Delete(fn);

            SQLiteServer svr = new SQLiteServer(fn);
            var db = new TimeSeriesDatabase(svr);

            var sql = "insert into quality_limit values ('*_OB',120,-50,null); ";

            svr.RunSqlCommand(sql);

            var s = new Series();
            s.Table.TableName = "karl_ob";
            s.Add("1-1-2013", 100);
            s.Add("1-2-2013", -51);
            s.Add("1-3-2013", 100);
            s.Add("1-4-2013", 150);

            //db.TimeSeriesImporter.Process(s);
            db.ImportSeriesUsingTableName(s, true, setQualityFlags: true);
            s = db.GetSeriesFromTableName("karl_ob");
            s.Read();
            Console.WriteLine("has flags = " + s.HasFlags);
            s.WriteToConsole(true);

            Assert.AreEqual("", s["1-1-2013"].Flag);
            Assert.AreEqual("-", s["1-2-2013"].Flag);
            Assert.AreEqual("", s["1-3-2013"].Flag);
            Assert.AreEqual("+", s["1-4-2013"].Flag);

        }
    }
}
