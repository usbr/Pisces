namespace Reclamation.TimeSeries.Forms
{
    partial class MovingAverageOptions
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
            this.timeWindowOptions1 = new Reclamation.TimeSeries.Forms.TimeWindowSelector();
            this.checkBox24hr = new System.Windows.Forms.CheckBox();
            this.checkBox120hr = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxRawData = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timeWindowOptions1
            // 
            this.timeWindowOptions1.Location = new System.Drawing.Point(26, 18);
            this.timeWindowOptions1.Name = "timeWindowOptions1";
            this.timeWindowOptions1.Size = new System.Drawing.Size(275, 115);
            this.timeWindowOptions1.TabIndex = 0;
            // 
            // checkBox24hr
            // 
            this.checkBox24hr.AutoSize = true;
            this.checkBox24hr.Checked = true;
            this.checkBox24hr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox24hr.Location = new System.Drawing.Point(15, 49);
            this.checkBox24hr.Name = "checkBox24hr";
            this.checkBox24hr.Size = new System.Drawing.Size(62, 17);
            this.checkBox24hr.TabIndex = 1;
            this.checkBox24hr.Text = "24 hour";
            this.checkBox24hr.UseVisualStyleBackColor = true;
            // 
            // checkBox120hr
            // 
            this.checkBox120hr.AutoSize = true;
            this.checkBox120hr.Location = new System.Drawing.Point(15, 72);
            this.checkBox120hr.Name = "checkBox120hr";
            this.checkBox120hr.Size = new System.Drawing.Size(108, 17);
            this.checkBox120hr.TabIndex = 2;
            this.checkBox120hr.Text = "120 hour (5 days)";
            this.checkBox120hr.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxRawData);
            this.groupBox1.Controls.Add(this.checkBox24hr);
            this.groupBox1.Controls.Add(this.checkBox120hr);
            this.groupBox1.Location = new System.Drawing.Point(26, 139);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(196, 124);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "moving average options";
            // 
            // checkBoxRawData
            // 
            this.checkBoxRawData.AutoSize = true;
            this.checkBoxRawData.Checked = true;
            this.checkBoxRawData.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxRawData.Location = new System.Drawing.Point(15, 26);
            this.checkBoxRawData.Name = "checkBoxRawData";
            this.checkBoxRawData.Size = new System.Drawing.Size(67, 17);
            this.checkBoxRawData.TabIndex = 3;
            this.checkBoxRawData.Text = "raw data";
            this.checkBoxRawData.UseVisualStyleBackColor = true;
            // 
            // MovingAverageOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.timeWindowOptions1);
            this.Name = "MovingAverageOptions";
            this.Size = new System.Drawing.Size(394, 317);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TimeWindowSelector timeWindowOptions1;
        private System.Windows.Forms.CheckBox checkBox24hr;
        private System.Windows.Forms.CheckBox checkBox120hr;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxRawData;
    }
}
