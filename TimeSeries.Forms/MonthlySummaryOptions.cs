using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public partial class MonthlySummaryOptions : Reclamation.TimeSeries.Forms.TimeSeriesOptions
    {
        public MonthlySummaryOptions()
        {
            InitializeComponent();
            analysisType = AnalysisType.MonthlySummary;
            groupBoxAdvanced.Visible = false;
        }


        public override void ReadFromSettings(PiscesSettings settings)
        {
            base.ReadFromSettings(settings);
            radioButtonMultiYear.Checked = settings.MultiYearMonthlyAggregate;
            statisiticalMethodOptions1.StatisticalMethods = settings.StatisticalMethods;


        }

        public override void WriteToSettings(PiscesSettings settings)
        {
            base.WriteToSettings(settings);
            settings.StatisticalMethods = statisiticalMethodOptions1.StatisticalMethods;
            settings.MultiYearMonthlyAggregate = radioButtonMultiYear.Checked;
        }
    }
}

