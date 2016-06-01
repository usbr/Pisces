namespace Reclamation.TimeSeries.Forms.ImportForms
{
    partial class ImportUsgsData
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
            this.textBoxSiteNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonResElevation = new System.Windows.Forms.RadioButton();
            this.radioButtonGwLevels = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.radioButtonRtGageHt = new System.Windows.Forms.RadioButton();
            this.radioButtonRtFlow = new System.Windows.Forms.RadioButton();
            this.radioButtonTempMin = new System.Windows.Forms.RadioButton();
            this.radioButtonTempMax = new System.Windows.Forms.RadioButton();
            this.radioButtonTempMean = new System.Windows.Forms.RadioButton();
            this.radioButtonMeanFlow = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.timeSelectorBeginEnd1 = new Reclamation.TimeSeries.Forms.TimeSelectorBeginEnd();
            this.linkLabelUSGSInfo = new System.Windows.Forms.LinkLabel();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxSiteNumber
            // 
            this.textBoxSiteNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Reclamation.TimeSeries.Forms.Properties.Settings.Default, "UsgsSiteNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxSiteNumber.Location = new System.Drawing.Point(25, 44);
            this.textBoxSiteNumber.Name = "textBoxSiteNumber";
            this.textBoxSiteNumber.Size = new System.Drawing.Size(215, 20);
            this.textBoxSiteNumber.TabIndex = 4;
            this.textBoxSiteNumber.Text = global::Reclamation.TimeSeries.Forms.Properties.Settings.Default.UsgsSiteNumber;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(221, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Enter Comma Seperated USGS Site Numbers";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(317, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(159, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Date range to import from USGS";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonResElevation);
            this.groupBox1.Controls.Add(this.radioButtonGwLevels);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.radioButtonRtGageHt);
            this.groupBox1.Controls.Add(this.radioButtonRtFlow);
            this.groupBox1.Controls.Add(this.radioButtonTempMin);
            this.groupBox1.Controls.Add(this.radioButtonTempMax);
            this.groupBox1.Controls.Add(this.radioButtonTempMean);
            this.groupBox1.Controls.Add(this.radioButtonMeanFlow);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(97, 105);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(373, 212);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select data type to import";
            // 
            // radioButtonResElevation
            // 
            this.radioButtonResElevation.AutoSize = true;
            this.radioButtonResElevation.Location = new System.Drawing.Point(27, 187);
            this.radioButtonResElevation.Name = "radioButtonResElevation";
            this.radioButtonResElevation.Size = new System.Drawing.Size(138, 17);
            this.radioButtonResElevation.TabIndex = 10;
            this.radioButtonResElevation.Text = "reservoir elevation (feet)";
            this.radioButtonResElevation.UseVisualStyleBackColor = true;
            // 
            // radioButtonGwLevels
            // 
            this.radioButtonGwLevels.AutoSize = true;
            this.radioButtonGwLevels.Location = new System.Drawing.Point(220, 59);
            this.radioButtonGwLevels.Name = "radioButtonGwLevels";
            this.radioButtonGwLevels.Size = new System.Drawing.Size(114, 17);
            this.radioButtonGwLevels.TabIndex = 9;
            this.radioButtonGwLevels.Text = "groundwater levels";
            this.radioButtonGwLevels.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(220, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(31, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "other";
            // 
            // radioButtonRtGageHt
            // 
            this.radioButtonRtGageHt.AutoSize = true;
            this.radioButtonRtGageHt.Location = new System.Drawing.Point(27, 169);
            this.radioButtonRtGageHt.Name = "radioButtonRtGageHt";
            this.radioButtonRtGageHt.Size = new System.Drawing.Size(108, 17);
            this.radioButtonRtGageHt.TabIndex = 7;
            this.radioButtonRtGageHt.Text = "gage height (feet)";
            this.radioButtonRtGageHt.UseVisualStyleBackColor = true;
            // 
            // radioButtonRtFlow
            // 
            this.radioButtonRtFlow.AutoSize = true;
            this.radioButtonRtFlow.Location = new System.Drawing.Point(27, 151);
            this.radioButtonRtFlow.Name = "radioButtonRtFlow";
            this.radioButtonRtFlow.Size = new System.Drawing.Size(67, 17);
            this.radioButtonRtFlow.TabIndex = 6;
            this.radioButtonRtFlow.Text = "flow (cfs)";
            this.radioButtonRtFlow.UseVisualStyleBackColor = true;
            // 
            // radioButtonTempMin
            // 
            this.radioButtonTempMin.AutoSize = true;
            this.radioButtonTempMin.Location = new System.Drawing.Point(27, 111);
            this.radioButtonTempMin.Name = "radioButtonTempMin";
            this.radioButtonTempMin.Size = new System.Drawing.Size(106, 17);
            this.radioButtonTempMin.TabIndex = 5;
            this.radioButtonTempMin.Text = "temperature (min)";
            this.radioButtonTempMin.UseVisualStyleBackColor = true;
            // 
            // radioButtonTempMax
            // 
            this.radioButtonTempMax.AutoSize = true;
            this.radioButtonTempMax.Location = new System.Drawing.Point(27, 93);
            this.radioButtonTempMax.Name = "radioButtonTempMax";
            this.radioButtonTempMax.Size = new System.Drawing.Size(109, 17);
            this.radioButtonTempMax.TabIndex = 4;
            this.radioButtonTempMax.Text = "temperature (max)";
            this.radioButtonTempMax.UseVisualStyleBackColor = true;
            // 
            // radioButtonTempMean
            // 
            this.radioButtonTempMean.AutoSize = true;
            this.radioButtonTempMean.Location = new System.Drawing.Point(27, 75);
            this.radioButtonTempMean.Name = "radioButtonTempMean";
            this.radioButtonTempMean.Size = new System.Drawing.Size(116, 17);
            this.radioButtonTempMean.TabIndex = 3;
            this.radioButtonTempMean.Text = "temperature (mean)";
            this.radioButtonTempMean.UseVisualStyleBackColor = true;
            // 
            // radioButtonMeanFlow
            // 
            this.radioButtonMeanFlow.AutoSize = true;
            this.radioButtonMeanFlow.Checked = true;
            this.radioButtonMeanFlow.Location = new System.Drawing.Point(27, 57);
            this.radioButtonMeanFlow.Name = "radioButtonMeanFlow";
            this.radioButtonMeanFlow.Size = new System.Drawing.Size(67, 17);
            this.radioButtonMeanFlow.TabIndex = 2;
            this.radioButtonMeanFlow.TabStop = true;
            this.radioButtonMeanFlow.Text = "flow (cfs)";
            this.radioButtonMeanFlow.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(24, 133);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(169, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "real time - typically every 15 mintes";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(24, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "daily - one value per day";
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(304, 341);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 8;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(395, 341);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // timeSelectorBeginEnd1
            // 
            this.timeSelectorBeginEnd1.Location = new System.Drawing.Point(288, 28);
            this.timeSelectorBeginEnd1.Name = "timeSelectorBeginEnd1";
            this.timeSelectorBeginEnd1.ShowTime = false;
            this.timeSelectorBeginEnd1.Size = new System.Drawing.Size(195, 46);
            this.timeSelectorBeginEnd1.T1 = new System.DateTime(2008, 3, 12, 7, 55, 34, 320);
            this.timeSelectorBeginEnd1.T2 = new System.DateTime(2008, 3, 12, 7, 55, 34, 320);
            this.timeSelectorBeginEnd1.TabIndex = 1;
            // 
            // linkLabelUSGSInfo
            // 
            this.linkLabelUSGSInfo.AutoSize = true;
            this.linkLabelUSGSInfo.Location = new System.Drawing.Point(25, 71);
            this.linkLabelUSGSInfo.Name = "linkLabelUSGSInfo";
            this.linkLabelUSGSInfo.Size = new System.Drawing.Size(63, 13);
            this.linkLabelUSGSInfo.TabIndex = 9;
            this.linkLabelUSGSInfo.TabStop = true;
            this.linkLabelUSGSInfo.Text = "info for sites";
            this.linkLabelUSGSInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelUSGSInfo_LinkClicked);
            // 
            // ImportUsgsData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 376);
            this.Controls.Add(this.linkLabelUSGSInfo);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxSiteNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.timeSelectorBeginEnd1);
            this.Name = "ImportUsgsData";
            this.Text = "Import USGS Data";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TimeSelectorBeginEnd timeSelectorBeginEnd1;
        private System.Windows.Forms.TextBox textBoxSiteNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.RadioButton radioButtonRtGageHt;
        private System.Windows.Forms.RadioButton radioButtonRtFlow;
        private System.Windows.Forms.RadioButton radioButtonTempMin;
        private System.Windows.Forms.RadioButton radioButtonTempMax;
        private System.Windows.Forms.RadioButton radioButtonTempMean;
        private System.Windows.Forms.RadioButton radioButtonMeanFlow;
        private System.Windows.Forms.RadioButton radioButtonGwLevels;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.LinkLabel linkLabelUSGSInfo;
        private System.Windows.Forms.RadioButton radioButtonResElevation;
    }
}