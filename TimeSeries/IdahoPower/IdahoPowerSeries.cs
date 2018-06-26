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

        public IdahoPowerSeries(string stationID, TimeInterval interval)
        {
            this.TimeInterval = interval;
            this.SiteID = stationID;
        }


        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            Clear();
            var s = ReadFromIdahoPower(SiteID, t1, t2);
            this.Add(s);
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
        private static Series ReadFromIdahoPower(string id, DateTime t1, DateTime t2)
        {
            //var url = "https://idastream.idahopower.com/Data/Export_Data/?dataset=18942&date=2017-05-12&endDate=2017-05-26&exporttype=csv&type=csv";


            var url = "https://idastream.idahopower.com/Data/Export_Data/?dataset="
                + id + "&date=" + t1.Date.ToString("yyyy-MM-dd")
                + "&endDate=" + t2.AddDays(1).ToString("yyyy-MM-dd") + "&exporttype=csv&type=csv";

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
