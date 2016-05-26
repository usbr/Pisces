using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PiscesWebServices
{
    /// <summary>
    /// dump site data from Pisces meta-data in csv format
    /// </summary>
    class SiteCsvTable
    {
        TimeSeriesDatabase db;
        public SiteCsvTable(TimeSeriesDatabase db)
        {
            this.db = db;
        }
        /// <summary>
        /// Prints csv dump of Sites
        /// </summary>
        /// <param name="propertyFilter">two part filter in property table i.e.  'program:agrimet'</param>
        public void Execute( string propertyFilter="")
        {
            Console.Write("Content-Type:  text/csv\n\n");
            Console.WriteLine("Content-disposition: attachment;filename=location.csv");

            var sites = db.GetSiteCatalog(propertyFilter: propertyFilter);

            var fn = FileUtility.GetTempFileName(".csv");
            CsvFile.WriteToCSV(sites, fn, false, true);
            var lines = File.ReadAllLines(fn);
            foreach (var item in lines)
            {
                Console.WriteLine(item);    
            }

        }
    }
}
