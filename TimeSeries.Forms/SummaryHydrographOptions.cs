using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public partial class SummaryHydrographOptions : UserControl, IExplorerSettings
    {
        public SummaryHydrographOptions()
        {
            InitializeComponent();
        }
       
        private int[] YearToPlot
        {
            get
            {
                var years = maskedTextBoxPlotYear.Text.Split(',');
                var yrs = new List<int>();
                foreach (var year in years)
                {
                    int yr = 2000;
                    Int32.TryParse(year, out yr);
                    yrs.Add(yr);
                }
                return yrs.ToArray();
            }
        }
        private void checkBoxPlotYear_CheckedChanged(object sender, EventArgs e)
        {
            if (this.checkBoxPlotYear.Checked)
            {
                this.maskedTextBoxPlotYear.Enabled = true;
            }
            else
            {
                this.maskedTextBoxPlotYear.Enabled = false;
            }
        }

        #region IExplorerSettingsView Members

        public void WriteToSettings(PiscesEngine settings)
        {
            settings.SelectedAnalysisType = AnalysisType.SummaryHydrograph;
            settings.TimeWindow = timeWindowOptions1.TimeWindow;

            settings.ExceedanceLevels = exceedanceLevelPicker1.ExceedanceLevels;
            settings.AlsoPlotYear = this.checkBoxPlotYear.Checked;
            settings.PlotYear = this.YearToPlot;

            settings.PlotMax = this.checkBoxMaximum.Checked;
            settings.PlotMin = this.checkBoxMinimum.Checked;
            settings.PlotAvg = this.checkBoxAverage.Checked;
            settings.BeginningMonth = this.yearTypeSelector1.BeginningMonth;
        }

        public void ReadFromSettings(PiscesEngine settings)
        {
            this.checkBoxAverage.Checked = settings.PlotAvg;
            this.checkBoxMaximum.Checked = settings.PlotMax;
            this.checkBoxMinimum.Checked = settings.PlotMin;
            this.checkBoxPlotYear.Checked = settings.AlsoPlotYear;
            var yearsText = "";
            foreach (var years in settings.PlotYear)
            {
                yearsText += years + ",";
            }
            yearsText = yearsText.Trim(',');
            this.maskedTextBoxPlotYear.Text = yearsText;
            timeWindowOptions1.TimeWindow = settings.TimeWindow;
            //this.timeWindowOptions1.AllowFullPeriodOfRecord = settings.AllowFullPeriodOfRecord;

            //this.exceedanceLevelPicker1.ExceedanceLevels = settings.ExceedanceLevels;
            this.yearTypeSelector1.BeginningMonth = settings.BeginningMonth;

        }

        #endregion
    }

}
