//#define CATCH_EXCEPTION
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries.Forms;
using Reclamation.TimeSeries.Forms.ImportForms;
using Reclamation.Core;
using System.IO;
using System.Diagnostics;

namespace Reclamation.TimeSeries.Forms
{
    /// <summary>
    /// ExplorerForm displays a Tree containing time series data on
    /// the left. the right displays a graph and table based on user interface 
    /// settings such as the selections in the tree.
    /// </summary>
    [Obsolete("create your own Explorer Form")]
    public partial class ExplorerForm : Form
    {
        private Explorer explorer1;
        private DisplayOptionsDialog displayOptionsDialog1;
        private ScenarioSelector scenarioChooser1;

        public ExplorerForm(Explorer explorer)
        {
            SeriesCatalogTree treeView = new SeriesCatalogTree(explorer.Database);
            InitializePisces(explorer,treeView,treeView);
        }
        //public ExplorerForm(Explorer explorer, UserControl treeControl,
        //    SeriesCatalogTree treeView)
        //{
        //    InitializePisces(explorer,treeControl,treeView);
        //}
        TeeChartExplorerView viewer1;
        SeriesCatalogTree tree1;
        
        private void InitializePisces(Explorer explorer, 
            UserControl treeControl,
            SeriesCatalogTree treeView)
        {
            InitializeComponent();
            this.explorer1 = explorer;
            viewer1 = new TeeChartExplorerView();
            viewer1.Parent = this.splitContainer1.Panel2;
            viewer1.BringToFront();
            viewer1.Dock = DockStyle.Fill;
            viewer1.Visible = true;
            treeControl.ContextMenuStrip = this.contextMenuStripTree;
            treeControl.Parent = this.splitContainer1.Panel1;
            treeControl.Dock = DockStyle.Fill;
            //explorer1.TreeView = treeView;
            tree1 = treeView;
            tree1.SelectionChanged += new EventHandler(tree1_SelectionChanged);
            explorer1.View = viewer1;
            Rebuild(explorer);
        }

        void explorer1_SeriesCatalogChanged(object sender, EventArgs e)
        {
            tree1.RefreshTree();
            //tree1.SetSeriesCatalog(explorer1.SeriesCatalog);
        }



        public TimeSeriesDatabase DB
        {
            get
            {
                return explorer1.Database;// explorer1.SeriesCatalog.Database as SqlTimeSeriesDatabase;
            }
        }



        public void Rebuild(Explorer explorer)
        {
            explorer.OnProgress += new ProgressEventHandler(explorer_OnProgress);
            this.explorer1.View = viewer1;
            displayOptionsDialog1 =
               new TimeSeries.Forms.DisplayOptionsDialog(explorer1);
            SetupScenarios();
            if (DB.Server == null)
            {
                this.menuDatabase.Visible = false;
            }
            else
            {
                this.menuDatabase.Visible = true;
            }
            
        }

