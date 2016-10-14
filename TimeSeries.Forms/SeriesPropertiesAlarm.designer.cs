namespace Reclamation.TimeSeries.Forms
{
    partial class SeriesPropertiesAlarm
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
            this.comboBoxGroup = new System.Windows.Forms.ComboBox();
            this.label28 = new System.Windows.Forms.Label();
            this.checkBoxAlarmEnable = new System.Windows.Forms.CheckBox();
            this.labelExpression = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonApplyAlarm = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxGroup
            // 
            this.comboBoxGroup.FormattingEnabled = true;
            this.comboBoxGroup.Location = new System.Drawing.Point(120, 246);
            this.comboBoxGroup.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxGroup.Name = "comboBoxGroup";
            this.comboBoxGroup.Size = new System.Drawing.Size(360, 24);
            this.comboBoxGroup.TabIndex = 30;
            this.toolTip1.SetToolTip(this.comboBoxGroup, "Group of recipients for the alarm");
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(33, 26);
            this.label28.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(52, 17);
            this.label28.TabIndex = 28;
            this.label28.Text = "Enable";
            // 
            // checkBoxAlarmEnable
            // 
            this.checkBoxAlarmEnable.AutoSize = true;
            this.checkBoxAlarmEnable.Location = new System.Drawing.Point(120, 27);
            this.checkBoxAlarmEnable.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxAlarmEnable.Name = "checkBoxAlarmEnable";
            this.checkBoxAlarmEnable.Size = new System.Drawing.Size(18, 17);
            this.checkBoxAlarmEnable.TabIndex = 27;
            this.checkBoxAlarmEnable.UseVisualStyleBackColor = true;
            // 
            // labelExpression
            // 
            this.labelExpression.AutoSize = true;
            this.labelExpression.Location = new System.Drawing.Point(33, 51);
            this.labelExpression.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelExpression.Name = "labelExpression";
            this.labelExpression.Size = new System.Drawing.Size(77, 17);
            this.labelExpression.TabIndex = 26;
            this.labelExpression.Text = "Expression";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(33, 246);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(74, 17);
            this.label22.TabIndex = 22;
            this.label22.Text = "Recipients";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(36, 277);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(444, 173);
            this.dataGridView1.TabIndex = 31;
            // 
            // buttonApplyAlarm
            // 
            this.buttonApplyAlarm.Location = new System.Drawing.Point(347, 456);
            this.buttonApplyAlarm.Name = "buttonApplyAlarm";
            this.buttonApplyAlarm.Size = new System.Drawing.Size(133, 28);
            this.buttonApplyAlarm.TabIndex = 37;
            this.buttonApplyAlarm.Text = "read test data";
            this.buttonApplyAlarm.UseVisualStyleBackColor = true;
            this.buttonApplyAlarm.Click += new System.EventHandler(this.buttonReadTestDAta_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(120, 50);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(360, 64);
            this.textBox1.TabIndex = 38;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(67, 4);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(120, 120);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(360, 72);
            this.textBox2.TabIndex = 40;
            this.textBox2.Text = "above 4198.20 or below 3.0 or rising 1.0";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(67, 4);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 120);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 17);
            this.label1.TabIndex = 42;
            this.label1.Text = "Example ";
            // 
            // SeriesPropertiesAlarm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonApplyAlarm);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.comboBoxGroup);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.checkBoxAlarmEnable);
            this.Controls.Add(this.labelExpression);
            this.Controls.Add(this.label22);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SeriesPropertiesAlarm";
            this.Size = new System.Drawing.Size(500, 500);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxGroup;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.CheckBox checkBoxAlarmEnable;
        private System.Windows.Forms.Label labelExpression;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonApplyAlarm;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.Label label1;
    }
}
