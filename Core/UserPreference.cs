// Karl Tarbet
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
namespace Reclamation.Core
{
    /// <summary>
    /// UserPreference class reads and writes basic user prefrences to
    /// the a named after product and version in the UserAppDataPath/>
    /// </summary>
    public class UserPreference
    {
        static DataTable m_table = null;

        private  UserPreference()
        {
        }

        private static DataTable Table
        {
            get
            {
                if (m_table == null)
                {
                   m_table = GetTableFromDisk();
                }
                return m_table;
            }
        }

        private static DataTable GetTableFromDisk()
        {

            try
            {
                string filename = GetFilename();
                if (File.Exists(filename))
                {
                    DataTable table = new DataTable();
                    table.ReadXml(filename);
                    return table;
                }
            }
            catch (Exception ex)
            {
                Logger.WriteLine(ex.Message);    
            }

            DataTable t = CreateDefaults();
            return t;
        }

        private static string GetFilename()
        {
         
            string appName = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            string tmpPath = System.Windows.Forms.Application.UserAppDataPath; 
            string filename = Path.Combine(tmpPath, appName+".xml");
            return filename;
        }

        private static DataTable CreateDefaults()
        {
            DataTable table = new DataTable("user");
            table.Columns.Add("Name", typeof(String));
            table.Columns.Add("Value", typeof(String));
            return table;
        }

        private static void AddRow(string name, string value)
        {
            DataRow row = Table.NewRow();
            row["Name"] = name;
            row["Value"] = value;
            Table.Rows.Add(row);
        }

        public static string Lookup(string name, string defaultValue="")
        {
            DataRow[] rows;
            string rval = "";

            rows = Table.Select("Name = '" + name + "'");
            if (rows.Length > 0)
            {
                rval = Convert.ToString(rows[0]["Value"]);
            }
            else
            {
                rval = defaultValue;
            }

            return rval;

        }

        private static bool Exists(string name)
        {
               DataRow[] rows;
            //string rval = "";

            rows = Table.Select("Name = '" + name + "'");
            return rows.Length > 0;
        }

        public static void Save(string name, string newValue)
        {
            DataRow[] rows;

            rows = Table.Select("Name = '" + name + "'");
            if (rows.Length > 0)
            {
                string oldValue = rows[0]["Value"].ToString();
                if (oldValue == newValue)
                    return;
                rows[0]["Value"] = newValue;
            }
            else
            { // insert row.
                DataRow newRow = Table.NewRow();
                newRow["Value"] = newValue;
                newRow["Name"] = name;
                Table.Rows.Add(newRow);
            }

            Table.WriteXml(GetFilename(), XmlWriteMode.WriteSchema);
        }

        public static void SetDefault(string name, string value, bool overwrite)
        {
            if (overwrite)
            {
                Save(name, value);
            }
            else
            {
                if (!Exists(name))
                {
                    Save(name, value);
                }
            }

        }
    }
}

