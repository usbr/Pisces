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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportIdwrData));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.tabPageStream = new System.Windows.Forms.TabPage();
            this.radioButtonNatQ = new System.Windows.Forms.RadioButton();
            this.radioButtonGainQ = new System.Windows.Forms.RadioButton();
            this.radioButtonActQ = new System.Windows.Forms.RadioButton();
            this.radioButtonStorQ = new System.Windows.Forms.RadioButton();
            this.tabPageReservoir = new System.Windows.Forms.TabPage();
            this.radioButtonCurrAf = new System.Windows.Forms.RadioButton();
            this.radioButtonResEvap = new System.Windows.Forms.RadioButton();
            this.radioButtonTotAcc = new System.Windows.Forms.RadioButton();
            this.radioButtonTotEvap = new System.Windows.Forms.RadioButton();
            this.radioButtonAccStor = new System.Windows.Forms.RadioButton();
            this.tabPageDiversion = new System.Windows.Forms.TabPage();
            this.radioButtonRemStor = new System.Windows.Forms.RadioButton();
            this.radioButtonDivFlow = new System.Windows.Forms.RadioButton();
            this.radioButtonStorDiv2Date = new System.Windows.Forms.RadioButton();
            this.radioButtonTotDiv2Date = new System.Windows.Forms.RadioButton();
            this.radioButtonStorDiv = new System.Windows.Forms.RadioButton();
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
            this.labelSType = new System.Windows.Forms.Label();
            this.textBoxSID = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.radioButtonHistorical = new System.Windows.Forms.RadioButton();
            this.radioButtonAccounting = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.radioButtonSiteNameSort = new System.Windows.Forms.RadioButton();
            this.radioButtonSiteIdSort = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.timeSelectorBeginEnd1 = new Reclamation.TimeSeries.Forms.TimeSelectorBeginEnd();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBoxSiteFilter = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageStream.SuspendLayout();
            this.tabPageReservoir.SuspendLayout();
            this.tabPageDiversion.SuspendLayout();
            this.statusStrip1.SuspendLayout();
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
            this.label2.Location = new System.Drawing.Point(24, 289);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Date range to import";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Controls.Add(this.radioButtonAF);
            this.groupBox1.Controls.Add(this.radioButtonFB);
            this.groupBox1.Controls.Add(this.radioButtonGH);
            this.groupBox1.Controls.Add(this.radioButtonQD);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(8, 327);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(400, 135);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select data type to import";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageMain);
            this.tabControl1.Controls.Add(this.tabPageStream);
            this.tabControl1.Controls.Add(this.tabPageReservoir);
            this.tabControl1.Controls.Add(this.tabPageDiversion);
            this.tabControl1.Location = new System.Drawing.Point(171, 9);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(223, 120);
            this.tabControl1.TabIndex = 23;
            // 
            // tabPageMain
            // 
            this.tabPageMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPageMain.Location = new System.Drawing.Point(4, 22);
            this.tabPageMain.Name = "tabPageMain";
            this.tabPageMain.Size = new System.Drawing.Size(215, 94);
            this.tabPageMain.TabIndex = 3;
            this.tabPageMain.Text = "Accounting";
            this.tabPageMain.UseVisualStyleBackColor = true;
            // 
            // tabPageStream
            // 
            this.tabPageStream.Controls.Add(this.radioButtonNatQ);
            this.tabPageStream.Controls.Add(this.radioButtonGainQ);
            this.tabPageStream.Controls.Add(this.radioButtonActQ);
            this.tabPageStream.Controls.Add(this.radioButtonStorQ);
            this.tabPageStream.Location = new System.Drawing.Point(4, 22);
            this.tabPageStream.Name = "tabPageStream";
            this.tabPageStream.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageStream.Size = new System.Drawing.Size(215, 94);
            this.tabPageStream.TabIndex = 0;
            this.tabPageStream.UseVisualStyleBackColor = true;
            // 
            // radioButtonNatQ
            // 
            this.radioButtonNatQ.AutoSize = true;
            this.radioButtonNatQ.Location = new System.Drawing.Point(6, 1);
            this.radioButtonNatQ.Name = "radioButtonNatQ";
            this.radioButtonNatQ.Size = new System.Drawing.Size(107, 17);
            this.radioButtonNatQ.TabIndex = 7;
            this.radioButtonNatQ.Text = "Natural Flow (cfs)";
            this.radioButtonNatQ.UseVisualStyleBackColor = true;
            // 
            // radioButtonGainQ
            // 
            this.radioButtonGainQ.AutoSize = true;
            this.radioButtonGainQ.Location = new System.Drawing.Point(6, 55);
            this.radioButtonGainQ.Name = "radioButtonGainQ";
            this.radioButtonGainQ.Size = new System.Drawing.Size(105, 17);
            this.radioButtonGainQ.TabIndex = 10;
            this.radioButtonGainQ.Text = "Reach Gain (cfs)";
            this.radioButtonGainQ.UseVisualStyleBackColor = true;
            // 
            // radioButtonActQ
            // 
            this.radioButtonActQ.AutoSize = true;
            this.radioButtonActQ.Location = new System.Drawing.Point(6, 19);
            this.radioButtonActQ.Name = "radioButtonActQ";
            this.radioButtonActQ.Size = new System.Drawing.Size(103, 17);
            this.radioButtonActQ.TabIndex = 8;
            this.radioButtonActQ.Text = "Actual Flow (cfs)";
            this.radioButtonActQ.UseVisualStyleBackColor = true;
            // 
            // radioButtonStorQ
            // 
            this.radioButtonStorQ.AutoSize = true;
            this.radioButtonStorQ.Location = new System.Drawing.Point(6, 37);
            this.radioButtonStorQ.Name = "radioButtonStorQ";
            this.radioButtonStorQ.Size = new System.Drawing.Size(104, 17);
            this.radioButtonStorQ.TabIndex = 9;
            this.radioButtonStorQ.Text = "Stored Flow (cfs)";
            this.radioButtonStorQ.UseVisualStyleBackColor = true;
            // 
            // tabPageReservoir
            // 
            this.tabPageReservoir.Controls.Add(this.radioButtonCurrAf);
            this.tabPageReservoir.Controls.Add(this.radioButtonResEvap);
            this.tabPageReservoir.Controls.Add(this.radioButtonTotAcc);
            this.tabPageReservoir.Controls.Add(this.radioButtonTotEvap);
            this.tabPageReservoir.Controls.Add(this.radioButtonAccStor);
            this.tabPageReservoir.Location = new System.Drawing.Point(4, 22);
            this.tabPageReservoir.Name = "tabPageReservoir";
            this.tabPageReservoir.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageReservoir.Size = new System.Drawing.Size(215, 94);
            this.tabPageReservoir.TabIndex = 1;
            this.tabPageReservoir.UseVisualStyleBackColor = true;
            // 
            // radioButtonCurrAf
            // 
            this.radioButtonCurrAf.AutoSize = true;
            this.radioButtonCurrAf.Location = new System.Drawing.Point(6, 74);
            this.radioButtonCurrAf.Name = "radioButtonCurrAf";
            this.radioButtonCurrAf.Size = new System.Drawing.Size(134, 17);
            this.radioButtonCurrAf.TabIndex = 15;
            this.radioButtonCurrAf.Text = "Current Contents (ac-ft)";
            this.radioButtonCurrAf.UseVisualStyleBackColor = true;
            // 
            // radioButtonResEvap
            // 
            this.radioButtonResEvap.AutoSize = true;
            this.radioButtonResEvap.Location = new System.Drawing.Point(6, 1);
            this.radioButtonResEvap.Name = "radioButtonResEvap";
            this.radioButtonResEvap.Size = new System.Drawing.Size(153, 17);
            this.radioButtonResEvap.TabIndex = 11;
            this.radioButtonResEvap.Text = "Reservoir Evaporation (cfs)";
            this.radioButtonResEvap.UseVisualStyleBackColor = true;
            // 
            // radioButtonTotAcc
            // 
            this.radioButtonTotAcc.AutoSize = true;
            this.radioButtonTotAcc.Location = new System.Drawing.Point(6, 55);
            this.radioButtonTotAcc.Name = "radioButtonTotAcc";
            this.radioButtonTotAcc.Size = new System.Drawing.Size(118, 17);
            this.radioButtonTotAcc.TabIndex = 14;
            this.radioButtonTotAcc.Text = "Total Accrual (ac-ft)";
            this.radioButtonTotAcc.UseVisualStyleBackColor = true;
            // 
            // radioButtonTotEvap
            // 
            this.radioButtonTotEvap.AutoSize = true;
            this.radioButtonTotEvap.Location = new System.Drawing.Point(6, 19);
            this.radioButtonTotEvap.Name = "radioButtonTotEvap";
            this.radioButtonTotEvap.Size = new System.Drawing.Size(139, 17);
            this.radioButtonTotEvap.TabIndex = 12;
            this.radioButtonTotEvap.Text = "Total Evaporation (ac-ft)";
            this.radioButtonTotEvap.UseVisualStyleBackColor = true;
            // 
            // radioButtonAccStor
            // 
            this.radioButtonAccStor.AutoSize = true;
            this.radioButtonAccStor.Location = new System.Drawing.Point(6, 37);
            this.radioButtonAccStor.Name = "radioButtonAccStor";
            this.radioButtonAccStor.Size = new System.Drawing.Size(128, 17);
            this.radioButtonAccStor.TabIndex = 13;
            this.radioButtonAccStor.Text = "Accrued Storage (cfs)";
            this.radioButtonAccStor.UseVisualStyleBackColor = true;
            // 
            // tabPageDiversion
            // 
            this.tabPageDiversion.Controls.Add(this.radioButtonRemStor);
            this.tabPageDiversion.Controls.Add(this.radioButtonDivFlow);
            this.tabPageDiversion.Controls.Add(this.radioButtonStorDiv2Date);
            this.tabPageDiversion.Controls.Add(this.radioButtonTotDiv2Date);
            this.tabPageDiversion.Controls.Add(this.radioButtonStorDiv);
            this.tabPageDiversion.Location = new System.Drawing.Point(4, 22);
            this.tabPageDiversion.Name = "tabPageDiversion";
            this.tabPageDiversion.Size = new System.Drawing.Size(215, 94);
            this.tabPageDiversion.TabIndex = 2;
            this.tabPageDiversion.UseVisualStyleBackColor = true;
            // 
            // radioButtonRemStor
            // 
            this.radioButtonRemStor.AutoSize = true;
            this.radioButtonRemStor.Location = new System.Drawing.Point(6, 74);
            this.radioButtonRemStor.Name = "radioButtonRemStor";
            this.radioButtonRemStor.Size = new System.Drawing.Size(145, 17);
            this.radioButtonRemStor.TabIndex = 20;
            this.radioButtonRemStor.Text = "Remaining Storage (ac-ft)";
            this.radioButtonRemStor.UseVisualStyleBackColor = true;
            // 
            // radioButtonDivFlow
            // 
            this.radioButtonDivFlow.AutoSize = true;
            this.radioButtonDivFlow.Location = new System.Drawing.Point(6, 1);
            this.radioButtonDivFlow.Name = "radioButtonDivFlow";
            this.radioButtonDivFlow.Size = new System.Drawing.Size(149, 17);
            this.radioButtonDivFlow.TabIndex = 16;
            this.radioButtonDivFlow.Text = "Diversion/Pump Flow (cfs)";
            this.radioButtonDivFlow.UseVisualStyleBackColor = true;
            // 
            // radioButtonStorDiv2Date
            // 
            this.radioButtonStorDiv2Date.AutoSize = true;
            this.radioButtonStorDiv2Date.Location = new System.Drawing.Point(6, 55);
            this.radioButtonStorDiv2Date.Name = "radioButtonStorDiv2Date";
            this.radioButtonStorDiv2Date.Size = new System.Drawing.Size(181, 17);
            this.radioButtonStorDiv2Date.TabIndex = 19;
            this.radioButtonStorDiv2Date.Text = "Storage Diversion To Date (ac-ft)";
            this.radioButtonStorDiv2Date.UseVisualStyleBackColor = true;
            // 
            // radioButtonTotDiv2Date
            // 
            this.radioButtonTotDiv2Date.AutoSize = true;
            this.radioButtonTotDiv2Date.Location = new System.Drawing.Point(6, 19);
            this.radioButtonTotDiv2Date.Name = "radioButtonTotDiv2Date";
            this.radioButtonTotDiv2Date.Size = new System.Drawing.Size(168, 17);
            this.radioButtonTotDiv2Date.TabIndex = 17;
            this.radioButtonTotDiv2Date.Text = "Total Diversion To Date (ac-ft)";
            this.radioButtonTotDiv2Date.UseVisualStyleBackColor = true;
            // 
            // radioButtonStorDiv
            // 
            this.radioButtonStorDiv.AutoSize = true;
            this.radioButtonStorDiv.Location = new System.Drawing.Point(6, 37);
            this.radioButtonStorDiv.Name = "radioButtonStorDiv";
            this.radioButtonStorDiv.Size = new System.Drawing.Size(158, 17);
            this.radioButtonStorDiv.TabIndex = 18;
            this.radioButtonStorDiv.Text = "Daily Storage Diversion (cfs)";
            this.radioButtonStorDiv.UseVisualStyleBackColor = true;
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
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Historical";
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(247, 468);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 8;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.idwrOkButton_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(333, 468);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // linkLabelIdwrApiInfo
            // 
            this.linkLabelIdwrApiInfo.AutoSize = true;
            this.linkLabelIdwrApiInfo.Location = new System.Drawing.Point(10, 473);
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
            this.comboBoxRiverSystems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxRiverSystems.FormattingEnabled = true;
            this.comboBoxRiverSystems.Location = new System.Drawing.Point(13, 26);
            this.comboBoxRiverSystems.Name = "comboBoxRiverSystems";
            this.comboBoxRiverSystems.Size = new System.Drawing.Size(401, 21);
            this.comboBoxRiverSystems.TabIndex = 11;
            this.comboBoxRiverSystems.Text = "Select A River System";
            this.comboBoxRiverSystems.DropDown += new System.EventHandler(this.comboBoxRiverSystems_OnDropDown);
            this.comboBoxRiverSystems.SelectedIndexChanged += new System.EventHandler(this.comboBoxRiverSystems_SelectedIndexChanged);
            // 
            // comboBoxRiverSites
            // 
            this.comboBoxRiverSites.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxRiverSites.FormattingEnabled = true;
            this.comboBoxRiverSites.Location = new System.Drawing.Point(12, 71);
            this.comboBoxRiverSites.Name = "comboBoxRiverSites";
            this.comboBoxRiverSites.Size = new System.Drawing.Size(402, 21);
            this.comboBoxRiverSites.TabIndex = 12;
            this.comboBoxRiverSites.SelectedIndexChanged += new System.EventHandler(this.comboBoxRiverSites_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 127);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(161, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Selected IDWR Site Information:";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(24, 165);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(41, 13);
            this.labelName.TabIndex = 14;
            this.labelName.Text = "Name: ";
            // 
            // labelSID
            // 
            this.labelSID.AutoSize = true;
            this.labelSID.Location = new System.Drawing.Point(24, 184);
            this.labelSID.Name = "labelSID";
            this.labelSID.Size = new System.Drawing.Size(45, 13);
            this.labelSID.TabIndex = 15;
            this.labelSID.Text = "Site ID: ";
            // 
            // labelYears
            // 
            this.labelYears.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelYears.AutoSize = true;
            this.labelYears.Location = new System.Drawing.Point(24, 203);
            this.labelYears.MaximumSize = new System.Drawing.Size(350, 0);
            this.labelYears.Name = "labelYears";
            this.labelYears.Size = new System.Drawing.Size(86, 13);
            this.labelYears.TabIndex = 16;
            this.labelYears.Text = "Years Available: ";
            // 
            // labelSType
            // 
            this.labelSType.AutoSize = true;
            this.labelSType.Location = new System.Drawing.Point(228, 184);
            this.labelSType.Name = "labelSType";
            this.labelSType.Size = new System.Drawing.Size(58, 13);
            this.labelSType.TabIndex = 17;
            this.labelSType.Text = "Site Type: ";
            // 
            // textBoxSID
            // 
            this.textBoxSID.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxSID.Location = new System.Drawing.Point(70, 181);
            this.textBoxSID.Name = "textBoxSID";
            this.textBoxSID.ReadOnly = true;
            this.textBoxSID.Size = new System.Drawing.Size(139, 13);
            this.textBoxSID.TabIndex = 18;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 494);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(420, 22);
            this.statusStrip1.TabIndex = 19;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(185, 17);
            this.toolStripStatusLabel1.Text = "Add an IDWR Series from web API";
            // 
            // radioButtonHistorical
            // 
            this.radioButtonHistorical.AutoSize = true;
            this.radioButtonHistorical.Location = new System.Drawing.Point(80, 145);
            this.radioButtonHistorical.Name = "radioButtonHistorical";
            this.radioButtonHistorical.Size = new System.Drawing.Size(68, 17);
            this.radioButtonHistorical.TabIndex = 20;
            this.radioButtonHistorical.Text = "Historical";
            this.radioButtonHistorical.UseVisualStyleBackColor = true;
            this.radioButtonHistorical.CheckedChanged += new System.EventHandler(this.radioButtonHistorical_CheckedChanged);
            // 
            // radioButtonAccounting
            // 
            this.radioButtonAccounting.AutoSize = true;
            this.radioButtonAccounting.Location = new System.Drawing.Point(246, 143);
            this.radioButtonAccounting.Name = "radioButtonAccounting";
            this.radioButtonAccounting.Size = new System.Drawing.Size(79, 17);
            this.radioButtonAccounting.TabIndex = 21;
            this.radioButtonAccounting.Text = "Accounting";
            this.radioButtonAccounting.UseVisualStyleBackColor = true;
            this.radioButtonAccounting.CheckedChanged += new System.EventHandler(this.radioButtonAccounting_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 147);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "Dataset: ";
            // 
            // radioButtonSiteNameSort
            // 
            this.radioButtonSiteNameSort.AutoSize = true;
            this.radioButtonSiteNameSort.Checked = true;
            this.radioButtonSiteNameSort.Location = new System.Drawing.Point(319, 98);
            this.radioButtonSiteNameSort.Name = "radioButtonSiteNameSort";
            this.radioButtonSiteNameSort.Size = new System.Drawing.Size(53, 17);
            this.radioButtonSiteNameSort.TabIndex = 23;
            this.radioButtonSiteNameSort.TabStop = true;
            this.radioButtonSiteNameSort.Text = "Name";
            this.radioButtonSiteNameSort.UseVisualStyleBackColor = true;
            // 
            // radioButtonSiteIdSort
            // 
            this.radioButtonSiteIdSort.AutoSize = true;
            this.radioButtonSiteIdSort.Location = new System.Drawing.Point(378, 98);
            this.radioButtonSiteIdSort.Name = "radioButtonSiteIdSort";
            this.radioButtonSiteIdSort.Size = new System.Drawing.Size(36, 17);
            this.radioButtonSiteIdSort.TabIndex = 24;
            this.radioButtonSiteIdSort.Text = "ID";
            this.radioButtonSiteIdSort.UseVisualStyleBackColor = true;
            this.radioButtonSiteIdSort.CheckedChanged += new System.EventHandler(this.radioButtonSiteIdSort_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(241, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 25;
            this.label7.Text = "Sort Sites by: ";
            // 
            // timeSelectorBeginEnd1
            // 
            this.timeSelectorBeginEnd1.Location = new System.Drawing.Point(133, 282);
            this.timeSelectorBeginEnd1.Name = "timeSelectorBeginEnd1";
            this.timeSelectorBeginEnd1.ShowTime = false;
            this.timeSelectorBeginEnd1.Size = new System.Drawing.Size(199, 46);
            this.timeSelectorBeginEnd1.T1 = new System.DateTime(2008, 3, 12, 7, 55, 34, 320);
            this.timeSelectorBeginEnd1.T2 = new System.DateTime(2008, 3, 12, 7, 55, 34, 320);
            this.timeSelectorBeginEnd1.TabIndex = 1;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(33, 100);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Filter Sites by: ";
            // 
            // comboBoxSiteFilter
            // 
            this.comboBoxSiteFilter.FormattingEnabled = true;
            this.comboBoxSiteFilter.Items.AddRange(new object[] {
            "A - Show All Sites",
            "F - Streams",
            "R - Reservoirs",
            "D - Diversions",
            "P - Pumps"});
            this.comboBoxSiteFilter.Location = new System.Drawing.Point(114, 97);
            this.comboBoxSiteFilter.Name = "comboBoxSiteFilter";
            this.comboBoxSiteFilter.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSiteFilter.TabIndex = 27;
            this.comboBoxSiteFilter.Text = "A - Show All Sites";
            this.comboBoxSiteFilter.SelectedIndexChanged += new System.EventHandler(this.comboBoxSiteFilter_SelectedIndexChanged);
            // 
            // ImportIdwrData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 516);
            this.Controls.Add(this.comboBoxSiteFilter);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.radioButtonSiteIdSort);
            this.Controls.Add(this.radioButtonSiteNameSort);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.radioButtonAccounting);
            this.Controls.Add(this.radioButtonHistorical);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.textBoxSID);
            this.Controls.Add(this.labelSType);
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
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(436, 451);
            this.Name = "ImportIdwrData";
            this.Text = "Import IDWR Data";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageStream.ResumeLayout(false);
            this.tabPageStream.PerformLayout();
            this.tabPageReservoir.ResumeLayout(false);
            this.tabPageReservoir.PerformLayout();
            this.tabPageDiversion.ResumeLayout(false);
            this.tabPageDiversion.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
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
        private System.Windows.Forms.Label labelSType;
        private System.Windows.Forms.TextBox textBoxSID;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.RadioButton radioButtonHistorical;
        private System.Windows.Forms.RadioButton radioButtonAccounting;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton radioButtonGainQ;
        private System.Windows.Forms.RadioButton radioButtonStorQ;
        private System.Windows.Forms.RadioButton radioButtonActQ;
        private System.Windows.Forms.RadioButton radioButtonNatQ;
        private System.Windows.Forms.TabPage tabPageMain;
        private System.Windows.Forms.TabPage tabPageStream;
        private System.Windows.Forms.TabPage tabPageReservoir;
        private System.Windows.Forms.RadioButton radioButtonCurrAf;
        private System.Windows.Forms.RadioButton radioButtonResEvap;
        private System.Windows.Forms.RadioButton radioButtonTotAcc;
        private System.Windows.Forms.RadioButton radioButtonTotEvap;
        private System.Windows.Forms.RadioButton radioButtonAccStor;
        private System.Windows.Forms.TabPage tabPageDiversion;
        private System.Windows.Forms.RadioButton radioButtonRemStor;
        private System.Windows.Forms.RadioButton radioButtonDivFlow;
        private System.Windows.Forms.RadioButton radioButtonStorDiv2Date;
        private System.Windows.Forms.RadioButton radioButtonTotDiv2Date;
        private System.Windows.Forms.RadioButton radioButtonStorDiv;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.RadioButton radioButtonSiteNameSort;
        private System.Windows.Forms.RadioButton radioButtonSiteIdSort;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBoxSiteFilter;
    }
}