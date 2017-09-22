/*********
 * 2009 works with pisces time series database
 * 2008 April (multiple runs based on counter. slot_set was not reliable now 
 * using END_RUN_PREAMBLE
 * 2006 August  (converted to c#)
 * 2004 January.   special case for multi run file.
 * 2003 -- August --
 *  added function to create pisces tree from rdf file (create with snapshot in riverware)
 * 
 * Karl Tarbet --  2003 - Feb  (Original code dos_yakima_riverware_rdf.cpp )
 * Program to connect RiverWare time series data to Pisces
 * This program uses .rdf files (riverware data files)
 *********/
using System;
using System.IO;
using System.Collections.Generic;
using Reclamation.TimeSeries;
using Reclamation.Core;
using System.Configuration;
using System.Data;
namespace Reclamation.TimeSeries.RiverWare
{

    public class RiverWareSeries : Series
    {
        List<DateTime> dates;
        string m_filename;
        int m_scenarioNumber = -1;
        TextFile m_textFile;
        static Dictionary<string, TextFile> s_cache = new Dictionary<string, TextFile>();
        string m_slotName = "";
        string m_objectName="";
        bool m_isSnapShot;
        
        int scenarioIndex = -1;

        /// <summary>
        /// Constructs a Riverware Series
        /// </summary>
        /// <param name="fileName">full path of .rdf file</param>
        /// <param name="objectName">RiverWare object name</param>
        /// <param name="slotName">type of data (RiverWare slot name)</param>
        /// <param name="scenarioNumber">index to locate mms scenario enter -1 if this is not a mss rdf file</param>
        /// <param name="isSnapShot">enter true if the .rdf file is a snapshot style object_type in rdf file is 'SnapShotObj'</param>
        public RiverWareSeries(string fileName, string objectName,
                     string slotName, int scenarioNumber, bool isSnapShot, string units="")
        {
            ExternalDataSource = true;
            m_filename = fileName;
            this.Name = objectName + "_" + slotName;
            this.Name = this.Name.Replace(":", "_");

            m_scenarioNumber = scenarioNumber;
            ScenarioName = "Run " + scenarioNumber;
            m_isSnapShot = isSnapShot;
            m_slotName = slotName;
            m_objectName = objectName;
            SiteID = m_objectName;
            if (m_isSnapShot) // get parameter from ending of slot_name
            {
                if (this.Name == "")
                {
                    Name = slotName;
                }
                int idx_ = m_slotName.LastIndexOf("_");
                Parameter = m_slotName.Substring(idx_ + 1); //Gage Outflow
            }

            else
            {
                Parameter = slotName;
            }


            Appearance.LegendText = objectName + ":" + slotName;
            Provider = "RiverWareSeries";
            ConnectionString = "FileName=" + fileName + ";ObjectName=" + objectName
                + ";SlotName=" + slotName + ";ScenarioNumber=" + scenarioNumber
                + ";IsSnapShot=" + isSnapShot;
            
            GetFileReference();
            ComputeScenarioIndex();

            DetermineInterval(); // searches for time_step
            if (units != "")
                this.Units = units;
            else
              FindIndexToSeriesDataAndSetUnits(scenarioIndex);

            //Units = ReadUnits(idxLocation-5);
        }




        /// <summary>
        /// Creates RiverWareSeries from TimeSeriesDatabase
        /// </summary>
        /// <param name="sdi"></param>
        /// <param name="db"></param>
        public RiverWareSeries(TimeSeriesDatabase db,TimeSeriesDatabaseDataSet.SeriesCatalogRow sr):base(db,sr)
        {

            m_filename = ConnectionStringUtility.GetFileName(ConnectionString, m_db.DataSource);
            this.ExternalDataSource = true;

            m_objectName =  ConnectionStringUtility.GetToken(ConnectionString, "ObjectName","");
            m_slotName = ConnectionStringUtility.GetToken(ConnectionString, "SlotName","");

            int.TryParse(ConnectionStringUtility.GetToken(ConnectionString, "ScenarioNumber",""), out m_scenarioNumber);
            bool.TryParse(ConnectionStringUtility.GetToken(ConnectionString, "IsSnapShot",""), out m_isSnapShot);
            
            InitTimeSeries(null, this.Units, this.TimeInterval, this.ReadOnly, false, true);
            ScenarioName = "Run " + m_scenarioNumber;
            Appearance.LegendText = Name;
        }


