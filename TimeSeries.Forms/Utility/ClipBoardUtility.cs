using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Reclamation.Core
{
    /// <summary>
    /// DataTableClipBoard handles copy/paste operations 
    /// to datatable 
    /// </summary>
    public static class ClipBoardUtility
    {

        public static void CopyToClipboard(DataTable dataTable)
        {
            CopyToClipboard(dataTable, true);
        }
        public static void CopyToClipboard(DataTable dataTable, bool includeColumnNames)
        {
            StringBuilder sb = new StringBuilder();
            int numCols = dataTable.Columns.Count;
            if (includeColumnNames)
            {
                for (int c = 0; c < numCols; c++)
                {
                    sb.Append(dataTable.Columns[c].ColumnName);
                    if (c != numCols - 1)
                        sb.Append("\t");
                }
                sb.Append("\r\n");
            }
            int numRows = dataTable.Rows.Count;


            for (int i = 0; i < numRows; i++)
            {
                for (int c = 0; c < numCols; c++)
                {
                    sb.Append(dataTable.Rows[i][c]);
                    if (!(c == numCols - 1))
                        sb.Append("\t");
                }
                sb.Append("\r\n");
            }
            Clipboard.SetDataObject(sb.ToString(), true);
        }


        public static DataTable GetDataTableFromClipboard()
        {
            DataTable tbl = new DataTable();
            string[][] data = GetTabSeparatedClipBoard();

           // Clipboard.ContainsText(TextDataFormat.CommaSeparatedValue);
            for (int i = 0; i < data.GetLength(0); i++)
            {
               while (data[i].Length > tbl.Columns.Count)
                {
                    tbl.Columns.Add();
                }

                DataRow row = tbl.NewRow();
                tbl.Rows.Add(row);
                for (int j = 0; j < data[i].Length; j++)
                {
                    if (data[i][j] == "")
                        row[j] = DBNull.Value;
                    else
                    row[j] = data[i][j];    
                }
            }

           
            return tbl;
        }


        /// <summary>
        /// Create a list of string arrays (tab separated)
        /// from the clipboard
        /// </summary>
        public static string[][] GetTabSeparatedClipBoard()
        {
            //  throw new NotImplementedException("not implemented");

            List<string[]> rval = new List<string[]>();
            //DataTable tbl = new DataTable();
            string str = "";
            if (Clipboard.ContainsText())
            {
                str = Clipboard.GetText();
            }
            if (str == "")
                return rval.ToArray();
            if (str.Length >1 && str[str.Length - 1] == '\n'
                 && str[str.Length - 2] == '\r')
                str = str.Substring(0, str.Length - 2);
            //str = str.TrimEnd();

            string[] lines = str.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                string[] tokens = lines[i].Split('\t');
                tokens[tokens.Length - 1] = tokens[tokens.Length - 1].Trim();

                rval.Add(tokens);
            }

            return rval.ToArray();

        }
        public static void InsertFromClipboard(DataTable dataTable)
        {
            IDataObject iData = Clipboard.GetDataObject();
            if (iData.GetDataPresent(DataFormats.Text))
            {
                string str = (string)iData.GetData(System.Windows.Forms.DataFormats.Text);
                if (str == "")
                    return;
                str = str.TrimEnd();

                int idx = str.IndexOf("\n");
                if (idx < 0)
                    return;
                //string columnNames = str.Substring(0, idx);
                str = str.Substring(idx + 1).Trim();
                string[] rows = str.Split(new char[] { '\n' });

                // if( rows.Length != this.dataTable.Rows.Count)
                // {
                //  MessageBox.Show("Error:  Data in clipboard has "+rows.Length+" rows \nbut the grid has "+dataTable.Rows.Count+" rows");
                // return;
                // }
                string[] cols = rows[0].Split('\t');
                if (cols.Length != dataTable.Columns.Count)
                {
                    MessageBox.Show("Error:  Data in clipboard has " + cols.Length + " columns \nbut the grid has " + dataTable.Columns.Count + " columns");
                    return;
                }
                for (int i = 0; i < rows.Length; i++)
                {
                    cols = rows[i].Split('\t');
                    if (cols.Length != dataTable.Columns.Count)
                    {
                        MessageBox.Show("Error:  Data in clipboard has " + cols.Length + " columns \nbut the grid has " + dataTable.Columns.Count + " columns");
                        return;
                    }
                    // check date in column 1.. it should not be changed.
                    DateTime date = Convert.ToDateTime(cols[0]);
                    DateTime date2 = (DateTime)dataTable.Rows[i][0];
                    if (date != date2)
                    {
                        MessageBox.Show("Error: Date in clipboard is different from the date here");
                        return;
                    }
                    for (int c = 1; c < cols.Length; c++)
                    { // put data into data grid.
                        if (cols[c].Trim() == "") // test for DB null
                        {
                            dataTable.Rows[i][c] = DBNull.Value;
                        }
                        else
                        {
                            dataTable.Rows[i][c] = Convert.ToDouble(cols[c]);
                        }
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
