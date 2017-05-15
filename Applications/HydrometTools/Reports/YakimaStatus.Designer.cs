namespace HydrometTools.Reports
{
    partial class YakimaStatus
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkLabelOpenReports = new System.Windows.Forms.LinkLabel();
            this.linkLabelWebStatus = new System.Windows.Forms.LinkLabel();
            this.textBoxYear2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxYear1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.checkBox30yravg = new System.Windows.Forms.CheckBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.linkLabelOpenReports);
            this.panel1.Controls.Add(this.linkLabelWebStatus);
            this.panel1.Controls.Add(this.textBoxYear2);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.textBoxYear1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.numericUpDown1);
            this.panel1.Controls.Add(this.dateTimePicker1);
            this.panel1.Controls.Add(this.checkBox30yravg);
            this.panel1.Controls.Add(this.buttonSave);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(655, 122);
            this.panel1.TabIndex = 0;
            // 
            // linkLabelOpenReports
            // 
            this.linkLabelOpenReports.AutoSize = true;
            this.linkLabelOpenReports.Location = new System.Drawing.Point(455, 34);
            this.linkLabelOpenReports.Name = "linkLabelOpenReports";
            this.linkLabelOpenReports.Size = new System.Drawing.Size(66, 13);
            this.linkLabelOpenReports.TabIndex = 14;
            this.linkLabelOpenReports.TabStop = true;
            this.linkLabelOpenReports.Text = "open reports";
            this.linkLabelOpenReports.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelOpenReports_LinkClicked);
            // 
            // linkLabelWebStatus
            // 
            this.linkLabelWebStatus.AutoSize = true;
            this.linkLabelWebStatus.Location = new System.Drawing.Point(244, 87);
            this.linkLabelWebStatus.Name = "linkLabelWebStatus";
            this.linkLabelWebStatus.Size = new System.Drawing.Size(85, 13);
            this.linkLabelWebStatus.TabIndex = 12;
            this.linkLabelWebStatus.TabStop = true;
            this.linkLabelWebStatus.Text = "open web status";
            this.linkLabelWebStatus.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelWebStatus_LinkClicked);
            // 
            // textBoxYear2
            // 
            this.textBoxYear2.Location = new System.Drawing.Point(197, 64);
            this.textBoxYear2.Name = "textBoxYear2";
            this.textBoxYear2.Size = new System.Drawing.Size(45, 20);
            this.textBoxYear2.TabIndex = 11;
            this.textBoxYear2.Text = "2010";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(175, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(16, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "to";
            // 
            // textBoxYear1
            // 
            this.textBoxYear1.Location = new System.Drawing.Point(124, 64);
            this.textBoxYear1.Name = "textBoxYear1";
            this.textBoxYear1.Size = new System.Drawing.Size(45, 20);
            this.textBoxYear1.TabIndex = 9;
            this.textBoxYear1.Text = "1981";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(201, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Y A K I M A  - S T A T U S - R E P O R T";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(359, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "hour";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(362, 37);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(41, 20);
            this.numericUpDown1.TabIndex = 4;
            this.numericUpDown1.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(247, 37);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(92, 20);
            this.dateTimePicker1.TabIndex = 3;
            // 
            // checkBox30yravg
            // 
            this.checkBox30yravg.AutoSize = true;
            this.checkBox30yravg.Location = new System.Drawing.Point(16, 63);
            this.checkBox30yravg.Name = "checkBox30yravg";
            this.checkBox30yravg.Size = new System.Drawing.Size(102, 17);
            this.checkBox30yravg.TabIndex = 2;
            this.checkBox30yravg.Text = "include average";
            this.checkBox30yravg.UseVisualStyleBackColor = true;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(113, 34);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 1;
            this.buttonSave.Text = "save";
            this.toolTip1.SetToolTip(this.buttonSave, "saves report to database and website");
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 34);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "create";
            this.toolTip1.SetToolTip(this.button1, "create report from template");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxStatus.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxStatus.Location = new System.Drawing.Point(0, 122);
            this.textBoxStatus.Multiline = true;
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxStatus.Size = new System.Drawing.Size(655, 399);
            this.textBoxStatus.TabIndex = 1;
            // 
            // YakimaStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxStatus);
            this.Controls.Add(this.panel1);
            this.Name = "YakimaStatus";
            this.Size = new System.Drawing.Size(655, 521);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxStatus;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox30yravg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxYear2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxYear1;
        private System.Windows.Forms.LinkLabel linkLabelWebStatus;
        private System.Windows.Forms.LinkLabel linkLabelOpenReports;
    }
}
