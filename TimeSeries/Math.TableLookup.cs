using System;
using System.Data;
using System.IO;
using Reclamation.Core;
using Reclamation.TimeSeries.Parser;

namespace Reclamation.TimeSeries
{
    public partial class Math
    {

        [FunctionAttribute("Performs simple Rating table lookup from a file",
            "FileRatingTable(series1,\"table.csv\")")]
        public static Series FileRatingTable(Series s, string fileName)
        {

            var rval = TimeSeriesDatabaseDataSet.RatingTableDataTable.ComputeSeries(s, fileName);
            rval.RemoveMissing();
            return rval;
        }

        [FunctionAttribute("Performs Rating table interpolation from a file",
            "FileRatingTableInterpolate(series1,\"table.csv\")")]
        public static Series FileRatingTableInterpolate(Series s, string fileName)
        {
            var rval = TimeSeriesDatabaseDataSet.RatingTableDataTable.ComputeSeries(s, fileName, InterpolationMethod.Linear);
            rval.RemoveMissing();
            return rval;
        }

        [FunctionAttribute("Performs log-log Rating table interpolation from a file",
            "FileRatingTableLogLogInterpolate(series1,\"table.csv\")")]
        public static Series FileRatingTableLogLogInterpolate(Series s, string fileName)
        {
            var rval = TimeSeriesDatabaseDataSet.RatingTableDataTable.ComputeSeries(s, fileName, InterpolationMethod.LogLog);
            rval.RemoveMissing();
            return rval;
        }


        [FunctionAttribute("Performs 2D lookup from a file with linear interpolation in both directions",
            "FileLookupInterpolate2D(series_row, series_column, csvFile)")]
        public static Series FileLookupInterpolate2D(Series s1, Series s2, string fileName)
        {
            var rval = new Series();

            if (s1.IsEmpty || s2.IsEmpty)
            {
                Logger.WriteLine("FileLookupInterpolate2D - input series empty");
                return rval;
            }

            var fn = fileName;

            if (!File.Exists(fn))
                fn = Path.Combine(Path.Combine(Globals.LocalConfigurationDataPath, "rating_tables"), fileName);

            if (!File.Exists(fn))
            {
                Logger.WriteLine("FileLookupInterpolate2D - input fileName, file not found");
                return rval;
            }

            CsvFile csv = new CsvFile(fn, CsvFile.FieldTypes.AllText);
            foreach (var pt in s1)
            {
                Point point = pt;

                var s2_idx = s2.IndexOf(pt.DateTime);
                if (s2_idx < 0)
                {
                    point.Value = Point.MissingValueFlag;
                }
                else
                {
                    point.Value = Interpoloate2D(csv, pt.Value, s2[s2_idx].Value);
                }
                rval.Add(point);
            }

            return rval;
        }


