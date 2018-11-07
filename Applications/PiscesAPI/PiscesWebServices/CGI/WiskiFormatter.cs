using Reclamation.TimeSeries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiscesWebServices.CGI
{
    /// <summary>
    /// Wiski ZRXP formatter for exporting data in WISKI 
    /// https://www.kisters.de/fileadmin/user_upload/Wasser/Downloads/ZRXP3.0_EN.pdf
    /// </summary>
    class WiskiFormatter : Formatter
    {

         public WiskiFormatter(TimeInterval interval)
             : base(interval, false)
         {
             OrderByDate = false; // order by series(tablename)
         }

        
         public override void PrintRow(string t0, string[] vals, string[] flags)
         {

         }

         const string missing = " -777      ";
         public override string FormatNumber(object o)
         {
             var rval = missing;
             if (o == DBNull.Value || o.ToString() == "")
                 rval = missing; 
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
             rval = t.ToString("yyyyMMddHHmm00");
             return rval;
         }
        public override void WriteSeriesHeader(SeriesList list)
        {
        }

        public override void WriteSeriesTrailer()
        {

        }

        /// <summary>
        /// Print DataTable composed of tablename,datetime,value[,flag]
        /// in wiski ZRXP format
        public override void PrintDataTable(SeriesList list, System.Data.DataTable table)
        {
            bool hasFlagCoumn = table.Columns.Count == 4;
            string prevTableName = "";
            for (int i = 0; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];
                var tableName = row[0].ToString();
                var flag = "";
                if (hasFlagCoumn)
                    flag = FormatFlag(row[3]);

                if (tableName != prevTableName)
                {
                    string seriesID = tableName.ToUpper().Replace("INSTANT_","");
                    seriesID = seriesID.Replace("DAILY", "");
                    WriteLine("\n#REXCHANGE" +seriesID + "|*|RTIMELVLhigh-resolution|*|RINVAL-777|*|");
                    prevTableName = tableName;
                }

                string line = FormatDate(row[1]);
                if (!GoodFlag(flag) )
                    line += " -777      ";
                else
                    line += " " + FormatNumber(row[2]);

                WriteLine(line);

            }

        }

        private bool GoodFlag(string flag)
        {
            return flag.Trim() == "" || flag.Trim() == "e";
        }

    }
}
