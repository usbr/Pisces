namespace Reclamation.TimeSeries.Forms
{
    partial class SeriesProperties
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.textBoxParameter = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxSiteName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxUnits = new System.Windows.Forms.ComboBox();
            this.labelunits = new System.Windows.Forms.Label();
            this.comboBoxTimeInterval = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxParentID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxDBTableName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxSiteID = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxSource = new System.Windows.Forms.TextBox();
            this.textBoxConnectString = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxFileIndex = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.checkBoxActive = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.buttonBuildExpression = new System.Windows.Forms.Button();
            this.textBoxExpression = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.textBoxPOR2 = new System.Windows.Forms.TextBox();
            this.textBoxRecordCount = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxPOR1 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tabPageAdvanced = new System.Windows.Forms.TabPage();
            this.label18 = new System.Windows.Forms.Label();
            this.dgvProperties = new System.Windows.Forms.DataGridView();
            this.label16 = new System.Windows.Forms.Label();
            this.textBoxProvider = new System.Windows.Forms.TextBox();
            this.labelSortOrder = new System.Windows.Forms.Label();
            this.textBoxSortOrder = new System.Windows.Forms.TextBox();
            this.tabPageNotes = new System.Windows.Forms.TabPage();
            this.textBoxNotes = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.tabPageAdvanced.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProperties)).BeginInit();
            this.tabPageNotes.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(333, 428);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 13;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(241, 428);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 12;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "name";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(85, 37);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(281, 20);
            this.textBoxName.TabIndex = 0;
            // 
            // textBoxParameter
            // 
            this.textBoxParameter.Location = new System.Drawing.Point(106, 17);
            this.textBoxParameter.Name = "textBoxParameter";
            this.textBoxParameter.ReadOnly = true;
            this.textBoxParameter.Size = new System.Drawing.Size(245, 20);
            this.textBoxParameter.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "parameter";
            // 
            // textBoxSiteName
            // 
            this.textBoxSiteName.Location = new System.Drawing.Point(106, 147);
            this.textBoxSiteName.Name = "textBoxSiteName";
            this.textBoxSiteName.ReadOnly = true;
            this.textBoxSiteName.Size = new System.Drawing.Size(245, 20);
            this.textBoxSiteName.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "parent id";
            // 
            // comboBoxUnits
            // 
            this.comboBoxUnits.FormattingEnabled = true;
            this.comboBoxUnits.Location = new System.Drawing.Point(85, 64);
            this.comboBoxUnits.Name = "comboBoxUnits";
            this.comboBoxUnits.Size = new System.Drawing.Size(175, 21);
            this.comboBoxUnits.TabIndex = 18;
            // 
            // labelunits
            // 
            this.labelunits.AutoSize = true;
            this.labelunits.Location = new System.Drawing.Point(21, 65);
            this.labelunits.Name = "labelunits";
            this.labelunits.Size = new System.Drawing.Size(29, 13);
            this.labelunits.TabIndex = 19;
            this.labelunits.Text = "units";
            // 
            // comboBoxTimeInterval
            // 
            this.comboBoxTimeInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTimeInterval.Items.AddRange(new object[] {
            "",
            "Daily",
            "Monthly",
            "Irregular",
            "Hourly",
            "Weekly",
            "Yearly"});
            this.comboBoxTimeInterval.Location = new System.Drawing.Point(85, 92);
            this.comboBoxTimeInterval.Name = "comboBoxTimeInterval";
            this.comboBoxTimeInterval.Size = new System.Drawing.Size(175, 21);
            this.comboBoxTimeInterval.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "interval";
            // 
            // textBoxParentID
            // 
            this.textBoxParentID.Location = new System.Drawing.Point(106, 121);
            this.textBoxParentID.Name = "textBoxParentID";
            this.textBoxParentID.ReadOnly = true;
            this.textBoxParentID.Size = new System.Drawing.Size(245, 20);
            this.textBoxParentID.TabIndex = 22;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "site name";
            // 
            // textBoxDBTableName
            // 
            this.textBoxDBTableName.Location = new System.Drawing.Point(106, 173);
            this.textBoxDBTableName.Name = "textBoxDBTableName";
            this.textBoxDBTableName.ReadOnly = true;
            this.textBoxDBTableName.Size = new System.Drawing.Size(245, 20);
            this.textBoxDBTableName.TabIndex = 24;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 176);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 13);
            this.label6.TabIndex = 25;
            this.label6.Text = "table name";
            // 
            // textBoxSiteID
            // 
            this.textBoxSiteID.Location = new System.Drawing.Point(106, 95);
            this.textBoxSiteID.Name = "textBoxSiteID";
            this.textBoxSiteID.ReadOnly = true;
            this.textBoxSiteID.Size = new System.Drawing.Size(245, 20);
            this.textBoxSiteID.TabIndex = 26;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 98);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(15, 13);
            this.label7.TabIndex = 27;
            this.label7.Text = "id";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 72);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 13);
            this.label8.TabIndex = 28;
            this.label8.Text = "source";
            // 
            // textBoxSource
            // 
            this.textBoxSource.Location = new System.Drawing.Point(106, 69);
            this.textBoxSource.Name = "textBoxSource";
            this.textBoxSource.ReadOnly = true;
            this.textBoxSource.Size = new System.Drawing.Size(245, 20);
            this.textBoxSource.TabIndex = 33;
            // 
            // textBoxConnectString
            // 
            this.textBoxConnectString.Location = new System.Drawing.Point(106, 43);
            this.textBoxConnectString.Name = "textBoxConnectString";
            this.textBoxConnectString.Size = new System.Drawing.Size(245, 20);
            this.textBoxConnectString.TabIndex = 34;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 46);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(88, 13);
            this.label11.TabIndex = 35;
            this.label11.Text = "connection string";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(17, 205);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(48, 13);
            this.label13.TabIndex = 38;
            this.label13.Text = "file index";
            // 
            // textBoxFileIndex
            // 
            this.textBoxFileIndex.Location = new System.Drawing.Point(106, 202);
            this.textBoxFileIndex.Name = "textBoxFileIndex";
            this.textBoxFileIndex.ReadOnly = true;
            this.textBoxFileIndex.Size = new System.Drawing.Size(245, 20);
            this.textBoxFileIndex.TabIndex = 39;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(17, 231);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(27, 13);
            this.label14.TabIndex = 41;
            this.label14.Text = "icon";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageGeneral);
            this.tabControl1.Controls.Add(this.tabPageAdvanced);
            this.tabControl1.Controls.Add(this.tabPageNotes);
            this.tabControl1.Location = new System.Drawing.Point(4, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(424, 423);
            this.tabControl1.TabIndex = 43;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.checkBoxActive);
            this.tabPageGeneral.Controls.Add(this.label17);
            this.tabPageGeneral.Controls.Add(this.buttonBuildExpression);
            this.tabPageGeneral.Controls.Add(this.textBoxExpression);
            this.tabPageGeneral.Controls.Add(this.label12);
            this.tabPageGeneral.Controls.Add(this.label15);
            this.tabPageGeneral.Controls.Add(this.textBoxPOR2);
            this.tabPageGeneral.Controls.Add(this.textBoxRecordCount);
            this.tabPageGeneral.Controls.Add(this.label9);
            this.tabPageGeneral.Controls.Add(this.textBoxPOR1);
            this.tabPageGeneral.Controls.Add(this.label10);
            this.tabPageGeneral.Controls.Add(this.label1);
            this.tabPageGeneral.Controls.Add(this.textBoxName);
            this.tabPageGeneral.Controls.Add(this.comboBoxUnits);
            this.tabPageGeneral.Controls.Add(this.labelunits);
            this.tabPageGeneral.Controls.Add(this.label5);
            this.tabPageGeneral.Controls.Add(this.comboBoxTimeInterval);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(416, 397);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // checkBoxActive
            // 
            this.checkBoxActive.AutoSize = true;
            this.checkBoxActive.Location = new System.Drawing.Point(87, 14);
            this.checkBoxActive.Name = "checkBoxActive";
            this.checkBoxActive.Size = new System.Drawing.Size(15, 14);
            this.checkBoxActive.TabIndex = 57;
            this.checkBoxActive.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(19, 14);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(36, 13);
            this.label17.TabIndex = 56;
            this.label17.Text = "active";
            // 
            // buttonBuildExpression
            // 
            this.buttonBuildExpression.Location = new System.Drawing.Point(352, 262);
            this.buttonBuildExpression.Name = "buttonBuildExpression";
            this.buttonBuildExpression.Size = new System.Drawing.Size(28, 23);
            this.buttonBuildExpression.TabIndex = 55;
            this.buttonBuildExpression.Text = "...";
            this.buttonBuildExpression.UseVisualStyleBackColor = true;
            this.buttonBuildExpression.Click += new System.EventHandler(this.buttonBuildExpression_Click);
            // 
            // textBoxExpression
            // 
            this.textBoxExpression.Location = new System.Drawing.Point(101, 262);
            this.textBoxExpression.Name = "textBoxExpression";
            this.textBoxExpression.Size = new System.Drawing.Size(245, 20);
            this.textBoxExpression.TabIndex = 54;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(21, 262);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(57, 13);
            this.label12.TabIndex = 53;
            this.label12.Text = "expression";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(21, 201);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(30, 13);
            this.label15.TabIndex = 51;
            this.label15.Text = "ends";
            // 
            // textBoxPOR2
            // 
            this.textBoxPOR2.Location = new System.Drawing.Point(85, 198);
            this.textBoxPOR2.Name = "textBoxPOR2";
            this.textBoxPOR2.ReadOnly = true;
            this.textBoxPOR2.Size = new System.Drawing.Size(140, 20);
            this.textBoxPOR2.TabIndex = 52;
            // 
            // textBoxRecordCount
            // 
            this.textBoxRecordCount.Location = new System.Drawing.Point(87, 226);
            this.textBoxRecordCount.Name = "textBoxRecordCount";
            this.textBoxRecordCount.ReadOnly = true;
            this.textBoxRecordCount.Size = new System.Drawing.Size(100, 20);
            this.textBoxRecordCount.TabIndex = 50;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 175);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 47;
            this.label9.Text = "begins";
            // 
            // textBoxPOR1
            // 
            this.textBoxPOR1.Location = new System.Drawing.Point(87, 172);
            this.textBoxPOR1.Name = "textBoxPOR1";
            this.textBoxPOR1.ReadOnly = true;
            this.textBoxPOR1.Size = new System.Drawing.Size(138, 20);
            this.textBoxPOR1.TabIndex = 49;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(21, 229);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 13);
            this.label10.TabIndex = 48;
            this.label10.Text = "record count";
            // 
            // tabPageAdvanced
            // 
            this.tabPageAdvanced.Controls.Add(this.label18);
            this.tabPageAdvanced.Controls.Add(this.dgvProperties);
            this.tabPageAdvanced.Controls.Add(this.label16);
            this.tabPageAdvanced.Controls.Add(this.textBoxProvider);
            this.tabPageAdvanced.Controls.Add(this.labelSortOrder);
            this.tabPageAdvanced.Controls.Add(this.textBoxSortOrder);
            this.tabPageAdvanced.Controls.Add(this.label2);
            this.tabPageAdvanced.Controls.Add(this.textBoxParameter);
            this.tabPageAdvanced.Controls.Add(this.label14);
            this.tabPageAdvanced.Controls.Add(this.textBoxSiteName);
            this.tabPageAdvanced.Controls.Add(this.label3);
            this.tabPageAdvanced.Controls.Add(this.textBoxFileIndex);
            this.tabPageAdvanced.Controls.Add(this.textBoxParentID);
            this.tabPageAdvanced.Controls.Add(this.label13);
            this.tabPageAdvanced.Controls.Add(this.label4);
            this.tabPageAdvanced.Controls.Add(this.textBoxDBTableName);
            this.tabPageAdvanced.Controls.Add(this.label6);
            this.tabPageAdvanced.Controls.Add(this.label11);
            this.tabPageAdvanced.Controls.Add(this.textBoxSiteID);
            this.tabPageAdvanced.Controls.Add(this.textBoxConnectString);
            this.tabPageAdvanced.Controls.Add(this.label7);
            this.tabPageAdvanced.Controls.Add(this.textBoxSource);
            this.tabPageAdvanced.Controls.Add(this.label8);
            this.tabPageAdvanced.Location = new System.Drawing.Point(4, 22);
            this.tabPageAdvanced.Name = "tabPageAdvanced";
            this.tabPageAdvanced.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAdvanced.Size = new System.Drawing.Size(416, 397);
            this.tabPageAdvanced.TabIndex = 1;
            this.tabPageAdvanced.Text = "Advanced";
            this.tabPageAdvanced.UseVisualStyleBackColor = true;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(19, 307);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(53, 13);
            this.label18.TabIndex = 48;
            this.label18.Text = "properties";
            // 
            // dgvProperties
            // 
            this.dgvProperties.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProperties.Location = new System.Drawing.Point(106, 284);
            this.dgvProperties.Name = "dgvProperties";
            this.dgvProperties.Size = new System.Drawing.Size(288, 107);
            this.dgvProperties.TabIndex = 47;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(17, 234);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(45, 13);
            this.label16.TabIndex = 46;
            this.label16.Text = "provider";
            // 
            // textBoxProvider
            // 
            this.textBoxProvider.Location = new System.Drawing.Point(106, 232);
            this.textBoxProvider.Name = "textBoxProvider";
            this.textBoxProvider.ReadOnly = true;
            this.textBoxProvider.Size = new System.Drawing.Size(245, 20);
            this.textBoxProvider.TabIndex = 45;
            // 
            // labelSortOrder
            // 
            this.labelSortOrder.AutoSize = true;
            this.labelSortOrder.Location = new System.Drawing.Point(17, 258);
            this.labelSortOrder.Name = "labelSortOrder";
            this.labelSortOrder.Size = new System.Drawing.Size(52, 13);
            this.labelSortOrder.TabIndex = 44;
            this.labelSortOrder.Text = "SortOrder";
            // 
            // textBoxSortOrder
            // 
            this.textBoxSortOrder.Location = new System.Drawing.Point(106, 258);
            this.textBoxSortOrder.Name = "textBoxSortOrder";
            this.textBoxSortOrder.ReadOnly = true;
            this.textBoxSortOrder.Size = new System.Drawing.Size(100, 20);
            this.textBoxSortOrder.TabIndex = 43;
            // 
            // tabPageNotes
            // 
            this.tabPageNotes.Controls.Add(this.textBoxNotes);
            this.tabPageNotes.Location = new System.Drawing.Point(4, 22);
            this.tabPageNotes.Name = "tabPageNotes";
            this.tabPageNotes.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageNotes.Size = new System.Drawing.Size(416, 397);
            this.tabPageNotes.TabIndex = 3;
            this.tabPageNotes.Text = "Notes";
            this.tabPageNotes.UseVisualStyleBackColor = true;
            // 
            // textBoxNotes
            // 
            this.textBoxNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNotes.Location = new System.Drawing.Point(6, 24);
            this.textBoxNotes.Multiline = true;
            this.textBoxNotes.Name = "textBoxNotes";
            this.textBoxNotes.Size = new System.Drawing.Size(366, 347);
            this.textBoxNotes.TabIndex = 0;
            // 
            // SeriesProperties
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(429, 463);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SeriesProperties";
            this.Text = "Series Properties";
            this.tabControl1.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageGeneral.PerformLayout();
            this.tabPageAdvanced.ResumeLayout(false);
            this.tabPageAdvanced.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProperties)).EndInit();
            this.tabPageNotes.ResumeLayout(false);
            this.tabPageNotes.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TextBox textBoxParameter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxSiteName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxUnits;
        private System.Windows.Forms.Label labelunits;
        private System.Windows.Forms.ComboBox comboBoxTimeInterval;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxParentID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxDBTableName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxSiteID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxSource;
        private System.Windows.Forms.TextBox textBoxConnectString;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBoxFileIndex;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabPage tabPageAdvanced;
        private System.Windows.Forms.Label labelSortOrder;
        private System.Windows.Forms.TextBox textBoxSortOrder;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox textBoxPOR2;
        private System.Windows.Forms.TextBox textBoxRecordCount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxPOR1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox textBoxProvider;
        private System.Windows.Forms.TabPage tabPageNotes;
        private System.Windows.Forms.TextBox textBoxNotes;
        private System.Windows.Forms.Button buttonBuildExpression;
        private System.Windows.Forms.TextBox textBoxExpression;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox checkBoxActive;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.DataGridView dgvProperties;
    }
}