namespace Reclamation.TimeSeries.Forms.Graphing
{
    partial class TestMeasurementPlotting
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
            this.ratingTableZedGraph1 = new Reclamation.TimeSeries.Graphing.RatingTableZedGraph();
            this.SuspendLayout();
            // 
            // ratingTableZedGraph1
            // 
            this.ratingTableZedGraph1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ratingTableZedGraph1.Location = new System.Drawing.Point(0, 0);
            this.ratingTableZedGraph1.Name = "ratingTableZedGraph1";
            this.ratingTableZedGraph1.Size = new System.Drawing.Size(615, 449);
            this.ratingTableZedGraph1.SubTitle = "";
            this.ratingTableZedGraph1.TabIndex = 0;
            this.ratingTableZedGraph1.Title = "";
            // 
            // TestMeasurementPlotting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 449);
            this.Controls.Add(this.ratingTableZedGraph1);
            this.Name = "TestMeasurementPlotting";
            this.Text = "TestMeasurementPlotting";
            this.ResumeLayout(false);

        }

        #endregion

        private TimeSeries.Graphing.RatingTableZedGraph ratingTableZedGraph1;
    }
}