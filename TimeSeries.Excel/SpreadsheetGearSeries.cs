using System;
using System.Collections.Generic;
using System.Text;
using SpreadsheetGear;
using SpreadsheetGear.Advanced.Cells;
using Reclamation.Core;
using System.IO;
using ConnectionString = Reclamation.Core.ConnectionStringUtility;
namespace Reclamation.TimeSeries.Excel
{
    public class SpreadsheetGearSeries : Reclamation.TimeSeries.Series
    {
        string filename;
        string sheetName;
        string dateColumn;
        string valueColumn;
        string units = "";
        bool waterYearFormat = false;
        int idxDate = -1;
        int idxValue = -1;
        int idxSiteColumn = -1;
        int beginningRowIndex = 1;
        int errorCount = 0;

        IWorkbook workbook;
        IWorksheet worksheet;

        public static bool AutoUpdate = true;
        private string siteColumn;
        private string siteFilter;

        /// <summary>
        /// Create Series that references data in Excel
        /// </summary>
        public SpreadsheetGearSeries(IWorkbook workbook, string sheetName,
            string dateColumn, string valueColumn, string units)
        {
            Init(workbook, sheetName, dateColumn, valueColumn,false,"","",units);
        }

        /// <summary>
        /// Create Series that references data in Excel
        /// </summary>
        public SpreadsheetGearSeries(IWorkbook workbook, string sheetName,
            string dateColumn, string valueColumn, bool waterYearFormat, string units)
        {
            Init(workbook, sheetName, dateColumn, valueColumn,waterYearFormat,"","",units);
        }


        
        /// <summary>
        /// Create Series that references data in Excel
        /// </summary>
        public SpreadsheetGearSeries(string filename, string sheetName,
            string dateColumn, string valueColumn)
            : base()
        {
            workbook = SpreadsheetGearExcel.GetWorkbookReference(filename);
            Init(workbook, sheetName, dateColumn, valueColumn,false,"","",this.units);
        }


        /// <summary>
        /// Create Series that references data in Excel
        /// </summary>
        public SpreadsheetGearSeries(string filename, string sheetName,
            string dateColumn, string valueColumn, bool waterYearFormat,string siteColumn="",
            string siteFilter="",string units="")
            : base()
        {
            workbook = SpreadsheetGearExcel.GetWorkbookReference(filename);
            Init(workbook, sheetName, dateColumn, valueColumn,waterYearFormat,siteColumn,siteFilter,units);
        }

      
       
        public SpreadsheetGearSeries(TimeSeriesDatabase db, Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow sr): base(db, sr)
        {

        }
        public SpreadsheetGearSeries(IWorkbook workbook, string sheetName, string dateColumn, string valueColumn, 
            string siteColumn, string siteFilter, string units)
        {
            Init(workbook, sheetName, dateColumn, valueColumn, false,siteColumn ,siteFilter, units);
        }

        public string SheetName
        {
            get
            {
                return this.sheetName;
            }
        }


