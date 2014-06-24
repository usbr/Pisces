namespace Reclamation.TimeSeries.Forms
{
    partial class ScenarioReferenceControl
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
            this.groupBoxComparisonn = new System.Windows.Forms.GroupBox();
            this.checkBoxIncludeSelected = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeReference = new System.Windows.Forms.CheckBox();
            this.checkBoxSubtractFromReference = new System.Windows.Forms.CheckBox();
            this.groupBoxComparisonn.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxComparisonn
            // 
            this.groupBoxComparisonn.Controls.Add(this.checkBoxIncludeSelected);
            this.groupBoxComparisonn.Controls.Add(this.checkBoxIncludeReference);
            this.groupBoxComparisonn.Controls.Add(this.checkBoxSubtractFromReference);
            this.groupBoxComparisonn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBoxComparisonn.Location = new System.Drawing.Point(3, 3);
            this.groupBoxComparisonn.Name = "groupBoxComparisonn";
            this.groupBoxComparisonn.Size = new System.Drawing.Size(137, 79);
            this.groupBoxComparisonn.TabIndex = 765;
            this.groupBoxComparisonn.TabStop = false;
            this.groupBoxComparisonn.Text = "comparison";
            // 
            // checkBoxIncludeSelected
            // 
            this.checkBoxIncludeSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxIncludeSelected.AutoSize = true;
            this.checkBoxIncludeSelected.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBoxIncludeSelected.Location = new System.Drawing.Point(7, 50);
            this.checkBoxIncludeSelected.Name = "checkBoxIncludeSelected";
            this.checkBoxIncludeSelected.Size = new System.Drawing.Size(103, 17);
            this.checkBoxIncludeSelected.TabIndex = 20;
            this.checkBoxIncludeSelected.Text = "include selected";
            this.checkBoxIncludeSelected.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeReference
            // 
            this.checkBoxIncludeReference.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxIncludeReference.AutoSize = true;
            this.checkBoxIncludeReference.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBoxIncludeReference.Location = new System.Drawing.Point(7, 33);
            this.checkBoxIncludeReference.Name = "checkBoxIncludeReference";
            this.checkBoxIncludeReference.Size = new System.Drawing.Size(108, 17);
            this.checkBoxIncludeReference.TabIndex = 19;
            this.checkBoxIncludeReference.Text = "include reference";
            this.checkBoxIncludeReference.UseVisualStyleBackColor = true;
            // 
            // checkBoxSubtractFromReference
            // 
            this.checkBoxSubtractFromReference.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxSubtractFromReference.AutoSize = true;
            this.checkBoxSubtractFromReference.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.checkBoxSubtractFromReference.Location = new System.Drawing.Point(7, 16);
            this.checkBoxSubtractFromReference.Name = "checkBoxSubtractFromReference";
            this.checkBoxSubtractFromReference.Size = new System.Drawing.Size(112, 17);
            this.checkBoxSubtractFromReference.TabIndex = 18;
            this.checkBoxSubtractFromReference.Text = "subtract reference";
            this.checkBoxSubtractFromReference.UseVisualStyleBackColor = true;
            // 
            // ScenarioReferenceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxComparisonn);
            this.Name = "ScenarioReferenceControl";
            this.Size = new System.Drawing.Size(145, 88);
            this.groupBoxComparisonn.ResumeLayout(false);
            this.groupBoxComparisonn.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxComparisonn;
        private System.Windows.Forms.CheckBox checkBoxIncludeSelected;
        private System.Windows.Forms.CheckBox checkBoxIncludeReference;
        private System.Windows.Forms.CheckBox checkBoxSubtractFromReference;
    }
}
