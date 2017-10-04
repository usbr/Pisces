using System;
using System.Data;
using System.Collections.Generic;

namespace Reclamation.Core
{
	/// <summary>
    /// DataTableUtility performs basic operations on a DataTable
	/// </summary>
	public static class DataTableUtility
	{

        public static void PrintRowState(DataTable table)
        {
            int i = 1;
            foreach (DataRow  row in table.Rows)
            {
                Console.WriteLine(i+" "+row.RowState);
                i++;
            }
        }

        public static DataTable InsertColumn(DataTable tbl,DataColumn newColumn,int index)
        {
            DataTable rval = new DataTable();

            for (int i = 0; i < tbl.Columns.Count; i++)
            {
                if (i == index)
                {
                    rval.Columns.Add(newColumn);
                }
                rval.Columns.Add(new DataColumn(tbl.Columns[i].ColumnName, tbl.Columns[i].DataType));
            }
            // copy data
            for (int r = 0; r < tbl.Rows.Count; r++)
            {
                DataRow row = rval.NewRow();
                rval.Rows.Add(row);
                for (int c = 0; c < tbl.Columns.Count; c++)
                {
                    string cName =tbl.Columns[c].ColumnName;
                    row[cName] = tbl.Rows[r][cName];
                }                
            }
            return rval;
        }
    

    public static DataTable Select(DataTable sourceTable, 
                            string sql,string sort)
    {
      DataTable newTable;
      DataRow[] orderedRows;
      newTable = sourceTable.Clone();

      orderedRows = sourceTable.Select(sql,sort);

      foreach (DataRow row in orderedRows)
      {
        DataRow newRow = newTable.NewRow();
        for(int i=0; i<sourceTable.Columns.Count; i++)
             newRow[i] = row[i];
        newTable.Rows.Add(newRow);
      }

      return newTable;
    }

    /// <summary>
    /// Return DataTable like a Select DISTINCT SQL command 
    /// </summary>
    /// <param name="SourceTable"></param>
    /// <param name="FieldNames">combination of columns</param>
    /// <returns></returns>
    public static DataTable SelectDistinct(DataTable SourceTable, 
      params string[] FieldNames)
    {
      object[] lastValues;
      DataTable newTable;
      DataRow[] orderedRows;

      if (FieldNames == null || FieldNames.Length == 0)
        throw new ArgumentNullException("FieldNames");

      lastValues = new object[FieldNames.Length];
      newTable = new DataTable();

      foreach (string fieldName in FieldNames)
        newTable.Columns.Add(fieldName, SourceTable.Columns[fieldName].DataType);

      orderedRows = SourceTable.Select("", string.Join(", ", FieldNames));

      foreach (DataRow row in orderedRows)
      {
        if (!fieldValuesAreEqual(lastValues, row, FieldNames))
        {
          newTable.Rows.Add(createRowClone(row, newTable.NewRow(), FieldNames));

          setLastValues(lastValues, row, FieldNames);
        }
      }

      return newTable;
    }

    private static bool fieldValuesAreEqual(object[] lastValues, DataRow currentRow, string[] fieldNames)
    {
      bool areEqual = true;

      for (int i = 0; i < fieldNames.Length; i++)
      {
        if (lastValues[i] == null || !lastValues[i].Equals(currentRow[fieldNames[i]]))
        {
          areEqual = false;
          break;
        }
      }

      return areEqual;
    }

    private static DataRow createRowClone(DataRow sourceRow, DataRow newRow, string[] fieldNames)
    {
      foreach (string field in fieldNames)
        newRow[field] = sourceRow[field];

      return newRow;
    }

    private static void setLastValues(object[] lastValues, DataRow sourceRow, string[] fieldNames)
    {
      for (int i = 0; i < fieldNames.Length; i++)
        lastValues[i] = sourceRow[fieldNames[i]];
    }


        public static List<int> IntegerList(DataTable tbl, string sqlSelect, string columnName)
        {
            List<int> rval = new List<int>();
            int[] Lst = Integers(tbl, sqlSelect, columnName);
            foreach (int i in Lst)
            {
                rval.Add(i);
            }
            return rval;
        }


    /// <summary>
    /// Returns a list of integers from a table
    /// using a select command and column name to return as integer array.
    /// </summary>
    /// <param name="tbl"></param>
    /// <param name="sqlSelect"></param>
    /// <param name="columnName"></param>
    /// <returns></returns>
    public static int[] Integers(DataTable tbl, string sqlSelect, string columnName)
    {
      return Integers(tbl,sqlSelect,columnName,"");
    }
		
		
    public static int[] Integers(DataTable tbl, string sqlSelect, string columnName, string sortCol)
    {
      DataRow[] rows = tbl.Select(sqlSelect, sortCol);
      int[] rval = new int[rows.Length];
      for(int i=0; i<rval.Length; i++)
      {
        if( rows[i][columnName] != DBNull.Value)
          rval[i] = (int)rows[i][columnName];
        else
          rval[i] = -1;
      }
      return rval;
    }


        public static List<bool> BooleanList(DataTable tbl, string sqlSelect, string columnName, bool defaultValue)
        {
            bool[] b = Booleans(tbl, sqlSelect, columnName, defaultValue);
            List<bool> rval = new List<bool>();
            foreach (bool myb in b)
            {
                rval.Add(myb);
            }
            return rval;
        }



