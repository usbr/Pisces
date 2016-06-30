using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries;
using Reclamation.Core;
using System.Configuration;
using System.IO;
using Mono.Options;

namespace Rwis.Sync
{
    class Program
    {
        static void Main(string[] argList)
        {
            Arguments args = new Arguments(argList);
            var p = new OptionSet();

            if (argList.Length == 0)
            {
                ShowHelp(p);
                return;
            }

            string errorFileName = "errors.txt";
            string detailFileName = "detail.txt";
            Performance perf = new Performance();

            if (args.Contains("debug"))
            {
                Logger.EnableLogger();
                Reclamation.TimeSeries.Parser.SeriesExpressionParser.Debug = true;
            }
            if (args.Contains("error-log"))
            {
                errorFileName = args["error-log"];
                File.AppendAllText(errorFileName, "HydrometServer.exe:  Started " + DateTime.Now.ToString() + "\n");
            }
            if (args.Contains("detail-log"))
            {
                detailFileName = args["detail-log"];
                File.AppendAllText(detailFileName, "HydrometServer.exe:  Started " + DateTime.Now.ToString() + "\n");
            }

            var db = TimeSeriesDatabase.InitDatabase(args);
            DateTime t1, t2;
            SetupDates(args, out t1, out t2);

            if (args.Contains("dbinventory"))
            {
                db.Inventory();
            }
            if (args.Contains("siteinventory"))
            {
                SiteInventory(args, db);
            }
            if (args.Contains("update"))
            {
                var updateType = args["update"].ToString();
                string sql = "";
                if (updateType.ToLower() == "all")
                {
                    sql = "provider IN ('HydrometDailySeries','HDBSeries')";
                }
                else
                {
                    sql = "provider = '" + db.Server.SafeSqlLiteral(args["update"]) + "'";
                }
                var updateList = db.GetSeriesCatalog(sql);
                Console.WriteLine("Updating  " + updateList.Count + " Series ");
                foreach (var item in updateList)
                {
                    try
                    {
                        Console.Write(item.Name.Substring(0, System.Math.Min(30, item.Name.Length)) + "... ");
                        var s = db.GetSeries(item.id);
                        s.Update(t1, t2);
                        s.Read(t1, t2);
                        Console.WriteLine("Updated " + s.Count + " values");
                        SaveProperties(s);                                  
                    }
                    catch (Exception e)
                    { Console.WriteLine(e.Message); }
                }
            }
            db.Server.Cleanup();

            File.AppendAllText(errorFileName, "RWIS Sync.exe:  Completed " + DateTime.Now.ToString() + "\n");

            var mem = GC.GetTotalMemory(true);
            double mb = mem / 1024.0 / 1024.0;
            Console.WriteLine("Mem Usage: " + mb.ToString("F3") + " Mb");
            perf.Report("RWIS Sync: finished ");
        }

        static void SiteInventory(Arguments args, TimeSeriesDatabase db)
        {
            var site = args["siteinventory"].ToString().ToLower();
            var siteCatalog = db.GetSiteCatalog("lower(description) LIKE '%" + site + "%'");
            if (siteCatalog.Rows.Count < 1)
            {
                Console.WriteLine("Site " + site + " not found... ");
                return;
            }
            else
            {
                foreach (var siteRow in siteCatalog)
                {
                    var seriesCatalog = db.GetSeriesCatalog("isfolder = 0 AND siteid = '" + siteRow["siteid"] + "'");
                    Console.WriteLine("---------------------------------------------------------------------");
                    Console.WriteLine(siteRow["siteid"] + " | " + siteRow["description"] + " | " + siteRow["state"] +
                        " | " + siteRow["agency_region"]);
                    foreach (var row in seriesCatalog)
                    { Console.WriteLine(" -- " + row["id"] + " | " + row["name"]); }
                }
                Console.WriteLine("---------------------------------------------------------------------");
            }
        }

        static void SaveProperties(Series s)
        {
            s.Read();
            s.Properties.Set("t1", s.MinDateTime.ToShortDateString());
            s.Properties.Set("t2", s.MaxDateTime.ToShortDateString());
            s.Properties.Set("count", s.Count.ToString());
            s.Properties.Save();
        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("RWIS Sync Program");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
            Console.WriteLine(@"--database=c:\data\mydata.pdb|192.168.50.111:timeseries ");
            Console.WriteLine("--debug");
            Console.WriteLine("      prints debugging messages to console");
            Console.WriteLine("--dbinventory");
            Console.WriteLine("      prints summary inventory of database");
            Console.WriteLine("--siteinventory=[X]");
            Console.WriteLine("      prints inventory of series for site [X] in database");
            Console.WriteLine("--error-log=errors.txt");
            Console.WriteLine("      file to log error messages");
            Console.WriteLine("--detail-log=detail.txt");
            Console.WriteLine("      file to log error messages");
            Console.WriteLine("--t1=[X]");
            Console.WriteLine("      starting date: default is yesterday");
            Console.WriteLine("      with [X] = yesterday, lastweek, lastmonth, lastyear, ");
            Console.WriteLine("                 or a valid date in YYYY-MM-DD format");
            Console.WriteLine("--t2=[X]");
            Console.WriteLine("      ending date: default is yesterday");
            Console.WriteLine("      with [X] = yesterday, lastweek, lastmonth, lastyear, ");
            Console.WriteLine("                 or a valid date in YYYY-MM-DD format");
            Console.WriteLine("--update t1=[X] t2=[Y]");
            Console.WriteLine("      Updates data and series properties given a period range");
            Console.WriteLine("      with [X] as a valid date in YYYY-MM-DD format and [X] < [Y]");
            Console.WriteLine("      with [Y] as a valid date in YYYY-MM-DD format and [X] < [Y]");

        }

        private static void SetupDates(Arguments args, out DateTime t1, out DateTime t2)
        {
            t1 = DateTime.Now.Date.AddDays(-1);
            t2 = DateTime.Now.Date.AddDays(-1);

            if (args.Contains("t1"))
            { t1 = ParseArgumentDate(args["t1"]); }
            if (args.Contains("t2"))
            { t2 = ParseArgumentDate(args["t2"]); }

            if (t1 > t2)
            {
                var tTemp = t2;
                t2 = t1;
                t1 = tTemp;
            }
            Logger.WriteLine("t1= " + t1.ToShortDateString());
            Logger.WriteLine("t2= " + t2.ToShortDateString());
        }

        private static DateTime ParseArgumentDate(string dateString)
        {
            dateString = dateString.Trim();
            if (dateString.ToLower() == "yesterday")
                return DateTime.Now.AddDays(-1);
            if (dateString.ToLower() == "lastweek")
                return DateTime.Now.AddDays(-7);
            if (dateString.ToLower() == "lastmonth")
                return DateTime.Now.AddDays(-31);
            if (dateString.ToLower() == "lastyear")
                return DateTime.Now.AddDays(-365);
            var t = DateTime.Parse(dateString);
            return t;
        }

        
    }
}
