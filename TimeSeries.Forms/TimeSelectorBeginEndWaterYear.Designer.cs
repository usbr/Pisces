namespace Reclamation.TimeSeries.Forms
{
    partial class TimeSelectorBeginEndWaterYear
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
            this.textBoxT1 = new System.Windows.Forms.TextBox();
            this.textBoxT2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxT1
            // 
            this.textBoxT1.Location = new System.Drawing.Point(45, 9);
            this.textBoxT1.Name = "textBoxT1";
            this.textBoxT1.Size = new System.Drawing.Size(72, 20);
            this.textBoxT1.TabIndex = 0;
            // 
            // textBoxT2
            // 
            this.textBoxT2.Location = new System.Drawing.Point(45, 32);
            this.textBoxT2.Name = "textBoxT2";
            this.textBoxT2.Size = new System.Drawing.Size(72, 20);
            this.textBoxT2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 16);
            this.label2.TabIndex = 26;
            this.label2.Text = "End";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 16);
            this.label1.TabIndex = 25;
            this.label1.Text = "Start";
            // 
            // TimeSelectorBeginEndWaterYear
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxT2);
            this.Controls.Add(this.textBoxT1);
            this.Name = "TimeSelectorBeginEndWaterYear";
            this.Size = new System.Drawing.Size(131, 62);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxT1;
        private System.Windows.Forms.TextBox textBoxT2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
