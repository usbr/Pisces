using System;
using System.Text.RegularExpressions;
using Reclamation.TimeSeries;

namespace HydrometNotifications
{
    class FlagAlarm : Alarm
    {

        public static string Expression = @"\s*AnyFlags\s*";

        public FlagAlarm(AlarmDataSet.alarm_definitionRow row, string conditionColumnName)
            : base(row)
        {
            string condition = row[conditionColumnName].ToString();
            var m = Regex.Match(condition, Expression);
            if (!m.Success)
            {
                throw new ArgumentException("Invalid condition '" + condition + "'");
            }
        }

        public override bool Check(DateTime t)
        {
            this.t = t;
            GetSeriesWithData(true);
            m_series.RemoveMissing();

            if (!m_series.HasFlags)
                throw new Exception("FlagAlarm: The underlying series does not have the required flag column");

            for (int i = 0; i < m_series.Count; i++)
            {
                Point pt = m_series[i];
                if (m_series.IsBadData(pt.Flag))
                    return true;
            }

            return false;
        }
    }
}
