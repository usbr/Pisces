namespace Reclamation.TimeSeries.Forms.Alarms
{
    partial class AlarmManagerControl
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageAlarmDef = new System.Windows.Forms.TabPage();
            this.tabPageAlarms = new System.Windows.Forms.TabPage();
            this.tabPageSetup = new System.Windows.Forms.TabPage();
            this.tabPageSounds = new System.Windows.Forms.TabPage();
            this.tabPageLog = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageAlarmDef);
            this.tabControl1.Controls.Add(this.tabPageAlarms);
            this.tabControl1.Controls.Add(this.tabPageSetup);
            this.tabControl1.Controls.Add(this.tabPageSounds);
            this.tabControl1.Controls.Add(this.tabPageLog);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(637, 527);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPageAlarmDef
            // 
            this.tabPageAlarmDef.Location = new System.Drawing.Point(4, 22);
            this.tabPageAlarmDef.Name = "tabPageAlarmDef";
            this.tabPageAlarmDef.Size = new System.Drawing.Size(629, 501);
            this.tabPageAlarmDef.TabIndex = 3;
            this.tabPageAlarmDef.Text = "Define Alarm";
            this.tabPageAlarmDef.UseVisualStyleBackColor = true;
            // 
            // tabPageAlarms
            // 
            this.tabPageAlarms.Location = new System.Drawing.Point(4, 22);
            this.tabPageAlarms.Name = "tabPageAlarms";
            this.tabPageAlarms.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAlarms.Size = new System.Drawing.Size(629, 501);
            this.tabPageAlarms.TabIndex = 1;
            this.tabPageAlarms.Text = "Alarm Status";
            this.tabPageAlarms.UseVisualStyleBackColor = true;
            // 
            // tabPageSetup
            // 
            this.tabPageSetup.Location = new System.Drawing.Point(4, 22);
            this.tabPageSetup.Name = "tabPageSetup";
            this.tabPageSetup.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSetup.Size = new System.Drawing.Size(629, 501);
            this.tabPageSetup.TabIndex = 0;
            this.tabPageSetup.Text = "Setup";
            this.tabPageSetup.UseVisualStyleBackColor = true;
            // 
            // tabPageSounds
            // 
            this.tabPageSounds.Location = new System.Drawing.Point(4, 22);
            this.tabPageSounds.Name = "tabPageSounds";
            this.tabPageSounds.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSounds.Size = new System.Drawing.Size(629, 501);
            this.tabPageSounds.TabIndex = 2;
            this.tabPageSounds.Text = "Sound Files";
            this.tabPageSounds.UseVisualStyleBackColor = true;
            // 
            // tabPageLog
            // 
            this.tabPageLog.Location = new System.Drawing.Point(4, 22);
            this.tabPageLog.Name = "tabPageLog";
            this.tabPageLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLog.Size = new System.Drawing.Size(629, 501);
            this.tabPageLog.TabIndex = 4;
            this.tabPageLog.Text = "log";
            this.tabPageLog.UseVisualStyleBackColor = true;
            // 
            // AlarmManagerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "AlarmManagerControl";
            this.Size = new System.Drawing.Size(637, 527);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageAlarmDef;
        private System.Windows.Forms.TabPage tabPageAlarms;
        private System.Windows.Forms.TabPage tabPageSetup;
        private System.Windows.Forms.TabPage tabPageSounds;
        private System.Windows.Forms.TabPage tabPageLog;
    }
}
