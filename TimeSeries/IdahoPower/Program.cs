using System;
using System.Xml;
using System.Xml.XPath;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using System.IO;
using System.Configuration;
using Reclamation.Core;
namespace GetIdahoPowerData
{
    public class Program
    {
        static void Main(string[] args)
        {

            if (args.Length != 6)
            {
                Console.WriteLine("Usage:  GetIdahowPowerData stationID q|d type ndays cbtt pcode ");
                Console.WriteLine("Where:   q     -- quarter hour (15 minute data)");
                Console.WriteLine("         d     -- daily data");
                Console.WriteLine("        type   -- idaho power code  i.e. 'HCOut' ");
                Console.WriteLine("        ndays  -- how many days to retrieve");
                Console.WriteLine("        cbtt   -- hydromet cbtt ");
                Console.WriteLine("        pcode  -- hydromet parameter code");
                return;
            }


           string stationID = args[0];
           string period = args[1];
           string ipcoType = args[2];
           int numDays = 7;
           int.TryParse(args[3], out numDays);
           string cbtt = args[4];
           string pcode = args[5];

           TimeInterval interval = TimeInterval.Daily;

           if (period.ToLower() != "d")
               interval = TimeInterval.Irregular;


           var s = Reclamation.TimeSeries.IdahoPower.IdahoPowerSeries.GetIdahoPowerData(stationID, ipcoType, numDays, interval);

           if (interval == TimeInterval.Daily)
               TimeSeriesRouting.RouteDaily(s, cbtt, pcode, RouteOptions.Outgoing);
           if (interval == TimeInterval.Irregular)
               TimeSeriesRouting.RouteInstant(s, cbtt, pcode, RouteOptions.Outgoing);

        }

        
    }
}
