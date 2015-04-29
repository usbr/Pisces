// PiscesForm.Add.cs contains user interface actions that add new
// objects to the database


using System.Windows.Forms;
using System;
using System.Data;
using Reclamation.TimeSeries.Forms.ImportForms;
using Reclamation.TimeSeries.Forms.Calculations;
using Reclamation.TimeSeries.Excel;
using Reclamation.TimeSeries.Urgsim;
using Reclamation.Core;
using HdbPoet;
using Reclamation.TimeSeries.OracleHdb;
using System.IO;
using Reclamation.TimeSeries.Nrcs;
using System.Collections.Generic;
using Pisces;
using System.Configuration;
namespace Reclamation.TimeSeries.Forms
{

    public partial class PiscesForm
    {

        private void addPiscesDatabase_Click(object sender, EventArgs e)
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
                    SQLiteServer svr = new SQLiteServer(fd.FileName);
                    TimeSeriesDatabase db = new TimeSeriesDatabase(svr);
                    DB.InsertDatabase(CurrentFolder, db);
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


        /// <summary>
        /// Adds new Calculation Series
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AddCalculationClick(object sender, EventArgs e)
        {
            
            CalculationSeries s = new CalculationSeries(DB);
            CalculationProperties p = new CalculationProperties(s, new TimeSeriesTreeModel( DB), DB.GetUniqueUnits());

            if (p.ShowDialog() == DialogResult.OK)
            {
                DB.AddSeries(s, CurrentFolder);
                // tree refresh.. Add node.
                s.Calculate(); // save again
                // refresh.
                DB.RefreshFolder(CurrentFolder);
                
            }
        }


        void AddSeriesClick(object sender, System.EventArgs e)
        {
            Series s = new Series();
            s.Name = "new Series";
            DB.AddSeries(s, CurrentFolder);
        }

