using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public partial class CorrelationOptions : Reclamation.TimeSeries.Forms.TimeSeriesOptions
    {
        public CorrelationOptions()
        {
            InitializeComponent();
            analysisType = AnalysisType.Correlation;
        }
    }
}

