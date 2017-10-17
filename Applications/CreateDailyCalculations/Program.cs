using Reclamation.Core;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HydrometDailyToPisces
{
    /// <summary>
    /// Creates daily Series in Pisces based on spreadsheet control file.
    /// if a calculation is needed use a CalculationSeries
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {

            if( args.Length != 5 && args.Length != 6)
            {
                Console.WriteLine("Usage: CreateDailyCalculations server user pass outputFile dryrun [sites]");
                Console.WriteLine(" server = lrgs1|lrgs2 ");
                Console.WriteLine(" user = username");
                Console.WriteLine(" pass = passwordfile");
                Console.WriteLine(" outputfile = filename for stats/results");
                Console.WriteLine(" dryrun = true|false -- when false only simulates changes");
                Console.WriteLine(" sites = amfi,anci,...   -- list of sites to filter");
                return;
            }

            var host = args[0];
            string user = args[1];
            string pass = File.ReadAllLines(args[2])[0];
            var outputFileName = args[3];
            bool dryRun = true;
            if (args[4] == "true")
                dryRun = true;
            else
            if (args[4] == "false")
                dryRun = false;
            else throw new ArgumentException("invalid setting for dryrun. Must be true or false");

            var sites = new string[] { };

            if (args.Length == 6)
                sites = args[5].ToLower().Split(',');


            var svr = PostgreSQL.GetPostgresServer("timeseries", host,user,pass);
            TimeSeriesDatabase db = new TimeSeriesDatabase(svr);
            Console.WriteLine(db.Server.ConnectionString);
            
            var fn = @"c:\temp\a.xlsx";

            File.Copy(Path.Combine(FileUtility.GetExecutableDirectory(),"daily_calcs_and_series.xlsx"), fn, true);
            var pcodeLookup = ExcelDB.Read(fn, "daily_instant_pcode");

            DailyCalcGenerator tool = new DailyCalcGenerator(db,outputFileName);

            tool.AddDailyCalculations(pcodeLookup,false,sites);
            

            // what about years like 6190.....
            //HydrometInfoUtility.ArchiverEnabled(.. ?

        }


         
    }
}
