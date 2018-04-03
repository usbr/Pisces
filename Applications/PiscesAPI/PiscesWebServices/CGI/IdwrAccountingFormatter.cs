using System.Data;
using Reclamation.TimeSeries;
using System;

namespace PiscesWebServices.CGI
{
    internal class IdwrAccountingFormatter : Formatter
    {

        public static DataTable CustomNames { get; set; }

        public IdwrAccountingFormatter(TimeInterval interval,bool printFlags) : base(interval, printFlags)
        {
        }

        /// <summary>
        /// Prints in format used as input to IDWR Planning model.
        /// for example:
        ///13010500R201711 6AF  642871.00
        ///13010500R201711 6FB    6760.77
        ///13010500R201711 7FB    6760.77
        ///13010500R201711 7AF  642871.00
        ///13010500R201711 9FB    6760.77
        ///13010500R20171110FB    6760.78
        /// </summary>
        /// <param name="list"></param>
        /// <param name="table"></param>
        public override void PrintDataTable(SeriesList list, DataTable table)
        {
            Point.MissingValueFlag = 998877;

            var sortedRows = table.Select("", "tablename");
            for (int i = 0; i < sortedRows.Length; i++)
            {
                var r = sortedRows[i];
                var tn = new TimeSeriesName(r["tablename"].ToString());

                Point p;
                if (r["value"] == DBNull.Value)
                    p = new Point((DateTime)r["datetime"], Point.MissingValueFlag, r["flag"].ToString());
                else
                   p = new Point((DateTime)r["datetime"], (double)r["value"], r["flag"].ToString());

                //if (p.IsMissing || p.FlaggedBad)
                //{
                //    continue;
                //}

                var pcode = tn.pcode.ToUpper();
                var siteid = "";// tn.siteid;
                if (CustomNames != null)
                {
                    var rows = CustomNames.Select("tablename = '" + tn.GetTableName() + "'");
                    if (rows.Length > 0)
                    {
                        r = rows[0];
                        siteid = r["siteid"].ToString();
                    }
                }

                if (siteid == "")
                    continue;

                WriteLine(siteid + p.DateTime.ToString("yyyyMM") + p.DateTime.Day.ToString().PadLeft(2)
                    + pcode + " "
                    + p.Value.ToString("F2").PadLeft(11));
            }


        }

        public override string FormatDate(object o)
        {
            return "";
        }

        public override string FormatFlag(object o)
        {
            return "";
        }

        public override string FormatNumber(object o)
        {
            return "";
        }

        public override void PrintRow(string t0, string[] vals, string[] flags)
        {
        }

        public override void WriteLine(string s)
        {
            Console.WriteLine(s);
        }

        public override void WriteSeriesHeader(SeriesList list)
        {
            WriteLine("<PRE>");
        }

        public override void WriteSeriesTrailer()
        {
        }
    }
}