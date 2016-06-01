namespace Reclamation.TimeSeries.Forms
{
    partial class TimeWindowSelector
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
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.radioButtonFromToDates = new System.Windows.Forms.RadioButton();
            this.radioButtonFullPeriodOfRecord = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker3 = new System.Windows.Forms.DateTimePicker();
            this.radioButtonNumDaysFromToday = new System.Windows.Forms.RadioButton();
            this.radioButtonFromDateToToday = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(147, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "to";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker2.Location = new System.Drawing.Point(169, 33);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(90, 20);
            this.dateTimePicker2.TabIndex = 12;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(53, 33);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(90, 20);
            this.dateTimePicker1.TabIndex = 11;
            // 
            // radioButtonFromToDates
            // 
            this.radioButtonFromToDates.AutoSize = true;
            this.radioButtonFromToDates.Location = new System.Drawing.Point(6, 36);
            this.radioButtonFromToDates.Name = "radioButtonFromToDates";
            this.radioButtonFromToDates.Size = new System.Drawing.Size(45, 17);
            this.radioButtonFromToDates.TabIndex = 15;
            this.radioButtonFromToDates.Text = "from";
            this.radioButtonFromToDates.UseVisualStyleBackColor = true;
            this.radioButtonFromToDates.CheckedChanged += new System.EventHandler(this.EnableDates);
            // 
            // radioButtonFullPeriodOfRecord
            // 
            this.radioButtonFullPeriodOfRecord.AutoSize = true;
            this.radioButtonFullPeriodOfRecord.Checked = true;
            this.radioButtonFullPeriodOfRecord.Location = new System.Drawing.Point(6, 15);
            this.radioButtonFullPeriodOfRecord.Name = "radioButtonFullPeriodOfRecord";
            this.radioButtonFullPeriodOfRecord.Size = new System.Drawing.Size(152, 17);
            this.radioButtonFullPeriodOfRecord.TabIndex = 16;
            this.radioButtonFullPeriodOfRecord.TabStop = true;
            this.radioButtonFullPeriodOfRecord.Text = "include full period of record";
            this.radioButtonFullPeriodOfRecord.UseVisualStyleBackColor = true;
            this.radioButtonFullPeriodOfRecord.CheckedChanged += new System.EventHandler(this.EnableDates);
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.buttonClose);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.dateTimePicker3);
            this.groupBox1.Controls.Add(this.radioButtonNumDaysFromToday);
            this.groupBox1.Controls.Add(this.radioButtonFromDateToToday);
            this.groupBox1.Controls.Add(this.radioButtonFullPeriodOfRecord);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.radioButtonFromToDates);
            this.groupBox1.Controls.Add(this.dateTimePicker2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(275, 110);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "time window options";
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(197, 81);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(62, 23);
            this.buttonClose.TabIndex = 23;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(104, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "days to today";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(53, 83);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(45, 20);
            this.numericUpDown1.TabIndex = 21;
            this.numericUpDown1.Value = new decimal(new int[] {
            14,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(147, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "to today";
            // 
            // dateTimePicker3
            // 
            this.dateTimePicker3.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker3.Location = new System.Drawing.Point(53, 59);
            this.dateTimePicker3.Name = "dateTimePicker3";
            this.dateTimePicker3.Size = new System.Drawing.Size(90, 20);
            this.dateTimePicker3.TabIndex = 19;
            // 
            // radioButtonNumDaysFromToday
            // 
            this.radioButtonNumDaysFromToday.AutoSize = true;
            this.radioButtonNumDaysFromToday.Location = new System.Drawing.Point(6, 86);
            this.radioButtonNumDaysFromToday.Name = "radioButtonNumDaysFromToday";
            this.radioButtonNumDaysFromToday.Size = new System.Drawing.Size(41, 17);
            this.radioButtonNumDaysFromToday.TabIndex = 18;
            this.radioButtonNumDaysFromToday.TabStop = true;
            this.radioButtonNumDaysFromToday.Text = "last";
            this.radioButtonNumDaysFromToday.UseVisualStyleBackColor = true;
            this.radioButtonNumDaysFromToday.CheckedChanged += new System.EventHandler(this.EnableDates);
            // 
            // radioButtonFromDateToToday
            // 
            this.radioButtonFromDateToToday.AutoSize = true;
            this.radioButtonFromDateToToday.Location = new System.Drawing.Point(6, 61);
            this.radioButtonFromDateToToday.Name = "radioButtonFromDateToToday";
            this.radioButtonFromDateToToday.Size = new System.Drawing.Size(45, 17);
            this.radioButtonFromDateToToday.TabIndex = 17;
            this.radioButtonFromDateToToday.TabStop = true;
            this.radioButtonFromDateToToday.Text = "from";
            this.radioButtonFromDateToToday.UseVisualStyleBackColor = true;
            this.radioButtonFromDateToToday.CheckedChanged += new System.EventHandler(this.EnableDates);
            // 
            // TimeWindowSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "TimeWindowSelector";
            this.Size = new System.Drawing.Size(275, 110);
            this.Load += new System.EventHandler(this.TimeWindowOptions_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.RadioButton radioButtonFromToDates;
        private System.Windows.Forms.RadioButton radioButtonFullPeriodOfRecord;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonNumDaysFromToday;
        private System.Windows.Forms.RadioButton radioButtonFromDateToToday;
        private System.Windows.Forms.DateTimePicker dateTimePicker3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonClose;
    }
}
