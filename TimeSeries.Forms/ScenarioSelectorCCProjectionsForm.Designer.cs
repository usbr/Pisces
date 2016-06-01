namespace Reclamation.TimeSeries.Forms
{
    partial class ScenarioSelectorCCProjectionsForm
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
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.tabControlCCScenarios = new System.Windows.Forms.TabControl();
            this.tabPageProjections = new System.Windows.Forms.TabPage();
            this.tabPageReference = new System.Windows.Forms.TabPage();
            this.labelReference = new System.Windows.Forms.Label();
            this.scenarioReferenceControl1 = new Reclamation.TimeSeries.Forms.ScenarioReferenceControl();
            this.tabControlCCScenarios.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonApply
            // 
            this.buttonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApply.Location = new System.Drawing.Point(718, 667);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(75, 23);
            this.buttonApply.TabIndex = 1;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(637, 667);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(556, 667);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // tabControlCCScenarios
            // 
            this.tabControlCCScenarios.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlCCScenarios.Controls.Add(this.tabPageProjections);
            this.tabControlCCScenarios.Controls.Add(this.tabPageReference);
            this.tabControlCCScenarios.Location = new System.Drawing.Point(12, 12);
            this.tabControlCCScenarios.Name = "tabControlCCScenarios";
            this.tabControlCCScenarios.SelectedIndex = 0;
            this.tabControlCCScenarios.Size = new System.Drawing.Size(782, 593);
            this.tabControlCCScenarios.TabIndex = 0;
            this.tabControlCCScenarios.Click += new System.EventHandler(this.tabPage_Click);
            // 
            // tabPageProjections
            // 
            this.tabPageProjections.AutoScroll = true;
            this.tabPageProjections.Location = new System.Drawing.Point(4, 22);
            this.tabPageProjections.Name = "tabPageProjections";
            this.tabPageProjections.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProjections.Size = new System.Drawing.Size(774, 567);
            this.tabPageProjections.TabIndex = 0;
            this.tabPageProjections.Text = "projections";
            this.tabPageProjections.UseVisualStyleBackColor = true;
            // 
            // tabPageReference
            // 
            this.tabPageReference.AutoScroll = true;
            this.tabPageReference.BackColor = System.Drawing.Color.LightCyan;
            this.tabPageReference.Location = new System.Drawing.Point(4, 22);
            this.tabPageReference.Name = "tabPageReference";
            this.tabPageReference.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageReference.Size = new System.Drawing.Size(774, 567);
            this.tabPageReference.TabIndex = 1;
            this.tabPageReference.Text = "reference builder";
            // 
            // labelReference
            // 
            this.labelReference.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelReference.AutoSize = true;
            this.labelReference.Location = new System.Drawing.Point(327, 622);
            this.labelReference.Name = "labelReference";
            this.labelReference.Size = new System.Drawing.Size(435, 13);
            this.labelReference.TabIndex = 5;
            this.labelReference.Text = "Note: the median of the projections selected will be the reference available for " +
    "comparisons";
            // 
            // scenarioReferenceControl1
            // 
            this.scenarioReferenceControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.scenarioReferenceControl1.Location = new System.Drawing.Point(182, 611);
            this.scenarioReferenceControl1.Name = "scenarioReferenceControl1";
            this.scenarioReferenceControl1.Size = new System.Drawing.Size(193, 95);
            this.scenarioReferenceControl1.TabIndex = 4;
            // 
            // ScenarioSelectorCCProjectionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(805, 702);
            this.Controls.Add(this.labelReference);
            this.Controls.Add(this.scenarioReferenceControl1);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.tabControlCCScenarios);
            this.Name = "ScenarioSelectorCCProjectionsForm";
            this.Text = "ScenarioSelectorCCProjectionsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScenarioSelectorCCProjectionsForm_FormClosing);
            this.Load += new System.EventHandler(this.ScenarioSelectorCCProjectionsForm_Load);
            this.tabControlCCScenarios.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TabControl tabControlCCScenarios;
        private System.Windows.Forms.TabPage tabPageProjections;
        private System.Windows.Forms.TabPage tabPageReference;
        private ScenarioReferenceControl scenarioReferenceControl1;
        private System.Windows.Forms.Label labelReference;
    }
}