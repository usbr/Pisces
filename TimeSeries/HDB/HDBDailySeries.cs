using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Reclamation.Core;
using System.Configuration;
using System.IO;
using System.Data.Linq;
using System.Linq;

namespace Reclamation.TimeSeries.HDB
{

    public enum HDBServer { UCHDB, LCHDB, ECAOHDB };


    /// <summary>
    /// Reads Daily Data from HDB 
    /// </summary>
    public class HDBDailySeries : Reclamation.TimeSeries.Series
    {

        int m_sdi=-1;
        HDBServer  m_server;
        public HDBDailySeries(int sdi,HDBServer server )
        {
            m_sdi = sdi;
            m_server = server;
            this.TimeInterval = TimeSeries.TimeInterval.Daily;
            Source = ""; //  need icon for HDB
            Provider = "HDBDailySeries";
            ConnectionString = "server=" + m_server
            + ";sdi=" + m_sdi + ";LastUpdate=" + DateTime.Now.ToString(DateTimeFormatInstantaneous);
            Init();
        }

        public HDBDailySeries(TimeSeriesDatabase db, TimeSeriesDatabaseDataSet.SeriesCatalogRow sr)
            : base(db, sr)
        {
            string str = ConnectionStringUtility.GetToken(ConnectionString, "server", "");
            m_server = (HDBServer)Enum.Parse(typeof(HDBServer), str);
            m_sdi = ConnectionStringUtility.GetIntFromConnectionString(ConnectionString, "sdi");
        }

        private void Init()
        {
            ReadOnly = true;
            Appearance.LegendText = Name;
        }

        /// <summary>
        /// CreateFromConnectionString is used when calling Update to
        /// update a portion of the record for this series.
        /// </summary>
        /// <returns></returns>
        protected override Series CreateFromConnectionString()
        {
            string str = ConnectionStringUtility.GetToken(ConnectionString, "server", "");
            m_server = (HDBServer)Enum.Parse(typeof(HDBServer), str);
            m_sdi = ConnectionStringUtility.GetIntFromConnectionString(ConnectionString, "sdi");

            HDBDailySeries s = new HDBDailySeries(m_sdi,m_server);
            return s;
        }
        protected override void ReadCore()
        {
           ReadCore(TimeSeriesDatabase.MinDateTime, TimeSeriesDatabase.MaxDateTime);
        }
        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            if (m_db == null)
                ReadFromWeb(t1, t2);
            else
                base.ReadCore(t1, t2);
        }

        private void ReadFromWeb(DateTime t1, DateTime t2)
        {
            if (t2 >= DateTime.Now && t2.Year < 6000 )
            {
                t2 = DateTime.Now.Date;
            }
            //http://ibr3lcrxcn01:8080/HDB_CGI.com?sdi=1930&tstp=DY&syer=2014&smon=4&sday=2&eyer=2015&emon=4&eday=16&format=3
            string payload = "sdi=" +m_sdi
                +"&tstp=DY"
                + "&syer=" + t1.Year.ToString()
                + "&smon=" + t1.Month.ToString()
                + "&sday=" + t1.Day.ToString()
                + "&eyer=" + t2.Year.ToString()
                + "&emon=" + t2.Month.ToString()
                + "&eday=" + t2.Day.ToString()
                + "&format=3";

            string cgi = Reclamation.TimeSeries.ReclamationURL.GetUrlToDataCgi(m_server.ToString(), TimeSeries.TimeInterval.Daily);
            Messages.Add(cgi + "?" + payload);
            var fn = FileUtility.GetTempFileName(".txt");
            Web.GetTextFile(cgi + "?" + payload, fn,false);
            var data = File.ReadAllLines(fn);
            //string[] data = Web.GetPage(cgi, payload, false);

            /*
DATETIME,     SDI_1930
04/09/2015 00:00,      1083.47
04/10/2015 00:00,      1083.25
04/11/2015 00:00,      1082.98
04/12/2015 00:00,      1082.85
04/13/2015 00:00,      1082.63
04/14/2015 00:00,      1082.38
04/15/2015 00:00,      1082.23
04/16/2015 00:00,      1081.91
             */
            

            int errorCount = 0;
            for (int i = 1; i < data.Length; i++)
            {
                string[] tokens = data[i].Split(',');

                if (tokens.Length != 2)
                {
                    continue;
                }

                DateTime t;
                if (DateTime.TryParse(tokens[0], out t))
                {
                    double result = Point.MissingValueFlag;
                    if (!double.TryParse(tokens[1], out result))
                    {
                        if (errorCount < 50)
                            Logger.WriteLine("Error parsing " + data[i]);
                        errorCount++;
                        AddMissing(t);
                    }
                    else
                    {
                        Add(t, result);
                    }
                }
            }
            Messages.Add("Returned " + Count + " records ");
        }

    }
}
