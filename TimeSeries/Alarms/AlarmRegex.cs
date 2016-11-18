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
            string expr = @"\s*(?<condition>above|below|rising|dropping)\s*(?<val>[0-9.]+)";
            re = new Regex(expr);
            m_condition = condition;
        }
        internal bool IsMatch()
        {
            return re.IsMatch(m_condition);
        }

        internal AlarmCondition[] AlarmConditions()
        {
            var rval = new List<AlarmCondition>();
            var mc = re.Matches(m_condition);
            for (int i = 0; i < mc.Count; i++)
            {
                var g = mc[i].Groups;
                if (g["condition"].Value == "above")
                    rval.Add(new AlarmCondition(AlarmType.Above,
                             Convert.ToDouble(g["val"].Value)));
                if (g["condition"].Value == "below")
                    rval.Add(new AlarmCondition(AlarmType.Below,
                             Convert.ToDouble(g["val"].Value)));

            }

            return rval.ToArray();
        }

        //internal bool IsAlarm(double d)
        //{
        //    string val = re.Match(m_condition).Groups["val"].Value;
        //    double dvalue = Convert.ToDouble(val);

        //    return (d > dvalue);

        //}
    }

 
}
