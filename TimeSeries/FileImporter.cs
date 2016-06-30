using Reclamation.Core;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.DataLogger;
using Reclamation.TimeSeries.Decodes;
using Reclamation.TimeSeries.Hydromet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// Imports 15 minute data, and performs calculations that depend on the imported data.
    /// </summary>
    public class FileImporter
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


        public void ImportFile(string fileName, RouteOptions routing = RouteOptions.None,
           bool computeDependencies = false, bool computeDailyOnMidnight = false, string searchPattern = "*.*")
        {
            this.m_computeDailyOnMidnight = computeDailyOnMidnight;
            this.m_computeDependencies = computeDependencies;
            Console.WriteLine(fileName);
            ProcessFile(routing, fileName);
        }
        public void Import( string path, RouteOptions routing=RouteOptions.None,
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
                var fn = fi.FullName;

                if (fi.CreationTime.AddSeconds(2) > DateTime.Now)
                {
                    Console.WriteLine(" skipping file newer than 2 seconds ago " + fn + " " + fi.CreationTime);
                    continue;
                }
                ProcessFile(routing, fi.FullName);
            }
          // needs .net 4.0 System.Threading.Tasks.Parallel.ForEach(ordered,(fi) => ProcessFile(routing,fi));

        }

        private void ProcessFile(RouteOptions routing, string fileName)
        {
            
            string importTag = "import"; // used to make friendly export filename
            try
            {
                TextFile tf = new TextFile(fileName);
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
                else if (DecodesRawFile.IsValidFile(tf))
                {
                    DecodesRawFile df = new DecodesRawFile(tf);
                    importTag = "raw";
                    sl = df.ToSeries();
                }
                else
                {
                    Console.WriteLine("skipped Unknown File Format: " + fileName);
                    return;
                }

                m_importer = new TimeSeriesImporter(m_db, routing);
                Console.WriteLine("Found " + sl.Count + " series in " + fileName);
                foreach (var item in sl)
                {
                    Logger.WriteLine(item.Table.TableName);
                }

                if (sl.Count > 0)
                {
                    m_importer.Import(sl, m_computeDependencies, m_computeDailyOnMidnight,importTag);
                    FileUtility.MoveToSubDirectory(Path.GetDirectoryName(fileName), "attic", fileName);
                }

            }
            catch (Exception ex)
            {
                Logger.WriteLine("Error:" + ex.Message);
                Console.WriteLine("Error:  skipping file, will move to error subdirectory " + fileName);
                FileUtility.MoveToSubDirectory(Path.GetDirectoryName(fileName), "error", fileName);

            }
        }

       

    }
}
