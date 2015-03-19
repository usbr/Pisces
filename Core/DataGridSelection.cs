using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Linq;
namespace Reclamation.Core
{
    public class DataGridSelection
    {

        DataGridView dgv;

        
        public DataGridSelection(DataGridView dgv)
        {
            this.dgv = dgv;
            ProcessSelection();
        }




        /// <summary>
        /// Computes count, max,min average, and sum
        /// </summary>
        /// <returns></returns>
        public string ComputeSelectedStats()
        {
            var rval = "";
            DataGridViewCell cell = null;

            var values = new List<double>();

            for (int i = 0; i < dgv.SelectedCells.Count; i++)
            {
                cell = dgv.SelectedCells[i];

                if (dgv.Columns[cell.ColumnIndex].ValueType == typeof(double)
                    ||
                    dgv.Columns[cell.ColumnIndex].ValueType == typeof(decimal)
                    ||
                    dgv.Columns[cell.ColumnIndex].ValueType == typeof(int)
                   )
                {
                     var o = dgv[cell.ColumnIndex, cell.RowIndex].Value;
                     if (o != DBNull.Value)
                     {
                         var d = Convert.ToDouble(o);
                         values.Add(d);
                     }
                }
            }

            if (values.Count > 0)
            {
                rval = "Count: " + values.Count +
                    " Average: " + values.ToArray().Average().ToString("F2") +
                    " Min: " + values.ToArray().Min().ToString("F2") +
                    " Max: " + values.ToArray().Max().ToString("F2") +
                    " Sum: " + values.ToArray().Sum().ToString("F2");
            }

            return rval;
        }

        /// <summary>
        /// Allows interpolation withing a single column of a DataGridView
        /// Required: first and last cell must not be null
        /// Required: single continous selection within a column
        /// Required: at lest three cells selected
        /// </summary>
        public void Interpolate()
        {
            if (m_isValidInterpolationSelection)
            {
                double d1 = Convert.ToDouble(dgv[columnIndex, firstRowIndex].Value);
                double d2 = Convert.ToDouble(dgv[columnIndex, lastRowIndex].Value);

                double increment = (d2 - d1) / (lastRowIndex - firstRowIndex);

                double v = d1;
                for (int rowIndex = firstRowIndex+1; rowIndex < lastRowIndex; rowIndex++)
                {
                    v += increment;
                    dgv[columnIndex, rowIndex].Value = v;
                }
            }
        }

        public void InterpolateAndFlag(string flag)
        {
            if (m_isValidInterpolationSelection)
            {
                double d1 = Convert.ToDouble(dgv[columnIndex, firstRowIndex].Value);
                double d2 = Convert.ToDouble(dgv[columnIndex, lastRowIndex].Value);

                double increment = (d2 - d1) / (lastRowIndex - firstRowIndex);

                double v = d1;
                for (int rowIndex = firstRowIndex + 1; rowIndex < lastRowIndex; rowIndex++)
                {
                    v += increment;
                    dgv[columnIndex, rowIndex].Value = v;
                    dgv[columnIndex + 1, rowIndex].Value = flag;
                }
            }
        }

        public void SetText(string flag)
        {
            for (int rowIndex = firstRowIndex ; rowIndex <= lastRowIndex; rowIndex++)
            {
                dgv[columnIndex, rowIndex].Value = flag;
            }
        }

        public bool ValidInterpolationSelection
        {
            get
            {
                return m_isValidInterpolationSelection;
            }
        }


        bool m_isValidInterpolationSelection = false;
        bool m_isValidTextRange = false;

        bool m_validPasteSelection = true; // nothing allowd in date column, single column only , continuous only.

        public bool IsValidTextRange
        {
            get { return m_isValidTextRange; }
            set { m_isValidTextRange = value; }
        }

        int columnIndex = -1;
        int firstRowIndex = -1;
        int lastRowIndex = -1;


