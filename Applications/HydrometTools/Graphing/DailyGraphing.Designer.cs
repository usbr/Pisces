namespace HydrometTools.Graphing
{
    partial class DailyGraphing
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
            Reclamation.Core.MonthDayRange monthDayRange1 = new Reclamation.Core.MonthDayRange();
            this.panel1 = new System.Windows.Forms.Panel();
            this.monthDayRangePicker1 = new Reclamation.TimeSeries.Forms.MonthDayRangePicker();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.timeSeriesGraph1 = new Reclamation.TimeSeries.Graphing.TimeSeriesTeeChartGraph();
            this.graphProperties1 = new HydrometTools.Graphing.DetailsComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.monthDayRangePicker1);
            this.panel1.Controls.Add(this.graphProperties1);
            this.panel1.Controls.Add(this.buttonRefresh);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(740, 87);
            this.panel1.TabIndex = 1;
            // 
            // monthDayRangePicker1
            // 
            this.monthDayRangePicker1.BeginningMonth = 10;
            this.monthDayRangePicker1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.monthDayRangePicker1.Location = new System.Drawing.Point(307, 3);
            this.monthDayRangePicker1.MonthDayRange = monthDayRange1;
            this.monthDayRangePicker1.Name = "monthDayRangePicker1";
            this.monthDayRangePicker1.Size = new System.Drawing.Size(157, 52);
            this.monthDayRangePicker1.TabIndex = 8;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(216, 58);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonRefresh.TabIndex = 5;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // timeSeriesGraph1
            // 
            this.timeSeriesGraph1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.timeSeriesGraph1.Location = new System.Drawing.Point(0, 87);
            this.timeSeriesGraph1.MissingDataValue = -999D;
            this.timeSeriesGraph1.Name = "timeSeriesGraph1";
            this.timeSeriesGraph1.Size = new System.Drawing.Size(740, 399);
            this.timeSeriesGraph1.SubTitle = "";
            this.timeSeriesGraph1.TabIndex = 2;
            this.timeSeriesGraph1.Title = "";
            // 
            // graphProperties1
            // 
            this.graphProperties1.Location = new System.Drawing.Point(3, 25);
            this.graphProperties1.Name = "graphProperties1";
            this.graphProperties1.Size = new System.Drawing.Size(214, 30);
            this.graphProperties1.TabIndex = 6;
            // 
            // DailyGraphing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.timeSeriesGraph1);
            this.Controls.Add(this.panel1);
            this.Name = "DailyGraphing";
            this.Size = new System.Drawing.Size(740, 486);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonRefresh;
        private DetailsComboBox graphProperties1;
        private Reclamation.TimeSeries.Graphing.TimeSeriesTeeChartGraph timeSeriesGraph1;
        private Reclamation.TimeSeries.Forms.MonthDayRangePicker monthDayRangePicker1;
    }
}
