namespace HydrometTools.Stats
{
    partial class MaxStat
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
            Reclamation.Core.MonthDayRange monthDayRange4 = new Reclamation.Core.MonthDayRange();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonGo = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.monthDayRangePicker1 = new Reclamation.TimeSeries.Forms.MonthDayRangePicker();
            this.multiWaterYearSelector1 = new HydrometTools.Stats.MultiWaterYearSelector();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.multiWaterYearSelector1);
            this.groupBox1.Controls.Add(this.monthDayRangePicker1);
            this.groupBox1.Controls.Add(this.buttonGo);
            this.groupBox1.Location = new System.Drawing.Point(4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(235, 322);
            this.groupBox1.TabIndex = 37;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Listing of Max and MIN";
            // 
            // buttonGo
            // 
            this.buttonGo.Location = new System.Drawing.Point(154, 265);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(75, 23);
            this.buttonGo.TabIndex = 35;
            this.buttonGo.Text = "Refresh";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(245, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(492, 341);
            this.panel1.TabIndex = 38;
            // 
            // monthDayRangePicker1
            // 
            this.monthDayRangePicker1.BeginningMonth = 10;
            this.monthDayRangePicker1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.monthDayRangePicker1.Location = new System.Drawing.Point(9, 19);
            this.monthDayRangePicker1.MonthDayRange = monthDayRange4;
            this.monthDayRangePicker1.Name = "monthDayRangePicker1";
            this.monthDayRangePicker1.Size = new System.Drawing.Size(157, 52);
            this.monthDayRangePicker1.TabIndex = 0;
            // 
            // multiWaterYearSelector1
            // 
            this.multiWaterYearSelector1.Location = new System.Drawing.Point(15, 80);
            this.multiWaterYearSelector1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.multiWaterYearSelector1.Name = "multiWaterYearSelector1";
            this.multiWaterYearSelector1.SetGroupBoxTitle = "compute multi year average";
            this.multiWaterYearSelector1.Size = new System.Drawing.Size(212, 158);
            this.multiWaterYearSelector1.TabIndex = 0;
            // 
            // MaxStat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "MaxStat";
            this.Size = new System.Drawing.Size(749, 358);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.Panel panel1;
        private Reclamation.TimeSeries.Forms.MonthDayRangePicker monthDayRangePicker1;
        private MultiWaterYearSelector multiWaterYearSelector1;
    }
}
