using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Reclamation.TimeSeries.Hydromet
{
    /// <summary>
    /// Manages an in memory copy of hydromet data.
    /// Used when recomputing forecast coefficients for
    /// multiple years, and other forecasting needs.  This avoids many round trips to the 
    /// hydromet server  (does NOT support multiple servers i.e. Yakima, PN at the same time)
    /// </summary>
    public class HydrometDataCache
    {
         //DateTime minT, maxT;
         DataSet m_dataSet; // list of tables.
         TimeInterval m_interval;
         public DataSet DataSet
         {
             get { return m_dataSet; }
             set { m_dataSet = value; }
         }
         //DataTable m_table;

         static int s_counter = 0;

         public HydrometDataCache()
         {
             m_dataSet = new DataSet();
         }
         public void Add(string[] cbttPcodeList, DateTime t1, DateTime t2,
            HydrometHost svr = HydrometHost.PNLinux, TimeInterval interval = TimeInterval.Monthly, int back=0)
        {
             // make cbttPcodeList unique.
            List<string> lst = new List<string>(cbttPcodeList);
            cbttPcodeList =  lst.Distinct().ToArray();

            //minT = t1;
            //maxT = t2;
            m_interval = interval;

            string query = String.Join(",",cbttPcodeList);

            if (query.Length >0)
            {
                DataTable m_table = new DataTable();
                if (interval == TimeInterval.Monthly)
                {
                    m_table = HydrometDataUtility.MPollTable(svr, query, t1, t2);
                }
                else if (interval == TimeInterval.Daily)
                {
                    m_table = HydrometDataUtility.ArchiveTable(svr, query, t1, t2,back);
                }
                else if (interval == TimeInterval.Irregular)
                {
                    m_table = HydrometDataUtility.DayFilesTable(svr, query, t1, t2,back);
                }
                m_table.ExtendedProperties.Add("interval", interval.ToString());
                s_counter++;
                m_table.TableName = svr.ToString()+"_"+interval.ToString() + s_counter;
                m_dataSet.Tables.Add(m_table);
            }

        }


        /// <summary>
        /// Returns the index to a table that contains the requested data
        /// or -1 if the data is not in the cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
         internal int TableIndex(string key, TimeInterval interval, DateTime t1, DateTime t2)
         {
             for (int i = 0; i < m_dataSet.Tables.Count; i++)
             {
                 var tbl = m_dataSet.Tables[i];
                 if (tbl.ExtendedProperties["interval"].ToString() == interval.ToString())
                 {
                     if (InCache(tbl, key, t1, t2,m_interval))
                         return i;
                 }
             }
             return -1;
         }

         private static bool InCache(DataTable tbl, string key, DateTime t1, DateTime t2, TimeInterval interval)
         {
             int idx = tbl.Columns.IndexOf(key);
             if (idx < 0)
                 return false;
             if (tbl.Rows.Count == 0)
                 return false;

             DateTime tMin = (DateTime)tbl.Rows[0][0];
             DateTime tMax = (DateTime)tbl.Rows[tbl.Rows.Count - 1][0];

             if (interval == TimeInterval.Monthly)
             {
                  tMin = tMin.FirstOfMonth();
                  tMax = tMax.EndOfMonth();
             }

             if (interval == TimeInterval.Irregular)
             { // hydromet/agrimet data comes in once per hour.. 
                 // loosen tolerence to corespond
                 tMin = tMin.AddHours(-1);
                 tMax = tMax.AddHours(1);
             }


             
             return (t1 >= tMin && t1 <= tMax)
                 && (t2 >= tMin && t2 <= tMax);
         }
    }
}
