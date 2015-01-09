using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using NPOI.POIFS.FileSystem;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Reclamation.Core;
using NPOI.SS.Util;

namespace Reclamation.Core
{
    /// <summary>
    /// http://poi.apache.org/spreadsheet/quick-guide.html#Iterator
    /// </summary>
    public class NpoiExcel
    {

       // DataSet m_workbook;
        DateTime m_lastWriteTime;
        IWorkbook npoi_workbook;
      //  IWorkbook _workBook;

        string m_filename;

        // creates blank workbook
        public NpoiExcel()
        {
            npoi_workbook = new HSSFWorkbook();
            npoi_workbook.CreateSheet("sheet1");
            //Write the stream data of workbook to the root directory
            var fn = Path.GetTempFileName();
            FileStream file = new FileStream(fn, FileMode.Create);
            npoi_workbook.Write(file);
            file.Close();
        }

        /// <summary>
        /// Reads workbook
        /// </summary>
        /// <param name="filename"></param>
        public NpoiExcel(string filename)
        {
            m_filename = filename;
            FileInfo fi = new FileInfo(filename);
            m_lastWriteTime = fi.LastWriteTime;

            FileStream file = new FileStream(filename, FileMode.Open);
            var fs = new POIFSFileSystem(file);

            npoi_workbook = new HSSFWorkbook(fs);

           // m_workbook = ReadDataSet(filename,oaDateTime:true);
           // m_workbook.DataSetName = filename;
        }

        //public DataSet Workbook
        //{
        //    get { return m_workbook; }
        //    //set { m_workbook = value; }
        //}


        public DateTime LastWriteTime
        {
            get { return m_lastWriteTime; }
            // set { m_creationTime = value; }
        }

        public string[] ColumnNames(string sheetName)
        {
            List<string> rval = new List<string>();
           var sheet = npoi_workbook.GetSheet(sheetName);
           var row = sheet.GetRow(0);
           if (row != null)
           {

               for (int i = 0; i < row.LastCellNum; i++)
               {
                   var c1 = row.GetCell(i);

                   if (c1 != null && c1.StringCellValue != null)
                       rval.Add(c1.StringCellValue);
               } 
           }
           return rval.ToArray();
        }



        public static DataTable Read(string fileName, string workSheetName, bool captureDateTime=false)
        {
            var ds = ReadDataSet(fileName,true,captureDateTime);
            return ds.Tables[workSheetName];
        }
        public static DataTable Read(string fileName, int workSheetIndex, bool captureDateTime=false)
        {
            var ds = ReadDataSet(fileName, true, captureDateTime);
            return ds.Tables[workSheetIndex];
        }

        /// <summary>
        /// Creates a new excel file
        /// </summary>
        /// <param name="filename"></param>
        public static void CreateXLS(string filename)
        {
            Stream stream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("Reclamation.Core.blank.xls");
            CreateXLS(filename, stream);
        }
        /// <summary>
        /// Creates a new excel file.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="stream"></param>
         static void CreateXLS(string filename, Stream stream)
        {

            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);

            File.Delete(filename);
            FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fs);

            bw.Write(buffer);

