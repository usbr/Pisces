using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.Core;
using System.Data;
namespace Reclamation.TimeSeries.Usgs
{
    public enum UsgsRealTimeParameter
    {
        Discharge = 60,
        ReservoirElevation = 62,
        GageHeight = 65,
        Temperature = 10
        
    }

    /// <summary>
    /// RealTime data from waterdata.usgs.gov/nwis
    /// </summary>
    public class UsgsRealTimeSeries:Series
    {
        UsgsRDBFile m_rdb;
        UsgsRealTimeParameter m_parameter;
        string m_columnName;
        string m_site_no;
        string m_flagColumnName;
        static string m_url = "https://staging.waterservices.usgs.gov/nwis/iv/?format=rdb&sites=14354200&startDT=2012-05-01&endDT=2012-05-01&parameterCd=00060";

        public UsgsRealTimeSeries(string site_no, UsgsRealTimeParameter parameter)
		{
            TimeInterval = TimeSeries.TimeInterval.Irregular;
			this.SiteID = site_no;
            this.Parameter = parameter.ToString();
            m_site_no = site_no;
            m_parameter = parameter;
            m_columnName = "";
            m_flagColumnName = "";
            Table.TableName = "UsgsRealTime_" + parameter.ToString() + "_" + site_no;
            ConnectionString = "Source=USGS;site_no=" + site_no+";UsgsParameter="+parameter.ToString();
            Source = "USGS";
            Provider = "UsgsRealTimeSeries";
		}

        public UsgsRealTimeSeries(TimeSeriesDatabase db,Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow sr)
            : base( db,sr)
        {
            var p = ConnectionStringUtility.GetToken(ConnectionString,"UsgsParameter","");
            m_parameter = (UsgsRealTimeParameter)Enum.Parse(typeof(UsgsRealTimeParameter), p);
            m_site_no = ConnectionStringToken( "site_no");
        }

        public static SeriesList GetSeries(string[] sites, UsgsRealTimeParameter parameter)
        {
            SeriesList rval = new SeriesList();
            foreach (string site in sites)
            {
                UsgsRealTimeSeries s = new UsgsRealTimeSeries(site, parameter);
                rval.Add(s);
            }

            return rval;
        }

        public static UsgsRealTimeSeries Read(string site_no, UsgsRealTimeParameter parameter, DateTime t1, DateTime t2)
        {
            var s = new UsgsRealTimeSeries(site_no, parameter);
            s.ReadCore(t1,t2);
            return s;
        }
       
        protected override Series CreateFromConnectionString()
        {
            UsgsRealTimeSeries s = new UsgsRealTimeSeries(
               ConnectionStringToken("site_no"),
                m_parameter);
            return s;
        }

		/// <summary>
		/// Read Published 15-min stream flow values.  
		/// gets last 10 days.
		/// </summary>
        protected override void ReadCore()
        {
            if (m_db != null)
            {
                base.ReadCore();
            }
            else
            {
                // not most efficient.. we are going for all parameters, 
                // get last 10 days by default.
                string url = m_url;
                url = url.Replace("sites=14354200", "sites=" + SiteID);
                url = url.Replace("startDT=2012-05-01&endDT=2012-05-01", "period=P10D");
                UsgsRealTimeParameter parameter = (UsgsRealTimeParameter)Enum.Parse(typeof(UsgsRealTimeParameter), Parameter);
                int paramCode = (int)parameter;
                url = url.Replace("parameterCd=00060", "parameterCd=000" + paramCode);

                Messages.Add(url);
                ReadSeriesData(url);
            }
        }

