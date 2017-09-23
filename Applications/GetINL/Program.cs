using Reclamation.Core;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace GetINL
{
    /// <summary>
    /// Download INL data as a zipped xml file. One file per day.
    /// convert to time series data for importing to Hydromet
    /// </summary>
    class Program
    {

        private static void Usage()
        {
            Console.WriteLine("Usage:  GetINL.exe --output=instant_inl_data.txt --config=inl_config.csv [--t=2014-2-1] | [ --t1=2014-3-24 --t2=2014-3-26 ]  --cbtt=ABEI");
        }


        static void Main(string[] argList)
        {

            if ( argList.Length == 0)
            {
                Usage();
                return;
            }

            DateTime t = DateTime.Now.Date;
            bool recentDataOnly = true;// defaults using only last 4 hours, unless dates are specified.
            

            Arguments args = new Arguments(argList);

            if (args.Contains("t"))
            {
                recentDataOnly = false;
                if (!DateTime.TryParse(args["t"], out t))
                {
                    Console.WriteLine("Error: invalid date '" + args["t"] + "'");
                    Usage();
                    return;
                }
            }
            DateTime t1 = t;
            DateTime t2 = t;

            if (args.Contains("t1"))
            {
                recentDataOnly = false;
                if (!DateTime.TryParse(args["t1"], out t1))
                {
                    Console.WriteLine("Error: invalid date t1 '" + args["t1"] + "'");
                    Usage();
                    return;
                }
            }
            if (args.Contains("t2"))
            {
                recentDataOnly = false;
                if (!DateTime.TryParse(args["t2"], out t2))
                {
                    Console.WriteLine("Error: invalid date t2 '" + args["t2"] + "'");
                    Usage();
                    return;
                }
            }

            if (!args.Contains("config"))
            {
                Console.WriteLine("Error:  --config=filename.csv  is required");
                Usage();
                return;
            }

            if (!args.Contains("output"))
            {
                Console.WriteLine("Error:  --output=filename.txt  is required");
                Usage();
                return;
            }
           

            // read config file.
            // cbtt,inel_id,inel_code,hydromet_pcode

            DataTable csv = new CsvFile(args["config"], CsvFile.FieldTypes.AllText);

            if( args.Contains("cbtt")) // filter specific site
            {
                Console.WriteLine("Filtering for cbtt = '"+args["cbtt"]+"'");
                csv = DataTableUtility.Select(csv,"cbtt='"+args["cbtt"]+"'","");
            }

            t = t1;
            while (t <= t2)
            {
                ProcessDate(t, args, csv, recentDataOnly);
                t = t.AddDays(1).Date;
            }
        }

        private static void ProcessDate(DateTime t, Arguments args, DataTable csv, bool recentDataOnly)
        {

            var xmlFileName = DownloadAndUnzip(t);

            XPathDocument doc = new XPathDocument(xmlFileName);

            Series ob = new Series(); // air temp
            Series tu = new Series(); //relative humidity

            Console.WriteLine("Saving to " + args["output"]);
            foreach (DataRow row in csv.Rows)
            {
                var cbtt = row["cbtt"].ToString();
                if (cbtt.Trim() == "")
                    continue;

                var inl_id = row["inl_id"].ToString();
                var inel_element = row["inel_element"].ToString();
                var hydromet_pcode = row["hydromet_pcode"].ToString();
                Console.Write(inl_id + " " + inel_element + "(" + hydromet_pcode + ")");
                //var s = ParseXmlData(doc,"ABE","ws",t);
                var s = ParseXmlData(doc, inl_id, inel_element, t);
                s.Parameter = hydromet_pcode;
                s.SiteID = cbtt;
                
                if (hydromet_pcode.ToLower() == "pi")
                    s = s * 0.01;
                
                if (inel_element.ToLower() == "sr"
                    || inel_element.ToLower() == "sr2")
                {
                    //0.0860 W/m2 = 1 Langley
                    s = s * 0.0860; // divide by 12 to convert to hourly rate (5 minute data)
                }


                if (hydromet_pcode.ToLower() == "ob")
                    ob = s;
                if (hydromet_pcode.ToLower() == "tu")
                    tu = s;

                if (recentDataOnly && t.Date == DateTime.Now.Date)
                {// only load the last 4 hours when reading the current date
                    Console.WriteLine("only saving last 4 hours of data");
                    s.Trim(DateTime.Now.AddHours(-4), DateTime.Now.AddHours(2));
                }

                HydrometInstantSeries.WriteToHydrometFile(s, cbtt, hydromet_pcode, "inl", args["output"], true);

                var tp = DewPointCalculation(ob, tu, cbtt);
                if (tp.Count > 0)
                {
                    HydrometInstantSeries.WriteToHydrometFile(tp, cbtt, "TP", "", args["output"], true);
                }

            }
        }

        private static string DownloadAndUnzip(DateTime t)
        {
            //http://www.noaa.inel.gov/mvp/data/2014/02/x28.zip
            string url = System.Configuration.ConfigurationManager.AppSettings["INEL_URL"];

            url += t.Year + "/" + t.Month.ToString().PadLeft(2, '0') + "/x" + t.Day.ToString().PadLeft(2, '0') + ".zip";
            var zip = FileUtility.GetTempFileName(".zip");
            Console.WriteLine("Downloading: " + url);
            Web.GetFile(url, zip);


            var xmlFileName = FileUtility.GetTempFileName(".xml");
            Console.WriteLine("Unzipping to-> " + xmlFileName);
            ZipFileUtility.UnzipFile(zip, xmlFileName);
            return xmlFileName;
        }

        private static Series DewPointCalculation(Series ob, Series tu, string cbtt)
        {
            Series tp = new Series();

            if (tu.SiteID == cbtt && ob.SiteID == cbtt
                && tu.Count > 0 && ob.Count > 0)
            {
                for (int i = 0; i < ob.Count; i++)
                {
                    var pt = ob[i];
                    if (tu.IndexOf(pt.DateTime) >= 0)
                    {
                        double tpVal = AsceEtCalculator.DewPtTemp(pt.Value, tu[pt.DateTime].Value);
                        if (!double.IsNaN(tpVal))
                        {
                            tp.Add(pt.DateTime, tpVal);
                        }
                        else
                        {
                            Console.WriteLine();
                        }
                    }
                }
            }
            return tp;
        }

        static Series ParseXmlData(XPathDocument doc, string siteID, string inel_element, DateTime t)
        {

            Series s = new Series();
            
            var nav = doc.CreateNavigator();


            DateTime inlTime = t.Date; // used to increment each 5 minutes

            TimeZoneInfo mst ;
             TimeZoneInfo mdt ;
             if (LinuxUtility.IsLinux())
             {
                 //var c = TimeZoneInfo.GetSystemTimeZones();
                 //foreach (var item in c)
                 //{
                 //    Console.WriteLine(item);
                 //}

                 mst = TimeZoneInfo.FindSystemTimeZoneById("US/Arizona"); // no daylight shift
                 mdt = TimeZoneInfo.FindSystemTimeZoneById("US/Mountain"); // with daylight shifting
             }
             else
             {
                 mst = TimeZoneInfo.FindSystemTimeZoneById("US Mountain Standard Time"); // no daylight shift
                 mdt = TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"); // with daylight shifting
             }

            string ts = "";
            do
            {
                ts = inlTime.ToString("HH:mm"); 
                var query = "mesonet/Time[@time=\"" + ts + "\"]/tower[@id=\"" + siteID + "\"]"; // /spd";

                var nodeName = inel_element;
                var tag = "";
                int idx = inel_element.IndexOf(".");
                if (idx > 0)
                {
                    tag = inel_element.Substring(0, idx);
                    query += "/" + tag;
                    nodeName = inel_element.Substring(idx + 1);
                }
                var nodes = nav.Select(query);

                while (nodes.MoveNext())
                {
                    //Console.WriteLine(ts+ "id=" + nodes.Current.GetAttribute("id", ""));

                    if (nodes.Current.HasChildren)
                    {
                        var children = nodes.Current.SelectChildren(XPathNodeType.All);
                        while (children.MoveNext())
                        {
                            var n = children.Current;
                            if (n.LocalName == nodeName)
                            {
                               // Console.WriteLine(n.LocalName + " = " + n.Value);
                                double val =0;
                                if (double.TryParse(n.Value, out val))
                                {
                                    //s.Add(t.Date.AddHours(time.Hour).AddMinutes(time.Minute), val, "inl");
                                    DateTimeWithZone td = new DateTimeWithZone(inlTime, mst);
                                    DateTime hydrometDateTime;
                                    if (td.TryConvertToTimeZone(mdt, out hydrometDateTime))
                                    {

                                        if (s.IndexOf(hydrometDateTime) < 0)
                                        {// in November time change, we have duplicate dates..
                                            s.Add(hydrometDateTime, val, "inl");
                                        }
                                        else
                                        {
                                            Console.WriteLine("skipping dateTime "+hydrometDateTime.ToString() );
                                        }
                                    }

                                }
                            }
                            else if (n.LocalName == inel_element)
                            {

                            }
                        }
                    }
                }
                inlTime = inlTime.AddMinutes(5);
            } while (ts != "23:55");

            Console.WriteLine("Read " + s.Count + " data points ");
            return s;
        }
    }
}
