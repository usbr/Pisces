namespace HydrometTools
{
    partial class RatingTableDisplay
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageGraph = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkLabelGraphEditor = new System.Windows.Forms.LinkLabel();
            this.tabPageHydrometTable = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.labelyparm = new System.Windows.Forms.Label();
            this.labelcbtt = new System.Windows.Forms.Label();
            this.labelSiteName = new System.Windows.Forms.Label();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelDate = new System.Windows.Forms.Label();
            this.ratingTableGraph1 = new Reclamation.TimeSeries.Graphing.RatingTableGraph();
            this.ratingTableTableHydromet = new HydrometTools.RatingTableTableViewer();
            this.ratingTableTableUsgs = new HydrometTools.RatingTableTableViewer();
            this.tabControl1.SuspendLayout();
            this.tabPageGraph.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPageHydrometTable.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageGraph);
            this.tabControl1.Controls.Add(this.tabPageHydrometTable);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 98);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(610, 295);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageGraph
            // 
            this.tabPageGraph.Controls.Add(this.ratingTableGraph1);
            this.tabPageGraph.Controls.Add(this.panel1);
            this.tabPageGraph.Location = new System.Drawing.Point(4, 22);
            this.tabPageGraph.Name = "tabPageGraph";
            this.tabPageGraph.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGraph.Size = new System.Drawing.Size(602, 269);
            this.tabPageGraph.TabIndex = 0;
            this.tabPageGraph.Text = "graph";
            this.tabPageGraph.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.linkLabelGraphEditor);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(596, 20);
            this.panel1.TabIndex = 2;
            // 
            // linkLabelGraphEditor
            // 
            this.linkLabelGraphEditor.AutoSize = true;
            this.linkLabelGraphEditor.Location = new System.Drawing.Point(13, 1);
            this.linkLabelGraphEditor.Name = "linkLabelGraphEditor";
            this.linkLabelGraphEditor.Size = new System.Drawing.Size(54, 13);
            this.linkLabelGraphEditor.TabIndex = 0;
            this.linkLabelGraphEditor.TabStop = true;
            this.linkLabelGraphEditor.Text = "edit graph";
            this.linkLabelGraphEditor.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelGraphEditor_LinkClicked);
            // 
            // tabPageHydrometTable
            // 
            this.tabPageHydrometTable.Controls.Add(this.ratingTableTableHydromet);
            this.tabPageHydrometTable.Location = new System.Drawing.Point(4, 22);
            this.tabPageHydrometTable.Name = "tabPageHydrometTable";
            this.tabPageHydrometTable.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHydrometTable.Size = new System.Drawing.Size(602, 288);
            this.tabPageHydrometTable.TabIndex = 1;
            this.tabPageHydrometTable.Text = "Hydromet Table";
            this.tabPageHydrometTable.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ratingTableTableUsgs);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(602, 288);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Usgs Table";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // labelyparm
            // 
            this.labelyparm.AutoSize = true;
            this.labelyparm.Location = new System.Drawing.Point(155, 32);
            this.labelyparm.Name = "labelyparm";
            this.labelyparm.Size = new System.Drawing.Size(35, 13);
            this.labelyparm.TabIndex = 3;
            this.labelyparm.Text = "yparm";
            // 
            // labelcbtt
            // 
            this.labelcbtt.AutoSize = true;
            this.labelcbtt.Location = new System.Drawing.Point(155, 53);
            this.labelcbtt.Name = "labelcbtt";
            this.labelcbtt.Size = new System.Drawing.Size(25, 13);
            this.labelcbtt.TabIndex = 2;
            this.labelcbtt.Text = "cbtt";
            // 
            // labelSiteName
            // 
            this.labelSiteName.AutoSize = true;
            this.labelSiteName.Location = new System.Drawing.Point(155, 11);
            this.labelSiteName.Name = "labelSiteName";
            this.labelSiteName.Size = new System.Drawing.Size(61, 13);
            this.labelSiteName.TabIndex = 1;
            this.labelSiteName.Text = "description:";
            // 
            // buttonSelect
            // 
            this.buttonSelect.Location = new System.Drawing.Point(7, 11);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(75, 23);
            this.buttonSelect.TabIndex = 0;
            this.buttonSelect.Text = "Select ...";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.buttonSelect_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labelDate);
            this.panel3.Controls.Add(this.buttonSelect);
            this.panel3.Controls.Add(this.labelcbtt);
            this.panel3.Controls.Add(this.labelyparm);
            this.panel3.Controls.Add(this.labelSiteName);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(610, 98);
            this.panel3.TabIndex = 1;
            // 
            // labelDate
            // 
            this.labelDate.AutoSize = true;
            this.labelDate.Location = new System.Drawing.Point(155, 74);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(49, 13);
            this.labelDate.TabIndex = 4;
            this.labelDate.Text = "modified:";
            // 
            // ratingTableGraph1
            // 
            this.ratingTableGraph1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ratingTableGraph1.Location = new System.Drawing.Point(3, 23);
            this.ratingTableGraph1.Name = "ratingTableGraph1";
            this.ratingTableGraph1.Size = new System.Drawing.Size(596, 243);
            this.ratingTableGraph1.TabIndex = 1;
            // 
            // ratingTableTableHydromet
            // 
            this.ratingTableTableHydromet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ratingTableTableHydromet.Location = new System.Drawing.Point(3, 3);
            this.ratingTableTableHydromet.Name = "ratingTableTableHydromet";
            this.ratingTableTableHydromet.RatingTable = null;
            this.ratingTableTableHydromet.Size = new System.Drawing.Size(596, 282);
            this.ratingTableTableHydromet.TabIndex = 0;
            // 
            // ratingTableTableUsgs
            // 
            this.ratingTableTableUsgs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ratingTableTableUsgs.Location = new System.Drawing.Point(3, 3);
            this.ratingTableTableUsgs.Name = "ratingTableTableUsgs";
            this.ratingTableTableUsgs.RatingTable = null;
            this.ratingTableTableUsgs.Size = new System.Drawing.Size(596, 282);
            this.ratingTableTableUsgs.TabIndex = 0;
            // 
            // RatingTableDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel3);
            this.Name = "RatingTableDisplay";
            this.Size = new System.Drawing.Size(610, 393);
            this.tabControl1.ResumeLayout(false);
            this.tabPageGraph.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabPageHydrometTable.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageGraph;
        private System.Windows.Forms.TabPage tabPageHydrometTable;
        private System.Windows.Forms.Button buttonSelect;
        private System.Windows.Forms.Label labelcbtt;
        private System.Windows.Forms.Label labelSiteName;
        private System.Windows.Forms.Label labelyparm;
        private Reclamation.TimeSeries.Graphing.RatingTableGraph ratingTableGraph1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TabPage tabPage1;
        private RatingTableTableViewer ratingTableTableHydromet;
        private RatingTableTableViewer ratingTableTableUsgs;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel linkLabelGraphEditor;
        private System.Windows.Forms.Label labelDate;
    }
}
