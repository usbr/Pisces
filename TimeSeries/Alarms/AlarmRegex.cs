using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Reclamation.TimeSeries.Alarms
{
    public class AlarmRegex
    {
        Regex re;
        string m_condition;
        public AlarmRegex(string condition)
        {
            string expr = @"\s*above\s*(?<val>[0-9.]+)";
            re = new Regex(expr);
            m_condition = condition;
        }
        internal bool IsMatch()
        {
            return re.IsMatch(m_condition);
        }

        internal bool IsAlarm(double d)
        {
            string val = re.Match(m_condition).Groups["val"].Value;
            double dvalue = Convert.ToDouble(val);

            return (d > dvalue);

        }
    }
}
