using System;
using Reclamation.Core;

namespace Reclamation.TimeSeries.OracleHdb
{
    public class HdbSeries:Series
    {
        string hdb_site_datatype_id;
        string hdb_r_table;
        string hdb_interval;
        string hdb_instant_interval;
        string hdb_time_zone;
        string hdb_host_name;
        public HdbSeries( TimeSeriesDatabase db, Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow sr)
            : base(db, sr)
        {
            ExternalDataSource = true;
            ReadOnly = true;
            Init();
        }

        private void Init()
        {
            hdb_r_table = ConnectionStringToken( "hdb_r_table", "");
            hdb_site_datatype_id = ConnectionStringToken( "hdb_site_datatype_id", "-1");
            hdb_interval = ConnectionStringToken( "hdb_interval", "");
            hdb_instant_interval = ConnectionStringToken( "hdb_instant_interval", "15");
            hdb_time_zone = ConnectionStringToken( "hdb_time_zone", "MST");
            hdb_host_name = ConnectionStringToken( "hdb_host_name", "");
        }


        protected override void ReadCore()
        {
            var por = GetPeriodOfRecord();
            ReadCore(por.T1, por.T2);
        }
        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            var hdb1 = HdbPoet.Hdb.GetInstance(hdb_host_name);

            if (hdb1 == null)
            {
                Clear();
                Logger.WriteLine("No login to oracle");
                return;
            }

            var tbl = hdb1.Table(Convert.ToDecimal(hdb_site_datatype_id), hdb_r_table, hdb_interval,
                Convert.ToInt32(hdb_instant_interval), t1, t2, hdb_time_zone);
            tbl.Columns.Remove("SourceColor");
            tbl.Columns.Remove("ValidationColor");

            this.Table = tbl;
        }

        public override PeriodOfRecord GetPeriodOfRecord()
        {
            string sql = "select min(start_date_time),max(end_date_time),count(*) from "+hdb_r_table +" where site_datatype_id = "+hdb_site_datatype_id;

            var hdb1 = HdbPoet.Hdb.GetInstance(hdb_host_name);
            var tbl = hdb1.Server.Table("stat", sql);
            if (tbl.Rows[0][0] == DBNull.Value || tbl.Rows[0][1] == DBNull.Value)
            {
                return base.GetPeriodOfRecord();
            }

            var t1 = Convert.ToDateTime(tbl.Rows[0][0]);
            var t2 = Convert.ToDateTime(tbl.Rows[0][1]);
            var count = Convert.ToInt32(tbl.Rows[0][2]);
            var rval = new PeriodOfRecord(t1, t2, count);
            return rval;

        }
    }
}
