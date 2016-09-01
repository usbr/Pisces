using Reclamation.Core;
using System.Collections.Generic;
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
        List<string> errors = new List<string>();
        public void Import(string filename, TimeSeriesDatabase db)
        {
             xls = new NpoiExcel(filename);
             m_db = db;
            var siteMapping = xls.ReadDataTable("siteMapping",true,true);
            Merge(siteMapping,false, MissingSchemaAction.Ignore);
            var scenarioMapping = xls.ReadDataTable("scenarioMapping",true,true);
            Merge(scenarioMapping, false, MissingSchemaAction.Ignore);

            ImportToPisces();
        }

        public event ProgressEventHandler OnProgress;

        private void ImportToPisces()
        {
            var scenarios = m_db.GetScenarios();
            //var sc = m_db.GetSeriesCatalog();
            foreach (var scenario in tableScenarioMapping)
            {
                scenarios.AddScenarioRow(scenario.ScenarioName, false, scenario.ScenarioNumber, 0);
                AddSeries(m_db, scenario.ScenarioName, scenario.ScenarioNumber );
            }
            m_db.Server.SaveTable(scenarios);

            if (errors.Count > 0)
            {
                var msg = string.Join("\n", errors.ToArray());
                Logger.WriteLine(msg);
                throw new FileNotFoundException(msg);
            }
        }

        private void AddSeries(TimeSeriesDatabase db, string scenarioName, string scenarioNumber)
        {
            DataTable scenarioSheet = xls.ReadDataTable(scenarioNumber,true,true);
           
            int count = 0;
            foreach (DataRow row in scenarioSheet.Rows)
            {
                var externalSiteID = row["ExternalSiteID"].ToString();
                var internalSiteID = LookupInternalSiteID(externalSiteID);
                string basin = LookupBasin(externalSiteID);
                var parent = db.GetOrCreateFolder(basin);
                string filename = row["FilePath"].ToString();
                if( !File.Exists(filename))
                {
                    if( filename!= "")
                       errors.Add("Missing File: " + filename);
                    continue;
                }

                Series s = ReadExternalSeriesData(scenarioName, filename, externalSiteID);
                s.Name = internalSiteID;
                if( row.Table.Columns.IndexOf("units")>=0)
                {
                    s.Units = row["units"].ToString();
                }
                s.ConnectionString = "ScenarioName=" + scenarioName;
                var id =-1;
                if (db.GetSeriesFromName(internalSiteID) == null)
                {
                    id = db.AddSeries(s, parent);
                    var sc = db.GetSeriesCatalog("id =" + id);
                  // alter entry in database to remove scenario postfix from table name
                    sc.Rows[0]["tablename"] = internalSiteID.ToLower();
                    
                    db.Server.SaveTable(sc);
                }
                else
                { // if this series allready exists (for another scenario)
                  // only save the TableData
                    s.Table.Columns[0].ColumnName = "datetime";
                    s.Table.Columns[1].ColumnName = "value";
                    db.CreateSeriesTable(s.Table.TableName, false);
                    db.Server.InsertTable(s.Table);
                }

                if (OnProgress != null)
                    OnProgress(this,
                        new ProgressEventArgs(
                         "saving " + internalSiteID + " " + scenarioName, count / scenarioSheet.Rows.Count * 100));
                count++;
            }
            
        }       

        private static Series ReadExternalSeriesData(string scenarioName, string filename, string externalSiteID)
        {
            Series s = new TextSeries(filename);
            s.Read();
            s.Provider = "Series";
            s.Table.TableName = (externalSiteID +"_"+ scenarioName).ToLower();
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

        /// <summary>
        /// Exports scenarios. One pisces database per scenario.
        /// 
        /// </summary>
        /// <param name="excelFileName"></param>
        /// <param name="DB"></param>
        public void Export(string excelFileName, TimeSeriesDatabase DB)
        {


        }
    }
}
