using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;
using System.IO;
using System.Text.RegularExpressions;

namespace Reclamation.TimeSeries.Owrd
{
    /// <summary>
    /// Oregon Department of Water Resources (web data)
    /// </summary>
    public class OwrdSeries: Series
    {

        string station_nbr;
        string dataset;
        bool m_includeProvisional=false;

        public enum OwrdDataSet {MDF ,
    Instantaneous_Flow ,
    Instantaneous_Stage,
    Midnight_Volume,
    Midnight_Stage,
    Mean_Daily_Volume,
    Mean_Daily_Stage,
    Measurements,
    RatingCurve}
    
        /// <summary>
        /// 
        /// <param name="station_nbr">Gage station number</param>
        /// <param name="dataset">one of MDF 
    ///Instantaneous_Flow 
    ///Instantaneous_Stage
    ///Midnight_Volume
    ///Midnight_Stage
    ///Mean_Daily_Volume
    ///Mean_Daily_Stage
    ///Measurements
    ///RatingCurve</param>
        /// </summary>
        public OwrdSeries(string station_nbr, string dataset="MDF", bool includeProvisional=false)
        {
            this.station_nbr = station_nbr;
            this.m_includeProvisional = includeProvisional;
            this.dataset = dataset;
            Init(dataset);
        }

        private void Init(string dataset)
        {
            TimeInterval = TimeInterval.Daily;
            if (dataset == "MDF")
                Units = "cfs";
            if (dataset == "Midnight_Volume")
                Units = "acre-feet";
            if (dataset == "Midnight_Stage")
                Units = "feet";

            if( dataset == OwrdDataSet.Mean_Daily_Stage.ToString()  )
               Units = "feet";

            ConnectionString = "StationNumber="+station_nbr+";DataSet=" + dataset + ";Provisional=" + m_includeProvisional;
            SetName();
            //Parameter = dataset;
            this.Source = "owrd";
            Provider = "OwrdSeries";

        }

        private void SetName()
        {
            Name = "OWRD " + station_nbr;

            string fn = FileUtility.GetFileReference("owrd_station_list.csv");
            //string fn = Path.Combine(FileUtility.GetExecutableDirectory(), "owrd_station_list.csv");
            if (File.Exists(fn))
            {
                CsvFile csv = new CsvFile(fn, CsvFile.FieldTypes.AllText);
                var rows = csv.Select("station_id = '"+station_nbr+"'");
                if (rows.Length == 1)
                {
                    Name = rows[0]["Name"].ToString();
                }
            }
            Name += " " + dataset;

        }

        protected override Series CreateFromConnectionString()
        {
            OwrdSeries s = new OwrdSeries(station_nbr,dataset,m_includeProvisional);
            return s;
        }
        public OwrdSeries(TimeSeriesDatabase db, TimeSeriesDatabaseDataSet.SeriesCatalogRow sr ):base(db,sr)
        {
            dataset = ConnectionStringUtility.GetToken(this.ConnectionString, "DataSet", "MDF");
            m_includeProvisional = ConnectionStringUtility.GetBoolean(this.ConnectionString, "Provisional",false);
            station_nbr = ConnectionStringUtility.GetToken(this.ConnectionString, "StationNumber", "");
        }

        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            Clear();
            if (m_db == null)
            {
                ReadFromWeb(t1, t2);
            }
            else
            {
                //if (true) //OwrdSeries.AutoUpdate)
                //{
                //    if (t2 >= DateTime.Now.Date.AddDays(1))
                //    { // don't waste time looking to the future
                //        // snotel includes today
                //        t2 = DateTime.Now.Date;
                //    }
                //    base.UpdateCore(t1, t2, true);
                //}
                base.ReadCore(t1, t2);
            }

        }

        private void ReadFromWeb(DateTime t1, DateTime t2)
        {
            //http://apps.wrd.state.or.us/apps/sw/hydro_near_real_time/hydro_download.aspx?station_nbr=14202450&start_date=1/16/2014%2012:00:00%20AM&end_date=1/24/2014%2012:00:00%20AM&dataset=Instantaneous_Stage&format=xls

            string url = "http://apps.wrd.state.or.us/apps/sw/hydro_near_real_time/hydro_download.aspx?"
            + "station_nbr=" + station_nbr + "&dataset=" + dataset + "&format=tsv&"
            + "start_date=" + t1.ToString("MM/dd/yyyy")
            + "&end_date=" + t2.ToString("MM/dd/yyyy");




            string filename = FileUtility.GetTempFileName(".txt");
            Web.GetFile(url, filename);

            TextFile tf = new TextFile(filename);
            filename = FileUtility.GetTempFileName(".csv");

            CreateCsvFile(filename, tf);
            ParseFile(filename);
        }

        private void ParseFile(string filename)
        {
            CsvFile csv = new CsvFile(filename);

            Clear();
            for (int i = 0; i < csv.Rows.Count; i++)
            {
                DateTime t = Convert.ToDateTime(csv.Rows[i]["record_date"]);

                string colName = "midnight_volume_acft";
                if (dataset == "MDF")
                    colName = "mean_daily_flow_cfs";
                if (dataset == OwrdDataSet.Instantaneous_Stage.ToString())
                    colName = "instananteous_stage_ft";

                if (dataset == OwrdDataSet.Instantaneous_Flow.ToString())
                    colName = "instananteous_flow_cfs";

                if (csv.Rows[i][colName] == DBNull.Value ||
                    csv.Rows[i][colName].ToString().Trim() == "")
                {
                    AddMissing(t);
                    continue;
                }

                double value = Convert.ToDouble(csv.Rows[i][colName]);
                string flag = csv.Rows[i]["published_status"].ToString();

                if (flag == "Published" || flag == "PUB" || m_includeProvisional)
                    Add(t, value, flag);


            }

            if( Count >0)
            NormalizeDaily(MinDateTime, MaxDateTime);
        }

        /// <summary>
        /// converts tab separated file to csv format
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="tf"></param>
        private static void CreateCsvFile(string filename, TextFile tf)
        {
            StreamWriter sw = new StreamWriter(filename);
            for (int i = 0; i < tf.Length; i++)
            {

                string[] tokens = Regex.Split(tf[i].Trim(), @"\t");
                for (int j = 0; j < tokens.Length; j++)
                {
                    tokens[j] = CsvFile.EncodeCSVCell(tokens[j]);
                }

                string output = String.Join(",", tokens);
                sw.WriteLine(output);

            }
            sw.Close();
        }

    }
}
