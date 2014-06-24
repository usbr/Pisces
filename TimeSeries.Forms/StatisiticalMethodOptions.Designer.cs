namespace Reclamation.TimeSeries.Forms
{
    partial class StatisiticalMethodOptions
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
            this.checkBoxMean = new System.Windows.Forms.CheckBox();
            this.checkBoxCount = new System.Windows.Forms.CheckBox();
            this.checkBoxSum = new System.Windows.Forms.CheckBox();
            this.checkBoxMax = new System.Windows.Forms.CheckBox();
            this.checkBoxMin = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBoxMean
            // 
            this.checkBoxMean.AutoSize = true;
            this.checkBoxMean.Location = new System.Drawing.Point(15, 82);
            this.checkBoxMean.Name = "checkBoxMean";
            this.checkBoxMean.Size = new System.Drawing.Size(65, 17);
            this.checkBoxMean.TabIndex = 3;
            this.checkBoxMean.Text = "average";
            this.checkBoxMean.UseVisualStyleBackColor = true;
            // 
            // checkBoxCount
            // 
            this.checkBoxCount.AutoSize = true;
            this.checkBoxCount.Location = new System.Drawing.Point(15, 105);
            this.checkBoxCount.Name = "checkBoxCount";
            this.checkBoxCount.Size = new System.Drawing.Size(53, 17);
            this.checkBoxCount.TabIndex = 4;
            this.checkBoxCount.Text = "count";
            this.checkBoxCount.UseVisualStyleBackColor = true;
            // 
            // checkBoxSum
            // 
            this.checkBoxSum.AutoSize = true;
            this.checkBoxSum.Location = new System.Drawing.Point(15, 13);
            this.checkBoxSum.Name = "checkBoxSum";
            this.checkBoxSum.Size = new System.Drawing.Size(45, 17);
            this.checkBoxSum.TabIndex = 0;
            this.checkBoxSum.Text = "sum";
            this.checkBoxSum.UseVisualStyleBackColor = true;
            // 
            // checkBoxMax
            // 
            this.checkBoxMax.AutoSize = true;
            this.checkBoxMax.Location = new System.Drawing.Point(15, 36);
            this.checkBoxMax.Name = "checkBoxMax";
            this.checkBoxMax.Size = new System.Drawing.Size(45, 17);
            this.checkBoxMax.TabIndex = 1;
            this.checkBoxMax.Text = "max";
            this.checkBoxMax.UseVisualStyleBackColor = true;
            // 
            // checkBoxMin
            // 
            this.checkBoxMin.AutoSize = true;
            this.checkBoxMin.Location = new System.Drawing.Point(15, 59);
            this.checkBoxMin.Name = "checkBoxMin";
            this.checkBoxMin.Size = new System.Drawing.Size(42, 17);
            this.checkBoxMin.TabIndex = 2;
            this.checkBoxMin.Text = "min";
            this.checkBoxMin.UseVisualStyleBackColor = true;
            // 
            // StatisiticalMethodOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxCount);
            this.Controls.Add(this.checkBoxMean);
            this.Controls.Add(this.checkBoxMin);
            this.Controls.Add(this.checkBoxMax);
            this.Controls.Add(this.checkBoxSum);
            this.Name = "StatisiticalMethodOptions";
            this.Size = new System.Drawing.Size(93, 164);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxSum;
        private System.Windows.Forms.CheckBox checkBoxMax;
        private System.Windows.Forms.CheckBox checkBoxMin;
        private System.Windows.Forms.CheckBox checkBoxMean;
        private System.Windows.Forms.CheckBox checkBoxCount;
    }
}
