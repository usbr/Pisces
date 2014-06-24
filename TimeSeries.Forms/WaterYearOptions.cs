using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public partial class WaterYearOptions : UserControl,IExplorerSettings
    {
        
       

        public WaterYearOptions()
        {
            InitializeComponent();
        }

        private int[] SelectedYears
        {
            get
            {

                return this.yearSelector1.SelectedYears;

            }
        }




        #region IExplorerSettingsView Members

        public void WriteToSettings(PiscesSettings settings)
        {
            settings.ThirtyYearAverage = this.checkBox30Year.Checked;
            settings.SelectedAnalysisType = AnalysisType.WaterYears;
            settings.WaterYears = this.SelectedYears;
            settings.BeginningMonth = yearTypeSelector1.BeginningMonth;
        }

        public void ReadFromSettings(PiscesSettings settings)
        {
            yearTypeSelector1.BeginningMonth = settings.BeginningMonth;
            checkBox30Year.Checked = settings.ThirtyYearAverage;
        }

        #endregion

        private void WaterYearOptions_Leave(object sender, EventArgs e)
        {

        }
    }
}
