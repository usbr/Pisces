using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Reports
{
    public class YakimaStatusReport
    {
        public YakimaStatusReport()
        {
        }

        string[] resList = { "kee", "kac", "cle", "bum", "rim", "clr" };
        double[] resCapacity ={157800,239000,436900,33970,198000,-1};
        double[] res_af = { 0, 0, 0, 0, 0, 0 };
        double[] res_af2 = { 0, 0, 0, 0, 0, 0 }; // previous day same hour
        double[] res_q = { 0, 0, 0, 0, 0 ,0};
        double totalCapacity= 1065400;
        double total_af = 0;
        double total_q = 0;
        double total_in = 0;

        /// <summary>
        /// Creates and returns the report.
        /// </summary>
        /// <returns></returns>
        public string Create(DateTime t) // default 8am.
        {
            string rval = GetTemplate();
            //13-OCT-2016  09:12:35
            var fmt = "dd-MMM-yyyy  HH:mm:ss";
            rval = rval.Replace(fmt, t.ToString(fmt));
            rval = rval.Replace("HH:mm", t.ToString("HH:mm"));


            res_af = Array.ConvertAll(res_af,x => x=double.MinValue);
            res_af2 = Array.ConvertAll(res_af, x => x = double.MinValue);
            res_q = Array.ConvertAll(res_af, x => x = double.MinValue);
            
            DateTime t1 =t.AddDays(-1);
            DateTime t2 = t;
            HydrometDataCache c = new HydrometDataCache();
            
            HydrometInstantSeries.Cache.Add(this.yakima_data, t1, t2, HydrometHost.Yakima, TimeInterval.Irregular);
            
            foreach (var cbtt in resList)
            {
              rval = ProcessParameter(rval ,t, cbtt, "fb");
              rval = ProcessParameter(rval, t, cbtt, "af");
              rval = ProcessParameter(rval, t, cbtt, "q");
            }

            rval = ReplaceSymbol(rval, "%total_af", total_af);
            double total_pct = total_af / totalCapacity * 100.0;
            rval = ReplaceSymbol(rval, "%total_pct", total_pct);
            rval = ReplaceSymbol(rval, "%total_q", total_q);

            // compute inflows.
            for (int i = 0; i < resList.Length; i++)
            {
                var qu = (res_af[i]-res_af2[i]) / 1.9835 + res_q[i];
                rval = ReplaceSymbol(rval, "%" + resList[i] + "_in", qu);
                total_in += qu;
            }
            rval = ReplaceSymbol(rval, "%total_in", total_in);
            foreach (var canal in DataSubset("qc"))
            {
                var x = canal.Substring(0, canal.IndexOf(" "));
                rval = ProcessParameter(rval, t, x, "qc");
            }

            foreach (var river in DataSubset("q"))
            {
                var x = river.Substring(0, river.IndexOf(" "));
                rval = ProcessParameter(rval, t, x, "q");
            }
            
            return rval;

        }

        private string[] DataSubset(string pcode)
        {
            var query = from a in yakima_data
                        where a.IndexOf(" " + pcode) > 0
                        select a;
            var rval =  query.ToArray();
            return rval;
        }

        private string ProcessParameter(string txt, DateTime t, string cbtt, string pcode)
        {
            var x = GetValue(cbtt, pcode, t);
            int decimals =  ( pcode.Trim() == "fb") ? 2 : 0;
            int idx = Array.IndexOf(resList, cbtt);

            var rval =  ReplaceSymbol(txt, "%" + cbtt + "_" + pcode, x, decimals);

            if( pcode == "af" && x != Point.MissingValueFlag)
            {// also take care of percent full
                total_af += x;
                
                if( idx >=0)
                {
                    res_af[idx] = x;
                    double pct = 100.0 * x /resCapacity[idx];
                    rval = ReplaceSymbol(rval, "%" + cbtt + "_pct", pct, 0);
                   // get yesterdays storage
                    var x2 = GetValue(cbtt, pcode, t.AddDays(-1));
                    if( x2 != Point.MissingValueFlag)
                    {
                        res_af2[idx] = x2;
                    }
                }
            }

            if (pcode == "q" && x != Point.MissingValueFlag )
            {
                total_q += x;
                if( idx>=0)
                  res_q[idx] = x;
            }

            return rval;
        }
        
        /// <summary>
        /// replaces placholder symbol with formated value 
        /// </summary>
        private string ReplaceSymbol(string text, string symbol, 
                                     double val,int decimals=0)
        {
            var str = val.ToString("F" + decimals.ToString());
            if (decimals == 0)
                str += ".";

            if (str.Length > 30 || val == double.MinValue)
                str = "Error".PadLeft(symbol.Length);
            else
            {
                str = str.PadLeft(symbol.Length);
            }

            return text.Replace(symbol, str);
        }

        public double GetValue(string cbtt, string pcode, DateTime t)
        {
            var s = new HydrometInstantSeries(cbtt, pcode);
            //DateTime th = new DateTime(t.Year, t.Month, t.Day, hour, 0, 0);
            s.Read(t,t);
            
            if( s.Count >0 && !s[0].IsMissing)
            {
                return s[0].Value;
            }
            return Point.MissingValueFlag;
        }
        
         private string GetTemplate()
        {
            return File.ReadAllText(
                  Path.Combine(
                  FileUtility.GetExecutableDirectory(),
                  "YakimaStatusTemplate.txt")
                  );

        }
        private string[] yakima_data =
            new string[]{
"kee fb",
"kac fb",
"cle fb",
"bum fb",
"rim fb",
"clr fb",
"kee af",
"kac af",
"cle af",
"bum af",
"rim af",
"easw q",
"yumw q",
"umtw q",
"nacw q",
"parw q",
"yrpw q",
"kee q",
"kac q",
"cle q",
"bum q",
"rim q",
"ktcw qc",
"wopw qc",
"rzcw qc",
"nscw qc",
"sncw qc",
"rscw qc",
"tiew qc",
"rozw qc",
"wesw qc",
"chcw qc",
"kncw qc",
"tnaw q",
"yrww q",
"clfw q",
"ticw q",
"rbdw q",
"yrcw q"            };

    }
}
