using System.Configuration;
using NUnit.Framework;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Urgsim;

namespace Pisces.NunitTests.SeriesTypes
{
    [TestFixture]
    class TestReadData
    {
        //[Test]
        //public void Test1()
        //{
        //    var s = new UrgsimSeries(@"\\ibr1pnrfp002\PN6200\KTarbet\EmissionsScenarios\URGSim", "WWCRAOutputReservoirs.xlsx",
        //        "bccr_bcm2_0.1.sresa1", "Heron SJC Alb [AF]");
        //    s.Read();
        //    Assert.IsTrue(1788 == s.Count);
        //    Assert.AreEqual(s[12].Value, 7528.15557, .1);
        //    Assert.AreEqual(s.Units, "acre-feet");
        //}

        //[Test]
        //public void Test2()
        //{
        //    string path = ConfigurationManager.AppSettings["UrgsimPath"];
        //    var s = new UrgsimSeries(path, @"\bias-corrected\ukmo_hadcm3.1.sresb1", "Heron SJC Alb [AF]");
        //    s.Read();

        //    var tbl = new TimeSeriesDatabaseDataSet.ScenarioDataTable();
        //    tbl.AddScenarioRow(@"bias-corrected\ukmo_hadcm3.1.sresb1", true, 
        //                       path + @"bias-corrected\ukmo_hadcm3.1.sresb1");

        //    var s2 = s.CreateScenario(tbl[0]);

        //}
    }
}
