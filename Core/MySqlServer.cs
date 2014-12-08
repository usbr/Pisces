using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Configuration;
namespace Reclamation.Core
{
    public class MySqlServer:BasicDBServer
    {
        protected string lastSqlCommand;

        public MySqlServer(string server="localhost",string dbname="timeseries")
        {
           MySqlConnectionStringBuilder b = new MySqlConnectionStringBuilder();
            b.Server = server;
            b.UserID = GetWindowsUserName()+"@localhost";
            b.Password = GeneratePassword();
            b.Database = dbname;
            this.ConnectionString = b.ConnectionString;
            string msg = ConnectionString;
            if (b.Password.Length > 0)
                msg = msg.Replace("password=" + b.Password, "password=" + "xxxxx");
            Logger.WriteLine(msg);
            this.ConnectionString = "server=localhost;uid=root;" +
    "pwd=;database=timeseries;";
            Logger.WriteLine(ConnectionString);

        }


        private static string GeneratePassword()
        {
            string fileName = FileUtility.GetFileReference("mysql_key.txt");

            if (File.Exists(fileName))
            {
                return GetWindowsUserName() + File.ReadAllText(fileName);
            }
            else
            {
                Logger.WriteLine("Error:  missing mysql_key.txt");
                throw new FileNotFoundException("mysql_key.txt");
            }
        }
        public override string DataSource
        {
            get {
                MySqlConnectionStringBuilder b = new MySqlConnectionStringBuilder(ConnectionString);
                return b.Server + ":" + b.Database;
            }
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

        public DataTable Table(string tableName, string sql, bool AcceptChangesDuringFill)
        {
            string strAccessSelect = sql;

            MySqlConnection c = new MySqlConnection(ConnectionString);
            var cmd = new MySqlCommand(strAccessSelect, c);

            var myDataAdapter = new MySqlDataAdapter(cmd);
            myDataAdapter.AcceptChangesDuringFill = AcceptChangesDuringFill;
            this.lastSqlCommand = sql;
            SqlCommands.Add(sql);
            DataSet myDataSet = new DataSet();
            try
            {
                c.Open();
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
                c.Close();
            }
            DataTable tbl = myDataSet.Tables[tableName];
            myDataSet.Tables.Remove(tbl);
            return tbl;
        }

        public override DataTable Table(string tableName)
        {
            if (tableName.Trim().IndexOf(" ") > 0)
                tableName = "`" + tableName + "`";
            return Table(tableName, "select * from " + tableName + "");
        }
        public override void FillTable(DataTable dataTable, string sql)
        {
            base.SqlCommands.Add("Fill(" + dataTable.TableName + ")");
            string strAccessSelect = sql;

            var myAccessConn = new MySqlConnection(ConnectionString);
            var myAccessCommand = new MySqlCommand(strAccessSelect, myAccessConn);
            var myDataAdapter = new MySqlDataAdapter(myAccessCommand);

            //Console.WriteLine(sql);
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

        public override int SaveTable(DataTable dataTable)
        {
            string sql = "select  * from " + dataTable.TableName + " where 2=1";
            return SaveTable(dataTable, sql);
        }

        public override int SaveTable(DataTable dataTable, string sql)
        {
            Performance perf = new Performance();
            Logger.WriteLine("Saving " + dataTable.TableName);
            DataSet myDataSet = new DataSet();
            myDataSet.Tables.Add(dataTable.TableName);

            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlCommand myAccessCommand = new MySqlCommand(sql, conn);
            MySqlDataAdapter myDataAdapter = new MySqlDataAdapter(myAccessCommand);
            MySqlCommandBuilder karlCB = new MySqlCommandBuilder(myDataAdapter);

            this.lastSqlCommand = sql;
            SqlCommands.Add(sql);
            int recordCount = 0;
            
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
        

        public override int RunSqlCommand(string sql)
        {
            return RunSqlCommand(sql, ConnectionString);
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
      var myConnection = new MySqlConnection(SqlConnString);
      myConnection.Open();
      var myCommand = new MySqlCommand();
      MySqlTransaction myTrans=null;

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
        throw e;
      }
      finally
      {
        myConnection.Close();
      }
      return rval;
    }

     /// <summary>
     /// gets a list of all 'base tables'
     /// </summary>
     /// <returns></returns>
     public override string[] TableNames()
     {
         string sql = "Select table_Name from Information_schema.Tables where table_schema = '" + GetDefaultSchema() + "' order by table_name ";
         DataTable tbl = Table("schema", sql);
         string[] rval = new string[tbl.Rows.Count];
         for (int i = 0; i < tbl.Rows.Count; i++)
         {
             rval[i] = tbl.Rows[i]["table_Name"].ToString();
         }
         return rval;
     }

     private string GetDefaultSchema()
     {
         var b = new MySqlConnectionStringBuilder(ConnectionString);
         return b.Database; 
     }

     public static BasicDBServer GetMySqlServer(string databaseName)
     {
         MySqlConnectionStringBuilder b = new MySqlConnectionStringBuilder();
         b.UserID = ConfigurationManager.AppSettings["MySqlUser"];
         b.Server = ConfigurationManager.AppSettings["MySqlServer"];
         b.Database = databaseName;

         if (LinuxUtility.IsLinux())
         {//Linux login is from config file.  Assuming localhost access
             return new MySqlServer(b.ConnectionString);
         }
         else
         { // use windows login for username
             b.UserID = GetWindowsUserName();
             b.Password = GeneratePassword();
             return new MySqlServer(b.ConnectionString);
         }
     }
    }
}
