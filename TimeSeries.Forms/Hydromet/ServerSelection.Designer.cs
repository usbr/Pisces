namespace Reclamation.TimeSeries.Forms.Hydromet
{
    partial class ServerSelection
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonBoiseLinux = new System.Windows.Forms.RadioButton();
            this.radioButtonGP = new System.Windows.Forms.RadioButton();
            this.radioButtonYakHydromet = new System.Windows.Forms.RadioButton();
            this.radioButtonPnHydromet = new System.Windows.Forms.RadioButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(158, 116);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "time series data source";
            // 
            // radioButtonBoiseLinux
            // 
            this.radioButtonBoiseLinux.Location = new System.Drawing.Point(2, 74);
            this.radioButtonBoiseLinux.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonBoiseLinux.Name = "radioButtonBoiseLinux";
            this.radioButtonBoiseLinux.Size = new System.Drawing.Size(136, 20);
            this.radioButtonBoiseLinux.TabIndex = 5;
            this.radioButtonBoiseLinux.Text = "Boise Hydromet/Linux";
            this.radioButtonBoiseLinux.CheckedChanged += new System.EventHandler(this.serverChanged);
            // 
            // radioButtonGP
            // 
            this.radioButtonGP.Location = new System.Drawing.Point(2, 50);
            this.radioButtonGP.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonGP.Name = "radioButtonGP";
            this.radioButtonGP.Size = new System.Drawing.Size(112, 20);
            this.radioButtonGP.TabIndex = 4;
            this.radioButtonGP.Text = "Billings Hydromet";
            this.radioButtonGP.CheckedChanged += new System.EventHandler(this.serverChanged);
            // 
            // radioButtonYakHydromet
            // 
            this.radioButtonYakHydromet.Location = new System.Drawing.Point(2, 26);
            this.radioButtonYakHydromet.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonYakHydromet.Name = "radioButtonYakHydromet";
            this.radioButtonYakHydromet.Size = new System.Drawing.Size(112, 20);
            this.radioButtonYakHydromet.TabIndex = 3;
            this.radioButtonYakHydromet.Text = "Yakima Hydromet";
            this.radioButtonYakHydromet.CheckedChanged += new System.EventHandler(this.serverChanged);
            // 
            // radioButtonPnHydromet
            // 
            this.radioButtonPnHydromet.Location = new System.Drawing.Point(2, 2);
            this.radioButtonPnHydromet.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonPnHydromet.Name = "radioButtonPnHydromet";
            this.radioButtonPnHydromet.Size = new System.Drawing.Size(152, 20);
            this.radioButtonPnHydromet.TabIndex = 0;
            this.radioButtonPnHydromet.Text = "Boise Hydromet";
            this.radioButtonPnHydromet.CheckedChanged += new System.EventHandler(this.serverChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.radioButtonPnHydromet);
            this.flowLayoutPanel1.Controls.Add(this.radioButtonYakHydromet);
            this.flowLayoutPanel1.Controls.Add(this.radioButtonGP);
            this.flowLayoutPanel1.Controls.Add(this.radioButtonBoiseLinux);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(2, 15);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(154, 99);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // ServerSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ServerSelection";
            this.Size = new System.Drawing.Size(158, 116);
            this.groupBox1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonBoiseLinux;
        private System.Windows.Forms.RadioButton radioButtonGP;
        private System.Windows.Forms.RadioButton radioButtonYakHydromet;
        private System.Windows.Forms.RadioButton radioButtonPnHydromet;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}
