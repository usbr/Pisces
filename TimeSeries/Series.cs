using System;
using System.IO;
using System.Text;
using System.Collections.Specialized;
using System.Data;
using System.Diagnostics;
using Reclamation.Core;
using System.Collections.Generic;
using SeriesCatalogRow = Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow;
using System.Text.RegularExpressions;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// Intervals for a Series
    /// </summary>
    public enum TimeInterval
    {
        Hourly,
        Daily,
        Monthly,
        Weekly,
        Yearly,
        Irregular
    }
    /// <summary>
    /// Series is the base class for all TimeSeries data.
    /// A derived series can 'depend' on another series for calculations such as Daily Average.
    /// 
    /// </summary>
    public partial class Series : PiscesObject, IEnumerable<Point>, IEquatable<Series>
    {

        private DataTable table;
        private TimeSeriesMessageList _messages; // used to display info such as data skipped during import
        private bool _readOnly;
        private bool _hasFlags;
        private bool _hasPercent;
        private TimeSeriesAppearance _appearance;
        private int _missingRecordCount = 0;
        private string m_flagColumnName = "flag";
        private int m_valueColumnIndex = 1;
        public string State {get; set;}

        /// <summary>
        /// Number of records missing
        /// </summary>
        public int CountMissing()
        {
            _missingRecordCount = 0;
            foreach (var pt in this)
            {
                if (pt.IsMissing)
                    _missingRecordCount++;

            }
            return _missingRecordCount;
        }

        /// <summary>
        /// Series of all missing records
        /// </summary>
        public Series GetMissing()
        {
            var rval = this.Clone();
            _missingRecordCount = 0;
            foreach (var pt in this)
            {
                if (pt.IsMissing)
                    rval.Add(pt);
            }
            return rval;
        }
    
        
        
        public string ScenarioName = "";
        //public string ConnectionString=""; // example: usgs url to data , or database connection string or modsim xy file name.
        public static string DateTimeFormatDailyAverage = "yyyy-MM-dd";
        public static string DateTimeFormatInstantaneous = "yyyy-MM-dd HH:mm:ss.ff";
        public static string DateTimeFormatMonthly = "yyyy MMM";
        private bool m_externalDataSource = false;

       

        /// <summary>
        /// true if data is stored external from TimeSeriesDatabase
        /// and you don't want anything saved in the database
        /// </summary>
        public bool ExternalDataSource
        {
            get { return m_externalDataSource; }

            set { m_externalDataSource = value; }
        }

        /// <summary>
        /// Determines Period of record by quering the local database.
        /// Series with external data (ExternalDataSource = true) should override this Method
        /// If you use the Update Feature you also need to override this method
        /// </summary>
        /// <returns></returns>
        public virtual PeriodOfRecord GetPeriodOfRecord() {

            if (m_db == null)
                return new PeriodOfRecord(TimeSeriesDatabase.MinDateTime, TimeSeriesDatabase.MaxDateTime, 0);
            return m_db.GetPeriodOfRecord(this.ID);
        }


        /// <summary>
        /// Reloads and appends new data from external data source.
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        public void Update(DateTime t1, DateTime t2)
        {
            UpdateCore(t1, t2, false);
        }
        /// <summary>
        /// Reloads and appends new data from from the derived Series source 
        /// overwriting any database data within the time range specified
        /// 
        /// Override the CreateFromConnectionString() method for 
        /// classes inheriting from Series if you want to store data in the database
        /// </summary>
        /// <param name="t1">beginning date/time</param>
        /// <param name="t2">ending date/time</param>
        /// <param name="minimal">if true only updates data not present in the local database</param>
        protected virtual void UpdateCore(DateTime t1, DateTime t2, bool minimal)
        {
            if (m_db != null)
            {
                if (minimal) // used for AutoUpdate feature for Hydromet, USGS, snotel
                {
                    MinimalUpdate(t1, t2);
                }
                else
                {
                    Series s = CreateFromConnectionString();
                    s.ReadCore(t1, t2);
                    if (s.Count == 0)
                    {
                        Logger.WriteLine("Update did not find any data");
                        return;
                    }
                    m_db.SaveTimeSeriesTable(ID, s, DatabaseSaveOptions.UpdateExisting);
                }
            }
            else
            {
                Logger.WriteLine("Update() only works on series stored in the database.");
            }
        }

        protected void MinimalUpdate( DateTime t1,  DateTime t2)
        {
            PeriodOfRecord sourceRange = this.GetPeriodOfRecord();
            PeriodOfRecord databaseRange = m_db.GetPeriodOfRecord(this.ID, t1, t2);
            if (sourceRange.Count == 0 || databaseRange.Count == 0) // period of record is undefined if nothing is in database.
            {
                Update(t1, t2);
            }
            else
            if (sourceRange.InRange(t1) && t2 > sourceRange.T2) // case 1:  use head of cache.
            { // query data that is not in cache.
                Logger.WriteLine("using head of cache ");
                Update(sourceRange.T2, t2);
            }
            else if (sourceRange.InRange(t2) && t1 < sourceRange.T1) // case 2: use tail of cache.
            {
                Logger.WriteLine("using tail of cache " );
                Update(t1, sourceRange.T1);
            }
            else
                if (sourceRange.InRange(t1) && sourceRange.InRange(t2)) // case 3: everything is in cache
                {
                    Logger.WriteLine("everything is in the cache");
                }
                else if (t2 > sourceRange.T2 && t1 < sourceRange.T1) // case 4:  spanning the cache... combine case 1 and 2 
                {
                    Logger.WriteLine("Query spans the cache, performing two queries " );
                    Update(sourceRange.T2, t2);
                    Update(t1, sourceRange.T1);

                }
                else
                { // cache does not have any data we need.
                    Logger.WriteLine("nothing in cache. everything comes from external source");
                    Update(t1, t2);
                }
        }

        /// <summary>
        /// Override this method to support the Update() feature
        /// </summary>
        /// <returns></returns>
        protected virtual Series CreateFromConnectionString()
        {
            Logger.WriteLine("Warning: This series could not be created from the connection string");
            Logger.WriteLine(this.Name);
            return new Series();
        }

        /// <summary>
        /// Usings this Series (from tree) as a starting point 
        /// create a new Series from another scenario
        /// </summary>
        /// <param name="scenarioName"></param>
        /// <returns></returns>
        public virtual Series CreateScenario(TimeSeriesDatabaseDataSet.ScenarioRow scenario)
        {
            
            return this;
        }

        public virtual Series CreateBaseline()
        {
            return this;
        }
        /// <summary>
        /// Converts String to SeriesType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TimeInterval TypeFromString(string type)
        {
            if (String.Compare(type, "Instantaneous", true) == 0)
            {
                return TimeInterval.Irregular;
            }
            if (String.Compare(type, "DailyAverage", true) == 0)
            {
                return TimeInterval.Daily;
            }
            TimeInterval rval = (TimeInterval)System.Enum.Parse(typeof(TimeInterval), type);
            return rval;
        }

        #region  Constructor Stuff

        protected TimeSeriesDatabase m_db;

        public TimeSeriesDatabase TimeSeriesDatabase
        {
            get { return m_db; }
            set { m_db = value; }
        }

        /// <summary>
        /// Used by CalculationSeries
        /// </summary>
        public TimeSeriesDatabaseDataSet.seriespropertiesDataTable Properties 
        {
            get
            {
                if (m_db == null)
                    return new TimeSeriesDatabaseDataSet.seriespropertiesDataTable();

                return m_db.GetSeriesProperties(true);
            }
        }

        public Series(TimeSeriesDatabase db, SeriesCatalogRow sr):base(sr) // 23 references
        {
            if (db == null)
                throw new ArgumentNullException("TimeSeriesDatabase cannot be null");
            m_db = db;
             
            InitTimeSeries(null,sr.Units,TimeIntervalFromString(sr.TimeInterval), false);
            Appearance.LegendText = Name;
            this.table.TableName = sr.TableName;
        }

        /// <summary>
        /// construct time series from DataTable
        /// </summary>
        public Series(DataTable table, string units, // 16 references
          TimeInterval tsType)
        {
            InitTimeSeries(table, units, tsType, false, true,true);
        }
        public Series(string name="") // 58 references
        {
            InitTimeSeries(null, "", TimeInterval.Irregular, false, true,true);
            if (name != "")
            {
                Name = name;
                table.TableName = name;
            }
        }
        public Series(string units, TimeInterval tsType) // 5 references
        {
            InitTimeSeries(null, units, tsType, true);
        }

        protected void InitTimeSeries(DataTable table, string units, TimeInterval tsType,
            bool readOnly)
        {
            TimeSeriesAppearance appearance = new TimeSeriesAppearance();
            InitTimeSeries(table, units, tsType, readOnly, appearance);
        }
        protected void InitTimeSeries(DataTable table, string units, TimeInterval tsType, 
            bool readOnly, TimeSeriesAppearance appearance)
        {
            bool hasFlags = TableHasFlags(table, m_flagColumnName);
            InitTimeSeries(table, units, tsType, readOnly, hasFlags, true);
            this.Appearance = appearance;
        }

        private static bool TableHasFlags(DataTable table,string flagColumnName)
        {
            bool hasFlags = true;
            if (table != null)
            {
                hasFlags = (table.Columns.IndexOf(flagColumnName) >= 0);
            }
            return hasFlags;
        }
        private static bool TableHasPercent(DataTable table)
        {
            bool hasPercent = false;
            if (table != null)
            {
                if (table.Columns.IndexOf("percent") >= 0)
                    hasPercent = true;
            }
            return hasPercent;
        }

        public TimeInterval TimeInterval
        {
            get
            {
             return TimeSeriesDatabase.TimeIntervalFromString(base.SeriesCatalogRow.TimeInterval);
            }
            set {
                base.SeriesCatalogRow.TimeInterval = value.ToString();
                }
        }
	
        protected void InitTimeSeries(DataTable table, string units, TimeInterval tsType,
            bool readOnly, bool hasFlags, bool hasConstraints)
        {
           
            State = "";
//            Expression = "";
            _readOnly = readOnly;
            this.TimeInterval = tsType;
            this._hasFlags = hasFlags;
            this.Units = units;
            this.HasConstraints = hasConstraints;
            if (table == null)
            {
                this.table = new DataTable("ts");
                this.table.Columns.Add("datetime", typeof(DateTime));
                this.table.Columns.Add("value", typeof(double));
            }
            else
            {
                this.table = table;
            }
            if (hasFlags)
            { 
                CreateFlagColumn();
            }
            this._hasPercent = TableHasPercent(this.table);
            // primary key should be on first column (examples: Date,tmstp,DateTime)
            if (_hasConstraints)
            {
                this.table.PrimaryKey = new DataColumn[] { this.table.Columns[0] };
            }
            this.Appearance = new TimeSeriesAppearance();
            this._messages = new TimeSeriesMessageList();
            this.table.DefaultView.Sort = this.Table.Columns[0].ColumnName;
            this.table.DefaultView.ApplyDefaultSort = true;
            this.table.Columns.CollectionChanged += new System.ComponentModel.CollectionChangeEventHandler(Columns_CollectionChanged);
        }
        
        void Columns_CollectionChanged(object sender, System.ComponentModel.CollectionChangeEventArgs e)
        {
            this._hasPercent = TableHasPercent(this.table);
            this._hasFlags = TableHasFlags(this.table,m_flagColumnName);
        }

        private void CreateFlagColumn()
        {
            if (_hasFlags )
            {
                if( !this.table.Columns.Contains(m_flagColumnName))
                {
                this.table.Columns.Add(m_flagColumnName);//, typeof(int));
                this.table.Columns[m_flagColumnName].DefaultValue = "";
                }
            }
            //else
            //{
            //    if (this.table.Columns.Contains("flag"))
            //    {
            //        table.Columns.Remove("flag");
            //    }
            //}
        }
        #endregion

        /// <summary>
        /// gets or sets if underlying
        /// DataTable has a Flag Column
        /// </summary>
        public bool HasFlags
        {
            get { return _hasFlags; }
            set { 
                _hasFlags = value;
                CreateFlagColumn();
            }
        }

        private bool _hasConstraints;
        public bool HasConstraints
        {
            get { return _hasConstraints; }
            set {
                _hasConstraints = value; 
                if( !_hasConstraints && table != null)
                {
                    table.PrimaryKey = new DataColumn[] { };
                }
            }
        }
	
        /// <summary>
        /// get or set HasPercent.
        /// Has percent is true if underlying DataTable has a percent column
        /// </summary>
        public bool HasPercent
        {
            get { return _hasPercent; }
            set { _hasPercent = value; }
        }

        public static Series operator +(Series a, Series b)
        {
            return Math.Add(a, b);
        }

        public static Series operator +(Series a, double d)
        {
            return Math.Add(a, d);
        }

        public static Series operator -(Series s)
        {
            Series rval = s.Copy();
            Math.Multiply(rval, -1);
            return rval;
        }

        public static Series operator -(Series a, Series b)
        {
            return Math.Subtract(a, b);
        }

        public static Series operator -(Series a, double d)
        {
            return Math.Subtract(a, d);
        }

        public static Series operator *(Series s, double d)
        {
            Series a = s.Copy();
            Math.Multiply(a, d);
            return a;
        }

        public static Series operator *(Series a, Series b)
        {
         return   Math.Multiply(a, b);
        }


        public static Series operator /(Series s, double d)
        {
            Series a = s.Copy();
            Math.Multiply(a, 1.0/d);
            return a;
        }

        public static Series operator /(Series a, Series b)
        {
            return Math.Divide(a, b);
        }
        /// <summary>
        /// reads full period of record from your database
        /// </summary>
        public void Read()
        {
            //Performance perf = new Performance();
            ReadCore();
            //perf.Report("read() command ",true);
        }


        /// <summary>
        /// Reads one month of data
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        public void Read(int year, int month)
        {
            DateTime t1 = new DateTime(year, month, 1);
            DateTime t2 = t1.EndOfMonth();
            Read(t1, t2);
        }

        /// <summary>
        /// Reads a range of data to nearest month
        /// </summary>
        /// <param name="year1"></param>
        /// <param name="month1"></param>
        /// <param name="year2"></param>
        /// <param name="month2"></param>
        public void Read(int year1, int month1, int year2, int month2)
        {
            DateTime t1 = new DateTime(year1, month1, 1);
            DateTime t2 = new DateTime(year2, month2, DateTime.DaysInMonth(year2, month2));
            Read(t1, t2);
        }

        /// <summary>
        /// Reads time series using specified dates 
        /// </summary>
        public void Read(DateTime t1, DateTime t2)
        {
            ReadCore(t1, t2);


            ExportSeriesToDatabase();
        }

        private void ExportSeriesToDatabase()
        {
            // hack to cache all monthly data being process.ed
            if (WindowsUtility.GetShortUserName().ToLower() == "ktarbetxss")
            {// 

                if (this is Hydromet.HydrometMonthlySeries)
                {
                    Hydromet.HydrometMonthlySeries s = this as Hydromet.HydrometMonthlySeries;
                    //File.AppendAllText(@"c:\temp\mpoll_forecast.txt", s.Cbtt + "," + s.Pcode + "\r\n");

                  

                    var fn = @"c:\temp\mpollfeb2013.db";

                    SQLiteServer svr = new SQLiteServer("Data Source=" + fn + ";");
                    var db = new TimeSeriesDatabase(svr);
                    string tn = s.Cbtt + "_" + s.Pcode;
                    
                 
                    s.Provider = "Series"; // drop conection to hydromet

                    db.ImportSeriesUsingTableName(s, true,s.Cbtt);

                }
            }
        }


        ///// <summary>
        ///// not used... YET!!
        ///// </summary>
        ///// <param name="window"></param>
        //public void Read(TimeWindow window)
        //{
        //    ReadCore(window.T1, window.T2);
        //}

        public void Read(DateTime t1, DateTime t2, bool computeDailyAvearge)
        { // ephrata only... need to do something better, and handle exceedance, and other stuff while
           // you are at it.
            ReadCore(t1, t2, computeDailyAvearge);

        }

        protected virtual void ReadCore(DateTime t1, DateTime t2, bool computeDailyAvearge)
        { // Used by Hydrography (Ephrata)
            if (computeDailyAvearge)
            {
                this.TimeInterval = TimeInterval.Daily;
            }
        }

        protected virtual void ReadCore(DateTime t1, DateTime t2)
        {
            if (m_db != null)
            {
                Table = m_db.ReadTimeSeriesTable(ID, t1, t2);

                
            }
        }


        protected virtual void ReadCore()
        {
            ReadCore(TimeSeriesDatabase.MinDateTime, TimeSeriesDatabase.MaxDateTime);
        }


        public virtual Series Exceedance(DateTime t1, DateTime t2,
            MonthDayRange range, RankType sortType)
        {
            if (m_db != null)
            {
               DataTable tbl  = m_db.Server.ExceedanceTable(this.Table.TableName, t1, t2, range, sortType);
               Series s = new Series(tbl, Units,TimeInterval);
               s.ReadOnly = true;
               return s;
            }
            else
            {
                this.Read(t1, t2);
                Series s = Math.Subset(this, range);
                return Math.Sort(s, sortType);
            }
        }


      

        /// <summary>
        /// Clear all data from Series
        /// </summary>
        public virtual void Clear()
        {
            _missingRecordCount = 0;
            Messages.Clear();
            if (this.table != null)
            {
                this.table.Rows.Clear();
                this.table.AcceptChanges();
            }
        }
        /// <summary>
        /// Creates a deep (data and structure) copy of the time series.
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual Series Copy()
        {
            Series s = new Series(this.table.Copy(), this.Units, this.TimeInterval); //,this.ReadOnly,this.HasFlags,this.HasConstraints);
            CopyAttributes(this, s);
            return s;
        }

        /// <summary>
        /// Copies basic structure (series points are NOT copied)
        /// </summary>
        /// <returns></returns>
        public Series Clone()
        {
            Series s = new Series(null, this.Units, this.TimeInterval);
            CopyAttributes(this, s);
            return s;
        }

        /// <summary>
        /// copies attributes such as units,type,scenairoName,and apperance
        /// </summary>
        public static void CopyAttributes(Series src, Series dest)
        {
            dest.Name = src.Name;
            dest.Units = src.Units;
            dest.TimeInterval = src.TimeInterval;
            dest.HasConstraints = src.HasConstraints;
            dest.HasFlags = src.HasFlags;
            //dest._missingRecordCount = src._missingRecordCount;
            dest.Appearance = src.Appearance.Copy();
            dest.Provider = src.Provider;
            dest.ReadOnly = src.ReadOnly;
            dest.ScenarioName = src.ScenarioName;
        }
        public static void CopyMessages(Series src,Series dest )
        {
            foreach (string msg in src.Messages)
            {
                dest.Messages.Add(msg);
            }
                
        }

        /// <summary>
        /// Saves time series data and properties.
        /// </summary>
        public virtual void Save()
        {
            if (m_db == null)
            {
                Logger.WriteLine("Can't save. the database is not defined");
                return;
            }
            m_db.Server.SaveTable(this.Table);
        }

       /// <summary>
       /// Removes a point from the Series
       /// </summary>
       /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            //if (ReadOnly)
            //{
            //    return;
            //}

            // TO DO.  _missingRecordCount--; if flagged missing
            table.Rows[index].Delete();
            table.AcceptChanges();
            
        }

        /// <summary>
        /// true if the underlying DataRow has changed at this index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool HasPointChanged(int index)
        {
            if (this.DataView[index].Row.RowState == DataRowState.Unchanged)
                return false;

            return true;
        }

        /// <summary>
        /// Deletes points inside the selection
        /// </summary>
        /// <param name="selected"></param>
        public int Delete(Selection selected)
        {
            if (ReadOnly)
            {
                Logger.WriteLine("can't delete read only series");
                return 0;
            }
            string datecolName = table.Columns[0].ColumnName;
            string valcolName = table.Columns[m_valueColumnIndex].ColumnName;
            string sql = datecolName + "  >= '"
              + selected.t1.ToString(Series.DateTimeFormatInstantaneous) + "'"
              + " and " + datecolName + " <= '"
              + selected.t2.ToString(Series.DateTimeFormatInstantaneous) + "'"
              + " and " + valcolName + " >= " + selected.ymin
              + " and " + valcolName + " <= " + selected.ymax;
            Logger.WriteLine(sql);
            DataRow[] rows = this.table.Select(sql, "", DataViewRowState.CurrentRows);

            int rval = rows.Length;
            // for large deletes how do we avoid excessive updates..

            for (int i = 0; i < rows.Length; i++)
            {
                rows[i].Delete();
            }
            return rval;
        }

        /// <summary>
        /// Adds a new point to the series
        /// </summary>
        /// <param name="t"></param>
        /// <param name="val"></param>
        /// <param name="flag"></param>
        public void Add(DateTime t, double val, string flag)
        {


            // TO DO: if the underlying table supports multiple tables 
            // need to use table.NewRow() and column indexes for data and date and flag,
            object o = val;
            if (val == Point.MissingValueFlag || flag == PointFlag.Missing)
            {
                _missingRecordCount++;
                o = DBNull.Value;
            }

            
            
            if (HasFlags)
            {
                this.table.Rows.Add(new object[] { t, o, flag });
            }
            else
            {
                this.table.Rows.Add(new object[] { t, o });
            }
        }
        
       
        /// <summary>
        /// Adds a new point to the series
        /// </summary>
        /// <param name="t"></param>
        /// <param name="val"></param>
        public void Add(DateTime t, double val)
        {
            Add(t, val, "");
        }

        public void Add(string date, double value)
        {
            Add(DateTime.Parse(date), value);
        }
        
        /// <summary>
        /// Adds all point from the series
        /// </summary>
        /// <param name="s"></param>
        public void Add(Series s)
        {
            for (int i = 0; i < s.Count; i++)
            {
                Add(s[i]);
            }
        }

        /// <summary>
        /// Add placeholder point for missing data
        /// </summary>
        /// <param name="t"></param>
        public void AddMissing(DateTime t)
        {
            Add(new Point(t, Point.MissingValueFlag, PointFlag.Missing));
        }

        /// <summary>
        /// Add Point
        /// </summary>
        /// <param name="point"></param>
        public void Add(Point point)
        {
            object o = point.Value;

            if (point.IsMissing) // point.Flag == PointFlag.Missing || point.Value == Point.MissingValueFlag)
            {
                _missingRecordCount++;
                o = DBNull.Value;
            }

            DataRow row = table.NewRow();

            row[0] = point.DateTime;
            row[m_valueColumnIndex] = o;

            if (HasFlags)
            {
                row[m_flagColumnName] = point.Flag;
            }

            if (HasPercent)
            {
                row["Percent"] = point.Percent;
            }
            table.Rows.Add(row);
        }

        /// <summary>
        /// Inserts all points from another series
        /// </summary>
        /// <param name="series"></param>
        public void Insert(Series series)
        {
            Insert(series, false);
        }

        /// <summary>
        /// Inserts all points from another series
        /// </summary>
        /// <param name="series"></param>
        public void Insert(Series series, bool overWrite)
        {
            for (int i = 0; i < series.Count; i++)
            {
                Insert(series[i], overWrite);
            }

        }

        /// <summary>
        /// Insert Point
        /// </summary>
        /// <param name="point"></param>
        public void Insert(Point point)
        {
            Insert(point, false);
        }
        /// <summary>
        /// Insert timeSeriesPoint in order based on date and time.
        /// If the point allready exists it may be 
        /// overwritten or ignored depending on the overWrite flag
        /// </summary>
        public void Insert(Point point, bool overWrite)
        {
            DateTime insertDate = point.DateTime;
            int sz = Count;
            if (sz == 0)
            {
                //InsertAt(point,0);
                Add(point);
                return;
            }
            if (insertDate > this.MaxDateTime)
            { // append this point
                Add(point);
                return;
            }
            int idx = this.LookupIndex(point.DateTime); // LookupIndex() is not exact

            if (idx >= 0 && this[idx].DateTime == point.DateTime)
            { 
                if (overWrite)
                {
                    Messages.Add("replaced " + this[idx] + " with " + point);
                    this[idx] = point;
                }
                else
                {
                    Messages.Add("skipped inserting " + this[idx] + " that date and time is allready being used");
                }
                
                return;
            }


            for (int i = 0; i < sz; i++)
            {// find location to insert.
                Point pt = this[i];
                if (pt.DateTime >= insertDate)
                {
                    if (pt.DateTime == insertDate)
                    {
                        if (!overWrite)
                        {
                            Messages.Add("skipped inserting " + pt + " that date and time is allready being used");
                            return;
                        }
                        Messages.Add("replaced " + pt + " with " + point);
                        pt.DateTime = point.DateTime;
                        pt.Value = point.Value;
                        pt.Flag = point.Flag;
                        pt.Percent = point.Percent;
                        this[i] = pt;
                        return;
                    }
                    InsertAt(point, i);
                    return;
                }
            }
        }

        /// <summary>
        /// Inserts a Point at position
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="pos"></param>
        public void InsertAt(Point pt, int pos)
        {
            DataRow row = this.table.NewRow();
            row[0] = pt.DateTime;
            row[m_valueColumnIndex] = pt.Value;
            if (pt.Flag == PointFlag.Missing || pt.Value == Point.MissingValueFlag)
                _missingRecordCount++;

            if (HasFlags)
            {
                row[m_flagColumnName] = pt.Flag;
            }
            // might throw primary key violation!
            if (HasPercent)
            {
                row["percent"] = pt.Percent;
            }
            this.table.Rows.InsertAt(row, pos);
        }

        #region Properties


        /// <summary>
        /// List of messages, such as data skipped during import
        /// </summary>
        public TimeSeriesMessageList Messages
        {
            get { return this._messages; }
        }
        public TimeSeriesAppearance Appearance
        {
            get { return this._appearance; }
            set { this._appearance = value; }
        }

       
        public static TimeInterval TimeIntervalFromString(string interval)
        {
            if (TimeInterval.Daily.ToString() == interval) return TimeInterval.Daily;
            if (TimeInterval.Monthly.ToString() == interval) return TimeInterval.Monthly;
            if (TimeInterval.Hourly.ToString() == interval) return TimeInterval.Hourly;
            if (TimeInterval.Yearly.ToString() == interval) return TimeInterval.Yearly;
            if (TimeInterval.Weekly.ToString() == interval) return TimeInterval.Weekly;
            return TimeInterval.Irregular;
        }

       



        public bool ReadOnly
        {
            set { this._readOnly = value; }
            get
            {
                return this._readOnly;
            }
        }

        //public string DisplayUnits { get; set; }
        //public string Notes = "";

       

        /// <summary>
        /// returns reference to underlying DataView holding the time series data,
        /// </summary>
        internal DataView DataView
        {
            get { return this.table.DefaultView; }
        }

        public DataTable Table
        {
            get { return this.table; }
            set
            {
                if (!value.Equals(this.table))
                {
                    value.Columns.CollectionChanged += new System.ComponentModel.CollectionChangeEventHandler(Columns_CollectionChanged);
                }
                this.table = value;

                if (this.table.Columns.Count < 2)
                {
                    throw new Exception("table must have at least two columns Date,value ");
                }

                if (this.table.Columns[0].DataType.ToString() != "System.DateTime")
                {
                    // Console.WriteLine(table.Columns[0].DataType.ToString());
                    throw new Exception("first column in table must be  a DateTime type ");
                }

                if (this.table.DefaultView.Sort == "")
                {
                    this.table.DefaultView.Sort = this.Table.Columns[0].ColumnName;
                    this.table.DefaultView.ApplyDefaultSort = true;
                }
                this._hasPercent = TableHasPercent(this.table);
                this._hasFlags = TableHasFlags(this.table, m_flagColumnName);

                //if (this.table.Columns[1].DataType.ToString() != "System.Double")
                //{
                //    // Console.WriteLine(table.Columns[0].DataType.ToString());
                //    throw new Exception("second column in table must be double");
                //}

            }
        }

        /// <summary>
        /// Count of points in this series
        /// </summary>
        public int Count
        {
            get
            {
                if (table == null)
                    return 0;
                return this.table.DefaultView.Count;
            }

        }

        /// <summary>
        /// DateTime of first entry in series
        /// if no entrys returns DateTime.MaxValue
        /// </summary>
        public DateTime MinDateTime
        {
            get
            {
                DateTime rval = DateTime.MaxValue;
                if (this.Count > 0)
                {
                    rval = this[0].DateTime;
                }
                return rval;
            }
        }
        /// <summary>
        /// DateTime of last entry in series
        /// if no entries returns DateTime.MinValue
        /// </summary>
        public DateTime MaxDateTime
        {
            get
            {
                DateTime rval = DateTime.MinValue;
                if (Count > 0)
                {
                    rval = this[Count - 1].DateTime;
                }
                return rval;
            }
        }


        public Point this[DateTime t]
        {
            get
            {
                int idx = IndexOf(t);
                return this[idx];
            }
            set
            {
                int idx = IndexOf(t);
                this[idx] = value;
            }
        }

        public Point this[string date]
        {
            get
            {
                int idx = IndexOf(DateTime.Parse(date));
                return this[idx];
            }
            set
            {
                int idx = IndexOf(DateTime.Parse(date));
                this[idx] = value;
            }
        }


        /// <summary>
        ///  DataRow returns a reference to underlying DataRow
        /// </summary>
        public DataRow DataRow(int index)
        {
            return table.DefaultView[index].Row;
        }


        public Point this[int index]
        {
            get
            {
                DataRowView row = table.DefaultView[index];
                Point pt = new Point();
                pt.DateTime = (DateTime)row[0];
                if (row[m_valueColumnIndex] == DBNull.Value)
                {
                    pt.Value = Point.MissingValueFlag;
                }
                else
                {
                    pt.Value = Convert.ToDouble(row[m_valueColumnIndex]);
                }

                string flag = "";

                if (_hasFlags && row[m_flagColumnName] != DBNull.Value)
                {
                    flag =Convert.ToString(row[m_flagColumnName]);
                }
                pt.Flag = flag;

                if (_hasPercent && row["percent"] != DBNull.Value)
                {
                    pt.Percent = Convert.ToDouble(row["percent"]);
                }

                //if (_hasNotes && row["notes"] != DBNull.Value)
                //{
                //    pt.Notes = row["notes"].ToString();
                //}

                return pt;
            }
            set
            {
                DataRowView row = table.DefaultView[index];
                row[0] = value.DateTime;
                row[m_valueColumnIndex] = value.Value;
                if (_hasFlags) 
                {
                    row[m_flagColumnName] = value.Flag;
                }
                if (_hasPercent)
                {
                    row["percent"] = value.Percent;
                }
                //if (_hasNotes)
                //{
                //    row["Notes"] = value.Notes;
                //}
            }
        }

        /// <summary>
        /// gets number of points that have been modifed
        /// </summary>
        public int ModifiedCount
        {
            get
            {
                return this.table.Select("", "", DataViewRowState.ModifiedCurrent).Length;
            }
        }

        public int DeletedCount
        {
            get
            {
                return this.table.Select("", "", DataViewRowState.Deleted).Length;
            }
        }

        public int InsertedCount
        {
            get
            {
                return this.table.Select("", "", DataViewRowState.Added).Length;
            }
        }
        #endregion





        /// <summary>
        /// determines if a given DateTime is within
        /// this time series.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool WithinRange(DateTime t)
        {
            if (Count == 0)
            {
                return false;
            }
            DateTime min = this[0].DateTime;
            DateTime max = this[Count - 1].DateTime;

            if (t >= min && t <= max)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Looks for exact index
        /// </summary>
        /// <param name="t"></param>
        /// <returns>index or -1 when exact match not found</returns>
        public int IndexOf(DateTime t)
        {
            // performance of Find as apposed to sequencial
            //Here are some test results:
            // sequencial :  22 minutes
            // binarySearch():  10 minutes
            //  Find()    :   4 minutes
            return DataView.Find(t);
            //return BinarySearch(t);
        }

        /// <summary>
        /// Finds t starting at specified index
        /// note: IndexOf(t) is proabably faster..
        /// </summary>
        /// <param name="t"></param>
        /// <param name="startingIndex"></param>
        /// <returns></returns>
        private int IndexOf(DateTime t, int startingIndex)
        {
            for (int i = startingIndex; i < Count; i++)
            {
                if (this[i].DateTime == t)
                {
                    return i;
                }
                if (this[i].DateTime > t)
                {
                    return -1;
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns value at date specified or next greater date in series.
        /// </summary>
        /// <param name="t">DateTime to search for</param>
        /// <returns></returns>
        public double Lookup(DateTime t)
        {
            int idx = LookupIndex(t);
            if (idx == -1 || idx >= Count)
            {
                Console.WriteLine("Error:  idx = " + idx);
                throw new IndexOutOfRangeException("date does not exist in time series");
            }
            return this[idx].Value;
        }

        public int LookupIndex(DateTime t,bool findNearest)
        {
            return BinaryLookupIndex(t, findNearest);
        }
        /// <summary>
        /// Returns approximate index to a date in a sorted time series.
        /// If the time series does not have the exact date the next
        /// greater date will be matched
        /// </summary>
        public int LookupIndex(DateTime t)
        {
            return BinaryLookupIndex(t,false);
        }

        /// <summary>
        /// Find index in this series exactly (first try) or close to DateTime t
        /// </summary>
        /// <param name="t"></param>
        /// <param name="findNearest">if true search for nearest index else return the index with DateTime greater than t</param>
        /// <returns>index to t or first index greater than t. returns -1 on failure</returns>
        private int BinaryLookupIndex(DateTime t, bool findNearest)
        {
            int index = -1;
            int firstIndex = 0;
            int lastIndex = Count - 1;
            while (1 != 0)
            {
                if (firstIndex > lastIndex)
                {
                    index = -1;
                    break;
                }
                int key = (int)System.Math.Round((double)(firstIndex + (((double)(lastIndex - firstIndex)) / 2)));
                //Console.WriteLine("new key = "+key);
                if ((DateTime)DataView[key][0] == t)
                {
                    index = key;
                    break;
                }
                if ((DateTime)DataView[key][0] > t)
                {
                    lastIndex = key - 1;
                }
                else
                {
                    firstIndex = key + 1;
                }
            }

            
            //Console.WriteLine("lastIndex = "+lastIndex);
            //Console.WriteLine("firstIndex = "+firstIndex);
            int first = System.Math.Max(System.Math.Min(firstIndex, lastIndex), 0);
            int last = System.Math.Max(System.Math.Max(firstIndex, lastIndex), 0);
            if (first >= Count || last >= Count)
                return -1;

            for (int i = first; i <= last; i++)
            {
                DateTime date = (DateTime)this.DataView[i][0];
                if (date == t)
                {
                    return i;
                }
                if (date > t && findNearest && i > 0)
                {
                    DateTime prev = (DateTime)this.DataView[i-1][0];
                    if (System.Math.Abs(prev.Ticks - t.Ticks)
                         < System.Math.Abs(date.Ticks - t.Ticks))
                    {
                        return i - 1;
                    }
                    return i;
                }
                else
                    if (date > t)
                    {

                        return i;
                    }

                
            }

            return -1;// not found

        }
        //private int BinarySearch(DateTime t)
        //{
        //    int index = -1;
        //    int firstIndex = 0;
        //    int lastIndex = Count - 1;
        //    while (1 != 0)
        //    {
        //        if (firstIndex > lastIndex)
        //        {
        //            index = -1;
        //            break;
        //        }
        //        int key = (int)System.Math.Round((double)(firstIndex + (((double)(lastIndex - firstIndex)) / 2)));
        //        if ((DateTime)DataView[key][0] == t)
        //        {
        //            index = key;
        //            break;
        //        }
        //        if ((DateTime)DataView[key][0] > t)
        //        {
        //            lastIndex = key - 1;
        //        }
        //        else
        //        {
        //            firstIndex = key + 1;
        //        }
        //    }
        //    return index;


        //}



        public void WriteToConsole()
        {
            WriteToConsole(false);
        }
        public void WriteToConsole(bool showFlag)
        {
            WriteToConsole(showFlag, false);
        }
        public void WriteToConsole(bool showFlag, bool showPercentage)
        {
            Console.WriteLine("Name: " + Name);
            Console.WriteLine("ScenarioName " + Provider);
            Console.WriteLine("TimeInterval: " + TimeInterval);
            Console.WriteLine("units:" + Units);
            for (int i = 0; i < Count; i++)
            {
                Console.WriteLine(this[i].ToString(showFlag, showPercentage));
            }
        }

        public void WriteCsv(string filename)
        {
            WriteCsv(filename, false);
        }

         public string DateTimeFormat
        {
            get
            {
                string dateFormat = Series.DateTimeFormatInstantaneous;
                if (this.TimeInterval == TimeInterval.Daily)
                {
                    dateFormat = Series.DateTimeFormatDailyAverage;
                }
                if (this.TimeInterval == TimeInterval.Monthly)
                {
                    dateFormat = Series.DateTimeFormatMonthly;
                }
                return dateFormat;
            }
        }
        /// <summary>
        /// save time series to comma seperated file.
        /// </summary>
        /// <param name="filename">filename to create</param>
        /// <param name="includeFlagColumn">true to include flag column</param>
        public void WriteCsv(string filename, bool includeFlagColumn)
        {
            StreamWriter sw = new StreamWriter(filename);
            sw.Write("Date,value");

            string fmt = DateTimeFormat;
            if (includeFlagColumn)
            {
                sw.Write(",flag");
            }
            sw.WriteLine();
            for (int i = 0; i < Count; i++)
            {
                Point pt = this[i];
                sw.Write(pt.DateTime.ToString(fmt) + ",");
                sw.Write(pt.Value);
                if (includeFlagColumn)
                {
                    sw.Write("," + pt.Flag);
                }
                sw.WriteLine();
            }

            sw.Close();
        }




       
        /// <summary>
        /// Removes all points that have a value of Point.MissingValueFlag
        /// or that are flagged bad
        /// </summary>
        /// <returns></returns>
        public int RemoveMissing()
        {
            int rval = 0;
            string valcolName = table.Columns[m_valueColumnIndex].ColumnName;

            string sql = "ISNull([" + valcolName + "]," + Point.MissingValueFlag + ") =" + Point.MissingValueFlag;

            if (HasFlags)
            {
                sql += " or  " + m_flagColumnName + " LIKE '" + PointFlag.QualityLow + "%' ";
                sql += " or  " + m_flagColumnName + " LIKE '" + PointFlag.QualityHigh + "%' ";
                sql += " or  " + m_flagColumnName + " LIKE '" + PointFlag.QualityRateOfChange + "%' ";
            }

            DataRow[] remove = table.Select(sql);
            
            foreach (DataRow row in remove)
            {
                table.Rows.Remove(row);
                rval++;
            }
            _missingRecordCount = 0;
            return rval;

        }

        public override string ToString()
        {
            return ToString(false);
        }
        public string ToString(bool allData=false)
        {

            DataView view = this.DataView;

            StringBuilder sb = new StringBuilder();

            string fmt = DateTimeFormat;
            sb.Append(Name + " ");
            sb.Append("Date\tValue");
            if (HasFlags)
            {
                sb.Append("\tflag");
            }
            sb.Append("\r\n");

            int numRows = this.Count;

            for (int i = 0; i < numRows; i++)
            {
                Point pt = this[i];
                sb.Append(pt.DateTime.ToString(fmt));
                sb.Append(" \t");
                sb.Append(pt.Value);
                if (HasFlags)
                {
                    sb.Append(" \t");
                    sb.Append(pt.Flag);
                }
                sb.Append("\r\n");
                if (i > 5 && !allData)
                {
                    sb.Append("\n...");
                    break;
                }
            }
            return sb.ToString();
        }


        public Series Subset(DateTime t1, DateTime t2)
        {
         return   Math.Subset(this, new DateRange(t1, t2));
        }
        /// <summary>
        /// Creates a new series using a query
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public Reclamation.TimeSeries.Series Subset(string sql)
        {
            Series rval = this.Clone();
            DataRow[] rows = this.table.Select(sql);

            for (int i = 0; i < rows.Length; i++)
            {
                DataRow row = rows[i];
                Point pt = new Point();
                pt.DateTime = (DateTime)row[0];
                if (row[m_valueColumnIndex] == DBNull.Value)
                {
                    pt.Value = Point.MissingValueFlag;
                }
                else
                {
                    pt.Value = Convert.ToDouble(row[m_valueColumnIndex]);
                }

                string flag = "";

                if (_hasFlags && row[m_flagColumnName] != DBNull.Value)
                {
                    flag = Convert.ToString(row[m_flagColumnName]);
                }
                pt.Flag = flag;

                if (_hasPercent && row["percent"] != DBNull.Value)
                {
                    pt.Percent = Convert.ToDouble(row["percent"]);
                }

                //if (_hasNotes && row["notes"] != DBNull.Value)
                //{
                //    pt.Notes = row["notes"].ToString();
                //}

                rval.Add(pt);
            }
            return rval;
        }

        /// <summary>
        /// Creates a series by copying Date and values
        /// from a DataTable
        /// </summary>
         public static Series SeriesFromTable(DataTable table, 
            int dateColumnIndex, int valueColumnIndex)
        {
            Series s = new Series();
            s.Name = table.Columns[valueColumnIndex].ColumnName;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                Point pt = new Point();
                pt.DateTime = Convert.ToDateTime(table.Rows[i][dateColumnIndex]);
                pt.Value = Convert.ToDouble(table.Rows[i][valueColumnIndex]);
                s.Add(pt);
            }

            return s;
        }

        public void CopyToClipboard()
        {
            CopyToClipboard(true);
        }
        public void CopyToClipboard(bool includeFlag)
        {
            DataView view = this.DataView;

            StringBuilder sb = new StringBuilder();

            string fmt = DateTimeFormat;

            sb.Append("Date\tValue");
            if (includeFlag)
            {
                sb.Append("\tflag");
            }
            sb.Append("\r\n");

            int numRows = this.Count;

            for (int i = 0; i < numRows; i++)
            {
                Point pt = this[i];
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




        ///// <summary>
        ///// Count of points flagged as Missing 'M'
        ///// </summary>
        ///// <returns></returns>
        //public int CountMissing()
        //{
        //    if (HasFlags)
        //    {
        //        DataRow[] rows = table.Select("Flag = '" + PointFlag.Missing + "'");
        //        return rows.Length;
        //    }
        //    else
        //    {
        //        return this.MissingRecordCount;
        //    }
          
        //}
       
      
        ///// <summary>
        ///// return an array of average annual values for the period of record
        ///// </summary>
        ///// <returns></returns>
        //private Point[] AnnualAverageArray()
        //{
        //    DateTime t1 = this.MinDateTime;
        //    DateTime t2 = this.MaxDateTime;
        //    return AnnualAverageArray(t1, t2);
        //}
        
        
        /// <summary>
        /// (for daily or monthly time steps)
        /// given a beginning and ending date, compute an array of average annual values
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        //private Point[] AnnualAverageArray(DateTime t1, DateTime t2)
        //{
        //    if (tsType != TimeInterval.Daily && 
        //        tsType != TimeInterval.Monthly) return null;
        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("Date", typeof(DateTime));
        //    dt.Columns.Add("value", this.table.Columns[1].DataType);
        //    int nts, nlyr, tsi;
        //    nts = nlyr = 0;
        //    DateTime date;
        //    if (tsType == TimeInterval.Monthly) nts = 12;
        //    int nyr = t2.Year - t1.Year;
        //    if (tsType == TimeInterval.Daily)
        //    {
        //        nlyr = NumLeapYears(t1, t2);
        //        if (nlyr == 0)
        //            nts = 365;
        //        else
        //            nts = 366;
        //    }
        //    double[] avg = new double[nts];
        //    Point[] rval = new Point[nts];
        //    for (int y = 0; y < nyr; y++)
        //    {
        //        date = new DateTime(y+t1.Year, t1.Month, 1);
        //        for (tsi = 0; tsi < nts; tsi++, date = IncremetDate(date))
        //        {
        //            if (TimeInterval == TimeInterval.Daily && date.Month == 2 && date.Day == 29)
        //                avg[tsi] += (Lookup(date) / nlyr);
        //            else
        //                avg[tsi] += (Lookup(date) / nyr);
        //            if (TimeInterval == TimeInterval.Daily && date.Month == 2 && date.Day == 28 && 
        //                !DateTime.IsLeapYear(date.Year)) tsi++;
        //        }
        //    }
        //    DateTime tsDate = t1;
        //    for (int ts = 0; ts < nts; ts++)
        //    {
        //        rval[ts].DateTime = tsDate;
        //        rval[ts].Value = avg[ts];
        //        tsDate = IncremetDate(tsDate);
        //    }
        //    return rval;
        //}
        /// <summary>
        /// (for Monthly or daily time step)given a date, return the next time step date
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public DateTime IncremetDate(DateTime t)
        {
            if (TimeInterval == TimeInterval.Daily) 
                return t.AddDays(1);
            if (TimeInterval == TimeInterval.Monthly)
            {
             t = t.AddMonths(1);
             return new DateTime(t.Year, t.Month, 1);//DateTime.DaysInMonth(t.Year, t.Month));
            }
            if (TimeInterval == TimeSeries.TimeInterval.Irregular)
                return t.AddMinutes(15);

            throw new InvalidOperationException(" Increment not implemented for " + TimeInterval.ToString());
        }
        ///// <summary>
        ///// return number of leap years between two dates inclusively
        ///// </summary>
        ///// <param name="t1"></param>
        ///// <param name="t2"></param>
        ///// <returns></returns>
        //private static int NumLeapYears(DateTime t1, DateTime t2)
        //{
        //    int nly = 0;
        //    int yr1 = t1.Year;
        //    if (t1.Month != 1) yr1++;
        //    int yr2 = t2.Year;
        //    for (int y = yr1; y <= yr2; y++)
        //    {
        //        if (DateTime.IsLeapYear(y)) nly++;
        //    }
        //    return nly;
        //}
        
        ///// <summary>
        ///// append years to a series; values are average of specified years
        ///// </summary>
        ///// <param name="swy1">first year of average</param>
        ///// <param name="swy2">last year of average</param>
        ///// <param name="addy1">first year to append</param>
        ///// <param name="addy2">last year to append</param>
        //private void AppendUsingAverage(DateTime s1, DateTime s2, DateTime a1, DateTime a2)
        //{
        //    Series avg = new Series();
        //    Series.CopyAttributes(this, avg);
        //    DateTime date;
        //    DataRow row;
        //    Point[] avgArray = this.AnnualAverageArray(s1, s2);
        //    int nyrs = a2.Year - a1.Year;
        //    for (int y = 0; y < nyrs; y++)
        //    {
        //        date = a1.AddYears(y);
        //        for (int ts = 0; ts < avgArray.Length; ts++)
        //        {
        //            row = avg.Table.NewRow();
        //            row[0] = date;
        //            row[1] = avgArray[ts].Value;
        //            row["Flag"] = string.Concat("average of ", s1.ToShortDateString(), " - ", s2.ToShortDateString());
        //            if (tsType == TimeInterval.Daily && DateTime.IsLeapYear(date.Year) == false &&
        //                date.Month == 2 && date.Day == 28) ts++;
        //            date = IncremetDate(date);
        //            avg.Table.Rows.Add(row);
        //        }
        //    }
        //    avg.SiteName = this.SiteName;
        //    this.Add(avg);
        //    Save();
        //}


        /// <summary>
        /// Looks at first two points in series to estimate the
        /// TimeInterval.  If there are less than two points
        /// the Interval of s is returned
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static TimeInterval EstimateInterval(Series s)
        {
            TimeInterval rval = s.TimeInterval;
            if (s.Count > 1)
            {
                TimeSpan ts = new TimeSpan(s[1].DateTime.Ticks - s[0].DateTime.Ticks);

                if (ts.TotalHours -1  < 0.01)
                {
                    return TimeInterval.Hourly;
                }

                
                if (ts.Days == 1)
                    return TimeInterval.Daily;
                else if (ts.Days == 7)
                    return TimeInterval.Weekly;
                else if (ts.Days >= 28 && ts.Days < 365)
                    return TimeInterval.Monthly;
            }

            return s.TimeInterval;
        }

        /// <summary>
        /// Get or set all values in single action
        /// This can improve performance writing to underlying DataTable
        /// </summary>
        public double[] Values
        {
            get
            {
                return DataTableUtility.Doubles(this.table, "", table.Columns[m_valueColumnIndex].ColumnName);
            }
            set
            {
                for (int i = 0; i < value.Length; i++)
                {
                    table.Rows[i][m_valueColumnIndex] = value[i];
                }
            }
        }

        /// <summary>
        /// Enforce consistent range of data for Daily Data
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        protected void NormalizeDaily(DateTime t1, DateTime t2)
        {
            if (t2 < t1)
                return;
            
            DateTime t = t1.Date;
            while (t < t2)
            {
                if (IndexOf(t) < 0)
                    AddMissing(t);

                t = t.AddDays(1).Date;
            }
        }


        /// <summary>
        /// Enforce consistent range of data, fills missing gaps with nulls
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="minutes">interval to be enforced between data points</param>
        /// </summary>
        protected void NormalizeInstant(DateTime t1, DateTime t2, int minutes)
        {
            if (t2 < t1 || minutes == 0)
                return;

            DateTime t = t1;
            while (t < t2)
            {
                if (IndexOf(t) < 0)
                    AddMissing(t);

                t = t.AddMinutes(minutes);
            }
        }


        /// <summary>
        /// Determines if flag indicates bad data
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public virtual bool IsBadData(string flag)
        {
            return false;
        }

        public IEnumerator<Point> GetEnumerator()
        {
            
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Trim(DateTime t1, DateTime t2)
        {

            while (Count > 0 && MaxDateTime > t2)
            {
                RemoveAt(Count - 1);
            }

            while (Count > 0 && MinDateTime < t1)
            {
                RemoveAt(0);
            }
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Series s = obj as Series;
            if (s == null) return false;
            else return Equals(s);
        }
        public override int GetHashCode()
        {
            return ID;
        }
        public bool Equals(Series other)
        {
            if (other == null) return false;
            return (this.ID.Equals(other.ID));
        }



        public bool IsEmpty {

            get { return Count == 0; }
             }
    }
}
