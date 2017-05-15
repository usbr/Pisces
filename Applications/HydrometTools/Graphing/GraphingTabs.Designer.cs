namespace HydrometTools.Graphing
{
    partial class GraphingTabs
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
            this.tabControlGraphs = new System.Windows.Forms.TabControl();
            this.tabPageDaily = new System.Windows.Forms.TabPage();
            this.dailyGraphing2 = new HydrometTools.Graphing.DailyGraphing();
            this.tabControlGraphs.SuspendLayout();
            this.tabPageDaily.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlGraphs
            // 
            this.tabControlGraphs.Controls.Add(this.tabPageDaily);
            this.tabControlGraphs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlGraphs.Location = new System.Drawing.Point(0, 0);
            this.tabControlGraphs.Name = "tabControlGraphs";
            this.tabControlGraphs.SelectedIndex = 0;
            this.tabControlGraphs.Size = new System.Drawing.Size(622, 447);
            this.tabControlGraphs.TabIndex = 1;
            // 
            // tabPageDaily
            // 
            this.tabPageDaily.Controls.Add(this.dailyGraphing2);
            this.tabPageDaily.Location = new System.Drawing.Point(4, 22);
            this.tabPageDaily.Name = "tabPageDaily";
            this.tabPageDaily.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDaily.Size = new System.Drawing.Size(614, 421);
            this.tabPageDaily.TabIndex = 0;
            this.tabPageDaily.Text = "Daily";
            this.tabPageDaily.UseVisualStyleBackColor = true;
            // 
            // dailyGraphing2
            // 
            this.dailyGraphing2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dailyGraphing2.Location = new System.Drawing.Point(3, 3);
            this.dailyGraphing2.Name = "dailyGraphing2";
            this.dailyGraphing2.Size = new System.Drawing.Size(608, 415);
            this.dailyGraphing2.TabIndex = 0;
            // 
            // GraphingTabs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlGraphs);
            this.Name = "GraphingTabs";
            this.Size = new System.Drawing.Size(622, 447);
            this.tabControlGraphs.ResumeLayout(false);
            this.tabPageDaily.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlGraphs;
        private System.Windows.Forms.TabPage tabPageDaily;
        //private DailyGraphing dailyGraphing1;
        private DailyGraphing dailyGraphing2;
    }
}
