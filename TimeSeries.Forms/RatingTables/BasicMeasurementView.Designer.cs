namespace Reclamation.TimeSeries.Forms.RatingTables
{
    partial class BasicMeasurementView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuItemCopy = new System.Windows.Forms.MenuItem();
            this.menuItemPaste = new System.Windows.Forms.MenuItem();
            this.tabPageData = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageGraph = new System.Windows.Forms.TabPage();
            this.textBox_Discharge = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.labelTitle = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label_Gate5 = new System.Windows.Forms.Label();
            this.label_Gate6 = new System.Windows.Forms.Label();
            this.textBox_Gate6 = new System.Windows.Forms.TextBox();
            this.textBox_Gate3 = new System.Windows.Forms.TextBox();
            this.label_Gate4 = new System.Windows.Forms.Label();
            this.label_Gate2 = new System.Windows.Forms.Label();
            this.textBox_Gate4 = new System.Windows.Forms.TextBox();
            this.label_Gate1 = new System.Windows.Forms.Label();
            this.textBox_Gate5 = new System.Windows.Forms.TextBox();
            this.textBox_Gate1 = new System.Windows.Forms.TextBox();
            this.textBox_Gate2 = new System.Windows.Forms.TextBox();
            this.label_Gate3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox_Spin = new System.Windows.Forms.TextBox();
            this.textBox_Meter_Susp = new System.Windows.Forms.TextBox();
            this.label_Spin = new System.Windows.Forms.Label();
            this.label_Weight_Susp = new System.Windows.Forms.Label();
            this.label_Method = new System.Windows.Forms.Label();
            this.label_Meter_No = new System.Windows.Forms.Label();
            this.label_Meter_Susp = new System.Windows.Forms.Label();
            this.textBox_Method = new System.Windows.Forms.TextBox();
            this.textBox_Weight_Susp = new System.Windows.Forms.TextBox();
            this.textBox_Meter_No = new System.Windows.Forms.TextBox();
            this.textBox_Prim_Gage = new System.Windows.Forms.TextBox();
            this.textBox_Sec_Gage = new System.Windows.Forms.TextBox();
            this.button_Compute_Discharge = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.textBox_Party = new System.Windows.Forms.TextBox();
            this.textBox_Memo = new System.Windows.Forms.TextBox();
            this.label_Party = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.tabPageData.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(47, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(32, 16);
            this.label3.TabIndex = 104;
            this.label3.Text = "Date";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(47, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 16);
            this.label4.TabIndex = 101;
            this.label4.Text = "Discharge";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(39, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 16);
            this.label5.TabIndex = 102;
            this.label5.Text = "Primary Stage";
            // 
            // dataGrid1
            // 
            this.dataGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGrid1.ContextMenu = this.contextMenu1;
            this.dataGrid1.DataMember = "";
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Location = new System.Drawing.Point(4, 4);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.Size = new System.Drawing.Size(776, 292);
            this.dataGrid1.TabIndex = 71;
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemCopy,
            this.menuItemPaste});
            // 
            // menuItemCopy
            // 
            this.menuItemCopy.Index = 0;
            this.menuItemCopy.Text = "&Copy";
            // 
            // menuItemPaste
            // 
            this.menuItemPaste.Index = 1;
            this.menuItemPaste.Text = "&Paste";
            // 
            // tabPageData
            // 
            this.tabPageData.Controls.Add(this.dataGrid1);
            this.tabPageData.Location = new System.Drawing.Point(4, 22);
            this.tabPageData.Name = "tabPageData";
            this.tabPageData.Size = new System.Drawing.Size(780, 298);
            this.tabPageData.TabIndex = 0;
            this.tabPageData.Text = "Data";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(39, 98);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 16);
            this.label6.TabIndex = 103;
            this.label6.Text = "Secondary Stage";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageData);
            this.tabControl1.Controls.Add(this.tabPageGraph);
            this.tabControl1.Location = new System.Drawing.Point(23, 298);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(788, 324);
            this.tabControl1.TabIndex = 97;
            // 
            // tabPageGraph
            // 
            this.tabPageGraph.Location = new System.Drawing.Point(4, 22);
            this.tabPageGraph.Name = "tabPageGraph";
            this.tabPageGraph.Size = new System.Drawing.Size(780, 298);
            this.tabPageGraph.TabIndex = 1;
            this.tabPageGraph.Text = "Graph";
            // 
            // textBox_Discharge
            // 
            this.textBox_Discharge.Location = new System.Drawing.Point(139, 54);
            this.textBox_Discharge.Name = "textBox_Discharge";
            this.textBox_Discharge.Size = new System.Drawing.Size(80, 20);
            this.textBox_Discharge.TabIndex = 88;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(359, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 16);
            this.label2.TabIndex = 100;
            this.label2.Text = "notes";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dateTimePicker1.Enabled = false;
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(139, 30);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(148, 20);
            this.dateTimePicker1.TabIndex = 99;
            // 
            // labelTitle
            // 
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(43, -2);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(472, 23);
            this.labelTitle.TabIndex = 98;
            this.labelTitle.Text = "site Near the river ";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label_Gate5);
            this.groupBox3.Controls.Add(this.label_Gate6);
            this.groupBox3.Controls.Add(this.textBox_Gate6);
            this.groupBox3.Controls.Add(this.textBox_Gate3);
            this.groupBox3.Controls.Add(this.label_Gate4);
            this.groupBox3.Controls.Add(this.label_Gate2);
            this.groupBox3.Controls.Add(this.textBox_Gate4);
            this.groupBox3.Controls.Add(this.label_Gate1);
            this.groupBox3.Controls.Add(this.textBox_Gate5);
            this.groupBox3.Controls.Add(this.textBox_Gate1);
            this.groupBox3.Controls.Add(this.textBox_Gate2);
            this.groupBox3.Controls.Add(this.label_Gate3);
            this.groupBox3.Location = new System.Drawing.Point(267, 134);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(112, 148);
            this.groupBox3.TabIndex = 95;
            this.groupBox3.TabStop = false;
            // 
            // label_Gate5
            // 
            this.label_Gate5.Location = new System.Drawing.Point(4, 104);
            this.label_Gate5.Name = "label_Gate5";
            this.label_Gate5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label_Gate5.Size = new System.Drawing.Size(40, 16);
            this.label_Gate5.TabIndex = 49;
            this.label_Gate5.Text = "Gate5";
            // 
            // label_Gate6
            // 
            this.label_Gate6.Location = new System.Drawing.Point(4, 124);
            this.label_Gate6.Name = "label_Gate6";
            this.label_Gate6.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label_Gate6.Size = new System.Drawing.Size(40, 16);
            this.label_Gate6.TabIndex = 50;
            this.label_Gate6.Text = "Gate6";
            // 
            // textBox_Gate6
            // 
            this.textBox_Gate6.Location = new System.Drawing.Point(48, 120);
            this.textBox_Gate6.Name = "textBox_Gate6";
            this.textBox_Gate6.Size = new System.Drawing.Size(48, 20);
            this.textBox_Gate6.TabIndex = 23;
            // 
            // textBox_Gate3
            // 
            this.textBox_Gate3.Location = new System.Drawing.Point(48, 60);
            this.textBox_Gate3.Name = "textBox_Gate3";
            this.textBox_Gate3.Size = new System.Drawing.Size(48, 20);
            this.textBox_Gate3.TabIndex = 20;
            // 
            // label_Gate4
            // 
            this.label_Gate4.Location = new System.Drawing.Point(4, 84);
            this.label_Gate4.Name = "label_Gate4";
            this.label_Gate4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label_Gate4.Size = new System.Drawing.Size(40, 16);
            this.label_Gate4.TabIndex = 48;
            this.label_Gate4.Text = "Gate4";
            // 
            // label_Gate2
            // 
            this.label_Gate2.Location = new System.Drawing.Point(4, 44);
            this.label_Gate2.Name = "label_Gate2";
            this.label_Gate2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label_Gate2.Size = new System.Drawing.Size(40, 16);
            this.label_Gate2.TabIndex = 46;
            this.label_Gate2.Text = "Gate2";
            // 
            // textBox_Gate4
            // 
            this.textBox_Gate4.Location = new System.Drawing.Point(48, 80);
            this.textBox_Gate4.Name = "textBox_Gate4";
            this.textBox_Gate4.Size = new System.Drawing.Size(48, 20);
            this.textBox_Gate4.TabIndex = 21;
            // 
            // label_Gate1
            // 
            this.label_Gate1.Location = new System.Drawing.Point(8, 24);
            this.label_Gate1.Name = "label_Gate1";
            this.label_Gate1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label_Gate1.Size = new System.Drawing.Size(36, 16);
            this.label_Gate1.TabIndex = 45;
            this.label_Gate1.Text = "Gate1";
            // 
            // textBox_Gate5
            // 
            this.textBox_Gate5.Location = new System.Drawing.Point(48, 100);
            this.textBox_Gate5.Name = "textBox_Gate5";
            this.textBox_Gate5.Size = new System.Drawing.Size(48, 20);
            this.textBox_Gate5.TabIndex = 22;
            // 
            // textBox_Gate1
            // 
            this.textBox_Gate1.Location = new System.Drawing.Point(48, 20);
            this.textBox_Gate1.Name = "textBox_Gate1";
            this.textBox_Gate1.Size = new System.Drawing.Size(48, 20);
            this.textBox_Gate1.TabIndex = 18;
            // 
            // textBox_Gate2
            // 
            this.textBox_Gate2.Location = new System.Drawing.Point(48, 40);
            this.textBox_Gate2.Name = "textBox_Gate2";
            this.textBox_Gate2.Size = new System.Drawing.Size(48, 20);
            this.textBox_Gate2.TabIndex = 19;
            // 
            // label_Gate3
            // 
            this.label_Gate3.Location = new System.Drawing.Point(4, 64);
            this.label_Gate3.Name = "label_Gate3";
            this.label_Gate3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label_Gate3.Size = new System.Drawing.Size(40, 16);
            this.label_Gate3.TabIndex = 47;
            this.label_Gate3.Text = "Gate3";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox_Spin);
            this.groupBox2.Controls.Add(this.textBox_Meter_Susp);
            this.groupBox2.Controls.Add(this.label_Spin);
            this.groupBox2.Controls.Add(this.label_Weight_Susp);
            this.groupBox2.Controls.Add(this.label_Method);
            this.groupBox2.Controls.Add(this.label_Meter_No);
            this.groupBox2.Controls.Add(this.label_Meter_Susp);
            this.groupBox2.Controls.Add(this.textBox_Method);
            this.groupBox2.Controls.Add(this.textBox_Weight_Susp);
            this.groupBox2.Controls.Add(this.textBox_Meter_No);
            this.groupBox2.Location = new System.Drawing.Point(87, 142);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(160, 136);
            this.groupBox2.TabIndex = 94;
            this.groupBox2.TabStop = false;
            // 
            // textBox_Spin
            // 
            this.textBox_Spin.Location = new System.Drawing.Point(76, 76);
            this.textBox_Spin.Name = "textBox_Spin";
            this.textBox_Spin.Size = new System.Drawing.Size(72, 20);
            this.textBox_Spin.TabIndex = 12;
            // 
            // textBox_Meter_Susp
            // 
            this.textBox_Meter_Susp.Location = new System.Drawing.Point(76, 56);
            this.textBox_Meter_Susp.Name = "textBox_Meter_Susp";
            this.textBox_Meter_Susp.Size = new System.Drawing.Size(72, 20);
            this.textBox_Meter_Susp.TabIndex = 11;
            // 
            // label_Spin
            // 
            this.label_Spin.Location = new System.Drawing.Point(40, 76);
            this.label_Spin.Name = "label_Spin";
            this.label_Spin.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label_Spin.Size = new System.Drawing.Size(32, 16);
            this.label_Spin.TabIndex = 39;
            this.label_Spin.Text = "Spin";
            // 
            // label_Weight_Susp
            // 
            this.label_Weight_Susp.Location = new System.Drawing.Point(4, 36);
            this.label_Weight_Susp.Name = "label_Weight_Susp";
            this.label_Weight_Susp.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label_Weight_Susp.Size = new System.Drawing.Size(72, 16);
            this.label_Weight_Susp.TabIndex = 37;
            this.label_Weight_Susp.Text = "Weight Susp";
            this.label_Weight_Susp.UseMnemonic = false;
            // 
            // label_Method
            // 
            this.label_Method.Location = new System.Drawing.Point(24, 96);
            this.label_Method.Name = "label_Method";
            this.label_Method.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label_Method.Size = new System.Drawing.Size(48, 16);
            this.label_Method.TabIndex = 40;
            this.label_Method.Text = "Method";
            // 
            // label_Meter_No
            // 
            this.label_Meter_No.Location = new System.Drawing.Point(20, 16);
            this.label_Meter_No.Name = "label_Meter_No";
            this.label_Meter_No.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label_Meter_No.Size = new System.Drawing.Size(56, 16);
            this.label_Meter_No.TabIndex = 36;
            this.label_Meter_No.Text = "Meter No";
            // 
            // label_Meter_Susp
            // 
            this.label_Meter_Susp.Location = new System.Drawing.Point(4, 56);
            this.label_Meter_Susp.Name = "label_Meter_Susp";
            this.label_Meter_Susp.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label_Meter_Susp.Size = new System.Drawing.Size(72, 16);
            this.label_Meter_Susp.TabIndex = 38;
            this.label_Meter_Susp.Text = "Meter Susp";
            // 
            // textBox_Method
            // 
            this.textBox_Method.Location = new System.Drawing.Point(76, 96);
            this.textBox_Method.Name = "textBox_Method";
            this.textBox_Method.Size = new System.Drawing.Size(72, 20);
            this.textBox_Method.TabIndex = 13;
            // 
            // textBox_Weight_Susp
            // 
            this.textBox_Weight_Susp.Location = new System.Drawing.Point(76, 36);
            this.textBox_Weight_Susp.Name = "textBox_Weight_Susp";
            this.textBox_Weight_Susp.Size = new System.Drawing.Size(72, 20);
            this.textBox_Weight_Susp.TabIndex = 10;
            // 
            // textBox_Meter_No
            // 
            this.textBox_Meter_No.Location = new System.Drawing.Point(76, 16);
            this.textBox_Meter_No.Name = "textBox_Meter_No";
            this.textBox_Meter_No.Size = new System.Drawing.Size(72, 20);
            this.textBox_Meter_No.TabIndex = 9;
            // 
            // textBox_Prim_Gage
            // 
            this.textBox_Prim_Gage.Location = new System.Drawing.Point(139, 74);
            this.textBox_Prim_Gage.Name = "textBox_Prim_Gage";
            this.textBox_Prim_Gage.Size = new System.Drawing.Size(80, 20);
            this.textBox_Prim_Gage.TabIndex = 89;
            // 
            // textBox_Sec_Gage
            // 
            this.textBox_Sec_Gage.Location = new System.Drawing.Point(139, 98);
            this.textBox_Sec_Gage.Name = "textBox_Sec_Gage";
            this.textBox_Sec_Gage.Size = new System.Drawing.Size(80, 20);
            this.textBox_Sec_Gage.TabIndex = 90;
            // 
            // button_Compute_Discharge
            // 
            this.button_Compute_Discharge.Location = new System.Drawing.Point(247, 62);
            this.button_Compute_Discharge.Name = "button_Compute_Discharge";
            this.button_Compute_Discharge.Size = new System.Drawing.Size(88, 48);
            this.button_Compute_Discharge.TabIndex = 93;
            this.button_Compute_Discharge.Text = "Compute Discharge";
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(727, 642);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 96;
            this.buttonSave.Text = "Save";
            // 
            // textBox_Party
            // 
            this.textBox_Party.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Party.Location = new System.Drawing.Point(415, 38);
            this.textBox_Party.Name = "textBox_Party";
            this.textBox_Party.Size = new System.Drawing.Size(380, 20);
            this.textBox_Party.TabIndex = 87;
            // 
            // textBox_Memo
            // 
            this.textBox_Memo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Memo.Location = new System.Drawing.Point(415, 74);
            this.textBox_Memo.Multiline = true;
            this.textBox_Memo.Name = "textBox_Memo";
            this.textBox_Memo.Size = new System.Drawing.Size(384, 212);
            this.textBox_Memo.TabIndex = 91;
            // 
            // label_Party
            // 
            this.label_Party.Location = new System.Drawing.Point(371, 42);
            this.label_Party.Name = "label_Party";
            this.label_Party.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label_Party.Size = new System.Drawing.Size(32, 16);
            this.label_Party.TabIndex = 92;
            this.label_Party.Text = "Party";
            // 
            // PiscesMeasurementView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.textBox_Discharge);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.textBox_Prim_Gage);
            this.Controls.Add(this.textBox_Sec_Gage);
            this.Controls.Add(this.button_Compute_Discharge);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBox_Party);
            this.Controls.Add(this.textBox_Memo);
            this.Controls.Add(this.label_Party);
            this.Name = "PiscesMeasurementView";
            this.Size = new System.Drawing.Size(835, 663);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.tabPageData.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGrid dataGrid1;
        private System.Windows.Forms.ContextMenu contextMenu1;
        private System.Windows.Forms.MenuItem menuItemCopy;
        private System.Windows.Forms.MenuItem menuItemPaste;
        private System.Windows.Forms.TabPage tabPageData;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageGraph;
        private System.Windows.Forms.TextBox textBox_Discharge;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label_Gate5;
        private System.Windows.Forms.Label label_Gate6;
        private System.Windows.Forms.TextBox textBox_Gate6;
        private System.Windows.Forms.TextBox textBox_Gate3;
        private System.Windows.Forms.Label label_Gate4;
        private System.Windows.Forms.Label label_Gate2;
        private System.Windows.Forms.TextBox textBox_Gate4;
        private System.Windows.Forms.Label label_Gate1;
        private System.Windows.Forms.TextBox textBox_Gate5;
        private System.Windows.Forms.TextBox textBox_Gate1;
        private System.Windows.Forms.TextBox textBox_Gate2;
        private System.Windows.Forms.Label label_Gate3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_Spin;
        private System.Windows.Forms.TextBox textBox_Meter_Susp;
        private System.Windows.Forms.Label label_Spin;
        private System.Windows.Forms.Label label_Weight_Susp;
        private System.Windows.Forms.Label label_Method;
        private System.Windows.Forms.Label label_Meter_No;
        private System.Windows.Forms.Label label_Meter_Susp;
        private System.Windows.Forms.TextBox textBox_Method;
        private System.Windows.Forms.TextBox textBox_Weight_Susp;
        private System.Windows.Forms.TextBox textBox_Meter_No;
        private System.Windows.Forms.TextBox textBox_Prim_Gage;
        private System.Windows.Forms.TextBox textBox_Sec_Gage;
        private System.Windows.Forms.Button button_Compute_Discharge;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox textBox_Party;
        private System.Windows.Forms.TextBox textBox_Memo;
        private System.Windows.Forms.Label label_Party;

    }
}
