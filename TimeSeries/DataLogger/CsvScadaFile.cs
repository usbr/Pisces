using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.Core;
using System.IO;
using System.Text.RegularExpressions;

namespace Reclamation.TimeSeries.DataLogger
{

    /// <summary>
    /// Class that reads csv real time data
    /// multiple locations (rows)
    /// multiple sensors (columns)
    /// </summary>
    public class CsvScadaFile
    {
        /*
         *
sitename,tmstp,gv1,gs1,gv2,gs2,gv3,gs3,gv4,gs4,gv5,gs5,gv6,gs6,lv1,ls1,lv2,ls2,lv3,ls3,lv4,ls4,lv5,ls5,lv6,ls6,sv,ss
'blank','1996-01-22 20:05:01.750',0.0,0,0.0,0,0.0,0,0.0,0,0.0,0,0.0,0,0.0,0,0.0
'SITE00','2018-01-09 11:20:41.838',0.0,1024,0.0,1024,0.0,1024,0.0,1024,0.0,1024,0.0,1024,14.073647499084473,0,0.0,0,0.0,0,0.0,0,0.0,0,0.0,0,0,0
'SITE01','2018-01-09 11:20:43.732',-0.010548770427703857,1280,-0.10091567039489746,1280,0.0027613937854766846,1280,0.019564807415008545,1280,0.007951468229293823,1280,0.02238321304321289,1280,1568.06396484375,0,1498.3232421875,0,1522.43115234375,0,1517.951904296875,0,914.4522705078125,0,1.1802990436553955,0,0,16
'SITE02','2018-01-09 11:20:45.405',0.0,1024,0.0,1024,0.0,1024,0.0,1024,0.0,1024,0.0,1024,1513.4854736328125,0,0.0,0,0.0,0,0.0,0,0.0,0,0.0,0,0,0
'SITE03','2018-01-09 11:20:46.888',13.285545349121094,1024,14.003377914428711,1024,0.0,1024,0.0,1024,0.0,1024,0.0,1024,1497.5538330078125,0,0.0,0,0.0,0,0.0,0,0.0,0,0.0,0,0,0
'SITE04','2018-01-09 
         */

        CsvFile csv;
         Dictionary<string,Series> dict = new Dictionary<string, Series>();

        public CsvScadaFile(string filename)
        {
            csv = new CsvFile(filename,CsvFile.FieldTypes.AllText);
        }

        public static bool IsValidFile(TextFile tf)
        {
           return tf.Length > 1 && tf[0].ToLower().IndexOf("sitename,tmstp,gv1,gv2") == 0;
        }


        public SeriesList ToSeries()
        {
            // match gate or level status// matches gv1, gv2,... lv1, lv2,...
            Regex re = new Regex("^(g|l)v[1-6]{1}$", RegexOptions.Compiled);
            for (int i = 0; i < csv.Rows.Count; i++)
            {
                var row = csv.Rows[i];
                //sitename,tmstp,gv1,gs1,gv2,gs2,gv3,gs3,gv4,gs4,gv5,gs5,gv6,gs6,lv1,ls1,lv2,ls2,lv3,ls3,lv4,ls4,lv5,ls5,lv6,ls6,sv,ss
                string siteid = row["sitename"].ToString().ToLower();
                string timestamp = row["tmstp"].ToString();
                DateTime t;
                if (!DateTime.TryParse(timestamp, out t))
                {
                    continue;
                }

                for (int j = 2; j < csv.Columns.Count; j++)
                {
                    string pcode = csv.Columns[j].ToString().ToLower();
                    if (!re.IsMatch(pcode))
                        continue;

                    string val = row[j].ToString();
                    double dval;
                    if (!double.TryParse(val, out dval))
                    {
                        continue;
                    }

                     var key = "instant_" + siteid + "_" + pcode.ToLower();
                    Series s = new Series();
                    if (dict.ContainsKey(key))
                    {
                        s = dict[key];
                    }
                    else
                    {
                        s.Name = siteid + "_" + pcode;
                        s.Parameter = pcode;
                        s.Table.TableName = key;
                        s.SiteID = siteid;
                        dict.Add(key, s);
                    }

                    if (s.IndexOf(t) >= 0)
                    {
                        Logger.WriteLine("skipping duplicate point "+s.Name +" " +timestamp+" "+val);
                    }
                    else
                    {
                        s.Add(t, dval);
                    }

                }
            }
            SeriesList sl = new SeriesList();
            sl.AddRange(dict.Values.ToArray());
            return sl;
        }




       
    }
}
