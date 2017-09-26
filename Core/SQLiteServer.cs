using System;
using System.Collections;
using System.Data;
#if NETCOREAPP2_0
using Microsoft.Data.Sqlite;
#else
using System.Data.SQLite;
#endif
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Data.Common;

namespace Reclamation.Core
{
    /// <summary>
    /// Basic SQL and DataTable operations 
    /// for SQLite
    /// Karl Tarbet
    /// </summary>
    public class SQLiteServer : BasicDBServer
    {
        /// <summary>
        /// yyyy-MM-dd HH:mm:ss.fff
        /// </summary>
        public static string DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
        static string _staticConnectionString = null;
        string _connectionString = null;
        /// <summary>
        /// last commnad that was sent to server.
        /// </summary>
        protected string lastSqlCommand;
        // private ArrayList sqlCommands  = new System.Collections.ArrayList();
        string lastMessage = "";

        public void ClearPool()
        {
#if !NETCOREAPP2_0
            System.Data.SqlClient.SqlConnection.ClearAllPools();
#endif
        }



        /// <summary>
        /// default constructor 
        /// if connection string is not set it will be read
        /// from the config file
        /// </summary>
        public SQLiteServer()
        {
            FileName = "";
        }
        public string FileName { get; set; }

         private bool UnixTimeStamps
        {
            get
            {
                var datetimeformat = ConnectionStringUtility.GetToken(ConnectionString, "datetimeformat", "");
                return datetimeformat == "UnixEpoch";
            }
        }
        

        public SQLiteServer(string connString )
        {
            
            if (File.Exists(connString) || connString.IndexOf("=") < 0)
            {
                ConnectionString = "Data Source=" + connString + ";";
                FileName = connString;
            }
            else
            {
            ConnectionString = connString;
            FileName = ConnectionStringUtility.GetToken(connString, "Data Source", "");
            }
            
        }

        public override string DataSource
        {
            get { return FileName; }
        }

        private DbConnection GetConnection(string connStr)
        {
#if NETCOREAPP2_0
            return new SqliteConnection(connStr);
#else
            return new SQLiteConnection(connStr);
#endif 
        }

        private DbCommand GetCommand(string sql,DbConnection conn)
        {
#if NETCOREAPP2_0
            return new SqliteCommand(sql,(SqliteConnection)conn);
#else
            return new SQLiteCommand(sql,(SQLiteConnection)conn);
#endif
        }

        private DbCommand GetCommand()
        {
#if NETCOREAPP2_0
            return new SqliteCommand();
#else
            return new SQLiteCommand();
#endif
        }
        private DbCommand GetCommand(string sql)
        {
#if NETCOREAPP2_0
            return new SqliteCommand(sql);
#else
            return new SQLiteCommand(sql);
#endif
        }
        private DbParameter GetParameter()
        {
#if NETCOREAPP2_0
            return new SqliteParameter();
#else
            return new SQLiteParameter();
#endif
        }
        private DbDataAdapter GetAdapter(DbCommand cmd)
        {
#if NETCOREAPP2_0
            return null;//new Microsoft.Data.Sqlite.SqliteDataAdapter(cmd);
#else
            return new SQLiteDataAdapter((SQLiteCommand)cmd);
#endif
        }
        private DbCommandBuilder GetBuilder(DataAdapter dataAdapter)
        {
#if NETCOREAPP2_0
            return null;// new Microsoft.Data.Sqlite.SQLiteCommandBuilder(dataAdapter);
#else
            return new SQLiteCommandBuilder((SQLiteDataAdapter)dataAdapter);
#endif
        }

