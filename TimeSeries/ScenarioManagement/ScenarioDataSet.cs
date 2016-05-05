using Reclamation.Core;
using System.Data;
using System.IO;
namespace Reclamation.TimeSeries.ScenarioManagement {
    
    
    /// <summary>
    /// Manages definition of Scenarios to import from hydrologic models into Pisces
    /// and to export with local inflow calculations to other Pisces databases
    /// </summary>
    public partial class ScenarioDataSet {

        NpoiExcel xls;
        TimeSeriesDatabase m_db;
        public void Import(string filename, TimeSeriesDatabase db)
        {
             xls = new NpoiExcel(filename);
             m_db = db;
            var siteMapping = xls.ReadDataTable("siteMapping");
            Merge(siteMapping,false, MissingSchemaAction.Ignore);
            var scenarioMapping = xls.ReadDataTable("scenarioMapping");
            Merge(scenarioMapping, false, MissingSchemaAction.Ignore);

            ImportToPisces();
        }

        private void ImportToPisces()
        {
            var scenarios = m_db.GetScenarios();
            //var sc = m_db.GetSeriesCatalog();
            string scenarioPrefix = "";
            foreach (var scenario in tableScenarioMapping)
            {
                scenarios.AddScenarioRow(scenario.ScenarioName, false, scenario.ScenarioNumber, 0, false);
                
                AddSeries(m_db, scenarioPrefix, scenario.ScenarioNumber );

                if (scenarioPrefix == "")
                    scenarioPrefix = scenario.ScenarioName+"_";

            }
            m_db.Server.SaveTable(scenarios);
        }

        private void AddSeries(TimeSeriesDatabase db, string scenarioPrefix, string scenarioNumber)
        {
            DataTable scenarioSheet = xls.ReadDataTable(scenarioNumber);
            foreach (DataRow row in scenarioSheet.Rows)
            {
                var externalSiteID = row["ExternalSiteID"].ToString();
                var internalSiteID = LookupInternalSiteID(externalSiteID);
                string basin = LookupBasin(externalSiteID);
                var parent = db.GetOrCreateFolder(basin);
                //var s = new Series(scenaroSite);
                Series s = ReadExternalSeriesData(scenarioPrefix, row, externalSiteID);
                s.Name = internalSiteID;
                db.AddSeries(s, parent);
            }
        }

        private static TextSeries ReadExternalSeriesData(string scenarioPrefix, DataRow row, string externalSiteID)
        {
            string filename = row["FilePath"].ToString();
            // hack (off network temporarly) -- 
            filename = @"C:\temp\test.csv";

            TextSeries s = new TextSeries(filename);
            s.Read();
            s.Provider = "Series";
            s.Table.TableName = scenarioPrefix + externalSiteID;
            return s;
        }

        private string LookupBasin(string externalSiteID)
        {
            DataRow[] rows = this.SiteMapping.Select("ExternalSiteID = '" + externalSiteID + "'");
            if (rows.Length == 1)
                return rows[0]["Basin"].ToString();
            return "basin_undefined";
        }
       
        private string LookupInternalSiteID(string externalSiteID)
        {
            DataRow[] rows = this.SiteMapping.Select("ExternalSiteID = '" + externalSiteID + "'");
            if (rows.Length == 1)
                return rows[0]["InternalSiteId"].ToString();
            if (rows.Length == 0)
                return "Error - SiteMapping does not contain ExternalSiteID of '" + externalSiteID + "'";
            else
                return "Error - SiteMapping has multiple mappings for ExternalSiteID of '" + externalSiteID + "'";
        }
    }
}
