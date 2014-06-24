using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Hydromet.Operations
{
    class HghRuleCurve: HydrometRuleCurve
    {
        public HghRuleCurve(string cbtt,FillType fType):base(cbtt,fType)
        {

        }

        public override double LookupRequiredSpace(DateTime t, double resid, out string flag)
        {
            flag = "";
            if (t.Month >= 5 && t.Month <= 9)
                return HydrometRuleCurve.MissingValue;
            return base.LookupRequiredSpace(t, resid, out flag);
        }
    }
}
