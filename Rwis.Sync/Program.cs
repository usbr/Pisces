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
using System.DirectoryServices;

namespace Rwis.Sync
{
    class Program
    {
        //[STAThreadAttribute]
        static void Main(string[] argList)
        {
            Rwis.Sync.Program.SyncMain(argList);
        }

        static void SyncMain(string[] argList)
        {
            Arguments args = new Arguments(argList);
            var p = new OptionSet();

            if (argList.Length == 0)
            {
                ShowHelp(p);
                return;
            }


            if (args.Contains("debug"))
            {
                Logger.EnableLogger();
                Reclamation.TimeSeries.Parser.SeriesExpressionParser.Debug = true;
            }

            if (args.Contains("test-web"))
            {
                var data = Reclamation.Core.Web.GetPage("https://www.usbr.gov");
                foreach (var item in data)
                {
                    Console.WriteLine(item);
                }
                return;
            }

            if (args.Contains("initialize"))
            {
                Rwis.Initialize.Program.initializeMain(argList);
                return;
            }

            Performance perf = new Performance();
            var db = TimeSeriesDatabase.InitDatabase(args);

            if (args.Contains("update-sitecatalog"))
            {
                var csvFileName = args["update-sitecatalog"];
                if( !File.Exists(csvFileName))
                {
                    Console.WriteLine("Cannot open file "+csvFileName);
                    return;
                }
                UpdateSiteCatalog(db, csvFileName);

                return;
            }


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
                    sql = "provider IN ('HydrometDailySeries','HDBSeries','ShefSeries')";
                }
                else if (updateType.Length == 2)
                {
                    sql = "SUBSTRING(tablename,1,2) = '" + db.Server.SafeSqlLiteral(args["update"]).ToLower() + "'";
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

            Console.WriteLine("RWIS Sync.exe:  Completed " + DateTime.Now.ToString() + "\n");

            var mem = GC.GetTotalMemory(true);
            double mb = mem / 1024.0 / 1024.0;
            Console.WriteLine("Mem Usage: " + mb.ToString("F3") + " Mb");
            perf.Report("RWIS Sync: finished ");
        }


        /// <summary>
        /// update or insert new records in sitecatlog
        /// </summary>
        /// <param name="db"></param>
        /// <param name="csvFileName">input csv sitecatalog</param>
        private static void UpdateSiteCatalog(TimeSeriesDatabase db, string csvFileName)
        {
            var scRWIS = db.GetSiteCatalog();
            var scLocal = new CsvFile(csvFileName, CsvFile.FieldTypes.AllText);

            for (int i = 0; i < scLocal.Rows.Count; i++)
            {
                var localRow = scLocal.Rows[i];
                var siteID = localRow["siteid"].ToString();
                //check for existing record
                var rwisRows = scRWIS.Select("siteid='" + siteID + "'");
                if( rwisRows.Length == 1)
                {// update
                    Console.WriteLine("updating record: " + siteID);
                    var rwisRow = rwisRows[0];
                    for (int c = 0; c < scRWIS.Columns.Count; c++)
                    {
                        var cn = scRWIS.Columns[c].ColumnName; 
                        if (cn == "siteid")
                            continue;
                        if ( cn == "vertical_accuracy") // float
                        {
                            double x = 0;
                            double.TryParse(localRow[cn].ToString(), out x);
                            rwisRow[cn] = x;
                            continue;
                        }
                        rwisRow[cn] = localRow[cn];
                    }
                }
                else
                {// insert
                    Console.WriteLine("new record: "+siteID);
                    double vertical_accuracy = 0;
                    double latitude = 0;
                    double longitude = 0;
                    double elevation = 0;
                    
                    double.TryParse(localRow["vertical_accuracy"].ToString(),out vertical_accuracy);
                    double.TryParse(localRow["latitude"].ToString(), out latitude);
                    double.TryParse(localRow["longitude"].ToString(), out longitude);
                    double.TryParse(localRow["elevation"].ToString(), out elevation);

                    scRWIS.AddsitecatalogRow(localRow["siteid"].ToString(), localRow["description"].ToString(), localRow["state"].ToString(),
                        latitude, longitude, elevation, localRow["timezone"].ToString(),
                        localRow["install"].ToString(), localRow["horizontal_datum"].ToString(), localRow["vertical_datum"].ToString(),
                        vertical_accuracy, localRow["elevation_method"].ToString(), localRow["tz_offset"].ToString(),
                        localRow["active_flag"].ToString(), localRow["type"].ToString(), localRow["responsibility"].ToString(), localRow["agency_region"].ToString());
                }
            }

            db.Server.SaveTable(scRWIS);
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
            Console.WriteLine(@"--update-sitecatalog=c:\temp\sitecatalog.csv");
            Console.WriteLine("      update or insert new records in sitecatalog table");
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
            Console.WriteLine("--update=[Z] t1=[X] t2=[Y]");
            Console.WriteLine("      Updates data and series properties given a period range");
            Console.WriteLine("      with [Z] as all, HydrometDailySeries, HDBSeries, ShefSeries, ");
            Console.WriteLine("                  or region code PN, GP, LC, UC, or MP");
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
