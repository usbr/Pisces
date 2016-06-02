using Reclamation.TimeSeries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PiscesWebServices.CGI
{
    /// <summary>
    /// base class for different output formats.
    /// </summary>
     class LegacyCsvFormatter: Formatter
    {
         public LegacyCsvFormatter(TimeInterval interval, bool printFlags):base(interval,printFlags)
         {
            
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
                     sb.Append(" ,");
             }
             Console.WriteLine(sb.ToString());

         }
         public override string FormatFlag(object o)
         {
             if (o == DBNull.Value)
                 return "";
             else
                 return o.ToString();

         }

         public override string FormatNumber(object o)
         {
             var rval = "";
             if (o == DBNull.Value || o.ToString() == "")
                 rval = "";//.PadLeft(11);
             else
                 rval = Convert.ToDouble(o).ToString("F02").PadLeft(11);
             return rval;
         }

          public override string FormatDate(object o)
         {

             var rval = "";
             var t = Convert.ToDateTime(o);
             if (Interval == TimeInterval.Irregular || Interval == TimeInterval.Hourly)
                 rval = t.ToString("MM/dd/yyyy HH:mm");
             else
                 rval = t.ToString("MM/dd/yyyy");
             return rval;
         }

         public override void WriteSeriesHeader(SeriesList list)
         {
             HydrometWebUtility.PrintHydrometHeader();
             Console.WriteLine("BEGIN DATA");

             string headLine = "DATE      ";
             if (m_interval == TimeInterval.Irregular || m_interval == TimeInterval.Hourly)
                 headLine = "DATE       TIME ";

             foreach (var item in list)
             {
                 TimeSeriesName tn = new TimeSeriesName(item.Table.TableName);
                 headLine += ",  " + tn.siteid.PadRight(8) + "" + tn.pcode.PadRight(8);
             }
             headLine = headLine.ToUpper();
             Console.WriteLine(headLine);
         }

         public override void WriteSeriesTrailer()
         {
             Console.WriteLine("END DATA");
         }

    }
}
