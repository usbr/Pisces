using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.TimeSeries.Hydromet;

namespace Reclamation.TimeSeries.Hydromet.Operations
{
    public class RuleCurveFactory
    {

        public static HydrometRuleCurve Create(FloodControlPoint pt, int waterYear,bool dashed)
        {
            var cbtt = pt.StationFC.ToLower();
            if (cbtt == "heii")// || stationQU == "jck")
                return new HeiiRuleCurve(cbtt, waterYear,pt.FillType,dashed);
            if (cbtt == "hgh")
                return new HghRuleCurve(cbtt,pt.FillType);

            if (cbtt == "wodi")
                return new WodiRuleCurve(cbtt, waterYear, FillType.Fixed);

          return new HydrometRuleCurve(pt);
            
        }
    }
}