            bw.Close();
            fs.Close();
        }


        public static DataSet ReadDataSet(string fileName, bool columnNamesInFirstRow = true, bool oaDateTime=false)
        {
            throw new NotImplementedException();

            FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);

            string ext = Path.GetExtension(fileName);

            return new DataSet("data");

        }



        public string[] SheetNames(string fileName)
        {
            var rval = new List<string>();

            for (int i = 0; i < this.npoi_workbook.NumberOfSheets; i++ )
            {
                rval.Add(npoi_workbook.GetSheetName(i));
            }
            return rval.ToArray();
        }

        public void SaveDataTable(DataTable table, string sheetName)
        {
            var sheet = npoi_workbook.GetSheet(sheetName);
            if (sheet == null) // create sheet
                sheet = npoi_workbook.CreateSheet(sheetName);

            List<string> dataTypes = new List<string>();
            for (int i = 0; i < table.Columns.Count; i++)
            {
                dataTypes.Add(table.Columns[i].DataType.ToString());
            }

            var row = sheet.CreateRow(0);
            for (int col = 0; col < table.Columns.Count; col++)
            {
                var cell = row.CreateCell(col);
                cell.SetCellValue(table.Columns[col].ColumnName);
            }
            for (int rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
			{   
			   row = sheet.CreateRow(rowIndex+1);

               for (int col = 0; col < table.Columns.Count; col++)
               {
                  var cell = row.CreateCell(col);
                  var obj = table.Rows[rowIndex][col];
                  var strDataType = dataTypes[col];
                  SetCellValue(cell, obj, strDataType);
               }

			}
        }

        private void SetCellValue(ICell cell, object obj, string strDataType)
        {
            switch (strDataType)
            {
                case "System.Int32":
                    SetInt(cell, obj);
                    break;
                case "System.Integer":
                    SetInt(cell, obj);
                    break;
                case "System.Double":
                    SetDouble(cell, obj);
                    break;
                case "System.String":
                    SetString(cell, obj);
                    break;
                case "System.DateTime":
                    SetDateTime(cell, obj);
                    break;
                case "System.Boolean":
                    SetBool(cell, obj);
                    break;
                default:
                    SetString(cell, obj);
                    break;
            }
        }

        private void SetBool(ICell cell, object obj)
        {
            if (obj != DBNull.Value)
                cell.SetCellValue(Convert.ToBoolean(obj));
        }

        private void SetInt(ICell cell, object obj)
        {
            if (obj != DBNull.Value)
                cell.SetCellValue(Convert.ToInt32(obj));
        }
        private void SetDouble(ICell cell, object obj)
        {
            if (obj != DBNull.Value)
                cell.SetCellValue(Convert.ToDouble(obj));
        }
        private void SetString(ICell cell, object obj)
        {
            if (obj != DBNull.Value)
                cell.SetCellValue(obj.ToString());
        }


        private void SetDateTime(ICell cell, object obj)
        {
            if (obj != DBNull.Value)
            {
                cell.SetCellValue(Convert.ToDateTime(obj));
                SetDateStyle(cell);
            }
        }


       ICellStyle m_dateCellStyle;
        private void SetDateStyle(ICell cell)
        {
            if (m_dateCellStyle == null)
            {
                m_dateCellStyle = npoi_workbook.CreateCellStyle();
                m_dateCellStyle.DataFormat = npoi_workbook.CreateDataFormat().GetFormat("mm-dd-yyyy");
            }
            cell.CellStyle = m_dateCellStyle;
        }

        public void Save(string filename)
        {
            //Write the stream data of workbook to the root directory
            FileStream file = new FileStream(filename, FileMode.Create);
            npoi_workbook.Write(file);
            file.Close();
        }
        internal DataTable ReadDataTable(int sheetIndex)
        {

            var sheet = npoi_workbook.GetSheetAt(sheetIndex); 

            return ReadTable(sheet);
        }


        internal DataTable ReadDataTable(string sheetName)
        {
            
            var sheet = npoi_workbook.GetSheet(sheetName);

            return ReadTable(sheet);
        }

        private DataTable ReadTable( ISheet sheet)
        {
            DataTable rval = new DataTable(sheet.SheetName);
            // get column names, in first row, estimate datatype by second row.
            var row = sheet.GetRow(0);

            if (row == null) // no column names
            {
                // TO DO.
            }
            var row2 = sheet.GetRow(1);
            for (int c = 0; c < row.LastCellNum; c++)
            {
                var cell = row.GetCell(c);
                rval.Columns.Add(cell.StringCellValue, EstimateType(row2.GetCell(c)));
            }

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                var newRow = rval.NewRow();
                row = sheet.GetRow(i);
                for (int c = 0; c < row.LastCellNum; c++)
                {
                    newRow[c] = GetCellValue(row.GetCell(c), rval.Columns[c].DataType);
                }
                rval.Rows.Add(newRow);
            }
            return rval;
        }

        /// <summary>
        /// http://stackoverflow.com/questions/4276641/how-to-get-the-cell-value-of-a1cell-address-using-apache-poi-3-6
        /// </summary>
        /// <param name="sheetIndex"></param>
        /// <param name="cellRef"></param>
        /// <param name="val"></param>
        public void SetCellText(int sheetIndex, string cellRef, string val)
        {
            GetCell(sheetIndex, cellRef).SetCellValue(val);

        }
        public void SetCellDouble(int sheetIndex, string cellRef, double val)
        {
            GetCell(sheetIndex, cellRef).SetCellValue(val);
        }

        //public void SetCellDate(int sheetIndex, int rowIndex, int colIndex, object t)
        //{
        //    var cell = GetCell(sheetIndex, rowIndex, colIndex);
        //    SetDateTime(cell, t);
        //}

        private ICell GetCell(int sheetIndex, string cellRef )
        {
            var sheet = npoi_workbook.GetSheetAt(sheetIndex);
            
            CellReference cr = new CellReference(cellRef);
            var row = sheet.GetRow(cr.Row);
            if (row == null)
                row = sheet.CreateRow(cr.Row);
            var cell = row.GetCell(cr.Col);
            if (cell == null)
                cell = row.CreateCell(cr.Col);
            return cell;
        }

        private ICell GetCell(int sheetIndex, int rowIndex , int colIndex)
        {
            var sheet = npoi_workbook.GetSheetAt(sheetIndex);
            var row = CellUtil.GetRow(rowIndex, sheet);
            var cell = CellUtil.GetCell(row, colIndex);
            return cell;
        }
        
        private object GetCellValue(ICell cell, Type t)
        {
           
            if (t == typeof(bool))
                return cell.BooleanCellValue;
            if (t == typeof(double))
                return cell.NumericCellValue;
            if (t == typeof(string))
                return cell.StringCellValue;
            if (t == typeof(DateTime))
                return cell.DateCellValue;

            return cell.StringCellValue;
        }

        private Type EstimateType(ICell cell)
        {
            if (cell.CellType ==   CellType.Boolean)
                return typeof(bool);
            if (cell.CellType ==  CellType.Numeric)
                return typeof(double);
            if (cell.CellType ==  CellType.String)
                return typeof(string);
            // TO DO.  date time.
            //if(cell.CellType == NPOI.SS.UserModel.CellType.
            return typeof(string);
        }

        public void InsertDataTable(int sheetIndex, string cellRef,DataTable tbl)
        {
            CellReference cr = new CellReference(cellRef);
            // insert column names
            for (int c = 0; c < tbl.Columns.Count; c++)
            {
                var cell = GetCell(sheetIndex, cr.Row , cr.Col + c);
                SetCellValue(cell, tbl.Columns[c].ColumnName,"System.String");
            }
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                for (int c = 0; c < tbl.Columns.Count; c++)
                {
                    var cell = GetCell(sheetIndex, cr.Row + i+1, cr.Col + c);
                    SetCellValue(cell, tbl.Rows[i][c], tbl.Columns[c].DataType.ToString());
                }
            }
        }
        /*
General
0
0.00
#,##0
#,##0.00
"$"#,##0_);("$"#,##0)
"$"#,##0_);[Red]("$"#,##0)
"$"#,##0.00_);("$"#,##0.00)
"$"#,##0.00_);[Red]("$"#,##0.00)
0%
0.00%
0.00E+00
# ?/?
# ??/??
m/d/yy
d-mmm-yy
d-mmm
mmm-yy
h:mm AM/PM
h:mm:ss AM/PM
h:mm
h:mm:ss
m/d/yy h:mm
reserved-0x17
reserved-0x18
reserved-0x19
reserved-0x1A
reserved-0x1B
reserved-0x1C
reserved-0x1D
reserved-0x1E
reserved-0x1F
reserved-0x20
reserved-0x21
reserved-0x22
reserved-0x23
reserved-0x24
#,##0_);(#,##0)
#,##0_);[Red](#,##0)
#,##0.00_);(#,##0.00)
#,##0.00_);[Red](#,##0.00)
_("$"* #,##0_);_("$"* (#,##0);_("$"* "-"_);_(@_)
_(* #,##0_);_(* (#,##0);_(* "-"_);_(@_)
_("$"* #,##0.00_);_("$"* (#,##0.00);_("$"* "-"??_);_(@_
_(* #,##0.00_);_(* (#,##0.00);_(* "-"??_);_(@_)
mm:ss
[h]:mm:ss
mm:ss.0
##0.0E+0
@
         
         */

        public void FormatRange(int sheetIndex, int r1, int c1, int r2, int c2, string fmt)
        {
            CellReference a1 = new CellReference(r1, c1);
            CellReference a2 = new CellReference(r2, c2);
            AreaReference a = new AreaReference(a1, a2);
            FormatRef(sheetIndex, a,fmt);
        }
        public void FormatRange(int sheetIndex, string rangeRef, string fmt)
        {
            //for (int i = 0; i < HSSFDataFormat.NumberOfBuiltinBuiltinFormats; i++)
           // {
            //    Console.WriteLine(HSSFDataFormat.GetBuiltinFormat((short)i));
           // }
            AreaReference ar = new AreaReference(rangeRef);
            FormatRef(sheetIndex, ar,fmt);
        }

        private void FormatRef(int sheetIndex,  AreaReference ar, string fmt)
        {
            var cellRef = ar.GetAllReferencedCells();
            var cellStyle = npoi_workbook.CreateCellStyle();
            cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat(fmt);

            foreach (var item in cellRef)
            {
                var cell = GetCell(sheetIndex, item.Row, item.Col);
                cell.CellStyle = cellStyle;
            }

            Console.WriteLine(cellRef);
        }
    }
}
