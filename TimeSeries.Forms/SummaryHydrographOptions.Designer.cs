namespace Reclamation.TimeSeries.Forms
{
    partial class SummaryHydrographOptions
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
            Reclamation.TimeSeries.TimeWindow timeWindow2 = new Reclamation.TimeSeries.TimeWindow();
            this.maskedTextBoxPlotYear = new System.Windows.Forms.MaskedTextBox();
            this.checkBoxPlotYear = new System.Windows.Forms.CheckBox();
            this.groupBoxExtras = new System.Windows.Forms.GroupBox();
            this.checkBoxAverage = new System.Windows.Forms.CheckBox();
            this.checkBoxMaximum = new System.Windows.Forms.CheckBox();
            this.checkBoxMinimum = new System.Windows.Forms.CheckBox();
            this.yearTypeSelector1 = new Reclamation.TimeSeries.Forms.YearTypeSelector();
            this.timeWindowOptions1 = new Reclamation.TimeSeries.Forms.TimeWindowSelector();
            this.exceedanceLevelPicker1 = new Reclamation.TimeSeries.Forms.ExceedanceLevelPicker();
            this.groupBoxExtras.SuspendLayout();
            this.SuspendLayout();
            // 
            // maskedTextBoxPlotYear
            // 
            this.maskedTextBoxPlotYear.Enabled = false;
            this.maskedTextBoxPlotYear.Location = new System.Drawing.Point(101, 17);
            this.maskedTextBoxPlotYear.Mask = "0000";
            this.maskedTextBoxPlotYear.Name = "maskedTextBoxPlotYear";
            this.maskedTextBoxPlotYear.Size = new System.Drawing.Size(46, 20);
            this.maskedTextBoxPlotYear.TabIndex = 3;
            this.maskedTextBoxPlotYear.Text = "2006";
            // 
            // checkBoxPlotYear
            // 
            this.checkBoxPlotYear.AutoSize = true;
            this.checkBoxPlotYear.Location = new System.Drawing.Point(11, 19);
            this.checkBoxPlotYear.Name = "checkBoxPlotYear";
            this.checkBoxPlotYear.Size = new System.Drawing.Size(84, 17);
            this.checkBoxPlotYear.TabIndex = 4;
            this.checkBoxPlotYear.Text = "plot the year";
            this.checkBoxPlotYear.UseVisualStyleBackColor = true;
            this.checkBoxPlotYear.CheckedChanged += new System.EventHandler(this.checkBoxPlotYear_CheckedChanged);
            // 
            // groupBoxExtras
            // 
            this.groupBoxExtras.Controls.Add(this.checkBoxAverage);
            this.groupBoxExtras.Controls.Add(this.checkBoxMaximum);
            this.groupBoxExtras.Controls.Add(this.checkBoxPlotYear);
            this.groupBoxExtras.Controls.Add(this.maskedTextBoxPlotYear);
            this.groupBoxExtras.Controls.Add(this.checkBoxMinimum);
            this.groupBoxExtras.Location = new System.Drawing.Point(171, 130);
            this.groupBoxExtras.Name = "groupBoxExtras";
            this.groupBoxExtras.Size = new System.Drawing.Size(173, 115);
            this.groupBoxExtras.TabIndex = 5;
            this.groupBoxExtras.TabStop = false;
            this.groupBoxExtras.Text = "additional lines";
            // 
            // checkBoxAverage
            // 
            this.checkBoxAverage.AutoSize = true;
            this.checkBoxAverage.Location = new System.Drawing.Point(11, 88);
            this.checkBoxAverage.Name = "checkBoxAverage";
            this.checkBoxAverage.Size = new System.Drawing.Size(85, 17);
            this.checkBoxAverage.TabIndex = 7;
            this.checkBoxAverage.Text = "plot average";
            this.checkBoxAverage.UseVisualStyleBackColor = true;
            // 
            // checkBoxMaximum
            // 
            this.checkBoxMaximum.AutoSize = true;
            this.checkBoxMaximum.Location = new System.Drawing.Point(11, 42);
            this.checkBoxMaximum.Name = "checkBoxMaximum";
            this.checkBoxMaximum.Size = new System.Drawing.Size(89, 17);
            this.checkBoxMaximum.TabIndex = 6;
            this.checkBoxMaximum.Text = "plot maximum";
            this.checkBoxMaximum.UseVisualStyleBackColor = true;
            // 
            // checkBoxMinimum
            // 
            this.checkBoxMinimum.AutoSize = true;
            this.checkBoxMinimum.Location = new System.Drawing.Point(11, 65);
            this.checkBoxMinimum.Name = "checkBoxMinimum";
            this.checkBoxMinimum.Size = new System.Drawing.Size(86, 17);
            this.checkBoxMinimum.TabIndex = 5;
            this.checkBoxMinimum.Text = "plot minimum";
            this.checkBoxMinimum.UseVisualStyleBackColor = true;
            // 
            // yearTypeSelector1
            // 
            this.yearTypeSelector1.BeginningMonth = 10;
            this.yearTypeSelector1.Location = new System.Drawing.Point(171, 251);
            this.yearTypeSelector1.Name = "yearTypeSelector1";
            this.yearTypeSelector1.Size = new System.Drawing.Size(295, 100);
            this.yearTypeSelector1.TabIndex = 12;
            // 
            // timeWindowOptions1
            // 
            this.timeWindowOptions1.Location = new System.Drawing.Point(13, 11);
            this.timeWindowOptions1.Name = "timeWindowOptions1";
            this.timeWindowOptions1.Size = new System.Drawing.Size(275, 113);
            this.timeWindowOptions1.TabIndex = 11;
            timeWindow2.FromDateToTodayT1 = new System.DateTime(2010, 11, 16, 10, 55, 11, 261);
            timeWindow2.FromToDatesT1 = new System.DateTime(2010, 11, 16, 10, 55, 11, 261);
            timeWindow2.FromToDatesT2 = new System.DateTime(2010, 11, 16, 10, 55, 11, 261);
            timeWindow2.NumDaysFromToday = new decimal(new int[] {
            14,
            0,
            0,
            0});
            timeWindow2.WindowType = Reclamation.TimeSeries.TimeWindowType.FullPeriodOfRecord;
            this.timeWindowOptions1.TimeWindow = timeWindow2;
            // 
            // exceedanceLevelPicker1
            // 
            this.exceedanceLevelPicker1.ExceedanceLevels = new int[] {
        10,
        50,
        90};
            this.exceedanceLevelPicker1.Location = new System.Drawing.Point(13, 130);
            this.exceedanceLevelPicker1.Name = "exceedanceLevelPicker1";
            this.exceedanceLevelPicker1.Size = new System.Drawing.Size(140, 203);
            this.exceedanceLevelPicker1.TabIndex = 0;
            // 
            // SummaryHydrographOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.yearTypeSelector1);
            this.Controls.Add(this.timeWindowOptions1);
            this.Controls.Add(this.groupBoxExtras);
            this.Controls.Add(this.exceedanceLevelPicker1);
            this.Name = "SummaryHydrographOptions";
            this.Size = new System.Drawing.Size(504, 388);
            this.groupBoxExtras.ResumeLayout(false);
            this.groupBoxExtras.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExceedanceLevelPicker exceedanceLevelPicker1;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxPlotYear;
        private System.Windows.Forms.CheckBox checkBoxPlotYear;
        private System.Windows.Forms.GroupBox groupBoxExtras;
        private System.Windows.Forms.CheckBox checkBoxMaximum;
        private System.Windows.Forms.CheckBox checkBoxMinimum;
        private TimeWindowSelector timeWindowOptions1;
        private YearTypeSelector yearTypeSelector1;
        private System.Windows.Forms.CheckBox checkBoxAverage;
    }
}
