using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.RatingTables
{
    /// <summary>
    /// Hydrography class is used to group flow measurment 
    /// and rating table methods that interact with a database.
    /// </summary>
    public class Hydrography
    {
        TimeSeriesDatabase m_db;
        public Hydrography(TimeSeriesDatabase db)
        {
            m_db = db;
        }

        /// <summary>
        /// Creates an new Manual measurement.
        /// Returns new MeasurementNumber for a Manaul Measurement.
        /// </summary>
        /// <param name="site_code"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public int NewMeasurement(string siteID, DateTime date,double flow =0.0,
            double stage = 0.0, string quality="", string party="", string notes="")
        {

            var sc = m_db.GetSeriesCatalog();

            var dt = new HydrographyDataSet.measurementDataTable();
            var mr = dt.NewmeasurementRow();
            mr.id= m_db.Server.NextID("measurement", "id");
            mr.siteid = siteID;
            mr.date_measured = date;
            mr.discharge = 0;
            mr.stage = stage;
            mr.quality = quality;
            mr.party = party;
            mr.notes = notes;
            dt.Rows.Add(mr);
            m_db.Server.SaveTable(dt);


            sc.AddMeasurement(siteID, date.ToString(MeasurementDateFormat),date);
            
            sc.Save();

            return mr.id;

        }

        public string MeasurementDateFormat = "yyyy-MM-dd HHMM";



         HydrographyDataSet.measurementDataTable GetMeasurements()
        {
            var rval = new HydrographyDataSet.measurementDataTable();
            m_db.Server.FillTable(rval,"select * from measurement order by siteid, date_measured" );

            return rval;
        }

        public void SyncTreeWithMeasurementTable()
        {
            Performance p = new Performance();
            var sc = m_db.GetSeriesCatalog();
            var measurements = GetMeasurements();

            for (int i = 0; i < measurements.Count; i++)
            {
                var m = measurements[i];

                sc.AddMeasurement(m.siteid, m.date_measured.ToString(MeasurementDateFormat),m.date_measured);

                if (i % 100 == 0)
                {
                    System.Windows.Forms.Application.DoEvents();
                    double x = i*1.0 / measurements.Count * 100.0;
                    Logger.WriteLine(x.ToString("F2")+"%", "ui");
                }
            }
            p.Report(); // 26 seconds
            sc.Save();
        }
    }
}
