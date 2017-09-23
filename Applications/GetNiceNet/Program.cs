using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Reclamation.Core;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using Math = Reclamation.TimeSeries.Math;

namespace GetNiceNet
{
    class Program
    {      
        private static void Usage()
        {
            Console.WriteLine(
                "Usage: GetNiceNet --config=nicenet_config.csv --output=instant_dri_data.txt [--cbtt=ABEI]  --back=4");
            Console.WriteLine("Where: nicenet_config.csv is a catalog of sites to import");
            Console.WriteLine("       instant_dri_data.txt is the name of the output file to be generated");
            Console.WriteLine("      back=4 indicates get the last four hours of data (default=2)");
            Console.WriteLine("       cbtt (optional) represents a single site for import");
            Console.WriteLine("");
        }

        static void Main(string[] argList)
        {
            //Logger.EnableLogger(); 

            //These could be populated from a config file
            string[] HydrometParameterCodes = new string[] { "SI", "OBM", "TU", "WS", "WD", "WG", "BP", "PC", "TP" };
            string[] NiceNetParameterNames = new string[]
            {
                "SolarRadKw/m2", 
                "AveAirTempF", 
                "AveRelHum%",                
                "AveWindSpeedmph",                
                "VectorWindDirdeg",                
                "MaxWindGustmph",                
                "BaroPressmb",               
                "TotalPrecipin",
                "DewPointTemp"
            };
            double[] NiceNetParameterUpperLimits = new double[] { 120, 120, 101, 100, 360, 100, 32, 51, 100 };
            double[] NiceNetParameterLowerLimits = new double[] { 0, -50, 0, 0, 0, 0, 27, 0, -35 };
            //SI comes in as Kw/m2 and is converted to ly/hr
            string[] NiceNetParameterUnits = new string[] { "ly/hr", "deg F", "%", "mph", "deg", "mph", "mb", "in", "deg F" };
            
            var ParameterSet = new Dictionary<string,Parameter>();

            for (int i = 0; i < HydrometParameterCodes.Length; i++)
            {            
                var tempParameter = new Parameter();
                tempParameter.UpperLimitValue = NiceNetParameterUpperLimits[i];
                tempParameter.LowerLimitValue = NiceNetParameterLowerLimits[i];
                tempParameter.UpperLimitFlag = "+";
                tempParameter.LowerLimitFlag = "-";
                tempParameter.Name = NiceNetParameterNames[i];
                tempParameter.Units = NiceNetParameterUnits[i];
                tempParameter.Code = HydrometParameterCodes[i];
                ParameterSet.Add(HydrometParameterCodes[i], tempParameter);
            }
        
            var titleSeparator = new string[] { " ", "." };

            if (argList.Length == 0)
            {
                Usage();
                return;
            }

            Arguments args = new Arguments(argList);

            if (!args.Contains("config"))
            {
                Console.WriteLine("Error: --config=filename.csv is required");
                Usage();
                return;
            }

            if (!args.Contains("output"))
            {
                Console.WriteLine("Error: --output=filename.txt is required");
                Usage();
                return;
            }

            int hoursBack = 4;
            if (args.Contains("back"))
            {
                hoursBack = int.Parse(args["back"]);
            }
            //FileUtility.CleanTempPath();

            //Read config file and filter on cbtt if a particular one is specified.  
            DataTable csv = new CsvFile(args["config"], CsvFile.FieldTypes.AllText);

            if (args.Contains("cbtt"))
            {
                Console.WriteLine("Filtering for cbtt = '" + args["cbtt"] + "'");
                csv = DataTableUtility.Select(csv, "cbtt='" + args["cbtt"] + "'", "");
            }

            var rows = csv.Select();  //Selects all rows of config file, excluding the header row
            Console.WriteLine("Processing data for " + rows.Length + " site(s)");

            Dictionary<string,string> bumParameters = new Dictionary<string,string>();
            List<String> bumSites = new List<string>();
            
            // Begin loop to read in data for each site.  
            for (int i = 0; i < rows.Length; i++)
            {
                //Get site/cbtt from config file
                var site = rows[i]["dri_id"].ToString();
                var cbtt = rows[i]["cbtt"].ToString();
                var obm = new Series();
                var tu = new Series();

                Console.WriteLine("Processing site " + site + "/" + cbtt);

                // example http://www.wrcc.dri.edu/cgi-bin/nclvLIST.pl
                string url = "http://www.wrcc.dri.edu/cgi-bin/" + site.ToLower() + "LIST.pl";

                //Catch missing web pages
                try
                {
                    string[] dataPage = Web.GetPage(url);
                    
                    //Work through unwanted header rows of NiceNet data file.
                    int j = 0;
                    while (dataPage[j].Contains("<")) j++;

                    //Get column header names
                    //Revise this so that column order can change with out breaking the code.  Might need to assume fixed width.
                    var columnNames = dataPage[j].Split(titleSeparator, StringSplitOptions.RemoveEmptyEntries).ToList();
                    j++;
                    while (!dataPage[j].Contains("--"))
                    {
                        var tempColumnNames = dataPage[j].Split(titleSeparator, StringSplitOptions.RemoveEmptyEntries).ToList();
                        if (tempColumnNames.Contains("Year")) tempColumnNames.Insert(2,"");
                        for (int n = 0; n < tempColumnNames.Count; n++)
                        {
                            columnNames[n] = columnNames[n] + tempColumnNames[n];
                        }
                        j++;
                    }

                    //Get year from header
                    var lastHeaderRow = j - 1;
                    var startDataRow = j + 1;
                    var dataHeaderRow = dataPage[lastHeaderRow].Split(titleSeparator, StringSplitOptions.RemoveEmptyEntries).ToList();
                    var year = Int32.Parse(dataHeaderRow[0]);

                   
                    //Process data for each parameter of current site and save to hydrometfile
                    //First two parameters are day and time
                    foreach (var p in ParameterSet)
                    {
                        //Assign PCode and look for missing/unexpected parameters in the 
                        //data file.
                        //Skip dew point (TP).  It is not in the input file.  It is calculated later.  
                        if (p.Key == "TP")
                            continue;
                        
                            try
                            {
                                var pCode = p.Key;
                                var dataIndex = columnNames.IndexOf(p.Value.Name);
                                var unitType = p.Value.Units;

                                var s = ProcessDataSeries(cbtt, year, unitType, pCode, dataIndex, dataPage, startDataRow);
                                s = ApplyFlags(s, p.Value);
                                if (pCode == "OBM")
                                {
                                    obm = s;
                                }
                                if (pCode == "TU")
                                {
                                    tu = s;
                                }
                                //Only load the last hoursBack hours of data
                               s.Trim(DateTime.Now.AddHours(-hoursBack), DateTime.Now.AddHours(2));
                                HydrometInstantSeries.WriteToHydrometFile(s, cbtt, pCode, "nicenet", args["output"], true);
                            }
                            catch
                            {
                                Console.WriteLine("Parameter \"" + p + "\" is not recognized.");
                            }                    
                             
                    }    
                    var tp = DewPointCalculation(obm, tu, cbtt);
                    if (tp.Count > 0)
                    {
                        tp = ApplyFlags(tp, ParameterSet["TP"]);
                        HydrometInstantSeries.WriteToHydrometFile(tp, cbtt, "TP", "nicenet", args["output"], true);
                    }
                    else
                    {
                        Console.WriteLine("Dew point temperature could not be calculated.");
                    }                  
                }
                catch ( Exception e)
                {
                    Console.WriteLine(e.Message);
                    bumSites.Add(site);
                }                
            }

            if (bumSites.Count >= 1) 
            {
                Console.WriteLine("The following sites were not found:");
                Console.WriteLine(String.Join("\n", bumSites.ToArray()));
            }
        }

