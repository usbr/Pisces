using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries;
using Reclamation.Core;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using Reclamation.TimeSeries.Parser;
using System.Reflection;
using Mono.Options;

namespace HydrometServer
{   
    /// <summary>
    /// This HydrometServer processes incoming data and stores it in a SQL database.
    /// values are flagged are based on high and low limits, computations are performed and 
    /// data is routed outgoing to the Hydromet vms server.
    /// </summary>
    class Program
    {
        static void Main(string[] argList)
        {
            Console.Write("HydrometServer " + Application.ProductVersion +" " + AssemblyUtility.CreationDate());

            Arguments args = new Arguments(argList);
            var p = new OptionSet();

            var cli = "";
            p.Add("cli=", "interface --cli=instant|daily|monthly", x => cli = x);

            try
            {
                p.Parse(argList);
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
            }
            if (argList.Length == 0)
            {
                ShowHelp(p);
                return;
            }


            



            string errorFileName = "errors.txt";
            string detailFileName = "detail.txt";
            Performance perf = new Performance();
         //   try
            {
            if (args.Contains("debug"))  
            {
                Logger.EnableLogger();
                Reclamation.TimeSeries.Parser.SeriesExpressionParser.Debug = true;
            }

            if (args.Contains("import-rating-tables"))
            {// --import-rating-tables=site_list.csv  [--generateNewTables]     : updates usgs,idahopower, and owrd rating tables
                ApplicationTrustPolicy.TrustAll();
                var cfg = args["import-rating-tables"];
                if (File.Exists(cfg))
                {
                    RatingTableDownload.UpdateRatingTables(cfg, args.Contains("generateNewTables"));
                }
                else
                {
                    Console.WriteLine("Error: File not found: " + cfg);
                }
                return;
            }


            // setup connection to Database
            if (args.Contains("create-database"))
            {
                if (CreatePostgresDatabase(args))
                    return;
            }
            var db = TimeSeriesDatabase.InitDatabase(args);

            if (cli != "")
            {
                if (cli == "instant")
                {
                    Console.WriteLine();
                    HydrometServer.CommandLine.PiscesCommandLine cmd = new CommandLine.PiscesCommandLine(db);
                    cmd.PiscesPrompt();
                }

                return;
            }

            bool simulate = args.Contains("simulate");
            bool hydrometCompare = args.Contains("hydromet-compare");

            
            if (args.Contains("error-log"))
            {
                errorFileName = args["error-log"];
                File.AppendAllText(errorFileName,"HydrometServer.exe:  Started " + DateTime.Now.ToString()+"\n");
            }

            if (args.Contains("detail-log"))
            {
                detailFileName = args["detail-log"];
                File.AppendAllText(detailFileName, "HydrometServer.exe:  Started " + DateTime.Now.ToString() + "\n");
            }

                string propertyFilter = "";
                if (args.Contains("property-filter"))
                {
                    propertyFilter = args["property-filter"];
                }

                string filter = "";
                if (args.Contains("filter"))
                {
                    filter = args["filter"];
                }

                if (args.Contains("inventory"))
                {
                    db.Inventory();
                }



                if (args.Contains("import")) // import and process data from DECODES
                {
                    bool computeDependencies = args.Contains("computeDependencies");
                    bool computeDailyOnMidnight = args.Contains("computeDailyOnMidnight");

                     
                    string searchPattern = args["import"];

                    if (searchPattern == "")
                        searchPattern = "*";

                    string incomingPath = ConfigurationManager.AppSettings["incoming"];
                    FileImporter importer = new FileImporter(db);
                    importer.Import(incomingPath, RouteOptions.Outgoing,computeDependencies,computeDailyOnMidnight,searchPattern);
                    //ImportDMS3.Import(db,incomingPath); 

                    //db.Import
                    //db.TimeSeriesImporter.ProcessFiles(incomingPath, TimeInterval.Irregular);
                    //db.TimeSeriesImporter.ProcessFiles(incomingPath, TimeInterval.Daily);
                }



                DateTime t1;
                DateTime t2;

                SetupDates(args, out t1, out t2);

                if (args.Contains("import-hydromet-instant"))
                {
                    File.AppendAllText(errorFileName, "begin: import-hydromet-instant " + DateTime.Now.ToString() + "\n");
                    ImportHydrometInstant(db, t1.AddDays(-2), t2.AddDays(1), filter, propertyFilter);
                }

                if (args.Contains("import-hydromet-daily"))
                {
                    File.AppendAllText(errorFileName, "begin: import-hydromet-daily " + DateTime.Now.ToString() + "\n");
                    ImportHydrometDaily(db, t1.AddDays(-100), t2, filter, propertyFilter);
                }

                if (args.Contains("import-hydromet-monthly"))
                {
                    File.AppendAllText(errorFileName, "begin: import-hydromet-monthly " + DateTime.Now.ToString() + "\n");
                    ImportHydrometMonthly(db, t1.AddYears(-5), t2.AddDays(1), filter, propertyFilter);
                }


                if (args.Contains("calculate-daily"))
                {
                    TimeSeriesCalculator calc = new TimeSeriesCalculator(db);
                    File.AppendAllText(errorFileName, "begin: calculate-daily " + DateTime.Now.ToString() + "\n");
                    calc.ComputeDailyValues(t1, t2, filter, propertyFilter, hydrometCompare, errorFileName,detailFileName, simulate);
                }


                if (args.Contains("import-dayfile"))
                {
                    var fn  = args["import-dayfile"];

                    ImportVaxFile(db,fn);
                }
                if (args.Contains("import-archive"))
                {
                    var fn = args["import-archive"];

                    ImportVaxFile(db, fn);
                }
                if (args.Contains("import-mpoll"))
                {
                    var fn = args["import-mpoll"];

                    ImportVaxFile(db, fn);
                }


                if (args.Contains("update-daily"))
                {
                   string sql = "provider = '" + db.Server.SafeSqlLiteral(args["update-daily"]) + "'";
                   var updateList = db.GetSeriesCatalog(sql);
                   Console.WriteLine("Updating  "+updateList.Count+" Series ");

                   foreach (var item in updateList)
                   {
                       try
                       {
                           Console.Write(item.Name + " ");
                           var s = db.GetSeries(item.id);
                           s.Update(t1, t2);
                       }
                       catch (Exception e)
                       {
                           Console.WriteLine(e.Message);
                       }
                   }
                }

                db.Server.Cleanup();
                

                File.AppendAllText(errorFileName, "HydrometServer.exe:  Completed " + DateTime.Now.ToString() + "\n");
            }
            //catch (Exception e )
            //{
            //    Logger.WriteLine(e.Message);
            //    File.AppendAllText(errorFileName, "Error: HydrometServer.exe: \n"+e.Message);
            //    // Console.ReadLine();
            //    throw e;
            //}

            var mem = GC.GetTotalMemory(true);
            double mb = mem / 1024.0 / 1024.0;
            Console.WriteLine("Mem Usage: " + mb.ToString("F3") + " Mb");
            perf.Report("HydrometServer: finished ");
        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("HydrometServer");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);


