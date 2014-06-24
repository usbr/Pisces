using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.TimeSeries.Hydromet;

namespace Reclamation.TimeSeries.Hydromet.Operations
{
    public class RuleCurveFactory
    {

        public static HydrometRuleCurve Create(FloodControlPoint pt, int waterYear)
        {
            var stationQU = pt.StationQU.ToLower();
            if (stationQU == "heii")// || stationQU == "jck")
                return new HeiiRuleCurve(stationQU, waterYear,pt.FillType);
            if (stationQU == "hgh")
                return new HghRuleCurve(stationQU,pt.FillType);

            if (stationQU == "wodi")
                return new WodiRuleCurve(stationQU, waterYear, FillType.Fixed);

          return new HydrometRuleCurve(pt);
            
        }
    }
}
