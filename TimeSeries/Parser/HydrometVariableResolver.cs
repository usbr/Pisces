using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Parser
{

    /// <summary>
    /// Used to find hydromet data using symbolic names 
    /// like AMF_AF  where the underscore separates the cbtt name
    /// from the parameter code.
    /// Use by forecasting program, and TimeSeriesDatabase when resolving 
    /// equations from Hydromet
    /// </summary>
    public class HydrometVariableResolver:VariableResolver
    {
        public HydrometVariableResolver():base()
        {
        }


        /// <summary>
        /// Lookup hydromet Series.
        /// name is  interval_cbtt_pcode
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override ParserResult Lookup(string name, TimeInterval defaultInterval)
        {
            var interval = defaultInterval;
           
            TimeSeriesName tn = new TimeSeriesName(name);
            if( tn.HasInterval)
            {
                if (tn.interval == "instant")
                    interval = TimeInterval.Irregular;
                if (tn.interval == "daily")
                    interval = TimeInterval.Daily;
                if (tn.interval == "monthly")
                    interval = TimeInterval.Monthly;
            }

            if (tn.Valid)
            {
                Logger.WriteLine("Hydromet Lookup " + tn.siteid + "," + tn.pcode);
                var s = new Series();

                if (interval ==  TimeInterval.Monthly)
                {
                    s = new HydrometMonthlySeries(tn.siteid, tn.pcode);
                }
                else if (interval == TimeInterval.Irregular)
                {
                    s = new HydrometInstantSeries(tn.siteid, tn.pcode);
                }
                else if (interval == TimeInterval.Daily)
                {
                    s = new HydrometDailySeries(tn.siteid, tn.pcode);
                }

                return new ParserResult(s);
            }
            else
            {
                return base.Lookup(name,interval);
            }
        }
    }
}