            Console.WriteLine(@"--database=c:\data\mydata.pdb|192.168.50.111:timeseries ");
            Console.WriteLine("--import[=searchPattern]   [--computeDependencies] [--computeDailyOnMidnight]");
            Console.WriteLine("           imports (and processes) data in incoming directory");
            Console.WriteLine("            supports DMS3, and LoggerNet files");
            Console.WriteLine("--debug");
            Console.WriteLine("           prints debugging messages to console");
            Console.WriteLine("--inventory");
            Console.WriteLine("           prints summary inventory of database");
            Console.WriteLine("--calculate-daily");
            Console.WriteLine("           computes daily data");
            Console.WriteLine("--calculate-monthly");
            Console.WriteLine("           computes all monthly equations");
            Console.WriteLine("--t1=1-31-2013|yesterday|lastyear");
            Console.WriteLine("           starting date: default is yesterday");
            Console.WriteLine("--t2=1-31-2013|yesterday|lastyear");
            Console.WriteLine("           ending date: default is yesterday");
            Console.WriteLine("--property-filter=program:agrimet");
            Console.WriteLine("           filtering based on series properties (key:value)");
            Console.WriteLine("--filter=\"siteid='boii'\"");
            Console.WriteLine("           raw sql filter against seriescatalog");
            Console.WriteLine("--error-log=errors.txt");
            Console.WriteLine("           file to log error messages");
            Console.WriteLine("--detail-log=detail.txt");
            Console.WriteLine("           file to log error messages");
            Console.WriteLine("--hydromet-compare");
            Console.WriteLine("           compare computed values to hydromet values");
            Console.WriteLine("--import-hydromet-instant");
            Console.WriteLine("           imports hydromet instant data default (t1-3 days)");
            Console.WriteLine("--import-hydromet-daily");
            Console.WriteLine("           imports hydromet daily data default ( t1-100 days)");
            Console.WriteLine("--import-hydromet-monthly");
            Console.WriteLine("           imports hydromet monthly data ( last 5 years)");
            Console.WriteLine("--simulate");
            Console.WriteLine("           simulate daily calcs (echo equation but don't compute)");
            Console.WriteLine("--create-database=timeseries");
            Console.WriteLine("          creates a new database");
            Console.WriteLine("--import-rating-tables=site_list.csv  [--generateNewTables]");
            Console.WriteLine("          updates usgs,idahopower, and owrd rating tables");
            Console.WriteLine("--import-dayfile=/data/dayfiles/2013*.DAY");
            Console.WriteLine("          imports data from VAX binary dayfile");
            Console.WriteLine("--import-archive=/data/archives/wy2013.acf");
            Console.WriteLine("          imports data from VAX binary archive file");
            Console.WriteLine("--import-mpoll=/data/mpoll/mpoll.ind");
            Console.WriteLine("          imports data from VAX binary monthly file");
            Console.WriteLine("--update-daily=HydrometDailySeries");

