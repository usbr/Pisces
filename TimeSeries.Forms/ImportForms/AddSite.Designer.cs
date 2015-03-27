namespace Reclamation.TimeSeries.Forms.ImportForms
{
    partial class AddSite
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
            this.components = new System.ComponentModel.Container();
            this.textBoxSiteID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxdescription = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxlatitude = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxlongitude = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButtonHydromet = new System.Windows.Forms.RadioButton();
            this.radioButtonAgriMet = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxReservoir = new System.Windows.Forms.CheckBox();
            this.checkBoxCanal = new System.Windows.Forms.CheckBox();
            this.checkBoxGenericWeir = new System.Windows.Forms.CheckBox();
            this.checkBoxRectWeir = new System.Windows.Forms.CheckBox();
            this.checkBoxQuality = new System.Windows.Forms.CheckBox();
            this.buttonIndividuals = new System.Windows.Forms.Button();
            this.checkBoxQ = new System.Windows.Forms.CheckBox();
            this.checkBoxWaterTemp = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.labelError = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxState = new System.Windows.Forms.TextBox();
            this.textBoxInstall = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxTimezone = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxElevation = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxSiteID
            // 
            this.textBoxSiteID.Location = new System.Drawing.Point(85, 18);
            this.textBoxSiteID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxSiteID.Name = "textBoxSiteID";
            this.textBoxSiteID.Size = new System.Drawing.Size(99, 22);
            this.textBoxSiteID.TabIndex = 0;
            this.textBoxSiteID.TextChanged += new System.EventHandler(this.textBoxSiteID_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "siteid";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "site name";
            // 
            // textBoxdescription
            // 
            this.textBoxdescription.Location = new System.Drawing.Point(85, 43);
            this.textBoxdescription.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxdescription.Name = "textBoxdescription";
            this.textBoxdescription.Size = new System.Drawing.Size(208, 22);
            this.textBoxdescription.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "latitude";
            // 
            // textBoxlatitude
            // 
            this.textBoxlatitude.Location = new System.Drawing.Point(100, 63);
            this.textBoxlatitude.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxlatitude.Name = "textBoxlatitude";
            this.textBoxlatitude.Size = new System.Drawing.Size(167, 22);
            this.textBoxlatitude.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "longitude";
            // 
            // textBoxlongitude
            // 
            this.textBoxlongitude.Location = new System.Drawing.Point(100, 89);
            this.textBoxlongitude.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxlongitude.Name = "textBoxlongitude";
            this.textBoxlongitude.Size = new System.Drawing.Size(167, 22);
            this.textBoxlongitude.TabIndex = 6;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(861, 639);
            this.tabControl1.TabIndex = 8;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Size = new System.Drawing.Size(853, 610);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "new site";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 222);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(847, 386);
            this.dataGridView1.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.labelError);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBoxdescription);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBoxSiteID);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(847, 220);
            this.panel1.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButtonHydromet);
            this.groupBox2.Controls.Add(this.radioButtonAgriMet);
            this.groupBox2.Location = new System.Drawing.Point(22, 70);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(213, 70);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "program";
            // 
            // radioButtonHydromet
            // 
            this.radioButtonHydromet.AutoSize = true;
            this.radioButtonHydromet.Checked = true;
            this.radioButtonHydromet.Location = new System.Drawing.Point(18, 46);
            this.radioButtonHydromet.Name = "radioButtonHydromet";
            this.radioButtonHydromet.Size = new System.Drawing.Size(88, 21);
            this.radioButtonHydromet.TabIndex = 1;
            this.radioButtonHydromet.TabStop = true;
            this.radioButtonHydromet.Text = "hydromet";
            this.radioButtonHydromet.UseVisualStyleBackColor = true;
            // 
            // radioButtonAgriMet
            // 
            this.radioButtonAgriMet.AutoSize = true;
            this.radioButtonAgriMet.Location = new System.Drawing.Point(18, 19);
            this.radioButtonAgriMet.Name = "radioButtonAgriMet";
            this.radioButtonAgriMet.Size = new System.Drawing.Size(76, 21);
            this.radioButtonAgriMet.TabIndex = 0;
            this.radioButtonAgriMet.TabStop = true;
            this.radioButtonAgriMet.Text = "agrimet";
            this.radioButtonAgriMet.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxReservoir);
            this.groupBox1.Controls.Add(this.checkBoxCanal);
            this.groupBox1.Controls.Add(this.checkBoxGenericWeir);
            this.groupBox1.Controls.Add(this.checkBoxRectWeir);
            this.groupBox1.Controls.Add(this.checkBoxQuality);
            this.groupBox1.Controls.Add(this.buttonIndividuals);
            this.groupBox1.Controls.Add(this.checkBoxQ);
            this.groupBox1.Controls.Add(this.checkBoxWaterTemp);
            this.groupBox1.Location = new System.Drawing.Point(348, 34);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox1.Size = new System.Drawing.Size(478, 170);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            // 
            // checkBoxReservoir
            // 
            this.checkBoxReservoir.AutoSize = true;
            this.checkBoxReservoir.Location = new System.Drawing.Point(8, 71);
            this.checkBoxReservoir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxReservoir.Name = "checkBoxReservoir";
            this.checkBoxReservoir.Size = new System.Drawing.Size(143, 21);
            this.checkBoxReservoir.TabIndex = 20;
            this.checkBoxReservoir.Text = "forebay, acre-feet";
            this.checkBoxReservoir.UseVisualStyleBackColor = true;
            // 
            // checkBoxCanal
            // 
            this.checkBoxCanal.AutoSize = true;
            this.checkBoxCanal.Location = new System.Drawing.Point(10, 53);
            this.checkBoxCanal.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxCanal.Name = "checkBoxCanal";
            this.checkBoxCanal.Size = new System.Drawing.Size(169, 21);
            this.checkBoxCanal.TabIndex = 19;
            this.checkBoxCanal.Text = "canal height,shift,flow,";
            this.checkBoxCanal.UseVisualStyleBackColor = true;
            // 
            // checkBoxGenericWeir
            // 
            this.checkBoxGenericWeir.AutoSize = true;
            this.checkBoxGenericWeir.Location = new System.Drawing.Point(9, 135);
            this.checkBoxGenericWeir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxGenericWeir.Name = "checkBoxGenericWeir";
            this.checkBoxGenericWeir.Size = new System.Drawing.Size(278, 21);
            this.checkBoxGenericWeir.TabIndex = 14;
            this.checkBoxGenericWeir.Text = "canal: ch,qc,shift (generic weir ch_weir)";
            this.toolTip1.SetToolTip(this.checkBoxGenericWeir, "width_factor*head^exponent");
            this.checkBoxGenericWeir.UseVisualStyleBackColor = true;
            // 
            // checkBoxRectWeir
            // 
            this.checkBoxRectWeir.AutoSize = true;
            this.checkBoxRectWeir.Location = new System.Drawing.Point(9, 114);
            this.checkBoxRectWeir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxRectWeir.Name = "checkBoxRectWeir";
            this.checkBoxRectWeir.Size = new System.Drawing.Size(293, 21);
            this.checkBoxRectWeir.TabIndex = 13;
            this.checkBoxRectWeir.Text = "canal: ch,qc,shift (rectangular weir) r_weir";
            this.toolTip1.SetToolTip(this.checkBoxRectWeir, "Q=3.33*h^1.5*(length-.2*h)");
            this.checkBoxRectWeir.UseVisualStyleBackColor = true;
            // 
            // checkBoxQuality
            // 
            this.checkBoxQuality.AutoSize = true;
            this.checkBoxQuality.Location = new System.Drawing.Point(9, 90);
            this.checkBoxQuality.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxQuality.Name = "checkBoxQuality";
            this.checkBoxQuality.Size = new System.Drawing.Size(245, 21);
            this.checkBoxQuality.TabIndex = 12;
            this.checkBoxQuality.Text = "power,lenerr,parity,timeerr,msglen";
            this.checkBoxQuality.UseVisualStyleBackColor = true;
            // 
            // buttonIndividuals
            // 
            this.buttonIndividuals.Location = new System.Drawing.Point(276, 17);
            this.buttonIndividuals.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonIndividuals.Name = "buttonIndividuals";
            this.buttonIndividuals.Size = new System.Drawing.Size(72, 23);
            this.buttonIndividuals.TabIndex = 11;
            this.buttonIndividuals.Text = "add";
            this.buttonIndividuals.UseVisualStyleBackColor = true;
            this.buttonIndividuals.Click += new System.EventHandler(this.buttonIndividuals_Click);
            // 
            // checkBoxQ
            // 
            this.checkBoxQ.AutoSize = true;
            this.checkBoxQ.Location = new System.Drawing.Point(9, 11);
            this.checkBoxQ.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxQ.Name = "checkBoxQ";
            this.checkBoxQ.Size = new System.Drawing.Size(167, 21);
            this.checkBoxQ.TabIndex = 9;
            this.checkBoxQ.Text = "gage height,shift,flow,";
            this.checkBoxQ.UseVisualStyleBackColor = true;
            // 
            // checkBoxWaterTemp
            // 
            this.checkBoxWaterTemp.AutoSize = true;
            this.checkBoxWaterTemp.Location = new System.Drawing.Point(9, 32);
            this.checkBoxWaterTemp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBoxWaterTemp.Name = "checkBoxWaterTemp";
            this.checkBoxWaterTemp.Size = new System.Drawing.Size(135, 21);
            this.checkBoxWaterTemp.TabIndex = 8;
            this.checkBoxWaterTemp.Text = "water temp degF";
            this.checkBoxWaterTemp.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(284, 6);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 17);
            this.label9.TabIndex = 7;
            this.label9.Text = "template";
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.ForeColor = System.Drawing.Color.Red;
            this.labelError.Location = new System.Drawing.Point(359, 47);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(0, 17);
            this.labelError.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(576, 6);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.AddTemplateClick);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(352, 2);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(217, 24);
            this.comboBox1.TabIndex = 4;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.textBoxState);
            this.tabPage2.Controls.Add(this.textBoxInstall);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.textBoxTimezone);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.textBoxElevation);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.textBoxlatitude);
            this.tabPage2.Controls.Add(this.textBoxlongitude);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage2.Size = new System.Drawing.Size(853, 610);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "advanced";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(25, 38);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 17);
            this.label8.TabIndex = 15;
            this.label8.Text = "state";
            // 
            // textBoxState
            // 
            this.textBoxState.Location = new System.Drawing.Point(100, 34);
            this.textBoxState.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxState.Name = "textBoxState";
            this.textBoxState.Size = new System.Drawing.Size(39, 22);
            this.textBoxState.TabIndex = 14;
            // 
            // textBoxInstall
            // 
            this.textBoxInstall.Location = new System.Drawing.Point(100, 169);
            this.textBoxInstall.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxInstall.Name = "textBoxInstall";
            this.textBoxInstall.Size = new System.Drawing.Size(167, 22);
            this.textBoxInstall.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 172);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 17);
            this.label7.TabIndex = 12;
            this.label7.Text = "install";
            // 
            // textBoxTimezone
            // 
            this.textBoxTimezone.Location = new System.Drawing.Point(100, 143);
            this.textBoxTimezone.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxTimezone.Name = "textBoxTimezone";
            this.textBoxTimezone.Size = new System.Drawing.Size(167, 22);
            this.textBoxTimezone.TabIndex = 11;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 146);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 17);
            this.label6.TabIndex = 10;
            this.label6.Text = "timezone";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "elevation";
            // 
            // textBoxElevation
            // 
            this.textBoxElevation.Location = new System.Drawing.Point(100, 114);
            this.textBoxElevation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBoxElevation.Name = "textBoxElevation";
            this.textBoxElevation.Size = new System.Drawing.Size(167, 22);
            this.textBoxElevation.TabIndex = 8;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonCancel);
            this.panel2.Controls.Add(this.buttonOK);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 550);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(861, 89);
            this.panel2.TabIndex = 9;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(747, 50);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 28);
            this.buttonCancel.TabIndex = 7;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(747, 15);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(100, 28);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // AddSite
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(861, 639);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "AddSite";
            this.Text = "AddSite";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxSiteID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxdescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxlatitude;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxlongitude;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox textBoxInstall;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxTimezone;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxElevation;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxState;
        private System.Windows.Forms.Label labelError;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonIndividuals;
        private System.Windows.Forms.CheckBox checkBoxQ;
        private System.Windows.Forms.CheckBox checkBoxWaterTemp;
        private System.Windows.Forms.CheckBox checkBoxQuality;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButtonHydromet;
        private System.Windows.Forms.RadioButton radioButtonAgriMet;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.CheckBox checkBoxGenericWeir;
        private System.Windows.Forms.CheckBox checkBoxRectWeir;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBoxCanal;
        private System.Windows.Forms.CheckBox checkBoxReservoir;
    }
}