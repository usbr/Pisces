namespace Rwis.Sync
{
    partial class rwisForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(rwisForm));
            this.regionSelectionGroupBox = new System.Windows.Forms.GroupBox();
            this.mpRadioButton = new System.Windows.Forms.RadioButton();
            this.ucRadioButton = new System.Windows.Forms.RadioButton();
            this.lcRadioButton = new System.Windows.Forms.RadioButton();
            this.gpRadioButton = new System.Windows.Forms.RadioButton();
            this.pnRadioButton = new System.Windows.Forms.RadioButton();
            this.tstepComboBox = new System.Windows.Forms.ComboBox();
            this.tstepLabel = new System.Windows.Forms.Label();
            this.metadataGroupBox = new System.Windows.Forms.GroupBox();
            this.rwisNameLabel = new System.Windows.Forms.Label();
            this.parameterInfoLabel2 = new System.Windows.Forms.Label();
            this.siteInfoLabel2 = new System.Windows.Forms.Label();
            this.parameterInfoLabel1 = new System.Windows.Forms.Label();
            this.siteInfoLabel1 = new System.Windows.Forms.Label();
            this.selectionInfoLabel = new System.Windows.Forms.Label();
            this.parameterTypeComboBox = new System.Windows.Forms.ComboBox();
            this.parameterTypeLabel = new System.Windows.Forms.Label();
            this.siteTypeComboBox = new System.Windows.Forms.ComboBox();
            this.siteTypeLabel = new System.Windows.Forms.Label();
            this.parameterComboBox = new System.Windows.Forms.ComboBox();
            this.parameterLabel = new System.Windows.Forms.Label();
            this.siteLabel = new System.Windows.Forms.Label();
            this.siteComboBox = new System.Windows.Forms.ComboBox();
            this.regionInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.t2Date = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.t1Date = new System.Windows.Forms.DateTimePicker();
            this.dataProviderLabel = new System.Windows.Forms.Label();
            this.testConnectionButton = new System.Windows.Forms.Button();
            this.conxnStringLabel = new System.Windows.Forms.Label();
            this.sdiLabel = new System.Windows.Forms.Label();
            this.pcodeLabel = new System.Windows.Forms.Label();
            this.siteCodeLabel = new System.Windows.Forms.Label();
            this.sdiTextBox = new System.Windows.Forms.TextBox();
            this.parameterCodeTextBox = new System.Windows.Forms.TextBox();
            this.siteCodeTextbox = new System.Windows.Forms.TextBox();
            this.addToSeriesCatalogButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.siteCatalogButton = new System.Windows.Forms.Button();
            this.parameterCatalogButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.openBatchFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.openBatchFileButton = new System.Windows.Forms.Button();
            this.regionSelectionGroupBox.SuspendLayout();
            this.metadataGroupBox.SuspendLayout();
            this.regionInfoGroupBox.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // regionSelectionGroupBox
            // 
            this.regionSelectionGroupBox.Controls.Add(this.mpRadioButton);
            this.regionSelectionGroupBox.Controls.Add(this.ucRadioButton);
            this.regionSelectionGroupBox.Controls.Add(this.lcRadioButton);
            this.regionSelectionGroupBox.Controls.Add(this.gpRadioButton);
            this.regionSelectionGroupBox.Controls.Add(this.pnRadioButton);
            this.regionSelectionGroupBox.Location = new System.Drawing.Point(13, 12);
            this.regionSelectionGroupBox.Name = "regionSelectionGroupBox";
            this.regionSelectionGroupBox.Size = new System.Drawing.Size(630, 40);
            this.regionSelectionGroupBox.TabIndex = 0;
            this.regionSelectionGroupBox.TabStop = false;
            this.regionSelectionGroupBox.Text = "Region Selection";
            // 
            // mpRadioButton
            // 
            this.mpRadioButton.AutoSize = true;
            this.mpRadioButton.Location = new System.Drawing.Point(513, 17);
            this.mpRadioButton.Name = "mpRadioButton";
            this.mpRadioButton.Size = new System.Drawing.Size(102, 17);
            this.mpRadioButton.TabIndex = 4;
            this.mpRadioButton.TabStop = true;
            this.mpRadioButton.Text = "Mid-Pacific (MP)";
            this.mpRadioButton.UseVisualStyleBackColor = true;
            this.mpRadioButton.CheckedChanged += new System.EventHandler(this.setRegion);
            // 
            // ucRadioButton
            // 
            this.ucRadioButton.AutoSize = true;
            this.ucRadioButton.Location = new System.Drawing.Point(384, 17);
            this.ucRadioButton.Name = "ucRadioButton";
            this.ucRadioButton.Size = new System.Drawing.Size(123, 17);
            this.ucRadioButton.TabIndex = 3;
            this.ucRadioButton.TabStop = true;
            this.ucRadioButton.Text = "Upper Colorado (UC)";
            this.ucRadioButton.UseVisualStyleBackColor = true;
            this.ucRadioButton.CheckedChanged += new System.EventHandler(this.setRegion);
            // 
            // lcRadioButton
            // 
            this.lcRadioButton.AutoSize = true;
            this.lcRadioButton.Location = new System.Drawing.Point(257, 17);
            this.lcRadioButton.Name = "lcRadioButton";
            this.lcRadioButton.Size = new System.Drawing.Size(121, 17);
            this.lcRadioButton.TabIndex = 2;
            this.lcRadioButton.TabStop = true;
            this.lcRadioButton.Text = "Lower Colorado (LC)";
            this.lcRadioButton.UseVisualStyleBackColor = true;
            this.lcRadioButton.CheckedChanged += new System.EventHandler(this.setRegion);
            // 
            // gpRadioButton
            // 
            this.gpRadioButton.AutoSize = true;
            this.gpRadioButton.Location = new System.Drawing.Point(145, 17);
            this.gpRadioButton.Name = "gpRadioButton";
            this.gpRadioButton.Size = new System.Drawing.Size(106, 17);
            this.gpRadioButton.TabIndex = 1;
            this.gpRadioButton.TabStop = true;
            this.gpRadioButton.Text = "Great Plains (GP)";
            this.gpRadioButton.UseVisualStyleBackColor = true;
            this.gpRadioButton.CheckedChanged += new System.EventHandler(this.setRegion);
            // 
            // pnRadioButton
            // 
            this.pnRadioButton.AutoSize = true;
            this.pnRadioButton.Location = new System.Drawing.Point(7, 17);
            this.pnRadioButton.Name = "pnRadioButton";
            this.pnRadioButton.Size = new System.Drawing.Size(132, 17);
            this.pnRadioButton.TabIndex = 0;
            this.pnRadioButton.TabStop = true;
            this.pnRadioButton.Text = "Pacific Northwest (PN)";
            this.pnRadioButton.UseVisualStyleBackColor = true;
            this.pnRadioButton.CheckedChanged += new System.EventHandler(this.setRegion);
            // 
            // tstepComboBox
            // 
            this.tstepComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tstepComboBox.FormattingEnabled = true;
            this.tstepComboBox.Items.AddRange(new object[] {
            "Day",
            "Month"});
            this.tstepComboBox.Location = new System.Drawing.Point(68, 17);
            this.tstepComboBox.Name = "tstepComboBox";
            this.tstepComboBox.Size = new System.Drawing.Size(121, 21);
            this.tstepComboBox.TabIndex = 1;
            this.tstepComboBox.SelectedIndex = 0;
            this.tstepComboBox.SelectedIndexChanged += new System.EventHandler(this.clearParameterComboBox);
            // 
            // tstepLabel
            // 
            this.tstepLabel.AutoSize = true;
            this.tstepLabel.Location = new System.Drawing.Point(6, 20);
            this.tstepLabel.Name = "tstepLabel";
            this.tstepLabel.Size = new System.Drawing.Size(55, 13);
            this.tstepLabel.TabIndex = 2;
            this.tstepLabel.Text = "Time-Step";
            // 
            // metadataGroupBox
            // 
            this.metadataGroupBox.Controls.Add(this.rwisNameLabel);
            this.metadataGroupBox.Controls.Add(this.parameterInfoLabel2);
            this.metadataGroupBox.Controls.Add(this.siteInfoLabel2);
            this.metadataGroupBox.Controls.Add(this.parameterInfoLabel1);
            this.metadataGroupBox.Controls.Add(this.siteInfoLabel1);
            this.metadataGroupBox.Controls.Add(this.selectionInfoLabel);
            this.metadataGroupBox.Controls.Add(this.parameterTypeComboBox);
            this.metadataGroupBox.Controls.Add(this.parameterTypeLabel);
            this.metadataGroupBox.Controls.Add(this.siteTypeComboBox);
            this.metadataGroupBox.Controls.Add(this.siteTypeLabel);
            this.metadataGroupBox.Controls.Add(this.parameterComboBox);
            this.metadataGroupBox.Controls.Add(this.parameterLabel);
            this.metadataGroupBox.Controls.Add(this.tstepComboBox);
            this.metadataGroupBox.Controls.Add(this.tstepLabel);
            this.metadataGroupBox.Controls.Add(this.siteLabel);
            this.metadataGroupBox.Controls.Add(this.siteComboBox);
            this.metadataGroupBox.Location = new System.Drawing.Point(13, 58);
            this.metadataGroupBox.Name = "metadataGroupBox";
            this.metadataGroupBox.Size = new System.Drawing.Size(630, 247);
            this.metadataGroupBox.TabIndex = 3;
            this.metadataGroupBox.TabStop = false;
            this.metadataGroupBox.Text = "Required RWIS Metadata";
            // 
            // rwisNameLabel
            // 
            this.rwisNameLabel.AutoSize = true;
            this.rwisNameLabel.Location = new System.Drawing.Point(34, 224);
            this.rwisNameLabel.Name = "rwisNameLabel";
            this.rwisNameLabel.Size = new System.Drawing.Size(93, 13);
            this.rwisNameLabel.TabIndex = 18;
            this.rwisNameLabel.Text = "RWIS Name: N/A";
            // 
            // parameterInfoLabel2
            // 
            this.parameterInfoLabel2.AutoSize = true;
            this.parameterInfoLabel2.Location = new System.Drawing.Point(109, 206);
            this.parameterInfoLabel2.Name = "parameterInfoLabel2";
            this.parameterInfoLabel2.Size = new System.Drawing.Size(27, 13);
            this.parameterInfoLabel2.TabIndex = 15;
            this.parameterInfoLabel2.Text = "N/A";
            // 
            // siteInfoLabel2
            // 
            this.siteInfoLabel2.AutoSize = true;
            this.siteInfoLabel2.Location = new System.Drawing.Point(79, 168);
            this.siteInfoLabel2.Name = "siteInfoLabel2";
            this.siteInfoLabel2.Size = new System.Drawing.Size(27, 13);
            this.siteInfoLabel2.TabIndex = 14;
            this.siteInfoLabel2.Text = "N/A";
            // 
            // parameterInfoLabel1
            // 
            this.parameterInfoLabel1.AutoSize = true;
            this.parameterInfoLabel1.Location = new System.Drawing.Point(34, 188);
            this.parameterInfoLabel1.Name = "parameterInfoLabel1";
            this.parameterInfoLabel1.Size = new System.Drawing.Size(102, 13);
            this.parameterInfoLabel1.TabIndex = 13;
            this.parameterInfoLabel1.Text = "Parameter Info: N/A";
            // 
            // siteInfoLabel1
            // 
            this.siteInfoLabel1.AutoSize = true;
            this.siteInfoLabel1.Location = new System.Drawing.Point(34, 150);
            this.siteInfoLabel1.Name = "siteInfoLabel1";
            this.siteInfoLabel1.Size = new System.Drawing.Size(72, 13);
            this.siteInfoLabel1.TabIndex = 12;
            this.siteInfoLabel1.Text = "Site Info: N/A";
            // 
            // selectionInfoLabel
            // 
            this.selectionInfoLabel.AutoSize = true;
            this.selectionInfoLabel.Location = new System.Drawing.Point(6, 129);
            this.selectionInfoLabel.Name = "selectionInfoLabel";
            this.selectionInfoLabel.Size = new System.Drawing.Size(197, 13);
            this.selectionInfoLabel.TabIndex = 11;
            this.selectionInfoLabel.Text = "Selected Site and Parameter Information";
            // 
            // parameterTypeComboBox
            // 
            this.parameterTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTypeComboBox.FormattingEnabled = true;
            this.parameterTypeComboBox.Location = new System.Drawing.Point(94, 45);
            this.parameterTypeComboBox.Name = "parameterTypeComboBox";
            this.parameterTypeComboBox.Size = new System.Drawing.Size(520, 21);
            this.parameterTypeComboBox.TabIndex = 9;
            this.parameterTypeComboBox.DropDown += new System.EventHandler(this.getParTypes);
            this.parameterTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.clearParameterComboBox);
            // 
            // parameterTypeLabel
            // 
            this.parameterTypeLabel.AutoSize = true;
            this.parameterTypeLabel.Location = new System.Drawing.Point(6, 48);
            this.parameterTypeLabel.Name = "parameterTypeLabel";
            this.parameterTypeLabel.Size = new System.Drawing.Size(82, 13);
            this.parameterTypeLabel.TabIndex = 10;
            this.parameterTypeLabel.Text = "Parameter Type";
            // 
            // siteTypeComboBox
            // 
            this.siteTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.siteTypeComboBox.FormattingEnabled = true;
            this.siteTypeComboBox.Location = new System.Drawing.Point(258, 17);
            this.siteTypeComboBox.Name = "siteTypeComboBox";
            this.siteTypeComboBox.Size = new System.Drawing.Size(356, 21);
            this.siteTypeComboBox.TabIndex = 7;
            this.siteTypeComboBox.DropDown += new System.EventHandler(this.getSiteTypes);
            this.siteTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.clearSiteComboBox);
            // 
            // siteTypeLabel
            // 
            this.siteTypeLabel.AutoSize = true;
            this.siteTypeLabel.Location = new System.Drawing.Point(196, 20);
            this.siteTypeLabel.Name = "siteTypeLabel";
            this.siteTypeLabel.Size = new System.Drawing.Size(52, 13);
            this.siteTypeLabel.TabIndex = 8;
            this.siteTypeLabel.Text = "Site Type";
            // 
            // parameterComboBox
            // 
            this.parameterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterComboBox.FormattingEnabled = true;
            this.parameterComboBox.Items.AddRange(new object[] {
            "SELECT A TIME-STEP AND PARAMETER TYPE..."});
            this.parameterComboBox.Location = new System.Drawing.Point(68, 100);
            this.parameterComboBox.Name = "parameterComboBox";
            this.parameterComboBox.Size = new System.Drawing.Size(547, 21);
            this.parameterComboBox.TabIndex = 6;
            this.parameterComboBox.DropDown += new System.EventHandler(this.getParametersByTypeTimeStep);
            this.parameterComboBox.SelectedIndexChanged += new System.EventHandler(this.GetParameterInfo);
            // 
            // parameterLabel
            // 
            this.parameterLabel.AutoSize = true;
            this.parameterLabel.Location = new System.Drawing.Point(6, 103);
            this.parameterLabel.Name = "parameterLabel";
            this.parameterLabel.Size = new System.Drawing.Size(55, 13);
            this.parameterLabel.TabIndex = 5;
            this.parameterLabel.Text = "Parameter";
            // 
            // siteLabel
            // 
            this.siteLabel.AutoSize = true;
            this.siteLabel.Location = new System.Drawing.Point(6, 75);
            this.siteLabel.Name = "siteLabel";
            this.siteLabel.Size = new System.Drawing.Size(25, 13);
            this.siteLabel.TabIndex = 4;
            this.siteLabel.Text = "Site";
            // 
            // siteComboBox
            // 
            this.siteComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.siteComboBox.FormattingEnabled = true;
            this.siteComboBox.Items.AddRange(new object[] {
            "SELECT A SITE TYPE..."});
            this.siteComboBox.Location = new System.Drawing.Point(68, 72);
            this.siteComboBox.Name = "siteComboBox";
            this.siteComboBox.Size = new System.Drawing.Size(547, 21);
            this.siteComboBox.TabIndex = 3;
            this.siteComboBox.DropDown += new System.EventHandler(this.getSitesByType);
            this.siteComboBox.SelectedIndexChanged += new System.EventHandler(this.GetSiteInfo);
            // 
            // regionInfoGroupBox
            // 
            this.regionInfoGroupBox.Controls.Add(this.label3);
            this.regionInfoGroupBox.Controls.Add(this.t2Date);
            this.regionInfoGroupBox.Controls.Add(this.label2);
            this.regionInfoGroupBox.Controls.Add(this.t1Date);
            this.regionInfoGroupBox.Controls.Add(this.dataProviderLabel);
            this.regionInfoGroupBox.Controls.Add(this.testConnectionButton);
            this.regionInfoGroupBox.Controls.Add(this.conxnStringLabel);
            this.regionInfoGroupBox.Controls.Add(this.sdiLabel);
            this.regionInfoGroupBox.Controls.Add(this.pcodeLabel);
            this.regionInfoGroupBox.Controls.Add(this.siteCodeLabel);
            this.regionInfoGroupBox.Controls.Add(this.sdiTextBox);
            this.regionInfoGroupBox.Controls.Add(this.parameterCodeTextBox);
            this.regionInfoGroupBox.Controls.Add(this.siteCodeTextbox);
            this.regionInfoGroupBox.Location = new System.Drawing.Point(12, 309);
            this.regionInfoGroupBox.Name = "regionInfoGroupBox";
            this.regionInfoGroupBox.Size = new System.Drawing.Size(631, 126);
            this.regionInfoGroupBox.TabIndex = 4;
            this.regionInfoGroupBox.TabStop = false;
            this.regionInfoGroupBox.Text = "Required Regional Connection Information";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(472, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "to";
            // 
            // t2Date
            // 
            this.t2Date.CustomFormat = "MMM dd, yyyy";
            this.t2Date.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.t2Date.Location = new System.Drawing.Point(492, 45);
            this.t2Date.Name = "t2Date";
            this.t2Date.Size = new System.Drawing.Size(123, 20);
            this.t2Date.TabIndex = 23;
            this.t2Date.Value = new System.DateTime(2016, 7, 13, 15, 32, 50, 219);
            this.t2Date.ValueChanged += new System.EventHandler(this.BuildConnectionString);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(246, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Initial Query Dates";
            // 
            // t1Date
            // 
            this.t1Date.CustomFormat = "MMM dd, yyyy";
            this.t1Date.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.t1Date.Location = new System.Drawing.Point(345, 45);
            this.t1Date.Name = "t1Date";
            this.t1Date.Size = new System.Drawing.Size(123, 20);
            this.t1Date.TabIndex = 21;
            this.t1Date.Value = new System.DateTime(2016, 7, 3, 15, 32, 50, 219);
            this.t1Date.ValueChanged += new System.EventHandler(this.BuildConnectionString);
            // 
            // dataProviderLabel
            // 
            this.dataProviderLabel.AutoSize = true;
            this.dataProviderLabel.Location = new System.Drawing.Point(246, 21);
            this.dataProviderLabel.Name = "dataProviderLabel";
            this.dataProviderLabel.Size = new System.Drawing.Size(78, 13);
            this.dataProviderLabel.TabIndex = 20;
            this.dataProviderLabel.Text = "Data Provider: ";
            // 
            // testConnectionButton
            // 
            this.testConnectionButton.Location = new System.Drawing.Point(473, 97);
            this.testConnectionButton.Name = "testConnectionButton";
            this.testConnectionButton.Size = new System.Drawing.Size(150, 23);
            this.testConnectionButton.TabIndex = 18;
            this.testConnectionButton.Text = "Test Connection...";
            this.testConnectionButton.UseVisualStyleBackColor = true;
            this.testConnectionButton.Click += new System.EventHandler(this.testConnection);
            // 
            // conxnStringLabel
            // 
            this.conxnStringLabel.AutoSize = true;
            this.conxnStringLabel.Location = new System.Drawing.Point(5, 102);
            this.conxnStringLabel.Name = "conxnStringLabel";
            this.conxnStringLabel.Size = new System.Drawing.Size(119, 13);
            this.conxnStringLabel.TabIndex = 17;
            this.conxnStringLabel.Text = "RWIS Connection: N/A";
            // 
            // sdiLabel
            // 
            this.sdiLabel.AutoSize = true;
            this.sdiLabel.Location = new System.Drawing.Point(5, 74);
            this.sdiLabel.Name = "sdiLabel";
            this.sdiLabel.Size = new System.Drawing.Size(89, 13);
            this.sdiLabel.TabIndex = 16;
            this.sdiLabel.Text = "Site DataType ID";
            // 
            // pcodeLabel
            // 
            this.pcodeLabel.AutoSize = true;
            this.pcodeLabel.Location = new System.Drawing.Point(5, 48);
            this.pcodeLabel.Name = "pcodeLabel";
            this.pcodeLabel.Size = new System.Drawing.Size(83, 13);
            this.pcodeLabel.TabIndex = 15;
            this.pcodeLabel.Text = "Parameter Code";
            // 
            // siteCodeLabel
            // 
            this.siteCodeLabel.AutoSize = true;
            this.siteCodeLabel.Location = new System.Drawing.Point(6, 22);
            this.siteCodeLabel.Name = "siteCodeLabel";
            this.siteCodeLabel.Size = new System.Drawing.Size(53, 13);
            this.siteCodeLabel.TabIndex = 14;
            this.siteCodeLabel.Text = "Site Code";
            // 
            // sdiTextBox
            // 
            this.sdiTextBox.Enabled = false;
            this.sdiTextBox.Location = new System.Drawing.Point(105, 71);
            this.sdiTextBox.Name = "sdiTextBox";
            this.sdiTextBox.Size = new System.Drawing.Size(131, 20);
            this.sdiTextBox.TabIndex = 2;
            this.sdiTextBox.Text = "[Enter an SDI]";
            this.sdiTextBox.TextChanged += new System.EventHandler(this.BuildConnectionString);
            // 
            // parameterCodeTextBox
            // 
            this.parameterCodeTextBox.Location = new System.Drawing.Point(105, 45);
            this.parameterCodeTextBox.Name = "parameterCodeTextBox";
            this.parameterCodeTextBox.Size = new System.Drawing.Size(131, 20);
            this.parameterCodeTextBox.TabIndex = 1;
            this.parameterCodeTextBox.Text = "[Enter a Parameter Code]";
            this.parameterCodeTextBox.TextChanged += new System.EventHandler(this.BuildConnectionString);
            // 
            // siteCodeTextbox
            // 
            this.siteCodeTextbox.Location = new System.Drawing.Point(105, 19);
            this.siteCodeTextbox.Name = "siteCodeTextbox";
            this.siteCodeTextbox.Size = new System.Drawing.Size(131, 20);
            this.siteCodeTextbox.TabIndex = 0;
            this.siteCodeTextbox.Text = "[Enter a Site Code]";
            this.siteCodeTextbox.TextChanged += new System.EventHandler(this.BuildConnectionString);
            // 
            // addToSeriesCatalogButton
            // 
            this.addToSeriesCatalogButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.addToSeriesCatalogButton.Location = new System.Drawing.Point(509, 487);
            this.addToSeriesCatalogButton.Name = "addToSeriesCatalogButton";
            this.addToSeriesCatalogButton.Size = new System.Drawing.Size(134, 23);
            this.addToSeriesCatalogButton.TabIndex = 19;
            this.addToSeriesCatalogButton.Text = "Add Dataset to RWIS";
            this.addToSeriesCatalogButton.UseVisualStyleBackColor = true;
            this.addToSeriesCatalogButton.Click += new System.EventHandler(this.addSingleDatasetToRWIS);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(434, 487);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(69, 23);
            this.cancelButton.TabIndex = 20;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CloseForm);
            // 
            // siteCatalogButton
            // 
            this.siteCatalogButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.siteCatalogButton.Location = new System.Drawing.Point(12, 487);
            this.siteCatalogButton.Name = "siteCatalogButton";
            this.siteCatalogButton.Size = new System.Drawing.Size(109, 23);
            this.siteCatalogButton.TabIndex = 21;
            this.siteCatalogButton.Text = "View Site Catalog";
            this.siteCatalogButton.UseVisualStyleBackColor = true;
            this.siteCatalogButton.Click += new System.EventHandler(this.viewSiteCat);
            // 
            // parameterCatalogButton
            // 
            this.parameterCatalogButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.parameterCatalogButton.Location = new System.Drawing.Point(127, 487);
            this.parameterCatalogButton.Name = "parameterCatalogButton";
            this.parameterCatalogButton.Size = new System.Drawing.Size(141, 23);
            this.parameterCatalogButton.TabIndex = 22;
            this.parameterCatalogButton.Text = "View Parameter Catalog";
            this.parameterCatalogButton.UseVisualStyleBackColor = true;
            this.parameterCatalogButton.Click += new System.EventHandler(this.viewParCat);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.toolStripStatusMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 515);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(656, 22);
            this.statusStrip1.TabIndex = 23;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(61, 17);
            this.toolStripStatusLabel.Text = "Messages:";
            // 
            // toolStripStatusMessage
            // 
            this.toolStripStatusMessage.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusMessage.ForeColor = System.Drawing.Color.RoyalBlue;
            this.toolStripStatusMessage.Name = "toolStripStatusMessage";
            this.toolStripStatusMessage.Size = new System.Drawing.Size(42, 17);
            this.toolStripStatusMessage.Text = "-----";
            this.toolStripStatusMessage.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            // 
            // openBatchFileDialog
            // 
            this.openBatchFileDialog.FileName = "openBatchFileDialog";
            // 
            // openBatchFileButton
            // 
            this.openBatchFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.openBatchFileButton.Location = new System.Drawing.Point(12, 441);
            this.openBatchFileButton.Name = "openBatchFileButton";
            this.openBatchFileButton.Size = new System.Drawing.Size(140, 23);
            this.openBatchFileButton.TabIndex = 24;
            this.openBatchFileButton.Text = "Open Batch Upload File";
            this.openBatchFileButton.UseVisualStyleBackColor = true;
            this.openBatchFileButton.Click += new System.EventHandler(this.addMultipleDatasetsToRWIS);

            // 
            // rwisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(656, 537);
            this.Controls.Add(this.openBatchFileButton);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.parameterCatalogButton);
            this.Controls.Add(this.siteCatalogButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.addToSeriesCatalogButton);
            this.Controls.Add(this.regionInfoGroupBox);
            this.Controls.Add(this.metadataGroupBox);
            this.Controls.Add(this.regionSelectionGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(672, 575);
            this.MinimumSize = new System.Drawing.Size(672, 575);
            this.Name = "rwisForm";
            this.Text = "RWIS Database Management Interface";
            this.regionSelectionGroupBox.ResumeLayout(false);
            this.regionSelectionGroupBox.PerformLayout();
            this.metadataGroupBox.ResumeLayout(false);
            this.metadataGroupBox.PerformLayout();
            this.regionInfoGroupBox.ResumeLayout(false);
            this.regionInfoGroupBox.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox regionSelectionGroupBox;
        private System.Windows.Forms.RadioButton mpRadioButton;
        private System.Windows.Forms.RadioButton ucRadioButton;
        private System.Windows.Forms.RadioButton lcRadioButton;
        private System.Windows.Forms.RadioButton gpRadioButton;
        private System.Windows.Forms.RadioButton pnRadioButton;
        private System.Windows.Forms.ComboBox tstepComboBox;
        private System.Windows.Forms.Label tstepLabel;
        private System.Windows.Forms.GroupBox metadataGroupBox;
        private System.Windows.Forms.ComboBox parameterTypeComboBox;
        private System.Windows.Forms.Label parameterTypeLabel;
        private System.Windows.Forms.ComboBox siteTypeComboBox;
        private System.Windows.Forms.Label siteTypeLabel;
        private System.Windows.Forms.ComboBox parameterComboBox;
        private System.Windows.Forms.Label parameterLabel;
        private System.Windows.Forms.Label siteLabel;
        private System.Windows.Forms.ComboBox siteComboBox;
        private System.Windows.Forms.GroupBox regionInfoGroupBox;
        private System.Windows.Forms.Label parameterInfoLabel2;
        private System.Windows.Forms.Label siteInfoLabel2;
        private System.Windows.Forms.Label parameterInfoLabel1;
        private System.Windows.Forms.Label siteInfoLabel1;
        private System.Windows.Forms.Label selectionInfoLabel;
        private System.Windows.Forms.Button testConnectionButton;
        private System.Windows.Forms.Label conxnStringLabel;
        private System.Windows.Forms.Label sdiLabel;
        private System.Windows.Forms.Label pcodeLabel;
        private System.Windows.Forms.Label siteCodeLabel;
        private System.Windows.Forms.TextBox sdiTextBox;
        private System.Windows.Forms.TextBox parameterCodeTextBox;
        private System.Windows.Forms.TextBox siteCodeTextbox;
        private System.Windows.Forms.Button addToSeriesCatalogButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button siteCatalogButton;
        private System.Windows.Forms.Button parameterCatalogButton;
        private System.Windows.Forms.Label rwisNameLabel;
        private System.Windows.Forms.Label dataProviderLabel;
        private System.Windows.Forms.DateTimePicker t2Date;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker t1Date;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusMessage;
        private System.Windows.Forms.OpenFileDialog openBatchFileDialog;
        private System.Windows.Forms.Button openBatchFileButton;
    }
}