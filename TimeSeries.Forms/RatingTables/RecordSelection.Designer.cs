namespace Reclamation.TimeSeries.Forms.RatingTables
{
    partial class RecordSelection
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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxSite = new System.Windows.Forms.ComboBox();
            this.comboBoxRatingTable = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxWaterYear = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "site";
            // 
            // comboBoxSite
            // 
            this.comboBoxSite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxSite.FormattingEnabled = true;
            this.comboBoxSite.Location = new System.Drawing.Point(85, 18);
            this.comboBoxSite.Name = "comboBoxSite";
            this.comboBoxSite.Size = new System.Drawing.Size(158, 21);
            this.comboBoxSite.TabIndex = 1;
            // 
            // comboBoxRatingTable
            // 
            this.comboBoxRatingTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxRatingTable.FormattingEnabled = true;
            this.comboBoxRatingTable.Location = new System.Drawing.Point(85, 45);
            this.comboBoxRatingTable.Name = "comboBoxRatingTable";
            this.comboBoxRatingTable.Size = new System.Drawing.Size(158, 21);
            this.comboBoxRatingTable.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "rating table";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "water year";
            // 
            // textBoxWaterYear
            // 
            this.textBoxWaterYear.Location = new System.Drawing.Point(85, 73);
            this.textBoxWaterYear.Name = "textBoxWaterYear";
            this.textBoxWaterYear.Size = new System.Drawing.Size(72, 20);
            this.textBoxWaterYear.TabIndex = 5;
            // 
            // RecordSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxWaterYear);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxRatingTable);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxSite);
            this.Controls.Add(this.label1);
            this.Name = "RecordSelection";
            this.Size = new System.Drawing.Size(294, 223);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxSite;
        private System.Windows.Forms.ComboBox comboBoxRatingTable;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxWaterYear;
    }
}
