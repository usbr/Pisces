using System;
using System.Collections;
using System.Reflection;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Reclamation.Core
{
  /// <summary>
  /// Basic SQL Operation
  /// for Microsoft Access Database
  /// Karl Tarbet
  /// List of Microsoft Jet 4.0 reserved words
  /// http://support.microsoft.com/default.aspx?scid=kb;EN-US;321266
  /// </summary>
  public class AccessDB : BasicDBServer
  {

      public override string DataSource
      {
          get {  return filename; }
      }
    private static string GetConnectionString(string fileName)
    {

        if (Path.GetExtension(fileName).ToLower() == ".mdb" || Path.GetExtension(fileName).ToLower() == ".accdb")
        {
            using (RegistryKey key = Registry.ClassesRoot.OpenSubKey("Microsoft.ACE.OLEDB.12.0"))
            {
                if (key != null)
                {
                    return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName;
                }
                else
                {
                    return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName;
                }
            }
        }
        else
            throw new Exception("Unsupported file format "+fileName);
    }

    static bool debugOutput = false;
    private string filename="";
 
    /// <summary>
    /// Constructor 
    /// </summary>
    /// <param name="filename"></param>
    public AccessDB(string filename)
    {
      this.filename = filename;
    }

    /// <summary>Retuns a DataTable</summary>
      public override DataTable Table(string tableName, string sql)
    {
      return AccessDB.Table(filename,tableName,sql);
    }
    /// <summary>Retuns a DataTable</summary>
      public override DataTable Table(string tableName)
    {
      return AccessDB.ReadTable(filename,tableName);
    }
    /// <summary>Saves DataTable</summary>
      public override int SaveTable(DataTable dataTable)
    {
      return AccessDB.Save(this.filename,dataTable);
    }
    /// <summary>Saves DataTable </summary>
      public override int SaveTable(DataTable dataTable, string sql)
    {
      return AccessDB.Save(this.filename,dataTable,sql);
    }
/// <summary>executes a sql command </summary>
      public override int RunSqlCommand(string sql)
    {
      return	AccessDB.RunSqlCommand(filename,sql);
    }
      public override void FillTable(DataTable dataTable, string sql)
      {
          base.SqlCommands.Add("Fill(" + dataTable.TableName + ")");
          string strAccessConn = GetConnectionString(filename);
          var myAccessConn = new OleDbConnection(strAccessConn);
          var myAccessCommand = new OleDbCommand(sql, myAccessConn);
          var myDataAdapter = new OleDbDataAdapter(myAccessCommand);

          Logger.WriteLine(sql);

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


    

      /// <summary>
      /// returns list of tables
      /// </summary>
      /// <returns>array of strings</returns>
      public override string[] TableNames()
      {
          string strAccessConn = GetConnectionString(filename);

          OleDbConnection conn = new OleDbConnection(strAccessConn);

          conn.Open();
          DataTable tbl = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
          conn.Close();
          return DataTableUtility.Strings(tbl,"TABLE_TYPE='TABLE'", "TABLE_name");
      }

      /// <summary>
      /// returns list of queries
      /// </summary>
      /// <returns></returns>
      public string[] QueryNames()
      {
          string strAccessConn = GetConnectionString(filename);
          OleDbConnection conn = new OleDbConnection(strAccessConn);
          conn.Open();
          DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
          conn.Close();

          return DataTableUtility.Strings(schemaTable, "TABLE_TYPE='VIEW'", "TABLE_name");
      }




    /// <summary>Creates a new Access file (*.mdb) </summary>
    public static void CreateMDB(string filename)
    {
      Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Reclamation.Core.blank.mdb");

      byte[] buffer = new byte[stream.Length];
        //following line is risky.. sholud check number of bytes read
      
       stream.Read(buffer,0,buffer.Length);

      File.Delete(filename);
      FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
      BinaryWriter bw = new BinaryWriter(fs);

      bw.Write(buffer);

      bw.Close();
      fs.Close();
      //      ADOX.CatalogClass cat = new ADOX.CatalogClass();
      //
      //      cat.Create("Provider=Microsoft.Jet.OLEDB.4.0;" +
      //        "Data Source="+filename+";" +
      //        "Jet OLEDB:Engine Type=5");
      //
      //      Console.WriteLine("Database Created Successfully");
      //
      //      cat = null;

    }

   

    /// <summary>
    /// Saves data from DataTable to database
    /// uses insert commands to save.
    /// This is an alternative to the SaveTable method
    /// </summary>
    /// <param name="table"></param>
    /// <param name="filename"></param>
    public static void InsertTable(DataTable table,string filename)
    {
      if( debugOutput)
        Console.Write("Inserting "+table.TableName+" to "+filename);
      int sz = table.Rows.Count;
      int cols = table.Columns.Count;

      string strAccessConn = GetConnectionString(filename);

      OleDbConnection myConnection = new OleDbConnection(strAccessConn);
      myConnection.Open();

      OleDbCommand myCommand = new OleDbCommand();
      OleDbTransaction myTrans;

      // Start a local transaction
      myTrans = myConnection.BeginTransaction(IsolationLevel.ReadCommitted);
      // Assign transaction object for a pending local transaction
      myCommand.Connection = myConnection;
      myCommand.Transaction = myTrans;

      try
      {


        for(int i=0; i<sz; i++)
        {
          string cmd = "insert into "+table.TableName+" Values ( ";
          for(int c = 0; c<cols; c++)
          {
            object o = table.Rows[i][c];
            string type =o.GetType().ToString();

            //TO do.  DateTime...

            if( type == "System.String")
            {// enclose with quotes.
              cmd += "'"+o.ToString()+"'";
            }
            else
            {
              cmd += o.ToString();
            }

            if( c!= cols-1)
              cmd += ",";

          }
          cmd += ")";
          myCommand.CommandText = cmd;
          myCommand.ExecuteNonQuery();
         
        }
        myTrans.Commit();
        //rval = true;
      }
      catch(Exception e)
      {
        myTrans.Rollback();
        //Console.WriteLine(sql);
        Console.WriteLine(e.ToString());
      }
      finally
      {
        myConnection.Close();
      }




      if( debugOutput)
        Console.WriteLine(" done.");
    }


    /// <summary>
    /// Copies table from one database to another.
    /// deletes original table in target
    /// </summary>
    /// <param name="filenameSource"></param>
    /// <param name="filenameTarget"></param>
    /// <param name="tableName"></param>
    public static void CopyTable(string filenameSource, string filenameTarget, string tableName)
    {
      string sql = "delete * from "+tableName;
      AccessDB.RunSqlCommand(filenameTarget,sql);
      AccessDB.InsertTable(AccessDB.ReadTable(filenameSource,tableName),filenameTarget);
    }
    /// <summary>
    /// Returns everything from specified table.
    /// </summary>
    /// <param name="filename">Access Database filename</param>
    /// <param name="tableName">Name of table to return</param>
    /// <returns></returns>
    public static DataTable ReadTable(string filename, string tableName)
    {
      CheckIfFileExists(filename);
      string sql = "SELECT * from "+tableName;
      DataTable rval = Table(filename,tableName,sql);
      rval.TableName = tableName;
      return rval;
    }


    static void CheckIfFileExists(string filename)
    {
      FileInfo Info = new FileInfo(filename);
      if (Info.Exists == false)
        throw new Exception("File "+filename+" does not exist");

    }
    public static DataTable Table(string filename, string tableName, string sql)
    {
        return Table(filename, tableName, sql, true);
    }

    /// <summary>
    /// Read File returns a DataTable from Microsoft Access database.
    /// </summary>
    /// <param name="filename"> mdb file</param>
    /// <param name="tableName">Name for returned DataTable</param>
    /// <param name="sql">sql command i.e. 'SELECT * from myTable'</param>
    /// <returns></returns>
    public static DataTable Table(string filename,string tableName, string sql,
        bool acceptChangesDuringFill)
    {
        Logger.WriteLine("Reading "+tableName+" from "+filename);
        Logger.WriteLine("sql: " + sql);
      CheckIfFileExists(filename);


      string strAccessConn = GetConnectionString(filename);

      DataSet myDataSet = new DataSet();
      myDataSet.Tables.Add(tableName);
      
      OleDbConnection myAccessConn = new OleDbConnection(strAccessConn);
      OleDbCommand myAccessCommand = new OleDbCommand(sql,myAccessConn);
      OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(myAccessCommand);
      myDataAdapter.AcceptChangesDuringFill = acceptChangesDuringFill;
      try
      {
        myAccessConn.Open();
        myDataAdapter.Fill(myDataSet,tableName);
      }
      finally
      {
        myAccessConn.Close();
      }
      if( debugOutput)
        Logger.WriteLine("completed reading MODSIM data");
      return myDataSet.Tables[tableName];
    }
    /// <summary>
    /// Saves DataTable
    /// </summary>
    static int Save( string filename,DataTable dataTable)
    {

      string sql = "Select ";
      for(int i=0; i<dataTable.Columns.Count; i++)
      {
        sql+= "["+dataTable.Columns[i].ColumnName+"]";
        if( i!= dataTable.Columns.Count-1)
          sql+=",";
      }
      sql +=" FROM "+dataTable.TableName
      +" where 1=2 ";
      return Save(filename,dataTable,sql);
    }




    /// <summary>
    /// Writes an updated table to a  Microsoft Access database.
    /// </summary>
    /// <param name="dataTable">  DataTable to update Access mdb with</param>
    /// <param name="filename">Access .mdb file</param>
    /// <param name="sql">sql command</param>
    /// <returns></returns>
    static int Save(string filename,DataTable dataTable,  string sql)
    {
      if( debugOutput)
        Console.Write("Writing "+dataTable.TableName+" to "+filename);
      CheckIfFileExists(filename);

      string strAccessConn = GetConnectionString(filename);

      DataSet myDataSet = new DataSet();
      myDataSet.Tables.Add(dataTable.TableName);
            

      OleDbConnection connection = new OleDbConnection(strAccessConn);
      OleDbDataAdapter da = new OleDbDataAdapter();
      da.SelectCommand = new OleDbCommand(sql,connection);
      
      da.RowUpdating += new OleDbRowUpdatingEventHandler(myDataAdapter_RowUpdating);
      OleDbCommandBuilder cb = new OleDbCommandBuilder(da);
      cb.QuotePrefix  = "[" ;
      cb.QuoteSuffix = "]";

      da.InsertCommand= cb.GetInsertCommand();
      da.DeleteCommand = cb.GetDeleteCommand();
      da.UpdateCommand = cb.GetUpdateCommand();


      // call Fill method only to make things work. (we ignore myDataSet)
     // myDataAdapter.Fill(myDataSet,dataTable.TableName); 
      //Console.WriteLine(karlCB.GetUpdateCommand().CommandText);
      //connection.Open();
      da.Fill(myDataSet, dataTable.TableName);
      int rowsChanged = da.Update(dataTable);
      Console.WriteLine(rowsChanged+" rows changed");
      return rowsChanged;
    }

    /// <summary>
    /// runs sql command.
    /// returns true if it works
    /// false if failed.
    /// </summary>
     static int RunSqlCommand(string filename, string sql)
    {
      int rval = 0;
      string strAccessConn = GetConnectionString(filename);

      OleDbConnection myConnection = new OleDbConnection(strAccessConn);
      myConnection.Open();

      OleDbCommand myCommand = new OleDbCommand();
      OleDbTransaction myTrans;

      // Start a local transaction
      myTrans = myConnection.BeginTransaction(IsolationLevel.ReadCommitted);
      // Assign transaction object for a pending local transaction
      myCommand.Connection = myConnection;
      myCommand.Transaction = myTrans;

      try
      {
        myCommand.CommandText = sql;
        //myCommand.CommandType = System.Data.CommandType.Text;
        rval =  myCommand.ExecuteNonQuery();
        myTrans.Commit();
      }
      catch(Exception e)
      {
        myTrans.Rollback();
        Console.WriteLine(sql);
        Console.WriteLine(e.ToString());
      }
      finally
      {
        myConnection.Close();
      }
      return rval;
    }


   
    /// <summary>
    /// Saves Access database table contents to an xml file
    /// </summary>
    /// <param name="InputJetfilename"></param>
    /// <param name="OutputXmlFilename"></param>
     static void ExportToXML(string InputJetfilename, string OutputXmlFilename)
    {
      DataSet myDataSet = new DataSet();

      string strAccessConn = GetConnectionString(InputJetfilename);
      OleDbConnection myAccessConn = new OleDbConnection(strAccessConn);
      OleDbCommand myAccessCommand = new OleDbCommand();
      OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(myAccessCommand);

      AccessDB db = new AccessDB(InputJetfilename);
      string[] tbls = db.TableNames();
      try
      {
        myAccessConn.Open();
        //Console.WriteLine("Closed connection");
        if( debugOutput)
          Console.WriteLine("Found "+tbls.Length+" tables");
        for(int i=0; i< tbls.Length; i++)
        {
            string tableName = tbls[i];
          if( tableName.IndexOf("Paste Errors") >=0)
            continue;

          if( debugOutput)
            Console.Write(tableName+ ":");
          myDataSet.Tables.Add(tableName);
          string sql = "Select * FROM "+tableName;
          myDataAdapter.SelectCommand = new OleDbCommand(sql,myAccessConn);
          //myDataAdapter.Fill(myDataSet,0,1000,tableName);
          myDataAdapter.Fill(myDataSet,tableName);
          if( debugOutput)
            Console.WriteLine(myDataSet.Tables[tableName].Rows.Count);
					
        }
      }
      finally
      {
        myAccessConn.Close();
      }
			
      myDataSet.WriteXml(OutputXmlFilename,XmlWriteMode.WriteSchema);
    }


    /// <summary>
    /// Determines the datatype for the DataColumn.
    /// To be used when creating a table with sql commands.
    /// </summary>
    /// <param name="dataColumn"></param>
    /// <returns></returns>
    public override string SqlColummType(DataColumn dataColumn)
    {
      Type t =dataColumn.DataType;
      string s = t.ToString();
      if( s =="System.Int32")
        return "integer";
		        
      if( s == "System.String")
      {
        // find max text width and multiply by 2.
        DataTable tbl = dataColumn.Table;
        int sz= tbl.Rows.Count;
        int len = 64;
        for(int i=0; i<sz; i++)
        {
          string str = (string)tbl.Rows[i][dataColumn];
          if( str.Length > len)
            len = str.Length*2;
        }
        return "char("+len+")";
      }
					   
      if( s =="System.DateTime")
        return "timestamp";
              
      if( s =="System.Double")
        return "double";
      
      if( s =="System.Boolean")
        return "logical";
        
      return "integer";
    }

    private static void myDataAdapter_RowUpdating(object sender, OleDbRowUpdatingEventArgs e)
    {
        Console.WriteLine(e.Row.RowState);
      Console.WriteLine("rowUpdating "+e.Command.CommandText);
    }
  }
}

