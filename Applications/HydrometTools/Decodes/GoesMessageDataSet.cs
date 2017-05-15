using Reclamation.Core;
using System;
namespace HydrometTools.Decodes
{
    
    
    public partial class GoesMessageDataSet {


        /// <summary>
        /// Gets messages for single id from USGS EDDN web stite
        /// </summary>
        /// <param name="nessid"></param>
        /// <returns></returns>
        public static MessageDataTable GetEDDNMessages(string nessid, string hoursBack="24")
        {
            var rval = new MessageDataTable();
            string cgi = "http://lrgseddn3.cr.usgs.gov/cgi-bin/fieldtest.pl";
            string postData = "DCPID=" + nessid + "&SINCE=" + hoursBack;
            var lines = Reclamation.Core.Web.GetPage(cgi, postData);

            for (int i = 0; i < lines.Length; i++)
            {
                string data = lines[i];
                if (data.IndexOf("<p>" + nessid) < 0)
                    continue;

                data = data.Replace("<p>",""); // trim <p> tag
                data = data.Replace("</p>","");
                
                var row = ParseMessage(data);
                if (row != null)
                {
                    var newRow = rval.NewMessageRow();
                    newRow.ItemArray = row.ItemArray;
                    rval.AddMessageRow(newRow);
                }
            }

            return rval;
        }

        public static GoesMessageDataSet.MessageRow ParseMessage(string data)
        {
            //parse data
            /*            //"<p>
 01234567yydddhhmmssXPW
 3474865415005104922G44+0NN172WXW00030bB1D@Fi@Fi@Fi@Fi@Fi@Fi@Fi@FiH </p>"
 123456789012345678901234567890123456789012345678901234567890
         10        20        30     
             */

            var tbl = new GoesMessageDataSet.MessageDataTable();
            var rval = tbl.NewMessageRow();
            try
            {
                if (data.Length < 37)
                    return null;

                rval.nessid = data.Substring(0, 8);

                var date = data.Substring(8, 11);
                int yr = Convert.ToInt32(date.Substring(0, 2)) + 2000;
                var jul = data.Substring(10, 3);
                var t = JulianToUTCDate(yr, Convert.ToInt32(jul));
                var hh = data.Substring(13, 2);
                t = t.AddHours(Convert.ToInt32(hh));
                var mm = data.Substring(15, 2);
                t = t.AddMinutes(Convert.ToInt32(mm));
                var ss = data.Substring(17, 2);
                t = t.AddSeconds(Convert.ToInt32(ss));
                rval.GMT = t;
                rval.failure = data.Substring(19, 1);
                rval.power = data.Substring(20, 2);
                rval.freq = data.Substring(22, 2);
                rval.mod = data.Substring(24, 1);
                rval.dataquality = data.Substring(25, 1);
                rval.channel = data.Substring(26, 3).TrimStart(new char[]{'0'});
                rval.satellite = data.Substring(29, 1);
                rval.drgs = data.Substring(30, 2);
                rval.length = Convert.ToInt32(data.Substring(32, 5));
                rval.message = data.Substring(37);

                Logger.WriteLine("gmt.Kind"+t.Kind);
                rval.MST = ConvertToMountainTime(t);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            return rval;
        }
        internal static DateTime ConvertToMountainTime(DateTime utc)
        {
            DateTime mountain;
            if (LinuxUtility.IsLinux())
            {
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("US/Mountain");
                Console.WriteLine("try ConvertTimeFromUtc");
                mountain = TimeZoneInfo.ConvertTimeFromUtc(utc, tzi);
                //Console.WriteLine("try other way..");
                //mountain = TimeZoneInfo.ConvertTimeBySystemTimeZoneId
                // (utc, "US/Mountain");
            }
            else
            {
                mountain = TimeZoneInfo.ConvertTimeBySystemTimeZoneId
                    (utc, "Mountain Standard Time");
            }
            //Console.WriteLine("{0} (UTC) = {1} Mountain time", utc, mountain);
            return mountain;
        }

        static DateTime JulianToUTCDate(int year, int julianDay)
        {
            DateTime t1 = new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime t2 = t1.AddDays(julianDay - 1);
            return t2;
        }

        partial class MessageDataTable
        {
        }
    }
}
