namespace HydrometForecast
{
    partial class RegressionResults
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.linkLabelCompareHistoryOut = new System.Windows.Forms.LinkLabel();
            this.textBoxOutput = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelComputed = new System.Windows.Forms.Label();
            this.labelExisting = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.linkLabelCompareHistoryOut);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(567, 70);
            this.panel1.TabIndex = 0;
            // 
            // linkLabelCompareHistoryOut
            // 
            this.linkLabelCompareHistoryOut.AutoSize = true;
            this.linkLabelCompareHistoryOut.Location = new System.Drawing.Point(32, 9);
            this.linkLabelCompareHistoryOut.Name = "linkLabelCompareHistoryOut";
            this.linkLabelCompareHistoryOut.Size = new System.Drawing.Size(111, 13);
            this.linkLabelCompareHistoryOut.TabIndex = 1;
            this.linkLabelCompareHistoryOut.TabStop = true;
            this.linkLabelCompareHistoryOut.Text = "compare to history.out";
            this.linkLabelCompareHistoryOut.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelCompareHistoryOut_LinkClicked);
            // 
            // textBoxOutput
            // 
            this.textBoxOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOutput.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxOutput.Location = new System.Drawing.Point(0, 70);
            this.textBoxOutput.Multiline = true;
            this.textBoxOutput.Name = "textBoxOutput";
            this.textBoxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxOutput.Size = new System.Drawing.Size(567, 233);
            this.textBoxOutput.TabIndex = 1;
            this.textBoxOutput.WordWrap = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelComputed);
            this.panel2.Controls.Add(this.labelExisting);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.buttonCancel);
            this.panel2.Controls.Add(this.buttonOk);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 303);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(567, 87);
            this.panel2.TabIndex = 2;
            // 
            // labelComputed
            // 
            this.labelComputed.AutoSize = true;
            this.labelComputed.Location = new System.Drawing.Point(128, 45);
            this.labelComputed.Name = "labelComputed";
            this.labelComputed.Size = new System.Drawing.Size(35, 13);
            this.labelComputed.TabIndex = 7;
            this.labelComputed.Text = "label5";
            // 
            // labelExisting
            // 
            this.labelExisting.AutoSize = true;
            this.labelExisting.Location = new System.Drawing.Point(128, 29);
            this.labelExisting.Name = "labelExisting";
            this.labelExisting.Size = new System.Drawing.Size(35, 13);
            this.labelExisting.TabIndex = 6;
            this.labelExisting.Text = "label4";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(58, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "computed";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(58, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "existing";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "coefficients";
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(480, 52);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOk
            // 
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Location = new System.Drawing.Point(399, 52);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 1;
            this.buttonOk.Text = "Save";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // RegressionResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 390);
            this.Controls.Add(this.textBoxOutput);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "RegressionResults";
            this.Text = "RegressionResults";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxOutput;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label labelComputed;
        private System.Windows.Forms.Label labelExisting;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel linkLabelCompareHistoryOut;
    }
}