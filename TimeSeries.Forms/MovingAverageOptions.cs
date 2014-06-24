using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public partial class MovingAverageOptions : UserControl, IExplorerSettings 
    {
        public MovingAverageOptions()
        {
            
            InitializeComponent();
        }

        public void WriteToSettings(PiscesSettings settings)
        {
            settings.SelectedAnalysisType = AnalysisType.MovingAverage;
            settings.TimeWindow = timeWindowOptions1.TimeWindow;
            settings.PlotMoving120HourAverage = checkBox120hr.Checked;
            settings.PlotMoving24HourAverage = checkBox24hr.Checked;
            settings.PlotRaw =checkBoxRawData.Checked;
        }

        public void ReadFromSettings(PiscesSettings settings)
        {
            timeWindowOptions1.TimeWindow = settings.TimeWindow;
            //this.timeWindowOptions1.AllowFullPeriodOfRecord = settings.AllowFullPeriodOfRecord;
            //this.timeWindowOptions1.Visible = !settings.HasTraces;
            this.checkBox120hr.Checked = settings.PlotMoving120HourAverage;
            this.checkBox24hr.Checked = settings.PlotMoving24HourAverage;
            this.checkBoxRawData.Checked = settings.PlotRaw;
        }

    }
}
