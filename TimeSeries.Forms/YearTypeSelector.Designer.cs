namespace Reclamation.TimeSeries.Forms
{
    partial class YearTypeSelector
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
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelDebug = new System.Windows.Forms.Label();
            this.labelEndingCustomMonth = new System.Windows.Forms.Label();
            this.radioButtonCustom = new System.Windows.Forms.RadioButton();
            this.radioButtonCalendarYear = new System.Windows.Forms.RadioButton();
            this.radioButtonWaterYear = new System.Windows.Forms.RadioButton();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"});
            this.comboBox1.Location = new System.Drawing.Point(103, 61);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(93, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSize = true;
            this.groupBox2.Controls.Add(this.labelDebug);
            this.groupBox2.Controls.Add(this.labelEndingCustomMonth);
            this.groupBox2.Controls.Add(this.radioButtonCustom);
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.radioButtonCalendarYear);
            this.groupBox2.Controls.Add(this.radioButtonWaterYear);
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(295, 113);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "what type of year?";
            // 
            // labelDebug
            // 
            this.labelDebug.AutoSize = true;
            this.labelDebug.Location = new System.Drawing.Point(65, 84);
            this.labelDebug.Name = "labelDebug";
            this.labelDebug.Size = new System.Drawing.Size(40, 13);
            this.labelDebug.TabIndex = 5;
            this.labelDebug.Text = "debug:";
            this.labelDebug.Visible = false;
            // 
            // labelEndingCustomMonth
            // 
            this.labelEndingCustomMonth.AutoSize = true;
            this.labelEndingCustomMonth.Location = new System.Drawing.Point(198, 65);
            this.labelEndingCustomMonth.Name = "labelEndingCustomMonth";
            this.labelEndingCustomMonth.Size = new System.Drawing.Size(70, 13);
            this.labelEndingCustomMonth.TabIndex = 4;
            this.labelEndingCustomMonth.Text = " - September)";
            // 
            // radioButtonCustom
            // 
            this.radioButtonCustom.AutoSize = true;
            this.radioButtonCustom.Location = new System.Drawing.Point(15, 61);
            this.radioButtonCustom.Name = "radioButtonCustom";
            this.radioButtonCustom.Size = new System.Drawing.Size(88, 17);
            this.radioButtonCustom.TabIndex = 3;
            this.radioButtonCustom.Text = "custom year (";
            this.radioButtonCustom.UseVisualStyleBackColor = true;
            this.radioButtonCustom.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButtonCalendarYear
            // 
            this.radioButtonCalendarYear.AutoSize = true;
            this.radioButtonCalendarYear.Location = new System.Drawing.Point(15, 40);
            this.radioButtonCalendarYear.Name = "radioButtonCalendarYear";
            this.radioButtonCalendarYear.Size = new System.Drawing.Size(193, 17);
            this.radioButtonCalendarYear.TabIndex = 1;
            this.radioButtonCalendarYear.Text = "calendar year (January - December)";
            this.radioButtonCalendarYear.UseVisualStyleBackColor = true;
            this.radioButtonCalendarYear.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButtonWaterYear
            // 
            this.radioButtonWaterYear.AutoSize = true;
            this.radioButtonWaterYear.Checked = true;
            this.radioButtonWaterYear.Location = new System.Drawing.Point(15, 20);
            this.radioButtonWaterYear.Name = "radioButtonWaterYear";
            this.radioButtonWaterYear.Size = new System.Drawing.Size(181, 17);
            this.radioButtonWaterYear.TabIndex = 0;
            this.radioButtonWaterYear.TabStop = true;
            this.radioButtonWaterYear.Text = "water year (October - September)";
            this.radioButtonWaterYear.UseVisualStyleBackColor = true;
            this.radioButtonWaterYear.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // YearTypeSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Name = "YearTypeSelector";
            this.Size = new System.Drawing.Size(295, 100);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButtonCustom;
        private System.Windows.Forms.RadioButton radioButtonCalendarYear;
        private System.Windows.Forms.RadioButton radioButtonWaterYear;
        private System.Windows.Forms.Label labelEndingCustomMonth;
        private System.Windows.Forms.Label labelDebug;
    }
}
