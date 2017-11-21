using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Options;
using Reclamation.TimeSeries;
using Reclamation.Core;
using System.IO;
using Nancy.Hosting.Self;
using PiscesWebServices.Tests;
using PiscesWebServices.CGI;
using System.Collections;

namespace PiscesWebServices
{
    class Program
    {
        /// <summary>
        /// Examples:
        /// --server to start a self-hosted Nancy web service on the host server
        /// --cgi=sites --propertyFilter=program:agrimet --json_required_properties=json_extra
        /// --cgi=instant
        /// 
        /// Web Service Usage:
        /// - On a command line, call: > PISCESWEBSERVICES --SERVER
        /// - The Web Service relies on the following tables and views for rendering the front-end web pages
        ///     - view_seriescatalog generated from the other catalogs below
        ///         - Columns: (siteid, sitename, units, unitstext, timeinterval, statistic, parameter,
        ///             tablename, name, enabled, region, server, isfolder, t1, t2, count)
        ///     - sitecatalog
        ///     - seriescatalog
        ///     - parametercatalog
        /// - Make sure to populate the PiscesWebServices.exe.config file in either the bin directory 
        ///     (for testing or running from source) or the directory where the executable is located 
        ///     to start the Nancy Web Service and connect it to your desired DB
        ///     ------MYSQL SAMPLE----------------------------------------------------------------
        ///     <add key="MySqlUser" value="jrocha" />        		    
        ///     <add key="MySqlPassword" value="*****" />        		
        ///     <add key="MySqlDatabase" value="timeseries" />     			
        ///     <add key="MySqlServer" value="ibr*******00.bor.doi.net"/>  
        ///     <add key="LocalConfigurationDataPath" value="~" />
        ///     <add key="ClientSettingsProvider.ServiceUri" value=""/>	  
        ///     ------POSTGRES SAMPLE----------------------------------------------------------------
        ///     <!--<add key="PostgresUser" value="web_user"/>                                                              
        ///     <!--<add key="PostgresPassword" value="*****"/>    
        ///     <!--<add key="PostgresDatabase" value="timeseries"/>  
        ///     <!--<add key="PostgresServer" value="*****"/>         
        ///     <!--<add key="InternalNetworkPrefix" value="140."/>                                                         
        ///     <!--<add key="LocalConfigurationDataPath" value="\\ibr*****00\ConfigurationData"/>  
        ///     <!--<add key="ClientSettingsProvider.ServiceUri" value=""/>													
        /// 
        /// 
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
            bool selfHost = false;
            var sqLiteDatabaseFileName = "";

            p.Add("server", x => selfHost = true);
            p.Add("cgi=", "required cgi to execute cgi=help|sites|series|instant|daily|wyreport|inventory", x => cgi = x);
            p.Add("json_property_stubs=", "comma separated list of properties (i.e. 'region,url,') to created empty stubs if neeed ",
                              x => json_property_stubs = x);
            p.Add("site-type=", "filter agrimet sites", x => siteType = BasicDBServer.SafeSqlLikeClauseLiteral(x));
            p.Add("payload=", "test query data for a CGI", x => payload = System.Uri.EscapeDataString(x));
            p.Add("format=", "format json(default) | csv ", x => format = x);
            p.Add("verbose", " get more details", x => verbose = true);
            p.Add("debug", " get more details", x => verbose = true);
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

            if (selfHost)
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
            else if (cgi == "instant" || cgi == "daily")
            {
                try
                {
                    WebTimeSeriesWriter c = null;
                    if (cgi == "instant")
                        c = new WebTimeSeriesWriter(db, TimeInterval.Irregular, payload);

                    else
                    if (cgi == "daily")
                        c = new WebTimeSeriesWriter(db, TimeInterval.Daily, payload);

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
