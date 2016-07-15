using System;
using System.Data;
using Reclamation.Core;
using System.Text.RegularExpressions;

namespace Reclamation.TimeSeries.Usgs
{
    /// <summary>
    /// USGS Daily Data from the web 
    /// </summary>
    public class UsgsGroundWaterLevelSeries : Series
    {
        UsgsRDBFile m_rdb;
        string m_columnName;
        string m_site_no;
        //string m_flagColumnName;
        public UsgsGroundWaterLevelSeries(string site_no)
        {
            this.SiteID = site_no;
            m_site_no = site_no;
            m_columnName = "lev_va";
            //m_flagColumnName = "";
            Table.TableName = "USGSGroundWaterLevel" + "_" + site_no;
            ConnectionString = "Source=USGS;site_no=" + site_no;
            TimeInterval = TimeInterval.Daily;
            Source = "USGS";
            Provider = "UsgsGroundWaterLevelSeries";
        }

        public UsgsGroundWaterLevelSeries( TimeSeriesDatabase db, Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow sr)
            : base(db,sr)
        {
            m_site_no = ConnectionStringUtility.GetToken(ConnectionString, "site_no", "");
        }

        //public UsgsGroundWaterLevelSeries(int sdi, TimeSeriesDatabase db)
        //    : base(sdi, db)
        //{
        //    m_site_no = ConnectionStringUtility.GetToken(ConnectionString, "site_no", "");
        //}

        public static UsgsGroundWaterLevelSeries Read(string site_no, DateTime t1, DateTime t2)
        {
            UsgsGroundWaterLevelSeries s = new UsgsGroundWaterLevelSeries(site_no);
            s.Read(t1, t2);
            return s;
        }

        protected override Series CreateFromConnectionString()
        {
            UsgsGroundWaterLevelSeries s = new UsgsGroundWaterLevelSeries(
                ConnectionStringUtility.GetToken(ConnectionString, "site_no", ""));
            return s;
        }

        /// <summary>
        /// Read Published daily stream flow values.  
        /// gets full period of record.
        /// </summary>
        protected override void ReadCore()
        {
            if (m_db != null)
            {
                base.ReadCore();
            }
            else
            {
                // GW Web Services URL Pattern
                //http://waterservices.usgs.gov/nwis/gwlevels/?format=rdb&sites=444401116463001&startDT=1974-05-16&endDT=2009-05-11
                // OLD URL
                //string url = "http://nwis.waterdata.usgs.gov/nwis/gwlevels?site_no=444401116463001&agency_cd=USGS&format=rdb";
                string url = "http://waterservices.usgs.gov/nwis/gwlevels/?format=rdb&sites=444401116463001";
                url = url.Replace("site_no=444401116463001", "site_no=" + m_site_no);
                Messages.Add(url);
                ReadSeriesData(url,TimeSeriesDatabase.MinDateTime,TimeSeriesDatabase.MaxDateTime);
            }
        }

        /// <summary>
        /// Read Published daily stream flow values.  
        /// gets data between dates specified.
        /// </summary>
        /// <param name="t1">beginning DateTime</param>
        /// <param name="t2">ending DateTime</param>
        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            if (m_db != null)
            {
                if (Utility.AutoUpdate)
                {
                    if (t2 >= DateTime.Now.Date)
                    { // don't waste time looking to the future
                        t2 = DateTime.Now.AddDays(-1).Date;
                    }
                    base.UpdateCore(t1, t2, true);
                }
                base.ReadCore(t1, t2);
            }
            else
            {
                string url = "http://waterservices.usgs.gov/nwis/gwlevels/?format=rdb&sites=444401116463001";
                url = url.Replace("site_no=444401116463001", "site_no=" + m_site_no);
                Messages.Add(url);
                ReadSeriesData(url,t1,t2);
            }
        }