        void explorer_OnProgress(object sender, ProgressEventArgs e)
        {
            toolStripStatusLabel1.Text = e.Message;
        }

       
        private void SetupScenarios()
        {
            scenarioChooser1 = new ScenarioSelector(explorer1.ScenarioNames);
            scenarioChooser1.AllowCustomScenarioTable = explorer1.AllowCustomScenarioTable;
            scenarioChooser1.OnCustomizeScenarioTable += new CustomScenarioEventHandler(scenarioChooser1_OnCustomizeScenarioTable);
            scenarioChooser1.OnApply += new EventHandler(scenarioChooser1_OnApply);
            scenarioChooser1.OnCancel += new EventHandler(scenarioChooser1_OnCancel);
            UpdateScenariosMenuLabel();

            if (explorer1.ScenarioListVisible)// 
            {
                menuScenarios.Visible = true;
            }
            else
            {
                menuScenarios.Visible = false;
            }
        }
        void sumSeriesListToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            //List<string> list = new List<string>();
            //string[] listOfSeries;
            //DataRow row;
            //DataTable dt = tree1.SelectedRows;
            //if (tree1.SelectedCount < 2)
            //{
            //    MessageBox.Show("Select more than one series to sum");
            //    return;
            //}
            //for (int d = 0; d < explorer1.SelectedScenarios.Length; d++)
            //{
            //    SeriesList sl = new SeriesList();
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        row = dt.Rows[i];
            //        list.Add(Convert.ToString(row["Name"]));
            //        Series s = explorer1.SeriesCatalog.DatabaseList[d].GetSeries(row);
            //        s.Read();
            //        sl.Add(s);
            //    }
            //    listOfSeries = new string[list.Count];
            //    listOfSeries = list.ToArray();
            //    DateTime t1 = DateTime.MinValue;
            //    DateTime t2 = DateTime.MaxValue;
            //    for (int i = 0; i < sl.Count; i++)
            //    {
            //        if (sl[i].MinDateTime > t1) t1 = sl[i].MinDateTime;
            //        if (sl[i].MaxDateTime < t2) t2 = sl[i].MaxDateTime;
            //    }
            //    SeriesListSumForm sum = new SeriesListSumForm(t1, t2, listOfSeries);
            //    if (sum.ShowDialog() == DialogResult.OK && sum.sName != "")
            //    {
            //        Series s = sl.Sum(sum.t1, sum.t2);
            //        s.Name = sum.sName;
            //        s.SiteName = s.Name;
            //        SeriesInfo si = explorer1.SeriesCatalog.NewSeriesInfo();
            //        si.TimeInterval = s.TimeInterval.ToString();
            //        si.ConnectionString = "seriesListSum";
            //        si.Source = "database computed";
            //        si.DatabaseName = explorer1.SeriesCatalog.DatabaseList[d].Name;
            //        si.Units = s.Units;
            //        si.SiteName = s.Name;
            //        si.TableName = s.Name;
            //        si.Name = s.Name; // title for tree
            //        //int kount = Convert.ToInt32(explorer1.SeriesCatalog.GetTree().Rows.Count);
            //        //si.ParentID = Convert.ToInt32(explorer1.SeriesCatalog.GetTree().Rows[kount - 1]["ParentID"]);
            //        si.ParentID = 0;
            //        explorer1.SeriesCatalog.Add(si);
            //        explorer1.SeriesCatalog.DatabaseList[d].ImportTimeSeriesTable(s.Table, si,false);
            //        explorer1.SeriesCatalog.DatabaseList[d].Save();
            //    }
            //}
            //tree1.RefreshTree();
        }

