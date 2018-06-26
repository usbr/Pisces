using Reclamation.Core;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries.IdahoPower;
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
            DateTime t1 = DateTime.Now.AddHours(-hoursBack);
            DateTime t2 = DateTime.Now.AddDays(1);
            for (int i = 0; i < csv.Rows.Count; i++)
            {
                try
                {
                    var row = csv.Rows[i];
                    var id = row["id"].ToString();
                    if (id.Trim() == "")
                    {
                        continue;
                    }
                    Console.WriteLine(row["name"]);

                    Series s = new IdahoPowerSeries(id, TimeInterval.Daily);
                    s.Read(t1,t2);
                    SaveToFile(row, s, dir);
                }
                catch(Exception e)
                {
                    Console.WriteLine("Error "+e.Message);
                }
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

    }
}
