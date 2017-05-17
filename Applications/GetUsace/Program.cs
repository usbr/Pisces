using Reclamation.Core;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Math = Reclamation.TimeSeries.Math;
namespace GetUsaceDaily
{
    /// <summary>
    /// Reads USACE project data (daily and instant)
    /// http://www.nwd-wc.usace.army.mil/ftppub/project_data/
    /// </summary>
    class Program
    {
        static void Test()
        {
            DateTime t;

            Console.WriteLine(CultureInfo.CurrentCulture.EnglishName);

            if (DateTime.TryParse("10Feb2013", out t))
            {
                Console.WriteLine(t.ToString());
            }
            else
            {
                Console.WriteLine("Error: parsing '10Feb2013'"); // mono prints this..
            }

            t = DateTime.ParseExact("10Feb2013","ddMMMyyyy",new CultureInfo("en-US"));
        }



        static void Main(string[] args)
        {
            Logger.EnableLogger();

            if (args.Length < 2 || args.Length > 3 )
            {
                Console.WriteLine("Usage: GetUsace site_list.csv hourly|daily  [dump.pdb] ");
                Console.WriteLine("Where: site_list.csv is a catalog of sites to import");
                Console.WriteLine("       houly or daily data");
                Console.WriteLine("       dump.db creates a test pisces database for comparison to hydromet");
                return;
            }

            FileUtility.CleanTempPath();

            CsvFile csv = new CsvFile(args[0]);
            //interval,filename,cbtt,pcode,header1,header2,header3,header4,header5
            //instant,gcl_h.dat,GCL,FB,Forebay,(ft),,,
            //instant,gcl_h.dat,GCL,TW,Tailwatr,(ft),,,
            //instant,gcl_h.dat,GCL,QE,Generatn,Flow,(kcfs),,

            TimeSeriesDatabase db=null;
            
            if (args.Length == 3)
            {
                SQLiteServer svr = new SQLiteServer(args[2]);
                db = new TimeSeriesDatabase(svr);
            }



            var rows = csv.Select("interval = '" + args[1] + "'");
            var interval = TimeInterval.Daily;
            if( args[1] == "hourly")
                interval = TimeInterval.Hourly;

            Console.WriteLine("Processing "+rows.Length+" parameters");
            for (int i = 0; i < rows.Length; i++)
            {
               var url = rows[i]["url"].ToString();
               var cbtt = rows[i]["cbtt"].ToString();
               var pcode = rows[i]["pcode"].ToString();
               

               string[] headers = GetHeaders(rows[i]);
               var soffset = rows[i]["offset"].ToString();
               double offset = 0;
               if (soffset.Trim() != "")
               {
                   offset = double.Parse(soffset);
               }
 
                var s = ProcessFile(url,interval, cbtt, pcode,offset, headers);



                if (db != null)
                {

                    SaveToDatabase(args, db, cbtt, pcode, s);
                }

            }


        }

        private static void SaveToDatabase(string[] args, TimeSeriesDatabase db, string cbtt, 
            string pcode, Series s)
        {
            Series hmet;
            if (args[1] == "daily")
            {
                hmet = Math.HydrometDaily(cbtt, pcode);
            }
            else
            {
                hmet = Math.HydrometInstant(cbtt, pcode);
            }
            hmet.Read(s.MinDateTime, s.MaxDateTime);
            s.Units = hmet.Units;
            s.Name = cbtt + "_" + pcode;
            db.AddSeries(s);
            db.AddSeries(hmet);
        }

        private static string[] GetHeaders(System.Data.DataRow dataRow)
        {
            var rval = new List<string>();
            for (int i = 1; i <= 5; i++) // max 5 headers
            {
                var s = dataRow["header" + i].ToString();
                if (s.Trim() == "")
                    break;
                rval.Add(s.Trim());
            }

            return rval.ToArray();

        }

        private static Series ProcessFile(string url,TimeInterval interval, string cbtt, string pcode,
            double offset, params string[] headers)
        {
            //Series s;
            //if (interval == TimeInterval.Hourly)
            //{
            //    s = CorpsHourlyFile.ReadCorpsDataFile(url, headers);
            //}
            //else
            //{
             var   s = CorpsDataFile.ReadCorpsDataFile(url, interval, headers);
           // }



            string units = headers[headers.Length - 1];

            if (offset != 0)
            {
                s = s + offset;
            }

            if  ( (cbtt.ToLower() == "gcl" || cbtt.ToLower() == "bnk") && pcode.ToLower() == "fb")
            {// compute acre-feet using rating table.

                var af = TimeSeriesDatabaseDataSet.RatingTableDataTable.ComputeSeries(s, cbtt.ToLower()+ "_af.txt");
                TimeSeriesTransfer.Import(af, cbtt, "af");
            }

            if (units == "(kcfs)" ||  units == "(kaf)" || pcode == "Q" || pcode == "QW" || pcode == "QE" )
            {// multiply by 1000
                Reclamation.TimeSeries.Math.Multiply(s, 1000.0);
            }

            TimeSeriesTransfer.Import(s, cbtt, pcode);

            return s;
        }



    }
}
