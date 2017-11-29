using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Reclamation.TimeSeries;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using Reclamation.Core;
using System.Net;
using System.Text;

namespace PiscesWebServices.CGI
{
    /// <summary>
    ///  returns results from web query to timeseries data in pisces.
    /// https://localhost/pn-bin/instant.pl?list=boii ob,boii obx&start=2016-04-15&end=2016-04-20
    /// https://localhost/pn-bin/instant.pl?list=bewo ob,bewo pc&start=2016-04-15&end=2016-04-20&format=zrxp
    /// https://localhost/pn-bin/instant.pl?site=bigi&back=24
    /// https://lrgs1/pn-bin/daily?list=jck fb, amf fb&start=2016-04-15&end=2016-04-20
    /// "https://localhost/pn-bin/daily.pl?site=luc&start=2016-04-01&end=2016-04-20"
    ///  https://localhost/pn-bin/daily.pl?parameter=CRSM%20ET,COVM%20ET,RDBM%20ET,DRLM%20ET,SIGM%20ET,CRSM%20SR,COVM%20SR,RDBM%20SR,DRLM%20SR,SIGM%20SR,CRSM%20WR,COVM%20WR,RDBM%20WR,DRLM%20WR,SIGM%20WR,CRSM%20WG,COVM%20WG,RDBM%20WG,DRLM%20WG,SIGM%20WG,CRSM%20MN,COVM%20MN,RDBM%20MN,DRLM%20MN&syer=2017&format=html&header=false
    /// options :  
    ///      back=12  (12 hours for instant, 12 days for daily)
    ///      print_hourly=true (print hourly data)
    /// 
    /// Legacy Test Samples
    /// https://localhost/pn-bin/instant.pl?station=ABEI&year=2016&month=1&day=1&year=2016&month=1&day=1&pcode=OB&pcode=OBX&pcode=OBM&pcode=TU&print_hourly=1
    /// https://localhost/pn-bin/instant.pl?station=BOII&year=2016&month=1&day=1&year=2016&month=1&day=1&pcode=OB&pcode=OBX&pcode=OBN&pcode=TU
    /// https://localhost/pn-bin/instant.pl?station=ABEI&year=2016&month=1&day=1&year=2016&month=1&day=1&pcode=OB&pcode=OBX&pcode=OBM&pcode=OBN&pcode=TUX&print_hourly=true
    /// </summary>
    public partial class WebTimeSeriesWriter
    {
        TimeSeriesDatabase db;
        DateTime start = DateTime.Now.AddDays(-1).Date;
        DateTime end = DateTime.Now.Date;
        Formatter m_formatter;
        string m_query = "";
        NameValueCollection m_collection;
        TimeInterval m_interval;
        bool contentTypeDefined = false;
        string format = "2";
        string title="";
        bool m_printFlags=false;
        bool printHeader = true;


        string[] supportedFormats = new string[] {"csv", // csv with headers
                                                "html", // basic html
                                                "zrxp", // wiski zxrp (kisters)
                                                "dfcgi", // legacy dayfile cgi program
                                                "1", // legacy tab separated.
                                                "2", // legacy csv
                                                "shefa", // simple shefA format.
                                                "idwr_accounting"
                                                };




        public WebTimeSeriesWriter(TimeSeriesDatabase db, TimeInterval interval, string query = "")
        {
            this.db = db;
            m_query = query;
            InitFormatter(interval);
        }

        private void InitFormatter(TimeInterval interval)
        {
            m_interval = interval;
            ParseQueryOptions(interval);
            DefineFormatter(interval);

            if (m_collection.AllKeys.Contains("print_hourly"))
                m_formatter.HourlyOnly = m_collection["print_hourly"] == "true";


        }

        private void DefineFormatter(TimeInterval interval)
        {
            if (format == "csv")
                m_formatter = new CsvFormatter(interval, m_printFlags);
            else if (format == "zrxp")
            {
                m_formatter = new WiskiFormatter(interval);
            }
            else if (format == "2")
            {
                m_formatter = new LegacyCsvFormatter(interval, m_printFlags);
            }
            else if (format == "1")
            {
                m_formatter = new LegacyCsvFormatter(interval, m_printFlags, "\t");
            }
            else if (format == "html")
            {
                bool printDescription = false;
                if (m_collection.AllKeys.Contains("description", StringComparer.OrdinalIgnoreCase))
                {
                    printDescription = m_collection["description"] == "true";
                }
                m_formatter = new HtmlFormatter(interval, m_printFlags, printHeader, printDescription, title);
            }
            else if (format == "dfcgi")
            {
                m_formatter = new HtmlFormatter(interval, false, true, true, title);
            }
            else if (format == "shefa")
            {
                m_formatter = new ShefAFormatter(interval, false);
            }
            else if( format == "idwr_accounting")
            {
                m_formatter = new IdwrAccountingFormatter(interval,false);
            }
            else
                m_formatter = new LegacyCsvFormatter(interval, m_printFlags);
        }

        private void ParseQueryOptions(TimeInterval interval)
        {
            if (m_query == "")
            {
                m_query = HydrometWebUtility.GetQuery();
            }
            m_query = System.Uri.UnescapeDataString(m_query);
            Logger.WriteLine("Raw query: = '" + m_query + "'");

            if (m_query == "")
            {
                StopWithError("Error: Invalid query");
            }

            m_query = LegacyTranslation(m_query, interval);

            if (!ValidQuery(m_query))
            {
                StopWithError("Error: Invalid query");
            }

            m_collection = HttpUtility.ParseQueryString(m_query);
            if (!HydrometWebUtility.GetDateRange(m_collection, interval, out start, out end))
            {
                StopWithError("Error: Invalid dates");
            }


            if (m_collection.AllKeys.Contains("format"))
                format = m_collection["format"].Trim().ToLower();

            if (m_collection.AllKeys.Contains("title"))
                title = m_collection["title"];

            // because of history daily defaults flags= false;
            // no flags (the old daily database did not have flags )
            m_printFlags = interval == TimeInterval.Hourly || interval == TimeInterval.Irregular;

            if (m_collection.AllKeys.Contains("flags"))
            {
                m_printFlags = m_collection["flags"] == "true";
            }


            if (m_collection.AllKeys.Contains("header"))
            {
                printHeader = m_collection["header"] == "true";
            }

            


            if (Array.IndexOf(supportedFormats, format) < 0)
                StopWithError("Error: invalid format " + format);
        }


