using FcPlot;
namespace FcPlot
{
    partial class FcPlotUI
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
            Reclamation.Core.MonthDayRange monthDayRange1 = new Reclamation.Core.MonthDayRange();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBoxTargetPercentages = new System.Windows.Forms.TextBox();
            this.compareData = new System.Windows.Forms.Label();
            this.dataInitial = new System.Windows.Forms.Label();
            this.showGreenLines = new System.Windows.Forms.CheckBox();
            this.showTarget = new System.Windows.Forms.CheckBox();
            this.pcodeComparison = new System.Windows.Forms.TextBox();
            this.pcodeInitial = new System.Windows.Forms.TextBox();
            this.waterYearLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxAlternateWaterYear = new System.Windows.Forms.TextBox();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.monthRangePicker1 = new Reclamation.TimeSeries.Forms.MonthRangePicker();
            this.textBoxWaterYear = new System.Windows.Forms.TextBox();
            this.comboBoxSite = new System.Windows.Forms.ComboBox();
            this.linkLabelReport = new System.Windows.Forms.LinkLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageGraph = new System.Windows.Forms.TabPage();
            this.hydrometChart1 = new FcPlot.HydrometTeeChart();
            this.tabPageReport = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelFlagLegend = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxDashed = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageGraph.SuspendLayout();
            this.tabPageReport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBoxDashed);
            this.panel1.Controls.Add(this.textBoxTargetPercentages);
            this.panel1.Controls.Add(this.compareData);
            this.panel1.Controls.Add(this.dataInitial);
            this.panel1.Controls.Add(this.showGreenLines);
            this.panel1.Controls.Add(this.showTarget);
            this.panel1.Controls.Add(this.pcodeComparison);
            this.panel1.Controls.Add(this.pcodeInitial);
            this.panel1.Controls.Add(this.waterYearLabel);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBoxAlternateWaterYear);
            this.panel1.Controls.Add(this.buttonRefresh);
            this.panel1.Controls.Add(this.monthRangePicker1);
            this.panel1.Controls.Add(this.textBoxWaterYear);
            this.panel1.Controls.Add(this.comboBoxSite);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(730, 84);
            this.panel1.TabIndex = 2;
            // 
            // textBoxTargetPercentages
            // 
            this.textBoxTargetPercentages.Location = new System.Drawing.Point(608, 31);
            this.textBoxTargetPercentages.Name = "textBoxTargetPercentages";
            this.textBoxTargetPercentages.Size = new System.Drawing.Size(115, 20);
            this.textBoxTargetPercentages.TabIndex = 13;
            this.toolTip1.SetToolTip(this.textBoxTargetPercentages, "space separated target levels: 100 120");
            // 
            // compareData
            // 
            this.compareData.AutoSize = true;
            this.compareData.Location = new System.Drawing.Point(308, 62);
            this.compareData.Name = "compareData";
            this.compareData.Size = new System.Drawing.Size(85, 13);
            this.compareData.TabIndex = 12;
            this.compareData.Text = "comparison data";
            // 
            // dataInitial
            // 
            this.dataInitial.AutoSize = true;
            this.dataInitial.Location = new System.Drawing.Point(4, 52);
            this.dataInitial.Name = "dataInitial";
            this.dataInitial.Size = new System.Drawing.Size(54, 13);
            this.dataInitial.TabIndex = 11;
            this.dataInitial.Text = "initial data";
            // 
            // showGreenLines
            // 
            this.showGreenLines.AutoSize = true;
            this.showGreenLines.Checked = true;
            this.showGreenLines.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showGreenLines.Location = new System.Drawing.Point(519, 58);
            this.showGreenLines.Name = "showGreenLines";
            this.showGreenLines.Size = new System.Drawing.Size(109, 17);
            this.showGreenLines.TabIndex = 10;
            this.showGreenLines.Text = "Show Rule Curve";
            this.showGreenLines.UseVisualStyleBackColor = true;
            // 
            // showTarget
            // 
            this.showTarget.AutoSize = true;
            this.showTarget.Location = new System.Drawing.Point(519, 34);
            this.showTarget.Name = "showTarget";
            this.showTarget.Size = new System.Drawing.Size(87, 17);
            this.showTarget.TabIndex = 9;
            this.showTarget.Text = "Show Target";
            this.showTarget.UseVisualStyleBackColor = true;
            // 
            // pcodeComparison
            // 
            this.pcodeComparison.Location = new System.Drawing.Point(398, 60);
            this.pcodeComparison.Name = "pcodeComparison";
            this.pcodeComparison.Size = new System.Drawing.Size(115, 20);
            this.pcodeComparison.TabIndex = 8;
            this.toolTip1.SetToolTip(this.pcodeComparison, "comparision series such as:  luc qu");
            // 
            // pcodeInitial
            // 
            this.pcodeInitial.Location = new System.Drawing.Point(63, 51);
            this.pcodeInitial.Name = "pcodeInitial";
            this.pcodeInitial.Size = new System.Drawing.Size(142, 20);
            this.pcodeInitial.TabIndex = 7;
            this.toolTip1.SetToolTip(this.pcodeInitial, "additional series such as:   bigi qd");
            // 
            // waterYearLabel
            // 
            this.waterYearLabel.AutoSize = true;
            this.waterYearLabel.Location = new System.Drawing.Point(4, 28);
            this.waterYearLabel.Name = "waterYearLabel";
            this.waterYearLabel.Size = new System.Drawing.Size(53, 13);
            this.waterYearLabel.TabIndex = 6;
            this.waterYearLabel.Text = "initial year";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(308, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "comparison year";
            // 
            // textBoxAlternateWaterYear
            // 
            this.textBoxAlternateWaterYear.Location = new System.Drawing.Point(398, 34);
            this.textBoxAlternateWaterYear.Name = "textBoxAlternateWaterYear";
            this.textBoxAlternateWaterYear.Size = new System.Drawing.Size(115, 20);
            this.textBoxAlternateWaterYear.TabIndex = 4;
            this.toolTip1.SetToolTip(this.textBoxAlternateWaterYear, "comparision year such as: 1997");
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(222, 36);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonRefresh.TabIndex = 2;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // monthRangePicker1
            // 
            this.monthRangePicker1.AutoSize = true;
            this.monthRangePicker1.BeginningMonth = 10;
            this.monthRangePicker1.Location = new System.Drawing.Point(222, 0);
            this.monthRangePicker1.Margin = new System.Windows.Forms.Padding(4);
            this.monthRangePicker1.MonthDayRange = monthDayRange1;
            this.monthRangePicker1.Name = "monthRangePicker1";
            this.monthRangePicker1.Size = new System.Drawing.Size(380, 35);
            this.monthRangePicker1.TabIndex = 1;
            // 
            // textBoxWaterYear
            // 
            this.textBoxWaterYear.Location = new System.Drawing.Point(63, 25);
            this.textBoxWaterYear.Name = "textBoxWaterYear";
            this.textBoxWaterYear.Size = new System.Drawing.Size(142, 20);
            this.textBoxWaterYear.TabIndex = 1;
            this.toolTip1.SetToolTip(this.textBoxWaterYear, "base year");
            // 
            // comboBoxSite
            // 
            this.comboBoxSite.FormattingEnabled = true;
            this.comboBoxSite.Items.AddRange(new object[] {
            "AMFI",
            "BEUO",
            "BUL",
            "BUM",
            "CLE",
            "CSCI",
            "DED",
            "GCL",
            "HEII",
            "HGH",
            "HRSI",
            "ISLI",
            "JCK",
            "KAC",
            "KEE",
            "LUC",
            "MCKO",
            "MFDO",
            "NACW",
            "OCHO",
            "OWY",
            "PARW",
            "PHL",
            "PRVO",
            "RIM",
            "RIR",
            "SCOO",
            "TDAO",
            "WARO",
            "WODI",
            "YUMW"});
            this.comboBoxSite.Location = new System.Drawing.Point(3, 3);
            this.comboBoxSite.Name = "comboBoxSite";
            this.comboBoxSite.Size = new System.Drawing.Size(213, 21);
            this.comboBoxSite.Sorted = true;
            this.comboBoxSite.TabIndex = 0;
            this.comboBoxSite.SelectedIndexChanged += new System.EventHandler(this.comboBoxSite_SelectedIndexChanged);
            // 
            // linkLabelReport
            // 
            this.linkLabelReport.AutoSize = true;
            this.linkLabelReport.Location = new System.Drawing.Point(16, 9);
            this.linkLabelReport.Name = "linkLabelReport";
            this.linkLabelReport.Size = new System.Drawing.Size(81, 13);
            this.linkLabelReport.TabIndex = 3;
            this.linkLabelReport.TabStop = true;
            this.linkLabelReport.Text = "open with excel";
            this.linkLabelReport.Visible = false;
            this.linkLabelReport.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelReport_LinkClicked);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageGraph);
            this.tabControl1.Controls.Add(this.tabPageReport);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 84);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(730, 456);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPageGraph
            // 
            this.tabPageGraph.Controls.Add(this.hydrometChart1);
            this.tabPageGraph.Location = new System.Drawing.Point(4, 22);
            this.tabPageGraph.Name = "tabPageGraph";
            this.tabPageGraph.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGraph.Size = new System.Drawing.Size(722, 430);
            this.tabPageGraph.TabIndex = 0;
            this.tabPageGraph.Text = "graph";
            this.tabPageGraph.UseVisualStyleBackColor = true;
            // 
            // hydrometChart1
            // 
            this.hydrometChart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hydrometChart1.Location = new System.Drawing.Point(3, 3);
            this.hydrometChart1.Margin = new System.Windows.Forms.Padding(4);
            this.hydrometChart1.Name = "hydrometChart1";
            this.hydrometChart1.Size = new System.Drawing.Size(716, 424);
            this.hydrometChart1.TabIndex = 1;
            // 
            // tabPageReport
            // 
            this.tabPageReport.Controls.Add(this.dataGridView1);
            this.tabPageReport.Controls.Add(this.panel2);
            this.tabPageReport.Location = new System.Drawing.Point(4, 22);
            this.tabPageReport.Name = "tabPageReport";
            this.tabPageReport.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageReport.Size = new System.Drawing.Size(722, 430);
            this.tabPageReport.TabIndex = 1;
            this.tabPageReport.Text = "report";
            this.tabPageReport.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 35);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(716, 392);
            this.dataGridView1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelFlagLegend);
            this.panel2.Controls.Add(this.linkLabelReport);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(716, 32);
            this.panel2.TabIndex = 1;
            // 
            // labelFlagLegend
            // 
            this.labelFlagLegend.AutoSize = true;
            this.labelFlagLegend.Location = new System.Drawing.Point(125, 13);
            this.labelFlagLegend.Name = "labelFlagLegend";
            this.labelFlagLegend.Size = new System.Drawing.Size(35, 13);
            this.labelFlagLegend.TabIndex = 4;
            this.labelFlagLegend.Text = "label2";
            // 
            // checkBoxDashed
            // 
            this.checkBoxDashed.AutoSize = true;
            this.checkBoxDashed.Checked = true;
            this.checkBoxDashed.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDashed.Location = new System.Drawing.Point(630, 58);
            this.checkBoxDashed.Name = "checkBoxDashed";
            this.checkBoxDashed.Size = new System.Drawing.Size(93, 17);
            this.checkBoxDashed.TabIndex = 14;
            this.checkBoxDashed.Text = "SRD - dashed";
            this.toolTip1.SetToolTip(this.checkBoxDashed, "when checked, use dashed (alternate) flood curve");
            this.checkBoxDashed.UseVisualStyleBackColor = true;
            // 
            // FcPlotUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Name = "FcPlotUI";
            this.Size = new System.Drawing.Size(730, 540);
            this.Load += new System.EventHandler(this.FcPlotUI_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageGraph.ResumeLayout(false);
            this.tabPageReport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private HydrometTeeChart hydrometChart1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ComboBox comboBoxSite;
        private Reclamation.TimeSeries.Forms.MonthRangePicker monthRangePicker1;
        private System.Windows.Forms.TextBox textBoxWaterYear;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.LinkLabel linkLabelReport;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxAlternateWaterYear;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageGraph;
        private System.Windows.Forms.TabPage tabPageReport;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label labelFlagLegend;
        private System.Windows.Forms.TextBox pcodeComparison;
        private System.Windows.Forms.TextBox pcodeInitial;
        private System.Windows.Forms.Label waterYearLabel;
        private System.Windows.Forms.CheckBox showGreenLines;
        private System.Windows.Forms.CheckBox showTarget;
        private System.Windows.Forms.Label compareData;
        private System.Windows.Forms.Label dataInitial;
        private System.Windows.Forms.TextBox textBoxTargetPercentages;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBoxDashed;
    }
}
