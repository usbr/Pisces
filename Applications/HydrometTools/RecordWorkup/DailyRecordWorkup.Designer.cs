namespace HydrometTools.RecordWorkup
{
    partial class DailyRecordWorkup
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxWaterYear = new System.Windows.Forms.TextBox();
            this.comboBoxSiteList = new System.Windows.Forms.ComboBox();
            this.labelSiteid = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelGraphTable = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tChart1 = new Steema.TeeChart.TChart();
            this.linkLabelRead = new System.Windows.Forms.LinkLabel();
            this.linkLabelSave = new System.Windows.Forms.LinkLabel();
            this.linkLabelCompute = new System.Windows.Forms.LinkLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.linkLabelGraphOption = new System.Windows.Forms.LinkLabel();
            this.panel1.SuspendLayout();
            this.panelGraphTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "water year";
            // 
            // textBoxWaterYear
            // 
            this.textBoxWaterYear.Location = new System.Drawing.Point(80, 41);
            this.textBoxWaterYear.Name = "textBoxWaterYear";
            this.textBoxWaterYear.Size = new System.Drawing.Size(69, 20);
            this.textBoxWaterYear.TabIndex = 1;
            // 
            // comboBoxSiteList
            // 
            this.comboBoxSiteList.FormattingEnabled = true;
            this.comboBoxSiteList.Location = new System.Drawing.Point(14, 13);
            this.comboBoxSiteList.Name = "comboBoxSiteList";
            this.comboBoxSiteList.Size = new System.Drawing.Size(449, 21);
            this.comboBoxSiteList.TabIndex = 2;
            this.comboBoxSiteList.SelectedIndexChanged += new System.EventHandler(this.comboBoxSiteList_SelectedIndexChanged);
            // 
            // labelSiteid
            // 
            this.labelSiteid.AutoSize = true;
            this.labelSiteid.Location = new System.Drawing.Point(177, 44);
            this.labelSiteid.Name = "labelSiteid";
            this.labelSiteid.Size = new System.Drawing.Size(174, 13);
            this.labelSiteid.TabIndex = 3;
            this.labelSiteid.Text = "please select a site in the list above";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.linkLabelGraphOption);
            this.panel1.Controls.Add(this.linkLabelCompute);
            this.panel1.Controls.Add(this.linkLabelSave);
            this.panel1.Controls.Add(this.linkLabelRead);
            this.panel1.Controls.Add(this.comboBoxSiteList);
            this.panel1.Controls.Add(this.labelSiteid);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBoxWaterYear);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(782, 107);
            this.panel1.TabIndex = 4;
            // 
            // panelGraphTable
            // 
            this.panelGraphTable.Controls.Add(this.splitter1);
            this.panelGraphTable.Controls.Add(this.tChart1);
            this.panelGraphTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGraphTable.Location = new System.Drawing.Point(0, 107);
            this.panelGraphTable.Name = "panelGraphTable";
            this.panelGraphTable.Size = new System.Drawing.Size(782, 492);
            this.panelGraphTable.TabIndex = 28;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(510, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(6, 492);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // tChart1
            // 
            // 
            // 
            // 
            this.tChart1.Aspect.View3D = false;
            this.tChart1.Aspect.ZOffset = 0D;
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Bottom.AxisPen.Width = 1;
            // 
            // 
            // 
            this.tChart1.Axes.Bottom.Title.Transparent = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Depth.Title.Transparent = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.DepthTop.Title.Transparent = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Left.AxisPen.Width = 1;
            this.tChart1.Axes.Left.EndPosition = 99D;
            this.tChart1.Axes.Left.StartPosition = 1D;
            // 
            // 
            // 
            this.tChart1.Axes.Left.Title.Transparent = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Right.AxisPen.Transparency = 77;
            this.tChart1.Axes.Right.AxisPen.Width = 0;
            // 
            // 
            // 
            this.tChart1.Axes.Right.Grid.Transparency = 80;
            // 
            // 
            // 
            this.tChart1.Axes.Right.Title.Transparent = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Axes.Top.Title.Transparent = true;
            this.tChart1.Axes.Top.Visible = false;
            this.tChart1.BackColor = System.Drawing.Color.Transparent;
            this.tChart1.Dock = System.Windows.Forms.DockStyle.Left;
            // 
            // 
            // 
            this.tChart1.Header.Lines = new string[] {
        ""};
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Legend.Font.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(68)))), ((int)(((byte)(102)))), ((int)(((byte)(163)))));
            this.tChart1.Legend.FontSeriesColor = true;
            this.tChart1.Location = new System.Drawing.Point(0, 0);
            this.tChart1.Name = "tChart1";
            // 
            // 
            // 
            // 
            // 
            // 
            this.tChart1.Panel.Bevel.ColorOne = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.tChart1.Panel.Brush.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            // 
            // 
            // 
            this.tChart1.Panel.Brush.Gradient.Visible = false;
            this.tChart1.Size = new System.Drawing.Size(510, 492);
            this.tChart1.TabIndex = 3;
            // 
            // 
            // 
            this.tChart1.Walls.Visible = false;
            // 
            // linkLabelRead
            // 
            this.linkLabelRead.AutoSize = true;
            this.linkLabelRead.Location = new System.Drawing.Point(106, 77);
            this.linkLabelRead.Name = "linkLabelRead";
            this.linkLabelRead.Size = new System.Drawing.Size(97, 13);
            this.linkLabelRead.TabIndex = 4;
            this.linkLabelRead.TabStop = true;
            this.linkLabelRead.Text = "read from hydromet";
            this.linkLabelRead.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelRead_LinkClicked);
            // 
            // linkLabelSave
            // 
            this.linkLabelSave.AutoSize = true;
            this.linkLabelSave.Location = new System.Drawing.Point(209, 77);
            this.linkLabelSave.Name = "linkLabelSave";
            this.linkLabelSave.Size = new System.Drawing.Size(144, 13);
            this.linkLabelSave.TabIndex = 5;
            this.linkLabelSave.TabStop = true;
            this.linkLabelSave.Text = "save flows/shifts to hydromet";
            // 
            // linkLabelCompute
            // 
            this.linkLabelCompute.AutoSize = true;
            this.linkLabelCompute.Location = new System.Drawing.Point(372, 77);
            this.linkLabelCompute.Name = "linkLabelCompute";
            this.linkLabelCompute.Size = new System.Drawing.Size(62, 13);
            this.linkLabelCompute.TabIndex = 6;
            this.linkLabelCompute.TabStop = true;
            this.linkLabelCompute.Text = "apply shifts ";
            this.toolTip1.SetToolTip(this.linkLabelCompute, "compute a new flow using shifts and rating table");
            this.linkLabelCompute.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelCompute_LinkClicked);
            // 
            // linkLabelGraphOption
            // 
            this.linkLabelGraphOption.AutoSize = true;
            this.linkLabelGraphOption.Location = new System.Drawing.Point(11, 77);
            this.linkLabelGraphOption.Name = "linkLabelGraphOption";
            this.linkLabelGraphOption.Size = new System.Drawing.Size(71, 13);
            this.linkLabelGraphOption.TabIndex = 7;
            this.linkLabelGraphOption.TabStop = true;
            this.linkLabelGraphOption.Text = "graph options";
            this.toolTip1.SetToolTip(this.linkLabelGraphOption, "compute a new flow using shifts and rating table");
            this.linkLabelGraphOption.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelGraphOption_LinkClicked);
            // 
            // DailyRecordWorkup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelGraphTable);
            this.Controls.Add(this.panel1);
            this.Name = "DailyRecordWorkup";
            this.Size = new System.Drawing.Size(782, 599);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelGraphTable.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxWaterYear;
        private System.Windows.Forms.ComboBox comboBoxSiteList;
        private System.Windows.Forms.Label labelSiteid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelGraphTable;
        private System.Windows.Forms.Splitter splitter1;
        private Steema.TeeChart.TChart tChart1;
        private System.Windows.Forms.LinkLabel linkLabelCompute;
        private System.Windows.Forms.LinkLabel linkLabelSave;
        private System.Windows.Forms.LinkLabel linkLabelRead;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.LinkLabel linkLabelGraphOption;
    }
}
