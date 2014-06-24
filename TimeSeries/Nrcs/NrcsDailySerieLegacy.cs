using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using Reclamation.TimeSeries.Hydromet;
namespace Reclamation.TimeSeries.Nrcs
{
    /// <summary>
    /// Reads SNOTEL data from nrcs web site.
    /// 
    /// </summary>
    [Obsolete()]
    class NrcsDailySerieLegacy: Series
    {

        string  siteName, columnName, state;
        string url =      "http://www.wcc.nrcs.usda.gov/ftpref/data/snow/snotel/cards/";
        string urlDepth = "http://www.wcc.nrcs.usda.gov/ftpref/data/snow/snotel/reports/snow_depth/";//idaho/";//15f04s.txt

        public NrcsDailySerieLegacy(string state, string siteName, string columnName)
        {
            this.state = LookupState(state.ToUpper());
            this.siteName = siteName.ToLower();
            this.columnName = columnName;
            this.TimeInterval = TimeInterval.Daily;
            int idx = Array.IndexOf(columnNames, columnName);
            if(idx >=0)
               Units = unitsList[idx];

        }
        static string[] unitsList = { "inches", "inches", "degrees C", "degrees C", "degrees C", "inches","inches"};
        static string[] parmCodes = { "SE", "PC", "MX", "MN", "MM", "PP", "SD" };
        static string[] columnNames = { "pill", "prec", "tmax", "tmin", "tavg", "prcp", "Depth" };


        public static NrcsDailySerieLegacy GetNRCS(string cbtt, string pcode, string snotelId)
        {
            string state = Hydromet.HydrometInfoUtility.LookupState(cbtt);
            int idx = Array.IndexOf(parmCodes, pcode.ToUpper());
            string col = "pill";
            if (idx >= 0)
            {
                col = columnNames[idx];
                NrcsDailySerieLegacy s = new NrcsDailySerieLegacy(state, snotelId, col);
                return s;
            }

            string msg = "Error pcode '" + pcode + "' not supported";
            Logger.WriteLine(msg);
            throw new InvalidOperationException(msg);
        }

