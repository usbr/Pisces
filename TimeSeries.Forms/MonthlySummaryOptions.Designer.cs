namespace Reclamation.TimeSeries.Forms
{
    partial class MonthlySummaryOptions
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statisiticalMethodOptions1 = new Reclamation.TimeSeries.Forms.StatisiticalMethodOptions();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton12Values = new System.Windows.Forms.RadioButton();
            this.radioButtonMultiYear = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxAdvanced
            // 
            this.groupBoxAdvanced.Location = new System.Drawing.Point(124, 130);
            this.groupBoxAdvanced.Size = new System.Drawing.Size(585, 263);
            // 
            // statisiticalMethodOptions1
            // 
            this.statisiticalMethodOptions1.Location = new System.Drawing.Point(12, 130);
            this.statisiticalMethodOptions1.Name = "statisiticalMethodOptions1";
            this.statisiticalMethodOptions1.Size = new System.Drawing.Size(124, 216);
            this.statisiticalMethodOptions1.StatisticalMethods = Reclamation.TimeSeries.StatisticalMethods.None;
            this.statisiticalMethodOptions1.TabIndex = 12;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton12Values);
            this.groupBox1.Controls.Add(this.radioButtonMultiYear);
            this.groupBox1.Location = new System.Drawing.Point(25, 274);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(125, 78);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "monthly aggregation";
            // 
            // radioButton12Values
            // 
            this.radioButton12Values.AutoSize = true;
            this.radioButton12Values.Location = new System.Drawing.Point(10, 45);
            this.radioButton12Values.Name = "radioButton12Values";
            this.radioButton12Values.Size = new System.Drawing.Size(74, 17);
            this.radioButton12Values.TabIndex = 1;
            this.radioButton12Values.Text = "12 months";
            this.radioButton12Values.UseVisualStyleBackColor = true;
            // 
            // radioButtonMultiYear
            // 
            this.radioButtonMultiYear.AutoSize = true;
            this.radioButtonMultiYear.Checked = true;
            this.radioButtonMultiYear.Location = new System.Drawing.Point(10, 22);
            this.radioButtonMultiYear.Name = "radioButtonMultiYear";
            this.radioButtonMultiYear.Size = new System.Drawing.Size(69, 17);
            this.radioButtonMultiYear.TabIndex = 0;
            this.radioButtonMultiYear.TabStop = true;
            this.radioButtonMultiYear.Text = "multi year";
            this.radioButtonMultiYear.UseVisualStyleBackColor = true;
            // 
            // MonthlySummaryOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statisiticalMethodOptions1);
            this.Name = "MonthlySummaryOptions";
            this.Size = new System.Drawing.Size(733, 410);
            this.Controls.SetChildIndex(this.groupBoxAdvanced, 0);
            this.Controls.SetChildIndex(this.statisiticalMethodOptions1, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private StatisiticalMethodOptions statisiticalMethodOptions1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton12Values;
        private System.Windows.Forms.RadioButton radioButtonMultiYear;
    }
}
