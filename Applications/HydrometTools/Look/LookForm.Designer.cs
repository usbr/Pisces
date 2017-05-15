namespace Look
{
    partial class LookForm
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
            this.newLook1 = new Look.NewLook();
            this.SuspendLayout();
            // 
            // newLook1
            // 
            this.newLook1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.newLook1.Location = new System.Drawing.Point(0, 0);
            this.newLook1.Name = "newLook1";
            this.newLook1.Size = new System.Drawing.Size(839, 729);
            this.newLook1.TabIndex = 0;
            // 
            // LookForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 729);
            this.Controls.Add(this.newLook1);
            this.Name = "LookForm";
            this.Text = "Site Search";
            this.ResumeLayout(false);

        }

        #endregion

        private NewLook newLook1;



    }
}

