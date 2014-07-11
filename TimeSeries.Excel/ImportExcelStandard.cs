using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.IO;
using Reclamation.TimeSeries.Excel;

namespace Reclamation.TimeSeries.Forms.ImportForms
{
    public partial class ImportExcelStandard : Form
    {
        SpreadsheetGearExcel db;
        public ImportExcelStandard()
        {
            InitializeComponent();
        }

        public SpreadsheetGear.IWorkbook WorkBook
        {
            get { return db.Workbook; }
        }

        public ImportExcelStandard(string filename, string[] DBunits)
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

        public string[] ValueColumns
        {
            
            get
            {
                var rval = new List<String>();
                foreach (var item in checkedListBoxColumns.CheckedItems)
	                {
                        rval.Add(item.ToString());
	                }
                //return  comboBoxValueColumn.SelectedItem.ToString();;
                return rval.ToArray();
            }
        }
        private void RefreshColumnNames()
        {
            this.comboBoxDateColumn.Items.Clear();
            
            this.checkedListBoxColumns.Items.Clear();
            //this.comboBoxValueColumn.Items.Clear();
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
            //this.comboBoxValueColumn.Items.AddRange(columnNames);
            
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
            Array.Sort(columnNames);
            this.checkedListBoxColumns.Items.AddRange(columnNames);
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
            for (int i = 0; i < this.checkedListBoxColumns.Items.Count; i++)
			{
                checkedListBoxColumns.SetItemChecked(i, true);
			}
        }

        private void buttonClearAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.checkedListBoxColumns.Items.Count; i++)
            {
                checkedListBoxColumns.SetItemChecked(i, false);
            }
        }


    }
}