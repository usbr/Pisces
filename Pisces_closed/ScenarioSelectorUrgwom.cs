using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Reclamation.Core;
using System.Configuration;

namespace Reclamation.TimeSeries.Forms
{
    /// <summary>
    /// Form that allows user to selectively choose multiple 
    /// scenarios
    /// </summary>
    public partial class ScenarioSelectorUrgwom : Form, Reclamation.TimeSeries.IScenarioSelector
    {
        private DataTable YearTable;
        private TimeSeriesDatabase m_db;
        CheckBox[] cbPercents;
        CheckBox[] cbMonths;
        string[] pctLevels = new string[] { "10", "30", "50", "70", "90" };
        public ScenarioSelectorUrgwom(TimeSeriesDatabase db)
        {
             m_db = db;
            InitializeComponent();
            LoadYearTable();
            cbPercents = new CheckBox[]{ checkBox10, checkBox30, checkBox50, checkBox70, checkBox90 };
            cbMonths = new CheckBox[] {checkBoxMonth1, checkBoxMonth2 };
            // put tag on checkboxes
            checkBoxMonth1.Text = UrgwomUtility.UrgwomMonths[0];
            checkBoxMonth2.Text = UrgwomUtility.UrgwomMonths[1];

        }
        public bool MergeSelected
        {
            get { return false; }
        }

        private void LoadYearTable()
        {
            YearTable = new DataTable("Years");
            YearTable.Columns.Add("Year");
            YearTable.Columns.Add("Checked", typeof(bool));
            YearTable.Columns.Add("Run");
            int run = 0;
            for (int year =UrgwomUtility.UrgwonStartYear ; year <=UrgwomUtility.UrgwomEndYear; year++)
            {
                YearTable.Rows.Add(year.ToString(),false, run);
                run++;
            }
            this.dataGridView1.DataSource = YearTable;
        }

        public bool SubtractFromBaseline
        {
            get { return this.checkBoxSubtractFromBaseline.Checked; }
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

        /// <summary>
        /// Save changes, translating from UI, to scenario table
        /// </summary>
        private void SaveChanges()
        {
            var scenarioTable = m_db.GetScenarios();

            foreach (var row in scenarioTable)
            {
                row.Checked = 
                    IsYearChecked(row.GetConnectionStringParameter("Year"))
                 && IsMonthSelected(row.GetConnectionStringParameter("Month"))
                 && IsPercentSelected(row.GetConnectionStringParameter("Percent"));
            }
            
            m_db.Server.SaveTable(scenarioTable);
            ReadSelectionsFromScenarioTable();
        }

        private void ReadSelectionsFromScenarioTable()
        {
            // clear all selections
            ClearAllSelections();

            foreach (var row in m_db.GetSelectedScenarios())
            {
                CheckYear(row.GetConnectionStringParameter("Year"));
                SelectMonth(row.GetConnectionStringParameter("Month"));
                SelectPercent(row.GetConnectionStringParameter("Percent"));
            }
        }

        private void SelectPercent(string percent)
        {
            int idx = Array.IndexOf(pctLevels, percent);
            if (idx >= 0)
            {
                cbPercents[idx].Checked = true;
            }
        }

        private void SelectMonth(string month)
        {
            foreach (var cb in cbMonths)
            {
                if (cb.Text.ToLower() == month.ToLower())
                {
                    cb.Checked = true;
                }
            }
        }

        private void CheckYear(string year)
        {
            var rows = YearTable.Select("Year='" + year + "'");
            if (rows.Length > 0)
            {
                rows[0]["Checked"] = true;
            }
        }

        private void ClearAllSelections()
        {
            LoadYearTable();  // reloading table clears years
            foreach (var cb in cbPercents)
            {
                cb.Checked = false;
            }
            foreach (var cb in cbMonths)
            {
                cb.Checked = false;
            }
        }

        
        private bool IsPercentSelected(string percent)
        {
            int idx = Array.IndexOf(pctLevels, percent);
            if (idx >= 0)
                return cbPercents[idx].Checked;

            return false;
        }

        private bool IsMonthSelected(string month)
        {
            foreach (var cb in cbMonths)
            {
                if( month.ToLower() == cb.Text.ToLower()
                    && cb.Checked )
                    return true;
            }

            return false;
        }

        private bool IsYearChecked(string year)
        {
            var rows = YearTable.Select("Year='" + year + "'");
            if (rows.Length > 0)
            {
                if (Convert.ToBoolean(rows[0]["Checked"]))
                    return true;
            }
            return false;

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
            if (OnCancel != null)
                OnCancel(this, EventArgs.Empty);
            Visible = false;
        }

        private void ScenarioSelector_Load(object sender, EventArgs e)
        {
            ReadSelectionsFromScenarioTable();
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

        private void checkBoxSubtractFromBaseline_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxIncludeSelected.Enabled = checkBoxSubtractFromBaseline.Checked;
        }
        
    }
}