using System;
using System.Data;
using Reclamation.Core;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

using SeriesCatalogRow = Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow;
using System.Windows.Forms;
using Reclamation.TimeSeries.Parser;

namespace Reclamation.TimeSeries
{
    /// <notes>
    /// TimeSeries Database with SQL based backend
    /// fundamental design decision was to have a single table containing information to create Series.
    /// Each series is stored in a separate table.
    ///       pros: flexable, simple, efficient for space (some cases), 
    ///       performance may be better?
    ///             you can browse data from database level
    ///             this leaves level where you could redirect to file system
    ///             or multiple database, etc.
    ///       cons: more complex, need this middle tier to manage tableName
    ///             table does not have id 
    ///             need permissions to create/drop tables.
    /// </notes>

    /// <summary>
    /// Options when Saving to TimeSeries Database
    /// </summary>
    public enum DatabaseSaveOptions
    {
        /// <summary>
        /// Saves any changes made to underlying DataTable
        /// </summary>
        Save,

        /// <summary>
        /// Insert is the fastest way to save to the database
        /// No checks for conflicting Date (primary key)
        /// </summary>
        Insert,

        /// <summary>
        /// Replaces existing data
        /// </summary>
        UpdateExisting,

        /// <summary>
        /// Deletes all existing data
        /// </summary>
        DeleteAllExisting
    }



    /// <summary>
    /// Database that contains timeseries data organized in folders
    /// </summary>
    public partial class TimeSeriesDatabase
    {
        BasicDBServer m_server;
        TimeSeriesDatabaseSettings m_settings;
        SeriesExpressionParser m_parser;
        Quality m_quality;

        public Quality Quality
        {
            get
            {
                if (m_quality == null)
                    m_quality = new Quality(this);
                return m_quality;
            }
        }
        public SeriesExpressionParser Parser
        {
            get { return m_parser; }
            set { m_parser = value; }
        }
        public TimeSeriesDatabaseSettings Settings
        {
            get { return m_settings; }
            // set { m_settings = value; }
        }


        //TimeSeriesImporter m_importer;
        //public TimeSeriesImporter TimeSeriesImporter
        //{
        //    get
        //    {
        //        return m_importer;
        //    }
        //}

        /// <summary>
        /// "yyyy-MM-dd HH:mm:ss.fff";
        /// </summary>
        internal static string dateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";

        TimeSeriesFactory factory;
        //int m_tablesPerFile = 5000;

        /// <summary>
        /// january 1, 1754
        /// </summary>
        public static readonly DateTime MinDateTime = new DateTime(1754, 1, 1); //  sql compact min 1/1/1753

        /// <summary>
        /// December 31, 3999
        /// </summary>
        public static readonly DateTime MaxDateTime = DateTime.Parse("3999-12-31"); //  sql compact max 12/31/4000


        /// <summary>
        /// The factory creates Series and folder objects
        /// </summary>
        internal TimeSeriesFactory Factory
        {
            get { return factory; }
            //set { factory = value; }
        }


        /// <summary>
        /// Constructor of TimeSeriesDatabase
        /// </summary>
        /// <param name="server"></param>
        public TimeSeriesDatabase(BasicDBServer server)
        {
            InitDatabaseSettings(server);

            LookupOption lookup = LookupOption.SeriesName;

            var opt = m_settings.Get("LookupOption", "");

            if (opt == "TableName")
            {
                lookup = LookupOption.TableName;
            }

            InitWithLookup(server, lookup);
        }

        /// <summary>
        /// Constructor of TimeSeriesDatabase
        /// </summary>
        /// <param name="server"></param>
        public TimeSeriesDatabase(BasicDBServer server, LookupOption lookup = LookupOption.SeriesName)
        {
            InitDatabaseSettings(server);
            InitWithLookup(server, lookup);
        }


        private void InitWithLookup(BasicDBServer server, LookupOption lookup)
        {
            InitDatabaseSettings(server);
            m_settings.Set("LookupOption", lookup.ToString());
            m_settings.Save();

            m_parser = new SeriesExpressionParser(this, lookup);
            factory = new TimeSeriesFactory(this);
            

            SetUnixDateTime(UnixDateTime);
        }

        private void InitDatabaseSettings(BasicDBServer server)
        {
            Filter = "";
            m_server = server;

            if (m_server.TableExists("piscesinfo"))
            {
                InitSettings();
                UpgradeV1ToV2();
            }

            CreateTablesWithSQL();
            InitSettings();

            CreateRootFolder();
        }


        /// <summary>
        /// set UnixDateTime to true for disk  efficent sqlite timeseries data
        /// </summary>
        public bool UnixDateTime
        {
            get {
                return this.Settings.ReadBoolean("UnixDateTime", false);
            }
            set
            {
                SetUnixDateTime(value);

            }
        }
        /// <summary>
        /// Set UnixDateTime in connection string and piscesInfo table
        /// </summary>
        /// <param name="value"></param>
        private void SetUnixDateTime(bool value)
        {
            if (!(m_server is SQLiteServer))
                return;

            this.Settings.Set("UnixDateTime", value);
            this.Settings.Save();
            var m_unixDateTime = value;
            if (m_unixDateTime)
            {
                var cs = this.Server.ConnectionString;
                cs = ConnectionStringUtility.Modify(cs, "datetimeformat", "UnixEpoch");
                this.Server.ConnectionString = cs;
            }
            else
            {
                var cs = this.Server.ConnectionString;
                cs = ConnectionStringUtility.Modify(cs, "datetimeformat", "Default");
                this.Server.ConnectionString = cs;

            }
        }

        private void InitSettings()
        {
            m_settings = new TimeSeriesDatabaseSettings(m_server);
        }



        public TimeSeriesDatabaseDataSet.sitecatalogDataTable GetSiteCatalog()
        {
            var tbl = new TimeSeriesDatabaseDataSet.sitecatalogDataTable();
            m_server.FillTable(tbl,"select * from sitecatalog order by siteid");
            return tbl;
        }

