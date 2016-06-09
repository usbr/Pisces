using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.RatingTables
{

    /// <summary>
    /// Reads Yakima Excel sheets that contain flow measurements
    /// </summary>
    public class YakimaExcelMeasurements
    {



        public static void FillTable(string filename, MeasurementsDataSet.measurementDataTable table)
        {
            var xls = new NpoiExcel(filename);
            var sheetNames = xls.SheetNames();

            if( !xls.SheetExists("summary"))
            {
                Console.WriteLine(filename + " Did not find sheet named 'summary'");
                return;
            }

            var tbl = xls.ReadDataTable("summary",true);
            Console.WriteLine(filename+" contains "+  tbl.Rows.Count+" rows ");

            tbl = CleanupTable(tbl);
        }

        private static DataTable CleanupTable(System.Data.DataTable tbl)
        {
            // use column headings from row 4 (Meas. #,DATE, ... )
            
            tbl.TableName  = GetCbtt(tbl);

            // Set column Names to match dataset
            FixColumnNames(tbl);
            // clean out junk?

            return tbl;

        }

        private static void FixColumnNames(DataTable tbl)
        {
            SetColumnName(tbl, "date_measured", "DATE");
            SetColumnName(tbl, "stage", "Gage Height");
            SetColumnName(tbl, "discharge", "Dischrg", "Dischrg. (Q)");
            SetColumnName(tbl, "quality", "Meas. Rated","Msmnt. Rated");
            SetColumnName(tbl, "party", "Made by");
            SetColumnName(tbl, "notes", "Remarks");
            
        }

        private static void SetColumnName(DataTable tbl, string newColumnName, params string[] columnNames)
        {
            var idx = FindColumnIndex(tbl, columnNames);
            if (idx < 0)
            {
                Console.WriteLine("Did not find link to column '"+newColumnName+"'");
            }
            else
            {
                tbl.Columns[idx].ColumnName = newColumnName;
            }
        }


        private static int FindColumnIndex(DataTable tbl, params string[] columnNames)
        {
            var rval = -1;

            int rowIndex = 2; // expect column names here.

            for (int i = 0; i < tbl.Columns.Count; i++)
            {
                var o = tbl.Rows[rowIndex][i];
                if (o != DBNull.Value)
                {
                    var str = o.ToString();
                    for (int j = 0; j < columnNames.Length; j++)
                    {
                        if (str.IndexOf(columnNames[j]) >= 0)
                            return i;
                    }
                }
            }
            return rval;
        }

        /// <summary>
        /// search first row for cbtt i.e. 'CBTT:  YRWW'
        /// </summary>
        /// <param name="tbl"></param>
        /// <returns></returns>
        private static string GetCbtt(DataTable tbl)
        {
            var cbtt = "";
            for (int i = 0; i < tbl.Columns.Count; i++)
            {
                object o = tbl.Rows[0][i];
                if (o != DBNull.Value)
                {
                    var str = o.ToString().ToLower();
                    if (str.IndexOf("cbtt:") >= 0) // TO Do: check for 'station' older files
                    {
                        cbtt = str.Substring(5).Trim();
                    }
                }
            }
            return cbtt;
        }
    }
}
