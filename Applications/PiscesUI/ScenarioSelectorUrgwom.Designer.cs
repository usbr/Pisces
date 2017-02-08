namespace Reclamation.TimeSeries.Forms
{
    partial class ScenarioSelectorUrgwom
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
            this.buttonClearAll = new System.Windows.Forms.Button();
            this.buttonSelectAll = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.checkBoxSubtractFromBaseline = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxIncludeBaseline = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeSelected = new System.Windows.Forms.CheckBox();
            this.checkBoxMonth1 = new System.Windows.Forms.CheckBox();
            this.checkBoxMonth2 = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox90 = new System.Windows.Forms.CheckBox();
            this.checkBox70 = new System.Windows.Forms.CheckBox();
            this.checkBox50 = new System.Windows.Forms.CheckBox();
            this.checkBox30 = new System.Windows.Forms.CheckBox();
            this.checkBox10 = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBoxComparisonn = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBoxComparisonn.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(194, 463);
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
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Controls.Add(this.buttonClearAll);
            this.groupBox1.Controls.Add(this.buttonSelectAll);
            this.groupBox1.Location = new System.Drawing.Point(12, 95);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(434, 336);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Scenarios";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(16, 19);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(314, 311);
            this.dataGridView1.TabIndex = 17;
            // 
            // buttonClearAll
            // 
            this.buttonClearAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearAll.Location = new System.Drawing.Point(337, 71);
            this.buttonClearAll.Name = "buttonClearAll";
            this.buttonClearAll.Size = new System.Drawing.Size(75, 23);
            this.buttonClearAll.TabIndex = 15;
            this.buttonClearAll.Text = "Select None";
            this.buttonClearAll.UseVisualStyleBackColor = true;
            this.buttonClearAll.Click += new System.EventHandler(this.buttonClearAll_Click);
            // 
            // buttonSelectAll
            // 
            this.buttonSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelectAll.Location = new System.Drawing.Point(336, 32);
            this.buttonSelectAll.Name = "buttonSelectAll";
            this.buttonSelectAll.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectAll.TabIndex = 14;
            this.buttonSelectAll.Text = "Select All";
            this.buttonSelectAll.UseVisualStyleBackColor = true;
            this.buttonSelectAll.Click += new System.EventHandler(this.buttonSelectAll_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApply.Location = new System.Drawing.Point(366, 463);
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
            this.buttonCancel.Location = new System.Drawing.Point(279, 463);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 17;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // checkBoxSubtractFromBaseline
            // 
            this.checkBoxSubtractFromBaseline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxSubtractFromBaseline.AutoSize = true;
            this.checkBoxSubtractFromBaseline.Location = new System.Drawing.Point(6, 30);
            this.checkBoxSubtractFromBaseline.Name = "checkBoxSubtractFromBaseline";
            this.checkBoxSubtractFromBaseline.Size = new System.Drawing.Size(106, 17);
            this.checkBoxSubtractFromBaseline.TabIndex = 18;
            this.checkBoxSubtractFromBaseline.Text = "subtract baseline";
            this.toolTip1.SetToolTip(this.checkBoxSubtractFromBaseline, "baseline is the first scenario in this list");
            this.checkBoxSubtractFromBaseline.UseVisualStyleBackColor = true;
            this.checkBoxSubtractFromBaseline.CheckedChanged += new System.EventHandler(this.checkBoxSubtractFromBaseline_CheckedChanged);
            // 
            // checkBoxIncludeBaseline
            // 
            this.checkBoxIncludeBaseline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxIncludeBaseline.AutoSize = true;
            this.checkBoxIncludeBaseline.Location = new System.Drawing.Point(6, 13);
            this.checkBoxIncludeBaseline.Name = "checkBoxIncludeBaseline";
            this.checkBoxIncludeBaseline.Size = new System.Drawing.Size(102, 17);
            this.checkBoxIncludeBaseline.TabIndex = 19;
            this.checkBoxIncludeBaseline.Text = "include baseline";
            this.toolTip1.SetToolTip(this.checkBoxIncludeBaseline, "baseline is the first scenario in this list");
            this.checkBoxIncludeBaseline.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeSelected
            // 
            this.checkBoxIncludeSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxIncludeSelected.AutoSize = true;
            this.checkBoxIncludeSelected.Enabled = false;
            this.checkBoxIncludeSelected.Location = new System.Drawing.Point(22, 47);
            this.checkBoxIncludeSelected.Name = "checkBoxIncludeSelected";
            this.checkBoxIncludeSelected.Size = new System.Drawing.Size(103, 17);
            this.checkBoxIncludeSelected.TabIndex = 20;
            this.checkBoxIncludeSelected.Text = "include selected";
            this.toolTip1.SetToolTip(this.checkBoxIncludeSelected, "baseline is the first scenario in this list");
            this.checkBoxIncludeSelected.UseVisualStyleBackColor = true;
            // 
            // checkBoxMonth1
            // 
            this.checkBoxMonth1.AutoSize = true;
            this.checkBoxMonth1.Checked = true;
            this.checkBoxMonth1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMonth1.Location = new System.Drawing.Point(14, 19);
            this.checkBoxMonth1.Name = "checkBoxMonth1";
            this.checkBoxMonth1.Size = new System.Drawing.Size(55, 17);
            this.checkBoxMonth1.TabIndex = 19;
            this.checkBoxMonth1.Text = "march";
            this.checkBoxMonth1.UseVisualStyleBackColor = true;
            // 
            // checkBoxMonth2
            // 
            this.checkBoxMonth2.AutoSize = true;
            this.checkBoxMonth2.Location = new System.Drawing.Point(14, 42);
            this.checkBoxMonth2.Name = "checkBoxMonth2";
            this.checkBoxMonth2.Size = new System.Drawing.Size(45, 17);
            this.checkBoxMonth2.TabIndex = 20;
            this.checkBoxMonth2.Text = "april";
            this.checkBoxMonth2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBox90);
            this.groupBox2.Controls.Add(this.checkBox70);
            this.groupBox2.Controls.Add(this.checkBox50);
            this.groupBox2.Controls.Add(this.checkBox30);
            this.groupBox2.Controls.Add(this.checkBox10);
            this.groupBox2.Location = new System.Drawing.Point(179, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(269, 59);
            this.groupBox2.TabIndex = 21;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "percentile";
            // 
            // checkBox90
            // 
            this.checkBox90.AutoSize = true;
            this.checkBox90.Location = new System.Drawing.Point(205, 19);
            this.checkBox90.Name = "checkBox90";
            this.checkBox90.Size = new System.Drawing.Size(46, 17);
            this.checkBox90.TabIndex = 24;
            this.checkBox90.Text = "90%";
            this.checkBox90.UseVisualStyleBackColor = true;
            // 
            // checkBox70
            // 
            this.checkBox70.AutoSize = true;
            this.checkBox70.Location = new System.Drawing.Point(153, 19);
            this.checkBox70.Name = "checkBox70";
            this.checkBox70.Size = new System.Drawing.Size(46, 17);
            this.checkBox70.TabIndex = 23;
            this.checkBox70.Text = "70%";
            this.checkBox70.UseVisualStyleBackColor = true;
            // 
            // checkBox50
            // 
            this.checkBox50.AutoSize = true;
            this.checkBox50.Checked = true;
            this.checkBox50.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox50.Location = new System.Drawing.Point(106, 19);
            this.checkBox50.Name = "checkBox50";
            this.checkBox50.Size = new System.Drawing.Size(46, 17);
            this.checkBox50.TabIndex = 22;
            this.checkBox50.Text = "50%";
            this.checkBox50.UseVisualStyleBackColor = true;
            // 
            // checkBox30
            // 
            this.checkBox30.AutoSize = true;
            this.checkBox30.Location = new System.Drawing.Point(54, 19);
            this.checkBox30.Name = "checkBox30";
            this.checkBox30.Size = new System.Drawing.Size(46, 17);
            this.checkBox30.TabIndex = 21;
            this.checkBox30.Text = "30%";
            this.checkBox30.UseVisualStyleBackColor = true;
            // 
            // checkBox10
            // 
            this.checkBox10.AutoSize = true;
            this.checkBox10.Location = new System.Drawing.Point(6, 19);
            this.checkBox10.Name = "checkBox10";
            this.checkBox10.Size = new System.Drawing.Size(46, 17);
            this.checkBox10.TabIndex = 20;
            this.checkBox10.Text = "10%";
            this.checkBox10.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBoxMonth1);
            this.groupBox3.Controls.Add(this.checkBoxMonth2);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(161, 60);
            this.groupBox3.TabIndex = 22;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "forecast";
            // 
            // groupBoxComparisonn
            // 
            this.groupBoxComparisonn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxComparisonn.Controls.Add(this.checkBoxIncludeSelected);
            this.groupBoxComparisonn.Controls.Add(this.checkBoxIncludeBaseline);
            this.groupBoxComparisonn.Controls.Add(this.checkBoxSubtractFromBaseline);
            this.groupBoxComparisonn.Location = new System.Drawing.Point(12, 431);
            this.groupBoxComparisonn.Name = "groupBoxComparisonn";
            this.groupBoxComparisonn.Size = new System.Drawing.Size(172, 67);
            this.groupBoxComparisonn.TabIndex = 23;
            this.groupBoxComparisonn.TabStop = false;
            this.groupBoxComparisonn.Text = "comparison";
            // 
            // ScenarioSelectorUrgwom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 498);
            this.Controls.Add(this.groupBoxComparisonn);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonOK);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 800);
            this.MinimumSize = new System.Drawing.Size(319, 293);
            this.Name = "ScenarioSelectorUrgwom";
            this.Text = "Choose scenarios";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScenarioSelector_FormClosing);
            this.Load += new System.EventHandler(this.ScenarioSelector_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBoxComparisonn.ResumeLayout(false);
            this.groupBoxComparisonn.PerformLayout();
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
        private System.Windows.Forms.CheckBox checkBoxSubtractFromBaseline;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBoxMonth1;
        private System.Windows.Forms.CheckBox checkBoxMonth2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox checkBox90;
        private System.Windows.Forms.CheckBox checkBox70;
        private System.Windows.Forms.CheckBox checkBox50;
        private System.Windows.Forms.CheckBox checkBox30;
        private System.Windows.Forms.CheckBox checkBox10;
        private System.Windows.Forms.GroupBox groupBoxComparisonn;
        private System.Windows.Forms.CheckBox checkBoxIncludeSelected;
        private System.Windows.Forms.CheckBox checkBoxIncludeBaseline;
    }
}