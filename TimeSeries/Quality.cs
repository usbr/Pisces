using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// Manages quality limits and flagging data
    /// </summary>
    class Quality
    {
        TimeSeriesDatabase m_db;
        TimeSeriesDatabaseDataSet.quality_limitDataTable m_limit;
        public Quality( TimeSeriesDatabase db)
        {
            m_db = db;
        }

        public void SetFlags(Series s)
        {
            if (m_db == null)
                return;

            if (m_limit == null)
            {
                m_limit = new TimeSeriesDatabaseDataSet.quality_limitDataTable();
                m_db.Server.FillTable(m_limit);
            }

            var row = GetRow(s.Table.TableName);

            if (row == null) // no limits defined.
                return;

            for (int i = 0; i < s.Count; i++)
            {
                var pt = s[i];
                if (pt.IsMissing)
                    continue;

                if ( !row.IsLowNull()  &&  pt.Value < row.Low)
                {
                    pt.Flag = PointFlag.QualityLow;
                    s[i] = pt;
                    continue;
                }

                if (!row.IsHighNull() && pt.Value > row.High)
                {
                    pt.Flag = PointFlag.QualityHigh;
                    s[i] = pt;
                    continue;
                }

                // To DO. rate of change

                if (!row.IsChangeNull() )
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
        private TimeSeriesDatabaseDataSet.quality_limitRow GetRow(string tableName)
        {
            // try exact match first.

           var rows =  m_limit.Select("TableMask = '" + tableName + "'");

           if (rows.Length == 1)
               return rows[0] as TimeSeriesDatabaseDataSet.quality_limitRow;

            // try to separate tablename into prefix using '_'  boii_ob  => 'boii','ob'
           var tokens = tableName.Split('_');
           if (tokens.Length == 2)
           {
                rows = m_limit.Select("TableMask like '[*]_" + tokens[1] + "'");
                if( rows.Length ==1)
                    return rows[0] as TimeSeriesDatabaseDataSet.quality_limitRow;
           }

            return null;
        }

    }
}
