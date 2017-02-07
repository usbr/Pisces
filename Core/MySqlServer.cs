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

        public MySqlServer(string connectionString)
        {
            this.ConnectionString = connectionString;
           

        }


        //private static string GeneratePassword()
        //{
        //    string fileName = FileUtility.GetFileReference("mysql_key.txt");

        //    if (File.Exists(fileName))
        //    {
        //        return GetWindowsUserName() + File.ReadAllText(fileName);
        //    }
        //    else
        //    {
        //        Logger.WriteLine("Error:  missing mysql_key.txt");
        //        throw new FileNotFoundException("mysql_key.txt");
        //    }
        //}
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

        public override void FillTable(DataTable dataTable)
        {
            var tableName = dataTable.TableName;

            if( MapToLowerCase)
                if (MapToLowerCase)
                    tableName = tableName.ToLower();

            FillTable(dataTable, "Select * from " + tableName);
        }

        public override DataTable Table(string tableName)
        {
            if (tableName.Trim().IndexOf(" ") > 0)
                tableName = "`" + tableName + "`";
            if (MapToLowerCase)
                tableName = tableName.ToLower();
            return Table(tableName, "select * from " + tableName + "");
        }
        public override void FillTable(DataTable dataTable, string sql)
        {
            base.SqlCommands.Add("Fill(" + dataTable.TableName + ")");
            if (dataTable.TableName == "")
            {
                dataTable.TableName = "table1";
            }

            string strAccessSelect = sql;

            var myAccessConn = new MySqlConnection(ConnectionString);
            var myAccessCommand = new MySqlCommand(strAccessSelect, myAccessConn);
            var myDataAdapter = new MySqlDataAdapter(myAccessCommand);

            if (MapToLowerCase)
            {
                MapToLower(dataTable, myDataAdapter);
            }
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
            string tn = dataTable.TableName;
            if( MapToLowerCase)
                  tn = tn.ToLower();
            string sql = "select  * from " + tn+ " where 2=1";
            return SaveTable(dataTable, sql);
        }

        public bool MapToLowerCase = true;

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
            if (MapToLowerCase)
            {
                MapToLower(dataTable, myDataAdapter);
            }

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

        private static void MapToLower(DataTable dataTable, MySqlDataAdapter myDataAdapter)
        {
            var map = myDataAdapter.TableMappings.Add(dataTable.TableName.ToLower(), dataTable.TableName);
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                var cn = dataTable.Columns[i].ColumnName;
                map.ColumnMappings.Add(cn.ToLower(), cn);
            }
            // PrintMapping(myDataAdapter);
        }
        private void PrintMapping(MySqlDataAdapter da)
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
           + " WHERE table_name = '" + tableName + "' and  table_schema = '" + GetDefaultSchema() + "' ";
         DataTable tbl = Table("exists", sql);
         if (tbl.Rows.Count > 0)
             return true;
         else
             return false;
     }

     private string GetDefaultSchema()
     {
         var b = new MySqlConnectionStringBuilder(ConnectionString);
         return b.Database; 
     }

        /// <summary>
        /// GetMySqlServer
        /// If windows based automatiallky generates login info
        /// Linux assumes local account.
        /// </summary>
        /// <param name="server"></param>
        /// <param name="databaseName"></param>
        /// <param name="user"></param>
        /// <returns></returns>
     public static BasicDBServer GetMySqlServer(string server, string databaseName, string user = "", string password="")
     {
         Logger.WriteLine("Linux="+LinuxUtility.IsLinux());

            if (LinuxUtility.IsLinux() || server == "localhost" )
            {//Linux login is from config file.  Assuming localhost access
                if (user == "")
                    user = WindowsUtility.GetShortUserName();

                var cs = "server=" + server + ";uid="
               + user + ";"
               + "database=" + databaseName + ";";
                Logger.WriteLine(cs);

                if (password != "")
                    cs += "pwd=" + password+";";
                var msg = cs;
                msg = msg.Replace("pwd=" + password, "pwd=" + "xxxxx");
                Logger.WriteLine(msg);
                return new MySqlServer(cs);
            }
            else
            {  
                if (password == "")
                {
                    var fn = "mysql_key.txt";
                    if( File.Exists(fn) && File.ReadAllLines(fn).Length >0)
                    { 
                        password = user + File.ReadAllLines(@"mysql_key.txt", Encoding.UTF8)[0]; 
                    }
                }
                var cs = "server=" + server + ";uid=" + WindowsUtility.GetShortUserName() + ";" + "pwd=" + password +
                       ";database=" + databaseName + ";";
                string msg = cs;
                msg = msg.Replace("pwd=" + password, "pwd=" + "xxxxx");
                Logger.WriteLine(msg);

                return new MySqlServer(cs);
            }
     }
    }
}
