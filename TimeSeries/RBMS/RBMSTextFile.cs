using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Reclamation.Core;

namespace Reclamation.TimeSeries.RBMS
{
    public class RBMSTextFile
    {

        public static void ImportDirectory(string path,
            TimeSeriesDatabase db)
        {

            string[] files = FileUtility.GetFilesRecursive(path);
            for (int i = 0; i < files.Length; i++)
            {
                try
                {
                    Logger.WriteLine("Parsing file " + files[i]);
                    ImportFile(files[i], db,true);
                }
                catch (Exception e)
                {
                    string msg = "Error reading " + files[i];
                    Logger.WriteLine(msg + e.Message, "ui");
                    // System.Windows.Forms.MessageBox.Show(msg); 
                }
            }
            Logger.WriteLine("done importing directory ");
        }




        public static void ImportFile(string filename, TimeSeriesDatabase db)
        {
            ImportFile(filename, db, false);
        }

        //static string ManualInstType = "M - manually read static water level";

        public static void ImportFile(string filename, TimeSeriesDatabase db, bool manual)
        {
            if( !manual)
                throw new NotImplementedException("this method is for importing manually entered data");
            DataTable rbmsDataTable = ReadRBMSFile(filename);

            if (rbmsDataTable.Rows.Count == 0)
            {
                Logger.WriteLine(filename + " has no valid data.  It will be skipped");
                return;
            }

            string sql = @"     select a.id,a.tablename,a.name, b.value as InstType , c.value as DrillHole, d.value as riser 
   from seriescatalog a   
   left join seriesproperties b on ( b.seriesid=a.id and b.name='InstType')
   left join seriesproperties c on ( c.seriesid=a.id and c.name='DrillHole')
   left join seriesproperties d on ( d.seriesid=a.id and d.name='Riser')

where b.value = 'M - manually read static water level' ";
            var sc = db.Server.Table("view_seriescatalog",sql);
            db.SuspendTreeUpdates();
            int skipcount = 0;
            int addCount = 0;
            var tableList = new List<string>();
            for (int i = 0; i < rbmsDataTable.Rows.Count; i++)
            {
                string drillHole = rbmsDataTable.Rows[i]["DH"].ToString();
                string riser = rbmsDataTable.Rows[i]["Riser"].ToString();
                var strDate = rbmsDataTable.Rows[i]["DateTime"].ToString();
                var t = DateTime.Now.Date;
                var filter = "DrillHole ='" + drillHole + "'" + " and  " + " riser = '" + riser + "'";

                if (!DateTime.TryParse(strDate, out t))
                {
                    Logger.WriteLine("Error: could not parse " + strDate);
                    Logger.WriteLine(filter);
                    continue;
                }

                var strVal = rbmsDataTable.Rows[i]["Measured"].ToString();
                

                double val = -999;
                if( !double.TryParse(strVal,out val))
                {
                    Logger.WriteLine("Error: could not parse " + strVal);
                    Logger.WriteLine(filter);
                    continue;
                }

                var rows = sc.Select(filter);

                if (rows.Length != 1)
                {
                    Logger.WriteLine("Warning:  Matched "+rows.Length +" records skipping: "+filter);
                    skipcount++;
                    continue;
                }
                //int id = Convert.ToInt32(sc.Rows[0]["id"]);
                int id = Convert.ToInt32(rows[0]["id"]);
                var s = db.Factory.GetSeries(id);
                if( !tableList.Contains(s.Table.TableName))
                   tableList.Add(s.Table.TableName);
                string fn = Path.GetFileName(filename);
                if (fn.Length > 50)
                    fn = fn.Substring(0, 50);
            s.Add(t, val, fn);
                db.SaveTimeSeriesTable(id, s, DatabaseSaveOptions.UpdateExisting);
                addCount++;
            }
            db.ResumeTreeUpdates();
            Logger.WriteLine("finished importing " + filename);
            Logger.WriteLine("skipped " + skipcount + " records.");
            Logger.WriteLine("added/updated " + addCount + " records.");
            Logger.WriteLine(tableList.Count +" tables were modified");
            for (int i = 0; i < tableList.Count; i++)
            {
                Console.WriteLine(tableList[i]);
            }
        }


