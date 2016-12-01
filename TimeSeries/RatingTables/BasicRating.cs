using Reclamation.TimeSeries.RatingTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.RatingTables
{
    public class BasicRating: PiscesObject
    {
        private TimeSeriesDatabase m_db;
        TimeSeriesDatabaseDataSet.SeriesCatalogRow m_sr;

        HydrographyDataSet.rating_tablesRow m_rating_tablesRow;

        public HydrographyDataSet.rating_tablesRow RatingRow
        {
            get {
                return m_rating_tablesRow;
            }
        }

        public Dictionary<double, double> Points
        {
            get{
            var rt = new Dictionary<double, double>();
               var lines = m_rating_tablesRow.csv_table.Split('\n');
                foreach (string s in lines)
	             {
		           var tokens = s.Split(',');
                    double x,y;
                    if( tokens.Length == 2 
                        && double.TryParse(tokens[0],out x)
                        && double.TryParse(tokens[1],out y))
                        rt.Add(x,y);
                 }

            return rt;
            }
        }

        public BasicRating(TimeSeriesDatabase db, 
            TimeSeriesDatabaseDataSet.SeriesCatalogRow sr):base(sr)
        {
            this.m_db = db;
            this.m_sr = sr;
            int id = Convert.ToInt32(ConnectionStringToken("id", "-1"));
            Console.WriteLine("ctor BasicRating  id =" + id);
            var tbl = m_db.Hydrography.GetRatingTables();

            m_rating_tablesRow = tbl.FindByid(id);

        }

        
    }
}
