namespace Reclamation.TimeSeries.Forms.RatingTables
{
    partial class RatingTableEditor
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radioButtonlog = new System.Windows.Forms.RadioButton();
            this.radioButtonPolynomial = new System.Windows.Forms.RadioButton();
            this.numericUpDownPolynomialOrder = new System.Windows.Forms.NumericUpDown();
            this.checkBoxZeroIntercept = new System.Windows.Forms.CheckBox();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonCompute = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxNote = new System.Windows.Forms.TextBox();
            this.textBoxEquation = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPolynomialOrder)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.checkBoxZeroIntercept);
            this.groupBox1.Controls.Add(this.textBoxName);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.buttonCompute);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxNote);
            this.groupBox1.Controls.Add(this.textBoxEquation);
            this.groupBox1.Location = new System.Drawing.Point(13, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(304, 421);
            this.groupBox1.TabIndex = 96;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rating Details";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.radioButtonlog);
            this.groupBox2.Controls.Add(this.radioButtonPolynomial);
            this.groupBox2.Controls.Add(this.numericUpDownPolynomialOrder);
            this.groupBox2.Location = new System.Drawing.Point(11, 40);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(260, 79);
            this.groupBox2.TabIndex = 106;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "curve fit type";
            // 
            // radioButtonlog
            // 
            this.radioButtonlog.AutoSize = true;
            this.radioButtonlog.Location = new System.Drawing.Point(12, 45);
            this.radioButtonlog.Name = "radioButtonlog";
            this.radioButtonlog.Size = new System.Drawing.Size(77, 17);
            this.radioButtonlog.TabIndex = 86;
            this.radioButtonlog.Text = "log,  type =";
            this.radioButtonlog.UseVisualStyleBackColor = true;
            // 
            // radioButtonPolynomial
            // 
            this.radioButtonPolynomial.AutoSize = true;
            this.radioButtonPolynomial.Checked = true;
            this.radioButtonPolynomial.Location = new System.Drawing.Point(12, 21);
            this.radioButtonPolynomial.Name = "radioButtonPolynomial";
            this.radioButtonPolynomial.Size = new System.Drawing.Size(113, 17);
            this.radioButtonPolynomial.TabIndex = 0;
            this.radioButtonPolynomial.TabStop = true;
            this.radioButtonPolynomial.Text = "polynomial, order =";
            this.radioButtonPolynomial.UseVisualStyleBackColor = true;
            // 
            // numericUpDownPolynomialOrder
            // 
            this.numericUpDownPolynomialOrder.Location = new System.Drawing.Point(146, 18);
            this.numericUpDownPolynomialOrder.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDownPolynomialOrder.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownPolynomialOrder.Name = "numericUpDownPolynomialOrder";
            this.numericUpDownPolynomialOrder.Size = new System.Drawing.Size(56, 20);
            this.numericUpDownPolynomialOrder.TabIndex = 85;
            this.numericUpDownPolynomialOrder.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // checkBoxZeroIntercept
            // 
            this.checkBoxZeroIntercept.AutoSize = true;
            this.checkBoxZeroIntercept.Location = new System.Drawing.Point(23, 242);
            this.checkBoxZeroIntercept.Name = "checkBoxZeroIntercept";
            this.checkBoxZeroIntercept.Size = new System.Drawing.Size(102, 17);
            this.checkBoxZeroIntercept.TabIndex = 105;
            this.checkBoxZeroIntercept.Text = "set intercept = 0";
            this.checkBoxZeroIntercept.UseVisualStyleBackColor = true;
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.Location = new System.Drawing.Point(52, 15);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(208, 20);
            this.textBoxName.TabIndex = 101;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(8, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 12);
            this.label7.TabIndex = 100;
            this.label7.Text = "name";
            // 
            // buttonCompute
            // 
            this.buttonCompute.Location = new System.Drawing.Point(11, 377);
            this.buttonCompute.Name = "buttonCompute";
            this.buttonCompute.Size = new System.Drawing.Size(75, 23);
            this.buttonCompute.TabIndex = 96;
            this.buttonCompute.Text = "Compute";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 16);
            this.label4.TabIndex = 92;
            this.label4.Text = "notes";
            // 
            // textBoxNote
            // 
            this.textBoxNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNote.Location = new System.Drawing.Point(11, 141);
            this.textBoxNote.Multiline = true;
            this.textBoxNote.Name = "textBoxNote";
            this.textBoxNote.Size = new System.Drawing.Size(242, 63);
            this.textBoxNote.TabIndex = 91;
            // 
            // textBoxEquation
            // 
            this.textBoxEquation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEquation.Location = new System.Drawing.Point(11, 295);
            this.textBoxEquation.Name = "textBoxEquation";
            this.textBoxEquation.ReadOnly = true;
            this.textBoxEquation.Size = new System.Drawing.Size(280, 20);
            this.textBoxEquation.TabIndex = 99;
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(324, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(366, 302);
            this.panel1.TabIndex = 97;
            // 
            // RatingTableEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "RatingTableEditor";
            this.Size = new System.Drawing.Size(744, 552);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPolynomialOrder)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButtonlog;
        private System.Windows.Forms.RadioButton radioButtonPolynomial;
        private System.Windows.Forms.NumericUpDown numericUpDownPolynomialOrder;
        private System.Windows.Forms.CheckBox checkBoxZeroIntercept;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonCompute;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxNote;
        private System.Windows.Forms.TextBox textBoxEquation;
        private System.Windows.Forms.Panel panel1;
    }
}
