namespace Reclamation.TimeSeries.Forms
{
    partial class ExceedanceGraphOptions
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
            this.aggregateOptions1 = new Reclamation.TimeSeries.Forms.AggregateOption();
            this.SuspendLayout();
            // 
            // aggregateOptions1
            // 
            this.aggregateOptions1.Location = new System.Drawing.Point(17, 14);
            this.aggregateOptions1.Name = "aggregateOptions1";
            this.aggregateOptions1.Size = new System.Drawing.Size(189, 151);
            this.aggregateOptions1.TabIndex = 8;
            // 
            // ExceedanceGraphOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.aggregateOptions1);
            this.Name = "ExceedanceGraphOptions";
            this.Size = new System.Drawing.Size(580, 287);
            this.ResumeLayout(false);

        }

        #endregion

        private AggregateOption aggregateOptions1;
    }
}
