using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Text.RegularExpressions;
using Reclamation;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.Riverware;
using Reclamation.Core;
using Reclamation.TimeSeries;

namespace Reclamation.RiverwareDmi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                int debugLevel = 0;

                Arguments arguments = new Arguments(args);

                if (arguments.Contains("udebuglevel"))
                {
                    debugLevel = Convert.ToInt32(arguments["udebuglevel"]);
                }

                if (debugLevel > 0)
                {
                    if (System.Windows.Forms.MessageBox.Show(
                        String.Join(" \n", args), "Diagnostic info", System.Windows.Forms.MessageBoxButtons.OKCancel)
                           == System.Windows.Forms.DialogResult.Cancel)
                        return;
                }


                string controlFilename = args[0];
                DateTime t1 = DateTime.Parse(args[2]);
                DateTime t2 = DateTime.Parse(args[4]);


                ProcessArguments(debugLevel, arguments, t1, t2, controlFilename);

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

          //  p.Report("RiverWare dmi completed.");

        }

        private static void ProcessArguments(int debugLevel, Arguments arguments, DateTime t1, DateTime t2, string controlFilename)
        {

            if (arguments.Contains("uxlsfilename"))
            {
                string xlsFileName = arguments["uxlsfilename"];
                int traceNumber = -1;
                if (arguments.Contains("strace"))
                {
                    traceNumber = Convert.ToInt32(arguments["strace"]);
                }
                else if (arguments.Contains("uwateryear"))
                {
                    if (!arguments.Contains("ufirstwateryear"))
                    {
                        System.Windows.Forms.MessageBox.Show("Error: FirstWaterYear must be defined as DMI argument");
                        return;
                    }

                    int wy1 = Convert.ToInt32(arguments["ufirstwateryear"]);
                    int wy = Convert.ToInt32(arguments["uwateryear"]);

                    traceNumber = wy - wy1 + 1;
                    if (debugLevel > 0)
                        System.Windows.Forms.MessageBox.Show("Trace Number = " + traceNumber);
                }

                ReadFromExcel(controlFilename, xlsFileName, t1, t2, traceNumber);
            }
            else if (arguments.Contains("uhydrometserver"))
            {//"-UHydrometServer=pnhyd0"
                string serverName = arguments["uhydrometserver"];
                ReadFromHydromet(controlFilename, serverName, t1, t2);
            }
            else if (arguments.Contains("updbfilename"))
            {
                string pdbFileName = arguments["updbfilename"];
                Regex regex = new Regex(@"\$(.*?)\\");
                Match match = regex.Match(pdbFileName);
                if (match.Success)
                {
                    var envVar = match.Groups[1].ToString();
                    var envPath = Environment.GetEnvironmentVariable(envVar);
                    pdbFileName = pdbFileName.Replace(envVar, envPath).Replace("$", "");
                }
                ReadFromPisces(pdbFileName, controlFilename, t1, t2);
            }
            else if (arguments.Contains("uembededdata") && arguments["uembededdata"].ToLower() == "true")
            {
                ControlFileWithData.ProcessFile(controlFilename);
            }
            else
            {
                Usage();
            }
        }

        private static void ReadFromPisces(string pdbFileName, string controlFilename, DateTime t1, DateTime t2)
        {
            PiscesDMI dmi = new PiscesDMI(pdbFileName, controlFilename, t1, t2);
            dmi.ExportTextFiles();
        }

        private static void ReadFromHydromet(string controlFilename, string serverName, DateTime t1, DateTime t2)
        {

            HydrometHost server = HydrometHost.PNLinux;
            if (serverName.IndexOf("yakhyd") >= 0)
            {
                server = HydrometHost.Yakima;
            }
            HydrometDMI dmi = new HydrometDMI(server,controlFilename,t1,t2);

            dmi.ExportTextFiles();
        }

        private static void Usage()
         //  arg[index]                           0        1          2     3          4     5    6                      7        8 
        {//  arg count                            1        2          3     4          5     6    7                      8        9 
//            -UxlsFileName=V:\PN6200\Models\BoiseRiverWare\BoiseModelData.xls -UDebugLevel=1 -UWaterYear=1943 -UFirstWaterYear=1919 
            Console.WriteLine("Usage 1: controlfile tempPath yyyy-mm-dd hh:mm yyyy-mm-dd hh:mm 1DAY -UxlsFileName=file.xls [-STrace=n]");
            Console.WriteLine("Where:");
            Console.WriteLine("       file.xls is an excel file");
            Console.WriteLine("       controlFile is a riverware control file with a user defined sheet name and column name");
            Console.WriteLine("       yyyy-mm-dd hh:mm is riverware simulation start and end date/time");
            Console.WriteLine("       1Day in interval");
            Console.WriteLine("      -UxlsFileName file.xls -- excel input file");
            Console.WriteLine("      -STrace n specifies water year to read into riverware as if it were the simulation year");
            Console.WriteLine("          integer (1..n) begins with first date in the spreadsheet increments by year");
            Console.WriteLine("");
            
         //                                       1        2          3     4          5     6    7                        8  
            Console.WriteLine("Usage 2: controlfile tempPath yyyy-mm-dd hh:mm yyyy-mm-dd hh:mm 1DAY  -UHydrometServer=pnhyd0");
            Console.WriteLine("Where:");
            Console.WriteLine("       -UHydrometServer pnhyd0|yakhyd specifies what hydromet server to read from");

            Console.WriteLine("Usage 3: controlfile tempPath yyyy-mm-dd hh:mm yyyy-mm-dd hh:mm 1DAY  -UpdbFileName=file.pdb");
            Console.WriteLine("Where:");
            Console.WriteLine("       -UpdbFileName file.pdb -- Pisces input file");

            Console.WriteLine();
            Console.WriteLine("Usage 4: controlfile tempPath yyyy-mm-dd hh:mm yyyy-mm-dd hh:mm 1DAY  -UEmbededData=True ");
            Console.WriteLine("     control file has embeded data with user option !data= ");


        }

        private static void ReadFromExcel(string controlFileName, string xlsFileName, DateTime t1, DateTime t2, int traceNumber)
        {
            // basic data import.
            ExcelDMI dmi = new ExcelDMI(xlsFileName, controlFileName);

            dmi.ExportTextFiles(t1,t2,traceNumber);
        }
    }
}
