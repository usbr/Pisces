namespace Reclamation.TimeSeries.Forms.Calculations
{
    partial class BasicEquation
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
            this.textBoxHelp = new System.Windows.Forms.TextBox();
            this.textBoxMath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.listBoxFunctions = new System.Windows.Forms.ListBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.textBoxSiteID = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageEquation = new System.Windows.Forms.TabPage();
            this.comboBoxInterval = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxCompute = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxHelp1 = new System.Windows.Forms.TextBox();
            this.textBoxParameter = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPageEquation.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxHelp
            // 
            this.textBoxHelp.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxHelp.Location = new System.Drawing.Point(3, 3);
            this.textBoxHelp.MinimumSize = new System.Drawing.Size(365, 72);
            this.textBoxHelp.Multiline = true;
            this.textBoxHelp.Name = "textBoxHelp";
            this.textBoxHelp.ReadOnly = true;
            this.textBoxHelp.Size = new System.Drawing.Size(368, 72);
            this.textBoxHelp.TabIndex = 6;
            this.textBoxHelp.Text = "Use this dialog to define a basic math equation for a computed series. Use standa" +
    "rd math operators to form an equation. For example:\r\n\r\nSeries1 * 1.98347\r\n(JCK_A" +
    "F[t] - JCK_AF[t-1])/1.98347 + JCK_QD";
            // 
            // textBoxMath
            // 
            this.textBoxMath.AllowDrop = true;
            this.textBoxMath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMath.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMath.Location = new System.Drawing.Point(3, 3);
            this.textBoxMath.Multiline = true;
            this.textBoxMath.Name = "textBoxMath";
            this.textBoxMath.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxMath.Size = new System.Drawing.Size(354, 217);
            this.textBoxMath.TabIndex = 3;
            this.textBoxMath.DragDrop += new System.Windows.Forms.DragEventHandler(this.textBoxMath_DragDrop_1);
            this.textBoxMath.DragEnter += new System.Windows.Forms.DragEventHandler(this.textBoxMath_DragEnter_1);
            this.textBoxMath.DragOver += new System.Windows.Forms.DragEventHandler(this.textBoxMath_DragOver);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(374, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "functions";
            // 
            // listBoxFunctions
            // 
            this.listBoxFunctions.AllowDrop = true;
            this.listBoxFunctions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxFunctions.HorizontalScrollbar = true;
            this.listBoxFunctions.Location = new System.Drawing.Point(377, 35);
            this.listBoxFunctions.Name = "listBoxFunctions";
            this.listBoxFunctions.Size = new System.Drawing.Size(295, 355);
            this.listBoxFunctions.TabIndex = 9;
            this.listBoxFunctions.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listBoxFunctions_KeyUp);
            this.listBoxFunctions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxFunctions_MouseDown);
            // 
            // textBoxSeriesID
            // 
            this.textBoxSiteID.Location = new System.Drawing.Point(249, 82);
            this.textBoxSiteID.Name = "textBoxSeriesID";
            this.textBoxSiteID.Size = new System.Drawing.Size(122, 20);
            this.textBoxSiteID.TabIndex = 13;
            this.toolTip1.SetToolTip(this.textBoxSiteID, "enter a simple name  like boise_flow");
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageEquation);
            this.tabControl1.Location = new System.Drawing.Point(3, 143);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(368, 249);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPageEquation
            // 
            this.tabPageEquation.Controls.Add(this.textBoxMath);
            this.tabPageEquation.Location = new System.Drawing.Point(4, 22);
            this.tabPageEquation.Name = "tabPageEquation";
            this.tabPageEquation.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEquation.Size = new System.Drawing.Size(360, 223);
            this.tabPageEquation.TabIndex = 0;
            this.tabPageEquation.Text = "expression";
            this.tabPageEquation.UseVisualStyleBackColor = true;
            // 
            // comboBoxInterval
            // 
            this.comboBoxInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxInterval.FormattingEnabled = true;
            this.comboBoxInterval.Items.AddRange(new object[] {
            "Hourly",
            "Daily",
            "Monthly",
            "Weekly",
            "Yearly",
            "Irregular"});
            this.comboBoxInterval.Location = new System.Drawing.Point(3, 81);
            this.comboBoxInterval.Name = "comboBoxInterval";
            this.comboBoxInterval.Size = new System.Drawing.Size(160, 21);
            this.comboBoxInterval.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(172, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "siteid (cbtt)";
            // 
            // checkBoxCompute
            // 
            this.checkBoxCompute.AutoSize = true;
            this.checkBoxCompute.Location = new System.Drawing.Point(7, 107);
            this.checkBoxCompute.Margin = new System.Windows.Forms.Padding(2);
            this.checkBoxCompute.Name = "checkBoxCompute";
            this.checkBoxCompute.Size = new System.Drawing.Size(160, 17);
            this.checkBoxCompute.TabIndex = 18;
            this.checkBoxCompute.Text = "compute full period of record";
            this.checkBoxCompute.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBoxParameter);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.textBoxHelp1);
            this.panel1.Controls.Add(this.textBoxHelp);
            this.panel1.Controls.Add(this.checkBoxCompute);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.listBoxFunctions);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.comboBoxInterval);
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Controls.Add(this.textBoxSiteID);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(675, 446);
            this.panel1.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(172, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 20;
            this.label4.Text = "parameter";
            // 
            // textBoxHelp1
            // 
            this.textBoxHelp1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxHelp1.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxHelp1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxHelp1.Location = new System.Drawing.Point(7, 398);
            this.textBoxHelp1.Multiline = true;
            this.textBoxHelp1.Name = "textBoxHelp1";
            this.textBoxHelp1.Size = new System.Drawing.Size(660, 45);
            this.textBoxHelp1.TabIndex = 19;
            this.textBoxHelp1.Text = "Help text will be seen here";
            // 
            // textBoxParameter
            // 
            this.textBoxParameter.Location = new System.Drawing.Point(249, 108);
            this.textBoxParameter.Name = "textBoxParameter";
            this.textBoxParameter.Size = new System.Drawing.Size(122, 20);
            this.textBoxParameter.TabIndex = 21;
            this.toolTip1.SetToolTip(this.textBoxParameter, "enter a simple name  like boise_flow");
            // 
            // BasicEquation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "BasicEquation";
            this.Size = new System.Drawing.Size(675, 446);
            this.tabControl1.ResumeLayout(false);
            this.tabPageEquation.ResumeLayout(false);
            this.tabPageEquation.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxHelp;
        private System.Windows.Forms.TextBox textBoxMath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listBoxFunctions;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageEquation;
        private System.Windows.Forms.ComboBox comboBoxInterval;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSiteID;
        private System.Windows.Forms.CheckBox checkBoxCompute;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBoxHelp1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxParameter;
    }
}
