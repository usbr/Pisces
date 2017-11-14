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
    public class SiteInfoCGI
    {
        TimeSeriesDatabase db;
        public SiteInfoCGI(TimeSeriesDatabase db)
        {
            this.db = db;
        }

        public void Run(string query = "")
        {
            Console.Write("Content-type: text/html\n\n");

            if (query == "")
                query = HydrometWebUtility.GetQuery();
            query = System.Uri.UnescapeDataString(query);

            if (!ValidQuery(query))
            {
                HydrometWebUtility.PrintHydrometTrailer("Error: Invalid query");
                return;
            }

            WriteCsv(query);
        }

        private static bool ValidQuery(string query)
        {
            if (query == "")
                return false;

            return !Regex.IsMatch(query,"[^A-Za-z0-9=&%+\\-]");
        }


        private void WriteCsv(string query)
        {
            var cbtt = HydrometWebUtility.GetParameter(query,"cbtt");
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