using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Reclamation.TimeSeries;
using Reclamation.Core;
using System.Configuration;
using System.IO;
using Mono.Options;

namespace Rwis.Initialize
{
    class Program
    {
        static bool initializeSeriesCatalog = false;
        static bool initializeParameterCatalog = false;
        static private string path = @"C:\Users\jrocha\Desktop\RWIS\";

        static void initializeMain(string[] argList)
        {
            // Connect to DB Server
            var dbname = ConfigurationManager.AppSettings["MySqlDatabase"];
            var server = ConfigurationManager.AppSettings["MySqlServer"];
            var user = ConfigurationManager.AppSettings["MySqlUser"];
            var svr = MySqlServer.GetMySqlServer(server, dbname, user);
            var db = new TimeSeriesDatabase(svr);
            if (initializeParameterCatalog)
            {
                // Get Parameter Catalog CSV
                var parCatCSV = ConvertCSVtoDataTable(path + "parametercatalog.csv");
                // Get DB Parameter Catalog
                var parCat = db.GetParameterCatalog();
                // Populate DB parametercatalog
                foreach (DataRow row in parCatCSV.Rows)
                {
                    parCat.AddparametercatalogRow(row["id"].ToString(), row["timeinterval"].ToString(), row["units"].ToString(),
                        row["statistic"].ToString(), row["name"].ToString());
                }
                // Save Table
                svr.SaveTable(parCat);
            }
            if (initializeSeriesCatalog)
            {
                // Get Site Catalog CSV
                var steCatCSV = ConvertCSVtoDataTable(path + "sitecatalog.csv");
                // Get DB Site Catalog
                var steCat = db.GetSiteCatalog();
                // Populate DB sitecatalog
                foreach (DataRow row in steCatCSV.Rows)
                {
                    steCat.AddsitecatalogRow(row["siteid"].ToString(), row["description"].ToString(), row["state"].ToString(),
                        row["latitude"].ToString(), row["longitude"].ToString(), row["elevation"].ToString(), row["timezone"].ToString(),
                        row["install"].ToString(), row["horizontal_datum"].ToString(), row["vertical_datum"].ToString(),
                        Convert.ToDouble(row["vertical_accuracy"]), row["elevation_method"].ToString(), row["tz_offset"].ToString(),
                        row["active_flag"].ToString(), row["type"].ToString(), row["responsibility"].ToString(), row["agency_region"].ToString());
                }
                // Save Table
                svr.SaveTable(steCat);
            }


        }


        /// <summary>
        /// Converts CSV file to C# DataTable
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <returns></returns>
        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                { dt.Columns.Add(header); }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    { dr[i] = rows[i]; }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
    }
}