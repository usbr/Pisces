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


            var host = "lrgs2";
            string user = "ktarbet";
            string pass = File.ReadAllLines(@"C:\utils\linux\ktarbet.postgres.txt")[0];

            var svr = PostgreSQL.GetPostgresServer("timeseries", host,user,pass);
            TimeSeriesDatabase db = new TimeSeriesDatabase(svr);
            Console.WriteLine(db.Server.ConnectionString);
            
            var fn = @"c:\temp\a.xlsx";

            File.Copy(Path.Combine(FileUtility.GetExecutableDirectory(),"daily_calcs_and_series.xlsx"), fn, true);
            var pcodeLookup = ExcelDB.Read(fn, "daily_instant_pcode");

            DailyCalcGenerator tool = new DailyCalcGenerator(db);

            tool.AddDailyCalculations(pcodeLookup);
            

            // what about years like 6190.....
            //HydrometInfoUtility.ArchiverEnabled(.. ?

        }


         
    }
}
