using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Reclamation.Core;

namespace Reclamation.TimeSeries
{
     public class ExcelOleDatabase: IDisposable
    {
        string m_filename;
        DataTable m_treeTable;
        public ExcelOleDatabase(string filename)
        {
            m_filename = filename;
            BuildSiteCatalog();
        }

        public bool ReadOnly
        {
            get { return true; }
        }
        /// <summary>
        /// Create a SeriesCatalog by looking at each sheet and column names inside
        /// </summary>
        private void BuildSiteCatalog()
        {
            
            m_treeTable = new DataTable("SeriesCatalog");

            m_treeTable.Columns.Add("Label", typeof(string));
            m_treeTable.Columns.Add("Level", typeof(Int32));
            m_treeTable.Columns.Add("DatabaseName");
            m_treeTable.Columns.Add("SheetName");
            m_treeTable.Columns.Add("DateColumn");
            m_treeTable.Columns.Add("ValueColumn");

            m_treeTable.Rows.Add(Name, 0, "", "", "", "");

            string[] sheetNames = ExcelDB.SheetNames(m_filename);

            for (int i = 0; i < sheetNames.Length; i++)
            {
                string sheetName = sheetNames[i];
                if (sheetName.Trim() != "")
                {
                    string sql = " Select * from [" + sheetName + "$] WHERE 2 = 1";
                    DataTable t1 = ExcelDB.Read(m_filename, sheetName, sql);

                    if (t1.Columns.Count >0 && t1.Columns[0].ColumnName.ToLower().IndexOf("date") == 0)
                    {
                        string dateColumn = t1.Columns[0].ColumnName;
                        for (int j = 1; j < t1.Columns.Count; j++)
                        {
                            string valueColumn = t1.Columns[j].ColumnName;

                            m_treeTable.Rows.Add(valueColumn, 1, Name, sheetName,dateColumn, valueColumn);
                        }
                    }
                }
            }
        }


        public  bool ValidRow(DataRow row)
        {
            if (row["ValueColumn"].ToString().Trim() != "")
            {
                return true;
            }

            return false;
        }

        public Series GetSeries(System.Data.DataRow row)
        {
            if (ValidRow(row))
            {
                return new ExcelOleSeries(m_filename, row["SheetName"].ToString(),
                                       row["DateColumn"].ToString(),
                                       row["ValueColumn"].ToString());
            }
            else
            {
                return new Series();
            }
        }

        public string Name
        {
            get { return System.IO.Path.GetFileNameWithoutExtension(m_filename); }
        }

        public bool SupportsPeriodOfRecordQueries
        {
            get { return true; }
        }

        public void Dispose()
        {
            
        }
    }
}
