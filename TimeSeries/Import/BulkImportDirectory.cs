using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.IO;

namespace Reclamation.TimeSeries.Import
{
    public static class BulkImportDirectory
    {
        /// <summary>
        /// Imports all files recursively 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileFilter">filter such as *.csv</param>
        public static void Import(TimeSeriesDatabase db,string path, string fileFilter, string scenarioRegex)
        {

            DirectoryScanner ds = new DirectoryScanner(path, fileFilter, scenarioRegex);
            var scenarios = db.GetScenarios();

            int scenarioNumber = 1;
            foreach (var scenario in ds.UniqueScenarios())
            {
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
                    
                    if (scenarioRegex != "" && ds.Scenario[i] != "")
                    {
                        s.Name = ds.Siteid[i];
                        s.ConnectionString = "ScenarioName=" + ds.Scenario[i];
                        s.SiteID = ds.Siteid[i];
                        s.Table.TableName = (ds.Siteid[i] + "_" + ds.Scenario[i]).ToLower();
                    }
                    else
                    {
                        var name = TimeSeriesDatabase.SafeTableName(Path.GetFileNameWithoutExtension(ds.Files[i]));
                        s.Name = name;
                        s.SiteID = name;
                        s.Table.TableName = name;
                        ds.Siteid[i] = name;
                    }

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
