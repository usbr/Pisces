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
        TimeInterval m_interval;
        public PiscesCommandLine(TimeSeriesDatabase db,TimeInterval interval)
        {
            m_db = db;
            m_interval = interval;
        }

        public void PiscesPrompt()
        {

            var input = new CommandLineInput(m_interval);
            do
            {
                Console.Write("pisces>");
                var s = Console.ReadLine();
                if (s.Trim() == "")
                    continue;
                input.Read(s);

                if (input.Parameters.Length == 0 && input.SiteList.Length ==1) // get all parameters in database
                    input.Parameters = GetAllParametersForSiteID(input.SiteList[0],m_interval);


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
                    Print(input,m_interval);

                }
                //Console.WriteLine("cmd = " + input.Command);
                //Console.WriteLine("sites = " + String.Join(",", input.SiteList));
                //Console.WriteLine("parameters  = " + String.Join(",", input.Parameters));

            } while (true);
        }

        private string[] GetAllParametersForSiteID(string siteId, TimeInterval m_interval)
        {
            string filter = "timeinterval = '" + m_interval.ToString() + "' and siteid = '"+siteId+"'";
            var sc = m_db.GetSeriesCatalog(filter , "", "order by parameter");

            var rval = new List<string>();
            foreach (var item in sc)
            {
                rval.Add(item.Parameter);
            }
            return rval.ToArray();
        }

        private void Print(CommandLineInput input, TimeInterval interval)
        {
            var list = CreateSeriesList(input, interval);
            //SeriesListDataTable sTable = new SeriesListDataTable(list, interval);

            int counter = 0;
            list.Read(input.T1, input.T2);

            if (interval == TimeInterval.Daily)
            {
                PrintDaily(list);

            }
            else
            {
                foreach (var s in list)
                {
                    if (s.Count <= 0)
                        continue;
                    var pt = s[s.Count - 1];
                    if (counter == 0)// print header
                        Console.Write(s.SiteID + "  " + pt.DateTime.ToString("MMM dd hh:mm  "));
                    Console.Write("# " + pt.Value.ToString("F2"));
                    counter++;
                }
            }

        }

        private static void PrintDaily(SeriesList list)
        {
            var tbl = list.ToDataTable(false);

            var title = "\nStation   Parameter     ";
            var title2 = "========= ==========    ========= ========= ========= ========= =========";
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                DateTime t = Convert.ToDateTime(tbl.Rows[i][0]);
                title += t.ToString("ddd MMMdd");
                title += " ";
            }
            Console.WriteLine(title);
            Console.WriteLine(title2);

            foreach (var item in list)
            {
                string x = item.SiteID.PadRight(10) + " " + item.Parameter.PadRight(11) + " ";
                foreach (var pt in item)
                {
                    x += pt.Value.ToString("F2").PadLeft(10);
                }
                Console.WriteLine(x);
            }
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
