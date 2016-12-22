namespace Reclamation.TimeSeries.Forms
{
    partial class RangePicker
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
            Reclamation.Core.MonthDayRange monthDayRange2 = new Reclamation.Core.MonthDayRange();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonMonthDay = new System.Windows.Forms.RadioButton();
            this.radioButtonMonths = new System.Windows.Forms.RadioButton();
            this.monthDayRangePicker1 = new Reclamation.TimeSeries.Forms.MonthDayRangePicker();
            this.monthRangePicker1 = new Reclamation.TimeSeries.Forms.MonthRangePicker();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.radioButtonMonthDay);
            this.groupBox1.Controls.Add(this.radioButtonMonths);
            this.groupBox1.Controls.Add(this.monthDayRangePicker1);
            this.groupBox1.Controls.Add(this.monthRangePicker1);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(420, 127);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "range";
            // 
            // radioButtonMonthDay
            // 
            this.radioButtonMonthDay.AutoSize = true;
            this.radioButtonMonthDay.Checked = true;
            this.radioButtonMonthDay.Location = new System.Drawing.Point(20, 77);
            this.radioButtonMonthDay.Name = "radioButtonMonthDay";
            this.radioButtonMonthDay.Size = new System.Drawing.Size(14, 13);
            this.radioButtonMonthDay.TabIndex = 14;
            this.radioButtonMonthDay.TabStop = true;
            this.radioButtonMonthDay.UseVisualStyleBackColor = true;
            this.radioButtonMonthDay.CheckedChanged += new System.EventHandler(this.Enabling);
            // 
            // radioButtonMonths
            // 
            this.radioButtonMonths.AutoSize = true;
            this.radioButtonMonths.Location = new System.Drawing.Point(20, 30);
            this.radioButtonMonths.Name = "radioButtonMonths";
            this.radioButtonMonths.Size = new System.Drawing.Size(14, 13);
            this.radioButtonMonths.TabIndex = 13;
            this.radioButtonMonths.UseVisualStyleBackColor = true;
            this.radioButtonMonths.CheckedChanged += new System.EventHandler(this.Enabling);
            // 
            // monthDayRangePicker1
            // 
            this.monthDayRangePicker1.BeginningMonth = 10;
            this.monthDayRangePicker1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.monthDayRangePicker1.Location = new System.Drawing.Point(43, 58);
            this.monthDayRangePicker1.MonthDayRange = monthDayRange1;
            this.monthDayRangePicker1.Name = "monthDayRangePicker1";
            this.monthDayRangePicker1.Size = new System.Drawing.Size(159, 50);
            this.monthDayRangePicker1.TabIndex = 12;
            // 
            // monthRangePicker1
            // 
            this.monthRangePicker1.AutoSize = true;
            this.monthRangePicker1.BeginningMonth = 10;
            this.monthRangePicker1.Location = new System.Drawing.Point(43, 24);
            this.monthRangePicker1.MonthDayRange = monthDayRange2;
            this.monthRangePicker1.Name = "monthRangePicker1";
            this.monthRangePicker1.Size = new System.Drawing.Size(341, 35);
            this.monthRangePicker1.TabIndex = 15;
            // 
            // RangePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "RangePicker";
            this.Size = new System.Drawing.Size(386, 125);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private MonthRangePicker monthRangePicker1;
        private System.Windows.Forms.RadioButton radioButtonMonthDay;
        private System.Windows.Forms.RadioButton radioButtonMonths;
        private MonthDayRangePicker monthDayRangePicker1;
    }
}
