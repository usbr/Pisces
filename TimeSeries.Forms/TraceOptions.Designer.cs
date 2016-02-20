namespace Reclamation.TimeSeries.Forms
{
    partial class TraceOptions
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
            Reclamation.TimeSeries.TimeWindow timeWindow1 = new Reclamation.TimeSeries.TimeWindow();
            this.maskedTextBoxPlotTrace = new System.Windows.Forms.MaskedTextBox();
            this.checkBoxPlotTrace = new System.Windows.Forms.CheckBox();
            this.groupBoxExtras = new System.Windows.Forms.GroupBox();
            this.exceedanceAnalysisGroupBox = new System.Windows.Forms.GroupBox();
            this.aggregationAnalysisGroupBox = new System.Windows.Forms.GroupBox();
            this.sumCYRadio = new System.Windows.Forms.RadioButton();
            this.sumWYRadio = new System.Windows.Forms.RadioButton();
            this.traceExceedanceCheckBox = new System.Windows.Forms.RadioButton();
            this.traceAggregationCheckBox = new System.Windows.Forms.RadioButton();
            this.traceAnalysisSelection = new System.Windows.Forms.GroupBox();
            this.exceedanceLevelPicker1 = new Reclamation.TimeSeries.Forms.ExceedanceLevelPicker();
            this.timeWindowOptions1 = new Reclamation.TimeSeries.Forms.TimeWindowSelector();
            this.groupBoxExtras.SuspendLayout();
            this.exceedanceAnalysisGroupBox.SuspendLayout();
            this.aggregationAnalysisGroupBox.SuspendLayout();
            this.traceAnalysisSelection.SuspendLayout();
            this.SuspendLayout();
            // 
            // maskedTextBoxPlotTrace
            // 
            this.maskedTextBoxPlotTrace.Enabled = false;
            this.maskedTextBoxPlotTrace.Location = new System.Drawing.Point(30, 37);
            this.maskedTextBoxPlotTrace.Name = "maskedTextBoxPlotTrace";
            this.maskedTextBoxPlotTrace.Size = new System.Drawing.Size(54, 20);
            this.maskedTextBoxPlotTrace.TabIndex = 3;
            this.maskedTextBoxPlotTrace.Text = "1";
            this.maskedTextBoxPlotTrace.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // checkBoxPlotTrace
            // 
            this.checkBoxPlotTrace.AutoSize = true;
            this.checkBoxPlotTrace.Location = new System.Drawing.Point(11, 19);
            this.checkBoxPlotTrace.Name = "checkBoxPlotTrace";
            this.checkBoxPlotTrace.Size = new System.Drawing.Size(70, 17);
            this.checkBoxPlotTrace.TabIndex = 4;
            this.checkBoxPlotTrace.Text = "plot trace";
            this.checkBoxPlotTrace.UseVisualStyleBackColor = true;
            this.checkBoxPlotTrace.CheckedChanged += new System.EventHandler(this.checkBoxPlotYear_CheckedChanged);
            // 
            // groupBoxExtras
            // 
            this.groupBoxExtras.Controls.Add(this.checkBoxPlotTrace);
            this.groupBoxExtras.Controls.Add(this.maskedTextBoxPlotTrace);
            this.groupBoxExtras.Location = new System.Drawing.Point(127, 14);
            this.groupBoxExtras.Name = "groupBoxExtras";
            this.groupBoxExtras.Size = new System.Drawing.Size(90, 196);
            this.groupBoxExtras.TabIndex = 5;
            this.groupBoxExtras.TabStop = false;
            this.groupBoxExtras.Text = "additional lines";
            // 
            // exceedanceAnalysisGroupBox
            // 
            this.exceedanceAnalysisGroupBox.Controls.Add(this.groupBoxExtras);
            this.exceedanceAnalysisGroupBox.Controls.Add(this.exceedanceLevelPicker1);
            this.exceedanceAnalysisGroupBox.Location = new System.Drawing.Point(3, 122);
            this.exceedanceAnalysisGroupBox.Name = "exceedanceAnalysisGroupBox";
            this.exceedanceAnalysisGroupBox.Size = new System.Drawing.Size(223, 217);
            this.exceedanceAnalysisGroupBox.TabIndex = 15;
            this.exceedanceAnalysisGroupBox.TabStop = false;
            this.exceedanceAnalysisGroupBox.Text = "Exceedance Analysis Options";
            // 
            // aggregationAnalysisGroupBox
            // 
            this.aggregationAnalysisGroupBox.Controls.Add(this.sumWYRadio);
            this.aggregationAnalysisGroupBox.Controls.Add(this.sumCYRadio);
            this.aggregationAnalysisGroupBox.Enabled = false;
            this.aggregationAnalysisGroupBox.Location = new System.Drawing.Point(232, 122);
            this.aggregationAnalysisGroupBox.Name = "aggregationAnalysisGroupBox";
            this.aggregationAnalysisGroupBox.Size = new System.Drawing.Size(141, 113);
            this.aggregationAnalysisGroupBox.TabIndex = 16;
            this.aggregationAnalysisGroupBox.TabStop = false;
            this.aggregationAnalysisGroupBox.Text = "Aggregation Options";
            // 
            // sumCYRadio
            // 
            this.sumCYRadio.AutoSize = true;
            this.sumCYRadio.Checked = true;
            this.sumCYRadio.Location = new System.Drawing.Point(10, 20);
            this.sumCYRadio.Name = "sumCYRadio";
            this.sumCYRadio.Size = new System.Drawing.Size(77, 17);
            this.sumCYRadio.TabIndex = 15;
            this.sumCYRadio.TabStop = true;
            this.sumCYRadio.Text = "Sum by CY";
            this.sumCYRadio.UseVisualStyleBackColor = true;
            // 
            // sumWYRadio
            // 
            this.sumWYRadio.AutoSize = true;
            this.sumWYRadio.Location = new System.Drawing.Point(10, 43);
            this.sumWYRadio.Name = "sumWYRadio";
            this.sumWYRadio.Size = new System.Drawing.Size(81, 17);
            this.sumWYRadio.TabIndex = 16;
            this.sumWYRadio.Text = "Sum by WY";
            this.sumWYRadio.UseVisualStyleBackColor = true;
            // 
            // traceExceedanceCheckBox
            // 
            this.traceExceedanceCheckBox.AutoSize = true;
            this.traceExceedanceCheckBox.Checked = true;
            this.traceExceedanceCheckBox.Location = new System.Drawing.Point(10, 19);
            this.traceExceedanceCheckBox.Name = "traceExceedanceCheckBox";
            this.traceExceedanceCheckBox.Size = new System.Drawing.Size(165, 17);
            this.traceExceedanceCheckBox.TabIndex = 17;
            this.traceExceedanceCheckBox.TabStop = true;
            this.traceExceedanceCheckBox.Text = "Perform Exceedance Analysis";
            this.traceExceedanceCheckBox.UseVisualStyleBackColor = true;
            this.traceExceedanceCheckBox.CheckedChanged += new System.EventHandler(this.traceExceedanceCheckBox_CheckedChanged);
            // 
            // traceAggregationCheckBox
            // 
            this.traceAggregationCheckBox.AutoSize = true;
            this.traceAggregationCheckBox.Location = new System.Drawing.Point(10, 42);
            this.traceAggregationCheckBox.Name = "traceAggregationCheckBox";
            this.traceAggregationCheckBox.Size = new System.Drawing.Size(162, 17);
            this.traceAggregationCheckBox.TabIndex = 18;
            this.traceAggregationCheckBox.Text = "Perform Aggregation Analysis";
            this.traceAggregationCheckBox.UseVisualStyleBackColor = true;
            this.traceAggregationCheckBox.CheckedChanged += new System.EventHandler(this.traceAggregationCheckBox_CheckedChanged);
            // 
            // traceAnalysisSelection
            // 
            this.traceAnalysisSelection.Controls.Add(this.traceAggregationCheckBox);
            this.traceAnalysisSelection.Controls.Add(this.traceExceedanceCheckBox);
            this.traceAnalysisSelection.Location = new System.Drawing.Point(3, 3);
            this.traceAnalysisSelection.Name = "traceAnalysisSelection";
            this.traceAnalysisSelection.Size = new System.Drawing.Size(257, 113);
            this.traceAnalysisSelection.TabIndex = 19;
            this.traceAnalysisSelection.TabStop = false;
            this.traceAnalysisSelection.Text = "Select Trace Analysis";
            // 
            // exceedanceLevelPicker1
            // 
            this.exceedanceLevelPicker1.ExceedanceLevels = new int[] {
        10,
        50,
        90};
            this.exceedanceLevelPicker1.Location = new System.Drawing.Point(7, 14);
            this.exceedanceLevelPicker1.Name = "exceedanceLevelPicker1";
            this.exceedanceLevelPicker1.Size = new System.Drawing.Size(114, 196);
            this.exceedanceLevelPicker1.TabIndex = 0;
            // 
            // timeWindowOptions1
            // 
            this.timeWindowOptions1.Location = new System.Drawing.Point(266, 3);
            this.timeWindowOptions1.Name = "timeWindowOptions1";
            this.timeWindowOptions1.Size = new System.Drawing.Size(275, 113);
            this.timeWindowOptions1.TabIndex = 11;
            timeWindow1.FromDateToTodayT1 = new System.DateTime(2010, 11, 16, 10, 55, 11, 261);
            timeWindow1.FromToDatesT1 = new System.DateTime(2010, 11, 16, 10, 55, 11, 261);
            timeWindow1.FromToDatesT2 = new System.DateTime(2010, 11, 16, 10, 55, 11, 261);
            timeWindow1.NumDaysFromToday = new decimal(new int[] {
            14,
            0,
            0,
            0});
            timeWindow1.WindowType = Reclamation.TimeSeries.TimeWindowType.FullPeriodOfRecord;
            this.timeWindowOptions1.TimeWindow = timeWindow1;
            // 
            // TraceOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.traceAnalysisSelection);
            this.Controls.Add(this.aggregationAnalysisGroupBox);
            this.Controls.Add(this.exceedanceAnalysisGroupBox);
            this.Controls.Add(this.timeWindowOptions1);
            this.Name = "TraceOptions";
            this.Size = new System.Drawing.Size(622, 440);
            this.groupBoxExtras.ResumeLayout(false);
            this.groupBoxExtras.PerformLayout();
            this.exceedanceAnalysisGroupBox.ResumeLayout(false);
            this.aggregationAnalysisGroupBox.ResumeLayout(false);
            this.aggregationAnalysisGroupBox.PerformLayout();
            this.traceAnalysisSelection.ResumeLayout(false);
            this.traceAnalysisSelection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExceedanceLevelPicker exceedanceLevelPicker1;
        private System.Windows.Forms.MaskedTextBox maskedTextBoxPlotTrace;
        private System.Windows.Forms.CheckBox checkBoxPlotTrace;
        private System.Windows.Forms.GroupBox groupBoxExtras;
        private TimeWindowSelector timeWindowOptions1;
        private System.Windows.Forms.GroupBox exceedanceAnalysisGroupBox;
        private System.Windows.Forms.GroupBox aggregationAnalysisGroupBox;
        private System.Windows.Forms.RadioButton sumWYRadio;
        private System.Windows.Forms.RadioButton sumCYRadio;
        private System.Windows.Forms.GroupBox traceAnalysisSelection;
        private System.Windows.Forms.RadioButton traceExceedanceCheckBox;
        private System.Windows.Forms.RadioButton traceAggregationCheckBox;
    }
}
