namespace Reclamation.TimeSeries.Forms.ImportForms
{
    partial class SelectAccessSeries
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.labelFileName = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxTableNames = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxValueColumn = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBoxDateColumn = new System.Windows.Forms.ComboBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.comboBoxSiteColumn = new System.Windows.Forms.ComboBox();
            this.groupBoxData = new System.Windows.Forms.GroupBox();
            this.buttonClearAll = new System.Windows.Forms.Button();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.checkedListBoxSites = new System.Windows.Forms.CheckedListBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBoxData.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(303, 411);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 0;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Enabled = false;
            this.buttonOK.Location = new System.Drawing.Point(212, 411);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // labelFileName
            // 
            this.labelFileName.AutoSize = true;
            this.labelFileName.Location = new System.Drawing.Point(40, 9);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(21, 13);
            this.labelFileName.TabIndex = 2;
            this.labelFileName.Text = "c:\\";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxTableNames);
            this.groupBox1.Location = new System.Drawing.Point(29, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 45);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Table/Query name";
            // 
            // comboBoxTableNames
            // 
            this.comboBoxTableNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTableNames.FormattingEnabled = true;
            this.comboBoxTableNames.Location = new System.Drawing.Point(14, 17);
            this.comboBoxTableNames.Name = "comboBoxTableNames";
            this.comboBoxTableNames.Size = new System.Drawing.Size(201, 21);
            this.comboBoxTableNames.TabIndex = 0;
            this.comboBoxTableNames.SelectedIndexChanged += new System.EventHandler(this.comboBoxSheetNames_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBoxValueColumn);
            this.groupBox2.Location = new System.Drawing.Point(28, 127);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(259, 48);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "value column";
            // 
            // comboBoxValueColumn
            // 
            this.comboBoxValueColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxValueColumn.FormattingEnabled = true;
            this.comboBoxValueColumn.Location = new System.Drawing.Point(14, 18);
            this.comboBoxValueColumn.Name = "comboBoxValueColumn";
            this.comboBoxValueColumn.Size = new System.Drawing.Size(201, 21);
            this.comboBoxValueColumn.TabIndex = 1;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBoxDateColumn);
            this.groupBox3.Location = new System.Drawing.Point(29, 74);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(259, 47);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Date column";
            // 
            // comboBoxDateColumn
            // 
            this.comboBoxDateColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDateColumn.FormattingEnabled = true;
            this.comboBoxDateColumn.Location = new System.Drawing.Point(14, 17);
            this.comboBoxDateColumn.Name = "comboBoxDateColumn";
            this.comboBoxDateColumn.Size = new System.Drawing.Size(201, 21);
            this.comboBoxDateColumn.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 7;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.comboBoxSiteColumn);
            this.groupBox4.Location = new System.Drawing.Point(28, 181);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(259, 50);
            this.groupBox4.TabIndex = 12;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "site column";
            // 
            // comboBoxSiteColumn
            // 
            this.comboBoxSiteColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSiteColumn.FormattingEnabled = true;
            this.comboBoxSiteColumn.Location = new System.Drawing.Point(14, 16);
            this.comboBoxSiteColumn.Name = "comboBoxSiteColumn";
            this.comboBoxSiteColumn.Size = new System.Drawing.Size(201, 21);
            this.comboBoxSiteColumn.TabIndex = 1;
            this.comboBoxSiteColumn.SelectedIndexChanged += new System.EventHandler(this.comboBoxFilter_SelectedIndexChanged);
            // 
            // groupBoxData
            // 
            this.groupBoxData.Controls.Add(this.buttonClearAll);
            this.groupBoxData.Controls.Add(this.buttonSelectAll);
            this.groupBoxData.Controls.Add(this.checkedListBoxSites);
            this.groupBoxData.Location = new System.Drawing.Point(16, 237);
            this.groupBoxData.Name = "groupBoxData";
            this.groupBoxData.Size = new System.Drawing.Size(362, 136);
            this.groupBoxData.TabIndex = 13;
            this.groupBoxData.TabStop = false;
            this.groupBoxData.Text = "sites to import";
            // 
            // buttonClearAll
            // 
            this.buttonClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearAll.Location = new System.Drawing.Point(283, 83);
            this.buttonClearAll.Name = "buttonClearAll";
            this.buttonClearAll.Size = new System.Drawing.Size(75, 23);
            this.buttonClearAll.TabIndex = 17;
            this.buttonClearAll.Text = "Select None";
            this.buttonClearAll.UseVisualStyleBackColor = true;
            this.buttonClearAll.Click += new System.EventHandler(this.buttonClearAll_Click);
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectAll.Location = new System.Drawing.Point(282, 44);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectAll.TabIndex = 16;
            this.buttonSelectAll.Text = "Select All";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // checkedListBoxSites
            // 
            this.checkedListBoxSites.CheckOnClick = true;
            this.checkedListBoxSites.FormattingEnabled = true;
            this.checkedListBoxSites.Location = new System.Drawing.Point(14, 16);
            this.checkedListBoxSites.Name = "checkedListBoxSites";
            this.checkedListBoxSites.Size = new System.Drawing.Size(233, 109);
            this.checkedListBoxSites.TabIndex = 2;
            // 
            // SelectAccessSeries
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(390, 453);
            this.Controls.Add(this.groupBoxData);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelFileName);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimumSize = new System.Drawing.Size(330, 405);
            this.Name = "SelectAccessSeries";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "New Series from database";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBoxData.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

#endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBoxTableNames;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBoxDateColumn;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox comboBoxValueColumn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ComboBox comboBoxSiteColumn;
        private System.Windows.Forms.GroupBox groupBoxData;
        private System.Windows.Forms.Button buttonClearAll;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.CheckedListBox checkedListBoxSites;
    }
}