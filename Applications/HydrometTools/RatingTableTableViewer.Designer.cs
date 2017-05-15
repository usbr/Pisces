namespace HydrometTools
{
    partial class RatingTableTableViewer
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBoxMultiColumn = new System.Windows.Forms.CheckBox();
            this.buttonXls = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 44);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(481, 313);
            this.dataGridView1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.checkBoxMultiColumn);
            this.panel2.Controls.Add(this.buttonXls);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(481, 44);
            this.panel2.TabIndex = 3;
            // 
            // checkBoxMultiColumn
            // 
            this.checkBoxMultiColumn.AutoSize = true;
            this.checkBoxMultiColumn.Location = new System.Drawing.Point(23, 9);
            this.checkBoxMultiColumn.Name = "checkBoxMultiColumn";
            this.checkBoxMultiColumn.Size = new System.Drawing.Size(86, 17);
            this.checkBoxMultiColumn.TabIndex = 1;
            this.checkBoxMultiColumn.Text = "Multi Column";
            this.checkBoxMultiColumn.UseVisualStyleBackColor = true;
            this.checkBoxMultiColumn.CheckedChanged += new System.EventHandler(this.checkBoxMultiColumn_CheckedChanged);
            // 
            // buttonXls
            // 
            this.buttonXls.Location = new System.Drawing.Point(208, 3);
            this.buttonXls.Name = "buttonXls";
            this.buttonXls.Size = new System.Drawing.Size(103, 23);
            this.buttonXls.TabIndex = 0;
            this.buttonXls.Text = "Open with Excel";
            this.buttonXls.UseVisualStyleBackColor = true;
            this.buttonXls.Click += new System.EventHandler(this.buttonXls_Click);
            // 
            // RatingTableTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel2);
            this.Name = "RatingTableTable";
            this.Size = new System.Drawing.Size(481, 357);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox checkBoxMultiColumn;
        private System.Windows.Forms.Button buttonXls;
    }
}
