namespace Reclamation.TimeSeries.Forms.RatingTables
{
    partial class RatingTableZedGraphOptions
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBoxXAxisIsLog = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxYAxisIsLog = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxXAxisIsLog);
            this.groupBox2.Location = new System.Drawing.Point(30, 41);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(137, 72);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "x-axis";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(45, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBoxXAxisIsLog
            // 
            this.checkBoxXAxisIsLog.AutoSize = true;
            this.checkBoxXAxisIsLog.Location = new System.Drawing.Point(15, 19);
            this.checkBoxXAxisIsLog.Name = "checkBoxXAxisIsLog";
            this.checkBoxXAxisIsLog.Size = new System.Drawing.Size(40, 17);
            this.checkBoxXAxisIsLog.TabIndex = 0;
            this.checkBoxXAxisIsLog.Text = "log";
            this.checkBoxXAxisIsLog.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxYAxisIsLog);
            this.groupBox1.Location = new System.Drawing.Point(193, 41);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(137, 72);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "y-axis";
            // 
            // checkBoxYAxisIsLog
            // 
            this.checkBoxYAxisIsLog.AutoSize = true;
            this.checkBoxYAxisIsLog.Location = new System.Drawing.Point(15, 19);
            this.checkBoxYAxisIsLog.Name = "checkBoxYAxisIsLog";
            this.checkBoxYAxisIsLog.Size = new System.Drawing.Size(40, 17);
            this.checkBoxYAxisIsLog.TabIndex = 0;
            this.checkBoxYAxisIsLog.Text = "log";
            this.checkBoxYAxisIsLog.UseVisualStyleBackColor = true;
            // 
            // RatingTableZedGraphOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Name = "RatingTableZedGraphOptions";
            this.Size = new System.Drawing.Size(480, 315);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxXAxisIsLog;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxYAxisIsLog;
    }
}
