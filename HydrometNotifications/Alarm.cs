using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries;
using Reclamation.Core;
using System.Text.RegularExpressions;
using System.IO;

namespace HydrometNotifications
{
    public class Alarm
    {
        protected AlarmDataSet.alarm_definitionRow m_row;

        internal string Details = "";
        internal Series m_series = new Series();
        internal Point? event_point;
        protected DateTime t;

        internal Dictionary<string, string> customMessageVariables = new Dictionary<string, string>();

        public Alarm(AlarmDataSet.alarm_definitionRow row)
        {
            m_row = row;
            event_point = null;
        }

        /// <summary>
        /// tests if condition of alarm is true
        /// </summary>
        /// <returns></returns>
        public virtual bool Check(DateTime t)
        {
            this.t = t;
            throw new NotImplementedException("Please override the Check() method");
        }

        public string CreateMessage(out string emailMessage, out string txtMessage)
        {

             emailMessage = m_row.message;

             txtMessage = m_row.message;
            var pattern = @"%file=(?<filename>\S+)";
            var m = Regex.Match(emailMessage, pattern);
            if( m.Success)
            {
                txtMessage = "";
                string fn = m.Groups["filename"].Value;
                if (File.Exists(fn))
                {
                    emailMessage = Regex.Replace(emailMessage, pattern, File.ReadAllText(fn));
                }
                else
                {
                    emailMessage = "Error:  File not found "+fn+"  \n\n" + emailMessage;
                }
            }

            txtMessage += m_row.cbtt.ToUpper() + " " + m_row.pcode.ToUpper() + " ";

            emailMessage = emailMessage.Replace("%site_name", m_series.SiteID);

            
            
            emailMessage = InsertTable(emailMessage);

            emailMessage = emailMessage.Replace("%data_source", m_row.data_source);

            if (event_point.HasValue)
            {
                emailMessage = emailMessage.Replace("%value", event_point.Value.Value.ToString("F2"));
                txtMessage += event_point.Value.Value.ToString("F2");
            }

            emailMessage = emailMessage.Replace("%date", t.ToString());

            if (event_point.HasValue)
            {
                emailMessage = emailMessage.Replace("%event_date_yyyymmdd", event_point.Value.DateTime.ToString("yyyyMMdd"));

                if( m_series.TimeInterval == TimeInterval.Daily)
                    emailMessage = emailMessage.Replace("%event_date", event_point.Value.DateTime.ToLongDateString());
                else
                    emailMessage = emailMessage.Replace("%event_date", event_point.Value.DateTime.ToString());
            }

            emailMessage = emailMessage.Replace("%cbtt", m_row.cbtt.ToUpper());
            emailMessage = emailMessage.Replace("%pcode", m_row.pcode.ToUpper());


            // custom variables (used by rogue)
            foreach (var var in customMessageVariables)
            {
                emailMessage = emailMessage.Replace(var.Key, var.Value);
            }

            emailMessage += "\n";

            if (m_row.database == "i") // instant
            {
                emailMessage += "<a href=\"http://www.usbr.gov/pn/hydromet/instant_graph.html?cbtt=" + m_row.cbtt + "&pcode="
                    + m_row.pcode + "&t2=" +this.t.ToString("yyyyMMdd")+"\">graph</a>";
            }

            else if (m_row.database == "d") // daily
            {
                emailMessage += "<a href=\"http://www.usbr.gov/pn/hydromet/daily_graph.html?cbtt=" + m_row.cbtt + "&pcode="
                    + m_row.pcode + "&t2=" + this.t.ToString("yyyyMMdd") + "\">graph</a>";

            }
            else // default...
            {
                 emailMessage += "\nhttp://www.usbr.gov/pn-bin/rtgraph.pl?sta=" + m_row.cbtt + "&parm=" + m_row.pcode;
            }

            emailMessage += Details;

            return emailMessage;
        }

        private string InsertTable(string rval)
        {

            bool instant = m_series.TimeInterval == TimeInterval.Irregular;
            
            int hours_back = 72;
            if (!instant)
                hours_back = 240; // 10 days
            var s_table = GetAndReadInternal(m_row.cbtt, m_row.pcode, t, hours_back, instant, true);

            rval = rval.Replace("%table", "<pre>\n" +
                                  s_table.ToString(true) + "</pre>");
            return rval;
        }

        public void GetSeriesWithData(bool keepFlaggedData)
        {
            bool instant = (m_row.data_source.ToLower().IndexOf("i:") == 0); 

            m_series = GetAndReadInternal(m_row.cbtt, m_row.pcode, t, m_row.hours_back,instant,keepFlaggedData);
        }

        private static Series GetAndReadInternal(string cbtt, string pcode,DateTime t, int hours_back,
            bool instant, bool keepFlaggedData)
        {

            if (instant)
            {
                HydrometInstantSeries.KeepFlaggedData = keepFlaggedData;
                return GetInstantSeries(cbtt, pcode, t, hours_back);
            }
            else
            {
                return GetDailySeries(cbtt, pcode, t, hours_back);
            }
        }
         static Series GetInstantSeries(string cbtt, string pcode,DateTime t, int hours_back)
        {
            Series s = new HydrometInstantSeries(cbtt, pcode);
            var t1 = t.AddHours(-hours_back);
            s.Read(t1, t);
            // Hydromet web query doesn't have hour resolution.  we must clean that ourselves.
            var s2 = Reclamation.TimeSeries.Math.Subset(s, t1, t);
            // keep data in HydrometInstantSeries (becuse it understands it's flags)
            s.Clear();
            s.Add(s2);
            return s;
        }

         static HydrometDailySeries GetDailySeries(string cbtt, string pcode,DateTime t,int hours_back)
        {
            var s = new HydrometDailySeries(cbtt, pcode);
            var t2 = t.Date; //.AddDays(-1); // hydromet daily data begins yesterday
            var t1 = t2.AddHours(-hours_back).Date;
            s.Read(t1, t2);
            return s;
        }

       
    }
}