    /// <summary>
    /// Returns a list of Booleans from a table
    /// using a select command and column name to return as integer array.
    /// </summary>
    /// <param name="tbl"></param>
    /// <param name="sqlSelect"></param>
    /// <param name="columnName"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static bool[] Booleans(DataTable tbl, string sqlSelect, string columnName,bool defaultValue)
    {
      DataRow[] rows = tbl.Select(sqlSelect);
      bool[] rval = new bool[rows.Length];
      for(int i=0; i<rval.Length; i++)
      {
        if( rows[i][columnName] != DBNull.Value)
          rval[i] = (bool)rows[i][columnName];
        else
          rval[i] = defaultValue ;
      }
      return rval;
    }

    /// <summary>
    /// Returns a list of doubles from a table
    /// using a select command and column name to return as integer array.
    /// </summary>
    /// <param name="tbl"></param>
    /// <param name="sqlSelect"></param>
    /// <param name="columnName"></param>
    /// <returns></returns>
    public static double[] Doubles(DataTable tbl, string sqlSelect, string columnName)
    {
      DataRow[] rows = tbl.Select(sqlSelect);
      double[] rval = new double[rows.Length];
      for(int i=0; i<rval.Length; i++)
      {
        if( rows[i][columnName] != DBNull.Value)
          rval[i] = (double)rows[i][columnName];
        else
          rval[i] = 0;
      }
      return rval;
    }

       public static List<string> StringList(DataTable tbl, string sqlSelect, string columnName)
        {
           string[] data =  Strings(tbl, sqlSelect, columnName, "");

           List<string> rval = new List<string>();

           foreach (string s in data)
           {
               rval.Add(s);
           }

           return rval;
        }

        /// <summary>
        /// Returns a list of strings from a table
       /// using a select command and column name.
        /// </summary>
        /// <param name="tbl"></param>
        /// <param name="sqlSelect"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
    public static string[] Strings(DataTable tbl, string sqlSelect, string columnName)
    {
      return Strings(tbl,sqlSelect,columnName,"");
    }
    /// <summary>
    /// Returns a list of strings from a table
    /// using a select command and column name.
    /// </summary>
    /// <param name="tbl"></param>
    /// <param name="sqlSelect"></param>
    /// <param name="columnName"></param>
    /// <param name="sort"></param>
    /// <returns></returns>
    public static string[] Strings(DataTable tbl, string sqlSelect, string columnName, string sort,bool removeNulls)
    {
      DataRow[] rows = tbl.Select(sqlSelect,sort);
     
      List<string> rval = new List<string>();
      for(int i=0; i<rows.Length; i++)
      {
        if( rows[i][columnName] != DBNull.Value)
          rval.Add(rows[i][columnName].ToString());
        else
            if( !removeNulls)
          rval.Add("None");
      }
      return rval.ToArray();
    }

        public static string[] Strings(DataTable tbl, string sqlSelect, string columnName, string sort)
        {
            return Strings(tbl, sqlSelect, columnName, sort, false);
        }

        /// <summary>
        /// from:
        /// http://www.codeproject.com/Articles/44274/Transpose-a-DataTable-using-C
        /// By S Satheesh Kumar, 
        /// </summary>
        /// <param name="inputTable"></param>
        /// <returns></returns>
        public static DataTable Transpose(DataTable inputTable)
        {
            DataTable outputTable = new DataTable();

            // Add columns by looping rows

            // Header row's first column is same as in inputTable
            outputTable.Columns.Add(inputTable.Columns[0].ColumnName.ToString());

            // Header row's second column onwards, 'inputTable's first column taken
            foreach (DataRow inRow in inputTable.Rows)
            {
                string newColName = inRow[0].ToString();
                outputTable.Columns.Add(newColName);
            }

            // Add rows by looping columns        
            for (int rCount = 1; rCount <= inputTable.Columns.Count - 1; rCount++)
            {
                DataRow newRow = outputTable.NewRow();

                // First column is inputTable's Header row's second column
                newRow[0] = inputTable.Columns[rCount].ColumnName.ToString();
                for (int cCount = 0; cCount <= inputTable.Rows.Count - 1; cCount++)
                {
                    string colValue = inputTable.Rows[cCount][rCount].ToString();
                    newRow[cCount + 1] = colValue;
                }
                outputTable.Rows.Add(newRow);
            }

            return outputTable;

        }

        /// <summary>
        /// http://www.c-sharpcorner.com/blogs/splitting-a-large-datatable-into-smaller-batches1
        /// </summary>
        /// <param name="originalTable"></param>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public static List<DataTable> SplitTable(DataTable originalTable, int batchSize)
        {
            List<DataTable> tables = new List<DataTable>();

            int i = 0;
            int j = 1;

            DataTable newDt = originalTable.Clone();
            newDt.TableName = "Table_" + j;
            newDt.Clear();

            foreach (DataRow row in originalTable.Rows)
            {
                DataRow newRow = newDt.NewRow();
                newRow.ItemArray = row.ItemArray;

                newDt.Rows.Add(newRow);
                i++;

                if (i == batchSize)
                {
                    tables.Add(newDt);

                    j++;
                    newDt = originalTable.Clone();
                    newDt.TableName = "Table_" + j;
                    newDt.Clear();

                    i = 0;
                }
            }
            return tables;
        }
    }
}
