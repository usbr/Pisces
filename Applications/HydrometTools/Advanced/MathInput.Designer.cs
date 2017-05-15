namespace HydrometTools
{
    partial class MathInput
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.buttonRunMath = new System.Windows.Forms.Button();
            this.textBoxPcodeIn = new System.Windows.Forms.TextBox();
            this.textBoxPcodeOut = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBoxArchiver = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBoxACE = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // textBoxCbtt
            // 
            this.textBoxCbtt.Location = new System.Drawing.Point(92, 69);
            this.textBoxCbtt.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxCbtt.Name = "textBoxCbtt";
            this.textBoxCbtt.Size = new System.Drawing.Size(117, 22);
            this.textBoxCbtt.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 73);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 17);
            this.label1.TabIndex = 7;
            this.label1.Text = "cbtt";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyyMMMdd HH:mm";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(64, 181);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(171, 22);
            this.dateTimePicker1.TabIndex = 3;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(21, 5);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(336, 56);
            this.textBox1.TabIndex = 6;
            this.textBox1.Text = "Enter range of dates, cbtt, pcode 1 - verified data, and pcode 2 - data to calcul" +
    "ate.";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "yyyyMMMdd HH:mm";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(64, 213);
            this.dateTimePicker2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(171, 22);
            this.dateTimePicker2.TabIndex = 4;
            // 
            // buttonRunMath
            // 
            this.buttonRunMath.Location = new System.Drawing.Point(269, 207);
            this.buttonRunMath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonRunMath.Name = "buttonRunMath";
            this.buttonRunMath.Size = new System.Drawing.Size(100, 28);
            this.buttonRunMath.TabIndex = 5;
            this.buttonRunMath.Text = "Run";
            this.buttonRunMath.UseVisualStyleBackColor = true;
            this.buttonRunMath.Click += new System.EventHandler(this.buttonRunMath_Click);
            // 
            // textBoxPcodeIn
            // 
            this.textBoxPcodeIn.Location = new System.Drawing.Point(92, 102);
            this.textBoxPcodeIn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxPcodeIn.Name = "textBoxPcodeIn";
            this.textBoxPcodeIn.Size = new System.Drawing.Size(117, 22);
            this.textBoxPcodeIn.TabIndex = 1;
            // 
            // textBoxPcodeOut
            // 
            this.textBoxPcodeOut.Location = new System.Drawing.Point(92, 135);
            this.textBoxPcodeOut.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxPcodeOut.Name = "textBoxPcodeOut";
            this.textBoxPcodeOut.Size = new System.Drawing.Size(117, 22);
            this.textBoxPcodeOut.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 106);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "pcode 1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 139);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "pcode 2";
            // 
            // checkBoxArchiver
            // 
            this.checkBoxArchiver.AutoSize = true;
            this.checkBoxArchiver.Location = new System.Drawing.Point(219, 157);
            this.checkBoxArchiver.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxArchiver.Name = "checkBoxArchiver";
            this.checkBoxArchiver.Size = new System.Drawing.Size(185, 21);
            this.checkBoxArchiver.TabIndex = 10;
            this.checkBoxArchiver.Text = "also compute daily value";
            this.checkBoxArchiver.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(263, 181);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "(run archiver)";
            // 
            // checkBoxACE
            // 
            this.checkBoxACE.AutoSize = true;
            this.checkBoxACE.Location = new System.Drawing.Point(220, 135);
            this.checkBoxACE.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxACE.Name = "checkBoxACE";
            this.checkBoxACE.Size = new System.Drawing.Size(115, 21);
            this.checkBoxACE.TabIndex = 12;
            this.checkBoxACE.Text = "use ace table";
            this.checkBoxACE.UseVisualStyleBackColor = true;
            // 
            // MathInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxACE);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.checkBoxArchiver);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxPcodeOut);
            this.Controls.Add(this.textBoxPcodeIn);
            this.Controls.Add(this.buttonRunMath);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxCbtt);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "MathInput";
            this.Size = new System.Drawing.Size(421, 255);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxCbtt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Button buttonRunMath;
        private System.Windows.Forms.TextBox textBoxPcodeIn;
        private System.Windows.Forms.TextBox textBoxPcodeOut;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBoxArchiver;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBoxACE;
    }
}
