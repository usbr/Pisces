using Reclamation.TimeSeries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Reclamation.TimeSeries.Hydromet;

namespace PiscesWebServices.CGI
{
    /// <summary>
    /// comma separated formatter, customized for real time data
    /// including daily 30 year avearge.
    /// </summary>
     class RealTimeFormatter : Formatter
    {

        NameValueCollection m_collection;
        TimeSeriesDatabase m_db;
        string dailySiteID = "";
        string dailyPcode = "";
         public RealTimeFormatter(TimeInterval interval, bool printFlags, 
             NameValueCollection collection, TimeSeriesDatabase db)
             : base(interval, printFlags)
         {
            ContentType = "Content-type: text/csv\n\n";
            m_collection = collection;
            m_db = db;

            if( collection.AllKeys.Contains("daily"))
            {
                var d = collection["daily"];
                var tokens = d.Split(' ');
                
                if( tokens.Length==2)
                {
                    dailySiteID = tokens[0];
                    dailyPcode = tokens[1];
                }

                if (dailyPcode == "" || Regex.IsMatch(dailyPcode, "[^_a-z0-1A-Z]"))
                {
                    StopWithError("invalid parameter");
                }
                if (dailySiteID == "" || Regex.IsMatch(dailySiteID, "[^_a-z0-1A-Z]"))
                {
                    StopWithError("invalid site");
                }
            }
         }

        private static void StopWithError(string message)
        {
            Console.Write("Content-type: text/html\n\n");
            Help.PrintInstant();
            Console.WriteLine("Error: " + message);
            throw new Exception(message);
        }

        public override void WriteLine(string s)
         {
             Console.WriteLine(s);
         }

         public override void PrintRow(string t0, string[] vals, string[] flags)
         {
             StringBuilder sb = new StringBuilder(vals.Length * 8);
             sb.Append(t0 + ",");
             for (int i = 0; i < vals.Length; i++)
             {
                 sb.Append(vals[i]);
                 if (PrintFlags)
                     sb.Append(flags[i]);
                 if (i != vals.Length - 1)
                     sb.Append(",");
             }
             Console.WriteLine(sb.ToString());

         }
         public override string FormatNumber(object o)
         {
             var rval = "";
             if (o == DBNull.Value || o.ToString() == "")
                 rval = "";//.PadLeft(11);
             else
                 rval = Convert.ToDouble(o).ToString("F02");
             return rval;
         }


         public override string FormatFlag(object o)
         {
             if (o == DBNull.Value)
                 return "";
             else
                 return o.ToString();

         }

        public override string FormatDate(object o)
         {
             var t = Convert.ToDateTime(o);
            var rval = t.ToString("yyyy/MM/dd HH:mm");
             return rval;
         }
        public override void WriteSeriesHeader(SeriesList list)
        {
        }
        public override void WriteSeriesTrailer()
        {
        }

        public override void PrintDataTable(SeriesList list, DataTable table)
        {
            // add a column for 30 year average to the table.
            //Series s = new HydrometDailySeries(dailySiteID, dailyPcode, HydrometHost.PNLinux);
            var s = m_db.GetSeriesFromTableName("daily_" + dailySiteID + "_" + dailyPcode);
            if (s == null)
            {
                Console.WriteLine("Error:  no data found: " + dailySiteID + "/" + dailyPcode);
                return;
            }
            DateTime t1 = new DateTime(1980, 10, 1);
            s.Read(t1, t1.AddYears(30));
            Series s30 = Reclamation.TimeSeries.Math.MultiYearDailyAverage(s, 10);
            s30.RemoveMissing();

            if (table.Columns.Contains("flag"))
                table.Columns.Remove("flag");
            Print(table, s30);

        }

        DateTime t;
        private void Print(DataTable table, Series s30)
        {
            Console.WriteLine("DateTime,Current Reading,Average");
            var avg = "";
            for (int i = 0; i < table.Rows.Count; i++)
            {
                var r = table.Rows[i];
                t = Convert.ToDateTime(r["datetime"]);
                avg = GetAvg(s30, t);
                Console.WriteLine(FormatDate(t) + "," + r[2].ToString() + "," +avg);
            }
            // add some more daily points to show average trend into future

            if( t.Hour < 12)
            {
                t = new DateTime(t.Year, t.Month, t.Day,12,0,0);
                avg = GetAvg(s30, t);
                Console.WriteLine(FormatDate(t) + ",," + avg);
            }
            Console.WriteLine(FormatDate(t.AddHours(12)) + ",,");
            t = t.AddHours(24);
              avg = GetAvg(s30, t);
              Console.WriteLine(FormatDate(t) + ",," + avg);
        }

        private static string GetAvg(Series s30, DateTime t)
        {
            if (t.Hour == 12 && t.Minute == 0)
            {
                int y = s30.MaxDateTime.Year;
                if (t.Month > 9)
                    y = y - 1;
                if (t.Day == 29 && t.Month == 2)
                    return "";

                DateTime tavg = new DateTime(y, t.Month, t.Day);

                var idx = s30.IndexOf(tavg);
                if (idx >= 0)
                {
                    var pt = s30[idx];
                    if (!pt.IsMissing)
                    {
                        return pt.Value.ToString("F2");
                    }
                }
            }

            return "";
        }
    }
}