        //private static int LookupID(BasicDBServer server, string drillHole, string riser,bool manual)
        //{
        //    string sql = "Select * from SiteCatalog where "
        //    + " DrillHole ='" + drillHole + "' and Riser = '" + riser + "'";

        //    if(! manual)
        //      sql += " and InstType <> '"+ManualInstType +"'";// M - manually read static water level'";
        //    else
        //        sql += " and InstType = '"+ManualInstType +"'";// M - manually read static water level'";


        //    DataTable tbl = server.Table("SiteCatalog", sql);
        //    if (tbl.Rows.Count == 1)
        //    {
        //        return Convert.ToInt32(tbl.Rows[0]["SiteDataTypeID"]);
        //    }
        //    if (tbl.Rows.Count > 1)
        //    {
        //        Logger.WriteLine("Warning: there were "+tbl.Rows.Count +" rows "
        //        + " found for drillhole = '"+drillHole + "' riser = '"+riser+"'");
        //    }
        //    return -1;
        //}

        /// <summary>
        /// Format data to be consistent with database 
        /// </summary>
        private static DataTable CreateDateValueTableForDatabase(DataTable table)
        {
            table.Columns.Add("DateTime", typeof(DateTime));
            bool hasTimeColumn = (table.Columns.IndexOf("Time") >= 0);

            for (int i = 0; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];
                string strDate = table.Rows[i]["Date"].ToString();
                if (hasTimeColumn)
                {
                    strDate += " " + table.Rows[i]["Time"].ToString();
                }

                DateTime d = DateTime.MinValue;
                if (DateTime.TryParse(strDate, out d))
                {
                    row["DateTime"] = d;
                }
                else
                {
                    Logger.WriteLine("Error reading Date " + strDate);
                }
            }
            return table;
        }

        /// <summary>
        /// Reads Raw RBMS Text file. Adds DateTime and Value Columns
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private static DataTable ReadRBMSFile(string filename)
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
            CsvFile raw = null;
            if (fieldCount == 5)
            {

                string[] dataTypes = new string[] { "String", "String", "String", "String", "String" };
                string[] fieldNames = new string[] { "DH", "Riser", "Date", "Time", "Measured" };
                //DH-438-RS   ,     1,01-apr-2000,00:00:54     ,    870.290
                //string pattern = @"\s*[A-Za-z\-0-9]{3,}\s*,\s*\d+,\d{1,2}-[A-Za-z]{3}-\d{4},\d{2}:\d{2}:\d{2}\s*,\s*[0-9\.\-\+Ee]+";
                raw = new CsvFile(filename, dataTypes, fieldNames);
            }
            else
            {
                string[] dataTypes = new string[] { "String", "String", "String", "String" };
                string[] fieldNames = new string[] { "DH", "Riser", "Date", "Measured" };
                //DH-259-RS   ,     1,01-apr-1991 00:00:32     ,    866.960
                //string pattern = @"\s*[A-Za-z\-0-9]{3,}\s*,\s*\d+,\d{1,2}-[A-Za-z]{3}-\d{4}\s\d{2}:\d{2}(:\d{2})?\s*,\s*[0-9\.\-\+Ee]+";
                raw = new CsvFile(filename, dataTypes, fieldNames);
            }

            return CreateDateValueTableForDatabase(raw);

            // format with date time together.
            //DH-259-RS   ,     1,01-apr-1991 00:00:32     ,    866.960
            // format with date time  csv.
            //DH-438-RS   ,     1,01-apr-2000,00:00:54     ,    870.290

            //Console.WriteLine(db.Rows.Count);
            // return raw;
        }

    }
}
