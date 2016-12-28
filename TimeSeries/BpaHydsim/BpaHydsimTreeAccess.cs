using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.Core;
using System.Data;

namespace Reclamation.TimeSeries.BpaHydsim
{
    public class BpaHydsimTreeAccess
    {
        static AccessDB mdb;
        static string m_databasePath = "";
        int ID = 0;
        int m_parentID;
        string m_mdbFileName;
        TimeSeriesDatabaseDataSet.SeriesCatalogDataTable seriesCatalog;

        public BpaHydsimTreeAccess(string mdbFileName,
            string databaseFilename, int startingID,
            int parentID)
        {
            m_databasePath = databaseFilename;
            ID = startingID;
            m_parentID = parentID;
            m_mdbFileName = mdbFileName;
        }

        public TimeSeriesDatabaseDataSet.SeriesCatalogDataTable CreateTree()
        {
            seriesCatalog = new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();
            int bpaRoot = AddFolder(ID++, m_parentID, Path.GetFileNameWithoutExtension(m_mdbFileName));

            // read access file
            mdb = new AccessDB(m_mdbFileName);

            // get table of distinct Plant Names and Data Types to create Tree
            string sql = "SELECT DISTINCT Working_Set.PlantName, Working_Set.DataType FROM Working_Set";
            DataTable tbl = mdb.Table("Working_Set", sql);
            
            // create folder for each Plant Name and a row for each Data Type
            DataTable tblPlantNames = DataTableUtility.SelectDistinct(tbl, "PlantName");
            for (int i = 0; i < tblPlantNames.Rows.Count; i++)
            {
                string plantName = tblPlantNames.Rows[i][0].ToString().Trim();
                int siteID = AddFolder(ID++, bpaRoot, plantName);

                string[] dataTypes = DataTableUtility.Strings(tbl, "[PlantName]='" + plantName + "'", "DataType");
                for (int j = 0; j < dataTypes.Count(); j++)
                {
                    string dataType = dataTypes[j].Trim();
                    string[] dTksfd = { "ENDSTO", "ECC", "URC" };
                    if (dTksfd.Contains(dataType) == true)
                    {
                        CreateSeries(mdb, plantName, dataType, siteID);
                        CreateSeriesAF(mdb, plantName, dataType, siteID);
                    }
                    else
                        CreateSeries(mdb, plantName, dataType, siteID);
                }
            }

            return seriesCatalog;
        }

        private void CreateSeriesAF(AccessDB mdb, string plantName, string dataType, int parentID)
        {
            string units = "acre-feet";
            string name = plantName + " " + dataType + " " + units;
            string sheetName = plantName + " " + dataType;
            string cs = "FileName=" + m_mdbFileName + ";PlantName=" + plantName + ";DataType=" + dataType; // connection string
            cs = ConnectionStringUtility.MakeFileNameRelative(cs, m_databasePath);
            seriesCatalog.AddSeriesCatalogRow(ID++, parentID, 0, ID, "BpaHydsimSeriesAccess", name, sheetName, units,
                        TimeInterval.Monthly.ToString(), "Parameter", "", "BpaHydsimSeriesAccess", cs, "", "",1);
        }

        private void CreateSeries(AccessDB mdb, string plantName, string dataType, int parentID)
        {
            string[] dTcfs = { "QBPF", "QOUT", "FRCSPILL", "BYPSPILL" };
            string[] dTMW = { "AVGEN", "SYSGEN", "SYSSURP", "SYSDP" };
            string[] dTksfd = { "ENDSTO", "ECC", "URC" };
            string[] dTfeet = { "ENDELEV", "ECCFT", "URCFT" };

            string units = "";
            if (dTcfs.Contains(dataType) == true)
                units = "cfs";
            else
                if (dTMW.Contains(dataType) == true)
                    units = "aMW";
                else
                    if (dTksfd.Contains(dataType) == true)
                        units = "ksfd";
                    else
                        if (dTfeet.Contains(dataType) == true)
                            units = "feet";
                        else
                        {
                            Logger.WriteLine("Error: unsupported data type '" + dataType + "'");
                        }
            string name = plantName + " " + dataType + " " + units;
            string sheetName = plantName + " " + dataType;
            string cs = "FileName=" + m_mdbFileName + ";PlantName=" + plantName + ";DataType=" + dataType; // connection string
            cs = ConnectionStringUtility.MakeFileNameRelative(cs, m_databasePath);
            seriesCatalog.AddSeriesCatalogRow(ID++, parentID, 0, ID, "BpaHydsimSeriesAccess", name, sheetName, units, 
                        TimeInterval.Monthly.ToString(), "Parameter", "",  "BpaHydsimSeriesAccess", cs, "", "",1);
                                    
        }

        private int AddFolder(int ID, int parentID, string folderName)
        {
            DataRow dr = seriesCatalog.NewRow();
            dr["id"] = ID;
            dr["ParentID"] = parentID;
            dr["IsFolder"] = true;
            dr["Name"] = folderName;

            seriesCatalog.Rows.Add(dr);

            // I'm assuming that subitems should use the id of this record for their ParentID entries?
            return ID;
        }
    }
}