        public TimeSeriesDatabaseDataSet.sitepropertiesDataTable GetSiteProperties()
        {

            var tbl =  new TimeSeriesDatabaseDataSet.sitepropertiesDataTable();
            tbl.ExtendedProperties.Add("datetime", DateTime.Now.ToString());

            string sql = "select * from siteproperties ";
            m_server.FillTable(tbl, sql);
            return tbl;
        }
        
        public TimeSeriesDatabaseDataSet.SeriesCatalogDataTable GetSeriesCatalog()
        {
            var tbl = new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();
            m_server.FillTable(tbl);
            return tbl;
        }

        public TimeSeriesDatabaseDataSet.SeriesCatalogDataTable GetSeriesCatalog(string filter)
        {

            var tbl = new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();
            string sql = "select * from seriescatalog where "+filter;
            m_server.FillTable(tbl,sql );
            return tbl;
        }

        TimeSeriesDatabaseDataSet.seriespropertiesDataTable m_seriesProperties = null;


        /// <summary>
        /// Manage rows from the SeriesProperties table
        /// </summary>
        /// <param name="seriesid">series id</param>
        /// <param name="useCache"></param>
        /// <returns></returns>
        public TimeSeriesDatabaseDataSet.seriespropertiesDataTable GetSeriesProperties(bool useCache=false)
        {
          if (m_seriesProperties != null &&  useCache )
             return m_seriesProperties;

            m_seriesProperties = new TimeSeriesDatabaseDataSet.seriespropertiesDataTable(this);
            m_seriesProperties.ExtendedProperties.Add("datetime", DateTime.Now.ToString());
            
            return m_seriesProperties;
        }


        public bool AnyUrgsimSeries()
        {
             var sc = GetSeriesCatalog("provider = 'UrgsimSeries'");
             return sc.Rows.Count > 0;
        }
        public bool AnyUrgwomSeries()
        {
            var sc = GetSeriesCatalog("provider = 'UrgsimSeries'");
            return sc.Rows.Count > 0;
        }

       


        public TimeSeriesDatabaseDataSet.ScenarioDataTable GetSelectedScenarios()
        {
            var tbl = new TimeSeriesDatabaseDataSet.ScenarioDataTable();
            m_server.FillTable(tbl,"select checked, name, path, sortorder from scenario where (checked = 1)");
            return tbl;
        }


        public TimeSeriesDatabaseDataSet.ScenarioDataTable GetScenarios()
        {
            var tbl = new TimeSeriesDatabaseDataSet.ScenarioDataTable();
            m_server.FillTable(tbl, "select * from scenario ");
            return tbl;
        }
        public void ClearScenarios()
        {
            m_server.RunSqlCommand("delete from scenario");
        }

        

        //public bool ProviderInUse(string provider)
        //{
        //    string sql = "Select count(*) from SeriesCatalog where [Provider] = 'ModsimSeries'";
        //   DataTable tbl =  m_server.Table("providers", sql);
        //   return Convert.ToInt32(tbl.Rows[0][0]) > 0;
        //}

        internal bool SeriesExists(int sdi)
        {
            string sql = "select id from seriescatalog where id = " + sdi;
            return Server.Table("sitecatalog", sql).Rows.Count > 0;
        }

        internal bool FolderExists(string name, int parentID)
        {
            string sql = "select id from seriescatalog where parentid = " + parentID
                + " and name = '" + name + "' ";
            return Server.Table("sitecatalog", sql).Rows.Count > 0;
        }

        /// <summary>
        /// Path to Database server
        /// </summary>
        public string DataSource
        {
            get
            {
                return m_server.DataSource;
            }
        }



        ///// <summary>
        ///// gets or sets the Number of tables the database
        ///// will use before creating another file on disk.
        ///// Used for advanced scenarios where you approach
        ///// the 4 GB limit of some databases.
        ///// </summary>
        //int TablesPerFile
        //{
        //    get { return m_tablesPerFile; }
        //    set
        //    {
        //        if (TablesPerFile <= 0)
        //            throw new ArgumentOutOfRangeException();

        //        m_tablesPerFile = value;

        //    }
        //}

        ///// <summary>
        ///// TO-DO: If the current file is over 3GB 
        ///// we better create another file so it does not exceed the
        ///// 4GB SQL Compact limit.
        ///// </summary>
        ///// <param name="i"></param>
        ///// <returns></returns>
        //private int NewFileIndex(int i)
        //{
        //    //int rval = (i - 1) / m_tablesPerFile;
        //    //return rval;
            
        //    return 0;
        //}


        public int AddSeries(Series s)
        {
            //PiscesFolder folder = RootObject as PiscesFolder;
            return AddSeries(s, RootFolder);
        }



        /// <summary>
        /// Adds a Series to the database 
        /// </summary>
        /// <returns>assigned database id</returns>    
        public int AddSeries(Series s, PiscesFolder folder)
        {
            s.TimeSeriesDatabase = this;
            s.Parent = folder;
            s.ID = NextSDI();
            s.ConnectionString = ConnectionStringUtility.MakeFileNameRelative(s.ConnectionString, this.DataSource);

            if (s.ExternalDataSource)
                s.SeriesCatalogRow.TableName = "";

            else
            {
                if (s.Table != null && s.Table.TableName.Trim() != "")
                {
                    FixInvalidTableName(s);
                    s.SeriesCatalogRow.TableName = GetUniqueTableName(s.Table.TableName);
                    s.Table.TableName = s.SeriesCatalogRow.TableName;
                    // 
                }
                else
                { // nothing to start with
                    s.SeriesCatalogRow.TableName = GetUniqueTableName(s.Name); 
                }
            }

            s.Table.TableName = s.SeriesCatalogRow.TableName;
            s.SortOrder = NextSortOrder(s.Parent.ID);
            m_server.SaveTable(s.SeriesCatalogRow.Table); // save catalog

            if (!s.ExternalDataSource)
            {
                ImportTimeSeriesTable(s.Table, s.SeriesCatalogRow, DatabaseSaveOptions.Insert);
            }

            if (!m_supspendTreeUpdates)
            {
                RefreshFolder(folder);
            }

            return s.ID;
        }


