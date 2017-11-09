namespace Reclamation.TimeSeries.Forms.ImportForms
{
    partial class ImportHydromet
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.radioButtonBoise = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButtonGP = new System.Windows.Forms.RadioButton();
            this.radioButtonYakima = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioButtonMpoll = new System.Windows.Forms.RadioButton();
            this.radioButtonFifteenMinute = new System.Windows.Forms.RadioButton();
            this.radioButtonDailyAverage = new System.Windows.Forms.RadioButton();
            this.checkBoxSimpleName = new System.Windows.Forms.CheckBox();
            this.radioButtonHyd1 = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(366, 212);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(366, 241);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::Reclamation.TimeSeries.Forms.Properties.Settings.Default, "HydrometT1", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(57, 16);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(125, 20);
            this.dateTimePicker1.TabIndex = 2;
            this.dateTimePicker1.Value = global::Reclamation.TimeSeries.Forms.Properties.Settings.Default.HydrometT1;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker2.Location = new System.Drawing.Point(57, 47);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(125, 20);
            this.dateTimePicker2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "From";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "To";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Controls.Add(this.dateTimePicker2);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 81);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Date Range";
            // 
            // textBox1
            // 
            this.textBox1.CausesValidation = false;
            this.textBox1.Location = new System.Drawing.Point(12, 128);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(166, 20);
            this.textBox1.TabIndex = 7;
            this.textBox1.Text = "pal af";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "example : jck af";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "enter station pcode pair";
            // 
            // radioButtonBoise
            // 
            this.radioButtonBoise.AutoSize = true;
            this.radioButtonBoise.Checked = true;
            this.radioButtonBoise.Location = new System.Drawing.Point(18, 19);
            this.radioButtonBoise.Name = "radioButtonBoise";
            this.radioButtonBoise.Size = new System.Drawing.Size(94, 17);
            this.radioButtonBoise.TabIndex = 10;
            this.radioButtonBoise.Text = "boise (pnhyd0)";
            this.radioButtonBoise.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButtonHyd1);
            this.groupBox2.Controls.Add(this.radioButtonGP);
            this.groupBox2.Controls.Add(this.radioButtonYakima);
            this.groupBox2.Controls.Add(this.radioButtonBoise);
            this.groupBox2.Location = new System.Drawing.Point(15, 154);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(179, 110);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Server";
            // 
            // radioButtonGP
            // 
            this.radioButtonGP.AutoSize = true;
            this.radioButtonGP.Location = new System.Drawing.Point(18, 83);
            this.radioButtonGP.Name = "radioButtonGP";
            this.radioButtonGP.Size = new System.Drawing.Size(79, 17);
            this.radioButtonGP.TabIndex = 12;
            this.radioButtonGP.Text = "great plains";
            this.radioButtonGP.UseVisualStyleBackColor = true;
            // 
            // radioButtonYakima
            // 
            this.radioButtonYakima.AutoSize = true;
            this.radioButtonYakima.Location = new System.Drawing.Point(18, 60);
            this.radioButtonYakima.Name = "radioButtonYakima";
            this.radioButtonYakima.Size = new System.Drawing.Size(101, 17);
            this.radioButtonYakima.TabIndex = 11;
            this.radioButtonYakima.Text = "yakima (yakhyd)";
            this.radioButtonYakima.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.radioButtonMpoll);
            this.groupBox3.Controls.Add(this.radioButtonFifteenMinute);
            this.groupBox3.Controls.Add(this.radioButtonDailyAverage);
            this.groupBox3.Location = new System.Drawing.Point(246, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(187, 90);
            this.groupBox3.TabIndex = 12;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Time Interval";
            // 
            // radioButtonMpoll
            // 
            this.radioButtonMpoll.AutoSize = true;
            this.radioButtonMpoll.Location = new System.Drawing.Point(18, 65);
            this.radioButtonMpoll.Name = "radioButtonMpoll";
            this.radioButtonMpoll.Size = new System.Drawing.Size(166, 17);
            this.radioButtonMpoll.TabIndex = 12;
            this.radioButtonMpoll.Text = "monthly (Reclamation internal)";
            this.radioButtonMpoll.UseVisualStyleBackColor = true;
            // 
            // radioButtonFifteenMinute
            // 
            this.radioButtonFifteenMinute.AutoSize = true;
            this.radioButtonFifteenMinute.Location = new System.Drawing.Point(18, 42);
            this.radioButtonFifteenMinute.Name = "radioButtonFifteenMinute";
            this.radioButtonFifteenMinute.Size = new System.Drawing.Size(91, 17);
            this.radioButtonFifteenMinute.TabIndex = 11;
            this.radioButtonFifteenMinute.Text = "instantaneous";
            this.radioButtonFifteenMinute.UseVisualStyleBackColor = true;
            // 
            // radioButtonDailyAverage
            // 
            this.radioButtonDailyAverage.AutoSize = true;
            this.radioButtonDailyAverage.Checked = true;
            this.radioButtonDailyAverage.Location = new System.Drawing.Point(18, 19);
            this.radioButtonDailyAverage.Name = "radioButtonDailyAverage";
            this.radioButtonDailyAverage.Size = new System.Drawing.Size(46, 17);
            this.radioButtonDailyAverage.TabIndex = 10;
            this.radioButtonDailyAverage.TabStop = true;
            this.radioButtonDailyAverage.Text = "daily";
            this.radioButtonDailyAverage.UseVisualStyleBackColor = true;
            // 
            // checkBoxSimpleName
            // 
            this.checkBoxSimpleName.AutoSize = true;
            this.checkBoxSimpleName.Location = new System.Drawing.Point(185, 128);
            this.checkBoxSimpleName.Name = "checkBoxSimpleName";
            this.checkBoxSimpleName.Size = new System.Drawing.Size(207, 17);
            this.checkBoxSimpleName.TabIndex = 13;
            this.checkBoxSimpleName.Text = "use this simple name in the Pisces tree";
            this.checkBoxSimpleName.UseVisualStyleBackColor = true;
            // 
            // radioButtonHyd1
            // 
            this.radioButtonHyd1.AutoSize = true;
            this.radioButtonHyd1.Location = new System.Drawing.Point(19, 40);
            this.radioButtonHyd1.Name = "radioButtonHyd1";
            this.radioButtonHyd1.Size = new System.Drawing.Size(94, 17);
            this.radioButtonHyd1.TabIndex = 13;
            this.radioButtonHyd1.Text = "boise (pnhyd1)";
            this.radioButtonHyd1.UseVisualStyleBackColor = true;
            // 
            // ImportHydromet
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(444, 276);
            this.Controls.Add(this.checkBoxSimpleName);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ImportHydromet";
            this.Text = "Import Hydromet/AgriMet Data from the Reclamation web site";
            this.Load += new System.EventHandler(this.FormImportHydromet_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton radioButtonBoise;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButtonYakima;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton radioButtonFifteenMinute;
        private System.Windows.Forms.RadioButton radioButtonDailyAverage;
        private System.Windows.Forms.RadioButton radioButtonGP;
        private System.Windows.Forms.RadioButton radioButtonMpoll;
        private System.Windows.Forms.CheckBox checkBoxSimpleName;
        private System.Windows.Forms.RadioButton radioButtonHyd1;
    }
}