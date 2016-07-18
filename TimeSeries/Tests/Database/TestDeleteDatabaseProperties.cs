using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using Reclamation.TimeSeries;
using Reclamation.Core;
using System.IO;
using Math = Reclamation.TimeSeries.Math;
using Reclamation.TimeSeries.Excel;
using Reclamation.TimeSeries.Hydromet;

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
            s.Properties.Set("elevation", " 690 m");

            db.AddSeries(s);
        }


        [Test]
        public void TestSeriesDeleteGlommaRiver()
        {
            var s = db.GetSeriesFromName("Glomma River");

            db.Delete(s);

            Assert.AreEqual(0, db.GetSeriesProperties().Count, "expect zero properties");

        }

    }
}