        bool m_supspendTreeUpdates = false;
        public void ResumeTreeUpdates()
        {
            m_supspendTreeUpdates = false;
        }
        public void SuspendTreeUpdates()
        {
            m_supspendTreeUpdates = true;
        }

        public PiscesFolder AddFolder(string name)
        {
            PiscesFolder folder = RootFolder as PiscesFolder;
            return AddFolder(folder, name);
        }
        /// <summary>
        /// Creates a new folder in the database
        /// </summary>
        /// <param name="parentID">parent of new folder to create</param>
        /// <returns>sid for the new folder</returns>
        public PiscesFolder AddFolder(PiscesFolder parent, string name)
        {
            int parentID = parent.ID;
            SeriesCatalogRow si = GetNewSeriesRow();
            si.ParentID = parentID;
            si.IsFolder = true;
            if (name.Trim() == "")
                si.Name = GetUniqueFolderName("New Folder");
            else
                si.Name = name;
            si.TableName = GetUniqueTableName("folder"); 
            si.SortOrder = NextSortOrder(parentID);
            SaveSeriesRow(si);

            if (!m_supspendTreeUpdates)
            {
                RefreshFolder(parent);
            }

            return this.factory.GetFolder(si.id);
        }

        /// <summary>
        /// Adds new site using template subset of a SeriesCatalog
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="template">copy this sereis catalog changing the siteid </param>
        /// <param name="SiteName"></param>
        /// <param name="SiteID"></param>
        /// <param name="elevation"></param>
        /// <param name="Lat"></param>
        /// <param name="Lon"></param>
        /// <param name="TimeZone"></param>
        /// <param name="Install"></param>
        public void AddSiteWithTemplate(PiscesFolder parent, 
            TimeSeriesDatabaseDataSet.SeriesCatalogDataTable template, string SiteName, string SiteID, 
            string state,string elevation, string Lat, string Lon, string TimeZone, string Install, string program)            
        {
          
            var siteCatalog = GetSiteCatalog();
            var rows = siteCatalog.Select("siteid='"+SiteID+"'");
            if (rows.Length == 0)  // check if site exists before creating.
            {
                siteCatalog.AddsitecatalogRow(SiteID, SiteName, state, Lat, Lon, elevation, TimeZone, Install, "", "", 0, "", "", "", "", "");
                Server.SaveTable(siteCatalog);
            }

            var siteFolder = GetOrCreateFolder(parent,SiteID);
            var sc = GetSeriesCatalog();
            var instant = sc.AddFolder("instant", siteFolder.ID);
            var daily = sc.AddFolder("daily", siteFolder.ID);
            var quality = sc.AddFolder("quality", siteFolder.ID);

            var series_properties = GetSeriesProperties();
            
            foreach (var item in template)
            {
                int id = sc.NextID();
                int parentID = siteFolder.ID;

                if (item.TimeInterval == "Daily" )
                    parentID = daily;
                if( item.TimeInterval == "Irregular")
                    parentID = instant;

                if (Decodes.DecodesRawFile.QualityParameters.Contains(item.Parameter.ToUpper()))
                {
                    parentID = quality;
                }
                sc.AddSeriesCatalogRow(id,parentID, false, id, item.iconname, item.Name, item.siteid, item.Units, 
                        item.TimeInterval, item.Parameter, item.TableName, item.Provider, item.ConnectionString, item.Expression, item.Notes, item.enabled);


                series_properties.AddseriespropertiesRow(series_properties.NextID(), id, "program", program);
                //series_properties.DuplicateProperties(series item.id, id);
           }
            series_properties.Save();
            Server.SaveTable(sc);
            

        }

        int NextSortOrder(int parentID)
        {
            string sql = "select sortorder from seriescatalog "
            + "where parentid = " + parentID + " and id <> " + parentID
            + " order by sortorder ";
            DataTable tbl = m_server.Table("tmp", sql);
            if (tbl.Rows.Count == 0)
                return 1;
            int rval = Convert.ToInt32(tbl.Rows[tbl.Rows.Count - 1]["sortorder"]) + 1;
            //Logger.WriteLine("New sort order of " + rval + " assigned");
            return rval;
        }

        private string GetUniqueFolderName(string prefix)
        {
            if (!NameExists(prefix))
                return prefix;
            for (int i = 0; i < 1000; i++)
            {
                string rval = prefix + i.ToString().PadLeft(3, '0');
                if (!NameExists(rval))
                    return rval;
            }
            return prefix + Guid.NewGuid().ToString();
        }

        private bool NameExists(string name)
        {
            var sql = "select count(*) from seriescatalog where name = '" + name + "'";
            return Convert.ToInt32(Server.Table("tmp", sql).Rows[0][0]) > 0;
        }

        /// <summary>
        /// Deletes data from the database that would be overlapping
        /// if the table is imported.
        /// </summary>
        /// <param name="table">table to be imported later</param>
        /// <param name="fileIndex"></param>
        /// <param name="server"></param>
        private void DeleteExistingData(DataTable table)
        {
            string sql = "Select count(*) from " + m_server.PortableTableName(table.TableName);

            DataTable tbl = m_server.Table("query", sql);
            if (tbl.Rows.Count > 0)
            {
                int count = Convert.ToInt32(tbl.Rows[0][0]);
                if (count == 0)
                    return;
            }

            // old method:  33 seconds total  56% of time in this routine... ( avg 1022 ms) per call  (save to temporary table )
            // this new  :  16 seconds total  2% of time in this routine... (avg 17 ms) per call
            string[] dates = (from DataRow row in table.Rows
                                  select  m_server.PortableDateString( (DateTime)row[0],dateTimeFormat)).ToArray();

            if (dates.Length > 0)
            {
                sql = "Delete from " + table.TableName + " Where datetime in "
                + " ( " + String.Join(",", dates) + " )";
                int i = m_server.RunSqlCommand(sql);
                Logger.WriteLine("Deleted " + i + " old records");
            }
        }

