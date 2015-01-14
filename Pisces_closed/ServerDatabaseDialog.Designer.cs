namespace Pisces
{
    partial class ServerDatabaseDialog
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
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxServerDatabase = new System.Windows.Forms.TextBox();
            this.comboBoxDbType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(344, 108);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 0;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(274, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "server:database   i.e. 127.0.0.1:timeseries";
            // 
            // textBoxServerDatabase
            // 
            this.textBoxServerDatabase.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Pisces.Properties.Settings.Default, "ServerDatabase", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxServerDatabase.Location = new System.Drawing.Point(51, 48);
            this.textBoxServerDatabase.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxServerDatabase.Name = "textBoxServerDatabase";
            this.textBoxServerDatabase.Size = new System.Drawing.Size(341, 22);
            this.textBoxServerDatabase.TabIndex = 1;
            this.textBoxServerDatabase.Text = global::Pisces.Properties.Settings.Default.ServerDatabase;
            // 
            // comboBoxDbType
            // 
            this.comboBoxDbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDbType.FormattingEnabled = true;
            this.comboBoxDbType.Items.AddRange(new object[] {
            "postgresql",
            "mysql"});
            this.comboBoxDbType.Location = new System.Drawing.Point(29, 108);
            this.comboBoxDbType.Name = "comboBoxDbType";
            this.comboBoxDbType.Size = new System.Drawing.Size(90, 24);
            this.comboBoxDbType.TabIndex = 3;
            // 
            // ServerDatabaseDialog
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 151);
            this.Controls.Add(this.comboBoxDbType);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxServerDatabase);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ServerDatabaseDialog";
            this.Text = "Connection Information";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxServerDatabase;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxDbType;
    }
}