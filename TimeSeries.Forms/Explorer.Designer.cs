using Reclamation.TimeSeries.Forms;

namespace Reclamation.TimeSeries.Forms
{
    partial class Explorer 
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDisplayType = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemScenarios = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControlSiteSelection = new System.Windows.Forms.TabControl();
            this.tabPageTree = new System.Windows.Forms.TabPage();
            this.tree1 = new cbp.Tree();
            this.tabPageExpert = new System.Windows.Forms.TabPage();
            this.expertSiteSelection1 = new Reclamation.TimeSeries.Forms.ExpertSiteSelection();
            this.menuStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlSiteSelection.SuspendLayout();
            this.tabPageTree.SuspendLayout();
            this.tabPageExpert.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 455);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(653, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolStripDisplayType,
            this.toolStripMenuItemScenarios});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(653, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.saveToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(106, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(109, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStripDisplayType
            // 
            this.toolStripDisplayType.Name = "toolStripDisplayType";
            this.toolStripDisplayType.Size = new System.Drawing.Size(109, 20);
            this.toolStripDisplayType.Text = "Option:Time Series";
            this.toolStripDisplayType.Click += new System.EventHandler(this.toolStripDisplayType_Click);
            // 
            // toolStripMenuItemScenarios
            // 
            this.toolStripMenuItemScenarios.Name = "toolStripMenuItemScenarios";
            this.toolStripMenuItemScenarios.Size = new System.Drawing.Size(65, 20);
            this.toolStripMenuItemScenarios.Text = "&Scenarios";
            this.toolStripMenuItemScenarios.Click += new System.EventHandler(this.toolStripMenuItemScenarios_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControlSiteSelection);
            this.splitContainer1.Size = new System.Drawing.Size(653, 431);
            this.splitContainer1.SplitterDistance = 241;
            this.splitContainer1.SplitterWidth = 16;
            this.splitContainer1.TabIndex = 2;
            // 
            // tabControlSiteSelection
            // 
            this.tabControlSiteSelection.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControlSiteSelection.Controls.Add(this.tabPageTree);
            this.tabControlSiteSelection.Controls.Add(this.tabPageExpert);
            this.tabControlSiteSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlSiteSelection.Location = new System.Drawing.Point(0, 0);
            this.tabControlSiteSelection.Margin = new System.Windows.Forms.Padding(0);
            this.tabControlSiteSelection.Name = "tabControlSiteSelection";
            this.tabControlSiteSelection.Padding = new System.Drawing.Point(1, 1);
            this.tabControlSiteSelection.SelectedIndex = 0;
            this.tabControlSiteSelection.Size = new System.Drawing.Size(241, 431);
            this.tabControlSiteSelection.TabIndex = 0;
            // 
            // tabPageTree
            // 
            this.tabPageTree.Controls.Add(this.tree1);
            this.tabPageTree.Location = new System.Drawing.Point(4, 23);
            this.tabPageTree.Margin = new System.Windows.Forms.Padding(1);
            this.tabPageTree.Name = "tabPageTree";
            this.tabPageTree.Padding = new System.Windows.Forms.Padding(1);
            this.tabPageTree.Size = new System.Drawing.Size(233, 404);
            this.tabPageTree.TabIndex = 0;
            this.tabPageTree.Text = "Tree";
            this.tabPageTree.UseVisualStyleBackColor = true;
            // 
            // tree1
            // 
            this.tree1.AllowDrop = true;
            this.tree1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tree1.Location = new System.Drawing.Point(1, 1);
            this.tree1.Name = "tree1";
            this.tree1.Size = new System.Drawing.Size(231, 402);
            this.tree1.TabIndex = 0;
            this.tree1.Selected += new cbp.NodeSelectedEventHandler(this.tree1_Selected);
            this.tree1.DragEnter += new System.Windows.Forms.DragEventHandler(this.tree1_DragEnter);
            // 
            // tabPageExpert
            // 
            this.tabPageExpert.Controls.Add(this.expertSiteSelection1);
            this.tabPageExpert.Location = new System.Drawing.Point(4, 23);
            this.tabPageExpert.Margin = new System.Windows.Forms.Padding(1);
            this.tabPageExpert.Name = "tabPageExpert";
            this.tabPageExpert.Padding = new System.Windows.Forms.Padding(1);
            this.tabPageExpert.Size = new System.Drawing.Size(233, 404);
            this.tabPageExpert.TabIndex = 1;
            this.tabPageExpert.Text = "Expert   ";
            this.tabPageExpert.UseVisualStyleBackColor = true;
            // 
            // expertSiteSelection1
            // 
            this.expertSiteSelection1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.expertSiteSelection1.AutoScroll = true;
            this.expertSiteSelection1.Location = new System.Drawing.Point(6, 3);
            this.expertSiteSelection1.Margin = new System.Windows.Forms.Padding(1);
            this.expertSiteSelection1.Name = "expertSiteSelection1";
            this.expertSiteSelection1.Size = new System.Drawing.Size(225, 399);
            this.expertSiteSelection1.TabIndex = 0;
            this.expertSiteSelection1.Tree = null;
            this.expertSiteSelection1.OnSubmit += new System.EventHandler(this.expertSiteSelection1_OnSubmit);
            // 
            // Explorer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(653, 477);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Explorer";
            this.Text = "Title";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tabControlSiteSelection.ResumeLayout(false);
            this.tabPageTree.ResumeLayout(false);
            this.tabPageExpert.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private cbp.Tree tree1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemScenarios;
        private System.Windows.Forms.ToolStripMenuItem toolStripDisplayType;
        private System.Windows.Forms.TabControl tabControlSiteSelection;
        private System.Windows.Forms.TabPage tabPageTree;
        private System.Windows.Forms.TabPage tabPageExpert;
        private ExpertSiteSelection expertSiteSelection1;

    }
}