        /// <summary>
        /// Deletes data from the database that would be overlapping
        /// if the table is imported.
        /// </summary>
        /// <param name="table">table to be imported later</param>
        /// <param name="fileIndex"></param>
        /// <param name="server"></param>
        private void DeleteExistingData__old(DataTable table, int fileIndex)
        {
            BasicDBServer server = m_server.NewConnection(fileIndex);
            string sql = "Select count(*) from " +m_server.PortableTableName( table.TableName);

            DataTable tbl = server.Table("query", sql);
            if (tbl.Rows.Count > 0)
            {
                int count = Convert.ToInt32(tbl.Rows[0][0]);
                if (count == 0)
                    return;
            }

            string tmpTableName = GetUniqueTableName("temp_");
            CreateSeriesTable(tmpTableName,true);
            DataTable tmp = table.Copy();
            tmp.TableName = tmpTableName;
            Logger.WriteLine("Saving temporary table");
            server.SaveTable(tmp);
            sql = "Delete from " + table.TableName +" Where datetime in "
            + " (SELECT datetime from " + tmpTableName + " )";
            int i = server.RunSqlCommand(sql);
            Logger.WriteLine("Deleted " + i + " old records");
            server.RunSqlCommand("Drop Table " + tmpTableName ); // requires permission?
        }

        
        
        internal PeriodOfRecord GetPeriodOfRecord(int siteDataTypeID)
        {

            SeriesCatalogRow si = GetSeriesRow(siteDataTypeID);

            string sql = "select count(*), min(datetime),max(datetime) from " + m_server.PortableTableName(si.TableName); 

            DateTime t1 = TimeSeriesDatabase.MinDateTime; 
            DateTime t2 = TimeSeriesDatabase.MaxDateTime; 
            int count = 0;
            if (m_server.TableExists(si.TableName))
            {
                DataTable por = Server.Table("por", sql);
                count = Convert.ToInt32(por.Rows[0][0]);
                if (count > 0)
                {
                    t1 = Convert.ToDateTime(por.Rows[0][1]);
                    t2 = Convert.ToDateTime(por.Rows[0][2]);
                }
            }


            PeriodOfRecord rval = new PeriodOfRecord(t1, t2, count);
            return rval;
        }
        /// <summary>
        /// Gets Period of record withing a specified data range (t1,t2).
        /// Used to determine ranges of dates for updates
        /// </summary>
        /// <param name="siteDataTypeID"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        internal PeriodOfRecord GetPeriodOfRecord(int siteDataTypeID, DateTime t1, DateTime t2)
        {

            SeriesCatalogRow si = GetSeriesRow(siteDataTypeID);

            string sql = "select count(*), min(datetime),max(datetime) from " + m_server.PortableTableName(si.TableName)
                  + " WHERE datetime >= " + m_server.PortableDateString( t1,dateTimeFormat)
                + " AND "
            + " datetime <= " + m_server.PortableDateString( t2,dateTimeFormat) ;


            t1 = TimeSeriesDatabase.MinDateTime;//.. DateTime.MinValue;
            t2 = TimeSeriesDatabase.MaxDateTime;// DateTime.MinValue;
            int count = 0;
            if (m_server.TableExists(si.TableName))
            {
                DataTable por = Server.Table("por", sql);
                count = Convert.ToInt32(por.Rows[0][0]);
                if (count > 0)
                {
                    t1 = Convert.ToDateTime(por.Rows[0][1]);
                    t2 = Convert.ToDateTime(por.Rows[0][2]);
                }
            }

            PeriodOfRecord rval = new PeriodOfRecord(t1, t2, count);
            return rval;
        }

        

        public int NextSDI()
        {
            return m_server.NextID("seriescatalog", "id");
        }



        /// <summary>
        /// Saves Properties to database for Series or Folder
        /// </summary>
        public void SaveProperties(PiscesObject o)
        {
            SaveSeriesRow(o.SeriesCatalogRow);
        }

        public static TimeInterval TimeIntervalFromString(string interval)
        {
            TimeInterval rval = TimeInterval.Irregular;
            try
            {
                rval = (TimeInterval)Enum.Parse(typeof(TimeInterval), interval);
            }
            catch (Exception ex)
            {
                Logger.WriteLine("when trying to parse TimeInterval '" + interval + "'");
                Logger.WriteLine(ex.Message);
                Logger.WriteLine(ex.StackTrace);
                rval = TimeInterval.Irregular;
            }
            return rval;
        }


        /// <summary>
        /// Updates any changes in this Series to the database
        /// </summary>
        /// <param name="sdi">database id for this series</param>
        /// <param name="s">series to be saved to database</param>
        /// <param name="overwrite">delete overlaping-existing data before saving</param>
        /// <returns>nubmer of series points updated</returns>
        public int SaveTimeSeriesTable(int sdi, Series s, DatabaseSaveOptions option)
        {
            SeriesCatalogRow si = GetSeriesRow(sdi);
            // I think we should set overwrite to true for all cases?
            // However, there would be performance penalty in cases
            // this is not necessary
            int rval = ImportTimeSeriesTable(s.Table, si, option);
            return rval;
        }

