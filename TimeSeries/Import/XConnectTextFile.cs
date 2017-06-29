using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reclamation.TimeSeries.Import
{
    /// <summary>
    /// Reads from X-Connect exported text files of time series data.
    /// http://www.sutron.com/product/xconnect-software/
    /// </summary>
    public class XConnectTextFile
    {
        /*
pcbase,TUFW.Q
11/15/2016 13:45:00,0.00
11/15/2016 14:00:00,0.00
pcbase,KESW.QC
11/15/2016 13:45:00,0.00
11/15/2016 14:00:00,0.00
pcbase,NRYW.BV,NRYW.GH
11/15/2016 13:45:00,12.60,80.61
11/15/2016 14:00:00,12.56,80.61
pcbase,RSCW.CH
11/15/2016 13:15:00,1.58
11/15/2016 13:30:00,1.57
         */

        TextFile m_tf;
        public XConnectTextFile(TextFile tf)
        {
            m_tf = tf;
        }

        internal static bool IsValidFile(TextFile tf)
        {
            return tf.Length > 0
                && tf[0].ToLower().StartsWith(tag);
        }

        static string tag = "pcbase,";
        internal SeriesList ToSeries()
        {
            var rval = new SeriesList();
            var tableNames = new string[] { };
            for (int i = 0; i < m_tf.Length; i++)
            {
                var line = m_tf[i];
                var tokens = line.Split(',');
                
                if (line.ToLower().StartsWith(tag))
                {
                    tableNames = GetTableNames(tokens);
                    continue;
                }

                DateTime t;
                if( !DateTime.TryParse(tokens[0],out t))
                {
                    Console.WriteLine("invalid date/time Skipping line: "+line);
                    continue;
                }
                    Series s = null;
                    for (int j = 1; j < tokens.Length; j++)
                    {
                        var tn = tableNames[j - 1];
                        int idx = rval.IndexOfTableName(tn);
                        if( idx <0)
                        { // create new series
                            s = new Series(tn);
                            s.Table.TableName = tn;
                            rval.Add(s);
                        }
                        else
                        { // get pointer to existing series.
                            s = rval[idx];
                        }
                        double d;
                       if( !double.TryParse(tokens[j], out d))
                       {
                           Console.WriteLine("could not parse number "+tokens[j]);
                           continue;
                       }

                       s.Add(t, d);
                    }
                
            }

            return rval;
        }

        private string[] GetTableNames(string[] tokens)
        {
            var rval = new List<String>();
            for (int j = 1; j < tokens.Length; j++)
            {
                var n = new TimeSeriesName(tokens[j].Replace(".", "_"), TimeInterval.Irregular);
                rval.Add(n.GetTableName());
            }
            return rval.ToArray();
        }
    }
}
