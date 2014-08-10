using System;
using System.Data;
using System.Collections.Generic;
namespace Reclamation.Core
{
	/// <summary>
	/// Basic server database access to 
	/// sql based databases
	/// </summary>
	public abstract class BasicDBServer
	{
        List<string> m_sqlCommands = new List<string>();
        string m_connectionString= "";

        /// <summary>
        /// either filename or server:database
        /// </summary>
        public abstract string DataSource { get; }

    /// <summary>Contains history of sql commands sent to the database server</summary>
        public List<string> SqlCommands { get { return m_sqlCommands; } }
    /// <summary>Retuns a DataTable</summary>
        public abstract DataTable Table(string tableName, string sql);
    /// <summary>Retuns a DataTable</summary>
        public abstract DataTable Table(string tableName);

        
    /// <summary>Saves DataTable to database server</summary>
        public abstract int SaveTable(DataTable dataTable);
    /// <summary>Saves DataTable to database server</summary>
        public abstract int SaveTable(DataTable dataTable, string sql);

    /// <summary>executes a sql command on the database server</summary>
        public abstract int RunSqlCommand(string sql);

        /// <summary>
        /// executes a supplied CREATE TABLE sql command on the database server
        /// This provides overloaded servers to set ownership and other permissions
        /// </summary>
        public virtual int CreateTable(string sql)
        {
            return RunSqlCommand(sql);
        }


        /// <summary>
        /// Returns the next largest value in a column 
        /// If the table is empty return 1
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public virtual int NextID(string tableName, string columnName)
        {
            string sql = "select count(*) as count,max(" + columnName + ") as max from " + tableName
               + " Where id >0 ";
            DataTable tbl =Table("seriescatalog", sql);
            int count = Convert.ToInt32(tbl.Rows[0]["count"]);
            if (count == 0)
            {
                return 1;
            }
            int max = Convert.ToInt32(tbl.Rows[0][1]);
            return (max + 1);
        }

        /// <summary>
        /// modifies tablename for use in more portable queries
        /// this base version wraps with [] for SQL server when there is a space in the name
        /// 
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public virtual string PortableTableName(string tableName)
        {
            var rval = tableName;
            if( tableName.Trim().IndexOf(" ") >= 0)
            {
                rval = "[" + tableName + "]";
            }
            return rval;
        }

        /// <summary>
        /// Formats a date in string format suitable for use in a where clause
        /// overriden by SQLiteServer to wrap with datetime function
        /// or to use Unix integer for the DateTime
        /// </summary>
        /// <param name="t"></param>
        /// <param name="fmt"></param>
        /// <returns></returns>
        public virtual string PortableDateString(DateTime t,string fmt)
        {
            return "'" + t.ToString(fmt) + "'";
        }

        /// <summary>
        /// Returns datetime string for use in Create Table statements
        /// </summary>
        /// <returns></returns>
        public virtual string PortableDateTimeType()
        {
            return "DateTime"; // sql server
        }

        /// <summary>
        /// Returns character column type given size
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public virtual string PortableCharacterType(int size)
        {
            return " nvarchar(" + size + ") "; // sql server
        }


        /// <summary>
        /// Fills existing DataTable from database
        /// this provides ability to fill a strongly typed dataset
        /// </summary>
        /// <param name="dataTable"></param>
        /// <param name="sql"></param>
        public virtual void FillTable(DataTable dataTable, string sql)
        {
            throw new NotImplementedException();
        }

        public virtual void FillTable(DataTable dataTable)
        {
            FillTable(dataTable, "Select * from " + dataTable.TableName);
        }
        
        /// <summary>
        /// Provides protection against SQL injection attacks
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnnetsec/html/SecNetch12.asp
        /// </summary>
        /// <param name="inputSQL"></param>
        /// <returns></returns>
        public string SafeSqlLiteral(string inputSQL)
        {
            return inputSQL.Replace("'", "''");
        }


        /// <summary>
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnnetsec/html/SecNetch12.asp
        /// </summary>
        /// <param name="inputSQL"></param>
        /// <returns></returns>
        public string SafeSqlLikeClauseLiteral(string inputSQL)
        {
            // Make the following replacements:
            // '  becomes  ''
            // [  becomes  [[]
            // %  becomes  [%]
            // _  becomes  [_]

            string s = inputSQL;
            s = inputSQL.Replace("'", "''");
            s = s.Replace("[", "[[]");
            s = s.Replace("%", "[%]");
            s = s.Replace("_", "[_]");
            return s;
        }

