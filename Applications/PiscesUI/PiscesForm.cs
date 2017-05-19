#define CATCH_EXCEPTION
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Reclamation.Core;
using Reclamation.TimeSeries.Graphing;
using Reclamation.TimeSeries.Parser;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries.Forms.RatingTables;
using Reclamation.TimeSeries.Forms.Graphing;
using System.Collections.Generic;
using Reclamation.TimeSeries.RatingTables;
using Reclamation.TimeSeries.Forms.Alarms;

namespace Reclamation.TimeSeries.Forms
{
    /// <summary>
    /// PiscesForm displays a Tree containing time series data on
    /// the left. the right displays a graph and table based on user interface 
    /// settings such as the selections in the tree.
    /// </summary>
    public partial class PiscesForm : Form
    {
        private PiscesEngine engine1;
        private DisplayOptionsDialog displayOptionsDialog1;
        private IScenarioSelector scenarioChooser1;
        private PluginManager m_pluginManager;

        public PiscesForm(string fileName)
        {
            m_pluginManager = new PluginManager();
            m_pluginManager.LoadPlugins();// loads assemblies into memory.

            InitializeComponent();
            InitializePisces(fileName);
            m_pluginManager.RegisterPlugins(this.contextMenuStripTree.Items["AddMenu"]);
            m_pluginManager.PluginClick += m_pluginManager_PluginClick;
            Enabling();
        }

        void m_pluginManager_PluginClick(object sender, EventArgs e)
        {
            var plugin = sender as IPlugin;
            DB.SuspendTreeUpdates();
            plugin.ModifyDatabase(DB, CurrentFolder);
            DB.ResumeTreeUpdates();
            DB.RefreshFolder(CurrentFolder);
            
        }

        GraphExplorerView graphView1;
        PiscesTree tree1;

        private void InitializePisces(string fileName)
        {
            
            Logger.OnLogEvent += new StatusEventHandler(Logger_OnLogEvent);

            UserControl uc=null;
#if PISCES_OPEN
            uc = new TimeSeriesZedGraph();
#else
            uc = new TimeSeriesTeeChartGraph();
#endif

            graphView1 = new GraphExplorerView(uc as ITimeSeriesGraph);

            engine1 = new PiscesEngine(graphView1,fileName);

            ReadSettingsFromDatabase();
            SetView(graphView1);

            tree1 = new PiscesTree(new TimeSeriesTreeModel( engine1.Database));
            tree1.FilterChanged += tree1_FilterChanged;
            tree1.ContextMenuStrip = this.contextMenuStripTree;
            tree1.Parent = this.splitContainer1.Panel1;
            tree1.Dock = DockStyle.Fill;
            tree1.SelectionChanged += new EventHandler(tree1_SelectionChanged);
            tree1.LabelChanged += new EventHandler<EventArgs>(tree1_LabelChanged);
            tree1.Delete += new EventHandler<EventArgs>(tree1_Delete);
            tree1.TreeNodeParentChanged += new EventHandler<ParentChangedEventArgs>(tree1_TreeNodeParentChanged);
            tree1.TreeNodeSortChanged += new EventHandler<SortChangedEventArgs>(tree1_TreeNodeSortChanged);
            engine1.View = graphView1;
            engine1.OnProgress += new ProgressEventHandler(explorer_OnProgress);

            DataMenu.DropDown = contextMenuStripTree;

           DatabaseChanged();
        }

        void tree1_FilterChanged(object sender, EventArgs e)
        {
            DB.Filter = tree1.Filter; 
        }

        private void SetView(UserControl control)
        {
            control.Parent = this.splitContainer1.Panel2;
            control.BringToFront();
            control.Dock = DockStyle.Fill;
            control.Visible = true;
            toolStripStatusMessage.Spring = true;
            toolStripStatusMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            engine1.View = control as IExplorerView;
        }

        delegate void SetStatusTextCallback(object sender, StatusEventArgs e);


        void Logger_OnLogEvent(object sender, StatusEventArgs e)
        {
            if (this.statusStrip1.InvokeRequired)
            {
                SetStatusTextCallback d = new SetStatusTextCallback(Logger_OnLogEvent);
                this.Invoke(d, new object[] { this, e });
            }
            else
            {
                SetMessageText(e.Message);
            }
        }

