using System;
using System.Data;
using Reclamation.Core;

namespace Reclamation.TimeSeries.Usgs
{
	/// <summary>
	/// USGS Daily Data from the web 
	/// </summary>
	public class UsgsDailyValueSeries: Series
	{

        
        UsgsRDBFile m_rdb;
        UsgsDailyParameter m_parameter;
        string m_columnName;
        string m_site_no;
        string m_flagColumnName;
        public UsgsDailyValueSeries(string site_no, UsgsDailyParameter parameter)
		{
            site_no = site_no.Trim();
            if (site_no.Length == 7)
                site_no = "0"+site_no;

			this.SiteName = site_no;
            m_site_no = site_no;
            m_parameter = parameter;
            m_columnName = "";
            m_flagColumnName = "";
            Table.TableName = "USGSDaily" + parameter.ToString() + "_" + site_no;
            ConnectionString = "Source=USGS;site_no=" + site_no+";UsgsDailyParameter="+parameter.ToString();
            TimeInterval = TimeInterval.Daily;
            Source = "USGS";
            Provider = "UsgsDailyValueSeries";
		}

        public UsgsDailyValueSeries(TimeSeriesDatabase db,TimeSeriesDatabaseDataSet.SeriesCatalogRow sr):base(db,sr)
        {
            m_parameter = UsgsDailyParameter.DailyMeanDischarge;
            string str = ConnectionStringUtility.GetToken(ConnectionString, "UsgsDailyParameter","");
            if( Enum.IsDefined(typeof(UsgsDailyParameter),str))
                m_parameter = (UsgsDailyParameter)Enum.Parse(typeof(UsgsDailyParameter), str);
            m_site_no = ConnectionStringUtility.GetToken(ConnectionString, "site_no","");
        }

         public static UsgsDailyValueSeries Read(string site_no, UsgsDailyParameter parameter,DateTime t1,DateTime t2)
        {
            UsgsDailyValueSeries s = new UsgsDailyValueSeries(site_no, parameter);
            s.Read(t1,t2);
            return s;
        }
       
        protected override Series CreateFromConnectionString()
        {
            UsgsDailyValueSeries s = new UsgsDailyValueSeries(
               ConnectionStringUtility.GetToken(ConnectionString, "site_no",""),
                m_parameter);
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
                //string url = "http://waterdata.usgs.gov/nwis/dv?site_no=13154500&begin_date=&end_date=&format=rdb&date_format=YYYY-MM-DD";

                string url = "http://waterservices.usgs.gov/nwis/dv/?format=rdb&sites=13236500&startDT=1900-10-01&endDT=2013-12-30";
                url = url.Replace("sites=13236500", "sites=" + SiteName);
                DateTime n = DateTime.Now;
                url = url.Replace("endDT=2013-12-30", "endDT=" + n.Year + "-" + n.Month + "-" + n.Day);
                Messages.Add(url);
                ReadSeriesData(url);
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
                //string url = "http://nwis.waterdata.usgs.gov/nwis/dv?site_no=13154500&begin_date=&end_date=&format=rdb&date_format=YYYY-MM-DD";
                string url = "http://waterservices.usgs.gov/nwis/dv/?format=rdb&sites=13236500&startDT=1900-10-01&endDT=2013-12-30";

                url = url.Replace("sites=13236500", "sites=" + SiteName);
                DateTime n = DateTime.Now;
                url = url.Replace("endDT=2013-12-30", "endDT=" + n.Year + "-" + n.Month + "-" + n.Day);
                Messages.Add(url);

                url = url.Replace("startDT=1900-10-01", "startDT=" + t1.ToString("yyyy-MM-dd") );
                url = url.Replace("endDT=2013-12-30", "endDT=" + t2.ToString("yyyy-MM-dd"));
                
                Messages.Add(url);
                ReadSeriesData(url);
            }
		}

