using System;
using Reclamation.Core;

namespace HydrometNotifications
{
    class Program
    {

        [STAThread]
        static int Main(string[] args)
        {
            Logger.EnableLogger();
            if (args.Length <1 || args.Length > 2)
            {
                Console.WriteLine("Usage: HydrometNotifications.exe alarm_group [\"m/d/yyyy hh:mm\"]");
                Console.WriteLine("Where: alarm_group argument is required");
                Console.WriteLine("[m/d/yyyy] is an optional date argument the default is now");
                Console.WriteLine("");
                return -1;
            }

            Performance perf = new Performance();

            var alarms = new AlarmGroup(args[0]);

            DateTime t = DateTime.Now;

            if (args.Length == 2)
            {
                t = DateTime.Parse(args[1]);
            }
            else
            if (alarms.Interval.ToLower() == "d")
            {
                t = DateTime.Now.Date.AddDays(-1); // hydromet daily
            }


            alarms.ProcessGroup(t);
            perf.Report();
            PostgreSQL.ClearAllPools();
            return 0;
        }

      
    }
}
