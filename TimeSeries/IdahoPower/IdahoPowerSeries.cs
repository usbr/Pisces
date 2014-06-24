using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Reclamation.TimeSeries.IdahoPower
{
    public class IdahoPowerSeries:Series
    {

        string m_ipcoType;

        public IdahoPowerSeries(string stationID, string ipcoType, TimeInterval interval)
        {
            this.TimeInterval = interval;
            this.SiteID = stationID;
            m_ipcoType = ipcoType;
        }


        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            Clear();
            var s = GetIdahoPowerData(this.SiteID, m_ipcoType, this.TimeInterval, t1, t2);
            this.Add(s);
        }
        /// <summary>
        /// </summary>
        /// <param name="stationID">example: 13245000</param>
        /// <param name="ipcoType">Q,Qin,HW,Qx,S</param>
        /// <param name="numDays">how many days back</param>
        /// <returns></returns>
        public static Series GetIdahoPowerData(string stationID, string ipcoType, int numDays, TimeInterval interval)
        {
            string xmlFileName = GetIdahoPowerXmlFile(stationID, interval, numDays);

            var s = ParseXmlData(xmlFileName, stationID, ipcoType, interval);
            return s;
        }

        public static Series GetIdahoPowerData(string stationID, string ipcoType, TimeInterval interval, DateTime t1, DateTime t2)
        {
            if( t1 > DateTime.Now)
                return new Series();

            var ts = TimeSpan.FromTicks( DateTime.Now.Ticks- t1.Ticks );

            string xmlFileName = GetIdahoPowerXmlFile(stationID, interval, ts.Days);

            var s = ParseXmlData(xmlFileName, stationID, ipcoType, interval);

            s.Trim(t1, t2);
            return s;
        }


        private static string GetIdahoPowerXmlFile(string stationID, TimeInterval interval, int numDays)
        {
            string period = "d"; // daily
            if (interval != TimeInterval.Daily)
                period = "q"; // quarter hour (15 min)

            // example: http://www.idahopower.com/OurEnvironment/WaterInformation/StreamFlow/GetStreamData.cfm?stationID=13289702&days=2&period=d
            //<add key="idahoPowerURL" value="http://www.idahopower.com/OurEnvironment/WaterInformation/StreamFlow/GetStreamData.cfm?stationID=__stationid__&days=__days__&period=__period__"/>

            string url = ConfigurationManager.AppSettings["idahoPowerURL"];
            url = url.Replace("__stationid__", stationID);
            url = url.Replace("__days__", numDays.ToString());
            url = url.Replace("__period__", period);

            string tmpFileName = FileUtility.GetTempFileName(".xml");
            Web.GetFile(url, tmpFileName);
            return tmpFileName;
        }


        static Series ParseXmlData(string xmlFileName, string stationID, string ipcoType, TimeInterval interval)
        {
            string period = "d"; // daily
            if (interval != TimeInterval.Daily)
                period = "q"; // quarter hour (15 min)

            Series s = new Series();
            //s.TimeInterval = TimeInterval.Daily;   
            XPathDocument doc = new XPathDocument(xmlFileName);
            var nav = doc.CreateNavigator();
            var ns = new XmlNamespaceManager(nav.NameTable);
            ns.AddNamespace("ns1", "getStreamDataWS");
            //            var query = "/ns1:Station[@Station_ID='13289702']/ns1:DataType[@TYPE='HW']/ns1:Reading[@PERIOD='D']";
            //var query = "/ns1:Station[@Station_ID='13289702']/ns1:DataType[@TYPE='" + ipcoType + "']/ns1:Reading[@PERIOD='D']";
            var query = "/ns1:Station[@Station_ID='" + stationID + "']/ns1:DataType[@TYPE='" + ipcoType + "']/ns1:Reading[@PERIOD='" + period.ToUpper() + "']";
            var nodes = nav.Select(query, ns);

            while (nodes.MoveNext())
            {
                DateTime t;
                double d;
                if (DateTime.TryParse(nodes.Current.GetAttribute("Date", ""), out t)
                     && double.TryParse(nodes.Current.GetAttribute("Value", ""), out d))
                {
                    Console.WriteLine(t.ToString() + ", " + d);
                    s.Add(t, d);
                }
            }
            Console.WriteLine("Read " + s.Count + " data points  for " + ipcoType);
            s.TimeInterval = Series.EstimateInterval(s);
            return s;
        }


    }
}
