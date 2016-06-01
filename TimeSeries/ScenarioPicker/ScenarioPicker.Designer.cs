namespace Reclamation.TimeSeries.ScenarioPicker
{
    partial class ScenarioPicker
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
            this.checkBoxScenarios = new System.Windows.Forms.CheckBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonAddScenario = new System.Windows.Forms.Button();
            this.listBoxScenarios = new System.Windows.Forms.ListBox();
            this.buttonRemove = new System.Windows.Forms.Button();
            this.buttonMoveUp = new System.Windows.Forms.Button();
            this.buttonMoveDown = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.checkBoxAddToTree = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // checkBoxScenarios
            // 
            this.checkBoxScenarios.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxScenarios.AutoSize = true;
            this.checkBoxScenarios.Checked = true;
            this.checkBoxScenarios.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxScenarios.Location = new System.Drawing.Point(195, 227);
            this.checkBoxScenarios.Name = "checkBoxScenarios";
            this.checkBoxScenarios.Size = new System.Drawing.Size(107, 17);
            this.checkBoxScenarios.TabIndex = 1;
            this.checkBoxScenarios.Text = "Create Scenarios";
            this.checkBoxScenarios.UseVisualStyleBackColor = true;
            this.checkBoxScenarios.CheckedChanged += new System.EventHandler(this.checkBoxScenarios_CheckedChanged);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(405, 238);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(324, 238);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonAddScenario
            // 
            this.buttonAddScenario.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddScenario.Location = new System.Drawing.Point(324, 36);
            this.buttonAddScenario.Name = "buttonAddScenario";
            this.buttonAddScenario.Size = new System.Drawing.Size(79, 23);
            this.buttonAddScenario.TabIndex = 4;
            this.buttonAddScenario.Text = "Add";
            this.buttonAddScenario.UseVisualStyleBackColor = true;
            this.buttonAddScenario.Click += new System.EventHandler(this.buttonAddScenario_Click);
            // 
            // listBoxScenarios
            // 
            this.listBoxScenarios.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxScenarios.FormattingEnabled = true;
            this.listBoxScenarios.Location = new System.Drawing.Point(43, 36);
            this.listBoxScenarios.Name = "listBoxScenarios";
            this.listBoxScenarios.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.listBoxScenarios.Size = new System.Drawing.Size(275, 173);
            this.listBoxScenarios.TabIndex = 5;
            // 
            // buttonRemove
            // 
            this.buttonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRemove.Location = new System.Drawing.Point(324, 66);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new System.Drawing.Size(79, 23);
            this.buttonRemove.TabIndex = 6;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new System.EventHandler(this.buttonRemove_Click);
            // 
            // buttonMoveUp
            // 
            this.buttonMoveUp.Font = new System.Drawing.Font("Wingdings 3", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonMoveUp.Location = new System.Drawing.Point(12, 36);
            this.buttonMoveUp.Name = "buttonMoveUp";
            this.buttonMoveUp.Size = new System.Drawing.Size(25, 23);
            this.buttonMoveUp.TabIndex = 7;
            this.buttonMoveUp.Text = "h";
            this.buttonMoveUp.UseVisualStyleBackColor = true;
            this.buttonMoveUp.Click += new System.EventHandler(this.buttonMoveUp_Click);
            // 
            // buttonMoveDown
            // 
            this.buttonMoveDown.Font = new System.Drawing.Font("Wingdings 3", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.buttonMoveDown.Location = new System.Drawing.Point(12, 66);
            this.buttonMoveDown.Name = "buttonMoveDown";
            this.buttonMoveDown.Size = new System.Drawing.Size(25, 23);
            this.buttonMoveDown.TabIndex = 8;
            this.buttonMoveDown.Text = "i";
            this.buttonMoveDown.UseVisualStyleBackColor = true;
            this.buttonMoveDown.Click += new System.EventHandler(this.buttonMoveDown_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.SystemColors.Menu;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(325, 121);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(155, 45);
            this.textBox1.TabIndex = 9;
            this.textBox1.Text = "Note: The first file in the list will be used as the Baseline if scenarios are us" +
    "ed.";
            // 
            // checkBoxAddToTree
            // 
            this.checkBoxAddToTree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxAddToTree.AutoSize = true;
            this.checkBoxAddToTree.Checked = true;
            this.checkBoxAddToTree.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAddToTree.Location = new System.Drawing.Point(195, 250);
            this.checkBoxAddToTree.Name = "checkBoxAddToTree";
            this.checkBoxAddToTree.Size = new System.Drawing.Size(82, 17);
            this.checkBoxAddToTree.TabIndex = 10;
            this.checkBoxAddToTree.Text = "Add to Tree";
            this.checkBoxAddToTree.UseVisualStyleBackColor = true;
            this.checkBoxAddToTree.CheckedChanged += new System.EventHandler(this.checkBoxAddToTree_CheckedChanged);
            // 
            // ScenarioPicker
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(492, 273);
            this.Controls.Add(this.checkBoxAddToTree);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonMoveDown);
            this.Controls.Add(this.buttonMoveUp);
            this.Controls.Add(this.buttonRemove);
            this.Controls.Add(this.listBoxScenarios);
            this.Controls.Add(this.buttonAddScenario);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.checkBoxScenarios);
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "ScenarioPicker";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxScenarios;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonAddScenario;
        private System.Windows.Forms.ListBox listBoxScenarios;
        private System.Windows.Forms.Button buttonRemove;
        private System.Windows.Forms.Button buttonMoveUp;
        private System.Windows.Forms.Button buttonMoveDown;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox checkBoxAddToTree;
    }
}