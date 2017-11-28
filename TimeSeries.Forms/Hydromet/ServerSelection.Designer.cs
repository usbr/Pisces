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
            this.radioButtonYakLinux = new System.Windows.Forms.RadioButton();
            this.textBoxDbName = new System.Windows.Forms.TextBox();
            this.labelDbName = new System.Windows.Forms.Label();
            this.radioButtonLocal = new System.Windows.Forms.RadioButton();
            this.radioButtonPnHydromet = new System.Windows.Forms.RadioButton();
            this.radioButtonYakHydromet = new System.Windows.Forms.RadioButton();
            this.radioButtonBoiseLinux = new System.Windows.Forms.RadioButton();
            this.radioButtonGP = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonYakLinux);
            this.groupBox1.Controls.Add(this.textBoxDbName);
            this.groupBox1.Controls.Add(this.labelDbName);
            this.groupBox1.Controls.Add(this.radioButtonLocal);
            this.groupBox1.Controls.Add(this.radioButtonPnHydromet);
            this.groupBox1.Controls.Add(this.radioButtonYakHydromet);
            this.groupBox1.Controls.Add(this.radioButtonBoiseLinux);
            this.groupBox1.Controls.Add(this.radioButtonGP);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(356, 201);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "time series data source";
            // 
            // radioButtonYakLinux
            // 
            this.radioButtonYakLinux.Location = new System.Drawing.Point(15, 105);
            this.radioButtonYakLinux.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonYakLinux.Name = "radioButtonYakLinux";
            this.radioButtonYakLinux.Size = new System.Drawing.Size(136, 20);
            this.radioButtonYakLinux.TabIndex = 47;
            this.radioButtonYakLinux.Text = "Yakima Linux Hydromet";
            // 
            // textBoxDbName
            // 
            this.textBoxDbName.Location = new System.Drawing.Point(35, 168);
            this.textBoxDbName.Name = "textBoxDbName";
            this.textBoxDbName.Size = new System.Drawing.Size(186, 20);
            this.textBoxDbName.TabIndex = 46;
            this.textBoxDbName.Text = "timeseries";
            // 
            // labelDbName
            // 
            this.labelDbName.AutoSize = true;
            this.labelDbName.Location = new System.Drawing.Point(32, 151);
            this.labelDbName.Name = "labelDbName";
            this.labelDbName.Size = new System.Drawing.Size(83, 13);
            this.labelDbName.TabIndex = 45;
            this.labelDbName.Text = "database name:";
            // 
            // radioButtonLocal
            // 
            this.radioButtonLocal.Location = new System.Drawing.Point(15, 126);
            this.radioButtonLocal.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonLocal.Name = "radioButtonLocal";
            this.radioButtonLocal.Size = new System.Drawing.Size(100, 20);
            this.radioButtonLocal.TabIndex = 6;
            this.radioButtonLocal.Text = "Local source:";
            // 
            // radioButtonPnHydromet
            // 
            this.radioButtonPnHydromet.Location = new System.Drawing.Point(15, 21);
            this.radioButtonPnHydromet.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonPnHydromet.Name = "radioButtonPnHydromet";
            this.radioButtonPnHydromet.Size = new System.Drawing.Size(152, 20);
            this.radioButtonPnHydromet.TabIndex = 0;
            this.radioButtonPnHydromet.Text = "Boise Hydromet";
            this.radioButtonPnHydromet.CheckedChanged += new System.EventHandler(this.serverChanged);
            // 
            // radioButtonYakHydromet
            // 
            this.radioButtonYakHydromet.Location = new System.Drawing.Point(15, 42);
            this.radioButtonYakHydromet.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonYakHydromet.Name = "radioButtonYakHydromet";
            this.radioButtonYakHydromet.Size = new System.Drawing.Size(112, 20);
            this.radioButtonYakHydromet.TabIndex = 3;
            this.radioButtonYakHydromet.Text = "Yakima Hydromet";
            this.radioButtonYakHydromet.CheckedChanged += new System.EventHandler(this.serverChanged);
            // 
            // radioButtonBoiseLinux
            // 
            this.radioButtonBoiseLinux.Location = new System.Drawing.Point(15, 84);
            this.radioButtonBoiseLinux.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonBoiseLinux.Name = "radioButtonBoiseLinux";
            this.radioButtonBoiseLinux.Size = new System.Drawing.Size(136, 20);
            this.radioButtonBoiseLinux.TabIndex = 5;
            this.radioButtonBoiseLinux.Text = "Boise Linux Hydromet ";
            this.radioButtonBoiseLinux.CheckedChanged += new System.EventHandler(this.serverChanged);
            // 
            // radioButtonGP
            // 
            this.radioButtonGP.Location = new System.Drawing.Point(15, 63);
            this.radioButtonGP.Margin = new System.Windows.Forms.Padding(2);
            this.radioButtonGP.Name = "radioButtonGP";
            this.radioButtonGP.Size = new System.Drawing.Size(112, 20);
            this.radioButtonGP.TabIndex = 4;
            this.radioButtonGP.Text = "Billings Hydromet";
            this.radioButtonGP.CheckedChanged += new System.EventHandler(this.serverChanged);
            // 
            // ServerSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ServerSelection";
            this.Size = new System.Drawing.Size(356, 201);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonBoiseLinux;
        private System.Windows.Forms.RadioButton radioButtonGP;
        private System.Windows.Forms.RadioButton radioButtonYakHydromet;
        private System.Windows.Forms.RadioButton radioButtonPnHydromet;
        private System.Windows.Forms.RadioButton radioButtonLocal;
        private System.Windows.Forms.TextBox textBoxDbName;
        private System.Windows.Forms.Label labelDbName;
        private System.Windows.Forms.RadioButton radioButtonYakLinux;
    }
}
