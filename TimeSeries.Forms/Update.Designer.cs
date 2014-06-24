namespace Reclamation.TimeSeries.Forms
{
    partial class Update
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
            this.textBoxInfo = new System.Windows.Forms.TextBox();
            this.checkBoxFullPeriod = new System.Windows.Forms.CheckBox();
            this.timeSelectorBeginEnd1 = new Reclamation.TimeSeries.Forms.TimeSelectorBeginEnd();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(90, 164);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 16;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(182, 164);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 17;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // textBoxInfo
            // 
            this.textBoxInfo.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxInfo.Location = new System.Drawing.Point(13, 13);
            this.textBoxInfo.Multiline = true;
            this.textBoxInfo.Name = "textBoxInfo";
            this.textBoxInfo.Size = new System.Drawing.Size(240, 50);
            this.textBoxInfo.TabIndex = 18;
            this.textBoxInfo.Text = "Click OK to update the selected series.  Pisces will reload data from the origina" +
                "l source.  Existing data will be overwritten.";
            // 
            // checkBoxFullPeriod
            // 
            this.checkBoxFullPeriod.AutoSize = true;
            this.checkBoxFullPeriod.Location = new System.Drawing.Point(85, 131);
            this.checkBoxFullPeriod.Name = "checkBoxFullPeriod";
            this.checkBoxFullPeriod.Size = new System.Drawing.Size(136, 17);
            this.checkBoxFullPeriod.TabIndex = 19;
            this.checkBoxFullPeriod.Text = "use full period of record";
            this.checkBoxFullPeriod.UseVisualStyleBackColor = true;
            this.checkBoxFullPeriod.CheckedChanged += new System.EventHandler(this.EnableDates);
            // 
            // timeSelectorBeginEnd1
            // 
            this.timeSelectorBeginEnd1.Location = new System.Drawing.Point(28, 79);
            this.timeSelectorBeginEnd1.Name = "timeSelectorBeginEnd1";
            this.timeSelectorBeginEnd1.ShowTime = false;
            this.timeSelectorBeginEnd1.Size = new System.Drawing.Size(195, 46);
            this.timeSelectorBeginEnd1.T1 = new System.DateTime(2009, 5, 1, 14, 44, 46, 511);
            this.timeSelectorBeginEnd1.T2 = new System.DateTime(2009, 5, 1, 14, 44, 46, 511);
            this.timeSelectorBeginEnd1.TabIndex = 0;
            // 
            // Update
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(267, 203);
            this.Controls.Add(this.checkBoxFullPeriod);
            this.Controls.Add(this.textBoxInfo);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.timeSelectorBeginEnd1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Update";
            this.Text = "Update Timeseries data";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TimeSelectorBeginEnd timeSelectorBeginEnd1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textBoxInfo;
        private System.Windows.Forms.CheckBox checkBoxFullPeriod;

    }
}