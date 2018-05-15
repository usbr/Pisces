#if SpreadsheetGear
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using SpreadsheetGear;
using SpreadsheetGear.Windows.Forms;
using Reclamation.Core;
using SpreadsheetGear.Advanced.Cells;
using Reclamation.TimeSeries.Excel;
using System.IO;
using System.Diagnostics;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries.Forms;
using Reclamation.TimeSeries;
using System.Configuration;
using Reclamation.TimeSeries.Parser;
using Reclamation.TimeSeries.Decodes;

namespace HydrometTools
{
    /// <summary>
    /// A spreadsheet with DateTime in the first column
    /// used for time-series data. 
    /// </summary>
    public partial class TimeSeriesSpreadsheetSG : UserControl, HydrometTools.ITimeSeriesSpreadsheet
    {
        TimeInterval interval;
        public TimeSeriesSpreadsheetSG()
        {
            InitializeComponent();
            m_dataTable = new DataTable();
            if (!DesignMode)
            {
                LoadWorkbookView();
            }
            SetupContextMenu();

        }

        ToolStripItem cutMenu;
        ToolStripItem pasteMenu;
        ToolStripItem interpolateMenu;
        ToolStripItem calculateMenu;
        ToolStripItem showEquationMenu;
        ToolStripItem interpolateWithStyleMenu;
        ToolStripItem advancedRawData;
        ToolStripItem ScaleToVolumeMenu;
        ToolStripMenuItem flagMenu;
        ToolStripMenuItem ExcelMenu;
        ToolStripMenuItem SpreadsheetMenu;
        ToolStripItem regressionMenu;
        

        private void SetupContextMenu()
        {

            var removeList = new List<ToolStripItem>();
            string[] keepers = { "Cu&t", "&Copy", /*"&Paste",*/ "Paste &Special..." };
            foreach (ToolStripItem item in wbView.ContextMenuStrip.Items)
            {
                if (Array.IndexOf(keepers, item.Text) < 0)
                {
                    removeList.Add(item);
                }
                if (item.Text == "Cu&t")
                    cutMenu = item;
                
            }
            foreach (ToolStripItem item in removeList)
            {
                wbView.ContextMenuStrip.Items.Remove(item);
            }

            wbView.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            
            interpolateMenu = wbView.ContextMenuStrip.Items.Add("Interpolate");
            interpolateMenu.Click += new EventHandler(interpolateMenu_Click);

            pasteMenu = wbView.ContextMenuStrip.Items.Add("&Paste");
           pasteMenu.Click += new EventHandler(pasteMenu_Click);


            calculateMenu = wbView.ContextMenuStrip.Items.Add("Calculate");
            calculateMenu.Click += new EventHandler(calculateMenu_Click);

            showEquationMenu = wbView.ContextMenuStrip.Items.Add("Show Equation");
            showEquationMenu.Click += showEquationMenu_Click;


            
            wbView.ContextMenuStrip.Items.Add(new ToolStripSeparator());


            regressionMenu = wbView.ContextMenuStrip.Items.Add("Regression");
            regressionMenu.ToolTipText = "performs regression with multiple columns";
            regressionMenu.Click += regressionMenu_Click;



            ScaleToVolumeMenu = new ToolStripMenuItem("Scale to Volume");
            ScaleToVolumeMenu.Click += new EventHandler(ScaleToVolumeMenu_Click);
            wbView.ContextMenuStrip.Items.Add(ScaleToVolumeMenu);


            SpreadsheetMenu = new ToolStripMenuItem("Full View Spreadsheet");
            SpreadsheetMenu.Click += new EventHandler(SpreadsheetMenu_Click);
            wbView.ContextMenuStrip.Items.Add(SpreadsheetMenu);
            
            ExcelMenu = new ToolStripMenuItem("Open in Excel");
            ExcelMenu.Click += new EventHandler(ExcelMenu_Click);
            wbView.ContextMenuStrip.Items.Add(ExcelMenu);


            var advanced= new ToolStripMenuItem("Advanced");
            wbView.ContextMenuStrip.Items.Add(advanced);
            interpolateWithStyleMenu = advanced.DropDownItems.Add("Interpolate with Style!");
            interpolateWithStyleMenu.ToolTipText = "Select two equal sized vertical ranges (one with a gap, one without)";
            interpolateWithStyleMenu.Click += new EventHandler(interpolateWithStyleMenu_Click);


            var fillGaps = new ToolStripMenuItem("Interpolate between all gaps");
            fillGaps.Click += FillGaps_Click;
            advanced.DropDownItems.Add(fillGaps);

            advancedRawData = new ToolStripMenuItem("decode raw data");
            advancedRawData.Click += AdvancedRawData_Click;
            advanced.DropDownItems.Add(advancedRawData);


            wbView.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            wbView.ContextMenuStrip.ShowItemToolTips = true;
        }

