namespace Depth
{
    partial class FormMain
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonPlot = new System.Windows.Forms.Button();
            this.panelChart = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.openMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ratingInputs1 = new Depth.RatingInputs();
            this.buttonTable = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.buttonTable);
            this.panel1.Controls.Add(this.buttonPlot);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(257, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(566, 100);
            this.panel1.TabIndex = 1;
            // 
            // buttonPlot
            // 
            this.buttonPlot.Location = new System.Drawing.Point(16, 25);
            this.buttonPlot.Name = "buttonPlot";
            this.buttonPlot.Size = new System.Drawing.Size(75, 23);
            this.buttonPlot.TabIndex = 0;
            this.buttonPlot.Text = "plot";
            this.buttonPlot.UseVisualStyleBackColor = true;
            this.buttonPlot.Click += new System.EventHandler(this.buttonPlot_Click);
            // 
            // panelChart
            // 
            this.panelChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelChart.Location = new System.Drawing.Point(257, 124);
            this.panelChart.Name = "panelChart";
            this.panelChart.Size = new System.Drawing.Size(566, 449);
            this.panelChart.TabIndex = 2;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(823, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMenu,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem1.Text = "&File";
            // 
            // openMenu
            // 
            this.openMenu.Name = "openMenu";
            this.openMenu.Size = new System.Drawing.Size(138, 22);
            this.openMenu.Text = "&Open";
            this.openMenu.Click += new System.EventHandler(this.openMenu_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // ratingInputs1
            // 
            this.ratingInputs1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ratingInputs1.Dock = System.Windows.Forms.DockStyle.Left;
            this.ratingInputs1.Lines = new string[] {
        "Title  S F Flathead River nr Columbia Falls MT",
        "Scaling  LogLog ",
        "#Scaling  Linear",
        "#Scaling Polynomial 3",
        "",
        "# stage    flow     label",
        "1.76   40.4   1995",
        "2.50  185",
        "4.44  1080  2010",
        "5.94  2630 2014",
        "8.10 5880 2004",
        "9.89 9390  1980",
        "10.64  11500  2017",
        "  "};
            this.ratingInputs1.Location = new System.Drawing.Point(0, 24);
            this.ratingInputs1.Name = "ratingInputs1";
            this.ratingInputs1.Size = new System.Drawing.Size(257, 549);
            this.ratingInputs1.TabIndex = 0;
            // 
            // buttonTable
            // 
            this.buttonTable.Location = new System.Drawing.Point(129, 25);
            this.buttonTable.Name = "buttonTable";
            this.buttonTable.Size = new System.Drawing.Size(75, 23);
            this.buttonTable.TabIndex = 1;
            this.buttonTable.Text = "table";
            this.toolTip1.SetToolTip(this.buttonTable, "generate a table based on equation");
            this.buttonTable.UseVisualStyleBackColor = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 573);
            this.Controls.Add(this.panelChart);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ratingInputs1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "D E P T H  -  Rating Table Utility";
            this.panel1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RatingInputs ratingInputs1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panelChart;
        private System.Windows.Forms.Button buttonPlot;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openMenu;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.Button buttonTable;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

