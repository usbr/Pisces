using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.Core;
using System.IO;

namespace Reclamation.TimeSeries.DataLogger
{

    /// <summary>
    /// Class that reads Campbell Scientific data logger files.
    /// *.dat csv files 
    /// https://www.campbellsci.com/loggernet
    /// 
    /// </summary>
    public class LoggerNetFile
    {
        /*
         * 
 "TOA5","minidoka_lagoon_test","CR1000","48192","CR1000.Std.24","CPU:Minidoka Test.cr1","14574","level1"
 "TIMESTAMP","RECORD","Freq","Digits","Level_Change","sensortemp","Water_Temp","Lagoon_Hgt"
 "TS","RN","Hz","","","","",""
 "","","Smp","Smp","Smp","Smp","Smp","Smp"
 "2014-05-09 11:21:00",0,2501,6257,-10.34253,57.21,56.15,-10.34253
 "2014-05-09 11:22:00",1,2498,6242,-10.31803,57.52,56.21,-10.32764
 "2014-05-09 11:23:00",2,2490,6199,-10.24673,57.93,56.21,-8.24352        
         * 
         * 
         * 
"TOACI1","WRDO","Min15"
"TMSTAMP","RECNBR","BV","OB","OBM","OBX","OBN","TU","TUX","TUN","EA","TP","WS","WSH","WD","WDS","WG","PC","SQ","UI","SI","ETo","ETr","SW","SV","LW","AU"
"2014-10-06 11:15:00",24460,13.75,67.55,66.65,68.16,65.19,31.18,33.35,28.42,0.671,35.23,1.575,1.491,67.83,36.45,3.946,7.992,3626,4953,49.55,0.1318045,0.1639985,48.23,51.77,7999,73.87
"2014-10-06 11:30:00",24461,13.87,68.87,68.25,69.6,66.7,28.7,32.98,25.35,0.668,34.51,1.288,1.375,92.8,75.01,5.042,7.992,3639,4953,51.74,0.1372422,0.1692722,48.31,51.76,7999,75.93
"2014-10-06 11:45:00",24462,13.96,70.55,69.68,70.74,68.38,25.89,29.43,23.99,0.619,33.17,1.702,1.633,200.1,73.09,4.603,7.992,3653,4953,53.34,0.1499646,0.1884647,48.39,51.72,7999,79.13
"2014-10-06 12:00:00",24463,13.95,71.51,71.14,72.6,70,23.42,25.98,21.63,0.572,31.93,2.097,1.763,126.7,55.03,5.918,7.992,3666,4954,54.22,0.1594979,0.2023601,48.52,51.72,7999,81.7
         
         */

        TextFile tf;
        public string FileFormat;
        public string SiteName;
        string[] infoHeader;
        string[] dataHeader;
         Dictionary<string,Series> dict = new Dictionary<string, Series>();
         bool valid_site = false;

        public LoggerNetFile(string filename)
        {
            tf = new TextFile(filename);
            ParseHeader();
        }
        public LoggerNetFile(TextFile  tf)
        {
            this.tf = tf;
            ParseHeader();
        }

        public static bool IsValidFile(TextFile tf)
        {
            var x = new LoggerNetFile(tf);
            return x.IsValidFormat();
        }
        private void ParseHeader()
        {
            FileFormat = "Error";
            if (tf.Length > 1)
            {
                infoHeader = tf[0].Replace("\"", "").Split(',');
                if (infoHeader.Length < 2)
                    return;
                // HACK for WS_mph in some Utah Sites...
                dataHeader = tf[1].Replace("\"", "").Replace("WS_mph", "WS").Split(',');
                FileFormat = infoHeader[0];
                var fn = Path.GetFileName(tf.FileName);
                int idx = fn.IndexOf("_");
                if (idx > 3)
                {
                    SiteName = fn.Substring(0, idx);
                    valid_site = SiteName.Length >= 3 && SiteName.Length <= 4;
                }
            }
        }

        private bool IsValidFormat()
        {
            
             bool validFomat = FileFormat.Contains("TOACI1") || FileFormat.Contains("TOA5");
            return validFomat;
        }

        public bool IsValid { 
            get{

                bool validFomat = IsValidFormat();

                if( !validFomat)
                    Console.WriteLine("Error: bad file format "+infoHeader);


                //bool validSite = Path.GetFileName(tf.FileName).IndexOf(SiteName.Trim()) == 0;
                //if (!validSite)
                //{
                //    Console.WriteLine("Error: site name in file does not match filename.");
                //    Console.WriteLine("Filename: '" + tf.FileName + "'  siteName: '" + SiteName + "'");
                //}

                return validFomat && valid_site;
            }
        }


        public SeriesList ToSeries(string[] valid_pcodes)
        {
            foreach (var pcode in dataHeader)
            {
                int idx = Array.IndexOf(dataHeader, pcode);
                if (idx >= 0 && Array.IndexOf(valid_pcodes, pcode) >= 0)
                {
                    string key = "instant_" + SiteName.ToLower() + "_" + pcode.ToLower();
                    Series s = new Series();
                    if (dict.ContainsKey(key))
                    {
                        s = dict[key];
                    }
                    else
                    {

                        s.Name = key;
                        s.Parameter = pcode;
                        s.Table.TableName = key;
                        s.SiteName = SiteName;
                        dict.Add(key, s);
                    }

                    for (int i = 2; i < tf.Length; i++)
                    {
                        var tokens = tf[i].Replace("\"", "").Split(',');
                        double d = 0;
                        DateTime t;
                        if (DateTime.TryParse(tokens[0], out t)
                            && double.TryParse(tokens[idx], out d))
                        {
                            if (s.IndexOf(t) >= 0)
                            {
                                Logger.WriteLine("LoggerNetFile: skipping duplicate point ");
                            }
                            else
                            {
                                s.Add(t, d);
                            }
                        }
                        else
                        {
                            Logger.WriteLine("LoggerNetFile: Skipping line " + tf[i]);
                        }
                    }
                    Logger.WriteLine("LoggerNetFile.ToSeries(): " + key + " has " + s.Count + " items");
                }
            }
            SeriesList sl = new SeriesList();
            sl.AddRange(dict.Values.ToArray());
            return sl;
        //    Logger.WriteLine("Found " + rval.Count + " parameters in " + tf.FileName);
        }




       
    }
}
