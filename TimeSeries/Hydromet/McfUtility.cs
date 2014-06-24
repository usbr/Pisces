using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.Core;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace Reclamation.TimeSeries.Hydromet
{
    public class McfUtility
    {


        static string[] tableNames = {"site","goes","pcode" };

        /// <summary>
        /// Loads xml version of  MCF
        /// </summary>
        /// <param name="svr"></param>
        /// <returns></returns>
        public static McfDataSet GetDataSetFromDisk( )
        {
          Performance p = new Performance();
          var ds = new McfDataSet();
          string fn = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "mcf.xml");
          ds.ReadXml(fn);
          p.Report("loaded mcf.xml");
          return ds;
        }


        /// <summary>
        /// Loads xml version of  MCF
        /// </summary>
        /// <param name="svr"></param>
        /// <returns></returns>
        public static McfDataSet GetDataSetFromCsvFiles(string path)
        {
            Performance p = new Performance();
            var ds = new McfDataSet();
          //  ds.EnforceConstraints = false;
            foreach (var item in tableNames)
            {
                string fn = Path.Combine(path, item + ".csv"); 
                if (!File.Exists(fn))
                {
                    Logger.WriteLine("Error: file missing '" + fn + "'");
                    continue;
                }
                var csv = new CsvFile(fn);

                if( item == "site")
                   FixLatLong(csv);
                // yakima has two extra columns.
                //if (item == "pcode" && csv.Columns.Contains("TAGTYPE"))
                //    csv.Columns.Remove("TAGTYPE"); 
                //if (item == "pcode" && csv.Columns.Contains("LOGORDER"))
                //    csv.Columns.Remove("LOGORDER");

                csv.TableName = item + "mcf";
                
                try
                {
                    ds.Merge(csv,true, MissingSchemaAction.Ignore);
                }
                catch (ConstraintException ce)
                {
                    Console.WriteLine("Error in table "+item +"\n "+ce.Message);
                    PrintTableErrors(csv);
                }
                
            }
            return ds;
        }
        

        private static void FixLatLong(CsvFile csv)
        {
            for (int i = 0; i < csv.Rows.Count; i++)
            {
                var r = csv.Rows[i];
                if (r["LAT"] == DBNull.Value)
                    r["LAT"] = "0";
                if (r["LONG"] == DBNull.Value)
                    r["LONG"] = "0";
            }
        }


        public static void PrintTableErrors(DataTable table)
        {
         // Test if the table has errors. If not, skip it.

        //if(table.HasErrors)
        {
            // Get an array of all rows with errors.
            var rowsInError = table.GetErrors();
            // Print the error of each column in each row.
            for(int i = 0; i < rowsInError.Length; i++)
            {
                foreach(DataColumn column in table.Columns)
                {
                    Console.WriteLine(column.ColumnName + " " + 
                        rowsInError[i].GetColumnError(column));
                }
                // Clear the row errors
                rowsInError[i].ClearErrors();
            }
        }
        }

        /// <summary>
        /// Creates XSD schema for Hydromet MCF
        /// Loads MCF csv files into strongly typed DataSet
        /// </summary>
        public static void CreateMcfDataSet(string path)
        {
            DataSet ds = new DataSet("McfDataSet");
            foreach (var item in tableNames)
            {
                string fn = Path.Combine(path, item + ".csv");
                var csv = new CsvFile(fn);
                csv.TableName = item+"mcf";
                ds.Tables.Add(csv);
            }
            ds.WriteXml(Path.Combine(path,"mcf.xml"));
            ds.WriteXmlSchema(Path.Combine(path, "mcf.xml"));

        }
    }
}
