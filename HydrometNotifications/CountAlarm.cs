using System;
using System.Text.RegularExpressions;

namespace HydrometNotifications
{
    class CountAlarm:Alarm
    {
        public static string Expression = @"Count\s*(?<equality><|>|=)\s*(?<limit>\d+)";

        int limit = 0;
        string equality = "";
        string conditionColumnName;
        public CountAlarm(AlarmDataSet.alarm_definitionRow row, string conditionColumnName)
            : base(row)
        {
            this.conditionColumnName = conditionColumnName;
            var m = Regex.Match(row.alarm_condition, Expression);
            if (m.Success)
            {
                limit = Convert.ToInt32(m.Groups["limit"].Value);
                equality = m.Groups["equality"].Value;
            }
            else
            {
                throw new ArgumentException("Invalid condition '"+row.alarm_condition+"' ");
            }
        }

        public override bool Check(DateTime t)
        {
            this.t = t;
            GetSeriesWithData(true);
            
            m_series.RemoveMissing();

            if (equality == "<")
                return m_series.Count < limit;
            else if( equality == ">")
                return m_series.Count > limit;
            else if (equality == "=")
                return m_series.Count == limit;


            throw new ArgumentException("invalid condition " + m_row[conditionColumnName].ToString());
        }

        
    }
}
