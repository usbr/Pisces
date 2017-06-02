namespace HydrometForecast
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.textBoxExcelFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openExcelDialog = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageEdit = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonRecomputeCoeficients = new System.Windows.Forms.Button();
            this.tabPageRun = new System.Windows.Forms.TabPage();
            this.runForecast1 = new HydrometForecast.RunForecast();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.textBoxLocalMpoll = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonOpenLocalMpoll = new System.Windows.Forms.Button();
            this.openFileDialogMpoll = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageEdit.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPageRun.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonOpenLocalMpoll);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBoxLocalMpoll);
            this.panel1.Controls.Add(this.buttonSave);
            this.panel1.Controls.Add(this.buttonOpenFile);
            this.panel1.Controls.Add(this.textBoxExcelFileName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(669, 53);
            this.panel1.TabIndex = 0;
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(20, 25);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 36;
            this.buttonSave.Text = "save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenFile.Image = ((System.Drawing.Image)(resources.GetObject("buttonOpenFile.Image")));
            this.buttonOpenFile.Location = new System.Drawing.Point(626, 8);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(35, 18);
            this.buttonOpenFile.TabIndex = 35;
            this.buttonOpenFile.UseVisualStyleBackColor = true;
            this.buttonOpenFile.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // textBoxExcelFileName
            // 
            this.textBoxExcelFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxExcelFileName.Location = new System.Drawing.Point(150, 7);
            this.textBoxExcelFileName.Name = "textBoxExcelFileName";
            this.textBoxExcelFileName.ReadOnly = true;
            this.textBoxExcelFileName.Size = new System.Drawing.Size(472, 20);
            this.textBoxExcelFileName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "forecast input spreadsheet:";
            // 
            // openExcelDialog
            // 
            this.openExcelDialog.DefaultExt = "txt";
            this.openExcelDialog.Filter = "Excel Spreadsheet (*.xls;*.csv;*.xlsx)|*.xls;*.csv;*.xlsx|All Files (*.*)|*.*";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageEdit);
            this.tabControl1.Controls.Add(this.tabPageRun);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 53);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(669, 385);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPageEdit
            // 
            this.tabPageEdit.Controls.Add(this.panel2);
            this.tabPageEdit.Location = new System.Drawing.Point(4, 22);
            this.tabPageEdit.Name = "tabPageEdit";
            this.tabPageEdit.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEdit.Size = new System.Drawing.Size(661, 359);
            this.tabPageEdit.TabIndex = 0;
            this.tabPageEdit.Text = "Edit";
            this.tabPageEdit.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonRecomputeCoeficients);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(655, 35);
            this.panel2.TabIndex = 0;
            // 
            // buttonRecomputeCoeficients
            // 
            this.buttonRecomputeCoeficients.Location = new System.Drawing.Point(24, 6);
            this.buttonRecomputeCoeficients.Name = "buttonRecomputeCoeficients";
            this.buttonRecomputeCoeficients.Size = new System.Drawing.Size(132, 23);
            this.buttonRecomputeCoeficients.TabIndex = 0;
            this.buttonRecomputeCoeficients.Text = "compute coeficients";
            this.buttonRecomputeCoeficients.UseVisualStyleBackColor = true;
            this.buttonRecomputeCoeficients.Click += new System.EventHandler(this.buttonRecomputeCoeficients_Click);
            // 
            // tabPageRun
            // 
            this.tabPageRun.Controls.Add(this.runForecast1);
            this.tabPageRun.Location = new System.Drawing.Point(4, 22);
            this.tabPageRun.Name = "tabPageRun";
            this.tabPageRun.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRun.Size = new System.Drawing.Size(661, 359);
            this.tabPageRun.TabIndex = 1;
            this.tabPageRun.Text = "Run";
            this.tabPageRun.UseVisualStyleBackColor = true;
            // 
            // runForecast1
            // 
            this.runForecast1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.runForecast1.Location = new System.Drawing.Point(3, 3);
            this.runForecast1.Name = "runForecast1";
            this.runForecast1.Size = new System.Drawing.Size(655, 353);
            this.runForecast1.TabIndex = 0;
            // 
            // textBoxLocalMpoll
            // 
            this.textBoxLocalMpoll.Location = new System.Drawing.Point(288, 27);
            this.textBoxLocalMpoll.Name = "textBoxLocalMpoll";
            this.textBoxLocalMpoll.Size = new System.Drawing.Size(194, 20);
            this.textBoxLocalMpoll.TabIndex = 37;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(164, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 13);
            this.label2.TabIndex = 38;
            this.label2.Text = "local mpoll file (optional)";
            // 
            // buttonOpenLocalMpoll
            // 
            this.buttonOpenLocalMpoll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenLocalMpoll.Image = ((System.Drawing.Image)(resources.GetObject("buttonOpenLocalMpoll.Image")));
            this.buttonOpenLocalMpoll.Location = new System.Drawing.Point(488, 27);
            this.buttonOpenLocalMpoll.Name = "buttonOpenLocalMpoll";
            this.buttonOpenLocalMpoll.Size = new System.Drawing.Size(35, 18);
            this.buttonOpenLocalMpoll.TabIndex = 39;
            this.buttonOpenLocalMpoll.UseVisualStyleBackColor = true;
            this.buttonOpenLocalMpoll.Click += new System.EventHandler(this.buttonOpenLocalMpoll_Click);
            // 
            // openFileDialogMpoll
            // 
            this.openFileDialogMpoll.DefaultExt = "txt";
            this.openFileDialogMpoll.Filter = "SQLite database (*.db)|*.db|All Files (*.*)|*.*";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 438);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "Hydromet Forecasting";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageEdit.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabPageRun.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxExcelFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openExcelDialog;
        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageEdit;
        private System.Windows.Forms.TabPage tabPageRun;
        private System.Windows.Forms.Button buttonSave;
        private RunForecast runForecast1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonRecomputeCoeficients;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button buttonOpenLocalMpoll;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxLocalMpoll;
        private System.Windows.Forms.OpenFileDialog openFileDialogMpoll;
    }
}