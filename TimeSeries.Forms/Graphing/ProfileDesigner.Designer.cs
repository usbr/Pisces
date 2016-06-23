namespace Reclamation.TimeSeries.Forms.Graphing
{
    partial class ProfileDesigner
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProfileDesigner));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBoxSeries = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxXlabel = new System.Windows.Forms.TextBox();
            this.textboxYlabel = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxYmax = new System.Windows.Forms.TextBox();
            this.textBoxYmin = new System.Windows.Forms.TextBox();
            this.textBoxXmax = new System.Windows.Forms.TextBox();
            this.textBoxXmin = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonForward = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonApply = new System.Windows.Forms.Button();
            this.textBoxWaterSurface = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.buttonStop = new System.Windows.Forms.Button();
            this.chart1 = new Reclamation.TimeSeries.Forms.Graphing.ProfileZedGraph();
            this.timeSelectorBeginEnd1 = new Reclamation.TimeSeries.Forms.TimeSelectorBeginEnd();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(887, 62);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "This tool generates an animated profile plot for the range of dates.  \r\nInputs ar" +
    "e pairs of named time series data (already in the Pisces tree).";
            // 
            // textBoxSeries
            // 
            this.textBoxSeries.Location = new System.Drawing.Point(11, 99);
            this.textBoxSeries.Multiline = true;
            this.textBoxSeries.Name = "textBoxSeries";
            this.textBoxSeries.Size = new System.Drawing.Size(237, 167);
            this.textBoxSeries.TabIndex = 1;
            this.textBoxSeries.Text = resources.GetString("textBoxSeries.Text");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "enter x-axis series, y-axis series (depth)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "x";
            // 
            // textBoxXlabel
            // 
            this.textBoxXlabel.Location = new System.Drawing.Point(46, 53);
            this.textBoxXlabel.Name = "textBoxXlabel";
            this.textBoxXlabel.Size = new System.Drawing.Size(113, 20);
            this.textBoxXlabel.TabIndex = 6;
            this.textBoxXlabel.Text = "water temperature";
            // 
            // textboxYlabel
            // 
            this.textboxYlabel.Location = new System.Drawing.Point(46, 80);
            this.textboxYlabel.Name = "textboxYlabel";
            this.textboxYlabel.Size = new System.Drawing.Size(113, 20);
            this.textboxYlabel.TabIndex = 7;
            this.textboxYlabel.Text = "elevation (feet)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "y";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(69, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "label";
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Location = new System.Drawing.Point(46, 32);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(113, 20);
            this.textBoxTitle.TabIndex = 9;
            this.textBoxTitle.Text = "Grand Coulee";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxYmax);
            this.groupBox1.Controls.Add(this.textBoxYmin);
            this.groupBox1.Controls.Add(this.textBoxXmax);
            this.groupBox1.Controls.Add(this.textBoxXmin);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxTitle);
            this.groupBox1.Controls.Add(this.textBoxXlabel);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textboxYlabel);
            this.groupBox1.Location = new System.Drawing.Point(11, 314);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(282, 117);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "formatting";
            // 
            // textBoxYmax
            // 
            this.textBoxYmax.Location = new System.Drawing.Point(223, 80);
            this.textBoxYmax.Name = "textBoxYmax";
            this.textBoxYmax.Size = new System.Drawing.Size(51, 20);
            this.textBoxYmax.TabIndex = 17;
            this.textBoxYmax.Text = "1300";
            // 
            // textBoxYmin
            // 
            this.textBoxYmin.Location = new System.Drawing.Point(166, 80);
            this.textBoxYmin.Name = "textBoxYmin";
            this.textBoxYmin.Size = new System.Drawing.Size(51, 20);
            this.textBoxYmin.TabIndex = 16;
            this.textBoxYmin.Text = "950";
            // 
            // textBoxXmax
            // 
            this.textBoxXmax.Location = new System.Drawing.Point(223, 50);
            this.textBoxXmax.Name = "textBoxXmax";
            this.textBoxXmax.Size = new System.Drawing.Size(51, 20);
            this.textBoxXmax.TabIndex = 15;
            this.textBoxXmax.Text = "20";
            // 
            // textBoxXmin
            // 
            this.textBoxXmin.Location = new System.Drawing.Point(166, 51);
            this.textBoxXmin.Name = "textBoxXmin";
            this.textBoxXmin.Size = new System.Drawing.Size(51, 20);
            this.textBoxXmin.TabIndex = 14;
            this.textBoxXmin.Text = "2";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(233, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(26, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "max";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(174, 35);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "min";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "title";
            // 
            // buttonForward
            // 
            this.buttonForward.Location = new System.Drawing.Point(262, 486);
            this.buttonForward.Name = "buttonForward";
            this.buttonForward.Size = new System.Drawing.Size(59, 23);
            this.buttonForward.TabIndex = 13;
            this.buttonForward.Text = "forward>";
            this.toolTip1.SetToolTip(this.buttonForward, "plots the next day of data");
            this.buttonForward.UseVisualStyleBackColor = true;
            this.buttonForward.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(196, 515);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(68, 23);
            this.buttonSave.TabIndex = 14;
            this.buttonSave.Text = "animate";
            this.toolTip1.SetToolTip(this.buttonSave, "saves animation to a file");
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(197, 486);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(59, 23);
            this.buttonBack.TabIndex = 15;
            this.buttonBack.Text = "<back";
            this.toolTip1.SetToolTip(this.buttonBack, "plots the next day of data");
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(234, 442);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(75, 23);
            this.buttonApply.TabIndex = 16;
            this.buttonApply.Text = "apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // textBoxWaterSurface
            // 
            this.textBoxWaterSurface.Location = new System.Drawing.Point(124, 279);
            this.textBoxWaterSurface.Name = "textBoxWaterSurface";
            this.textBoxWaterSurface.Size = new System.Drawing.Size(100, 20);
            this.textBoxWaterSurface.TabIndex = 17;
            this.textBoxWaterSurface.Text = "gcl_fb_daily";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 282);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "water surface series:";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(262, 515);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(68, 23);
            this.buttonStop.TabIndex = 19;
            this.buttonStop.Text = "stop";
            this.toolTip1.SetToolTip(this.buttonStop, "saves animation to a file");
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // chart1
            // 
            this.chart1.Dock = System.Windows.Forms.DockStyle.Right;
            this.chart1.Location = new System.Drawing.Point(337, 62);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(550, 573);
            this.chart1.TabIndex = 12;
            // 
            // timeSelectorBeginEnd1
            // 
            this.timeSelectorBeginEnd1.Location = new System.Drawing.Point(0, 486);
            this.timeSelectorBeginEnd1.Name = "timeSelectorBeginEnd1";
            this.timeSelectorBeginEnd1.ShowTime = false;
            this.timeSelectorBeginEnd1.Size = new System.Drawing.Size(213, 46);
            this.timeSelectorBeginEnd1.T1 = new System.DateTime(2005, 1, 1, 10, 3, 0, 0);
            this.timeSelectorBeginEnd1.T2 = new System.DateTime(2016, 1, 22, 10, 3, 0, 0);
            this.timeSelectorBeginEnd1.TabIndex = 4;
            // 
            // ProfileDesigner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(887, 635);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxWaterSurface);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonForward);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.timeSelectorBeginEnd1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxSeries);
            this.Controls.Add(this.textBox1);
            this.Name = "ProfileDesigner";
            this.Text = "Pisces Profile Tool";
            this.Load += new System.EventHandler(this.ProfileDesigner_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBoxSeries;
        private System.Windows.Forms.Label label2;
        private TimeSelectorBeginEnd timeSelectorBeginEnd1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxXlabel;
        private System.Windows.Forms.TextBox textboxYlabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxYmax;
        private System.Windows.Forms.TextBox textBoxYmin;
        private System.Windows.Forms.TextBox textBoxXmax;
        private System.Windows.Forms.TextBox textBoxXmin;
        private ProfileZedGraph chart1;
        private System.Windows.Forms.Button buttonForward;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.TextBox textBoxWaterSurface;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button buttonStop;
    }
}