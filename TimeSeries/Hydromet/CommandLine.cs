using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Reclamation.TimeSeries.Hydromet
{
    /// <summary>
    /// Manages command line requests for Hydromet data 
    /// examples:
    /// hgh af, daily:hgh af,daily:hgh fb
    /// luc qm, daily:luc qd
    /// </summary>
    public class CommandLine
    {
        string cmd;
        List<string> m_cbtt = new List<string>();
        List<string> m_pcode = new List<string>();
        List<string> m_interval = new List<string>();
        string defaultInterval = "";

        public string Title { get; set; }

        public CommandLine(string cmd, TimeInterval  defaultDataBase)
        {
            this.cmd = cmd;
            ParseCommand(cmd, defaultDataBase);
        }
        
        private void ParseCommand(string cmd, TimeInterval defaultDataBase)
        {
            if (defaultDataBase == TimeInterval.Daily)
                defaultInterval = "daily";
            if (defaultDataBase == TimeInterval.Irregular)
            {
                defaultInterval = "instant";
            }
            if (defaultDataBase == TimeInterval.Monthly)
            {
                defaultInterval = "mpoll";
            }

            // look for title prefixed with #
            Title = "";
            var idx = cmd.IndexOf("#");
            if (idx >= 0)
            {
                Title = cmd.Substring(idx + 1);
                cmd = cmd.Substring(0, idx);
            }

            string pattern = "((?<prefix>\\w+)\\:\\s*)?(?<cbtt>\\w+)\\s+(?<pcode>[A-Za-z0-9\\-_\\.]+)(\\s*\\,|\\s*$)";
            var mc = Regex.Matches(cmd, pattern, RegexOptions.IgnorePatternWhitespace);

            for (int i = 0; i < mc.Count; i++)
            {
                m_cbtt.Add(mc[i].Groups["cbtt"].ToString());

                var prefix = mc[i].Groups["prefix"].ToString().Trim();
                if (prefix == "")
                    prefix = defaultInterval;

                m_interval.Add(prefix);

                m_pcode.Add(mc[i].Groups["pcode"].ToString());
            }
        }

        public string[] CbttList { get { return m_cbtt.ToArray(); } }

        public bool IsSingleCbtt
        {
            get
            {
                return m_cbtt.Distinct().Count() == 1;
            }
        }
        //public string[] Pcode { get { return m_pcode.ToArray(); } }
        //public string[] Prefix { get { return m_interval.ToArray(); } }

        public int Count { get { return m_cbtt.Count; } }


        public SeriesList CreateSeries(HydrometHost server)
        {
            SeriesList rval = new SeriesList();

            for (int i = 0; i < Count; i++)
            {
             if( m_interval[i] ==  "mpoll" || m_interval[i] == "m" )
                 rval.Add(new HydrometMonthlySeries(m_cbtt[i],m_pcode[i],server));
             if( m_interval[i] == "daily" || m_interval[i] == "arc"
                 || m_interval[i] == "d" )
                 rval.Add(new HydrometDailySeries(m_cbtt[i], m_pcode[i], server));
            if( m_interval[i] == "day" || m_interval[i] == "instant"
                || m_interval[i] == "i")
                rval.Add(new HydrometInstantSeries(m_cbtt[i], m_pcode[i], server));
            }


            return rval;
        }




        public string GetDefaultQuery()
        {
            //hgh af, daily:hgh af,daily:hgh fb
            string rval = "";
            for (int i = 0; i < m_interval.Count; i++)
            {
                if (m_interval[i] == defaultInterval)
                    rval += m_cbtt[i] + " " + m_pcode[i] + ", ";
            }

            return rval;
        }
    }
}