            // --update-daily=HydrometDailySeries --t1=lastyear
            // --update-daily=HDBDailySeries  --t2=yesterday

            Console.WriteLine(" updates all daily series from source data");

        }
        
        
        private static bool CreatePostgresDatabase(Arguments args)
        {
            if (args.Contains("create-database"))
            {
                var settings = ConfigurationManager.AppSettings;
                var dbname = settings["PostgresDatabase"];
                var owner = settings["PostgresTableOwner"];

                dbname = args["create-database"];
                Console.WriteLine("Creating Database:" + dbname);
                var pgSvr = PostgreSQL.GetPostgresServer("postgres") as PostgreSQL;

                pgSvr.CreateDatabase(dbname, owner);
                pgSvr = PostgreSQL.GetPostgresServer(dbname) as PostgreSQL;
                pgSvr.CreateSchema(owner, owner);
                pgSvr.SetSearchPath(dbname, owner);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Goal: High performance import of Dayfiles to TimeSeriesDatabase
        /// </summary>
        /// <param name="fileName"></param>
        private static void ImportVaxFile(TimeSeriesDatabase db,string fileNamePattern)
        {
            // we have  12,000  dayfiles to import.  (avg 3.0 MB each)
            // if it takes 4 seconds to read one file and sort
            // that is allready  13 hours just to read the files.

            // initial results   72  seconds to import 2013sep21.day (agrimet/hydromet subset)
            // that would take 10 days

            Console.WriteLine("Reading file(s): "+fileNamePattern);
            Performance p = new Performance();
            string path = Path.GetDirectoryName(fileNamePattern);
            string searchPattern = Path.GetFileName(fileNamePattern);


            var files = Directory.GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
            DataTable tbl = null;
            foreach (var fileName in files)
            {
                if (db.Server.SqlCommands.Count > 5000)
                    db.Server.SqlCommands.Clear(); // we can run out out of memory if we save all commands.

                Console.Write("Reading " + fileName + " ");
                string interval = "instant";

                if (Path.GetExtension(fileName).ToLower() == ".day")
                {
                    var df = new DayFile(fileName, false, false);
                    tbl = df.GetTable();
                }
                if (Path.GetExtension(fileName).ToLower() == ".acf")
                {
                    var acf = new ArchiveFile(fileName);
                    interval = "daily";
                    tbl = acf.GetTable();
                }
                if (Path.GetExtension(fileName).ToLower() == ".ind")
                {
                    var acf = new MpollFile(fileName);
                    interval = "daily";
                    tbl = acf.GetTable();
                }

                // sw.WriteLine("DateTime, site,pcode,value,flag");
                // tbl = DataTableUtility.Select(tbl, "", "site,pcode");
                //p.Report("sorted");  // 4.3 sec

                var distinct = DataTableUtility.SelectDistinct(tbl, "site", "pcode");
                //p.Report("distinct list " + distinct.Rows.Count + " items");


                foreach (DataRow row in distinct.Rows)
                {
                    string cbtt = row["site"].ToString();
                    string pcode = row["pcode"].ToString();
                    
                    
                    TimeSeriesName tn = new TimeSeriesName(cbtt + "_" + pcode, interval);

                    if (!tn.Valid)
                    {
                        Console.WriteLine("skipping Invalid cbtt/pcode "+ cbtt+"/"+pcode);
                        continue;
                    }


                    string filter = "site = '" + cbtt + "' and pcode = '" + pcode + "'";
                    var filterRows = tbl.Select(filter, "");

                    var s = db.GetSeriesFromTableName(tn.GetTableName(),"",true);

                    if (s == null)
                    {
                        //Console.WriteLine("Skipping : " + tn.GetTableName() + " not found in database ");
                        continue;
                    }
                    else
                    {
                        for (int i = 0; i < filterRows.Length; i++)
                        {
                            DateTime t = Convert.ToDateTime(filterRows[i]["DateTime"]);
                            if (s.IndexOf(t) >=0 )
                            {
                                Console.WriteLine("Warning: skipping duplicate date "+t.ToString());
                                continue;
                            }
                            s.Add(t, Convert.ToDouble(filterRows[i]["value"]),
                                filterRows[i]["flag"].ToString());
                        }

                        int count = db.SaveTimeSeriesTable(s.ID, s, DatabaseSaveOptions.Insert);
                        Console.WriteLine(tn.GetTableName() + ": Saved " + count + " rows");
                    }

                }
            }

            p.Report("Done. importing " + searchPattern);

        }

        public static IEnumerable<String> GetBlockOfQueries(TimeSeriesDatabase db,TimeInterval interval, string filter,string propertyFilter="", int blockSize=75)
        {
            var rval = new List<string>();
            foreach (Series s in db.GetSeries(interval, filter,propertyFilter).ToArray())
            {
                TimeSeriesName tn = new TimeSeriesName(s.Table.TableName);
                //rval.Add(s.SiteID + " " + s.Parameter);
                rval.Add(tn.siteid + " " + tn.pcode);
                if (rval.Count >= blockSize)
                {
                    yield return String.Join(",",rval.ToArray());
                    rval.Clear();
                }
            }
            yield return String.Join(",", rval.ToArray());
        }


        /// <summary>
        /// Imports daily data from Hydromet into TimeSeriesDatabase
        /// </summary>
        /// <param name="db"></param>
        private static void ImportHydrometDaily(TimeSeriesDatabase db, DateTime t1, DateTime t2, string filter, string propertyFilter)
        {
            Performance perf = new Performance();
            Console.WriteLine("ImportHydrometDaily");
            int block = 1;
            foreach (string query in GetBlockOfQueries(db,TimeInterval.Daily,filter,propertyFilter))
            {
                var table = HydrometDataUtility.ArchiveTable(HydrometHost.PN, query, t1, t2, 0);
                Console.WriteLine("Block " + block + " has " + table.Rows.Count + " rows ");
                Console.WriteLine(query);
                SaveTableToSeries(db, table, TimeInterval.Daily);
                block++;
            }
            perf.Report("Finished importing daily data"); // 15 seconds
        }

        private static void SaveTableToSeries(TimeSeriesDatabase db, DataTable table, TimeInterval interval)
        {
            int i = 1;
            string tablePrefix="daily";
            if (interval == TimeInterval.Irregular)
                tablePrefix = "instant";
            if (interval == TimeInterval.Monthly)
                tablePrefix = "monthly";
            while( i < table.Columns.Count )
            {
                string tn = table.Columns[i].ColumnName.Trim().ToLower();
                tn = tn.Replace(" ", "_");
                TimeSeriesName tsn = new TimeSeriesName(tn,interval.ToString().ToLower());
                var series1 = db.GetSeriesFromTableName(tn, tablePrefix);
                Console.Write(tn+ " ");
                for (int r = 0; r < table.Rows.Count; r++)
                {
                    var row = table.Rows[r];
                    object o = row[i];
                    double val = Point.MissingValueFlag;
                    if (o != DBNull.Value)
                        val = Convert.ToDouble(row[i]);
                    else
                    {
                        continue; // mixing 5 and 15-minute data can cause gaps
                    }

                    string flag = "hmet-import";
                    if (interval == TimeInterval.Irregular|| interval == TimeInterval.Monthly)
                        flag = row[i + 1].ToString();


                    DateTime t = Convert.ToDateTime(row[0]);
                    if (interval == TimeInterval.Monthly)
                    {
                        if( tsn.pcode.ToLower() == "fc" || tsn.pcode.ToLower() == "se" || tsn.pcode.ToLower() == "fcm")
                        t = t.FirstOfMonth();
                        if (val != Point.MissingValueFlag && HydrometMonthlySeries.LookupUnits(tsn.pcode) == "1000 acre-feet")
                            val = val * 1000;
                    
                    }
                   var pt = new Point(t, val, flag);
                    series1.Add(pt);
                }

                if (interval == TimeInterval.Irregular || interval == TimeInterval.Monthly)
                {
                    i += 2;// flag column
                }
                else
                {
                    i++;
                }

                int rc = series1.Count;
                if( rc>0)
                   rc = db.SaveTimeSeriesTable(series1.ID, series1, DatabaseSaveOptions.UpdateExisting);
                Console.WriteLine(rc + " records saved");
            }
        }


        /// <summary>
        /// Imports instant data from Hydromet into TimeSeriesDatabase
        /// </summary>
        /// <param name="db"></param>
        private static void ImportHydrometInstant(TimeSeriesDatabase db,DateTime t1, DateTime t2, string filter,string propertyFilter)
        {
            Console.WriteLine("ImportHydrometInstant");
            int block = 1;
            foreach (string query in GetBlockOfQueries(db, TimeInterval.Irregular, filter,propertyFilter))
            {
                var table = HydrometDataUtility.DayFilesTable(HydrometHost.PN, query, t1, t2, 0);
                Console.WriteLine("Block " + block + " has " + table.Rows.Count + " rows ");
                Console.WriteLine(query);
                SaveTableToSeries(db, table,  TimeInterval.Irregular);
                block++;
            }
            Console.WriteLine("Finished importing 15-minute data");
        }

        /// <summary>
        /// Imports instant data from Hydromet into TimeSeriesDatabase
        /// </summary>
        /// <param name="db"></param>
        private static void ImportHydrometMonthly(TimeSeriesDatabase db, DateTime t1, DateTime t2, string filter,string propertyFilter)
        {
            Console.WriteLine("ImportHydrometMonthly");
            int block = 1;
            foreach (string query in GetBlockOfQueries(db, TimeInterval.Monthly, filter, propertyFilter))
            {
                var table = HydrometDataUtility.MPollTable(HydrometHost.PN, query, t1, t2);
                Console.WriteLine("Block "+block + " has "+table.Rows.Count     +" rows ");
                Console.WriteLine(query);
                SaveTableToSeries(db, table, TimeInterval.Monthly);
                block++;
            }
            Console.WriteLine("Finished importing monthly data");
        }



        private static void SetupDates(Arguments args, out DateTime t1, out DateTime t2)
        {
            t1 = DateTime.Now.Date.AddDays(-1);
            t2 = DateTime.Now.Date.AddDays(-1);

            if (args.Contains("t1"))
                t1 = ParseArgumentDate(args["t1"]);
            if (args.Contains("t2"))
                t2 = ParseArgumentDate(args["t2"]);

            Logger.WriteLine("t1= "+t1.ToShortDateString());
            Logger.WriteLine("t2= "+t2.ToShortDateString());
            
        }

        private static DateTime ParseArgumentDate(string dateString)
        {
            dateString = dateString.Trim();
            if( dateString.ToLower() == "yesterday")
            return DateTime.Now.AddDays(-1);

            if (dateString.ToLower() == "lastyear")
                return DateTime.Now.AddDays(-365);

            var t1 = DateTime.Parse(dateString);
            return t1;
        }


       
    }
}
