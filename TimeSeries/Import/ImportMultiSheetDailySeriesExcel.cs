using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;

namespace Reclamation.TimeSeries.Import
{
    /// <summary>
    /// Imports a single spreasheet into a Series.
    /// The tabs (sheets) are labeled as years, and within
    /// each sheet are 13 columns (Day,Jan,Feb,....Dec)
    /// </summary>
    public class ImportMultiSheetDailySeriesExcel
    {

        public static Series ImportSpreadsheet(string fileName)
        {
            Console.WriteLine("Reading "+fileName);
            Series s = new Series("series1", TimeInterval.Daily);

            var xls = new NpoiExcel(fileName);
            var sheetNames = xls.SheetNames();

            //ExcelUtility xls = new ExcelUtility(fileName);
            //var sheetNames = ExcelUtility.SheetNames(fileName);

            foreach (string sheet in sheetNames)
            {
                if (Regex.IsMatch(sheet.Trim(), "[0-9]{4}")) // is 4 digit year
                {
                    int yr = int.Parse(sheet);
                    Console.WriteLine("Reading sheet:"+sheet);
                    //var tbl = ExcelUtility.Read(fileName, sheet, false);
                    var tbl = xls.ReadDataTable(sheet, false, true);
                    ReadTable(s, yr, tbl);
                }
            }
            return s;
        }

        private static void ReadTable(Series s, int yr, DataTable tbl)
        {
            // look in column 1 to find 'Day'
            int idxDay = FindRowIndex(tbl,0,"DAY");
            if (idxDay < 0)
                return;

            for (int m = 1; m <= 12; m++)
            {
                DateTime t = new DateTime(yr,m, 1);

                // Check Column head is the right month
                if (tbl.Rows[idxDay][m].ToString().ToLower() != t.ToString("MMM").ToLower())
                {
                    Console.WriteLine("oops");
                    continue;
                }

                while( t.Month == m)
                {
                    int idx = idxDay + 1+t.Day;
                    if( idx >= tbl.Rows.Count || idx <0)
                    {
                        Console.WriteLine("Error with index"); 
                    }
                    var row = tbl.Rows[idx];
                    var x = row[m ].ToString();
                    double val;

                    // check the day is correct
                    var day =0;
                    if (int.TryParse(row[0].ToString(), out day))
                    {
                        if (double.TryParse(x, out val))
                        {
                            s.Add(t, val);
                        }
                    }
                    t = t.AddDays(1);
                }
                
            }
        }

        private static int FindRowIndex(DataTable tbl, int colIndex, string find)
        {
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                if (colIndex >= tbl.Columns.Count)
                    return -1;
                if (tbl.Rows[i][colIndex].ToString() == find)
                    return i;
            }

            return -1;
        }
    }
}
