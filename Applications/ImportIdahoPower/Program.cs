using Reclamation.Core;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportIdahoPower
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length != 3)
            {
                Console.WriteLine("Usage: ImportIdahoPower list.csv outputDirectory hoursBack");
                Console.WriteLine("Example:");
                Console.WriteLine("ImportIdahoPower.exe idaho_power_list.csv /home/hydromet/incoming/  12");
                return;
            }

            var csv = new CsvFile(args[0]);
            var dir = args[1];
            var hoursBack = Convert.ToInt32(args[2]);
            for (int i = 0; i < csv.Rows.Count; i++)
            {
                var row = csv.Rows[i];
                var id = row["id"].ToString();
                if(id.Trim() == "")
                {
                    continue;
                }
                Console.WriteLine(row["name"]);
                
                Series s = GetData(id,hoursBack);
                SaveToFile(row, s,dir);
            }
        }


        private static void SaveToFile(System.Data.DataRow row, Series s,string dir)
        {
            var cbtt = row["cbtt"].ToString();
            var pcode = row["pcode"].ToString();
            var d = DateTime.Now.ToString("yyyyMMddHHmm");
            var interval = row["interval"].ToString();

            
            if (interval.ToLower() == "instant")
            {
                s.TimeInterval = TimeInterval.Irregular;
                var fn = Path.Combine(dir, "instant_ipco_" + cbtt + "_" + pcode +d+ ".txt");
                Console.WriteLine(fn);
                HydrometInstantSeries.WriteToHydrometFile(s, cbtt,
                  pcode , "idahopower", fn);
            }
            else if (interval.ToLower() == "daily")
            {
                s.TimeInterval = TimeInterval.Daily;
                var fn = Path.Combine(dir, "daily_ipco_" + cbtt + "_" + pcode + d + ".txt");
                Console.WriteLine(fn);
                HydrometDailySeries.WriteToArcImportFile(s, cbtt,
                  pcode,  fn);
            }
        }
        /*
         * 
Data Set Export - Flow.DayMean@13087505 Milner Lwr Pwr Plant at Milner - Range: 2017-05-12 00:00 - 2017-05-26 00:00 (UTC-07:00),,,,,
Data on this site may be provisional and subject to revision,,,,,
Timestamp (UTC-07:00),Value (Cubic Feet Per Second),Grade Code,Approval Level,Interpolation Type,Comment
2017-05-12 00:00:00,5260,0,,8,
2017-05-13 00:00:00,5250,0,,8,
2017-05-14 00:00:00,5250,0,,8,
2017-05-15 00:00:00,5260,0,,8,
2017-05-16 00:00:00,5240,0,,8,
2017-05-17 00:00:00,5240,0,,8,
2017-05-18 00:00:00,5200,0,,8,
2017-05-19 00:00:00,4290,0,,8,
2017-05-20 00:00:00,2160,0,,8,
2017-05-21 00:00:00,244,0,,8,
2017-05-22 00:00:00,0,0,,8,
2017-05-23 00:00:00,0,0,,8,
2017-05-24 00:00:00,0,0,,8,
2017-05-25 00:00:00,0,0,,8,
2017-05-26 00:00:00,0,0,,8,         
         */
        private static Series GetData(string id, int hoursBack)
        {
            //var url = "https://idastream.idahopower.com/Data/Export_Data/?dataset=18942&date=2017-05-12&endDate=2017-05-26&type=csv";

            var t2 = DateTime.Now.AddDays(1);
            var t1 = DateTime.Now.AddHours(-hoursBack);

            var url = "https://idastream.idahopower.com/Data/Export_Data/?dataset="
                +id+"&date="+t1.ToString("yyyy-MM-dd")
                +"&endDate="+t2.ToString("yyyy-MM-dd")+"&type=csv";

            var fn = DownloadAndUnzip(url);

            TextSeries s = new TextSeries(fn);
            s.Read();
            s.Trim(t1, t2);
            return s;
        }
        private static string DownloadAndUnzip(string url)
        {
            var zip = FileUtility.GetTempFileName(".zip");
            Console.WriteLine("Downloading: " + url);
            Web.GetFile(url, zip);


            var csv = FileUtility.GetTempFileName(".csv");
            Console.WriteLine("Unzipping to-> " + csv);
            ZipFileUtility.UnzipFile(zip, csv);
            return csv;
        }


    }
}
