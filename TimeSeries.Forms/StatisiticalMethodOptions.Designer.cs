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
            this.checkBoxMedian = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxMean
            // 
            this.checkBoxMean.AutoSize = true;
            this.checkBoxMean.Location = new System.Drawing.Point(3, 95);
            this.checkBoxMean.Name = "checkBoxMean";
            this.checkBoxMean.Size = new System.Drawing.Size(65, 17);
            this.checkBoxMean.TabIndex = 3;
            this.checkBoxMean.Text = "average";
            this.checkBoxMean.UseVisualStyleBackColor = true;
            // 
            // checkBoxCount
            // 
            this.checkBoxCount.AutoSize = true;
            this.checkBoxCount.Location = new System.Drawing.Point(3, 118);
            this.checkBoxCount.Name = "checkBoxCount";
            this.checkBoxCount.Size = new System.Drawing.Size(53, 17);
            this.checkBoxCount.TabIndex = 4;
            this.checkBoxCount.Text = "count";
            this.checkBoxCount.UseVisualStyleBackColor = true;
            // 
            // checkBoxSum
            // 
            this.checkBoxSum.AutoSize = true;
            this.checkBoxSum.Location = new System.Drawing.Point(3, 3);
            this.checkBoxSum.Name = "checkBoxSum";
            this.checkBoxSum.Size = new System.Drawing.Size(45, 17);
            this.checkBoxSum.TabIndex = 0;
            this.checkBoxSum.Text = "sum";
            this.checkBoxSum.UseVisualStyleBackColor = true;
            // 
            // checkBoxMax
            // 
            this.checkBoxMax.AutoSize = true;
            this.checkBoxMax.Location = new System.Drawing.Point(3, 26);
            this.checkBoxMax.Name = "checkBoxMax";
            this.checkBoxMax.Size = new System.Drawing.Size(45, 17);
            this.checkBoxMax.TabIndex = 1;
            this.checkBoxMax.Text = "max";
            this.checkBoxMax.UseVisualStyleBackColor = true;
            // 
            // checkBoxMin
            // 
            this.checkBoxMin.AutoSize = true;
            this.checkBoxMin.Location = new System.Drawing.Point(3, 49);
            this.checkBoxMin.Name = "checkBoxMin";
            this.checkBoxMin.Size = new System.Drawing.Size(42, 17);
            this.checkBoxMin.TabIndex = 2;
            this.checkBoxMin.Text = "min";
            this.checkBoxMin.UseVisualStyleBackColor = true;
            // 
            // checkBoxMedian
            // 
            this.checkBoxMedian.AutoSize = true;
            this.checkBoxMedian.Location = new System.Drawing.Point(3, 72);
            this.checkBoxMedian.Name = "checkBoxMedian";
            this.checkBoxMedian.Size = new System.Drawing.Size(60, 17);
            this.checkBoxMedian.TabIndex = 5;
            this.checkBoxMedian.Text = "median";
            this.checkBoxMedian.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.checkBoxCount, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxMean, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxMedian, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxSum, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxMax, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.checkBoxMin, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(73, 141);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // StatisiticalMethodOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "StatisiticalMethodOptions";
            this.Size = new System.Drawing.Size(73, 141);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxSum;
        private System.Windows.Forms.CheckBox checkBoxMax;
        private System.Windows.Forms.CheckBox checkBoxMin;
        private System.Windows.Forms.CheckBox checkBoxMean;
        private System.Windows.Forms.CheckBox checkBoxCount;
        private System.Windows.Forms.CheckBox checkBoxMedian;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
