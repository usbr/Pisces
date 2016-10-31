namespace Reclamation.TimeSeries.Forms.RatingTables
{
    partial class RatingEquationBuilder
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
            this.panelGraph = new System.Windows.Forms.Panel();
            this.textBoxEquation = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPolynomialOrder)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.textBoxEquation);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.checkBoxZeroIntercept);
            this.groupBox1.Controls.Add(this.textBoxName);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.buttonCompute);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxNote);
            this.groupBox1.Location = new System.Drawing.Point(13, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(285, 493);
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
            this.radioButtonlog.Checked = true;
            this.radioButtonlog.Location = new System.Drawing.Point(12, 19);
            this.radioButtonlog.Name = "radioButtonlog";
            this.radioButtonlog.Size = new System.Drawing.Size(39, 17);
            this.radioButtonlog.TabIndex = 86;
            this.radioButtonlog.TabStop = true;
            this.radioButtonlog.Text = "log";
            this.radioButtonlog.UseVisualStyleBackColor = true;
            // 
            // radioButtonPolynomial
            // 
            this.radioButtonPolynomial.AutoSize = true;
            this.radioButtonPolynomial.Location = new System.Drawing.Point(12, 43);
            this.radioButtonPolynomial.Name = "radioButtonPolynomial";
            this.radioButtonPolynomial.Size = new System.Drawing.Size(113, 17);
            this.radioButtonPolynomial.TabIndex = 0;
            this.radioButtonPolynomial.Text = "polynomial, order =";
            this.radioButtonPolynomial.UseVisualStyleBackColor = true;
            // 
            // numericUpDownPolynomialOrder
            // 
            this.numericUpDownPolynomialOrder.Location = new System.Drawing.Point(148, 40);
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
            this.checkBoxZeroIntercept.Location = new System.Drawing.Point(11, 258);
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
            this.textBoxName.Size = new System.Drawing.Size(189, 20);
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
            this.buttonCompute.Location = new System.Drawing.Point(11, 281);
            this.buttonCompute.Name = "buttonCompute";
            this.buttonCompute.Size = new System.Drawing.Size(75, 23);
            this.buttonCompute.TabIndex = 96;
            this.buttonCompute.Text = "Compute";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(8, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 16);
            this.label4.TabIndex = 92;
            this.label4.Text = "notes";
            // 
            // textBoxNote
            // 
            this.textBoxNote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNote.Location = new System.Drawing.Point(11, 189);
            this.textBoxNote.Multiline = true;
            this.textBoxNote.Name = "textBoxNote";
            this.textBoxNote.Size = new System.Drawing.Size(241, 63);
            this.textBoxNote.TabIndex = 91;
            // 
            // panelGraph
            // 
            this.panelGraph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelGraph.Location = new System.Drawing.Point(304, 15);
            this.panelGraph.Name = "panelGraph";
            this.panelGraph.Size = new System.Drawing.Size(597, 521);
            this.panelGraph.TabIndex = 97;
            // 
            // textBoxEquation
            // 
            this.textBoxEquation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEquation.Location = new System.Drawing.Point(17, 140);
            this.textBoxEquation.Name = "textBoxEquation";
            this.textBoxEquation.Size = new System.Drawing.Size(235, 20);
            this.textBoxEquation.TabIndex = 107;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 17);
            this.label1.TabIndex = 108;
            this.label1.Text = "equation:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(11, 329);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 109;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // RatingEquationBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelGraph);
            this.Controls.Add(this.groupBox1);
            this.Name = "RatingEquationBuilder";
            this.Size = new System.Drawing.Size(919, 557);
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
        private System.Windows.Forms.Panel panelGraph;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxEquation;
        private System.Windows.Forms.Button button1;
    }
}
