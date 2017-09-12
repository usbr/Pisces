using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Collections;
using System.Reflection;
namespace Reclamation.Core
{
  
    /// <summary>
    /// Summary description for ExcelDB.
    /// Table Headers
    /** from www.microsoft.com ///
     also see
     * http://support.microsoft.com/kb/316934
     * 
      With Excel workbooks, the first row in a range is considered to be 
      the header row (or field names) by default. If the first range does 
      not contain headers, you can specify HDR=NO in the extended properties 
      in your connection string. If the first row does not contain headers, 
      the OLE DB provider automatically names the fields for you (where F1 would 
      represent the first field, F2 would represent the second field, and so forth).

      Data Types

      Unlike a traditional database, there is no direct way to specify
       the data types for columns in Excel tables. Instead, the OLE DB provider
        scans a limited number of rows in a column to "guess" the data type for 
        the field. The number of rows to scan defaults to eight (8) rows; you can 
        change the number of rows to scan by specifying a value between one (1) and
         sixteen (16) for the MAXSCANROWS setting in the extended properties of your 
         connection string. 
    ***/
    /// </summary>
    public class ExcelDB
    {
      // Create connection string variable. Modify the "Data Source"
      // parameter as appropriate for your environment.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="xlsx">when false uses excel 8.0 oldb driver.</param>
        /// <returns></returns>
      static string ConnectionString(string filename)
      {
        bool xlsx = Path.GetExtension(filename).ToLower().Contains("xlsx");

        string connectionStr= "Provider=Microsoft.Jet.OLEDB.4.0;" +
          "Data Source=" + filename + ";" +
          "Extended Properties=Excel 8.0;";

        if (xlsx)
        {
            connectionStr = "Provider=Microsoft.ACE.OLEDB.12.0;" +
           "Data Source=" + filename + ";" +
           "Extended Properties=Excel 12.0";

        }

        return connectionStr;
      }

      /// <summary>
      /// Creates a new excel file
      /// </summary>
      /// <param name="filename"></param>
      public static void CreateXLS(string filename)
      {
      Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Reclamation.Core.blank.xls");
      CreateXLS(filename,stream);
      }
      /// <summary>
      /// Creates a new excel file.
      /// </summary>
      /// <param name="filename"></param>
      /// <param name="stream"></param>
      private static void CreateXLS(string filename,Stream stream)
      {

        byte[] buffer = new byte[stream.Length];
        stream.Read(buffer,0,buffer.Length);

        File.Delete(filename);
        FileStream fs = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
        BinaryWriter bw = new BinaryWriter(fs);

        bw.Write(buffer);

        bw.Close();
        fs.Close();
    }

        /// <summary>
        /// Read worksheet from Excel
        /// </summary>
        public static DataTable Read(string filename, string worksheet)
        {
            string sql = "SELECT * FROM [" + worksheet + "$]";
            return Read(filename, worksheet, sql);
        }
    

      /// <summary>
      /// Advanced way to Read data from excel
      /// </summary>
      public static DataTable Read(string filename,string worksheet, string sql)
      {

        Debug.Assert(File.Exists(filename));


       

        string strAccessConn = ConnectionString(filename);

        DataSet myDataSet = new DataSet();
        myDataSet.Tables.Add(worksheet);
    
        OleDbConnection myAccessConn = new OleDbConnection(strAccessConn);
        OleDbCommand myAccessCommand = new OleDbCommand(sql,myAccessConn);
        OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(myAccessCommand);

        myAccessConn.Open();


        try
        {
          myDataAdapter.Fill(myDataSet,worksheet);
        }
        catch  (Exception e)
        {
          Console.WriteLine("Error : "+sql+" "+e.ToString());
          //return null;
          throw e;

        }
        finally
        {
          myAccessConn.Close();
        }
          DataTable t = myDataSet.Tables[worksheet];
          myDataSet.Tables.Remove(t);
          return t;
      }

      /// <summary>
      /// Reads excel sheet
      /// From http://weblogs.asp.net/donxml/archive/2003/08/21/24908.aspx
      /// </summary>
      /// <param name="fileName"></param>
      /// <param name="workSheetNumber"></param>
      /// <returns></returns>
      public static DataTable Read(string fileName,int workSheetNumber)
      {  
        OleDbConnection ExcelConnection = new OleDbConnection(ConnectionString(fileName));
        OleDbCommand ExcelCommand = new OleDbCommand();
        ExcelCommand.Connection = ExcelConnection;
        OleDbDataAdapter ExcelAdapter = new OleDbDataAdapter(ExcelCommand);

        ExcelConnection.Open();
        DataTable ExcelSheets = ExcelConnection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables,new object[] {null, null, null, "TABLE"});
        string SpreadSheetName = "["+ExcelSheets.Rows[workSheetNumber]["TABLE_NAME"].ToString()+"]";

        DataSet ExcelDataSet = new DataSet();
        ExcelCommand.CommandText = @"SELECT * FROM "+SpreadSheetName;
        ExcelAdapter.Fill(ExcelDataSet);
 
        ExcelConnection.Close();
        DataTable t = ExcelDataSet.Tables[0];
        ExcelDataSet.Tables.Remove(t);
        return t;

      }

		public static bool SheetExists(string filename, string sheetname)
		{
			string[] sheets = SheetNames(filename);
				for(int i=0; i<sheets.Length; i++)
				{
					if(String.Compare(sheets[i],sheetname,true) ==0)
					{
						return true;
					}
				}

			return false;
		}

		public static string[] SheetNames(string filename)
		{
			//ArrayList list =new ArrayList();
            OleDbConnection ExcelConnection = new OleDbConnection(ConnectionString(filename));
			OleDbCommand ExcelCommand = new OleDbCommand();
			ExcelCommand.Connection = ExcelConnection;
			OleDbDataAdapter ExcelAdapter = new OleDbDataAdapter(ExcelCommand);

			ExcelConnection.Open();
			DataTable ExcelSheets = ExcelConnection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables,new object[] {null, null, null, "TABLE"});
			//string SpreadSheetName = "["+ExcelSheets.Rows[workSheetNumber]["TABLE_NAME"].ToString()+"]";
			string[] rval = new string[ExcelSheets.Rows.Count];
			for(int i=0; i<ExcelSheets.Rows.Count; i++)
			{
				string s = ExcelSheets.Rows[i]["Table_name"].ToString();
				if( s[s.Length-1] == '$')
				{
						s = s.Replace("$","");
				}
					rval[i] = s;
			}

			return rval;
		}


      /// <summary>
      /// http://weblogs.asp.net/donxml/archive/2003/08/21/24908.aspx
      ///   Here’s the same thing, but opening a CSV file:
      /// </summary>
      /// <param name="pathName"></param>
      /// <param name="fileName"></param>
      /// <returns></returns>
      private DataSet GetCVSFile(string pathName,string fileName)
      {
        OleDbConnection ExcelConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source="+pathName+";Extended Properties=Text;");
        OleDbCommand ExcelCommand = new OleDbCommand(@"SELECT * FROM "+fileName,ExcelConnection);

        OleDbDataAdapter ExcelAdapter = new OleDbDataAdapter(ExcelCommand);
  
        ExcelConnection.Open();

        DataSet ExcelDataSet = new DataSet();
        ExcelAdapter.Fill(ExcelDataSet);
   
        ExcelConnection.Close();   
        return ExcelDataSet;
      } 


    }
  }

