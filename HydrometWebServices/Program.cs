using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Options;

namespace HydrometWebServices
{
    class Program
    {
        static void Main(string[] args)
        {

            string propertyFilter="";
            var cgi = "";
            var json_property_stubs="";

            var p = new OptionSet();
            p.Add("cgi=","required cgi to execute cgi=sites or cgi=series",x => cgi=x);
            p.Add("json_property_stubs=", "comma separated list of properties (i.e. 'region,url,') to created empty stubs if neeed ",
                              x => json_property_stubs = x);
            p.Add("propertyFilter=", "property filter like program:agrimet", x => propertyFilter = x);
            try
            {
                p.Parse(args);
            }
            catch (OptionException e)
            {
                Console.WriteLine(e.Message);
            }
            if (cgi == "")
            {
                ShowHelp(p);
                return;
            }

            if (cgi == "sites")
            {
                SiteDump.Execute(json_property_stubs.Split(','), propertyFilter);
            }

        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("HydrometWebServices");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }

    }
}
