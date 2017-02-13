using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Configuration;
using Npgsql;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace Reclamation.Core
{
  /// <summary>
  /// Basic SQL and DataTable operations 
  /// for PostgreSQL
  /// Karl Tarbet
  /// http://npgsql.projects.postgresql.org/docs/manual/UserManual.html
  /// </summary>
  public class PostgreSQL:BasicDBServer
  {
    /// <summary>
    /// last command that was sent to server.
    /// </summary>
    protected string lastSqlCommand;
   // string lastMessage = "";

      /// <summary>
      /// Creates a BasicDBServer object.
      /// empty parameters userName and databaseName can be set in Config file
      /// </summary>
      /// <param name="databaseName"></param>
      /// <returns></returns>
    public static BasicDBServer GetPostgresServer(string databaseName="", 
        string serverName="", string userName="", string password="") 
    {
        string server = serverName;
        if(server == "") // use config file.
          server = ConfigurationManager.AppSettings["PostgresServer"];

        if( databaseName == "")
        {
            databaseName = ConfigurationManager.AppSettings["PostgresDatabase"];
        }

        if (LinuxUtility.IsLinux())
        {
            if (userName == "")
            {
                userName = ConfigurationManager.AppSettings["PostgresUser"];
            }
        }
        else{ // windows
            if (userName == "")
            {
                userName = WindowsUtility.GetShortUserName();
            }
        }
           
        string cs = "Server=" + server + ";Database=" + databaseName + ";User id=" + userName + ";";

        
        if( password == "")
        { //check config file for password
            password = ConfigurationManager.AppSettings["PostgresPassword"];

            if (password == null)
                password = "";

            if (File.Exists(password)) // might be file with password
                password = File.ReadAllText(password);
        }

        if (password.Length > 0)
        {
            cs += "password=" + password + ";";
        }


        var msg = cs;
        if (password.Length > 0)
            msg = cs.Replace("password="+password, "password="+"xxxxx");
        Logger.WriteLine(msg);

       return new PostgreSQL(cs);

    }


    public override string DataSource
    {
        get {
            // .."Server=" + server + ";Database=" + database +
            var cs = ConnectionString;
            var rval = ConnectionStringUtility.GetToken(cs, "Server", "") + ":";
            rval += ConnectionStringUtility.GetToken(cs, "Database", "");
            return rval;

        }
    }

        

   
    public override BasicDBServer NewConnection(int fileIndex)
    {
        return base.NewConnection(fileIndex);
    }

    public PostgreSQL(string cs)
    {
        //SearchPath=wtr..
        //searchpath=schemanamehere.. 
        //NpgsqlConnection conn = new NpgsqlConnection(cs);
        //"Server=127.0.0.1;Database=eeeeee;User id=npgsql_tests;password=npgsql_tests;");
        ConnectionString = cs;

        Name = ConnectionStringUtility.GetToken(cs, "Database","");
        var pw = ConnectionStringUtility.GetToken(cs, "password","");
        var logSafeConnectionString = cs;
        if( pw.Trim() != "")
            logSafeConnectionString = logSafeConnectionString.ToLower().Replace(pw,"*******");

        SqlCommands.Add(logSafeConnectionString);
    }

    public static void ClearAllPools()
    {
           NpgsqlConnection.ClearAllPools();
    }
    public void CloseAllConnections()
    {//http://stackoverflow.com/questions/444750/spring-net-drop-all-adotemplate-connections
        NpgsqlConnection.ClearAllPools();
        // if any connections were being used at the time of the clear, hopefully waiting
        // 3 seconds will give them time to be released and we can now close them as well
        //System.Threading.Thread.Sleep(3000);
        //clear again
        //SqlConnection.ClearAllPools();
    }

    public bool DatabaseExists(string dbName)
    {
        var tmp = Table("exists", " select datname from pg_database where datname = '"+dbName+"'");
        return tmp.Rows.Count == 1;
    }

    //public static string Convert
    //FOR r IN (SELECT * FROM ( VALUES ('datetime', 'timestamp with time zone', 'CURRENT_TIMESTAMP'),     
    //        ('bit', 'boolean', 'true'),
    //        ('varchar(max)', 'text', ''), 
    //        ('nvarchar', 'varchar', ''), 
    //        ('tinyint','smallint', '0') ,
    //        ('[int] identity(1,1)', 'serial', NULL)
      /// <summary>
      /// vacuums all tables
      /// </summary>
    public override void Vacuum() 
    {
        RunSqlCommand("Vacuum",this.ConnectionString,false);
    }

    /// <summary>
    /// Creates new database.  Existing file will be overwritten
    /// </summary>
    /// <param name="filename"></param>
    public void CreateNewDatabase(string dbname)
    {
       RunSqlCommand("Create Database "+dbname,this.ConnectionString,false);
    }

    /// <summary>
    /// Creates new database.  Existing file will be overwritten
    /// </summary>
    /// <param name="filename"></param>
    public void DropDatabase(string dbname)
    {
        RunSqlCommand("Drop Database " + dbname, this.ConnectionString, false);
    }

    private string GetDefaultSchema()
    {
        var tbl = Table("x", "show search_path");
        var rval = tbl.Rows[0][0].ToString();
        Console.WriteLine("search_path = '"+rval+"'");
        return rval;
    }
    /// <summary>
    /// gets a list of all 'base tables'
    /// </summary>
    /// <returns></returns>
    public override string[] TableNames()
    {
        string sql = "Select table_Name from Information_schema.Tables where table_schema = '"+GetDefaultSchema()+"' order by table_name ";
      DataTable tbl = Table("schema",sql);
      string[] rval = new string[tbl.Rows.Count];
      for(int i=0; i<tbl.Rows.Count; i++)
      {
          rval[i] = tbl.Rows[i]["table_Name"].ToString();
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
        if (tableName.Trim() == "")
            return false;
      string sql = "Select table_name from Information_schema.Tables "
        +" WHERE table_name = '"+tableName +"'";
      DataTable tbl = Table("exists",sql);
      if( tbl.Rows.Count >0)
        return true;
      else
        return false;
    }



    /// <summary>
    /// Create a sql command that will return no data
    /// from a given table.
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    private string GetEmptyTableSqlCommand(DataTable table)
    {
        // return "Select * from \"" + table.TableName.ToLower() + "\" where 2=1";
        string tableName = table.TableName;
        string columns = "";
        for (int i = 0; i < table.Columns.Count; i++)
        {
            columns += " \"" + table.Columns[i].ColumnName.ToLower() + "\" ";
            if (i != table.Columns.Count - 1)
                columns += ", ";
        }
        string sql = "select " + columns + " from " + tableName + " Where 2=1";
        return sql;
    }


    //Saves DataTable in database
    public override int SaveTable(DataTable dataTable)
    {
        string sql = GetEmptyTableSqlCommand(dataTable);
        return SaveTable(dataTable, sql);
    }


    public override int InsertTable(DataTable table)
    {

        var newTable = table.Clone();
        for (int i = 0; i < table.Rows.Count; i++)
        {
            newTable.Rows.Add(table.Rows[i].ItemArray);
        }
        string sql = GetEmptyTableSqlCommand(newTable);
        return SaveTable(newTable, sql);
        //return SaveTable1(newTable, sql, true); // saveTable1 is slow.. not in transaction

    }

    public bool MapToLowerCase = true;
    public bool SetAllValuesInCommandBuilder = false;

    public override int SaveTable(DataTable dataTable, string sql)
    {

        //Logger.WriteLine("Save Table with transaction");
        Performance perf = new Performance();
        Logger.WriteLine("Saving " + dataTable.TableName);
        //Logger.WriteLine(sql);
        DataSet myDataSet = new DataSet();
        myDataSet.Tables.Add(dataTable.TableName);

        NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
        NpgsqlCommand myAccessCommand = new NpgsqlCommand(sql, conn);
        NpgsqlDataAdapter da = new NpgsqlDataAdapter(myAccessCommand);
        NpgsqlCommandBuilder cb = new NpgsqlCommandBuilder(da);
        da.UpdateCommand = cb.GetUpdateCommand();
        da.InsertCommand = cb.GetInsertCommand();
        da.DeleteCommand = cb.GetDeleteCommand();

        cb.SetAllValues = SetAllValuesInCommandBuilder;
        cb.ConflictOption = ConflictOption.OverwriteChanges; // this fixes System.InvalidCastException : Specified cast is not valid.
                                                              // when reserved word  (group) was a column name

        if (MapToLowerCase)
        {
            var map = da.TableMappings.Add(dataTable.TableName.ToLower(), dataTable.TableName);
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                var cn = dataTable.Columns[i].ColumnName;
                map.ColumnMappings.Add(cn.ToLower(), cn);
            }
            //PrintMapping(da);
        }
        
        SqlCommands.Add(sql);
        int recordCount = 0;
        //da.RowUpdating += myDataAdapter_RowUpdating;
        
        try
        {
            conn.Open();
            var dbTrans = conn.BeginTransaction();
            da.Fill(myDataSet, dataTable.TableName);
            
            recordCount = da.Update(dataTable);
            dbTrans.Commit(); 
        }
        finally
        {
            if (conn != null)
                conn.Close();
        }

        string msg = "Saved " + recordCount + " records ["+dataTable.TableName+"] " + perf.ElapsedSeconds + "seconds";
        Logger.WriteLine(msg,"ui");
        Console.WriteLine(msg);
        if (SqlCommands.Count > 5000)
        {
            SqlCommands.Clear();
        }
        return recordCount;
    }

    private void PrintMapping(NpgsqlDataAdapter da)
    {
        for (int i = 0; i < da.TableMappings.Count; i++)
        {
            var m = da.TableMappings[i];
            Logger.WriteLine("SourceTable:" + m.SourceTable);
            Logger.WriteLine("DataSetTable:" + m.DataSetTable);
            for (int j = 0; j < m.ColumnMappings.Count; j++)
			{
                Logger.WriteLine("SourceColumn:" + m.ColumnMappings[j].SourceColumn);
                Logger.WriteLine("DataSetColumn:" + m.ColumnMappings[j].DataSetColumn);
			}
            
        }

    }

    void myDataAdapter_RowUpdating(object sender, NpgsqlRowUpdatingEventArgs e)
    {
        Logger.WriteLine("Debug ");
        Logger.WriteLine(e.Command.CommandText);
        //NpgsqlCommand cmd = e.Command as NpgsqlCommand;

        for (int i = 0; i < e.Command.Parameters.Count; i++)
			{
             NpgsqlParameter p = e.Command.Parameters[i] as NpgsqlParameter;

                Logger.WriteLine("param "+i+":" + p.Value);
                Logger.WriteLine("SourceColumn", p.SourceColumn);
            //    Logger.WriteLine("param " + i + ":" + p.NpgsqlValue+"  ( NpgsqlValue )");	 
			}
        
    }



    //public override int SaveTable(DataTable dataTable, string sql)
    //{
    //    return SaveTable(dataTable, sql, false);
    //}


      /// <summary>
    /// SaveTable1 is SLOW -- not using a transaction.
      /// </summary>
      /// <param name="dataTable"></param>
      /// <param name="sql"></param>
      /// <param name="insert"></param>
      /// <returns></returns>
     private int SaveTable1(DataTable dataTable, string sql, bool insert=false)
    {
      base.SqlCommands.Add(sql);
      //Logger.WriteLine("Saving " + dataTable.TableName + "\n ");
      DataSet myDataSet = new DataSet();
      myDataSet.Tables.Add(dataTable.TableName);

      NpgsqlConnection myAccessConn = new NpgsqlConnection(ConnectionString);
      NpgsqlCommand myAccessCommand = new NpgsqlCommand(sql, myAccessConn);
        
      var da = new NpgsqlDataAdapter(myAccessCommand);
     // myDataAdapter.TableMappings.Add(dataTable.TableName.ToLower(), dataTable.TableName);
      var cb = new  NpgsqlCommandBuilder(da);
      cb.ConflictOption = ConflictOption.OverwriteChanges;
      da.InsertCommand = (NpgsqlCommand)cb.GetInsertCommand();

      //da.RowUpdated += da_RowUpdated;
      Logger.WriteLine(da.InsertCommand.CommandText);


         if( !insert)
      try
      {

          da.UpdateCommand = (NpgsqlCommand)cb.GetUpdateCommand();

      }
      catch (InvalidOperationException ioe)
      {
          Console.WriteLine(ioe.Message);
      }

     if(!insert)
      try
      {
          da.DeleteCommand = (NpgsqlCommand)cb.GetDeleteCommand();
      }
      catch (InvalidOperationException ioe)
      {
          Console.WriteLine(ioe.Message);
      }


      this.lastSqlCommand = sql;
      SqlCommands.Add(sql);
      myAccessConn.Open();
      int recordCount = 0;
      try
      {   // call Fill method only to make things work. (we ignore myDataSet)
          da.Fill(myDataSet, dataTable.TableName);
          recordCount = da.Update(dataTable);
      }
      catch (DBConcurrencyException e)
      {
          throw e;
           
      }
      finally
      {
        myAccessConn.Close();
      }
      return recordCount;
    }

     

    
    /// <summary>
    /// Returns an empty table
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public DataTable TableSchema(string tableName)
    {
      return Table(tableName,"select * from "+tableName+ " limit 0");
    }

    
    /// <summary>
    /// returns all rows from a table.
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
      public override DataTable Table(string tableName)
    {
        if (tableName.Trim().IndexOf(" ") > 0)
            tableName = "\"" + tableName + "\"";
      return Table(tableName,"select * from "+tableName+"");
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
        base.SqlCommands.Add("Table(" + sql + ")\n AcceptChangesDuringFill " + AcceptChangesDuringFill);
      string strAccessSelect = sql;
      
      var myAccessConn = new  NpgsqlConnection(ConnectionString);
        
      var myAccessCommand = new  NpgsqlCommand(strAccessSelect,myAccessConn);
      var myDataAdapter = new NpgsqlDataAdapter(myAccessCommand);
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
        string msg = "Error reading from database "+sql +" Exception "+e.Message;
        Logger.WriteLine(msg);
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
        if (dataTable.TableName == "")
        { 
            dataTable.TableName = "table1";
            
        }   
        base.SqlCommands.Add("Fill(" + dataTable.TableName + ")");
        string strAccessSelect = sql;

        var myAccessConn = new NpgsqlConnection(ConnectionString);
        var myAccessCommand = new NpgsqlCommand(strAccessSelect, myAccessConn);
        var myDataAdapter = new NpgsqlDataAdapter(myAccessCommand);

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
            myAccessConn.Close(); //
        }
    }


    public override int CreateTable(string sql)
    {
        Logger.WriteLine(sql);
        int rval = base.CreateTable(sql);

        string owner = ConfigurationManager.AppSettings["PostgresTableOwner"];
        if (owner == null || owner == "")
        {
            Logger.WriteLine("Warning:  PostgresTableOwner was not defined in the config file. Using owner = '"+Name+"'");
            owner = Name;
        }

        if( owner != "")
        {
         var m =  Regex.Match(sql,@"create\s+table\s+(?<table_name>\w+)", RegexOptions.IgnoreCase);
         if (m.Success)
         {
             var tableName = m.Groups["table_name"].Value;
             string cmd = "ALTER TABLE "+tableName+" OWNER TO "+owner;
             RunSqlCommand(cmd);

             var r = ConfigurationManager.AppSettings["PostgresTableReader"];
             if( r != null && r!= "")
             { // grant select for PostgresTableReader
                 cmd = "grant select on table "+tableName+ " to "+r;
                 RunSqlCommand(cmd);
             }
         }
         else
         {
             Logger.WriteLine("Error searching for table_name");
         }


        }

        
        return rval;
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
     int RunSqlCommand(string sql, string SqlConnString, bool useTransaction = true)
    {
       
      int rval =0;
      //this.lastMessage = "";
      var myConnection = new NpgsqlConnection(SqlConnString);
      myConnection.Open();
      var myCommand = new NpgsqlCommand();
      NpgsqlTransaction myTrans=null;

      myCommand.Connection = myConnection;
      if (useTransaction)
      {
          // Start a local transaction
          myTrans = myConnection.BeginTransaction(IsolationLevel.ReadCommitted);
          myCommand.Transaction = myTrans;
      }
      try
      {
        myCommand.CommandText = sql;
        rval = myCommand.ExecuteNonQuery();
        if (useTransaction)
            myTrans.Commit();
        //Logger.WriteLine(rval + " rows affected");
        this.lastSqlCommand = sql;
        SqlCommands.Add(sql);
      }
      catch(Exception e)
      {
          if( useTransaction)
              myTrans.Rollback();

        Logger.WriteLine(e.ToString());
        Logger.WriteLine("Error running "+sql);
        //this.lastMessage = e.ToString();
        throw e;
      }
      finally
      {
        myConnection.Close();
      }
      return rval;
    }

     public override string PortableTableName(string tableName)
     {
         var rval = tableName;
         if (tableName.Trim().IndexOf(" ") >= 0)
         {
             rval = "\"" + tableName + "\"";
         }
         return rval;
     }

     /// <summary>
     /// Returns character column type given size
     /// </summary>
     /// <param name="size"></param>
     /// <returns></returns>
     public override string PortableCharacterType(int size)
     {
         return " varchar(" + size + ") ";
     }


     public override string PortableDateTimeType()
     {
         return "timestamp without time zone"; 
     }



     public bool MemberOfRole(string rolename)
     {
         string username = ConnectionStringUtility.GetToken(ConnectionString, "User id", "");

         string sql = "select rolname from pg_user join pg_auth_members on (pg_user.usesysid=pg_auth_members.member)  "
   + " join pg_roles on (pg_roles.oid=pg_auth_members.roleid) where pg_user.usename='"
         + username + "' and  rolname ='" + rolename + "'";
         var tbl = Table("roles1", sql);
         return tbl.Rows.Count > 0;
     }


     public int CreateDatabase(string database_name, string owner)
     {
         string sql = "Create DATABASE " + database_name+" with owner = "+owner;
         return RunSqlCommand(sql,this.ConnectionString,false);
     }


     public int CreateSchema(string schemaName, string authorization)
     {
         string sql = "Create schema " + schemaName + " authorization " + authorization;
         return RunSqlCommand(sql);
     }

     public int SetSearchPath(string dbname, string searchPath)
     {
         
         string sql = "alter database " + dbname + " set search_path =" + searchPath;
         return RunSqlCommand(sql);
     }

     public override void Cleanup()
     {
         PostgreSQL.ClearAllPools(); // Hack to fix error on application exit
     }

        public override bool HasSavePrivilge(string tableName)
        {
            string sql = "select has_table_privilege('" + tableName + "', 'update')   ";
            var tbl = Table("a", sql);
            return tbl.Rows[0][0].ToString().ToLower() == "t";
        }
    }
}
