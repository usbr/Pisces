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
            var tbl = xls.ReadDataTable("summary");
            Console.WriteLine(tbl.Rows.Count);

            tbl = CleanupTable(tbl);
        }

        private static DataTable CleanupTable(System.Data.DataTable tbl)
        {
            // use column headings from row 4 (Meas. #,DATE, ... )
            
            tbl.TableName  = GetCbtt(tbl);

            // Set column Names to match dataset
            FixColumnNames(tbl);


            return tbl;

        }

        private static void FixColumnNames(DataTable tbl)
        {
            SetColumnName(tbl, "date_measured", "DATE");
            SetColumnName(tbl, "stage", "Gage Height");
            SetColumnName(tbl, "discharge", "Dischrg", "Dischrg. (Q)");
            SetColumnName(tbl, "quality", "Meas. Rated","Msmnt. Rated");
            SetColumnName(tbl, "party", "Made by");
            
        }

        private static void SetColumnName(DataTable tbl, string newColumnName, params string[] columnNames)
        {
            var dateIndex = FindColumnIndex(tbl, columnNames);
            tbl.Columns[dateIndex].ColumnName = newColumnName;
        }


        private static int FindColumnIndex(DataTable tbl, params string[] columnNames)
        {
            var rval = -1;

            int rowIndex = 3; // expect column names here.

            for (int i = 0; i < tbl.Columns.Count; i++)
            {
                var o = tbl.Rows[rowIndex][i];
                if (o != DBNull.Value)
                {
                    var str = o.ToString();

                    
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
