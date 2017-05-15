namespace HydrometTools.Reports
{
    partial class Reports
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
            this.tabPageYakima = new System.Windows.Forms.TabPage();
            this.tabPageUpperSnake = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageYakima);
            this.tabControl1.Controls.Add(this.tabPageUpperSnake);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(716, 509);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPageYakima
            // 
            this.tabPageYakima.Location = new System.Drawing.Point(4, 22);
            this.tabPageYakima.Name = "tabPageYakima";
            this.tabPageYakima.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageYakima.Size = new System.Drawing.Size(708, 483);
            this.tabPageYakima.TabIndex = 0;
            this.tabPageYakima.Text = "yakima";
            this.tabPageYakima.UseVisualStyleBackColor = true;
            // 
            // tabPageUpperSnake
            // 
            this.tabPageUpperSnake.Location = new System.Drawing.Point(4, 22);
            this.tabPageUpperSnake.Name = "tabPageUpperSnake";
            this.tabPageUpperSnake.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageUpperSnake.Size = new System.Drawing.Size(192, 74);
            this.tabPageUpperSnake.TabIndex = 1;
            this.tabPageUpperSnake.Text = "upper snake";
            this.tabPageUpperSnake.UseVisualStyleBackColor = true;
            // 
            // Reports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "Reports";
            this.Size = new System.Drawing.Size(716, 509);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageYakima;
        private System.Windows.Forms.TabPage tabPageUpperSnake;
    }
}
