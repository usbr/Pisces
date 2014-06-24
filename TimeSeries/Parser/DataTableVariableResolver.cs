using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Parser
{
    /// <summary>
    /// Returns Series from a DataTable
    /// </summary>
    public class DataTableVariableResolver:VariableResolver
    {
        private string dateColumn;
        private TimeInterval m_interval;
        private System.Data.DataTable data;

        
        public DataTableVariableResolver(DataTable tbl, string dateColumn,TimeInterval interval= TimeInterval.Hourly)
        {
            m_interval = interval;
            this.data = tbl;
            this.dateColumn = dateColumn;
        }

        public override ParserResult Lookup(string name,TimeInterval defaultInterval)
        {
            if (data.Columns.IndexOf(name) >= 0)
            {
                Series s = new DataTableSeries(data,m_interval, dateColumn,name);
                return new ParserResult(s);
            }
            else
            {
                return base.Lookup(name,defaultInterval);
            }
        }

       

    }
}
