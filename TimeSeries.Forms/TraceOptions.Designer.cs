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
            Reclamation.Core.MonthDayRange monthDayRange1 = new Reclamation.Core.MonthDayRange();
            Reclamation.TimeSeries.TimeWindow timeWindow1 = new Reclamation.TimeSeries.TimeWindow();
            this.checkBoxPlotTrace = new System.Windows.Forms.CheckBox();
            this.groupBoxExtras = new System.Windows.Forms.GroupBox();
            this.comboBoxSelectedTrace = new System.Windows.Forms.ComboBox();
            this.checkBoxPlotMax = new System.Windows.Forms.CheckBox();
            this.checkBoxPlotAvg = new System.Windows.Forms.CheckBox();
            this.checkBoxPlotMin = new System.Windows.Forms.CheckBox();
            this.exceedanceAnalysisGroupBox = new System.Windows.Forms.GroupBox();
            this.aggregationAnalysisGroupBox = new System.Windows.Forms.GroupBox();
            this.sumRangeRadio = new System.Windows.Forms.RadioButton();
            this.sumWYRadio = new System.Windows.Forms.RadioButton();
            this.sumCYRadio = new System.Windows.Forms.RadioButton();
            this.traceExceedanceCheckBox = new System.Windows.Forms.RadioButton();
            this.traceAggregationCheckBox = new System.Windows.Forms.RadioButton();
            this.traceAnalysisSelection = new System.Windows.Forms.GroupBox();
            this.rangePicker1 = new Reclamation.TimeSeries.Forms.RangePicker();
            this.exceedanceLevelPicker1 = new Reclamation.TimeSeries.Forms.ExceedanceLevelPicker();
            this.timeWindowOptions1 = new Reclamation.TimeSeries.Forms.TimeWindowSelector();
            this.groupBoxExtras.SuspendLayout();
            this.exceedanceAnalysisGroupBox.SuspendLayout();
            this.aggregationAnalysisGroupBox.SuspendLayout();
            this.traceAnalysisSelection.SuspendLayout();
            this.SuspendLayout();
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
            this.groupBoxExtras.Controls.Add(this.comboBoxSelectedTrace);
            this.groupBoxExtras.Controls.Add(this.checkBoxPlotMax);
            this.groupBoxExtras.Controls.Add(this.checkBoxPlotAvg);
            this.groupBoxExtras.Controls.Add(this.checkBoxPlotMin);
            this.groupBoxExtras.Controls.Add(this.checkBoxPlotTrace);
            this.groupBoxExtras.Location = new System.Drawing.Point(127, 14);
            this.groupBoxExtras.Name = "groupBoxExtras";
            this.groupBoxExtras.Size = new System.Drawing.Size(90, 196);
            this.groupBoxExtras.TabIndex = 5;
            this.groupBoxExtras.TabStop = false;
            this.groupBoxExtras.Text = "additional lines";
            // 
            // comboBoxSelectedTrace
            // 
            this.comboBoxSelectedTrace.Enabled = false;
            this.comboBoxSelectedTrace.FormattingEnabled = true;
            this.comboBoxSelectedTrace.Location = new System.Drawing.Point(27, 37);
            this.comboBoxSelectedTrace.Name = "comboBoxSelectedTrace";
            this.comboBoxSelectedTrace.Size = new System.Drawing.Size(54, 21);
            this.comboBoxSelectedTrace.TabIndex = 20;
            this.comboBoxSelectedTrace.DropDown += new System.EventHandler(this.comboBoxSelectedTrace_DropDown);
            // 
            // checkBoxPlotMax
            // 
            this.checkBoxPlotMax.AutoSize = true;
            this.checkBoxPlotMax.Location = new System.Drawing.Point(11, 109);
            this.checkBoxPlotMax.Name = "checkBoxPlotMax";
            this.checkBoxPlotMax.Size = new System.Drawing.Size(65, 17);
            this.checkBoxPlotMax.TabIndex = 7;
            this.checkBoxPlotMax.Text = "plot max";
            this.checkBoxPlotMax.UseVisualStyleBackColor = true;
            // 
            // checkBoxPlotAvg
            // 
            this.checkBoxPlotAvg.AutoSize = true;
            this.checkBoxPlotAvg.Location = new System.Drawing.Point(11, 86);
            this.checkBoxPlotAvg.Name = "checkBoxPlotAvg";
            this.checkBoxPlotAvg.Size = new System.Drawing.Size(64, 17);
            this.checkBoxPlotAvg.TabIndex = 6;
            this.checkBoxPlotAvg.Text = "plot avg";
            this.checkBoxPlotAvg.UseVisualStyleBackColor = true;
            // 
            // checkBoxPlotMin
            // 
            this.checkBoxPlotMin.AutoSize = true;
            this.checkBoxPlotMin.Location = new System.Drawing.Point(11, 63);
            this.checkBoxPlotMin.Name = "checkBoxPlotMin";
            this.checkBoxPlotMin.Size = new System.Drawing.Size(62, 17);
            this.checkBoxPlotMin.TabIndex = 5;
            this.checkBoxPlotMin.Text = "plot min";
            this.checkBoxPlotMin.UseVisualStyleBackColor = true;
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
            this.aggregationAnalysisGroupBox.Controls.Add(this.rangePicker1);
            this.aggregationAnalysisGroupBox.Controls.Add(this.sumRangeRadio);
            this.aggregationAnalysisGroupBox.Controls.Add(this.sumWYRadio);
            this.aggregationAnalysisGroupBox.Controls.Add(this.sumCYRadio);
            this.aggregationAnalysisGroupBox.Enabled = false;
            this.aggregationAnalysisGroupBox.Location = new System.Drawing.Point(232, 122);
            this.aggregationAnalysisGroupBox.Name = "aggregationAnalysisGroupBox";
            this.aggregationAnalysisGroupBox.Size = new System.Drawing.Size(387, 217);
            this.aggregationAnalysisGroupBox.TabIndex = 16;
            this.aggregationAnalysisGroupBox.TabStop = false;
            this.aggregationAnalysisGroupBox.Text = "Aggregation Options";
            // 
            // sumRangeRadio
            // 
            this.sumRangeRadio.AutoSize = true;
            this.sumRangeRadio.Location = new System.Drawing.Point(10, 66);
            this.sumRangeRadio.Name = "sumRangeRadio";
            this.sumRangeRadio.Size = new System.Drawing.Size(125, 17);
            this.sumRangeRadio.TabIndex = 17;
            this.sumRangeRadio.Text = "sum by custom range";
            this.sumRangeRadio.UseVisualStyleBackColor = true;
            this.sumRangeRadio.CheckedChanged += new System.EventHandler(this.sumRangeRadio_CheckedChanged);
            // 
            // sumWYRadio
            // 
            this.sumWYRadio.AutoSize = true;
            this.sumWYRadio.Location = new System.Drawing.Point(10, 43);
            this.sumWYRadio.Name = "sumWYRadio";
            this.sumWYRadio.Size = new System.Drawing.Size(79, 17);
            this.sumWYRadio.TabIndex = 16;
            this.sumWYRadio.Text = "sum by WY";
            this.sumWYRadio.UseVisualStyleBackColor = true;
            // 
            // sumCYRadio
            // 
            this.sumCYRadio.AutoSize = true;
            this.sumCYRadio.Checked = true;
            this.sumCYRadio.Location = new System.Drawing.Point(10, 20);
            this.sumCYRadio.Name = "sumCYRadio";
            this.sumCYRadio.Size = new System.Drawing.Size(75, 17);
            this.sumCYRadio.TabIndex = 15;
            this.sumCYRadio.TabStop = true;
            this.sumCYRadio.Text = "sum by CY";
            this.sumCYRadio.UseVisualStyleBackColor = true;
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
            // rangePicker1
            // 
            this.rangePicker1.Enabled = false;
            this.rangePicker1.Location = new System.Drawing.Point(6, 89);
            this.rangePicker1.MonthDayRange = monthDayRange1;
            this.rangePicker1.Name = "rangePicker1";
            this.rangePicker1.Size = new System.Drawing.Size(371, 121);
            this.rangePicker1.TabIndex = 20;
            this.rangePicker1.Tag = "1";
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
        private System.Windows.Forms.CheckBox checkBoxPlotMax;
        private System.Windows.Forms.CheckBox checkBoxPlotAvg;
        private System.Windows.Forms.CheckBox checkBoxPlotMin;
        private RangePicker rangePicker1;
        private System.Windows.Forms.RadioButton sumRangeRadio;
        private System.Windows.Forms.ComboBox comboBoxSelectedTrace;
    }
}
