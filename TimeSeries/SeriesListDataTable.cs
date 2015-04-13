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
    /// Goal to be able to edit values and flags in this
    /// table which also modifies individual series.
    /// </summary>
    public class SeriesListDataTable : DataTable
    {
        SeriesList m_seriesList;
        List<int> m_columnToSeries; // index to series from each column in table
        /// <summary>
        /// Creates a DataTable composed from multiple time series
        /// </summary>
        public SeriesListDataTable(SeriesList list,
            TimeInterval interval) : base(interval.ToString())
        {
            m_seriesList = list.FilteredList(interval);
            m_columnToSeries = new List<int>();
            CreateMultiColumnSchema();
            AddData();
            AcceptChanges();
            ColumnChanged += new DataColumnChangeEventHandler(m_table_ColumnChanged);
            SetupPrimaryKey();
        }

        private void AddData()
        {
            Series tempSeries = new Series(this, "", TimeInterval.Irregular);

            // tempSeries is not a "valid" series.  just taking advantage
            // of insert and lookup functions
            tempSeries.Table.Columns.Remove("flag"); // this was added by constructor above..

            int colIndex = 1; // skip date column
            do
            {
                Series s = m_seriesList[m_columnToSeries[colIndex]];
                DataRow row = null;

                for (int i = 0; i < s.Count; i++)
                {
                    Point pt = s[i];
                    int idx = tempSeries.IndexOf(pt.DateTime);
                    if (idx < 0)
                    {// need new row..
                        row = NewRow();
                        row[0] = pt.DateTime;
                        row[colIndex] = Point.DoubleOrNull(ref pt);
                        if (s.HasFlags)
                        {
                            row[colIndex+1] = pt.Flag;
                        }
                        // find spot to insert 
                        int sz = tempSeries.Count;
                        if (sz == 0 || pt.DateTime > tempSeries.MaxDateTime)
                        {   // append
                            Rows.Add(row);
                        }
                        else
                        {
                            int j = tempSeries.LookupIndex(pt.DateTime);
                            Rows.InsertAt(row, j);
                        }
                    }
                    else
                    { // using existing row
                        row = Rows[idx];
                        row[colIndex] = Point.DoubleOrNull(ref pt);
                        if (s.HasFlags)
                            row[colIndex+1] = pt.Flag;
                        continue;
                    }
                }
                colIndex++;
                if (s.HasFlags)
                    colIndex++;

            } while (colIndex < Columns.Count-1);



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

        private void CreateMultiColumnSchema()
        {
            if (m_seriesList.Count == 0)
            {
                return;
            }

            var tbl = m_seriesList[0].Table;
            AppendColumn( tbl.Columns[0], tbl.Columns[0].ColumnName); // DateTime
            m_columnToSeries.Add(0);

            for (int i = 0; i < m_seriesList.Count; i++)
            {
                var s = m_seriesList[i];
                tbl = s.Table;
                TimeSeriesName tn = new TimeSeriesName(s.Table.TableName);

                AppendColumn(tbl.Columns[1], tn.siteid.PadRight(8) +tn.pcode); // value column
                m_columnToSeries.Add(i);
                if (s.HasFlags)
                {
                 AppendColumn(tbl.Columns[2], "flag" + (i + 1)); // flag
                 m_columnToSeries.Add(i);
                }
            }

        }

        private DataColumn AppendColumn(DataColumn dataColumn, string columnName)
        {
            DataTable tbl = dataColumn.Table;
            columnName = MakeUniqueColumnName(columnName);
           var rval =  Columns.Add(columnName, dataColumn.DataType);
           return rval;
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
