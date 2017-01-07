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
        private TimeSeriesDatabaseDataSet.ScenarioDataTable scenarioTable;

        public TraceOptions()
        {
            InitializeComponent();
        }

        

        #region IExplorerSettingsView Members

        public void WriteToSettings(PiscesEngine settings)
        {
            settings.SelectedAnalysisType = AnalysisType.TraceAnalysis;
            settings.ExceedanceLevels = exceedanceLevelPicker1.ExceedanceLevels;
            settings.AlsoPlotTrace = this.checkBoxPlotTrace.Checked;
            try // stupid hack... needs some better logic here. problem is that Pisces crashes when the combobox is left blank
            { settings.PlotTrace = this.comboBoxSelectedTrace.SelectedItem.ToString(); }
            catch
            { settings.PlotTrace = "Run0"; }
            settings.traceExceedanceAnalysis = this.traceExceedanceCheckBox.Checked;
            settings.traceAggregationAnalysis = this.traceAggregationCheckBox.Checked;
            settings.sumCYRadio = this.sumCYRadio.Checked;
            settings.sumWYRadio = this.sumWYRadio.Checked;
            settings.sumCustomRangeRadio = this.sumRangeRadio.Checked;
            settings.PlotMinTrace = this.checkBoxPlotMin.Checked;
            settings.PlotAvgTrace = this.checkBoxPlotAvg.Checked;
            settings.PlotMaxTrace = this.checkBoxPlotMax.Checked;
            settings.TimeWindow = timeWindowOptions1.TimeWindow;
            settings.MonthDayRange = this.rangePicker1.MonthDayRange;
        }

        public void ReadFromSettings(PiscesEngine settings)
        {
            this.checkBoxPlotTrace.Checked = settings.AlsoPlotTrace;
            this.comboBoxSelectedTrace.Text = settings.PlotTrace.ToString();
            this.traceExceedanceCheckBox.Checked = settings.traceExceedanceAnalysis;
            this.traceAggregationCheckBox.Checked = settings.traceAggregationAnalysis;
            this.sumCYRadio.Checked = settings.sumCYRadio;
            this.sumWYRadio.Checked = settings.sumWYRadio;
            this.sumRangeRadio.Checked = settings.sumCustomRangeRadio;
            this.checkBoxPlotMin.Checked = settings.PlotMinTrace;
            this.checkBoxPlotAvg.Checked = settings.PlotAvgTrace;
            this.checkBoxPlotMax.Checked = settings.PlotMaxTrace;
            this.timeWindowOptions1.TimeWindow = settings.TimeWindow; 
            this.rangePicker1.BeginningMonth = settings.BeginningMonth;
            this.rangePicker1.MonthDayRange = settings.MonthDayRange;
            this.scenarioTable = settings.Database.GetSelectedScenarios();
        }

        #endregion
          
        private void checkBoxPlotYear_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxPlotTrace.Checked)
            {
                this.comboBoxSelectedTrace.Enabled = true;
            }
            else
            { this.comboBoxSelectedTrace.Enabled = false; }
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

        private void sumRangeRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (this.sumRangeRadio.Checked)
            {
                this.rangePicker1.Enabled = true;
            }
            else
            {
                this.rangePicker1.Enabled = false;
            }
        }

        private void LoadTraceList()
        {
            this.comboBoxSelectedTrace.Items.Clear();
            if (!(scenarioTable.Rows.Count < 10))
            {
                int dropdownWidth = 54;
                foreach (var s in scenarioTable)
                {

                    this.comboBoxSelectedTrace.Items.Add(s["Name"].ToString());
                    var nameLength = TextRenderer.MeasureText(s.Name, this.comboBoxSelectedTrace.Font).Width;
                    if (nameLength > dropdownWidth)
                    { dropdownWidth = nameLength; }

                }
                this.comboBoxSelectedTrace.DropDownWidth = dropdownWidth;

            }
            else
            { this.comboBoxSelectedTrace.Items.Add("Run0"); }
            this.comboBoxSelectedTrace.SelectedIndex = 0;
        }

        private void comboBoxSelectedTrace_DropDown(object sender, EventArgs e)
        {
            LoadTraceList();
        }
    }
}

