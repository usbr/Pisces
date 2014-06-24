using System;
using System.Collections.Generic;
using System.Text;
using Reclamation.TimeSeries;
using Reclamation.Core;
using System.IO;
using System.Data;

namespace Reclamation.TimeSeries.DataLogger
{
    /// <summary>
    /// Time series data from Campbell Scientific Cr10x data file
    /// </summary>
    public class Cr10xSeries: Series
    {
        string m_filename;
        string m_interval;
        int m_columnNumber;
       // string m_path;

        //filename=Buckhorn.DAT;interval=60;ColumnNumber=7



        /// <summary>
        /// Creates Cr10xSeries from basic elements.
        /// </summary>
        /// <param name="filename">path to *.dat file</param>
        /// <param name="interval">interval of data (minutes) examples: 15,30,60,24</param>
        /// <param name="columnNumber"></param>
        public Cr10xSeries(string filename, int interval, int columnNumber)
        {
            this.ExternalDataSource = true;
            m_filename = filename;
            m_interval = interval.ToString();
            ReadOnly = true;
            m_columnNumber = columnNumber;
            Source = "cr10x";
            Provider = "Cr10xSeries";
            Name = Path.GetFileNameWithoutExtension(m_filename);
            SiteName = Path.GetFileNameWithoutExtension(m_filename);
            Parameter = "";
            ConnectionString = "FileName=" + m_filename + ";Interval=" + m_interval
              + ";ColumnNumber=" + columnNumber ;

        }

       
        public Cr10xSeries(TimeSeriesDatabase db,Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow sr)
            : base(db,sr)
        {
            ExternalDataSource = true;
            ReadOnly = true;
            m_filename = ConnectionStringUtility.GetToken(ConnectionString, "FileName","");

            if (!Path.IsPathRooted(m_filename))
            {
                string dir = Path.GetDirectoryName(m_db.DataSource);
                m_filename = Path.Combine(dir, m_filename);
            }
            m_interval = ConnectionStringToken("Interval");

            m_columnNumber = Convert.ToInt32(ConnectionStringUtility.GetToken(ConnectionString, "ColumnNumber",""));

            InitTimeSeries(null, this.Units, this.TimeInterval, this.ReadOnly, false, true);
            Appearance.LegendText = Name;

        }


        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            ReadFile(t1, t2);   
        }

        public static DateTime JulianToDate(int year, int julianDay)
        {
            DateTime t1 = new DateTime(year, 1, 1);
            DateTime t2 = t1.AddDays(julianDay - 1);
            return t2;
        }
   


        private void ReadFile(DateTime t1, DateTime t2)
        {
            //try
            //{
            int skipCounter = 0;
                this.Clear();
               // string fn = Path.Combine(m_path, m_filename);
                //CsvFile db = GetTextDB(fn);
                string[] lines = File.ReadAllLines(m_filename);
                //string sql = "Column1 = '" + m_interval + "'";
               // DataRow[] rows = db.Select(sql);
                for (int i=0; i<lines.Length; i++)
                {
                    string[] r = CsvFile.ParseCSV(lines[i]);

                    double val = -1;
                    int year = -1;
                    int julianDay = -1;
                    if (r.Length <4 || 
                          r[0] != m_interval
                      ||  m_columnNumber  >  r.Length 
                        || Double.TryParse(r[m_columnNumber - 1],out val) == false
                        || int.TryParse( r[1],out year) == false
                        || int.TryParse( r[2] ,out julianDay)== false
                        || r[3].Trim() == "")
                    { // last line in ash_crk.dat was truncated..
                        int lineNumber = i + 1;
                        //Logger.WriteLine("skipping line number "+lineNumber+"'" + lines[i] + "'");
                        continue;
                    }

                    double minutes = 0;
                    bool is2400 = false;
                    if (!TryParseMinutes(r[3], m_interval, out minutes, out is2400))
                    {
                        int lineNumber = i+1;
                        if( skipCounter < 25)
                        Logger.WriteLine("skipping line number " + lineNumber + "'" + lines[i] + "'");
                        skipCounter++;

                    }
                    else
                    {
                        //int julianDay = Convert.ToInt32(r[2].ToString());
                        DateTime t = JulianToDate(year, julianDay);
                        if (is2400)
                        {
                            t = t.AddDays(1);
                        }
                        else
                        {
                            t = t.AddMinutes(minutes);
                        }
                        
                        if (t >= t1 && t <= t2)
                        {
                            if (IndexOf(t) >= 0)
                            {
                                if (skipCounter < 25)
                                {
                                    Logger.WriteLine("================");
                                    Logger.WriteLine("filename " + m_filename);
                                    Logger.WriteLine("duplicate data skipped ");
                                    Logger.WriteLine("julianDay " + julianDay);
                                    Logger.WriteLine("Year " + year);
                                    Logger.WriteLine("interval " + m_interval);
                                    Logger.WriteLine("minutes " + minutes);
                                    skipCounter++;
                                }
                            }
                            else
                            {
                                Add(t, val);
                            }
                        }
                    }
                }

            //}
            //catch (Exception ex)
            //{
            //    Logger.WriteLine(ex.Message);
            //}
        }

        private void WriteSkipRowMessage(DataRow r)
        {
            Logger.WriteLine("SKipping row");
            Logger.WriteLine("column " + m_columnNumber + " is null ");
            Logger.WriteLine("interval is " + m_interval);
            for (int i = 0; i < r.Table.Columns.Count; i++)
            {
                Logger.WriteLine(r.Table.Columns[i].ColumnName + " : " + r[i].ToString());
            }
        }

        /// <summary>
        /// public for testing frameworks
        /// </summary>
        public static bool TryParseMinutes(string hmm, string interval, out double minutes, out bool is2400)
        {
            is2400 = false;
            minutes = -1;
            if (interval == "24")
            {// billings.dat does not have 2400 column
                is2400 = true;
                minutes = 23 * 60 + 59.999;
                return true;
            }
            
            if( hmm.IndexOf(".")>=0)
            {
                return false;
            }

            if (hmm.Length == 2) // minutes only
            {
                minutes = Convert.ToDouble(hmm);
                return true;
            }
            if (hmm == "2400")
            {
                is2400 = true;
               minutes = 23 * 60 + 59.999;
                return true;
            }
            else if (hmm.Length == 3)
            {
                int m = Convert.ToInt32(hmm.Substring(1));
                int h = Convert.ToInt32(hmm.Substring(0, 1));
                minutes = h * 60 + m;
                return true;
            }
            else if (hmm.Length == 4)
            {
                int m = Convert.ToInt32(hmm.Substring(2));
                int h = Convert.ToInt32(hmm.Substring(0, 2));
               minutes = h * 60 + m;
                return true;
            }

            return false;
        }



    }
}
