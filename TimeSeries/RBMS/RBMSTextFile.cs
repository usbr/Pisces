using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Reclamation.Core;

namespace Reclamation.TimeSeries
{
    public class RBMSTextFile
    {

        //public static void ImportDirectory(string path,
        //    SQLTimeSeriesDatabase db)
        //{

        //    string[] files = FileUtility.GetFilesRecursive(path);
        //    for (int i = 0; i < files.Length; i++)
        //    {
        //        try
        //        {
        //            Logger.WriteLine("Parsing file " + files[i]);
        //            ImportFile(files[i], db);
        //        }
        //        catch (Exception e)
        //        {
        //            string msg = "Error reading " + files[i];
        //            Logger.WriteLine(msg, e);
        //            // System.Windows.Forms.MessageBox.Show(msg); 
        //        }
        //    }
        //    Logger.WriteLine("done importing directory ");
        //}



        private static string NiceFilename(string filename)
        {
            if (filename.Length < 20)
            {
                return filename.PadLeft(20, ' ');
            }
            return filename.Substring(filename.Length - 20);
        }
        private static int errorCount = 0;


        public static void ImportFile(string filename, TimeSeriesDatabase db)
        {
            ImportFile(filename, db, false);
        }
        public static void ImportFile(string filename, TimeSeriesDatabase db, bool manual)
        {
            DataTable rbmsDataTable = ReadRawRBMSFile(filename);
            if (rbmsDataTable.Rows.Count == 0)
            {
                Logger.WriteLine(filename + " has no valid data.  It will be skipped");
                return;
            }

            DataTable siteList = DataTableUtility.SelectDistinct(rbmsDataTable, "DH", "Riser");
            Logger.WriteLine(NiceFilename(filename) + " has " + siteList.Rows.Count + " tags ");
            for (int i = 0; i < siteList.Rows.Count; i++)
            {
                string drillHole = siteList.Rows[i]["DH"].ToString();
                string riser = siteList.Rows[i]["Riser"].ToString();

                //db.GetSeriesCatalog("
                string siteName = drillHole + "-" + riser;
                
                string sql = "DH = '" + drillHole + "' and " + "RISER = '" + riser + "'";
                DataTable table = DataTableUtility.Select(rbmsDataTable, sql, "");

                // Logger.WriteLine("Found " + table.Rows.Count + " records for " + siteName);
                if (table.Rows.Count == 0)
                {
                    continue;
                }
                DataTable dbTable = new DataTable();
                int sdi = -1;
                try
                {
                    dbTable = CreateDateValueTableForDatabase(table);

                    sdi = LookupID(db.Server, drillHole, riser,manual);
                    if (sdi < 0)
                    {
                        //Logger.WriteLine("Creating new entry for unknown tag riser='" + riser + "'  DH ='" + drillHole + "'");
                        //SeriesInfo si = db.SiteCatalog.NewSiteInfo();
                        //sdi = si.SiteDataTypeID;
                        //si.TimeInterval = TimeInterval.Hourly.ToString();
                        //si.ParentID = 0;
                        //si.Row["DrillHole"] = drillHole;
                        //si.Row["Riser"] = riser;
                        //si.Row["Area"]  ="unknown";
                        //si.Row["InstType"] = "?";
                        
                        //si.IsFolder = false;
                        //si.Name = siteName;
                        //si.TableName = siteName;
                        //si.Source = "RBMS Text File " + Path.GetFileName(filename);
                        //si.ConnectionString = "Unknown Tag:" + siteName;

                        //if (manual)
                        //{
                        //    si.Row["InstType"] = ManualInstType;
                        //    si.TableName = "M"+siteName;
                        //}

                        //db.SiteCatalog.Add(si);
                    }
                    // Check Period of record 

                    int newRows= dbTable.Rows.Count;
                    var s = db.GetSeries(sdi);
                    s.Table = dbTable;
                    db.ImportSeriesUsingTableName(s);
                    PeriodOfRecord por = s.GetPeriodOfRecord();
                   Logger.WriteLine(
                       drillHole.PadLeft(9)
                       + riser.PadLeft(4)
                       + dbTable.TableName.PadLeft(10)+" "
                       + por.T1.ToShortDateString().PadLeft(10) + " "
                   + por.T2.ToShortDateString().PadLeft(10) + " "
                   + newRows.ToString().PadLeft(5)
                   + por.Count.ToString().PadLeft(8));

                }
                catch (Exception ex)
                {
                    Logger.WriteLine("Error with siteName = '" + siteName + "'", ex);
                    //PeriodOfRecord por = db.PeriodOfRecord(sdi);
                   // Logger.WriteLine("Database period of record = " + por.T1.ToString()
                   // + " " + por.T2.ToString());
                    DateTime t1 = Convert.ToDateTime(dbTable.Rows[0][0]);
                    DateTime t2 = Convert.ToDateTime(dbTable.Rows[dbTable.Rows.Count - 1][0]);

                    Logger.WriteLine("File being imported has " + dbTable.Rows.Count +" rows");
                    Logger.WriteLine("File being imported begins " + t1.ToString());
                    Logger.WriteLine("File being imported ends   " + t2.ToString());
                    
                 //  PeriodOfRecord porInRange = db.PeriodOfRecord(sdi, t1, t2);
                  // Logger.WriteLine("database has " + porInRange.Count + " records in this same range");

                   errorCount++;
                    //string errorFilename = @"c:\temp\Error" + errorCount + ".txt";
                    //Logger.WriteLine("Saving to  " + errorFilename);
                    //TextDB.WriteToCSV(table, errorFilename, true);
                }
            }
        }

