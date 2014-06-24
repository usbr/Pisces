using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Forms
{
    /// <summary>
    /// Form that allows user to selectively choose multiple 
    /// scenarios
    /// </summary>
    public partial class ScenarioSelector : Form, Reclamation.TimeSeries.IScenarioSelector
    {
        TimeSeriesDatabase m_db;
        private TimeSeriesDatabaseDataSet.ScenarioDataTable scenarioTable;
        public ScenarioSelector(TimeSeriesDatabase db)
        {
            InitializeComponent();
            m_db = db;
            ReloadGrid();
        }

        private void ReloadGrid()
        {
            this.scenarioTable = m_db.GetScenarios();
            this.dataGridView1.DataSource = scenarioTable;
            dataGridView1.Columns["SortOrder"].Visible = false;
        }


        public bool SubtractFromBaseline
        {
            get { return this.checkBoxSubtractFromBaseline.Checked; }
        }

        public string BaselineScenario
        {
            get{
                if (dataGridView1.RowCount == 0)
                {
                    return "";
                }
                return dataGridView1[1, 0].Value.ToString();
            }
        }
        

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1["Checked", i].Value = true;
            }
            dataGridView1.Select();// ui hack
        }

        private void buttonClearAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1["Checked", i].Value = false;
            }
            dataGridView1.Select();// ui hack
        }
        
        private void CustomizeClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
        }

        private void SaveChanges()
        {
            m_db.Server.SaveTable(scenarioTable);
            ReloadGrid();
        }

        public event EventHandler OnApply;

        private void buttonApply_Click(object sender, EventArgs e)
        {
            SaveChanges();

            if (OnApply != null)
                OnApply(this, EventArgs.Empty);
        }

       

        private void buttonOK_Click(object sender, EventArgs e)
        {
            SaveChanges();
            if (OnApply != null)
                OnApply(this, EventArgs.Empty);
            Visible = false;
        }

        public event EventHandler OnCancel;
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.scenarioTable.RejectChanges();
            if (OnCancel != null)
                OnCancel(this, EventArgs.Empty);
            Visible = false;
        }

        private void ScenarioSelector_Load(object sender, EventArgs e)
        {
            //Console.WriteLine("hi");
        }

        private void ScenarioSelector_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Visible = false;
        }
        public bool IncludeBaseline
        {
            get { return checkBoxIncludeBaseline.Checked; }
        }

        public bool IncludeSelected
        {
            get { return checkBoxIncludeSelected.Checked; }
        }


        public bool MergeSelected
        {
            get { return checkBoxMerge.Checked; }
        }


        private void checkBoxSubtractFromBaseline_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSubtractFromBaseline.Checked)
            {
                checkBoxIncludeBaseline.Enabled = true;
                checkBoxIncludeSelected.Enabled = true;
            }
            else
            {
                checkBoxIncludeBaseline.Enabled = false;
                checkBoxIncludeSelected.Enabled = false;
            }

        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            
        }

        private void linkLabelSortOrder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            FormSumRange sr = new FormSumRange();
            if (sr.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var s = m_db.GetSeriesFromName(sr.SeriesName);
                if (s == null)
                {
                    MessageBox.Show("Error: The Series '"+sr.SeriesName +"' could not be found. Please check the spelling.  A short name without spaces works best.");
                    return;
                }

                s.Read();
               
                Series sum = Reclamation.TimeSeries.Math.AnnualSum(s, sr.MonthDayRange, 10);
                for (int i = 0; i < scenarioTable.Rows.Count; i++)
                {
                    int yr;
                    if (!Int32.TryParse(scenarioTable[i].Name, out yr))
                    {
                        MessageBox.Show("Error: The scenario name must be a year (integer) ");
                    }

                    YearRange yRange = new YearRange(yr, 10);
                    Series sYear = Reclamation.TimeSeries.Math.Subset(sum, yRange.DateTime1, yRange.DateTime2);

                    if (sYear.Count > 0)
                    {
                        scenarioTable[i].SortOrder = Convert.ToInt32(sYear[0].Value);
                        //scenarioTable[i].Path+=.Rows[i]["Note"] = sYear[0].Flag;
                    }
                }
                dataGridView1.Columns["SortOrder"].Visible = true;

            }
        }
        
    }
}