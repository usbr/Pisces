using System;
using System.Data;
using System.Collections.Generic;
using SpreadsheetGear;
using SpreadsheetGear.Advanced.Cells;
using System.IO;
using Reclamation.Core;
namespace Reclamation.TimeSeries.Excel
{

    /// <summary>
    /// SpreadsheetGearExcel assists working with excel spreadsheets
    /// </summary>
    public class SpreadsheetGearExcel :IDisposable
    {
        IWorkbook m_workbook;
        DateTime m_lastWriteTime;

        public IWorkbook Workbook
        {
            get { return m_workbook; }
            //set { m_workbook = value; }
        }
        string m_filename;
        public SpreadsheetGearExcel(string filename)
        {
            m_filename = filename;
            FileInfo fi = new FileInfo(filename);
            m_lastWriteTime = fi.LastWriteTime;

            m_workbook = SpreadsheetGear.Factory.GetWorkbook(filename);
            Console.WriteLine(Workbook.FileFormat.ToString());
        }

        public DateTime LastWriteTime
        {
            get { return m_lastWriteTime; }
            // set { m_creationTime = value; }
        }

        public static void CreateWorkBook(string filename)
        {
            SpreadsheetGear.IWorkbookSet workbookSet = SpreadsheetGear.Factory.GetWorkbookSet();
            SpreadsheetGear.IWorkbook workbook = workbookSet.Workbooks.Add();
            workbook.SaveAs(filename, FileFormat.Excel8);
        }

        public void AddSheet(string sheetName)
        {
            if (Array.IndexOf(SheetNames, sheetName) >= 0)
                throw new ArgumentException(sheetName + " allready exists in the file " + m_filename);

            IWorksheet sheet = Workbook.Worksheets.Add();
            sheet.Name = sheetName;

        }

        public void Save()
        {
            Console.WriteLine(Workbook.FileFormat.ToString());
            Console.WriteLine(Workbook.FullName);
            Workbook.SaveAs(Workbook.FullName, Workbook.FileFormat);
           // Workbook.SaveAs(Workbook.FullName, FileFormat.
        }
        public string[] SheetNames
        {
            get
            {
                List<string> names = new List<string>();
                for (int i = 0; i < Workbook.Worksheets.Count; i++)
                {
                    IWorksheet sheet = Workbook.Worksheets[i];
                    names.Add(sheet.Name);
                }

                return names.ToArray();
            }
        }

        public DataTable GetDataTable(string sheetName)
        {
            return GetDataTable(sheetName, SpreadsheetGear.Data.GetDataFlags.None);
        }

        public DataTable GetDataTable(string sheetName, SpreadsheetGear.Data.GetDataFlags flags )
        {
            IWorksheet worksheet = Workbook.Worksheets[sheetName];
            DataTable t = worksheet.UsedRange.GetDataTable(flags);
            return t;
        }

        /// <summary>
        /// retrieves DataTable with the first column as a DateTime Type
        /// </summary>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        public DataTable GetTimeSeriesTable(string sheetName)
        {
            var t = GetDataTable(sheetName);

            if( t.Columns.Count >0 && t.Columns[0].DataType != typeof(double))
            {
                throw new InvalidCastException("The first column must contain numbers");
            }

            DataTable t2 = new DataTable();
            t2.Columns.Add("DateTime", typeof(DateTime));
            for (int i = 0; i < t.Columns.Count; i++) 
            {
                t2.Columns.Add(t.Columns[i].ColumnName, t.Columns[i].DataType);
            }
            // copy data
            for (int r = 0; r < t.Rows.Count; r++)
            {
                var data = new List<object>();
                if (t.Rows[r][0] == DBNull.Value)
                    continue;

                double val = Convert.ToDouble(t.Rows[r][0]);

                DateTime dateTime1 = DoubleToDateTime(val, this.Workbook);
                data.Add(dateTime1);
                data.AddRange(t.Rows[r].ItemArray);

                t2.Rows.Add(data.ToArray());
            }

            t2.Columns.RemoveAt(1); 
            return t2;
        }

        public void SaveDataTable(DataTable table, string sheetName)
        {
            IWorksheet worksheet = Workbook.Worksheets[sheetName];
            IRange dstRange = worksheet.Cells["A1"];
            //IRange dstRange = worksheet.Cells[rowIndex, columnIndex, rowIndex + table.Rows.Count, columnIndex + table.Columns.Count];
                
            dstRange.CopyFromDataTable(table, SpreadsheetGear.Data.SetDataFlags.None);
        }

        public void Calculate()
        {
            
         }

      //  public enum ColumnTypes { All, DateTime, NonDateTime };

        public string[] ColumnReferenceNames(string sheetName)
        {
            List<string> columns = new List<string>();
            IWorksheet worksheet = Workbook.Worksheets[sheetName];

            int c0 = worksheet.UsedRange.Column;
            int cN = worksheet.UsedRange.ColumnCount + c0;
            IValues values = (IValues)worksheet;
            IRange range = worksheet.Cells;
            for (int i = c0; i < cN; i++)
            {
                columns.Add(ReferenceFromIndex(i));
            }

            return columns.ToArray();
        }

