using Reclamation.TimeSeries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PiscesWebServices.CGI
{
    /// <summary>
    /// comma separated formatter.
    /// </summary>
     class ShefAFormatter : Formatter
    {
        public static DataTable CustomNames { get; set; }

        public ShefAFormatter(TimeInterval interval, bool printFlags)
             : base(interval, printFlags)
         {

         }


        public override void PrintDataTable(SeriesList list, DataTable table)
        {
            var sortedRows = table.Select("", "tablename");
            for (int i = 0; i < sortedRows.Length; i++)
            {
                var r = sortedRows[i];
                var tn = new TimeSeriesName(r["tablename"].ToString());

                if (r["value"] == DBNull.Value)
                    continue;

                Point p = new Point((DateTime)r["datetime"], (double)r["value"], r["flag"].ToString());
                
                if( p.IsMissing || p.FlaggedBad)
                {
                    continue;
                }

                var siteid = tn.siteid;
                var pcode = tn.pcode;
                var timeZone = "M";

                if( CustomNames != null)
                {
                    var rows = CustomNames.Select("tablename = '" + tn.GetTableName() + "'");
                    if( rows.Length >0)
                    {
                        r = rows[0];
                        timeZone = r["timezone"].ToString();
                        siteid = r["siteid"].ToString();
                        pcode = r["parameter"].ToString();
                    }
                }

                WriteLine(".A " + siteid + " "
                    + p.DateTime.ToString("yyMMdd") + " "
                    + timeZone + " "
                    + "DH" + p.DateTime.ToString("HHmm") + "/" + pcode + " " + p.Value.ToString("F2"));

            }
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

             var rval = "";
             var t = Convert.ToDateTime(o);
             if (Interval == TimeInterval.Irregular || Interval == TimeInterval.Hourly)
                 rval = t.ToString("yyyy-MM-dd HH:mm");
             else
                 rval = t.ToString("yyyy-MM-dd");
             return rval;
         }
        public override void WriteSeriesHeader(SeriesList list)
        {
            WriteLine("<PRE>");

        }

        public override void WriteSeriesTrailer()
        {
            HydrometWebUtility.PrintHydrometTrailer();
        }

    }
}