        public void CloseAllConnections()
        {//http://stackoverflow.com/a/24501130/2333687
#if !NETCOREAPP2_0
            SQLiteConnection.ClearAllPools();
#endif
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Creates new database.  Existing file will be overwritten
        /// </summary>
        /// <param name="filename"></param>
        public static void CreateNewDatabase(string filename)
        {
            SQLiteServer db = new SQLiteServer("Data Source=" + filename + ";");
            if (File.Exists(filename))
            {
                db.CloseAllConnections();
                File.Delete(filename);
            }
        }

       
        /// <summary>
        /// Gets the Sqlconnection string value.
        /// SqlConnString is read only.  To set the connectionstring edit your App.config
        /// </summary>
        public override string ConnectionString
        {
            // if the connection string is not defined use the
            // global connection string.
            // if the global connection string is not defined
            // try the app.config file.
            get
            {
                if (_connectionString == null)
                {

                    _connectionString = _staticConnectionString; //global
                    if (_connectionString == null)
                        _connectionString = ConfigurationManager.AppSettings["SqlConnectionString"];
                    if (_connectionString == null)
                    {
                        _connectionString = "initial catalog=Fa-Tracking;User ID=Hi_This_will_crashr;Pwd=317EB34E-F52C-4dc4-AF09-C1531480DCA1";
                    }
                }
                return _connectionString;
            }
            set
            {
                _connectionString = value;
                _staticConnectionString = value;

                string cleanConnStr = _connectionString;
                string pwd = ConnectionStringUtility.GetToken(_connectionString, "Pwd","");
                // need to strip out the password so it does not get logged.
                if (pwd != "")
                    cleanConnStr = cleanConnStr.Replace(pwd, "**********");

                SqlCommands.Add(cleanConnStr);

            }
        }

        /// <summary>
        /// gets a list of all 'base tables'
        /// </summary>
        /// <returns></returns>
        public override string[] TableNames()
        {
            string sql = "SELECT name FROM sqlite_master WHERE type='table' ORDER BY name";

            DataTable tbl = Table("schema", sql);
            string[] rval = new string[tbl.Rows.Count];
            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                rval[i] = tbl.Rows[i]["name"].ToString();
            }
            return rval;
        }


        public object ExecuteScalarCmd(DbCommand sqlCmd)
        {
            this.SqlCommands.Add(sqlCmd.CommandText);
            // Validate Command Properties
            if (ConnectionString == string.Empty)
                throw (new ArgumentOutOfRangeException("ConnectionString"));

            if (sqlCmd == null)
                throw (new ArgumentNullException("sqlCmd"));

            Object result = null;

            using (DbConnection cn = GetConnection(ConnectionString))
            {
                sqlCmd.Connection = cn;
                cn.Open();
                result = sqlCmd.ExecuteScalar();
            }



            return result;
        }
        public object ExecuteScalarCmd(string sql)
        {
            DbCommand sqlCmd = GetCommand(sql);
            return ExecuteScalarCmd(sqlCmd);
        }
        public void AddParamToSQLCmd(DbCommand sqlCmd, string paramId, DbType sqlType, int paramSize, ParameterDirection paramDirection, object paramvalue)
        {
            // Validate Parameter Properties
            if (sqlCmd == null)
                throw (new ArgumentNullException("sqlCmd"));
            if (paramId == string.Empty)
                throw (new ArgumentOutOfRangeException("paramId"));

            // Add Parameter
            var newSqlParam = GetParameter();
            newSqlParam.ParameterName = paramId;
            newSqlParam.DbType = sqlType;
            newSqlParam.Direction = paramDirection;

            if (paramSize > 0)
                newSqlParam.Size = paramSize;

            if (paramvalue != null)
                newSqlParam.Value = paramvalue;

            sqlCmd.Parameters.Add(newSqlParam);
        }


