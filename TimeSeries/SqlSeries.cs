using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.TimeSeries;
using System.Data;

namespace Reclamation.TimeSeries
{
    public class SqlSeries : Series
    {

        private int m_sdi;
        private TimeSeriesDatabase m_db;
       // private SeriesCatalog m_sc;

        public SqlSeries( TimeSeriesDatabase db, int sdi)
        {
            m_sdi = sdi;
            m_db = db;

            // set properties from data base.
            db.UpdateSeriesProperties(this, sdi);


            
        }

        public override void Read()
        {
            Read(DateTime.Parse("1800-01-01"), DateTime.Parse("4000-12-31"));
        }
        public override void Save()
        {
            m_db.Server.SaveTable(this.Table);
            //m_db.UpdatePeriodOfRecord(m_sdi);
        }

        public override void Read(DateTime t1, DateTime t2)
        {
            DataTable tbl = m_db.ReadTimeSeriesTable(m_sdi, t1, t2);
            InitTimeSeries(tbl, Units, TimeInterval, false);
        }
    }
}
