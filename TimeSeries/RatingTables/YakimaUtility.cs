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

    
    public class YakimaUtility
    {
       
  
        /// <summary>
        /// Reads Yakima Excel sheets that contain flow measurements
        /// </summary>
        public static void FillMeasurementTable(string filename, HydrographyDataSet.measurementDataTable table)
        {
            var xls = new NpoiExcel(filename);

            var cbtt = GetCbtt(filename);
            
            
            var sheetName = GetSheetName( xls);

            if (sheetName == "")
            {
                Console.WriteLine( "Did not find a sheet to read from "+Path.GetFileNameWithoutExtension(filename));
                return;
            }
            else
            {
               Logger.WriteLine("Reading from sheet "+sheetName);
            }

            var tbl = xls.ReadDataTable(sheetName,false,true);
            Logger.WriteLine(filename+" contains "+  tbl.Rows.Count+" rows ");

            
            
            tbl.TableName = Path.GetFileNameWithoutExtension(filename);

            if (tbl.TableName == "")
                tbl.TableName = cbtt;

            FixColumnNames(tbl);

            // load data into measurement table
            if (tbl.Columns.Contains("stage") && tbl.Columns.Contains("discharge"))
            {
                for (int i = 0; i < tbl.Rows.Count; i++)
                {
                    var newRow = table.NewmeasurementRow();

                    string siteid = cbtt;
                    DateTime? date_measured = TryGetDateTime(tbl.Rows[i], "date_measured");
                    double? stage = TryGetDouble(tbl.Rows[i], "stage");
                    double? discharge = TryGetDouble(tbl.Rows[i], "discharge");
                    string quality = TryGetString(tbl.Rows[i], "quality");
                    string party = TryGetString(tbl.Rows[i], "party");
                    string notes = TryGetString(tbl.Rows[i], "notes");

                    if (stage.HasValue && discharge.HasValue)
                        table.AddmeasurementRow(table.NextID(), siteid, date_measured.GetValueOrDefault(), stage.Value,
                            discharge.GetValueOrDefault(), quality, party, notes);
                }

            }

        }
        private static string TryGetString(DataRow dataRow, string colName)
        {
            string rval = "";

            if (!dataRow.Table.Columns.Contains(colName))
                return rval;

            return dataRow[colName].ToString().Replace("\n"," ").Replace("\r"," ");
            
        }

        private static DateTime? TryGetDateTime(DataRow dataRow, string colName)
        {
            DateTime? rval = null;

            if (!dataRow.Table.Columns.Contains(colName))
                return rval;

            var o = dataRow[colName];
            DateTime d;

            if (DateTime.TryParse(o.ToString(), out d))
            {
                rval = d;
            }
            return rval;
        }

        private static double? TryGetDouble(DataRow dataRow, string colName)
        {
            double? rval = null;
            if (!dataRow.Table.Columns.Contains(colName))
                return rval;
            var o = dataRow[colName];
            double d = 0;

            if( double.TryParse(o.ToString(),out d))
            {
                rval = d;
            }

            return rval;
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


        private static void FixColumnNames(DataTable tbl)
        {
            SetColumnName(tbl, "date_measured", "DATE","Date");
            SetColumnName(tbl, "stage", "Gage Height","Gage");
            SetColumnName(tbl, "discharge", "Dischrg", "Dischrg. (Q)","Discharg. (Q)");
            SetColumnName(tbl,"quality", "Meas. Rated","Msmnt. Rated","Rated");
            SetColumnName(tbl, "party", "Made by","Made");
            SetColumnName(tbl, "notes", "Remarks");
            
        }

        private static void SetColumnName(DataTable tbl, string newColumnName, params string[] columnNames)
        {
            var idx = FindColumnIndex(tbl, columnNames);
            if (idx < 0)
            {
                Console.WriteLine(tbl.TableName +": Did not find link to column '"+newColumnName+"'");
                if (newColumnName == "stage")
                    Console.Write("");
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

         
    }
}
