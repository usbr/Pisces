using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydrometNotifications
{
    class RogueSystemState:Alarm
    {

        public static string Expression = @"\s*RogueSystemState";

        public RogueSystemState(AlarmDataSet.alarm_definitionRow row)
            : base(row)
        {
            // dont set the state alarm (it should alarm for each daily state change )
            row.IsVirtual = true; // prevents active flag from being set.
        }


        public override bool Check(DateTime t)
        {
            this.t = t;
            var s1 = RogueMinimumFlow.DetermineSystemState(t);
            var s2 = RogueMinimumFlow.DetermineSystemState(t.AddDays(-1));

            return s1 != s2;
        }
    }
}