        private void Init(IWorkbook workbook, string sheetName, string dateColumn, 
            string valueColumn, bool isWaterYearFormat, string siteColumn, string siteFilter, string units)
        {
            this.filename = workbook.Name;
            this.sheetName = sheetName;
            this.dateColumn = dateColumn.Trim();
            this.valueColumn = valueColumn.Trim();
            this.waterYearFormat = isWaterYearFormat;
            this.workbook = workbook;
            this.Source = "Excel";
            this.Provider = "SpreadsheetGearSeries";
            this.Units = units;
            this.Name = CleanTextForTreeName(valueColumn + "_" + sheetName + "_" + workbook.Name);
            if (siteFilter != "")
            {
                Name = CleanTextForTreeName(siteFilter + "_" + sheetName + "_" + workbook.Name);
            }
            this.siteColumn = siteColumn;
            this.siteFilter = siteFilter;
            FileInfo fi = new FileInfo(workbook.FullName);

            this.ConnectionString = "FileName=" + workbook.FullName
                + ";SheetName=" + sheetName + ";DateColumn=" + dateColumn
                + ";ValueColumn=" + valueColumn + ";IsWaterYearFormat=" + isWaterYearFormat
                + ";SiteColumn=" + siteColumn + ";SiteFilter="+siteFilter
                + ";LastWriteTime=" + fi.LastWriteTime.ToString(DateTimeFormatInstantaneous);

            this.Parameter = valueColumn;

            worksheet = workbook.Worksheets[sheetName];

            if (dateColumn.Trim() == "" || valueColumn.Trim() == "")
            {
                throw new ArgumentException("Error: Date and Value columns are required\nA blank Date or value Column Name was input");
            }

            if (!SearchForColumnNames(worksheet,dateColumn,valueColumn,siteColumn,out idxDate,out idxValue,out idxSiteColumn))
            {
                //string msg = "Error: " + workbook.Name + "Error finding date Column '" + dateColumn + "' or '" + valueColumn + "'";
                string msg = "Error in worksheet: " + worksheet.Name + ". Error finding date Column '" + dateColumn + "' or '" + valueColumn + "'";
                Logger.WriteLine(msg);

                //throw new ArgumentException(msg);
            }
        }


        protected override void ReadCore()
        {
            Read(TimeSeriesDatabase.MinDateTime, TimeSeriesDatabase.MaxDateTime);
        }

        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            Clear();
            if (m_db != null )// using local database
            {
                if (Excel.SpreadsheetGearSeries.AutoUpdate)
                {
                    UpdateCore(t1, t2,false);
                }

                this.Table = m_db.ReadTimeSeriesTable(ID, t1, t2);
               // m_db.ReadSeriesProperties(this, m_sr);
            }
            else
            {
                if (waterYearFormat)
                    ReadWaterYearsFromExcel(t1, t2);
                else
                    ReadFromExcel(t1, t2);
            }

            this.TimeInterval = Series.EstimateInterval(this);
        }

        
        private void ReadFromExcel(DateTime t1, DateTime t2)
        {
            errorCount = 0;
            if (idxDate < 0 || idxValue < 0)
                return;
            IRange rng = worksheet.UsedRange.Range;
            IValues values = (IValues)worksheet;

            for (int i = beginningRowIndex; i < rng.RowCount; i++)
            {
                //Console.WriteLine("row index "+i);
                if (idxSiteColumn >= 0)
                {// filtering enabled.
                    var v = values[i, idxSiteColumn];
                    if (v == null)
                        continue;

                    if (v.Type == SpreadsheetGear.Advanced.Cells.ValueType.Number)
                        if (v.Number.ToString() != siteFilter)
                            continue;
                    if (v.Type == SpreadsheetGear.Advanced.Cells.ValueType.Text)
                        if (v.Text != siteFilter)
                            continue;
                }


                if (values[i, idxDate] == null) 
                {
                    errorCount++;
                    if (errorCount > 100)
                        continue;
                    Logger.WriteLine("date is null ;skipping row at index = " + i);
                    continue;
                }

                DateTime t;
                if (!SpreadsheetGearExcel.TryReadingDate(workbook, values[i, idxDate], out t))
                {
                    errorCount++;
                    if (errorCount > 100)
                        continue;
                    Logger.WriteLine(filename + ": can't read date; row at index = " + i);
                    continue;
                }

                double d = Point.MissingValueFlag;
                    
                    if (values[i, idxValue] != null && 
                        !SpreadsheetGearExcel.TryReadingValue(values[i, idxValue], out d))
                    {
                        errorCount++;
                        if (errorCount < 100)
                        Logger.WriteLine(filename + ": can't read value ; row at index = " + i);
                        d = Point.MissingValueFlag;

                        //continue;
                    }
                    if (double.IsNaN(d))
                        d = Point.MissingValueFlag;

                int idxTime = this.IndexOf(t);

                if (idxTime >= 0)
                {
                    errorCount++;
                    if (errorCount > 100)
                        continue;
                    string msg = "duplicate date found in row " + i + " date = '" + t.ToString() + "' value =" + d;
                    Logger.WriteLine(msg);
                    Messages.Add(msg);
                    msg = "previously imported value = " + this[idxTime].Value;
                    Logger.WriteLine(msg);
                    Messages.Add(msg);

                }
                else if (t >= t1 && t <= t2)
                {
                    Add(t, d);
                }
            }

            if (errorCount > 100)
            {
                Logger.WriteLine("Skipped " + (errorCount - 100) + " messages");
            }

        }