        private void ReadSeriesData(string url,DateTime t1, DateTime t2)
        {
            url += "&startDT=" + t1.ToString("yyyy-MM-dd") + "&endDT=" + t2.ToString("yyyy-MM-dd");
            string[] response = Web.GetPage(url, true);

            m_rdb = new UsgsRDBFile(response,true);
            ParsePreamble();
            m_columnName = "lev_va";

            for (int i = 0; i < m_rdb.Rows.Count; i++)
            {
                DataRow row = m_rdb.Rows[i];
                DateTime t = DateTime.MinValue;

                if (!DateTime.TryParse(row["lev_dt"].ToString(), out t))
                {
                    break;
                }
                // Read Time component
                string s = row["lev_tm"].ToString().Trim();
                if (s.Length == 4 && Regex.IsMatch(s, "[0-9]{4}"))
                {
                    int hr = Convert.ToInt32(s.Substring(0, 2));
                    int min = Convert.ToInt32(s.Substring(2));
                    t = t.AddHours(hr);
                    t = t.AddMinutes(min);
                }

                if (t < t1 || t > t2)
                    continue;

                double d = Point.MissingValueFlag;
                Point pt = Point.Missing;
                pt.DateTime = t;
                pt.Flag = row["lev_status_cd"].ToString();
                if (!Double.TryParse(row[m_columnName].ToString(), out  d))
                {
                    Messages.Add("Error reading '" + row[m_columnName] + "' as a number");
                    AddMissing(t);
                }
                else
                {
                    pt.Value = d;
                    Add(pt);
                }
            }
        }

