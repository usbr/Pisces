using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SpreadsheetGear;
using SpreadsheetGear.Windows.Forms;
using SpreadsheetGear.Advanced.Cells;
using Reclamation.TimeSeries.Excel;

namespace Reclamation.AgriMet.UI
{
    public partial class SpreadsheetControl : UserControl
    {
        public SpreadsheetControl()
        {
            InitializeComponent();

            LoadWorkbookView();
            SetupContextMenus();
            

        }

        ToolStripItem insertRowMenu;
        ToolStripItem deleteRowMenu;

        private void SetupContextMenus()
        {
            var removeList = new List<ToolStripItem>();
            string[] keepers = { "&Copy", "&Paste" };
            foreach (ToolStripItem item in wbView.ContextMenuStrip.Items)
            {
                if (Array.IndexOf(keepers, item.Text) < 0)
                {
                    removeList.Add(item);
                }

            }
            foreach (ToolStripItem item in removeList)
            {
                wbView.ContextMenuStrip.Items.Remove(item);
            }

            wbView.ContextMenuStrip.Items.Add(new ToolStripSeparator());

            insertRowMenu = wbView.ContextMenuStrip.Items.Add("Insert Row");
            insertRowMenu.Click += new EventHandler(insertRowMenu_Click);

            deleteRowMenu = wbView.ContextMenuStrip.Items.Add("Delete Row");
            deleteRowMenu.Click += new EventHandler(deleteRowMenu_Click);

        }

        void deleteRowMenu_Click(object sender, EventArgs e)
        {
            // insert row in datatable, then reload the view.
            if (m_dataTable == null || m_dataTable.Rows.Count == 0 || wbView.RangeSelection.Row <= 0)
                return;
            m_suspendUpdates = true;
            wbView.GetLock();
            try
            {
                int sz = wbView.RangeSelection.RowCount;

                for (int i = 0; i < sz; i++)
                {
                    var row = m_dataTable.Rows[wbView.RangeSelection.Row - 1 + i];
                    row.Delete();
                }

                wbView.RangeSelection.EntireRow.Delete();
            }
            finally
            {
                wbView.ReleaseLock();
                m_suspendUpdates = false;
            }
            
        }

        void insertRowMenu_Click(object sender, EventArgs e)
        {
            // insert row in datatable, then reload the view.
            if (m_dataTable == null || m_dataTable.Rows.Count == 0 || wbView.RangeSelection.Row<=0)
                return;

            m_suspendUpdates = true;
            wbView.GetLock();
            try
            {
                int sz = wbView.RangeSelection.RowCount;

                for (int i = 0; i < sz; i++)
                {
                    var idx = wbView.RangeSelection.Row - 1;
                    var row = m_dataTable.NewRow();
                    m_dataTable.Rows.InsertAt(row, idx);
                }
                wbView.RangeSelection.EntireRow.Insert();

            }
            finally
            {
                wbView.ReleaseLock();
                m_suspendUpdates = false;
            }
            
        }

        IWorkbook workbook;
        WorkbookView wbView;
        IWorksheet worksheet;
        private void LoadWorkbookView()
        {
            //workbook = SpreadsheetGear.Factory.GetWorkbook(this.tmpXlsName);
            workbook = SpreadsheetGear.Factory.GetWorkbook();
            worksheet = workbook.ActiveWorksheet;

            wbView = new SpreadsheetGear.Windows.Forms.WorkbookView(workbook);
            wbView.GetLock();
            try
            {
                SpreadsheetGear.IWorkbookWindowInfo windowInfo = wbView.ActiveWorkbookWindowInfo;
                windowInfo.DisplayWorkbookTabs = false;
                worksheet.WindowInfo.Zoom = 100;
                wbView.Parent = this;
                wbView.BringToFront();
                wbView.Dock = DockStyle.Fill;
                wbView.CellBeginEdit += new CellBeginEditEventHandler(wbView_CellBeginEdit);
                wbView.RangeChanged += new RangeChangedEventHandler(wbView_RangeChanged);
            }
            finally
            {
                wbView.ReleaseLock();
            }
        }



        void wbView_RangeChanged(object sender, RangeChangedEventArgs e)
        {
            if (m_suspendUpdates)
                return;
            // update datatable

            var rng = e.Range;

            UpdateSourceDataTable(rng);

        }

