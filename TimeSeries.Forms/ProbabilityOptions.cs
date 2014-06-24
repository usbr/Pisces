using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public partial class ProbabilityOptions : Reclamation.TimeSeries.Forms.TimeSeriesOptions
    {
        public ProbabilityOptions()
        {
            base.analysisType = AnalysisType.Probability;
            InitializeComponent();
        }
    }
}

