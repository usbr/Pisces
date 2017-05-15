namespace Look
{
    partial class NewLook
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
            this.SearchButton = new System.Windows.Forms.Button();
            this.LabelSearch = new System.Windows.Forms.Label();
            this.SearchTextBox = new System.Windows.Forms.TextBox();
            this.SearchStringButton = new System.Windows.Forms.Button();
            this.DataResultsTable = new System.Windows.Forms.DataGridView();
            this.CheckIntervals = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.DataResultsTable)).BeginInit();
            this.SuspendLayout();
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(465, 20);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 23);
            this.SearchButton.TabIndex = 9;
            this.SearchButton.Text = "Refresh";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // LabelSearch
            // 
            this.LabelSearch.AutoSize = true;
            this.LabelSearch.Location = new System.Drawing.Point(24, 25);
            this.LabelSearch.Name = "LabelSearch";
            this.LabelSearch.Size = new System.Drawing.Size(120, 13);
            this.LabelSearch.TabIndex = 8;
            this.LabelSearch.Text = "Type Search Keywords:";
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.Location = new System.Drawing.Point(150, 22);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.Size = new System.Drawing.Size(291, 20);
            this.SearchTextBox.TabIndex = 7;
            this.SearchTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SearchTextBox_KeyDown);
            // 
            // SearchStringButton
            // 
            this.SearchStringButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchStringButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.SearchStringButton.Location = new System.Drawing.Point(611, 567);
            this.SearchStringButton.Name = "SearchStringButton";
            this.SearchStringButton.Size = new System.Drawing.Size(141, 23);
            this.SearchStringButton.TabIndex = 17;
            this.SearchStringButton.Text = "Generate Search String";
            this.SearchStringButton.UseVisualStyleBackColor = true;
            this.SearchStringButton.Click += new System.EventHandler(this.SearchStringButton_Click);
            // 
            // DataResultsTable
            // 
            this.DataResultsTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataResultsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DataResultsTable.Location = new System.Drawing.Point(16, 59);
            this.DataResultsTable.Name = "DataResultsTable";
            this.DataResultsTable.Size = new System.Drawing.Size(749, 485);
            this.DataResultsTable.TabIndex = 18;
            // 
            // CheckIntervals
            // 
            this.CheckIntervals.AutoSize = true;
            this.CheckIntervals.Location = new System.Drawing.Point(622, 24);
            this.CheckIntervals.Name = "CheckIntervals";
            this.CheckIntervals.Size = new System.Drawing.Size(106, 17);
            this.CheckIntervals.TabIndex = 19;
            this.CheckIntervals.Text = "All Time Intervals";
            this.CheckIntervals.UseVisualStyleBackColor = true;
            // 
            // NewLook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CheckIntervals);
            this.Controls.Add(this.DataResultsTable);
            this.Controls.Add(this.SearchStringButton);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.LabelSearch);
            this.Controls.Add(this.SearchTextBox);
            this.Name = "NewLook";
            this.Size = new System.Drawing.Size(791, 605);
            ((System.ComponentModel.ISupportInitialize)(this.DataResultsTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.Label LabelSearch;
        private System.Windows.Forms.TextBox SearchTextBox;
        private System.Windows.Forms.Button SearchStringButton;
        private System.Windows.Forms.DataGridView DataResultsTable;
        private System.Windows.Forms.CheckBox CheckIntervals;
    }
}
