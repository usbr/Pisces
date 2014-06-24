using Reclamation.TimeSeries.Forms;

namespace Reclamation.TimeSeries.Forms
{
    partial class ExplorerForm 
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuView = new System.Windows.Forms.ToolStripMenuItem();
            this.logToolStripMenuItemViewLog = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDatabase = new System.Windows.Forms.ToolStripMenuItem();
            this.logToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseInternalsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.menuScenarios = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripTree = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuRrefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemNewFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.newSeries = new System.Windows.Forms.ToolStripMenuItem();
            this.newExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.newHydromet = new System.Windows.Forms.ToolStripMenuItem();
            this.newTextFile = new System.Windows.Forms.ToolStripMenuItem();
            this.newUsgs = new System.Windows.Forms.ToolStripMenuItem();
            this.newAccess = new System.Windows.Forms.ToolStripMenuItem();
            this.menuImport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuImportExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMDB = new System.Windows.Forms.ToolStripMenuItem();
            this.menuImportUSGS = new System.Windows.Forms.ToolStripMenuItem();
            this.menuImportHydromet = new System.Windows.Forms.ToolStripMenuItem();
            this.menuImportTextFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuData = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDataConvertToDaily = new System.Windows.Forms.ToolStripMenuItem();
            this.sumSeriesListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.appendAverageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.factorSeriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.openTextFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.openExcelDialog = new System.Windows.Forms.OpenFileDialog();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStripTree.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1,
            this.toolStripStatusMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 456);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(693, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            this.toolStripProgressBar1.Step = 5;
            this.toolStripProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.toolStripProgressBar1.Visible = false;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusMessage
            // 
            this.toolStripStatusMessage.Name = "toolStripStatusMessage";
            this.toolStripStatusMessage.Size = new System.Drawing.Size(0, 17);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuFile,
            this.menuView,
            this.menuDatabase,
            this.menuOptions,
            this.menuScenarios});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(693, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuFile
            // 
            this.menuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.menuFile.Name = "menuFile";
            this.menuFile.Size = new System.Drawing.Size(35, 20);
            this.menuFile.Text = "&File";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // menuView
            // 
            this.menuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logToolStripMenuItemViewLog});
            this.menuView.Name = "menuView";
            this.menuView.Size = new System.Drawing.Size(41, 20);
            this.menuView.Text = "&View";
            // 
            // logToolStripMenuItemViewLog
            // 
            this.logToolStripMenuItemViewLog.Name = "logToolStripMenuItemViewLog";
            this.logToolStripMenuItemViewLog.Size = new System.Drawing.Size(102, 22);
            this.logToolStripMenuItemViewLog.Text = "&Log";
            this.logToolStripMenuItemViewLog.Click += new System.EventHandler(this.logToolStripMenuItemViewLog_Click);
            // 
            // menuDatabase
            // 
            this.menuDatabase.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logToolStripMenuItem,
            this.databaseInternalsToolStripMenuItem});
            this.menuDatabase.Name = "menuDatabase";
            this.menuDatabase.Size = new System.Drawing.Size(65, 20);
            this.menuDatabase.Text = "&Database";
            // 
            // logToolStripMenuItem
            // 
            this.logToolStripMenuItem.Name = "logToolStripMenuItem";
            this.logToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.logToolStripMenuItem.Text = "&SQL History";
            this.logToolStripMenuItem.Click += new System.EventHandler(this.logToolStripMenuItem_Click);
            // 
            // databaseInternalsToolStripMenuItem
            // 
            this.databaseInternalsToolStripMenuItem.Name = "databaseInternalsToolStripMenuItem";
            this.databaseInternalsToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.databaseInternalsToolStripMenuItem.Text = "&Tables";
            this.databaseInternalsToolStripMenuItem.Click += new System.EventHandler(this.databaseInternalsToolStripMenuItem_Click);
            // 
            // menuOptions
            // 
            this.menuOptions.Name = "menuOptions";
            this.menuOptions.Size = new System.Drawing.Size(109, 20);
            this.menuOptions.Text = "Option:Time Series";
            this.menuOptions.Click += new System.EventHandler(this.toolStripDisplayType_Click);
            // 
            // menuScenarios
            // 
            this.menuScenarios.Name = "menuScenarios";
            this.menuScenarios.Size = new System.Drawing.Size(65, 20);
            this.menuScenarios.Text = "&Scenarios";
            this.menuScenarios.Click += new System.EventHandler(this.toolStripMenuItemScenarios_Click);
            // 
            // contextMenuStripTree
            // 
            this.contextMenuStripTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuRrefresh,
            this.toolStripSeparator2,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.menuNew,
            this.menuImport,
            this.toolStripMenuItem1,
            this.toolStripSeparator3,
            this.menuDelete,
            this.toolStripSeparator5,
            this.menuData,
            this.toolStripSeparator4,
            this.menuProperties});
            this.contextMenuStripTree.Name = "contextMenuStripTree";
            this.contextMenuStripTree.Size = new System.Drawing.Size(151, 226);
            // 
            // menuRrefresh
            // 
            this.menuRrefresh.Name = "menuRrefresh";
            this.menuRrefresh.Size = new System.Drawing.Size(150, 22);
            this.menuRrefresh.Text = "&Refresh";
            this.menuRrefresh.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(147, 6);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Enabled = false;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.ShortcutKeyDisplayString = "";
            this.toolStripMenuItem2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.toolStripMenuItem2.Size = new System.Drawing.Size(150, 22);
            this.toolStripMenuItem2.Text = "&Copy";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Enabled = false;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.toolStripMenuItem3.Size = new System.Drawing.Size(150, 22);
            this.toolStripMenuItem3.Text = "&Paste";
            // 
            // menuNew
            // 
            this.menuNew.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemNewFolder,
            this.toolStripSeparator6,
            this.newSeries,
            this.newExcel,
            this.newHydromet,
            this.newTextFile,
            this.newUsgs,
            this.newAccess});
            this.menuNew.Name = "menuNew";
            this.menuNew.Size = new System.Drawing.Size(150, 22);
            this.menuNew.Text = "&New";
            // 
            // toolStripMenuItemNewFolder
            // 
            this.toolStripMenuItemNewFolder.Name = "toolStripMenuItemNewFolder";
            this.toolStripMenuItemNewFolder.Size = new System.Drawing.Size(236, 22);
            this.toolStripMenuItemNewFolder.Text = "Folder";
            this.toolStripMenuItemNewFolder.Click += new System.EventHandler(this.newFolder_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(233, 6);
            // 
            // newSeries
            // 
            this.newSeries.Name = "newSeries";
            this.newSeries.Size = new System.Drawing.Size(236, 22);
            this.newSeries.Text = "Series";
            this.newSeries.Click += new System.EventHandler(this.newSeries_Click);
            // 
            // newExcel
            // 
            this.newExcel.Name = "newExcel";
            this.newExcel.Size = new System.Drawing.Size(236, 22);
            this.newExcel.Text = "&Excel Series...";
            this.newExcel.Click += new System.EventHandler(this.newExcel_Click);
            // 
            // newHydromet
            // 
            this.newHydromet.Name = "newHydromet";
            this.newHydromet.Size = new System.Drawing.Size(236, 22);
            this.newHydromet.Text = "&Hydromet Series...";
            this.newHydromet.Click += new System.EventHandler(this.newHydromet_Click);
            // 
            // newTextFile
            // 
            this.newTextFile.Name = "newTextFile";
            this.newTextFile.Size = new System.Drawing.Size(236, 22);
            this.newTextFile.Text = "&Text file Series(*.txt, *.csv) ...";
            this.newTextFile.Click += new System.EventHandler(this.newTextFile_Click);
            // 
            // newUsgs
            // 
            this.newUsgs.Name = "newUsgs";
            this.newUsgs.Size = new System.Drawing.Size(236, 22);
            this.newUsgs.Text = "&USGS Series...";
            // 
            // newAccess
            // 
            this.newAccess.Name = "newAccess";
            this.newAccess.Size = new System.Drawing.Size(236, 22);
            this.newAccess.Text = "&Access Series...";
            // 
            // menuImport
            // 
            this.menuImport.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuImportExcel,
            this.toolStripMenuItemMDB,
            this.menuImportUSGS,
            this.menuImportHydromet,
            this.menuImportTextFile});
            this.menuImport.Name = "menuImport";
            this.menuImport.Size = new System.Drawing.Size(150, 22);
            this.menuImport.Text = "&Import ";
            // 
            // menuImportExcel
            // 
            this.menuImportExcel.Name = "menuImportExcel";
            this.menuImportExcel.Size = new System.Drawing.Size(174, 22);
            this.menuImportExcel.Text = "&Excel Spreadsheet";
            this.menuImportExcel.Click += new System.EventHandler(this.menuImportExcel_Click);
            // 
            // toolStripMenuItemMDB
            // 
            this.toolStripMenuItemMDB.Enabled = false;
            this.toolStripMenuItemMDB.Name = "toolStripMenuItemMDB";
            this.toolStripMenuItemMDB.Size = new System.Drawing.Size(174, 22);
            this.toolStripMenuItemMDB.Text = "Access (MDB)";
            // 
            // menuImportUSGS
            // 
            this.menuImportUSGS.Name = "menuImportUSGS";
            this.menuImportUSGS.Size = new System.Drawing.Size(174, 22);
            this.menuImportUSGS.Text = "&USGS";
            this.menuImportUSGS.Click += new System.EventHandler(this.importUSGS_Click);
            // 
            // menuImportHydromet
            // 
            this.menuImportHydromet.Name = "menuImportHydromet";
            this.menuImportHydromet.Size = new System.Drawing.Size(174, 22);
            this.menuImportHydromet.Text = "&Hydromet";
            this.menuImportHydromet.Click += new System.EventHandler(this.hydrometDailyToolStripMenuItem_Click);
            // 
            // menuImportTextFile
            // 
            this.menuImportTextFile.Name = "menuImportTextFile";
            this.menuImportTextFile.Size = new System.Drawing.Size(174, 22);
            this.menuImportTextFile.Text = "&Text File";
            this.menuImportTextFile.Click += new System.EventHandler(this.importTextFile_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Enabled = false;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(150, 22);
            this.toolStripMenuItem1.Text = "&Export";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(147, 6);
            // 
            // menuDelete
            // 
            this.menuDelete.Name = "menuDelete";
            this.menuDelete.Size = new System.Drawing.Size(150, 22);
            this.menuDelete.Text = "Delete";
            this.menuDelete.Click += new System.EventHandler(this.delete_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(147, 6);
            // 
            // menuData
            // 
            this.menuData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemDataConvertToDaily,
            this.sumSeriesListToolStripMenuItem,
            this.appendAverageToolStripMenuItem,
            this.factorSeriesToolStripMenuItem});
            this.menuData.Name = "menuData";
            this.menuData.Size = new System.Drawing.Size(150, 22);
            this.menuData.Text = "Math";
            // 
            // toolStripMenuItemDataConvertToDaily
            // 
            this.toolStripMenuItemDataConvertToDaily.Name = "toolStripMenuItemDataConvertToDaily";
            this.toolStripMenuItemDataConvertToDaily.Size = new System.Drawing.Size(175, 22);
            this.toolStripMenuItemDataConvertToDaily.Text = "&Convert to Daily...";
            this.toolStripMenuItemDataConvertToDaily.Click += new System.EventHandler(this.toolStripMenuItemDataConvertToDaily_Click);
            // 
            // sumSeriesListToolStripMenuItem
            // 
            this.sumSeriesListToolStripMenuItem.Name = "sumSeriesListToolStripMenuItem";
            this.sumSeriesListToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.sumSeriesListToolStripMenuItem.Text = "Sum";
            this.sumSeriesListToolStripMenuItem.Click += new System.EventHandler(this.sumSeriesListToolStripMenuItem_Click);
            // 
            // appendAverageToolStripMenuItem
            // 
            this.appendAverageToolStripMenuItem.Name = "appendAverageToolStripMenuItem";
            this.appendAverageToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.appendAverageToolStripMenuItem.Text = "Append Average";
            this.appendAverageToolStripMenuItem.Click += new System.EventHandler(this.appendAverageToolStripMenuItem_Click);
            // 
            // factorSeriesToolStripMenuItem
            // 
            this.factorSeriesToolStripMenuItem.Name = "factorSeriesToolStripMenuItem";
            this.factorSeriesToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.factorSeriesToolStripMenuItem.Text = "Factor Series";
            this.factorSeriesToolStripMenuItem.Click += new System.EventHandler(this.factorSeriesToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(147, 6);
            // 
            // menuProperties
            // 
            this.menuProperties.Name = "menuProperties";
            this.menuProperties.Size = new System.Drawing.Size(150, 22);
            this.menuProperties.Text = "&Properties";
            this.menuProperties.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "*.pisces";
            this.saveFileDialog1.Filter = "Pisces Database files (*.pisces) |*.pisces |All files (*.*)|*.*";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Size = new System.Drawing.Size(693, 432);
            this.splitContainer1.SplitterDistance = 254;
            this.splitContainer1.SplitterWidth = 16;
            this.splitContainer1.TabIndex = 2;
            // 
            // openTextFileDialog
            // 
            this.openTextFileDialog.DefaultExt = "txt";
            this.openTextFileDialog.Filter = "Comma Separated (*.csv) |*.csv|Text (*.txt)|*.txt|All Files (*.*)|*.*\";";
            // 
            // openExcelDialog
            // 
            this.openExcelDialog.DefaultExt = "txt";
            this.openExcelDialog.Filter = "Excel Spreadsheet (*.xls)|*.xls|All Files (*.*)|*.*\";";
            // 
            // ExplorerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 478);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ExplorerForm";
            this.Text = "Title";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ExplorerForm_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStripTree.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuFile;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuScenarios;
        private System.Windows.Forms.ToolStripMenuItem menuOptions;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTree;
        private System.Windows.Forms.ToolStripMenuItem menuRrefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuNew;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemNewFolder;
        private System.Windows.Forms.ToolStripMenuItem menuImport;
        private System.Windows.Forms.ToolStripMenuItem menuImportExcel;
        private System.Windows.Forms.ToolStripMenuItem menuImportUSGS;
        private System.Windows.Forms.ToolStripMenuItem menuImportHydromet;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuProperties;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuDatabase;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuDelete;
        private System.Windows.Forms.ToolStripMenuItem databaseInternalsToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusMessage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem menuData;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDataConvertToDaily;
        private System.Windows.Forms.ToolStripMenuItem menuView;
        private System.Windows.Forms.ToolStripMenuItem logToolStripMenuItemViewLog;
        private System.Windows.Forms.ToolStripMenuItem menuImportTextFile;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem appendAverageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sumSeriesListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMDB;
        private System.Windows.Forms.ToolStripMenuItem factorSeriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.OpenFileDialog openTextFileDialog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem newSeries;
        private System.Windows.Forms.ToolStripMenuItem newUsgs;
        private System.Windows.Forms.ToolStripMenuItem newHydromet;
        private System.Windows.Forms.ToolStripMenuItem newTextFile;
        private System.Windows.Forms.ToolStripMenuItem newAccess;
        private System.Windows.Forms.ToolStripMenuItem newExcel;
        private System.Windows.Forms.OpenFileDialog openExcelDialog;

    }
}

