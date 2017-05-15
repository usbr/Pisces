namespace HydrometTools.Stats
{
    partial class StatsControl
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.monthly1 = new HydrometTools.Stats.Monthly();
            this.tabVol = new System.Windows.Forms.TabPage();
            this.vol1 = new HydrometTools.Stats.VolumeInRange();
            this.tabMaxMin = new System.Windows.Forms.TabPage();
            this.max1 = new HydrometTools.Stats.MaxStat();
            this.tabPage30YearAverage = new System.Windows.Forms.TabPage();
            this.multiYearAverage1 = new HydrometTools.Stats.MultiYearAverage();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage2.SuspendLayout();
            this.tabVol.SuspendLayout();
            this.tabMaxMin.SuspendLayout();
            this.tabPage30YearAverage.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(798, 15);
            this.panel1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.monthly1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(790, 538);
            this.tabPage2.TabIndex = 4;
            this.tabPage2.Text = "Monthly";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // monthly1
            // 
            this.monthly1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.monthly1.Location = new System.Drawing.Point(3, 3);
            this.monthly1.Margin = new System.Windows.Forms.Padding(4);
            this.monthly1.Name = "monthly1";
            this.monthly1.Size = new System.Drawing.Size(784, 532);
            this.monthly1.TabIndex = 0;
            // 
            // tabVol
            // 
            this.tabVol.Controls.Add(this.vol1);
            this.tabVol.Location = new System.Drawing.Point(4, 22);
            this.tabVol.Name = "tabVol";
            this.tabVol.Padding = new System.Windows.Forms.Padding(3);
            this.tabVol.Size = new System.Drawing.Size(790, 538);
            this.tabVol.TabIndex = 2;
            this.tabVol.Text = "Volume in Range";
            this.tabVol.UseVisualStyleBackColor = true;
            // 
            // vol1
            // 
            this.vol1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.vol1.Location = new System.Drawing.Point(3, 3);
            this.vol1.Margin = new System.Windows.Forms.Padding(4);
            this.vol1.Name = "vol1";
            this.vol1.Size = new System.Drawing.Size(784, 532);
            this.vol1.TabIndex = 0;
            // 
            // tabMaxMin
            // 
            this.tabMaxMin.Controls.Add(this.max1);
            this.tabMaxMin.Location = new System.Drawing.Point(4, 22);
            this.tabMaxMin.Name = "tabMaxMin";
            this.tabMaxMin.Padding = new System.Windows.Forms.Padding(3);
            this.tabMaxMin.Size = new System.Drawing.Size(790, 538);
            this.tabMaxMin.TabIndex = 1;
            this.tabMaxMin.Text = "Max and Min Range";
            this.tabMaxMin.UseVisualStyleBackColor = true;
            // 
            // max1
            // 
            this.max1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.max1.Location = new System.Drawing.Point(3, 3);
            this.max1.Margin = new System.Windows.Forms.Padding(4);
            this.max1.Name = "max1";
            this.max1.Size = new System.Drawing.Size(784, 532);
            this.max1.TabIndex = 0;
            // 
            // tabPage30YearAverage
            // 
            this.tabPage30YearAverage.Controls.Add(this.multiYearAverage1);
            this.tabPage30YearAverage.Location = new System.Drawing.Point(4, 22);
            this.tabPage30YearAverage.Name = "tabPage30YearAverage";
            this.tabPage30YearAverage.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage30YearAverage.Size = new System.Drawing.Size(790, 538);
            this.tabPage30YearAverage.TabIndex = 0;
            this.tabPage30YearAverage.Text = "multi year avg";
            this.tabPage30YearAverage.UseVisualStyleBackColor = true;
            // 
            // multiYearAverage1
            // 
            this.multiYearAverage1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.multiYearAverage1.Location = new System.Drawing.Point(3, 3);
            this.multiYearAverage1.Margin = new System.Windows.Forms.Padding(4);
            this.multiYearAverage1.Name = "multiYearAverage1";
            this.multiYearAverage1.Size = new System.Drawing.Size(784, 532);
            this.multiYearAverage1.TabIndex = 0;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage30YearAverage);
            this.tabControl.Controls.Add(this.tabMaxMin);
            this.tabControl.Controls.Add(this.tabVol);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 15);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(798, 564);
            this.tabControl.TabIndex = 1;
            // 
            // StatsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.panel1);
            this.Name = "StatsControl";
            this.Size = new System.Drawing.Size(798, 579);
            this.tabPage2.ResumeLayout(false);
            this.tabVol.ResumeLayout(false);
            this.tabMaxMin.ResumeLayout(false);
            this.tabPage30YearAverage.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabPage tabPage2;
        private Monthly monthly1;
        private System.Windows.Forms.TabPage tabVol;
        private VolumeInRange vol1;
        private System.Windows.Forms.TabPage tabMaxMin;
        private MaxStat max1;
        private System.Windows.Forms.TabPage tabPage30YearAverage;
        private System.Windows.Forms.TabControl tabControl;
        private MultiYearAverage multiYearAverage1;
    }
}
