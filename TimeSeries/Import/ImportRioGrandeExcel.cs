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
    /// The tabs (sheets) are years, and within
    /// each sheet are 12 columns 
    /// </summary>
    public class ImportRioGrandeExcel
    {

        public static Series ImportSpreadsheet(string fileName)
        {
            Series s = new Series("series1", TimeInterval.Daily);

            var ds = ExcelUtility.GetWorkbookReference(fileName);

            for (int i = 0; i < ds.Tables.Count; i++)
            {
                var tbl = ds.Tables[i];
                string tn = tbl.TableName.Trim();
                if (Regex.IsMatch(tn, "[0-9]{4}")) // is 4 digit year
                {
                    int yr = int.Parse(tn);

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
                    var x = tbl.Rows[idxDay + 1+t.Day][m ].ToString();
                    double val;
                    if( double.TryParse(x,out val))
                    {
                        s.Add(t, val);
                    }
                    t = t.AddDays(1);
                }
                
            }
        }

        private static int FindRowIndex(DataTable tbl, int colIndex, string find)
        {
            for (int i = 0; i < tbl.Rows.Count; i++)
            {

                if (tbl.Rows[i][colIndex].ToString() == find)
                    return i;
            }

            return -1;
        }
    }
}
