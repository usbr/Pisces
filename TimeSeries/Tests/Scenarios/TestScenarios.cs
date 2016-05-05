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

        [Test]
        public void ReadExcelScenario()
        {
            string fn = Path.Combine(TestData.DataPath, "Scenarios", "InputScenarioConfig.xlsx");

            var ds = new ScenarioManagement.ScenarioDataSet();
            ds.Read(fn);

            Assert.IsTrue(ds.ScenarioMapping.Count == 1);

            // create Scenarios
            var fn1 = FileUtility.GetTempFileName(".pdb");
            ds.CreatePiscesDatabase(fn1);
        }
    }
}
