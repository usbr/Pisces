using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Forms
{
    public partial class TimeSeriesOptions : UserControl, IExplorerSettings 
    {
        protected AnalysisType analysisType;
        public TimeSeriesOptions()
        {
             analysisType = AnalysisType.TimeSeries;
            InitializeComponent();
            this.aggregateOptions1.AggregateTypeChanged += new EventHandler<EventArgs>(aggregateOptions1_AggregateTypeChanged);
            
           
        }

        void aggregateOptions1_AggregateTypeChanged(object sender, EventArgs e)
        {
            Enabling();
        }

        public virtual void WriteToSettings(PiscesSettings settings)
        {
            settings.SelectedAnalysisType = analysisType;
            settings.TimeWindow = timeWindowOptions1.TimeWindow;
            settings.MonthDayRange = this.rangePicker1.MonthDayRange;
            settings.StatisticalMethods = aggregateOptions1.StatisticalMethods;
            settings.BeginningMonth = yearTypeSelector1.BeginningMonth;

        }
        public virtual void ReadFromSettings(PiscesSettings settings)
        {
            timeWindowOptions1.TimeWindow = settings.TimeWindow;
            //this.timeWindowOptions1.AllowFullPeriodOfRecord = settings.AllowFullPeriodOfRecord;
            //this.timeWindowOptions1.Visible = !settings.HasTraces;
            rangePicker1.BeginningMonth = settings.BeginningMonth;
            rangePicker1.MonthDayRange = settings.MonthDayRange;
            aggregateOptions1.StatisticalMethods = settings.StatisticalMethods;
            yearTypeSelector1.BeginningMonth = settings.BeginningMonth;
            Enabling();

        }

        /// <summary>
        /// enable relevant interface components 
        /// </summary>
        private void Enabling()
        {
            if (aggregateOptions1.StatisticalMethods == StatisticalMethods.None
                && analysisType == AnalysisType.TimeSeries)
            {
                this.rangePicker1.Enabled = false;
                this.yearTypeSelector1.Enabled = false;
            }
            else
            {
                this.rangePicker1.Enabled = true;
                this.yearTypeSelector1.Enabled = true;
            }

        }

        private void yearTypeSelector1_BeginningMonthChanged(object sender, EventArgs e)
        {
            this.rangePicker1.BeginningMonth = yearTypeSelector1.BeginningMonth;
        }
	
    }
}
