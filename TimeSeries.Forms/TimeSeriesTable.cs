using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.IO;
using Reclamation.TimeSeries;
using System.Text;

namespace Reclamation.TimeSeries.Forms
{
	/// <summary>
	/// TimeSeriesTable allows a user to view and edit time series
	/// data in a table format.
	/// </summary>
	public class TimeSeriesTable : System.Windows.Forms.Form
	{
    
    private Series series;
        private DataTable table;
    private System.Windows.Forms.ToolBar toolBar1;
    private System.Windows.Forms.ToolBarButton toolBarButtonCopy;
    private System.Windows.Forms.ImageList imageList1;
    private System.Windows.Forms.ToolBarButton toolBarButtonExcel;
    private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    private System.Windows.Forms.StatusBar statusBar1;
    //private System.Windows.Forms.ToolBarButton toolBarButtonSaveToDatabase;
        private DataGridView dataGridView1;
        private ToolBarButton toolBarButtonSave=null;
        private ToolBarButton toolBarButtonCustom1;
    private System.ComponentModel.IContainer components;


    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="series"></param>
		public TimeSeriesTable(Series series)
		{
		  InitializeComponent();
          this.series = series;
          this.Text = series.Name;
          table = series.Table;
          table.RowChanged +=  new DataRowChangeEventHandler(table_RowChanged);
          table.RowDeleted +=  new DataRowChangeEventHandler(table_RowDeleted);
          this.Name = series.Name;
          FormatGrid();
          this.toolBarButtonSave.Enabled = !series.ReadOnly;
           
		}

    //public void AddCustomButton(string text)
    //{
    //  this.toolBarButtonCustom1.Visible = true;
    //  this.toolBarButtonCustom1.Text = text;
    //}

