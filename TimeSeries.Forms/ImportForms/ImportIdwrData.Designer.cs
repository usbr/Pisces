namespace Reclamation.TimeSeries.Forms.ImportForms
{
    partial class ImportIdwrData
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonAF = new System.Windows.Forms.RadioButton();
            this.radioButtonFB = new System.Windows.Forms.RadioButton();
            this.radioButtonGH = new System.Windows.Forms.RadioButton();
            this.radioButtonQD = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.linkLabelIdwrApiInfo = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxRiverSystems = new System.Windows.Forms.ComboBox();
            this.comboBoxRiverSites = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.labelSID = new System.Windows.Forms.Label();
            this.labelYears = new System.Windows.Forms.Label();
            this.timeSelectorBeginEnd1 = new Reclamation.TimeSeries.Forms.TimeSelectorBeginEnd();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Select an IDWR River System:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 184);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Date range to import";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonAF);
            this.groupBox1.Controls.Add(this.radioButtonFB);
            this.groupBox1.Controls.Add(this.radioButtonGH);
            this.groupBox1.Controls.Add(this.radioButtonQD);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(218, 184);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(199, 115);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select data type to import";
            // 
            // radioButtonAF
            // 
            this.radioButtonAF.AutoSize = true;
            this.radioButtonAF.Location = new System.Drawing.Point(16, 86);
            this.radioButtonAF.Name = "radioButtonAF";
            this.radioButtonAF.Size = new System.Drawing.Size(149, 17);
            this.radioButtonAF.TabIndex = 5;
            this.radioButtonAF.Text = "Reservoir Content (acre-ft)";
            this.radioButtonAF.UseVisualStyleBackColor = true;
            // 
            // radioButtonFB
            // 
            this.radioButtonFB.AutoSize = true;
            this.radioButtonFB.Location = new System.Drawing.Point(16, 68);
            this.radioButtonFB.Name = "radioButtonFB";
            this.radioButtonFB.Size = new System.Drawing.Size(132, 17);
            this.radioButtonFB.TabIndex = 4;
            this.radioButtonFB.Text = "Reservoir Elevation (ft)";
            this.radioButtonFB.UseVisualStyleBackColor = true;
            // 
            // radioButtonGH
            // 
            this.radioButtonGH.AutoSize = true;
            this.radioButtonGH.Location = new System.Drawing.Point(16, 50);
            this.radioButtonGH.Name = "radioButtonGH";
            this.radioButtonGH.Size = new System.Drawing.Size(100, 17);
            this.radioButtonGH.TabIndex = 3;
            this.radioButtonGH.Text = "Gage Height (ft)";
            this.radioButtonGH.UseVisualStyleBackColor = true;
            // 
            // radioButtonQD
            // 
            this.radioButtonQD.AutoSize = true;
            this.radioButtonQD.Checked = true;
            this.radioButtonQD.Location = new System.Drawing.Point(16, 32);
            this.radioButtonQD.Name = "radioButtonQD";
            this.radioButtonQD.Size = new System.Drawing.Size(70, 17);
            this.radioButtonQD.TabIndex = 2;
            this.radioButtonQD.TabStop = true;
            this.radioButtonQD.Text = "Flow (cfs)";
            this.radioButtonQD.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "daily - one value per day";
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(318, 314);
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
            this.buttonCancel.Location = new System.Drawing.Point(404, 314);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // linkLabelIdwrApiInfo
            // 
            this.linkLabelIdwrApiInfo.AutoSize = true;
            this.linkLabelIdwrApiInfo.Location = new System.Drawing.Point(9, 319);
            this.linkLabelIdwrApiInfo.Name = "linkLabelIdwrApiInfo";
            this.linkLabelIdwrApiInfo.Size = new System.Drawing.Size(78, 13);
            this.linkLabelIdwrApiInfo.TabIndex = 9;
            this.linkLabelIdwrApiInfo.TabStop = true;
            this.linkLabelIdwrApiInfo.Text = "IDWR API Info";
            this.linkLabelIdwrApiInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelIdwrInfo_LinkClicked);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Select an IDWR River Site:";
            // 
            // comboBoxRiverSystems
            // 
            this.comboBoxRiverSystems.FormattingEnabled = true;
            this.comboBoxRiverSystems.Location = new System.Drawing.Point(13, 26);
            this.comboBoxRiverSystems.Name = "comboBoxRiverSystems";
            this.comboBoxRiverSystems.Size = new System.Drawing.Size(471, 21);
            this.comboBoxRiverSystems.TabIndex = 11;
            this.comboBoxRiverSystems.DropDown += new System.EventHandler(this.comboBoxRiverSystems_OnDropDown);
            this.comboBoxRiverSystems.SelectedIndexChanged += new System.EventHandler(this.comboBoxRiverSystems_SelectedIndexChanged);
            // 
            // comboBoxRiverSites
            // 
            this.comboBoxRiverSites.FormattingEnabled = true;
            this.comboBoxRiverSites.Location = new System.Drawing.Point(12, 71);
            this.comboBoxRiverSites.Name = "comboBoxRiverSites";
            this.comboBoxRiverSites.Size = new System.Drawing.Size(472, 21);
            this.comboBoxRiverSites.TabIndex = 12;
            this.comboBoxRiverSites.SelectedIndexChanged += new System.EventHandler(this.comboBoxRiverSites_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(161, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Selected IDWR Site Information:";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(24, 119);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(41, 13);
            this.labelName.TabIndex = 14;
            this.labelName.Text = "Name: ";
            // 
            // labelSID
            // 
            this.labelSID.AutoSize = true;
            this.labelSID.Location = new System.Drawing.Point(24, 138);
            this.labelSID.Name = "labelSID";
            this.labelSID.Size = new System.Drawing.Size(45, 13);
            this.labelSID.TabIndex = 15;
            this.labelSID.Text = "Site ID: ";
            // 
            // labelYears
            // 
            this.labelYears.AutoSize = true;
            this.labelYears.Location = new System.Drawing.Point(24, 157);
            this.labelYears.Name = "labelYears";
            this.labelYears.Size = new System.Drawing.Size(86, 13);
            this.labelYears.TabIndex = 16;
            this.labelYears.Text = "Years Available: ";
            // 
            // timeSelectorBeginEnd1
            // 
            this.timeSelectorBeginEnd1.Location = new System.Drawing.Point(13, 200);
            this.timeSelectorBeginEnd1.Name = "timeSelectorBeginEnd1";
            this.timeSelectorBeginEnd1.ShowTime = false;
            this.timeSelectorBeginEnd1.Size = new System.Drawing.Size(199, 46);
            this.timeSelectorBeginEnd1.T1 = new System.DateTime(2008, 3, 12, 7, 55, 34, 320);
            this.timeSelectorBeginEnd1.T2 = new System.DateTime(2008, 3, 12, 7, 55, 34, 320);
            this.timeSelectorBeginEnd1.TabIndex = 1;
            // 
            // ImportIdwrData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 349);
            this.Controls.Add(this.labelYears);
            this.Controls.Add(this.labelSID);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxRiverSites);
            this.Controls.Add(this.comboBoxRiverSystems);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.linkLabelIdwrApiInfo);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.timeSelectorBeginEnd1);
            this.MinimumSize = new System.Drawing.Size(507, 387);
            this.Name = "ImportIdwrData";
            this.Text = "Import IDWR Data";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TimeSelectorBeginEnd timeSelectorBeginEnd1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.RadioButton radioButtonAF;
        private System.Windows.Forms.RadioButton radioButtonFB;
        private System.Windows.Forms.RadioButton radioButtonGH;
        private System.Windows.Forms.RadioButton radioButtonQD;
        private System.Windows.Forms.LinkLabel linkLabelIdwrApiInfo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxRiverSystems;
        private System.Windows.Forms.ComboBox comboBoxRiverSites;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelSID;
        private System.Windows.Forms.Label labelYears;
    }
}