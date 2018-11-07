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

        public static void Import(Series s,string siteID, string parameter, string fileExtension="txt"){

            if (s.TimeInterval == TimeInterval.Daily)
            {
                var fn = TimeSeriesTransfer.GetIncommingFileName("daily", siteID, parameter,fileExtension);
                HydrometDailySeries.WriteToArcImportFile(s, siteID, parameter, fn);
            }
            if (s.TimeInterval == TimeInterval.Irregular ||
                s.TimeInterval == TimeInterval.Hourly)
            {
                var fn = TimeSeriesTransfer.GetIncommingFileName("instant", siteID, parameter,fileExtension);
                HydrometInstantSeries.WriteToHydrometFile(s, siteID, parameter,Environment.UserName , fn);
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

            var routes = RouteDestinations(list);
            foreach (string route in routes.Keys)
            {
                WriteListToOutgoingFile(routes[route], name, interval, route);
            }

        }
        private void WriteListToOutgoingFile(SeriesList list, string name, TimeInterval interval,  string route)
        {
            var tmpFileName = FileUtility.GetTempFileName(".txt");
            File.Delete(tmpFileName);
            Logger.WriteLine("writing " + list.Count + " series ");
            Logger.WriteLine("temp file:" + tmpFileName);

            if (interval == TimeInterval.Daily)
            {
                SeriesList exportList = new SeriesList();
                foreach (var s in list)
                {
                    exportList.Add(s);
                }
                HydrometDailySeries.WriteToArcImportFile(exportList, tmpFileName);
            }

            if (interval == TimeInterval.Irregular)
            {
                foreach (var s in list)
                {
                    HydrometInstantSeries.WriteToHydrometFile(s, s.SiteID, s.Parameter, Environment.UserName, tmpFileName, true);
                }
            }
            if (File.Exists(tmpFileName))
            {
                string fn = interval == TimeInterval.Daily ? "daily" : "instant";
                var fileName = GetOutgoingFileName(fn, name, "all", route);

                var dir = Path.GetDirectoryName(fileName);
                if (! Directory.Exists(fileName))
                {
                    var msg = "Error:  Directory does not exist " + dir;
                    Console.WriteLine(msg);
                    Logger.WriteLine(msg);
                }
                else
                {
                    Logger.WriteLine("Moving: " + tmpFileName);
                    Logger.WriteLine("To: " + fileName);
                    File.Move(tmpFileName, fileName);
                }
            }
        }



        private bool AllowExport(Series s)
        {
            var val = m_siteproperty.GetValue(s.SiteID, "export", "true");
            return  val.ToLower() == "true";
        }


        /// <summary>
        /// Returns of list of routes, from the site property table
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private Dictionary<string, SeriesList> RouteDestinations(SeriesList list)
        {
            var rval = new Dictionary<string, SeriesList>();
            foreach (var item in list)
            {
                if (!AllowExport(item))
                    continue;

                var prop = m_siteproperty.GetValue(item.SiteID, "route", "");

                var keys = prop.Split(',');
                for (int i = 0; i < keys.Length; i++)
                {
                    var key = keys[i].Trim();
                    AddToRoute(rval, item, key);
                }
                
            }
            return rval;
        }

        private static void AddToRoute(Dictionary<string, SeriesList> rval, Series s, string key)
        {
            SeriesList sl = null;
            if (!rval.ContainsKey(key))
            {
                sl = new SeriesList();
                rval.Add(key, sl);
            }
            else
            {
                sl = rval[key];
            }

            sl.Add(s);
        }


        /// <summary>
        /// Path for incoming data files
        /// </summary>
        /// <returns></returns>
        static string GetIncommingFileName(string prefix, string cbtt, string pcode, string fileExtension)
        {
            string incoming = ConfigurationManager.AppSettings["incoming"];
            if (incoming == "" || incoming == null)
            {
                Console.WriteLine("Error: 'incoming' directory not defined in config file");
                Logger.WriteLine("Error: 'incoming' directory not defined in config file");

                // hack hack hack
                // hack hack hack
                if (LinuxUtility.IsLinux())
                {
                    incoming = "/tmp";
                }
                else
                {
                    incoming = "C:\\Temp\\";
                }
            }

            return Path.Combine(incoming, TimeSeriesTransfer.GetUniqueFileName(incoming, prefix, cbtt, pcode,fileExtension));
        }

        public static string GetOutgoingFileName(string prefix, string cbtt, string pcode, string route = "")
        {
            string outgoing = ConfigurationManager.AppSettings["outgoing"];
            if (outgoing == "" || outgoing == null)
            {
                Console.WriteLine("Error: 'outgoing' directory not defined in config file");
                Logger.WriteLine("Error: 'outgoing' directory not defined in config file");

                // hack hack hack
                if (LinuxUtility.IsLinux())
                {
                    outgoing = "/tmp";
                } else
                {
                    outgoing = "C:\\Temp\\";
                }
            }

            if (!Directory.Exists(outgoing) || outgoing == null)
            {
                Console.WriteLine("Error: path does not exist: '" + outgoing + "'");
            }

            // route is subdirectory i.e.  /home/hydromet/outgoing/route
            if (route != "") 
                outgoing = Path.Combine(outgoing, route);

            if( !Directory.Exists(outgoing))
                Console.WriteLine("Error:  Directoy does not exist "+outgoing);

            return Path.Combine(outgoing, GetUniqueFileName(outgoing, prefix, cbtt, pcode));
        }
        internal static string GetUniqueFileName(string dir, string prefix, string cbtt, string pcode, string fileExtension="txt")
        {
            string fileName = prefix + "_" + cbtt + "_" + pcode + "_" + DateTime.Now.ToString("MMMdyyyyHHmmssfff") + "."+fileExtension;
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