        /// <summary>
        /// truncate all time series data in database
        /// </summary>
        /// <param name="sdi"></param>
        private void Truncate(int sdi)
        {
            SeriesCatalogRow si = GetSeriesRow(sdi);
            if (m_server.TableExists(si.TableName))
            {
                string sql = "delete from  " + m_server.PortableTableName(si.TableName);
                m_server.RunSqlCommand(sql);
            }
        }


        
        /// <summary>
        /// Imports DataTable to the Database
        /// </summary>
        private int ImportTimeSeriesTable(DataTable table, SeriesCatalogRow si,
             DatabaseSaveOptions option)
        {

            table.Columns[0].ColumnName = "datetime";
            table.Columns[1].ColumnName = "value";
            // table.Columns[2].ColumnName = "flag";
            table.TableName = si.TableName;

            if (table.Columns.Count == 2)
            { 
                table.Columns.Add("flag");
                table.Columns["flag"].DefaultValue = "";
            }
            else
            { }

            if (!m_server.TableExists(table.TableName))
            {
                CreateSeriesTable(si.TableName, true);
            }

            int count = 0;
         

            if (option == DatabaseSaveOptions.UpdateExisting)
            {
                //count = InsertOrUpdate(table, si.FileIndex); 
                DeleteExistingData(table);
                count = m_server.InsertTable(table);
            }
            else
                if (option == DatabaseSaveOptions.DeleteAllExisting)
                {
                    Truncate(si.id);
                    count = m_server.InsertTable(table);
                }
                else if (option == DatabaseSaveOptions.Insert)
                {
                    count = m_server.InsertTable(table);
                }
                else if (option == DatabaseSaveOptions.Save)
                {
                    count = m_server.SaveTable(table);
                }
            Logger.WriteLine("Saved " + count + " records "+ table.TableName+" "+m_server.DataSource);

            return count;
        }


