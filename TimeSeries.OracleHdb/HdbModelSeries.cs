using System;
using Reclamation.Core;
using HdbPoet;
namespace Reclamation.TimeSeries.OracleHdb
{
    public class HdbModelSeries:Series
    {

        string m_table;
        string site_datatype_id;
        string model_run_id; // scenario
        string model_run_name;
        string run_date;
        string hdb_host_name;
        public HdbModelSeries(TimeSeriesDatabase db,
            Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow sr):base(db,sr)
        {
            ExternalDataSource = true;
            ReadOnly = true;
            m_table = ConnectionStringUtility.GetToken(ConnectionString, "m_table", "");
            site_datatype_id = ConnectionStringUtility.GetToken(ConnectionString, "site_datatype_id", "-1");
            model_run_id = ConnectionStringUtility.GetToken(ConnectionString, "model_run_id", "-1");
            model_run_name = ConnectionStringUtility.GetToken(ConnectionString, "mode_run_name", "");
            run_date = ConnectionStringUtility.GetToken(ConnectionString, "run_date", "");
            hdb_host_name = ConnectionStringUtility.GetToken(ConnectionString, "hdb_host_name", "");

            ScenarioName = BuildScenairoName(model_run_name, model_run_id, run_date);
            Appearance.LegendText = SiteName + " " + Name;
        }


        public static string BuildConnectionString(string m_table, string model_run_id,
            string model_run_name,string run_date, string site_datatype_id)
        {
            string cs = "model_run_id=" + model_run_id
                     + ";site_datatype_id=" + site_datatype_id
                     + ";m_table=" + m_table
                     + ";model_run_name=" + model_run_name
                     + ";run_date=" + run_date
                     + ";hdb_host_name=" + Hdb.Instance.Server.Host;
            return cs;
        }
        /// <summary>
        /// Scenairo name for UI
        /// </summary>
        /// <param name="model_run_name"></param>
        /// <param name="model_run_id"></param>
        /// <param name="run_date"></param>
        /// <returns></returns>
        public static string BuildScenairoName(string model_run_name,string model_run_id, string run_date )
        {
            return "(" + model_run_id + ") " + model_run_name + " : " + run_date;
        }

        /// <summary>
        /// Creates path component of Scenario
        /// </summary>
        /// <param name="model_run_name"></param>
        /// <param name="model_run_id"></param>
        /// <param name="run_date"></param>
        /// <returns></returns>
        public static string BuildScenairoPath(string model_run_name, string model_run_id, string run_date)
        {
            return "model_run_id=" + model_run_id + ";model_run_name=" + model_run_name + ";run_date=" + run_date;
        }


        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            string sql = "Select start_date_time,value " +
                "from " + m_table + " where model_run_id = " + model_run_id
                + " and site_datatype_id = " + site_datatype_id 
                + " and start_date_time >= "+Hdb.ToHdbDate(t1)
                + " and start_date_time <= "+Hdb.ToHdbDate(t2)
                + 
                " order by start_date_time desc";

            var hdb1 = HdbPoet.Hdb.GetInstance(hdb_host_name);

            if (hdb1 == null)
            {
                Clear();
                Logger.WriteLine("No login to oracle");
                return;
            }


            Table = hdb1.Server.Table(m_table, sql);
        }

        public override Series CreateScenario(TimeSeriesDatabaseDataSet.ScenarioRow scenario)
        {
            if (ScenarioName  == scenario.Name)
            {
                return this;
            }

            var sr = m_db.GetNewSeriesRow(false);
            sr.ItemArray = SeriesCatalogRow.ItemArray;


            string cs = sr.ConnectionString;
            string scenario_model_run_id = ConnectionStringUtility.GetToken(scenario.Path,"model_run_id","-1");
            string scenario_model_run_name = ConnectionStringUtility.GetToken(scenario.Path, "model_run_name", "");
            string scenario_run_date = ConnectionStringUtility.GetToken(scenario.Path, "run_date", "");

            cs = ConnectionStringUtility.Modify(cs, "model_run_id", scenario_model_run_id);
            cs = ConnectionStringUtility.Modify(cs, "model_run_name", scenario_model_run_name);
            cs = ConnectionStringUtility.Modify(cs, "run_date", scenario_run_date);

            sr.ConnectionString = cs;
            var s = new HdbModelSeries( m_db, sr);
            s.Appearance.LegendText = BuildScenairoName(scenario_model_run_name, scenario_model_run_id, scenario_run_date)
                + " " +Name;
            return s;
            

        }
    }
}
