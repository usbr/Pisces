using NUnit.Framework;
using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Tests.Scenarios
{

    [TestFixture]
    public class TestScenarios
    {

        [Test, Category("Internal")]
        public void ReadExcelScenario()
        {
            string fn = Path.Combine(TestData.DataPath, "Scenarios", "InputScenarioConfig.xlsx");

            var ds = new ScenarioManagement.ScenarioDataSet();
            var fn1 = FileUtility.GetTempFileName(".pdb");

            SQLiteServer svr = new SQLiteServer(fn1);
            var db = new TimeSeriesDatabase(svr,false);
            ds.Import(fn, db);

            Assert.IsTrue(ds.ScenarioMapping.Count > 0);

            // create Scenarios

            File.Delete(fn1);
        }
    }
}
