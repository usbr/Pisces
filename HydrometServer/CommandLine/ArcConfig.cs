using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydrometServer.CommandLine
{
    class ArcConfig
    {
        public string input;
        public string[] cbtt;
        public string[] pcode;
        public Int32 NDays;
        public Int32 AddDays;
        public DateTime StartTime = DateTime.Today.AddDays(-5);
        public Int32 WaterYear;
        public string t1Range;
        public string t2Range;
        public string error;

        public ArcConfig(string input)
        {
            this.input = input;
            if (IsNDay)
            {
                string [] split = input.Split('=');
                NDays = Convert.ToInt32(split[1]);
            }

            if (IsDate)
            {
                if (input.Contains('+') || input.Contains('-'))
                {
                    string[] split = input.Split('=');
                    AddDays = Convert.ToInt32(split[1]);
                }
                else
                {
                    StartTime = DateTime.ParseExact(input.ToLower().Trim().Substring(input.Length - 9),
                    "yyyyMMMdd", System.Globalization.CultureInfo.InvariantCulture);
                }
            }

            if (IsYear)
            {
                string[] split = input.Split('=');
                WaterYear = Convert.ToInt32(split[1]);
            }

            if (IsDateRange)
            {
                string[] split = input.Split('=');
                split = split[1].Split(',');
                t1Range = split[0].Trim().Split(' ')[0];
                t2Range = split[1].Trim().Split(' ')[0];
            }

            if (IsGet)
            {
                string[] split = input.ToUpper().Trim().Split(' ');
                if (split.Count() > 2)
                {
                    error = "To many spaces";
                }
                cbtt = split[1].Split(',');
            }

            if (IsGetPcode)
            {
                string[] split = input.ToUpper().Trim().Split(' ');
                if (split.Count() > 2)
                {
                    error = "To many spaces";
                }
                cbtt = split[1].Split(',');
                split = split[0].ToUpper().Trim().Split('/');
                pcode = split[1].Split(',');

            }
         

        }

        public bool IsNDay
        {
            get
            {
                return (input.ToLower().Contains("ndays"));
            }
        }

        public bool IsYear
        {
            get
            {
                return (input.ToLower().Contains("year"));
            }
        }

        public bool IsDateRange
        {
            get
            {
                return (input.ToLower().Trim().Contains("/d=") || input.ToLower().Trim().Contains("/date="));
            }
        }

        public bool IsDate
        {
            get
            {
                return ((input.ToLower().Trim()[0] == 'd' || input.ToLower().Trim().Contains("date")) && IsDateRange == false);
            }
        }

        public bool IsShow
        {
            get
            {
                return (input.ToLower().Trim()[0] == 's' || input.ToLower().Trim().Contains("show"));
            }
        }

        public bool IsFormat
        {
            get
            {
                return (input.ToLower().Trim()[0] == 'f' || input.ToLower().Trim().Contains("format"));
            }
        }

        public bool IsGetPcode
        {
            get
            {
                return ((input.ToLower().Trim().Contains("g/") || input.ToLower().Trim().Contains("get/")) && IsGetAllDateRange == false);
            }
        }

        public bool IsGet
        {
            get
            {
                return (input.ToLower().Trim().Contains("g ") || input.ToLower().Trim().Contains("get "));
            }
        }

        public bool IsGetAllDateRange
        {
            get
            {
                return (input.ToLower().Trim().Contains("g/d") || input.ToLower().Trim().Contains("get/d"));
            }
        }
    }
}