        private void ProcessSelection()
        {
            m_isValidInterpolationSelection = false;
            m_isValidTextRange = false;

            columnIndex = -1;
            firstRowIndex = -1;
            lastRowIndex = -1;

            DataGridViewCell cell = null;
            if (dgv.SelectedCells.Count > 0)
            {
                cell = dgv.SelectedCells[0];
                columnIndex = cell.ColumnIndex;
                firstRowIndex = cell.RowIndex;
                lastRowIndex = cell.RowIndex;
            }

            for( int i =0; i< dgv.SelectedCells.Count; i++)
            {
                cell = dgv.SelectedCells[i];
                if (cell.ColumnIndex == 0)
                { // we cant interpolate or set flags in date column
                    m_validPasteSelection = false;
                    break;
                }
                if (cell.ColumnIndex != columnIndex)
                {
                    break; // only one column allowed for interpolation
                }

                lastRowIndex = Math.Max(cell.RowIndex,lastRowIndex);
                firstRowIndex = Math.Min(cell.RowIndex,firstRowIndex);
            }

            if (lastRowIndex - firstRowIndex + 1 != dgv.SelectedCells.Count)
            {
                m_validPasteSelection = false;
                return; // discontinous selection not allowed
            }

            CheckIfValidForInterpolation();


            if (dgv.Columns[columnIndex].ValueType == typeof(string))
            {
                m_isValidTextRange = true;
            }
            
            m_validPasteSelection = (lastRowIndex >=0 && firstRowIndex >=0);

        }

        private void CheckIfValidForInterpolation()
        {
            if (m_validPasteSelection
                && dgv[columnIndex, firstRowIndex].Value != DBNull.Value
               && dgv[columnIndex, lastRowIndex].Value != DBNull.Value
               &&
                  (dgv.Columns[columnIndex].ValueType == typeof(double)
                   ||
                    dgv.Columns[columnIndex].ValueType == typeof(decimal)
                   )

                && dgv.SelectedCells.Count >= 3 )
            {
                m_isValidInterpolationSelection = true;
            }
        }

        public bool IsValidDataRange()
        {
            bool rval = true;

            for (int i = 0; i < dgv.SelectedCells.Count; i++)
            {
                var cell = dgv.SelectedCells[i];
                if (dgv.Columns[cell.ColumnIndex].ValueType != typeof(double) 
                    &&
                    dgv.Columns[cell.ColumnIndex].ValueType != typeof(decimal) 
                    &&
                    dgv.Columns[cell.ColumnIndex].ValueType != typeof(int))
                {
                    rval = false;
                    break;
                }
            }
        
            return rval;
        }

        /// <summary>
        /// Paste contents of a DataTable into DataGridView selection
        /// if DataTable is a single cell,  replicate that cell into selection
        /// </summary>
        /// <param name="dataTable"></param>
        public void Paste(DataTable tbl)
        {
            if (!m_validPasteSelection)
                return;

            var autoSizeMode = dgv.AutoSizeColumnsMode;
            // turn off autosize mode.. this really helps on performance
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            if (tbl.Rows.Count == 0)
                return; // nothing to paste.
            if (dgv.SelectedCells.Count == 0)
                return; // nothing selected.

            DataGridViewCell cell = dgv.CurrentCell;

            if (cell == null)
                return;

            int maxColumnIndex = cell.ColumnIndex + tbl.Columns.Count - 1;

            if (maxColumnIndex >= dgv.Columns.Count)
                throw new InvalidOperationException("There are not enough columns to paste this data");

            

            if (dgv.SelectedCells.Count == 1)   // single cell is selected
            {
                int maxRowIndex = cell.RowIndex + tbl.Rows.Count - 1;
                if (maxRowIndex >= dgv.Rows.Count)
                    throw new InvalidOperationException("There are not enough rows to paste this data");



                int row = cell.RowIndex;
                int column = cell.ColumnIndex;
                for (int r = 0; r < tbl.Rows.Count; r++)
                {
                    for (int c = 0; c < tbl.Columns.Count; c++)
                    {
                        dgv[c + column, r + row].Value = tbl.Rows[r][c];
                    }
                }
            }
            else
            {    // selection is larger than clipboard (repeat data)
                int tableRow = 0; 
                int tableColumn = 0;
                for (int r = firstRowIndex; r <= lastRowIndex; r++)
                {
                    for (int c = columnIndex; c <= maxColumnIndex; c++)
                    {
                        dgv[c, r ].Value = tbl.Rows[tableRow][tableColumn];
                        tableColumn++;
                        if (tableColumn >= tbl.Columns.Count)
                            tableColumn=0;
                    }
                    tableRow++;
                    if (tableRow >= tbl.Rows.Count)
                        tableRow = 0;
                }
            }

            dgv.AutoSizeColumnsMode = autoSizeMode;
        }