        void tree1_TreeNodeSortChanged(object sender, SortChangedEventArgs e)
        {
            DB.ChangeSortOrder(e.PiscesObject, e.SortOrder);
        }

        void tree1_TreeNodeParentChanged(object sender, ParentChangedEventArgs e)
        {
            if( e.Folder != null)
            DB.ChangeParent(e.PiscesObject, e.Folder);
        }

        void tree1_Delete(object sender, EventArgs e)
        {
            DeleteClick(sender, e);
        }

        void tree1_LabelChanged(object sender, EventArgs e)
        {
            var o = tree1.SelectedObject as PiscesObject;
            DB.SaveProperties(o);
        }


        TimeSeriesDatabase DB
        {
            get
            {
                return engine1.Database;
            }
        }

        PiscesFolder CurrentFolder
        {
            get
            {
                if (tree1.IsFolderSelected)
                    return tree1.SelectedFolder;

                if (tree1.SelectedCount > 0)
                    return tree1.SelectedObject.Parent;

                else
                {
                    return tree1.RootFolder;
                }
            }
        }
        

        void DatabaseChanged()
        {
            tree1.SetModel(new TimeSeriesTreeModel ( engine1.Database));

            this.Text = engine1.Database.DataSource + " - Pisces";

            ReadSettingsFromDatabase();
            this.engine1.View = graphView1;
            displayOptionsDialog1 = new DisplayOptionsDialog(engine1);
            SetupScenarioSelector();

            engine1.SelectedSeries = new Series[] { };

            engine1.View.SeriesList.Clear();
            engine1.View.Clear();
            engine1.Run();
        }

        private void SaveSettingsToDatabase()
        {

            var m_settings = engine1.Settings;

            m_settings.Set("HydrometWebCaching", HydrometInfoUtility.WebCaching);
            m_settings.Set("HydrometAutoUpdate", HydrometInfoUtility.AutoUpdate);
            m_settings.Set("HydrometIncludeFlaggedData", HydrometInstantSeries.KeepFlaggedData);
            m_settings.Set("HydrometWebOnly", HydrometInfoUtility.WebOnly);

            m_settings.Set("UsgsAutoUpdate", Reclamation.TimeSeries.Usgs.Utility.AutoUpdate);
            m_settings.Set("ModsimDisplayFlowInCfs", Reclamation.TimeSeries.Modsim.ModsimSeries.DisplayFlowInCfs);

            var w = engine1.TimeWindow;
            m_settings.Set("FromToDatesT1", w.FromToDatesT1);
            m_settings.Set("FromToDatesT2", w.FromToDatesT2);
            m_settings.Set("FromDateToTodayT1", w.FromDateToTodayT1);
            m_settings.Set("NumDaysFromToday", w.NumDaysFromToday);
            m_settings.Set("TimeWindowType", w.WindowType.ToString());

            m_settings.Set("AutoRefresh", DB.AutoRefresh);
            #if !PISCES_OPEN
             m_settings.Set("ExcelAutoUpdate", Reclamation.TimeSeries.Excel.SpreadsheetGearSeries.AutoUpdate);
#endif
            m_settings.Save();

        }

        private void ReadSettingsFromDatabase()
        {
            var m_settings = DB.Settings;
            HydrometInfoUtility.WebCaching = m_settings.ReadBoolean("HydrometWebCaching", false);
            HydrometInfoUtility.AutoUpdate = m_settings.ReadBoolean("HydrometAutoUpdate", false);
            HydrometInstantSeries.KeepFlaggedData = m_settings.ReadBoolean("HydrometIncludeFlaggedData", false);
            HydrometInfoUtility.WebOnly = m_settings.ReadBoolean("HydrometWebOnly", false);
            Reclamation.TimeSeries.Usgs.Utility.AutoUpdate = m_settings.ReadBoolean("UsgsAutoUpdate", false);
            //  db.se

            Reclamation.TimeSeries.Modsim.ModsimSeries.DisplayFlowInCfs = m_settings.ReadBoolean("ModsimDisplayFlowInCfs", false);
            //SpreadsheetGearSeries.AutoUpdate = m_settings.ReadBoolean("ExcelAutoUpdate", true);

            var w = engine1.TimeWindow;
            w.FromToDatesT1 = m_settings.ReadDateTime("FromToDatesT1", w.FromToDatesT1);
            w.FromToDatesT2 = m_settings.ReadDateTime("FromToDatesT2", w.FromToDatesT2);
            w.FromDateToTodayT1 = m_settings.ReadDateTime("FromDateToTodayT1", w.FromDateToTodayT1);
            w.NumDaysFromToday = m_settings.ReadDecimal("NumDaysFromToday", w.NumDaysFromToday);

            string s = m_settings.ReadString("TimeWindowType", "FromToDates");
            w.WindowType = (TimeWindowType)System.Enum.Parse(typeof(TimeWindowType), s);
            DB.AutoRefresh = m_settings.ReadBoolean("AutoRefresh", true);
        }

