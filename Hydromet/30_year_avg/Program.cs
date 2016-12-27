using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.TimeSeries.Hydromet;
using System.IO;
using Reclamation.TimeSeries;
using Reclamation.Core;
using System.Data;
namespace _30_year_avg
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <4 || args.Length > 5)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("30_year_avg creates 30 year average pisces database");
                Console.WriteLine("Usage:  30_year_avg  config.csv group|all  output.db boise|yakima [hmet.txt]");
                Console.WriteLine("config.csv example below:\n");
                Console.WriteLine("group,station,daily_pcode,title,ylabel");
                Console.WriteLine("Boise Payette,plei,qd,\"Payette River near Letha, ID\",Discharge - cfs");
                Console.WriteLine(" Boise Payette,emm,qd,\"Payette River near Emmett, ID\",Discharge - cfs");
                Console.WriteLine("\ngroup is used to filter specific parts of config file.  enter all to disable filtering");
                Console.WriteLine("output.db is the name of a pisces database that will be created.");
                Console.WriteLine("boise|yakima specifiy which hydromet server to read data from");
                Console.WriteLine("hmet.txt is an optional output with hydromet daily format");
                return;
            }

                string fn = args[2];
                if (File.Exists(fn))
                {
                    Console.WriteLine("Deleting existing database ");
                    File.Delete(fn);
                }
                var svr = new SQLiteServer(fn);
                var db = new TimeSeriesDatabase(svr);

                HydrometHost host = HydrometHost.PN;
                if (args[3] == "yakima")
                    host = HydrometHost.Yakima;


                DataTable config = new CsvFile(args[0]);

                 if( args[1] != "all")
                 { // filter out specific group
                     config = DataTableUtility.Select(config, "group = '" + args[1] + "'", "");
                }

                 if (args.Length == 5 && File.Exists(args[4]))
                 {
                     Console.WriteLine("deleting "+args[4]);
                     File.Delete(args[4]);
                 }

                 var prevFolderName = Guid.NewGuid().ToString();

                 PiscesFolder folder = null;

                for (int x = 0; x < config.Rows.Count; x++)
                {
                    var row = config.Rows[x];
                    string folderName = row["group"].ToString();
                    
                    if (prevFolderName != folderName)
                    {
                        prevFolderName = folderName;
                        folder = db.AddFolder(folderName);
                    }
                    string CBTT = row["station"].ToString();
                    string Pcode = row["daily_pcode"].ToString();
                    Console.WriteLine(CBTT+" " +Pcode);
                    Series s = new HydrometDailySeries(CBTT, Pcode,host);
                    // Data ranges collected   
                    var t1 = new DateTime(1980, 10, 1);
                    var t2 = new DateTime(2010, 9, 30);
                    s.Read(t1, t2);
                    var s7100 = LabelAndSave30Year(db, 7100, CBTT, Pcode, folder,host);
                    var s8110 = LabelAndSave30Year(db, 8110, CBTT, Pcode, folder,host);
                    var s6190 = LabelAndSave30Year(db, 6190, CBTT, Pcode, folder,host);

                    //Creates thirty-year average from raw data and adds to database
                    var avg = Reclamation.TimeSeries.Math.MultiYearDailyAverage(s, 10);
                    avg.Name = "avg 1981-2010 " + CBTT + " " + Pcode;
                    avg.Table.TableName = "avg_1981_2010" + CBTT + "" + Pcode;
                    db.AddSeries(avg, folder);
                    avg = Reclamation.TimeSeries.Math.ShiftToYear(avg, 8109);
                    if (args.Length == 5)
                    {
                        HydrometDailySeries.WriteToArcImportFile(avg, CBTT, Pcode, args[4], true);
                    }
                }
        }

        static Series LabelAndSave30Year(TimeSeriesDatabase db1, int year, string cbtt, string pcode, 
            PiscesFolder folder, HydrometHost host)
        {
            Series s = new HydrometDailySeries(cbtt, pcode,host);
            var t1 = new DateTime(year - 1, 10, 1);
            var t2 = new DateTime(year, 9, 30);
            int t3 = (year) - 1;
            int t4 = year;

            s.Read(t1, t2);
            s.Provider = "Series";
            s.Source = "";
            s = Reclamation.TimeSeries.Math.ShiftToYear(s, 2000);
            s.Name = t3 + "-" + t4 + cbtt + " " + pcode;
            s.Table.TableName = t4 + cbtt + "" + pcode;

            if (s.Count > 0 && s.CountMissing() < 3)
            {
                db1.AddSeries(s, folder);
            }
            return s;
        }
    }  
}
