namespace HydrometForecast
{
    partial class RunForecast
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
            this.components = new System.ComponentModel.Container();
            this.buttonRun = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkLabelLogfiles = new System.Windows.Forms.LinkLabel();
            this.linkLabelNotepad = new System.Windows.Forms.LinkLabel();
            this.comboBoxSubsequent = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonTestAllYears = new System.Windows.Forms.Button();
            this.checkBoxLookAhead = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePickerForecastDate = new System.Windows.Forms.DateTimePicker();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSummary = new System.Windows.Forms.TabPage();
            this.textBoxSummary = new System.Windows.Forms.TextBox();
            this.tabPageDetails = new System.Windows.Forms.TabPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.forecastList1 = new HydrometForecast.ForecastList();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageSummary.SuspendLayout();
            this.tabPageDetails.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonRun
            // 
            this.buttonRun.Location = new System.Drawing.Point(13, 55);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(62, 23);
            this.buttonRun.TabIndex = 2;
            this.buttonRun.Text = "run";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.linkLabelLogfiles);
            this.panel1.Controls.Add(this.linkLabelNotepad);
            this.panel1.Controls.Add(this.comboBoxSubsequent);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.buttonTestAllYears);
            this.panel1.Controls.Add(this.checkBoxLookAhead);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dateTimePickerForecastDate);
            this.panel1.Controls.Add(this.buttonRun);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(248, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(503, 98);
            this.panel1.TabIndex = 3;
            // 
            // linkLabelLogfiles
            // 
            this.linkLabelLogfiles.AutoSize = true;
            this.linkLabelLogfiles.Location = new System.Drawing.Point(230, 78);
            this.linkLabelLogfiles.Name = "linkLabelLogfiles";
            this.linkLabelLogfiles.Size = new System.Drawing.Size(37, 13);
            this.linkLabelLogfiles.TabIndex = 11;
            this.linkLabelLogfiles.TabStop = true;
            this.linkLabelLogfiles.Text = "log file";
            this.linkLabelLogfiles.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelLogfiles_LinkClicked);
            // 
            // linkLabelNotepad
            // 
            this.linkLabelNotepad.AutoSize = true;
            this.linkLabelNotepad.Location = new System.Drawing.Point(120, 78);
            this.linkLabelNotepad.Name = "linkLabelNotepad";
            this.linkLabelNotepad.Size = new System.Drawing.Size(84, 13);
            this.linkLabelNotepad.TabIndex = 10;
            this.linkLabelNotepad.TabStop = true;
            this.linkLabelNotepad.Text = "open in notepad";
            this.linkLabelNotepad.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelNotepad_LinkClicked);
            // 
            // comboBoxSubsequent
            // 
            this.comboBoxSubsequent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSubsequent.FormattingEnabled = true;
            this.comboBoxSubsequent.Items.AddRange(new object[] {
            "100%",
            "100% 120%  80%",
            "100% 150%  50%"});
            this.comboBoxSubsequent.Location = new System.Drawing.Point(310, 65);
            this.comboBoxSubsequent.Name = "comboBoxSubsequent";
            this.comboBoxSubsequent.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSubsequent.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(307, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "subsequent conditions";
            // 
            // buttonTestAllYears
            // 
            this.buttonTestAllYears.Location = new System.Drawing.Point(81, 55);
            this.buttonTestAllYears.Name = "buttonTestAllYears";
            this.buttonTestAllYears.Size = new System.Drawing.Size(75, 23);
            this.buttonTestAllYears.TabIndex = 6;
            this.buttonTestAllYears.Text = "run all years";
            this.toolTip1.SetToolTip(this.buttonTestAllYears, "forecast each historical year, and compare to actual runoff");
            this.buttonTestAllYears.UseVisualStyleBackColor = true;
            this.buttonTestAllYears.Click += new System.EventHandler(this.buttonTestAllYears_Click);
            // 
            // checkBoxLookAhead
            // 
            this.checkBoxLookAhead.AutoSize = true;
            this.checkBoxLookAhead.Location = new System.Drawing.Point(13, 32);
            this.checkBoxLookAhead.Name = "checkBoxLookAhead";
            this.checkBoxLookAhead.Size = new System.Drawing.Size(254, 17);
            this.checkBoxLookAhead.TabIndex = 5;
            this.checkBoxLookAhead.Text = "perfect forecast (use future data when available)";
            this.toolTip1.SetToolTip(this.checkBoxLookAhead, "when running a historical year look at future data beyond the forecast date");
            this.checkBoxLookAhead.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "forecast date:";
            // 
            // dateTimePickerForecastDate
            // 
            this.dateTimePickerForecastDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerForecastDate.Location = new System.Drawing.Point(85, 10);
            this.dateTimePickerForecastDate.Name = "dateTimePickerForecastDate";
            this.dateTimePickerForecastDate.Size = new System.Drawing.Size(99, 20);
            this.dateTimePickerForecastDate.TabIndex = 3;
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOutput.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxOutput.Location = new System.Drawing.Point(3, 3);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxOutput.Size = new System.Drawing.Size(489, 308);
            this.textBoxOutput.TabIndex = 4;
            this.textBoxOutput.WordWrap = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageSummary);
            this.tabControl1.Controls.Add(this.tabPageDetails);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(248, 98);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(503, 340);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPageSummary
            // 
            this.tabPageSummary.Controls.Add(this.statusStrip1);
            this.tabPageSummary.Controls.Add(this.textBoxSummary);
            this.tabPageSummary.Location = new System.Drawing.Point(4, 22);
            this.tabPageSummary.Name = "tabPageSummary";
            this.tabPageSummary.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSummary.Size = new System.Drawing.Size(495, 314);
            this.tabPageSummary.TabIndex = 1;
            this.tabPageSummary.Text = "summary";
            this.tabPageSummary.UseVisualStyleBackColor = true;
            // 
            // textBoxSummary
            // 
            this.textBoxSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSummary.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSummary.Location = new System.Drawing.Point(3, 3);
            this.textBoxSummary.Multiline = true;
            this.textBoxSummary.Name = "textBoxSummary";
            this.textBoxSummary.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxSummary.Size = new System.Drawing.Size(489, 308);
            this.textBoxSummary.TabIndex = 5;
            this.textBoxSummary.WordWrap = false;
            // 
            // tabPageDetails
            // 
            this.tabPageDetails.Controls.Add(this.textBoxOutput);
            this.tabPageDetails.Location = new System.Drawing.Point(4, 22);
            this.tabPageDetails.Name = "tabPageDetails";
            this.tabPageDetails.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDetails.Size = new System.Drawing.Size(495, 314);
            this.tabPageDetails.TabIndex = 0;
            this.tabPageDetails.Text = "details";
            this.tabPageDetails.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(3, 289);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(489, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // forecastList1
            // 
            this.forecastList1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.forecastList1.Dock = System.Windows.Forms.DockStyle.Left;
            this.forecastList1.Location = new System.Drawing.Point(0, 0);
            this.forecastList1.Name = "forecastList1";
            this.forecastList1.Size = new System.Drawing.Size(248, 438);
            this.forecastList1.TabIndex = 1;
            // 
            // RunForecast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.forecastList1);
            this.Name = "RunForecast";
            this.Size = new System.Drawing.Size(751, 438);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageSummary.ResumeLayout(false);
            this.tabPageSummary.PerformLayout();
            this.tabPageDetails.ResumeLayout(false);
            this.tabPageDetails.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ForecastList forecastList1;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePickerForecastDate;
        private System.Windows.Forms.CheckBox checkBoxLookAhead;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.Button buttonTestAllYears;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageDetails;
        private System.Windows.Forms.TabPage tabPageSummary;
        private System.Windows.Forms.TextBox textBoxSummary;
        private System.Windows.Forms.ComboBox comboBoxSubsequent;
        private System.Windows.Forms.LinkLabel linkLabelNotepad;
        private System.Windows.Forms.LinkLabel linkLabelLogfiles;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}
