using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reclamation.Core;

namespace PiscesWebServices.CGI
{
    /// <summary>
    /// describe purpose and examples web queries.
    /// </summary>
    internal static class Help
    {

        public static void Print()
        {
            Help.PrintInstantHelp();
            Help.PrintDailyHelp();
            Help.PrintWaterYearHelp();
            PrintInventoryHelp();
        }


        public static void PrintInstantHelp()
        {
            var r = new Dictionary<string, string>();
            r.Add("all parameters last 24 hours for specified site", "list=bigi&back=24");
            r.Add("multiple sites and parameters for the last 24 hours" ,"list=boii ob, bfgi ob&back=24");
            r.Add("multiple sites and parameters, with specific date range", "list=boii ob, bfgi ob,pmai ob&start=2016-04-01&end=2016-04-2");
            r.Add("html format (no flags), with description above table", "list=boii ob, bfgi ob,pmai ob&back=12&format=html&flags=false&description=true");
            r.Add("wiski/kisters format", "list=boii ob, bfgi ob,pmai ob&back=12&format=zrxp");
            r.Add("15-minute idwr sites in Shef A format", "custom_list=idwr&format=shefa");
            r.Add("15-minute data, and 30 year daily average ", "list=heii q&format=csv-analysis&back=7&list-avg=heii qd");

            Print(r,"instant","Near real-time data");
        }

        internal static void PrintDailyHelp()
        {
            var r = new Dictionary <string,string>();
            r.Add("all parameters last 24 days for specified site", "list=luc&back=24&format=csv");
            r.Add("multiple sites and parameters for the last 30 days", "list=luc fb,luc af&back=30&format=csv");
            r.Add("multiple sites and parameters, with specific date range", "list=luc af, and af, ark af, ded af, boisys af&start=2016-04-01&end=2016-04-2");
            r.Add("html format (no flags), with description above table", "list=bsei&back=7&format=html&flags=false&description=true");
            r.Add("wiski/kisters format", "list=scoo qd&back=12&format=zrxp");
            r.Add("daily idwr sites for accounting, for wd01", "custom_list=wd01&format=idwr_accounting");
            r.Add("daily idwr sites for accounting, for wd63", "custom_list=wd63&format=idwr_accounting");
            r.Add("daily idwr sites for accounting, for wd65", "custom_list=wd65&format=idwr_accounting");
            r.Add("html report with title", "list=LRS&flags=false&description=true&format=html&back=12&title=MixedCase");


            Print(r, "daily", "Daily Data");
        }

        internal static void PrintWaterYearHelp()
        {
            var r = new Dictionary<string, string>();
            r.Add("water year report 2012", "site=abei&parameter=pp&start=2012&end=2012&format=usgs-html");
            r.Add("water year data.  Includes previous year and 30 year average", "site=culo&parameter=qd&start=2018&format=csv-analysis");

            Print(r, "wyreport", "Water Year Report");
        }

        internal static void PrintInventoryHelp()
        {
            var r = new Dictionary<string, string>();
            r.Add("Daily Inventory", "site=hghm&interval=daily");
            r.Add("Instant Inventory", "site=hghm&interval=instant");
            Print(r, "inventory", "Inventory");
        }

        private static void Print(Dictionary<string, string> d, string cgi, string header)
        {
            DataTable t = new DataTable();
            t.Columns.Add("name");
            t.Columns.Add("example");

            //REQUEST_URI
            var REMOTE_ADDR = Environment.GetEnvironmentVariable("REMOTE_ADDR");
            //Console.WriteLine("REMOTE_ADDR:" + REMOTE_ADDR + "<br/>");

            if (REMOTE_ADDR.IndexOf("140.215.")>=0)
                cgi += ".pl?";
            else
                cgi += "?";

            foreach (var item in d)
            {
                var example = "<a href=\"" + cgi + item.Value + "\">" + item.Value + "</a>";
                t.Rows.Add(item.Key,example);
            }
            var s = DataTableOutput.ToHTML(t, true, header);
            Console.WriteLine(s);
        }

        
    }
}