        /// <summary>
        /// https://en.wikipedia.org/wiki/Bilinear_interpolation
        /// </summary>
        /// <param name="tbl"></param>
        /// <param name="rowValue"></param>
        /// <param name="columnValue"></param>
        /// <returns></returns>
        public static double Interpoloate2D(DataTable tbl, double rowValue, double columnValue)
        {
            int row = RowIndexValue(tbl, rowValue);
            int col = ColumnIndexValue(tbl, columnValue);

            string errmsg = "Error: Interpolate2D() row = " + row + " col = " + col;
            if (row < 0 || col < 1)
            {
                Logger.WriteLine(errmsg);
                return Point.MissingValueFlag;
            }

            // if first row or first column do linear interpolation
            if (row == 0)
            {
                return InterpolateRow(tbl, row, columnValue);
            }

            if (col == 1)
            {
                double val = InterpolateColumn(tbl, col, rowValue);
                if (val < 0)
                {
                    Logger.WriteLine(errmsg);
                    Logger.WriteLine("row value: " + rowValue + " outside of table rows");
                    return Point.MissingValueFlag;
                }
                else
                    return val;
            }

            /*
             * Bilinear Interpolation Equation
             *   where: rowValue = y
             *          columnValue = x
             *          row = y2
             *          col = x2
             * 
             *        x1   |  x   |	 x2
             * -------------------------
               y1  |  Q11  |      |  Q21
             * -------------------------
               y   |         rval	
             * -------------------------
               y2  |  Q12  |      |  Q22
             * -------------------------
             * 
             *            (x2 - x)(y2 - y)            (x - x1)(y2 - y)
             *  rval ~=  ------------------ * Q11  +  ----------------- * Q21
             *            (x2 - x1)(y2 - y1)          (x2 - x1)(y2 - y1)
             * 
             *            (x2 - x)(y - y1)            (x - x1)(y - y1)
             *          + ----------------- * Q12  +  ----------------- * Q22
             *            (x2 - x1)(y2 - y1)          (x2 - x1)(y2 - y1)
             */

            double y = rowValue;
            double y1 = Convert.ToDouble(tbl.Rows[row - 1][0]);
            double y2 = Convert.ToDouble(tbl.Rows[row][0]);
            
            // out of bounds, interpolation unknown
            if (y > y2)
            {
                Logger.WriteLine(errmsg);
                Logger.WriteLine("value outside of table rows");
                return Point.MissingValueFlag;
            }

            double x = columnValue;
            double x1 = Convert.ToDouble(tbl.Columns[col - 1].ColumnName);
            double x2 = Convert.ToDouble(tbl.Columns[col].ColumnName);

            // out of bounds, using last column of data for interpolation
            if (x > x2)
            {
                Logger.WriteLine("Interpolate2D() using last column of data " +
                                 "for interpolation, value provided greater" +
                                 "than column max");
                x = x2;
            }

            double Q11 = ValueAt(tbl, row - 1, col - 1);
            double Q12 = ValueAt(tbl, row, col - 1);
            double Q21 = ValueAt(tbl, row - 1, col);
            double Q22 = ValueAt(tbl, row, col);

            double denom = (x2 - x1) * (y2 - y1);

            if (denom == 0)
            {
                Logger.WriteLine(errmsg);
                Logger.WriteLine("error: division by zero");
                return Point.MissingValueFlag;
            }

            double rval = ((x2 - x) * (y2 - y)) / denom * Q11;
            rval += ((x - x1) * (y2 - y)) / denom * Q21;
            rval += ((x2 - x) * (y - y1)) / denom * Q12;
            rval += ((x - x1) * (y - y1)) / denom * Q22;

            return rval;
        }

        private static double InterpolateRow(DataTable tbl, int row, double columnValue)
        {
            int col = ColumnIndexValue(tbl, columnValue);

            // no interpolation needed, first value in table
            if (row == 0 && col == 1)
                return ValueAt(tbl, row, col);

            double x = columnValue;
            double x1 = Convert.ToDouble(tbl.Columns[col - 1].ColumnName);
            double x2 = Convert.ToDouble(tbl.Columns[col].ColumnName);

            // out of bounds, using last column of data for interpolation
            if (x > x2)
                x = x2;

            double y1 = ValueAt(tbl, row, col - 1);
            double y2 = ValueAt(tbl, row, col);

            return Interpolate(x, x1, x2, y1, y2);
        }

        private static double InterpolateColumn(DataTable tbl, int col, double rowValue)
        {
            int row = RowIndexValue(tbl, rowValue);

            // no interpolation needed, first value in table
            if (row == 0 && col == 1)
                return ValueAt(tbl, row, col);

            double x = rowValue;
            double x1 = ValueAt(tbl, row - 1, 0);
            double x2 = ValueAt(tbl, row, 0);

            // out of bounds, interpolation unknown
            if (x > x2)
                return -1;

            double y1 = ValueAt(tbl, row - 1, col);
            double y2 = ValueAt(tbl, row, col);

            return Interpolate(x, x1, x2, y1, y2);
        }

        private static double Interpolate(double x, double x1, double x2, double y1, double y2)
        {
            if ((x2 - x1) == 0)
            {
                return (y1 + y2) / 2;
            }
            return y1 + (x - x1) * (y2 - y1) / (x2 - x1);
        }

        private static double ValueAt(DataTable tbl, int row, int col)
        {
            return Convert.ToDouble(tbl.Rows[row][col]);
        }

        private static int ColumnIndexValue(DataTable tbl, double columnValue)
        {
            for (int i = 1; i < tbl.Columns.Count; i++)
            {
                string cn = tbl.Columns[i].ColumnName;
                double val = 0.0;
                if (double.TryParse(cn, out val))
                {
                    if (val >= columnValue)
                        return i;
                }
                else
                {
                    return -1;
                }
            }

            return tbl.Columns.Count - 1;
        }

        private static int RowIndexValue(DataTable tbl, double rowValue)
        {
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                string cn = tbl.Rows[i][0].ToString();
                double val = 0;
                if (double.TryParse(cn, out val))
                {
                    if (val >= rowValue)
                        return i;
                }
            }
            return -1;
        }

    }
}
