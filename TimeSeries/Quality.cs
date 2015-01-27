using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// Manages quality limits and flagging data
    /// </summary>
    public class Quality
    {
        TimeSeriesDatabase m_db;
        TimeSeriesDatabaseDataSet.quality_limitDataTable m_limit;
        public Quality( TimeSeriesDatabase db)
        {
            m_db = db;
            if (m_limit == null)
            {
                m_limit = new TimeSeriesDatabaseDataSet.quality_limitDataTable();
                m_db.Server.FillTable(m_limit);
            }
        }

        public void SetFlags(Series s)
        {
            if (m_db == null)
                return;

            

            var row = GetRow(s.Table.TableName);

            if (row == null) // no limits defined.
                return;

            for (int i = 0; i < s.Count; i++)
            {
                var pt = s[i];
                if (pt.IsMissing)
                    continue;

                if ( !row.IslowNull()  &&  pt.Value < row.low)
                {
                    pt.Flag = PointFlag.QualityLow;
                    s[i] = pt;
                    continue;
                }

                if (!row.IshighNull() && pt.Value > row.high)
                {
                    pt.Flag = PointFlag.QualityHigh;
                    s[i] = pt;
                    continue;
                }

                // To DO. rate of change

                if (!row.IsdeltaNull() )
                {
                    //"^"

                }

            }


        }

        /// <summary>
        /// Finds a matching row
        /// TableMask can be exact or with wildcards.  
        /// For example  *_ob will apply to all table names that  end with ‘_ob’.    
        /// Flags will not be applied to tables that are not defined by a mask.  
        /// Also nulls in the columns (High, Low, Change)  will cause no flagging to occur.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public TimeSeriesDatabaseDataSet.quality_limitRow GetRow(string tableName)
        {
            // try exact match first.   instant_odsw_ob
             
           var rows =  m_limit.Select("TableMask = '" + tableName + "'");
           if (rows.Length == 1)
               return rows[0] as TimeSeriesDatabaseDataSet.quality_limitRow;

           TimeSeriesName tn = new TimeSeriesName(tableName);

           // try site specific next:  odsw_ob
           var mask = tn.siteid + "_" + tn.pcode;
            rows = m_limit.Select("TableMask = '" + mask + "'");
           if (rows.Length == 1)
               return rows[0] as TimeSeriesDatabaseDataSet.quality_limitRow;

            // try parameter alone next

            mask = tn.pcode;
           rows = m_limit.Select("TableMask like '[*]_" + mask + "'");
           if (rows.Length == 1)
               return rows[0] as TimeSeriesDatabaseDataSet.quality_limitRow;


            return null;
        }


        public void SaveLimits(string tableName, double high, double low, double changePerHour=0)
        {
            var row = GetRow(tableName);
            if (row == null)
            {
                row = m_limit.Newquality_limitRow();
            }
            row.tablemask = tableName;
            row.high = high;
            row.low = low;
            row.delta = changePerHour;
            
            if( row.RowState == System.Data.DataRowState.Detached )
               m_limit.Addquality_limitRow(row);

            m_db.Server.SaveTable(m_limit);

        }
    }
}