        void explorer_OnProgress(object sender, ProgressEventArgs e)
        {
            SetMessageText(e.Message,true);
        }


        private void SetupScenarioSelector()
        {
            
            if (DB.AnyUrgsimSeries())
            {
                scenarioChooser1 = new ScenarioSelectorCCProjectionsForm(DB);
            }
            else if (DB.AnyUrgwomSeries())
            {
                scenarioChooser1 = new ScenarioSelectorUrgwom(DB);
            }
            else
            {
                scenarioChooser1 = new ScenarioSelector(DB); 
            }

            scenarioChooser1.OnApply += new EventHandler(scenarioChooser1_OnApply);
            scenarioChooser1.OnCancel += new EventHandler(scenarioChooser1_OnCancel);
            UpdateScenariosMenuLabel();

            menuScenarios.Visible = true;// explorer1.ScenarioListVisible;
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
            engine1.CustomizeScenarioTable(ea);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ClearDisplay()
        {
            this.graphView1.Clear();
        }

        private void toolStripSplitButtonRefresh_ButtonClick(object sender, EventArgs e)
        {
            DrawBasedOnTreeSelection();
        }

        private void tree1_SelectionChanged(object sender, EventArgs e)
        {
            Enabling();
            if (DB.AutoRefresh 
                &&
                (tree1.MouseClickButtons == System.Windows.Forms.MouseButtons.Left
                 || tree1.IsCommandLine)
                
                )
                DrawBasedOnTreeSelection();
            else
                ClearDisplay();
        }

        private void DrawBasedOnTreeSelection()
        {
            Enabling();
            UpdateView();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if( engine1.View != null)
                     engine1.View.Draw();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Logger.WriteLine(ex.Message);
                Logger.WriteLine(ex.StackTrace);
            }
            finally
            {
                Cursor = Cursors.Default;
                toolStripProgressBar1.Visible = false;
                string msg = "ok";
                if (engine1.SelectedSeries.Length == 1)
                {
                    var s = engine1.SelectedSeries[0];
                     msg = s.Name;
                     if (s is CalculationSeries)
                         msg += " = " + s.Expression;
                     else
                         msg += " table name: " + s.Table.TableName;
                }
                SetMessageText(msg);
            }
        }




        private void SetMessageText(string msg, bool doEvents=false)
        {
            toolStripStatusMessage.Text = msg;
            if( doEvents )
              Application.DoEvents();
        }

        /// <summary>
        /// Show Wait Cursor and progress bar
        /// </summary>
        private void ShowAsBusy(string msg)
        {
            Cursor = Cursors.WaitCursor;
            toolStripProgressBar1.Visible = true;
            SetMessageText("busy",true);
        }

        /// <summary>
        /// Hide progress bar and show default cursor
        /// </summary>
        private void ShowAsReady(string msg)
        {
            toolStripProgressBar1.Visible = false;
            SetMessageText(msg);
            Cursor = Cursors.Default;
        }


        BasicMeasurementView measurementView = new BasicMeasurementView();
        RatingTableZedGraph ratingTableView = new RatingTableZedGraph();