        /// <summary>
        /// If Decodes software is installed locally
        /// DECODE raw data for a single parameter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdvancedRawData_Click(object sender, EventArgs e)
        {
            // determine siteid (cbtt) and pcode

            SpreadsheetRange r = new SpreadsheetRange(wbView.RangeSelection);
            var col = r.SelectedRangeColumnNames[0];

                var tokens = col.Trim().Split(' ');
            if (tokens.Length != 2)
                return; 

                var cbtt = tokens[0];
                var pcode = tokens[1];

            // find date range that is selected.
            var t = r.SelectedDateRange;

            var db = Database.DB();
            if (db == null)
            {
                MessageBox.Show("Error connecting to the database.  Please check your password");
                return;
            }

            var fn = FileUtility.GetSimpleTempFileName(".txt");

            // run DECODES to create output file
            DecodesUtility.RunDecodesRoutingSpec((PostgreSQL)db.Server,
                "hydromet-tools", t.DateTime1, t.DateTime2, cbtt, fn);

            TextFile tf = new TextFile(fn);
            if( !HydrometInstantSeries.IsValidDMS3(tf) )
            {
                MessageBox.Show("Error reading Decodes output");
                return;
            }
            // Read Decodes output 
            var sl = HydrometInstantSeries.HydrometDMS3DataToSeriesList(tf);
            // filter by cbtt and pcode

            // filter by date range

            // put values into hydromet tools

        }

        private void FillGaps_Click(object sender, EventArgs e)
        {
            try
            {
                m_suspendUpdates = true;
                SpreadsheetRange gaps = new SpreadsheetRange(wbView.RangeSelection);
                //string flag = "";
                //if (interval == TimeInterval.Irregular)
                //    flag = "e";
                gaps.FillGaps();
            }
            finally
            {
                m_suspendUpdates = false;
            }
            OnUpdateCompleted(EventArgs.Empty);
        }

        void regressionMenu_Click(object sender, EventArgs e)
        {
           
        }

        

        void pasteMenu_Click(object sender, EventArgs e)
        {

            try
            {
                wbView.GetLock();
                m_suspendUpdates = true;

                wbView.Paste();
            }
            finally
            {
                wbView.ReleaseLock();
                m_suspendUpdates = false;
            }
            OnUpdateCompleted(EventArgs.Empty);

        }

