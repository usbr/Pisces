using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// using Reclamation.Core;

namespace Reclamation.TimeSeries.Analysis
{
    /// <summary>
    ///  Algorithm and possible notification system of inconsistent data
    ///  resulting from manual bucket dumps.
    /// </summary>
    class ManualBucketDump : DataAnalysis
    {
        public override bool Analyze(Series series) {
            /* XXX: Could be done by grabbing values for simple bool return, or
                    with points for actual manipulation of data within this method.
                    We could also return a list of what data was bad/lacking and return it
                    to be handled by a handler class(or for testing purposes where we know
                    the location of the bad data).
             */
            double[] values = series.Values;
            // Point prevPoint = series[0];
            double prevVal = values[0];
            int dumpTriggerVal = 10;
            for (int i = 1; i < values.Count(); i++) {
                // Point currentPoint = series[i];
                double currVal = values[i];
                // if((prevPoint.Value - currentPoint.Value) > dumpTriggerVal) {
                if ( (prevVal - currVal) > dumpTriggerVal ) { // The difference is great enought to trigger bucket dump trigger
                    // Once triggered either notify/set flag/correct data
                    return true;
                }
            }
            return false;
        }
    }
}