        private void UpdateView()
        {
            if (backgroundWorker1.IsBusy)
            {
                MessageBox.Show("Warning: The system is still busy");
                return;
            }
            toolStripProgressBar1.Visible = true;
            //Cursor = Cursors.WaitCursor;

            if( tree1.IsMeasurementSelected && !tree1.IsCommandLine)
            {
                var measurements = tree1.GetSelectedMeasurements();

                if (measurements.Length > 1)
                {
                    SetView(ratingTableView);
                    ratingTableView.Draw(measurements);

                }else // view edit single measurement
                {
                    SetView(measurementView);
                    measurementView.Measurement = tree1.SelectedObject as BasicMeasurement;
                    measurementView.Draw();
                }

                toolStripProgressBar1.Visible = false;
            }
            else if (tree1.IsRatingSelected && !tree1.IsCommandLine)
            {
                var ratings = tree1.GetSelectedRatings();

                if (ratings.Length > 1)
                {
                    SetView(ratingTableView);

                    ratingTableView.Draw(ratings);

                }
                else // view edit single measurement
                {
                    SetView(ratingTableView);
                    ratingTableView.Draw(ratings);
                }

                toolStripProgressBar1.Visible = false;
            }


            else
            {
                if ( !(engine1.View is GraphExplorerView) || engine1.View == null)
                { // need to switch back to timeseries views
                    SetView(graphView1);
                }
                engine1.SelectedSeries = tree1.GetSelectedSeries();

                if (engine1.SelectedSeries.Length == 0)
                {
                    ClearDisplay();
                    toolStripProgressBar1.Visible = false;
                    Console.WriteLine("no update needed");
                    return;
                }

                engine1.SubtractFromBaseline = scenarioChooser1.SubtractFromBaseline;
                engine1.IncludeBaseline = scenarioChooser1.IncludeBaseline;
                engine1.IncludeSelected = scenarioChooser1.IncludeSelected;
                engine1.MergeSelected = scenarioChooser1.MergeSelected;
                backgroundWorker1.RunWorkerAsync();

            }


           
          
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
#if (CATCH_EXCEPTION)
            try
            {
#endif
              if( engine1.View != null)
                  engine1.Run();
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
            var scenarios = DB.GetSelectedScenarios();
            if(scenarios.Count ==0 )
            {
                this.menuScenarios.Text = "Scenarios: (none)";
            }
            else
            if (scenarios.Count == 1)
            {
                this.menuScenarios.Text = "Scenarios: " + scenarios[0].Name;
            }
            else
            {
                this.menuScenarios.Text = "Scenarios:(" + scenarios.Count + ") selected";
            }
        }

        private void toolStripDisplayType_Click(object sender, EventArgs e)
        {
            if (displayOptionsDialog1.ShowDialog() == DialogResult.OK)
            {
                SaveSettingsToDatabase();
                if (DB.AutoRefresh)
                    DrawBasedOnTreeSelection();  
                else
                    ClearDisplay();
            }

            this.menuOptions.Text = "Analysis:" + engine1.SelectedTimeSeriesAnalysis.Name;
        }

        


        private void ExplorerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalSettings.Save();
            SaveSettingsToDatabase();
        }


        private void toolStripMenuItemAddFolder_Click(object sender, EventArgs e)
        {
            PiscesFolder f = CurrentFolder;
            f = DB.AddFolder(f, "");
        }


        private void newSite(object sender, EventArgs e)
        {
            var d = new Reclamation.TimeSeries.Forms.ImportForms.AddSite(DB);
            DB.SuspendTreeUpdates();
            if (d.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                double lat = 0;
                double lon = 0;
                double elev = 0;

                double.TryParse(d.Lat, out lat);
                double.TryParse(d.Lon, out lon);
                double.TryParse(d.Elevation, out elev);

                DB.AddSiteWithTemplate(CurrentFolder, d.SeriesCatalog, d.SiteName, d.SiteID, 
                    d.State, elev, lat, lon, d.TimeZone, d.Install,d.Program);
                DB.ResumeTreeUpdates();
                DatabaseChanged();
            }
        }

