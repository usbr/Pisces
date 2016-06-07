using System;
using System.Collections.Generic;
using System.Text;

namespace Reclamation.TimeSeries
{
    public class PiscesFolder:PiscesObject
    {
        private TimeSeriesDatabase m_db;
        //public PiscesFolder(TimeSeriesDatabase db)
        //{
        //    m_db = db;
        //    base.IsFolder = true;
        //}

        public PiscesFolder(TimeSeriesDatabase db, 
            TimeSeriesDatabaseDataSet.SeriesCatalogRow sr):base(sr)
        {
            this.m_db = db;
        }

        public void AddSeries(Series s)
        {
            m_db.AddSeries(s, this);
        }

        public PiscesFolder AddFolder(string name)
        {
            return m_db.AddFolder(this, name);
        }

        public bool FolderExists(string name)
        {
            return m_db.FolderExists(name, this.ID);
        }
        
    }
}
