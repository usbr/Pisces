using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Reclamation.TimeSeries
{

    public enum RouteOptions { None,Incoming, Outgoing, Both };

    public class TimeSeriesRouting
    {

        /// <summary>
        /// Path for incoming data files
        /// </summary>
        /// <returns></returns>
        private static string GetIncommingFileName(string prefix, string cbtt, string pcode)
        {
            string incoming = ConfigurationManager.AppSettings["incoming"];
            return Path.Combine(incoming, GetUniqueFileName(incoming,prefix, cbtt, pcode));
        }
        public static string GetOutgoingFileName(string prefix, string cbtt, string pcode)
        {
            string outgoing = ConfigurationManager.AppSettings["outgoing"];
            if (outgoing == "" || outgoing == null)
            {
                Console.WriteLine("Error: 'outgoing' directory not defined in config file");
                Logger.WriteLine("Error: 'outgoing' directory not defined in config file");
            }
            return Path.Combine(outgoing, GetUniqueFileName(outgoing,prefix, cbtt, pcode));
        }


        private static string GetUniqueFileName(string dir,string prefix, string cbtt, string pcode)
        {
            string fileName = prefix + "_" + cbtt + "_" + pcode + "_" + DateTime.Now.ToString("MMMdyyyyHHmmssfff") + ".txt";
            fileName = Path.Combine(dir, fileName.ToLower());
        
            if (File.Exists(fileName))
            {
                Console.WriteLine("ERROR: "+fileName + " allready exists, will use GUID");
                fileName = prefix + "_" + cbtt + "_" + pcode + "_" + Guid.NewGuid().ToString() + ".txt";
                fileName = Path.Combine(dir, fileName.ToLower());
        
            }

            return fileName;
        }

        /// <summary>
        /// Routes a list of Series as a group 
        /// hydromet cbtt is copied from list[i].SiteName
        /// hydromet pcode is copied from list[i].Parameter
        /// </summary>
        /// <param name="list"></param>
        /// <param name="route"></param>
        /// <param name="name">identity used as part of filename </param>
        public static void RouteDaily(SeriesList list, string name, RouteOptions route = RouteOptions.Both)
        {
            if (list.Count == 0)
                return;

            string fileName = "";

            if (route == RouteOptions.Both || route == RouteOptions.Outgoing)
            {
                fileName = GetOutgoingFileName("daily", name, "all");
                Console.WriteLine("saving daily outgoing to:"+fileName);
                HydrometDailySeries.WriteToArcImportFile(list, fileName);
            }

            if (route == RouteOptions.Both || route == RouteOptions.Incoming)
            {
                foreach (var s in list)
                {
                    fileName = GetIncommingFileName("daily", s.SiteID, s.Parameter);
                    Console.WriteLine("saving daily incoming to:" + fileName);
                    s.WriteCsv(fileName, true);
                }

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
        public static void RouteInstant(SeriesList list, string name, RouteOptions route = RouteOptions.Both)
        {
            if (list.Count == 0)
                return;
            
            if (route == RouteOptions.None)
                return;

            if (route == RouteOptions.Both || route == RouteOptions.Outgoing)
            {

                
                var tmpFileName = FileUtility.GetTempFileName(".txt");
                File.Delete(tmpFileName);
                Logger.WriteLine("writing " + list.Count + " series ");
                Logger.WriteLine("temp file:" + tmpFileName);
                Performance p = new Performance();
                foreach (var s in list)
                {
                  HydrometInstantSeries.WriteToHydrometFile(s, s.SiteID, s.Parameter, WindowsUtility.GetShortUserName(), tmpFileName,true);
                }
                Logger.WriteLine("finished saving in " + p.ElapsedSeconds + " seconds");
                Logger.WriteLine("Moving: " + tmpFileName);
                var fileName = GetOutgoingFileName("instant", name, "all");
                Logger.WriteLine("To: " + fileName);
                File.Move(tmpFileName, fileName);

            }
            else
            {
                throw new NotImplementedException("incoming not supported.");
            }

           
        }

        

        public static void RouteDaily(Series s, string cbtt, string pcode, RouteOptions route = RouteOptions.Both)
        {
            if (s.Count == 0)
                return;
            string fileName = "";

            if (route == RouteOptions.Both || route == RouteOptions.Outgoing)
            {
                fileName = GetOutgoingFileName("daily", cbtt, pcode);
                Console.WriteLine(fileName);
                HydrometDailySeries.WriteToArcImportFile(s, cbtt, pcode, fileName);
            }

            if (route == RouteOptions.Both || route == RouteOptions.Incoming)
            {
                fileName = GetIncommingFileName("daily", cbtt, pcode);
                s.WriteCsv(fileName, true);
            }
        }

      
        public static void RouteInstant(Series s, string cbtt, string pcode, RouteOptions route = RouteOptions.Both)
        {
            if (s.Count == 0)
                return;
            string fileName = "";
            if (route == RouteOptions.Both || route == RouteOptions.Outgoing)
            {
                fileName = GetOutgoingFileName("instant", cbtt, pcode);
                 HydrometInstantSeries.WriteToHydrometFile(s, cbtt, pcode, WindowsUtility.GetShortUserName(),fileName);
            }
            if (route == RouteOptions.Both || route == RouteOptions.Incoming)
            {
                fileName = GetIncommingFileName("instant", cbtt, pcode);
                HydrometInstantSeries.WriteToHydrometFile(s, cbtt, pcode, WindowsUtility.GetShortUserName(), fileName);
            }
        }

       

    }
}
