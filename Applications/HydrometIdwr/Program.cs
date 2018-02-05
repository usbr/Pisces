using System;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System.Configuration;
using System.IO;
using Reclamation.TimeSeries.Hydromet;

namespace HydrometIdwr
{
    static class Program
    {
        /// <summary>
        /// Save hydromet data to local hard drive for IDWR.  
        ///  a) Daily data  
        ///      HydrometIdwr --source:daily --daysPrevious:5 --config:daily_sites.csv --outputPath:"c:\temp"
        ///      HydrometIdwr --source:daily --t1:2007-7-1 --t2:2007-7-12 --config:daily_sites.csv --outputPath:"c:\temp"
        ///      
        ///  b) 15 minute data.
        ///      HydrometIdwr --source:hourly --hoursPrevious:3 --config:hourly_sites.csv --outputPath:"c:\temp"
        ///      HydrometIdwr --source:hourly --t1:2007-1-31 --t2:2007-1-31 --config:hourly_sites.csv --outputPath:"c:\temp"
        ///      
        /// </summary>
        [STAThread]
        static void Main(string[] cmdLine)
        {
            Logger.EnableLogger();
            Performance perf = new Performance();
            Arguments args = new Arguments(cmdLine);

            CsvFile csv = new CsvFile(args["config"]);
            DateTime t1 = DateTime.Now.AddDays(-5);
            DateTime t2 = DateTime.Now;
            bool isDaily = args["source"] == "daily";

            string filename = "";
            if (isDaily)
            {
                filename = DateTime.Now.ToString("yyyyMMMdd") + "arc.nws";
            }
            else if (args["source"] == "hourly")
            {
                filename = DateTime.Now.ToString("yyyyMMMddHH") + ".nws";
            }
            else
            {
                Console.WriteLine("Error: /source of daily or hourly not specified. Program stopped");
                return;
            }
            Console.WriteLine("saving to "+filename);
            ReadDates(args,isDaily, ref t1, ref t2);

            filename = Path.Combine(args["outputPath"], filename);
            StreamWriter sw = new StreamWriter(filename);


            for (int i = 0; i < csv.Rows.Count; i++)
            {
                string cbtt = csv.Rows[i]["Cbtt"].ToString();
                string pcode = csv.Rows[i]["pcode"].ToString();
                string shefName = csv.Rows[i]["ShefName"].ToString();
                string shefPcode = csv.Rows[i]["ShefPcode"].ToString();
                string timeZone = csv.Rows[i]["timeZone"].ToString();

                if (isDaily)
                {
                    WriteDaily( t1,  t2, sw, cbtt, pcode, shefName, shefPcode, timeZone);
                }
                else
                {
                    WriteHourly(t1, t2, sw, cbtt, pcode, shefName, shefPcode, timeZone);
                }
            }
            sw.Close();
            perf.Report();
        }
        /// <summary>
        /// .A LWOI1 080313 M DH0530/QRIRG 72.07
        /// .A LWOI1 080313 M DH0545/QRIRG 69.50
        /// .A LWOI1 080313 M DH0600/QRIRG 70.78
        /// .A LWOI1 080313 M DH0615/QRIRG 69.50
        /// </summary>
        private static void WriteHourly(DateTime t1, DateTime t2, StreamWriter sw, string cbtt, string pcode, string shefName, string shefPcode, string timeZone)
        {
            //HydrometInstantSeries.UseCaching = false;
            Series s = HydrometInstantSeries.Read(cbtt, pcode, t1, t2, HydrometHost.PNLinux);
            s.RemoveMissing();

            for (int j = 0; j < s.Count; j++)
            {
                DateTime t = s[j].DateTime;
                sw.WriteLine(".A " + shefName + " "
                    + t.ToString("yyMMdd") + " "
                    + timeZone + " "
                    + "DH"+t.ToString("HHmm")+  "/" + shefPcode + " " + s[j].Value.ToString("F2"));
            }
            System.Threading.Thread.Sleep(100);
        }
        /// <summary>
        ///  .A WSRI1 20080326 M DH2400/QRDRG 2788.91
        ///  .A ARKI1 20080322 M DH2400/QRDRG 3063.14
        ///  .A ARKI1 20080323 M DH2400/QRDRG 3051.89     
        ///  </summary>
        private static void WriteDaily( DateTime t1,  DateTime t2, StreamWriter sw, string cbtt, string pcode, string shefName, string shefPcode, string timeZone)
        {
            //HydrometDailySeries.UseInternet = true;
            Series s = HydrometDailySeries.Read(cbtt, pcode, t1, t2, HydrometHost.PNLinux);
            s.RemoveMissing();

            for (int j = 0; j < s.Count; j++)
            {
                sw.WriteLine(".A " + shefName + " "
                    + s[j].DateTime.ToString("yyyyMMdd") + " "
                    + timeZone + " "
                    + "DH2400/" + shefPcode + " " + s[j].Value.ToString("F2"));
            }
            System.Threading.Thread.Sleep(2000);
        }

        private static void ReadDates(Arguments args, bool isDaily,
            ref DateTime t1,ref DateTime t2)
        {
            if (args["source"] == "daily")
            {
                 ReadTime(args,isDaily,"daysPrevious", ref t1, ref t2);
            }
            else if (args["source"] == "hourly")
            {
                ReadTime(args, isDaily, "hoursPrevious", ref t1, ref t2);
            }
            else
            {
                Console.WriteLine("Error: invalid source '"+args["source"]+"'");
            }

        }

        private static void ReadTime(Arguments args,bool isDaily, string argName, 
            ref DateTime t1, ref DateTime t2)
        {
            if (args.Contains(argName))
            {
                string strDaysPrevious = args[argName];
                int intPrevious = 5;
                if (!int.TryParse(strDaysPrevious, out intPrevious))
                {
                    Console.WriteLine("Error parsing "+argName +" as integer " + strDaysPrevious);
                    intPrevious = 5;
                }

                if (isDaily)
                    t1 = DateTime.Now.AddDays(-intPrevious);
                else
                    t1 = DateTime.Now.AddHours(-intPrevious);
                t2 = DateTime.Now;
            }
            else
            {
                t1 = ReadT1T2(args, ref t1, ref t2);
            }
        }

        private static DateTime ReadT1T2(Arguments args, ref DateTime t1, ref DateTime t2)
        {
            if (!(args.Contains("t1") && args.Contains("t2")))
            {
                Console.WriteLine("Error: must specifiy /daysPrevious or /t1 and /t2 argument");
            }
            else
            {
                t1 = DateTime.Parse(args["t1"]);
                t2 = DateTime.Parse(args["t2"]);
            }
            return t1;
        }

    }
}

