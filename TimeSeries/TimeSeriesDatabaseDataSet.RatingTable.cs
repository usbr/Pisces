using Reclamation.Core;
using Reclamation.TimeSeries.RatingTables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries
{
    public enum InterpolationMethod { None, Linear, LogLog };
    public partial class TimeSeriesDatabaseDataSet
    {
        public partial class RatingTableDataTable
        {

            /// <summary>
            /// Compute values using a RatingTableDataTable
            /// for example compute flows based on gage height input series
            /// </summary>
            /// <param name="s"></param>
            /// <param name="fileName">rating table filename</param>
            /// <returns></returns>
            public static Series ComputeSeries(Series s, string fileName, 
                InterpolationMethod method = InterpolationMethod.None)
            {
                var rval = new Series();
                
                Logger.WriteLine("RatingTableDataTable.ComputeSeries(" + s.Table.TableName + "," + fileName);

                var fn = RatingTableUtility.GetRatingTableAsLocalFile(fileName);
                 
                if (!File.Exists(fn))
                {
                    string msg = "Error: File not found " + fn;
                    Console.WriteLine(msg);
                    Logger.WriteLine(msg);
                    rval.TimeInterval = s.TimeInterval;    
                    return rval;
                }

                var rt = new RatingTableDataTable();
                rt.ReadFile(fn);

                rval =rt.Lookup(s,method);
                
                rval.TimeInterval = s.TimeInterval;
                return rval;
            }


            public void ReadFile(string fileName)
            {
                //m_cbtt = cbtt;
                //tring fileName = cbtt + "_" + pcode + ".txt";
                
                var tf = new TextFile(fileName.ToLower());
                int idx = tf.IndexOf("BEGIN TABLE"); // hydromet raw dumps
                if (idx < 0)
                    idx = 0;
                else
                    idx += 2;

                for (int i = idx; i < tf.Length; i++)
                {

                    var tokens = tf[i].Split(',');

                    if (tokens.Length == 2) // csv.
                    {
                        TryParseTokens(fileName, tokens);
                    }
                    else
                    {
                        tokens = tf[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if( tokens.Length == 2)
                        {
                          TryParseTokens(fileName, tokens);
                        }
                    }
                }
            }

            private void TryParseTokens(string fileName, string[] tokens)
            {
                if (tokens.Length != 2)
                    return;
                double x, y;
                if (double.TryParse(tokens[0], out x)
                      && double.TryParse(tokens[1], out y))
                {
                    var r = this.FindByx(x);
                    if (r == null)
                        AddRatingTableRow(x, y);
                    else
                    {
                        Console.WriteLine("Warning: Rating table has duplicate independent values. Skipping: " + x + "\n" + fileName);
                    }
                }
            }

            public string YUnits = "";
            public string XUnits = "";
            public string Name = "";
            public string EditDate = "";

            /// <summary>
            /// Lookup values for each point in the Series
            /// </summary>
            /// <param name="s"></param>
            /// <returns></returns>
            public Series Lookup(Series s, 
                InterpolationMethod method = InterpolationMethod.None)
            {
                Series rval = new Series();

                foreach (var pt in s)
                {
                    if (method  == InterpolationMethod.Linear
                   || method == InterpolationMethod.LogLog)
                    {
                        rval.Add(Interpolate(pt,method));
                    }
                    else
                    {
                        rval.Add(Lookup(pt));
                    }
                }

                return rval;
            }

            /// <summary>
            /// interpolation for OWRD shifts.
            /// extrapolate values lower than minimum using the minimum
            /// </summary>
            /// <param name="val"></param>
            /// <returns></returns>
            public double InterpolateExtrapolateLow(double val)
            {
                if (val > MaxXValue())
                    return Point.MissingValueFlag;

                if (val < MinXValue())
                { 
                    return MinYValue();
                }

                return Math.Interpolate(this, val, this.columnx.ColumnName, this.columny.ColumnName);
            }

             private Point Interpolate(Point pt, InterpolationMethod method)
            {
                if (pt.IsMissing)
                    return new Point(pt.DateTime, Point.MissingValueFlag);

                if (pt.Value > MaxXValue())
                    return new Point(pt.DateTime, Point.MissingValueFlag);

                if (pt.Value < MinXValue())
                { // if first value in table computes zero, then extrapolate a zero.
                    if (System.Math.Abs(MinYValue()) < 0.01)
                        return new Point(pt.DateTime, 0, PointFlag.Edited);

                    return new Point(pt.DateTime, Point.MissingValueFlag);
                }

               var d = Math.Interpolate(this, pt.Value, this.columnx.ColumnName, this.columny.ColumnName,method);
               return new Point(pt.DateTime, d);
            }
            /// <summary>
            /// Lookup value in RatingTable
            /// </summary>
            /// <param name="pt"></param>
            /// <returns></returns>
            private Point Lookup(Point pt)
            {
                if (pt.IsMissing)
                    return new Point(pt.DateTime, Point.MissingValueFlag);


                if (pt.Value > MaxXValue())
                    return new Point(pt.DateTime, Point.MissingValueFlag);

                if (pt.Value < MinXValue())
                { // if first value in table computes zero, then extrapolate a zero.
                    if (System.Math.Abs(MinYValue()) < 0.01)
                        return new Point(pt.DateTime, 0, PointFlag.Edited); 

                    return new Point(pt.DateTime, Point.MissingValueFlag);
                }


                double d = Lookup(pt.Value);

                if (Point.IsMissingValue(d))
                {
                    Logger.WriteLine("Rating Table Lookup failed for "+ pt.ToString());
                    return new Point(pt.DateTime, Point.MissingValueFlag);
                }

                return new Point(pt.DateTime, d);
            }


            private double MinXValue()
            {
                if (Rows.Count == 0)
                    return Point.MissingValueFlag;

                return this[0].x;
            }
            private double MinYValue()
            {
                if (Rows.Count == 0)
                    return Point.MissingValueFlag;

                return this[0].y;
            }

            private double MaxXValue()
            {
                if (Rows.Count == 0)
                    return Point.MissingValueFlag;

                return this[Rows.Count - 1].x;
            }

            public double Lookup(double p)
            {
                var r = FindByx(p);
                if (r == null)
                {
                    r = FindByx(System.Math.Round(p,2));
                    if( r==null)
                      return Point.MissingValueFlag;
                }
                    
                return r.y;
            }
        }

    }

}
