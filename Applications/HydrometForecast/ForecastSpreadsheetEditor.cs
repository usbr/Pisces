using System.Windows.Forms;
using SpreadsheetGear;
using SpreadsheetGear.Windows.Forms;
using System.Collections.Generic;
using SpreadsheetGear.Advanced.Cells;

namespace HydrometForecast
{
    public partial class ForecastSpreadsheetEditor : UserControl
    {
        public ForecastSpreadsheetEditor()
        {
            InitializeComponent();
        }

       
        IWorkbook workbook;
        WorkbookView wbView;
        IWorksheet worksheet;
        private void LoadWorkbookView()
        {
            //workbook = SpreadsheetGear.Factory.GetWorkbook(this.tmpXlsName);
           // workbook = SpreadsheetGear.Factory.GetWorkbook();
            worksheet = workbook.ActiveWorksheet;

            wbView = new SpreadsheetGear.Windows.Forms.WorkbookView(workbook);
            wbView.GetLock();
            try
            {
                //SpreadsheetGear.IWorkbookWindowInfo windowInfo = wbView.ActiveWorkbookWindowInfo;
                //windowInfo.DisplayWorkbookTabs = false;
                worksheet.WindowInfo.Zoom = 100;
                wbView.Parent = this; ;
                wbView.BringToFront();
                wbView.Dock = DockStyle.Fill;
            }
            finally
            {
                wbView.ReleaseLock();
            }
        }

        string m_filename = "";
        FileFormat m_fileFormat;
        internal void Open(string filename)
        {
            this.m_filename = filename;
            workbook = SpreadsheetGear.Factory.GetWorkbook(filename);
            LoadWorkbookView();
            m_fileFormat = workbook.FileFormat;
        }

        internal void Save()
        {
            wbView.GetLock();
            try
            {
                workbook.SaveAs(m_filename, m_fileFormat);
                //workbook.Save();
            }
            finally
            {
                wbView.ReleaseLock();
            }

        }

        public string[] SheetNames {

            get
            {
                 wbView.GetLock();
                 try
                 {
                     var rval = new List<string>();
                     for (int i = 0; workbook != null && i < workbook.Sheets.Count; i++)
                     {
                         rval.Add(workbook.Sheets[i].Name);
                     }
                     return rval.ToArray();
                 }
                 finally
                 {
                     wbView.ReleaseLock();
                 }
            }
            
             }


        internal string ActiveSheetName
        {
            get
            {
                string rval = "";
                 wbView.GetLock();
                 try
                 {
                     rval = workbook.ActiveSheet.Name;
                 }
                 finally
                 {
                     wbView.ReleaseLock();
                 }
                 return rval;
            }
        }

        internal void UpdateCoeficients(string sheetName, double[] newCoeficients)
        {
            wbView.GetLock();
            try
            {
                worksheet = (IWorksheet)workbook.Sheets[sheetName];
                IValues values = (IValues)worksheet;

                for (int rowIndex = 0; rowIndex < worksheet.UsedRange.RowCount; rowIndex++)
			{
                if ( values[rowIndex, 0].Text == "Coefficients")
                {
                    for (int j = 0; j < newCoeficients.Length; j++)
                    {
                        values.SetNumber(rowIndex, j + 1, newCoeficients[j]);
                    }
                    break;
                }
			}
             
                //workbook.SaveAs(fileName, FileFormat.CSV);
            }
            finally
            {
                wbView.ReleaseLock();
            }
        }

        internal void SaveSheetToCsv(string sheetName, string fileName)
        {
                 wbView.GetLock();
                 try
                 {
                     workbook.Sheets[sheetName].Select();
                     
                     workbook.SaveAs(fileName, FileFormat.CSV);
                 }
                 finally
                 {
                     wbView.ReleaseLock();
                 }
        }
    }
}
