using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;
using System.IO;
using ConnectionString = Reclamation.Core.ConnectionStringUtility;
using System.Data;
namespace Reclamation.TimeSeries.Excel
{
    public class ExcelDataReaderSeries : Reclamation.TimeSeries.Series
    {
        string filename;
        string sheetName;
        string dateColumn;
        string valueColumn;
        int errorCount = 0;


        public static bool AutoUpdate = true;

        DataSet workbook;
        DataTable worksheet;

        
        /// <summary>
        /// Create Series that references data in Excel
        /// </summary>
        public ExcelDataReaderSeries(string filename, string sheetName,
            string dateColumn, string valueColumn, string units="")
            : base()
        {
            workbook = ExcelUtility.GetWorkbookReference(filename);
            Init(workbook, sheetName, dateColumn, valueColumn,units);
        }

       
        public ExcelDataReaderSeries(TimeSeriesDatabase db, Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow sr): base(db, sr)
        {

        }
    
        public string SheetName
        {
            get
            {
                return this.sheetName;
            }
        }


        private void Init(DataSet workbook, string sheetName, string dateColumn, 
            string valueColumn, string units)
        {
            this.filename = Path.GetFileName(workbook.DataSetName);
            this.sheetName = sheetName;
            this.dateColumn = dateColumn.Trim();
            this.valueColumn = valueColumn.Trim();
            this.workbook = workbook;
            this.Source = "Excel";
            this.Provider = "ExcelDataReaderSeries";
            this.Units = units;
            this.Name = CleanTextForTreeName(valueColumn + "_" + sheetName + "_" + workbook.DataSetName);
            FileInfo fi = new FileInfo(workbook.DataSetName);

            this.ConnectionString = "FileName=" + workbook.DataSetName
                + ";SheetName=" + sheetName + ";DateColumn=" + dateColumn
                + ";ValueColumn=" + valueColumn 
                + ";LastWriteTime=" + fi.LastWriteTime.ToString(DateTimeFormatInstantaneous);

            this.Parameter = valueColumn;

            worksheet = workbook.Tables[sheetName];

            idxDate = FindColumnIndex(dateColumn);
            idxValue = FindColumnIndex(valueColumn);

            if (idxDate <0 || idxValue < 0)
            {
                //string msg = "Error: " + workbook.Name + "Error finding date Column '" + dateColumn + "' or '" + valueColumn + "'";
                string msg = "Error in worksheet: " +sheetName + ". Error finding date Column '" + dateColumn + "' or '" + valueColumn + "'";
                Logger.WriteLine(msg);

                //throw new ArgumentException(msg);
            }


        }

        private int FindColumnIndex(string colName)
        {
            if (worksheet.Columns.IndexOf(colName) >= 0)
                return worksheet.Columns.IndexOf(colName);

            return ColumnIndexFromRef(colName);
        }

        int idxDate;
        int idxValue;

        protected override void ReadCore()
        {
            Read(TimeSeriesDatabase.MinDateTime, TimeSeriesDatabase.MaxDateTime);
        }

        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            Clear();
            if (m_db != null )// using local database
            {
                if (true) //Excel.SpreadsheetGearSeries.AutoUpdate)
                {
                    UpdateCore(t1, t2,false);
                }

                this.Table = m_db.ReadTimeSeriesTable(ID, t1, t2);
               // m_db.ReadSeriesProperties(this, m_sr);
            }
            else
            {
               ReadFromExcel(t1, t2);
            }

            this.TimeInterval = Series.EstimateInterval(this);
        }

        
        private void ReadFromExcel(DateTime t1, DateTime t2)
        {
            errorCount = 0;

             for (int j = 0; j < worksheet.Rows.Count; j++)
            {
                DateTime t;
                double d;



                string str = worksheet.Rows[j][idxDate].ToString();
                if (!DateTime.TryParse(str, out t))
                {
                    Logger.WriteLine("Error parsing date '" + str + "'");
                    continue;
                }
                str = worksheet.Rows[j][idxValue].ToString();
                if (!Double.TryParse(str, out d))
                {
                    Logger.WriteLine("Error parsing value '" + str + "'");
                    continue;
                }

                if (IndexOf(t) >= 0)
                {
                    Logger.WriteLine("duplicate value skipped " + t.ToString());
                    continue;
                }

                 if( t >= t1 && t <=t2)
                   Add(t, d);
            }
            

            if (errorCount > 100)
            {
                Logger.WriteLine("Skipped " + (errorCount - 100) + " messages");
            }

        }


        protected override Series CreateFromConnectionString()
        {
           string dir = Path.GetDirectoryName(m_db.DataSource);
           return CreateFromConnectionString(this.ConnectionString, dir);
        }



        public static ExcelDataReaderSeries CreateFromConnectionString(string connectionString, 
            string dataPath)
        {
            string fileName = ConnectionStringUtility.GetToken(connectionString, "FileName", "");
            if (!Path.IsPathRooted(fileName))
            {
                fileName = Path.Combine(dataPath, fileName);
            }
            ExcelDataReaderSeries rval = new ExcelDataReaderSeries(
                fileName,
                ConnectionStringUtility.GetToken(connectionString, "SheetName", ""),
                ConnectionStringUtility.GetToken(connectionString, "DateColumn", ""),
                ConnectionStringUtility.GetToken(connectionString, "ValueColumn", "")
                );
            return rval;
        }


        /// <summary>
        /// Updates database if the original source file still exists and has 
        /// been modified. 
        /// </summary>
        protected override void UpdateCore(DateTime t1, DateTime t2, bool minimal)
        {
            Logger.WriteLine("Checking Excel series " + Name + " (" + ID + ") for updates");
            string dir = Path.GetDirectoryName(m_db.DataSource);
            if (TextSeries.CanUpdateFromFile(ConnectionString, dir))
            {
                Logger.WriteLine("Update: File has changed");
                ExcelDataReaderSeries g = ExcelDataReaderSeries.CreateFromConnectionString(ConnectionString, dir);
                g.Read();
                //m_db.Truncate(ID);
                ConnectionString = g.ConnectionString;
                ConnectionString = ConnectionStringUtility.MakeFileNameRelative(ConnectionString, m_db.DataSource);
                m_db.SaveProperties(this);// LastWriteTime proabably changed
                m_db.SaveTimeSeriesTable(ID, g, DatabaseSaveOptions.DeleteAllExisting);
            }
        }




        /// <summary>
        /// Converts column reference such as C to 
        /// the index 2  (zero based index)
        /// or IV to 256.
        /// </summary>
         static int ColumnIndexFromRef(string colRef)
        {
            string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int index1 = 0; // index to first character
            int index2 = -1; // index to second character
            int counter = 0;
            string current = letters[index1].ToString();
            do
            {
                // Console.WriteLine(counter + " " + current);
                if (String.Compare(colRef, current) == 0)
                    return counter;

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

            return -1;
        }


    }
}
