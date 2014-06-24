namespace Reclamation.TimeSeries.Forms
{
    partial class SummaryOption
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
            this.radioButtonAverage = new System.Windows.Forms.RadioButton();
            this.radioButtonAnnualMin = new System.Windows.Forms.RadioButton();
            this.radioButtonAnnualMax = new System.Windows.Forms.RadioButton();
            this.radioButtonSum = new System.Windows.Forms.RadioButton();
            this.radioButtonNone = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonAverage);
            this.groupBox1.Controls.Add(this.radioButtonAnnualMin);
            this.groupBox1.Controls.Add(this.radioButtonAnnualMax);
            this.groupBox1.Controls.Add(this.radioButtonSum);
            this.groupBox1.Controls.Add(this.radioButtonNone);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(157, 152);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "summary";
            // 
            // radioButtonAverage
            // 
            this.radioButtonAverage.AutoSize = true;
            this.radioButtonAverage.Location = new System.Drawing.Point(19, 115);
            this.radioButtonAverage.Name = "radioButtonAverage";
            this.radioButtonAverage.Size = new System.Drawing.Size(64, 17);
            this.radioButtonAverage.TabIndex = 4;
            this.radioButtonAverage.TabStop = true;
            this.radioButtonAverage.Text = "average";
            this.radioButtonAverage.UseVisualStyleBackColor = true;
            this.radioButtonAverage.CheckedChanged += new System.EventHandler(this.radioButtonCheckedChanged);
            this.radioButtonAverage.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioButtonNone_MouseClick);
            // 
            // radioButtonAnnualMin
            // 
            this.radioButtonAnnualMin.AutoSize = true;
            this.radioButtonAnnualMin.Location = new System.Drawing.Point(19, 92);
            this.radioButtonAnnualMin.Name = "radioButtonAnnualMin";
            this.radioButtonAnnualMin.Size = new System.Drawing.Size(41, 17);
            this.radioButtonAnnualMin.TabIndex = 3;
            this.radioButtonAnnualMin.TabStop = true;
            this.radioButtonAnnualMin.Text = "min";
            this.radioButtonAnnualMin.UseVisualStyleBackColor = true;
            this.radioButtonAnnualMin.CheckedChanged += new System.EventHandler(this.radioButtonCheckedChanged);
            this.radioButtonAnnualMin.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioButtonNone_MouseClick);
            // 
            // radioButtonAnnualMax
            // 
            this.radioButtonAnnualMax.AutoSize = true;
            this.radioButtonAnnualMax.Location = new System.Drawing.Point(19, 68);
            this.radioButtonAnnualMax.Name = "radioButtonAnnualMax";
            this.radioButtonAnnualMax.Size = new System.Drawing.Size(44, 17);
            this.radioButtonAnnualMax.TabIndex = 2;
            this.radioButtonAnnualMax.TabStop = true;
            this.radioButtonAnnualMax.Text = "max";
            this.radioButtonAnnualMax.UseVisualStyleBackColor = true;
            this.radioButtonAnnualMax.CheckedChanged += new System.EventHandler(this.radioButtonCheckedChanged);
            this.radioButtonAnnualMax.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioButtonNone_MouseClick);
            // 
            // radioButtonSum
            // 
            this.radioButtonSum.AutoSize = true;
            this.radioButtonSum.Location = new System.Drawing.Point(19, 44);
            this.radioButtonSum.Name = "radioButtonSum";
            this.radioButtonSum.Size = new System.Drawing.Size(44, 17);
            this.radioButtonSum.TabIndex = 1;
            this.radioButtonSum.TabStop = true;
            this.radioButtonSum.Text = "sum";
            this.radioButtonSum.UseVisualStyleBackColor = true;
            this.radioButtonSum.CheckedChanged += new System.EventHandler(this.radioButtonCheckedChanged);
            this.radioButtonSum.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioButtonNone_MouseClick);
            // 
            // radioButtonNone
            // 
            this.radioButtonNone.AutoSize = true;
            this.radioButtonNone.Checked = true;
            this.radioButtonNone.Location = new System.Drawing.Point(19, 20);
            this.radioButtonNone.Name = "radioButtonNone";
            this.radioButtonNone.Size = new System.Drawing.Size(49, 17);
            this.radioButtonNone.TabIndex = 0;
            this.radioButtonNone.TabStop = true;
            this.radioButtonNone.Text = "none";
            this.radioButtonNone.UseVisualStyleBackColor = true;
            this.radioButtonNone.CheckedChanged += new System.EventHandler(this.radioButtonCheckedChanged);
            this.radioButtonNone.MouseClick += new System.Windows.Forms.MouseEventHandler(this.radioButtonNone_MouseClick);
            // 
            // SummaryOption
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "SummaryOption";
            this.Size = new System.Drawing.Size(157, 152);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.SummaryOption_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SummaryOption_MouseDown);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonAnnualMin;
        private System.Windows.Forms.RadioButton radioButtonAnnualMax;
        private System.Windows.Forms.RadioButton radioButtonSum;
        private System.Windows.Forms.RadioButton radioButtonNone;
        private System.Windows.Forms.RadioButton radioButtonAverage;
    }
}
