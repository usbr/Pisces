using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;
using System.Data;
using System.IO;
namespace Reclamation.TimeSeries
{

    /// <summary>
    /// Reads TimeSeries data from a Access Database
    /// </summary>
    public class AccessSeries:Series
    {
        string m_filename;
        string m_tableName;
        string m_dateColumn;
        string m_valueColumn;
        string m_filterColumn;
        string m_filterValue;

        ///// <summary>
        ///// Creates AccessSeries
        ///// </summary>
        //public AccessSeries(int sdi, TimeSeriesDatabase db)
        //    : base(sdi, db)
        //{
        //    SetupLocals();
        //}

        public AccessSeries(TimeSeriesDatabase db,Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow sr)
            : base(db,sr)
        {
            SetupLocals();
        }




        private void SetupLocals()
        {

            m_filename = ConnectionStringToken("FileName");
            if (!Path.IsPathRooted(m_filename))
            {
                string dir = Path.GetDirectoryName(m_db.DataSource);
                m_filename = Path.Combine(dir, m_filename);
            }


            m_tableName =    ConnectionStringToken("TableName");
            m_dateColumn =   ConnectionStringToken("DateColumn");
            m_valueColumn =  ConnectionStringToken("ValueColumn");
            m_filterColumn = ConnectionStringToken("FilterColumn");
            m_filterValue =  ConnectionStringToken("FilterValue");
        }

        /// <summary>
        /// Creates AccessSeries and prepares for storage in TimeSeriesDatabase
        /// </summary>
        public AccessSeries(string filename, string tableName,
            string dateColumn, string valueColumn, string filterColumn, string filterValue)
        {
            m_filename = filename;
            m_tableName = tableName;
            m_dateColumn = dateColumn;
            m_valueColumn = valueColumn;
            m_filterColumn = filterColumn;
            m_filterValue = filterValue;
            this.Provider = "AccessSeries";
            this.Source = "Access";
            FileInfo fi = new FileInfo(filename);
            Name = valueColumn +":"+ filterValue + ":" + tableName + ":" + Path.GetFileNameWithoutExtension(filename);

            this.ConnectionString = "FileName=" + filename
               + ";TableName=" +tableName + ";DateColumn=" + dateColumn + ";ValueColumn=" + valueColumn
               +";FilterColumn="+filterColumn +";FilterValue="+filterValue
               + ";LastWriteTime=" + fi.LastWriteTime.ToString(DateTimeFormatInstantaneous);
                

        }

        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            Clear();
          ReadFromMdb(t1, t2);
        }

        public override PeriodOfRecord GetPeriodOfRecord()
        {

            string sql = "select count(*), min(" + m_dateColumn + "),max(" + m_dateColumn + ") from [" + m_tableName + "]";

            string query = "Select * from [" + m_tableName + "]  Where 1 = 0";
            DataTable schema = AccessDB.Table(m_filename, m_tableName, new System.Data.SqlClient.SqlCommand(query));

           if (m_filterValue != "" && m_filterColumn != "")
            {
                if (NeedQuotes(schema.Columns[m_filterColumn]))
                    sql += " WHERE [" + m_filterColumn + "] = '" + m_filterValue + "'";
                else
                    sql += " WHERE [" + m_filterColumn + "] = " + m_filterValue;
            }
                

            DateTime t1 = TimeSeriesDatabase.MinDateTime;//.. DateTime.MinValue;
            DateTime t2 = TimeSeriesDatabase.MaxDateTime;// DateTime.MinValue;
            int count = 0;

            DataTable por = AccessDB.Table(m_filename,"por", new System.Data.SqlClient.SqlCommand(sql));
                count = Convert.ToInt32(por.Rows[0][0]);
                if (count > 0)
                {
                    t1 = Convert.ToDateTime(por.Rows[0][1]);
                    t2 = Convert.ToDateTime(por.Rows[0][2]);
                }

            PeriodOfRecord rval = new PeriodOfRecord(t1, t2, count);
            return rval;
        }

        private void ReadFromMdb(DateTime t1, DateTime t2)
        {
            string dateFormat = "yyyy-MM-dd HH:mm:ss";
            string sql = "SELECT [" + m_dateColumn + "] as [DateTime], [" + m_valueColumn + "]"
                + " FROM [" + m_tableName + "] "
                + " WHERE [" + m_dateColumn + "] >= #" + t1.ToString(dateFormat) + "#"
                + " AND [" + m_dateColumn + "] <= #" + t2.ToString(dateFormat) + "#";


            string query = "Select * from [" + m_tableName + "]  Where 1 = 0";
            DataTable schema = AccessDB.Table(m_filename, m_tableName, new System.Data.SqlClient.SqlCommand(query));

            if (m_filterValue != "" && m_filterColumn != "")
            {
                if (NeedQuotes(schema.Columns[m_filterColumn]))
                    sql += " AND [" + m_filterColumn + "] = '" + m_filterValue + "'";
                else
                    sql += " AND [" + m_filterColumn + "] = " + m_filterValue;
            }


            // get columns to define table
            DataTable tbl = AccessDB.Table(m_filename, m_tableName, new System.Data.SqlClient.SqlCommand(sql +" AND 1 = 0"));
            InitTimeSeries(tbl, this.Units, this.TimeInterval, false);

            tbl = AccessDB.Table(m_filename, m_tableName, new System.Data.SqlClient.SqlCommand(sql));
           // CsvFile.WriteToCSV(tbl, @"c:\temp\a.csv",true);
            foreach (DataRow r in tbl.Rows)
            {
                double v = Point.MissingValueFlag;
                if (r[1] != DBNull.Value)
                {
                    v = Convert.ToDouble(r[1]);     
                }

               DateTime t = Convert.ToDateTime(r[0]);
               
               if (this.IndexOf(t) >= 0)
               {
                   Logger.WriteLine("Skipping duplicate value " + t + ", " + v);
               }
               else
               {
                   Add(t, v);
               }
            }
            
        }


        private bool NeedQuotes(DataColumn c)
        {
            return (
                c.DataType == typeof(string)
                ||
                c.DataType == typeof(DateTime)
                );
        }
    }
}
