namespace Reclamation.TimeSeries.Forms.Alarms
{
    partial class AlarmSetup
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
            this.panelList = new System.Windows.Forms.Panel();
            this.buttonTest = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dgvList = new System.Windows.Forms.DataGridView();
            this.panelRecipients = new System.Windows.Forms.Panel();
            this.buttonSaveRecipients = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.dataGridViewRecipient = new System.Windows.Forms.DataGridView();
            this.panelList.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).BeginInit();
            this.panelRecipients.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRecipient)).BeginInit();
            this.SuspendLayout();
            // 
            // panelList
            // 
            this.panelList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelList.Controls.Add(this.buttonTest);
            this.panelList.Controls.Add(this.buttonDelete);
            this.panelList.Controls.Add(this.buttonAdd);
            this.panelList.Controls.Add(this.panel2);
            this.panelList.Controls.Add(this.dgvList);
            this.panelList.Location = new System.Drawing.Point(2, 1);
            this.panelList.Name = "panelList";
            this.panelList.Size = new System.Drawing.Size(253, 308);
            this.panelList.TabIndex = 0;
            // 
            // buttonTest
            // 
            this.buttonTest.Enabled = false;
            this.buttonTest.Location = new System.Drawing.Point(174, 268);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 23);
            this.buttonTest.TabIndex = 4;
            this.buttonTest.Text = "test...";
            this.buttonTest.UseVisualStyleBackColor = true;
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(93, 268);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonDelete.TabIndex = 3;
            this.buttonDelete.Text = "delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(12, 268);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 2;
            this.buttonAdd.Text = "add";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(251, 44);
            this.panel2.TabIndex = 1;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 15);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(206, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "alarm group";
            // 
            // dgvList
            // 
            this.dgvList.AllowUserToAddRows = false;
            this.dgvList.AllowUserToDeleteRows = false;
            this.dgvList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvList.Location = new System.Drawing.Point(3, 50);
            this.dgvList.MultiSelect = false;
            this.dgvList.Name = "dgvList";
            this.dgvList.ReadOnly = true;
            this.dgvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvList.Size = new System.Drawing.Size(245, 212);
            this.dgvList.TabIndex = 0;
            this.dgvList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridViewList_MouseClick);
            // 
            // panelRecipients
            // 
            this.panelRecipients.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRecipients.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelRecipients.Controls.Add(this.buttonSaveRecipients);
            this.panelRecipients.Controls.Add(this.panel5);
            this.panelRecipients.Controls.Add(this.dataGridViewRecipient);
            this.panelRecipients.Location = new System.Drawing.Point(257, 1);
            this.panelRecipients.Name = "panelRecipients";
            this.panelRecipients.Size = new System.Drawing.Size(469, 308);
            this.panelRecipients.TabIndex = 1;
            // 
            // buttonSaveRecipients
            // 
            this.buttonSaveRecipients.Location = new System.Drawing.Point(12, 268);
            this.buttonSaveRecipients.Name = "buttonSaveRecipients";
            this.buttonSaveRecipients.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveRecipients.TabIndex = 5;
            this.buttonSaveRecipients.Text = "save";
            this.buttonSaveRecipients.UseVisualStyleBackColor = true;
            this.buttonSaveRecipients.Click += new System.EventHandler(this.buttonSaveRecipients_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.textBox3);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(467, 44);
            this.panel5.TabIndex = 1;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(12, 15);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(206, 20);
            this.textBox3.TabIndex = 0;
            this.textBox3.Text = "recipients";
            // 
            // dataGridViewRecipient
            // 
            this.dataGridViewRecipient.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewRecipient.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRecipient.Location = new System.Drawing.Point(3, 50);
            this.dataGridViewRecipient.Name = "dataGridViewRecipient";
            this.dataGridViewRecipient.Size = new System.Drawing.Size(461, 212);
            this.dataGridViewRecipient.TabIndex = 0;
            // 
            // AlarmSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelRecipients);
            this.Controls.Add(this.panelList);
            this.Name = "AlarmSetup";
            this.Size = new System.Drawing.Size(727, 544);
            this.Load += new System.EventHandler(this.AlarmSetup_Load);
            this.panelList.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvList)).EndInit();
            this.panelRecipients.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRecipient)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelList;
        private System.Windows.Forms.DataGridView dgvList;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panelRecipients;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.DataGridView dataGridViewRecipient;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonSaveRecipients;
    }
}