        private string GetUniqueTableName(string prefix)
        {
            string stn = SafeTableName(prefix);

            if (!TableNameInUse(stn))
            {
                return stn;
            }
            else
            {
                var rval = stn + DateTime.Now.ToString("yyyyMMMddHHmmssfff").ToLower();
                Logger.WriteLine(stn + " is allready being used. Creating unique name "+rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns true if table is being used in Database
        /// or being used in SeriesCatalog.  It is possible a table name is reseved in the catalog
        /// but not yet used in the database.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool TableNameInUse(string tableName)
        {
            string sql = "select tablename from seriescatalog where tablename = '" + tableName + "'";
            if (m_server.Table("seriesCatalog", sql).Rows.Count > 0)
                return true;

            return m_server.TableExists(tableName);
        }

        public static string SafeTableName(string tableName)
        {
            return Regex.Replace(tableName, @"[^A-Za-z0-9_\-]", "_").ToLower();
        }

        public BasicDBServer Server
        {
            get
            {
                return m_server;
            }
        }

        //public string Name
        //{
        //    get
        //    {
        //        if (Server != null)
        //            return Server.Name;
        //        return "SqlTimeSeriesDatabase";
        //    }
        //}

        public bool SupportsPeriodOfRecordQueries
        {
            get { return true; }
        }


        public DataTable ReadTimeSeriesTable(int sdi, DateTime t1, DateTime t2)
        {
            var sr = GetSeriesRow(sdi);
            string tableName = sr.TableName;
            string sql = "SELECT * from " + m_server.PortableTableName(tableName);

            if (t1 != MinDateTime || t2 != MaxDateTime)
            {
                sql += " WHERE datetime >= " + m_server.PortableDateString(t1, dateTimeFormat)
                + " AND "
                + " datetime <= " + m_server.PortableDateString(t2, dateTimeFormat);
            }
            sql += " order by datetime ";


            if (!m_server.TableExists(tableName))
            {
                Logger.WriteLine("Table " + tableName + " does not exist");
                CreateSeriesTable(tableName, true);
            }
            DataTable tbl = m_server.Table(tableName, sql);
            return tbl;
        }

        /// <summary>
        /// Clears all data from local database for this series
        /// </summary>
        /// <param name="series"></param>
        public void ClearSeries(Series series)
        {
            if (series.ExternalDataSource)
            {
                Logger.WriteLine("Can't clear a series that is linked to external data");
                return;
            }
            SeriesCatalogRow r = GetSeriesRow(series.ID);
            string sql = "delete from  " + m_server.PortableTableName(r.TableName);
            m_server.RunSqlCommand(sql);
            series.Table.Rows.Clear();
            series.Table.AcceptChanges();
        }


        public void Delete(PiscesObject o)
        {
            PiscesObject parent = o.Parent;
            TimeSeriesDatabaseDelete d = new TimeSeriesDatabaseDelete(this, o);
            d.Delete();
            if (parent is PiscesFolder)
                RefreshFolder(parent as PiscesFolder);
        }
        public bool DeleteFolderByName(string folderName, PiscesFolder selectedFolder, bool promptUser=false)
        {
            PiscesObject[] obj = GetChildren(selectedFolder);
            bool process = true;
            foreach (PiscesObject o in obj)
            {
                if (string.Equals(o.Name, folderName))
                {
                    var result = DialogResult.OK;

                    if (promptUser)
                    {
                        result = MessageBox.Show("This will delete the current '" + selectedFolder.Name + "/" + folderName + "' folder.",
                                                   "Warning!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    }

                    if (result == DialogResult.OK)
                    {
                        Delete(o);
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        process = false;
                        break;
                    }
                }
            }
            return process;
        }


        internal void DropTable(string tableName)
        {
            if (m_server.TableExists(tableName))
            {
                string sql = "Drop Table " +m_server.PortableTableName( tableName);
                m_server.RunSqlCommand(sql);
            }
        }

        /// <summary>
        /// Creates a new SeriesRow and Assigns SiteDataTypeID
        /// </summary>
        /// <returns></returns>
        public SeriesCatalogRow GetNewSeriesRow(bool assignID = true)
        {
            var tbl = new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();
            var rval = tbl.NewSeriesCatalogRow();
            rval.id = NextSDI();
            rval.enabled = true;
            tbl.Rows.Add(rval);
            return rval;
        }

        internal SeriesCatalogRow GetSeriesRow(int sdi)
        {
            string sql = "Select * from seriescatalog where id = " + sdi;
            var tbl = new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();
            m_server.FillTable(tbl, sql);
            if (tbl.Rows.Count == 0)
                throw new InvalidOperationException("this SiteDataTypeID does not exist "+sdi);

            return tbl[0];
        }

        internal TimeSeriesDatabaseDataSet.sitecatalogRow GetSiteRow(string siteID)
        {
            Logger.WriteLine("GetSiteRow('"+siteID+"')");
            string sql = "Select * from sitecatalog where lower(siteid) = '" + siteID.ToLower() + "'";
            var tbl = new TimeSeriesDatabaseDataSet.sitecatalogDataTable();
            m_server.FillTable(tbl, sql);
            if (tbl.Rows.Count == 0)
            {
                Logger.WriteLine("Error: GetSiteRow() Could not find site with ID = '" + siteID + "'");
                return null;
            }
            var rval = tbl[0];

            return rval;
        }


        /// <summary>
        /// Gets a Series Catalog using SQL.
        /// </summary>
        /// <param name="filter">Select * from seriesCatalog where [filter] </param>
        /// <returns></returns>
        internal SeriesCatalogRow GetSeriesRow(string filter)
        {
            string sql = "Select * from seriescatalog where " + filter;
            var tbl = new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();
            m_server.FillTable(tbl, sql);
            if (tbl.Rows.Count == 0)
                return null;

            return tbl[0];
        }

        public Series GetSeries(int id)
        {
            return Factory.GetSeries(id);
        }

        public IEnumerable<Series> GetSeries(TimeInterval interval, string filter,string propertyFilter="")
        {
            var ie = factory.GetSeries(interval, filter,propertyFilter);

            return ie;
            //throw new NotImplementedException();
        }


        //public Series GetSeries(string siteID, string parameterCode, TimeInterval timeInterval)
        //{
        //    return GetSeriesFromTableName(siteID + "_" + parameterCode,timeInterval.ToString().ToLower());
        //}

        /// <summary>
        /// Lookup Series based on the display Name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Series GetSeriesFromName(string name, string timeInterval="", bool exactMatch=false)
        {
            //string sql = " select * from seriesCatalog  where name = '" + name + "'";
            //string sql = "Select * from SeriesCatalog where name = '"+name +"' union select * from SeriesCatalog where name like 'one:%'";
            string sql = "Select * from seriescatalog where name = '" + name + "' and isfolder = 0 ";

            if (!exactMatch)
            {
                sql += " UNION "
                + " select * from seriescatalog where name like '" + name + ":%' and isfolder = 0";
            }
            if (timeInterval != "")
            {
                sql = sql.Replace("where name", "where lower(timeinterval) ='"+timeInterval.ToLower()+"' and name");
            }

            var tbl = new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();
            m_server.FillTable(tbl, sql);
            if (tbl.Rows.Count == 0)
            {
                Logger.WriteLine("Error: GetSeriesFromName: This name was not found '" + name + "'");
                return null;
                
            }
            if (tbl.Rows.Count > 1)
            {
                Logger.WriteLine("Error: GetSeriesFromName Series '" + name + "' was found " + tbl.Rows.Count + " times");
                return null;
            }

            return factory.GetSeries(tbl[0]);
        }

        /// <summary>
        /// Lookup Series based on the internal database TableName
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="defaultInterval">default tableName prefix interval</param>
        /// <param name="prefix">table prefix usually 'instant' , 'daily' or monthly </param>
        /// <param name="createMissing"></param>
        /// <returns></returns>
        public Series GetSeriesFromTableName(string tableName, string prefix="",bool createMissing=false)
        {
            Logger.WriteLine("GetSeriesFromTableName(" + tableName + ", '" + prefix+"')");
            if (prefix.ToLower() == "irregular")
                prefix = "instant"; // inconsistency..

            if (Regex.IsMatch(tableName, "^[0-9]")) // table name starting with number is not allowed
            {
                tableName = "_" + tableName; // append with underscore
            }

            TimeSeriesName tn = new TimeSeriesName(tableName);

            if ( tn.Valid && !tn.HasInterval && prefix != "")
            {
                tableName = prefix + "_" + tableName; // add prefix before searching
            }

            string sql = "Select * from seriescatalog where tablename = '" + tableName.ToLower() + "' and isfolder = 0 ";
            var tbl = new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();
            m_server.FillTable(tbl, sql);
            if (tbl.Rows.Count == 0)
            {
                
                if (createMissing)
                {
                    Logger.WriteLine("Creating series "+tableName);
                    if (m_missingFolder == null )
                        m_missingFolder =  GetOrCreateFolder("missing");

                    Series ns = new Series();
                    ns.Name = tableName;
                    ns.Table.TableName = tableName;
                    ns.Parameter = tn.pcode;
                    var id = AddSeries(ns,m_missingFolder);
                    return factory.GetSeries(id);
                }
                else
                {
                    Logger.WriteLine("Error: This table name was not found '" + tableName + "'");
                    return null;
                }
            }
            var s =factory.GetSeries(tbl[0]);
            Logger.WriteLine(s.Name + ":  " + s.Table.TableName);
            return s;
        }

        PiscesFolder m_missingFolder = null;

        ///// <summary>
        ///// Creates or returns folder named 'missing'
        ///// for importing data not defined.
        ///// </summary>
        ///// <returns></returns>
        //private int GetOrCreateFoldera(string name)
        //{
        //    var tbl = GetSeriesCatalog("name = '"+name+"'");
        //    if (tbl.Rows.Count == 1)
        //    {
        //        return Convert.ToInt32(tbl.Rows[0]["name"]);
        //    }



        //    return -1;
        //}



        private void SaveSeriesRow(SeriesCatalogRow si)
        {
            m_server.SaveTable(si.Table);
        }


        public string[] GetUniqueUnits()
        {
            DataTable tbl = Server.Table("catalog", "select distinct units from seriescatalog");
            List<string> rval = DataTableUtility.StringList(
                DataTableUtility.SelectDistinct(tbl, "units"),
                "", "units");
            if (rval.IndexOf("") < 0)
            {
                rval.Add("");
            }
            return rval.ToArray();
        }


        /// <summary>
        /// Imports site catalog from CSV file 
        /// requies same format as created from Export command
        /// This is called on newly created 'empty' database.
        /// </summary>
        /// <param name="filename"></param>
        public void ImportCsvDump(string filename, bool importSeriesData)
        {
            string dir = Path.GetDirectoryName(filename);
            string sql = "select id from seriescatalog where isfolder = 0";
            DataTable sc = m_server.Table("seriescatalog", sql);

            if (sc.Rows.Count > 0)
                throw new InvalidOperationException("Database must be empty to import new Catalog");

            sql = "delete from seriescatalog";
            m_server.RunSqlCommand(sql);

            sc = m_server.Table("seriescatalog");

            CsvFile oldCatalog = new CsvFile(filename);

            sc.Constraints.Add("pk_sdi", sc.Columns["id"], true);
            string[] oldColumnNames = { "sitedatatypeid", "sitename", "source" };
            string[] newColumnName = { "id", "siteid", "iconname" };

            for (int i = 0; i < oldCatalog.Rows.Count; i++)
            {
                var newRow = sc.NewRow();

                for (int c = 0; c < sc.Columns.Count; c++)
                {
                    string new_cn = sc.Columns[c].ColumnName;
                    string old_cn = new_cn;
                    int idx = oldCatalog.Columns.IndexOf(new_cn);

                    if (idx < 0) // look for mapping
                    {
                        idx = Array.IndexOf(newColumnName, new_cn);
                        if (idx >= 0)
                        {
                            old_cn = oldColumnNames[idx];
                        }
                        else
                        {// skip this column
                            continue;
                        }
                    }

                    newRow[new_cn] = oldCatalog.Rows[i][old_cn];
                }

                sc.Rows.Add(newRow);
            }
            m_server.SaveTable(sc);
            sc = GetSeriesCatalog();

            if (importSeriesData)
            {
                for (int i = 0; i < sc.Rows.Count; i++)
                {
                    object o = sc.Rows[i]["TableName"];
                    if (o != DBNull.Value && o.ToString().Trim().Length > 0)
                    {
                        // check if file exists (same as table name with .csv extension)
                        string fn = Path.Combine(dir, Path.ChangeExtension(o.ToString().Trim(), ".csv"));
                        if (File.Exists(fn))
                        {
                            // read file into datatable.
                            CsvFile tbl = new CsvFile(fn);
                            tbl.TableName = GetUniqueTableName(Path.GetFileNameWithoutExtension(fn));
                            if (tbl.Columns.Count == 2 || tbl.Columns.Count == 3)
                            {
                                // create/save 
                                var sr = GetNewSeriesRow(false);
                                sr.ItemArray = sc.Rows[i].ItemArray;
                                ImportTimeSeriesTable(tbl, sr, DatabaseSaveOptions.Insert);
                            }
                            else
                            {
                                Logger.WriteLine("Skipped file '" + fn + "'");
                            }
                        }
                    }
                }
            }
            // TO DO 
            // import/export ScenarioTable.
            // import/export PiscesInfo table
            OnDatabaseChanged(new object[] { });
            //this.OnStructureChanged(new TreePathEventArgs(TreePath.Empty));
        }

        /// <summary>
        /// Exports the time series database as a group of text files
        /// </summary>
        public void Export(string path)
        {
            Directory.CreateDirectory(path);
            var sc = GetSeriesCatalog();
            string filename = Path.Combine(path, "SeriesCatalog.csv");
            CsvFile.WriteToCSV(sc, filename, true, true);
            int sz = sc.Rows.Count;


            CsvFile.WriteToCSV(this.GetScenarios(), Path.Combine(path, "scenario.csv"), true, true);
            

            for (int i = 0; i < sz; i++) 
            {
                var si = sc[i];
                if (!si.IsFolder && m_server.TableExists(si.TableName))
                {
                    DataTable t = m_server.Table(si.TableName);
                    filename = Path.Combine(path, si.TableName + ".csv");
                    DataTableOutput.Write(t, filename, true);
                }
            }
        }



        private void CreateRootFolder()
        {
            TimeSeriesDatabaseDataSet.SeriesCatalogDataTable tbl= new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();

            m_server.FillTable(tbl, "Select * from seriescatalog where id = parentid ");
            //DataTable tbl = m_server.Table("seriescatalog", "Select * from seriesCatalog where id = parentid ");

            if (tbl.Rows.Count == 0)
            {
                Logger.WriteLine("Creating Root Folder in database '"+m_server.Name+"'");
                int id = NextSDI();
                Logger.WriteLine("id = "+id);
               // DataRow row = tbl.NewRow();
               // row["id"] = id;
               // row["ParentID"] = id;
               // row["IsFolder"] = true;
               //// row["sortorder"] = 0;
               // row["Name"] = m_server.Name;
                tbl.AddFolder(m_server.Name,id, id);
               // tbl.Rows.Add(row);
                Logger.WriteLine("before save: tbl.rows[0][id]= "+tbl.Rows[0]["id"].ToString());
                m_server.SaveTable(tbl);
                Logger.WriteLine("Root folder created");
            }
        }


        public event EventHandler<TimeSeriesDatabaseSettingsEventArgs> OnReadSettingsFromDatabase;

        public void ReadSettingsFromDatabase(TimeWindow w)
        {

            if( OnReadSettingsFromDatabase != null)
            {
                OnReadSettingsFromDatabase(this, new TimeSeriesDatabaseSettingsEventArgs(m_settings,w));
            }
        }

        

        public bool AutoRefresh = true; // refresh when tree selection changes.

        public event EventHandler<TimeSeriesDatabaseSettingsEventArgs> OnSaveSettingsToDatabase;

        public void SaveSettingsToDatabase(TimeWindow w)
        {

            if (OnSaveSettingsToDatabase != null)
            {
                OnSaveSettingsToDatabase(this, new TimeSeriesDatabaseSettingsEventArgs(m_settings, w));
            }

        }


        /// <summary>
        /// breaks links to external data and make a local copy
        /// </summary>
        public void CreateStandalone()
        {
            var table = GetSeriesCatalog();

            try
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    var row = table[i];
                    if (!row.IsFolder )
                    {
                        if (!m_server.TableExists(row.TableName)) // table name is blank for MODSIM
                        {
                            Series s = Factory.GetSeries(row);
                            s.Read();

                            s.SeriesCatalogRow.TableName = GetUniqueTableName(s.Provider + s.ID.ToString());
                            s.Table.TableName = s.SeriesCatalogRow.TableName;
                            row.Provider = "Series";

                            ImportTimeSeriesTable(s.Table, row, DatabaseSaveOptions.Insert);
                        }
                        else // may be Hydromet data with existing data
                        {
                            row.Provider = "Series";
                        }
                    }
                }
            }
            finally
            {
                m_server.SaveTable(table);
            }
        }


        //public event EventHandler<SeriesEventArgs> AfterSave;
        //private void OnAfterSave(SeriesEventArgs e)
        //{
        //    EventHandler<SeriesEventArgs> handler = AfterSave;

        //    if (handler != null)
        //        handler(this, e);

        //}

        

        /// <summary>
        /// Import Series s into the database.
        /// Series s is saved to the tablename defined in the Series s.DataTable.TableName
        /// the table will be created if necessary. Properties such as units will be used
        /// when creating a new series
        /// Computes data dependent on the imported data
        /// Returns List of computed data.
        /// </summary>
        public void ImportSeriesUsingTableName(Series s, string folderName="" )
        {
            Logger.WriteLine("ImportSeriesUsingTableName: '" + s.Table.TableName+"'");
            FixInvalidTableName(s);

            var sr  = GetSeriesRow("tablename ='" + s.Table.TableName.ToLower() +"'");

            if (sr == null)
            {// create new series.
                //sr = GetNewSeriesRow();
                Logger.WriteLine("table: " + s.Table.TableName + " does not exist in the catalog");
                if (folderName == "")
                {
                    TimeSeriesName tn = new TimeSeriesName(s.Table.TableName);
                    if (tn.interval != "")
                        folderName = tn.interval;
                }

                PiscesFolder folder = null;
                if (folderName != "")
                    folder = GetOrCreateFolder(folderName);
                else
                    folder = RootFolder;

                sr = GetNewSeriesRow();
                sr.Name = s.Name;
                sr.Parameter = s.Parameter;
                sr.ParentID = folder.ID;

                Logger.WriteLine("Info: ImportSeriesUsingTableName()  series: " + s.Name + " tablename=" + s.Table.TableName);
                sr.TableName = s.Table.TableName;

                m_server.SaveTable(sr.Table);

            }
            

            
                ImportTimeSeriesTable(s.Table, sr, DatabaseSaveOptions.UpdateExisting);
            
          //  OnAfterSave(new SeriesEventArgs(s));    

        }

        
        public PiscesFolder GetOrCreateFolder( string folderName)
        {
           return GetOrCreateFolder(null,folderName);
        }
        
        public PiscesFolder GetOrCreateFolder( PiscesFolder parent=null,params string[] folderNames)
        {
            PiscesFolder rval = parent;
            for (int i = 0; i < folderNames.Length; i++)
            {
                var fn = folderNames[i];
                string sql = "name ='" + fn + "' and isfolder = 1";
                if (rval != null)
                    sql += " and parentid = " + rval.ID; 
                var sr = GetSeriesRow(sql);
                if (sr == null)
                {
                    if (rval != null)
                        rval = AddFolder(rval, fn);
                    else
                        rval = AddFolder(fn);
                }
                else
                {
                   rval = this.Factory.GetFolder(sr.id);
                   Logger.WriteLine(" found existing folder '"+fn+"'");
                }
            }
            return rval;
        }

        

        /// <summary>
        /// enforce that tableName doesn't start with number,
        /// and remove dashes in table name
        /// </summary>
        /// <param name="s"></param>
        private static void FixInvalidTableName(Series s)
        {
            var tn = s.Table.TableName;
            if (Regex.IsMatch(tn, "^[0-9]")) // table name starting with number is not allowed
            {
                tn = "_" + tn; // prefix with underscore instead.
            }

            tn = tn.Replace("-", "_");
            s.Table.TableName = tn;

            // check for '-' in the table name

        }

        public void Inventory()
        {
            Console.WriteLine("Inventory of Database "+m_server.Name);
            Console.WriteLine("Tables in Database:"+m_server.TableNames().Count());

            Console.WriteLine("Instant Series:"+GetSeriesCatalog("timeinterval = 'Irregular'").Count());
            Console.WriteLine("Daily Series:" + GetSeriesCatalog("timeinterval = 'Daily'").Count());
            Console.WriteLine("Monthly Series:" + GetSeriesCatalog("timeinterval = 'Monthly'").Count());
            Console.WriteLine("Series in Catalog: "+GetSeriesCatalog().Count());
            Console.WriteLine("");

        }

        public void DailySummaryReport(DateTime t1 , DateTime t2)
        {
            var sc = GetSeriesCatalog("timeinterval = 'Daily'");



        }
       
    }
}