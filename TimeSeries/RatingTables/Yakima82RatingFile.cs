using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Reclamation.TimeSeries.RatingTables
{
    /// <summary>
    /// manage  fortran style rating table file
    /// </summary>
    public class Yakima82RatingFile
    {
        TextFile tf;
        public Yakima82RatingFile(string filename)
        {
           tf = new TextFile(filename);
        }

        public string[] UniqueTables()
        {
           var rval = new List<string>();
            for (int i = 0; i < tf.Length; i++)
            {
                if (tf[i].Trim() == "")
                    continue;
                var tokens = tf.Split(i);
                if( tokens.Length < 3)
                {
                    var k = tf[i].Trim();
                    if( !rval.Contains(k))
                      rval.Add(k);
                    else
                        Console.WriteLine("allready have:" +tf[i].Trim());
                }
            }

           return rval.ToArray();
        }

         HydrographyDataSet.rating_tablesDataTable AppendToTable(string tableName, HydrographyDataSet.rating_tablesDataTable tbl)
        {
            var rt = new Dictionary<double, double>();
            int idx = tf.IndexBeginningWith(tableName, 0);
            string cbtt = tf[idx].Substring(0, 4).Trim();
            string pc = tf[idx].Substring(4, 2);
            string version = tf[idx].Substring(7).Trim();
            for (int i = idx; i < tf.Length; i++)
            {
                if (tf[i].Trim() == "")
                    continue;
                string c = tf[i].Substring(0, 4).Trim();
                string p = tf[i].Substring(4, 2);

                if (c != cbtt || p != pc)
                    break;

                var tokens = tf[i].Substring(6).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length >= 2)
                {
                    for (int j = 0; j < tokens.Length; j += 2)
                    {
                        double x, y;
                        if (double.TryParse(tokens[j], out x)
                            && double.TryParse(tokens[j + 1], out y))
                        {
                            if( rt.ContainsKey(x))
                            {
                                Console.WriteLine("duplicate entry skipped "+x+", "+y);  
                            }
                            else
                              rt.Add(x, y);
                        }
                        else
                        {
                            Console.WriteLine("Error parsing: " + tf[i]);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("skipping :"+ tf[i]);
                }
            }
            string s = string.Join("\n", rt.Select(x => x.Key + "," + x.Value).ToArray());
            if (Regex.IsMatch(version, "^([0-9]){6}$"))
            { //050586
                int yr = int.Parse(version.Substring(4));
                int fourDigitYear = CultureInfo.CurrentCulture.Calendar.ToFourDigitYear(yr);
                version = fourDigitYear.ToString()+"-"+ version.Substring(0,2)+"-"+version.Substring(2,2);
            }
            if (version == "")
                version = "untitled";
            var discharge_variable = "XX";
            if (pc == "GD")
                discharge_variable = "QD";
            if (pc == "GJ")
                discharge_variable = "QJ";

             
            tbl.Addrating_tablesRow(tbl.NextID(), version, cbtt, pc,discharge_variable, s);
            return tbl;
                
        }
         public HydrographyDataSet.rating_tablesDataTable SaveToTable(HydrographyDataSet.rating_tablesDataTable tbl)
        {

            foreach (var item in UniqueTables())
            {
               AppendToTable(item,tbl);
            } 

            /*
                                                                                                           
BLEWGD    060677                                                                                                                    
BLEWGD         0.00      0.00      0.10      0.16      0.20      0.32      0.30      0.67                                           
BLEWGD         0.40      1.12      0.50      1.68      0.60      2.37      0.70      3.18                                           
BLEWGD         0.80      4.09      0.90      5.12      1.00      6.28      1.10      7.57                                           
BLEWGD         1.20      8.97      1.30     10.50      1.40     12.10      1.50     13.70                                           
BLEWGD         1.60     15.35      1.70     16.95      1.80     18.60      1.90     20.30                                           
BLEWGD         2.00     22.10      2.10     24.00      2.20     26.00      2.30     28.00                                           
BOCWGD    061677                                                                                                                    
BOCWGD         0.00      0.00      0.10      0.35      0.20      0.70      0.30      1.20                                           
BOCWGD         0.40      1.70      0.50      2.70      0.60      3.70      0.70      5.35                                           
BOCWGD         0.80      7.00      0.90      9.30      1.00     12.10      1.10     16.80                                           
BOCWGD         1.20     20.40      1.30     25.85      1.40     31.30      1.50     37.00                                           
BOCWGD         1.60     42.75      1.70     48.55      1.80     54.40      1.90     60.30                                           
BOCWGD         2.00     66.20      2.10     72.15      2.20     78.15      2.30     84.20                                           
BOCWGD         2.40     90.30      2.50     96.50      2.60    102.80      2.70    109.20                                           
BOCWGD         2.80    115.60      2.90    122.10      3.00    128.65                                                               
BUM GD   011375               
             */
            
            return tbl;
        }

        }
    }

