using Reclamation.TimeSeries.Hydromet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydrometServer.CommandLine
{
    class Daily
    {
        static void Main(string[] args)
        {
            DateTime t1 = DateTime.Today.AddDays(-5);
            DateTime t2 = DateTime.Today.AddDays(-1);
            string[] cbtt;
            string[] pcode;
            string CPstr = "";

            do
            {
                Console.Write("ARCHIVES>");
                var r = Console.ReadLine();
                try
                {
                    var cfg = new ArcConfig(r);
                    if (r.ToLower() == "exit" || r.ToLower() == "quit")
                    {
                        return;
                    }

                    else if (cfg.IsDate)
                    {
                        if (r.Contains('+') || r.Contains('-'))
                        {
                            t1 = t1.AddDays(cfg.AddDays);
                            t2 = t2.AddDays(cfg.AddDays);
                        }
                        else
                        {
                            t1 = cfg.StartTime;
                            t2 = t1.AddDays(4);
                        }
                    }

                    else if (cfg.IsNDay)
                    {
                        t1 = t2.AddDays(-cfg.NDays);
                    }

                    else if (cfg.IsYear)
                    {
                        if (t2.Month >= 10)
                        {
                            t1 = new DateTime(cfg.WaterYear - 1, t1.Month, t1.Day);
                            t2 = new DateTime(cfg.WaterYear - 1, t2.Month, t2.Day);
                        }
                        else
                        {
                            t1 = new DateTime(cfg.WaterYear, t1.Month, t1.Day);
                            t2 = new DateTime(cfg.WaterYear, t2.Month, t2.Day);
                        }
                    }

                    else if (cfg.IsDateRange)
                    {
                        //t1
                        Int32 Month = DateTime.ParseExact(cfg.t1Range.Substring(0, 3), "MMM",
                            System.Globalization.CultureInfo.InvariantCulture).Month;
                        Int32 Day = Convert.ToInt32(cfg.t1Range.Substring(3, 2));
                        t1 = new DateTime(t1.Year, Month, Day);
                        //t2
                        Month = DateTime.ParseExact(cfg.t2Range.Substring(0, 3), "MMM",
                            System.Globalization.CultureInfo.InvariantCulture).Month;
                        Day = Convert.ToInt32(cfg.t2Range.Substring(3, 2));
                        t2 = new DateTime(t2.Year, Month, Day);
                    }

                    else if (cfg.IsGet)
                    {
                        //generate string of cbtt and pcode
                        cbtt = cfg.cbtt;
                        for (int i = 0; i < cbtt.Length; i++)
                        {
                            pcode = Reclamation.TimeSeries.Hydromet.HydrometInfoUtility.ArchiveParameters(cbtt[i]);
                            for (int j = 0; j < pcode.Length; j++)
                            {
                                CPstr = CPstr + cbtt[i] + " " + pcode[j] + ",";
                            }
                        }
                        CPstr = Get(CPstr, t1, t2);
                        Console.WriteLine(CPstr);
                    }

                    else if (cfg.IsGetPcode)
                    {
                        cbtt = cfg.cbtt;
                        pcode = cfg.pcode;
                        for (int i = 0; i < cbtt.Length; i++)
                        {
                            for (int j = 0; j < pcode.Length; j++)
                            {
                                CPstr = CPstr + cbtt[i] + " " + pcode[j] + ",";
                            }
                        }
                        CPstr = Get(CPstr, t1, t2);
                        Console.WriteLine(CPstr);
                    }


                }
                catch
                {
                    Console.WriteLine("%E-Archive, unrecognized command");
                }
            }
            while (true);
        }

        private static string format(string input)
        {
            string[] s = input.Split(',');
            string v = s[0] + " ";
            v = v + s[1].PadRight(13, ' ');
            v = v + s[2].PadRight(9, ' ');
            for (int i = 3; i < s.Length; i++)
            {
                string q = s[i].ToString();
                if (q.Length > 9)
                {
                    q = q.Substring(0, 9);
                }
                v = v + q.PadLeft(10, ' ');
            }
            return v;
        }

        private static string Get(string CPstr, DateTime t1, DateTime t2)
        {
            string border = "==== ============ ========= ========= ========= ========= ========= =========";
            string labels = "";

            if ((t2 - t1).TotalDays == 4)
            {
                int wy = t2.Year;
                if (t2.Month >= 10)
                {
                    wy = t2.Year + 1;
                }
                labels = "WY      STATION   PARAMETER " + t1.ToString("ddd MMMdd") + " " + t1.AddDays(1).ToString("ddd MMMdd")
                    + " " + t1.AddDays(2).ToString("ddd MMMdd") + " " + t1.AddDays(3).ToString("ddd MMMdd")
                    + " " + t1.AddDays(4).ToString("ddd MMMdd");
                Console.WriteLine(labels);
                Console.WriteLine(border);
                string[] ID = CPstr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var cache = new HydrometDataCache();
                cache.Add(ID, t1, t2, HydrometHost.PNLinux, Reclamation.TimeSeries.TimeInterval.Irregular);
                HydrometDailySeries.Cache = cache;
                for (int j = 0; j < ID.Length; j++)
                {
                    var c = ID[j].Split(' ')[0];
                    var p = ID[j].Split(' ')[1];
                    HydrometDailySeries ts = new HydrometDailySeries(c, p);
                    ts.Read(t1, t2);
                    string t = wy + "," + c + "," + p + "," +
                        ts[t1].Value + "," + ts[t1.AddDays(1)].Value + "," +
                        ts[t1.AddDays(2)].Value + "," + ts[t1.AddDays(3)].Value + "," +
                        ts[t1.AddDays(4)].Value;
                    Console.WriteLine(format(t));
                }
            }
            return CPstr = "";
        }
    }
}