        /*
# US Geological Survey ground-water levels
# retrieved: 2010-02-23 11:45:05 EST
# URL: http://waterdata.usgs.gov/nwis/gwlevels
#
# The fields in this file include:
# ---------------------------------
# agency_cd     Agency Code
# site_no       USGS site number
# lev_dt        date level measured
# lev_tm        time level measured
# lev_va        level value in feet below land surface
# sl_lev_va     level value referenced to mean sea level
# lev_status_cd level status code
#  The 'lev_status_cd' field is defined at
#   http://waterdata.usgs.gov/nwis/gwlevels/?help
#
# Sites in this file include:
#  USGS 444401116463001 14N 02W 10BCA1
#
#
agency_cd	site_no	lev_dt	lev_tm	lev_va	sl_lev_va	lev_status_cd
5s	15s	10d	4s	12s	12s	1s
USGS	444401116463001	1974-05-16		4.87		
USGS	444401116463001	1974-06-12		4.61		
USGS	444401116463001	1974-07-09		4.81		
USGS	444401116463001	1974-08-07		4.80		
USGS	444401116463001	1974-09-10		4.82		
USGS	444401116463001	1974-10-08		4.54		
USGS	444401116463001	1974-11-11		4.68		
USGS	444401116463001	1974-12-04		4.52		
USGS	444401116463001	1975-01-07		4.40		
USGS	444401116463001	1975-02-06		4.24		
USGS	444401116463001	1975-03-05		4.00		
USGS	444401116463001	1975-04-09		4.31		
USGS	444401116463001	1975-05-08		4.39		
USGS	444401116463001	1975-06-12		4.58		
USGS	444401116463001	1975-07-08		4.71		
USGS	444401116463001	1975-08-05		4.49		
USGS	444401116463001	1975-09-08		4.78		
USGS	444401116463001	1975-09-23		4.69		
USGS	444401116463001	1975-10-08		4.42		
USGS	444401116463001	1975-11-13		4.59		
USGS	444401116463001	1975-12-03		4.42		
USGS	444401116463001	1976-02-05		4.42		
USGS	444401116463001	1976-03-17		4.03		
USGS	444401116463001	1976-05-01		4.51		
USGS	444401116463001	1976-06-11		4.59		
USGS	444401116463001	1976-07-16		5.12		
USGS	444401116463001	1976-09-01		4.71		
USGS	444401116463001	1976-12-08		4.45		
USGS	444401116463001	1977-01-12		3.42		
USGS	444401116463001	1977-03-10		4.43		
USGS	444401116463001	1977-05-12		4.54		
USGS	444401116463001	1977-06-23		4.88		
USGS	444401116463001	1977-08-04		5.40		
USGS	444401116463001	1977-09-15		5.05		
USGS	444401116463001	1977-10-30		4.78		
USGS	444401116463001	1977-12-15		3.95		
USGS	444401116463001	1978-02-01		4.39		
USGS	444401116463001	1978-03-22		4.48		
USGS	444401116463001	1978-04-25		4.49		
USGS	444401116463001	1978-06-08		4.77		
USGS	444401116463001	1978-07-18		4.57		
USGS	444401116463001	1978-09-12		4.78		
USGS	444401116463001	1978-10-25		4.77		
USGS	444401116463001	1978-12-07		4.77		
USGS	444401116463001	1979-01-23		4.60		
USGS	444401116463001	1979-03-06		4.07		
USGS	444401116463001	1979-04-10		4.50		
USGS	444401116463001	1979-05-16		4.83		
USGS	444401116463001	1979-06-20		4.84		
USGS	444401116463001	1979-08-16		5.02		
USGS	444401116463001	1979-09-20		5.17		
USGS	444401116463001	1979-11-07		4.93		
USGS	444401116463001	1979-12-11		4.67		
USGS	444401116463001	1980-01-22		4.65		
USGS	444401116463001	1980-03-26		4.63		
USGS	444401116463001	1980-05-21		5.29		
USGS	444401116463001	1980-07-16		5.89		
USGS	444401116463001	1980-09-18		4.90		
USGS	444401116463001	1980-11-04		4.89		
USGS	444401116463001	1980-12-09		4.65		
USGS	444401116463001	1981-01-29		4.25		
USGS	444401116463001	1981-03-12		4.60		
USGS	444401116463001	1981-05-28		5.35		
USGS	444401116463001	1981-07-07		5.37		
USGS	444401116463001	1981-09-10		5.32		
USGS	444401116463001	1981-11-03		4.82		
USGS	444401116463001	1982-01-07		4.67		
USGS	444401116463001	1982-03-09		4.14		
USGS	444401116463001	1982-05-06		5.77		
USGS	444401116463001	1982-07-14		5.49		
USGS	444401116463001	1982-09-16		4.95		
USGS	444401116463001	1982-11-09		4.61		
USGS	444401116463001	1983-01-12		4.20		
USGS	444401116463001	1983-03-08		4.11		
USGS	444401116463001	1983-05-03		4.54		
USGS	444401116463001	1983-07-21		4.80		
USGS	444401116463001	1983-09-16		4.81		
USGS	444401116463001	1983-11-17		3.82		
USGS	444401116463001	1984-01-11		3.88		
USGS	444401116463001	1984-03-14		3.32		
USGS	444401116463001	1984-05-04		3.94		
USGS	444401116463001	1984-07-11		4.65		
USGS	444401116463001	1984-09-20		4.73		
USGS	444401116463001	1984-11-08		4.30		
USGS	444401116463001	1985-01-23		4.14		
USGS	444401116463001	1985-03-13		4.03		
USGS	444401116463001	1985-05-09		4.45		
USGS	444401116463001	1985-07-19		5.80		
USGS	444401116463001	1985-09-24		4.49		
USGS	444401116463001	1986-03-04		3.97		
USGS	444401116463001	1986-07-18		4.48		
USGS	444401116463001	1986-10-03		4.42		
USGS	444401116463001	1986-11-06		4.36		
USGS	444401116463001	1987-01-27		4.12		
USGS	444401116463001	1987-03-20		4.05		
USGS	444401116463001	1987-05-14	1445	4.46		
USGS	444401116463001	1987-07-21	0700	4.53		
USGS	444401116463001	1987-09-01	0707	4.68		
USGS	444401116463001	1987-11-17	0718	4.43		
USGS	444401116463001	1988-01-27		4.27		
USGS	444401116463001	1988-03-08		4.04		
USGS	444401116463001	1988-05-17	0710	4.37		
USGS	444401116463001	1988-07-26	0900	5.04		
USGS	444401116463001	1988-10-04	0850	5.02		
USGS	444401116463001	1988-11-15	1000	4.53		
USGS	444401116463001	1989-01-24	0930	4.33		
USGS	444401116463001	1989-03-08		4.04		
USGS	444401116463001	1989-05-17		4.54		
USGS	444401116463001	1989-07-26		4.85		
USGS	444401116463001	1989-10-05		4.63		
USGS	444401116463001	1989-11-08		4.19		
USGS	444401116463001	1990-01-24		4.52		
USGS	444401116463001	1990-03-13		4.32		
USGS	444401116463001	1990-05-22		4.66		
USGS	444401116463001	1990-07-11		5.00		
USGS	444401116463001	1990-10-05		5.01		
USGS	444401116463001	1990-11-29		5.05		
USGS	444401116463001	1991-01-09		5.06		
USGS	444401116463001	1991-02-20		4.57		
USGS	444401116463001	1991-05-15		4.71		
USGS	444401116463001	1991-08-01		5.09		
USGS	444401116463001	1991-09-12		5.02		
USGS	444401116463001	1991-10-30		5.35		
USGS	444401116463001	1992-02-07		4.50		
USGS	444401116463001	1992-03-26		4.69		
USGS	444401116463001	1992-05-15		4.83		
USGS	444401116463001	1992-08-07		5.03		
USGS	444401116463001	1992-09-17		6.32		
USGS	444401116463001	1992-11-17		5.13		
USGS	444401116463001	1993-01-26		4.28		
USGS	444401116463001	1993-03-29		4.10		
USGS	444401116463001	1993-05-24		4.95		
USGS	444401116463001	1993-07-29		4.90		
USGS	444401116463001	1993-09-21		5.08		
USGS	444401116463001	1993-11-29		4.92		
USGS	444401116463001	1994-01-26		4.46		
USGS	444401116463001	1994-03-29		4.70		
USGS	444401116463001	1994-05-25		4.75		
USGS	444401116463001	1994-07-26		5.69		
USGS	444401116463001	1994-09-21		7.80		
USGS	444401116463001	1994-12-06		4.75		
USGS	444401116463001	1995-02-06		4.26		
USGS	444401116463001	1995-03-31		4.56		
USGS	444401116463001	1995-06-06		5.02		
USGS	444401116463001	1995-07-23		5.51		
USGS	444401116463001	1995-07-26		5.67		
USGS	444401116463001	1995-09-27		4.93		
USGS	444401116463001	1995-11-24		4.76		
USGS	444401116463001	1996-01-18		4.31		
USGS	444401116463001	1996-03-13		4.45		
USGS	444401116463001	1996-05-29		4.75		
USGS	444401116463001	1996-07-23		5.51		
USGS	444401116463001	1996-09-18		5.00		
USGS	444401116463001	1996-10-29		4.90		
USGS	444401116463001	1997-01-29		4.60		
USGS	444401116463001	1997-03-12		4.26		
USGS	444401116463001	1997-05-21		5.01		
USGS	444401116463001	1997-07-09		4.93		
USGS	444401116463001	1997-09-16		5.09		
USGS	444401116463001	1997-11-20		4.61		
USGS	444401116463001	1998-01-09		4.49		
USGS	444401116463001	1998-03-05		4.06		
USGS	444401116463001	1998-05-20		4.34		
USGS	444401116463001	1998-07-07		5.01		
USGS	444401116463001	1998-09-22		4.89		
USGS	444401116463001	1998-11-19		4.58		
USGS	444401116463001	1999-01-07		4.40		
USGS	444401116463001	1999-03-24		3.89		
USGS	444401116463001	1999-05-21		4.25		
USGS	444401116463001	1999-07-21		4.78		
USGS	444401116463001	1999-09-30		4.71		
USGS	444401116463001	1999-11-23		4.58		
USGS	444401116463001	2000-01-24		3.95		
USGS	444401116463001	2000-03-02		3.85		
USGS	444401116463001	2000-05-25		4.69		
USGS	444401116463001	2000-07-25		5.22		
USGS	444401116463001	2000-09-24		5.10		
USGS	444401116463001	2000-11-22		4.39		
USGS	444401116463001	2001-01-25		4.23		
USGS	444401116463001	2001-03-16		3.95		
USGS	444401116463001	2001-05-25		4.69		
USGS	444401116463001	2001-07-30		4.95		
USGS	444401116463001	2001-09-24		6.54		
USGS	444401116463001	2002-01-29		4.14		
USGS	444401116463001	2002-03-13		3.55		
USGS	444401116463001	2002-05-15		4.28		
USGS	444401116463001	2002-07-31		4.85		
USGS	444401116463001	2002-09-24		4.73		
USGS	444401116463001	2002-11-27		4.60		
USGS	444401116463001	2003-01-23		3.63		
USGS	444401116463001	2003-03-27	1004	3.71		
USGS	444401116463001	2003-05-21	1122	4.18		
USGS	444401116463001	2003-07-24	0910	6		
USGS	444401116463001	2003-09-24	1003	6		
USGS	444401116463001	2003-11-19	1032	4.39		
USGS	444401116463001	2004-01-27	1218	4.20		
USGS	444401116463001	2004-03-26	1105	4.16		
USGS	444401116463001	2004-05-21	0831	4.42		
USGS	444401116463001	2004-07-29	1257	6.42		
USGS	444401116463001	2004-09-23	0828	5.23		
USGS	444401116463001	2004-11-02	1436	4.80		
USGS	444401116463001	2005-01-05	1240	4.60		
USGS	444401116463001	2005-03-16	0820	4.61		
USGS	444401116463001	2005-05-25	1137	4.76		
USGS	444401116463001	2005-07-14	0857	6.16		
USGS	444401116463001	2005-09-29	1633	5.84		
USGS	444401116463001	2005-11-21	1318	4.73		
USGS	444401116463001	2006-01-25	1221	4.34		
USGS	444401116463001	2006-03-20	1240	4.01		
USGS	444401116463001	2006-05-23	1313	4.71		
USGS	444401116463001	2006-07-25	1259	6.71		
USGS	444401116463001	2006-09-26	1218	5.66		
USGS	444401116463001	2006-11-29	1136	5.29		
USGS	444401116463001	2007-01-16	1210	4.83		
USGS	444401116463001	2007-03-21	1201	4.63		
USGS	444401116463001	2007-05-22	1209	4.79		
USGS	444401116463001	2007-07-30	1311	7.53		
USGS	444401116463001	2007-09-25	1212	7.68		
USGS	444401116463001	2007-11-08	1406	5.30		
USGS	444401116463001	2008-01-31	1132	4.57		
USGS	444401116463001	2008-03-20	1351	4.32		
USGS	444401116463001	2008-05-28	1729	4.78		
USGS	444401116463001	2008-07-24	1545	6.16		
USGS	444401116463001	2008-09-11	1326	6.07		
USGS	444401116463001	2008-11-20	1026	5.04		
USGS	444401116463001	2009-01-09	1158	4.90		
USGS	444401116463001	2009-02-26	1340	4.38		
USGS	444401116463001	2009-05-11	1329	4.90		
       
         
         */

        /// <summary>
        /// Determine column name where this data exists.
        /// </summary>
        private void ParsePreamble()
        {
            int idx = -1;
            string findMe = "#    USGS " + SiteID;
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


        }





    }
}
