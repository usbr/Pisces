using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Reclamation.Core;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// A repeating series of data, with multiple column 
    /// can be used for flood control Rule curves, and irrigation diversions
    /// that repeat.  Column names are numbers to support
    /// interpolation between rows and columns.
    /// </summary>
    public class PeriodicSeries: Series
    {
        JulianIndex julian;

        /// <summary>
        /// Constructor for Periodic Series
        /// </summary>
        /// <param name="table">Table that is used to represent periodic data. </param>
        public PeriodicSeries(DataTable table)
        {
            base.HasFlags = false;
            this.Table = table;
            if (table.Rows.Count == 0)
                throw new Exception("No data supplied in Series");
            julian = new JulianIndex(this.MinDateTime, this.MaxDateTime);

        }

        

        public double Interpolate2D(DateTime t, double columnValue)
        {
            //string xColumn = Table.Columns[0].ColumnName;
            int col = FindYColumnIndex(columnValue);
            int row = FindRowIndex(Table, t);
            if( row <1 || col <2)
            {
                Logger.WriteLine("Error: Interpolate2D() row= "+row + " col = "+col);
                return Point.MissingValueFlag;
            }

            int jdayT = julian.LookupJulianDay(t);
            int jdayr = julian.LookupJulianDay(this[row].DateTime);
            double colValue = Convert.ToDouble(Table.Columns[col].ColumnName);
             
            double verticalDistance =  jdayr - julian.LookupJulianDay(this[row-1].DateTime);
            double verticalPercent = (jdayr-jdayT)/verticalDistance;
               
              double colValuem1 = Convert.ToDouble(Table.Columns[col-1].ColumnName);
              double horizontalDistance = colValue - colValuem1;
              double horizontalPercent = (columnValue-colValuem1  ) /horizontalDistance;
              
            double vertInterp1 = (1.0 - verticalPercent) * ValueAt(row, col - 1)
                + verticalPercent * ValueAt(row - 1, col - 1);
            double vertInterp2 = (1.0 - verticalPercent) * ValueAt(row, col) 
                + verticalPercent *ValueAt(row - 1, col);
 
      double horzInterp = (1.0 - horizontalPercent) * vertInterp1
                  + horizontalPercent*vertInterp2; 
            return horzInterp;
        }

        private double ValueAt(int rowIndex, int columnIndex)
        {
             return Convert.ToDouble(Table.Rows[rowIndex][columnIndex]);
        }
        private int FindRowIndex(DataTable Table, DateTime t)
        {
            int jday = julian.LookupJulianDay(t);

            for (int i = 1; i < Count; i++)
            {
                var pt = this[i];
                int jday1 = julian.LookupJulianDay(pt.DateTime);
                if (jday1 >= jday)
                    return i;
            }
            return -1;
        }



        private int FindYColumnIndex(double columnValue)
        {
            for (int i = 2; i < Table.Columns.Count; i++) 
            {
                string cn = Table.Columns[i].ColumnName;
                double val = 0;
                if (double.TryParse(cn, out val))
                {
                    if (val >= columnValue)
                        return i;
                }
            }

            return Table.Columns.Count - 1;
        }


        /// <summary>
        /// Lineraly Interpolates y value from a DataTable 
        /// sorted based on DateTime values (in the first column)
        /// </summary>
        /// <param name="tbl"></param>
        /// <param name="t">DateTime t</param>
        /// <param name="xColumnName">name of column that contains DateTime values</param>
        /// <param name="yColumnName">name of column that contains y values</param>
        /// <param name="nearestIndex">index to row nearest to x_value in your DataTable </param>
        /// <returns></returns>
        public static double Interpolate(DataTable tbl, DateTime t,
                                    string xColumnName,
                                    string yColumnName,
                                    out int nearestIndex
                                    )
        {
            nearestIndex = -1;
            if (tbl.Rows.Count == 0)
            {
                throw new ArgumentException("Interpolate can not work with an empty DataTable");
            }

            DataView rows = tbl.DefaultView;

            int n = rows.Count;

            DateTime currentT = Convert.ToDateTime(rows[0][xColumnName]);
            DateTime previousT = currentT;
            DateTime maxT = Convert.ToDateTime(rows[n - 1][xColumnName]);

            if (t == currentT) // first value in table matches.
            {
                nearestIndex = 0;
                return Convert.ToDouble(tbl.Rows[0][yColumnName]);
            }

            if (t > maxT || t < currentT)
            {
                string msg = "Cannot interpolate " + xColumnName + "=" + t + " it is out of the range of the input DataTable";
                Console.WriteLine(msg);
                //cbp.Utility.Write(tbl);
                throw new ArgumentOutOfRangeException(msg);
            }
            int x_pos = 0;

            do
            {
                x_pos++;
                if (x_pos >= n || previousT > currentT)
                {
                    string msg = "Interpolate failed!  is your DataTable sorted on the " + xColumnName + " column";
                    msg += "\n x_value = " + t + " x_pos = " + x_pos;
                    Logger.WriteLine(msg);
                    throw new InvalidOperationException(msg);
                }
                previousT = currentT;
                currentT = Convert.ToDateTime(rows[x_pos][xColumnName]);
            }
            while (t > currentT);

            if (t == currentT)
            {
                nearestIndex = x_pos;
                return Convert.ToDouble(rows[x_pos][yColumnName]);
            }

            var delta = t - previousT;
            var diff = currentT - previousT;
            double percent = delta.Seconds / diff.Seconds;
            //double percent = (t - previousT) / (currentT - previousT);
            if (percent >= 0.5)
            {
                nearestIndex = x_pos;
            }
            else
            {
                nearestIndex = x_pos - 1;
            }

            double y = Convert.ToDouble(rows[x_pos][yColumnName]);
            double ym1 = Convert.ToDouble(rows[x_pos - 1][yColumnName]);

            if (Point.IsMissingValue(y) || Point.IsMissingValue(ym1))
            {
                return Point.MissingValueFlag;
            }

            return ((1.0 - percent) * ym1 + percent * y);

        }


        //public double Interpolate(DateTime t)
        //{
        //    ////return Interpolate2D(t, Convert.ToDouble(Table.Columns[1].ColumnName));
        //    string xCol = Table.Columns[0].ColumnName;
        //    string yCol = Table.Columns[1].ColumnName;
        //    int idx = 0;
        //    return Interpolate(Table, t, xCol, yCol, out idx);
        //}

        public double Interpolate(DateTime t)
        {
            int col = 1;
            int row = FindRowIndex(Table, t);
            if (row < 1 )
            {
                Logger.WriteLine("Error: Interpolate() row= " + row + " col = " + col);
                return Point.MissingValueFlag;
            }

            int jdayT = julian.LookupJulianDay(t);
            int jdayr = julian.LookupJulianDay(this[row].DateTime);

            double verticalDistance = jdayr - julian.LookupJulianDay(this[row - 1].DateTime);
            double verticalPercent = (jdayr - jdayT) / verticalDistance;

            return   (1.0 - verticalPercent) * ValueAt(row, col)
                + verticalPercent * ValueAt(row - 1, col);

        }

    }
}
