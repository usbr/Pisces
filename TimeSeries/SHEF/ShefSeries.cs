using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Reclamation.TimeSeries.SHEF
{

    /*
     * TO DO: 
     * 1. Build a ReadCore() method using the connection string to read from the source text file
     * 2. Move some of the functions that gets Stations and P-Codes from ImportShef.cs in Forms/ImportForms
     * 
     */

    public class ShefSeries : Series
    {
        DataTable shefDataTable = new DataTable();
        string m_location, m_pecode, m_filename;
        TextFile m_textFile;
        static Dictionary<string, TextFile> s_cache = new Dictionary<string, TextFile>();


        public ShefSeries()
        {
            new ShefSeries("", "", "");
        }

        public ShefSeries(string location, string pecode, string filename)
        {
            ExternalDataSource = false;
            this.Name = location + "_" + pecode;
            this.SiteID = location;
            this.Parameter = pecode;
            this.Source = "SHEF";
            this.Provider = "ShefSeries";
            getShefTimeInterval(pecode);
            this.ConnectionString = "File=" + filename + ";ShefLocation=" + location + ";ShefCode=" + pecode + "";
            this.Table.TableName = this.Name;
        }

        public ShefSeries(TimeSeriesDatabase db, TimeSeriesDatabaseDataSet.SeriesCatalogRow sr):base(db, sr)
        {
            m_location = ConnectionStringUtility.GetToken(ConnectionString, "ShefLocation", "");
            m_pecode = ConnectionStringUtility.GetToken(ConnectionString, "ShefCode", "");
            m_filename = ConnectionStringUtility.GetToken(ConnectionString, "File", "");
            InitTimeSeries(null, "", this.TimeInterval, true);
        }

        private void getShefTimeInterval(string pecode)
        {
            try
            {
                var tChar = pecode.ToCharArray()[pecode.Length - 1];
                if (tChar == 'H')
                { this.TimeInterval = TimeSeries.TimeInterval.Hourly; }
                else if (tChar == 'D')
                { this.TimeInterval = TimeSeries.TimeInterval.Daily; }
                else if (tChar == 'M')
                { this.TimeInterval = TimeSeries.TimeInterval.Monthly; }
                else
                { this.TimeInterval = TimeSeries.TimeInterval.Irregular; }
            }
            catch
            { this.TimeInterval = TimeSeries.TimeInterval.Irregular; }
        }

        public DataTable ReadShefFile(string fileName)
        {
            m_filename = fileName;
            shefDataTable = new DataTable();
            shefDataTable.Columns.Add(new DataColumn("location", typeof(string)));
            shefDataTable.Columns.Add(new DataColumn("datetime", typeof(DateTime)));
            shefDataTable.Columns.Add(new DataColumn("shefcode", typeof(string)));
            shefDataTable.Columns.Add(new DataColumn("value", typeof(double)));

            if (!s_cache.ContainsKey(m_filename))
            {
                GetFileReference();
            }

            TextFile lines = s_cache[m_filename];
            for (int i = 0; i < lines.Length; i++)
            {
                addShefDataTableLine(lines[i]);
            }


            return shefDataTable;
        }

        private void addShefDataTableLine(string line)
        {
            var lineItems = System.Text.RegularExpressions.Regex.Split(line, @"\s+");
            string location = lineItems[1];
            DateTime t = DateTime.ParseExact(lineItems[2] + lineItems[4].Replace("DH", ""), "yyyyMMddHHmm", System.Globalization.CultureInfo.InvariantCulture);
            var lineShefCodes = System.Text.RegularExpressions.Regex.Split(line, @"\/+");
            for (int i = 1; i < lineShefCodes.Count(); i++)
            {
                var shefItems = System.Text.RegularExpressions.Regex.Split(lineShefCodes[i], @"\s+");
                var shefcode = shefItems[0];
                double shefValue;
                if (!double.TryParse(shefItems[1], out shefValue)) { shefValue = double.NaN; }
                shefDataTable.Rows.Add(location, t, shefcode, shefValue);
            }
        }

        protected override void UpdateCore(DateTime t1, DateTime t2, bool minimal = false)
        {
            ReadFromFile(t1, t2);
        }

        private void ReadFromFile(DateTime t1, DateTime t2)
        {
            Clear();
            GetFileReference();
            shefDataTable = ReadShefFile(m_filename);
            //var newSeries = new ShefSeries(m_location, m_pecode, m_filename);
            var valTable = shefDataTable.Select(string.Format("location = '{0}' AND shefcode = '{1}'", m_location, m_pecode));
            foreach (DataRow item in valTable)
            {
                this.Add(DateTime.Parse(item["datetime"].ToString()), Convert.ToDouble(item["value"]));
            }
            m_db.SaveProperties(this);// LastWriteTime proabably changed
            m_db.SaveTimeSeriesTable(this.ID, this, DatabaseSaveOptions.DeleteAllExisting);
        }

        /// <summary>
        /// Get TextFile from disk, memory, or web
        /// </summary>
        private void GetFileReference()
        {
            if (m_filename.Contains("http") && !s_cache.ContainsKey(m_filename))
            {
                Logger.WriteLine("Reading file from web: '" + m_filename + "'");
                System.Net.WebClient client = new System.Net.WebClient();
                Stream stream = client.OpenRead(m_filename);
                StreamReader reader = new StreamReader(stream);
                String content = reader.ReadToEnd();

                string[] stringSeparators = new string[] { "\r\n" };
                string[] lines = content.Split(stringSeparators, StringSplitOptions.None);

                m_textFile = new TextFile();
                foreach (var line in lines)
                {
                    if (line != "")
                    { m_textFile.Add(line); }
                }

                int max_cache_size = 3;
                if (s_cache.Count >= max_cache_size)
                {
                    s_cache.Clear();
                    Logger.WriteLine(" s_cache.Clear();");
                }
                s_cache.Add(m_filename, m_textFile);
            }
            else if (!s_cache.ContainsKey(m_filename))
            {
                Logger.WriteLine("reading file from disk: " + Path.GetFileName(m_filename));
                if (!File.Exists(m_filename))
                {
                    Logger.WriteLine("File does not exist: '" + m_filename + "'");
                }

                m_textFile = new TextFile(m_filename);
                int max_cache_size = 3;
                if (s_cache.Count >= max_cache_size)
                {
                    s_cache.Clear();
                    Logger.WriteLine(" s_cache.Clear();");
                }
                s_cache.Add(m_filename, m_textFile);
            }
            else
            { // already in memory.
                Logger.WriteLine("Found file in cache: " + Path.GetFileName(m_filename));
                m_textFile = s_cache[m_filename];

                if (!m_filename.Contains("http"))
                {
                    FileInfo fi = new FileInfo(m_filename);
                    if (fi.LastWriteTime > m_textFile.LastWriteTime)
                    {
                        Logger.WriteLine("File has changed... updating cache");
                        m_textFile = new TextFile(m_filename);
                        s_cache[m_filename] = m_textFile;
                    }
                }
            }            
        }
        
                        
    }
}
