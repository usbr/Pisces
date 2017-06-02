using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Reclamation.Core;

namespace HydrometForecast
{
    /// <summary>
    /// wrapper aound 'history.out' which is the hydromet
    /// static version of forecast terms
    /// </summary>
    class HistoricalTerms
    {
        public DataTable Table = new DataTable();

        public HistoricalTerms(string filename, string forecastName)
        {
            BuildTable(filename, forecastName);
        }

        private void BuildTable(string filename, string forecastName)
        {
            Table = new DataTable();
            TextFile tf = new TextFile(filename);

            int idx = FindIndexToForecastName(forecastName, tf);

            if (idx >= 0)
            {
                idx++;
                int idx2 = tf.IndexOfRegex("^[a-zA-Z]", idx);

                if (idx2 > 0)
                {
                    CreateColumns(tf, idx);
                    while (idx < idx2)
                    {
                        var row = Table.NewRow();
                        Table.Rows.Add(row);

                        var tokens = TextFile.Split(tf[idx]);
                        for (int col = 0; col < tokens.Length; col++)
                        {
                            
                            if( col == tokens.Length-1)
                            {
                                row[0] = Convert.ToDouble(tokens[col]);
                            }
                            else
                            {
                                row[col+1] = Convert.ToDouble(tokens[col]);
                            }
                        }
                        idx++;
                    }

                }
            }
        }

        private static int FindIndexToForecastName(string forecastName, TextFile tf)
        {
            int idx = tf.IndexOfRegex("^" + forecastName.Trim() + @"\s+$");
            int idx2 = -1;
            if (idx >= 0)
            {// try for the same name further in the file.
                idx2 = tf.IndexOfRegex("^" + forecastName.Trim() + @"\s+$", idx + 1);
            }

            return Math.Max(idx, idx2);
        }

        private string[] CreateColumns(TextFile tf, int idx)
        {
            var tokens = TextFile.Split(tf[idx]);
            // add columns to datatable
            Table.Columns.Add("Year", typeof(int));
            for (int i = 1; i < tokens.Length - 1; i++)
            {
                Table.Columns.Add("X" + i, typeof(double));
            }
            Table.Columns.Add("Y1", typeof(double));
            return tokens;
        }

        
    }
}
