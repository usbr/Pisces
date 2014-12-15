using Reclamation.Core;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.DataLogger;
using Reclamation.TimeSeries.Hydromet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace HydrometServer
{
    /// <summary>
    /// Imports 15 minute data, and performs calculations that depend on the imported data.
    /// files with "decodes" prefix are imported. Others are ignored.
    /// </summary>
    class FileImporter
    {
        private TimeSeriesDatabase m_db;
        private bool m_computeDailyOnMidnight=false;
        private bool m_computeDependencies=false;
        string[] validPcodes = new string[]{};
        string[] validSites = new string[] { };
        private TimeSeriesImporter m_importer;

        public FileImporter(Reclamation.TimeSeries.TimeSeriesDatabase db)
        {
            m_db = db;
            if(  ConfigurationManager.AppSettings["ValidLoggerNetPcodes"] != null)
            validPcodes = ConfigurationManager.AppSettings["ValidLoggerNetPcodes"].Split(',');
            if (ConfigurationManager.AppSettings["ValidLoggerNetSites"] != null)
            validSites = ConfigurationManager.AppSettings["ValidLoggerNetSites"].Replace(" ", "").Replace("\r\n", "").Split(',');
        }
        
        internal void Import( string path, RouteOptions routing=RouteOptions.None,
            bool computeDependencies = false, bool computeDailyOnMidnight = false, string searchPattern = "*.*")
        {
            this.m_computeDailyOnMidnight = computeDailyOnMidnight;
            this.m_computeDependencies = computeDependencies;
            Console.WriteLine(path);
             DirectoryInfo di = new DirectoryInfo(path);
            FileSystemInfo[] fsinfos = di.GetFileSystemInfos(searchPattern);
            var ordered1 = fsinfos.OrderBy(f => f.CreationTime);
            Console.WriteLine("found "+ordered1.Count()+" items" );
            var ordered = ordered1.Where(f => (f.Attributes & FileAttributes.Directory) != FileAttributes.Directory);
            Console.WriteLine("Found "+ordered.Count()+" files to import");
            foreach (var fi in ordered) 
            {
                ProcessFile(routing, fi);
            }
          // needs .net 4.0 System.Threading.Tasks.Parallel.ForEach(ordered,(fi) => ProcessFile(routing,fi));

        }

        private void ProcessFile(RouteOptions routing, FileSystemInfo fi)
        {
            var fn = fi.FullName;
            string dir = System.IO.Path.GetDirectoryName(fn);

            if (fi.CreationTime.AddSeconds(2) > DateTime.Now)
            {
                Console.WriteLine(" skipping file newer than 2 seconds ago " + fn+ " "+fi.CreationTime);
                return;
            }
            string importTag = "import"; // used to make friendly export filename
            try
            {
                TextFile tf = new TextFile(fi.FullName);
                SeriesList sl = new SeriesList();

                if (HydrometInstantSeries.IsValidDMS3(tf)) 
                {
                    importTag = "decodes";
                    sl = Reclamation.TimeSeries.Hydromet.HydrometInstantSeries.HydrometDMS3DataToSeriesList(tf);
                }
                else if (LoggerNetFile.IsValidFile(tf))
                {
                    LoggerNetFile lf = new LoggerNetFile(tf);
                    
                    if (lf.IsValid && Array.IndexOf(validSites, lf.SiteName) >= 0)
                    {
                        importTag = lf.SiteName;
                        sl = lf.ToSeries(validPcodes);
                    }
                }
                else
                {
                    Console.WriteLine("skipped Unknown File Format: "+fn);
                    return;
                }

                m_importer = new TimeSeriesImporter(m_db, routing);
                Console.WriteLine("Found " + sl.Count + " series in " + fn);
                foreach (var item in sl)
                {
                    Logger.WriteLine(item.Table.TableName);
                }

                if (sl.Count > 0)
                {
                    m_importer.Import(sl, m_computeDependencies, m_computeDailyOnMidnight,importTag);
                    MoveToSubDirectory(Path.GetDirectoryName(fn), "attic", fn);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("skipping file, will move to error subdirectory " + fn);
                MoveToSubDirectory(Path.GetDirectoryName(fn), "error", fn);

            }
        }

        private static void MoveToSubDirectory(string path, string subdirectory, string file)
        {
            var attic = Path.Combine(path, subdirectory);

            if (!Directory.Exists(attic))
                Directory.CreateDirectory(attic);

            string dest = Path.Combine(attic, Path.GetFileName(file));
            if (File.Exists(dest))
                File.Delete(dest);

            File.Move(file, dest);
        }

    }
}
