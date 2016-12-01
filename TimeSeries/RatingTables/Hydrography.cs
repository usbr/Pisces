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

        public HydrographyDataSet.rating_tablesDataTable GetRatingTables()
        {
            var rval = new HydrographyDataSet.rating_tablesDataTable();
            m_db.Server.FillTable(rval);
            return rval;
        }

        public MeasurementList GetMeasurements(string siteID)
        {
            var rval = new MeasurementList();
            var tbl = m_db.GetSeriesCatalog("siteid = '" + siteID + "' and provider = 'BasicMeasurement'");

            foreach (TimeSeriesDatabaseDataSet.SeriesCatalogRow item in tbl)
            {
                rval.Add(m_db.Factory.GetMeasurement(item) );
            }

            return rval;
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
            mr.discharge = flow;
            mr.stage = stage;
            mr.quality = quality;
            mr.party = party;
            mr.notes = notes;
            dt.Rows.Add(mr);
            m_db.Server.SaveTable(dt);


            sc.AddMeasurement(mr);
            
            sc.Save();

            return mr.id;

        }

        public static string MeasurementDateFormat = "yyyy-MM-dd HHMM";


        static HydrographyDataSet.measurementDataTable s_measurememnt;

         internal HydrographyDataSet.measurementDataTable GetMeasurements()
        {
            if (s_measurememnt == null)
            {
                s_measurememnt = new HydrographyDataSet.measurementDataTable();
                if (m_db.Server.TableExists("measurement"))
                m_db.Server.FillTable(s_measurememnt, "select * from measurement order by siteid, date_measured");
            }
            return s_measurememnt;
         }
        
        public void AddMeasurementsToTree()
        {
            Performance p = new Performance();
            var sc = m_db.GetSeriesCatalog();
            var measurements = GetMeasurements();

            for (int i = 0; i < measurements.Count; i++)
            {
                var m = measurements[i];

                sc.AddMeasurement(m);

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

        public void AddRatingTablesToTree()
        {
            Performance p = new Performance();
            var sc = m_db.GetSeriesCatalog();
            var r = GetRatingTables();

            for (int i = 0; i < r.Count; i++)
            {
                var m = r[i];

                sc.AddRatingTable(m);

                if (i % 100 == 0)
                {
                    System.Windows.Forms.Application.DoEvents();
                    double x = i * 1.0 / r.Count * 100.0;
                    Logger.WriteLine(x.ToString("F2") + "%", "ui");
                }
            }
            p.Report(); // 26 seconds
            sc.Save();
        }
    }
}