        public bool DeleteDataBase(string database_name)
        {
            string sql = "DROP DATABASE " + database_name;
            if (this.RunSqlCommandNonTransaction(sql))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CreateDataBase(string database_name)
        {
            string sql = "Create DATABASE " + database_name;
            return this.RunSqlCommandNonTransaction(sql);
        }


        private bool RunSqlCommandNonTransaction(string sql)
        {
            var myConnection = GetConnection(ConnectionString);
            myConnection.Open();
            var myCommand = GetCommand(sql);
            myCommand.Connection = myConnection;
            bool rval = false;
            try
            {
                myCommand.CommandText = sql;
                if (myCommand.ExecuteNonQuery() > 0)
                    rval = true;
                this.lastSqlCommand = sql;
                SqlCommands.Add(sql);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Error running " + sql);
                this.lastMessage = e.ToString();
                rval = false;
            }
            finally
            {
                myConnection.Close();
            }
            return rval;
        }


        /// <summary>
        /// checks if table exists
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override bool TableExists(string tableName)
        {
            string sql = "SELECT name FROM sqlite_master WHERE type='table' AND name='" + tableName + "'";
            return Table("exists", sql).Rows.Count ==1;
        }

        //Saves DataTable in database
        public override int SaveTable(DataTable dataTable)
        {
            string sql = "select  * from " + PortableTableName( dataTable.TableName) + " where 2=1";
            return SaveTable(dataTable, sql);

        }

        public override int SaveTable(DataTable dataTable, string sql)
        {
            Performance perf = new Performance();
            Logger.WriteLine("Saving " + dataTable.TableName);
            DataSet myDataSet = new DataSet();
            myDataSet.Tables.Add(dataTable.TableName);
            int recordCount = 0;

            using ( var conn = GetConnection(ConnectionString))
            using ( var myAccessCommand = GetCommand(sql, conn))
            using ( var myDataAdapter = GetAdapter(myAccessCommand))
            using ( var karlCB = GetBuilder(myDataAdapter))
            {
                this.lastSqlCommand = sql;
                SqlCommands.Add(sql);
                
                //baseline     Saved 1000 records in 6.938seconds 
                // transaction Saved 1000 records in 0.052seconds

                //baseline Saved 50000 records in 1.659seconds
                //(use Insert only) Saved 50000 records in 1.194seconds

                try
                {
                    conn.Open();
                    var dbTrans = conn.BeginTransaction();
                    myDataAdapter.Fill(myDataSet, dataTable.TableName);
                    recordCount = myDataAdapter.Update(dataTable);
                    dbTrans.Commit();
                }
                finally
                {
                    if (conn != null)
                        conn.Close();
                }
            }
	     string msg = "[" + dataTable.TableName + "] " + recordCount;
           Logger.WriteLine(msg,"ui");
           Console.WriteLine(msg);
	  
            
            return recordCount;
        }

        public int SaveTable(DataTable[] dataTable)
        {
            Performance perf = new Performance();
            DataSet myDataSet = new DataSet();
            
            int recordCount = 0;

            using (var conn = GetConnection(ConnectionString))
            using (var myAccessCommand = GetCommand() )
            using (var myDataAdapter = GetAdapter(myAccessCommand))
            using (var karlCB = GetBuilder(myDataAdapter))
            {

                try
                {
                    conn.Open();
                    
                    var dbTrans = conn.BeginTransaction();

                    for (int i = 0; i < dataTable.Length; i++)
                    {
                        var tn = dataTable[i].TableName;
                        myDataSet.Tables.Add(tn);
                        string sql = "select  * from " + PortableTableName(tn) + " where 2=1";
                        myDataAdapter.SelectCommand = GetCommand(sql,conn);
                        myDataAdapter.Fill(myDataSet, tn);
                        recordCount = myDataAdapter.Update(dataTable[i]);
                        myDataSet.Tables.Remove(tn);
                    }
                    dbTrans.Commit();
                }
                finally
                {
                    if (conn != null)
                        conn.Close();
                }
            }

            return recordCount;
        }


        /// <summary>
        /// returns all rows from a table.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public override DataTable Table(string tableName)
        {
            return Table(tableName, "select * from " + tableName);
        }

        /// <summary>
        /// returns table using sql
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public override DataTable Table(string tableName, string sql)
        {
            return Table(tableName, sql, true);
        }
        /// <summary>
        /// returns table using sql
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="sql"></param>
        /// <param name="AcceptChangesDuringFill"></param>
        /// <returns></returns>
        public DataTable Table(string tableName, string sql, bool AcceptChangesDuringFill)
        {

            string strAccessSelect = sql;

            using (var myAccessConn = GetConnection(ConnectionString))
            {
                //myAccessConn.ConnectionTimeout = 30;
                using (var myAccessCommand = GetCommand(strAccessSelect, myAccessConn))
                {
                    myAccessCommand.CommandTimeout = myAccessConn.ConnectionTimeout;

                    var myDataAdapter = GetAdapter(myAccessCommand);
                    myDataAdapter.AcceptChangesDuringFill = AcceptChangesDuringFill;
                    //Console.WriteLine(sql);
                    this.lastSqlCommand = sql;
                    SqlCommands.Add(sql);
                    DataSet myDataSet = new DataSet();
                    try
                    {
                        myAccessConn.Open();

                        myDataAdapter.Fill(myDataSet, tableName);
                    }
                    catch (Exception e)
                    {
                        string msg = "Error reading from database " + sql + " Exception " + e.ToString();
                        Console.WriteLine(msg);
                        throw e;
                    }
                    finally
                    {
                        myAccessConn.Close();
                    }
                    DataTable tbl = myDataSet.Tables[tableName];
                    myDataSet.Tables.Remove(tbl);
                    return tbl;
                }
            }
        }

        public override void FillTable(DataTable dataTable, string sql)
        {
            base.SqlCommands.Add("Fill(" + dataTable.TableName + ")");

            var myAccessConn = GetConnection(ConnectionString);
            var myAccessCommand = GetCommand(sql, myAccessConn);
            var myDataAdapter = GetAdapter(myAccessCommand);

            //Console.WriteLine(sql);
            this.lastSqlCommand = sql;
            SqlCommands.Add(sql);
            try
            {
                myAccessConn.Open();

                myDataAdapter.Fill(dataTable);
            }
            catch (Exception e)
            {
                string msg = "Error reading from database " + sql + " Exception " + e.ToString();
                Console.WriteLine(msg);
                throw e;
            }
            finally
            {
                myAccessConn.Close();
                myAccessConn.Dispose();
            }
            //      DataTable tbl = myDataSet.Tables[tableName];
            //    return tbl;
        }

        public DataTable Fill(string tableName, string sql, DataSet myDataSet)
        {

            string strAccessSelect = sql;

            var myAccessConn = GetConnection(ConnectionString);
            var myAccessCommand = GetCommand(sql, myAccessConn);
            var myDataAdapter = GetAdapter(myAccessCommand);

            //Console.WriteLine(sql);
            this.lastSqlCommand = sql;
            SqlCommands.Add(sql);
            try
            {
                myAccessConn.Open();
                myDataAdapter.Fill(myDataSet, tableName);
            }
            catch (Exception e)
            {
                string msg = "Error reading from database " + sql + " Exception " + e.ToString();
                Console.WriteLine(msg);
                throw e;
            }
            finally
            {
                myAccessConn.Close();
            }
            DataTable tbl = myDataSet.Tables[tableName];
            return tbl;
        }



        public override int RunSqlCommand(string sql)
        {
            return this.RunSqlCommand(sql, ConnectionString);
        }
        /// <summary>
        /// runs sql command.
        /// returns number of rows affected.
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int RunSqlCommand(string sql, string SqlConnString)
        {
            int rval = 0;
            this.lastMessage = "";
            using (var myConnection = GetConnection(SqlConnString))
            {
                myConnection.Open();
                using (var myCommand = GetCommand())
                {
                    DbTransaction myTrans;

                    // Start a local transaction
                    myTrans = myConnection.BeginTransaction(IsolationLevel.ReadCommitted);
                    // Assign transaction object for a pending local transaction
                    myCommand.Connection = myConnection;
                    myCommand.Transaction = myTrans;

                    try
                    {
                        myCommand.CommandText = sql;
                        rval = myCommand.ExecuteNonQuery();
                        myTrans.Commit();
                        this.lastSqlCommand = sql;
                        SqlCommands.Add(sql);
                    }
                    catch (Exception e)
                    {
                        myTrans.Rollback();
                        Console.WriteLine(e.ToString());
                        Console.WriteLine("Error running " + sql);
                        this.lastMessage = e.ToString();
                        throw e;
                    }
                    finally
                    {
                        myConnection.Close();
                    }
                    return rval;
                }
            }
        }
        ///
        ///http://stackoverflow.com/questions/10797011/sql-lite-data-types-c-sharp
        ///
        public override string PortableDateString(DateTime t, string fmt)
        {
#if !NETCOREAPP2_0
// TO DO -- ToUnixEpoch ??
            if ( UnixTimeStamps)
                return System.Data.SQLite.SQLiteConvert.ToUnixEpoch(t).ToString();
#endif
            return "datetime(" + base.PortableDateString(t, fmt) + ")";
        }

        public override void Vacuum()
        {
          var i = RunSqlCommandNonTransaction("vacuum");
        }


        public override string PortableTableName(string tableName)
        {
            var rval = tableName;
            if (tableName.Trim().IndexOf(" ") >= 0
               || tableName.Trim().IndexOf("-") >= 0)
            {
                rval = "[" + tableName + "]";
            }
            return rval;
        }

        public override string PortableWhereBool(bool p)
        {
          return p ? "1" : "0";
        }

        public override double SpaceUsedGB( )
        {
            if(File.Exists(FileName))
            {
                FileInfo fi = new FileInfo(FileName);
                return fi.Length / 1024.0 / 1024.0 / 1024.0;
            }
            return 0;
        }
    }
}
