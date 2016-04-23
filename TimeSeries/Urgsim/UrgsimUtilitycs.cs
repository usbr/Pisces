using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Reclamation.Core;
using System.Data;
using System.IO;

namespace Reclamation.TimeSeries.Urgsim
{
    public class UrgsimUtilitycs
    {

        internal static string UrgsimPath = ConfigurationManager.AppSettings["UrgsimPath"];


        static CsvFile s_csv = null;
        public static DataTable GetOperationsModelMatrix()
        {
            if (s_csv == null)
            {
                var lines = Web.GetPage(UrgsimPath + "modelmatrix.csv", true);
                var fn = FileUtility.GetTempFileName(".csv");
                File.WriteAllLines(fn, lines);
                s_csv = new CsvFile(fn, CsvFile.FieldTypes.AllText);
            }
            return s_csv;
        }

        public static string[] OperationModels()
        {
            var rval = new List<string>();
            var csv = GetOperationsModelMatrix();
            for (int r = 0; r < csv.Rows.Count; r++)
            {
                for (int c = 1; c < csv.Columns.Count; c++)
                {
                    var o = csv.Rows[r][c];
                    if (o != DBNull.Value && o.ToString() != "")
                        rval.Add(o.ToString());
                }
            }

            return rval.ToArray();

        }
        public static void CreateTree(TimeSeriesDatabase DB, PiscesFolder selectedFolder)
        {

            string[] variablesFile = Web.GetPage(UrgsimPath + "WWCRA_variables.csv", true);

            // build Pisces tree
            var root = DB.AddFolder(selectedFolder, "URGSiM");
            var sc = DB.GetSeriesCatalog();
            var sr = DB.GetNewSeriesRow();
            int id = sr.id;

            string prevFolderName = "";
            int folderID = root.ID;

            for (int j = 1; j < variablesFile.Length; j++)
            {//add urgsim variables to pisces tree as urgsim series
                string[] tokens = variablesFile[j].Trim().Split(',');
                if (tokens.Length < 2)
                {
                    continue;
                }
                
                string piscesFolderName = tokens[0];
                string variableName = tokens[1];

                string dataFolderName = "rawflows";
                
                if (piscesFolderName.StartsWith("input-") )
                {
                    dataFolderName = "inputdata";
                }

                var s = new Urgsim.UrgsimSeries(UrgsimPath, dataFolderName+"/bccr_bcm2_0.1.sresa1b", variableName);
                s.TimeInterval = TimeInterval.Monthly;

                if (prevFolderName != piscesFolderName)
                {// create new folder.
                    sc.AddFolder(piscesFolderName, id, root.ID);
                    folderID = id;
                    id++;
                    prevFolderName = piscesFolderName;
                }

                sc.AddSeriesCatalogRow(s, id, folderID, "");
                id++;
            }

            DB.Server.SaveTable(sc);

        }


        /// <summary>
        /// build scenario table in Pisces database 
        /// </summary>
        /// <param name="DB"></param>
        public static void LoadScenarioTable(TimeSeriesDatabase DB)
        {
            string[] projectionsFile = Web.GetPage(UrgsimPath + "runindex.csv", true);
            DB.ClearScenarios();
            var scenarioTable = DB.GetScenarios();
            bool selected = true;
            for (int k = 1; k < projectionsFile.Length - 1; k++)
            {//load scenario picker
                string[] tokens = projectionsFile[k].Split(',');
                string projectionName = tokens[1];
                if (k > 1)
                    selected = false;
                foreach (string om in OperationModels())
                {
                    scenarioTable.AddScenarioRow(om + "/" + projectionName, selected, UrgsimPath, 0, false);
                }
            }
            DB.Server.SaveTable(scenarioTable);

            // build xml file for reference builder from scenario table
            scenarioTable.WriteLocalXml();
        }


    }
}