        public string[] ColumnNames(string sheetName )
        {
            
            List<string> columns = new List<string>();
            IWorksheet worksheet = Workbook.Worksheets[sheetName];

            // GetDataTable is used to enforce unique column names
            //DataTable t = worksheet.UsedRange.GetDataTable(SpreadsheetGear.Data.GetDataFlags.None);

            int c0 = worksheet.UsedRange.Column;
            int cN = worksheet.UsedRange.ColumnCount + c0 ;
            IValues values = (IValues)worksheet;
            IRange range = worksheet.Cells;
            for (int i = c0; i < cN; i++)
            {
                    
                // find column headers on first row.
                if (values[0, i] == null)
                {
                    continue;
                }

                string name = values[0, i].Text;
                if (name != null)
                {
                    if (columns.Contains(name)) 
                        throw new DuplicateNameException("the column '"+name+"' exists more than once in the spreasheet.");
                    
                    columns.Add(name);

                }
            }
            return columns.ToArray();
        }


        public void Dispose()
        {
            Workbook.Close();            
        }

        /// <summary>
        /// Converts column index to column reference.
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public static string ReferenceFromIndex(int idx)
        {
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int index1 = 0; // index to first character
            int index2 = -1; // index to second character
            int counter = 0;
            string current = letters[index1].ToString();
            do
            {
                // Console.WriteLine(counter + " " + current);
                //                if (String.Compare(colRef, current) == 0)
                if (idx == counter)
                    return current;

                if (index1 == 25)
                {
                    index1 = 0;
                    index2++;
                }
                else
                {
                    index1++;
                }

                current = letters[index1].ToString();
                if (index2 >= 0)
                    current = letters[index2].ToString() + letters[index1].ToString();

                counter++;

            } while (counter <= 255);

            return "";
        }







        public bool SheetExist(string sheetName)
        {
            foreach (var item in SheetNames)
            {
                if (string.Compare(item, sheetName, true) == 0)
                    return true;
            }

            return false;
        }


        public static bool TryReadingValue(IValue v, out int i)
        {
            i = -1;
            if (v == null)
                return false;

            if (v.Type == SpreadsheetGear.Advanced.Cells.ValueType.Number)
            {
                i = Convert.ToInt32(v.Number);
            }
            else
                if (v.Type == SpreadsheetGear.Advanced.Cells.ValueType.Text)
                {
                    return int.TryParse(v.Text, out i);
                }

                else
                {
                    return false;
                }

            return true;
        }
        public static bool TryReadingValue(IValue v, out double d)
        {
            d = Point.MissingValueFlag;
            if (v == null)
                return false;

            if (v.Type == SpreadsheetGear.Advanced.Cells.ValueType.Number)
            {
                d = v.Number;
            }
            else
                if (v.Type == SpreadsheetGear.Advanced.Cells.ValueType.Text)
                {
                    return Double.TryParse(v.Text, out d);
                }

                else
                {
                    return false;
                }

            return true;
        }

        private static DateTime DoubleToDateTime(double d,IWorkbook workbook)
        {
            var t = new DateTime();

            //January 1st, 1900 is 1.
            if (d < 0)
            {// hack.. negative number before jan 1,1900
                DateTime j = new DateTime(1900, 1, 1);
                t = j.AddDays(d - 1);
            }
            else
            {
                t = workbook.NumberToDateTime(d);
            }
            return t;
        }

        public static bool TryReadingDate(IWorkbook workbook, IValue v, out DateTime t)
        {
            t = DateTime.MinValue;
            if (v.Type == SpreadsheetGear.Advanced.Cells.ValueType.Number)
            {
                t = DoubleToDateTime(v.Number,workbook);
            }
            else
                if (v.Type == SpreadsheetGear.Advanced.Cells.ValueType.Text)
                {
                    return DateTime.TryParse(v.Text, out t);
                }
                else
                {
                    return false;
                }


            return true;

        }

        private static Dictionary<string, SpreadsheetGearExcel> s_cache = new Dictionary<string, SpreadsheetGearExcel>();

        /// <summary>
        /// loads spreadsheet from disk or gets a referecne to cached refererence
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static IWorkbook GetWorkbookReference(string filename)
        {
            IWorkbook workbook = null;
            SpreadsheetGearExcel xls = null;
            if (s_cache.ContainsKey(filename))
            { // allready in memory.
                Logger.WriteLine("Found file in cache: " + Path.GetFileName(filename));
                xls = s_cache[filename];
                workbook = xls.Workbook;
                FileInfo fi = new FileInfo(filename);

                if (fi.LastWriteTime > xls.LastWriteTime)
                {
                    Logger.WriteLine("xls File has changed... updating cache");
                    xls = new SpreadsheetGearExcel(filename);
                    workbook = xls.Workbook;
                    s_cache[filename] = xls;
                }
            }
            else
            {
                Logger.WriteLine("reading file from disk: " + Path.GetFileName(filename));
                if (!File.Exists(filename))
                {
                    Logger.WriteLine("File does not exist: '" + filename + "'");
                }
                xls = new SpreadsheetGearExcel(filename);
                workbook = xls.Workbook;

                int max_cache_size = 10;
                if (s_cache.Count >= max_cache_size)
                {
                    s_cache.Clear();
                    Logger.WriteLine(" s_cache.Clear()");
                }
                s_cache.Add(filename, xls);
            }
            return workbook;
        }

    }
}
