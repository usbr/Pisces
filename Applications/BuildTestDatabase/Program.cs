using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reclamation.Core;
using System.Data;
using System.IO;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;

namespace BuildTestDatabase
{
    /// <summary>
    /// Build a test database with precipitation data
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {


            string dbFileName = "test.pdb";
            if( File.Exists(dbFileName))
                File.Delete(dbFileName);

            SQLiteServer sqlite = new SQLiteServer(dbFileName);
            TimeSeriesDatabase db = new TimeSeriesDatabase(sqlite);

            // read seriescatalog.csv -- from the production database. This is a template without the data

            DataTable csv = new CsvFile("seriescatalog.csv");

            //get a list of site that have instant (irregular) data

            string[] sites = GetSitesWithPrecip(csv);

            // for each site with precipitation
            // -------------
            for (int i = 0; i < sites.Length; i++)
            {
                // add instant precip (pc -- raw in the bucket values)

                var folder = db.GetOrCreateFolder(sites[i]);
                Series s = new Series("", TimeInterval.Irregular);

                s.Table = HydrometInstantSeries.Read(sites[i], "pc", DateTime.Parse("2010-10-1"), DateTime.Now.Date, HydrometHost.PNLinux).Table;
                s.Table.TableName = "instant_" + sites[i] + "_pc";
                s.Name = s.Table.TableName;
                db.AddSeries(s, folder);

                if (i > 3) // make testing faster
                    break;

                // add daily  precip (pc -- midnight value of the bucket)
                // add daily incremental  ( pp -- daily sum of precipitation)
                // add daily cumulative (pu -- water year cummulative)

            }


        }

        private static string[] GetSitesWithPrecip(DataTable csv)
        {
            var rval = new List<string>();

            var rows = csv.Select("timeinterval = 'Irregular' and parameter = 'pc'");

            foreach (var item in rows)
            {
                rval.Add(item["siteid"].ToString());
            }

            return rval.ToArray();
        }
    }
}