        private void ReadSeriesData(string url)
        {
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
                    AddMissing(t);
                }
                else
                {
                    pt.Value = d;
                    Add(pt);
                }
            }
        }

        /* Example daily value data
# ---------------------------------- WARNING ----------------------------------------
# The data you have obtained from this automated U.S. Geological Survey database
# have not received Director's approval and as such are provisional and subject to
# revision.  The data are released on the condition that neither the USGS nor the
# United States Government may be held liable for any damages resulting from its use.
# Additional info: http://waterdata.usgs.gov/nwis/help/?provisional
#
# File-format description:  http://waterdata.usgs.gov/nwis/?tab_delimited_format_info
# Automated-retrieval info: http://waterdata.usgs.gov/nwis/?automated_retrieval_info
#
# Contact:   gs-w_support_nwisweb@usgs.gov
# retrieved: 2007-03-09 11:51:55 EST
#
# Data for the following site(s) are contained in this file
#    USGS 13010065 SNAKE RIVER AB JACKSON LAKE AT FLAGG RANCH WY
# -----------------------------------------------------------------------------------
#
# Data provided for site 13010065
#    DD parameter statistic   Description
#    02   00060     00003     Discharge, cubic feet per second (Mean)
#    03   00010     00001     Temperature, water, degrees Celsius (Maximum)
#    03   00010     00002     Temperature, water, degrees Celsius (Minimum)
#    03   00010     00003     Temperature, water, degrees Celsius (Mean)
#    04   00095     00001     Specific conductance, water, unfiltered, microsiemens per centimeter at 25 degrees Celsius (Maximum)
#    04   00095     00002     Specific conductance, water, unfiltered, microsiemens per centimeter at 25 degrees Celsius (Minimum)
#
# Data-value qualification codes included in this output: 
#     Ice  Ice affected  
#     A  Approved for publication -- Processing and review completed.  
#     P  Provisional data subject to revision.  
#     e  Value has been estimated.  
# 
agency_cd	site_no	datetime	02_00060_00003	02_00060_00003_cd	03_00010_00001	03_00010_00001_cd	03_00010_00002	03_00010_00002_cd	03_00010_00003	03_00010_00003_cd	04_00095_00001	04_00095_00001_cd	04_00095_00002	04_00095_00002_cd
5s	15s	16s	14s	14s	14s	14s	14s	14s	14s	14s	14s	14s	14s	14s
USGS	13010065	2006-03-09	361	A										
USGS	13010065	2006-03-10	351	A										
USGS	13010065	2006-03-11	340	Ae										
USGS	13010065	2006-03-12	344	A										
USGS	13010065	2006-03-13	348	A										
USGS	13010065	2006-03-14	344	A										
USGS	13010065	2006-03-15	353	A										
USGS	13010065	2006-03-16	353	A										
USGS	13010065	2006-03-17	356	A										
USGS	13010065	2006-03-18	355	A										
USGS	13010065	2006-03-19	355	A										
USGS	13010065	2006-03-20	346	A										
USGS	13010065	2006-03-21	338	A										
USGS	13010065	2006-03-22	340	A										
USGS	13010065	2006-03-23	334	A										
USGS	13010065	2006-03-24	328	A										
USGS	13010065	2006-03-25	331	A										
USGS	13010065	2006-03-26	336	A										
USGS	13010065	2006-03-27	329	A										
USGS	13010065	2006-03-28	323	A										
USGS	13010065	2006-03-29	332	A										
USGS	13010065	2006-03-30	330	A										
USGS	13010065	2006-03-31	323	A										
USGS	13010065	2006-04-01	337	A										
USGS	13010065	2006-04-02	336	A										
USGS	13010065	2006-04-03	338	A										
USGS	13010065	2006-04-04	363	A										
USGS	13010065	2006-04-05	404	A										
USGS	13010065	2006-04-06	462	A										
USGS	13010065	2006-04-07	473	A										
USGS	13010065	2006-04-08	491	A										
USGS	13010065	2006-04-09	539	A										
USGS	13010065	2006-04-10	547	A										
USGS	13010065	2006-04-11	517	A										
USGS	13010065	2006-04-12	515	A										
USGS	13010065	2006-04-13	618	A										
USGS	13010065	2006-04-14	804	A										
USGS	13010065	2006-04-15	932	A										
USGS	13010065	2006-04-16	1110	A										
USGS	13010065	2006-04-17	1210	A										
USGS	13010065	2006-04-18	969	A										
USGS	13010065	2006-04-19	839	A										
USGS	13010065	2006-04-20	782	A										
USGS	13010065	2006-04-21	825	A										
USGS	13010065	2006-04-22	976	A										
USGS	13010065	2006-04-23	1450	A										
USGS	13010065	2006-04-24	1700	A										
USGS	13010065	2006-04-25	1490	A										
USGS	13010065	2006-04-26	1580	A										
USGS	13010065	2006-04-27	1900	A										
USGS	13010065	2006-04-28	2050	A										
USGS	13010065	2006-04-29	2370	A										
USGS	13010065	2006-04-30	2830	A										
USGS	13010065	2006-05-01	2980	A										
USGS	13010065	2006-05-02	3090	A										
USGS	13010065	2006-05-03	2760	A										
USGS	13010065	2006-05-04	2790	A										
USGS	13010065	2006-05-05	2710	A										
USGS	13010065	2006-05-06	3230	A										
USGS	13010065	2006-05-07	3490	A										
USGS	13010065	2006-05-08	3570	A										
USGS	13010065	2006-05-09	2970	A										
USGS	13010065	2006-05-10	2480	A										
USGS	13010065	2006-05-11	2430	A										
USGS	13010065	2006-05-12	2920	A										
USGS	13010065	2006-05-13	3940	A										
USGS	13010065	2006-05-14	4960	A										
USGS	13010065	2006-05-15	5480	A										
USGS	13010065	2006-05-16	5840	A										
USGS	13010065	2006-05-17	6510	A										
USGS	13010065	2006-05-18	7190	A										
USGS	13010065	2006-05-19	7750	A										
USGS	13010065	2006-05-20	7310	A										
USGS	13010065	2006-05-21	6740	A										
USGS	13010065	2006-05-22	7700	A										
USGS	13010065	2006-05-23	6740	A										
USGS	13010065	2006-05-24	5460	A										
USGS	13010065	2006-05-25	5740	A										
USGS	13010065	2006-05-26	5930	A										
USGS	13010065	2006-05-27	5210	A										
USGS	13010065	2006-05-28	4180	A										
USGS	13010065	2006-05-29	3530	A										
USGS	13010065	2006-05-30	3070	A										
USGS	13010065	2006-05-31	2870	A										
USGS	13010065	2006-06-01	2940	A										
USGS	13010065	2006-06-02	3440	A										
USGS	13010065	2006-06-03	4350	A										
USGS	13010065	2006-06-04	4580	A										
USGS	13010065	2006-06-05	4650	A										
USGS	13010065	2006-06-06	4460	A										
USGS	13010065	2006-06-07	4590	A										
USGS	13010065	2006-06-08	4740	A										
USGS	13010065	2006-06-09	4130	A										
USGS	13010065	2006-06-10	3960	A										
USGS	13010065	2006-06-11	3460	A										
USGS	13010065	2006-06-12	3300	A										
USGS	13010065	2006-06-13	3260	A										
USGS	13010065	2006-06-14	3120	A										
USGS	13010065	2006-06-15	2820	A										
USGS	13010065	2006-06-16	2480	A										
USGS	13010065	2006-06-17	2310	A										
USGS	13010065	2006-06-18	2190	A										
USGS	13010065	2006-06-19	2130	A										
USGS	13010065	2006-06-20	2040	A										
USGS	13010065	2006-06-21	1890	A										
USGS	13010065	2006-06-22	1740	A										
USGS	13010065	2006-06-23	1650	A										
USGS	13010065	2006-06-24	1560	A										
USGS	13010065	2006-06-25	1490	A										
USGS	13010065	2006-06-26	1400	A										
USGS	13010065	2006-06-27	1330	A										
USGS	13010065	2006-06-28	1290	A										
USGS	13010065	2006-06-29	1240	A										
USGS	13010065	2006-06-30	1160	A										
USGS	13010065	2006-07-01	1110	A										
USGS	13010065	2006-07-02	1050	A										
USGS	13010065	2006-07-03	1000	A										
USGS	13010065	2006-07-04	947	A										
USGS	13010065	2006-07-05	908	A										
USGS	13010065	2006-07-06	897	A										
USGS	13010065	2006-07-07	857	A										
USGS	13010065	2006-07-08	803	A										
USGS	13010065	2006-07-09	769	A										
USGS	13010065	2006-07-10	755	A										
USGS	13010065	2006-07-11	719	A										
USGS	13010065	2006-07-12	695	A										
USGS	13010065	2006-07-13	691	A										
USGS	13010065	2006-07-14	647	A										
USGS	13010065	2006-07-15	619	A										
USGS	13010065	2006-07-16	603	A										
USGS	13010065	2006-07-17	583	A										
USGS	13010065	2006-07-18	554	A										
USGS	13010065	2006-07-19	533	A										
USGS	13010065	2006-07-20	522	A										
USGS	13010065	2006-07-21	513	A										
USGS	13010065	2006-07-22	499	A										
USGS	13010065	2006-07-23	491	A										
USGS	13010065	2006-07-24	487	A										
USGS	13010065	2006-07-25	490	A										
USGS	13010065	2006-07-26	482	A										
USGS	13010065	2006-07-27	480	A										
USGS	13010065	2006-07-28	457	A										
USGS	13010065	2006-07-29	444	A										
USGS	13010065	2006-07-30	432	A										
USGS	13010065	2006-07-31	421	A										
USGS	13010065	2006-08-01	439	A										
USGS	13010065	2006-08-02	442	A										
USGS	13010065	2006-08-03	417	A										
USGS	13010065	2006-08-04	415	A										
USGS	13010065	2006-08-05	425	A										
USGS	13010065	2006-08-06	404	A										
USGS	13010065	2006-08-07	396	A										
USGS	13010065	2006-08-08	391	A										
USGS	13010065	2006-08-09	382	A										
USGS	13010065	2006-08-10	371	A										
USGS	13010065	2006-08-11	361	A										
USGS	13010065	2006-08-12	352	A										
USGS	13010065	2006-08-13	347	A										
USGS	13010065	2006-08-14	341	A										
USGS	13010065	2006-08-15	340	A										
USGS	13010065	2006-08-16	346	A										
USGS	13010065	2006-08-17	340	A										
USGS	13010065	2006-08-18	330	A										
USGS	13010065	2006-08-19	325	A										
USGS	13010065	2006-08-20	320	A										
USGS	13010065	2006-08-21	316	A										
USGS	13010065	2006-08-22	312	A										
USGS	13010065	2006-08-23	306	A										
USGS	13010065	2006-08-24	301	A										
USGS	13010065	2006-08-25	300	A										
USGS	13010065	2006-08-26	310	A										
USGS	13010065	2006-08-27	327	A										
USGS	13010065	2006-08-28	357	A										
USGS	13010065	2006-08-29	326	A										
USGS	13010065	2006-08-30	313	A										
USGS	13010065	2006-08-31	302	A										
USGS	13010065	2006-09-01	300	A										
USGS	13010065	2006-09-02	298	A										
USGS	13010065	2006-09-03	296	A										
USGS	13010065	2006-09-04	292	A										
USGS	13010065	2006-09-05	292	A										
USGS	13010065	2006-09-06	295	A										
USGS	13010065	2006-09-07	293	A										
USGS	13010065	2006-09-08	294	A										
USGS	13010065	2006-09-09	296	A										
USGS	13010065	2006-09-10	314	A										
USGS	13010065	2006-09-11	305	A										
USGS	13010065	2006-09-12	298	A										
USGS	13010065	2006-09-13	292	A										
USGS	13010065	2006-09-14	290	A										
USGS	13010065	2006-09-15	317	A										
USGS	13010065	2006-09-16	342	A										
USGS	13010065	2006-09-17	318	A										
USGS	13010065	2006-09-18	305	A										
USGS	13010065	2006-09-19	301	A										
USGS	13010065	2006-09-20	300	A										
USGS	13010065	2006-09-21	328	A										
USGS	13010065	2006-09-22	349	A										
USGS	13010065	2006-09-23	328	A										
USGS	13010065	2006-09-24	316	A										
USGS	13010065	2006-09-25	310	A										
USGS	13010065	2006-09-26	305	A										
USGS	13010065	2006-09-27	302	A										
USGS	13010065	2006-09-28	297	A										
USGS	13010065	2006-09-29	293	A										
USGS	13010065	2006-09-30	292	A										
USGS	13010065	2006-10-01	288	P										
USGS	13010065	2006-10-02	296	P										
USGS	13010065	2006-10-03	389	P										
USGS	13010065	2006-10-04	354	P										
USGS	13010065	2006-10-05	338	P										
USGS	13010065	2006-10-06	334	P										
USGS	13010065	2006-10-07	423	P										
USGS	13010065	2006-10-08	423	P										
USGS	13010065	2006-10-09	385	P										
USGS	13010065	2006-10-10	376	P										
USGS	13010065	2006-10-11	365	P										
USGS	13010065	2006-10-12	357	P										
USGS	13010065	2006-10-13	348	P										
USGS	13010065	2006-10-14	341	P										
USGS	13010065	2006-10-15	338	P										
USGS	13010065	2006-10-16	427	P										
USGS	13010065	2006-10-17	415	P										
USGS	13010065	2006-10-18	373	P										
USGS	13010065	2006-10-19	366	P										
USGS	13010065	2006-10-20	393	P										
USGS	13010065	2006-10-21	367	P										
USGS	13010065	2006-10-22	347	P										
USGS	13010065	2006-10-23	340	P										
USGS	13010065	2006-10-24	346	P										
USGS	13010065	2006-10-25	360	P										
USGS	13010065	2006-10-26	323	P										
USGS	13010065	2006-10-27	333	P										
USGS	13010065	2006-10-28	323	P										
USGS	13010065	2006-10-29	318	P										
USGS	13010065	2006-10-30	320	P										
USGS	13010065	2006-10-31	302	P										
USGS	13010065	2006-11-01	269	P										
USGS	13010065	2006-11-02	277	P										
USGS	13010065	2006-11-03	316	P										
USGS	13010065	2006-11-04	317	P										
USGS	13010065	2006-11-05	319	P										
USGS	13010065	2006-11-06	375	P										
USGS	13010065	2006-11-07	454	P										
USGS	13010065	2006-11-08	655	P										
USGS	13010065	2006-11-09	650	P										
USGS	13010065	2006-11-10	475	P										
USGS	13010065	2006-11-11	434	P										
USGS	13010065	2006-11-12	425	P										
USGS	13010065	2006-11-13	401	P										
USGS	13010065	2006-11-14	438	P										
USGS	13010065	2006-11-15	406	P										
USGS	13010065	2006-11-16	420	P										
USGS	13010065	2006-11-17	419	P										
USGS	13010065	2006-11-18	415	P										
USGS	13010065	2006-11-19	382	P										
USGS	13010065	2006-11-20	391	P										
USGS	13010065	2006-11-21	390	P										
USGS	13010065	2006-11-22	377	P										
USGS	13010065	2006-11-23	377	P										
USGS	13010065	2006-11-24	348	P										
USGS	13010065	2006-11-25	349	P										
USGS	13010065	2006-11-26	359	P										
USGS	13010065	2006-11-27	354	P										
USGS	13010065	2006-11-28	348	P										
USGS	13010065	2006-11-29	324	P										
USGS	13010065	2006-11-30	328	P										
USGS	13010065	2006-12-01	344	P										
USGS	13010065	2006-12-02	335	P										
USGS	13010065	2006-12-03	335	P										
USGS	13010065	2006-12-04	332	P										
USGS	13010065	2006-12-05	333	P										
USGS	13010065	2006-12-06	326	P										
USGS	13010065	2006-12-07	332	P										
USGS	13010065	2006-12-08	318	P										
USGS	13010065	2006-12-09	314	P										
USGS	13010065	2006-12-10	316	P										
USGS	13010065	2006-12-11	324	P										
USGS	13010065	2006-12-12	321	P										
USGS	13010065	2006-12-13	319	P										
USGS	13010065	2006-12-14	371	P										
USGS	13010065	2006-12-15	361	P										
USGS	13010065	2006-12-16	365	P										
USGS	13010065	2006-12-17	397	P										
USGS	13010065	2006-12-18	375	P										
USGS	13010065	2006-12-19	407	P										
USGS	13010065	2006-12-20	375	P										
USGS	13010065	2006-12-21	371	P										
USGS	13010065	2006-12-22	376	P										
USGS	13010065	2006-12-23	369	P										
USGS	13010065	2006-12-24	367	P										
USGS	13010065	2006-12-25	361	P										
USGS	13010065	2006-12-26	372	P										
USGS	13010065	2006-12-27	386	P										
USGS	13010065	2006-12-28	391	P										
USGS	13010065	2006-12-29	370	P										
USGS	13010065	2006-12-30	360	P										
USGS	13010065	2006-12-31	368	P										
USGS	13010065	2007-01-01	360	P										
USGS	13010065	2007-01-02	360	P										
USGS	13010065	2007-01-03	356	P										
USGS	13010065	2007-01-04	364	P										
USGS	13010065	2007-01-05	372	P										
USGS	13010065	2007-01-06	360	P										
USGS	13010065	2007-01-07	380	P										
USGS	13010065	2007-01-08	375	P										
USGS	13010065	2007-01-09	378	P										
USGS	13010065	2007-01-10	379	P										
USGS	13010065	2007-01-11	Ice	P										
USGS	13010065	2007-01-12	Ice	P										
USGS	13010065	2007-01-13	Ice	P										
USGS	13010065	2007-01-14	Ice	P										
USGS	13010065	2007-01-15	Ice	P										
USGS	13010065	2007-01-16	Ice	P										
USGS	13010065	2007-01-17	Ice	P										
USGS	13010065	2007-01-18	Ice	P										
USGS	13010065	2007-01-19	Ice	P										
USGS	13010065	2007-01-20	Ice	P										
USGS	13010065	2007-01-21	Ice	P										
USGS	13010065	2007-01-22	Ice	P										
USGS	13010065	2007-01-23	Ice	P										
USGS	13010065	2007-01-24	Ice	P										
USGS	13010065	2007-01-25	Ice	P										
USGS	13010065	2007-01-26	Ice	P										
USGS	13010065	2007-01-27	Ice	P										
USGS	13010065	2007-01-28	Ice	P										
USGS	13010065	2007-01-29	Ice	P										
USGS	13010065	2007-01-30	Ice	P										
USGS	13010065	2007-01-31	Ice	P										
USGS	13010065	2007-02-01	Ice	P										
USGS	13010065	2007-02-02	Ice	P										
USGS	13010065	2007-02-03	Ice	P										
USGS	13010065	2007-02-04	299	P										
USGS	13010065	2007-02-05	302	P										
USGS	13010065	2007-02-06	302	P										
USGS	13010065	2007-02-07	298	P										
USGS	13010065	2007-02-08	301	P										
USGS	13010065	2007-02-09	302	P										
USGS	13010065	2007-02-10	306	P										
USGS	13010065	2007-02-11	316	P										
USGS	13010065	2007-02-12	318	P										
USGS	13010065	2007-02-13	319	P										
USGS	13010065	2007-02-14	313	P										
USGS	13010065	2007-02-15	314	P										
USGS	13010065	2007-02-16	319	P										
USGS	13010065	2007-02-17												
USGS	13010065	2007-02-18	307	P										
USGS	13010065	2007-02-19	316	P										
USGS	13010065	2007-02-20	319	P										
USGS	13010065	2007-02-21	332	P										
USGS	13010065	2007-02-22	335	P										
USGS	13010065	2007-02-23	338	P										
USGS	13010065	2007-02-24	344	P										
USGS	13010065	2007-02-25	342	P										
USGS	13010065	2007-02-26	347	P										
USGS	13010065	2007-02-27	352	P										
USGS	13010065	2007-02-28	347	P										
USGS	13010065	2007-03-01	352	P										
USGS	13010065	2007-03-02	349	P										
USGS	13010065	2007-03-03	338	P										
USGS	13010065	2007-03-04	332	P										
USGS	13010065	2007-03-05	336	P										
USGS	13010065	2007-03-06	335	P										
USGS	13010065	2007-03-07	337	P										
USGS	13010065	2007-03-08	338	P										


         
         */
        /// <summary>
        /// Determine column name where this data exists.
        /// </summary>
        private void ParsePreamble()
        {
            int idx = -1;
            string findMe = "#    USGS "+SiteName;
            idx = m_rdb.TextFile.IndexOf(findMe);
            Name = SiteName;
            SiteName = SiteName;
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
            if (m_parameter == UsgsDailyParameter.DailyMeanDischarge)
            {
               idx = m_rdb.TextFile.IndexOf("Discharge, cubic feet per second (Mean)");
               Units = "cfs";
               Parameter = "Discharge";
            }
            if (m_parameter == UsgsDailyParameter.DailyMeanTemperature)
            {
                idx = m_rdb.TextFile.IndexOf("Temperature, water, degrees Celsius (Mean)");
                Units = "degrees Celsius";
                Parameter = "mean water temperature";
            }
            if (m_parameter == UsgsDailyParameter.DailyMaxTemperature)
            {
                idx = m_rdb.TextFile.IndexOf("Temperature, water, degrees Celsius (Maximum)");
                Units = "degrees Celsius";
                Parameter = "max water temperature";
            }
            if (m_parameter == UsgsDailyParameter.DailyMinTemperature)
            {
                idx = m_rdb.TextFile.IndexOf("Temperature, water, degrees Celsius (Minimum)");
                Units = "degrees Celsius";
                Parameter = "min water temperature";
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
            #    USGS 13010065 SNAKE RIVER AB JACKSON LAKE AT FLAGG RANCH WY
             * 
            #    DD parameter statistic   Description
            #    02   00060     00003     Discharge, cubic feet per second (Mean)
            #    03   00010     00001     Temperature, water, degrees Celsius (Maximum)
            012345670901234567890
             * 
            #
# Data provided for site 13010065
#    DD parameter statistic   Description
#    02   00060     00003     Discharge, cubic feet per second (Mean)
#    03   00010     00001     Temperature, water, degrees Celsius (Maximum)
#    03   00010     00002     Temperature, water, degrees Celsius (Minimum)
#    03   00010     00003     Temperature, water, degrees Celsius (Mean)
#    04   00095     00001     Specific conductance, water, unfiltered, microsiemens per centimeter at 25 degrees Celsius (Maximum)
#    04   00095     00002     Specific conductance, water, unfiltered, microsiemens per centimeter at 25 degrees Celsius (Minimum)
#

            */
            string line = m_rdb.TextFile[idx];
            string DD = line.Substring(5, 2);
            string p = line.Substring(10, 5);
            string stat = line.Substring(20, 5);

            m_columnName = DD + "_" + p + "_" + stat;
            m_flagColumnName = m_columnName + "_cd";

            if (m_rdb.Columns.IndexOf(m_columnName) < 0)
            {
                throw new Exception("Could not find column name for '" + line + "'");
            }


        }






        
    }
}