        private void StopWithError(string msg)
        {
            PrintContentType();
            Console.WriteLine(msg);
            Logger.WriteLine(msg);
            HydrometWebUtility.PrintHydrometTrailer(msg);

            if(m_interval == TimeInterval.Irregular)
               Help.PrintInstantHelp();
            if (m_interval == TimeInterval.Daily)
                Help.PrintDailyHelp();

                throw new Exception(msg);
        }

        public void Run(string outputFile = "")
        {
            StreamWriter sw = null;
            if (outputFile != "")
            {
                sw = new StreamWriter(outputFile);
                Console.SetOut(sw);
            }
            PrintContentType();

            try
            {
                SeriesList list = CreateSeriesList();

                if (list.Count == 0)
                {
                    StopWithError("Error: list of series is empty");
                }

                WriteSeries(list);
            }
            finally
            {
                if (sw != null)
                    sw.Close();

            }
            //catch (Exception e)
            //{
            //    Logger.WriteLine(e.Message);
            //  Console.WriteLine("Error: Data");	
            //}

            StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
        }

        private void PrintContentType()
        {
            if(!contentTypeDefined)
                Console.Write("Content-type: text/html\n\n");
            contentTypeDefined = true;
        }

        private static bool ValidQuery(string query)
        {
            if (query == "")
                return false;
            if (query.Length > 9000)
                return false;

            bool badMatch = Regex.IsMatch(query, "[^A-Za-z0-9=&%+\\-_,\\s]"); // any other character is considered bad

            return !badMatch;
        }


        /// <summary>
        /// Gets the queried series and generates simple text output
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private void WriteSeries(SeriesList list)
        {

            m_formatter.WriteSeriesHeader(list);

            int daysStored = 30;

            if (m_formatter.Interval == TimeInterval.Daily)
                daysStored = 3650; // 10 years

            if (m_formatter.Interval == TimeInterval.Monthly)
                daysStored = 36500;

            TimeRange timeRange = new TimeRange(start, end);
            WebData wd = new WebData(db);

            foreach (TimeRange item in timeRange.Split(daysStored))
            {
                var interval = m_formatter.Interval;
                var tbl = wd.Read(list, item.StartDate, item.EndDate, interval,m_formatter.OrderByDate);
                m_formatter.PrintDataTable(list, tbl);
            }
            m_formatter.WriteSeriesTrailer();

        }

        
        private SeriesList CreateSeriesList()
        {
            var interval = m_formatter.Interval;

            TimeSeriesName[] names = GetTimeSeriesName(m_collection, interval,db);

            var tableNames = (from n in names select n.GetTableName()).ToArray();

            var sc = db.GetSeriesCatalog("tablename in ('" + String.Join("','", tableNames) + "')");

            SeriesList sList = new SeriesList();
            foreach (var tn in names)
            {
                Series s = new Series();

                s.TimeInterval = interval;
                Logger.WriteLine("tablename: "+tn.GetTableName());
                if (sc.Select("tablename = '" + tn.GetTableName() + "'").Length == 1)
                {
                    s = db.GetSeriesFromTableName(tn.GetTableName());
                }
                s.Table.TableName = tn.GetTableName();
                sList.Add(s);
            }
            return sList;
        }

/// <summary>
/// convert queries such as: list=boii ob, bigi gh 
/// into an array of time series names.
/// 
/// </summary>
/// <param name="query"></param>
/// <param name="interval"></param>
/// <param name="db"></param>
/// <returns></returns>
        private static TimeSeriesName[] GetTimeSeriesName(NameValueCollection query, TimeInterval interval,TimeSeriesDatabase db)
        {
            
            var custom_list = HydrometWebUtility.GetParameter(query, "custom_list");
            if (custom_list == "idwr" && interval == TimeInterval.Irregular)
            {
                return IdwrCustom.GetIDWRInstantList(db.Server).ToArray();
            }
            else if( custom_list.ToLower().IndexOf("wd") == 0
                   && interval == TimeInterval.Daily)
            {
                return IdwrCustom.GetIDWRDailyList(db.Server, custom_list).ToArray();
            }

            List<TimeSeriesName> rval = new List<TimeSeriesName>();
            var listParameter = HydrometWebUtility.GetParameter(query, "list");

            Logger.WriteLine("GetTimeSeriesName()");
            Logger.WriteLine(query.ToString());

            var siteCodePairs = listParameter.ToLower().Split(',');

            foreach (var item in siteCodePairs)
            {
                var tokens = item.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 2)
                {
                    TimeSeriesName tn = new TimeSeriesName(tokens[0] + "_" + tokens[1], interval);
                    rval.Add(tn);
                }
                else if( tokens.Length ==1 )
                {//just the site return a list of all parameters
                    var parms = db.GetParameters(tokens[0].Trim(), interval,false);
                    for (int i = 0; i < parms.Length; i++)
                    {
                        TimeSeriesName tn = new TimeSeriesName(tokens[0] + "_" + parms[i], interval);
                        rval.Add(tn);
                    }

                }
            }
           
            return rval.ToArray();
        }

        
    }
}