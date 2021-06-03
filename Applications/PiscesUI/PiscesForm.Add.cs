// PiscesForm.Add.cs contains user interface actions that add new
// objects to the database


using System.Windows.Forms;
using System;
using System.Data;
using Reclamation.TimeSeries.Forms.ImportForms;
using Reclamation.TimeSeries.Forms.Calculations;
using Reclamation.Core;
using System.IO;
using Reclamation.TimeSeries.Nrcs;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries.RBMS;
using Reclamation.TimeSeries.Import;

#if !PISCES_OPEN
using Reclamation.TimeSeries.Excel;
#endif 
using Reclamation.TimeSeries.DataLogger;
using Reclamation.TimeSeries.SHEF;
using System.Collections.Generic;


namespace Reclamation.TimeSeries.Forms
{

    public partial class PiscesForm
    {



        private void addRBMSDirectory(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string[] files = Directory.GetFiles(fbd.SelectedPath);
                for (int i = 0; i < files.Length; i++)
                {
                    RBMSTextFile.ImportFile(files[i], DB, true);
                } 
            }
        }


        private void AddRBMS_csv_File_Click(object sender, EventArgs e)
        {

            try
            {
                Logger.EnableLogger();
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.DefaultExt = ".csv";
                openFileDialog.Filter = "CSV File *.csv | *.csv";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    RBMSTextFile.ImportFile(openFileDialog.FileName, DB, true);
                    MessageBox.Show("Manual Import Complete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        /// <summary>
        /// SQLite convered from DSS
        /// https://github.com/usbr/convertdss
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addSqLiteModel_Click(object sender, EventArgs e)
        {
            var dlg = new Reclamation.TimeSeries.ScenarioPicker.ScenarioPicker();

            dlg.Text = "Select SqLite files";
            dlg.Dialog.DefaultExt = ".db";
            dlg.Dialog.Filter = "SQLite *.db|*.db|All Files|*.*";
            dlg.Dialog.Title = "Open SQLite File (from DSS)";
            try
            {
                DB.SuspendTreeUpdates();
                var result = dlg.ShowDialog();

                if (result == DialogResult.OK)
                {
                    if (dlg.ScenariosChecked && dlg.ScenarioFiles.Count > 0)
                    {
                        //create scenarios
                        ShowAsBusy("Reading data");
                        var tblScen = DB.GetScenarios();
                        foreach (var item in dlg.ScenarioFiles)
                        {
                            string scenarioPath = ConnectionStringUtility.MakeFileNameRelative("FileName=" + item, DB.DataSource);
                            tblScen.AddScenarioRow(Path.GetFileNameWithoutExtension(item), true, scenarioPath,0);
                        }
                        //add first file in the list to the tree
                        if (dlg.AddToTreeChecked)
                        {
                            SQLiteSeries.CreatePiscesTree(dlg.ScenarioFiles[0], CurrentFolder, DB);
                        }
                        DB.Server.SaveTable(tblScen);
                        DatabaseChanged();
                    }
                    else
                        if (dlg.AddToTreeChecked)
                        {
                            //add to tree, but not to scenairo list
                            ShowAsBusy("Reading  data");
                            for (int i = 0; i < dlg.ScenarioFiles.Count; i++)
                            {
                                string fn = dlg.ScenarioFiles[i].ToString();
                              SQLiteSeries.CreatePiscesTree(fn, CurrentFolder, DB);
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
                    TimeSeriesDatabase db = new TimeSeriesDatabase(svr,false);
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
            CalculationProperties p = new CalculationProperties(s, new TimeSeriesTreeModel( DB), DB);

            if (p.ShowDialog() == DialogResult.OK)
            {
                DB.AddSeries(s, CurrentFolder);
                // tree refresh.. Add node.
                if( p.Calculate)
                        s.Calculate(); // save again
                // refresh.
                //DB.RefreshFolder(CurrentFolder);
                DatabaseChanged(); ;
                
            }
        }


        void AddSeriesClick(object sender, System.EventArgs e)
        {
            var a = new AddSeries();
            if (a.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Series s = new Series();
                s.Name = a.SeriesName;
                s.Table.TableName = a.TableName;
                s.Parameter = a.Parameter;
                s.TimeInterval = a.TimeInterval;
                DB.AddSeries(s, CurrentFolder);
                DatabaseChanged();
            }
            
        }

        /// <summary>
        /// Creates a new series from a text file
        /// </summary>
        void AddTextFileClick(object sender, System.EventArgs e)
        {
            if (openTextFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string filename in openTextFileDialog.FileNames)
                {
                    Series s = TextSeries.ReadFromFile(filename);
                    if (s.Count == 0)
                    {
                        MessageBox.Show("No data found in file:" + filename);
                        continue;
                    }

                    if (s.Count > 0)
                    {
                        DB.AddSeries(s, CurrentFolder);
                    }
                }
            }
        }

        private void AddCampbellDataLogger(object sender, EventArgs e)
        {
            if (openFileDialogCr10x.ShowDialog() == DialogResult.OK)
            {
                TextFile tf = new TextFile(openFileDialogCr10x.FileName);
                if (LoggerNetFile.IsValidFile(tf))
                {
                    // import all columns in LoggerNet file.
                    LoggerNetFile ln = new LoggerNetFile(tf);

                    foreach (var item in ln.ToSeries())
                    {
                        DB.AddSeries(item);
                    }                    
                    //ln.ToSeries()
                }
                else
                {
                    ImportCr10x();
                }
            }
        }

        private void ImportCr10x()
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

                }
                else
                    if (s.Count > 0)
                    {
                        DB.AddSeries(s, CurrentFolder);
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

                    if (w.ImportType == ExcelImportType.Standard)
                    {
                        ImportExcelStandard(openExcelDialog.FileName);
                    }
                    else if (w.ImportType == ExcelImportType.WaterYear)
                    {
                        ImportExcelWaterYear(openExcelDialog.FileName);
                    }
                    else if (w.ImportType == ExcelImportType.Database)
                    {
                        ImportExcelDatabaseStyle(openExcelDialog.FileName);
                    }
                    else if (w.ImportType == ExcelImportType.Traces)
                    {
                        ImportExcelTraces(openExcelDialog.FileName);
                    }
                }
            }
        }


        private void ImportExcelTraces(string filename)
        {
#if !PISCES_OPEN
            ImportExcelTraces dlg = new ImportExcelTraces(filename);
            if (dlg.ShowDialog() == DialogResult.OK)
            {

            }
#endif
        }
        


        private void ImportExcelDatabaseStyle(string filename)
        {
#if !PISCES_OPEN
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
#endif
        }


        private void ImportExcelWaterYear(string filename)
        {
#if!PISCES_OPEN
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
#endif
        }


        private void ImportExcelStandard(string filename)
        {
#if !PISCES_OPEN
            ImportExcelStandard dlg = new ImportExcelStandard(filename, DB.GetUniqueUnits());

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var messageList = new List<string>();
                // No scenarios 
                if (dlg.ScenarioValues == null)
                {
                    for (int i = 0; i < dlg.ValueColumns.Length; i++)
                    {
                        Series s = SpreadsheetGearSeries.ReadFromWorkbook(dlg.WorkBook, dlg.SheetName, dlg.DateColumn, dlg.ValueColumns[i], false, dlg.ComboBoxUnits.Text);
                        if (s.Count > 0)
                        {
                            DB.AddSeries(s, CurrentFolder);
                        }
                        else
                        {
                            messageList.Add("No data in the selection.  Worksheet: " + dlg.SheetName);
                        }

                        if (s.Messages.Count > 0)
                        {
                            messageList.Add(s.Messages.ToString());
                        }
                    }
                }
                // Process all scenarios
                else
                {
                    var scenarioList = dlg.ScenarioValues;
                    for (int i = 0; i < dlg.ValueColumns.Length; i++)
                    {
                        SeriesList sList = new SeriesList();
                        foreach (string scenario in scenarioList)
                        {
                            Series s = SpreadsheetGearSeries.ReadFromWorkbook(dlg.WorkBook, dlg.SheetName, dlg.DateColumn, dlg.ValueColumns[i], dlg.ScenarioColumn, scenario, dlg.ComboBoxUnits.Text);
                            sList.Add(s);

                        }
                    }
                }
                if (messageList.Count > 0)
                {
                    MessageBox.Show(String.Join("\n",messageList.ToArray()),"Import Warnings",MessageBoxButtons.OK);
                }
            }
#endif
        }

        private void AddBpaHydsimClick(object sender, EventArgs e)
        {
            var dlg = new Reclamation.TimeSeries.ScenarioPicker.ScenarioPicker();

            dlg.Text = "Select Hydsim Output Files";
            dlg.Dialog.DefaultExt = ".mdb";
            dlg.Dialog.Filter = "BPA Hydsim Access (*.mdb, *.accdb)|*.mdb;*.accdb|All Files|*.*";
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
                            tblScen.AddScenarioRow(Path.GetFileNameWithoutExtension(item), true, scenarioPath,0);
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
                    Series s = HydrometInfoUtility.Read(dlg.Cbtt, dlg.ParameterCode, dlg.T1, dlg.T2,
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
                        if (dlg.IsGroundWaterLevel || dlg.IsGroundWaterDepth)
                        {
                            if (dlg.IsGroundWaterDepth)
                            {
                                var sTemp = new Usgs.UsgsGroundWaterLevelSeries(siteID, true);
                                sTemp.Read(TimeSeriesDatabase.MinDateTime, TimeSeriesDatabase.MaxDateTime);
                                s = sTemp;
                                //s = Usgs.UsgsGroundWaterLevelSeries.Read(siteID, TimeSeriesDatabase.MinDateTime, TimeSeriesDatabase.MaxDateTime);//, dlg.T1, dlg.T2);
                            }
                            else
                            {
                                s = Usgs.UsgsGroundWaterLevelSeries.Read(siteID, TimeSeriesDatabase.MinDateTime, TimeSeriesDatabase.MaxDateTime);//, dlg.T1, dlg.T2);
                            }
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
                            tblScen.AddScenarioRow(Path.GetFileNameWithoutExtension(item), true, scenarioPath,0);
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
                        tblScen.AddScenarioRow(Path.GetFileNameWithoutExtension(item), true, scenarioPath,0);
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
                            tblScen.AddScenarioRow(Path.GetFileNameWithoutExtension(item), true, scenarioPath,0);
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
        }


        private void addHDB_Click(object sender, EventArgs e)
        {
        }
        private void addHDBConfigFile_Click(object sender, EventArgs e)
        {
        }


        /// <summary>
        /// Add SHEF data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addShef_Click(object sender, EventArgs e)
        {
            ImportShef dlg = new ImportShef();


            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                bool isImportAll = dlg.IsImportAll();

                DataTable dTab;
                string shefLocation, shefCode, shefFile;
                dTab = dlg.GetShefTable();
                shefLocation = dlg.GetShefLocation();
                shefCode = dlg.GetShefCode();
                shefFile = dlg.GetShefFileName();

                if (isImportAll)
                {
                    AddAllShef(dTab, shefFile);
                }
                else
                {
                    var s = new ShefSeries(shefLocation, shefCode, shefFile);
                    var valTable = dTab.Select(string.Format("location = '{0}' AND shefcode = '{1}'", shefLocation, shefCode));
                    foreach (DataRow item in valTable)
                    {
                        s.Add(DateTime.Parse(item["datetime"].ToString()), Convert.ToDouble(item["value"]));
                    }
                    DB.AddSeries(s, CurrentFolder);
                }
            }
        }

        private void AddAllShef(DataTable dTab, string shefFile)
        {
            // Get Locations
            DataView view = new DataView(dTab);
            DataTable distinctLocations = view.ToTable(true, "location");
            foreach (DataRow locationItem in distinctLocations.Rows)
            {
                string shefLocation = locationItem["location"].ToString();
                // Get location-pcode pairs
                DataTable distinctPairs = view.ToTable(true, "location", "shefcode");
                var distinctCodes = new DataView(distinctPairs);
                distinctCodes.RowFilter = "location = '" + shefLocation + "'";
                // Get Pcodes
                var codeTable = distinctCodes.ToTable();
                foreach (DataRow codeItem in codeTable.Rows)
                {
                    string shefCode = codeItem["shefcode"].ToString();
                    // Add Series
                    var s = new ShefSeries(shefLocation, shefCode, shefFile);
                    var valTable = dTab.Select(string.Format("location = '{0}' AND shefcode = '{1}'", shefLocation, shefCode));
                    foreach (DataRow item in valTable)
                    {
                        s.Add(DateTime.Parse(item["datetime"].ToString()), Convert.ToDouble(item["value"]));
                    }
                    DB.AddSeries(s, CurrentFolder);
                }
            }
        }


        private void importFromDirectoryToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {

                DB.SuspendTreeUpdates();
                var path = UserPreference.Lookup("bulk_import_folder");
                
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                if (Directory.Exists(path))
                {
                    dlg.SelectedPath = path;
                }
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    UserPreference.Save("bulk_import_folder",dlg.SelectedPath);
                    BulkImportForm f = new BulkImportForm();
                    f.ImportClick += delegate {
                        BulkImportDirectory.Import(DB,f.SelectedPath, f.Filter,f.RegexFilter);
                    };
                    f.SelectedPath = dlg.SelectedPath;

                    f.ShowDialog();
                    DatabaseChanged();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                DB.ResumeTreeUpdates();
            }

          
        }

        void f_ImportClick(object sender, EventArgs e)
        {

        }        
        
        
        /// <summary>
        /// Add IDWR data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addIdwr_Click(object sender, EventArgs e)
        {
            ImportIdwrData dlg = new ImportIdwrData();

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string station = dlg.station;
                string parameter = dlg.parameter;
                DateTime t1 = dlg.tStart;
                DateTime t2 = dlg.tEnd;

                var s = new IDWR.IDWRDailySeries(station, parameter);
                s.Read(t1, t2);
                DB.AddSeries(s, CurrentFolder);
            }
        }

    }
}