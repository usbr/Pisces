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
            this.label27 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.textBoxAlarmTextMessage = new System.Windows.Forms.TextBox();
            this.textBoxClearAlarmValue = new System.Windows.Forms.TextBox();
            this.textBoxAlarmValue = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.radioButtonMinValue = new System.Windows.Forms.RadioButton();
            this.radioButtonMaxValue = new System.Windows.Forms.RadioButton();
            this.radioButtonROC = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonApplyAlarm = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBoxGroup
            // 
            this.comboBoxGroup.FormattingEnabled = true;
            this.comboBoxGroup.Location = new System.Drawing.Point(204, 246);
            this.comboBoxGroup.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxGroup.Name = "comboBoxGroup";
            this.comboBoxGroup.Size = new System.Drawing.Size(276, 24);
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
            this.checkBoxAlarmEnable.Location = new System.Drawing.Point(204, 26);
            this.checkBoxAlarmEnable.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxAlarmEnable.Name = "checkBoxAlarmEnable";
            this.checkBoxAlarmEnable.Size = new System.Drawing.Size(18, 17);
            this.checkBoxAlarmEnable.TabIndex = 27;
            this.checkBoxAlarmEnable.UseVisualStyleBackColor = true;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(33, 188);
            this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(96, 17);
            this.label27.TabIndex = 26;
            this.label27.Text = "Text/Message";
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
            // textBoxAlarmTextMessage
            // 
            this.textBoxAlarmTextMessage.Location = new System.Drawing.Point(204, 185);
            this.textBoxAlarmTextMessage.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxAlarmTextMessage.Multiline = true;
            this.textBoxAlarmTextMessage.Name = "textBoxAlarmTextMessage";
            this.textBoxAlarmTextMessage.Size = new System.Drawing.Size(276, 53);
            this.textBoxAlarmTextMessage.TabIndex = 21;
            this.toolTip1.SetToolTip(this.textBoxAlarmTextMessage, "Custom message for the alarm.");
            // 
            // textBoxClearAlarmValue
            // 
            this.textBoxClearAlarmValue.Location = new System.Drawing.Point(204, 155);
            this.textBoxClearAlarmValue.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxClearAlarmValue.Name = "textBoxClearAlarmValue";
            this.textBoxClearAlarmValue.Size = new System.Drawing.Size(276, 22);
            this.textBoxClearAlarmValue.TabIndex = 20;
            this.toolTip1.SetToolTip(this.textBoxClearAlarmValue, "Alarm condition will clear with this value.");
            // 
            // textBoxAlarmValue
            // 
            this.textBoxAlarmValue.Location = new System.Drawing.Point(204, 65);
            this.textBoxAlarmValue.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxAlarmValue.Name = "textBoxAlarmValue";
            this.textBoxAlarmValue.Size = new System.Drawing.Size(276, 22);
            this.textBoxAlarmValue.TabIndex = 18;
            this.toolTip1.SetToolTip(this.textBoxAlarmValue, "Alarm will occur if value is higher than this.");
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
            // radioButtonMinValue
            // 
            this.radioButtonMinValue.AutoSize = true;
            this.radioButtonMinValue.Location = new System.Drawing.Point(36, 65);
            this.radioButtonMinValue.Name = "radioButtonMinValue";
            this.radioButtonMinValue.Size = new System.Drawing.Size(91, 21);
            this.radioButtonMinValue.TabIndex = 32;
            this.radioButtonMinValue.TabStop = true;
            this.radioButtonMinValue.Text = "Min Value";
            this.radioButtonMinValue.UseVisualStyleBackColor = true;
            // 
            // radioButtonMaxValue
            // 
            this.radioButtonMaxValue.AutoSize = true;
            this.radioButtonMaxValue.Location = new System.Drawing.Point(36, 93);
            this.radioButtonMaxValue.Name = "radioButtonMaxValue";
            this.radioButtonMaxValue.Size = new System.Drawing.Size(94, 21);
            this.radioButtonMaxValue.TabIndex = 33;
            this.radioButtonMaxValue.TabStop = true;
            this.radioButtonMaxValue.Text = "Max Value";
            this.radioButtonMaxValue.UseVisualStyleBackColor = true;
            // 
            // radioButtonROC
            // 
            this.radioButtonROC.AutoSize = true;
            this.radioButtonROC.Location = new System.Drawing.Point(36, 121);
            this.radioButtonROC.Name = "radioButtonROC";
            this.radioButtonROC.Size = new System.Drawing.Size(128, 21);
            this.radioButtonROC.TabIndex = 34;
            this.radioButtonROC.TabStop = true;
            this.radioButtonROC.Text = "Rate of Change";
            this.radioButtonROC.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 155);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 17);
            this.label1.TabIndex = 35;
            this.label1.Text = "Clear Alarm Value";
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
            // SeriesPropertiesAlarm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonApplyAlarm);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioButtonROC);
            this.Controls.Add(this.radioButtonMaxValue);
            this.Controls.Add(this.radioButtonMinValue);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.comboBoxGroup);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.checkBoxAlarmEnable);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.textBoxAlarmTextMessage);
            this.Controls.Add(this.textBoxClearAlarmValue);
            this.Controls.Add(this.textBoxAlarmValue);
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
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox textBoxAlarmTextMessage;
        private System.Windows.Forms.TextBox textBoxClearAlarmValue;
        private System.Windows.Forms.TextBox textBoxAlarmValue;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.RadioButton radioButtonMinValue;
        private System.Windows.Forms.RadioButton radioButtonMaxValue;
        private System.Windows.Forms.RadioButton radioButtonROC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonApplyAlarm;
    }
}
