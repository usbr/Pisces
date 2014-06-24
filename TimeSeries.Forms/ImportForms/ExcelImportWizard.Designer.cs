namespace Reclamation.TimeSeries.Forms.ImportForms
{
    partial class ExcelImportWizard
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonDatabase = new System.Windows.Forms.RadioButton();
            this.radioButtonWaterYear = new System.Windows.Forms.RadioButton();
            this.radioButtonStandard = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(290, 252);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(381, 252);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonDatabase);
            this.groupBox1.Controls.Add(this.radioButtonWaterYear);
            this.groupBox1.Controls.Add(this.radioButtonStandard);
            this.groupBox1.Location = new System.Drawing.Point(40, 23);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(357, 188);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "select the format or your excel spreadsheet";
            // 
            // radioButtonDatabase
            // 
            this.radioButtonDatabase.AutoSize = true;
            this.radioButtonDatabase.Location = new System.Drawing.Point(33, 121);
            this.radioButtonDatabase.Name = "radioButtonDatabase";
            this.radioButtonDatabase.Size = new System.Drawing.Size(273, 17);
            this.radioButtonDatabase.TabIndex = 2;
            this.radioButtonDatabase.Text = "database format: has multiple sites in a single column";
            this.radioButtonDatabase.UseVisualStyleBackColor = true;
            // 
            // radioButtonWaterYear
            // 
            this.radioButtonWaterYear.AutoSize = true;
            this.radioButtonWaterYear.Location = new System.Drawing.Point(33, 85);
            this.radioButtonWaterYear.Name = "radioButtonWaterYear";
            this.radioButtonWaterYear.Size = new System.Drawing.Size(299, 17);
            this.radioButtonWaterYear.TabIndex = 1;
            this.radioButtonWaterYear.Text = "water year data has a columns by month: Oct,Nov,Dec, ...";
            this.radioButtonWaterYear.UseVisualStyleBackColor = true;
            // 
            // radioButtonStandard
            // 
            this.radioButtonStandard.AutoSize = true;
            this.radioButtonStandard.Checked = true;
            this.radioButtonStandard.Location = new System.Drawing.Point(33, 48);
            this.radioButtonStandard.Name = "radioButtonStandard";
            this.radioButtonStandard.Size = new System.Drawing.Size(282, 17);
            this.radioButtonStandard.TabIndex = 0;
            this.radioButtonStandard.TabStop = true;
            this.radioButtonStandard.Text = "each series of data has its own column (most common)";
            this.radioButtonStandard.UseVisualStyleBackColor = true;
            // 
            // ExcelImportWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 287);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Name = "ExcelImportWizard";
            this.Text = "Import from Excel";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonDatabase;
        private System.Windows.Forms.RadioButton radioButtonWaterYear;
        private System.Windows.Forms.RadioButton radioButtonStandard;
    }
}