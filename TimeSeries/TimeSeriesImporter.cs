using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries
{
    /// <summary>
    /// TimeSeriesImporter Manages importing data with following features:
    /// 1) set flags
    /// 2) active alarms (TO DO)
    /// 3) compute dependent data (same interval)
    /// 4) compute daily data when encountering midnight values
    /// </summary>
    public class TimeSeriesImporter
    {

        TimeSeriesDatabase m_db;
        TimeSeriesRouting m_routing;
        public TimeSeriesImporter(TimeSeriesDatabase db, TimeSeriesRouting routing)
        {
            m_db = db;
            m_routing = routing;
        }


        public void Import(SeriesList s)
        {

        }
    }
}