        private string LookupState(string stateCode)
        {
            switch (stateCode)
            {
                case "WA":
                    return "washington";
                case  "ID":
                    return "idaho";
                case "MT":
                    return "montana";
                case "NV":
                    return "nevada";
                case "OR":
                    return "oregon";
                case "WY":
                    return "wyoming";
                default:
                    break;
            }

            return "";
        }

        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            Clear();
            try
            {
                ReadFile(t1, t2);
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex.Message);
                Clear();
            }
            
        }

        private bool InCurrentWaterYear(DateTime t1, DateTime t2)
        {
            return t1.WaterYear() == t2.WaterYear() && 
                   t1.WaterYear() == DateTime.Now.WaterYear();
        }

        private void ReadFile( DateTime t1,  DateTime t2)
        {
            if (columnName == "Depth")
            {// special case, different source of data, different location for data
             // only 8 days of data possible
                ReadSnowDepth(t1, t2);
            }
            else
            {
                ReadHistoricalFilesFromWeb(t1, t2);
            }

        }

        private void ReadSnowDepth(DateTime t1, DateTime t2)
        {
            /*
--------------------------------------------------------------------------------
	           					           Change In
                                  Snow Water   Snow  Snow Water  Change In
Site Name               Date Time   Equivalent  Depth  Equivalent  Snow Depth
------------------------------------------------------------------------------
ATLANTA SUMMIT         01/07 0000       16.1     58.0
                       01/08 0000       16.1     57.0       0.0        -1.0
                       01/09 0000       16.0     57.0      -0.1         0.0
                       01/10 0000       15.9     57.0      -0.1         0.0
                       01/11 0000       15.9     56.0       0.0        -1.0
                       01/12 0000       16.0     56.0       0.1         0.0
                       01/13 0000       16.8     61.0       0.8         5.0
                       01/14 0000       17.4     64.0       0.6         3.0
             * 
--------------------------------------------------------------------------------
						       Change In
	                            Snow Water   Snow  Snow Water  Change In
Site Name               Date Time   Equivalent  Depth  Equivalent  Snow Depth
------------------------------------------------------------------------------
JACKSON PEAK           01/07 0000       13.5     50.0
                       01/08 0000       13.5     48.0       0.0        -2.0
                       01/09 0000       13.4     49.0      -0.1         1.0
                       01/10 0000       13.4     48.0       0.0        -1.0
                       01/11 0000       13.4     48.0       0.0         0.0
                       01/12 0000       13.5    -99.9       0.1       -99.9
                       01/13 0000       14.5    -99.9       1.0       -99.9
                       01/14 0000       15.4     62.0       0.9       -99.9
 
------------------------------------------------------------------------------
 

            
 
------------------------------------------------------------------------------
             */
            var data = Web.GetPage(urlDepth + state + "/" + siteName + ".txt", true);

            TextFile tf = new TextFile(data);
            var idx = tf.IndexOf("Site Name               Date Time   Equivalent  Depth");
            if (idx > 0)
            {
                string pattern = "^.{23}(?<month>\\d{2})/(?<day>\\d{2})\\s{1}0000\\s*(?<se>[0-9\\.+-]+)\\s*(?<sd>[0-9\\.+-]+)";
                for (int i = idx+2; i < idx+10; i++)
                {
                    Regex re = new Regex(pattern);
                    
                    var m = re.Match(tf[i]);
                    if (m.Success)
                    {
                        int month = int.Parse(m.Groups["month"].Value);
                        int day =   int.Parse(m.Groups["day"].Value);
                        double sd = double.Parse(m.Groups["sd"].Value);
                        DateTime t = new DateTime(DateTime.Now.Year, month, day);
                        if (t >= t1 && t <= t2)
                        {
                            if (System.Math.Abs(sd - 99.9) < 0.1)
                                AddMissing(t);

                              Add(t, sd);
                        }
                    }
                    
                }
            }

        }

        private void ReadHistoricalFilesFromWeb(DateTime t1, DateTime t2)
        {
            int wy = DateTime.Now.WaterYear();
            TextFile tf = new TextFile(new string[] { });

            if (!InCurrentWaterYear(t1, t2))
            {
                //string filename = Path.Combine(dir, siteName + "_all.txt");
                var data = Web.GetPage(url + state + "/" + siteName + "_all.txt", true);
                tf.Append(data);
            }

            var lines = Web.GetPage(url + state + "/" + siteName + "_" + wy + ".tab", true);
            tf.Append(lines);

            int errorCount = 0;
            if (tf.Length == 0)
                return;
            string a = tf[0];
            int idx = a.LastIndexOf("-");
            Name = a.Substring(0, idx);
            a = a.Substring(idx + 1);
            string[] cols = TextFile.Split(a);

            int idxData = Array.IndexOf(cols, columnName);
            if (idxData < 0)
            {
                Console.WriteLine("Can't find column '" + columnName + "'");
                return;
            }


            for (int i = 1; i < tf.Length; i++)
            {
                string[] tokens = tf[i].Split('\t');//.Split(i);

                if (idxData >= tokens.Length)
                {
                    continue;
                }

                if (!Regex.IsMatch(tokens[0], "[0-9]{6}"))
                {
                    errorCount++;
                    if (errorCount < 100)
                        Console.WriteLine("skipping:'" + tf[i] + "'");
                    continue;
                }
                DateTime t = ParseDate(tokens[0]);

                if (columnName.Trim().ToLower() != "prcp")
                {
                    t = t.AddDays(-1);
                }
                if (t < t1 || t >= t2)
                    continue;


                double num = 0;
                if (!Double.TryParse(tokens[idxData], out num))
                {
                    errorCount++;
                    if (errorCount < 100)
                    {
                        Console.WriteLine("Error parsing '" + tokens[idxData] + "'");
                        Console.WriteLine("from line " + tf[i]);
                    }
                    AddMissing(t);
                    continue;
                }
                Add(t, num);
            }
        }

        private static DateTime ParseDate(string d)
        {
            int m = int.Parse(d.Substring(0, 2));
            int day = int.Parse(d.Substring(2, 2));
            int y = int.Parse(d.Substring(4, 2));

            if (y > 50)
                y += 1900;
            else
                y += 2000;

            DateTime t = new DateTime(y, m, day);
            return t;
        }
    }
}