        void showEquationMenu_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            string msg = "";
            try
            {
                var db = Database.DB();
                if (db == null)
                {
                    MessageBox.Show("Error connecting to the database.  Please check your password");
                    return;
                }

                SpreadsheetRange ssRng = new SpreadsheetRange(wbView.RangeSelection);
                var colNames = ssRng.SelectedRangeColumnNames;

                
                for (int c = 0; c < colNames.Length; c++)
                {
                    var tokens = colNames[c].Trim().Split(' ');

                    if (tokens.Length != 2)
                        continue;

                    var cbtt = tokens[0];
                    var pcode = tokens[1];

                    var s = db.GetCalculationSeries(cbtt, pcode, interval);

                        if (s != null)
                        {
                            msg += cbtt+"_"+pcode+" = " +s.Expression + "\n";
                        }
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            if( msg != "")
                MessageBox.Show(msg);
        }

        

        void calculateMenu_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            
            try
            {
                var db = Database.DB();
                if (db == null)
                {
                    MessageBox.Show("Error connecting to the database.  Please check your password");
                    return;
                }


                SpreadsheetRange ssRng = new SpreadsheetRange(wbView.RangeSelection);
                var colNames = ssRng.SelectedRangeColumnNames;

                for (int c = 0; c < colNames.Length; c++)
                {
                    var tokens = colNames[c].Trim().Split(' ');

                    if (tokens.Length != 2)
                        continue;

                    var cbtt = tokens[0];
                    var pcode = tokens[1];

                    if (interval == TimeInterval.Monthly)
                    {
                        MonthlyCalculation(ssRng, cbtt, pcode);

                    }
                    else if (interval == TimeInterval.Daily)
                    {
                        DailyCalculation(ssRng, cbtt, pcode, c);
                    }
                    else if( interval == TimeInterval.Irregular)
                    {
                        InstantCalculation(ssRng, cbtt, pcode, c);
                    }
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void InstantCalculation(SpreadsheetRange ssRng, string cbtt, string pcode, int columnOffset)
        {
            var db = Database.DB();

            var series = db.GetCalculationSeries(cbtt, pcode, TimeInterval.Irregular);
            if (series == null)
                return;

            Reclamation.TimeSeries.Parser.SeriesExpressionParser.Debug = true;
            var rng = ssRng.SelectedDateRange;
            series.Calculate(rng.DateTime1, rng.DateTime2);
            if (ssRng.RowCount != series.Count)
            {
                MessageBox.Show(series.Messages.ToString(50), "Error with Calculation");
                return;
            }
            ssRng.InsertSeriesValues(series, "", columnOffset);
   
        }

        private static void DailyCalculation(SpreadsheetRange ssRng, string cbtt, string pcode, int columnOffset)
        {
            var db = Database.DB();
            
            var series = db.GetCalculationSeries(cbtt, pcode, TimeInterval.Daily);
            if (series == null)
                return;

            Reclamation.TimeSeries.Parser.SeriesExpressionParser.Debug = true;
            var rng = ssRng.SelectedDateRange;
            series.Calculate(rng.DateTime1, rng.DateTime2);
            if (ssRng.RowCount != series.Count)
            {
                MessageBox.Show(series.Messages.ToString(50),"Error with Calculation");
                return;  
            }
            ssRng.InsertSeriesValues(series,"",columnOffset);


        }

        private static void MonthlyCalculation(SpreadsheetRange ssRng, string cbtt, string pcode)
        {
            var  db = Database.DB();

            CalculationSeries series = db.GetCalculationSeries(cbtt, pcode, TimeInterval.Monthly);

            if (series == null)
                return;

            var rng = ssRng.SelectedDateRange;
            series.Calculate(rng.DateTime1.FirstOfMonth(), rng.DateTime2.EndOfMonth());

            series.Name = "new";
            var old = ssRng.SelectionToMonthlySeries(false);
            old.Name = "old";

            var diff = series - old;
            diff.Name = "Difference";

            var list = new SeriesList();

            list.Add(series);
            list.Add(old);
            list.Add(diff);

            var dlg = new MonthlyCalculationPreview();
            dlg.DataSource = list.ToDataTable(true);


            if (dlg.ShowDialog() == DialogResult.OK)
            {
                // insert into range..
                ssRng.InsertSeriesValues(series, dlg.SelectedFlag);
            }
        }

        void interpolateWithStyleMenu_Click(object sender, EventArgs e)
        {

            try
            {
                m_suspendUpdates = true;
                SpreadsheetRange ssRng = new SpreadsheetRange(wbView.RangeSelection);

                ssRng.InterpolateWithStyle();
            }
            finally
            {
                m_suspendUpdates = false;
            }
            OnUpdateCompleted(EventArgs.Empty);
        }

        void ScaleToVolumeMenu_Click(object sender, EventArgs e)
        {
            var f = new InputScaleToVolume();
            if (f.ShowDialog() == DialogResult.OK)
            {
                SpreadsheetRange ssRng = new SpreadsheetRange(wbView.RangeSelection);
                double val = ssRng.Sum() * 1.98347; // assume flow in cfs
                if (val > 0)
                {
                    ssRng.ScaleSelectedRange(f.Value / val);
                }
            }
        }

        void SpreadsheetMenu_Click(object sender, EventArgs e)
        {
            WorkbookDesigner d = new WorkbookDesigner(this.workbook.WorkbookSet);
            d.Show();
        }

        void ExcelMenu_Click(object sender, EventArgs e)
        {
            if (m_dataTable == null || m_dataTable.Rows.Count <= 0)
                return;
            string csvFile = Path.ChangeExtension(Path.GetTempFileName(), ".csv");
            CsvFile.WriteToCSV(m_dataTable, csvFile, false);
            ProcessStartInfo psi = new ProcessStartInfo(csvFile);
            Process p = Process.Start(psi);
            Process.Start(csvFile);
        }

        

        void interpolateMenu_Click(object sender, EventArgs e)
        {
            try
            {
                m_suspendUpdates = true;
                SpreadsheetRange inter = new SpreadsheetRange(wbView.RangeSelection);
                string flag = "";
                if (interval ==  TimeInterval.Irregular)
                    flag = "e";
                inter.Interpolate(flag);
            }
            finally
            {
                m_suspendUpdates = false;
            }
            OnUpdateCompleted(EventArgs.Empty);
        }


        

        void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuEnabling();

        }

        private void ContextMenuEnabling()
        {
            if (interpolateMenu == null || flagMenu == null)
                return;
            interpolateMenu.Enabled = true;
            ScaleToVolumeMenu.Enabled = true;
            regressionMenu.Enabled = false;
            advancedRawData.Enabled = false;

            flagMenu.Enabled = true;
            interpolateWithStyleMenu.Enabled = false;
            calculateMenu.Enabled = false;

            if (wbView.ActiveCell != null)
            {
                SpreadsheetRange ssRng = new SpreadsheetRange(wbView.RangeSelection);

                calculateMenu.Enabled = ssRng.ValidCalculationRange;
                
                advancedRawData.Enabled = ssRng.ValidCalculationRange && wbView.RangeSelection.ColumnCount == 1;

                if (ssRng.ValidInterpolationWithStyle)
                {
                    interpolateWithStyleMenu.Enabled = true;
                }

                if (!ssRng.ValidInterpolationRange)
                {
                    interpolateMenu.Enabled = false;
                    ScaleToVolumeMenu.Enabled = false;
                }

                if (!ssRng.ValidFlagRange)
                    flagMenu.Enabled = false;

                if (wbView.ActiveCell.Row == 0
                    || wbView.ActiveCell.Column == 0)
                { // date column or header row.
                    interpolateMenu.Enabled = false;
                    flagMenu.Enabled = false;
                    calculateMenu.Enabled = false;
                }

            }
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
        public void  SetDataTable(DataTable tbl, TimeInterval db, bool scrollToTop) { 
            m_dataTable = tbl;

            this.interval = TimeInterval.Irregular;

            if (db == TimeInterval.Monthly)
                this.interval = TimeInterval.Monthly;
            if (db == TimeInterval.Daily)
                this.interval = TimeInterval.Daily;
            if (db == TimeInterval.Irregular)
                this.interval = TimeInterval.Irregular;

            wbView.GetLock();
            try
            {
                m_suspendUpdates = true;
                worksheet.UsedRange.Clear();
                if (scrollToTop)
                {
                    worksheet.WindowInfo.ScrollColumn = 0;
                    worksheet.WindowInfo.ScrollRow = 0;
                }
                worksheet.WindowInfo.FreezePanes = true;

                var range = worksheet.Cells["A1"];
                range.Select();
                
                range.CopyFromDataTable(m_dataTable, SpreadsheetGear.Data.SetDataFlags.None);
                worksheet.UsedRange.Columns.AutoFit();
                SetupFlagContextMenu(db);
                FormatCells(db);

            }
            finally
            {
                wbView.ReleaseLock();
                m_suspendUpdates = false;
            }
        }

       
        private void FormatCells(TimeInterval db)
        {
            if (db == TimeInterval.Irregular)
                {
                    for (int i = 1; i < m_dataTable.Columns.Count; i+=2)
                    {
                        string dataColumn = SpreadsheetGearExcel.ReferenceFromIndex(i);
                        string flagColumn = SpreadsheetGearExcel.ReferenceFromIndex(i + 1);
                        ConditionalFormatting(dataColumn,flagColumn);
                    }
                }
                else
                    if (db == TimeInterval.Monthly)
                    {
                        string dateRange = "A:A";// +m_dataTable.Rows.Count.ToString();
                        worksheet.Range[dateRange].NumberFormat = "mmm yyyy";
                    }
        }


        private void ConditionalFormatting(string dataColumn, string flagColumn)
        {

            int rangeSize = m_dataTable.Rows.Count + 5;
            string strRange = dataColumn + "2:" + dataColumn + rangeSize.ToString();
            IRange range = wbView.ActiveWorksheet.Cells[strRange];
            SpreadsheetGear.IFormatConditions conditions = range.FormatConditions;

            // Delete any existing formats in the collection.
            conditions.Delete();

            string formula = "=OR(C2=\"e\",C2=\"C\")";
            formula = formula.Replace("C2", flagColumn+"2");
            SpreadsheetGear.IFormatCondition condition = conditions.Add(
                SpreadsheetGear.FormatConditionType.Expression,
                SpreadsheetGear.FormatConditionOperator.Between, formula, null);

            condition.Font.Color = System.Drawing.Color.Black;
            condition.Interior.Color = System.Drawing.Color.Chartreuse;

            formula = "=AND(C2<>\"e\",C2<>\"\",C2<>\" \")";
            formula = formula.Replace("C", flagColumn);

            condition = conditions.Add(
             SpreadsheetGear.FormatConditionType.Expression,
             SpreadsheetGear.FormatConditionOperator.Between, formula, null);

            condition.Font.Color = System.Drawing.Color.Black;
            condition.Interior.Color = System.Drawing.Color.Red;

        }
        private void SetupFlagContextMenu(TimeInterval db)
        {

            if (flagMenu != null)
                return;
            else
                flagMenu = new ToolStripMenuItem("Flag");

            wbView.ContextMenuStrip.Items.Add(flagMenu);

            string[] flags = { };
            // setup flags context menu
            if (db == TimeInterval.Irregular)
            {
                flags = new string[] { "Clear", "edited/math (e)", "missing (m)", "low (-)", "high (+)" };
            }
            else if (db == TimeInterval.Monthly)
            {
                flags = HydrometMonthlySeries.Flags;
            }
            foreach (var item in flags)
            {
                ToolStripMenuItem m = new ToolStripMenuItem(item);
                m.Click += new EventHandler(FlagClick);
                flagMenu.DropDownItems.Add(m);
            }

            if (flagMenu.DropDownItems.Count == 0)
                wbView.ContextMenuStrip.Items.Remove(flagMenu);
        }

        void FlagClick(object sender, EventArgs e)
        {
            
                if (sender is ToolStripDropDownItem)
                {
                    string flag = " ";
                    var item = sender as ToolStripDropDownItem;
                    string txt = item.Text;
                    if (interval ==  TimeInterval.Monthly)
                    {
                        flag = txt.Substring(0, 1);
                    }
                    else if (interval ==  TimeInterval.Irregular)
                    {
                        if (txt == "Clear")
                        {
                            flag = " ";
                        }
                        else
                        {
                            int startIndex = txt.IndexOf("(") + 1;
                            flag = txt.Substring(startIndex, 1);
                        }
                    }


                    SpreadsheetRange ssRng = new SpreadsheetRange(wbView.RangeSelection);
                    ssRng.SetFlag(flag);

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
                wbView.Parent = this; ;
                wbView.BringToFront();
                wbView.Dock = DockStyle.Fill;
                wbView.CellBeginEdit += new CellBeginEditEventHandler(wbView_CellBeginEdit);
                wbView.RangeChanged += new RangeChangedEventHandler(wbView_RangeChanged);
                wbView.RangeSelectionChanged += wbView_RangeSelectionChanged;
            }
            finally
            {
                wbView.ReleaseLock();
            }
        }

        void wbView_RangeSelectionChanged(object sender, RangeSelectionChangedEventArgs e)
        {
            var rng = e.RangeSelection;
            this.toolStripStatusLabelStats.Text = "";
            if (rng.ColumnCount == 1)
            {
                SpreadsheetRange r = new SpreadsheetRange(rng);
                double sum, min, max;
                int count;
                r.Stats(out min,out  max,out sum, out count);
                if (count > 0)
                {
                    double avg = sum / count;
                    toolStripStatusLabelStats.Text = "min: " + min.ToString("F2")
                        + " max: " + max.ToString("F2")
                        + " avg: " + avg.ToString("F2")
                        + " sum: " + sum.ToString("F2")
                        + " count: " + count.ToString();
                }
                    

            }
        }

        void wbView_RangeChanged(object sender, RangeChangedEventArgs e)
        {
            //if (m_suspendUpdates)
              //  return;
            // update datatable

            var rng = e.Range;

            UpdateSourceDataTable(rng);

        }

        bool m_AutoFlag = false;

        /// <summary>
        /// Automatically flag edits with 'e' flag
        /// </summary>
        public bool AutoFlagDayFiles
        {
            get { return m_AutoFlag; }
            set { m_AutoFlag = value; }
        }



        /// <summary>
        /// Sets value in spreadsheet, also updates underlying DataTable
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="val"></param>
        public void SetCellValue(int rowIndex, int columnIndex, double val)
        {
            IValues values = (IValues)worksheet;
            m_dataTable.Rows[rowIndex ][columnIndex] = val;
            values.SetNumber(rowIndex+1, columnIndex, val);
            //worksheet.Select();
            
        }

        /// <summary>
        /// Update source DataTable 
        /// </summary>
        /// <param name="rng"></param>
        private void UpdateSourceDataTable(IRange rng)
        {
//            m_suspendUpdates = true;
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
                            && c > 0 // don't allow date column
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
                                        object o = m_dataTable.Rows[r - 1][c];
                                        if (o != DBNull.Value && Convert.ToDouble(o) == d)
                                            continue; // not changed (HACK To fix dragging nub 

                                        m_dataTable.Rows[r - 1][c] = d;
                                        if (AutoFlagDayFiles && interval == TimeInterval.Irregular
                                            && rng.ColumnCount == 1)
                                        {
                                            values.SetText(r, c + 1, "e");
                                            //rng[r, c].Value = "e";
                                            m_dataTable.Rows[r - 1][c + 1] = "e";
                                        }
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
                                        if (txt == null)
                                            m_dataTable.Rows[r - 1][c] = DBNull.Value;
                                        else
                                            m_dataTable.Rows[r - 1][c] = txt;
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
              //  m_suspendUpdates = false;
            }

           // OnUpdateCompleted(EventArgs.Empty);

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
            if (rng.Column == 0 || rng.Row ==0 )
            {
                e.Cancel = true;
            }
        }



        public void Clear()
        {
            wbView.GetLock();
            try
            {
                m_suspendUpdates = true;
                worksheet.UsedRange.Clear();
            }
            finally
            {
                m_suspendUpdates = false;
                wbView.ReleaseLock();
            } 
        }

        private void ClearFlags_Click(object sender, EventArgs e)
        {

        }

        private void mathFlag_Click(object sender, EventArgs e)
        {

        }

        private void lowToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void highToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
#endif