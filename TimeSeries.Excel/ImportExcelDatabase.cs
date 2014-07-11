using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.IO;
using Reclamation.TimeSeries.Excel;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Forms.ImportForms
{
    /// <summary>
    /// Imports Excell data from a well formed (database like)
    /// sheet.  Sheet should have a date,value, and site column
    /// </summary>
    public partial class ImportExcelDatabase : Form
    {
        SpreadsheetGearExcel db;
        public ImportExcelDatabase()
        {
            InitializeComponent();
        }

        public SpreadsheetGear.IWorkbook WorkBook
        {
            get { return db.Workbook; }
        }

        public ImportExcelDatabase(string filename, string[] DBunits)
        {
            InitializeComponent();
            db = new SpreadsheetGearExcel(filename);

            this.label1.Text = Path.GetFileName(filename);
            comboBoxSheetNames.Items.Clear();
            comboBoxSheetNames.Items.AddRange(db.SheetNames);
            comboBoxSheetNames.SelectedIndex = 0;
            LoadList(comboBoxUnits, DBunits);
            RefreshColumnNames();

        }

        public string SheetName
        {
            get {
                return this.comboBoxSheetNames.SelectedItem.ToString();
            }
        }

        public string DateColumn
        {
            get
            {
              return  comboBoxDateColumn.SelectedItem.ToString();
            }
        }

        public string ValueColumn
        {
            get
            {
                return  comboBoxValueColumn.SelectedItem.ToString();
            }
        }

        public string SiteColumn
        {
            get { return comboBoxFilter.SelectedItem.ToString(); }
        }

        public string[] SelectedSites
        {
            get
            {
                var rval = new List<String>();
                foreach (var item in checkedListBoxSites.CheckedItems)
                {
                    rval.Add(item.ToString());
                }
                return rval.ToArray();
            }
        }
        private void RefreshColumnNames()
        {
            this.comboBoxDateColumn.Items.Clear();
            this.comboBoxFilter.Items.Clear();
            this.checkedListBoxSites.Items.Clear();
            this.comboBoxValueColumn.Items.Clear();

            string[] columnNames = { };

            labelError.Text = "";
            try
            {
                if (this.radioButtonHeaderRow.Checked)
                    columnNames = db.ColumnNames(SheetName);
                else
                    columnNames = db.ColumnReferenceNames(SheetName);
            }
            catch (DuplicateNameException e)
            {
                 columnNames = new string[] {};
                labelError.Text = "Error: "+e.Message;
            }

            this.comboBoxDateColumn.Items.AddRange(columnNames);
            this.comboBoxFilter.Items.AddRange(columnNames);
            this.comboBoxValueColumn.Items.AddRange(columnNames);

            

            if (columnNames.Length > 0)
            {
                comboBoxDateColumn.SelectedIndex = 0;
                //comboBoxValueColumn.SelectedIndex = 0;
                buttonOK.Enabled = true;
            }
            else
            {
                buttonOK.Enabled = false;
            }

        }

        public ComboBox ComboBoxUnits
        {
            get { return this.comboBoxUnits; }
        }

        private void LoadList(ComboBox owner, string[] list)
        {
            owner.Items.Clear();
            for (int i = 0; i < list.Length; i++)
            {
                owner.Items.Add(list[i]);
            }
        }

        private void comboBoxSheetNames_SelectedIndexChanged(object sender, EventArgs e)
        {
          RefreshColumnNames();
        }

        private void radioButtonNoHeader_CheckedChanged(object sender, EventArgs e)
        {
            RefreshColumnNames();
        }

        private void radioButtonHeaderRow_CheckedChanged(object sender, EventArgs e)
        {
            RefreshColumnNames();
        }

        private void buttonSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.checkedListBoxSites.Items.Count; i++)
			{
                checkedListBoxSites.SetItemChecked(i, true);
			}
        }

        private void buttonClearAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.checkedListBoxSites.Items.Count; i++)
            {
                checkedListBoxSites.SetItemChecked(i, false);
            }
        }

        private void comboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                Application.DoEvents();
                this.checkedListBoxSites.Items.Clear();
                var tbl = db.GetDataTable(SheetName, SpreadsheetGear.Data.GetDataFlags.FormattedText);

                var table = DataTableUtility.SelectDistinct(tbl, SiteColumn);
                var sites = DataTableUtility.StringList(table, "", SiteColumn);

                this.checkedListBoxSites.Items.AddRange(sites.ToArray());
            }
            finally
            {
                Cursor = Cursors.Default;
            }

        }


    }
}