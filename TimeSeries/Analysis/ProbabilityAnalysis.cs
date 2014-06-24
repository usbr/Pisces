using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Analysis
{
    public class ProbabilityAnalysis : BaseAnalysis
    {
        public ProbabilityAnalysis(PiscesSettings explorer)
            : base(explorer)
        {
            m_analysisType = AnalysisType.Probability;
            m_name = "Sorted Data - Probability";
            Description = "Selected items are sorted, ranked "
                             + "\nand plotted using a simple ranking i=(1,2,3 ... n)"
                             +"\npercent = i/n";
        }

        public override IExplorerView Run()
        {
            SortAndRank(RankType.Proabability);
            return view;
        }

       
    }
}
