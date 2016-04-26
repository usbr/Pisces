namespace Reclamation.TimeSeries.SHEF
{
    partial class ImportShef
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.stationsListBox = new System.Windows.Forms.ListBox();
            this.peCodesListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.shefSelectButton = new System.Windows.Forms.Button();
            this.shefFileSelected = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Location = new System.Drawing.Point(2, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(365, 44);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SHEF Format";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(6, 17);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(63, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "SHEF-A";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // stationsListBox
            // 
            this.stationsListBox.FormattingEnabled = true;
            this.stationsListBox.Location = new System.Drawing.Point(7, 96);
            this.stationsListBox.Name = "stationsListBox";
            this.stationsListBox.Size = new System.Drawing.Size(173, 17);
            this.stationsListBox.TabIndex = 1;
            this.stationsListBox.SelectedIndexChanged += new System.EventHandler(this.GetShefCodeForLocation);
            // 
            // peCodesListBox
            // 
            this.peCodesListBox.FormattingEnabled = true;
            this.peCodesListBox.Location = new System.Drawing.Point(194, 96);
            this.peCodesListBox.Name = "peCodesListBox";
            this.peCodesListBox.Size = new System.Drawing.Size(173, 17);
            this.peCodesListBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Available Station Codes";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(191, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Available Parameter Codes";
            // 
            // shefSelectButton
            // 
            this.shefSelectButton.Location = new System.Drawing.Point(2, 53);
            this.shefSelectButton.Name = "shefSelectButton";
            this.shefSelectButton.Size = new System.Drawing.Size(82, 23);
            this.shefSelectButton.TabIndex = 5;
            this.shefSelectButton.Text = "Select SHEF";
            this.shefSelectButton.UseVisualStyleBackColor = true;
            this.shefSelectButton.Click += new System.EventHandler(this.shefSelectButton_Click);
            // 
            // shefFileSelected
            // 
            this.shefFileSelected.Location = new System.Drawing.Point(90, 53);
            this.shefFileSelected.Name = "shefFileSelected";
            this.shefFileSelected.Size = new System.Drawing.Size(277, 20);
            this.shefFileSelected.TabIndex = 6;
            // 
            // ImportShef
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 122);
            this.Controls.Add(this.shefFileSelected);
            this.Controls.Add(this.shefSelectButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.peCodesListBox);
            this.Controls.Add(this.stationsListBox);
            this.Controls.Add(this.groupBox1);
            this.Name = "ImportShef";
            this.Text = "ImportShef";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.ListBox stationsListBox;
        private System.Windows.Forms.ListBox peCodesListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button shefSelectButton;
        private System.Windows.Forms.TextBox shefFileSelected;
    }
}