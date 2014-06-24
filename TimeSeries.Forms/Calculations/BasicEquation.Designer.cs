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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageEquation = new System.Windows.Forms.TabPage();
            this.labelToolTip = new System.Windows.Forms.Label();
            this.comboBoxInterval = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabPageEquation.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxHelp
            // 
            this.textBoxHelp.BackColor = System.Drawing.SystemColors.Control;
            this.textBoxHelp.Location = new System.Drawing.Point(19, 3);
            this.textBoxHelp.MinimumSize = new System.Drawing.Size(410, 72);
            this.textBoxHelp.Multiline = true;
            this.textBoxHelp.Name = "textBoxHelp";
            this.textBoxHelp.ReadOnly = true;
            this.textBoxHelp.Size = new System.Drawing.Size(410, 72);
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
            this.textBoxMath.Size = new System.Drawing.Size(448, 291);
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
            this.label3.Location = new System.Drawing.Point(499, 27);
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
            this.listBoxFunctions.Location = new System.Drawing.Point(502, 43);
            this.listBoxFunctions.Name = "listBoxFunctions";
            this.listBoxFunctions.Size = new System.Drawing.Size(208, 381);
            this.listBoxFunctions.TabIndex = 9;
            this.listBoxFunctions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.listBoxFunctions_MouseDown);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageEquation);
            this.tabControl1.Location = new System.Drawing.Point(19, 108);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(462, 323);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPageEquation
            // 
            this.tabPageEquation.Controls.Add(this.textBoxMath);
            this.tabPageEquation.Location = new System.Drawing.Point(4, 22);
            this.tabPageEquation.Name = "tabPageEquation";
            this.tabPageEquation.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEquation.Size = new System.Drawing.Size(454, 297);
            this.tabPageEquation.TabIndex = 0;
            this.tabPageEquation.Text = "expression";
            this.tabPageEquation.UseVisualStyleBackColor = true;
            // 
            // labelToolTip
            // 
            this.labelToolTip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelToolTip.AutoSize = true;
            this.labelToolTip.Location = new System.Drawing.Point(23, 446);
            this.labelToolTip.Name = "labelToolTip";
            this.labelToolTip.Size = new System.Drawing.Size(10, 13);
            this.labelToolTip.TabIndex = 11;
            this.labelToolTip.Text = ".";
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
            this.comboBoxInterval.Location = new System.Drawing.Point(19, 81);
            this.comboBoxInterval.Name = "comboBoxInterval";
            this.comboBoxInterval.Size = new System.Drawing.Size(160, 21);
            this.comboBoxInterval.TabIndex = 12;
            // 
            // BasicEquation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxInterval);
            this.Controls.Add(this.labelToolTip);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.listBoxFunctions);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxHelp);
            this.MinimumSize = new System.Drawing.Size(447, 380);
            this.Name = "BasicEquation";
            this.Size = new System.Drawing.Size(724, 495);
            this.tabControl1.ResumeLayout(false);
            this.tabPageEquation.ResumeLayout(false);
            this.tabPageEquation.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxHelp;
        private System.Windows.Forms.TextBox textBoxMath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox listBoxFunctions;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageEquation;
        private System.Windows.Forms.Label labelToolTip;
        private System.Windows.Forms.ComboBox comboBoxInterval;
    }
}
