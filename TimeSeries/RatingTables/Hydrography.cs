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
            
            int folderID = sc.GetOrCreateFolder(
                m_db.GetRootObjects()[0].Name,      //"Untitled"
                   siteID,                          //  SouthBend
                     "Flow Measurements");          //    "Flow Measurements"

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

            sc.AddSeriesCatalogRow(sc.NextID() , folderID, false, 0,
                "measurement", date.ToString("yyy-MM-dd HHMM"),
                siteID, "", "Instant", "", "", "BasicMeasurement", "", "", "", true);

            sc.Save();

            return mr.id;

        }


    }
}
