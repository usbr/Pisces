using Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Reclamation.Core
{

    /// <summary>
    /// wrapper for http://exceldatareader.codeplex.com/
    /// </summary>
    public class ExcelUtility
    {
        DataSet m_workbook;
        DateTime m_lastWriteTime;

        string m_filename;
        public ExcelUtility(string filename)
        {
            m_filename = filename;
            FileInfo fi = new FileInfo(filename);
            m_lastWriteTime = fi.LastWriteTime;

            m_workbook = ReadDataSet(filename,oaDateTime:true);
            m_workbook.DataSetName = filename;
        }

        public DataSet Workbook
        {
            get { return m_workbook; }
            //set { m_workbook = value; }
        }


        public DateTime LastWriteTime
        {
            get { return m_lastWriteTime; }
            // set { m_creationTime = value; }
        }

        public string[] ColumnNames(string sheetName)
        {
            var rval = new List<string>();
            var cols = m_workbook.Tables[sheetName].Columns;
            for (int i = 0; i < cols.Count; i++)
            {
                rval.Add(cols[i].ColumnName);       
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
        public static void CreateXLS(string filename, Stream stream)
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
            FileStream stream = File.Open(fileName, FileMode.Open, FileAccess.Read);

            string ext = Path.GetExtension(fileName);
            
            IExcelDataReader excelReader;
            if (ext.ToLower() == ".xls")
            { //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else
            { //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
               excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
             
            //...
            //3. DataSet - The result of each spreadsheet will be created in the result.Tables
            //DataSet result = excelReader.AsDataSet();
            //...
            //4. DataSet - Create column names from first row
            excelReader.IsFirstRowAsColumnNames = columnNamesInFirstRow;

            DataSet result = excelReader.AsDataSet(oaDateTime);

            ////5. Data Reader methods
            //while (excelReader.Read())
            //{
            //    //excelReader.GetInt32(0);
            //}

            //6. Free resources (IExcelDataReader is IDisposable)
            excelReader.Close();
            excelReader.Dispose();
            return result;

        }


        private static Dictionary<string, ExcelUtility> s_cache = new Dictionary<string, ExcelUtility>();

        /// <summary>
        /// loads spreadsheet from disk or gets a reference to cached refererence
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static DataSet GetWorkbookReference(string filename)
        {
            DataSet workbook = null;
            ExcelUtility xls = null;
            if (s_cache.ContainsKey(filename))
            { // allready in memory.
                Logger.WriteLine("Found file in cache: " + Path.GetFileName(filename));
                xls = s_cache[filename];
                workbook = xls.Workbook;
                FileInfo fi = new FileInfo(filename);

                if (fi.LastWriteTime > xls.LastWriteTime)
                {
                    Logger.WriteLine("xls File has changed... updating cache");
                    xls = new ExcelUtility(filename);
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
                xls = new ExcelUtility(filename);
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




        public static string[] SheetNames(string fileName)
        {
            var rval = new List<string>();
            ExcelUtility xls = new ExcelUtility(fileName);
            foreach (DataTable item in xls.Workbook.Tables)
            {
                rval.Add(item.TableName);
            }
            return rval.ToArray();
        }
    }
}