        /// <summary>
        /// Paste contents of a DataTable into DataGridView selection
        /// if DataTable is a single cell,  replicate that cell into selection
        /// insert primary key in first (hidden?) column 
        /// </summary>
        /// <param name="dataTable"></param>
        public void PasteWithPrimaryKey(DataTable tbl, int primaryKey)
        {
            if (!m_validPasteSelection)
                return;

            var autoSizeMode = dgv.AutoSizeColumnsMode;
            
            // turn off autosize mode.. this really helps on performance
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            if (tbl.Rows.Count == 0)
                return; // nothing to paste.
            if (dgv.SelectedCells.Count == 0)
                return; // nothing selected.

            DataGridViewCell cell = dgv.CurrentCell;

            if (cell == null)
                return;

            int irow = cell.RowIndex;
            int icol = cell.ColumnIndex;

            int maxColumnIndex = icol + tbl.Columns.Count - 1;

            if (maxColumnIndex >= dgv.Columns.Count)
                throw new InvalidOperationException("There are not enough columns to paste this data");

            var sourceTable = (DataTable)dgv.DataSource;
            // rows that will get overwritten by paste

            int overwriteRows = Math.Max(sourceTable.Rows.Count - irow,0);

            int rowsToAdd = Math.Max(tbl.Rows.Count - overwriteRows, 0);

            dgv.DataSource = null;

            for (int i = 0; i < rowsToAdd; i++)
            {

                var newRow = sourceTable.NewRow();
                newRow[0] = primaryKey;
                sourceTable.Rows.Add(newRow);
            }

            // paste data
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                for (int c = 0; c < tbl.Columns.Count; c++)
                {
                    sourceTable.Rows[irow + i][c + icol] = tbl.Rows[i][c]; 
                }    
            }


            dgv.DataSource = sourceTable;
          //  dgv.CurrentCell = dgv.Rows[irow].Cells[icol];


            //if (dgv.SelectedCells.Count == 1)   // single cell is selected
            //{
            //    int maxRowIndex = cell.RowIndex + tbl.Rows.Count - 1;
            //   // if (maxRowIndex >= dgv.Rows.Count && !autoAddRows)
            //     //   throw new InvalidOperationException("There are not enough rows to paste this data");


            //    int row = cell.RowIndex;
            //    int column = cell.ColumnIndex;
            //    for (int r = 0; r < tbl.Rows.Count; r++)
            //    {
            //        for (int c = 0; c < tbl.Columns.Count; c++)
            //        {
            //            dgv[c + column, r + row].Value = tbl.Rows[r][c];
            //        }
            //    }
            //}
            //else
            //{    // selection is larger than clipboard (repeat data)
            //    int tableRow = 0;
            //    int tableColumn = 0;
            //    for (int r = firstRowIndex; r <= lastRowIndex; r++)
            //    {
            //        for (int c = columnIndex; c <= maxColumnIndex; c++)
            //        {
            //            dgv[c, r].Value = tbl.Rows[tableRow][tableColumn];
            //            tableColumn++;
            //            if (tableColumn >= tbl.Columns.Count)
            //                tableColumn = 0;
            //        }
            //        tableRow++;
            //        if (tableRow >= tbl.Rows.Count)
            //            tableRow = 0;
            //    }
            //}

            dgv.AutoSizeColumnsMode = autoSizeMode;
        }



        private DateTime GetSelectedDate(int rowIndex)
        {
            return Convert.ToDateTime(dgv[0, rowIndex].Value);
        }
        public DateTime T1 { get { return GetSelectedDate(firstRowIndex); } }

        public DateTime T2 { get { return GetSelectedDate(lastRowIndex); } }
    }
}
