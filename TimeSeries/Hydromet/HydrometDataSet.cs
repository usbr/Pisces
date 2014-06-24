using System.Configuration;
using Reclamation.Core;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace Reclamation.TimeSeries.Hydromet {
    
    
    public partial class HydrometDataSet {


       



        //public static shiftsDataTable GetDailyShiftsTable(DateTime since)
        //{
        //    string sql = "select * from shifts where cbtt = '" + cbtt + "' order by date_measured DESC, date_entered DESC LIMIT 20";
        //    if (cbtt == "")
        //    {
        //        sql = "select * from shifts order by date_measured DESC, date_entered DESC";
        //    }

        //    shiftsDataTable tbl = new shiftsDataTable();

        //    GetDatabaseServer("hydromet").FillTable(tbl, sql);
        //    return tbl;

        //}

        public static shiftsDataTable GetShiftsTable(string cbtt="")
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

            shiftsDataTable tbl = new shiftsDataTable();

            PostgreSQL.GetPostgresServer("hydromet").FillTable(tbl, sql);
            return tbl;

        }

        public static shiftsDataTable GetAllShifts()
        {
            string sql;

            sql = "select * from shifts order by date_entered DESC, date_measured DESC";

            shiftsDataTable tbl = new shiftsDataTable();

            PostgreSQL.GetPostgresServer("hydromet").FillTable(tbl, sql);
            return tbl;

        }

        public static shiftsDataTable GetDailyShifts(DateTime PreviousDay)
        {

            string sql = "select * from shifts where date_entered >'" + PreviousDay + "' order by date_entered";

            shiftsDataTable tbl = new shiftsDataTable();

            PostgreSQL.GetPostgresServer("hydromet").FillTable(tbl, sql);
            return tbl;

        }

        public static void insertshift(string cbtt, string pcode, DateTime date_measured,
            double? discharge, double? gage_height, double shift, string comments, DateTime date_entered)
        {
            string sql = "select * from shifts where 2=1";
           
            
            shiftsDataTable tbl = new shiftsDataTable();

            PostgreSQL.GetPostgresServer("hydromet").FillTable(tbl, sql);
            var row = tbl.NewshiftsRow();

            //due to our Sequence in the Dbase the id is 0 here but will add 1 to last id in dbase
            row.id = 0;
            row.cbtt = cbtt.ToUpper();
            row.pcode = pcode.ToUpper();
            row.date_measured = date_measured;
            if (discharge.HasValue)
            {
                row["discharge"] = discharge;
            }
            
            if (gage_height.HasValue)
            {
                row["stage"] = gage_height;
            }
            row.shift = shift;
            row.comments = comments;
            row.username = WindowsUtility.GetShortUserName().ToLower();
            row.date_entered = date_entered;

            tbl.AddshiftsRow(row);
            PostgreSQL.GetPostgresServer("hydromet").SaveTable(tbl);
        }


        /// <summary>
        /// using the daily_calculation table preload hydrmet data referenced in the equations
        /// </summary>
        /// <param name="groupNames"></param>
        public static void PreloadDailyHydrometData(string[] groupNames,DateTime t1,DateTime t2)
        {
            
            string sql = "select * from daily_calculation where group_name in ( '"
                          + System.String.Join("','",groupNames)+"' )";

            daily_calculationDataTable tbl = new daily_calculationDataTable();

            PostgreSQL.GetPostgresServer("hydromet").FillTable(tbl, sql);

            var cbttPcodeList = new List<string>();

            // check for calculations (where cbtt_pcode) format is used.
            // example  JCK_AF 
            foreach (var row in tbl)
            {
                string pattern = "(?<cbttPcode>[A-Z]{2,8}_[A-Z]{2,8})";
                var mc = Regex.Matches(row.equation, pattern);
                if (mc.Count > 0)
                {
                    foreach (Match item in mc)
                    {
                        cbttPcodeList.Add(item.Groups["cbttPcode"].Value.Replace("_", " "));
                    }
                }
            }

            cbttPcodeList = cbttPcodeList.Distinct().ToList();

            var cache = new HydrometDataCache();

            cache.Add(cbttPcodeList.ToArray(),t1,t2, HydrometHost.PN, TimeInterval.Daily);

            HydrometDailySeries.Cache = cache;

        }
    }
}
