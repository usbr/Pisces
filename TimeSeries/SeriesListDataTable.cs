using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// 
    /// </summary>
    /// <summary>
    /// MultipleSeriesDataTable is used to combine
    /// multiple series into a single DataTable.  
    /// </summary>
    public class SeriesListDataTable : DataTable
    {
        SeriesList m_seriesList;
        /// <summary>
        /// Creates a DataTable composed from multiple time series
        /// </summary>
        public SeriesListDataTable(SeriesList list,
            TimeInterval interval) : base(interval.ToString())
        {
            m_seriesList = list.FilteredList(interval);
            
            CreateMultiColumnTable();
            AcceptChanges();
            ColumnChanged += new DataColumnChangeEventHandler(m_table_ColumnChanged);
            SetupPrimaryKey();
        }


        private void SetupPrimaryKey()
        {
            this.Columns[0].Unique = true;
            this.PrimaryKey = new DataColumn[] { this.Columns[0] };
            this.DefaultView.Sort = this.Columns[0].ColumnName;
            this.DefaultView.ApplyDefaultSort = true;
        }



        public void UpdateValue(int seriesIndex, DateTime t, object value, string flag)
        {
            if (this.DefaultView.Sort == "")
            {
                this.DefaultView.Sort = this.Columns[0].ColumnName;
                this.DefaultView.ApplyDefaultSort = true;
            }

            int rowIndex = DefaultView.Find(t);
            //int idxSeries = m_seriesIndexList.IndexOf(seriesIndex);
            //if (idxSeries < 0)
              //  throw new InvalidOperationException("Internal Error: SeriesIndex :" + seriesIndex);
            //DefaultView[rowIndex].Row[idxSeries + 1] = value;
        }


        /// <summary>
        ///  update TimeSeriesDataSet that holds the source DataTable
        ///  for this column
        /// </summary>
        void m_table_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            Console.WriteLine("Column Changed");

            Console.WriteLine("Column_Changed Event: name={0}; Column={1}; original name={2}",
                             e.Row[e.Column.ColumnName],
                             e.Column.ColumnName,
                             e.Row[e.Column,
                             DataRowVersion.Original]);

            int idx = Columns.IndexOf(e.Column);
            if (idx == 0)
            {
                Console.WriteLine("??? ColumnChanged for " + e.Column.ColumnName);
                return;
            }
            idx -= 1;
            DateTime date = Convert.ToDateTime(e.Row[0]);

            // TO DO...
          //  ds.UpdateValue(m_seriesIndexList[idx], date, e.Row[e.Column]);

            // Debug..
            //            if( this.DefaultView.DataViewManager.
            for (int i = 0; i < Rows.Count; i++)
            {
                DataRow r = this.Rows[i];
                Console.WriteLine(r.RowState);
            }

        }

        private void CreateMultiColumnTable()
        {
            if (m_seriesList.Count == 0)
            {
                return;
            }

            var tbl = m_seriesList[0].Table;
            AppendColumn( tbl.Columns[0], tbl.Columns[0].ColumnName); // DateTime

            for (int i = 0; i < m_seriesList.Count; i++)
            {
                var s = m_seriesList[i];
                tbl = s.Table;
                TimeSeriesName tn = new TimeSeriesName(s.Table.TableName);

                AppendColumn(tbl.Columns[1], tn.siteid.PadRight(8) +tn.pcode); // value column
                if( s.HasFlags)
                  AppendColumn(tbl.Columns[2], "flag"+(i+1)); // flag
            }

        }

        private void AppendColumn(DataColumn dataColumn, string columnName)
        {
            DataTable tbl = dataColumn.Table;

            columnName = MakeUniqueColumnName(columnName);

            Columns.Add(columnName, dataColumn.DataType);

            for (int i = 0; i < tbl.Rows.Count; i++)
            {
                while (Rows.Count <= i)
                {
                    Rows.Add(NewRow());
                }
                Rows[i][columnName] = tbl.Rows[i][dataColumn.ColumnName];
            }
        }

        private string MakeUniqueColumnName(string columnName)
        {
            string rval = columnName;
            int i = 1;
            while (Columns.IndexOf(rval) >= 0)
            {
                rval = columnName + i.ToString();
                i++;
            }
            return rval;
        }

    }
}
