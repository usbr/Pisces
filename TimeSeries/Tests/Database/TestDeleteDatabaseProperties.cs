using NUnit.Framework;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System.IO;

namespace Pisces.NunitTests.Database
{
    [TestFixture]
    public class TestDeleteDatabaseProperties
    {


        TimeSeriesDatabase db;

        public TestDeleteDatabaseProperties()
        {
            string fn = Path.Combine(@"C:\temp", "factory.pdb");
            FileUtility.GetTempFileNameInDirectory(@"C:\temp\", ".pdb");

            SQLiteServer.CreateNewDatabase(fn);
            SQLiteServer svr = new SQLiteServer(fn);
            db = new TimeSeriesDatabase(svr, false);

            string dataPath = TestData.DataPath;
            Series s = new Series("Glomma River");
            
            db.AddSeries(s);

            s = db.GetSeriesFromName("Glomma River");
            s.TimeSeriesDatabase = db;
            s.Properties.Set("elevation", " 690 m");
            s.Properties.Save();
        }


        [Test]
        public void TestSeriesDeleteGlommaRiver()
        {
            var s = db.GetSeriesFromName("Glomma River");
           Assert.AreEqual(" 690 m", s.Properties.Get("elevation"));
            db.Delete(s);

            Assert.AreEqual(0, db.GetSeriesProperties().Count, "expect zero properties");

        }

    }
}