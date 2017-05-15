namespace HydrometTools
{
    partial class ImportDaily
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImportDaily));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxIdwr = new System.Windows.Forms.TextBox();
            this.checkBoxProvisional = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxLastUpdate = new System.Windows.Forms.TextBox();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.labelFileName = new System.Windows.Forms.Label();
            this.checkBoxAutoRefresh = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxSnotel = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxIdaCorp = new System.Windows.Forms.TextBox();
            this.comboBoxSite = new System.Windows.Forms.ComboBox();
            this.buttonSaveCsv = new System.Windows.Forms.Button();
            this.textBoxNotes = new System.Windows.Forms.TextBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxOwrd = new System.Windows.Forms.TextBox();
            this.buttonPrevious = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            this.textBoxPcode = new System.Windows.Forms.TextBox();
            this.pcode = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxUsgs = new System.Windows.Forms.TextBox();
            this.textBoxcbtt = new System.Windows.Forms.TextBox();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.timeSelectorBeginEnd1 = new Reclamation.TimeSeries.Forms.TimeSelectorBeginEnd();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.timeSeriesGraph2 = new Reclamation.TimeSeries.Graphing.TimeSeriesTeeChartGraph();
            this.panelChart = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.textBoxIdwr);
            this.panel1.Controls.Add(this.checkBoxProvisional);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.textBoxLastUpdate);
            this.panel1.Controls.Add(this.buttonOpenFile);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.labelFileName);
            this.panel1.Controls.Add(this.checkBoxAutoRefresh);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.textBoxSnotel);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.textBoxIdaCorp);
            this.panel1.Controls.Add(this.comboBoxSite);
            this.panel1.Controls.Add(this.buttonSaveCsv);
            this.panel1.Controls.Add(this.textBoxNotes);
            this.panel1.Controls.Add(this.labelStatus);
            this.panel1.Controls.Add(this.buttonSave);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.textBoxOwrd);
            this.panel1.Controls.Add(this.buttonPrevious);
            this.panel1.Controls.Add(this.buttonNext);
            this.panel1.Controls.Add(this.textBoxPcode);
            this.panel1.Controls.Add(this.pcode);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBoxUsgs);
            this.panel1.Controls.Add(this.textBoxcbtt);
            this.panel1.Controls.Add(this.buttonRefresh);
            this.panel1.Controls.Add(this.timeSelectorBeginEnd1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(939, 110);
            this.panel1.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(632, 43);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(26, 13);
            this.label9.TabIndex = 39;
            this.label9.Text = "idwr";
            // 
            // textBoxIdwr
            // 
            this.textBoxIdwr.Location = new System.Drawing.Point(617, 59);
            this.textBoxIdwr.Name = "textBoxIdwr";
            this.textBoxIdwr.Size = new System.Drawing.Size(76, 20);
            this.textBoxIdwr.TabIndex = 38;
            // 
            // checkBoxProvisional
            // 
            this.checkBoxProvisional.AutoSize = true;
            this.checkBoxProvisional.Location = new System.Drawing.Point(440, 83);
            this.checkBoxProvisional.Name = "checkBoxProvisional";
            this.checkBoxProvisional.Size = new System.Drawing.Size(76, 17);
            this.checkBoxProvisional.TabIndex = 37;
            this.checkBoxProvisional.Text = "provisional";
            this.toolTip1.SetToolTip(this.checkBoxProvisional, "check to include USGS provisional data");
            this.checkBoxProvisional.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(722, 5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "last update";
            // 
            // textBoxLastUpdate
            // 
            this.textBoxLastUpdate.Location = new System.Drawing.Point(787, 5);
            this.textBoxLastUpdate.Name = "textBoxLastUpdate";
            this.textBoxLastUpdate.ReadOnly = true;
            this.textBoxLastUpdate.Size = new System.Drawing.Size(170, 20);
            this.textBoxLastUpdate.TabIndex = 35;
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Image = ((System.Drawing.Image)(resources.GetObject("buttonOpenFile.Image")));
            this.buttonOpenFile.Location = new System.Drawing.Point(12, 3);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(35, 18);
            this.buttonOpenFile.TabIndex = 34;
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(51, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 33;
            this.label7.Text = "filename:";
            // 
            // labelFileName
            // 
            this.labelFileName.AutoSize = true;
            this.labelFileName.Location = new System.Drawing.Point(103, 4);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(77, 13);
            this.labelFileName.TabIndex = 32;
            this.labelFileName.Text = "select filename";
            // 
            // checkBoxAutoRefresh
            // 
            this.checkBoxAutoRefresh.AutoSize = true;
            this.checkBoxAutoRefresh.Checked = true;
            this.checkBoxAutoRefresh.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAutoRefresh.Location = new System.Drawing.Point(272, 4);
            this.checkBoxAutoRefresh.Name = "checkBoxAutoRefresh";
            this.checkBoxAutoRefresh.Size = new System.Drawing.Size(82, 17);
            this.checkBoxAutoRefresh.TabIndex = 23;
            this.checkBoxAutoRefresh.Text = "auto refresh";
            this.checkBoxAutoRefresh.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(785, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(64, 13);
            this.label6.TabIndex = 22;
            this.label6.Text = "nrcs (snotel)";
            // 
            // textBoxSnotel
            // 
            this.textBoxSnotel.Location = new System.Drawing.Point(787, 59);
            this.textBoxSnotel.Name = "textBoxSnotel";
            this.textBoxSnotel.Size = new System.Drawing.Size(65, 20);
            this.textBoxSnotel.TabIndex = 21;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(703, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "idaho power";
            // 
            // textBoxIdaCorp
            // 
            this.textBoxIdaCorp.Location = new System.Drawing.Point(699, 59);
            this.textBoxIdaCorp.Name = "textBoxIdaCorp";
            this.textBoxIdaCorp.Size = new System.Drawing.Size(82, 20);
            this.textBoxIdaCorp.TabIndex = 19;
            // 
            // comboBoxSite
            // 
            this.comboBoxSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSite.FormattingEnabled = true;
            this.comboBoxSite.Location = new System.Drawing.Point(305, 22);
            this.comboBoxSite.Name = "comboBoxSite";
            this.comboBoxSite.Size = new System.Drawing.Size(429, 21);
            this.comboBoxSite.TabIndex = 18;
            this.comboBoxSite.SelectedIndexChanged += new System.EventHandler(this.comboBoxSite_SelectedIndexChanged);
            // 
            // buttonSaveCsv
            // 
            this.buttonSaveCsv.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonSaveCsv.BackgroundImage")));
            this.buttonSaveCsv.Location = new System.Drawing.Point(876, 79);
            this.buttonSaveCsv.Name = "buttonSaveCsv";
            this.buttonSaveCsv.Size = new System.Drawing.Size(30, 26);
            this.buttonSaveCsv.TabIndex = 17;
            this.toolTip1.SetToolTip(this.buttonSaveCsv, "saves config file");
            this.buttonSaveCsv.UseVisualStyleBackColor = true;
            this.buttonSaveCsv.Click += new System.EventHandler(this.buttonSaveCsv_Click);
            // 
            // textBoxNotes
            // 
            this.textBoxNotes.Location = new System.Drawing.Point(557, 83);
            this.textBoxNotes.Name = "textBoxNotes";
            this.textBoxNotes.Size = new System.Drawing.Size(313, 20);
            this.textBoxNotes.TabIndex = 16;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(42, 83);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(38, 13);
            this.labelStatus.TabIndex = 15;
            this.labelStatus.Text = "status:";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(197, 50);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(62, 23);
            this.buttonSave.TabIndex = 12;
            this.buttonSave.Text = "Import";
            this.toolTip1.SetToolTip(this.buttonSave, "imports data to hydromet database");
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(546, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "owrd";
            // 
            // textBoxOwrd
            // 
            this.textBoxOwrd.Location = new System.Drawing.Point(531, 59);
            this.textBoxOwrd.Name = "textBoxOwrd";
            this.textBoxOwrd.Size = new System.Drawing.Size(84, 20);
            this.textBoxOwrd.TabIndex = 10;
            this.textBoxOwrd.Text = "13018750";
            // 
            // buttonPrevious
            // 
            this.buttonPrevious.Location = new System.Drawing.Point(269, 44);
            this.buttonPrevious.Name = "buttonPrevious";
            this.buttonPrevious.Size = new System.Drawing.Size(30, 23);
            this.buttonPrevious.TabIndex = 9;
            this.buttonPrevious.Text = "<-";
            this.toolTip1.SetToolTip(this.buttonPrevious, "Previous Site");
            this.buttonPrevious.UseVisualStyleBackColor = true;
            this.buttonPrevious.Click += new System.EventHandler(this.buttonPrevious_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Location = new System.Drawing.Point(268, 20);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(31, 23);
            this.buttonNext.TabIndex = 8;
            this.buttonNext.Text = "->";
            this.toolTip1.SetToolTip(this.buttonNext, "Next Site");
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // textBoxPcode
            // 
            this.textBoxPcode.Location = new System.Drawing.Point(393, 60);
            this.textBoxPcode.Name = "textBoxPcode";
            this.textBoxPcode.Size = new System.Drawing.Size(41, 20);
            this.textBoxPcode.TabIndex = 7;
            this.textBoxPcode.Text = "QD";
            // 
            // pcode
            // 
            this.pcode.AutoSize = true;
            this.pcode.Location = new System.Drawing.Point(390, 45);
            this.pcode.Name = "pcode";
            this.pcode.Size = new System.Drawing.Size(37, 13);
            this.pcode.TabIndex = 6;
            this.pcode.Text = "pcode";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(455, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "usgs";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(332, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "cbtt";
            // 
            // textBoxUsgs
            // 
            this.textBoxUsgs.Location = new System.Drawing.Point(440, 60);
            this.textBoxUsgs.Name = "textBoxUsgs";
            this.textBoxUsgs.Size = new System.Drawing.Size(68, 20);
            this.textBoxUsgs.TabIndex = 3;
            this.textBoxUsgs.Text = "13018750";
            // 
            // textBoxcbtt
            // 
            this.textBoxcbtt.Location = new System.Drawing.Point(323, 60);
            this.textBoxcbtt.Name = "textBoxcbtt";
            this.textBoxcbtt.Size = new System.Drawing.Size(64, 20);
            this.textBoxcbtt.TabIndex = 2;
            this.textBoxcbtt.Text = "JKSY";
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(197, 25);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(62, 23);
            this.buttonRefresh.TabIndex = 1;
            this.buttonRefresh.Text = "Refresh";
            this.toolTip1.SetToolTip(this.buttonRefresh, "reads data from internet");
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // timeSelectorBeginEnd1
            // 
            this.timeSelectorBeginEnd1.Location = new System.Drawing.Point(7, 24);
            this.timeSelectorBeginEnd1.Name = "timeSelectorBeginEnd1";
            this.timeSelectorBeginEnd1.ShowTime = false;
            this.timeSelectorBeginEnd1.Size = new System.Drawing.Size(195, 46);
            this.timeSelectorBeginEnd1.T1 = new System.DateTime(2009, 12, 16, 13, 55, 46, 802);
            this.timeSelectorBeginEnd1.T2 = new System.DateTime(2009, 12, 16, 13, 55, 46, 802);
            this.timeSelectorBeginEnd1.TabIndex = 0;
            // 
            // timeSeriesGraph2
            // 
            this.timeSeriesGraph2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.timeSeriesGraph2.Location = new System.Drawing.Point(0, 373);
            this.timeSeriesGraph2.MissingDataValue = -999D;
            this.timeSeriesGraph2.MultiLeftAxis = false;
            this.timeSeriesGraph2.Name = "timeSeriesGraph2";
            this.timeSeriesGraph2.Size = new System.Drawing.Size(939, 159);
            this.timeSeriesGraph2.SubTitle = "";
            this.timeSeriesGraph2.TabIndex = 1;
            this.timeSeriesGraph2.Title = "";
            // 
            // panelChart
            // 
            this.panelChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChart.Location = new System.Drawing.Point(0, 110);
            this.panelChart.Name = "panelChart";
            this.panelChart.Size = new System.Drawing.Size(939, 263);
            this.panelChart.TabIndex = 3;
            // 
            // ImportUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelChart);
            this.Controls.Add(this.timeSeriesGraph2);
            this.Controls.Add(this.panel1);
            this.Name = "ImportUI";
            this.Size = new System.Drawing.Size(939, 532);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Reclamation.TimeSeries.Graphing.TimeSeriesTeeChartGraph timeSeriesGraph2;
        private System.Windows.Forms.Panel panel1;
        private Reclamation.TimeSeries.Forms.TimeSelectorBeginEnd timeSelectorBeginEnd1;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxUsgs;
        private System.Windows.Forms.TextBox textBoxcbtt;
        private System.Windows.Forms.TextBox textBoxPcode;
        private System.Windows.Forms.Label pcode;
        private System.Windows.Forms.Button buttonPrevious;
        private System.Windows.Forms.Button buttonNext;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxOwrd;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.TextBox textBoxNotes;
        private System.Windows.Forms.Button buttonSaveCsv;
        private System.Windows.Forms.ComboBox comboBoxSite;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxIdaCorp;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxSnotel;
        private System.Windows.Forms.CheckBox checkBoxAutoRefresh;
        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxLastUpdate;
        private System.Windows.Forms.CheckBox checkBoxProvisional;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxIdwr;
        private Reclamation.TimeSeries.Graphing.GraphExplorerView teeChartExplorerView1;
        private System.Windows.Forms.Panel panelChart;
    }
}