        private void Enabling()
        {

            this.toolStripSplitButtonRefresh.Visible = !DB.AutoRefresh;
            Series[] selected = tree1.GetSelectedSeries();
            bool folderSelected = tree1.IsFolderSelected;
            bool singleSelection = tree1.SelectedCount == 1;
            bool anySelected = tree1.SelectedCount > 0;
            bool singleSeriesSelected = selected.Length == 1 && tree1.SelectedCount == 1;
            bool multiSeriesSelected = selected.Length > 1;
            bool isCalculation = true;
            for (int i = 0; i < selected.Length; i++)
            {
                if (!(selected[i] is CalculationSeries))
                {
                    isCalculation = false;
                    break;
                }
            }

            sortMenu.Enabled = folderSelected && singleSelection;
            OrganizeBySiteidPcode.Enabled = folderSelected && singleSelection;

            bool canAddStuff = (CurrentFolder != null);

            AddMenu.Enabled = canAddStuff; // hydromet,access,excel, usgs... are below this

            var hideItemsPiscesOpen = new List<string> { "addExcel", "addHDBconfig",
             "addHDBmodeldata", "addHDBseries" };

            var addMenuItem = AddMenu as ToolStripDropDownItem;
            foreach (var item in addMenuItem.DropDownItems)
            {
                var toolstripItem = item as ToolStripMenuItem;
                if (toolstripItem != null)
                {
                    toolstripItem.Enabled = canAddStuff;
#if PISCES_OPEN
                    toolstripItem.Visible = !hideItemsPiscesOpen.Contains(toolstripItem.Name);
#endif
                }

            }

            menuUpdate.Enabled = anySelected;
            menuDelete.Enabled = anySelected;
            menuProperties.Enabled = singleSelection;
            menuClear.Enabled = singleSeriesSelected;
            menuCalculate.Enabled = isCalculation && anySelected;

        }



        private void sqlCommands_Click(object sender, EventArgs e)
        {
            if (DB.Server != null)
            {
                Reclamation.Core.SqlView v = new Reclamation.Core.SqlView(DB.Server);
                v.ShowDialog();
                DatabaseChanged();
            }
        }

        private void tableEditor_Click(object sender, EventArgs e)
        {
            Reclamation.Core.SqlTableEditor edit =
                new Reclamation.Core.SqlTableEditor(DB.Server);
            edit.Show();
        }


        /// <summary>
        /// Propertie can be selected for a single object
        /// </summary>
        private void Properties(object sender, EventArgs e)
        {
            try
            {
                PiscesObject v = tree1.SelectedObject;

                if (v is Series)
                {
                    Series s = v as Series;
                    string tmpExp = s.Expression;
                    SeriesProperties p = new SeriesProperties(s, DB);
                    if (p.ShowDialog() == DialogResult.OK)
                    {
                        DB.SaveProperties(s);
                        if (s is CalculationSeries && tmpExp != s.Expression && s.Expression.Trim() != "")
                        {
                           // ShowAsBusy("calculating " + s.Expression);
                            //(s as CalculationSeries).Calculate();
                        }
                        //tree1_SelectionChanged(this, EventArgs.Empty);
                        DrawBasedOnTreeSelection();
                    }
                }
                else if (v is PiscesFolder)
                {
                    PiscesFolder f = v as PiscesFolder;
                    FolderProperties p = new FolderProperties(f);
                    if (p.ShowDialog() == DialogResult.OK)
                    {
                        DB.SaveProperties(f);
                    }
                }
            }
            catch (Exception propEx)
            {
                MessageBox.Show(propEx.Message);
            }
        }




        private void ClearClick(object sender, EventArgs e)
        {
            foreach (var s in tree1.GetSelectedSeries())
            {
                DB.ClearSeries(s);
            }
        }

