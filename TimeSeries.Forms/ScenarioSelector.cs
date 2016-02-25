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
            LoadSeriesList();
            LoadMonthList();
        }

        private void ReloadGrid()
        {
            this.scenarioTable = m_db.GetScenarios();
            this.dataGridView1.DataSource = scenarioTable;
            dataGridView1.Columns["SortOrder"].Visible = false;
        }

        private void LoadSeriesList()
        {
            var seriesList = m_db.GetSeriesCatalog();
            int dropdownWidth = 256;
            this.comboBoxSelectedSeries.Items.Add("Run Index");
            foreach (var s in seriesList)
            {
                if (s.IsFolder == false)
                { 
                    this.comboBoxSelectedSeries.Items.Add(s.Name);
                    var nameLength = TextRenderer.MeasureText(s.Name, this.comboBoxSelectedSeries.Font).Width;
                    if (nameLength > dropdownWidth)
                    { dropdownWidth = nameLength; }
                }
            }
            this.comboBoxSelectedSeries.DropDownWidth = dropdownWidth;
            this.comboBoxSelectedSeries.SelectedIndex = 0;
        }

        private void LoadMonthList()
        {
            for (int i = 1; i <= 12; i++)
            { this.comboBoxMonths.Items.Add(new DateTime(2000, i, 1).ToString("MMM")); }
            this.comboBoxMonths.SelectedIndex = 0;
        }

        private void countYears()
        {
            this.comboBoxOutYear.Items.Clear();
            int yearCount = 0;
            var sName = comboBoxSelectedSeries.SelectedItem.ToString();
            if (!(sName == "Run Index"))
            {
                var s = m_db.GetSeriesFromName(sName);
                s.Read();
                if (this.radioButtonWY.Checked)
                { yearCount = s.MaxDateTime.WaterYear() - s.MinDateTime.WaterYear(); }
                else
                { yearCount = s.MaxDateTime.Year - s.MinDateTime.Year; }
                for (int i = 2; i <= yearCount; i++)
                { this.comboBoxOutYear.Items.Add(i); }
            }
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

        private void buttonSelectTop20P_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (i < (dataGridView1.Rows.Count - 1) * 0.20)
                { dataGridView1["Checked", i].Value = true; }
                else
                { dataGridView1["Checked", i].Value = false; }
            }
            dataGridView1.Select();// ui hack
        }

        private void buttonSelectMid20P_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                if (i >= (dataGridView1.Rows.Count - 1) * 0.40 && i <= (dataGridView1.Rows.Count - 1) * 0.60)
                { dataGridView1["Checked", i].Value = true; }
                else
                { dataGridView1["Checked", i].Value = false; }
            }
            dataGridView1.Select();// ui hack
        }

        private void buttonSelectLow20P_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (dataGridView1.Rows.Count - 1); i++)
            {
                if (i >= (dataGridView1.Rows.Count - 1) * 0.80)
                { dataGridView1["Checked", i].Value = true; }
                else
                { dataGridView1["Checked", i].Value = false; }
            }
            dataGridView1.Select();// ui hack
        }

        private void radioButtonYear2_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonYear2.Checked)
            { 
                this.comboBoxOutYear.Enabled = true;
                countYears();
            }
            else
            { this.comboBoxOutYear.Enabled = false; }
        }

        private void radioButtonMonth_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonMonth.Checked)
            { 
                this.comboBoxMonths.Enabled = true;
                this.radioButtonSum.Enabled = false;
                this.radioButtonMonth.Checked = true;
            }
            else
            { 
                this.comboBoxMonths.Enabled = false;
                this.radioButtonSum.Enabled = true;
            }
        }

        private void comboBoxSelectedSeries_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.radioButtonYear2.Checked)
            { countYears(); }
        }

        private void buttonSort_Click(object sender, EventArgs e)
        {
            var sName = "Powell_Outflow";// comboBoxSelectedSeries.SelectedItem.ToString();
            buttonClearAll_Click(sender, e);
                       
            if (!(sName == "Run Index"))
            {

                var s = m_db.GetSeriesFromName(sName);
                s.Read();

                int year1 = s.MinDateTime.Year;
                if (radioButtonYear2.Checked)
                { int year2 = s.MinDateTime.Year + Convert.ToInt16(comboBoxOutYear.SelectedItem); }
                else
                { int year2 = year1; }
                
                var mdRange = new MonthDayRange();
                if (radioButtonWY.Checked)
                { mdRange = new MonthDayRange(10, 1, 9, 30); }
                else
                { mdRange = new MonthDayRange(1, 1, 12, 31); }                
                
                for (int i = 0; i < scenarioTable.Rows.Count; i++)
                {
                    var ithScenario = s.CreateScenario(m_db.GetScenarios()[i]);
                    ithScenario.Read();

                    Series sSum = new Series();
                    if (radioButtonCY.Checked || radioButtonWY.Checked)
                    { sSum = Reclamation.TimeSeries.Math.AnnualSum(ithScenario, mdRange, mdRange.Month1); }

                    //int yr;
                    //if (!Int32.TryParse(scenarioTable[i].Name, out yr))
                    //{
                    //    MessageBox.Show("Error: The scenario name must be a year (integer) ");
                    //}

                    //YearRange yRange = new YearRange(yr, 10);
                    //Series sYear = Reclamation.TimeSeries.Math.Subset(sSum, yRange.DateTime1, yRange.DateTime2);

                    //if (sYear.Count > 0)
                    //{
                    //    scenarioTable[i].SortOrder = Convert.ToInt32(sYear[0].Value);
                    //    //scenarioTable[i].Path+=.Rows[i]["Note"] = sYear[0].Flag;
                    //}
                }
                dataGridView1.Columns["SortOrder"].Visible = true;
            }
            


        }
        
    }
}