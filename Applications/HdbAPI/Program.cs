using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.Hosting.Self;

namespace HdbApi
{
    class Program
    {
        public static bool jrdebug = true;

        static void Main(string[] argList)
        {
            if (!jrdebug)
            {
                if (argList.Length == 0)
                {
                    ShowHelp();
                    return;
                }
            }
            else
            {
                argList = new string[1];
                argList[0] = "--server";
            }


            Arguments args = new Arguments(argList);

            if (args.Contains("server"))
            {
                // Start web service
                var serverUri = "http://localhost:8080/";
                var cfg = new HostConfiguration();
                var host = new Nancy.Hosting.Self.NancyHost(cfg, new Uri(serverUri));
                using (host)
                {
                    host.Start();
                    Console.WriteLine("HDB API is now listening on " 
                        + serverUri.Replace("localhost", Environment.MachineName)
                        + ". Press ctrl-c to stop");
                    Console.ReadLine();
                }
            }

        }

        private static void ShowHelp()
        {
            Console.WriteLine("HELP MESSAGE AND USAGE GOES HERE");
        }


    }

    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get["/"] = x =>
            {
                return WebPageBuilder.BuildHomePage();
            };
        }
    }
}