        static string ManualInstType = "M - manually read static water level";

        private static int LookupID(BasicDBServer server, string drillHole, string riser,bool manual)
        {
            string sql = "Select * from SiteCatalog where "
            + " DrillHole ='" + drillHole + "' and Riser = '" + riser + "'";

            if(! manual)
              sql += " and InstType <> '"+ManualInstType +"'";// M - manually read static water level'";
            else
                sql += " and InstType = '"+ManualInstType +"'";// M - manually read static water level'";


            DataTable tbl = server.Table("SiteCatalog", sql);
            if (tbl.Rows.Count == 1)
            {
                return Convert.ToInt32(tbl.Rows[0]["SiteDataTypeID"]);
            }
            if (tbl.Rows.Count > 1)
            {
                Logger.WriteLine("Warning: there were "+tbl.Rows.Count +" rows "
                + " found for drillhole = '"+drillHole + "' riser = '"+riser+"'");
            }
            return -1;
        }

        /// <summary>
        /// Format data to be consistent with database 
        /// </summary>
        private static DataTable CreateDateValueTableForDatabase(DataTable table)
        {
            DataTable dbTable = new DataTable();
            dbTable.Columns.Add("DateTime", typeof(DateTime));
            dbTable.PrimaryKey = new DataColumn[] { dbTable.Columns[0] };
            dbTable.Columns.Add("Value", typeof(double));
            bool hasTimeColumn = (table.Columns.IndexOf("Time") >= 0);
            string prevStrDate = Guid.NewGuid().ToString();

            for (int i = 0; i < table.Rows.Count; i++)
            {
                string strDate = table.Rows[i]["Date"].ToString();
                if (hasTimeColumn)
                {
                    strDate += " " + table.Rows[i]["Time"].ToString();
                }

                if (strDate == prevStrDate)
                {
                    Logger.WriteLine("Skipping duplicate date " + strDate);
                    continue;
                }
                prevStrDate = strDate;
                DateTime d = DateTime.MinValue;
                if (DateTime.TryParse(strDate, out d))
                {
                    //DateTime d = DateTime.Parse(strDate);
                    DataRow row = dbTable.NewRow();
                    row["DateTime"] = d;
                    row["Value"] = Convert.ToDouble(table.Rows[i]["Measured"]);
                    if (dbTable.Rows.Find(d) != null)
                    {
                        Logger.WriteLine("duplicate date '" + d.ToString() + "' is being skipped ");
                    }
                    else
                    {
                        dbTable.Rows.Add(row);
                    }
                }
                else
                {
                    Logger.WriteLine("Error reading Date " + strDate);
                }
            }
            return dbTable;
        }

        /// <summary>
        /// Reads Raw RBMS Text file. Adds DateTime and Value Columns
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static DataTable ReadRawRBMSFile(string filename)
        {
            TextFile tf = new TextFile(filename);
            int fieldCount = 0;
            if (tf.FileData.Length > 0)
            {
                fieldCount = tf[0].Split(',').Length;

                if (fieldCount < 4 || fieldCount > 5)
                {
                   throw new Exception("Error: " + filename + "is not a valid RBMS File. It should have 4 or 5 columns of data");
                }
            }
            else
            {
                throw new Exception("Error: There was not data in the input file");
            }
            tf = null;
            if (fieldCount == 5)
            {

                string[] dataTypes = new string[] { "String", "String", "String", "String", "String" };
                string[] fieldNames = new string[] { "DH", "Riser", "Date", "Time", "Measured" };
                //DH-438-RS   ,     1,01-apr-2000,00:00:54     ,    870.290
                string pattern = @"\s*[A-Za-z\-0-9]{3,}\s*,\s*\d+,\d{1,2}-[A-Za-z]{3}-\d{4},\d{2}:\d{2}:\d{2}\s*,\s*[0-9\.\-\+Ee]+";
                CsvFile raw = new CsvFile(filename, dataTypes, fieldNames, pattern);
                return raw;
            }
            else
            {
                string[] dataTypes = new string[] { "String", "String", "String", "String" };
                string[] fieldNames = new string[] { "DH", "Riser", "Date", "Measured" };
                //DH-259-RS   ,     1,01-apr-1991 00:00:32     ,    866.960
                string pattern = @"\s*[A-Za-z\-0-9]{3,}\s*,\s*\d+,\d{1,2}-[A-Za-z]{3}-\d{4}\s\d{2}:\d{2}(:\d{2})?\s*,\s*[0-9\.\-\+Ee]+";

                CsvFile raw = new CsvFile(filename, dataTypes, fieldNames, pattern);
                return raw;
            }

            // format with date time together.
            //DH-259-RS   ,     1,01-apr-1991 00:00:32     ,    866.960
            // format with date time  csv.
            //DH-438-RS   ,     1,01-apr-2000,00:00:54     ,    870.290

            //Console.WriteLine(db.Rows.Count);
            // return raw;
        }

    }
}
