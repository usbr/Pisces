namespace Reclamation.AgriMet.UI
{
    partial class AgriMetCropChartEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgriMetCropChartEditor));
            this.panelTop = new System.Windows.Forms.Panel();
            this.buttonCopyLastYear = new System.Windows.Forms.Button();
            this.textBoxGroup = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxCbtt = new System.Windows.Forms.TextBox();
            this.textBoxYear = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonRead = new System.Windows.Forms.Button();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.spreadsheetControl1 = new Reclamation.AgriMet.UI.SpreadsheetControl();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelTop.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.buttonCopyLastYear);
            this.panelTop.Controls.Add(this.textBoxGroup);
            this.panelTop.Controls.Add(this.label3);
            this.panelTop.Controls.Add(this.textBoxCbtt);
            this.panelTop.Controls.Add(this.textBoxYear);
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Controls.Add(this.buttonSave);
            this.panelTop.Controls.Add(this.buttonRead);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(624, 82);
            this.panelTop.TabIndex = 0;
            // 
            // buttonCopyLastYear
            // 
            this.buttonCopyLastYear.Location = new System.Drawing.Point(503, 3);
            this.buttonCopyLastYear.Name = "buttonCopyLastYear";
            this.buttonCopyLastYear.Size = new System.Drawing.Size(94, 23);
            this.buttonCopyLastYear.TabIndex = 25;
            this.buttonCopyLastYear.Text = "create new year";
            this.toolTip1.SetToolTip(this.buttonCopyLastYear, "copy last years data to this year");
            this.buttonCopyLastYear.UseVisualStyleBackColor = true;
            this.buttonCopyLastYear.Click += new System.EventHandler(this.buttonCopyLastYear_Click);
            // 
            // textBoxGroup
            // 
            this.textBoxGroup.Location = new System.Drawing.Point(206, 25);
            this.textBoxGroup.Name = "textBoxGroup";
            this.textBoxGroup.Size = new System.Drawing.Size(33, 20);
            this.textBoxGroup.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(205, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "group";
            // 
            // textBoxCbtt
            // 
            this.textBoxCbtt.Location = new System.Drawing.Point(149, 25);
            this.textBoxCbtt.Name = "textBoxCbtt";
            this.textBoxCbtt.Size = new System.Drawing.Size(52, 20);
            this.textBoxCbtt.TabIndex = 21;
            this.toolTip1.SetToolTip(this.textBoxCbtt, "enter cbtt such as BOII, or leave blank for all sites");
            // 
            // textBoxYear
            // 
            this.textBoxYear.Location = new System.Drawing.Point(91, 24);
            this.textBoxYear.Name = "textBoxYear";
            this.textBoxYear.Size = new System.Drawing.Size(52, 20);
            this.textBoxYear.TabIndex = 19;
            this.toolTip1.SetToolTip(this.textBoxYear, "enter year");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(156, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "cbtt";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(99, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(27, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "year";
            // 
            // buttonSave
            // 
            this.buttonSave.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("buttonSave.BackgroundImage")));
            this.buttonSave.Location = new System.Drawing.Point(263, 14);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(30, 26);
            this.buttonSave.TabIndex = 18;
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonRead
            // 
            this.buttonRead.Location = new System.Drawing.Point(21, 22);
            this.buttonRead.Name = "buttonRead";
            this.buttonRead.Size = new System.Drawing.Size(62, 23);
            this.buttonRead.TabIndex = 0;
            this.buttonRead.Text = "Refresh";
            this.buttonRead.UseVisualStyleBackColor = true;
            this.buttonRead.Click += new System.EventHandler(this.buttonRead_Click);
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.spreadsheetControl1);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(0, 82);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(624, 344);
            this.panelBottom.TabIndex = 1;
            // 
            // spreadsheetControl1
            // 
            this.spreadsheetControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spreadsheetControl1.Location = new System.Drawing.Point(0, 0);
            this.spreadsheetControl1.Name = "spreadsheetControl1";
            this.spreadsheetControl1.Size = new System.Drawing.Size(624, 344);
            this.spreadsheetControl1.TabIndex = 0;
            // 
            // AgriMetCropChartEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.Name = "AgriMetCropChartEditor";
            this.Size = new System.Drawing.Size(624, 426);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button buttonRead;
        private System.Windows.Forms.Button buttonSave;
        private SpreadsheetControl spreadsheetControl1;
        private System.Windows.Forms.TextBox textBoxYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxCbtt;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox textBoxGroup;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonCopyLastYear;


    }
}
