namespace Reclamation.TimeSeries.Forms.Graphing
{
    partial class ProfileZedGraph
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
            this.components = new System.ComponentModel.Container();
            this.chart1 = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            this.chart1.ScrollGrace = 0D;
            this.chart1.ScrollMaxX = 0D;
            this.chart1.ScrollMaxY = 0D;
            this.chart1.ScrollMaxY2 = 0D;
            this.chart1.ScrollMinX = 0D;
            this.chart1.ScrollMinY = 0D;
            this.chart1.ScrollMinY2 = 0D;
            this.chart1.Size = new System.Drawing.Size(582, 435);
            this.chart1.TabIndex = 0;
            // 
            // TemperatureProfileZedGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chart1);
            this.Name = "TemperatureProfileZedGraph";
            this.Size = new System.Drawing.Size(582, 435);
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl chart1;
    }
}
