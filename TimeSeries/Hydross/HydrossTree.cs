using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydross;
using Reclamation.Core;
using System.Data;

namespace Reclamation.TimeSeries.Hydross
{
    public class HydrossTree
    {

        static string s_databasePath;
        /// <summary>
        /// Generate the tree.csv file for 2004 pisces.
        /// </summary>
        /// <param name="seriesCatalog">The seriesCatalog table that will be added to.</param>
        /// <param name="scenarioList">full path names of all scenarios to load into Pisces.
        ///  Do not include the .ods extension.</param>
        ///  <param name="startingSiteDataTypeID">The beginning site SiteDataTypeID</param>
        ///  <param name="topParentID">The ParentID of the folder that will contain the hydross subfolders.</param>
        /// <returns>The number of records added.</returns>
        public static int Generate(DataTable seriesCatalog, string databasePath, List<string> scenarioList,
            int startingSiteDataTypeID, int topParentID)
        {
            s_databasePath = databasePath;
            // Variable codes will be looked up using station ID.
            Dictionary<string, HashSet<int>> stationID_Varcodes;
            Dictionary<string, List<string>> station113_Diversions;
            Dictionary<string, HashSet<int>> diversion_Varcodes;
            Dictionary<string, HashSet<int>> IFR_Varcodes;
            Dictionary<string, string> diversionTitles;
            Dictionary<string, string> ifrTitles;
            if (!ODSFuncs.GetVarcodesForAll(scenarioList, out stationID_Varcodes, out station113_Diversions,
                  out diversion_Varcodes, out IFR_Varcodes, out diversionTitles, out ifrTitles))
            {
                return 0;
            }

            // It looks like the SiteDataTypeID is not autoincrement, so I'm taking care of that here.
            int ID = startingSiteDataTypeID;
            int hydrossID = AddRow(seriesCatalog, ID++, topParentID++, "Hydross");

            int subFolderID;

            foreach (string scenario in scenarioList)
            {
                FileInfo fi = new FileInfo(scenario);
                string scen_name = fi.Name;

                // Get the list of stations that have STATION variables..
                List<string> allStationsList = ODSFuncs.GetScenarioStations(ODSFuncs.dataClassEnum.STATION, scenarioList);

                //ostr.WriteLine("HydrossName,Description,Level,HydrossStation,HydrossVarcode,HydrossUnits,HydrossExtra1,HydrossExtra2");

                // Create station folder
                int stationFolderID = AddRow(seriesCatalog, ID++, hydrossID, "Stations");
                //ostr.WriteLine("Stations,,0,,,,");

                foreach (string stationName in allStationsList)
                {
                    string stationID = stationName.Substring(0, stationName.IndexOf('-'));

                    // Find the varcodes for this station.
                    HashSet<int> varcodes = stationID_Varcodes[stationID];

                    Debug.Assert(varcodes.Count != 0);

                    // Create station entries under the station folder.
                    subFolderID = AddRow(seriesCatalog, ID++, stationFolderID, stationName);
                    //ostr.WriteLine(string.Format("{0},,1,,,,", FormatCSVString(stationName)));

                    foreach (int varcode in varcodes)
                    {
                        int varcodeIdx = ODSFuncs.GetVarcodeIndexFromCode(varcode);
                        if (varcodeIdx >= 0)
                        {
                            AddRow(seriesCatalog, ID++, subFolderID, scenario, varcode.ToString(), ODSFuncs.var_codes_str[varcodeIdx], stationID, varcode.ToString(), ODSFuncs.units[ODSFuncs.var_units[varcodeIdx]]);
                            //ostr.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                            //    varcode, ODSFuncs.var_codes_str[varcodeIdx], 2, stationID, varcode, ODSFuncs.units[ODSFuncs.var_units[varcodeIdx]], "-", "-"));

                            // Some varcodes will have additional structures under them,
                            //   like diversions.
                            switch (varcode)
                            {
                                case 113:
                                    {
                                        foreach (string div in station113_Diversions[stationID])
                                        {
                                            string divcode = div.Substring(0, 4);
                                            int rftype = int.Parse(div.Substring(5, 1));
                                            // If no diversion specified, then leave it blank.
                                            string divname = divcode;
                                            if (diversionTitles.ContainsKey(divcode))
                                            {
                                                divname = diversionTitles[divcode];
                                            }
                                            AddRow(seriesCatalog, ID++, subFolderID, scenario, divcode, divname + " (" + ODSFuncs.flow_strings[rftype] + ")", stationID, varcode.ToString(),
                                                ODSFuncs.units[ODSFuncs.var_units[varcodeIdx]], divcode, rftype.ToString());
                                            //ostr.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                                            //    divcode, divname + " (" + ODSFuncs.flow_strings[rftype] + ")", 3, stationID, varcode,
                                            //    ODSFuncs.units[ODSFuncs.var_units[varcodeIdx]], divcode, rftype));
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        else Console.WriteLine(string.Format("Skipping code {0}.", varcode));
                    }

                    List<string> diversionList = ODSFuncs.GetDiversionList(ODSFuncs.dataClassEnum.DIVERSION, scenario, stationID);

                    if (diversionList.Count > 0)
                    {
                        // Display any diversions.
                        subFolderID = AddRow(seriesCatalog, ID++, stationFolderID, "Diversions");
                        //ostr.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                        //    400, "diversions", 2, stationID, 400, "-", "-", "-"));

                        foreach (string div in diversionList)
                        {
                            string divcode = div.Substring(0, 4);
                            string divname = div.Substring(5, div.Length - 5);

                            // Trim trailing spaces.
                            divname = divname.Trim();

                            // Make diversion name palatable to csv form.
                            //divname = '"' + divname.Replace('"', '\'') + '"';

                            AddRow(seriesCatalog, ID++, subFolderID, divname);
                            //ostr.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                            //    divcode, divname, 3, stationID, divcode, "-", div.Substring(0, 4), "-"));

                            varcodes = diversion_Varcodes[divcode];

                            foreach (int varcode in varcodes)
                            {
                                int varcodeIdx = ODSFuncs.GetVarcodeIndexFromCode(varcode);
                                if (varcodeIdx >= 0)
                                {
                                    AddRow(seriesCatalog, ID++, subFolderID, scenario,
                                        varcode.ToString(), ODSFuncs.var_codes_str[varcodeIdx], stationID, varcode.ToString(), ODSFuncs.units[ODSFuncs.var_units[varcodeIdx]],
                                        divcode, "-");
                                    //ostr.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                                    //    varcode, ODSFuncs.var_codes_str[varcodeIdx], 4, stationID, varcode, ODSFuncs.units[ODSFuncs.var_units[varcodeIdx]],
                                    //    divcode, "-"));

                                }
                            }
                        }
                    }

                    List<string> ifrList = ODSFuncs.GetDiversionList(ODSFuncs.dataClassEnum.IFR, scenario, stationID);

                    if (ifrList.Count > 0)
                    {
                        subFolderID = AddRow(seriesCatalog, ID++, stationFolderID, "Instream Flow Requirements");
                        //ostr.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                        //500, "Instream Flow Requirements", 2, stationID, 500, "-", "-", "-"));

                        foreach (string ifr in ifrList)
                        {
                            string ifrcode = ifr.Substring(0, 4);
                            ifrcode = ifrcode.Trim();

                            string ifrname = ifr.Substring(5, ifr.Length - 5);

                            subFolderID = AddRow(seriesCatalog, ID++, stationFolderID, ifrname);
                            //ostr.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                            //    ifrcode, FormatCSVString(ifrname), 3, stationID, ifrcode, "-", ifr.Substring(0, 4), "-"));

                            varcodes = IFR_Varcodes[ifrcode];

                            foreach (int varcode in varcodes)
                            {
                                int varcodeIdx = ODSFuncs.GetVarcodeIndexFromCode(varcode);
                                if (varcodeIdx >= 0)
                                {
                                    AddRow(seriesCatalog, ID++, subFolderID, scenario,
                                        varcode.ToString(), ODSFuncs.var_codes_str[varcodeIdx], stationID, varcode.ToString(),
                                        ODSFuncs.units[ODSFuncs.var_units[varcodeIdx]], ifrcode, "-");
                                    //ostr.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                                    //    varcode, ODSFuncs.var_codes_str[varcodeIdx], 4, stationID, varcode,
                                    //    ODSFuncs.units[ODSFuncs.var_units[varcodeIdx]], ifrcode, "-"));

                                }
                            }
                        }
                    }
                }

                // Now output same data but in variable-major format.
                int paramFolderID = AddRow(seriesCatalog, ID++, topParentID, "Parameters");
                //ostr.WriteLine("Parameters,,0,,,,");

                Dictionary<int, HashSet<string>> var_station = new Dictionary<int, HashSet<string>>();
                Dictionary<int, HashSet<string>> var_div = new Dictionary<int, HashSet<string>>();
                Dictionary<int, HashSet<string>> var_IFR = new Dictionary<int, HashSet<string>>();
                if (!ODSFuncs.GetVarcodesForAll(scenarioList, out var_station, out var_div, out var_IFR))
                {
                    Console.WriteLine("GetVarcodesForAll failed.");
                    return 0;
                }

                // First all station codes
                for (int icode = ODSFuncs.sta_vars[0]; icode <= ODSFuncs.pow_vars[1]; icode++)
                {
                    int varcode = int.Parse(ODSFuncs.var_codes[icode]);
                    int varcodeIdx = ODSFuncs.GetVarcodeIndexFromCode(varcode);

                    if (var_station.ContainsKey(varcode))
                    {
                        // There is at least one station 
                        subFolderID = AddRow(seriesCatalog, ID++, paramFolderID, varcode.ToString());
                        //ostr.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                        //    varcode, ODSFuncs.var_codes_str[varcodeIdx], 1, "", "", "", "", ""));

                        foreach (string stationName in allStationsList)
                        {
                            string stationID = stationName.Substring(0, stationName.IndexOf('-'));

                            if (var_station[varcode].Contains(stationID))
                            {
                                if (varcode != 113)
                                {
                                    AddRow(seriesCatalog, ID++, subFolderID, scenario,
                                        stationID, stationName, stationID, varcode.ToString(), ODSFuncs.units[ODSFuncs.var_units[varcodeIdx]]);
                                    //ostr.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                                    //   stationID, FormatCSVString(stationName), 2, stationID, varcode, ODSFuncs.units[ODSFuncs.var_units[varcodeIdx]], "-", "-"));
                                }
                                else
                                {
                                    List<string> diversionList = station113_Diversions[stationID];
                                    foreach (string di in diversionList)
                                    {
                                        string divcode = di.Substring(0, 4);
                                        int rftype = int.Parse(di.Substring(5, 1));
                                        if (diversionTitles.ContainsKey(divcode))
                                        {
                                            string divname = diversionTitles[divcode];

                                            AddRow(seriesCatalog, ID++, subFolderID, scenario,
                                                divcode, divname, stationID, varcode.ToString(), ODSFuncs.units[ODSFuncs.var_units[varcodeIdx]], divcode, rftype.ToString());
                                            //ostr.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                                            //    divcode, divname, 2, stationID, varcode, ODSFuncs.units[ODSFuncs.var_units[varcodeIdx]], divcode, rftype));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                // Diversions
                for (int icode = ODSFuncs.div_vars[0]; icode <= ODSFuncs.div_vars[1]; icode++)
                {
                    int varcode = int.Parse(ODSFuncs.var_codes[icode]);
                    int varcodeIdx = ODSFuncs.GetVarcodeIndexFromCode(varcode);

                    if (var_div.ContainsKey(varcode))
                    {
                        subFolderID = AddRow(seriesCatalog, ID++, paramFolderID, varcode.ToString());
                        //ostr.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                        //    varcode, ODSFuncs.var_codes_str[varcodeIdx], 1, "", "", "", "", ""));

                        foreach (string stationName in allStationsList)
                        {
                            string stationID = stationName.Substring(0, stationName.IndexOf('-'));

                            List<string> diversionList = ODSFuncs.GetDiversionList(ODSFuncs.dataClassEnum.DIVERSION, scenario, stationID);

                            foreach (string di in diversionList)
                            {
                                string divcode = di.Substring(0, 4);
                                string divname = di.Substring(5, di.Length - 5);

                                if (var_div[varcode].Contains(divcode))
                                {

                                    // Trim trailing spaces.
                                    divname = divname.Trim();

                                    // Make diversion name palatable to csv form.
                                    //divname = "\"" + divname.Replace('"', '\'') + "\"";

                                    AddRow(seriesCatalog, ID++, subFolderID, scenario,
                                        divcode, divname, stationID, divcode, "-", divcode, "-");
                                    //ostr.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                                    //    divcode, divname, 2, stationID, divcode, "-", divcode, "-"));
                                }
                            }
                        }
                    }
                }

                // Instream Flow Requirements
                for (int icode = ODSFuncs.ifr_vars[0]; icode <= ODSFuncs.ifr_vars[1]; icode++)
                {
                    int varcode = int.Parse(ODSFuncs.var_codes[icode]);
                    int varcodeIdx = ODSFuncs.GetVarcodeIndexFromCode(varcode);

                    if (var_IFR.ContainsKey(varcode))
                    {
                        subFolderID = AddRow(seriesCatalog, ID++, paramFolderID, "Instream Flow Requirements");
                        //ostr.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                        //    varcode, ODSFuncs.var_codes_str[varcodeIdx], 1, "-", "-", "-", "-", "-"));

                        foreach (string stationName in allStationsList)
                        {
                            string stationID = stationName.Substring(0, stationName.IndexOf('-'));

                            List<string> ifrList = ODSFuncs.GetDiversionList(ODSFuncs.dataClassEnum.IFR, scenario, stationID);

                            foreach (string ifr in ifrList)
                            {
                                string ifrcode = ifr.Substring(0, 4);
                                ifrcode = ifrcode.Trim();

                                if (var_IFR[varcode].Contains(ifrcode))
                                {
                                    string ifrname = ifr.Substring(5, ifr.Length - 5);
                                    // Trim trailing spaces.
                                    ifrname = ifrname.Trim();

                                    // Make diversion name palatable to csv form.
                                    //ifrname = "\"" + ifrname.Replace('"', '\'') + "\"";

                                    AddRow(seriesCatalog, ID++, subFolderID, scenario,
                                        varcode.ToString(), ODSFuncs.var_codes_str[varcodeIdx], stationID, varcode.ToString(), ODSFuncs.units[ODSFuncs.var_units[varcodeIdx]], ifrcode, "-");
                                    //ostr.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                                    //    varcode, ODSFuncs.var_codes_str[varcodeIdx], 2, stationID, varcode, ODSFuncs.units[ODSFuncs.var_units[varcodeIdx]], ifrcode, "-"));
                                }
                            }
                        }
                    }
                }

                // Rest of the station codes
                for (int icode = ODSFuncs.flo_vars[0]; icode <= ODSFuncs.flo_vars[1]; icode++)
                {
                    int varcode = int.Parse(ODSFuncs.var_codes[icode]);
                    int varcodeIdx = ODSFuncs.GetVarcodeIndexFromCode(varcode);

                    if (var_station.ContainsKey(varcode))
                    {
                        subFolderID = AddRow(seriesCatalog, ID++, paramFolderID, varcode.ToString());
                        //ostr.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                        //varcode, ODSFuncs.var_codes_str[varcodeIdx], 1, "", "", "", "", ""));

                        foreach (string stationName in allStationsList)
                        {
                            string stationID = stationName.Substring(0, stationName.IndexOf('-'));

                            if (var_station[varcode].Contains(stationID))
                            {
                                AddRow(seriesCatalog, ID++, subFolderID, scenario,
                                    stationName, "", stationID, varcode.ToString(), ODSFuncs.units[ODSFuncs.var_units[varcodeIdx]], "-", "_");
                                //ostr.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7}",
                                //    FormatCSVString(stationName), "", 2, stationID, varcode, ODSFuncs.units[ODSFuncs.var_units[varcodeIdx]], "-", "_"));
                            }
                        }
                    }
                }
            }

            return ID - startingSiteDataTypeID;
        }

        /// <summary>
        /// Alter the string so that it can be read by a CSV parser.
        /// </summary>
        /// <param name="csv_str"></param>
        /// <returns></returns>
        private static string FormatCSVString(string csv_str)
        {
            // Trim trailing spaces.
            csv_str = csv_str.Trim();

            // Change any double-quotes to a pair of apostrophes and surround by double-quotes
            csv_str = '"' + csv_str.Replace('"', '\'') + '"';

            return csv_str;
        }

        /// <summary>
        /// Adds a folder entry to the seriesCatalog.
        /// </summary>
        /// <param name="seriesCatalog"></param>
        /// <param name="ID"></param>
        /// <param name="parentID"></param>
        /// <param name="folderName"></param>
        /// <returns>The ID of the folder to be used as the parentID for subitems.</returns>
        private static int AddRow(DataTable seriesCatalog, int ID, int parentID, string folderName)
        {
            DataRow dr = seriesCatalog.NewRow();
            dr["id"] = ID;
            dr["ParentID"] = parentID;
            dr["IsFolder"] = true;
            dr["Name"] = folderName;

            seriesCatalog.Rows.Add(dr);

            // I'm assuming that subitems should use the SiteDataTypeID of this record for their ParentID entries?
            return ID;
        }

        private static bool AddRow(DataTable seriesCatalog, int ID, int parentID, string scenName, string HydrossName, string Description, string HydrossStation, string HydrossVarcode, string HydrossUnits)
        {
            return AddRow(seriesCatalog, ID, parentID, scenName, HydrossName, Description, HydrossStation, HydrossVarcode, HydrossUnits, "-", "-");
        }

        private static bool AddRow(DataTable seriesCatalog, int ID, int parentID, string scenName, string HydrossName, string Description, string HydrossStation, string HydrossVarcode, string HydrossUnits, string HydrossExtra1, string HydrossExtra2)
        {
            DataRow dr = seriesCatalog.NewRow();
            dr["id"] = ID;
            dr["ParentID"] = parentID;
            dr["IsFolder"] = false;
            dr["SortOrder"] = 1;
            dr["Source"] = "Hydross";
            dr["Name"] = HydrossName + ": " + Description;
            dr["SiteName"] = HydrossStation;
            dr["Units"] = HydrossUnits;
            dr["TimeInterval"] = "Monthly";
            dr["Parameter"] = "Flow";
            dr["FileIndex"] = 0;
            dr["Provider"] = "HydrossSeries";

            string cstr = string.Format("FileName={0};stationID={1};varcode={2};extra1={3};extra2={4};",
                new object[] { scenName, HydrossStation, HydrossVarcode, HydrossExtra1, HydrossExtra2 });

            dr["ConnectionString"] = ConnectionStringUtility.MakeFileNameRelative(cstr, s_databasePath);

            seriesCatalog.Rows.Add(dr);

            return true;
        }
    }
}