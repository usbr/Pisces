using System;
using System.Collections.Generic;
using Reclamation.Core;
using System.Windows.Forms;
using Reclamation.TimeSeries;

namespace Reclamation.TimeSeries.Hydromet
{
    public class IDWRDailySeries : Series
    {
         ///<summary>
         ///SCRIPT TO TEST IDWR SERIES GENERATION and UPDATING
         ///</summary>
         ///<param name="args"></param>
        static void Main(string[] args)
        {

            string fn = @"C:\temp\IDWR_UpperSnake_DB.pdb";
            var db = new TimeSeriesDatabase(new SQLiteServer(fn),false);
            string station = "13080000";
            var s = db.GetSeriesFromName("IDWR"+station);
            s.Read(DateTime.Parse("1/1/2009"), DateTime.Parse("12/31/2009"));
            //s.IDWRUpdate(2011);
            Console.WriteLine(s);
        }

        
        string station;

        public IDWRDailySeries(string station)
        {
            this.station = station;
            TimeInterval = TimeSeries.TimeInterval.Daily;
            Units = "cfs";
        }

        /// <summary>
        /// Reads IDWR Series from pdb Database.
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            if (m_db != null)
            {
                base.ReadCore(t1, t2);
            }
            else
            {
                Add(
                IDWRWebDownload(station, t1, t2));
            }

            
        }


        /// <summary>
        /// Updates IDWR SDF Database with downloaded IDWR values.
        /// Relies on an existing Pisces DB located at the path below.
        /// </summary>
        /// <param name="args"></param>
        //public void IDWRUpdate(int year)
        //{
        //    // Opens Pisces Connection and reads Pisces DB.
        //    Reclamation.Core.SqlServerCompact pDB = new SqlServerCompact
        //        (@"C:\Documents and Settings\jrocha\Desktop\IDWR_UpperSnake_DB.sdf");
        //    TimeSeriesDatabase DB = new TimeSeriesDatabase(pDB);

        //    // Gets Pisces table and series.
        //    string tableName = "IDWR" + station;
        //    var seriesPisces = DB.GetSeriesFromName(tableName);
        //    seriesPisces.Read();

        //    // Gets IDWR series.
        //    string calendarType = "C";
        //    Series seriesIDWR = IDWRWebDownload(station, year, year, calendarType);

        //    // Updates Pisces series with new IDWR data.
        //    int countPisces = seriesPisces.Count;
        //    int countIDWR = seriesIDWR.Count;
        //    for (int i = 1; i <= countIDWR; i++)
        //    {
        //        if (seriesPisces[countPisces - 1].DateTime < seriesIDWR[countIDWR - i].DateTime)
        //        {
        //            seriesPisces.Add(seriesIDWR[countIDWR - i]);
        //        }
        //    }

        //    // Updates Pisces values with modified IDWR values.
        //    countPisces = seriesPisces.Count;
        //    for (int i = 1; i <= countIDWR; i++)
        //    {
        //        if (!seriesPisces[countPisces - i].Value.Equals(seriesIDWR[countIDWR - i].Value))
        //        {
        //            seriesPisces[countPisces - i] = seriesIDWR[countIDWR - i];
        //        }
        //    }
        //    int dbID = 1;//seriesPisces.SiteDataTypeID;
        //    DB.SaveTimeSeriesTable(dbID, seriesPisces, DatabaseSaveOptions.Save);
        //}


        /// <summary>
        /// Downloads IDWR series from IDWR website and returns a series.
        /// </summary>
        /// <param name="station">IDWR station number</param>
        /// <returns></returns>
        private static Series IDWRWebDownload(string station, DateTime t1, DateTime t2  )
        {
        
        //  W: Water Year, I: Irrigation Year, C: Calendar Year
            string calendarType = "C";

            int year1 = t1.Year;
            int year2  =t2.Year;

            // Produces URL for data download. Data download string researched from this website:
            //http://maps.idwr.idaho.gov/qWRAccounting/WRA_Select.aspx
            string urlDate = station + "." + year1;
            year1++;
            while (year1 <= year2)
            {
                urlDate = urlDate + "," + station + "." + year1;
                year1++;
            }
            string url = "http://maps.idwr.idaho.gov/qWRAccounting/WRA_Download.aspx?req="
                + urlDate + "&datatypedatatype=HST&calendartype=" + calendarType + "&file=CSV";
            var data = Web.GetPage(url);
            if (data[0] == "")
            { throw new ArgumentException("Unexpected IDWR Database Error: Try Again Later"); }

            // Populates a Hydromet Series with IDWR data.
            DateTime t; double value; var series = new Series();
            int j = 1;
            int count = data.Length;
            while (j < count - 3)  // Outputs series with raw data without consideration for missing data points.
            {
                //"Site,Date,Value,Title"
                var s = Reclamation.Core.CsvFile.ParseCSV(data[j]);
                if (s.Length < 3)
                    continue;

                // s[0] contains site number for all rows. Duplicates.
                if (!DateTime.TryParse(s[1], out t))
                {
                    Logger.WriteLine("invalid date " + s[1]);
                    continue;
                }
                t = DateTime.Parse(s[1]);

                if (!double.TryParse(s[2], out value))
                {
                    Logger.WriteLine("invalid value " + s[2]);
                    continue;
                }
                
                // s[3] contains 'value' units for all rows. Duplicates.
                if( t >= t1 && t <= t2)
                  series.Add(t, value);
                j++;
            }
            series.TimeInterval = TimeInterval.Daily;
            series = Math.FillMissingWithZero(series);
            return series;
        }


        /// <summary>
        /// Parses text file containing all of IDWR Upper Snake data and returns a series.
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        private static Series IDWRSeriesDatabase(string station)
        {
            // Read Raw Data.
            TextFile RawData = new TextFile
                (@"C:\Documents and Settings\jrocha\Desktop\SnakeDiversionData.txt");
            int count = RawData.Length;
            var ParsedData = new List<string>();
            for (int i = 1; i < count; i++)
            {
                ParsedData.AddRange(RawData[i].Split('\t'));
            }
            int ParsedDataCount = ParsedData.Count;

            Series s = new Series();
            int k = 0;
            while (k < ParsedDataCount - 4)
            {
                if (station == ParsedData[k])
                {
                    DateTime t = DateTime.Parse(ParsedData[k + 2]);
                    if (ParsedData[k + 3] == "")
                    { ParsedData[k + 3] = "0.00"; }
                    double val = Convert.ToDouble(ParsedData[k + 3]);
                    s.Add(t, val);
                }
                k = k + 4;
            }
            return s;
        }


    }
}
