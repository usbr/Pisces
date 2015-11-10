using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HydrometNotifications
{

    /// <summary>
    /// Checks for value above or below a limit
    /// </summary>
    class LimitAlarm:Alarm
    {
        public static string Expression = @"\s*(?<operator><|>|!=)\s*(?<value>\d+\.?\d*)";

        double m_alarmValue;
        string m_alarmOperator;

        public LimitAlarm(AlarmDataSet.alarm_definitionRow row, string conditionColumnName)
            : base(row)
        {
            string condition = row[conditionColumnName].ToString();
            var m = Regex.Match(condition, Expression);
            if (m.Success)
            {
                m_alarmValue = Convert.ToDouble(m.Groups["value"].Value);
                m_alarmOperator = m.Groups["operator"].Value;
                customMessageVariables.Add("%limit_expression", m.Groups[0].Value);
            }
            else
            {
                throw new ArgumentException("Invalid condition '" + condition + "'");
            }
        }

        public override bool Check(DateTime t)
        {
            this.t = t;
            GetSeriesWithData(false);
            
            m_series.RemoveMissing();
            if (m_series.Count == 0)
                return false;

            bool rval = false;

            for (int i = 0; i < m_series.Count; i++)
            {
                event_point = m_series[i];
                if (m_series.IsBadData(event_point.Value.Flag))
                    continue;

                if (m_alarmOperator == "!=")
                {
                    rval = event_point.Value.Value != m_alarmValue;
                }
                else
                    if (m_alarmOperator == ">")
                    {
                        rval = event_point.Value.Value > m_alarmValue;
                    }
                    else
                        if (m_alarmOperator == "<")
                        {
                            rval = event_point.Value.Value < m_alarmValue;
                        }
                        else
                        {
                            throw new InvalidOperationException(m_alarmOperator);
                        }
                if (rval)
                    return true;
            }
            return false;
        }

     
    }
}
