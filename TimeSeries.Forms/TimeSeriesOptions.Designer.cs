namespace Reclamation.TimeSeries.Forms
{
    partial class TimeSeriesOptions
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Reclamation.Core.MonthDayRange monthDayRange1 = new Reclamation.Core.MonthDayRange();
            this.groupBoxAdvanced = new System.Windows.Forms.GroupBox();
            this.rangePicker1 = new Reclamation.TimeSeries.Forms.RangePicker();
            this.yearTypeSelector1 = new Reclamation.TimeSeries.Forms.YearTypeSelector();
            this.aggregateOptions1 = new Reclamation.TimeSeries.Forms.SummaryOption();
            this.timeWindowOptions1 = new Reclamation.TimeSeries.Forms.TimeWindowSelector();
            this.groupBoxAdvanced.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxAdvanced
            // 
            this.groupBoxAdvanced.Controls.Add(this.rangePicker1);
            this.groupBoxAdvanced.Controls.Add(this.yearTypeSelector1);
            this.groupBoxAdvanced.Controls.Add(this.aggregateOptions1);
            this.groupBoxAdvanced.Location = new System.Drawing.Point(12, 140);
            this.groupBoxAdvanced.Name = "groupBoxAdvanced";
            this.groupBoxAdvanced.Size = new System.Drawing.Size(586, 267);
            this.groupBoxAdvanced.TabIndex = 11;
            this.groupBoxAdvanced.TabStop = false;
            this.groupBoxAdvanced.Text = "advanced";
            // 
            // rangePicker1
            // 
            this.rangePicker1.Location = new System.Drawing.Point(177, 135);
            this.rangePicker1.MonthDayRange = monthDayRange1;
            this.rangePicker1.Name = "rangePicker1";
            this.rangePicker1.Size = new System.Drawing.Size(396, 113);
            this.rangePicker1.TabIndex = 12;
            // 
            // yearTypeSelector1
            // 
            this.yearTypeSelector1.BeginningMonth = 10;
            this.yearTypeSelector1.Location = new System.Drawing.Point(177, 19);
            this.yearTypeSelector1.Name = "yearTypeSelector1";
            this.yearTypeSelector1.Size = new System.Drawing.Size(358, 100);
            this.yearTypeSelector1.TabIndex = 11;
            this.yearTypeSelector1.BeginningMonthChanged += new System.EventHandler<System.EventArgs>(this.yearTypeSelector1_BeginningMonthChanged);
            // 
            // aggregateOptions1
            // 
            this.aggregateOptions1.Location = new System.Drawing.Point(18, 19);
            this.aggregateOptions1.Name = "aggregateOptions1";
            this.aggregateOptions1.Size = new System.Drawing.Size(129, 145);
            this.aggregateOptions1.StatisticalMethods = Reclamation.TimeSeries.StatisticalMethods.None;
            this.aggregateOptions1.TabIndex = 9;
            // 
            // periodOfRecordOptions1
            // 
            this.timeWindowOptions1.Location = new System.Drawing.Point(12, 13);
            this.timeWindowOptions1.Name = "timeWindowOptions1";
            this.timeWindowOptions1.Size = new System.Drawing.Size(275, 111);
            this.timeWindowOptions1.TabIndex = 10;
            // 
            // TimeSeriesOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxAdvanced);
            this.Controls.Add(this.timeWindowOptions1);
            this.Name = "TimeSeriesOptions";
            this.Size = new System.Drawing.Size(600, 410);
            this.groupBoxAdvanced.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SummaryOption aggregateOptions1;
        private TimeWindowSelector timeWindowOptions1;
        protected System.Windows.Forms.GroupBox groupBoxAdvanced;
        private YearTypeSelector yearTypeSelector1;
        private RangePicker rangePicker1;
    }
}
