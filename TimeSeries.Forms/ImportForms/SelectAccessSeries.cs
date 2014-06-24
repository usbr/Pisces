using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Forms.ImportForms
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SelectAccessSeries : Form
    {
        AccessDB db;
        public SelectAccessSeries()
        {
            InitializeComponent();
        }

        public SelectAccessSeries(string filename)
        {
            InitializeComponent();
            db = new AccessDB(filename);
            this.labelFileName.Text = filename;
            comboBoxTableNames.Items.Clear();
            comboBoxTableNames.Items.AddRange(db.TableNames());
            comboBoxTableNames.Items.AddRange(db.QueryNames());
            if (comboBoxTableNames.Items.Count > 0)
            {
                comboBoxTableNames.SelectedIndex = 0;
            }
            ReloadDropDowns();

        }
        public string FileName
        {
            get { return labelFileName.Text; }
        }

        public string TableName
        {
            get {
                return this.comboBoxTableNames.SelectedItem.ToString();
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
                return  comboBoxValueColumn.SelectedItem.ToString();;
            }
        }

        public string FilterColumn
        {
            get
            {
                if (comboBoxSiteColumn.SelectedIndex >= 0)
                    return comboBoxSiteColumn.SelectedItem.ToString();
                return "";
            }
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

        private void ReloadDropDowns()
        {
            ReloadComboBoxDate();
            ReloadComboBoxValues();
            ReloadFilterComboBox();
        }

        private void ReloadFilterComboBox()
        {
            comboBoxSiteColumn.Items.Clear();
            string[] columnNames = ColumnNames(TableName, ColumnType.All);
            this.comboBoxSiteColumn.Items.AddRange(columnNames);
            ReloadSiteList();
        }

        private void ReloadSiteList()
        {
            checkedListBoxSites.Items.Clear();
            if (comboBoxSiteColumn.SelectedIndex >= 0)
            {
                string colName = comboBoxSiteColumn.SelectedItem.ToString();
                string[] distinct = GetDistinct(colName);
                checkedListBoxSites.Items.AddRange(distinct);
            }
        }

        private string[] GetDistinct(string colName)
        {
            string sql = "Select Distinct [" + colName + "] from [" + TableName + "]";
            DataTable tbl = db.Table(TableName, sql);

            return DataTableUtility.Strings(tbl, "", colName);
        }

        private void ReloadComboBoxValues()
        {
            this.comboBoxValueColumn.Items.Clear();
            string[] valueColumns = ColumnNames(TableName, ColumnType.NoDate);
            this.comboBoxValueColumn.Items.AddRange(valueColumns);
            if (valueColumns.Length > 0)
            {
                comboBoxValueColumn.SelectedIndex = 0;
            }
            else
            {
                buttonOK.Enabled = false;
            }
        }

        private void ReloadComboBoxDate()
        {
            this.comboBoxDateColumn.Items.Clear();
            string[] dateColumns = ColumnNames(TableName, ColumnType.Date);
            this.comboBoxDateColumn.Items.AddRange(dateColumns);
            if (dateColumns.Length > 0)
            {
                comboBoxDateColumn.SelectedIndex = 0;
                buttonOK.Enabled = true;
            }
            else
            {
                buttonOK.Enabled = false;
            }

        }

        enum ColumnType { Date,NoDate,All};

        private string[] ColumnNames(string tableName, ColumnType columnType)
        {
            var rval = new List<string>();
            string sql = "select * from [" + tableName + "] where 0 = 1";
           DataTable tbl =  db.Table(tableName, sql);

            for (int i = 0; i < tbl.Columns.Count; i++)
			{
                DataColumn c = tbl.Columns[i];

                if (columnType == ColumnType.All)
                {
                    rval.Add(c.ColumnName);
                }
                else
                if (c.DataType == typeof(DateTime)
                    && columnType == ColumnType.Date)
                {
                    rval.Add(c.ColumnName);
                }
                else
                    if (c.DataType != typeof(DateTime)
                    && columnType == ColumnType.NoDate)
                    {
                        rval.Add(c.ColumnName);
                    }
			}
            return rval.ToArray();
        }

        private void comboBoxSheetNames_SelectedIndexChanged(object sender, EventArgs e)
        {
          ReloadDropDowns();
        }

        private void radioButtonNoHeader_CheckedChanged(object sender, EventArgs e)
        {
            ReloadDropDowns();
        }

        private void radioButtonHeaderRow_CheckedChanged(object sender, EventArgs e)
        {
            ReloadDropDowns();
        }

        private void comboBoxFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadSiteList();
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
    }
}