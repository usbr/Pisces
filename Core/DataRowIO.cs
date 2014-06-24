// karl tarbet
using System;
using System.Data;
using System.Collections;

namespace Reclamation.Core
{
	/// <summary>
	/// Used to read and write data from a DataRow .
	/// </summary>
	public class DataRowIO
	{
        public DataRowIO(bool allowNulls)
        {
            _allowNulls = allowNulls;
            array = new ArrayList();
        }
    ArrayList array;
    bool _allowNulls=false;
		public DataRowIO()
		{
         array  = new ArrayList();
		}

    public bool AllowNulls
    {
      get{ return this._allowNulls;}
      set { this._allowNulls=value;}
    }

    public bool AnyErrors
    {
      get { return array.Count >0;}
    }
    public string Messages
    {
      get 
      {
        string msg="";
        for(int i=0; i<array.Count; i++)
        {
            msg += array[i].ToString()+"\n";
        }
        return msg;
      }
    }

    public void SaveFloat(DataRow row,string columnName, string strFloat)
    {
      try
      {
        if( AllowNulls)
        {
          if( strFloat.Trim()=="")
          {
            row[columnName]=DBNull.Value;
            return;
          }
        }
        float f = Convert.ToSingle(strFloat);
        row[columnName] = f;
      }
      catch(Exception)
      {
        string msg = "Error converting "+strFloat+ " to a float. It is not reconigized as a number";
        array.Add(msg);
      }
    }

    /// <summary>
    /// Save string to decimal..
    /// money format is ok.
    /// </summary>
    public void SaveDecimal(DataRow row,string columnName, string strDecimal)
    {
      try
      {
        if( AllowNulls)
        {
          if( strDecimal.Trim()=="")
          {
            row[columnName]=DBNull.Value;
            return;
          }
        }
        decimal d = decimal.Parse(strDecimal,System.Globalization.NumberStyles.Currency);
        row[columnName] = d;
      }
      catch(Exception)
      {
        string msg = "Error converting "+strDecimal+ " to a decimal. It is not reconigized as a number";
        array.Add(msg);
      }
    }

        public void SaveDouble(DataRow row, string columnName, string strDouble)
        {
            try
            {
                if (AllowNulls)
                {
                    if (strDouble.Trim() == "")
                    {
                        row[columnName] = DBNull.Value;
                        return;
                    }
                }
                double  d = Convert.ToDouble(strDouble);
                row[columnName] = d;
            }
            catch (Exception)
            {
                string msg = "Error converting " + strDouble + " to a double. It is not reconigized as a double";
                array.Add(msg);
            }
        }
    public void SaveInt(DataRow row,string columnName, string value)
    {
      try
      {
        if( AllowNulls)
        {
          if( value.Trim()=="")
          {
            row[columnName]=DBNull.Value;
            return;
          }
        }
        int i = Convert.ToInt32(value);
        row[columnName] = i;
      }
      catch(Exception)
      {
        string msg = "Error converting "+value+ " to a integer. It is not reconigized as a integer";
        msg += " ColumnName = '" + columnName + "' " + row.Table.TableName;
        array.Add(msg);
      }
    }

    public string ReadDecimal(DataRow row, string columnName)
    {
      if( row[columnName] == DBNull.Value)
      {
        return "";
      }
     decimal d = Convert.ToDecimal(row[columnName]);
      return d.ToString("c");
    }
    public static string ReadString(DataRow row, string columnName)
    {
      
      if( row[columnName] == DBNull.Value)
      {
        return "";
      }
      return row[columnName].ToString();
    }

   
}
}
