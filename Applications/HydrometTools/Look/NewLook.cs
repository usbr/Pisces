using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries;

namespace Look
{
    public partial class NewLook : UserControl
    {
        TimeInterval db = TimeInterval.Irregular;
        string str = "";
        public NewLook()
        {
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            //generate results from search string
            DataTable tblSite = HydrometInfoUtility.HydrometInventory;
            DataTable Results = tblSite.Clone();
            DataTable RefinedResults = tblSite.Clone();
            DataRow[] RowSearchResults;

            string[] col = { "DataType", "cbtt", "cbttDescr", "pcode", "pcodeDescr", "years", "units" };
            string srch = SearchTextBox.Text;
            string[] search = SearchTextBox.Text.Split(' ');
            string intervals = "";

            if (CheckIntervals.Checked == true)
            {
                intervals = "instant daily monthly";
            }
            else
            {
                if (db ==  TimeInterval.Irregular)
                {
                    intervals = "instant";
                }
                else if (db ==  TimeInterval.Daily)
                {
                    intervals = "daily";
                }
                else if (db ==  TimeInterval.Monthly)
                {
                    intervals = "monthly";
                }
            }

            string[] inter = intervals.Split(' ');
            for (int k = 0; k < inter.Length; k++)
            {
                for (int i = 1; i < col.Length; i++)
                {
                    RowSearchResults = tblSite.Select("DataType = '" + inter[k] + "' AND " + col[i] + " LIKE '%" + srch + "%'");

                    foreach (DataRow temp in RowSearchResults)
                    {
                        Results.ImportRow(temp);
                    }
                }

                for (int i = 1; i < col.Length; i++)
                {
                    for (int j = 0; j < search.Length; j++)
                    {
                        RowSearchResults = tblSite.Select("DataType = '" + inter[k] + "' AND " + col[i] + " LIKE '%" + search[j] + "%'");

                        foreach (DataRow temp in RowSearchResults)
                        {
                            Results.ImportRow(temp);
                        }
                    }
                }
            }

            // if at least two enteries returned from search the results are important
            for (int i = 0; i < Results.Rows.Count; i++)
            {
                RowSearchResults = Results.Select("DataType = '" + Results.Rows[i]["DataType"].ToString() +
                    "' AND cbtt = '" + Results.Rows[i]["cbtt"].ToString() +
                    "' AND pcode = '" + Results.Rows[i]["pcode"].ToString() + "'");
                if (RowSearchResults.Count() >= search.Length)
                {
                    RefinedResults.ImportRow(RowSearchResults[0]);
                }
            }

            RefinedResults = RefinedResults.DefaultView.ToTable(true, col);
            DataResultsTable.Columns.Clear();
            DataResultsTable.DataSource = RefinedResults;

            //add check box for the user to select needed sites
            DataResultsTable = LookClass.AddCheckBox(DataResultsTable, RefinedResults);
            DataResultsTable.AutoResizeColumns();
            DataResultsTable.AutoResizeRows();
        }

        public string PassSearch
        {
            get { return str; }
        }

        public TimeInterval DataType
        {
            set
            {
                db = value;
            }
        }

        private void SearchStringButton_Click(object sender, EventArgs e)
        {
            str = "";
            foreach (DataGridViewRow row in DataResultsTable.Rows)
            {
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells[DataResultsTable.ColumnCount - 1];
                if (chk.Value == chk.TrueValue)
                {
                    if (DataResultsTable["DataType", chk.RowIndex].Value.ToString() == "instant")
                    {
                        str = str + "i:" + DataResultsTable["cbtt", chk.RowIndex].Value.ToString() + " " +
                        DataResultsTable["pcode", chk.RowIndex].Value.ToString() + ", ";
                    }
                    else if (DataResultsTable["DataType", chk.RowIndex].Value.ToString() == "daily")
                    {
                        str = str + "d:" + DataResultsTable["cbtt", chk.RowIndex].Value.ToString() + " " +
                            DataResultsTable["pcode", chk.RowIndex].Value.ToString() + ", ";
                    }
                    else if (DataResultsTable["DataType", chk.RowIndex].Value.ToString() == "monthly")
                    {
                        str = str + "m:" + DataResultsTable["cbtt", chk.RowIndex].Value.ToString() + " " +
                           DataResultsTable["pcode", chk.RowIndex].Value.ToString() + ", ";
                    }

                }
            }
            if (str.Length >= 1)
            {
                str = str.Remove(str.Length - 2);
                if (db ==  TimeInterval.Irregular)
                {
                    str = str.Replace("i:", "");
                }
                else if (db ==  TimeInterval.Daily)
                {
                    str = str.Replace("d:", "");
                }
                else if (db ==  TimeInterval.Monthly)
                {
                    str = str.Replace("m:", "");
                }
            }

        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchButton.PerformClick();
            }
        }

    }
}