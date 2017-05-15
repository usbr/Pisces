namespace HydrometTools.Advanced
{
    partial class ArchiverInput
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
            this.textBoxCbtt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.buttonRunRawData = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxPcode = new System.Windows.Forms.TextBox();
            this.buttonHelpArchiver = new System.Windows.Forms.Button();
            this.checkBoxAllPcodes = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBoxCbtt
            // 
            this.textBoxCbtt.Location = new System.Drawing.Point(48, 32);
            this.textBoxCbtt.Name = "textBoxCbtt";
            this.textBoxCbtt.Size = new System.Drawing.Size(89, 20);
            this.textBoxCbtt.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "cbtt";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyyMMMdd HH:mm";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(18, 56);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(102, 20);
            this.dateTimePicker1.TabIndex = 2;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "yyyyMMMdd HH:mm";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker2.Location = new System.Drawing.Point(18, 82);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(102, 20);
            this.dateTimePicker2.TabIndex = 3;
            // 
            // buttonRunRawData
            // 
            this.buttonRunRawData.Location = new System.Drawing.Point(185, 79);
            this.buttonRunRawData.Name = "buttonRunRawData";
            this.buttonRunRawData.Size = new System.Drawing.Size(75, 23);
            this.buttonRunRawData.TabIndex = 4;
            this.buttonRunRawData.Text = "Run";
            this.buttonRunRawData.UseVisualStyleBackColor = true;
            this.buttonRunRawData.Click += new System.EventHandler(this.buttonRunRawData_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(145, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "pcode";
            // 
            // textBoxPcode
            // 
            this.textBoxPcode.Location = new System.Drawing.Point(185, 32);
            this.textBoxPcode.Name = "textBoxPcode";
            this.textBoxPcode.Size = new System.Drawing.Size(89, 20);
            this.textBoxPcode.TabIndex = 1;
            // 
            // buttonHelpArchiver
            // 
            this.buttonHelpArchiver.Location = new System.Drawing.Point(3, 3);
            this.buttonHelpArchiver.Name = "buttonHelpArchiver";
            this.buttonHelpArchiver.Size = new System.Drawing.Size(60, 23);
            this.buttonHelpArchiver.TabIndex = 8;
            this.buttonHelpArchiver.Text = "help";
            this.buttonHelpArchiver.UseVisualStyleBackColor = true;
            this.buttonHelpArchiver.Click += new System.EventHandler(this.buttonHelpArchiver_Click);
            // 
            // checkBoxAllPcodes
            // 
            this.checkBoxAllPcodes.AutoSize = true;
            this.checkBoxAllPcodes.Location = new System.Drawing.Point(186, 9);
            this.checkBoxAllPcodes.Name = "checkBoxAllPcodes";
            this.checkBoxAllPcodes.Size = new System.Drawing.Size(74, 17);
            this.checkBoxAllPcodes.TabIndex = 9;
            this.checkBoxAllPcodes.Text = "all pcodes";
            this.checkBoxAllPcodes.UseVisualStyleBackColor = true;
            this.checkBoxAllPcodes.CheckedChanged += new System.EventHandler(this.checkBoxAllPcodes_CheckedChanged);
            // 
            // ArchiverInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxAllPcodes);
            this.Controls.Add(this.buttonHelpArchiver);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxPcode);
            this.Controls.Add(this.buttonRunRawData);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxCbtt);
            this.Name = "ArchiverInput";
            this.Size = new System.Drawing.Size(312, 141);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxCbtt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Button buttonRunRawData;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxPcode;
        private System.Windows.Forms.Button buttonHelpArchiver;
        private System.Windows.Forms.CheckBox checkBoxAllPcodes;
    }
}
