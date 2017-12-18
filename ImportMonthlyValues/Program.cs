using Reclamation.Core;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ImportMonthlyValues
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 3 && args.Length != 4)
            {
                Console.WriteLine("Usage: ImportMonthlyValues server user pass [siteid]");
                Console.WriteLine(" server = hostname ");
                Console.WriteLine(" user = username");
                Console.WriteLine(" pass = passwordfile");
                return;
            }

            var host = args[0];
            string user = args[1];
            string pass = File.ReadAllLines(args[2])[0];
            string cbtt = "";
            if (args.Length == 4)
                cbtt = args[3];

            var svr = PostgreSQL.GetPostgresServer("timeseries", host, user, pass);
            //UpdateVMS_daily_por(svr);
            TimeSeriesDatabase db = new TimeSeriesDatabase(svr);
            Console.WriteLine(db.Server.ConnectionString);

            var sitecatalog = db.GetSiteCatalog();
            // cbtt, pcode, years, descr, units
            DataTable mpoll = new DataTable("mpoll");
            if (File.Exists(@"c:\temp\mpoll.xml"))
                mpoll.ReadXml(@"c:\temp\mpoll.xml");
            else
            {
                mpoll = HydrometInfoUtility.MonthlyInventory;
                mpoll.WriteXml(@"C:\temp\mpoll.xml", System.Data.XmlWriteMode.WriteSchema);
            }

            for (int i = 0; i < mpoll.Rows.Count; i++)
            {
                var r = mpoll.Rows[i];
                var siteid = r["cbtt"].ToString().ToLower();
                var pcode = r["pcode"].ToString().ToLower();
                var years = r["years"].ToString();

                if (cbtt != "" && cbtt.ToLower() != siteid.ToLower())
                    continue;

                HydrometMonthlySeries m = new HydrometMonthlySeries(siteid, pcode, HydrometHost.PN);
                HydrometMonthlySeries.ConvertToAcreFeet = false;
                // does site id exist in sitecatalog?
                if( KeepThisSeries(sitecatalog, siteid,pcode,years))
                {
                    
                    m.Read();
                    m.RemoveMissing();
                    Console.WriteLine(siteid + "_" + pcode+" ["+m.Count+"]");
                    if (m.Count == 0)
                        continue;

                    var folder = db.GetOrCreateFolder("hydromet",  siteid, "monthly");
                    var tn = "monthly_" + siteid + "_" + pcode;
                    var s = db.GetSeriesFromTableName(tn);
                    if( s == null)
                    { // need to create series.
                        s = new Series("", TimeInterval.Monthly);
                        s.Name = siteid + "_" + pcode;
                        s.Table.TableName = tn;
                        s.Parameter = pcode;
                        s.SiteID = siteid;
                        s.TimeInterval = TimeInterval.Monthly;
                        db.AddSeries(s, folder);
                    }

                    
                    s.Table = m.Table;
                    db.SaveTimeSeriesTable(s.ID, s, DatabaseSaveOptions.DeleteAllExisting);
                    
                }

            }


        }

        private static bool KeepThisSeries(TimeSeriesDatabaseDataSet.sitecatalogDataTable sitecatalog, 
            string siteid, string pcode, string years)
        {
            HydrometMonthlySeries m = new HydrometMonthlySeries(siteid, pcode, HydrometHost.PN);

            var goodYears = Regex.IsMatch(years, "20[0-9]{2}");

            var rows = sitecatalog.Select("siteid='" + siteid + "'");

            if( rows.Length == 0 && m.Name != "")
            {
                //Console.WriteLine("siteid could be added to catalog:'"+siteid+"'  "+m.Name);
            }

            return rows.Length == 1 && goodYears;

        }
    }
}
