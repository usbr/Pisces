using System;
using System.Data;
using HdbPoet;
using Reclamation.Core;

namespace Reclamation.TimeSeries.OracleHdb
{
    public class HdbModelTreeBuilder
    {

        /// <summary>
        /// Creates table that can be 
        /// appended to the SeriesCatalog in Pisces
        /// </summary>
        /// <returns></returns>
        public static TimeSeriesDatabaseDataSet.SeriesCatalogDataTable PiscesSeriesCatalog(int model_id,
            string m_table, // model table i.e. m_month, m_day,...
            DateTime t, // consider model runs after this date
            int nextPiscesID, // next avaliable id in Pisces
            int parentID) // container for this model
        { // get unique list of site_datatype_id -- based on these parameters

            var rval = new TimeSeriesDatabaseDataSet.SeriesCatalogDataTable();
            DataTable sitesAndParameters = Hdb.Instance.ModelParameterList(t, model_id, m_table);
            DataTable siteList = DataTableUtility.SelectDistinct(sitesAndParameters, "Site_ID");
            int model_run_id =-1;
            string model_run_name = "";
            string run_date = "";
            Hdb.Instance.FirstModelRunInfo(t, model_id,out model_run_id,out model_run_name,out run_date);
            //string scenarioName =  "";// "(" + model_run_id + ")";

            for (int i = 0; i < siteList.Rows.Count; i++)
            {
                int site_id = Convert.ToInt32(siteList.Rows[i]["site_id"]);
                DataRow[] rows = sitesAndParameters.Select("Site_ID=" + site_id);
                int siteRowID = nextPiscesID++;
                rval.AddSeriesCatalogRow(siteRowID, parentID, true, 1, "HdbModel",
                rows[0]["site_common_name"].ToString(), "", "", "", "", "",  "","","","",true);

                for (int j = 0; j < rows.Length; j++)
                {
                    string cs = HdbModelSeries.BuildConnectionString(m_table, model_run_id.ToString(),model_run_name,run_date, rows[j]["site_datatype_id"].ToString());
                    rval.AddSeriesCatalogRow(nextPiscesID++, siteRowID, false, j,
                        "HdbModel", rows[j]["datatype_common_name"].ToString(),
                        rows[j]["site_common_name"].ToString(),
                        rows[j]["unit_common_name"].ToString(),
                        IntervalString(m_table),
                        rows[j]["datatype_common_name"].ToString(),
                        "",  "HdbModelSeries",cs , "", "",true);

                }
                

            }
            return rval;
        }

        
        private static string IntervalString(string modelTable)
        {
            if( String.Compare("m_month",modelTable,true) ==0)
                return "Monthly";
            if (String.Compare("m_hour", modelTable, true) == 0)
                return "Hourly";
            if (String.Compare("m_day", modelTable, true) == 0)
                return "Daily";
            if (String.Compare("m_year", modelTable, true) == 0)
                return "Yearly";

            return "Irregular";

        }

    }
}
