using Reclamation.TimeSeries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydrometServer.CommandLine
{

    
    class PiscesCommandLine
    {

        private TimeSeriesDatabase m_db;
        public PiscesCommandLine(TimeSeriesDatabase db)
        {
            m_db = db;
        }

        public void PiscesPrompt()
        {

            var input = new CommandLineInput();
            do
            {
                Console.Write("pisces>");
                var s = Console.ReadLine();
                if (s.Trim() == "")
                    continue;
                input.Read(s);


                if (!input.Valid)
                {
                    Console.WriteLine("Error: Invalid Input");
                    continue;
                }

                if (input.Command == Command.Exit)
                    break;

                if (input.Command == Command.Help)
                {
                    Help();
                }

                if (input.Command == Command.Get)
                {
                    if (input.SiteList.Length == 0)
                    {
                        Console.WriteLine("site is required");
                        continue;
                    }


                }
                Console.WriteLine("cmd = " + input.Command);
                Console.WriteLine("sites = " + String.Join(",", input.SiteList));
                Console.WriteLine("parameters  = " + String.Join(",", input.Parameters));

            } while (true);
        }

        private SeriesList CreateSeriesList(CommandLineInput input, TimeInterval interval)
        {
            
            List<TimeSeriesName> names = new List<TimeSeriesName>();
            foreach (var cbtt in input.SiteList)
            {
                foreach (var pcode in input.Parameters)
                {
                    string sInterval = TimeSeriesName.GetTimeIntervalForTableName(interval);
                    TimeSeriesName tn = new TimeSeriesName(cbtt + "_" + pcode,sInterval);
                    names.Add(tn);
                }
            }



            var tableNames = (from n in names select n.GetTableName()).ToArray();

            var sc = m_db.GetSeriesCatalog("tablename in ('" + String.Join("','", tableNames) + "')");

            SeriesList sList = new SeriesList();
            foreach (var tn in names)
            {
                Series s = new Series();

                s.TimeInterval = interval;
                if (sc.Select("tablename = '" + tn.GetTableName() + "'").Length == 1)
                {
                    s = m_db.GetSeriesFromTableName(tn.GetTableName());
                }
                s.Table.TableName = tn.GetTableName();
                sList.Add(s);
            }
            return sList;
        }

        private static void Help()
        {
            Console.WriteLine("Pisces command line access");
            Console.WriteLine("Examples:");
            Console.WriteLine("Get/ob jck   # gets Jackson Lake (jck) air temperature (ob)");
            Console.WriteLine("g jck        # get all parameters for site JCK");
            Console.WriteLine("interval=daily|instant|monthly # set time interval default is instant");
            Console.WriteLine("Help         # show this screen");

        }

    }
}
