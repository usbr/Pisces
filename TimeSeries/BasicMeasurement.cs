using Reclamation.TimeSeries.RatingTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries
{
    public class BasicMeasurement: PiscesObject
    {
        private TimeSeriesDatabase m_db;
        TimeSeriesDatabaseDataSet.SeriesCatalogRow m_sr;

        HydrographyDataSet.measurementRow m_measurementRow;

        public BasicMeasurement(TimeSeriesDatabase db, 
            TimeSeriesDatabaseDataSet.SeriesCatalogRow sr):base(sr)
        {
            this.m_db = db;
            this.m_sr = sr;
            // TO DO: Lookup Measurement Row

        }


    }
}
