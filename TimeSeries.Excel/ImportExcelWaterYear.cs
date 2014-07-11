using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.IO;
using Reclamation.TimeSeries.Excel;

namespace Reclamation.TimeSeries.Forms.ImportForms
{
    public partial class ImportExcelWaterYear : Form
    {
        SpreadsheetGearExcel db;
        public ImportExcelWaterYear()
        {
            InitializeComponent();
        }

        public SpreadsheetGear.IWorkbook WorkBook
        {
            get { return db.Workbook; }
        }

        public ImportExcelWaterYear(string filename, string[] DBunits)
        {
            InitializeComponent();
            db = new SpreadsheetGearExcel(filename);

            this.label1.Text = Path.GetFileName(filename);
            checkedListBoxSheets.Items.Clear();
            checkedListBoxSheets.Items.AddRange(db.SheetNames);
            //comboBoxSheetNames.SelectedIndex = 0;
            LoadList(comboBoxUnits, DBunits);
            RefreshColumnNames();

        }

        public string[] SheetNames
        {
            get {

                var rval = new List<String>();
                foreach (var item in checkedListBoxSheets.CheckedItems)
                {
                    rval.Add(item.ToString());
                }
                return rval.ToArray();
            }
        }

        private string SheetName()
        {
            if( SheetNames.Length >0)
            return SheetNames[0];
            return "";
        }

        public string DateColumn
        {
            get
            {
              return  comboBoxWaterYear.SelectedItem.ToString();
            }
        }

        public string ValueColumn
        {
            get
            {
                return comboBoxValueColumn.SelectedItem.ToString();
            }
        }
        private void RefreshColumnNames()
        {
            this.comboBoxWaterYear.Items.Clear();
            
            this.comboBoxValueColumn.Items.Clear();
            string[] columnNames = { };

            labelError.Text = "";
            try
            {
                if (SheetName() != "")
                {

                    if (this.radioButtonHeaderRow.Checked)
                        columnNames = db.ColumnNames(SheetName());
                    else
                        columnNames = db.ColumnReferenceNames(SheetName());
                }
            }
            catch (DuplicateNameException e)
            {
                 columnNames = new string[] {};
                labelError.Text = "Error: "+e.Message;
            }

            this.comboBoxWaterYear.Items.AddRange(columnNames);
            this.comboBoxValueColumn.Items.AddRange(columnNames);
            //this.checkedListBoxColumns.Items.AddRange(columnNames);
            if (columnNames.Length > 0)
            {
                comboBoxWaterYear.SelectedIndex = 0;
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

        //private void comboBoxSheetNames_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //  RefreshColumnNames();
        //}

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
            for (int i = 0; i < this.checkedListBoxSheets.Items.Count; i++)
            {
                checkedListBoxSheets.SetItemChecked(i, true);
            }
        }

        private void buttonClearAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.checkedListBoxSheets.Items.Count; i++)
            {
                checkedListBoxSheets.SetItemChecked(i, false);
            }
        }

        private void checkedListBoxSheets_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshColumnNames();
        }

    }
}