        /// <summary>
        /// Get TextFile from disk or memory
        /// </summary>
        private void GetFileReference()
        {
            if (s_cache.ContainsKey(m_filename))
            { // allready in memory.
                Logger.WriteLine("Found file in cache: "+Path.GetFileName(m_filename));
                m_textFile = s_cache[m_filename];

                FileInfo fi = new FileInfo(m_filename);

                if (fi.LastWriteTime > m_textFile.LastWriteTime)
                {
                    Logger.WriteLine("File has changed... updating cache");
                    m_textFile = new TextFile(m_filename);
                    s_cache[m_filename] = m_textFile;
                }
            }
            else
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
        }
     
         private void DetermineInterval()
         {
           this.TimeInterval = Reclamation.TimeSeries.TimeInterval.Irregular; //default
             ///time_step_unit:day
             int idx = m_textFile.IndexOf("time_step_unit:");
             if (idx >= 0)
             {
                 string s = m_textFile[idx].Substring(15).Trim();
                if( s == "day")
                     this.TimeInterval = Reclamation.TimeSeries.TimeInterval.Daily;
                 if( s == "month")
                     this.TimeInterval = Reclamation.TimeSeries.TimeInterval.Monthly;
             }

         }

         public override PeriodOfRecord GetPeriodOfRecord()
         {
            //var dates = ReadDates();
             Read();
             return new PeriodOfRecord(this.MinDateTime, this.MaxDateTime, Count);
         }

         public override Series CreateScenario(TimeSeriesDatabaseDataSet.ScenarioRow scenario)
         {
             //string s = ConfigurationManager.AppSettings["scenarioList"];
             string fn = this.m_filename; // TO DO..update scenarioNumber if needed

             if (m_scenarioNumber >= 0 && m_db != null)
             {
                 
                int idx = Array.IndexOf(m_db.GetScenarios().GetNames(), scenario.Name);
                m_scenarioNumber = idx + 1;
             }
             else
             {
                 string path = Path.GetDirectoryName(m_filename);
                 fn = Path.Combine(path, scenario.Name + ".rdf");
                 Logger.WriteLine("Reading series from " + fn);
                 if (!File.Exists(fn))
                 {
                     Logger.WriteLine("Error: Can't create scenario '" + scenario.Name + "'");
                     return new Series();
                 }
             }

         var rval = new RiverWareSeries(fn, m_objectName, m_slotName, m_scenarioNumber, m_isSnapShot);
         rval.Appearance.LegendText = scenario.Name + " " + Name;
         rval.ScenarioName = scenario.Name;
         return rval;
             
         }

         protected override void ReadCore(DateTime t1, DateTime t2)
        {
            ReadFromFile(t1, t2);   
        }
        
        private void ReadFromFile(DateTime t1, DateTime t2)
        {
            Clear();

           // if( m_textFile == null)
               GetFileReference();

            if (scenarioIndex < 0) //  database constructor does not read file until Read()
                ComputeScenarioIndex();

            dates = ReadDates(m_textFile, scenarioIndex);

            List<double> data = ReadData( dates.Count,scenarioIndex);

            if (dates.Count != data.Count)
            {
                Logger.WriteLine("Error:count of data and dates must be the same. There were " + dates.Count + " dates and " + data.Count + " points in time series");
                return;
            }

            for (int i = 0; i < dates.Count; i++)
            {
                if (dates[i] >= t1 && dates[i] <= t2)
                {
                    this.Add(dates[i], data[i]);
                }
            }

            
        }

        private void ComputeScenarioIndex()
        {
             scenarioIndex = 0;

            if (m_scenarioNumber != -1)
            {
                scenarioIndex = m_textFile.NthIndexOf("END_RUN_PREAMBLE", m_scenarioNumber);
                // backup a litte to also capture time_steps.
                for (int i = 1; i < 20; i++)
                {
                    int j = scenarioIndex - i;
                    if (j <= 0)
                        break;
                    if (m_textFile[j].IndexOf("END") == 0)
                    {
                        scenarioIndex = j;
                        break;
                    }
                }
            }

            if (scenarioIndex < 0)
            {
                Console.WriteLine(m_textFile.FileName);

                throw new ArgumentException("File " + m_textFile.FileName + " does not have a MMS scenairo number '" + m_scenarioNumber + "'");
            }

            scenarioIndex -= 4; // backup before flag 'time_steps:' or 'timesteps' (count of timesteps)
            scenarioIndex = System.Math.Max(0, scenarioIndex);  // can't be negative

           // return startingIndex;
        }

        

