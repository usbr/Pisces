using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.TimeSeries.Nrcs;
using Reclamation.TimeSeries;
using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;
using System.Net;

namespace GetSnotel
{
    class Program
    {

        // TO DO: use hydromet convention for time stamps for PC and SWE?

        /// <summary>
        /// Reads daily snowtel data from NRCS web service 
        /// and saves to a text file 
        ///  --output=snowtel.txt              : output filename (required)
        ///  --t1=1-31-2013                    : starting date: default is 30 days ago
        ///  --t2=1-31-2013                    : ending date: default is today
        ///  --filter="cbtt='PVRO'"            : filter snotel site list.
        ///  --debug                           : enable debugging
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] argList)
        {


            ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            System.Security.Cryptography.AesCryptoServiceProvider b = new System.Security.Cryptography.AesCryptoServiceProvider();

            string outputFileName = "";
            bool appendToFile = false; // for output file.


            Arguments args = new Arguments(argList);

            if (args.Contains("output"))
            {
                outputFileName = args["output"];
                Console.WriteLine("Saving to "+outputFileName);
            }
            else
            {
                Console.WriteLine("Usage:  GetSnotel --output=filename [--debug] [--t1=1-31-2013] [--t2=1-31-2013] [--filter=\"cbtt='jkpi'\"]");
                return;
            }


            if (args.Contains("debug"))
            {
                Logger.EnableLogger();
                Reclamation.TimeSeries.Parser.SeriesExpressionParser.Debug = true;
            }


            string filter = "";
            if (args.Contains("filter"))
            {
                filter = args["filter"];
            }


            DateTime t1 = DateTime.Now.AddDays(-30);
            DateTime t2 = DateTime.Now;

            if (args.Contains("t1"))
                t1 = DateTime.Parse(args["t1"]);
            if (args.Contains("t2"))
                t2 = DateTime.Parse(args["t2"]);



            var tbl = NrcsSnotelSeries.SnotelSites;

            if (filter != "")
            {
                tbl = DataTableUtility.Select(tbl, filter, "");
                Logger.WriteLine("found " + tbl.Rows.Count + " with filter=" + filter);
            }


            for (int i = 0; i < tbl.Rows.Count; i++)
            {


                var cbtt = tbl.Rows[i]["cbtt"].ToString();
                if (cbtt.Trim() == "")
                    continue;
                Logger.WriteLine(cbtt+" ");
                double pct = (double)i / (double)tbl.Rows.Count * 100;
                Console.WriteLine(cbtt + " "+pct.ToString("F1")+"%");
                var triplet = SnotelSeries.GetTriplet(cbtt);


                Series pc = new SnotelSeries(triplet, SnotelParameterCodes.PREC);
                Series se = new SnotelSeries(triplet, SnotelParameterCodes.WTEQ); 
                Series sd = new SnotelSeries(triplet, SnotelParameterCodes.SNWD);
                Series mm = new SnotelSeries(triplet, SnotelParameterCodes.TAVG);
                Series mx = new SnotelSeries(triplet, SnotelParameterCodes.TMAX);
                Series mn = new SnotelSeries(triplet, SnotelParameterCodes.TMIN);

                Series[] items = new Series[] {pc,se,sd,mm,mx,mn };
                String[] pcodes = new string[] { "pc", "se", "sd", "mm", "mx", "mn" };
                for (int p = 0; p < items.Length; p++)
			      {
                    var s = items[p];

                    s.Read(t1, t2);
                    
                    if (s.Count == 0)
                    { // add to message
                        Console.WriteLine("No data found for "+cbtt+"/"+pcodes[p]);
                       // Console.WriteLine(s.Messages.ToString());
                    }
                    else
                    {
                        //TimeSeriesRouting.RouteDaily(s, cbtt, pcodes[p], RouteOptions.Incoming);
                        HydrometDailySeries.WriteToArcImportFile(s, cbtt, pcodes[p], outputFileName, appendToFile);
                        if (!appendToFile)
                            appendToFile = true; // append after the first time.
                    }

                    System.Threading.Thread.Sleep(100); // small pause seems to be preventing timeouts?
                  }

                // compute snow density
                if (se.Count > 0 && sd.Count >0)
                {
                    Series ss = se / sd;
                    for (int j = 0; j < ss.Count; j++)
                    {
                        var pt = ss[j];
                        if ( (sd.IndexOf(pt.DateTime) >=0 &&  sd[pt.DateTime].Value == 0 )
                            || (se.IndexOf(pt.DateTime) >=0 &&  se[pt.DateTime].Value == 0 ))
                        {
                            pt.Value = 0;
                            pt.Flag = "";
                            ss[j] = pt;
                        }

                    }


                   //TimeSeriesRouting.RouteDaily(ss, cbtt, "ss", RouteOptions.Incoming);
                    HydrometDailySeries.WriteToArcImportFile(ss, cbtt, "ss", outputFileName, appendToFile);
                    if (!appendToFile)
                        appendToFile = true; // append after the first time.
                }

            }
            Console.WriteLine((GC.GetTotalMemory(false) / 1024.0 / 1024.0).ToString("F3") + " Mb memory used");

        }

    }
}
