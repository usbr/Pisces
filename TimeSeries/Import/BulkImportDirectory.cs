using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Import
{
    public static class BulkImportDirectory
    {
        /// <summary>
        /// Imports all files recursively 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileFilter">filter such as *.csv</param>
        public static void Import(TimeSeriesDatabase db,string path, string fileFilter, string regexFilter)
        {

            DirectoryScanner ds = new DirectoryScanner(path, fileFilter, regexFilter);
            var scenarios = db.GetScenarios();

            int scenarioNumber = 1;
            foreach (var scenario in ds.UniqueScenarios())
            {
                if (scenario != "")
                    scenarios.AddScenarioRow(scenario, false, scenarioNumber.ToString(), 0);
            }

            db.Server.SaveTable(scenarios);
            List<string> errorList = new List<string>();
            for (int i = 0; i < ds.Files.Length; i++)
            {
                try
                {
                    TextSeries s = new TextSeries(ds.Files[i]);
                    s.Read();
                    s.Name = ds.Siteid[i];
                    s.ConnectionString = "ScenarioName=" + ds.Scenario[i]; ;
                    s.SiteID = ds.Siteid[i];
                    if (scenarios.Count > 0)
                        s.Table.TableName = (ds.Siteid[i] + "_" + ds.Scenario[i]).ToLower();
                    else
                        s.Table.TableName = Path.GetFileNameWithoutExtension(ds.Files[i]);

                    if (db.GetSeriesFromName(ds.Siteid[i]) == null)
                    {
                        int id = db.AddSeries(s);
                        var sc = db.GetSeriesCatalog("id =" + id);
                        // alter entry in database to remove scenario postfix from table name
                        sc.Rows[0]["tablename"] = ds.Siteid[i];
                        db.Server.SaveTable(sc);
                    }
                    else
                    { // if this series already exists (for another scenario)
                        // only save the TableData
                        s.Table.Columns[0].ColumnName = "datetime";
                        s.Table.Columns[1].ColumnName = "value";
                        db.CreateSeriesTable(s.Table.TableName, false);
                        db.Server.InsertTable(s.Table);
                    }

                    Logger.WriteLine("importing [" + i + "] --> " + ds.Files[i], "ui");
                }
                catch (Exception ex)
                {
                    errorList.Add(ex.Message);
                    Logger.WriteLine(ex.Message);
                }
            }

          
            if( errorList.Count > 0)
            {
                throw new Exception(String.Join("\n", errorList.ToArray()));
            }

           
        }
    
    }
}