        /// <summary>
        /// Update source DataTable 
        /// </summary>
        /// <param name="rng"></param>
        private void UpdateSourceDataTable(IRange rng)
        {

            m_suspendUpdates = true;

            try
            {

                IValues values = (IValues)worksheet;

                var errorMessages = new List<string>();
                int maxRowIndex = rng.Row + rng.RowCount;
                int maxColumnIndex = rng.Column + rng.ColumnCount;
                for (int r = rng.Row; r < maxRowIndex; r++)
                {
                    for (int c = rng.Column; c < maxColumnIndex; c++)
                    {
                        // source DataTable is offset one row from spreadsheet because of header row
                        // only update where spreasheet and DataTable overlap
                        if (r <= m_dataTable.Rows.Count
                            && c < m_dataTable.Columns.Count
                            && r > 0 // don't change column header
                            && c >= 0 // don't allow date column
                            )
                        {
                            // TO DO.. append rows to datatable?
                            // TO DO.. allow dates?
                            IValue val = values[r, c];
                            if (val == null)
                            {
                                m_dataTable.Rows[r - 1][c] = DBNull.Value;
                            }
                            else
                                if (m_dataTable.Columns[c].DataType == typeof(double))
                                {
                                    double d = Reclamation.TimeSeries.Point.MissingValueFlag;
                                    if (SpreadsheetGearExcel.TryReadingValue(val, out d))
                                    {
                                        m_dataTable.Rows[r - 1][c] = d;
                                    }
                                    else
                                    {
                                        errorMessages.Add("Invalid number on row " + r + " " + val.ToString());
                                    }
                                }
                                else
                                    if (m_dataTable.Columns[c].DataType == typeof(int))
                                    {
                                        int i = 0;
                                        if (SpreadsheetGearExcel.TryReadingValue(val, out i))
                                        {
                                            m_dataTable.Rows[r - 1][c] = i;
                                        }
                                        else
                                        {
                                            errorMessages.Add("Invalid number on row " + r + " " + val.ToString());
                                        }
                                    }
                                else
                                    if (m_dataTable.Columns[c].DataType == typeof(string))
                                    {
                                        string txt = val.Text;
                                        if (txt != null)
                                            m_dataTable.Rows[r - 1][c] = txt;
                                        else
                                        {
                                            if (val.Type == SpreadsheetGear.Advanced.Cells.ValueType.Number)
                                                m_dataTable.Rows[r - 1][c] = val.Number.ToString();
                                            else
                                                m_dataTable.Rows[r - 1][c] = DBNull.Value;
                                        }
                                    }
                                    else if (m_dataTable.Columns[c].DataType == typeof(DateTime))
                                    {

                                        DateTime t;
                                        if (SpreadsheetGearExcel.TryReadingDate(workbook, val, out t))
                                        {
                                            m_dataTable.Rows[r - 1][c] = t;
                                        }
                                        else
                                        {
                                            m_dataTable.Rows[r - 1][c] = DBNull.Value;
                                        }
                                    }

                        }
                    }
                }

                if (errorMessages.Count > 0)
                {
                    MessageBox.Show(String.Join("\n", errorMessages.ToArray()));
                }
            }
            finally
            {
                m_suspendUpdates = false;
            }

            OnUpdateCompleted(EventArgs.Empty);

        }

        public event EventHandler<EventArgs> UpdateCompleted;

        protected void OnUpdateCompleted(EventArgs e)
        {
            EventHandler<EventArgs> handler = UpdateCompleted;
            if (handler != null)
                handler(this, e);
        }

        void wbView_CellBeginEdit(object sender, CellBeginEditEventArgs e)
        {
            var rng = wbView.ActiveCell;
            // don't allow edits on dateTime column, or header row.
            //if (rng.Column == 0 || rng.Row == 0)
            //{
            //    e.Cancel = true;
            //}
        }


        private bool m_suspendUpdates = false;

        public bool SuspendUpdates
        {
            get { return m_suspendUpdates; }
            // set { m_suspendUpdates = value; }
        }
        DataTable m_dataTable;

        /// <summary>
        /// Sets source data and type of data.
        /// </summary>
        /// <param name="tbl"></param>
        /// <param name="db"></param>
         public void SetDataTable(DataTable tbl)
        {
            m_dataTable = tbl;
            LoadWorkbookView();
            SetupContextMenus();
            wbView.GetLock();
            try
            {
                m_suspendUpdates = true;
                worksheet.UsedRange.Clear();
                worksheet.WindowInfo.ScrollColumn = 0;
                worksheet.WindowInfo.ScrollRow = 0;
                worksheet.WindowInfo.FreezePanes = true;


                var range = worksheet.Cells["A1"];
                range.EntireColumn.ColumnWidth = 1;
                range.Select();
                range.CopyFromDataTable(tbl, SpreadsheetGear.Data.SetDataFlags.None);
                worksheet.UsedRange.Columns.AutoFit();
               // range.EntireColumn.ColumnWidth = 0;
                
            }
            finally
            {
                wbView.ReleaseLock();
                m_suspendUpdates = false;
            }
        }

       

    }
}
