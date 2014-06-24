using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// Series tied to a DataTable
    /// </summary>
    public class DataTableSeries : Series
    {
        private string dateColumn;
        private System.Data.DataTable data;
        private string valueColumnName;


        public DataTableSeries(DataTable tbl, TimeInterval interval, string dateColumn, string valueColumnName)
        {
            this.data = tbl;
            this.TimeInterval = interval;
            this.dateColumn = dateColumn;
            this.valueColumnName = valueColumnName;
        }

        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            //DateTime t1 = DateTime.Now.AddDays(-daysBack);
            for (int j = 0; j < data.Rows.Count; j++)
            {
                DateTime t;
                double d;

                string str = data.Rows[j][dateColumn].ToString();
                if (!DateTime.TryParse(str, out t))
                {
                    Logger.WriteLine("Error parsing date '" + str + "'");
                    continue;
                }
                str = data.Rows[j][valueColumnName].ToString();
                if (!Double.TryParse(str, out d))
                {
                    Logger.WriteLine("Error parsing value '" + str + "'");
                    continue;
                }

                if (IndexOf(t) >= 0)
                {
                    Logger.WriteLine("duplicate value skipped " + t.ToString());
                    continue;
                }

                Add(t, d);
            }
        }

    }
}

