using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Reclamation.TimeSeries;
using System.Linq;
using System.Web;
using System.IO;

namespace PiscesWebServices
{
    /// <summary>
    /// query for a single site.
    /// </summary>
    public class SiteInfo
    {
        TimeSeriesDatabase db;
        public SiteInfo(TimeSeriesDatabase db)
        {
            this.db = db;
        }

        public void Run(string query = "")
        {
            Console.Write("Content-type: text/html\n\n");

            if (query == "")
                query = WebUtility.GetQuery();
            query = HttpUtility.HtmlDecode(query);

            if (!ValidQuery(query))
            {
                WebUtility.PrintHydrometTrailer("Error: Invalid query");
                return;
            }

            WriteCsv(query);
        }

        private static bool ValidQuery(string query)
        {
            if (query == "")
                return false;

            return Regex.IsMatch(query,"[^A-Za-z0-9=&%+-]");
        }


        private void WriteCsv(string query)
        {
            var cbtt = WebUtility.GetParameter(query,"cbtt");
            if (cbtt == "")
                return;

            var sites = db.GetSiteCatalog("siteid='" + db.Server.SafeSqlLiteral(cbtt) + "' ");
            if (sites.Count > 0)
            {
                sites.WriteXml(Console.Out, System.Data.XmlWriteMode.WriteSchema);
            }

        }

       


    }
}