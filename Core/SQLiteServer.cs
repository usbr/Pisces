using System;
using System.Collections;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;

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
            System.Data.SqlClient.SqlConnection.ClearAllPools();
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


        public void CloseAllConnections()
        {//http://stackoverflow.com/a/24501130/2333687
            SQLiteConnection.ClearAllPools();
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

        ///// <summary>
        ///// returns DataTable from stored procedure.
        ///// </summary>
        ///// <param name="storeProcedure"></param>
        ///// <returns></returns>
        //public DataTable TableFromProcs(SQLiteCommand cmd)
        //{
        //    DataTable table = new DataTable();
        //    cmd.Connection = new SQLiteConnection(this._connectionString);
        //    cmd.Connection.Open();
        //    System.Data.SQLite.SQLiteDataReader reader = cmd.ExecuteReader();

        //    if ((table.Columns.Count == 0))
        //    {
        //        table.TableName = cmd.CommandText;
        //        for (int i = 0; (i < reader.FieldCount); i = i + 1
        //          )
        //        {
        //            System.Type type = reader.GetFieldType(i);
        //            string name = reader.GetName(i);
        //            table.Columns.Add(name, type);
        //        }
        //    }
        //    table.Clear();
        //    int result = 0;
        //    for (; reader.Read(); result = result + 1)
        //    {
        //        System.Data.DataRow row = table.NewRow();
        //        object[] rowdata = new object[reader.FieldCount];
        //        reader.GetValues(rowdata);
        //        row.ItemArray = rowdata;
        //        table.Rows.Add(row);
        //    }
        //    reader.Close();

        //    return table;
        //}


       

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


        public object ExecuteScalarCmd(SQLiteCommand sqlCmd)
        {
            this.SqlCommands.Add(sqlCmd.CommandText);
            // Validate Command Properties
            if (ConnectionString == string.Empty)
                throw (new ArgumentOutOfRangeException("ConnectionString"));

            if (sqlCmd == null)
                throw (new ArgumentNullException("sqlCmd"));

            Object result = null;

            using (SQLiteConnection cn = new SQLiteConnection(ConnectionString))
            {
                sqlCmd.Connection = cn;
                cn.Open();
                result = sqlCmd.ExecuteScalar();
            }



            return result;
        }
        public object ExecuteScalarCmd(string sql)
        {
            SQLiteCommand sqlCmd = new SQLiteCommand(sql);
            return ExecuteScalarCmd(sqlCmd);
        }
        public void AddParamToSQLCmd(SQLiteCommand sqlCmd, string paramId, DbType sqlType, int paramSize, ParameterDirection paramDirection, object paramvalue)
        {
            // Validate Parameter Properties
            if (sqlCmd == null)
                throw (new ArgumentNullException("sqlCmd"));
            if (paramId == string.Empty)
                throw (new ArgumentOutOfRangeException("paramId"));

            // Add Parameter
            SQLiteParameter newSqlParam = new SQLiteParameter();
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
            SQLiteConnection myConnection = new SQLiteConnection(ConnectionString);
            myConnection.Open();
            SQLiteCommand myCommand = new SQLiteCommand();
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
            string sql = "select  * from " + dataTable.TableName + " where 2=1";
            return SaveTable(dataTable, sql);

        }

        //public override int InsertTable(DataTable dataTable)
        //{
        //    //if (DateTime.Now.Month == 2) ;
        //    //return SaveTable(dataTable);

        //    //http://sqlite.phxsoftware.com/forums/t/134.aspx
        //    Performance perf = new Performance();
        //    Logger.WriteLine("InsertTable " + dataTable.TableName);
        //    string sql = "select  * from " + dataTable.TableName + " where 2=1";
        //    DataSet myDataSet = new DataSet();
        //    myDataSet.Tables.Add(dataTable.TableName);

        //    SQLiteConnection conn = new SQLiteConnection(ConnectionString);
        //    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
        //    int recordCount = 0;

        //    try
        //    {
        //            conn.Open();
        //        SQLiteTransaction tran = conn.BeginTransaction();

        //            SQLiteDataAdapter da = new SQLiteDataAdapter(cmd);
        //            SQLiteCommandBuilder bld = new SQLiteCommandBuilder(da);
        //            this.lastSqlCommand = sql;
        //            SqlCommands.Add(sql);

        //            da.InsertCommand = (SQLiteCommand)((ICloneable)bld.GetInsertCommand()).Clone();
        //            bld.DataAdapter = null; // prevent callbacks
        //            da.Fill(myDataSet, dataTable.TableName);
        //            recordCount = da.Update(dataTable);
        //            tran.Commit();

        //    }finally
        //    {
        //        if( conn != null)
        //          conn.Close();
        //    }

        //    Logger.WriteLine("Saved " + recordCount + " records in " + perf.ElapsedSeconds + "seconds");
        //    return recordCount;
        //}
        public override int SaveTable(DataTable dataTable, string sql)
        {
            Performance perf = new Performance();
            Logger.WriteLine("Saving " + dataTable.TableName);
            DataSet myDataSet = new DataSet();
            myDataSet.Tables.Add(dataTable.TableName);

            SQLiteConnection conn = new SQLiteConnection(ConnectionString);
            SQLiteCommand myAccessCommand = new SQLiteCommand(sql, conn);
            SQLiteDataAdapter myDataAdapter = new SQLiteDataAdapter(myAccessCommand);
            SQLiteCommandBuilder karlCB = new SQLiteCommandBuilder(myDataAdapter);

            this.lastSqlCommand = sql;
            SqlCommands.Add(sql);
            int recordCount = 0;
            //baseline     Saved 1000 records in 6.938seconds 
            // transaction Saved 1000 records in 0.052seconds

            //baseline Saved 50000 records in 1.659seconds
            //(use Insert only) Saved 50000 records in 1.194seconds
            
            try
            {
                conn.Open();
                var dbTrans =  conn.BeginTransaction();
                myDataAdapter.Fill(myDataSet, dataTable.TableName);
                recordCount = myDataAdapter.Update(dataTable);
                dbTrans.Commit();
            }
            finally
            {
                if (conn != null)
                    conn.Close();
            }

            Logger.WriteLine("Saved "+recordCount+" records in " + perf.ElapsedSeconds + "seconds");
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

            using (SQLiteConnection myAccessConn = new SQLiteConnection(ConnectionString))
            {
                //myAccessConn.ConnectionTimeout = 30;
                using (SQLiteCommand myAccessCommand = new SQLiteCommand(strAccessSelect, myAccessConn))
                {
                    myAccessCommand.CommandTimeout = myAccessConn.ConnectionTimeout;

                    SQLiteDataAdapter myDataAdapter = new SQLiteDataAdapter(myAccessCommand);
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

            var myAccessConn = new SQLiteConnection(ConnectionString);
            var myAccessCommand = new SQLiteCommand(sql, myAccessConn);
            var myDataAdapter = new SQLiteDataAdapter(myAccessCommand);

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

            SQLiteConnection myAccessConn = new SQLiteConnection(ConnectionString);
            SQLiteCommand myAccessCommand = new SQLiteCommand(strAccessSelect, myAccessConn);
            SQLiteDataAdapter myDataAdapter = new SQLiteDataAdapter(myAccessCommand);

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
            using (SQLiteConnection myConnection = new SQLiteConnection(SqlConnString))
            {
                myConnection.Open();
                using (SQLiteCommand myCommand = new SQLiteCommand())
                {
                    SQLiteTransaction myTrans;

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
            if( UnixTimeStamps)
                return System.Data.SQLite.SQLiteConvert.ToUnixEpoch(t).ToString();

            return "datetime(" + base.PortableDateString(t, fmt) + ")";
        }

        public override void Vacuum()
        {
          var i = RunSqlCommandNonTransaction("vacuum");
        }
    }
}
