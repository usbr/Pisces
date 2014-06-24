namespace Reclamation.TimeSeries.Forms
{
    partial class PiscesTree
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
            this.treeView1 = new Aga.Controls.Tree.TreeViewAdv();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageTree = new System.Windows.Forms.TabPage();
            this.tabPageCommand = new System.Windows.Forms.TabPage();
            this.hydrometCommandLine1 = new HydrometPisces.HydrometCommandLine();
            this.textBoxTreeFilter = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPageTree.SuspendLayout();
            this.tabPageCommand.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.BackColor = System.Drawing.SystemColors.Window;
            this.treeView1.DefaultToolTipProvider = null;
            this.treeView1.DisplayDraggingNodes = true;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.DragDropMarkColor = System.Drawing.Color.Black;
            this.treeView1.LineColor = System.Drawing.SystemColors.ControlDark;
            this.treeView1.LoadOnDemand = true;
            this.treeView1.Location = new System.Drawing.Point(3, 23);
            this.treeView1.Model = null;
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedNode = null;
            this.treeView1.Size = new System.Drawing.Size(335, 417);
            this.treeView1.TabIndex = 0;
            this.treeView1.Text = "treeView1";
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeViewAdv1_ItemDrag);
            this.treeView1.Expanding += new System.EventHandler<Aga.Controls.Tree.TreeViewAdvEventArgs>(this.treeView1_Expanding);
            this.treeView1.Expanded += new System.EventHandler<Aga.Controls.Tree.TreeViewAdvEventArgs>(this.treeView1_Expanded);
            this.treeView1.Click += new System.EventHandler(this.treeView1_Click);
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop);
            this.treeView1.DragOver += new System.Windows.Forms.DragEventHandler(this.treeViewAdv1_DragOver);
            this.treeView1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.treeView1_KeyPress);
            this.treeView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.treeView1_KeyUp);
            this.treeView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseClick);
            this.treeView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseDown);
            this.treeView1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseUp);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageTree);
            this.tabControl1.Controls.Add(this.tabPageCommand);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(349, 469);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPageTree
            // 
            this.tabPageTree.Controls.Add(this.treeView1);
            this.tabPageTree.Controls.Add(this.textBoxTreeFilter);
            this.tabPageTree.Location = new System.Drawing.Point(4, 22);
            this.tabPageTree.Name = "tabPageTree";
            this.tabPageTree.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTree.Size = new System.Drawing.Size(341, 443);
            this.tabPageTree.TabIndex = 0;
            this.tabPageTree.Text = "Tree";
            this.tabPageTree.UseVisualStyleBackColor = true;
            // 
            // tabPageCommand
            // 
            this.tabPageCommand.Controls.Add(this.hydrometCommandLine1);
            this.tabPageCommand.Location = new System.Drawing.Point(4, 22);
            this.tabPageCommand.Name = "tabPageCommand";
            this.tabPageCommand.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCommand.Size = new System.Drawing.Size(341, 443);
            this.tabPageCommand.TabIndex = 1;
            this.tabPageCommand.Text = "Commands";
            this.tabPageCommand.UseVisualStyleBackColor = true;
            // 
            // hydrometCommandLine1
            // 
            this.hydrometCommandLine1.AutoScroll = true;
            this.hydrometCommandLine1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hydrometCommandLine1.Location = new System.Drawing.Point(3, 3);
            this.hydrometCommandLine1.MinimumSize = new System.Drawing.Size(260, 0);
            this.hydrometCommandLine1.Name = "hydrometCommandLine1";
            this.hydrometCommandLine1.Size = new System.Drawing.Size(335, 437);
            this.hydrometCommandLine1.TabIndex = 0;
            // 
            // textBoxTreeFilter
            // 
            this.textBoxTreeFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxTreeFilter.Location = new System.Drawing.Point(3, 3);
            this.textBoxTreeFilter.Name = "textBoxTreeFilter";
            this.textBoxTreeFilter.Size = new System.Drawing.Size(335, 20);
            this.textBoxTreeFilter.TabIndex = 1;
            this.textBoxTreeFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxTreeFilter_KeyPress);
            // 
            // PiscesTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "PiscesTree";
            this.Size = new System.Drawing.Size(349, 469);
            this.tabControl1.ResumeLayout(false);
            this.tabPageTree.ResumeLayout(false);
            this.tabPageTree.PerformLayout();
            this.tabPageCommand.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Aga.Controls.Tree.TreeViewAdv treeView1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageTree;
        private System.Windows.Forms.TabPage tabPageCommand;
        private HydrometPisces.HydrometCommandLine hydrometCommandLine1;
        private System.Windows.Forms.TextBox textBoxTreeFilter;
    }
}
