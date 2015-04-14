using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.Core;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using System.Text.RegularExpressions;

namespace HydrometServer.CommandLine
{
    class Config
    {
        public string input;
        public string[] cbtt;
        public string[] pcode;
        public string Id = "";
        public string value = "";
        public string pc = "";
        public DateTime T1 ;
        public DateTime T2 ;

        public Config(string input, DateTime t1, DateTime t2)
        {
            this.input = input;

            if (IsDate)
            {
                if (input.Contains('+') || input.Contains('-'))
                {
                    string[] t = input.Split('=');
                    T1 = t1.AddDays(Convert.ToDouble(t[1]));
                    T2 = t2.AddDays(Convert.ToDouble(t[1]));
                }
                else
                {
                    T1 = DateTime.ParseExact(input.ToLower().Trim().Substring(input.Length - 9),
                    "yyyyMMMdd", System.Globalization.CultureInfo.InvariantCulture);
                    T2 = T1;
                }
            }

            if (IsDateRange)
            {
                string t = input.ToLower().Trim().Substring(input.IndexOf("/file"));
                t = t.Replace(" ", "").Replace("=", "").Replace("/file", "");
                string[] n = t.Split(',');
                T1 = DateTime.ParseExact(n[0],
                    "yyyyMMMdd", System.Globalization.CultureInfo.InvariantCulture);
                T2 = DateTime.ParseExact(n[1],
                    "yyyyMMMdd", System.Globalization.CultureInfo.InvariantCulture);
                input = input.Remove(input.IndexOf("/file="));
            }

            if (IsGetAll)
            {
                string a = input.Replace("g/a", "").Replace("get/a", "");
                cbtt = a.Split(' ')[1].Split(',');
            }

            if (IsGet)
            {
                string a = input.Replace("g", "").Replace("get", "");
                cbtt = a.Split(' ')[1].Split(',');
            }

            if (IsGetPcode && IsGetAll == false)
            {
                string a = input.Replace("g/", "").Replace("get/", "");
                cbtt = a.Split(' ')[1].Split(',');
                pcode = a.Split(' ')[0].Split(',');
            }
        }
        
        //Logic to determine the input we have from the user
        public bool IsGetAll
        {
            get
            {
                return (input.ToLower().Trim().Contains("g/a ") || input.ToLower().Trim().Contains("get/a "));
            }
        }

        public bool IsGet
        {
            get
            {
                return (input.ToLower().Trim().Contains("g ") || input.ToLower().Trim().Contains("get "));
            }
        }
        // Note for pcode need to check if IsGetAll is false and IsGetPcode is true
        public bool IsGetPcode
        {
            get
            {
                return (input.ToLower().Trim().Contains("g/") || input.ToLower().Trim().Contains("get/"));
            }
        }
        
        public bool IsShow
        {
            get
            {
                return (input.ToLower().Trim()[0] == 's' || input.ToLower().Trim().Contains("show"));
            }
        }

        public bool IsDate
        {
            get
            {
                return (input.ToLower().Trim()[0] == 'd' || input.ToLower().Trim().Contains("date"));
            }
        }
        
        public bool IsDateRange
        {
            get
            {
                return (input.ToLower().Trim().Contains("file"));
            }
        }
    }
}
