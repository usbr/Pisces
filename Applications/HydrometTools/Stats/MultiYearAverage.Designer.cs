namespace HydrometTools.Stats
{
    partial class MultiYearAverage
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxCompareSingleYear = new System.Windows.Forms.TextBox();
            this.textBoxSingleYearCbtt = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxSingleYearPcode = new System.Windows.Forms.TextBox();
            this.buttonGo = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.compare = new HydrometTools.Stats.MultiWaterYearSelector();
            this.multi = new HydrometTools.Stats.MultiWaterYearSelector();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.textBoxCompareSingleYear);
            this.groupBox3.Controls.Add(this.textBoxSingleYearCbtt);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.textBoxSingleYearPcode);
            this.groupBox3.Location = new System.Drawing.Point(8, 306);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(210, 70);
            this.groupBox3.TabIndex = 42;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "comparison year";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(17, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(56, 13);
            this.label9.TabIndex = 36;
            this.label9.Text = "water year";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(106, 17);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(25, 13);
            this.label10.TabIndex = 37;
            this.label10.Text = "cbtt";
            // 
            // textBoxCompareSingleYear
            // 
            this.textBoxCompareSingleYear.Location = new System.Drawing.Point(19, 33);
            this.textBoxCompareSingleYear.Name = "textBoxCompareSingleYear";
            this.textBoxCompareSingleYear.Size = new System.Drawing.Size(52, 20);
            this.textBoxCompareSingleYear.TabIndex = 33;
            this.textBoxCompareSingleYear.Text = "1971";
            // 
            // textBoxSingleYearCbtt
            // 
            this.textBoxSingleYearCbtt.Location = new System.Drawing.Point(99, 33);
            this.textBoxSingleYearCbtt.Name = "textBoxSingleYearCbtt";
            this.textBoxSingleYearCbtt.Size = new System.Drawing.Size(52, 20);
            this.textBoxSingleYearCbtt.TabIndex = 34;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(155, 17);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(37, 13);
            this.label12.TabIndex = 38;
            this.label12.Text = "pcode";
            // 
            // textBoxSingleYearPcode
            // 
            this.textBoxSingleYearPcode.Location = new System.Drawing.Point(156, 34);
            this.textBoxSingleYearPcode.Name = "textBoxSingleYearPcode";
            this.textBoxSingleYearPcode.Size = new System.Drawing.Size(33, 20);
            this.textBoxSingleYearPcode.TabIndex = 35;
            this.textBoxSingleYearPcode.Text = "QU";
            // 
            // buttonGo
            // 
            this.buttonGo.Location = new System.Drawing.Point(14, 13);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(75, 23);
            this.buttonGo.TabIndex = 35;
            this.buttonGo.Text = "Refresh";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Location = new System.Drawing.Point(237, 37);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(353, 339);
            this.panel1.TabIndex = 46;
            // 
            // compare
            // 
            this.compare.Location = new System.Drawing.Point(8, 182);
            this.compare.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.compare.Name = "compare";
            this.compare.SetGroupBoxTitle = "compare years";
            this.compare.Size = new System.Drawing.Size(212, 119);
            this.compare.TabIndex = 45;
            // 
            // multi
            // 
            this.multi.Location = new System.Drawing.Point(8, 53);
            this.multi.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.multi.Name = "multi";
            this.multi.SetGroupBoxTitle = "compute multi year average";
            this.multi.Size = new System.Drawing.Size(212, 117);
            this.multi.TabIndex = 44;
            // 
            // MultiYearAverage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.compare);
            this.Controls.Add(this.multi);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.buttonGo);
            this.Name = "MultiYearAverage";
            this.Size = new System.Drawing.Size(612, 413);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBoxCompareSingleYear;
        private System.Windows.Forms.TextBox textBoxSingleYearCbtt;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxSingleYearPcode;
        private System.Windows.Forms.Button buttonGo;
        private MultiWaterYearSelector multi;
        private MultiWaterYearSelector compare;
        private System.Windows.Forms.Panel panel1;
    }
}
