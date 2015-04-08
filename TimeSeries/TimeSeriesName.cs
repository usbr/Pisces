using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// Manages a time series name in the form interval_siteid_pcode
    /// </summary>
    public class TimeSeriesName
    {
        string m_name="";
        public string siteid;
        public string pcode;
        public string interval;
        private string m_defaultInterval;

        public bool Valid = false;

        public TimeSeriesName(string name, TimeInterval interval)
        {
            if (interval == TimeInterval.Irregular)
                Init(name, "instant");
            else if (interval == TimeInterval.Daily)
                Init(name, "daily");
            else if (interval == TimeInterval.Monthly)
                Init(name, "monthly");
            else throw new ArgumentException("TimeSeriesName: interval " + interval.ToString() + " not supported ");
        }
        public TimeSeriesName(string name, string defaultInterval = "")
        {
            Init(name, defaultInterval);
        }

        public TimeInterval GetTimeInterval()
        {
            TimeInterval rval = TimeInterval.Irregular;
            if (interval == "instant")
                rval = TimeInterval.Irregular;
            if (interval == "daily")
                rval = TimeInterval.Daily;
            if (interval == "monthly")
                rval = TimeInterval.Monthly;

            return rval;
        }

        private void Init(string name, string defaultInterval)
        {
            m_defaultInterval = defaultInterval;
            m_name = name;
            Parse();   
            if (!Valid)
            {
                Logger.WriteLine("Init(): Invalid table name '" + name + "'   [" +defaultInterval+"]" );
            }
        }




        public string Name
        {
            get
            {
                return m_name;
            }
        }

        public string GetTableName()
        {
            if( Valid)
            return interval + "_" + siteid + "_" + pcode;

            throw new Exception("GetTableName(): Invalid name");
        }

         private void Parse()
        {

            string pattern = @"^(?<prefix>(instant|daily|monthly))?_?(?<cbtt>[a-zA-Z]{1,8}\w)_(?<pcode>[a-zA-Z0-9]{1,8}$)";
            var m = Regex.Match(m_name, pattern);

            siteid = "";
            pcode = "";
            interval = "";
            Valid = m.Success;
            if (Valid)
            {
                interval = m.Groups["prefix"].Value.ToLower();
                siteid = m.Groups["cbtt"].Value.ToLower();
                pcode = m.Groups["pcode"].Value.ToLower() ;
                if (interval == "" && m_defaultInterval != "")
                    interval = m_defaultInterval;
            }
        }


        public bool HasInterval { 
            get{
                return interval != "";
            } 
        
        }
    }
}