		/// <summary>
		/// Read Published 15-min stream flow values.  
		/// gets data between dates specified.
		/// </summary>
		/// <param name="t1">beginning DateTime</param>
		/// <param name="t2">ending DateTime</param>
        protected override void ReadCore(DateTime t1, DateTime t2)
		{
            if (t2 >= DateTime.Now.Date)
            { // don't waste time looking to the future
                t2 = DateTime.Now.Date;
            }
            if (m_db != null)
            {
                if (Utility.AutoUpdate)
                {
                    base.UpdateCore(t1, t2, true);
                }
                base.ReadCore(t1, t2);
            }
            else
            {
                string url = m_url;
                url = url.Replace("sites=14354200", "sites=" + SiteID);
                url = url.Replace("startDT=2012-05-01", "startDT=" + t1.Year + "-" + t1.Month + "-" + t1.Day);
                url = url.Replace("endDT=2012-05-01", "endDT=" + t2.Year + "-" + t2.Month + "-" + t2.Day);
                UsgsRealTimeParameter parameter = (UsgsRealTimeParameter)Enum.Parse(typeof(UsgsRealTimeParameter), Parameter);
                int paramCode = (int)parameter;
                url = url.Replace("parameterCd=00060", "parameterCd=000" + paramCode);

                Messages.Add(url);
                ReadSeriesData(url);
            }
		}

        private void ReadSeriesData(string url)
        {
            int errorCount = 0;
            string[] response = Web.GetPage(url,true);
             
            m_rdb = new UsgsRDBFile(response);
            ParsePreamble();
            if (m_columnName == "")
            {
                return;
            }

            for (int i = 0; i < m_rdb.Rows.Count; i++)
            {
                DataRow row = m_rdb.Rows[i];
                DateTime t = DateTime.MinValue;

                if( !DateTime.TryParse(row["dateTime"].ToString(),out t))
                {
                    break;
                }
                double d = Point.MissingValueFlag;
                Point pt = Point.Missing;
                pt.DateTime = t;
                pt.Flag = row[m_flagColumnName].ToString();

                if (!Double.TryParse(row[m_columnName].ToString(), out  d))
                {
                    Messages.Add("Error reading '" + row[m_columnName] + "' as a number");
                }
                    else
                    {
                        pt.Value = d;
                    }

                int idxTime = this.IndexOf(t);

                if (idxTime >= 0)
                {
                    errorCount++;
                    if (errorCount > 100)
                        continue;
                    string msg = "duplicate record found at date = '" + t.ToString() + "', value =" + d;
                    Logger.WriteLine(msg);
                    Messages.Add(msg);
                    msg = "previously imported record at date ='"+this[idxTime].DateTime+"', value =" + this[idxTime].Value;
                    Logger.WriteLine(msg);
                    Messages.Add(msg);
                }
                    else
                    {
                        Add(pt);
                    }
            }
            
            if (errorCount > 100)
            {
                Logger.WriteLine("Skipped " + (errorCount - 100) + " messages");
            }
        }

        /*
         * 
# ---------------------------------- WARNING ----------------------------------------
# Provisional data are subject to revision. Go to
# http://waterdata.usgs.gov/nwis/help/?provisional for more information.
#
# File-format description:  http://waterdata.usgs.gov/nwis/?tab_delimited_format_info
# Automated-retrieval info: http://waterdata.usgs.gov/nwis/?automated_retrieval_info
#
# Contact:   gs-w_support_nwisweb@usgs.gov
# retrieved: 2012-05-02 15:58:03 EDT	(vaas01)
#
# Data for the following 1 site(s) are contained in this file
#    USGS 13206000 BOISE RIVER AT GLENWOOD BRIDGE NR BOISE ID
# -----------------------------------------------------------------------------------
#
# Data provided for site 13206000
#    DD parameter   Description
#    01   00060     Discharge, cubic feet per second
#    02   00065     Gage height, feet
#
# Data-value qualification codes included in this output:
#     P  Provisional data subject to revision.
#
agency_cd	site_no	datetime	tz_cd	01_00060	01_00060_cd	02_00065	02_00065_cd
5s	15s	20d	6s	14n	10s	14n	10s
USGS	13206000	2012-05-02 10:00	MDT	7830	P	10.63	P
USGS	13206000	2012-05-02 10:15	MDT	7700	P	10.56	P
USGS	13206000	2012-05-02 10:30	MDT	7800	P	10.61	P
USGS	13206000	2012-05-02 10:45	MDT	7780	P	10.60	P
USGS	13206000	2012-05-02 11:00	MDT	7820	P	10.62	P
USGS	13206000	2012-05-02 11:15	MDT	7780	P	10.60	P
USGS	13206000	2012-05-02 11:30	MDT	7830	P	10.63	P
USGS	13206000	2012-05-02 11:45	MDT	7820	P	10.62	P
USGS	13206000	2012-05-02 12:00	MDT	7700	P	10.56	P
USGS	13206000	2012-05-02 12:15	MDT	7850	P	10.64	P
USGS	13206000	2012-05-02 12:30	MDT	7740	P	10.58	P
USGS	13206000	2012-05-02 12:45	MDT	7830	P	10.63	P
USGS	13206000	2012-05-02 13:00	MDT	7910	P	10.67	P
USGS	13206000	2012-05-02 13:15	MDT	7930	P	10.68	P
USGS	13206000	2012-05-02 13:30	MDT	7970	P	10.70	P
USGS	13206000	2012-05-02 13:45	MDT	8000	P	10.72	P
         */


