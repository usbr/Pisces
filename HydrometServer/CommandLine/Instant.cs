using Reclamation.TimeSeries.Hydromet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HydrometServer.CommandLine
{
    class Instant
    {
        public static void Go()
        {
            var s = "s";
            DateTime t1 = DateTime.Today;
            DateTime t2 = DateTime.Today;
            DateTime today = DateTime.Today;
            do
            {

                Console.Write("DAYFILES>");
                var r = Console.ReadLine();
                try
                {
                    //Exit to the program
                    if (r.ToLower() == "exit" || r.ToLower() == "quit")
                    {
                        return;
                    }
                    //Error Check
                    if (check(r, s) == false)
                    {
                    }
                    else
                    {
                        var cfg = new Config(r, t1, t2);
                        var cfgS = new Config(s, t1, t2);
                        //Check if user is specifying dates
                        if (cfg.IsDate)
                        {
                            t1 = cfg.T1;
                            t2 = cfg.T2;
                        }
                        else if (cfg.IsShow & !cfgS.IsDateRange)
                        {
                            Get(s, t1, t2);
                        }
                        else if (cfg.IsShow && cfgS.IsDateRange)
                        {
                            Get(cfgS.input, today, today);
                        }
                        else
                        {
                            s = r;

                            if (cfg.IsDateRange)
                            {
                                t1 = cfg.T1;
                                t2 = cfg.T2;
                                if (t2 < t1)
                                {
                                    Console.WriteLine("%E-Dayfile, start time must preceed end time");
                                }
                            }
                            //Get function to get the data
                            if (cfg.IsGet || cfg.IsGetAll || cfg.IsGetPcode || cfg.IsShow || cfg.IsDate || cfg.IsDateRange)
                            {
                                Get(s, t1, t2);
                            }
                            else
                            {
                                Console.WriteLine("%E-Dayfile, unrecognized command");
                            }
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("%E-Dayfile, unrecognized command");
                }
            }
            while (true);


        }
        private static string format(string input)
        {
            string v = "|";
            string[] s = input.Split(' ');
            for (int i = 0; i < s.Length; i++)
            {
                string q = s[i].ToString();
                int padding = 10 - q.Length;
                v = v + q.PadLeft(Convert.ToInt32(q.Length) + padding / 2, ' ').PadRight(10, ' ') + "|";
                int c = new Regex("\n").Matches(v).Count;
                if (v.Length / (c + 1) >= 75 && v[v.Length - 1] == '|')
                {
                    v = v + "\n" + "|";
                }
            }
            return (v);

        }

        private static string WriteLines(string str)
        {
            str = format(str);
            Console.Write(str);
            Console.WriteLine("");
            return str;
        }

        private static string LoopCBTT(string cbtt)
        {
            string Id = "";
            string[] pcode = Reclamation.TimeSeries.Hydromet.HydrometInfoUtility.DayfileParameters(cbtt);
            for (int j = 0; j < pcode.Length; j++)
            {
                if (String.IsNullOrEmpty(pcode[j]) == true)
                {
                    return "";
                }
                else
                {
                    Id = Id + cbtt + " " + pcode[j] + ",";
                }
            }
            return Id;
        }

        //try to take the code from above and make it into here for the show cmd
        private static string Get(string s, DateTime t1, DateTime t2)
        {
            string[] cbtt = { "" };
            string[] pcode = { "" };
            string Id = "";
            string value = "";
            string pc = "";
            var cfg = new Config(s, t1, t2);
            if (s.Trim().ToLower()[0] == 'g')
            {
                cbtt = cfg.cbtt;
                for (int i = 0; i < cbtt.Length; i++)
                {
                    Id = LoopCBTT(cbtt[i]);

                    if (String.IsNullOrEmpty(Id) == true)
                    {
                        Console.WriteLine("%W-Dayfile, no data found for get request:" + cbtt[i]);
                    }

                    else
                    {
                        string[] ID = Id.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        //improving the speed of the program
                        var cache = new HydrometDataCache();
                        cache.Add(ID, t1, t2, HydrometHost.PNLinux, Reclamation.TimeSeries.TimeInterval.Irregular);
                        HydrometInstantSeries.Cache = cache;
                        HydrometInstantSeries.KeepFlaggedData = true;

                        if (cfg.IsGetAll)
                        {
                            HydrometInstantSeries ts = new HydrometInstantSeries(ID[0].Split(' ')[0], ID[0].Split(' ')[1]);
                            ts.Read(t1, t2);
                            var count = (ts.MaxDateTime - ts.MinDateTime).TotalMinutes;
                            for (int t = 0; t <= count / 15; t++)
                            {
                                pc = cbtt[i].ToUpper() + " " + t1.AddMinutes(t * 15).ToString("yyMMMdd");
                                for (int j = 0; j < ID.Length; j++)
                                {
                                    var c = ID[j].Split(' ')[0];
                                    var p = ID[j].Split(' ')[1];
                                    HydrometInstantSeries.KeepFlaggedData = true;
                                    ts = new HydrometInstantSeries(c, p);
                                    ts.Read(t1, t2);
                                    pc = pc + " " + p.ToUpper();
                                    if (j == 0)
                                    {
                                        value = " " + ts.MinDateTime.AddMinutes(t * 15).TimeOfDay + " " + ts[ts.MinDateTime.AddMinutes(t * 15)].Value;
                                    }
                                    else
                                    {
                                        value = value + " " + ts[ts.MinDateTime.AddMinutes(t * 15)].Value;
                                    }
                                }
                                if (ts.MinDateTime.AddMinutes(t * 15).TimeOfDay.TotalHours == 0 || ts.MinDateTime.AddMinutes(t * 15).TimeOfDay.TotalHours == 12)
                                {
                                    WriteLines(pc);
                                }
                                WriteLines(value);
                            }
                        }

                        if (cfg.IsGet || (cfg.IsGetAll == false && cfg.IsGetPcode))
                        {
                            if (cfg.IsGetAll == false && cfg.IsGetPcode)
                            {
                                pcode = cfg.pcode;
                                Id = "";
                                for (int j = 0; j < pcode.Length; j++)
                                {
                                    Id = Id + cbtt[i] + " " + pcode[j] + ",";
                                }
                                ID = Id.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            }
                            var count = (t2 - t1).TotalDays;
                            for (int t = 0; t <= count; t++)
                            {
                                pc = cbtt[i].ToUpper() + " " + t1.AddDays(t).ToString("yyMMMdd");
                                for (int j = 0; j < ID.Length; j++)
                                {
                                    var c = ID[j].Split(' ')[0];
                                    var p = ID[j].Split(' ')[1];
                                    HydrometInstantSeries.KeepFlaggedData = true;
                                    HydrometInstantSeries ts = new HydrometInstantSeries(c, p);
                                    ts.Read(t1, t2);
                                    pc = pc + " " + p.ToUpper();
                                    if (j == 0)
                                    {
                                        value = " " + ts.MaxDateTime.AddDays(-count + t).TimeOfDay + " " + ts[ts.MaxDateTime.AddDays(-count + t)].Value;
                                    }
                                    else
                                    {
                                        value = value + " " + ts[ts.MaxDateTime.AddDays(-count + t)].Value;
                                    }
                                }
                                WriteLines(pc);
                                WriteLines(value);
                            }
                        }
                    }
                }
            }
            return s;
        }

        //Final part of code need to catch exceptions
        private static bool check(string r, string s)
        {
            bool istrue = true;
            if (r.ToLower()[0] == 's' && s.ToLower()[0] == 's')
            {
                Console.WriteLine("%E-Dayfile, no station specified");
                istrue = false;
                return istrue;
            }
            else if ((r.ToLower().Trim()[0] == 'd' || r.ToLower().Trim().Contains("date"))
                && r.ToLower().Contains('+') == false && r.ToLower().Contains('-') == false)
            {
                try
                {
                    DateTime.ParseExact(r.ToLower().Trim().Substring(r.Length - 9),
                    "yyyyMMMdd", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    Console.WriteLine("%E-Dayfile, invalid day specified format = yyyyMMMdd");
                    istrue = false;
                }
                return istrue;
            }
            else if (r.ToLower().Trim().Contains("file"))
            {
                string t = r.ToLower().Trim().Substring(r.IndexOf("/file"));
                t = t.Replace(" ", "").Replace("=", "").Replace("/file", "");
                string[] n = t.Split(',');
                try
                {
                    DateTime.ParseExact(n[0],
                        "yyyyMMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    DateTime.ParseExact(n[1],
                        "yyyyMMMdd", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    Console.WriteLine("%E-Dayfile, invalid day specified format = yyyyMMMdd");
                    istrue = false;
                }
                return istrue;
            }
            else
            {
                return istrue;
            }
        }






    }
}
