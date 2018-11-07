using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reclamation.TimeSeries;

namespace Reclamation.TimeSeries.Analysis
{
    /// <summary>
    ///  Parent class for analyis problems presented to the team.
    /// </summary>
    class DataAnalysis
    {
        protected Series data;

        public virtual bool Analyze(Series series) {
            // Base case?
            return false;
        }
    }
}