        /// <summary>
        /// Saves all tables to a xml data set.
        /// Low budget backup.
        /// </summary>
        /// <param name="filename"></param>
        public void BackupToXML(string filename)
        {
            DataSet ds = new DataSet();
            string[] tnames = TableNames();
            foreach (string t in tnames)
            {
                ds.Tables.Add(Table(t));
            }

            ds.WriteXml(filename, XmlWriteMode.WriteSchema);

        }

        public void CreateTable(DataTable tbl)
        {
            string sql = "Create Table " + tbl.TableName
              + "\n(";
            for (int i = 0; i < tbl.Columns.Count; i++)
            {
                sql += "[" + tbl.Columns[i].ColumnName + "]      "
                  + SqlColummType(tbl.Columns[i]);
                if (i == tbl.Columns.Count - 1)
                    sql += "   NULL\n";
                else
                    sql += "   NULL,\n";
            }
            sql += "\n)";
            Console.WriteLine(sql);
            RunSqlCommand(sql);
            SqlCommands.Add(sql);
        }

        /// <summary>
        /// Determines the datatype for the DataColumn.
        /// To be used when creating a table with sql commands.
        /// May need to override.  this was designed for Microsoft SQL server
        /// </summary>
        /// <param name="dataColumn"></param>
        /// <returns></returns>
        public virtual string SqlColummType(DataColumn dataColumn)
        {
            Type t = dataColumn.DataType;
            string s = t.ToString();
            if (s == "System.Int32")
                return "integer";

            if (s == "System.String")
            {
                // find max text width and multiply by 2.
                DataTable tbl = dataColumn.Table;
                int sz = tbl.Rows.Count;
                int len = 64;
                for (int i = 0; i < sz; i++)
                {
                    if (tbl.Rows[i][dataColumn] == DBNull.Value)
                        continue;
                    string str = (string)tbl.Rows[i][dataColumn];
                    if (str.Length > len)
                        len = str.Length * 2;
                }
                return "char(" + len + ")";
            }

            if (s == "System.DateTime")
                return "DateTime";

            if (s == "System.Double")
                return "float(53)";

            if (s == "System.Boolean")
                return "logical";

            return "integer";
        }

        public virtual string ConnectionString
        {
            get { return m_connectionString; }
            set { 

                m_connectionString = value; 
            }
        }
      
        private string m_name = "Untitled";

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        /// <summary>
        /// For specialized connection to multiple database files
        /// </summary>
        /// <param name="fileIndex"></param>
        /// <returns></returns>
        public virtual BasicDBServer NewConnection(int fileIndex)
        {
            return this;
        }


        public virtual string[] TableNames()
        {
            return new string[] { };
        }

        /// <summary>
        /// determines if named table exists in database.
        /// you may override this for better performance since
        /// this calls TableNames() to get full list of all tables.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public virtual bool TableExists(string tableName)
        {
            string[] names = TableNames();

            for (int i = 0; i < names.Length; i++)
            {
                if (String.Compare(tableName, names[i], true)
                     == 0)
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Override this for specific database for better 
        /// performance
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public virtual int InsertTable(DataTable table)
        {
            return SaveTable(table);
        }

        /// <summary>
        /// Returns exceedance values from a time series table
        /// </summary>
        /// <param name="tableName">name of timeseries table</param>
        /// <returns></returns>
        public virtual DataTable ExceedanceTable(string tableName, DateTime t1, DateTime t2, MonthDayRange range , RankType sortType)
        {
            throw new NotImplementedException("ExceedanceTable");
            //return null;
            //            if (type == SortType.Weibul)
            //{
            //    for (int i = 0; i < sz; i++)
            //    {
            //        DataRowView row = tbl.DefaultView[i];
            //        row["Percent"] = ((double)(i + 1.0) / (sz + 1.0)) * 100.0; //Weibul
            //    }
            //}
            //else
            //if (type == SortType.Proabability)
            //{
            //    for (int i = 0; i < sz; i++)
            //    {
            //        DataRowView row = tbl.DefaultView[i];
            //        row["Percent"] = ((double)(i + 1.0) / (sz )) * 100.0; 
            //    }

        }

        public virtual void Vacuum()
        {
           
        }
    }


}
