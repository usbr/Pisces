using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Reclamation.TimeSeries.Forms;

namespace Reclamation.TimeSeries.Forms
{
   /// <summary>
   /// Explorer allows user to view time series data as a graph or table.
   /// user can edit and save changes to time series data.
   /// </summary>
    public partial class Explorer : Form
    {


        private TimeSeriesPresenter presenter;
        TimeSeriesSettings input;
        private DisplayOptionsDialog displayOptionsDialog1;
        private Viewer viewer1;
        private DataTable tblTree;
        
        private ScenarioChooser scenarioChooser1;

        public Explorer(TimeSeriesDatabase database, DataTable tree)
        {
            //PeriodOfRecordOptions.EnableFullPeriodOfRecordQueries = database.SupportsPeriodOfRecordQueries;
            input = new TimeSeriesSettings();
            input.AllowFullPeriodOfRecord  =database.SupportsPeriodOfRecordQueries;
            input.DatabaseList.Add(database);
            Init(tree);
            presenter = new TimeSeriesPresenter(viewer1);
        }

        /// <summary>
        /// Constructs time Series Explorer.
        /// </summary>
        /// <param name="databaseList"></param>
        /// <param name="tree">DataTable to represent the tree, must have label and level columns
        /// label is a string, level is an integer</param>
        public Explorer(TimeSeriesDatabaseList databaseList, DataTable tree)
        {
            input = new TimeSeriesSettings();
            input.DatabaseList = databaseList;
            Init( tree);
        }

        private void Init( DataTable tree)
        {
            InitializeComponent();
            displayOptionsDialog1 = new TimeSeries.Forms.DisplayOptionsDialog(input);
            this.tblTree = tree;

            SetupScenarios();

            if (!tblTree.Columns.Contains("DataSource"))
            {
                tblTree.Columns.Add("DataSource");
                tblTree.Columns["DataSource"].DefaultValue = "";
            }

            viewer1 = new Viewer();
            viewer1.Parent = this.splitContainer1.Panel2;
            viewer1.BringToFront();
            viewer1.Dock = DockStyle.Fill;
            viewer1.Visible = true;

            if (input.DatabaseList.SupportExpertQueries)
            {
                this.expertSiteSelection1.Tree = tree;
            }
            else
            {
                this.tree1.Parent = this.splitContainer1.Panel1;
                this.tabControlSiteSelection.SendToBack();
            }
        }

        

        public ToolStripItemCollection MenuItems
        {
        get{ return this.menuStrip1.Items;}

         }
       

        private void SetupScenarios()
        {
            
            scenarioChooser1 = new ScenarioChooser(input.DatabaseList.ScenarioNames.ToArray());
            UpdateScenariosMenuLabel();
            if (input.DatabaseList.ScenarioNames.Count <= 1)
            {
                this.toolStripMenuItemScenarios.Visible = false;
            }
        }

        
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        public string Title
        {
            set { this.Text = value; }
        }
        private void FormMain_Load(object sender, EventArgs e)
        {
          tree1.LoadTree(tblTree,null);
        }

       

        private void tree1_Selected(object sender, EventArgs e)
        {
            bool catchExceptions = true;

            if (catchExceptions)
            {
                Cursor = Cursors.WaitCursor;
                try
                {
                    UpdateView();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
                }
                finally
                {
                    Cursor = Cursors.Default;
                    if (formProgress1 != null && formProgress1.Visible )
                    {
                        formProgress1.Hide();
                    }
                }
            }

            else
            {
                UpdateView();
                if (formProgress1 != null && formProgress1.Visible)
                {
                    formProgress1.Hide();
                    // formProgress1.Dispose();
                    //formProgress1 = null;

                }
            }

        }

        FormProgress formProgress1;
        private void UpdateView()
        {
            formProgress1 = new FormProgress();
            formProgress1.StartPosition = FormStartPosition.Manual;
            formProgress1.Location = new System.Drawing.Point(this.Location.X +this.Width/2, this.Location.Y + this.Height/2);
            formProgress1.Show();
            formProgress1.Refresh();
            displayOptionsDialog1.OnProgress += new cbp.ProgressEventHandler(displayDirector1_OnProgress);


            TimeSeries.SeriesList list = input.CreateSelectedSeries(scenarioChooser1.Selected,
                                                        SelectedSites());

            if (list.Count > 0)
            {
                presenter.Draw(input);
               // displayOptionsDialog1.Draw(list, viewer1);
            }
            else
            {
                viewer1.Clear();
            }
            
          
        }

      
        void displayDirector1_OnProgress(object sender, cbp.ProgressEventArgs e)
        {
            if (formProgress1 != null)
            {
                formProgress1.UpdateProgress(e.Message, e.PrecentComplete);
            }
        }

        private DataTable SelectedSites()
        {
            DataTable tblSelection = null;

            if (tabControlSiteSelection.SelectedTab == tabPageTree)
            {
                tblSelection = tree1.SelectedRows;
            }
            else
            {
                tblSelection = expertSiteSelection1.SelectedRows;
            }
            return tblSelection;
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (TimeSeriesDatabase db in input.DatabaseList)
            {
                db.Save();
            }
            
        }

        private void tree1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }


        private void toolStripMenuItemScenarios_Click(object sender, EventArgs e)
        {

            if (scenarioChooser1.ShowDialog() == DialogResult.OK)
            {
                tree1_Selected(this, EventArgs.Empty);
            }


            UpdateScenariosMenuLabel();
        }

        private void UpdateScenariosMenuLabel()
        {
            string[] scenarios = scenarioChooser1.Selected;
            if (scenarios.Length == 1)
            {
                this.toolStripMenuItemScenarios.Text = "Scenarios: " + scenarios[0];
            }
            else
            {
                this.toolStripMenuItemScenarios.Text = "Scenarios:(" + scenarios.Length + ") selected";
            }
        }

        

        private void toolStripDisplayType_Click(object sender, EventArgs e)
        {

            if (displayOptionsDialog1.ShowDialog() == DialogResult.OK)
            {
                tree1_Selected(this, new EventArgs());
            }

            this.toolStripDisplayType.Text = "Option:" + displayOptionsDialog1.SelectedOption;
        }

        private void expertSiteSelection1_OnSubmit(object sender, EventArgs e)
        {
            UpdateView();
        }

       
    }
}