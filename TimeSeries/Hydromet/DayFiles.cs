using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Reclamation.TimeSeries.Hydromet
{
    public class DayFiles
    {

        // Dayfile update file
        public static int WriteDayfileUpdateFile(DataTable tblNew, DataTable tblOld,
            string outputFilename, out string[] arcCommandList, out string[] modifiedParameters,
            out string[] modifiedCbtt,
            out DateRange range)
        {
            range = new DateRange();
            DateTime t1 = DateTime.MaxValue;
            DateTime t2 = DateTime.MinValue;
            var arcCommands = new List<string>();
            var modParameterList = new List<string>();
            var modcbtt = new List<string>();
            arcCommandList = arcCommands.ToArray();
            modifiedCbtt = modcbtt.ToArray();
            modifiedParameters = modParameterList.ToArray();
            if (tblNew.Rows.Count != tblOld.Rows.Count)
            {
                Logger.WriteLine("Error:  the number of rows in the data has changed. no update file will be written");
                return 0;
            }
            if (tblNew.Columns.Count != tblOld.Columns.Count)
            {
                Logger.WriteLine("Error:  the number of columns in the data has changed. no update file will be written");
                return 0;
            }
            StreamWriter output = new StreamWriter(outputFilename);

            int modifiedCounter = 0;
            // first column is date, other columns are values
            output.WriteLine("yyyyMMMdd hhmm cbtt     PC        NewValue   OldValue   Flag user:"+ Environment.UserName);
            for (int c = 1; c < tblOld.Columns.Count; c += 2)
            {
                string columnName = tblOld.Columns[c].ColumnName;
                var parts = columnName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string stationName = parts[0];
                string pcode = parts[1];
                string flagCode = "-00";

                for (int r = 0; r < tblOld.Rows.Count; r++)
                {
                    double valNew = HydrometDataUtility.ReadDouble(tblNew.Rows[r][c]);
                    double valOld = HydrometDataUtility.ReadDouble(tblOld.Rows[r][c]);

                    string newFlag = HydrometDataUtility.ReadFlag(tblNew.Rows[r][c + 1]);
                    string oldFlag = HydrometDataUtility.ReadFlag(tblOld.Rows[r][c + 1]);


                    if (valNew != valOld || newFlag != oldFlag)
                    { // something changed.  write to script.
                        DateTime date = (DateTime)tblNew.Rows[r][0];

                        UpdateDateRange(ref t1, ref t2, modifiedCounter, date);

                        UpdateDayfileDependencies(modcbtt, arcCommands, modParameterList, stationName, pcode, date);

                        flagCode = FlagCode(newFlag);


                        System.Globalization.NumberFormatInfo nf = new System.Globalization.NumberFormatInfo();
                        nf.NumberDecimalDigits = 2;

                        string str = date.ToString("yyyyMMMdd HHmm").ToUpper()
                            + " " + stationName.Trim().PadRight(8)
                            + " " + pcode.Trim().PadRight(9)
                            + " " + FortranFormat(valNew)
                            + " " + FortranFormat(valOld)
                            + " " + flagCode.ToString().PadRight(3);
                        output.WriteLine(str);
                        modifiedCounter++;
                    }

                }
            }

            output.Close();
            range = new DateRange(t1, t2);
            arcCommandList = arcCommands.ToArray();
            modifiedParameters = modParameterList.ToArray();
            modifiedCbtt = modcbtt.ToArray();
            return modifiedCounter;
        }

        private static string FortranFormat(double valNew)
        {
            string s = valNew.ToString("F2").PadRight(10);
            if (s.Length > 10)
                s = valNew.ToString("#.#####E0").PadRight(10);

                return s;
        }

        private static void UpdateDayfileDependencies(List<string> modcbtt, List<string> arcCommands, 
            List<string> modParameterList, string stationName, string pcode, DateTime date)
        {
      
            if (modParameterList.IndexOf(pcode.Trim().ToLower()) < 0)
            {
                modParameterList.Add(pcode.Trim().ToLower());
            }

             if (MidnightParameter(pcode) && date.Hour == 0 && date.Minute == 0)
                    date = date.Date.AddDays(-1);


             string cmd = ArchiveCommand(stationName, pcode, date);

            if (arcCommands.IndexOf(cmd) < 0 && date.Date < DateTime.Now.Date)
            {
                 if( HydrometInfoUtility.ArchiverEnabled(stationName,pcode))
                   arcCommands.Add(cmd);

                if (pcode.Trim().ToLower() == "fb"
                    && HydrometInfoUtility.ArchiverEnabled(stationName,"AF"))
                {
                    // run archiver for AF
                    arcCommands.Add(ArchiveCommand(stationName, "AF",date));
                }
                if (pcode.Trim().ToLower() == "gh" && HydrometInfoUtility.ArchiverEnabled(stationName,"Q"))
                {
                    // run archiver for q
                    arcCommands.Add(ArchiveCommand(stationName, "Q",date));
                }
            }

            // list of cbtt
            if (modcbtt.IndexOf(stationName.Trim().ToLower()) < 0)
            {
                modcbtt.Add(stationName.Trim().ToLower());
            }

        }

        public static string ArchiveCommand(string cbtt, string pcode, DateTime date)
        {
            HydrometHost svr = HydrometInfoUtility.HydrometServerFromPreferences();

            if (cbtt.Trim() == "")
                throw new ArgumentException("cbtt cannot be blank");
            var acm = "acm";
            if (svr == HydrometHost.GreatPlains)
            {
                acm = "acm_hydro";
            }
            string cmd = "$ interpret " + acm + "/nodebug " + date.ToString("yyyyMMMdd") + " " + cbtt + " " + pcode;
            return cmd;
        }

        private static bool MidnightParameter(string pcode)
        {
            string[] midnight = new string[] { "fb", "pc", "sp", "af" };
            return Array.IndexOf(midnight, pcode.ToLower().Trim()) >= 0;
        }
        

        private static void UpdateDateRange(ref DateTime t1, ref DateTime t2, int modifiedCounter, DateTime date)
        {
            if (modifiedCounter == 0)
            {
                t1 = date;
                t2 = date.AddMinutes(1);
            }
            else
            {
                if (date < t1)
                    t1 = date;
                if (date > t2)
                    t2 = date;
            }
        }

        private static string[] s_flags = { "e","u", "n", "m", "p", "i", "f", "r", "?", "a", "-", "+", "^", "~", "|"};
        private static string[] s_flagCodes = { "-03", "-00", "-02", "-04", "-06", "-08", "-10", "-14", "-16", "-18", "-20", 
                                                  "-22", "-24", "-26", "-28"};


        public static string FlagFromCode(string code)
        {
            string flag = " "; // default not flag
            int idx = Array.IndexOf(s_flagCodes, code);
            if (idx >= 0)
                flag = s_flags[idx];

            return flag;
        }
       public static string FlagCode(string flag)
        {

           string flagCode = "-01";
             int idx = Array.IndexOf(s_flags, flag);
             if (idx >= 0)
                 flagCode = s_flagCodes[idx];

             return flagCode;
        }


    }
}
