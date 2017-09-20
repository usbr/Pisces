using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;

namespace Reclamation.Core
{
    public class DataGridViewUtility
    {
        private DataGridView m_dgv;
        public DataGridViewUtility (DataGridView dgv)
	{
            m_dgv = dgv;
	}

        bool IsGridEmpty()
        {
            return m_dgv.Rows.Count == 0
                && m_dgv.Columns.Count == 0;
        }


        public void PasteFromClipboard( )
        {
           DataTable tbl  = ClipBoardUtility.GetDataTableFromClipboard();
           var autoSizeMode = m_dgv.AutoSizeColumnsMode; 
            // turn off autosize mode.. this really helps on performance
           m_dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            if( tbl.Rows.Count ==0)
                return;
            if (IsGridEmpty())
            {
                for (int i = 0; i < tbl.Columns.Count; i++)
                {
                    m_dgv.Columns.Add("", "");
                }
                m_dgv.Rows.Add(tbl.Rows.Count);
            }
               
          DataGridViewCell cell = m_dgv.CurrentCell;
            
          if (cell == null)
              return;

          int maxColumnIndex = cell.ColumnIndex + tbl.Columns.Count - 1;

          if (maxColumnIndex >= m_dgv.Columns.Count)
              throw new InvalidOperationException("There are not enough columns to paste this data");

          int maxRowIndex = cell.RowIndex + tbl.Rows.Count - 1;

            if( maxRowIndex >= m_dgv.Rows.Count )
                throw new InvalidOperationException("There are not enough rows to paste this data");

            if (cell == null)
                return;
            int row = cell.RowIndex;
            int column = cell.ColumnIndex;


            for (int r = 0; r < tbl.Rows.Count; r++)
            {
                for (int c = 0; c <tbl.Columns.Count; c++)
                {
                    //Console.WriteLine("before:" + m_dgv[c + column, r + row].Value);
                    //Console.Write("[" + r + "][" + c + "]'" + data[r][c] + "' ");
                    m_dgv[c + column, r + row].Value
                        = tbl.Rows[r][c];
                }
                //Console.WriteLine();
            }

            m_dgv.AutoSizeColumnsMode = autoSizeMode;

        }
    }
}
