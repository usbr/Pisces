using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public partial class TraceOptions : UserControl, IExplorerSettings
    {
        public TraceOptions()
        {
            InitializeComponent();
        }

        

        #region IExplorerSettingsView Members

        public void WriteToSettings(PiscesSettings settings)
        {
            settings.SelectedAnalysisType = AnalysisType.TraceAnalysis;
            settings.ExceedanceLevels = exceedanceLevelPicker1.ExceedanceLevels;
            settings.AlsoPlotTrace = this.checkBoxPlotTrace.Checked;
            settings.PlotTrace = this.TraceToPlot;
            settings.traceExceedanceAnalysis = this.traceExceedanceCheckBox.Checked;
            settings.traceAggregationAnalysis = this.traceAggregationCheckBox.Checked;
            settings.sumCYRadio = this.sumCYRadio.Checked;
            settings.sumWYRadio = this.sumWYRadio.Checked;
        }

        public void ReadFromSettings(PiscesSettings settings)
        {
            this.checkBoxPlotTrace.Checked = settings.AlsoPlotTrace;
            this.maskedTextBoxPlotTrace.Text = settings.PlotTrace.ToString();
            this.traceExceedanceCheckBox.Checked = settings.traceExceedanceAnalysis;
            this.traceAggregationCheckBox.Checked = settings.traceAggregationAnalysis;
            this.sumCYRadio.Checked = settings.sumCYRadio;
            this.sumWYRadio.Checked = settings.sumWYRadio;
        }

        #endregion

        

        private int TraceToPlot
        {
            get
            {
                int trc = 1;
                Int32.TryParse(maskedTextBoxPlotTrace.Text, out trc);
                return trc;
            }
        }

        private void checkBoxPlotYear_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxPlotTrace.Checked)
            { this.maskedTextBoxPlotTrace.Enabled = true; }
            else
            { this.maskedTextBoxPlotTrace.Enabled = false; }
        }

        private void traceAggregationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.traceAggregationCheckBox.Checked)
            {
                this.exceedanceAnalysisGroupBox.Enabled = false;
                this.aggregationAnalysisGroupBox.Enabled = true;
            }
        }

        private void traceExceedanceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.traceExceedanceCheckBox.Checked)
            {
                this.exceedanceAnalysisGroupBox.Enabled = true;
                this.aggregationAnalysisGroupBox.Enabled = false;
            }
        }
    }
}

