namespace Depth
{
    partial class RatingInputs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RatingInputs));
            this.textBoxPoints = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxPoints
            // 
            this.textBoxPoints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxPoints.Location = new System.Drawing.Point(0, 0);
            this.textBoxPoints.Multiline = true;
            this.textBoxPoints.Name = "textBoxPoints";
            this.textBoxPoints.Size = new System.Drawing.Size(330, 553);
            this.textBoxPoints.TabIndex = 0;
            this.textBoxPoints.Text = resources.GetString("textBoxPoints.Text");
            // 
            // RatingInputs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.textBoxPoints);
            this.Name = "RatingInputs";
            this.Size = new System.Drawing.Size(330, 553);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxPoints;
    }
}
