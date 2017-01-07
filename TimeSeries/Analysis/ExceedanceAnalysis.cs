using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Analysis
{
    public class ExceedanceAnalysis : BaseAnalysis
    {
        public ExceedanceAnalysis(PiscesEngine explorer)
            : base(explorer)
        {
            m_analysisType = AnalysisType.Exceedance;
            m_name = "Sorted Data - Exceedance";
            Description = "Selected items are sorted, ranked "
                             + "\nand plotted using the Weibul ranking i=(1,2,3 ... n)"
                            + "\npercent = i/(n+1)";
        }

        public override IExplorerView Run()
        {
            SortAndRank(RankType.Weibul);
            return view;
        }
    }
}
