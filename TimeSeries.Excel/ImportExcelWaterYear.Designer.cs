namespace Reclamation.TimeSeries.Forms.ImportForms
{
    partial class ImportExcelWaterYear
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
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonClearAll = new System.Windows.Forms.Button();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.checkedListBoxSheets = new System.Windows.Forms.CheckedListBox();
            this.groupBoxDate = new System.Windows.Forms.GroupBox();
            this.comboBoxWaterYear = new System.Windows.Forms.ComboBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.labelError = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButtonHeaderRow = new System.Windows.Forms.RadioButton();
            this.radioButtonNoHeader = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxValueColumn = new System.Windows.Forms.ComboBox();
            this.comboBoxUnits = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBoxDate.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(329, 412);
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
            this.buttonOK.Location = new System.Drawing.Point(329, 383);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "c:\\";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonClearAll);
            this.groupBox1.Controls.Add(this.buttonSelectAll);
            this.groupBox1.Controls.Add(this.checkedListBoxSheets);
            this.groupBox1.Location = new System.Drawing.Point(29, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(358, 140);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "sheet name";
            // 
            // buttonClearAll
            // 
            this.buttonClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearAll.Location = new System.Drawing.Point(258, 68);
            this.buttonClearAll.Name = "buttonClearAll";
            this.buttonClearAll.Size = new System.Drawing.Size(75, 23);
            this.buttonClearAll.TabIndex = 19;
            this.buttonClearAll.Text = "Select None";
            this.buttonClearAll.UseVisualStyleBackColor = true;
            this.buttonClearAll.Click += new System.EventHandler(this.buttonClearAll_Click);
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectAll.Location = new System.Drawing.Point(258, 28);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectAll.TabIndex = 18;
            this.buttonSelectAll.Text = "Select All";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // checkedListBoxSheets
            // 
            this.checkedListBoxSheets.CheckOnClick = true;
            this.checkedListBoxSheets.FormattingEnabled = true;
            this.checkedListBoxSheets.Location = new System.Drawing.Point(16, 14);
            this.checkedListBoxSheets.Name = "checkedListBoxSheets";
            this.checkedListBoxSheets.Size = new System.Drawing.Size(236, 109);
            this.checkedListBoxSheets.TabIndex = 3;
            this.checkedListBoxSheets.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxSheets_SelectedIndexChanged);
            // 
            // groupBoxDate
            // 
            this.groupBoxDate.Controls.Add(this.comboBoxWaterYear);
            this.groupBoxDate.Location = new System.Drawing.Point(25, 237);
            this.groupBoxDate.Name = "groupBoxDate";
            this.groupBoxDate.Size = new System.Drawing.Size(362, 46);
            this.groupBoxDate.TabIndex = 4;
            this.groupBoxDate.TabStop = false;
            this.groupBoxDate.Text = "water year  column";
            // 
            // comboBoxWaterYear
            // 
            this.comboBoxWaterYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWaterYear.FormattingEnabled = true;
            this.comboBoxWaterYear.Location = new System.Drawing.Point(14, 19);
            this.comboBoxWaterYear.Name = "comboBoxWaterYear";
            this.comboBoxWaterYear.Size = new System.Drawing.Size(201, 21);
            this.comboBoxWaterYear.TabIndex = 0;
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.ForeColor = System.Drawing.Color.Red;
            this.labelError.Location = new System.Drawing.Point(29, 220);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(28, 13);
            this.labelError.TabIndex = 6;
            this.labelError.Text = "error";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 178);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 7;
            // 
            // radioButtonHeaderRow
            // 
            this.radioButtonHeaderRow.AutoSize = true;
            this.radioButtonHeaderRow.Checked = true;
            this.radioButtonHeaderRow.Location = new System.Drawing.Point(6, 23);
            this.radioButtonHeaderRow.Name = "radioButtonHeaderRow";
            this.radioButtonHeaderRow.Size = new System.Drawing.Size(80, 17);
            this.radioButtonHeaderRow.TabIndex = 8;
            this.radioButtonHeaderRow.TabStop = true;
            this.radioButtonHeaderRow.Text = "Header &row";
            this.radioButtonHeaderRow.UseVisualStyleBackColor = true;
            this.radioButtonHeaderRow.CheckedChanged += new System.EventHandler(this.radioButtonHeaderRow_CheckedChanged);
            // 
            // radioButtonNoHeader
            // 
            this.radioButtonNoHeader.AutoSize = true;
            this.radioButtonNoHeader.Location = new System.Drawing.Point(110, 23);
            this.radioButtonNoHeader.Name = "radioButtonNoHeader";
            this.radioButtonNoHeader.Size = new System.Drawing.Size(97, 17);
            this.radioButtonNoHeader.TabIndex = 9;
            this.radioButtonNoHeader.TabStop = true;
            this.radioButtonNoHeader.Text = "No Header ro&w";
            this.radioButtonNoHeader.UseVisualStyleBackColor = true;
            this.radioButtonNoHeader.CheckedChanged += new System.EventHandler(this.radioButtonNoHeader_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.radioButtonHeaderRow);
            this.groupBox4.Controls.Add(this.radioButtonNoHeader);
            this.groupBox4.Location = new System.Drawing.Point(29, 167);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(358, 47);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "My spreadsheet has";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.comboBoxValueColumn);
            this.groupBox2.Location = new System.Drawing.Point(25, 289);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(362, 56);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "column for first month (October)";
            // 
            // comboBoxValueColumn
            // 
            this.comboBoxValueColumn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxValueColumn.FormattingEnabled = true;
            this.comboBoxValueColumn.Location = new System.Drawing.Point(14, 19);
            this.comboBoxValueColumn.Name = "comboBoxValueColumn";
            this.comboBoxValueColumn.Size = new System.Drawing.Size(201, 21);
            this.comboBoxValueColumn.TabIndex = 0;
            // 
            // comboBoxUnits
            // 
            this.comboBoxUnits.FormattingEnabled = true;
            this.comboBoxUnits.Location = new System.Drawing.Point(14, 19);
            this.comboBoxUnits.Name = "comboBoxUnits";
            this.comboBoxUnits.Size = new System.Drawing.Size(175, 21);
            this.comboBoxUnits.TabIndex = 11;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBoxUnits);
            this.groupBox3.Location = new System.Drawing.Point(25, 351);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 59);
            this.groupBox3.TabIndex = 15;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "units";
            // 
            // ImportExcelWaterYear
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 447);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelError);
            this.Controls.Add(this.groupBoxDate);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimumSize = new System.Drawing.Size(330, 370);
            this.Name = "ImportExcelWaterYear";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Import WaterYear formatted spreadsheet";
            this.groupBox1.ResumeLayout(false);
            this.groupBoxDate.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

#endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBoxDate;
        private System.Windows.Forms.ComboBox comboBoxWaterYear;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label labelError;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButtonHeaderRow;
        private System.Windows.Forms.RadioButton radioButtonNoHeader;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox comboBoxValueColumn;
        private System.Windows.Forms.ComboBox comboBoxUnits;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckedListBox checkedListBoxSheets;
        private System.Windows.Forms.Button buttonClearAll;
        private System.Windows.Forms.Button buttonSelectAll;
    }
}