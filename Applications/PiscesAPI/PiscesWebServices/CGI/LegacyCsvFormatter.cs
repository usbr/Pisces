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
         string delim;
         public LegacyCsvFormatter(TimeInterval interval, bool printFlags, string delimeter=","):base(interval,printFlags)
         {
             delim = delimeter;
         }

         public override void PrintRow(string t0, string[] vals, string[] flags)
         {
             StringBuilder sb = new StringBuilder(vals.Length * 8);
             sb.Append(t0 + delim);
             for (int i = 0; i < vals.Length; i++)
             {
                 if (m_interval == TimeInterval.Daily && vals[i] == null)
                 {
                     sb.Append(s_NO_RECORD);
                 }
                 else
                 {
                     sb.Append(vals[i]);
                 }
                 if (PrintFlags)
                 {   // 
                     if( flags[i]=="")
                         sb.Append(" ");//VMS default flag is ' '
                     else
                         sb.Append(flags[i]);
                 }
                 if (i != vals.Length - 1)
                     sb.Append(delim);
             }
             WriteLine(sb.ToString());

         }

         public override string FormatFlag(object o)
         {
             if (o == DBNull.Value)
                 return "";
             else
                 return o.ToString();

         }

         static string s_NO_RECORD = "NO RECORD   ";

         public override string FormatNumber(object o)
         {
             var rval = "";
             if (o == DBNull.Value || o.ToString() == "")
             {
                 if (m_interval == TimeInterval.Daily)
                     rval = s_NO_RECORD;
                 else
                     rval = "";//.PadLeft(11);
             }
             else
             {
                 if( m_interval == TimeInterval.Daily)
                    rval = Convert.ToDouble(o).ToString("F02").PadLeft(12);//%12.2f
                 else
                    rval = Convert.ToDouble(o).ToString("F02").PadLeft(11);
             }
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
            WriteLine(HydrometWebUtility.HydrometHeader());
             WriteLine("BEGIN DATA");

             string headLine = "DATE      ";
             if (m_interval == TimeInterval.Irregular || m_interval == TimeInterval.Hourly)
                 headLine = "DATE       TIME ";

             foreach (var item in list)
             {
                 TimeSeriesName tn = new TimeSeriesName(item.Table.TableName);
                 if (m_interval == TimeInterval.Daily)
                 {
                     //fprintf(stdout,",   %4.8s %-4.8s",params[i].station,params[i].pcode);
                     headLine += delim + "   " + tn.siteid.PadRight(4) + " " + tn.pcode.PadRight(4);
                 }
                 else
                 {
                     headLine += delim + "  " + tn.siteid.PadRight(8) + "" + tn.pcode.PadRight(8);
                 }
             }
             headLine = headLine.ToUpper();
             WriteLine(headLine);
         }

         public override void WriteSeriesTrailer()
         {
             WriteLine("END DATA");
             HydrometWebUtility.PrintHydrometTrailer();
         }

    }
}
