using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Reclamation.TimeSeries.Hydross
{
    public class ODSFuncs
    {
        public enum dataClassEnum
        {
            STATION, RESERVOIR, POWER, DIVERSION, IFR, FLOW, ALL, YEARS_ONLY
        };

        public static string[] var_codes = {
            "110",	/* index = 0  */
            "111",	/* index = 1  */
            "112",	/* index = 2  */
            "113",	/* index = 3  */
            "114",	/* index = 4  */
            "120",	/* index = 5  */
            "121", 	/* index = 6  */
            "122",	/* index = 7  */
            "125",	/* index = 8  */
            "130",	/* index = 9  */
            "131",	/* index = 10 */
            "132",	/* index = 11 */
            "140",	/* index = 12 */
            "214",	/* index = 13 */
            "220",	/* index = 14 */
            "221",	/* index = 15 */
            "222",	/* index = 16 */
            "223",	/* index = 17 */
            "224",	/* index = 18 */
            "225",	/* index = 19 */
            "226",	/* index = 20 */
            "227",	/* index = 21 */
            "228",	/* index = 22 */
            "229",	/* index = 23 */
            "230",	/* index = 24 */
            "231",	/* index = 25 */
            "232",	/* index = 26 */
            "233",	/* index = 27 */
            "310",	/* index = 28 */
            "311",	/* index = 29 */
            "312",	/* index = 30 */
            "320",	/* index = 31 */
            "321",	/* index = 32 */
            "410",	/* index = 33 */
            "411",  /* index = 34 */
            "412",	/* index = 35 */
            "420",	/* index = 36 */
            "421",	/* index = 37 */
            "422",  /* index = 38 */
            "423",  /* index = 39 */
            "430",	/* index = 40 */
            "510",	/* index = 41 */
            "512",	/* index = 42 */
            "610",	/* index = 43 */
            "611",  /* index = 44 */
            "620",	/* index = 45 */
            "621",	/* index = 46 */
            "622",	/* index = 47 */
            "630",	/* index = 48 */
            "631",	/* index = 49 */
            "632"   /* index = 50 */
                                             };

        public static string[] var_codes_str = {
  "Runoff",	/* index = 0  */
  "Local flow",	/* index = 1  */
  "Other gain",	/* index = 2  */
  "Return flow inflows",	/* index = 3  */
  "Upstream inflows",	/* index = 4  */
  "Total natural inflow",	/* index = 5  */
  "Total project inflow",	/* index = 6  */
  "Total inflow",	/* index = 7  */
  "Other loss",	/* index = 8  */
  "Pumping schedule",	/* index = 9  */
  "Diversion capacity",	/* index = 10 */
  "Channel capacity",	/* index = 11 */
  "Total diversion",	/* index = 12 */
  "Res evaporation table",	/* index = 13 */
  "Res max content",	/* index = 14 */
  "Res min content",	/* index = 15 */
  "Res target content",	/* index = 16 */
  "Res BOM content",	/* index = 17 */
  "Res EOM content",	/* index = 18 */
  "Max discharge",	/* index = 19 */
  "BOM elevation",	/* index = 20 */
  "EOM elevation",	/* index = 21 */
  "Elev at avg cont",	/* index = 22 */
  "EOM surface area",	/* index = 23 */
  "Evaporation",	/* index = 24 */
  "Discharge",	/* index = 25 */
  "Spill",	/* index = 26 */
  "Seepage",	/* index = 27 */
  "Power requirement",	/* index = 28 */
  "Power generated",	/* index = 29 */
  "Power discharged",	/* index = 30 */
  "Tailwater elevation",	/* index = 31 */
  "Power plant efficiency",	/* index = 32 */
  "Diversion requirement",	/* index = 33 */
  "Diversion flow",	/* index = 34 */
  "Diversion shortage",	/* index = 35 */
  "Unit requirement table",	/* index = 36 */
  "Canal efficiency",	/* index = 37 */
  "Canal capacity",	/* index = 38 */
  "% consumed",	/* index = 39 */
  "Return flows",	/* index = 40 */
  "IFR requirement",	/* index = 41 */
  "IFR shortage",	/* index = 42 */
  "Station flow",	/* index = 43 */
  "Available flow",	/* index = 44 */
  "Reach efficiency",	/* index = 45 */
  "Reach loss",	/* index = 46 */
  "Reach loss RTN flows",	/* index = 47 */
  "Natural flow to next sta",	/* index = 48 */
  "Project flow to next sta",	/* index = 49 */
  "Total flow to next sta",	/* index = 50 */
};

        // Indices into the units[] string array.
        public static int[] var_units = {
  52,	/* index = 0  */
  52,	/* index = 1  */
  52,	/* index = 2  */
  52,	/* index = 3  */
  52,	/* index = 4  */
  52,	/* index = 5  */
  52,	/* index = 6  */
  52,	/* index = 7  */
  52,	/* index = 8  */
  52,	/* index = 9  */
  52,	/* index = 10 */
  52,	/* index = 11 */
  52,	/* index = 12 */
  14,	/* index = 13 */
  34,	/* index = 14 */
  34,	/* index = 15 */
  34,	/* index = 16 */
  34,	/* index = 17 */
  34,	/* index = 18 */
  52,	/* index = 19 */
  14,	/* index = 20 */
  14,	/* index = 21 */
  14,	/* index = 22 */
  34,	/* index = 23 */
  52,	/* index = 24 */
  52,	/* index = 25 */
  52,	/* index = 26 */
  52,	/* index = 27 */
  72,	/* index = 28 */
  72,	/* index = 29 */
  52,	/* index = 30 */
  14,	/* index = 31 */
  60,	/* index = 32 */
  52,	/* index = 33 */
  52,   /* index = 34 */
  52,	/* index = 35 */
  14,	/* index = 36 */
  60,	/* index = 37 */
  52,   /* index = 38 */
  60,	/* index = 39 */
  52,	/* index = 40 */
  52,	/* index = 41 */
  52,	/* index = 42 */
  52,	/* index = 43 */
  52,   /* index = 44 */
  60,	/* index = 45 */
  52,	/* index = 46 */
  52,	/* index = 47 */
  52,	/* index = 48 */
  52,	/* index = 49 */
  52,	/* index = 50 */
                                              };

        public static int[] all_vars = {
  0, 50,
                                          };

        public static int[] sta_vars = {
  0, 12
};

        public static int[] res_vars = {
  13, 27
};

        public static int[] pow_vars = {
  28, 32
};

        public static int[] div_vars = {
  33, 40
};

        public static int[] ifr_vars = {
  41, 42
};

        public static int[] flo_vars = {
  43, 50
};


        public static string[] units = {
                 null, null, null, null, null, null, null, null, null, null,
                 "mm", "m", "km", "in", "ft", "mi", null, null, null, null,
                 "m^2", "ha", "km^2", "kha", "ft^2", "ac", "ka", null, null, null,
                 "m^3", "dam^3", "kdam^3", "ft^3", "af", "kaf", null, null, null, null,
                 "m^3/sec", "m^3/day", "m^3/month", "dam^3/sec", "dam^3/day", "dam^3/month", "kdam^3/month", "cfs", "cfd", "cfm",
                 "afs", "afd", "af/mo", "kaf/mo", null, null, null, null, null, null,
                 "%", null, null, null, null, null, null, null, null, null,
                 "Kwh/day", "Kwh/month", "Mwh/month", "Gwh/month"
                                             };
        public static string[] flow_strings = {
                "Natural", "Project",
                "Site Loss -> Natural", "Site Loss -> Project",
                "Canal Loss -> Natural", "Canal Loss -> Project"
                                       };


        public static bool GetVarcodesForAll(List<string> scenarios,
            out Dictionary<string, HashSet<int>> stationID_Varcodes,
            out Dictionary<string, List<string>> station113_Diversions,
            out Dictionary<string, HashSet<int>> diversion_Varcodes,
            out Dictionary<string, HashSet<int>> IFR_Varcodes,
            out Dictionary<string, string> diversionTitles,
            out Dictionary<string, string> ifrTitles)
        {
            stationID_Varcodes = new Dictionary<string, HashSet<int>>();
            station113_Diversions = new Dictionary<string, List<string>>();
            diversion_Varcodes = new Dictionary<string, HashSet<int>>();
            IFR_Varcodes = new Dictionary<string, HashSet<int>>();
            diversionTitles = new Dictionary<string, string>();
            ifrTitles = new Dictionary<string, string>();

            // This keeps track of the current year being outputted.  When a new
            //   year is reached in the ods file, the temp file for the new year
            //   is opened for output.
            int last_output_year = 0;

            // Build the output data set.
            foreach (string scenBaseName in scenarios)
            {
                /* 
                 *        Open the .ods file for the current scenario.
                 */

                FileInfo fi = new FileInfo(scenBaseName+ ".ods");
                if (!fi.Exists)
                {
                    Console.WriteLine(string.Format("{0} does not exist.", fi.FullName));
                    return false;
                }
                StreamReader sr = new StreamReader(fi.FullName);

                string scenName = fi.Name;

                // Read the year from the first line.
                string input = sr.ReadLine();

                // The year is the last two items
                string[] tokens = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int scenStartYear = int.Parse(tokens[tokens.Length - 2]);
                int scenEndYear = int.Parse(tokens[tokens.Length - 1]);

                // The next six lines are skipped.
                for (int i = 1; i < 6; i++)
                {
                    sr.ReadLine();
                }

                // If we come across a new station, then add a new record to station
                //   and varcodes.  Otherwise append to the current varcode list.
                string lastStation = "";

                // Do the same for Diversions and IFRs.
                string lastDiversion = "";
                string lastIFR = "";

                while (!sr.EndOfStream)
                {
                    string strLine = sr.ReadLine();

                    int currentYear = int.Parse(strLine.Substring(0, 4));

                    // Check if we can stop searching (only need to scan the first year
                    //   of data).
                    if (last_output_year == 0) last_output_year = currentYear;
                    if (last_output_year != currentYear)
                    {
                        break;
                    }

                    int currentcode = int.Parse(strLine.Substring(15, 3));

                    if (currentcode == 100)
                    {
                        lastStation = strLine.Substring(4, 6);
                    }
                    else if (currentcode < 400)
                    {
                        // This is a station code.
                        if (!stationID_Varcodes.ContainsKey(lastStation))
                        {
                            stationID_Varcodes.Add(lastStation, new HashSet<int>());
                        }
                        stationID_Varcodes[lastStation].Add(currentcode);

                        if (currentcode == 113)
                        {
                            // This is a diversion inflow code.
                            string divname = "d";
                            divname += strLine.Substring(24, 3);
                            divname += "_" + strLine.Substring(27, 1);

                            if (!station113_Diversions.ContainsKey(lastStation))
                            {
                                station113_Diversions.Add(lastStation, new List<string>());
                            }
                            station113_Diversions[lastStation].Add(divname);
                        }
                    }
                    else if (currentcode == 400)
                    {
                        // This is a new diversion.
                        lastDiversion = strLine.Substring(11, 4);

                        // Store full name of diversion
                        string divname = strLine.Substring(84, strLine.Length - 84);
                        divname = divname.Trim();

                        if (!diversionTitles.ContainsKey(lastDiversion))
                        {
                            diversionTitles.Add(lastDiversion, "");
                        }
                        diversionTitles[lastDiversion] = divname;
                    }
                    else if (currentcode < 500)
                    {
                        // This is a diversion code.
                        if (!diversion_Varcodes.ContainsKey(lastDiversion))
                        {
                            diversion_Varcodes.Add(lastDiversion, new HashSet<int>());
                        }
                        diversion_Varcodes[lastDiversion].Add(currentcode);
                    }
                    else if (currentcode == 500)
                    {
                        // This is a new IFR.
                        lastIFR = strLine.Substring(11, 4);
                        lastIFR = lastIFR.Trim();

                        // Store full name of IFR
                        string ifrname = strLine.Substring(37, 40);
                        ifrname = ifrname.Trim();

                        if (!ifrTitles.ContainsKey(lastIFR))
                        {
                            ifrTitles.Add(lastIFR, "");
                        }
                        ifrTitles[lastIFR] = ifrname;
                    }
                    else if (currentcode < 600)
                    {
                        // This is an IFR code.
                        if (!IFR_Varcodes.ContainsKey(lastIFR))
                        {
                            IFR_Varcodes.Add(lastIFR, new HashSet<int>());
                        }
                        IFR_Varcodes[lastIFR].Add(currentcode);
                    }
                    else
                    {			// >= 600
                        // Flow code, add to station.
                        if (!stationID_Varcodes.ContainsKey(lastStation))
                        {
                            stationID_Varcodes.Add(lastStation, new HashSet<int>());
                        }
                        stationID_Varcodes[lastStation].Add(currentcode);
                    }
                }
            }

            return true;
        }

        public static bool GetVarcodesForAll(List<string> scenarios,
            out Dictionary<int, HashSet<string>> var_station,
            out Dictionary<int, HashSet<string>> var_div,
            out Dictionary<int, HashSet<string>> var_IFR)
        {
            var_station = new Dictionary<int, HashSet<string>>();
            var_div = new Dictionary<int, HashSet<string>>();
            var_IFR = new Dictionary<int, HashSet<string>>();

            // This keeps track of the current year being outputted.  When a new
            //   year is reached in the ods file, the temp file for the new year
            //   is opened for output.
            int last_output_year = 0;

            // Build the output data set.
            foreach (string scenBaseName in scenarios)
            {
                /* 
                 *        Open the .ods file for the current scenario.
                 */

                FileInfo fi = new FileInfo(scenBaseName + ".ods");
                if (!fi.Exists)
                {
                    Console.WriteLine(string.Format("{0} does not exist.", fi.FullName));
                    return false;
                }
                StreamReader sr = new StreamReader(fi.FullName);

                string scenName = fi.Name;

                // Read the year from the first line.
                string input = sr.ReadLine();

                // The year is the last two items
                string[] tokens = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int scenStartYear = int.Parse(tokens[tokens.Length - 2]);
                int scenEndYear = int.Parse(tokens[tokens.Length - 1]);

                // The next six lines are skipped.
                for (int i = 1; i < 6; i++)
                {
                    sr.ReadLine();
                }

                // If we come across a new station, then add a new record to station
                //   and varcodes.  Otherwise append to the current varcode list.
                string lastStation = "";

                // Do the same for Diversions and IFRs.
                string lastDiversion = "";
                string lastIFR = "";

                while (!sr.EndOfStream)
                {
                    string strLine = sr.ReadLine();

                    int currentYear = int.Parse(strLine.Substring(0, 4));

                    // Check if we can stop searching (only need to scan the first year
                    //   of data).
                    if (last_output_year == 0) last_output_year = currentYear;
                    if (last_output_year != currentYear)
                    {
                        break;
                    }

                    int currentcode = int.Parse(strLine.Substring(15, 3));

                    if (currentcode == 100)
                    {
                        lastStation = strLine.Substring(4, 6);
                    }
                    else if (currentcode < 400)
                    {
                        // This is a station code.
                        if (!var_station.ContainsKey(currentcode))
                        {
                            var_station.Add(currentcode, new HashSet<string>());
                        }
                        var_station[currentcode].Add(lastStation);
                    }
                    else if (currentcode == 400)
                    {
                        // This is a new diversion.
                        lastDiversion = strLine.Substring(11, 4);
                    }
                    else if (currentcode < 500)
                    {
                        // This is a diversion code.
                        if (!var_div.ContainsKey(currentcode))
                        {
                            var_div.Add(currentcode, new HashSet<string>());
                        }
                        var_div[currentcode].Add(lastDiversion);
                    }
                    else if (currentcode == 500)
                    {
                        // This is a new IFR.
                        lastIFR = strLine.Substring(11, 4);
                        lastIFR = lastIFR.Trim();
                    }
                    else if (currentcode < 600)
                    {
                        // This is an IFR code.
                        if (!var_IFR.ContainsKey(currentcode))
                        {
                            var_IFR.Add(currentcode, new HashSet<string>());
                        }
                        var_IFR[currentcode].Add(lastIFR);
                    }
                    else
                    {			// >= 600
                        // Flow code, add to station.
                        if (!var_station.ContainsKey(currentcode))
                        {
                            var_station.Add(currentcode, new HashSet<string>());
                        }
                        var_station[currentcode].Add(lastStation);
                    }
                }
            }

            return true;
        }

        public static List<string> GetScenarioStations(dataClassEnum dataClass, List<string> scenarioList)
        {
            int startYear = 0;
            int endYear = 0;
            return GetScenarioStations(dataClass, scenarioList, out startYear, out endYear);
        }

        public static List<string> GetScenarioStations(dataClassEnum dataClass,
            List<string> scenarioList,
            out int startYear, out int endYear)
        {
            // Taken from HSS's UPDATE_station_list.c

            /* 
             * Loop through each scenario. 
             * 1. Open the ns file for the scenario.
             * 2. Depending on the data class [e.g. ifr],
             *    grab the stations for the scenario and
             *    add to the station_list widget.
             * 3. Close the ns file.
             */
            List<string> stationList = new List<string>();
            startYear = -1;
            endYear = -1;

            int indexPtr = 0;

            // The last element of indices is -1, so if we hit this value, bail out.
            foreach (string scenBaseName in scenarioList)
            {
                // Open the .ods file to get the min and max year for this scenario.
                FileInfo fi = new FileInfo(scenBaseName + ".ods");
                if (!fi.Exists)
                {
                    Console.WriteLine(string.Format("File {0} does not exist.", fi.FullName));
                    return null;
                }

                StreamReader sr = new StreamReader(fi.FullName);

                // Read the year from the first line.
                string input = sr.ReadLine();
                // The year is the last two items
                string[] tokens = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int scenStartYear = int.Parse(tokens[tokens.Length - 2]);
                int scenEndYear = int.Parse(tokens[tokens.Length - 1]);

                if (scenStartYear > startYear)
                    startYear = scenStartYear;
                if (scenEndYear < endYear)
                    endYear = scenEndYear;

                sr.Close();

                // Bail out if we are looking for dates.
                if (dataClass == dataClassEnum.YEARS_ONLY)
                    return stationList;

                /* 
                 *        Open the .ns file for the current scenario.
                 */

                fi = new FileInfo(scenBaseName + ".ns");
                if (!fi.Exists)
                {
                    Console.WriteLine(string.Format("File {0} does not exist.", fi.FullName));
                    return null;
                }
                sr = new StreamReader(fi.FullName);

                // The stations will be accumulated in a list which will later be ORed
                //   with the total list.
                List<string> scenarioStationList = new List<string>();

                /* 
                 * Add new stations to the station list.
                 */
                switch (dataClass)
                {

                    /* 
                     * For variable groups station, flow, and all,
                     * all station nodes are presented in the
                     * list.
                     */
                    case dataClassEnum.STATION:
                    case dataClassEnum.FLOW:
                    case dataClassEnum.ALL:
                        {
                            while (!sr.EndOfStream)
                            {
                                string strLine = sr.ReadLine();

                                if (strLine.IndexOf("STA") == 0)
                                {
                                    string id = strLine.Substring(4, 6);
                                    string name = strLine.Substring(37, 40);
                                    if (scenarioStationList.IndexOf(name) < 0)
                                    {
                                        scenarioStationList.Add(id + "-" + name);
                                    }
                                }
                            }
                        }
                        break;

                    /* 
                     * For variable groups reservoir, power, ifr,
                     * and diversion, only the station nodes where the
                     * appropriate subgroups exist are presented in
                     * the list.
                     */
                    case dataClassEnum.RESERVOIR:
                    case dataClassEnum.POWER:
                    case dataClassEnum.IFR:
                    case dataClassEnum.DIVERSION:
                        {
                            string[] dataClassStringAbrev = { "STA", "RES", "PWR", "DV1", "IFR" };

                            while (!sr.EndOfStream)
                            {
                                string stationName = "";
                                string stationID = "";

                                string strLine = sr.ReadLine();

                                if (strLine.IndexOf("STA") == 0)
                                {
                                    stationID = strLine.Substring(4, 6);
                                    stationName = strLine.Substring(37, 40);
                                }
                                else if (strLine.IndexOf(dataClassStringAbrev[(int)dataClass]) == 0)
                                {
                                    string name_and_id = stationID + "-" + stationName;
                                    if (scenarioStationList.IndexOf(name_and_id) < 0)
                                    {
                                        scenarioStationList.Add(name_and_id);
                                    }
                                }
                            }
                        }
                        break;
                }
                sr.Close();

                // Initialize or pare the station list.
                if (indexPtr > 0)
                {
                    // OR the total list and the scenario list.  Save in a temp
                    //   list, which will later be saved as the new total list.
                    List<string> templist = new List<string>();

                    foreach (string station in stationList)
                    {
                        // Look for an occurrance of the station from another scenario.
                        if (scenarioStationList.IndexOf(station) >= 0)
                        {
                            // Save any intersections.
                            templist.Add(station);
                        }
                    }

                    // Update the total list from the temp list.
                    stationList.Clear();
                    stationList.AddRange(templist);
                }
                else
                {
                    // Copy the scenario list into the total list.
                    stationList.AddRange(scenarioStationList);
                }

                indexPtr++;
            }

            return stationList;
        }

        public static List<string> GetDiversionList(dataClassEnum dataClass, List<string> scenarioList,
                    List<string> stationList)
        {
            // Taken from HSS's UPDATE_diversion_list.c
            List<string> diversionList = new List<string>();

            int indexPtr = 0;

            // The last element of indices is -1, so if we hit this value, bail out.
            foreach (string scenBaseName in scenarioList)
            {
                /* 
                 *        Open the .ods file for the current scenario.
                 */
                FileInfo fi = new FileInfo(scenBaseName + ".ods");
                if (!fi.Exists)
                {
                    Console.WriteLine(string.Format("{0} does not exist.", fi.FullName));
                    return null;
                }
                StreamReader sr = new StreamReader(fi.FullName);

                string scenName = fi.Name;

                // Read the year from the first line.
                string input = sr.ReadLine();
                // The year is the last two items
                string[] tokens = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                int scenStartYear = int.Parse(tokens[tokens.Length - 2]);
                int scenEndYear = int.Parse(tokens[tokens.Length - 1]);

                // The next six lines are skipped.
                for (int i = 1; i < 6; i++)
                {
                    sr.ReadLine();
                }

                // The diversions will be accumulated in a list which will
                //    later be ORed with the total list.
                List<string> scenarioDiversionList = new List<string>();


                int currentYear = 0;
                switch (dataClass)
                {
                    case dataClassEnum.IFR:
                        while (!sr.EndOfStream && currentYear <= scenStartYear)
                        {
                            string strLine = sr.ReadLine();
                            string stationName = "";
                            string ifrName = "";
                            string title = "";

                            currentYear = int.Parse(strLine.Substring(0, 4));
                            int code = int.Parse(strLine.Substring(15, 3));

                            // Grab all ods records for ifrs.
                            if (code == 500 && currentYear == scenStartYear)
                            {
                                stationName = strLine.Substring(4, 6);
                                ifrName = strLine.Substring(11, 4);
                                title = strLine.Substring(37, 40);

                                // Check to see if the station is in the selected list.
                                if (stationList.IndexOf(stationName) >= 0)
                                {
                                    string newIFR = ifrName + "-" + title;
                                    if (scenarioDiversionList.IndexOf(newIFR) < 0)
                                    {
                                        scenarioDiversionList.Add(newIFR);
                                    }
                                }
                            }
                        }
                        break;
                    case dataClassEnum.DIVERSION:
                        while (!sr.EndOfStream && currentYear <= scenStartYear)
                        {
                            string strLine = sr.ReadLine();

                            currentYear = int.Parse(strLine.Substring(0, 4));
                            int code = int.Parse(strLine.Substring(15, 3));

                            // Grab all ods records for diversions.
                            if (code == 400 && currentYear == scenStartYear)
                            {
                                string stationName = strLine.Substring(4, 6);
                                string div = strLine.Substring(11, 4);
                                string title = strLine.Substring(84, 40);


                                // Check to see if the station is in the selected list.
                                if (stationList.IndexOf(stationName) >= 0)
                                {
                                    string newDiv = div + "-" + title;
                                    if (scenarioDiversionList.IndexOf(newDiv) < 0)
                                    {
                                        scenarioDiversionList.Add(newDiv);
                                    }
                                }
                            }
                        }
                        break;
                }
                sr.Close();

                // Initialize or pare the station list.
                if (indexPtr > 0)
                {
                    // OR the total list and the scenario's diversion list.
                    foreach (string station in diversionList)
                    {
                        // Look for this station in the current list.  IF it isn't there, 
                        //   then remove it from the list in order to only keep common
                        //   stations.
                        if (scenarioDiversionList.IndexOf(station) < 0)
                        {
                            // The station does not appear in both scenarios, so remove it.
                            int idx = diversionList.IndexOf(station);
                            diversionList.RemoveAt(idx);
                        }
                    }
                }
                else
                {
                    // Copy the scenario's diversion list into the total list.
                    diversionList.AddRange(scenarioDiversionList);
                }

                indexPtr++;
            }

            return diversionList;
        }

        public static List<string> GetDiversionList(ODSFuncs.dataClassEnum dataClass, string scenario, string stationID)
        {
            // Convert to MFC structures
            List<string> scenarios = new List<string>();
            scenarios.Add(scenario);

            List<string> stations = new List<string>();
            stations.Add(stationID);

            return GetDiversionList(dataClass, scenarios, stations);
        }

        public static int GetVarcodeIndexFromCode(int varcode)
        {
            for (int i = 0; i < 51; i++)
            {
                if (int.Parse(var_codes[i]) == varcode)
                {
                    return i;
                }
            }

            return -1;
        }

    }
}

