namespace HydrometForecast
{
    partial class CompareTerms
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageCoeffNew = new System.Windows.Forms.TabPage();
            this.dataGridViewTerms = new System.Windows.Forms.DataGridView();
            this.tabPageHistory = new System.Windows.Forms.TabPage();
            this.dataGridViewHistory = new System.Windows.Forms.DataGridView();
            this.tabPageDiff = new System.Windows.Forms.TabPage();
            this.dataGridDiff = new System.Windows.Forms.DataGridView();
            this.tabControl1.SuspendLayout();
            this.tabPageCoeffNew.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTerms)).BeginInit();
            this.tabPageHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHistory)).BeginInit();
            this.tabPageDiff.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDiff)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageCoeffNew);
            this.tabControl1.Controls.Add(this.tabPageHistory);
            this.tabControl1.Controls.Add(this.tabPageDiff);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(542, 440);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageCoeffNew
            // 
            this.tabPageCoeffNew.Controls.Add(this.dataGridViewTerms);
            this.tabPageCoeffNew.Location = new System.Drawing.Point(4, 22);
            this.tabPageCoeffNew.Name = "tabPageCoeffNew";
            this.tabPageCoeffNew.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCoeffNew.Size = new System.Drawing.Size(534, 414);
            this.tabPageCoeffNew.TabIndex = 0;
            this.tabPageCoeffNew.Text = "terms by year";
            this.tabPageCoeffNew.UseVisualStyleBackColor = true;
            // 
            // dataGridViewCoef
            // 
            this.dataGridViewTerms.AllowUserToAddRows = false;
            this.dataGridViewTerms.AllowUserToDeleteRows = false;
            this.dataGridViewTerms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTerms.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTerms.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewTerms.Name = "dataGridViewCoef";
            this.dataGridViewTerms.ReadOnly = true;
            this.dataGridViewTerms.Size = new System.Drawing.Size(528, 408);
            this.dataGridViewTerms.TabIndex = 0;
            // 
            // tabPageHistory
            // 
            this.tabPageHistory.Controls.Add(this.dataGridViewHistory);
            this.tabPageHistory.Location = new System.Drawing.Point(4, 22);
            this.tabPageHistory.Name = "tabPageHistory";
            this.tabPageHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHistory.Size = new System.Drawing.Size(534, 414);
            this.tabPageHistory.TabIndex = 1;
            this.tabPageHistory.Text = "history.out";
            this.tabPageHistory.UseVisualStyleBackColor = true;
            // 
            // dataGridViewHistory
            // 
            this.dataGridViewHistory.AllowUserToAddRows = false;
            this.dataGridViewHistory.AllowUserToDeleteRows = false;
            this.dataGridViewHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewHistory.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewHistory.Name = "dataGridViewHistory";
            this.dataGridViewHistory.ReadOnly = true;
            this.dataGridViewHistory.Size = new System.Drawing.Size(528, 408);
            this.dataGridViewHistory.TabIndex = 1;
            // 
            // tabPageDiff
            // 
            this.tabPageDiff.Controls.Add(this.dataGridDiff);
            this.tabPageDiff.Location = new System.Drawing.Point(4, 22);
            this.tabPageDiff.Name = "tabPageDiff";
            this.tabPageDiff.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDiff.Size = new System.Drawing.Size(534, 414);
            this.tabPageDiff.TabIndex = 2;
            this.tabPageDiff.Text = "difference";
            this.tabPageDiff.UseVisualStyleBackColor = true;
            // 
            // dataGridDiff
            // 
            this.dataGridDiff.AllowUserToAddRows = false;
            this.dataGridDiff.AllowUserToDeleteRows = false;
            this.dataGridDiff.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridDiff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridDiff.Location = new System.Drawing.Point(3, 3);
            this.dataGridDiff.Name = "dataGridDiff";
            this.dataGridDiff.ReadOnly = true;
            this.dataGridDiff.Size = new System.Drawing.Size(528, 408);
            this.dataGridDiff.TabIndex = 1;
            // 
            // CompareCoeficients
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(542, 440);
            this.Controls.Add(this.tabControl1);
            this.Name = "CompareCoeficients";
            this.Text = "Compare";
            this.tabControl1.ResumeLayout(false);
            this.tabPageCoeffNew.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTerms)).EndInit();
            this.tabPageHistory.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewHistory)).EndInit();
            this.tabPageDiff.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridDiff)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageCoeffNew;
        private System.Windows.Forms.TabPage tabPageHistory;
        private System.Windows.Forms.TabPage tabPageDiff;
        private System.Windows.Forms.DataGridView dataGridViewTerms;
        private System.Windows.Forms.DataGridView dataGridViewHistory;
        private System.Windows.Forms.DataGridView dataGridDiff;
    }
}