namespace Reclamation.TimeSeries.Forms
{
    partial class WaterYearOptions
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.yearTypeSelector1 = new Reclamation.TimeSeries.Forms.YearTypeSelector();
            this.yearSelector1 = new Reclamation.TimeSeries.Forms.YearSelector();
            this.checkBox30Year = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(265, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Enter years separated by spaces i.e.   1977 2001 2005";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(290, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "you can enter a range of years using a dash like  1977-2001";
            // 
            // yearTypeSelector1
            // 
            this.yearTypeSelector1.BeginningMonth = 10;
            this.yearTypeSelector1.Location = new System.Drawing.Point(16, 79);
            this.yearTypeSelector1.Name = "yearTypeSelector1";
            this.yearTypeSelector1.Size = new System.Drawing.Size(295, 100);
            this.yearTypeSelector1.TabIndex = 4;
            // 
            // yearSelector1
            // 
            this.yearSelector1.Location = new System.Drawing.Point(16, 53);
            this.yearSelector1.Name = "yearSelector1";
            this.yearSelector1.Size = new System.Drawing.Size(379, 20);
            this.yearSelector1.TabIndex = 5;
            // 
            // checkBox30Year
            // 
            this.checkBox30Year.AutoSize = true;
            this.checkBox30Year.Location = new System.Drawing.Point(317, 90);
            this.checkBox30Year.Name = "checkBox30Year";
            this.checkBox30Year.Size = new System.Drawing.Size(103, 17);
            this.checkBox30Year.TabIndex = 6;
            this.checkBox30Year.Text = "30 year average";
            this.toolTip1.SetToolTip(this.checkBox30Year, "computes 30 year aveage based on available data for the last 30 years from todays" +
                    " date");
            this.checkBox30Year.UseVisualStyleBackColor = true;
            // 
            // WaterYearOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBox30Year);
            this.Controls.Add(this.yearSelector1);
            this.Controls.Add(this.yearTypeSelector1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "WaterYearOptions";
            this.Size = new System.Drawing.Size(426, 199);
            this.Leave += new System.EventHandler(this.WaterYearOptions_Leave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private YearTypeSelector yearTypeSelector1;
        private YearSelector yearSelector1;
        private System.Windows.Forms.CheckBox checkBox30Year;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