        /// <summary>
        /// Creates a new series from a text file
        /// </summary>
        void AddTextFileClick(object sender, System.EventArgs e)
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
                    DB.AddSeries(s, CurrentFolder);
                }
            }
        }

        private void Addcr10x_Click(object sender, EventArgs e)
        {
            if (openFileDialogCr10x.ShowDialog() == DialogResult.OK)
            {
                ImportCr10x i = new ImportCr10x();
                if (i.ShowDialog() == DialogResult.OK)
                {
                    DataLogger.Cr10xSeries s = new Reclamation.TimeSeries.DataLogger.Cr10xSeries(openFileDialogCr10x.FileName,
                        i.SelectedInterval, i.SelectedColumn);
                    s.Read();
                    if (s.Count == 0)
                    {
                        MessageBox.Show("No data found in file:" + openTextFileDialog.FileName);
                        return;
                    }

                    if (s.Count > 0)
                    {
                        DB.AddSeries(s, CurrentFolder);
                    }

                }
            }
        }

        private void AddExcelClick(object sender, EventArgs e)
        {
            if (openExcelDialog.ShowDialog() == DialogResult.OK) // select filename
            {
                ExcelImportWizard w = new ExcelImportWizard();
                if (w.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {

                    if(w.ImportType == ExcelImportType.Standard)
                           ImportExcelStandard(openExcelDialog.FileName);
                    else if (w.ImportType == ExcelImportType.WaterYear)
                    {
                        ImportExcelWaterYear(openExcelDialog.FileName);
                    }
                    else if (w.ImportType == ExcelImportType.Database)
                    {
                        ImportExcelDatabaseStyle(openExcelDialog.FileName);
                    }
                }
            }
        }

        private void ImportExcelDatabaseStyle(string filename)
        {
            var dlg = new ImportExcelDatabase(filename, DB.GetUniqueUnits());

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var messageList = new List<string>();
                for (int i = 0; i < dlg.SelectedSites.Length; i++)
                {
                    Series s = SpreadsheetGearSeries.ReadFromWorkbook(dlg.WorkBook,
                        dlg.SheetName, dlg.DateColumn, dlg.ValueColumn, dlg.SiteColumn, dlg.SelectedSites[i],dlg.ComboBoxUnits.Text);
                    if (s.Count > 0)
                    {
                        DB.AddSeries(s, CurrentFolder);
                    }
                    else
                    {
                        //messageList.Add("No data in the selection.  File " + openExcelDialog.FileName);
                        messageList.Add("No data in the selection.  Site: " + dlg.SelectedSites[i]);
                    }

                    if (s.Messages.Count > 0)
                    {
                        messageList.Add(s.Messages.ToString());
                    }

                }
                if (messageList.Count > 0)
                {
                    MessageBox.Show(String.Join("\n",messageList.ToArray()),"Import Warnings",MessageBoxButtons.OK);
                }
                
            }
        }

        private void ImportExcelWaterYear(string filename)
        {
            var dlg = new ImportExcelWaterYear(filename, DB.GetUniqueUnits());

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                for (int i = 0; i < dlg.SheetNames.Length; i++)
                {

                    SpreadsheetGearSeries s = SpreadsheetGearSeries.ReadFromWorkbook(dlg.WorkBook, dlg.SheetNames[i], dlg.DateColumn, dlg.ValueColumn, true, dlg.ComboBoxUnits.Text);
                    s.Name = "Monthly"+s.SheetName;

                    if (s.Count > 0)
                    {
                        DB.AddSeries(s, CurrentFolder);
                    }
                    else
                    {
                        //MessageBox.Show("No data in the selection.  File " + openExcelDialog.FileName);
                        MessageBox.Show("No data in selection.  Worksheet: " + s.SheetName);
                    }

                    if (s.Messages.Count > 0)
                    {
                        MessageBox.Show(s.Messages.ToString(), "Import Warnings ", MessageBoxButtons.OK);
                    }
                }

            }
        }

        private void ImportExcelStandard(string filename)
        {
            ImportExcelStandard dlg = new ImportExcelStandard(filename, DB.GetUniqueUnits());

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var messageList = new List<string>();
                for (int i = 0; i < dlg.ValueColumns.Length; i++)
                {
                    Series s = SpreadsheetGearSeries.ReadFromWorkbook(dlg.WorkBook, dlg.SheetName, dlg.DateColumn, dlg.ValueColumns[i],false,dlg.ComboBoxUnits.Text);
                    if (s.Count > 0)
                    {
                        DB.AddSeries(s, CurrentFolder);
                    }
                    else
                    {
                        //messageList.Add("No data in the selection.  File " + openExcelDialog.FileName);
                        messageList.Add("No data in the selection.  Worksheet: " + dlg.SheetName);
                    }

                    if (s.Messages.Count > 0)
                    {
                        messageList.Add(s.Messages.ToString());
                    }

                }
                if (messageList.Count > 0)
                {
                    MessageBox.Show(String.Join("\n",messageList.ToArray()),"Import Warnings",MessageBoxButtons.OK);
                }
            }
        }

        private void AddBpaHydsimClick(object sender, EventArgs e)
        {
            var dlg = new Reclamation.TimeSeries.ScenarioPicker.ScenarioPicker();

            dlg.Text = "Select Hydsim Output Files";
            dlg.Dialog.DefaultExt = ".mdb";
            dlg.Dialog.Filter = "BPA Hydsim Access *.mdb|*.mdb|All Files|*.*";
            dlg.Dialog.Title = "Open BPA Hydsim Access File";
            
            try
            {
                DB.SuspendTreeUpdates();
                var result = dlg.ShowDialog();

                if (result == DialogResult.OK)
                {
                    if (dlg.ScenariosChecked && dlg.ScenarioFiles.Count > 0)
                    {
                        //create scenarios
                        ShowAsBusy("Reading Hydsim data");
                        var tblScen = DB.GetScenarios();

                        foreach (var item in dlg.ScenarioFiles)
                        {
                            string scenarioPath = ConnectionStringUtility.MakeFileNameRelative("FileName=" + item, DB.DataSource);
                            tblScen.AddScenarioRow(Path.GetFileNameWithoutExtension(item), true, scenarioPath);
                        }
                        //add first file in the list to the tree
                        if (dlg.AddToTreeChecked)
                        {
                            var bpa = new BpaHydsim.BpaHydsimTreeAccess(dlg.ScenarioFiles[0], DB.DataSource, DB.NextSDI(), this.CurrentFolder.ID);
                            var tbl = bpa.CreateTree();
                            DB.Server.SaveTable(tbl);
                        }

                        DB.Server.SaveTable(tblScen);
                        DatabaseChanged();
                    }
                    else
                        if (dlg.AddToTreeChecked)
                        {
                            //add to tree, but not to scenairo list
                            ShowAsBusy("Reading Hydsim data");
                            for (int i = 0; i < dlg.ScenarioFiles.Count; i++)
                            {
                                string fn = dlg.ScenarioFiles[i].ToString();
                                var bpa = new BpaHydsim.BpaHydsimTreeAccess(fn, DB.DataSource, DB.NextSDI(), this.CurrentFolder.ID);
                                var tbl = bpa.CreateTree();
                                DB.Server.SaveTable(tbl);
                            }
                            DatabaseChanged();
                        }
                }
            }

            finally
            {
                ShowAsReady("Done with Hydsim import");
                DB.ResumeTreeUpdates();
            }
        }


        private void AddAccessClick(object sender, EventArgs e)
        {
            if (openAccessDialog.ShowDialog() == DialogResult.OK)
            {
                SelectAccessSeries dlg = new SelectAccessSeries(openAccessDialog.FileName);
                if (dlg.ShowDialog() == DialogResult.OK)
                {

                    for (int i = 0; i < dlg.SelectedSites.Length; i++)
                    {
                        AccessSeries s = new AccessSeries(dlg.FileName, dlg.TableName, dlg.DateColumn,
                            dlg.ValueColumn, dlg.FilterColumn, dlg.SelectedSites[i]);
                        DB.AddSeries(s, CurrentFolder);
                    }
                }
            }

        }

        private void AddNrcsSnotel(object sender, EventArgs e)
        {

            ImportNrcsSnotel dlg = new ImportNrcsSnotel();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                DB.SuspendTreeUpdates();
                for (int i = 0; i < dlg.SelectedSiteNumbers.Length; i++)
                {
                    var siteNumber = dlg.SelectedSiteNumbers[i];

                    for (int j = 0; j < dlg.SelectedParameters.Length; j++)
                    {
                        var parameter = dlg.SelectedParameters[j];
                        var s = new NrcsSnotelSeries(siteNumber, parameter);
                        DB.AddSeries(s, CurrentFolder);
                    }
                }
                DB.ResumeTreeUpdates();
                DatabaseChanged();
            }

        }


        void AddHydrometClick(object sender, System.EventArgs e)
        {

            ImportHydromet dlg = new ImportHydromet();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                UserPreference.SetDefault("HydrometServer", dlg.HydrometServer.ToString(), true);

                string msg = "";
                try
                {
                    ShowAsBusy("connecting to hydromet");
                    Series s = Hydromet.HydrometInfoUtility.Read(dlg.Cbtt, dlg.ParameterCode, dlg.T1, dlg.T2,
                        dlg.TimeInterval, dlg.HydrometServer);

                    if (s.Count == 0)
                    {
                        msg = "Error: Could not find any Hydromet Data";
                    }
                    else
                    {
                        msg = "read " + s.Count + " records";
                        if (dlg.UseSimpleName)
                            s.Name = dlg.Cbtt + " " + dlg.ParameterCode;
                        DB.AddSeries(s, CurrentFolder);
                    }
                }
                finally
                {
                    ShowAsReady(msg);
                }
            }
        }


        /// <summary>
        /// Add USGS data.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addOwrd_Click(object sender, EventArgs e)
        {
            ImportOWRD dlg = new ImportOWRD();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                string msg = "";
                try
                {
                    ShowAsBusy("connecting to OWRD web site");

                    foreach (string siteID in dlg.SiteIDs)
                    {
                        var ds = dlg.OwrdDataSet;
                        var s = new Owrd.OwrdSeries(siteID, ds,dlg.IncludeProvisional);
                        s.Read(dlg.T1, dlg.T2);

                        if (s.Count == 0)
                        {
                            msg = "Error: no OWRD data was found";
                        }
                        else
                        {
                            msg = "read " + s.Count + " records";
                            DB.AddSeries(s, CurrentFolder);
                        }
                        if (s.Messages.Count > 1)
                        {
                            MessageBox.Show(s.Messages.ToString() + "\nCheck Tools->View Log for additional information.", "Duplicates Ignored", MessageBoxButtons.OK);
                        }
                    }

                }
                finally
                {
                    ShowAsReady(msg);
                }

            }

        }

        /// <summary>
        /// Creates a new Usgs Series from the internet
        /// </summary>
        void AddUsgsClick(object sender, System.EventArgs e)
        {
            //OldImportUSGS();
            ImportUsgsData dlg = new ImportUsgsData();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string msg = "";
                try
                {
                    ShowAsBusy("connecting to USGS web site");

                    foreach (string siteID in dlg.SiteIDs)
                    {
                        Series s = new Series();
                        if (dlg.IsGroundWaterLevel)
                        {
                            s = Usgs.UsgsGroundWaterLevelSeries.Read(siteID, TimeSeriesDatabase.MinDateTime, TimeSeriesDatabase.MaxDateTime);//, dlg.T1, dlg.T2);
                        }
                        else
                            if (!dlg.IsRealTime)
                            {
                                s = Usgs.UsgsDailyValueSeries.Read(siteID, (Reclamation.TimeSeries.Usgs.UsgsDailyParameter)dlg.SelectedParameter,
                                 dlg.T1, dlg.T2);
                            }
                            else
                            {
                                s = Usgs.UsgsRealTimeSeries.Read(siteID, (Reclamation.TimeSeries.Usgs.UsgsRealTimeParameter)dlg.SelectedParameter, dlg.T1, dlg.T2);
                            }
                        if (s.Count == 0)
                        {
                            msg = "Error: no USGS data was found";
                        }
                        else
                        {
                            msg = "read " + s.Count + " records";
                            DB.AddSeries(s, CurrentFolder);
                        }
                        if (s.Messages.Count > 1)
                        {
                            MessageBox.Show(s.Messages.ToString() + "\nCheck Tools->View Log for additional information.", "Duplicates Ignored", MessageBoxButtons.OK);
                        }
                    }

                }
                finally
                {
                    ShowAsReady(msg);
                }

            }
        }


        private void newModsim_Click(object sender, EventArgs e)
        {
            var dlg = new ScenarioPicker.ScenarioPicker();

            dlg.Text = "Select Modsim Files";
            dlg.Dialog.DefaultExt = ".xy";
            dlg.Dialog.Filter = "Modsim File (*.xy)|*.xy|All Files|*.*";
            dlg.Dialog.Title = "Open Modsim XY File";

            try
            {
                DB.SuspendTreeUpdates();
                var result = dlg.ShowDialog();

                if (result == DialogResult.OK)
                {
                    if (dlg.ScenariosChecked && dlg.ScenarioFiles.Count > 0)
                    {
                        //create scenarios
                        ShowAsBusy("Reading Modsim data");
                        var tblScen = DB.GetScenarios();
                        foreach (var item in dlg.ScenarioFiles)
                        {
                            string scenarioPath = ConnectionStringUtility.MakeFileNameRelative("FileName=" + item, DB.DataSource);
                            tblScen.AddScenarioRow(Path.GetFileNameWithoutExtension(item), true, scenarioPath);
                        }
                        //add first file in the list to the tree
                        if (dlg.AddToTreeChecked)
                        {
                            Modsim.PiscesTree.CreatePiscesTree(dlg.ScenarioFiles[0], CurrentFolder, DB);
                        }
                        DB.Server.SaveTable(tblScen);
                        DatabaseChanged();
                    }
                    else
                        if (dlg.AddToTreeChecked)
                        {
                            //add to tree, but not to scenairo list
                            ShowAsBusy("Reading Modsim data");
                            for (int i = 0; i < dlg.ScenarioFiles.Count; i++)
                            {
                                string fn = dlg.ScenarioFiles[i].ToString();
                                Modsim.PiscesTree.CreatePiscesTree(fn, CurrentFolder, DB);
                            }
                            DatabaseChanged();
                        }
                }
            }
            finally
            {
                ShowAsReady("Done with Modsim import");
                DB.ResumeTreeUpdates();
            }
        }

        private void AddHydrossClick(object sender, EventArgs e)
        {

            Hydross.HydrossScenarioPicker dlg = new Reclamation.TimeSeries.Hydross.HydrossScenarioPicker();

            try
            {
                DB.SuspendTreeUpdates();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ShowAsBusy("Reading Hydross data");
                    var tbl = new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();
                    Hydross.HydrossTree.Generate(tbl, DB.DataSource, dlg.ScenarioFiles, DB.NextSDI(), this.CurrentFolder.ID);
                    DB.Server.SaveTable(tbl);
                    // add scenarios..
                    var tblScen = DB.GetScenarios();

                    foreach (var item in dlg.ScenarioFiles)
                    {
                        string scenarioPath = ConnectionStringUtility.MakeFileNameRelative("FileName=" + item, DB.DataSource);
                        tblScen.AddScenarioRow(Path.GetFileNameWithoutExtension(item), true, scenarioPath);
                    }
                    DB.Server.SaveTable(tblScen);
                    DatabaseChanged();

                }
            }
            finally
            {
                ShowAsReady("Done with Hydross import");
                DB.ResumeTreeUpdates();
            }
        }


        private void AddRiverWareSingleRdf_Click(object sender, EventArgs e)
        {
            var dlg = new ScenarioPicker.ScenarioPicker();

            dlg.Text = "Select RiverWare Output Files";
            dlg.Dialog.DefaultExt = ".rdf";
            dlg.Dialog.Filter = "RiverWare Data File (*.rdf)|*.rdf|All Files|*.*";
            dlg.Dialog.Title = "Open RiverWare RDF Files";

            try
            {
                DB.SuspendTreeUpdates();
                var result = dlg.ShowDialog();

                if (result == DialogResult.OK)
                {
                    if (dlg.ScenariosChecked && dlg.ScenarioFiles.Count > 0)
                    {
                        //create scenarios
                        ShowAsBusy("Reading RiverWare data");
                        var tblScen = DB.GetScenarios();
                        foreach (var item in dlg.ScenarioFiles)
                        {
                            string scenarioPath = ConnectionStringUtility.MakeFileNameRelative("FileName=" + item, DB.DataSource);
                            tblScen.AddScenarioRow(Path.GetFileNameWithoutExtension(item), true, scenarioPath);
                        }
                        //add first file in the list to the tree
                        if (dlg.AddToTreeChecked)
                        {
                            RiverWare.RiverWareTree.AddRiverWareFileToDatabase(dlg.ScenarioFiles[0], CurrentFolder, DB);
                        }
                        DB.Server.SaveTable(tblScen);
                        DatabaseChanged();
                    }
                    else
                        if (dlg.AddToTreeChecked)
                        {
                            //add to tree, but not to scenairo list
                            ShowAsBusy("Reading RiverWare data");
                            for (int i = 0; i < dlg.ScenarioFiles.Count; i++)
                            {
                                string fn = dlg.ScenarioFiles[i].ToString();
                                RiverWare.RiverWareTree.AddRiverWareFileToDatabase(fn, CurrentFolder, DB);
                            }
                            DatabaseChanged();
                        }
                }
            }
            finally
            {
                ShowAsReady("Done with RiverWare import");
                DB.ResumeTreeUpdates();
            }
        }

        private void AddRiverWareMultipleRdf_Click(object sender, EventArgs e)
        {
            if (openRdfFileDialog.ShowDialog() == DialogResult.OK)
            {
                RiverWare.RiverWareTree.AddRiverWareFileToDatabase(
                    openRdfFileDialog.FileName, CurrentFolder, DB);
                DatabaseChanged();
            }
        }

     
      
        private void AddHDBModel_Click(object sender, EventArgs e)
        {

             var server = OracleServer.ConnectToOracle();

             if (server == null)
                 return;
             Hdb.Instance = new Hdb(server);

            SelectHdbModel dlg = new SelectHdbModel();

            if (dlg.ShowDialog() == DialogResult.OK && dlg.ModelID > 0)
            {
                // create tree for this model. 
                var folder = DB.AddFolder(CurrentFolder, dlg.ModelName + " " + dlg.ModelTable);
                var tbl = HdbModelTreeBuilder.PiscesSeriesCatalog(dlg.ModelID, dlg.ModelTable, dlg.OldestModelRunDate, DB.NextSDI(), folder.ID);

                if (tbl.Rows.Count == 0)
                {
                    MessageBox.Show("No model runs found for model_id " + dlg.ModelID + " after " + dlg.OldestModelRunDate.ToString());
                    return;
                }
                DB.Server.SaveTable(tbl);

                // create scenario list.
                var scenarioTable = DB.GetScenarios();
                var ref_model_run = Hdb.Instance.ref_model_run(dlg.ModelID, dlg.OldestModelRunDate);
                foreach (DataRow item in ref_model_run.Rows)
                {
                    string date = Convert.ToDateTime(item["run_date"]).ToShortDateString();
                    string model_run_name = item["model_run_name"].ToString();
                    string id = item["model_run_id"].ToString();
                    string path = HdbModelSeries.BuildScenairoPath(model_run_name, id, date);
                    string name = HdbModelSeries.BuildScenairoName(model_run_name, id, date);
                    scenarioTable.AddScenarioRow(name, true, path);
                }

                DB.Server.SaveTable(scenarioTable);
                DatabaseChanged();
            }
        }


        private void addHDB_Click(object sender, EventArgs e)
        {
            var server = OracleServer.ConnectToOracle();

            if (server != null )
            {
                Hdb.Instance = new Hdb(server);
                var dlg = new HdbPoet.FormAddSeries();
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    HdbPoet.Properties.Settings.Default.Save();
                    ImportFromHdb.Import(dlg.DataSet, DB, CurrentFolder);
                    DatabaseChanged();
                }
            }

        }
        private void addHDBConfigFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "HDB Files |*.hdb|AllFiles|*.*";

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImportFromHdb.Import(dlg.FileName, DB, CurrentFolder);
                DatabaseChanged();
            }
        }

        /// <summary>
        /// URGWOM is a custom RiverWare model.
        /// Pisces reads the ouput as excel files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddUrgwomClick(object sender, EventArgs e)
        {
            if (openExcelDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                
                UrgwomUtility u = new UrgwomUtility(DB,openExcelDialog.FileName);
                u.LoadTree();// load slots into Tree from first run -- column names
                u.LoadScenarios(); // Load scenario selector with years 1976 - 2005)
                DatabaseChanged();   
            }

        }


        private void AddURGSIM_Click(object sender, EventArgs e)
        {
            if (!DB.DeleteFolderByName("URGSiM", CurrentFolder, true))
                return;

            DB.SuspendTreeUpdates();
            ShowAsBusy("Reading URGSiM projections and variables");

            UrgsimUtilitycs.CreateTree(DB,CurrentFolder);
            UrgsimUtilitycs.LoadScenarioTable(DB);

            SetupScenarioSelector();
            
            DatabaseChanged();
            ShowAsReady("Done with URGSiM import");
            DB.ResumeTreeUpdates();

        }

       
    }
}