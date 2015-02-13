using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries
{
    public class SeriesProperties
    {

        private TimeSeriesDatabaseDataSet.seriespropertiesDataTable m_seriesProperties;

        int m_seriesID;
        TimeSeriesDatabase m_db;
        public SeriesProperties(int id)
        {
            m_seriesID = id;
            m_seriesProperties = new TimeSeriesDatabaseDataSet.seriespropertiesDataTable();
        }
        public SeriesProperties(int id, TimeSeriesDatabase db)
        {
            m_seriesID = id;
            m_seriesProperties = db.GetSeriesProperties(true);
            m_db = db;
        }

        public string Get(string name,string defaultValue= "")
        {
           return m_seriesProperties.Get(name, defaultValue, m_seriesID);
        }

        public void Set(string name, string value )
        {
            m_seriesProperties.Set(name, value, m_seriesID);
        }

        public bool Contains(string name)
        {
           return m_seriesProperties.Contains(name, m_seriesID);
        }

        public void Save()
        {
            if (m_db != null)
            {
                m_seriesProperties.Save();
            }
        }
    }
}
