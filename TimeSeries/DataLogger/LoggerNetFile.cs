using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.Core;
using System.IO;

namespace Reclamation.TimeSeries.DataLogger
{
    public class LoggerNetFile
    {
       

        TextFile tf;
        string[] valid_pcodes;
        public string FileFormat;
        public string SiteName;
        string[] infoHeader;
        string[] dataHeader;
        static Dictionary<string,Series> dict = new Dictionary<string, Series>();
        public LoggerNetFile(string[] valid_pcodes, string filename)
        {
            this.valid_pcodes = valid_pcodes;
            tf = new TextFile(filename);
            if (tf.Length > 0)
            {
                infoHeader = tf[0].Replace("\"","").Split(',');
                dataHeader = tf[1].Replace("\"", "").Split(',');
                FileFormat = infoHeader[0];
                SiteName = infoHeader[1];
            }
        }

        
        public static Series[] ReadFiles(string[] valid_pcodes,string inputDir,string processedDir, int maxFiles=10)
        {
            int count = 0;
            dict.Clear();

            var files = Directory.GetFiles(inputDir, "*.dat");
            foreach (var item in files)
            {
                // only process files that have finished writing.. wait 2 seconds to be sure
                FileInfo fi = new FileInfo(item);
                if (fi.CreationTime.AddSeconds(2) > DateTime.Now)
                    continue;
                Console.WriteLine(" processing " + item);

                LoggerNetFile lf = new LoggerNetFile(valid_pcodes,item);

                lf.ReadFileIntoSeries();
                var dest =  Path.Combine(processedDir, Path.GetFileName(item));
                if (File.Exists(dest))
                    File.Delete(dest);
                File.Move(item,dest);

                if (count >= maxFiles)
                    break;
                count++;
            }

            return dict.Values.ToArray();
        }

        private void ReadFileIntoSeries()
        {
            foreach (var pcode in dataHeader) 
            {
                int idx = Array.IndexOf(dataHeader,pcode);
                if (idx >=0 && Array.IndexOf(valid_pcodes, pcode) >=0)
                {
                    string key =  SiteName.ToLower() + "_" + pcode.ToLower();
                    Series s = new Series();
                    if (dict.ContainsKey(key))
                    {
                        s = dict[key];
                    }
                    else
                    {

                        s.Name = SiteName;
                        s.Parameter = pcode;
                        s.Table.TableName = key;
                        dict.Add(key, s);
                    }

                    for (int i = 2; i < tf.Length; i++)
                    {
                        var tokens = tf[i].Replace("\"","").Split(',');
                        double d =0;
                        DateTime t;
                        if( DateTime.TryParse(tokens[0],out t)
                            && double.TryParse(tokens[idx],out d) )
                        {
                            if (s.IndexOf(t) >= 0)
                            {
                                Logger.WriteLine("skipping duplicate point ");
                            }
                            else
                            {
                                s.Add(t, d);
                            }
                        }
                    }
                }

            }
        //    Logger.WriteLine("Found " + rval.Count + " parameters in " + tf.FileName);
        }



    }
}