        private static Series ProcessDataSeries(string cbtt, int year, string units, string parameter, 
            int dataColumnIndex, string[] dataPage, int startDataRow)
        {
            // Parse data row by row for specified parameter
            var stringSeparator = new string[] {" "};
            var timeSeparator = new char[] {':'};

            Series rval = new Series(units, TimeInterval.Irregular);
            rval.Parameter = parameter;
            rval.SiteID = cbtt;

            for (int rowIndex = startDataRow; rowIndex <= dataPage.Length; rowIndex++)
            {
                if (dataPage[rowIndex].Contains('<')) break;
                var dataRow = dataPage[rowIndex].Split(stringSeparator, StringSplitOptions.RemoveEmptyEntries);
                var julianDay = Double.Parse(dataRow[0]);
                var timeArray = dataRow[1].Split(timeSeparator);
                var hour = Int32.Parse(timeArray[0]);
                var minute = Int32.Parse(timeArray[1]);

                DateTime dataDateTime = new DateTime(year,1,1,hour,minute,0).AddDays(julianDay-1);
                var dataValue = double.Parse(dataRow[dataColumnIndex]);
                
                //Convert from Kw/m^2 to Lang/hr
                if (dataColumnIndex == 2) dataValue *= 85.985;
                //Convert from mb to inHg
                if (dataColumnIndex == 8) dataValue *= 0.0295300;

                Point dataRecord = new Point(dataDateTime, dataValue);

                rval.Add(dataRecord);
            }

            return rval;
        }

        private static Series ApplyFlags(Series inputSeries, Parameter parameter)
        {
            var flaggedSeries = new Series();
            flaggedSeries = inputSeries;
            //var flagList = new NiceNetParameters();
            
            for (int i = 0; i < inputSeries.Count; i++)
            {
                Point dataRecord = new Point();
                dataRecord = flaggedSeries[i];
                var highLimit = parameter.UpperLimitValue;
                var lowLimit = parameter.LowerLimitValue;
                var highFlag = parameter.UpperLimitFlag;
                var lowFlag = parameter.LowerLimitFlag;
                if (dataRecord.Value < lowLimit) dataRecord.Flag = lowFlag;
                if (dataRecord.Value > highLimit) dataRecord.Flag = highFlag;
                flaggedSeries[i] = dataRecord;
            }
            return flaggedSeries;
        }

        private static Series DewPointCalculation(Series ob, Series tu, string cbtt)
        {
            Series tp = new Series();

            if (tu.SiteID == cbtt && ob.SiteID == cbtt && tu.Count > 0 && ob.Count > 0)
            {
                for (int i = 0; i < ob.Count; i++)
                {
                    var pt = ob[i];
                    if (tu.IndexOf(pt.DateTime) >= 0)
                    {
                        double tpVal = AsceEtCalculator.DewPtTemp(pt.Value, tu[pt.DateTime].Value);
                        //Attach appropriate flag for TP
                        if (!double.IsNaN(tpVal)) 
                        {                                               
                            tp.Add(pt.DateTime, tpVal);
                        }
                    }
                }
            }
            return tp;
        }
    }
}
