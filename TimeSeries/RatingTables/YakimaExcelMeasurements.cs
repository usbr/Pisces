using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

            var cbtt = GetCbtt(filename);
            
            if(cbtt.ToLower() =="bum" )
            {
                Console.WriteLine("skipping reservoir");
            }

            var sheetName = GetSheetName( xls);

            if (sheetName == "")
            {
                Console.WriteLine("Did not find a sheet to read from");
                return;
            }
            else
            {
                Console.WriteLine("Reading from sheet "+sheetName);
            }

            var tbl = xls.ReadDataTable(sheetName,false,true);
            Console.WriteLine(filename+" contains "+  tbl.Rows.Count+" rows ");

            tbl = CleanupTable(tbl);
        }

        private static string GetSheetName( NpoiExcel xls)
        {
            string sheetName = "summary";
            if (!xls.SheetExists(sheetName))
            {
                foreach (var item in xls.SheetNames())
                {
                    if (item.ToLower().IndexOf("summary") == 0) //SUMMARY SHEET-EASW-WY98
                      return item;
                }

                if (xls.SheetExists("sheet1"))
                {
                
                    sheetName = "sheet1";
                }
                else
                {
                    sheetName = "";
                }
            }

            return sheetName;
        }

        private static string GetCbtt(string filename)
        {
            var estimateCBTT = Path.GetFileNameWithoutExtension(filename);
            // trim out numbers
            estimateCBTT = Regex.Replace(estimateCBTT,"[0-9]{2}","");
            return estimateCBTT;
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
            SetColumnName(tbl, "stage", "Gage Height","Gage");
            SetColumnName(tbl, "discharge", "Dischrg", "Dischrg. (Q)");
            SetColumnName(tbl, "quality", "Meas. Rated","Msmnt. Rated");
            SetColumnName(tbl, "party", "Made by","Made");
            SetColumnName(tbl, "notes", "Remarks");
            
        }

        private static void SetColumnName(DataTable tbl, string newColumnName, params string[] columnNames)
        {
            var idx = FindColumnIndex(tbl, columnNames);
            if (idx < 0)
            {
                Console.WriteLine("Did not find link to column '"+newColumnName+"'");
                if( newColumnName == "stage")
                    Console.WriteLine();
            }
            else
            {
                tbl.Columns[idx].ColumnName = newColumnName;
            }
        }


        private static int FindColumnIndex(DataTable tbl, params string[] columnNames)
        {
            int rval = -1;
            for (int rowToSearch = 0; rowToSearch < 7; rowToSearch++)
            {
                var x = SearchForColumnIndex(tbl, columnNames, rowToSearch);
                if( x >=0 )
                {
                    rval = x;
                    break;
                }
            }
            return rval;
        }

        private static int SearchForColumnIndex(DataTable tbl, string[] columnNames, int rowIndex)
        {
            int rval = -1;
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