        /// <summary>
        /// Determine column name where this data exists.
        /// </summary>
        private void ParsePreamble()
        {
            int idx = -1;
            string findMe = "#    USGS "+SiteID;
            idx = m_rdb.TextFile.IndexOf(findMe);
            Name = SiteID;
            Source = "USGS";

            if (idx >= 0)
            {
                Name = m_rdb.TextFile[idx].Substring(findMe.Length);
            }
            else
            {
                string msg = "Could not find site name " + findMe;
                Core.Logger.WriteLine(msg);
                Messages.Add(msg);
                return;
            }


            idx = -1;
            if (m_parameter ==  UsgsRealTimeParameter.Discharge)
            {
               idx = m_rdb.TextFile.IndexOf("Discharge, cubic feet per second");
               Units = "cfs";
               Parameter = "Discharge";
            }
            if (m_parameter ==  UsgsRealTimeParameter.GageHeight)
            {
                idx = m_rdb.TextFile.IndexOf("Gage height, feet");
                Units = "feet";
                Parameter = "Gage height";
            }
            if (m_parameter == UsgsRealTimeParameter.Temperature)
            {
                idx = m_rdb.TextFile.IndexOf("Temperature, water, degrees Celsius");
                Units = "degC";
                Parameter = "Water Temperature";
            }
            //"#    02   00062     Elevation of reservoir water surface above datum, feet
            else if( m_parameter == UsgsRealTimeParameter.ReservoirElevation)
            {
                idx = m_rdb.TextFile.IndexOf("Elevation of reservoir water surface above datum, feet");
                Units = "ft";
                Parameter = "elevation";
            }


            if (idx < 0 || idx > m_rdb.DataIndex )
            {
                string msg = "could not find " + m_parameter.ToString();
                Core.Logger.WriteLine(msg);
                Messages.Add(msg);
                m_columnName = "";
                return;
            }
            Name += (" " + Parameter);

            // find the parts of the column name
            /*
            # Data for the following site(s) are contained in this file
            #    USGS 13206000 BOISE RIVER AT GLENWOOD BRIDGE NR BOISE ID
            # -----------------------------------------------------------------------------------
            #
            # Data provided for site 13069500
            #    TS_ID       Parameter Description
            #    47218       00060     Discharge, cubic feet per second

            */
            string line = m_rdb.TextFile[idx];
            string TS_ID = line.Substring(5, 6).Trim();
            string p = line.Substring(17, 5).Trim();
            //string stat = line.Substring(20, 5); RealTime doesn't have stat

            m_columnName = TS_ID + "_" + p;
            m_flagColumnName = m_columnName + "_cd";

            if (m_rdb.Columns.IndexOf(m_columnName) < 0)
            {
                throw new Exception("Could not find column name for '" + line + "'");
            }

        }
    }
}