        void appendAverageToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            //int sdi = tree1.SelectedID;
       //     SeriesDataDialogs.AvgAppendDialog(explorer1.SeriesCatalog.Database, sdi);
        }

        void scenarioChooser1_OnCancel(object sender, EventArgs e)
        {
            UpdateScenariosMenuLabel();
        }

        void scenarioChooser1_OnApply(object sender, EventArgs e)
        {
            UpdateView();
            UpdateScenariosMenuLabel();
        }

        void scenarioChooser1_OnCustomizeScenarioTable(object sender, ScenarioTableEventArgs ea)
        {
            explorer1.CustomizeScenarioTable(ea);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public string Title
        {
            set { this.Text = value; }
        }
        private void FormMain_Load(object sender, EventArgs e)
        {
            tree1.RefreshTree();
        }

        private void tree1_SelectionChanged(object sender, EventArgs e)
        {
            DrawBasedOnTreeSelection();
        }

        private void DrawBasedOnTreeSelection()
        {
            EnableContextMenus();
            UpdateView();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                explorer1.View.Draw();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
                toolStripProgressBar1.Visible = false;
                toolStripStatusLabel1.Text = "";
            }
        }

        private void UpdateView()
        {
            if (backgroundWorker1.IsBusy)
            {
                MessageBox.Show("Warning: The system is still busy");
                return;
            }
            toolStripProgressBar1.Visible = true;
            Cursor = Cursors.WaitCursor;
            //explorer1.SelectedSites = tree1.SelectedRows;
            explorer1.Selected = tree1.SelectedSeries;
            explorer1.SelectedScenarios = scenarioChooser1.Selected;
            explorer1.SubtractFromBaseline = scenarioChooser1.SubtractFromBaseline;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
#if (CATCH_EXCEPTION)
            try
            {
#endif
                explorer1.Run();
#if (CATCH_EXCEPTION)

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\nline 166 n" + ex.StackTrace);
            }
#endif
        }
       
       

        private void tree1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void toolStripMenuItemScenarios_Click(object sender, EventArgs e)
        {
            scenarioChooser1.Show();
        }

        private void UpdateScenariosMenuLabel()
        {
            string[] scenarios = scenarioChooser1.Selected;
            if (scenarios.Length == 1)
            {
                this.menuScenarios.Text = "Scenarios: " + scenarios[0];
            }
            else
            {
                this.menuScenarios.Text = "Scenarios:(" + scenarios.Length + ") selected";
            }
        }

        private void toolStripDisplayType_Click(object sender, EventArgs e)
        {
            if (displayOptionsDialog1.ShowDialog() == DialogResult.OK)
            {
                DrawBasedOnTreeSelection(); //tree1_Selected(this, new EventArgs());
            }

            this.menuOptions.Text = "Option:" + explorer1.SelectedTimeSeriesAnalysis.Name; 
        }

        

        private void ExplorerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

      
        private void newFolder_Click(object sender, EventArgs e)
        {
           
            int parentID = tree1.SelectedID;
            int id = DB.AddFolder( parentID);
            tree1.RefreshTree();
        }

        private void EnableContextMenus()
        {
            menuNew.Enabled = tree1.IsFolderSelected == true;
            menuData.Enabled = tree1.IsFolderSelected == false;
            menuImport.Enabled = tree1.IsFolderSelected == false;
            if (menuImport.Enabled == true && tree1.SelectedCount > 1) 
                menuImport.Enabled = false;
            menuDelete.Enabled = tree1.SelectedCount > 0;
            menuProperties.Enabled = tree1.SelectedCount == 1;

            appendAverageToolStripMenuItem.Enabled = tree1.SelectedCount == 1;
            factorSeriesToolStripMenuItem.Enabled = tree1.SelectedCount == 1;
            sumSeriesListToolStripMenuItem.Enabled = tree1.SelectedCount > 1;
            newSeries.Enabled = tree1.IsFolderSelected == true;
            toolStripMenuItemDataConvertToDaily.Enabled = false;
            if (tree1.IsFolderSelected == false && tree1.SelectedCount == 1 )
               // DB.ReadOnly == false)
            {
                toolStripMenuItemDataConvertToDaily.Enabled = true;
                // TODO enable convert to Daily for appropriate time steps.
            }
        }

        private void EnableMenus()
        {
            Reclamation.Core.BasicDBServer svr = DB.Server;

         if (svr != null)
         {
             menuDatabase.Visible = true;
         }
         else
         {
             menuDatabase.Visible = true;
         }
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if( DB.Server != null)
            {
                Reclamation.Core.SqlView v = new Reclamation.Core.SqlView(DB.Server);
                v.ShowDialog();
            }
        }

        private void databaseInternalsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reclamation.Core.FormTableEditor edit =
                new Reclamation.Core.FormTableEditor(DB.Server);
            edit.ShowDialog();
            tree1.RefreshTree();
        }

        private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //.SeriesInfo si = tree1.SelectedSiteInfo;
            Series s = DB.GetSeries(tree1.SelectedID);
            SeriesProperties pf = new SeriesProperties(s, DB.GetUniqueUnits());
            if (pf.ShowDialog() == DialogResult.OK)
            {
                DB.UpdateSeriesProperties(s, tree1.SelectedID);
               
                tree1.RefreshTree();
            }
        }

       

       

        private void delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Click Yes to delete", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                
                foreach (var id in tree1.SelectedSeries)
                {
                    DB.Delete(id);
                }
                //tree1.SelectParent();
                tree1.RefreshTree();
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tree1.RefreshTree();
        }
        private void hydrometDailyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SeriesInfo si = tree1.SelectedSiteInfo;
            //TimeSeriesDatabase db = explorer1.SeriesCatalog.Database;
            //dataDialog.HydrometDialog(db, si);
            //Series s = dataDialog.Series;
            //if(s != null && s.Table.Rows.Count > 0)
            //    hasHydromet = true;
            //tree1.RefreshTree();
        }

        //public void RefreshTree()
        //{
        //    tree1.RefreshTree();
        //}


        public ToolStripMenuItem MenuImport
        {
            get { return this.menuImport; }
        }

        public ToolStripItemCollection MenuItems
        {
            get { return this.menuStrip1.Items; }
        }


        public void SetMessage(string msg)
        {
            this.toolStripStatusLabel1.Text = msg;
            Application.DoEvents();
        }
        public ToolStripMenuItem ViewMenu
        {
            get { return menuView; }
        }

        public ToolStripMenuItem FileMenu
        {
            get { return menuFile; }
        }

        private void toolStripMenuItemDataConvertToDaily_Click(object sender, EventArgs e)
        {
            int sdi = tree1.SelectedID;
            int id = DB.ConvertToDaily(sdi);
            tree1.RefreshTree();
        }

        //void convertToMonthlyToolStripMenuItem_Click(object sender, System.EventArgs e)
        //{
            //SeriesCatalog sc = explorer1.SeriesCatalog;
            //SeriesInfo ssi = sc.GetSeriesInfo(tree1.SelectedID);
            //if (ssi.TimeInterval != "Daily")
            //{
            //    MessageBox.Show("Must be a Daily time interval");
            //    return;
            //}
            //int parent = ssi.ParentID;
            //SeriesProperties sf = new SeriesProperties(sc.GetUniqueUnits);

            //sf.TimeInterval = TimeInterval.Monthly.ToString();

            //if (sf.ShowDialog() == DialogResult.OK)
            //{
            //    SeriesInfo si = sc.NewSeriesInfo(parent);
                
            //    si.Name = sf.SeriesName;
            //    si.SiteName = sf.SeriesName;
            //    //si.TimeInterval = TimeInterval.Monthly.ToString();
            //    si.Units = sf.Units;

            //    DataTable mt = SeriesDataDialogs.Daily2Monthly(ssi, explorer1.SeriesCatalog.Database);
            //    if (mt != null && mt.Rows.Count > 0)
            //    {
            //        explorer1.SeriesCatalog.Add(si);
            //        DB.ImportTimeSeriesTable(mt, si);
            //    }
            //}
            //tree1.RefreshTree();
        //}

        private void logToolStripMenuItemViewLog_Click(object sender, EventArgs e)
        {
            LoggerView v = new LoggerView();
            v.ShowDialog();
        }

        private void menuImportExcel_Click(object sender, EventArgs e)
        {
            //SeriesInfo si = tree1.SelectedSiteInfo;
            //TimeSeriesDatabase db = explorer1.SeriesCatalog.Database;
            //dataDialog.ExcelDialog(db, si, true);

            //tree1.RefreshTree();
        }

       

       
        void factorSeriesToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            //TimeSeriesDatabase db = explorer1.SeriesCatalog.Database;
            //SeriesInfo osi = tree1.SelectedSeriesInfo[0];
            //SeriesDataDialogs.FactorSeriesDialog(db, osi);
            //tree1.RefreshTree();
        }

        void newSeries_Click(object sender, System.EventArgs e)
        {
            DB.AddSeries(new Series(), tree1.SelectedID);

            tree1.RefreshTree();
            //SeriesInfo si = explorer1.SeriesCatalog.NewSeriesInfo(tree1.SelectedID);
            //SingleTextBox tb = new SingleTextBox("Series Name", si.Name);
            //if (tb.ShowDialog() == DialogResult.OK)
            //{
            //    explorer1.SeriesCatalog.Add(si);
            //    RefreshTree();
            //}
        }

        /// <summary>
        /// Import a text file into an existing series
        /// </summary>
        private void importTextFile_Click(object sender, EventArgs e)
        {
            if (openTextFileDialog.ShowDialog() == DialogResult.OK)
            {
                Series s = TextSeries.ReadFromFile(openTextFileDialog.FileName);
                if (s.Count == 0)
                {
                    MessageBox.Show("No data found in file:"+openTextFileDialog.FileName);
                    return;
                }
                if ( s.Count > 0)
                {
                    DB.UpdateTimeSeriesTable(tree1.SelectedID, s, true);
                }
            }
        }

        /// <summary>
        /// Creates a new series from a text file
        /// </summary>
        void newTextFile_Click(object sender, System.EventArgs e)
        {
            if (openTextFileDialog.ShowDialog() == DialogResult.OK)
            {
                Series s = TextSeries.ReadFromFile(openTextFileDialog.FileName);

                if (s.Count == 0)
                {
                    MessageBox.Show("No data found in file:" + openTextFileDialog.FileName);
                    return;
                }
          
                
                if (s.Count > 0)
                {
                    DB.AddSeries(s, tree1.SelectedID);
                    tree1.RefreshTree();
                }
            }
        }

        /// <summary>
        /// Creates a new Usgs Series from the internet
        /// </summary>
        void createNewUsgsSeries_Click(object sender, System.EventArgs e)
        {
            ImportUsgs dlg = new ImportUsgs();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Series s = new Series();
                if (dlg.Daily)
                {
                  s = Usgs.UsgsDailyValueSeries.Read(dlg.SiteID, dlg.UsgsDailyParameter,
                      dlg.T1, dlg.T2);
                }
                if (dlg.Instantaneous)
                {
                  s = Usgs.UsgsInstantaneousSeries.Read(dlg.SiteID, dlg.T1, dlg.T2);
                }

                if (s.Count == 0)
                {
                    MessageBox.Show("Error: no USGS data pwas found");
                    return;
                }
                DB.AddSeries(s, tree1.SelectedID);
                tree1.RefreshTree();
            }
        }

        /// <summary>
        /// Imports USGS data into an existing series
        /// </summary>
        private void importUSGS_Click(object sender, EventArgs e)
        {
            Series s = DB.GetSeries(tree1.SelectedID);
            string site_no = "1350000";
            Usgs.UsgsDailyParameter par = Reclamation.TimeSeries.Usgs.UsgsDailyParameter.DailyMeanDischarge;

            if (s.Source.IndexOf("USGS") >= 0)
            {// help user by looking up previous USGS site_no
                site_no = Usgs.Utility.SiteNumberFromConnectionString(s.ConnectionString);
                par = Usgs.Utility.DailyParameterFromString(s.Parameter);
            }
             // TODO: Daily    and instant need separate UI..
            // instant shoud not be specifing daily parameter (data type)
            DateTime  t1 = new DateTime(1989, 10, 1);
            DateTime t2 = DateTime.Now.AddDays(-1);
            if (s.PeriodOfRecord.Count > 0)
            {
                t1 = s.PeriodOfRecord.T1;
                t2 = s.PeriodOfRecord.T2;
            }
            ImportUsgs dlg = new ImportUsgs(site_no,par,t1,t2);

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if (dlg.Daily)
                {
                    s = Usgs.UsgsDailyValueSeries.Read(dlg.SiteID, dlg.UsgsDailyParameter,
                        dlg.T1, dlg.T2);
                }
                if (dlg.Instantaneous)
                {
                    s = Usgs.UsgsInstantaneousSeries.Read(dlg.SiteID, dlg.T1, dlg.T2);
                }

                if (s.Count == 0)
                {
                    MessageBox.Show("Error: no USGS data pwas found");
                    return;
                }
                DB.UpdateTimeSeriesTable(tree1.SelectedID, s, true);
            }
            //Series s = dataDialog.Series;
            //if (s.Table.Rows.Count > 0)
            //{
            //    db.ImportTimeSeriesTable(s.Table, si, dataDialog.OverWrite);
            //}
        }
       


        void newHydromet_Click(object sender, System.EventArgs e)
        {

            ImportHydromet dlg = new ImportHydromet();
                  
                   if(  dlg.ShowDialog() == DialogResult.OK)
                      {
                          Series s = Hydromet.Hydromet.Read( dlg.Cbtt,dlg.ParameterCode,dlg.T1,dlg.T2,
                           dlg.TimeInterval,dlg.HydrometServer );

                        if( s.Count ==0)
                         {
                             MessageBox.Show("Error: Could not find any Hydromet Data");
                          return;
                         }
                        DB.AddSeries(s, tree1.SelectedID);
                        tree1.RefreshTree();  
                      }
              
              
             


            //SeriesInfo si = explorer1.SeriesCatalog.NewSeriesInfo(tree1.SelectedID);
            //dataDialog.HydrometDialog(DB, si);

            //Series s = dataDialog.Series; 
            //if (s != null && s.Table.Rows.Count > 0)
            //{
            //    if(DB.SeriesCatalog.Exists(si.SiteDataTypeID) == false)
            //        DB.SeriesCatalog.Add(si);
            //    DB.ImportTimeSeriesTable(s.Table, si, dataDialog.OverWrite);
            //    hasHydromet = true;
            //}
            //RefreshTree();
        }




        public void UpdateScenarioList(DataTable scenarioTable)
        {
            scenarioChooser1.SetTable(scenarioTable);
        }

        private void newExcel_Click(object sender, EventArgs e)
        {
            if (openExcelDialog.ShowDialog() == DialogResult.OK)
            {
                SelectExcelSeries dlg = new SelectExcelSeries(openExcelDialog.FileName);

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Series s = SpreadsheetGearSeries.ReadFromFile(dlg.WorkBook, dlg.SheetName, dlg.DateColumn, dlg.ValueColumn);
                    if (s.Count > 0)
                    {
                        DB.AddSeries(s, tree1.SelectedID);
                    }
                    else
                    {
                        MessageBox.Show("No data in the selection.  File "+openExcelDialog.FileName);
                    }
                }
            }
        }
    }
}
