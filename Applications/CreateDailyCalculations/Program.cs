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
                Console.WriteLine(" server = hostname ");
                Console.WriteLine(" user = username");
                Console.WriteLine(" pass = passwordfile");
                Console.WriteLine(" outputfile = filename for stats/results");
                Console.WriteLine(" dryrun = true|false -- when true only simulates changes");
                Console.WriteLine(" sites = amfi,anci,...   -- list of sites to filter");
                return;
            }

            var host = args[0];
            string user = args[1];
            string pass = File.ReadAllLines(args[2])[0];
            var outputFileName = args[3];
            bool dryRun = (args[4] == "true");

            var sites = new string[] { };

            if (args.Length == 6)
                sites = args[5].ToLower().Split(',');


            var svr = PostgreSQL.GetPostgresServer("timeseries", host,user,pass);
            //UpdateVMS_daily_por(svr);
            TimeSeriesDatabase db = new TimeSeriesDatabase(svr);
            Console.WriteLine(db.Server.ConnectionString);
            
            var fn = @"c:\temp\a.xlsx";

            File.Copy(Path.Combine(FileUtility.GetExecutableDirectory(),"daily_calcs_and_series.xlsx"), fn, true);
            var pcodeLookup = ExcelDB.Read(fn, "daily_instant_pcode");

            DailyCalcGenerator tool = new DailyCalcGenerator(db,outputFileName);

            tool.AddDailyCalculations(pcodeLookup,dryRun,sites);
            

            // what about years like 6190.....
            //HydrometInfoUtility.ArchiverEnabled(.. ?

        }

        private static void UpdateVMS_daily_por(BasicDBServer svr)
        {
            var tbl = HydrometInfoUtility.DailyInventory;

            svr.RunSqlCommand("truncate table vms_daily_por");
            DataTable vms_daily_por = svr.Table("vms_daily_por");


            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                var cbtt = tbl.Rows[i]["cbtt"].ToString().ToLower();
                if (cbtt.Trim() == "")
                    continue;
                var pcode = tbl.Rows[i]["pcode"].ToString().ToLower();
                var por = HydrometInfoUtility.ArchivePeriodOfRecord(cbtt, pcode);

                vms_daily_por.Rows.Add(cbtt, pcode, por.T1, por.T2);
                Console.WriteLine(cbtt+" "+pcode);

            }

            svr.SaveTable(vms_daily_por);
        }


}
}