        private void ReadWaterYearsFromExcel(DateTime t1, DateTime t2)
        {
            if (idxDate < 0 || idxValue < 0)
                return;
            errorCount = 0;
            IRange rng = worksheet.UsedRange.Range;
            IValues values = (IValues)worksheet;

            for (int i = beginningRowIndex; i < rng.RowCount; i++)
            {

                if (values[i, idxDate] == null || values[i, idxValue] == null)
                {
                    errorCount++;
                    if( errorCount >100)
                       Logger.WriteLine("date or value is null ;skipping row at index = " + i);
                    
                    continue;
                }
                double dyear = -1;
                if (!SpreadsheetGearExcel.TryReadingValue(values[i, idxDate], out dyear))
                {
                    errorCount++;
                    if( errorCount>100)
                       Logger.WriteLine("error reading water year");
                    continue;
                }
                int wy = Convert.ToInt32(dyear);

                for (int monthIndex = idxValue; monthIndex < idxValue + 12; monthIndex++)
                {
                    DateTime t = GetDate(wy, monthIndex,idxValue);

                    if (values[i, monthIndex] == null)
                    {
                        AddMissing(t);
                    }
                    else
                    {
                        double d;
                        if (!SpreadsheetGearExcel.TryReadingValue(values[i, monthIndex], out d))
                        {
                            errorCount++;

                            Logger.WriteLine("error reading value");
                            throw new InvalidCastException("Could not read value");
                        }
                        Add(t, d);
                    }
                }
            }
            if (errorCount > 100)
            {
                Logger.WriteLine("Skipped " + (errorCount - 100) + " messages");
            }
        }

