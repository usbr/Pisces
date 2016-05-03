using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Options;
using Reclamation.TimeSeries;
using Reclamation.Core;
using System.IO;
using Nancy.Hosting.Self;

namespace PiscesWebServices
{
    class Program
    {
        /// <summary>
        /// Examples:
        /// 
        /// --cgi=sites --propertyFilter=program:agrimet --json_required_properties=json_extra
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {

            string propertyFilter="";
            var cgi = "";
            var json_property_stubs="";
            var payload = "";
            var p = new OptionSet();
            var format = "json";
            var verbose = false;
            bool selfHost = false;
            var sqLiteDatabaseFileName="";

            p.Add("server", x => selfHost = true);
            p.Add("cgi=","required cgi to execute cgi=sites or cgi=series",x => cgi=x);
            p.Add("json_property_stubs=", "comma separated list of properties (i.e. 'region,url,') to created empty stubs if neeed ",
                              x => json_property_stubs = x);
            //p.Add("propertyFilter=", "property filter like program:agrimet", x => propertyFilter = x);
            p.Add("payload=", "test query data for a CGI", x => payload = x);
            p.Add("format=","format json(default) | csv ",x => format=x);
            p.Add("verbose"," get more details", x => verbose =true);
            p.Add("database", "filename for SQLite database", x => sqLiteDatabaseFileName = x);

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
            
            if( selfHost)
            {
                try
                {
                    var serverUri = "http://localhost:8080";
                    var cfg = new HostConfiguration();
                    //cfg.RewriteLocalhost = false;
                    //c..fg.UrlReservations.CreateAutomatically=true;
                    var host = new Nancy.Hosting.Self.NancyHost(cfg, new Uri(serverUri));
                    //var host = new Nancy.Hosting.Self.NancyHost();
                    using (host)
                    {
                        host.Start();
                        Console.WriteLine("Running on " + serverUri);
                        Console.ReadLine();
                    }
                }
                catch (Exception nancyEx)
                {

                    Console.WriteLine(nancyEx.Message);
                }
                return;
            }

            if (cgi == "")
            {
                ShowHelp(p);
                return;
            }

            if (verbose)
            {
                Console.Write("Content-type: text/html\n\n");
                Logger.EnableLogger();
                Logger.WriteLine("verbose=true");
                Logger.WriteLine("payload = " + payload);
            }

            

            if (cgi == "inventory")
            {
                Console.Write("Content-Type: text/html\n\n");
                db.Inventory();
            }
            else
            if (cgi == "sites")
            {
                if (format == "json")
                {
                    JSONSites d = new JSONSites(db);
                    d.Execute(json_property_stubs.Split(','), propertyFilter);
                }
                else if (format == "csv")
                {
                    CsvTable c = new CsvTable(db);
                    c.Execute(propertyFilter);
                }
            }
            else
            if (cgi == "instant" || cgi == "daily")
            {
                CsvTimeSeriesWriter c = new CsvTimeSeriesWriter(db);
                if( cgi == "instant")
                c.Run(TimeInterval.Hourly,payload);
                else if( cgi == "daily")
                c.Run(TimeInterval.Daily,payload);

            }
            else
            if (cgi == "site")
            {
                SiteInfo si = new SiteInfo(db);
                si.Run(payload);
            }
            else
            if (cgi == "test-perf-large")
            {
                TestCGI c = new TestCGI();
                c.PerfTestLarge();
                
            }
            else
                if (cgi == "test-perf-small")
                {
                    TestCGI c = new TestCGI();
                    c.PerfTestSmall();
                }
                else
                    if (cgi == "dump")
                    {
                        TestCGI c = new TestCGI();
                        c.DumpTest();
                    }
                else
                {
                    Console.WriteLine("invalid cgi: "+cgi);
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