        private void DeleteClick(object sender, EventArgs e)
        {
            if (MessageBox.Show("Click Yes to delete", "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {

                try
                {
                    Cursor = Cursors.WaitCursor;
                    foreach (var s in tree1.GetSelectedSeries())
                    {
                        DB.Delete(s);
                    }

                    foreach (var f in tree1.SelectedFolders)
                    {
                        DB.Delete(f);
                    }
                    DatabaseChanged(); 
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        

        // ToolStripMenuItem MenuImport
        //{
        //    get { return this.menuImport; }
        //}

         ToolStripItemCollection MenuItems
        {
            get { return this.menuStrip1.Items; }
        }



         ToolStripMenuItem ViewMenu
        {
            get { return menuView; }
        }

         ToolStripMenuItem FileMenu
        {
            get { return fileMenu; }
        }

        private void logToolStripMenuItemViewLog_Click(object sender, EventArgs e)
        {
            LoggerView v = new LoggerView();
            v.ShowDialog();
        }



        private void NewDatabaseClick(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Title = "Create a new Database";
            fd.DefaultExt = "*.pdb";
            fd.Filter = "Pisces database (*.pdb)|*.pdb";
            if (fd.ShowDialog() == DialogResult.OK)
            {
                SQLiteServer.CreateNewDatabase(fd.FileName);
                engine1.Open(fd.FileName);
                DatabaseChanged();
                UserPreference.Save("fileName", fd.FileName);
            }
        }

        private void OpenClick(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                Performance p = new Performance();
                OpenFileDialog fd = new OpenFileDialog();
                fd.DefaultExt = "*.pdb";
                fd.Filter = "Pisces database (*.pdb)|*.pdb";
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    engine1.Open(fd.FileName);
                    DatabaseChanged();
                }
                UserPreference.Save("fileName", fd.FileName);
                p.Report("done reading " + fd.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void connectToServer_Click(object sender, EventArgs e)
        {
            try
            {
              ServerDatabaseDialog dlg = new ServerDatabaseDialog();

                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    engine1.ConnectToServer(dlg.ServerName, dlg.DatabaseName,dlg.DatabaseType, dlg.Password);
                    DatabaseChanged();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About a = new About();
            a.ShowDialog();
        }





        private void CalculateClick(object sender, EventArgs e)
        {
            if (tree1.SelectedFolders.Length == 0)
            {
                Series[] list = tree1.GetSelectedSeries();
                ProcessSelectedSeries(SeriesProcess.Calculate,list);
            }
            else if (tree1.SelectedFolders.Length == 1)
            {
                SeriesList list = new SeriesList();
                foreach (Series s in tree1.GetSeriesRecursive())
                {
                    if (s.Expression != "") // only perform calculations on calculation series with a valid expression
                    { list.Add(s); }
                }
                if (list.Count > 0)
                { ProcessSelectedSeries(SeriesProcess.Calculate, list.ToArray()); }
                else
                {
                    MessageBox.Show("No Calculation Series found in folder.");
                    ClearDisplay();
                    return;
                }
            }
            else
            {
                MessageBox.Show("Please select a single folder to calculate.");
                ClearDisplay();
                return;
            }
            
            //tree1_SelectionChanged(this, EventArgs.Empty);
            DrawBasedOnTreeSelection();
        }

        /// <summary>
        /// Update Selected Series or folders
        /// Enabled only for Series or Folders selected NOT both at the
        /// same time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuUpdate_Click(object sender, EventArgs e)
        {
            if (tree1.SelectedFolders.Length == 0)
            {
                Series[] list = tree1.GetSelectedSeries();
                ProcessSelectedSeries(SeriesProcess.Update,list);
            }
            else if (tree1.SelectedFolders.Length == 1)
            {
                SeriesList list = new SeriesList();
                foreach (Series s in tree1.GetSeriesRecursive())
                {
                    list.Add(s);
                }
                ProcessSelectedSeries(SeriesProcess.Update, list.ToArray());
            }
            else
            {
                MessageBox.Show("Please select a single folder to update.");
            }
                
        }

        private void ProcessSelectedSeries(SeriesProcess process, Series[] list)
        {
            DateTime t1 = DateTime.Now.AddDays(-7);
            DateTime t2 = DateTime.Now.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            //Series[] list = tree1.GetSelectedSeries();

            // Causing stange results when using Hydromet years 9999, 7170,
            // added extract check (not into the future)
            PeriodOfRecord p = list[0].GetPeriodOfRecord();
            if (list.Length > 0 && p.Count > 0 && p.T2 < DateTime.Now.Date) // begin at the end of data
                t1 = p.T2;

            for (int i = 1; i < list.Length; i++)
            {
                PeriodOfRecord por = list[i].GetPeriodOfRecord();
                if (por.Count > 0 && por.T2 < t1 && por.T2 < DateTime.Now.Date)
                    t1 = por.T2;
            }

            try
            {
                Update u = new Update(t1, t2, process);

                if (u.ShowDialog() == DialogResult.OK)
                {
                    ShowAsBusy("");
                    Logger.WriteLine(process + " t1 = " + u.T1 + " t2= " + u.T2);
                    for (int i = 0; i < list.Length; i++)
                    {
                        Logger.WriteLine(process.ToString() + " " + list[i].Name);
                        Application.DoEvents();

                        if (process == SeriesProcess.Update && !(list[i] is CalculationSeries))
                        {
                            list[i].Update(u.T1, u.T2);
                        }
                        if (process == SeriesProcess.Calculate || list[i] is CalculationSeries)
                        {
                            var cs = list[i] as CalculationSeries;
                            if (DB.Settings.ReadBoolean("HydrometVariableResolver", false))
                            { // this reads data from hydromet server, over http, instead of 'this' database
                                var svr = HydrometInfoUtility.HydrometServerFromPreferences();
                                cs.Parser.VariableResolver = new HydrometVariableResolver(svr);

                            }

                            if (u.FullPeriodOfRecord)
                                cs.Calculate();
                            else
                                cs.Calculate(u.T1, u.T2);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                ShowAsReady("");
            }
        }
        //public TimeWindow TimeWindow;
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options o = new Options();
            
           
            o.HydrometUseWebCache = HydrometInfoUtility.WebCaching;
            o.HydrometAutoUpdate = HydrometInfoUtility.AutoUpdate;
            o.HydrometIncludeFlaggedData = HydrometInstantSeries.KeepFlaggedData;
            o.HydrometWebOnly = HydrometInfoUtility.WebOnly;
            o.DecodesOutputDirectory = DB.Settings.ReadString("DecodesOutputDirectory","");
            o.UsgsDailyAutoUpdate = Usgs.Utility.AutoUpdate;
            o.MultipleYAxis = DB.Settings.ReadBoolean("MultipleYAxis", false);
           // o.ScenarioNames = DB.Scenario;
            o.ModsimDisplayFlowInCfs = Modsim.ModsimSeries.DisplayFlowInCfs;
#if !PISCES_OPEN
            o.ExcelAutoUpdate = Reclamation.TimeSeries.Excel.SpreadsheetGearSeries.AutoUpdate;
#endif
            o.AutoRefresh = DB.AutoRefresh;
            o.HydrometVariableResolver = DB.Settings.ReadBoolean("HydrometVariableResolver", false);
            o.VerboseLogging = DB.Settings.ReadBoolean("VerboseLogging", false);
            

            if (o.ShowDialog() == DialogResult.OK)
            {
                HydrometInfoUtility.WebCaching = o.HydrometUseWebCache;
                HydrometInfoUtility.AutoUpdate = o.HydrometAutoUpdate;
                HydrometInstantSeries.KeepFlaggedData = o.HydrometIncludeFlaggedData;
                HydrometInfoUtility.WebOnly = o.HydrometWebOnly;

                DB.Settings.Set("DecodesOutputDirectory", o.DecodesOutputDirectory);
                DB.Settings.Set("MultipleYAxis", o.MultipleYAxis);
                DB.Settings.Set("HydrometVariableResolver", o.HydrometVariableResolver);
                DB.Settings.Set("VerboseLogging", o.VerboseLogging);
                if( o.VerboseLogging )
                    Logger.EnableLogger(true);
                Usgs.Utility.AutoUpdate = o.UsgsDailyAutoUpdate;
                Modsim.ModsimSeries.DisplayFlowInCfs = o.ModsimDisplayFlowInCfs;
                #if !PISCES_OPEN
                Reclamation.TimeSeries.Excel.SpreadsheetGearSeries.AutoUpdate = o.ExcelAutoUpdate;
#endif
                DB.AutoRefresh = o.AutoRefresh;
                SaveSettingsToDatabase();
               // DB.Scenario = o.ScenarioNames;
            }
            Enabling();
        }

        private void DataMenu_Click(object sender, EventArgs e)
        {
           // Enabling();
        }

        private void toolStripMenuCompactDB_Click(object sender, EventArgs e)
        {
            DB.Server.Vacuum();
            //if (DB.Server is SqlServerCompact)
            //{
            //    var s = DB.Server as SqlServerCompact;
            //    SqlServerCompact.Compact(s.FileName);
            //}
        }

        private void exportMenu_Click(object sender, EventArgs e)
        {
            if (folderBrowserExport.ShowDialog() == DialogResult.OK)
            {
                DB.Export(folderBrowserExport.SelectedPath);
            }
        }

        /// <summary>
        /// Imports a directory of text files into a new Pisces Database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuImport_Click(object sender, EventArgs e)
        {

            ImportDatabaseForm import = new ImportDatabaseForm();

            if (import.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ShowAsBusy("importing data");
                    
                    //SQLiteServer.CreateNewDatabase(import.DatabaseFilename);
                    //SqlServerCompact.CreateNewDatabase(import.DatabaseFilename);
                    //explorer1.Open(import.DatabaseFilename);
                    //DatabaseChanged();

                    if (DB.GetSeriesCatalog().Count > 1)
                    {
                        if (MessageBox.Show(
                            "Are you sure you want to delete your database?  There are " + DB.GetSeriesCatalog().Count + " series in it","Delete all data ?", MessageBoxButtons.OKCancel)
                              != DialogResult.OK)
                            return;

                    }

                    DB.SuspendTreeUpdates();
                    DB.ImportCsvDump(import.CatalogFilename, import.IncludeSeriesData);
                    DB.ResumeTreeUpdates();
                    //explorer1.Open(import.DatabaseFilename);
                    DatabaseChanged();

                }
                finally
                {
                    ShowAsReady("Done with import");
                }
            }
        }

        private void PiscesForm_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("PiscesForm_MouseDown");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void toolStripMenuItemStandalone_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("This will remove all links to external data and save the data to this database.",
                                         "Warning!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (result == DialogResult.OK)
            {
                ShowAsBusy("Creating standalone database");
                try
                {
                    DB.CreateStandalone(true);
                }
                finally
                {
                    ShowAsReady("Done saving data to database");
                }
                
            }
            else
                return;
        }

        private void toolStripProfileTool_Click(object sender, EventArgs e)
        {
            ProfileDesigner d = new ProfileDesigner(DB);
            d.Show();
        }

        private void toolStripMenuRWIS_Click(object sender, EventArgs e)
        {
            System.DirectoryServices.ActiveDirectory.Domain usrDom;
            bool allowForm = false;
            try
            {
                usrDom = System.DirectoryServices.ActiveDirectory.Domain.GetComputerDomain();
                if (usrDom.Name != "bor.doi.net")
                { System.Windows.Forms.MessageBox.Show("RWIS Management Interface only available within the DOI-USBR network..."); }
                else
                { allowForm = true; }
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("RWIS Management Interface only available within the DOI-USBR network...");
                toolStripMenuRWIS.Enabled = false;
            }

            if (allowForm)
            {
                var f = new Rwis.Sync.rwisForm();
                f.DB = this.DB;
                f.ShowDialog();
            }
        }


        private void toolStripMenuItemExportModelScenarios_Click(object sender, EventArgs e)
        {
            DB.SuspendTreeUpdates();
            toolStripProgressBar1.Visible = true;
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Title ="Export Gains and Forecasts for each scenario";
            dlg.Filter = "Excel (*.xls;*.xlsx) |*.xls;*.xlsx|All files (*.*)|*.*";
            var ds = new ScenarioManagement.ScenarioDataSet();
            ds.OnProgress += explorer_OnProgress;
            try
            {
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ds.Export(dlg.FileName, DB);
                }
            }
            catch (Exception eex)
            {
                MessageBox.Show(eex.Message, "Error");

            }
            finally
            {
                DB.ResumeTreeUpdates();
                DatabaseChanged();
                toolStripProgressBar1.Visible = false;
            }
        }

        private void toolStripMenuItemAlarmManager_Click(object sender, EventArgs e)
        {
            try
            {
                AlarmManagerMain am = new AlarmManagerMain(DB.Server);
                am.Size = new System.Drawing.Size(600, 400);
                am.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

       

        private void sortMenu_Click_1(object sender, EventArgs e)
        {
            if (!tree1.IsFolderSelected)
                return;
            var parent = DB.GetOrCreateFolder(tree1.SelectedFolder.Name);
            var u = new TimeSeriesDatabaseUtility(DB);
            u.SortByName(parent);
            DatabaseChanged();
        }

        private void OrganizeBySiteidPcode_Click(object sender, EventArgs e)
        {
            if (!tree1.IsFolderSelected)
                return;
            var parent = DB.GetOrCreateFolder(tree1.SelectedFolder.Name);
            var u = new TimeSeriesDatabaseUtility(DB);
            u.OrganizeCatalogBySiteInterval(parent);
            DatabaseChanged();

        }

        

        
    }


}
