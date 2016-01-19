namespace HydrometNotifications.UserInterface
{
    partial class NotificationMain
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonNewGroup = new System.Windows.Forms.Button();
            this.textBoxNewGroup = new System.Windows.Forms.TextBox();
            this.groupnameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewLinkColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.groupnameDataGridViewTextBoxColumn});
            this.dataGridView1.Location = new System.Drawing.Point(48, 20);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(299, 610);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // buttonNewGroup
            // 
            this.buttonNewGroup.Location = new System.Drawing.Point(404, 68);
            this.buttonNewGroup.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonNewGroup.Name = "buttonNewGroup";
            this.buttonNewGroup.Size = new System.Drawing.Size(100, 28);
            this.buttonNewGroup.TabIndex = 1;
            this.buttonNewGroup.Text = "new";
            this.buttonNewGroup.UseVisualStyleBackColor = true;
            this.buttonNewGroup.Click += new System.EventHandler(this.buttonNewGroup_Click);
            // 
            // textBoxNewGroup
            // 
            this.textBoxNewGroup.Location = new System.Drawing.Point(512, 68);
            this.textBoxNewGroup.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxNewGroup.Name = "textBoxNewGroup";
            this.textBoxNewGroup.Size = new System.Drawing.Size(132, 22);
            this.textBoxNewGroup.TabIndex = 2;
            // 
            // groupnameDataGridViewTextBoxColumn
            // 
            this.groupnameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.groupnameDataGridViewTextBoxColumn.DataPropertyName = "group_name";
            this.groupnameDataGridViewTextBoxColumn.HeaderText = "group_name";
            this.groupnameDataGridViewTextBoxColumn.Name = "groupnameDataGridViewTextBoxColumn";
            this.groupnameDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.groupnameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.groupnameDataGridViewTextBoxColumn.Width = 113;
            // 
            // NotificationMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxNewGroup);
            this.Controls.Add(this.buttonNewGroup);
            this.Controls.Add(this.dataGridView1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "NotificationMain";
            this.Size = new System.Drawing.Size(783, 650);
            this.Load += new System.EventHandler(this.NotificationMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonNewGroup;
        private System.Windows.Forms.TextBox textBoxNewGroup;
        private System.Windows.Forms.DataGridViewLinkColumn groupnameDataGridViewTextBoxColumn;

    }
}
