namespace Reclamation.TimeSeries.Forms
{
    partial class ScenarioSelector
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonSelectMid20P = new System.Windows.Forms.Button();
            this.buttonSelectLow20P = new System.Windows.Forms.Button();
            this.buttonSelectTop20P = new System.Windows.Forms.Button();
            this.buttonClearAll = new System.Windows.Forms.Button();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxIncludeSelected = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeBaseline = new System.Windows.Forms.CheckBox();
            this.checkBoxSubtractFromBaseline = new System.Windows.Forms.CheckBox();
            this.checkBoxMerge = new System.Windows.Forms.CheckBox();
            this.groupBoxComparisonn = new System.Windows.Forms.GroupBox();
            this.groupBoxAll = new System.Windows.Forms.GroupBox();
            this.groupBoxSorting = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.comboBoxOutYear = new System.Windows.Forms.ComboBox();
            this.radioButtonYear2 = new System.Windows.Forms.RadioButton();
            this.radioButtonYear1 = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radioButtonEOP = new System.Windows.Forms.RadioButton();
            this.radioButtonSum = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBoxMonths = new System.Windows.Forms.ComboBox();
            this.radioButtonMonth = new System.Windows.Forms.RadioButton();
            this.radioButtonWY = new System.Windows.Forms.RadioButton();
            this.radioButtonCY = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxSelectedSeries = new System.Windows.Forms.ComboBox();
            this.buttonSort = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBoxComparisonn.SuspendLayout();
            this.groupBoxAll.SuspendLayout();
            this.groupBoxSorting.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(234, 606);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 9;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Location = new System.Drawing.Point(6, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(482, 429);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Traces / Scenarios";
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(7, 19);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(468, 403);
            this.dataGridView1.TabIndex = 17;
            // 
            // buttonSelectMid20P
            // 
            this.buttonSelectMid20P.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSelectMid20P.Location = new System.Drawing.Point(112, 117);
            this.buttonSelectMid20P.Name = "buttonSelectMid20P";
            this.buttonSelectMid20P.Size = new System.Drawing.Size(97, 23);
            this.buttonSelectMid20P.TabIndex = 20;
            this.buttonSelectMid20P.Text = "Select Mid 20%";
            this.buttonSelectMid20P.UseVisualStyleBackColor = true;
            this.buttonSelectMid20P.Click += new System.EventHandler(this.buttonSelectMid20P_Click);
            // 
            // buttonSelectLow20P
            // 
            this.buttonSelectLow20P.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSelectLow20P.Location = new System.Drawing.Point(212, 117);
            this.buttonSelectLow20P.Name = "buttonSelectLow20P";
            this.buttonSelectLow20P.Size = new System.Drawing.Size(97, 23);
            this.buttonSelectLow20P.TabIndex = 19;
            this.buttonSelectLow20P.Text = "Select Low 20%";
            this.buttonSelectLow20P.UseVisualStyleBackColor = true;
            this.buttonSelectLow20P.Click += new System.EventHandler(this.buttonSelectLow20P_Click);
            // 
            // buttonSelectTop20P
            // 
            this.buttonSelectTop20P.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSelectTop20P.Location = new System.Drawing.Point(9, 117);
            this.buttonSelectTop20P.Name = "buttonSelectTop20P";
            this.buttonSelectTop20P.Size = new System.Drawing.Size(97, 23);
            this.buttonSelectTop20P.TabIndex = 18;
            this.buttonSelectTop20P.Text = "Select Top 20%";
            this.buttonSelectTop20P.UseVisualStyleBackColor = true;
            this.buttonSelectTop20P.Click += new System.EventHandler(this.buttonSelectTop20P_Click);
            // 
            // buttonClearAll
            // 
            this.buttonClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonClearAll.Location = new System.Drawing.Point(408, 447);
            this.buttonClearAll.Name = "buttonClearAll";
            this.buttonClearAll.Size = new System.Drawing.Size(76, 23);
            this.buttonClearAll.TabIndex = 15;
            this.buttonClearAll.Text = "Select None";
            this.buttonClearAll.UseVisualStyleBackColor = true;
            this.buttonClearAll.Click += new System.EventHandler(this.buttonClearAll_Click);
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSelectAll.Location = new System.Drawing.Point(339, 447);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(63, 23);
            this.buttonSelectAll.TabIndex = 14;
            this.buttonSelectAll.Text = "Select All";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApply.Location = new System.Drawing.Point(406, 606);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(75, 23);
            this.buttonApply.TabIndex = 16;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(319, 606);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 17;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // checkBoxIncludeSelected
            // 
            this.checkBoxIncludeSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxIncludeSelected.AutoSize = true;
            this.checkBoxIncludeSelected.Enabled = false;
            this.checkBoxIncludeSelected.Location = new System.Drawing.Point(23, 50);
            this.checkBoxIncludeSelected.Name = "checkBoxIncludeSelected";
            this.checkBoxIncludeSelected.Size = new System.Drawing.Size(103, 17);
            this.checkBoxIncludeSelected.TabIndex = 20;
            this.checkBoxIncludeSelected.Text = "include selected";
            this.toolTip1.SetToolTip(this.checkBoxIncludeSelected, "baseline is the first scenario in this list");
            this.checkBoxIncludeSelected.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeBaseline
            // 
            this.checkBoxIncludeBaseline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxIncludeBaseline.AutoSize = true;
            this.checkBoxIncludeBaseline.Enabled = false;
            this.checkBoxIncludeBaseline.Location = new System.Drawing.Point(23, 33);
            this.checkBoxIncludeBaseline.Name = "checkBoxIncludeBaseline";
            this.checkBoxIncludeBaseline.Size = new System.Drawing.Size(102, 17);
            this.checkBoxIncludeBaseline.TabIndex = 19;
            this.checkBoxIncludeBaseline.Text = "include baseline";
            this.toolTip1.SetToolTip(this.checkBoxIncludeBaseline, "baseline is the first scenario in this list");
            this.checkBoxIncludeBaseline.UseVisualStyleBackColor = true;
            // 
            // checkBoxSubtractFromBaseline
            // 
            this.checkBoxSubtractFromBaseline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxSubtractFromBaseline.AutoSize = true;
            this.checkBoxSubtractFromBaseline.Location = new System.Drawing.Point(7, 16);
            this.checkBoxSubtractFromBaseline.Name = "checkBoxSubtractFromBaseline";
            this.checkBoxSubtractFromBaseline.Size = new System.Drawing.Size(106, 17);
            this.checkBoxSubtractFromBaseline.TabIndex = 18;
            this.checkBoxSubtractFromBaseline.Text = "subtract baseline";
            this.toolTip1.SetToolTip(this.checkBoxSubtractFromBaseline, "baseline is the first scenario in this list");
            this.checkBoxSubtractFromBaseline.UseVisualStyleBackColor = true;
            this.checkBoxSubtractFromBaseline.CheckedChanged += new System.EventHandler(this.checkBoxSubtractFromBaseline_CheckedChanged);
            // 
            // checkBoxMerge
            // 
            this.checkBoxMerge.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxMerge.AutoSize = true;
            this.checkBoxMerge.Location = new System.Drawing.Point(7, 67);
            this.checkBoxMerge.Name = "checkBoxMerge";
            this.checkBoxMerge.Size = new System.Drawing.Size(103, 17);
            this.checkBoxMerge.TabIndex = 25;
            this.checkBoxMerge.Text = "merge scenarios";
            this.toolTip1.SetToolTip(this.checkBoxMerge, "For operational model runs, less than one year traces, merges data into a single " +
        "series.  Only valid if you select a single series in the tree.");
            this.checkBoxMerge.UseVisualStyleBackColor = true;
            // 
            // groupBoxComparisonn
            // 
            this.groupBoxComparisonn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxComparisonn.Controls.Add(this.checkBoxIncludeSelected);
            this.groupBoxComparisonn.Controls.Add(this.checkBoxMerge);
            this.groupBoxComparisonn.Controls.Add(this.checkBoxIncludeBaseline);
            this.groupBoxComparisonn.Controls.Add(this.checkBoxSubtractFromBaseline);
            this.groupBoxComparisonn.Location = new System.Drawing.Point(339, 476);
            this.groupBoxComparisonn.Name = "groupBoxComparisonn";
            this.groupBoxComparisonn.Size = new System.Drawing.Size(149, 117);
            this.groupBoxComparisonn.TabIndex = 24;
            this.groupBoxComparisonn.TabStop = false;
            this.groupBoxComparisonn.Text = "comparison";
            // 
            // groupBoxAll
            // 
            this.groupBoxAll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxAll.BackColor = System.Drawing.SystemColors.Control;
            this.groupBoxAll.Controls.Add(this.groupBoxSorting);
            this.groupBoxAll.Controls.Add(this.groupBoxComparisonn);
            this.groupBoxAll.Controls.Add(this.groupBox1);
            this.groupBoxAll.Controls.Add(this.buttonClearAll);
            this.groupBoxAll.Controls.Add(this.buttonSelectAll);
            this.groupBoxAll.Location = new System.Drawing.Point(2, 0);
            this.groupBoxAll.Name = "groupBoxAll";
            this.groupBoxAll.Size = new System.Drawing.Size(494, 600);
            this.groupBoxAll.TabIndex = 25;
            this.groupBoxAll.TabStop = false;
            // 
            // groupBoxSorting
            // 
            this.groupBoxSorting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxSorting.BackColor = System.Drawing.SystemColors.Control;
            this.groupBoxSorting.Controls.Add(this.panel3);
            this.groupBoxSorting.Controls.Add(this.panel2);
            this.groupBoxSorting.Controls.Add(this.panel1);
            this.groupBoxSorting.Controls.Add(this.label3);
            this.groupBoxSorting.Controls.Add(this.label2);
            this.groupBoxSorting.Controls.Add(this.buttonSelectLow20P);
            this.groupBoxSorting.Controls.Add(this.buttonSelectMid20P);
            this.groupBoxSorting.Controls.Add(this.label1);
            this.groupBoxSorting.Controls.Add(this.comboBoxSelectedSeries);
            this.groupBoxSorting.Controls.Add(this.buttonSelectTop20P);
            this.groupBoxSorting.Location = new System.Drawing.Point(6, 447);
            this.groupBoxSorting.Name = "groupBoxSorting";
            this.groupBoxSorting.Size = new System.Drawing.Size(327, 146);
            this.groupBoxSorting.TabIndex = 26;
            this.groupBoxSorting.TabStop = false;
            this.groupBoxSorting.Text = "sort";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.comboBoxOutYear);
            this.panel3.Controls.Add(this.radioButtonYear2);
            this.panel3.Controls.Add(this.radioButtonYear1);
            this.panel3.Location = new System.Drawing.Point(54, 85);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(254, 25);
            this.panel3.TabIndex = 34;
            // 
            // comboBoxOutYear
            // 
            this.comboBoxOutYear.Enabled = false;
            this.comboBoxOutYear.FormattingEnabled = true;
            this.comboBoxOutYear.Location = new System.Drawing.Point(206, 2);
            this.comboBoxOutYear.Name = "comboBoxOutYear";
            this.comboBoxOutYear.Size = new System.Drawing.Size(45, 21);
            this.comboBoxOutYear.TabIndex = 4;
            // 
            // radioButtonYear2
            // 
            this.radioButtonYear2.AutoSize = true;
            this.radioButtonYear2.Location = new System.Drawing.Point(89, 4);
            this.radioButtonYear2.Name = "radioButtonYear2";
            this.radioButtonYear2.Size = new System.Drawing.Size(116, 17);
            this.radioButtonYear2.TabIndex = 1;
            this.radioButtonYear2.Text = "current year to year";
            this.radioButtonYear2.UseVisualStyleBackColor = true;
            this.radioButtonYear2.CheckedChanged += new System.EventHandler(this.radioButtonYear2_CheckedChanged);
            // 
            // radioButtonYear1
            // 
            this.radioButtonYear1.AutoSize = true;
            this.radioButtonYear1.Checked = true;
            this.radioButtonYear1.Location = new System.Drawing.Point(3, 4);
            this.radioButtonYear1.Name = "radioButtonYear1";
            this.radioButtonYear1.Size = new System.Drawing.Size(81, 17);
            this.radioButtonYear1.TabIndex = 0;
            this.radioButtonYear1.TabStop = true;
            this.radioButtonYear1.Text = "current year";
            this.radioButtonYear1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.radioButtonEOP);
            this.panel2.Controls.Add(this.radioButtonSum);
            this.panel2.Location = new System.Drawing.Point(54, 59);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(254, 25);
            this.panel2.TabIndex = 33;
            // 
            // radioButtonEOP
            // 
            this.radioButtonEOP.AutoSize = true;
            this.radioButtonEOP.Location = new System.Drawing.Point(45, 4);
            this.radioButtonEOP.Name = "radioButtonEOP";
            this.radioButtonEOP.Size = new System.Drawing.Size(116, 17);
            this.radioButtonEOP.TabIndex = 1;
            this.radioButtonEOP.Text = "end-of-period value";
            this.radioButtonEOP.UseVisualStyleBackColor = true;
            // 
            // radioButtonSum
            // 
            this.radioButtonSum.AutoSize = true;
            this.radioButtonSum.Checked = true;
            this.radioButtonSum.Location = new System.Drawing.Point(3, 4);
            this.radioButtonSum.Name = "radioButtonSum";
            this.radioButtonSum.Size = new System.Drawing.Size(44, 17);
            this.radioButtonSum.TabIndex = 0;
            this.radioButtonSum.TabStop = true;
            this.radioButtonSum.Text = "sum";
            this.radioButtonSum.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboBoxMonths);
            this.panel1.Controls.Add(this.radioButtonMonth);
            this.panel1.Controls.Add(this.radioButtonWY);
            this.panel1.Controls.Add(this.radioButtonCY);
            this.panel1.Location = new System.Drawing.Point(54, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(254, 25);
            this.panel1.TabIndex = 32;
            // 
            // comboBoxMonths
            // 
            this.comboBoxMonths.Enabled = false;
            this.comboBoxMonths.FormattingEnabled = true;
            this.comboBoxMonths.Location = new System.Drawing.Point(146, 2);
            this.comboBoxMonths.Name = "comboBoxMonths";
            this.comboBoxMonths.Size = new System.Drawing.Size(59, 21);
            this.comboBoxMonths.TabIndex = 3;
            // 
            // radioButtonMonth
            // 
            this.radioButtonMonth.AutoSize = true;
            this.radioButtonMonth.Location = new System.Drawing.Point(89, 4);
            this.radioButtonMonth.Name = "radioButtonMonth";
            this.radioButtonMonth.Size = new System.Drawing.Size(54, 17);
            this.radioButtonMonth.TabIndex = 2;
            this.radioButtonMonth.Text = "month";
            this.radioButtonMonth.UseVisualStyleBackColor = true;
            this.radioButtonMonth.CheckedChanged += new System.EventHandler(this.radioButtonMonth_CheckedChanged);
            // 
            // radioButtonWY
            // 
            this.radioButtonWY.AutoSize = true;
            this.radioButtonWY.Location = new System.Drawing.Point(45, 4);
            this.radioButtonWY.Name = "radioButtonWY";
            this.radioButtonWY.Size = new System.Drawing.Size(38, 17);
            this.radioButtonWY.TabIndex = 1;
            this.radioButtonWY.Text = "wy";
            this.radioButtonWY.UseVisualStyleBackColor = true;
            // 
            // radioButtonCY
            // 
            this.radioButtonCY.AutoSize = true;
            this.radioButtonCY.Checked = true;
            this.radioButtonCY.Location = new System.Drawing.Point(3, 4);
            this.radioButtonCY.Name = "radioButtonCY";
            this.radioButtonCY.Size = new System.Drawing.Size(36, 17);
            this.radioButtonCY.TabIndex = 0;
            this.radioButtonCY.TabStop = true;
            this.radioButtonCY.Text = "cy";
            this.radioButtonCY.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(22, 13);
            this.label3.TabIndex = 31;
            this.label3.Text = "for:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "sort by:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "using:";
            // 
            // comboBoxSelectedSeries
            // 
            this.comboBoxSelectedSeries.FormattingEnabled = true;
            this.comboBoxSelectedSeries.Location = new System.Drawing.Point(53, 12);
            this.comboBoxSelectedSeries.Name = "comboBoxSelectedSeries";
            this.comboBoxSelectedSeries.Size = new System.Drawing.Size(256, 21);
            this.comboBoxSelectedSeries.TabIndex = 0;
            this.comboBoxSelectedSeries.SelectedIndexChanged += new System.EventHandler(this.comboBoxSelectedSeries_SelectedIndexChanged);
            // 
            // buttonSort
            // 
            this.buttonSort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSort.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSort.Location = new System.Drawing.Point(2, 606);
            this.buttonSort.Name = "buttonSort";
            this.buttonSort.Size = new System.Drawing.Size(75, 23);
            this.buttonSort.TabIndex = 26;
            this.buttonSort.Text = "Sort";
            this.buttonSort.UseVisualStyleBackColor = true;
            this.buttonSort.Click += new System.EventHandler(this.buttonSort_Click);
            // 
            // ScenarioSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(499, 638);
            this.Controls.Add(this.buttonSort);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.groupBoxAll);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(515, 625);
            this.Name = "ScenarioSelector";
            this.Text = "Trace / Scenario Filter";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScenarioSelector_FormClosing);
            this.Load += new System.EventHandler(this.ScenarioSelector_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBoxComparisonn.ResumeLayout(false);
            this.groupBoxComparisonn.PerformLayout();
            this.groupBoxAll.ResumeLayout(false);
            this.groupBoxSorting.ResumeLayout(false);
            this.groupBoxSorting.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonSelectAll;
        private System.Windows.Forms.Button buttonClearAll;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBoxComparisonn;
        private System.Windows.Forms.CheckBox checkBoxIncludeSelected;
        private System.Windows.Forms.CheckBox checkBoxIncludeBaseline;
        private System.Windows.Forms.CheckBox checkBoxSubtractFromBaseline;
        private System.Windows.Forms.GroupBox groupBoxAll;
        private System.Windows.Forms.CheckBox checkBoxMerge;
        private System.Windows.Forms.GroupBox groupBoxSorting;
        private System.Windows.Forms.ComboBox comboBoxSelectedSeries;
        private System.Windows.Forms.Button buttonSelectMid20P;
        private System.Windows.Forms.Button buttonSelectLow20P;
        private System.Windows.Forms.Button buttonSelectTop20P;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton radioButtonYear2;
        private System.Windows.Forms.RadioButton radioButtonYear1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton radioButtonEOP;
        private System.Windows.Forms.RadioButton radioButtonSum;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButtonMonth;
        private System.Windows.Forms.RadioButton radioButtonWY;
        private System.Windows.Forms.RadioButton radioButtonCY;
        private System.Windows.Forms.ComboBox comboBoxMonths;
        private System.Windows.Forms.ComboBox comboBoxOutYear;
        private System.Windows.Forms.Button buttonSort;
    }
}