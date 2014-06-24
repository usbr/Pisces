namespace Reclamation.TimeSeries.Forms
{
    partial class FormSumRange
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
            Reclamation.Core.MonthDayRange monthDayRange1 = new Reclamation.Core.MonthDayRange();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSiteName = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.monthDayRangePicker1 = new Reclamation.TimeSeries.Forms.MonthDayRangePicker();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "text files (*.txt)|*.txt|All files (*.*)|*.*";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 134);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "name of series to use in volume summation";
            // 
            // textBoxSiteName
            // 
            this.textBoxSiteName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSiteName.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::Reclamation.TimeSeries.Forms.Properties.Settings.Default, "SumVolumeSiteName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.textBoxSiteName.Location = new System.Drawing.Point(16, 150);
            this.textBoxSiteName.Name = "textBoxSiteName";
            this.textBoxSiteName.Size = new System.Drawing.Size(180, 20);
            this.textBoxSiteName.TabIndex = 1;
            this.textBoxSiteName.Text = global::Reclamation.TimeSeries.Forms.Properties.Settings.Default.SumVolumeSiteName;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(261, 187);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(171, 187);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 13);
            this.label2.TabIndex = 6;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 13);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(324, 47);
            this.textBox2.TabIndex = 7;
            this.textBox2.Text = "Select a range of dates and a series to sum during a period of the year. This can" +
                " be used to rank and compare scenarios to a volume forecast. Summing is done on " +
                "a water year (october-september)";
            // 
            // monthDayRangePicker1
            // 
            this.monthDayRangePicker1.BeginningMonth = 10;
            this.monthDayRangePicker1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.monthDayRangePicker1.Location = new System.Drawing.Point(16, 66);
            this.monthDayRangePicker1.MonthDayRange = monthDayRange1;
            this.monthDayRangePicker1.Name = "monthDayRangePicker1";
            this.monthDayRangePicker1.Size = new System.Drawing.Size(182, 65);
            this.monthDayRangePicker1.TabIndex = 0;
            // 
            // FormSumRange
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(342, 216);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxSiteName);
            this.Controls.Add(this.monthDayRangePicker1);
            this.Name = "FormSumRange";
            this.Text = "Select Series and Range ";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Reclamation.TimeSeries.Forms.MonthDayRangePicker monthDayRangePicker1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        public System.Windows.Forms.TextBox textBoxSiteName;
    }
}