        List<double> ReadData(int numDates,int  scenarioIndex )
        {
            List<double> rval = new List<double>();

            int idxLocation = FindIndexToSeriesDataAndSetUnits(scenarioIndex);

            if (idxLocation < 0)
            {
                return rval;
            }

            // find scale

            double scale = ReadScalingFactor(idxLocation);

            for (int i = idxLocation; i < (idxLocation + numDates); i++)
            {
                if (m_textFile[i].IndexOf("NaN") >= 0)
                {
                    rval.Add(Reclamation.TimeSeries.Point.MissingValueFlag);
                }
                else
                {
                    double d;
                    if (double.TryParse(m_textFile[i], out d))
                    {
                        rval.Add(d*scale);
                    }
                    else
                    {
                        throw new InvalidCastException("can't convert " + m_textFile[i] + " to a number");
                    }
                }

            }
            return rval;
        }

        private double ReadScalingFactor(int idxLocation)
        {
           double rval = 1.0;

           string s = m_textFile[idxLocation-1];
           if (s.IndexOf("scale:") == 0 && s.Length > 6)
           {
               // parse out scale
               s =  s.Substring(6);
               double d = 0.0;
               if (double.TryParse(s, out d))
                   rval = d;
           }
                return rval;
        }

        private int FindIndexToSeriesDataAndSetUnits(int scenarioIndex)
        {
            int idxLocation = -1;

            if (m_isSnapShot)
            { // snapshot style rdf file.
                /* search based on slot_name only
               object_type: SnapShotObj  (same through whole file)
               object_name: Target Flows 1  (same through whole file)
               slot_name: Wolf Gage_Gage Outflow    # object_name (under score) slot_name
               END_SLOT_PREAMBLE
               units: cfs
               scale: 1
               */
                idxLocation = m_textFile.IndexOf(m_slotName, scenarioIndex);

                if (idxLocation >= 0)
                {
                    this.Units = ReadUnits(idxLocation);
                    idxLocation += 4;
                }
            }
            else // 'regular' rdf file
            {
                /*
             object_name: Local above CLFW     # location
             slot_name: Inflow1                # type
             END_SLOT_PREAMBLE
             units: cfs
             scale: 1
                */
                string on = m_objectName.Replace("^", "\\^"); // Warren Sharps model had ^ in the object name..

                idxLocation = m_textFile.IndexOfBothRegex("^object_name: " + on + "$", "^slot_name: " + m_slotName + "$", scenarioIndex);
                if (idxLocation >= 0)
                {
                    this.Units = ReadUnits(idxLocation);
                    idxLocation += 5;
                }
            }
            return idxLocation;
        }

        /// <summary>
        /// reads units
        /// </summary>
        /// <param name="tf"></param>
        /// <param name="idxLocation"></param>
        /// <returns></returns>
        internal string ReadUnits( int idxLocation)
        {
            TextFile tf = m_textFile;
            string units = "";
            int idx = tf.IndexOf("units:",idxLocation);
            int distance = (idx - idxLocation);
            if (idx >= 0 && distance < 10)
            {
                units = m_textFile[idx].Substring(6).Trim();
            }

            return units;
        }

     


   

        /***************************************
 * returns list of dates from beginning of riverware file in *.rdf format.
 **************************************************/
        private List<DateTime> ReadDates(Reclamation.Core.TextFile tf, int startingIndex)
        {
            List<DateTime> rval = new List<DateTime>();


            int idx = tf.IndexOfAny(new string[] { "time_steps:", "timesteps" }, startingIndex);

            if (idx < 0)
            {
                throw new Exception("Error looking for 'timesteps' or 'time_steps:' in " + tf.FileName);
            }

            //timesteps:15706
            //time_steps:366
            string s = tf[idx];
            int i2 = s.IndexOf(":");
            s = s.Substring(i2 + 1); // just the number part
            int numDates = Convert.ToInt32(s);

            int idxToDates = tf.IndexOf("END_RUN_PREAMBLE", idx);


            if (idxToDates < 0)
            {
                throw new Exception("Error looking for END_RUN_PREAMBLE in " + tf.FileName);
            }

            idxToDates++;

            for (int i = 0; i < numDates; i++)
            {
                DateTime d;
                string dateString = tf[idxToDates + i];
                dateString = dateString.Replace(" 24:00", " 23:59:59");
                if (DateTime.TryParse(dateString, out d))
                {
                    rval.Add(d);
                }
                else
                {
                    throw new Exception("Can't parse " + tf[idxToDates + i] + " as a date and time");
                }
            }

            return rval;
        }
    }
}