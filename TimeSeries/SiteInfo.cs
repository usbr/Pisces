namespace Reclamation.TimeSeries
{
    public class SiteInfo 
    {
        TimeSeriesDatabase m_db;
        string m_siteID;
        TimeSeriesDatabaseDataSet.sitecatalogRow m_siteRow;

        public SiteInfo(TimeSeriesDatabase db,string siteID) 
        {
            m_db = db;
            m_siteID = siteID;
            
            var a = db.GetSiteCatalog("siteid = '"+m_siteID +"'");
            if( a.Rows.Count == 0)
                m_siteRow = a.NewsitecatalogRow();
            else
            {
                m_siteRow = a[0];
            }
        }

        public string state
        {
            get
            {
                if (m_siteRow.IsstateNull())
                    return "";
                return m_siteRow.state;
            }
        }
            public string timezone
        {
            get
            {
                if (m_siteRow.IstimezoneNull())
                    return "";
                return m_siteRow.timezone;
            }
        }

            public TimeSeriesDatabaseDataSet.SeriesCatalogDataTable SeriesList()
            {
                var rval = new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();
                var sql = "select * from seriescatalog where siteid='" + m_siteID + "'";
                m_db.Server.FillTable(rval, sql);
                return rval;
            }
        public TimeSeriesDatabaseDataSet.parametercatalogDataTable Parameters()
        {
            var rval = new TimeSeriesDatabaseDataSet.parametercatalogDataTable();
             
            var sql = "select * from parametercatalog where id in ( "
                      + "select parameter from seriescatalog where "
                      + "isfolder =0 and siteid='"+m_siteID+"' )";
            m_db.Server.FillTable(rval, sql);
            return rval;
        }
        public TimeSeriesDatabaseDataSet.seriespropertiesDataTable SeriesProperties()
        {
            var rval = new TimeSeriesDatabaseDataSet.seriespropertiesDataTable();

            var sql = "select * from seriesproperties where seriesid in ( "
                      + "select seriesid from seriescatalog where "
                      + " isfolder = 0 and siteid='" + m_siteID + "' )";
            m_db.Server.FillTable(rval, sql);
            return rval;
        }

        public TimeSeriesDatabaseDataSet.sitepropertiesDataTable SiteProperties()
        {
            var rval = new TimeSeriesDatabaseDataSet.sitepropertiesDataTable();

            var sql = "select * from siteproperties where siteid in ( "
                      + "select siteid from seriescatalog where "
                      + " isfolder = 0 and siteid='" + m_siteID + "' )";
            m_db.Server.FillTable(rval, sql);
            return rval;
        }

        
    }
}
