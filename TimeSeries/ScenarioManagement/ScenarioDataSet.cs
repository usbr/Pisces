using Reclamation.Core;
using System.Data;
using System.IO;
namespace Reclamation.TimeSeries.ScenarioManagement {
    
    
    public partial class ScenarioDataSet {

       
        public void Read(string filename)
        {
            NpoiExcel xls = new NpoiExcel(filename);

            var siteMapping = xls.ReadDataTable("siteMapping");
            Merge(siteMapping,false, MissingSchemaAction.Ignore);
            var scenarioMapping = xls.ReadDataTable("scenarioMapping");
            Merge(scenarioMapping, false, MissingSchemaAction.Ignore);
        }

        internal void CreatePiscesDatabase(string filename)
        {
            if (File.Exists(filename))
                File.Delete(filename);

            SQLiteServer svr = new SQLiteServer(filename);
            TimeSeriesDatabase db = new TimeSeriesDatabase(svr);
            var scenarios = db.GetScenarios();
            string scenarioPrefix = "";
            foreach (var s in tableScenarioMapping)
            {
                scenarios.AddScenarioRow(s.ScenarioName, false, s.ScenarioNumber, 0, false);

                


                if (scenarioPrefix == "")
                    scenarioPrefix = s.ScenarioName+"_";

            }
            svr.SaveTable(scenarios);

        }
    }
}
