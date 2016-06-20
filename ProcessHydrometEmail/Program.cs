using ImapX;
using ImapX.Enums;
using Reclamation.Core;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace ProcessHydrometEmail
{

    class Program
    {
        //http://imapx.codeplex.com/documentation

        static void Main(string[] args)
        {

           // ApplicationTrustPolicy.TrustAll();

            string credientials = ConfigurationManager.AppSettings["credentials"];
            string imap_server = ConfigurationManager.AppSettings["imap_server"];
            string logfilename = ConfigurationManager.AppSettings["log"];
            string output_dir = ConfigurationManager.AppSettings["output_dir"];
            string email_folder = ConfigurationManager.AppSettings["email_folder"];


            var client = new ImapClient(imap_server, true,false);

            if (logfilename != null)
            {
                Logger.EnableLogger();
                Logger.WriteLine("logging to '" + logfilename + "'");
                Debug.Listeners.Add(new TextWriterTraceListener(logfilename));
                Debug.AutoFlush = true;
                client.IsDebug = true;
            }
            
            var files = Directory.GetFiles(credientials, "*.*");
            var user =  Path.GetFileName(files[0]);
            var pass = File.ReadAllLines(files[0])[0];
            Logger.WriteLine("about to connect:");
            if (client.Connect())
            {

                if (client.Login(user,pass))
                {
                    // login successful
                    Console.WriteLine("ok"); 
                    Logger.WriteLine("listing labels...");
                    foreach (var item in client.Folders)
                    {
                        Logger.WriteLine(item.Name);
                    }

                    var folder = client.Folders[email_folder];
                   folder.Messages.Download("UNSEEN", MessageFetchMode.Basic | MessageFetchMode.GMailExtendedData, 72);
                   var s = GetSerieData(folder);
                    var cbtt = "mpci";
                    var pcode="qp2";

                    string fn = "instant_" + cbtt + "_" + pcode + "_" + DateTime.Now.ToString("MMMdyyyyHHmmssfff") + ".txt";
                    fn = Path.Combine(output_dir, fn);
                   HydrometInstantSeries.WriteToHydrometFile(s, cbtt, pcode, "hydromet", fn);
                }
            }
            else
            {
                Logger.WriteLine("connection not successful");
            }

        }

        private static Series GetSerieData(Folder folder)
        {
            var s = new Series();
            s.TimeInterval = TimeInterval.Daily;
            foreach (var msg in folder.Messages)
            {
                var txt = msg.Body.Text;
                var exp = @"Pump Station Average Flow:(\s*\d{1,10}(\.){0,1}\d{0,3})\s*cfs";
                Regex re = new Regex(exp);
                var m = Regex.Match(txt, exp);
                if (m.Success)
                {
                    double d = Convert.ToDouble(m.Groups[1].Value);
                    var t = Round(msg.Date.Value);

                    if (s.IndexOf(t) < 0)
                    {
                        s.Add(t, d);
                        //msg.Flags.Add(ImapX.Flags.MessageFlags.Seen);
                        msg.Flags.Add(ImapX.Flags.MessageFlags.Deleted);
                        Console.WriteLine(t.ToString() + " " + d.ToString("F2"));
                    }
                }
            }
            return s;
        }
        /// <summary>
        /// http://stackoverflow.com/questions/2499479/how-to-round-off-hours-based-on-minuteshours0-if-min30-hours1-otherwise
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime Round(DateTime dateTime)
        {
            var updated = dateTime.AddMinutes(30);
            return new DateTime(updated.Year, updated.Month, updated.Day,
                                 updated.Hour, 0, 0, dateTime.Kind);
        }
    }
}
