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
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonBoiseLinux);
            this.groupBox1.Controls.Add(this.radioButtonGP);
            this.groupBox1.Controls.Add(this.radioButtonYakHydromet);
            this.groupBox1.Controls.Add(this.radioButtonPnHydromet);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(209, 168);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "time series data source";
            // 
            // radioButtonBoiseLinux
            // 
            this.radioButtonBoiseLinux.Location = new System.Drawing.Point(16, 128);
            this.radioButtonBoiseLinux.Name = "radioButtonBoiseLinux";
            this.radioButtonBoiseLinux.Size = new System.Drawing.Size(181, 24);
            this.radioButtonBoiseLinux.TabIndex = 5;
            this.radioButtonBoiseLinux.Text = "Boise Hydromet/Linux";
            this.radioButtonBoiseLinux.CheckedChanged += new System.EventHandler(this.serverChanged);
            // 
            // radioButtonGP
            // 
            this.radioButtonGP.Location = new System.Drawing.Point(16, 98);
            this.radioButtonGP.Name = "radioButtonGP";
            this.radioButtonGP.Size = new System.Drawing.Size(149, 24);
            this.radioButtonGP.TabIndex = 4;
            this.radioButtonGP.Text = "Billings Hydromet";
            this.radioButtonGP.CheckedChanged += new System.EventHandler(this.serverChanged);
            // 
            // radioButtonYakHydromet
            // 
            this.radioButtonYakHydromet.Location = new System.Drawing.Point(16, 70);
            this.radioButtonYakHydromet.Name = "radioButtonYakHydromet";
            this.radioButtonYakHydromet.Size = new System.Drawing.Size(149, 24);
            this.radioButtonYakHydromet.TabIndex = 3;
            this.radioButtonYakHydromet.Text = "Yakima Hydromet";
            this.radioButtonYakHydromet.CheckedChanged += new System.EventHandler(this.serverChanged);
            // 
            // radioButtonPnHydromet
            // 
            this.radioButtonPnHydromet.Location = new System.Drawing.Point(16, 40);
            this.radioButtonPnHydromet.Name = "radioButtonPnHydromet";
            this.radioButtonPnHydromet.Size = new System.Drawing.Size(136, 24);
            this.radioButtonPnHydromet.TabIndex = 0;
            this.radioButtonPnHydromet.Text = "Boise Hydromet";
            this.radioButtonPnHydromet.CheckedChanged += new System.EventHandler(this.serverChanged);
            // 
            // ServerSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "ServerSelection";
            this.Size = new System.Drawing.Size(222, 176);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonBoiseLinux;
        private System.Windows.Forms.RadioButton radioButtonGP;
        private System.Windows.Forms.RadioButton radioButtonYakHydromet;
        private System.Windows.Forms.RadioButton radioButtonPnHydromet;
    }
}
