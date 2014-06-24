namespace Reclamation.TimeSeries.Forms
{
    partial class SeriesListSumForm
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
            this.CancelButton1 = new System.Windows.Forms.Button();
            this.sumButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.sumTimeSelector = new Reclamation.TimeSeries.Forms.TimeSelectorBeginEnd();
            this.ListOfSeries = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // CancelButton1
            // 
            this.CancelButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButton1.Location = new System.Drawing.Point(191, 294);
            this.CancelButton1.Name = "CancelButton1";
            this.CancelButton1.Size = new System.Drawing.Size(75, 23);
            this.CancelButton1.TabIndex = 14;
            this.CancelButton1.Text = "Cancel";
            this.CancelButton1.Click += new System.EventHandler(this.button1Cancel_Click);
            // 
            // sumButton
            // 
            this.sumButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.sumButton.Location = new System.Drawing.Point(191, 265);
            this.sumButton.Name = "sumButton";
            this.sumButton.Size = new System.Drawing.Size(75, 23);
            this.sumButton.TabIndex = 13;
            this.sumButton.Text = "OK";
            this.sumButton.Click += new System.EventHandler(this.buttonSum_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(202, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 26);
            this.label3.TabIndex = 19;
            this.label3.Text = "Series to be\r\n  Summed";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(173, 225);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "New Series Name";
            // 
            // textBox1
            // 
            this.textBox1.AcceptsReturn = true;
            this.textBox1.AcceptsTab = true;
            this.textBox1.Location = new System.Drawing.Point(32, 222);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(135, 20);
            this.textBox1.TabIndex = 23;
            this.textBox1.WordWrap = false;
            // 
            // sumTimeSelector
            // 
            this.sumTimeSelector.T1 = new System.DateTime(2003, 7, 10, 12, 34, 9, 921);
            this.sumTimeSelector.T2 = new System.DateTime(2003, 7, 10, 12, 34, 9, 921);
            this.sumTimeSelector.Location = new System.Drawing.Point(32, 150);
            this.sumTimeSelector.Name = "sumTimeSelector";
            this.sumTimeSelector.ShowTime = false;
            this.sumTimeSelector.Size = new System.Drawing.Size(192, 48);
            this.sumTimeSelector.TabIndex = 24;
            // 
            // ListOfSeries
            // 
            this.ListOfSeries.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ListOfSeries.ForeColor = System.Drawing.SystemColors.WindowText;
            this.ListOfSeries.FormattingEnabled = true;
            this.ListOfSeries.HorizontalScrollbar = true;
            this.ListOfSeries.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ListOfSeries.Location = new System.Drawing.Point(32, 10);
            this.ListOfSeries.Name = "ListOfSeries";
            this.ListOfSeries.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.ListOfSeries.Size = new System.Drawing.Size(165, 95);
            this.ListOfSeries.TabIndex = 25;
            // 
            // SeriesListSumForm
            // 
            this.AcceptButton = this.sumButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 325);
            this.Controls.Add(this.ListOfSeries);
            this.Controls.Add(this.sumTimeSelector);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CancelButton1);
            this.Controls.Add(this.sumButton);
            this.Name = "SeriesListSumForm";
            this.Text = "Selected Series Summation";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CancelButton1;
        private System.Windows.Forms.Button sumButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private TimeSelectorBeginEnd sumTimeSelector;
        private System.Windows.Forms.ListBox ListOfSeries;
    }
}