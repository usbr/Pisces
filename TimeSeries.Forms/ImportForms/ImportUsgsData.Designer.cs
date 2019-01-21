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
            this.linkLabelUSGSInfo = new System.Windows.Forms.LinkLabel();
            this.radioButtonCustomDaily = new System.Windows.Forms.RadioButton();
            this.textBoxCustomDaily = new System.Windows.Forms.TextBox();
            this.textBoxCustomRealTime = new System.Windows.Forms.TextBox();
            this.radioButtonCustomRealTime = new System.Windows.Forms.RadioButton();
            this.timeSelectorBeginEnd1 = new Reclamation.TimeSeries.Forms.TimeSelectorBeginEnd();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxSiteNumber
            // 
            this.textBoxSiteNumber.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Reclamation.TimeSeries.Forms.Properties.Settings.Default, "UsgsSiteNumber", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxSiteNumber.Location = new System.Drawing.Point(36, 59);
            this.textBoxSiteNumber.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.textBoxSiteNumber.Name = "textBoxSiteNumber";
            this.textBoxSiteNumber.Size = new System.Drawing.Size(598, 38);
            this.textBoxSiteNumber.TabIndex = 4;
            this.textBoxSiteNumber.Text = global::Reclamation.TimeSeries.Forms.Properties.Settings.Default.UsgsSiteNumber;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(592, 32);
            this.label1.TabIndex = 3;
            this.label1.Text = "Enter Comma Seperated USGS Site Numbers";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(746, 21);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(422, 32);
            this.label2.TabIndex = 5;
            this.label2.Text = "Date range to import from USGS";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxCustomRealTime);
            this.groupBox1.Controls.Add(this.radioButtonCustomRealTime);
            this.groupBox1.Controls.Add(this.textBoxCustomDaily);
            this.groupBox1.Controls.Add(this.radioButtonCustomDaily);
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
            this.groupBox1.Location = new System.Drawing.Point(36, 193);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.groupBox1.Size = new System.Drawing.Size(1186, 549);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select data type to import";
            // 
            // radioButtonResElevation
            // 
            this.radioButtonResElevation.AutoSize = true;
            this.radioButtonResElevation.Location = new System.Drawing.Point(61, 456);
            this.radioButtonResElevation.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.radioButtonResElevation.Name = "radioButtonResElevation";
            this.radioButtonResElevation.Size = new System.Drawing.Size(358, 36);
            this.radioButtonResElevation.TabIndex = 10;
            this.radioButtonResElevation.Text = "reservoir elevation (feet)";
            this.radioButtonResElevation.UseVisualStyleBackColor = true;
            // 
            // radioButtonGwLevels
            // 
            this.radioButtonGwLevels.AutoSize = true;
            this.radioButtonGwLevels.Location = new System.Drawing.Point(576, 92);
            this.radioButtonGwLevels.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.radioButtonGwLevels.Name = "radioButtonGwLevels";
            this.radioButtonGwLevels.Size = new System.Drawing.Size(291, 36);
            this.radioButtonGwLevels.TabIndex = 9;
            this.radioButtonGwLevels.Text = "groundwater levels";
            this.radioButtonGwLevels.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(576, 49);
            this.label5.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 32);
            this.label5.TabIndex = 8;
            this.label5.Text = "other";
            // 
            // radioButtonRtGageHt
            // 
            this.radioButtonRtGageHt.AutoSize = true;
            this.radioButtonRtGageHt.Location = new System.Drawing.Point(61, 413);
            this.radioButtonRtGageHt.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.radioButtonRtGageHt.Name = "radioButtonRtGageHt";
            this.radioButtonRtGageHt.Size = new System.Drawing.Size(275, 36);
            this.radioButtonRtGageHt.TabIndex = 7;
            this.radioButtonRtGageHt.Text = "gage height (feet)";
            this.radioButtonRtGageHt.UseVisualStyleBackColor = true;
            // 
            // radioButtonRtFlow
            // 
            this.radioButtonRtFlow.AutoSize = true;
            this.radioButtonRtFlow.Location = new System.Drawing.Point(61, 370);
            this.radioButtonRtFlow.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.radioButtonRtFlow.Name = "radioButtonRtFlow";
            this.radioButtonRtFlow.Size = new System.Drawing.Size(164, 36);
            this.radioButtonRtFlow.TabIndex = 6;
            this.radioButtonRtFlow.Text = "flow (cfs)";
            this.radioButtonRtFlow.UseVisualStyleBackColor = true;
            // 
            // radioButtonTempMin
            // 
            this.radioButtonTempMin.AutoSize = true;
            this.radioButtonTempMin.Location = new System.Drawing.Point(61, 216);
            this.radioButtonTempMin.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.radioButtonTempMin.Name = "radioButtonTempMin";
            this.radioButtonTempMin.Size = new System.Drawing.Size(276, 36);
            this.radioButtonTempMin.TabIndex = 5;
            this.radioButtonTempMin.Text = "temperature (min)";
            this.radioButtonTempMin.UseVisualStyleBackColor = true;
            // 
            // radioButtonTempMax
            // 
            this.radioButtonTempMax.AutoSize = true;
            this.radioButtonTempMax.Location = new System.Drawing.Point(61, 173);
            this.radioButtonTempMax.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.radioButtonTempMax.Name = "radioButtonTempMax";
            this.radioButtonTempMax.Size = new System.Drawing.Size(283, 36);
            this.radioButtonTempMax.TabIndex = 4;
            this.radioButtonTempMax.Text = "temperature (max)";
            this.radioButtonTempMax.UseVisualStyleBackColor = true;
            // 
            // radioButtonTempMean
            // 
            this.radioButtonTempMean.AutoSize = true;
            this.radioButtonTempMean.Location = new System.Drawing.Point(61, 130);
            this.radioButtonTempMean.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.radioButtonTempMean.Name = "radioButtonTempMean";
            this.radioButtonTempMean.Size = new System.Drawing.Size(301, 36);
            this.radioButtonTempMean.TabIndex = 3;
            this.radioButtonTempMean.Text = "temperature (mean)";
            this.radioButtonTempMean.UseVisualStyleBackColor = true;
            // 
            // radioButtonMeanFlow
            // 
            this.radioButtonMeanFlow.AutoSize = true;
            this.radioButtonMeanFlow.Checked = true;
            this.radioButtonMeanFlow.Location = new System.Drawing.Point(61, 87);
            this.radioButtonMeanFlow.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.radioButtonMeanFlow.Name = "radioButtonMeanFlow";
            this.radioButtonMeanFlow.Size = new System.Drawing.Size(164, 36);
            this.radioButtonMeanFlow.TabIndex = 2;
            this.radioButtonMeanFlow.TabStop = true;
            this.radioButtonMeanFlow.Text = "flow (cfs)";
            this.radioButtonMeanFlow.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(53, 327);
            this.label4.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(456, 32);
            this.label4.TabIndex = 1;
            this.label4.Text = "real time - typically every 15 mintes";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(53, 49);
            this.label3.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(323, 32);
            this.label3.TabIndex = 0;
            this.label3.Text = "daily - one value per day";
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(806, 767);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(200, 55);
            this.buttonOK.TabIndex = 8;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(1022, 767);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(200, 55);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // linkLabelUSGSInfo
            // 
            this.linkLabelUSGSInfo.AutoSize = true;
            this.linkLabelUSGSInfo.Location = new System.Drawing.Point(466, 126);
            this.linkLabelUSGSInfo.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.linkLabelUSGSInfo.Name = "linkLabelUSGSInfo";
            this.linkLabelUSGSInfo.Size = new System.Drawing.Size(168, 32);
            this.linkLabelUSGSInfo.TabIndex = 9;
            this.linkLabelUSGSInfo.TabStop = true;
            this.linkLabelUSGSInfo.Text = "info for sites";
            this.linkLabelUSGSInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelUSGSInfo_LinkClicked);
            // 
            // radioButtonCustomDaily
            // 
            this.radioButtonCustomDaily.AutoSize = true;
            this.radioButtonCustomDaily.Location = new System.Drawing.Point(62, 266);
            this.radioButtonCustomDaily.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.radioButtonCustomDaily.Name = "radioButtonCustomDaily";
            this.radioButtonCustomDaily.Size = new System.Drawing.Size(275, 36);
            this.radioButtonCustomDaily.TabIndex = 11;
            this.radioButtonCustomDaily.Text = "custom data code";
            this.radioButtonCustomDaily.UseVisualStyleBackColor = true;
            // 
            // textBoxCustomDaily
            // 
            this.textBoxCustomDaily.Location = new System.Drawing.Point(348, 266);
            this.textBoxCustomDaily.Name = "textBoxCustomDaily";
            this.textBoxCustomDaily.Size = new System.Drawing.Size(161, 38);
            this.textBoxCustomDaily.TabIndex = 12;
            // 
            // textBoxCustomRealTime
            // 
            this.textBoxCustomRealTime.Location = new System.Drawing.Point(348, 501);
            this.textBoxCustomRealTime.Name = "textBoxCustomRealTime";
            this.textBoxCustomRealTime.Size = new System.Drawing.Size(161, 38);
            this.textBoxCustomRealTime.TabIndex = 14;
            // 
            // radioButtonCustomRealTime
            // 
            this.radioButtonCustomRealTime.AutoSize = true;
            this.radioButtonCustomRealTime.Location = new System.Drawing.Point(62, 501);
            this.radioButtonCustomRealTime.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.radioButtonCustomRealTime.Name = "radioButtonCustomRealTime";
            this.radioButtonCustomRealTime.Size = new System.Drawing.Size(275, 36);
            this.radioButtonCustomRealTime.TabIndex = 13;
            this.radioButtonCustomRealTime.Text = "custom data code";
            this.radioButtonCustomRealTime.UseVisualStyleBackColor = true;
            // 
            // timeSelectorBeginEnd1
            // 
            this.timeSelectorBeginEnd1.Location = new System.Drawing.Point(728, 59);
            this.timeSelectorBeginEnd1.Margin = new System.Windows.Forms.Padding(21, 17, 21, 17);
            this.timeSelectorBeginEnd1.Name = "timeSelectorBeginEnd1";
            this.timeSelectorBeginEnd1.ShowTime = false;
            this.timeSelectorBeginEnd1.Size = new System.Drawing.Size(520, 110);
            this.timeSelectorBeginEnd1.T1 = new System.DateTime(2008, 3, 12, 7, 55, 34, 320);
            this.timeSelectorBeginEnd1.T2 = new System.DateTime(2008, 3, 12, 7, 55, 34, 320);
            this.timeSelectorBeginEnd1.TabIndex = 1;
            // 
            // ImportUsgsData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1236, 838);
            this.Controls.Add(this.linkLabelUSGSInfo);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxSiteNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.timeSelectorBeginEnd1);
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
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
        private System.Windows.Forms.TextBox textBoxCustomDaily;
        private System.Windows.Forms.RadioButton radioButtonCustomDaily;
        private System.Windows.Forms.TextBox textBoxCustomRealTime;
        private System.Windows.Forms.RadioButton radioButtonCustomRealTime;
    }
}