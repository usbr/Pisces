using System;
using System.Data;
using Reclamation.Core;
using System.IO;
using System.Configuration;
//using Math = System.Math;

namespace Reclamation.TimeSeries.RatingTables
{
    public class RatingTableUtility
    {

        /// <summary>
        /// Find path to rating table as a local file.  If app.config
        /// has RatingTablePath as a url download and return the path to the temporay file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fn"></param>
        /// <returns></returns>
        public static string GetRatingTableAsLocalFile(string fileName)
        {

            if (!File.Exists(fileName))
                fileName = Path.Combine(ConfigurationManager.AppSettings["RatingTablePath"], fileName);
            if (fileName.ToLower().StartsWith("http"))
            {
                var fn_tmp = FileUtility.GetTempFileName(".csv");
                Web.GetFile(fileName, fn_tmp);
                fileName = fn_tmp;
            }
            return fileName;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullRatingTable">table with two columns (gh,q)</param>
        /// <param name="title"></param>
        /// <param name="htmlFile"></param>
        public static void WriteHtmlFile(DataTable fullRatingTable, string title,string htmlFile)
        {
            var tbl = ConvertToMultiColumn(fullRatingTable);
            var s = DataTableOutput.ToHTML(tbl, FormatCell , true,title);
            File.WriteAllText(htmlFile, s);
        }

        private static string FormatCell(DataColumn c, DataRow r, string txt)
        {
            double d;
            if (Double.TryParse(r[c].ToString(),out d))
            {
                d = SetSigFigs(d, 3);
                return "<td>" + d.ToString() + "</td>";
            }

            return "<td>" + txt + "</td>";
        }

        
        /// <summary>
        /// http://stackoverflow.com/questions/374316/round-a-double-to-x-significant-figures
        /// </summary>
        /// <returns></returns>
        private static double SetSigFigs(double d, int digits)
        {
            double scale = System.Math.Pow(10, System.Math.Floor(System.Math.Log10(System.Math.Abs(d))) + 1);
            return scale * System.Math.Round(d / scale, digits);
        }


        /// <summary>
        /// Convert two column table (gh,q) to 10 column with each hundredth (0.01) as column
        /// and each row as tenth (0.1)
        /// </summary>
        /// <param name="dTbl"></param>
        /// <returns></returns>
        public static DataTable ConvertToMultiColumn(DataTable dTbl)
        {
                        
            DataTable rval = new DataTable("RatingTable");
            rval.Columns.Add(dTbl.Columns[0].ColumnName);
            rval.Columns.Add("0.00");
            rval.Columns.Add("0.01");
            rval.Columns.Add("0.02");
            rval.Columns.Add("0.03");
            rval.Columns.Add("0.04");
            rval.Columns.Add("0.05");
            rval.Columns.Add("0.06");
            rval.Columns.Add("0.07");
            rval.Columns.Add("0.08");
            rval.Columns.Add("0.09");

            rval.PrimaryKey = new DataColumn[] { rval.Columns[dTbl.Columns[0].ColumnName] };
            if (dTbl.Rows.Count == 0)
                return rval;


            for (int i = 0; i < dTbl.Rows.Count; i++)
            {
                var x = Convert.ToDouble(dTbl.Rows[i][0]);
                var y = Convert.ToDouble(dTbl.Rows[i][1]);
                AddToMultiColumnTable(rval, x, y);
            }

            return rval;
            
        }

        /// <summary>
        /// Add value to multi column table, insert rows as needed
        /// </summary>
        /// <param name="rval"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private static void AddToMultiColumnTable(DataTable rval, double x, double y)
        {
            x = System.Math.Round(x, 2);// example 32.63
            var TruncateToTenth = System.Math.Truncate(x * 10.0) / 10.0;

            var hundrethColumn = x - System.Math.Truncate(x * 10.0) / 10;

            if (hundrethColumn < 0)
                hundrethColumn = System.Math.Abs(hundrethColumn);

            var rows = rval.Select(rval.Columns[0].ColumnName + " = '" + TruncateToTenth.ToString("F2") + "'");
            DataRow row ;
            if (rows.Length == 0)
            {
                row = rval.NewRow();
                row[0] = TruncateToTenth.ToString("F2");
                rval.Rows.Add(row);
            }
            else
            {
                row = rows[0];
            }
            var colName = hundrethColumn.ToString("F2");
            row[colName] = y;
        }


    }
}


