using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Estimation
{
    class MultipleLinearRegressionResults
    {
        public string[] Report { get; set; }
        public Series EstimatedSeries { get; set; }
    }
}
