using Reclamation.TimeSeries.RatingTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.RatingTables
{
    public class BasicMeasurement: PiscesObject
    {
        private TimeSeriesDatabase m_db;
        TimeSeriesDatabaseDataSet.SeriesCatalogRow m_sr;

        HydrographyDataSet.measurementRow m_measurementRow;

        public HydrographyDataSet.measurementRow MeasurementRow
        {
            get { return m_measurementRow; }
        }

        
        public BasicMeasurement(TimeSeriesDatabase db, 
            TimeSeriesDatabaseDataSet.SeriesCatalogRow sr):base(sr)
        {
            this.m_db = db;
            this.m_sr = sr;
            int id = Convert.ToInt32(ConnectionStringToken("id", "-1"));
            var tbl = m_db.Hydrography.GetMeasurements();

            m_measurementRow = tbl.FindByid(id);

        }


    }
}
