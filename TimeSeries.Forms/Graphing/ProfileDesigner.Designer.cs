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
            this.buttonSave = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.buttonStep = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.textBoxWaterSurface = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBoxOutputFile = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxspeed = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.textBoxStatic1Label = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxStatic2Label = new System.Windows.Forms.TextBox();
            this.textBoxStatic1Value = new System.Windows.Forms.TextBox();
            this.textBoxStatic2Value = new System.Windows.Forms.TextBox();
            this.chart1 = new Reclamation.TimeSeries.Forms.Graphing.ProfileZedGraph();
            this.timeSelector1 = new Reclamation.TimeSeries.Forms.TimeSelectorBeginEnd();
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
            this.textBoxXlabel.Text = "water temperature (celsius)";
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
            this.groupBox1.Location = new System.Drawing.Point(11, 349);
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
            this.textBoxXmax.Text = "30";
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
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(188, 515);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(68, 23);
            this.buttonSave.TabIndex = 14;
            this.buttonSave.Text = "animate";
            this.toolTip1.SetToolTip(this.buttonSave, "saves animation to a file");
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonStep
            // 
            this.buttonStep.Location = new System.Drawing.Point(188, 486);
            this.buttonStep.Name = "buttonStep";
            this.buttonStep.Size = new System.Drawing.Size(68, 23);
            this.buttonStep.TabIndex = 22;
            this.buttonStep.Text = "step";
            this.toolTip1.SetToolTip(this.buttonStep, "saves animation to a file");
            this.buttonStep.UseVisualStyleBackColor = true;
            this.buttonStep.Click += new System.EventHandler(this.buttonStep_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(263, 515);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(68, 23);
            this.buttonStop.TabIndex = 23;
            this.buttonStop.Text = "stop";
            this.toolTip1.SetToolTip(this.buttonStop, "saves animation to a file");
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click_1);
            // 
            // textBoxWaterSurface
            // 
            this.textBoxWaterSurface.Location = new System.Drawing.Point(127, 279);
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
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBoxOutputFile
            // 
            this.textBoxOutputFile.Location = new System.Drawing.Point(12, 582);
            this.textBoxOutputFile.Name = "textBoxOutputFile";
            this.textBoxOutputFile.Size = new System.Drawing.Size(311, 20);
            this.textBoxOutputFile.TabIndex = 20;
            this.textBoxOutputFile.Text = "c:\\temp\\temp.gif";
            this.textBoxOutputFile.Enabled = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(-345, 802);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(104, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "speed (milli seconds)";
            // 
            // textBoxspeed
            // 
            this.textBoxspeed.Location = new System.Drawing.Point(163, 547);
            this.textBoxspeed.Name = "textBoxspeed";
            this.textBoxspeed.Size = new System.Drawing.Size(37, 20);
            this.textBoxspeed.TabIndex = 24;
            this.textBoxspeed.Text = "3";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(32, 550);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(116, 13);
            this.label10.TabIndex = 25;
            this.label10.Text = "speed (frames/second)";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 302);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 13);
            this.label11.TabIndex = 27;
            this.label11.Text = "static line1";
            // 
            // textBoxStatic1Label
            // 
            this.textBoxStatic1Label.Location = new System.Drawing.Point(188, 299);
            this.textBoxStatic1Label.Name = "textBoxStatic1Label";
            this.textBoxStatic1Label.Size = new System.Drawing.Size(129, 20);
            this.textBoxStatic1Label.TabIndex = 26;
            this.textBoxStatic1Label.Text = "third  power plant intake";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 320);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(57, 13);
            this.label12.TabIndex = 29;
            this.label12.Text = "static line2";
            // 
            // textBoxStatic2Label
            // 
            this.textBoxStatic2Label.Location = new System.Drawing.Point(188, 323);
            this.textBoxStatic2Label.Name = "textBoxStatic2Label";
            this.textBoxStatic2Label.Size = new System.Drawing.Size(129, 20);
            this.textBoxStatic2Label.TabIndex = 28;
            this.textBoxStatic2Label.Text = "left and right intake";
            // 
            // textBoxStatic1Value
            // 
            this.textBoxStatic1Value.Location = new System.Drawing.Point(114, 301);
            this.textBoxStatic1Value.Name = "textBoxStatic1Value";
            this.textBoxStatic1Value.Size = new System.Drawing.Size(71, 20);
            this.textBoxStatic1Value.TabIndex = 30;
            this.textBoxStatic1Value.Text = "1140";
            // 
            // textBoxStatic2Value
            // 
            this.textBoxStatic2Value.Location = new System.Drawing.Point(114, 323);
            this.textBoxStatic2Value.Name = "textBoxStatic2Value";
            this.textBoxStatic2Value.Size = new System.Drawing.Size(71, 20);
            this.textBoxStatic2Value.TabIndex = 31;
            this.textBoxStatic2Value.Text = "1041";
            // 
            // chart1
            // 
            this.chart1.Dock = System.Windows.Forms.DockStyle.Right;
            this.chart1.Location = new System.Drawing.Point(337, 62);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(550, 573);
            this.chart1.TabIndex = 12;
            // 
            // timeSelector1
            // 
            this.timeSelector1.Location = new System.Drawing.Point(0, 486);
            this.timeSelector1.Name = "timeSelector1";
            this.timeSelector1.ShowTime = false;
            this.timeSelector1.Size = new System.Drawing.Size(213, 46);
            this.timeSelector1.T1 = new System.DateTime(2005, 1, 1, 10, 3, 0, 0);
            this.timeSelector1.T2 = new System.DateTime(2016, 1, 22, 10, 3, 0, 0);
            this.timeSelector1.TabIndex = 4;
            // 
            // ProfileDesigner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(887, 635);
            this.Controls.Add(this.textBoxStatic2Value);
            this.Controls.Add(this.textBoxStatic1Value);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBoxStatic2Label);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBoxStatic1Label);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBoxspeed);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonStep);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBoxOutputFile);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBoxWaterSurface);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.timeSelector1);
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
        private TimeSelectorBeginEnd timeSelector1;
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
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox textBoxWaterSurface;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBoxOutputFile;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button buttonStep;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.TextBox textBoxspeed;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBoxStatic1Label;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBoxStatic2Label;
        private System.Windows.Forms.TextBox textBoxStatic1Value;
        private System.Windows.Forms.TextBox textBoxStatic2Value;
    }
}