        /// <summary>
        /// Returns date based on 1st of the month
        /// baed on water year where monthIndex 1 for october.
        /// </summary>
        /// <param name="wy"></param>
        /// <param name="monthIndex"></param>
        /// <param name="idxFirstMonth"></param>
        /// <returns></returns>
        private static DateTime GetDate(int wy, int monthIndex, int idxFirstMonth)
        {
            int[] wyIndex = { 10, 11, 12, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            int idx = monthIndex - idxFirstMonth;
            int month = wyIndex[idx];
            int year = wy;

            if (month >= 10)
                year = wy - 1;

            DateTime t = new DateTime(year, month, 1);
            return t;
        }

        private static Boolean SearchForColumnNames(IWorksheet worksheet,
             string dateColumn, string valueColumn, string siteColumn, out int idxDate, out int idxValue,
            out int idxSiteName)
        {
            idxDate = -1;
            idxValue = -1;
            idxSiteName = -1;
            int c0 = worksheet.UsedRange.Column;
            int cN = worksheet.UsedRange.ColumnCount + c0;
            IValues values = (IValues)worksheet;

            for (int i = c0; i < cN; i++)
            {
                // find column headers on first row.

                if (values[0, i] != null)
                {
                    string name = values[0, i].Text;
                    if (String.Compare(dateColumn, name, true) == 0)
                    {
                        idxDate = i;
                    }
                    if (String.Compare(valueColumn, name, true) == 0)
                    {
                        idxValue = i;
                    }
                    if (string.Compare(siteColumn, name, true) == 0)
                        idxSiteName = i;
                }
            }

            if (idxDate < 0)
            {
                idxDate = SpreadsheetGearSeries.ColumnIndexFromRef(dateColumn);
            }

            if (idxValue < 0)
            {
                idxValue = SpreadsheetGearSeries.ColumnIndexFromRef(valueColumn);
            }
            if (idxSiteName < 0)
            {
                idxSiteName = SpreadsheetGearSeries.ColumnIndexFromRef(siteColumn);
            }

            if (idxDate < 0)
            {
                Logger.WriteLine("Error: could not find column named '" + dateColumn + "'");
                return false;
            }
            if (idxValue < 0)
            {
                Logger.WriteLine("Error: could not find column named '" + valueColumn + "'");
                return false;
            }
            return true;

        }



        /// <summary>
        /// Converts column reference such as C to 
        /// the index 2  (zero based index)
        /// or IV to 256.
        /// </summary>
        public static int ColumnIndexFromRef(string colRef)
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


        public static SpreadsheetGearSeries ReadFromFile(string fileName, string sheetName, string dateColumn,
            string valueColumn)
        {
            SpreadsheetGearSeries s = new SpreadsheetGearSeries(fileName, sheetName, dateColumn, valueColumn);
            s.Read();
            return s;
        }

        public static SpreadsheetGearSeries ReadFromWorkbook(IWorkbook workbook, string sheetName, string dateColumn,
            string valueColumn, bool isWaterYearformat, string units)
        {
            SpreadsheetGearSeries s = new SpreadsheetGearSeries(workbook, sheetName, dateColumn, valueColumn,isWaterYearformat,units);
            s.Read();
            return s;
        }

        public static SpreadsheetGearSeries ReadFromWorkbook(IWorkbook workbook, string sheetName, string dateColumn,
            string valueColumn, string siteColumn, string siteFilter, string units)
        {
            SpreadsheetGearSeries s = new SpreadsheetGearSeries(workbook, sheetName, dateColumn, valueColumn,siteColumn, siteFilter,units);
            s.Read();
            return s;
        }


        protected override Series CreateFromConnectionString()
        {
           string dir = Path.GetDirectoryName(m_db.DataSource);
           return CreateFromConnectionString(this.ConnectionString, dir);
        }


        //public static SpreadsheetGearSeries ReadFromConnectionString(string connectionString,
        //    string dataPath)
        //{
        //    SpreadsheetGearSeries rval = CreateFromConnectionString(connectionString, dataPath);
        //    rval.Read();
        //    return rval;
        //}


        

        public static SpreadsheetGearSeries CreateFromConnectionString(string connectionString, 
            string dataPath)
        {
            string fileName = ConnectionStringUtility.GetToken(connectionString, "FileName", "");
            if (!Path.IsPathRooted(fileName))
            {
                fileName = Path.Combine(dataPath, fileName);
            }
            SpreadsheetGearSeries rval = new SpreadsheetGearSeries(
                fileName,
                ConnectionStringUtility.GetToken(connectionString, "SheetName", ""),
                ConnectionStringUtility.GetToken(connectionString, "DateColumn", ""),
                ConnectionStringUtility.GetToken(connectionString, "ValueColumn", ""),
                ConnectionStringUtility.GetBoolean(connectionString, "IsWaterYearFormat", false),
                ConnectionStringUtility.GetToken(connectionString,"SiteColumn",""),
                ConnectionStringUtility.GetToken(connectionString,"SiteFilter","")
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
                SpreadsheetGearSeries g = SpreadsheetGearSeries.CreateFromConnectionString(ConnectionString, dir);
                g.Read();
                //m_db.Truncate(ID);
                ConnectionString = g.ConnectionString;
                ConnectionString = ConnectionStringUtility.MakeFileNameRelative(ConnectionString, m_db.DataSource);
                m_db.SaveProperties(this);// LastWriteTime proabably changed
                m_db.SaveTimeSeriesTable(ID, g, DatabaseSaveOptions.DeleteAllExisting);
            }
        }

    }
}
