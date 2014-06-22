using System;
using Reclamation.Core;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Reclamation.TimeSeries.Hec
{
    public class HecDssSeries: Series
    {
        HecDssPath path;
        string m_filename;
        string m_dssPath;
        public HecDssSeries(string filename, string dssPath)
        {
            ExternalDataSource = true;
            path = new HecDssPath(dssPath);
            m_filename = filename;
            m_dssPath = dssPath;

            FileInfo fi = new FileInfo(filename);
            Name = path.B + " " + path.C + " " + path.E + " " + path.F;
            SiteName = path.B;
            this.ConnectionString = "FileName=" + filename
               + ";LastWriteTime=" + fi.LastWriteTime.ToString(DateTimeFormatInstantaneous)
               + ";DssPath="+dssPath;
            Provider = "HecDssSeries";
            EstimateInterval();
            Source = "hecdss"; // for icon name
            ReadOnly = true;
            HasFlags = true;
        }

        

        public HecDssSeries(TimeSeriesDatabase db,Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow sr)
            : base(db,sr)
        {
            ExternalDataSource = true;
            m_filename = ConnectionStringToken("FileName");
            if (!Path.IsPathRooted(m_filename))
            {
                string dir = Path.GetDirectoryName(m_db.DataSource);
                m_filename = Path.Combine(dir, m_filename);
            }
            m_dssPath = ConnectionStringToken("DssPath");
            path = new HecDssPath(m_dssPath);
            ReadOnly = true;
            ScenarioName = Path.GetFileNameWithoutExtension(m_filename);
            InitTimeSeries(null, this.Units, this.TimeInterval, this.ReadOnly, true, true);
            Appearance.LegendText = Name;
        }
        /// <summary>
        /// Creates Scenario based on scenaroName as dss filename without extension (.dss)
        /// </summary>
        public override Series CreateScenario(TimeSeriesDatabaseDataSet.ScenarioRow scenario)
        {

            if (scenario.Name == ScenarioName)
            {
                this.Appearance.LegendText = scenario.Name + " " + Name;
                return this;
            }

            string path = Path.GetDirectoryName(m_filename);
            string fn = Path.Combine(path, scenario.Name + ".dss");
            Logger.WriteLine("Reading series from " + fn);
            if (!File.Exists(fn))
            {
                Logger.WriteLine("Error: Can't create scenario");
                return new Series();
            }


            var rval = new HecDssSeries(m_filename, m_dssPath);
            rval.Name = this.Name;
            rval.Appearance.LegendText = scenario.Name + " " + Name;

            rval.SiteName = this.SiteName;
            rval.TimeInterval = this.TimeInterval;
            return rval;
        }



        private void EstimateInterval()
        {
            this.TimeInterval = TimeInterval.Irregular; // default.

            if (path.E.IndexOf("IR-") == 0)
                this.TimeInterval = TimeInterval.Irregular;
            else if (path.E.IndexOf("1DAY") == 0)
                this.TimeInterval = TimeInterval.Daily;
            else if (path.E.IndexOf("1HOUR") == 0)
                this.TimeInterval = TimeInterval.Hourly;
            else if (path.E.IndexOf("1MON") == 0)
                this.TimeInterval = TimeInterval.Monthly;
            else if (path.E.IndexOf("1WEEK") == 0)
                this.TimeInterval = TimeInterval.Weekly;
            else if (path.E.IndexOf("1YEAR") == 0)
                this.TimeInterval = TimeInterval.Yearly;
        }

     

        private string PathToFile
        {
            get { return Path.GetDirectoryName(m_filename); }
        }


        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            Clear();
            string tmpFile = FileUtility.GetTempFileNameInDirectory(PathToFile, ".out"); // temp output file.

            DumpPathToTempFile(tmpFile);
            ReadTempFile(tmpFile,t1,t2);

            File.Delete(tmpFile);
        }

        private void DumpPathToTempFile(string fnOut)
        {
            string fnScript = FileUtility.GetTempFileNameInDirectory(PathToFile, ".txt");

            StreamWriter sw = new StreamWriter(fnScript);

            sw.WriteLine(Path.GetFileName(m_filename));
            sw.WriteLine("CA.NCF");// create (N)ew (C)ondenced catalog  (F)ull batch mode.
            sw.WriteLine("fo f12.3");
            sw.WriteLine("wr.t to=" + Path.GetFileName(fnOut) + " "
              + " A=" + path.A
              + " B=" + path.B
              + " C=" + path.C
                // +" d="+path.D skip part d (dates...) we want all dates..
              + " E=" + path.E
              + " F=" + path.F);


            if (!path.IsValid)
            {
                MessageBox.Show("Warning: Path'"+path.CondensedName+"' contains invalid characters");
            }


            sw.Close();

            string dssutl = GetPathToDssUtl();

            ProgramRunner pr = new ProgramRunner();
            pr.Run(dssutl, "input=" + Path.GetFileName(fnScript), PathToFile);
            pr.WaitForExit();
            string[] stdout = pr.Output ;
            //string[] stdout = HecDssProgramRunner.Run(dssutl, "input=" + fnScript, PathToFile);

            Console.WriteLine("---ReadTimeSeries() ");
            Console.WriteLine(String.Join("\n", stdout));

            File.Delete(fnScript);
        }
        /*
/CBP/BANKS/ELEV/16MAY2005/IR-DAY/OBS/
ITS  Ver:  1   Prog:DSSITS  LW:17JUN05  15:23:48   Tag:T1         Prec:*
Start: 16MAY2005 at 2350 hours;   End: 16MAY2005 at 2400 hours;  Number:    2
Units: FEET        Type: INST-VAL
16MAY2005, 2350;     1568.718
16MAY2005, 2400;     1568.757
END DATA
/CBP/BANKS/ELEV/17MAY2005/IR-DAY/OBS/
ITS  Ver:  1   Prog:DSSITS  LW:17JUN05  15:23:48   Tag:T2         Prec:*
Start: 17MAY2005 at 0010 hours;   End: 17MAY2005 at 2400 hours;  Number:  145
Units: FEET        Type: INST-VAL
17MAY2005, 0010;     1568.705
17MAY2005, 0020;     1568.731
17MAY2005, 0030;     1568.718
17MAY2005, 0040;     1568.654
17MAY2005, 0050;     1568.680
17MAY2005, 0100;     1568.680
17MAY2005, 0110;     1568.628 
 * */



        private void ReadTempFile(string filename, DateTime t1, DateTime t2)
        {

            string strLine;

            Regex regex = new Regex("^(?<day>[0-9]{2})(?<month>[a-zA-Z]{3})(?<year>[0-9]{4}),\\s*(?<hour>[0-9]{2})(?<minute>[0-9]{2});\\s*(?<value>[-]{0,1}[0-9\\.]*)",
              (RegexOptions.Compiled | RegexOptions.Singleline));
            // TO DO capture units and type also.
            //Units: FEET        Type: PER-AVER
            string[] MonthStr = { "", "jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec" };
            TextFile tf = new TextFile(filename);

            EstimateUnits(tf);

            //ts.TimeInterval = //AssignType(type);
            for (int i = 0; i < tf.Length; i++)   //     while( (strLine = sw.ReadLine()) != null )
            {
                strLine = tf[i];
                bool IsMatch = regex.IsMatch(strLine, 0);
                MatchCollection mc = regex.Matches(strLine);
                if (mc.Count == 1) //
                {
                    string strDay = mc[0].Groups["day"].Value;
                    int day = Convert.ToInt32(strDay);

                    string strMonth = mc[0].Groups["month"].Value;
                    int m = Array.IndexOf(MonthStr, strMonth.ToLower());
                    int month = Convert.ToInt32(m);

                    string strYear = mc[0].Groups["year"].Value;
                    int year = Convert.ToInt32(strYear);

                    string strHour = mc[0].Groups["hour"].Value;
                    int hour = Convert.ToInt32(strHour);

                    string strMinute = mc[0].Groups["minute"].Value;
                    int minute = Convert.ToInt32(strMinute);

                    string strValue = mc[0].Groups["value"].Value;
                    double val = Convert.ToDouble(strValue);

                    DateTime date;
                    if (hour == 24) // minute must be zero '00'
                    { // HEC-DSS puts in hour 24 which means midnight:
                        // for PER-AVER this means the end of period.
                        date = new DateTime(year, month, day, 0, 0, 0); // hour =0
                        date = date.AddDays(1); // we are at 12:00 am next day.
                    }
                    else
                    {
                        date = new DateTime(year, month, day, hour, minute, 0);
                    }
                    if (date >= t1 && date <= t2)
                    {
                        string flag = PointFlag.None;
                        if( val == -901.00)
                            flag = PointFlag.Missing;
                        Add(date, val,flag);
                    }
                }
            }

        }

        private void EstimateUnits(TextFile tf)
        {
            //Units: CFS         Type: INST-VAL
            int idx = tf.IndexOf("Units:");// FEET        Type: PER-AVER
            if (idx >= 0)
            {
                int idx2 = tf[idx].IndexOf("Type:");
                if (idx2 > 0)
                {
                    Units = tf[idx].Substring(7, idx2 - idx -7).Trim();
                }
            }
        }

      

        internal static string GetPathToDssUtl()
        {
            String sp = Application.StartupPath;

            if (sp.IndexOf("Visual Studio") >= 0  || sp.IndexOf("NUnit")>=0 )
            {// debugging...

                string exeDir = @"C:\Users\KTarbet\Documents\project\Pisces\Pisces\bin\Debug";
                if (Directory.Exists(exeDir))
                    sp = exeDir;

                exeDir = @"C:\blounsbury\Programming\C#\Karl\Pisces\Pisces\bin\Debug";
                if (Directory.Exists(exeDir))
                    sp = exeDir;
            }

            string dssutl = Path.Combine(sp, "dssutl.exe");
            return dssutl;
        }


    }
}
