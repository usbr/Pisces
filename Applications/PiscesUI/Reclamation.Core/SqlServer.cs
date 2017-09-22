using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;

namespace Reclamation.Core
{
  /// <summary>
  /// Basic SQL and DataTable operations 
  /// for SQLServer
  /// Karl Tarbet
  /// </summary>
  public class SqlServer:BasicDBServer
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
    public SqlServer()
    {
      
    }
    public SqlServer(string connString)
    {
     ConnectionString=connString;
    }

    public SqlServer(string server, string database)
    {
        // TODO: Complete member initialization
        this.server = server;
        this.database = database;
        ConnectionString = "Data Source=" + server + ";Integrated Security=SSPI;Initial Catalog=" + database;
    }

      public void CloseAllConnections()
    {//http://stackoverflow.com/questions/444750/spring-net-drop-all-adotemplate-connections
          SqlConnection.ClearAllPools();
          // if any connections were being used at the time of the clear, hopefully waiting
          // 3 seconds will give them time to be released and we can now close them as well
          //System.Threading.Thread.Sleep(3000);
          //clear again
          //SqlConnection.ClearAllPools();
      }

    SqlServerAdmin _admin;
    private string server;
    private string database;
    public SqlServerAdmin Admin
    {
      get{
        if( this._admin ==null)
        {
          _admin = new SqlServerAdmin(this);
        }
        return _admin;
        }
    }
    private static string GlobalConnectionString
    {
      set { _staticConnectionString=value;}
      get { return _staticConnectionString;}
    }
    /// <summary>
    /// Gets the Sqlconnection string value.
    /// SqlConnString is read only.  To set the connectionstring edit your App.config
    ///<add key="SqlConnectionString" value="workstation id=KTARBET;packet size=4096;integrated security=SSPI;data source=KTARBET;persist security info=True;Database=northwind" /> 
    /// </summary>
      public override string ConnectionString
    {
      // if the connection string is not defined use the
      // global connection string.
      // if the global connection string is not defined
      // try the app.config file.
      get
      {
        if( _connectionString == null)
        {

          _connectionString = _staticConnectionString; //global
          if( _connectionString == null)
              _connectionString = ConfigurationManager.AppSettings["SqlConnectionString"];
          if ( _connectionString == null)
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
        string pwd =GetConnectionStringToken(_connectionString,"Pwd");
        // need to strip out the password so it does not get logged.
        if( pwd != "")
          cleanConnStr =  cleanConnStr.Replace(pwd,"**********");
        
        SqlCommands.Add(cleanConnStr);
       
      }
    }


      public override string DataSource
      {
          get
          {
              // .."Server=" + server + ";Database=" + database +
              var cs = ConnectionString;
              var rval = ConnectionStringUtility.GetToken(cs, "data source", "") + ":";
              rval += ConnectionStringUtility.GetToken(cs, "initial catalog", "");
              return rval;

          }
      }

    /// <summary>
    /// returns DataTable from stored procedure.
    /// </summary>
    /// <param name="storeProcedure"></param>
    /// <returns></returns>
    public DataTable TableFromProc(SqlCommand cmd)
    {
        DataTable table = new DataTable();
        cmd.Connection = new SqlConnection(this._connectionString);
        cmd.Connection.Open();
        System.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader();

        if ((table.Columns.Count == 0))
        {
            table.TableName = cmd.CommandText;
            for (int i = 0; (i < reader.FieldCount); i = i + 1
              )
            {
                System.Type type = reader.GetFieldType(i);
                string name = reader.GetName(i);
                table.Columns.Add(name, type);
            }
        }
        table.Clear();
        int result = 0;
        for (; reader.Read(); result = result + 1)
        {
            System.Data.DataRow row = table.NewRow();
            object[] rowdata = new object[reader.FieldCount];
            reader.GetValues(rowdata);
            row.ItemArray = rowdata;
            table.Rows.Add(row);
        }
        reader.Close();

        return table;
    }


    public string ServerName
    {
          get 
      {
          string sql = "select @@servername";
          sql =" select convert(char(20),ServerProperty('servername'))";
        DataTable tbl = Table("servername",sql);
        if( tbl.Rows[0][0] == DBNull.Value)
        {
          return "";
        }
        else
          return tbl.Rows[0][0].ToString();
      }
    }
    
    public string DatabaseName
    {
      get 
      {
        string sql = "select db_name()";
        return ExecuteScalarCmd(sql).ToString();
      }
    }
    public string InstanceName
    {
      get 
      {//  Console.WriteLine(ExecuteScalarCmd());
        string sql = "SELECT SERVERPROPERTY('InstanceName')";
        object o = ExecuteScalarCmd(sql);
        if( o == DBNull.Value)
          return "default"; // default instance
        return o.ToString();
      }
    }
    public string FullDatabaseName
    {
      get
      {
        return ServerName+"\\"+InstanceName+"\\"+DatabaseName;
      }
    }
    /// <summary>
    /// Get a variable in connection string.
    /// for example get the password
    /// </summary>
    /// <returns></returns>
    public static string GetConnectionStringToken( string connStr,string tokenName)
    {
      string s = connStr;
      int tokenLength = tokenName.Length+1; // add one for '='
      int idx = s.IndexOf(tokenName+"=");
      if(idx <0)
        return "";
      int idx2 = s.IndexOf(";",idx+1); 
      if( idx2 <0)
        return s.Substring(idx+tokenLength);
      return s.Substring(idx+tokenLength,idx2-(idx+tokenLength));
    }
    /// <summary>
    /// Sets a variable in connection string.
    /// for example set the password
    /// </summary>
    /// <param name="tokenName"></param>
    /// <returns></returns>
    public static string ModifiyConnectionStringStringToken(string connStr, string tokenName, string newValue)
    {
      // insert new password in connection string.
      string prevValue = GetConnectionStringToken(connStr,tokenName);
      string find =  tokenName+"="+prevValue;
      string replace =tokenName+"="+newValue;
      int idx = connStr.IndexOf(find);
      if(idx <0)
        throw new Exception("Error setting "+tokenName);
      string rval = connStr.Replace(find,replace);
      return rval;
    }
    /// <summary>
    /// returns username for this connection.
    /// </summary>
    /// <returns></returns>
    public string Username()
    {
      string sql = "Select user";
      DataTable tbl = Table("user",sql);
      if(tbl.Rows.Count==0)
        return "";
      return tbl.Rows[0][0].ToString();
    }

   

    /// <summary>
    /// gets a list of all 'base tables'
    /// </summary>
    /// <returns></returns>
    public override string[] TableNames()
    {
      DataTable tbl = Table("schema","select * FROM Information_Schema.Tables  where Table_Type = 'BASE TABLE'  and table_name <> 'dtproperties'");
      string[] rval = new string[tbl.Rows.Count];
      for(int i=0; i<tbl.Rows.Count; i++)
      {
        rval[i]= tbl.Rows[i]["table_name"].ToString();
      }
      return rval;
    }

    
    public object ExecuteScalarCmd(SqlCommand sqlCmd) 
    {
      this.SqlCommands.Add(sqlCmd.CommandText);
      // Validate Command Properties
      if (ConnectionString == string.Empty)
        throw (new ArgumentOutOfRangeException("ConnectionString"));

      if (sqlCmd== null)
        throw (new ArgumentNullException("sqlCmd"));

      Object result = null;

      using (SqlConnection cn = new SqlConnection(ConnectionString)) 
      {
        sqlCmd.Connection = cn;
        cn.Open();
        result = sqlCmd.ExecuteScalar();
      }

      

      return result;
    }
    public object ExecuteScalarCmd(string sql) 
    {
      SqlCommand sqlCmd =new SqlCommand(sql);
      return ExecuteScalarCmd(sqlCmd); 
    }
    public void AddParamToSQLCmd(SqlCommand sqlCmd, string paramId, SqlDbType sqlType, int paramSize, ParameterDirection paramDirection, object paramvalue) 
    {
      // Validate Parameter Properties
      if (sqlCmd== null)
        throw (new ArgumentNullException("sqlCmd"));
      if (paramId == string.Empty)
        throw (new ArgumentOutOfRangeException("paramId"));

      // Add Parameter
      SqlParameter newSqlParam = new SqlParameter();
      newSqlParam.ParameterName= paramId;
      newSqlParam.SqlDbType = sqlType;
      newSqlParam.Direction = paramDirection;

      if (paramSize > 0)
        newSqlParam.Size=paramSize;

      if (paramvalue != null)
        newSqlParam.Value = paramvalue;

      sqlCmd.Parameters.Add (newSqlParam);
    }

     
    public bool DeleteDataBase(string database_name)
    {
      string sql = "DROP DATABASE "+database_name;
      if(  this.RunSqlCommandNonTransaction(sql))
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
      SqlConnection myConnection = new SqlConnection(ConnectionString);
      myConnection.Open();
      SqlCommand myCommand = new SqlCommand();
      myCommand.Connection = myConnection;
      bool rval= false;
      try
      {
        myCommand.CommandText = sql;
        if( myCommand.ExecuteNonQuery()>0)
          rval = true;
        this.lastSqlCommand = sql;
        SqlCommands.Add(sql);
      }
      catch(Exception e)
      {
        Console.WriteLine(e.ToString());
        Console.WriteLine("Error running "+sql);
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
      string sql = "SELECT OBJECT_ID('"+tableName+"')";
      DataTable tbl = Table("exists",sql);
      if( tbl.Rows[0][0] == DBNull.Value)
        return false;
      else
        return true;
    }

    //Saves DataTable in database
    public override  int SaveTable(DataTable dataTable)
    {
      string sql = "select top 0 * from "+dataTable.TableName;
      return SaveTable(dataTable,sql);

    }

     public override int SaveTable(DataTable dataTable, string sql)
    {
      Console.WriteLine("Saving "+dataTable.TableName);
            Performance perf = new Performance();
            DataSet myDataSet = new DataSet();
      myDataSet.Tables.Add(dataTable.TableName);


      using (SqlConnection conn = new SqlConnection(ConnectionString))
      {
          using (SqlCommand cmd = new SqlCommand(sql, conn))
          {
              conn.Open();
              SqlDataAdapter da = new SqlDataAdapter(cmd);
              SqlCommandBuilder karlCB = new SqlCommandBuilder(da);
              SqlTransaction tran = conn.BeginTransaction();
              cmd.Transaction = tran;

              this.lastSqlCommand = sql;
              SqlCommands.Add(sql);

                    da.UpdateBatchSize = 0;
              int recordCount = 0;
              try
              {   // call Fill method only to make things work. (we ignore myDataSet)
                  da.Fill(myDataSet, dataTable.TableName);
                  recordCount = da.Update(dataTable);
                  tran.Commit();
              }
              finally
              {
                  conn.Close();
              }

                    Logger.WriteLine("Saved " + recordCount + " records in " + perf.ElapsedSeconds + "seconds");
                    return recordCount;
          }
      }
    }

    
    /// <summary>
    /// returns all rows from a table.
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
      public override DataTable Table(string tableName)
    {
      return Table(tableName,"select * from "+tableName);
    }

    /// <summary>
    /// returns table using sql
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="sql"></param>
    /// <returns></returns>
      public override DataTable Table(string tableName, string sql)
    {
      return Table(tableName,sql,true);
    }
    /// <summary>
    /// returns table using sql
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="sql"></param>
    /// <param name="AcceptChangesDuringFill"></param>
    /// <returns></returns>
    public DataTable Table(string tableName,string sql, bool AcceptChangesDuringFill)
    {

      string strAccessSelect = sql;
      
      SqlConnection myAccessConn = new SqlConnection(ConnectionString);
      //myAccessConn.ConnectionTimeout = 30;
      SqlCommand myAccessCommand = new SqlCommand(strAccessSelect,myAccessConn);
      myAccessCommand.CommandTimeout = myAccessConn.ConnectionTimeout;

      SqlDataAdapter myDataAdapter = new SqlDataAdapter(myAccessCommand);
      myDataAdapter.AcceptChangesDuringFill = AcceptChangesDuringFill;
      //Console.WriteLine(sql);
      this.lastSqlCommand = sql;
      SqlCommands.Add(sql);
      DataSet myDataSet = new DataSet();
      try
      {
        myAccessConn.Open();
				
        myDataAdapter.Fill(myDataSet,tableName);
      }
      catch(Exception e)
      {
        string msg = "Error reading from database "+sql +" Exception "+e.ToString();
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

    public override void FillTable(DataTable dataTable, string sql)
    {
        base.SqlCommands.Add("Fill(" + dataTable.TableName + ")");

        var myAccessConn = new SqlConnection(ConnectionString);
        var myAccessCommand = new SqlCommand(sql, myAccessConn);
        var myDataAdapter = new SqlDataAdapter(myAccessCommand);

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

    public DataTable Fill(string tableName,string sql, DataSet myDataSet)
    {

      string strAccessSelect = sql;
      
      SqlConnection myAccessConn = new SqlConnection(ConnectionString);
      SqlCommand myAccessCommand = new SqlCommand(strAccessSelect,myAccessConn);
      SqlDataAdapter myDataAdapter = new SqlDataAdapter(myAccessCommand);
		
      //Console.WriteLine(sql);
      this.lastSqlCommand = sql;
      SqlCommands.Add(sql);
      try
      {
        myAccessConn.Open();
        myDataAdapter.Fill(myDataSet,tableName);
      }
      catch(Exception e)
      {
        string msg = "Error reading from database "+sql +" Exception "+e.ToString();
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
      return this.RunSqlCommand(sql,ConnectionString);
    }
    /// <summary>
    /// runs sql command.
    /// returns number of rows affected.
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    public int RunSqlCommand(string sql, string SqlConnString)
    {
      int rval =0;
      this.lastMessage = "";
      SqlConnection myConnection = new SqlConnection(SqlConnString);
      myConnection.Open();
      SqlCommand myCommand = new SqlCommand();
      SqlTransaction myTrans;

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
      catch(Exception e)
      {
        myTrans.Rollback();
        Console.WriteLine(e.ToString());
        Console.WriteLine("Error running "+sql);
        this.lastMessage = e.ToString();
        throw e;
      }
      finally
      {
        myConnection.Close();
      }
      return rval;
    }
		

    public void PrintDebugInfo()
    {
      Console.WriteLine(ExecuteScalarCmd("SELECT SERVERPROPERTY('InstanceName')"));
      Console.WriteLine(ExecuteScalarCmd("SELECT SERVERPROPERTY('MachineName')"));
      Console.WriteLine(ExecuteScalarCmd("select db_name()"));
      Console.WriteLine(ExecuteScalarCmd("select @@ServerName"));
      Console.WriteLine(ExecuteScalarCmd("select @@Version"));
      Console.WriteLine(ExecuteScalarCmd("select System_user"));

    }

    /// <summary>
    /// Reads a blob from sybase table.  
    /// Select statment must return single row and column
    /// </summary>
    public void SaveBlobToFile(string outputFilename, string selectStatement)
    {
        SqlConnection conn = new SqlConnection(ConnectionString);

        SqlCommand cmd = new SqlCommand(selectStatement, conn);
        //"SELECT Filename, FileData FROM "+tableName+" where FileID ='"+fileID+"'", conn);

        FileStream fs;                          // Writes the BLOB to a file
        BinaryWriter bw;                        // Streams the BLOB to the FileStream object.

        int bufferSize = 100;                   // Size of the BLOB buffer.
        byte[] outbyte = new byte[bufferSize];  // The BLOB byte[] buffer to be filled by GetBytes.
        long retval;                            // The bytes returned from GetBytes.
        long startIndex = 0;                    // The starting position in the BLOB output.

        // Open the connection and read data into the DataReader.
        conn.Open();
        SqlDataReader myReader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

        while (myReader.Read())
        {
            // string tmpfilename = myReader.GetString(0);  

            // Create a file to hold the output.
            fs = new FileStream(outputFilename, FileMode.OpenOrCreate, FileAccess.Write);
            bw = new BinaryWriter(fs);

            // Reset the starting byte for the new BLOB.
            startIndex = 0;

            // Read the bytes into outbyte[] and retain the number of bytes returned.
            retval = myReader.GetBytes(0, startIndex, outbyte, 0, bufferSize);

            // Continue reading and writing while there are bytes beyond the size of the buffer.
            while (retval == bufferSize)
            {
                bw.Write(outbyte);
                bw.Flush();

                // Reposition the start index to the end of the last buffer and fill the buffer.
                startIndex += bufferSize;
                retval = myReader.GetBytes(0, startIndex, outbyte, 0, bufferSize);
            }

            // Write the remaining buffer.
            bw.Write(outbyte, 0, (int)retval);
            bw.Flush();

            // Close the output file.
            bw.Close();
            fs.Close();
        }

        // Close the reader and the connection.
        myReader.Close();
        conn.Close();

    }

    public override int InsertTable(DataTable table)
    {
        for (int i = 0; i < table.Rows.Count; i++)
        {
           
            if( table.Rows[i].RowState == DataRowState.Unchanged)
                table.Rows[i].SetAdded();
        }
        return SaveTable(table);

    }

  }
}
