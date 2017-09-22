namespace Reclamation.TimeSeries.RiverWare
{
    partial class ImportRiverWare
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
            this.textBoxYear = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonDefault = new System.Windows.Forms.RadioButton();
            this.radioButtonYear = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(163, 153);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(244, 153);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // textBoxYear
            // 
            this.textBoxYear.Location = new System.Drawing.Point(112, 26);
            this.textBoxYear.Name = "textBoxYear";
            this.textBoxYear.Size = new System.Drawing.Size(47, 20);
            this.textBoxYear.TabIndex = 6;
            this.textBoxYear.Text = "1919";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonDefault);
            this.groupBox1.Controls.Add(this.radioButtonYear);
            this.groupBox1.Controls.Add(this.textBoxYear);
            this.groupBox1.Location = new System.Drawing.Point(34, 26);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 92);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "scenario naming convention";
            // 
            // radioButtonDefault
            // 
            this.radioButtonDefault.AutoSize = true;
            this.radioButtonDefault.Location = new System.Drawing.Point(10, 60);
            this.radioButtonDefault.Name = "radioButtonDefault";
            this.radioButtonDefault.Size = new System.Drawing.Size(104, 17);
            this.radioButtonDefault.TabIndex = 9;
            this.radioButtonDefault.Text = "Run0,Run1, etc.";
            this.radioButtonDefault.UseVisualStyleBackColor = true;
            // 
            // radioButtonYear
            // 
            this.radioButtonYear.AutoSize = true;
            this.radioButtonYear.Checked = true;
            this.radioButtonYear.Location = new System.Drawing.Point(7, 26);
            this.radioButtonYear.Name = "radioButtonYear";
            this.radioButtonYear.Size = new System.Drawing.Size(99, 17);
            this.radioButtonYear.TabIndex = 8;
            this.radioButtonYear.TabStop = true;
            this.radioButtonYear.Text = "begin with year:";
            this.radioButtonYear.UseVisualStyleBackColor = true;
            // 
            // ImportRiverWare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 188);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Name = "ImportRiverWare";
            this.Text = "Import RiverWare Data File (MRM)";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textBoxYear;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonDefault;
        private System.Windows.Forms.RadioButton radioButtonYear;
    }
}