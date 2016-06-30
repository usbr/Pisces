using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System.Data;

namespace Reclamation.TimeSeries.Profile
{

    /// <summary>
    /// WaterTemperatureDatabase reads depth and water temperature data from 
    /// a Pisces database
    /// </summary>
    public class WaterProfileDatabase
    {

        TimeSeriesDatabase m_db;
        SeriesList sensors = new SeriesList();
        SeriesList depths = new SeriesList();
        Series waterSurfaceSeries;

        public WaterProfileDatabase(TimeSeriesDatabase db)
        {
            m_db = db;
        }


        /// <summary>
        /// Defines the timeseries data (by name) that defines
        /// water temperature values, and the corresponding depths 
        /// of the water temperature sensors
        /// </summary>
        /// <param name="sensorSeriesNames"></param>
        /// <param name="depthSeriesNames"></param>
        public void LoadSeries(string[] sensorSeriesNames, string[] depthSeriesNames, string waterSurfaceSeriesName="")
        {
            if (sensorSeriesNames.Length != depthSeriesNames.Length)
                throw new ArgumentException("sensorSeriesNames must have the same number of items as depthSeriesNames");
            for (int i = 0; i < sensorSeriesNames.Length; i++)
			{
                var ss = m_db.GetSeriesFromName(sensorSeriesNames[i]);
                var ds = m_db.GetSeriesFromName(depthSeriesNames[i]);

                depths.Add(ds);
                sensors.Add(ss);
            }

            if (waterSurfaceSeriesName.Trim() != "")
            {
                waterSurfaceSeries = m_db.GetSeriesFromName(waterSurfaceSeriesName);
                if (waterSurfaceSeries != null)
                    waterSurfaceSeries.Read();
            }

            depths.Read();
            sensors.Read();

        }


        public double GetWaterSurface(DateTime t)
        {
            if (waterSurfaceSeries == null)
                return Point.MissingValueFlag;

           int idx = waterSurfaceSeries.IndexOf(t);
           if (idx >= 0 && !waterSurfaceSeries[idx].IsMissing)
               return waterSurfaceSeries[idx].Value;

           return Point.MissingValueFlag;
        }

        /// <summary>
        /// Returns a DataTable with two columns (value, depth)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public DataTable GetPlotData(DateTime t)
        {
            
            DataTable rval = new DataTable();
            rval.Columns.Add("value", typeof(double));
            rval.Columns.Add("depth", typeof(double));
            
            for (int i = 0; i < sensors.Count; i++)
            {
               int idx_s= sensors[i].IndexOf(t);
               int idx_d = depths[i].IndexOf(t);
                
                if( idx_s >=0 && idx_d >=0
                    && !sensors[i][idx_s].IsMissing && !depths[i][idx_d].IsMissing)
                {
                    rval.Rows.Add(sensors[i][idx_s].Value, depths[i][idx_d].Value);
                }
            }

            return rval;
        }

    }
}
