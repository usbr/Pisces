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

        public static string Print()
        {
          return  Help.PrintInstant()
                + Help.PrintDaily()
                + Help.PrintMonthly()
                + Help.PrintWaterYear()
                + PrintInventory();
        }

        public static string PrintMonthly()
        {
            var r = new Dictionary<string, string>();
            r.Add("one parameter one site with flags", "list=heii qm&back=24&format=csv");
            r.Add("all parameters for a single site without flags", "list=heii qm&back=24&format=csv&flags=false");
            r.Add("multiple sites and parameters, with specific date range", "list=BEUO QU , BEU PM , BNOO PM , VAEO PM , RVDO PM , BLPO SE , LKCO SE , RCSO SE&start=2015-10-01&end=2016-09-30");
            return Print(r, "monthly", "Monthly database");
        }

        public static string PrintInstant()
        {
            var r = new Dictionary<string, string>();
            r.Add("all parameters last 24 hours for specified site", "list=bigi&back=24");
            r.Add("multiple sites and parameters for the last 24 hours" ,"list=boii ob, bfgi ob&back=24");
            r.Add("multiple sites and parameters, with specific date range", "list=boii ob, bfgi ob,pmai ob&start=2016-04-01&end=2016-04-2");
            r.Add("html format (no flags), with description above table", "list=boii ob, bfgi ob,pmai ob&back=12&format=html&flags=false&description=true");
            r.Add("wiski/kisters format", "list=boii ob, bfgi ob,pmai ob&back=12&format=zrxp");
            r.Add("15-minute idwr sites in Shef A format", "custom_list=idwr&format=shefa");
            r.Add("15-minute data, and 30 year daily average ", "list=heii q&daily=heii qd&format=realtime-graph");
            r.Add("most recent data for each series", "list=cra,crpo&format=recent");
            return Print(r,"instant","Near real-time data");
        }

        internal static string PrintDaily()
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


            return Print(r, "daily", "Daily Data");
        }

        internal static string PrintWaterYear()
        {
            var r = new Dictionary<string, string>();
            r.Add("water year report 2012", "site=abei&parameter=pp&start=2012&end=2012&format=usgs-html");
            r.Add("water year data.  Includes previous year and 30 year average", "site=culo&parameter=qd&start=2018&format=csv-analysis");

           return Print(r, "wyreport", "Water Year Report");
        }

        internal static string PrintInventory()
        {
            var r = new Dictionary<string, string>();
            r.Add("Daily Inventory", "site=hghm&interval=daily");
            r.Add("Instant Inventory", "site=hghm&interval=instant");
           return  Print(r, "inventory", "Inventory");
        }

        private static string Print(Dictionary<string, string> d, string cgi, string header)
        {
            DataTable t = new DataTable();
            t.Columns.Add("name");
            t.Columns.Add("example");

            if (CgiUtility.IsRemoteRequest())
                cgi += ".pl?";
            else
                cgi += "?";

            foreach (var item in d)
            {
                var example = "<a href=\"" + cgi + item.Value + "\">" + item.Value + "</a>";
                t.Rows.Add(item.Key,example);
            }
            var s = DataTableOutput.ToHTML(t, true, header);
            return s;
        }

        
    }
}
