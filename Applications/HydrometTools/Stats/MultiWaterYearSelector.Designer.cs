namespace HydrometTools.Stats
{
    partial class MultiWaterYearSelector
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelYearCount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxYear = new System.Windows.Forms.TextBox();
            this.textBoxCbtt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxEndYear = new System.Windows.Forms.TextBox();
            this.textBoxPcode = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelYearCount);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxYear);
            this.groupBox1.Controls.Add(this.textBoxCbtt);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxEndYear);
            this.groupBox1.Controls.Add(this.textBoxPcode);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(212, 138);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "inputs";
            // 
            // labelYearCount
            // 
            this.labelYearCount.AutoSize = true;
            this.labelYearCount.Location = new System.Drawing.Point(82, 92);
            this.labelYearCount.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelYearCount.Name = "labelYearCount";
            this.labelYearCount.Size = new System.Drawing.Size(47, 13);
            this.labelYearCount.TabIndex = 33;
            this.labelYearCount.Text = "10 years";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "start water year";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(93, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 28;
            this.label2.Text = "cbtt";
            // 
            // textBoxYear
            // 
            this.textBoxYear.Location = new System.Drawing.Point(6, 49);
            this.textBoxYear.Name = "textBoxYear";
            this.textBoxYear.Size = new System.Drawing.Size(52, 20);
            this.textBoxYear.TabIndex = 0;
            this.textBoxYear.Text = "1981";
            this.textBoxYear.TextChanged += new System.EventHandler(this.updateYearLabel);
            // 
            // textBoxCbtt
            // 
            this.textBoxCbtt.Location = new System.Drawing.Point(86, 49);
            this.textBoxCbtt.Name = "textBoxCbtt";
            this.textBoxCbtt.Size = new System.Drawing.Size(52, 20);
            this.textBoxCbtt.TabIndex = 1;
            this.textBoxCbtt.TextChanged += new System.EventHandler(this.updateYearLabel);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 32;
            this.label4.Text = "end water year";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(142, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 30;
            this.label3.Text = "pcode";
            // 
            // textBoxEndYear
            // 
            this.textBoxEndYear.Location = new System.Drawing.Point(6, 90);
            this.textBoxEndYear.Name = "textBoxEndYear";
            this.textBoxEndYear.Size = new System.Drawing.Size(52, 20);
            this.textBoxEndYear.TabIndex = 31;
            this.textBoxEndYear.Text = "2010";
            this.textBoxEndYear.TextChanged += new System.EventHandler(this.updateYearLabel);
            // 
            // textBoxPcode
            // 
            this.textBoxPcode.Location = new System.Drawing.Point(143, 50);
            this.textBoxPcode.Name = "textBoxPcode";
            this.textBoxPcode.Size = new System.Drawing.Size(33, 20);
            this.textBoxPcode.TabIndex = 2;
            this.textBoxPcode.Text = "QU";
            // 
            // MultiWaterYearSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MultiWaterYearSelector";
            this.Size = new System.Drawing.Size(212, 138);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxYear;
        private System.Windows.Forms.TextBox textBoxCbtt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxEndYear;
        private System.Windows.Forms.TextBox textBoxPcode;
        private System.Windows.Forms.Label labelYearCount;
    }
}
