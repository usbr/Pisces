using Mono.Options;
using PiscesWebServices.CGI;
using PiscesWebServices.Tests;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System;
using System.Collections;

namespace PiscesWebServices
{
    class Program
    {
        /// <summary>
        /// PiscesWebServices contains several CGI programs in support
        /// of migrating from the Legacy Hydromet System.
        /// Longer term the PiscesAPI (.net core) project can replace this program
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {

            var siteType = ""; // agrimet, hydromet (blank means all)
            var cgi = "";
            var json_property_stubs = "";
            var payload = "";
            var p = new OptionSet();
            var format = "json";
            var verbose = false;

            p.Add("cgi=", "required cgi to execute cgi=help|sites|series|instant|daily|monthly|wyreport|inventory", x => cgi = x);
            p.Add("json_property_stubs=", "comma separated list of properties (i.e. 'region,url,') to created empty stubs if neeed ",
                              x => json_property_stubs = x);
            p.Add("site-type=", "filter agrimet sites", x => siteType = BasicDBServer.SafeSqlLikeClauseLiteral(x));
            p.Add("payload=", "test query data for a CGI", x => payload = System.Uri.EscapeDataString(x));
            p.Add("format=", "format json(default) | csv ", x => format = x);
            p.Add("verbose", " get more details", x => verbose = true);
            p.Add("debug", " get more details", x => verbose = true);

            try
            {
                p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            Database.InitDB(args);
            var db = Database.DB();


            if (cgi == "")
            {
                ShowHelp(p);
                return;
            }
            

            if (verbose)
            {
                Console.Write("Content-type: text/html\n\n");
                Logger.EnableLogger();
                var ev = Environment.GetEnvironmentVariables();
                
                foreach (DictionaryEntry item in ev)
                {
                    Console.WriteLine(item.Key+": "+item.Value+"<br/>");
                }
                Logger.WriteLine("verbose=true");
                Logger.WriteLine("payload = " + payload);
            }

            if (cgi == "help")
            {
                if( !verbose)
                Console.Write("Content-type: text/html\n\n");
                Help.Print();
                return;
            }

            if (cgi == "inventory")
            {
                try
                {
                    InventoryReport r = new InventoryReport(db, payload);
                    r.Run();
                }
                catch (Exception e)
                {
                    Logger.WriteLine(e.Message);
                }
                
            }
            else if (cgi == "sites")
            {
                if (format == "json")
                {
                    JSONSites d = new JSONSites(db);
                    d.Execute(json_property_stubs.Split(','), siteType);
                }
                else if (format == "csv")
                {
                    SiteCsvTable c = new SiteCsvTable(db);
                    c.Execute(siteType);
                }
            }
            else if (cgi == "instant" || cgi == "daily" || cgi == "monthly")
            {
                try
                {
                    WebTimeSeriesWriter c = null;
                    if (cgi == "instant")
                        c = new WebTimeSeriesWriter(db, TimeInterval.Irregular, payload);

                    else
                    if (cgi == "daily")
                        c = new WebTimeSeriesWriter(db, TimeInterval.Daily, payload);

                    if (cgi == "monthly")
                        c = new WebTimeSeriesWriter(db, TimeInterval.Monthly, payload);


                    c.Run();
                }
                catch (Exception e)
                {
                    Logger.WriteLine("Error: " + e.Message);
                }
            }
            else if (cgi == "wyreport")
            {
                try
                {
                    WaterYearReport wy = new WaterYearReport(db, payload);
                    wy.Run();
                }
                catch (Exception e)
                {

                    Logger.WriteLine("Error: " + e.Message);
                }
            }
            else if (cgi == "site")
            {
                SiteInfoCGI si = new SiteInfoCGI(db);
                si.Run(payload);
            }
            else if (cgi == "test-perf-large")
            {
                var c = new HydrometGCITests();
                c.CGI_PerfTestLarge();

            }
            else if (cgi == "test-perf-small")
            {
                var c = new HydrometGCITests();
                c.CGI_PerfTestSmall();
            }
            else if (cgi == "dump")
            {
                var c = new HydrometGCITests();
                c.DumpTest();
            }
            else
            {
                Console.WriteLine("invalid cgi: " + cgi);
            }

        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("PiscesWebServices");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }

    }
}