    /// <summary>
    /// call SuspendUpdates during long operations
    /// so the DataGrid will not suck away cpu cycles.
    /// Be sure to call ResumeUpdates after long operation.
    /// </summary>
    public void SuspendUpdates()
    {
      this.dataGridView1.DataSource=null;
    }
    /// <summary>
    /// resumes event firing
    /// </summary>
    public void ResumeUpdates()
    {
      FormatGrid();
    }

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
        this.table = null;
        this.dataGridView1.DataSource=null;
        this.series = null;
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TimeSeriesTable));
            this.toolBar1 = new System.Windows.Forms.ToolBar();
            this.toolBarButtonCopy = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonExcel = new System.Windows.Forms.ToolBarButton();
            this.toolBarButtonCustom1 = new System.Windows.Forms.ToolBarButton();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.statusBar1 = new System.Windows.Forms.StatusBar();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolBar1
            // 
            this.toolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.toolBarButtonCopy,
            this.toolBarButtonExcel,
            this.toolBarButtonCustom1});
            this.toolBar1.DropDownArrows = true;
            this.toolBar1.ImageList = this.imageList1;
            this.toolBar1.Location = new System.Drawing.Point(0, 0);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.ShowToolTips = true;
            this.toolBar1.Size = new System.Drawing.Size(426, 36);
            this.toolBar1.TabIndex = 4;
            this.toolBar1.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.toolBar1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.toolBar1_ButtonClick);
            // 
            // toolBarButtonCopy
            // 
            this.toolBarButtonCopy.ImageIndex = 1;
            this.toolBarButtonCopy.Name = "toolBarButtonCopy";
            this.toolBarButtonCopy.ToolTipText = "copy to clipboard";
            // 
            // toolBarButtonExcel
            // 
            this.toolBarButtonExcel.ImageIndex = 4;
            this.toolBarButtonExcel.Name = "toolBarButtonExcel";
            this.toolBarButtonExcel.ToolTipText = "export to text file and open with excel";
            // 
            // toolBarButtonCustom1
            // 
            this.toolBarButtonCustom1.ImageIndex = 0;
            this.toolBarButtonCustom1.Name = "toolBarButtonCustom1";
            this.toolBarButtonCustom1.ToolTipText = "saves changes to the database";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "database_pipes_24bit.bmp");
            this.imageList1.Images.SetKeyName(3, "excelsmall.bmp");
            this.imageList1.Images.SetKeyName(4, "EXCEL_257.ico");
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "CSV (comma delimited) (*.csv)|*.csv";
            // 
            // statusBar1
            // 
            this.statusBar1.Location = new System.Drawing.Point(0, 455);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Size = new System.Drawing.Size(426, 20);
            this.statusBar1.TabIndex = 5;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 36);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(426, 419);
            this.dataGridView1.TabIndex = 6;
            // 
            // TimeSeriesTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 475);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.toolBar1);
            this.Name = "TimeSeriesTable";
            this.Closed += new System.EventHandler(this.TimeSeriesTable_Closed);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }
		#endregion

    private void toolBar1_ButtonClick(object sender,
            System.Windows.Forms.ToolBarButtonClickEventArgs e)
    {
      Cursor = Cursors.WaitCursor;
      try
      {
        if (e.Button ==toolBarButtonCopy )
        {// copy to clipboard.
          CopyToClipboard(series);
        }
        else if( e.Button == toolBarButtonExcel)
        {
            string tmpFilename = Path.GetTempFileName();
            File.Delete(tmpFilename);
            tmpFilename = Path.ChangeExtension(tmpFilename, ".csv");
            series.WriteCsv(tmpFilename);
            System.Diagnostics.Process.Start(tmpFilename);
        }
        else if (e.Button == toolBarButtonSave) 
        {
            if (SaveInDatabase != null)
            {
                SaveInDatabase(this, new SeriesEventArgs(this.series));
            }
        }
           
       }
      catch(Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
      Cursor = Cursors.Default;
    }

        private void CopyToClipboard(Series series)
        {
            bool includeFlag = true;
            //DataView view = series.Table.;//.DataView;

            StringBuilder sb = new StringBuilder();

            string fmt = series.DateTimeFormat;

            sb.Append("Date\tValue");
            if (includeFlag)
            {
                sb.Append("\tflag");
            }
            sb.Append("\r\n");

            int numRows = series.Count;

            for (int i = 0; i < numRows; i++)
            {
                Point pt = series[i];
                sb.Append(pt.DateTime.ToString(fmt));
                sb.Append("\t");
                sb.Append(pt.Value);
                if (includeFlag)
                {
                    sb.Append("\t");
                    sb.Append(pt.Flag);
                }
                sb.Append("\r\n");
            }
            System.Windows.Forms.Clipboard.SetDataObject(sb.ToString(), true);

        }

        public event EventHandler SaveInDatabase;


    /// <summary>
    /// Fires when user makes an edit.
    /// </summary>
    public event EventHandler Changed;

    private void table_RowChanged(object sender, DataRowChangeEventArgs e)
    {
        if (e.Action == DataRowAction.Add)
            return;

      if (Changed != null)
        Changed(this, e);
    }

    private void table_RowDeleted(object sender, DataRowChangeEventArgs e)
    {
      if (Changed != null)
        Changed(this, e);
    }

    //public delegate void SeriesEventHandler(object sender, SeriesEventArgs e);

    //// Now, create a public event "FireEvent" whose type is our FireEventHandler delegate. 

    //public event SeriesEventHandler CustomButtonClicked;


    

    private void TimeSeriesTable_Closed(object sender, System.EventArgs e)
    {
    
    }
    private void FormatGrid()
    {
      //cbp.DataGridFormater f = new cbp.DataGridFormater(this.dataGrid1,
        //           table,series.IsReadOnly,true);
        this.dataGridView1.DataSource = table;
        this.dataGridView1.ReadOnly = series.ReadOnly;
        this.dataGridView1.AllowUserToAddRows = !series.ReadOnly;
        this.dataGridView1.AllowUserToDeleteRows = !series.ReadOnly;

        this.dataGridView1.Columns[0].ReadOnly = series.ReadOnly;
        this.dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        this.dataGridView1.Columns[0].DefaultCellStyle.Format = series.DateTimeFormat;

        this.dataGridView1.Columns[1].ReadOnly = series.ReadOnly;
        this.dataGridView1.Columns[1].DefaultCellStyle.Format = "F3";

        for (int i = 2; i < table.Columns.Count; i++)
        {
            this.dataGridView1.Columns[i].Visible = false;
        }
      //string columnName = table.Columns[0].ColumnName;
      //f.Add(columnName,dateFormat,120,columnName);
      //columnName = table.Columns[1].ColumnName;
      //f.Add(columnName,"F3",80,columnName,series.IsReadOnly);
      //  columnName = table.Columns[2].ColumnName;
      //f.Add(columnName); // flag column
    }


  }
}
