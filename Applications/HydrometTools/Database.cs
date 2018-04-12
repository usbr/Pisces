using Reclamation.Core;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries.Parser;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace HydrometTools
{
    class Database
    {

        static TimeSeriesDatabase s_db;

        internal static TimeSeriesDatabase DB()
        {
            HydrometHost h = HydrometInfoUtility.HydrometServerFromPreferences();

            var dbname = UserPreference.Lookup("TimeSeriesDatabaseName");
            if( File.Exists(dbname))
            { // local sqlite
                Logger.WriteLine("reading: " + dbname);
                var x = TryLocalDatabase(dbname,h);
                if( x != null)
                {
                    return x;
                }
                
            }

            if (s_db == null || s_db.Server.Name != dbname
                || HydrometHostDiffers(h) )
            {

                if (IsPasswordBlank())
                    return null;

                BasicDBServer svr = GetServer(dbname);
                if (svr == null)
                    return null;
                s_db = new TimeSeriesDatabase(svr, LookupOption.TableName, false);
                s_db.Parser.VariableResolver = new HydrometVariableResolver(h);
                s_db.Parser.VariableResolver.LookupOption = LookupOption.TableName;
            }

            return s_db;
        }

        internal static bool IsPasswordBlank()
        {
            var pw = GetPassword();
            return pw == "";
        }

        private static string GetPassword()
        {
            var pw = UserPreference.Lookup("timeseries_database_password", "");
            if (pw != "")
            {
                pw = StringCipher.Decrypt(pw, "");
            }
            return pw;
        }

        public static PostgreSQL GetServer(string dbname)
        {
            var pw = GetPassword();
            if (pw == "")
            {
                throw new Exception("Error: the password is blank. Please set this in the settings tab");
            }
            HydrometHost h = HydrometInfoUtility.HydrometServerFromPreferences();
            var server = ConfigurationManager.AppSettings["PostgresServer"];
            if( h == HydrometHost.YakimaLinux)
                server = ConfigurationManager.AppSettings["YakimaPostgresServer"];

            BasicDBServer svr = PostgreSQL.GetPostgresServer(dbname,server, password: pw);
            return svr as PostgreSQL;
        }

        private static TimeSeriesDatabase TryLocalDatabase(string dbname,HydrometHost h)
        {
           var fn = Path.Combine(dbname);

            if( File.Exists(fn))
            {
                SQLiteServer svr = new SQLiteServer(fn);
                s_db = new TimeSeriesDatabase(svr, LookupOption.TableName, false);
                s_db.Parser.VariableResolver = new HydrometVariableResolver(h);
                s_db.Parser.VariableResolver.LookupOption = LookupOption.TableName;
                Console.WriteLine("using databas: "+fn);
                return s_db;
            }
            return null;
        }

        private static bool HydrometHostDiffers( HydrometHost h)
        {
            var vr = s_db.Parser.VariableResolver as HydrometVariableResolver;
            return (h != vr.Server);
        }

        static TimeSeriesDatabaseDataSet.sitecatalogDataTable s_sites;
        internal static bool IsAgrimetSite(string cbtt)
        {
            if( s_sites == null)
            s_sites = s_db.GetSiteCatalog("type = 'agrimet'");
            return s_sites.Select("siteid = '" + cbtt + "'").Length > 0;
        }


        /// <summary>
        /// Import text files (into a TimeSeriesDatabase) that 
        /// are formatted for the DMS3 VMS system.
        /// </summary>
        /// <param name="editsFileName"></param>
        internal static void ImportVMSTextFile(string editsFileName, bool computeDependencies)
        {
            
         FileImporter importer = new FileImporter(DB());
         importer.ImportFile(editsFileName, computeDependencies, computeDependencies);

        }

        // static PostgreSQL Server(string databaseName = "hydromet")
        //{
        //    PostgreSQL svr = PostgreSQL.GetPostgresServer(databaseName) as PostgreSQL;
        //    return svr;
        //}

        internal static DataTable YakimaStatusReports()
         {
             var svr = GetServer("hydromet");
             var sql = "select report_date, modified from yakima_status "
                + " order by report_date";

             DataTable tbl = svr.Table("yakima_status", sql);
             return tbl;
         }

        internal static void UpdateYakimaStatusReport(DateTime t, string report)
        {
            var svr = GetServer("hydromet");
            svr.SetAllValuesInCommandBuilder = true;
            string fmt = TimeSeriesDatabase.dateTimeFormat;

            var sql = "select * from yakima_status where report_date="
                +svr.PortableDateString(t,fmt) ;

            DataTable tbl = svr.Table("yakima_status", sql);
            DataRow r = null;
            if( tbl.Rows.Count == 0)
            {
                r = tbl.NewRow();
                r["report_date"] = t;
                tbl.Rows.Add(r);
            }
            else
            {
                r = tbl.Rows[0];
            }
            
            r["modified"] = DateTime.Now;
            r["report"] = report.ToString();//.Replace('\r','\n');

            svr.SaveTable(tbl);
        }

        internal static string GetYakimaStatusReport(DateTime t)
        {
            var svr = GetServer("hydromet");
            svr.SetAllValuesInCommandBuilder = true;
            string fmt = TimeSeriesDatabase.dateTimeFormat;

            var sql = "select * from yakima_status where report_date="
                + svr.PortableDateString(t, fmt) + " order by report_date";

            DataTable tbl = svr.Table("yakima_status", sql);

            if (tbl.Rows.Count == 0)
                return "";

            var rval = tbl.Rows[0]["report"].ToString();
            return rval;
        }

        public static void InsertShift(string cbtt, string pcode, System.DateTime date_measured, double? discharge, double? gage_height, double shift, string comments, System.DateTime date_entered)
        {
            string sql = "select * from shifts where 2=1";
            var svr = GetServer("hydromet");
            DataTable shiftsDataTable = svr.Table("shifts",sql);

            var shiftsRow = shiftsDataTable.NewRow();
            shiftsRow["id"] = 0;
            shiftsRow["cbtt"] = cbtt.ToUpper();
            shiftsRow["pcode"] = pcode.ToUpper();
            shiftsRow["date_measured"] = date_measured;
            if (discharge.HasValue)
            {
                shiftsRow["discharge"] = discharge;
            }
            if (gage_height.HasValue)
            {
                shiftsRow["stage"] = gage_height;
            }
            shiftsRow["shift"] = shift;
            shiftsRow["comments"] = comments;
            shiftsRow["username"] = Environment.UserName.ToLower();
            shiftsRow["date_entered"] = date_entered;
            shiftsDataTable.Rows.Add(shiftsRow);
            svr.SaveTable(shiftsDataTable);
        }


        public static DataTable GetShiftsTable(string cbtt = "")
        {
            string sql;
            if (cbtt == "ALL")
            {
                sql = "select * from shifts order by date_entered DESC, date_measured DESC";
            }
            else
            {
                sql = "select * from shifts where cbtt = '" + cbtt + "' order by date_entered DESC, date_measured DESC LIMIT 20";
            }
            return GetServer("hydromet").Table("shifts", sql);
        }

        public static DataTable GetAllShifts()
        {
            string sql = "select * from shifts order by date_entered DESC, date_measured DESC";
            return GetServer("hydromet").Table("shifts", sql);
        }

        public static DataTable GetDailyShifts(System.DateTime PreviousDay)
        {
            string sql = "select * from shifts where date_entered >'" + PreviousDay + "' order by date_entered";
            return GetServer("hydromet").Table("shifts", sql);
        }

    }
}
