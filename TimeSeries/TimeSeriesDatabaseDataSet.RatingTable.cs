using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries
{
    public partial class TimeSeriesDatabaseDataSet
    {
        public partial class RatingTableDataTable
        {

            //string m_cbtt;

            public static Series ComputeSeries(Series s, string fileName)
            {
                var rt = new RatingTableDataTable();
                rt.ReadTable(fileName);
                var rval = rt.Lookup(s);
                rval.TimeInterval = s.TimeInterval;
                return rval;
            }


            void ReadTable(string fileName)
            {
                //m_cbtt = cbtt;
                //tring fileName = cbtt + "_" + pcode + ".txt";
                var fn = Path.Combine(Path.Combine(Globals.LocalConfigurationDataPath, "rating_tables"), fileName);
                var tf = new TextFile(fn.ToLower());
                int idx = tf.IndexOf("BEGIN TABLE"); // hydromet raw dumps
                if (idx < 0)
                    idx = 0;
                else
                    idx += 2;

                for (int i = idx; i < tf.Length; i++)
                {

                    var tokens = tf[i].Split(',');

                    if (tokens.Length == 2)
                    {
                        double x, y;
                        if (double.TryParse(tokens[0], out x)
                              && double.TryParse(tokens[1], out y))
                        {
                            AddRatingTableRow(x, y);
                        }
                    }

                }

                //this.AddRatingTableRow(
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
            public Series Lookup(Series s)
            {
                Series rval = new Series();

                foreach (var pt in s)
                {

                    if (pt.IsMissing)
                        rval.AddMissing(pt.DateTime);
                    else
                        rval.Add(Lookup(pt));
                }

                return rval;
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
                    return new Point(pt.DateTime, Point.MissingValueFlag);


                double d = Lookup(pt.Value);

                if (d == Point.MissingValueFlag)
                    return new Point(pt.DateTime, Point.MissingValueFlag);

                return new Point(pt.DateTime, d);
            }


            private double MinXValue()
            {
                if (Rows.Count == 0)
                    return Point.MissingValueFlag;

                return this[0].x;
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
                return r.y;
            }
        }

    }

}
