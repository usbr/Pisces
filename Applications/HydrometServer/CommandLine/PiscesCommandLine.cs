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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="interval"></param>
        public PiscesCommandLine(TimeSeriesDatabase db,TimeInterval interval)
        {
            m_db = db;
            m_interval = interval;
        }

        /// <summary>
        /// The command Prompt for Pisces
        /// </summary>
        public void PiscesPrompt()
        {

            var input = new CommandLineInput(m_interval);
            do
            {
                Console.Write(">");
                var s = Console.ReadLine();
                if (s.Trim() == "")
                    continue;
                input.Read(s);

                if (input.Parameters.Length == 0 && input.SiteList.Length ==1) // get all parameters in database
                    input.Parameters = GetAllParametersForSiteID(input.SiteList[0], m_interval, input.Command== Command.GetQ);

                if (input.Command == Command.Exit)
                    break;

                if (!input.Valid)
                {
                    Console.WriteLine("Error: Invalid Input");
                    continue;
                }

               

                if (input.Command == Command.Help)
                {
                    Help();
                }

                if (input.Command == Command.Get || input.Command == Command.GetQ)
                {
                    if (input.SiteList.Length == 0)
                    {
                        Console.WriteLine("site is required");
                        continue;
                    }
                    Print(input,m_interval);

                }

                if( input.Command == Command.Date)
                {

                    Console.WriteLine(input.T1.ToString("yyyy-MM-dd"));
                }
                //Console.WriteLine("cmd = " + input.Command);
                //Console.WriteLine("sites = " + String.Join(",", input.SiteList));
                //Console.WriteLine("parameters  = " + String.Join(",", input.Parameters));

            } while (true);
        }


       

        /// <summary>
        /// Gets all parameters for a site ID
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="m_interval"></param>
        /// <returns></returns>
        private string[] GetAllParametersForSiteID(string siteId, TimeInterval m_interval, bool quality=false)
        {
            string filter = "timeinterval = '" + m_interval.ToString() + "' and siteid = '"+siteId+"'";
            var sc = m_db.GetSeriesCatalog(filter , "", "order by parameter");

            
            var rval = new List<string>();
            foreach (var item in sc)
            {
                if (quality)
                {
                    if (TimeSeriesDatabase.IsQuality(item.Parameter))
                        rval.Add(item.Parameter);
                }
                else
                    if (!TimeSeriesDatabase.IsQuality(item.Parameter))
                    {
                        rval.Add(item.Parameter);
                    }
            }
            return rval.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param> command line input
        /// <param name="interval"></param> time interval 
        private void Print(CommandLineInput input, TimeInterval interval)
        {
            var list = CreateSeriesList(input, interval);
            //SeriesListDataTable sTable = new SeriesListDataTable(list, interval);

            //int counter = 0;
            list.Read(input.T1, input.T2);// example for read input

            if (interval == TimeInterval.Daily)
            {
                PrintDaily(list);
            }
            else
            {
                PrintInstant(list);
             
            }
        }

        /// <summary>
        /// Print the daily data from the Series List
        /// </summary>
        /// <param name="list"></param> Series list
        private static void PrintDaily(SeriesList list)
        {
            var tbl = list.ToDataTable(false);
            if (tbl.Rows.Count == 0)
                return;

            var t1 = list.MinDateTime;
            var t2 = list.MaxDateTime;

            var title = "\nStation   Parameter     ";
            var title2 = "========= ==========    ========= ========= ========= ========= =========";
            DateTime t = t1;
            while(t <= t2)
            {
                title += t.ToString("ddd MMMdd");
                title += " ";
                t = t.AddDays(1);
            }
            Console.WriteLine(title);
            Console.WriteLine(title2);

            


            foreach (var item in list)
            {
                string x = item.SiteID.PadRight(10) + " " + item.Parameter.PadRight(11) + " ";

                t = t1;
                while (t <= t2)
                {
                    var idx = item.IndexOf(t);
                    if (idx < 0)
                        x += "".PadLeft(10);
                    else
                        x += item[t].Value.ToString("F2").PadLeft(10);
                    t = t.AddDays(1);
                }
                Console.WriteLine(x);

            }
        }
       
        /// <summary>
        ///  Print the 15 minute data from the Series List
        /// </summary>
        /// <param name="list"></param>
        private static void PrintInstant(SeriesList list)
        {
            var table = list.ToDataTable(false);
           
            TablePrinter.Print(table, 4);
        }

        /// <summary>
        /// Create a Series List 
        /// </summary>
        /// <param name="input"></param> input from the command line
        /// <param name="interval"></param> time interval 
        /// <returns></returns>
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

        /// <summary>
        /// The Help function for the Pisces Command Line 
        /// </summary>
        private static void Help()
        {
            Console.WriteLine("Pisces command line access");
            Console.WriteLine("Examples:");
            Console.WriteLine("Get/ob jck   # gets Jackson Lake (jck) air temperature (ob)");
            Console.WriteLine("g jck        # get all parameters for site JCK");
            Console.WriteLine("gq jck        # get all quality parameters for site JCK");
            Console.WriteLine("interval=daily|instant|monthly # set time interval default is instant");
            Console.WriteLine("Help         # show this screen");

        }

    }
}
