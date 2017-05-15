namespace HydrometTools.Advanced
{
    partial class AdvancedControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageVms = new System.Windows.Forms.TabPage();
            this.tabPageLinux = new System.Windows.Forms.TabPage();
            this.tabControlLinux = new System.Windows.Forms.TabControl();
            this.tabPageSites = new System.Windows.Forms.TabPage();
            this.tabPageMCFToDecodes = new System.Windows.Forms.TabPage();
            this.tabPageQuality = new System.Windows.Forms.TabPage();
            this.dataGridViewQuality = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxHoursBack = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonDownloadNessid = new System.Windows.Forms.Button();
            this.textBoxNessid = new System.Windows.Forms.TextBox();
            this.linkLabelHelp = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonDownloadQuality = new System.Windows.Forms.Button();
            this.textBoxCbttQuality = new System.Windows.Forms.TextBox();
            this.tabPageEncode = new System.Windows.Forms.TabPage();
            this.tabPageEquations = new System.Windows.Forms.TabPage();
            this.rawDataInput1 = new HydrometTools.RawDataInput();
            this.dayfileInsert1 = new HydrometTools.Advanced.DayfileInsert();
            this.archiverFileList1 = new HydrometTools.Advanced.ArchiverFileList();
            this.mathInput1 = new HydrometTools.MathInput();
            this.archiverInput1 = new HydrometTools.Advanced.ArchiverInput();
            this.mcf2Decodes1 = new HydrometTools.Advanced.Mcf2Decodes();
            this.encodeAscii1 = new HydrometTools.Advanced.ManualEncodeDecode.EncodeAscii();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageVms.SuspendLayout();
            this.tabPageLinux.SuspendLayout();
            this.tabControlLinux.SuspendLayout();
            this.tabPageMCFToDecodes.SuspendLayout();
            this.tabPageQuality.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewQuality)).BeginInit();
            this.panel1.SuspendLayout();
            this.tabPageEncode.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rawDataInput1);
            this.groupBox1.Location = new System.Drawing.Point(12, 17);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(315, 187);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "rawdata";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.archiverInput1);
            this.groupBox2.Location = new System.Drawing.Point(12, 219);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(315, 167);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "daily calculation (archiver)";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.mathInput1);
            this.groupBox3.Location = new System.Drawing.Point(342, 25);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(327, 221);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "instant rating table calculation (dayfiles)";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dayfileInsert1);
            this.groupBox4.Location = new System.Drawing.Point(360, 247);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(301, 180);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "insert zeros into instant database (dayfiles)";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.archiverFileList1);
            this.groupBox5.Location = new System.Drawing.Point(15, 392);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(330, 205);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "daily calculation -- multiple sites in txt file";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageVms);
            this.tabControl1.Controls.Add(this.tabPageLinux);
            this.tabControl1.Controls.Add(this.tabPageQuality);
            this.tabControl1.Controls.Add(this.tabPageEncode);
            this.tabControl1.Controls.Add(this.tabPageEquations);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(737, 684);
            this.tabControl1.TabIndex = 9;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPageVms
            // 
            this.tabPageVms.Controls.Add(this.groupBox1);
            this.tabPageVms.Controls.Add(this.groupBox4);
            this.tabPageVms.Controls.Add(this.groupBox5);
            this.tabPageVms.Controls.Add(this.groupBox3);
            this.tabPageVms.Controls.Add(this.groupBox2);
            this.tabPageVms.Location = new System.Drawing.Point(4, 22);
            this.tabPageVms.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageVms.Name = "tabPageVms";
            this.tabPageVms.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageVms.Size = new System.Drawing.Size(729, 658);
            this.tabPageVms.TabIndex = 0;
            this.tabPageVms.Text = "openvms";
            this.tabPageVms.UseVisualStyleBackColor = true;
            // 
            // tabPageLinux
            // 
            this.tabPageLinux.Controls.Add(this.tabControlLinux);
            this.tabPageLinux.Location = new System.Drawing.Point(4, 22);
            this.tabPageLinux.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageLinux.Name = "tabPageLinux";
            this.tabPageLinux.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageLinux.Size = new System.Drawing.Size(729, 658);
            this.tabPageLinux.TabIndex = 1;
            this.tabPageLinux.Text = "linux";
            this.tabPageLinux.UseVisualStyleBackColor = true;
            // 
            // tabControlLinux
            // 
            this.tabControlLinux.Controls.Add(this.tabPageSites);
            this.tabControlLinux.Controls.Add(this.tabPageMCFToDecodes);
            this.tabControlLinux.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlLinux.Location = new System.Drawing.Point(2, 2);
            this.tabControlLinux.Name = "tabControlLinux";
            this.tabControlLinux.SelectedIndex = 0;
            this.tabControlLinux.Size = new System.Drawing.Size(725, 654);
            this.tabControlLinux.TabIndex = 1;
            this.tabControlLinux.SelectedIndexChanged += new System.EventHandler(this.tabControlLinux_SelectedIndexChanged);
            // 
            // tabPageSites
            // 
            this.tabPageSites.Location = new System.Drawing.Point(4, 22);
            this.tabPageSites.Name = "tabPageSites";
            this.tabPageSites.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSites.Size = new System.Drawing.Size(717, 628);
            this.tabPageSites.TabIndex = 0;
            this.tabPageSites.Text = "sites";
            this.tabPageSites.UseVisualStyleBackColor = true;
            // 
            // tabPageMCFToDecodes
            // 
            this.tabPageMCFToDecodes.Controls.Add(this.mcf2Decodes1);
            this.tabPageMCFToDecodes.Location = new System.Drawing.Point(4, 22);
            this.tabPageMCFToDecodes.Name = "tabPageMCFToDecodes";
            this.tabPageMCFToDecodes.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMCFToDecodes.Size = new System.Drawing.Size(717, 628);
            this.tabPageMCFToDecodes.TabIndex = 1;
            this.tabPageMCFToDecodes.Text = "mcf-->decodes";
            this.tabPageMCFToDecodes.UseVisualStyleBackColor = true;
            // 
            // tabPageQuality
            // 
            this.tabPageQuality.Controls.Add(this.dataGridViewQuality);
            this.tabPageQuality.Controls.Add(this.panel1);
            this.tabPageQuality.Location = new System.Drawing.Point(4, 22);
            this.tabPageQuality.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageQuality.Name = "tabPageQuality";
            this.tabPageQuality.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageQuality.Size = new System.Drawing.Size(729, 658);
            this.tabPageQuality.TabIndex = 2;
            this.tabPageQuality.Text = "quality";
            this.tabPageQuality.UseVisualStyleBackColor = true;
            // 
            // dataGridViewQuality
            // 
            this.dataGridViewQuality.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewQuality.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewQuality.Location = new System.Drawing.Point(2, 88);
            this.dataGridViewQuality.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewQuality.Name = "dataGridViewQuality";
            this.dataGridViewQuality.RowTemplate.Height = 24;
            this.dataGridViewQuality.Size = new System.Drawing.Size(725, 568);
            this.dataGridViewQuality.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBoxHoursBack);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.buttonDownloadNessid);
            this.panel1.Controls.Add(this.textBoxNessid);
            this.panel1.Controls.Add(this.linkLabelHelp);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.buttonDownloadQuality);
            this.panel1.Controls.Add(this.textBoxCbttQuality);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(725, 86);
            this.panel1.TabIndex = 1;
            // 
            // textBoxHoursBack
            // 
            this.textBoxHoursBack.Location = new System.Drawing.Point(91, 11);
            this.textBoxHoursBack.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxHoursBack.Name = "textBoxHoursBack";
            this.textBoxHoursBack.Size = new System.Drawing.Size(59, 20);
            this.textBoxHoursBack.TabIndex = 9;
            this.textBoxHoursBack.Text = "24";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 13);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "hours back";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 61);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "nessid";
            // 
            // buttonDownloadNessid
            // 
            this.buttonDownloadNessid.Location = new System.Drawing.Point(110, 61);
            this.buttonDownloadNessid.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDownloadNessid.Name = "buttonDownloadNessid";
            this.buttonDownloadNessid.Size = new System.Drawing.Size(58, 19);
            this.buttonDownloadNessid.TabIndex = 6;
            this.buttonDownloadNessid.Text = "Download";
            this.buttonDownloadNessid.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonDownloadNessid.UseVisualStyleBackColor = true;
            this.buttonDownloadNessid.Click += new System.EventHandler(this.buttonDownloadNessid_Click);
            // 
            // textBoxNessid
            // 
            this.textBoxNessid.Location = new System.Drawing.Point(47, 61);
            this.textBoxNessid.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxNessid.Name = "textBoxNessid";
            this.textBoxNessid.Size = new System.Drawing.Size(59, 20);
            this.textBoxNessid.TabIndex = 5;
            this.textBoxNessid.Text = "34748654";
            // 
            // linkLabelHelp
            // 
            this.linkLabelHelp.AutoSize = true;
            this.linkLabelHelp.Location = new System.Drawing.Point(268, 28);
            this.linkLabelHelp.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.linkLabelHelp.Name = "linkLabelHelp";
            this.linkLabelHelp.Size = new System.Drawing.Size(27, 13);
            this.linkLabelHelp.TabIndex = 4;
            this.linkLabelHelp.TabStop = true;
            this.linkLabelHelp.Text = "help";
            this.linkLabelHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelHelp_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 37);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "cbtt";
            // 
            // buttonDownloadQuality
            // 
            this.buttonDownloadQuality.Location = new System.Drawing.Point(110, 34);
            this.buttonDownloadQuality.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDownloadQuality.Name = "buttonDownloadQuality";
            this.buttonDownloadQuality.Size = new System.Drawing.Size(58, 19);
            this.buttonDownloadQuality.TabIndex = 2;
            this.buttonDownloadQuality.Text = "Download";
            this.buttonDownloadQuality.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonDownloadQuality.UseVisualStyleBackColor = true;
            this.buttonDownloadQuality.Click += new System.EventHandler(this.buttonDownloadQuality_Click);
            // 
            // textBoxCbttQuality
            // 
            this.textBoxCbttQuality.Location = new System.Drawing.Point(47, 34);
            this.textBoxCbttQuality.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxCbttQuality.Name = "textBoxCbttQuality";
            this.textBoxCbttQuality.Size = new System.Drawing.Size(59, 20);
            this.textBoxCbttQuality.TabIndex = 0;
            this.textBoxCbttQuality.Text = "bigi";
            // 
            // tabPageEncode
            // 
            this.tabPageEncode.Controls.Add(this.encodeAscii1);
            this.tabPageEncode.Location = new System.Drawing.Point(4, 22);
            this.tabPageEncode.Name = "tabPageEncode";
            this.tabPageEncode.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEncode.Size = new System.Drawing.Size(729, 658);
            this.tabPageEncode.TabIndex = 3;
            this.tabPageEncode.Text = "encode";
            this.tabPageEncode.UseVisualStyleBackColor = true;
            // 
            // tabPageEquations
            // 
            this.tabPageEquations.Location = new System.Drawing.Point(4, 22);
            this.tabPageEquations.Name = "tabPageEquations";
            this.tabPageEquations.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEquations.Size = new System.Drawing.Size(729, 658);
            this.tabPageEquations.TabIndex = 4;
            this.tabPageEquations.Text = "equations";
            this.tabPageEquations.UseVisualStyleBackColor = true;
            // 
            // rawDataInput1
            // 
            this.rawDataInput1.Location = new System.Drawing.Point(6, 19);
            this.rawDataInput1.Margin = new System.Windows.Forms.Padding(4);
            this.rawDataInput1.Name = "rawDataInput1";
            this.rawDataInput1.Size = new System.Drawing.Size(292, 162);
            this.rawDataInput1.TabIndex = 0;
            // 
            // dayfileInsert1
            // 
            this.dayfileInsert1.Location = new System.Drawing.Point(6, 19);
            this.dayfileInsert1.Margin = new System.Windows.Forms.Padding(4);
            this.dayfileInsert1.Name = "dayfileInsert1";
            this.dayfileInsert1.Size = new System.Drawing.Size(271, 138);
            this.dayfileInsert1.TabIndex = 5;
            // 
            // archiverFileList1
            // 
            this.archiverFileList1.Location = new System.Drawing.Point(4, 22);
            this.archiverFileList1.Margin = new System.Windows.Forms.Padding(4);
            this.archiverFileList1.Name = "archiverFileList1";
            this.archiverFileList1.Size = new System.Drawing.Size(317, 168);
            this.archiverFileList1.TabIndex = 7;
            // 
            // mathInput1
            // 
            this.mathInput1.Location = new System.Drawing.Point(6, 19);
            this.mathInput1.Margin = new System.Windows.Forms.Padding(4);
            this.mathInput1.Name = "mathInput1";
            this.mathInput1.Size = new System.Drawing.Size(315, 196);
            this.mathInput1.TabIndex = 0;
            // 
            // archiverInput1
            // 
            this.archiverInput1.Location = new System.Drawing.Point(6, 19);
            this.archiverInput1.Margin = new System.Windows.Forms.Padding(4);
            this.archiverInput1.Name = "archiverInput1";
            this.archiverInput1.Size = new System.Drawing.Size(303, 137);
            this.archiverInput1.TabIndex = 1;
            this.archiverInput1.Load += new System.EventHandler(this.archiverInput1_Load);
            // 
            // mcf2Decodes1
            // 
            this.mcf2Decodes1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mcf2Decodes1.Location = new System.Drawing.Point(3, 3);
            this.mcf2Decodes1.Margin = new System.Windows.Forms.Padding(2);
            this.mcf2Decodes1.Name = "mcf2Decodes1";
            this.mcf2Decodes1.Size = new System.Drawing.Size(711, 622);
            this.mcf2Decodes1.TabIndex = 0;
            // 
            // encodeAscii1
            // 
            this.encodeAscii1.Location = new System.Drawing.Point(27, 33);
            this.encodeAscii1.Name = "encodeAscii1";
            this.encodeAscii1.Size = new System.Drawing.Size(456, 297);
            this.encodeAscii1.TabIndex = 0;
            // 
            // AdvancedControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "AdvancedControl";
            this.Size = new System.Drawing.Size(737, 684);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageVms.ResumeLayout(false);
            this.tabPageLinux.ResumeLayout(false);
            this.tabControlLinux.ResumeLayout(false);
            this.tabPageMCFToDecodes.ResumeLayout(false);
            this.tabPageQuality.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewQuality)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPageEncode.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private RawDataInput rawDataInput1;
        private ArchiverInput archiverInput1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private MathInput mathInput1;
        private DayfileInsert dayfileInsert1;
        private System.Windows.Forms.GroupBox groupBox4;
        private ArchiverFileList archiverFileList1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageVms;
        private System.Windows.Forms.TabPage tabPageLinux;
        private System.Windows.Forms.TabPage tabPageQuality;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonDownloadQuality;
        private System.Windows.Forms.TextBox textBoxCbttQuality;
        private System.Windows.Forms.DataGridView dataGridViewQuality;
        private System.Windows.Forms.LinkLabel linkLabelHelp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonDownloadNessid;
        private System.Windows.Forms.TextBox textBoxNessid;
        private System.Windows.Forms.TextBox textBoxHoursBack;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabControl tabControlLinux;
        private System.Windows.Forms.TabPage tabPageSites;
        private System.Windows.Forms.TabPage tabPageMCFToDecodes;
        private Mcf2Decodes mcf2Decodes1;
        private System.Windows.Forms.TabPage tabPageEncode;
        private ManualEncodeDecode.EncodeAscii encodeAscii1;
        private System.Windows.Forms.TabPage tabPageEquations;
    }
}
