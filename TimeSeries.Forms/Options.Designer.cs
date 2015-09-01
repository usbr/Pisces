namespace Reclamation.TimeSeries.Forms
{
    partial class Options
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
            this.checkBoxHydrometAutoUpdate = new System.Windows.Forms.CheckBox();
            this.checkBoxHydrometIncludeFlaggedData = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxHydrometWebOnly = new System.Windows.Forms.CheckBox();
            this.checkBoxHydrometWebCache = new System.Windows.Forms.CheckBox();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.checkBoxMultiYAxis = new System.Windows.Forms.CheckBox();
            this.checkBoxAutoRefresh = new System.Windows.Forms.CheckBox();
            this.tabPageHydromet = new System.Windows.Forms.TabPage();
            this.tabPageUsgs = new System.Windows.Forms.TabPage();
            this.checkBoxUsgsAutoUpdate = new System.Windows.Forms.CheckBox();
            this.tabPageModsim = new System.Windows.Forms.TabPage();
            this.checkBoxModsimDisplayCfs = new System.Windows.Forms.CheckBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.checkBoxExcelAutoUpdate = new System.Windows.Forms.CheckBox();
            this.tabPageDecodes = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxDecodesPath = new System.Windows.Forms.TextBox();
            this.tabPageCalcs = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxHydrometVariableResolver = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.serverSelection1 = new Reclamation.TimeSeries.Forms.Hydromet.ServerSelection();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPageHydromet.SuspendLayout();
            this.tabPageUsgs.SuspendLayout();
            this.tabPageModsim.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPageDecodes.SuspendLayout();
            this.tabPageCalcs.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxHydrometAutoUpdate
            // 
            this.checkBoxHydrometAutoUpdate.AutoSize = true;
            this.checkBoxHydrometAutoUpdate.Location = new System.Drawing.Point(32, 52);
            this.checkBoxHydrometAutoUpdate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxHydrometAutoUpdate.Name = "checkBoxHydrometAutoUpdate";
            this.checkBoxHydrometAutoUpdate.Size = new System.Drawing.Size(256, 21);
            this.checkBoxHydrometAutoUpdate.TabIndex = 0;
            this.checkBoxHydrometAutoUpdate.Text = "update Hydromet data from the web";
            this.toolTip1.SetToolTip(this.checkBoxHydrometAutoUpdate, "update pisces database from the web as needed");
            this.checkBoxHydrometAutoUpdate.UseVisualStyleBackColor = true;
            // 
            // checkBoxHydrometIncludeFlaggedData
            // 
            this.checkBoxHydrometIncludeFlaggedData.AutoSize = true;
            this.checkBoxHydrometIncludeFlaggedData.Location = new System.Drawing.Point(32, 75);
            this.checkBoxHydrometIncludeFlaggedData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxHydrometIncludeFlaggedData.Name = "checkBoxHydrometIncludeFlaggedData";
            this.checkBoxHydrometIncludeFlaggedData.Size = new System.Drawing.Size(158, 21);
            this.checkBoxHydrometIncludeFlaggedData.TabIndex = 1;
            this.checkBoxHydrometIncludeFlaggedData.Text = "include flagged data";
            this.toolTip1.SetToolTip(this.checkBoxHydrometIncludeFlaggedData, "include data flagged bad such as above high limit");
            this.checkBoxHydrometIncludeFlaggedData.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxHydrometWebOnly);
            this.groupBox1.Controls.Add(this.checkBoxHydrometWebCache);
            this.groupBox1.Controls.Add(this.checkBoxHydrometAutoUpdate);
            this.groupBox1.Controls.Add(this.checkBoxHydrometIncludeFlaggedData);
            this.groupBox1.Location = new System.Drawing.Point(20, 7);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox1.Size = new System.Drawing.Size(383, 150);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hydromet";
            // 
            // checkBoxHydrometWebOnly
            // 
            this.checkBoxHydrometWebOnly.AutoSize = true;
            this.checkBoxHydrometWebOnly.Location = new System.Drawing.Point(32, 23);
            this.checkBoxHydrometWebOnly.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxHydrometWebOnly.Name = "checkBoxHydrometWebOnly";
            this.checkBoxHydrometWebOnly.Size = new System.Drawing.Size(179, 21);
            this.checkBoxHydrometWebOnly.TabIndex = 3;
            this.checkBoxHydrometWebOnly.Text = "always use web queries";
            this.toolTip1.SetToolTip(this.checkBoxHydrometWebOnly, "hydromet data will only come from the web.  Hydromet data in pisces database will" +
        " not be used");
            this.checkBoxHydrometWebOnly.UseVisualStyleBackColor = true;
            this.checkBoxHydrometWebOnly.CheckedChanged += new System.EventHandler(this.checkBoxHydrometWebOnly_CheckedChanged);
            // 
            // checkBoxHydrometWebCache
            // 
            this.checkBoxHydrometWebCache.AutoSize = true;
            this.checkBoxHydrometWebCache.Location = new System.Drawing.Point(32, 100);
            this.checkBoxHydrometWebCache.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxHydrometWebCache.Name = "checkBoxHydrometWebCache";
            this.checkBoxHydrometWebCache.Size = new System.Drawing.Size(302, 21);
            this.checkBoxHydrometWebCache.TabIndex = 2;
            this.checkBoxHydrometWebCache.Text = "use web cache (good for slow connections)";
            this.toolTip1.SetToolTip(this.checkBoxHydrometWebCache, "a web request repeated withing two minutes will be saved to disk to speed up grap" +
        "hing.");
            this.checkBoxHydrometWebCache.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(413, 458);
            this.buttonOK.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(100, 28);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(521, 458);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 28);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPageHydromet);
            this.tabControl1.Controls.Add(this.tabPageUsgs);
            this.tabControl1.Controls.Add(this.tabPageModsim);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPageDecodes);
            this.tabControl1.Controls.Add(this.tabPageCalcs);
            this.tabControl1.Location = new System.Drawing.Point(-3, 16);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(624, 434);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.checkBoxMultiYAxis);
            this.tabPage2.Controls.Add(this.checkBoxAutoRefresh);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Size = new System.Drawing.Size(616, 405);
            this.tabPage2.TabIndex = 5;
            this.tabPage2.Text = "General";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // checkBoxMultiYAxis
            // 
            this.checkBoxMultiYAxis.AutoSize = true;
            this.checkBoxMultiYAxis.Location = new System.Drawing.Point(41, 106);
            this.checkBoxMultiYAxis.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxMultiYAxis.Name = "checkBoxMultiYAxis";
            this.checkBoxMultiYAxis.Size = new System.Drawing.Size(342, 21);
            this.checkBoxMultiYAxis.TabIndex = 1;
            this.checkBoxMultiYAxis.Text = "dynamically create a separate y-axis for each unit";
            this.checkBoxMultiYAxis.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoRefresh
            // 
            this.checkBoxAutoRefresh.AutoSize = true;
            this.checkBoxAutoRefresh.Location = new System.Drawing.Point(41, 50);
            this.checkBoxAutoRefresh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxAutoRefresh.Name = "checkBoxAutoRefresh";
            this.checkBoxAutoRefresh.Size = new System.Drawing.Size(107, 21);
            this.checkBoxAutoRefresh.TabIndex = 0;
            this.checkBoxAutoRefresh.Text = "auto refresh";
            this.toolTip1.SetToolTip(this.checkBoxAutoRefresh, "when checked pisces updates the graph as you select items in the tree. Otherwise " +
        "use the refresh button after selecting items");
            this.checkBoxAutoRefresh.UseVisualStyleBackColor = true;
            // 
            // tabPageHydromet
            // 
            this.tabPageHydromet.Controls.Add(this.serverSelection1);
            this.tabPageHydromet.Controls.Add(this.groupBox1);
            this.tabPageHydromet.Location = new System.Drawing.Point(4, 25);
            this.tabPageHydromet.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageHydromet.Name = "tabPageHydromet";
            this.tabPageHydromet.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageHydromet.Size = new System.Drawing.Size(616, 405);
            this.tabPageHydromet.TabIndex = 0;
            this.tabPageHydromet.Text = "Hydromet/AgriMet";
            this.tabPageHydromet.UseVisualStyleBackColor = true;
            // 
            // tabPageUsgs
            // 
            this.tabPageUsgs.Controls.Add(this.checkBoxUsgsAutoUpdate);
            this.tabPageUsgs.Location = new System.Drawing.Point(4, 25);
            this.tabPageUsgs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageUsgs.Name = "tabPageUsgs";
            this.tabPageUsgs.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageUsgs.Size = new System.Drawing.Size(616, 405);
            this.tabPageUsgs.TabIndex = 1;
            this.tabPageUsgs.Text = "USGS";
            this.tabPageUsgs.UseVisualStyleBackColor = true;
            // 
            // checkBoxUsgsAutoUpdate
            // 
            this.checkBoxUsgsAutoUpdate.AutoSize = true;
            this.checkBoxUsgsAutoUpdate.Location = new System.Drawing.Point(31, 28);
            this.checkBoxUsgsAutoUpdate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxUsgsAutoUpdate.Name = "checkBoxUsgsAutoUpdate";
            this.checkBoxUsgsAutoUpdate.Size = new System.Drawing.Size(191, 21);
            this.checkBoxUsgsAutoUpdate.TabIndex = 1;
            this.checkBoxUsgsAutoUpdate.Text = "auto update from the web";
            this.checkBoxUsgsAutoUpdate.UseVisualStyleBackColor = true;
            // 
            // tabPageModsim
            // 
            this.tabPageModsim.Controls.Add(this.checkBoxModsimDisplayCfs);
            this.tabPageModsim.Location = new System.Drawing.Point(4, 25);
            this.tabPageModsim.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageModsim.Name = "tabPageModsim";
            this.tabPageModsim.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageModsim.Size = new System.Drawing.Size(616, 405);
            this.tabPageModsim.TabIndex = 3;
            this.tabPageModsim.Text = "MODSIM";
            this.tabPageModsim.UseVisualStyleBackColor = true;
            // 
            // checkBoxModsimDisplayCfs
            // 
            this.checkBoxModsimDisplayCfs.AutoSize = true;
            this.checkBoxModsimDisplayCfs.Location = new System.Drawing.Point(33, 33);
            this.checkBoxModsimDisplayCfs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxModsimDisplayCfs.Name = "checkBoxModsimDisplayCfs";
            this.checkBoxModsimDisplayCfs.Size = new System.Drawing.Size(134, 21);
            this.checkBoxModsimDisplayCfs.TabIndex = 0;
            this.checkBoxModsimDisplayCfs.Text = "show flows in cfs";
            this.toolTip1.SetToolTip(this.checkBoxModsimDisplayCfs, "check to display flow in cfs, otherwise flows are acre-feet");
            this.checkBoxModsimDisplayCfs.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.checkBoxExcelAutoUpdate);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Size = new System.Drawing.Size(616, 405);
            this.tabPage1.TabIndex = 4;
            this.tabPage1.Text = "Excel";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // checkBoxExcelAutoUpdate
            // 
            this.checkBoxExcelAutoUpdate.AutoSize = true;
            this.checkBoxExcelAutoUpdate.Location = new System.Drawing.Point(29, 39);
            this.checkBoxExcelAutoUpdate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxExcelAutoUpdate.Name = "checkBoxExcelAutoUpdate";
            this.checkBoxExcelAutoUpdate.Size = new System.Drawing.Size(350, 21);
            this.checkBoxExcelAutoUpdate.TabIndex = 2;
            this.checkBoxExcelAutoUpdate.Text = "reload when source Excel or CSV files are modified";
            this.toolTip1.SetToolTip(this.checkBoxExcelAutoUpdate, "when checked excel series will be updated, on next query, if the source excel fil" +
        "e has been modified");
            this.checkBoxExcelAutoUpdate.UseVisualStyleBackColor = true;
            // 
            // tabPageDecodes
            // 
            this.tabPageDecodes.Controls.Add(this.label1);
            this.tabPageDecodes.Controls.Add(this.textBoxDecodesPath);
            this.tabPageDecodes.Location = new System.Drawing.Point(4, 25);
            this.tabPageDecodes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageDecodes.Name = "tabPageDecodes";
            this.tabPageDecodes.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageDecodes.Size = new System.Drawing.Size(616, 405);
            this.tabPageDecodes.TabIndex = 6;
            this.tabPageDecodes.Text = "Data Import";
            this.tabPageDecodes.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Data import directory";
            // 
            // textBoxDecodesPath
            // 
            this.textBoxDecodesPath.Location = new System.Drawing.Point(48, 54);
            this.textBoxDecodesPath.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxDecodesPath.Name = "textBoxDecodesPath";
            this.textBoxDecodesPath.Size = new System.Drawing.Size(445, 22);
            this.textBoxDecodesPath.TabIndex = 0;
            // 
            // tabPageCalcs
            // 
            this.tabPageCalcs.Controls.Add(this.groupBox2);
            this.tabPageCalcs.Location = new System.Drawing.Point(4, 25);
            this.tabPageCalcs.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageCalcs.Name = "tabPageCalcs";
            this.tabPageCalcs.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPageCalcs.Size = new System.Drawing.Size(616, 405);
            this.tabPageCalcs.TabIndex = 7;
            this.tabPageCalcs.Text = "Calculations";
            this.tabPageCalcs.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxHydrometVariableResolver);
            this.groupBox2.Location = new System.Drawing.Point(27, 111);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(323, 91);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "variable resolving";
            // 
            // checkBoxHydrometVariableResolver
            // 
            this.checkBoxHydrometVariableResolver.AutoSize = true;
            this.checkBoxHydrometVariableResolver.Location = new System.Drawing.Point(8, 37);
            this.checkBoxHydrometVariableResolver.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxHydrometVariableResolver.Name = "checkBoxHydrometVariableResolver";
            this.checkBoxHydrometVariableResolver.Size = new System.Drawing.Size(247, 21);
            this.checkBoxHydrometVariableResolver.TabIndex = 0;
            this.checkBoxHydrometVariableResolver.Text = "variables reference hydromet data";
            this.toolTip1.SetToolTip(this.checkBoxHydrometVariableResolver, "query hydromet to resolve variable names such as  jck_af  (for jackson lake conte" +
        "nts)");
            this.checkBoxHydrometVariableResolver.UseVisualStyleBackColor = true;
            // 
            // serverSelection1
            // 
            this.serverSelection1.Location = new System.Drawing.Point(20, 165);
            this.serverSelection1.Name = "serverSelection1";
            this.serverSelection1.Size = new System.Drawing.Size(222, 176);
            this.serverSelection1.TabIndex = 3;
            // 
            // Options
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(628, 500);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MinimumSize = new System.Drawing.Size(487, 440);
            this.Name = "Options";
            this.Text = "Options";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPageHydromet.ResumeLayout(false);
            this.tabPageUsgs.ResumeLayout(false);
            this.tabPageUsgs.PerformLayout();
            this.tabPageModsim.ResumeLayout(false);
            this.tabPageModsim.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPageDecodes.ResumeLayout(false);
            this.tabPageDecodes.PerformLayout();
            this.tabPageCalcs.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxHydrometAutoUpdate;
        private System.Windows.Forms.CheckBox checkBoxHydrometIncludeFlaggedData;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxHydrometWebCache;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageHydromet;
        private System.Windows.Forms.TabPage tabPageUsgs;
        private System.Windows.Forms.CheckBox checkBoxUsgsAutoUpdate;
        private System.Windows.Forms.CheckBox checkBoxHydrometWebOnly;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabPage tabPageModsim;
        private System.Windows.Forms.CheckBox checkBoxModsimDisplayCfs;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox checkBoxExcelAutoUpdate;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox checkBoxAutoRefresh;
        private System.Windows.Forms.TabPage tabPageDecodes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxDecodesPath;
        private System.Windows.Forms.CheckBox checkBoxMultiYAxis;
        private System.Windows.Forms.TabPage tabPageCalcs;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxHydrometVariableResolver;
        private Hydromet.ServerSelection serverSelection1;
    }
}