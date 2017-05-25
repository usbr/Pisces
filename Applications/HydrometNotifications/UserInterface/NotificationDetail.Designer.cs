namespace HydrometNotifications.UserInterface
{
    partial class NotificationDetail
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
            this.components = new System.ComponentModel.Container();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.linkLabelBack = new System.Windows.Forms.LinkLabel();
            this.linkLabelDelete = new System.Windows.Forms.LinkLabel();
            this.textBoxEmailList = new System.Windows.Forms.TextBox();
            this.linkLabelSave = new System.Windows.Forms.LinkLabel();
            this.dataGridViewSiteList = new System.Windows.Forms.DataGridView();
            this.dataGridViewNotifications = new System.Windows.Forms.DataGridView();
            this.labelStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSiteList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewNotifications)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "email list";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "notifications";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 396);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "site lists";
            this.toolTip1.SetToolTip(this.label5, "site lists are used for notifications of multiple sites. Such as all AgriMet site" +
        "s.");
            // 
            // linkLabelBack
            // 
            this.linkLabelBack.AutoSize = true;
            this.linkLabelBack.Location = new System.Drawing.Point(15, 13);
            this.linkLabelBack.Name = "linkLabelBack";
            this.linkLabelBack.Size = new System.Drawing.Size(43, 13);
            this.linkLabelBack.TabIndex = 6;
            this.linkLabelBack.TabStop = true;
            this.linkLabelBack.Text = "<<back";
            this.linkLabelBack.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelBack_LinkClicked);
            // 
            // linkLabelDelete
            // 
            this.linkLabelDelete.AutoSize = true;
            this.linkLabelDelete.Location = new System.Drawing.Point(197, 13);
            this.linkLabelDelete.Name = "linkLabelDelete";
            this.linkLabelDelete.Size = new System.Drawing.Size(48, 13);
            this.linkLabelDelete.TabIndex = 7;
            this.linkLabelDelete.TabStop = true;
            this.linkLabelDelete.Text = "delete ...";
            this.linkLabelDelete.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelDelete_LinkClicked);
            // 
            // textBoxEmailList
            // 
            this.textBoxEmailList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEmailList.Location = new System.Drawing.Point(92, 82);
            this.textBoxEmailList.Name = "textBoxEmailList";
            this.textBoxEmailList.Size = new System.Drawing.Size(588, 20);
            this.textBoxEmailList.TabIndex = 8;
            // 
            // linkLabelSave
            // 
            this.linkLabelSave.AutoSize = true;
            this.linkLabelSave.Location = new System.Drawing.Point(89, 13);
            this.linkLabelSave.Name = "linkLabelSave";
            this.linkLabelSave.Size = new System.Drawing.Size(30, 13);
            this.linkLabelSave.TabIndex = 9;
            this.linkLabelSave.TabStop = true;
            this.linkLabelSave.Text = "save";
            this.linkLabelSave.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSave_LinkClicked);
            // 
            // dataGridViewSiteList
            // 
            this.dataGridViewSiteList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewSiteList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSiteList.Location = new System.Drawing.Point(18, 412);
            this.dataGridViewSiteList.Name = "dataGridViewSiteList";
            this.dataGridViewSiteList.Size = new System.Drawing.Size(662, 140);
            this.dataGridViewSiteList.TabIndex = 10;
            // 
            // dataGridViewNotifications
            // 
            this.dataGridViewNotifications.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewNotifications.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewNotifications.Location = new System.Drawing.Point(18, 139);
            this.dataGridViewNotifications.Name = "dataGridViewNotifications";
            this.dataGridViewNotifications.Size = new System.Drawing.Size(662, 245);
            this.dataGridViewNotifications.TabIndex = 11;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(63, 35);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(0, 13);
            this.labelStatus.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "name:";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(92, 51);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.ReadOnly = true;
            this.textBoxName.Size = new System.Drawing.Size(168, 20);
            this.textBoxName.TabIndex = 14;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(176, 106);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(465, 20);
            this.textBox1.TabIndex = 15;
            this.textBox1.Text = "symbols:  d:jck af (daily)  i:amf fb (instant)   %value %data_source";
            // 
            // NotificationDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.dataGridViewNotifications);
            this.Controls.Add(this.dataGridViewSiteList);
            this.Controls.Add(this.linkLabelSave);
            this.Controls.Add(this.textBoxEmailList);
            this.Controls.Add(this.linkLabelDelete);
            this.Controls.Add(this.linkLabelBack);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "NotificationDetail";
            this.Size = new System.Drawing.Size(704, 568);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSiteList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewNotifications)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.LinkLabel linkLabelBack;
        private System.Windows.Forms.LinkLabel linkLabelDelete;
        private System.Windows.Forms.TextBox textBoxEmailList;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.LinkLabel linkLabelSave;
        private System.Windows.Forms.DataGridView dataGridViewSiteList;
        private System.Windows.Forms.DataGridView dataGridViewNotifications;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.TextBox textBox1;
    }
}
