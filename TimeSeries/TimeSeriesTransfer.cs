using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// Manage exporting data as part of the data processing stream
    /// </summary>
    public class TimeSeriesTransfer
    {
        TimeSeriesDatabase m_db;
        TimeSeriesDatabaseDataSet.sitepropertiesDataTable m_siteproperty;
        public TimeSeriesTransfer(TimeSeriesDatabase db)
        {
            m_db = db;
            m_siteproperty = m_db.GetSiteProperties();
        }

        public static void Import(Series s,string siteID, string parameter){

            if (s.TimeInterval == TimeInterval.Daily)
            {
                var fn = TimeSeriesTransfer.GetIncommingFileName("daily", siteID, parameter);
                HydrometDailySeries.WriteToArcImportFile(s, siteID, parameter, fn);
            }
            if (s.TimeInterval == TimeInterval.Irregular ||
                s.TimeInterval == TimeInterval.Hourly)
            {
                var fn = TimeSeriesTransfer.GetIncommingFileName("instant", siteID, parameter);
                HydrometInstantSeries.WriteToHydrometFile(s, siteID, parameter, WindowsUtility.GetShortUserName(), fn);
            }

        }

        /// <summary>
        /// Routes a list of Series as a group 
        /// hydromet cbtt is copied from list[i].SiteName
        /// hydromet pcode is copied from list[i].Parameter
        /// </summary>
        /// <param name="list"></param>
        /// <param name="route"></param>
        /// <param name="name">identity used as part of filename </param>
        public void Export(SeriesList list, string name, TimeInterval interval)
        {
            if (list.Count == 0)
                return;
           
            var tmpFileName = FileUtility.GetTempFileName(".txt");
            File.Delete(tmpFileName);
            Logger.WriteLine("writing " + list.Count + " series ");
            Logger.WriteLine("temp file:" + tmpFileName);

            if (interval == TimeInterval.Daily)
            {
                SeriesList exportList = new SeriesList();
                foreach (var s in list)
                {
                    if(AllowExport(s))
                    exportList.Add(s);
                }
                HydrometDailySeries.WriteToArcImportFile(exportList, tmpFileName);
            }

            if (interval == TimeInterval.Irregular)
            {
                foreach (var s in list)
                {
                    if (AllowExport(s))
                    HydrometInstantSeries.WriteToHydrometFile(s, s.SiteID, s.Parameter, WindowsUtility.GetShortUserName(), tmpFileName, true);
                }
            }
            if (File.Exists(tmpFileName))
            {
                Logger.WriteLine("Moving: " + tmpFileName);
                string fn = interval == TimeInterval.Daily ? "daily" : "instant";
                var fileName = GetOutgoingFileName(fn, name, "all");
                Logger.WriteLine("To: " + fileName);
                File.Move(tmpFileName, fileName);
            }

        }

        private bool AllowExport(Series s)
        {
            var val = m_siteproperty.GetValue(s.SiteID, "export", "true");
            return  val.ToLower() == "true";
        }



        /// <summary>
        /// Path for incoming data files
        /// </summary>
        /// <returns></returns>
         static string GetIncommingFileName(string prefix, string cbtt, string pcode)
        {
            string incoming = ConfigurationManager.AppSettings["incoming"];
            return Path.Combine(incoming, TimeSeriesTransfer.GetUniqueFileName(incoming, prefix, cbtt, pcode));
        }


        public static string GetOutgoingFileName(string prefix, string cbtt, string pcode)
        {
            string outgoing = ConfigurationManager.AppSettings["outgoing"];
            if (outgoing == "" || outgoing == null)
            {
                Console.WriteLine("Error: 'outgoing' directory not defined in config file");
                Logger.WriteLine("Error: 'outgoing' directory not defined in config file");
            }
            if( !Directory.Exists(outgoing) || outgoing == null)
            {
                Console.WriteLine("Error: path does not exist: '"+outgoing+"'");
            }
            return Path.Combine(outgoing, GetUniqueFileName(outgoing, prefix, cbtt, pcode));
        }

        internal static string GetUniqueFileName(string dir, string prefix, string cbtt, string pcode)
        {
            string fileName = prefix + "_" + cbtt + "_" + pcode + "_" + DateTime.Now.ToString("MMMdyyyyHHmmssfff") + ".txt";
            fileName = Path.Combine(dir, fileName.ToLower());

            if (File.Exists(fileName))
            {
                Console.WriteLine("ERROR: " + fileName + " allready exists, will use GUID");
                fileName = prefix + "_" + cbtt + "_" + pcode + "_" + Guid.NewGuid().ToString() + ".txt";
                fileName = Path.Combine(dir, fileName.ToLower());

            }

            return fileName;
        }
    }
}
