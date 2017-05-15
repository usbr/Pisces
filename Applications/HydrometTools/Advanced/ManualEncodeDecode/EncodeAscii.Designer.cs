namespace HydrometTools.Advanced.ManualEncodeDecode
{
    partial class EncodeAscii
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
            this.textBoxResult = new System.Windows.Forms.TextBox();
            this.textBoxGroup3 = new System.Windows.Forms.TextBox();
            this.textBoxGroup2 = new System.Windows.Forms.TextBox();
            this.textBoxGroup1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelGroup1 = new System.Windows.Forms.Label();
            this.textBoxBinary = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDecimal = new System.Windows.Forms.TextBox();
            this.buttonDecimalToBinary = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxResult
            // 
            this.textBoxResult.Location = new System.Drawing.Point(128, 195);
            this.textBoxResult.Name = "textBoxResult";
            this.textBoxResult.Size = new System.Drawing.Size(77, 20);
            this.textBoxResult.TabIndex = 23;
            // 
            // textBoxGroup3
            // 
            this.textBoxGroup3.Location = new System.Drawing.Point(128, 160);
            this.textBoxGroup3.Name = "textBoxGroup3";
            this.textBoxGroup3.Size = new System.Drawing.Size(77, 20);
            this.textBoxGroup3.TabIndex = 22;
            // 
            // textBoxGroup2
            // 
            this.textBoxGroup2.Location = new System.Drawing.Point(128, 134);
            this.textBoxGroup2.Name = "textBoxGroup2";
            this.textBoxGroup2.Size = new System.Drawing.Size(77, 20);
            this.textBoxGroup2.TabIndex = 21;
            // 
            // textBoxGroup1
            // 
            this.textBoxGroup1.Location = new System.Drawing.Point(128, 105);
            this.textBoxGroup1.Name = "textBoxGroup1";
            this.textBoxGroup1.Size = new System.Drawing.Size(77, 20);
            this.textBoxGroup1.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(38, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "third 6 bits";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(38, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "second 6 bits";
            // 
            // labelGroup1
            // 
            this.labelGroup1.AutoSize = true;
            this.labelGroup1.Location = new System.Drawing.Point(38, 108);
            this.labelGroup1.Name = "labelGroup1";
            this.labelGroup1.Size = new System.Drawing.Size(51, 13);
            this.labelGroup1.TabIndex = 17;
            this.labelGroup1.Text = "first 6 bits";
            // 
            // textBoxBinary
            // 
            this.textBoxBinary.Location = new System.Drawing.Point(72, 60);
            this.textBoxBinary.Name = "textBoxBinary";
            this.textBoxBinary.Size = new System.Drawing.Size(316, 20);
            this.textBoxBinary.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "binary";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "decimal";
            // 
            // textBoxDecimal
            // 
            this.textBoxDecimal.Location = new System.Drawing.Point(72, 34);
            this.textBoxDecimal.Name = "textBoxDecimal";
            this.textBoxDecimal.Size = new System.Drawing.Size(120, 20);
            this.textBoxDecimal.TabIndex = 13;
            this.textBoxDecimal.Text = "572";
            // 
            // buttonDecimalToBinary
            // 
            this.buttonDecimalToBinary.Location = new System.Drawing.Point(198, 32);
            this.buttonDecimalToBinary.Name = "buttonDecimalToBinary";
            this.buttonDecimalToBinary.Size = new System.Drawing.Size(75, 23);
            this.buttonDecimalToBinary.TabIndex = 12;
            this.buttonDecimalToBinary.Text = "convert";
            this.buttonDecimalToBinary.UseVisualStyleBackColor = true;
            this.buttonDecimalToBinary.Click += new System.EventHandler(this.buttonDecimalToBinary_Click);
            // 
            // EncodeAscii
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxResult);
            this.Controls.Add(this.textBoxGroup3);
            this.Controls.Add(this.textBoxGroup2);
            this.Controls.Add(this.textBoxGroup1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelGroup1);
            this.Controls.Add(this.textBoxBinary);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxDecimal);
            this.Controls.Add(this.buttonDecimalToBinary);
            this.Name = "EncodeAscii";
            this.Size = new System.Drawing.Size(456, 297);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxResult;
        private System.Windows.Forms.TextBox textBoxGroup3;
        private System.Windows.Forms.TextBox textBoxGroup2;
        private System.Windows.Forms.TextBox textBoxGroup1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelGroup1;
        private System.Windows.Forms.TextBox textBoxBinary;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDecimal;
        private System.Windows.Forms.Button buttonDecimalToBinary;
    }
}
