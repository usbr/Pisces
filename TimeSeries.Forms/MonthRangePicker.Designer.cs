namespace  Reclamation.TimeSeries.Forms
{

    partial class MonthRangePicker
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
            this.oct = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nov = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.feb = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.apr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.may = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jun = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jul = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aug = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sep = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ColumnHeadersVisible = false;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.oct,
            this.nov,
            this.dec,
            this.jan,
            this.feb,
            this.mar,
            this.apr,
            this.may,
            this.jun,
            this.jul,
            this.aug,
            this.sep});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.RowTemplate.ReadOnly = true;
            this.dataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.Size = new System.Drawing.Size(301, 58);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDown);
            this.dataGridView1.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.dataGridView1_PreviewKeyDown);
            // 
            // oct
            // 
            this.oct.HeaderText = "oct";
            this.oct.Name = "oct";
            this.oct.Width = 5;
            // 
            // nov
            // 
            this.nov.HeaderText = "nov";
            this.nov.Name = "nov";
            this.nov.Width = 5;
            // 
            // dec
            // 
            this.dec.HeaderText = "dec";
            this.dec.Name = "dec";
            this.dec.Width = 5;
            // 
            // jan
            // 
            this.jan.HeaderText = "jan";
            this.jan.Name = "jan";
            this.jan.Width = 5;
            // 
            // feb
            // 
            this.feb.HeaderText = "feb";
            this.feb.Name = "feb";
            this.feb.Width = 5;
            // 
            // mar
            // 
            this.mar.HeaderText = "mar";
            this.mar.Name = "mar";
            this.mar.Width = 5;
            // 
            // apr
            // 
            this.apr.HeaderText = "apr";
            this.apr.Name = "apr";
            this.apr.Width = 5;
            // 
            // may
            // 
            this.may.HeaderText = "may";
            this.may.Name = "may";
            this.may.Width = 5;
            // 
            // jun
            // 
            this.jun.HeaderText = "jun";
            this.jun.Name = "jun";
            this.jun.Width = 5;
            // 
            // jul
            // 
            this.jul.HeaderText = "jul";
            this.jul.Name = "jul";
            this.jul.Width = 5;
            // 
            // aug
            // 
            this.aug.HeaderText = "aug";
            this.aug.Name = "aug";
            this.aug.Width = 5;
            // 
            // sep
            // 
            this.sep.HeaderText = "sep";
            this.sep.Name = "sep";
            this.sep.Width = 5;
            // 
            // MonthRangePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Name = "MonthRangePicker";
            this.Size = new System.Drawing.Size(301, 58);
            this.VisibleChanged += new System.EventHandler(this.MonthRangePicker_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn oct;
        private System.Windows.Forms.DataGridViewTextBoxColumn nov;
        private System.Windows.Forms.DataGridViewTextBoxColumn dec;
        private System.Windows.Forms.DataGridViewTextBoxColumn jan;
        private System.Windows.Forms.DataGridViewTextBoxColumn feb;
        private System.Windows.Forms.DataGridViewTextBoxColumn mar;
        private System.Windows.Forms.DataGridViewTextBoxColumn apr;
        private System.Windows.Forms.DataGridViewTextBoxColumn may;
        private System.Windows.Forms.DataGridViewTextBoxColumn jun;
        private System.Windows.Forms.DataGridViewTextBoxColumn jul;
        private System.Windows.Forms.DataGridViewTextBoxColumn aug;
        private System.Windows.Forms.DataGridViewTextBoxColumn sep;
    }
}
