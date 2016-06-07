using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries
{
    public class PiscesMeasurement: PiscesObject
    {
        private TimeSeriesDatabase m_db;
        public PiscesMeasurement(TimeSeriesDatabase db, 
            TimeSeriesDatabaseDataSet.SeriesCatalogRow sr):base(sr)
        {
            this.m_db = db;
        }
    }
}
