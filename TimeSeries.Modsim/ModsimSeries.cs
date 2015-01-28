using System;
using System.Data;
using System.Text;
using Reclamation;
using Reclamation.Core;
using Reclamation.TimeSeries;
using Csu.Modsim.ModsimIO;
using Csu.Modsim.ModsimModel;
using System.IO;

namespace Reclamation.TimeSeries.Modsim
{


    /// <summary>
    /// Reads modsim version 8.0 - 8.4 input and output
    /// ModsimSeries is a wrapper around a Modsim.TimeSeries.  
    /// Has the ability to scale data and display data in different units than the xy file has. 
    /// You can edit using cfs but save in acre-feet.
    /// </summary>
    public class ModsimSeries : Reclamation.TimeSeries.Series
    {
        //private Reclamation.TimeSeries.Series rawModsimSeries;// has direct reference to modsim time series DataTable
        Model m_mi;
        string m_databaseName="";
        string m_mdbFilename;
        string m_accdbFilename;
        string m_xyFilename;
        Version m_xyFileVersion;
        DataTable m_outputTablesInfo;
        private string modsimName;
        private string timeSeriesName;
        private bool _isReservoir = false;
        private string m_defaultUnits = "acre-feet";
        private Csu.Modsim.ModsimModel.TimeSeries _modsimSeries;
        private bool _isOutput = true;
        private bool IsOutput
        {
            get { return _isOutput; }
        }
        internal bool IsReservoir
        {
            get { return _isReservoir; }
        }


        public ModsimSeries(string xyFileName, string modsimName, string timeSeriesName)
        {
            m_xyFilename = xyFileName;
            Init(m_xyFilename, modsimName, timeSeriesName);
        }


        public static bool DisplayFlowInCfs { get; set; }


        /// <summary>
        /// Creates ModsimSeries independent of TimeSeriesDatabase
        /// Used to add ModsimSeries to PiscesTree (TimeSeriesDatabase)
        /// </summary>
        public ModsimSeries(Model mi, string modsimName, string timeSeriesName)
        {
            if (mi == null)
            {
                throw new ArgumentNullException("Model mi is null");
            }
            m_mi = mi;
            m_xyFilename = mi.fname;
            TimeInterval = GetSeriesType(mi); // time step
            Init(m_xyFilename, modsimName, timeSeriesName);
        }

        private void Init(string xyFileName, string modsimName, string timeSeriesName)
        {
            int idx = timeSeriesName.ToLower().IndexOf("input.");
            string t = timeSeriesName;
            if (idx == 0) // display a MODSIM input data timeseries (ie adaTargetsM)
            {
                t = timeSeriesName.Substring(idx + 6);
            }

            this.Units = "acre-feet";
            if (timeSeriesName == "Elev_End")
            {
                Units = "feet";//TO DO.. more units.. or look in mdb file for units.
            }
            else
                if (timeSeriesName == "Hydro_State_Res" || timeSeriesName == "Hydro_State_Dem")
                {
                    Units = "";
                }
            m_defaultUnits = Units; // might get changed to cfs..

            this.modsimName = modsimName;  // node or link name
            this.timeSeriesName = t;
            SiteName = modsimName;
            ScenarioName = Path.GetFileNameWithoutExtension(xyFileName);
            this.Appearance.LegendText = Name;
            m_mdbFilename = Path.ChangeExtension(m_xyFilename, null) + "OUTPUT.mdb";
            m_accdbFilename = Path.ChangeExtension(m_xyFilename, null) + "OUTPUT.accdb";

            if (File.Exists(m_accdbFilename))
            {
                m_databaseName = m_accdbFilename;
            }
            else
                m_databaseName = m_mdbFilename;

            StreamReader sr = File.OpenText(m_xyFilename);
            string line1 = sr.ReadLine();
            string[] line1Parts = line1.Split(' ');
            m_xyFileVersion = new Version(line1Parts[1]);
            sr.Close();

            ConnectionString = "FileName=" + m_xyFilename + ";ModsimName=" + modsimName
                + ";TimeSeriesName=" + timeSeriesName;// +";DisplayUnits=" + displayUnits;
            this.Provider = "ModsimSeries";
            ReadOnly = true;
        }



        /// <summary>
        /// Used to create ModsimSeries from TimeSeriesDatabase
        /// </summary>
        public ModsimSeries(TimeSeriesDatabase db, Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow sr)
            : base(db, sr)
        {
            m_defaultUnits = Units;
            m_xyFilename = ConnectionStringUtility.GetFileName(ConnectionString, db.DataSource);
            m_mdbFilename = Path.ChangeExtension(m_xyFilename, null) + "OUTPUT.mdb";
            m_accdbFilename = Path.ChangeExtension(m_xyFilename, null) + "OUTPUT.accdb";
            if (File.Exists(m_accdbFilename))
            {
                m_databaseName = m_accdbFilename;
            }
            else
                m_databaseName = m_mdbFilename;

            if (File.Exists(m_xyFilename))
            {
                StreamReader srr = File.OpenText(m_xyFilename);
                string line1 = srr.ReadLine();
                string[] line1Parts = line1.Split(' ');
                m_xyFileVersion = new Version(line1Parts[1]);
                srr.Close();
            }
            else
            {
                Logger.WriteLine("Error: File missing " + m_xyFilename);
            }
            ScenarioName = Path.GetFileNameWithoutExtension(m_xyFilename);
            ExternalDataSource = true;
            modsimName = ConnectionStringToken("ModsimName");
            timeSeriesName = ConnectionStringToken("TimeSeriesName");
            ReadOnly = true;
        }


        /// <summary>
        /// Crates Scenario based on scenaroName as xyfile name without extension (.xy)
        /// </summary>
        public override Series CreateScenario(TimeSeriesDatabaseDataSet.ScenarioRow scenario)
        {
            string fn = ConnectionStringUtility.GetFileName(scenario.Path, m_db.DataSource);
            if (fn == m_xyFilename)
            {
                return this;
            }

            Logger.WriteLine("Reading series from: '" + fn + "'");
            if (!File.Exists(fn))
            {
                Logger.WriteLine("File not found: '" + fn + "'");
                throw new FileNotFoundException();
                //Logger.WriteLine("Error: Can't create scenario");
                //return new Series();
            }

            var rval = new ModsimSeries(fn, modsimName, timeSeriesName);
            rval.Name = this.Name;
            rval.Appearance.LegendText = scenario.Name + " " + Name;
            rval.ScenarioName = scenario.Name;
            rval.SiteName = this.SiteName;
            rval.TimeInterval = this.TimeInterval;
            return rval;
        }
        private new void Insert(Reclamation.TimeSeries.Series series, bool overWrite)
        {
            for (int i = 0; i < series.Count; i++)
            {
                Insert(series[i], overWrite);
            }
            if (Count > 0)
            {//modsim needs dataStartDate as the first entry.
                DateTime dataStartDate = m_mi.TimeStepManager.dataStartDate;
                if (this[0].DateTime != dataStartDate)
                {
                    this.Insert(new Reclamation.TimeSeries.Point(dataStartDate, -999));

                    if (this[0].DateTime != dataStartDate)
                    {
                        throw new Exception("Internal error.");
                    }
                }
            }
            DateTime dataEndDate = m_mi.TimeStepManager.dataEndDate;
            if (series.Count > 0 && series.MaxDateTime > dataEndDate)
            {
                Logger.WriteLine("extending modsim date range");
                m_mi.TimeStepManager.dataEndDate = series.MaxDateTime.AddHours(23).AddMinutes(59);
                m_mi.TimeStepManager.UpdateTimeStepsInfo(m_mi.timeStep);
            }
        }

        public override void Clear()
        {
            base.Clear();
            m_mi = null;
        }

        protected override void ReadCore()
        {
            ReadFromModsim(TimeSeriesDatabase.MinDateTime, TimeSeriesDatabase.MaxDateTime);
        }
        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            ReadFromModsim(t1, t2);
        }

        private void ReadFromModsim(DateTime t1, DateTime t2)
        {
            Units = m_defaultUnits;
            if (IsInputDataType())
                Logger.WriteLine("Reading: Modsim " + modsimName + " " + timeSeriesName + " in " + m_xyFilename);
            else
                Logger.WriteLine("Reading: Modsim " + modsimName + " " + timeSeriesName + " in " + m_databaseName);

            if (!File.Exists(m_databaseName))
            {
                Logger.WriteLine("File not found: '" + m_databaseName + "'");
                throw new FileNotFoundException();
            }

            // reads from modsim input (.xy) or output (.mdb) depending on timeSeriesName
            if (IsInputDataType())
            {
                if (m_mi == null)
                {
                    m_mi = new Model();
                    m_mi.fname = m_xyFilename;
                }

                XYFileReader.Read(m_mi, m_mi.fname);
                ModsimInput(modsimName,t1,t2);//, timeSeriesName);
            }
            else
            {
                if (File.Exists(m_databaseName))
                {
                    ReadModsimOutput(modsimName, timeSeriesName, t1, t2);
                }
            }
        }

        /// <summary>
        /// reads MODSIM output time series
        /// </summary>
        private void ReadModsimOutput(string modsimName, string columnName, DateTime t1, DateTime t2)
        {
            string tableName = ModsimTableName(columnName);

            if (String.Compare(tableName, "LinksOutput", true) == 0)
            {
                LinkOutput(modsimName, columnName, t1, t2);
            }
            else
                if (String.Compare(tableName, "DemOutput", true) == 0)
                {
                    DemandOutput(modsimName, columnName, t1, t2);
                }
                else
                    if (String.Compare(tableName, "RES_STOROutput", true) == 0)
                    {
                        _isReservoir = true;
                        ReservoirStorageOutput(modsimName, columnName, t1, t2);
                    }
                    else
                        if (String.Compare(tableName, "RESOutput", true) == 0)
                        {
                            _isReservoir = true;
                            ReservoirOutput(modsimName, columnName, t1, t2);
                        }
            this.Appearance.NumberFormat = "F0";
            if (String.Compare(columnName, "Elev_End", true) == 0)
            {
                this.Appearance.NumberFormat = "F2";
            }

            //            Name = columnName;
            //SiteName = modsimName;
            Appearance.LegendText = SiteName + " " + Name;
            ApplyDisplayScaling();
            SetStairStep(timeSeriesName);
        }

        void SetStairStep(string timeSeriesName)
        {
            Appearance.StairStep = !UseStairStep(timeSeriesName);
        }

        //void ShiftTimeStepsForward()
        //{
        //    DateTime currentDate;
        //    DataTable dt = Table.Clone();
        //    DataRow row;
        //    for (int i = 0; i < Table.Rows.Count; i++)
        //    {
        //        currentDate = Convert.ToDateTime(Table.Rows[i][0]);
        //        row = dt.NewRow();
        //        row[0] = modsim.mi.TimeStepManager.IncrementIniDate(currentDate);
        //        row[1] = Table.Rows[i][1];
        //        dt.Rows.Add(row);
        //    }
        //    Table = dt.Copy();
        //}
        static Boolean UseStairStep(string timeSeriesName)
        {
            if (timeSeriesName.IndexOf("adaTargetsM") >= 0) return true;
            if (timeSeriesName.IndexOf("StorLeft") >= 0) return true;
            if (timeSeriesName.IndexOf("Accrual") >= 0) return true;
            if (timeSeriesName.IndexOf("Stor_End") >= 0) return true;
            if (timeSeriesName.IndexOf("Stor_Beg") >= 0) return true;
            if (timeSeriesName.IndexOf("Stor_Trg") >= 0) return true;
            if (timeSeriesName.IndexOf("Elev_End") >= 0) return true;
            return false;
        }


        /// <summary>
        /// returns MODSIM Reservoir Storage output
        /// </summary>
        private void ReservoirStorageOutput(string modsimName, string columnName, DateTime t1, DateTime t2)
        {
            // using EndDate -- karl
            string sql = "SELECT TimeSteps.TSDate, RES_STOROutput." + columnName //+ SQLMultiplyByScale(columnName)
                    + " FROM (TimeSteps INNER JOIN RES_STOROutput ON TimeSteps.TSIndex = RES_STOROutput.TSIndex) "
                    + " INNER JOIN NodesInfo ON RES_STOROutput.NNo = NodesInfo.NNumber "
                    + " where NodesInfo.NName = '" + modsimName + "'  AND"
                      + GetDateClause(t1, t2);

            if (modsimName.IndexOf(",") >= 0)
            {// sum reservoir group together.

                string where = "( '" + modsimName.Replace(",", "','") + "') ";

                sql = "SELECT TimeSteps.TSDate, Sum(RES_STOROutput." + columnName + " ) AS " + columnName
                + " FROM TimeSteps INNER JOIN (RES_STOROutput INNER JOIN NodesInfo ON RES_STOROutput.NNo = NodesInfo.NNumber) "
                + " ON TimeSteps.TSIndex = RES_STOROutput.TSIndex "
                + " WHERE  " + GetDateClause(t1, t2)
                + "  AND  (NodesInfo.NName   in " + where + " ) "
                + " GROUP BY TimeSteps.TSDate ";
            }

            DataTable tbl = AccessDB.Table(m_databaseName, "RES_STOROutput", sql);
            InitTimeSeries(tbl, Units, this.TimeInterval, true);
        }

        /// returns MODSIM Reservoir (other than storage) output
        /// </summary>
        private void ReservoirOutput(string modsimName, string columnName, DateTime t1, DateTime t2)
        {
            if (columnName.Contains("Hydro_State"))
            {
                columnName = "Hydro_State";
            }
            // assumes date should be MidDate --Leslie
            /* BLounsbury - Why MidDate? - Changing to tsdate to match flow/demand/etc */
            string sql = "SELECT TimeSteps.TsDate, RESOutput." + columnName //+ SQLMultiplyByScale(columnName)
                    + " FROM (TimeSteps INNER JOIN RESOutput ON TimeSteps.TSIndex = RESOutput.TSIndex) "
                    + " INNER JOIN NodesInfo ON RESOutput.NNo = NodesInfo.NNumber "
                    + " where NodesInfo.NName = '" + modsimName + "' AND "
                      + GetDateClause(t1, t2);

            DataTable tbl = AccessDB.Table(m_databaseName, "RESOutput", sql);
            InitTimeSeries(tbl, Units, TimeInterval, true);
        }

        /// <summary>

        /// <summary>
        /// returns MODSIM Demand output
        /// </summary>
        private void DemandOutput(string modsimName, string columnName, DateTime t1, DateTime t2)
        {
            if (columnName.Contains("Hydro_State"))
            {
                columnName = "Hydro_State";
            }
            string sql = "SELECT TimeSteps.TsDate, DEMOutput." + columnName //+ SQLMultiplyByScale(columnName)
                           + " FROM (TimeSteps INNER JOIN DEMOutput ON TimeSteps.TSIndex = DEMOutput.TSIndex) "
                           + " INNER JOIN NodesInfo ON DEMOutput.NNo = NodesInfo.NNumber "
                           + " where NodesInfo.NName = '" + modsimName + "' AND "
                             + GetDateClause(t1, t2);


            if (modsimName.IndexOf(",") >= 0)
            {// sum demand group together.

                string where = "( '" + modsimName.Replace(",", "','") + "') ";

                sql = "SELECT TimeSteps.TSDate, Sum(DEMOutput." + columnName + " ) AS " + columnName
                + " FROM TimeSteps INNER JOIN (DEMOutput INNER JOIN NodesInfo ON DEMOutput.NNo = NodesInfo.NNumber) "
                + " ON TimeSteps.TSIndex = DEMOutput.TSIndex "
                + " WHERE  " + GetDateClause(t1, t2)
                + "  AND  (NodesInfo.NName   in " + where + " ) "
                + " GROUP BY TimeSteps.TSDate ";
            }

            DataTable tbl = AccessDB.Table(m_databaseName, "DEMOutput", sql);
            InitTimeSeries(tbl, Units, TimeInterval, true);
        }

        private string GetDateClause(DateTime t1, DateTime t2)
        {
            string rval = " (TimeSteps.TsDate>= #" + t1.Date.ToString("MM/dd/yyyy") + "#) AND (TimeSteps.EndDate<= #" + t2.Date.AddHours(23).AddMinutes(59).AddSeconds(59.9).ToString("MM/dd/yyyy HH:mm:ss") + "#) ";

            return rval;
        }

        /// <summary>
        /// returns MODSIM Link output
        /// </summary>
        private void LinkOutput(string modsimName, string columnName, DateTime t1, DateTime t2)
        {
            string sql = " SELECT TimeSteps.TsDate, LinksOutput." + columnName //+ SQLMultiplyByScale(columnName)
                    + " FROM (LinksOutput INNER JOIN TimeSteps ON LinksOutput.TSIndex = TimeSteps.TSIndex) "
                    + " INNER JOIN LinksInfo ON LinksOutput.LNumber = LinksInfo.LNumber "
                    + " where LinksInfo.LName = '" + modsimName + "'";
            if (!FullPeriod(t1, t2)) // performance improvement when using full period of record 
            {
                sql += " AND " + GetDateClause(t1, t2);
            }

            if (modsimName.IndexOf(",") >= 0)
            {// sum link group together.

                string where = "( '" + modsimName.Replace(",", "','") + "') ";

                sql = "SELECT TimeSteps.TSDate, Sum(LinksOutput." + columnName + " ) AS " + columnName
                + " FROM TimeSteps INNER JOIN (LinksOutput INNER JOIN LinksInfo ON LinksOutput.LNumber = LinksInfo.LNumber) "
                + " ON TimeSteps.TSIndex = LinksOutput.TSIndex "
                + " WHERE  " + GetDateClause(t1, t2)
                + "  AND  (LinksInfo.LName   in " + where + " ) "
                + " GROUP BY TimeSteps.TSDate ";
            }

            Performance perf = new Performance();
            DataTable tbl = AccessDB.Table(m_databaseName, "LinksOutput", sql);
            perf.Report();
            InitTimeSeries(tbl, Units, TimeInterval, true);
        }

        private bool FullPeriod(DateTime t1, DateTime t2)
        {
            if (t1 == TimeSeriesDatabase.MinDateTime
                && t2 == TimeSeriesDatabase.MaxDateTime)
                return true;

            return false;
        }



        /// <summary>
        /// given a MODSIM output column name  (in the access database)
        /// returns what table to read in modsim.
        /// </summary>
        private string ModsimTableName(string columnName)
        {
            string tableName;
            if (columnName == "Hydro_State_Res")
            {
                return tableName = "RESOutput";
            }
            if (columnName == "Hydro_State_Dem")
            {
                return tableName = "DemOutput";
            }

            if (m_outputTablesInfo == null)
            {
                m_outputTablesInfo = AccessDB.ReadTable(m_databaseName, "OutputTablesInfo");
            }
            string sql = "OutputName = '" + columnName + "'";
            DataTable tbl1 = DataTableUtility.Select(m_outputTablesInfo, sql, "");

            if (tbl1.Rows.Count == 0)
            {
                throw new Exception("Error: A column named '" + columnName + "' could not be found in the modsim output file: " + this.m_databaseName);
            }
            
            tableName = tbl1.Rows[0]["Object"].ToString();
            Console.WriteLine(tableName);
            Logger.WriteLine("tableName = " + tableName);
            return tableName;
        }

        /// <summary>
        /// Converts from modsim Series Type to Karls TimeSeries.SeriesType
        /// </summary>
        internal static Reclamation.TimeSeries.TimeInterval GetSeriesType(Csu.Modsim.ModsimModel.Model mi)
        {
            Reclamation.TimeSeries.TimeInterval type = Reclamation.TimeSeries.TimeInterval.Monthly;
            // this is not exactly right. This could be specific for each series.
            if (mi.timeStep.TSType == Csu.Modsim.ModsimModel.ModsimTimeStepType.Daily)
            {
                type = Reclamation.TimeSeries.TimeInterval.Daily;
            }
            else
                if (mi.timeStep.TSType == Csu.Modsim.ModsimModel.ModsimTimeStepType.Monthly)
                {
                    type = Reclamation.TimeSeries.TimeInterval.Monthly;
                }
                else
                    if (mi.timeStep.TSType == Csu.Modsim.ModsimModel.ModsimTimeStepType.Weekly)
                    {
                        type = Reclamation.TimeSeries.TimeInterval.Weekly;
                    }
            return type;
        }

        /// <summary>
        /// determines if timeSeriesName string is a output type.
        /// Example  IsInputDataType("adaInflowsM") is true
        /// Example  IsInputDataType("FLOW") is false
        /// </summary>
        private bool IsInputDataType() //(string timeSeriesName)
        {
            string[] supported = {"adaDemandsM","adaEvaporationsM", "adaForcastsM",
                "adaGeneratingHrsM", "adaInfiltrationsM", "adaInflowsM", "adaTargetsM",  };
            if (Array.IndexOf(supported, timeSeriesName) >= 0)
                return true;
            return false;
        }

        /// <summary>
        /// Reads Modsim Input time series data
        /// </summary>
        private void ModsimInput(string modsimName, DateTime t1, DateTime t2/*, string timeSeriesName*/)
        {
            try
            {
                if (modsimName.IndexOf(",") >= 0)
                {
                    Series total = new Series();
                    string[] nodes = modsimName.Split(',');
                    for (int i = 0; i < nodes.Length; i++)
                    {
                        Csu.Modsim.ModsimModel.TimeSeries mts = FindModsimTimeSeries(nodes[i]);
                        DataTable mtbl = mts.GetTable();

                        var s = new Reclamation.TimeSeries.Series(mtbl, Units, TimeInterval);
                        s.ReadOnly = true;
                        s.HasFlags = false;

                        ModsimUtilities.FillModsimStyle(m_mi, s, m_mi.TimeStepManager.dataStartDate, 
                                                        m_mi.TimeStepManager.dataEndDate);
                        //DataTable publicTable = CopyModsimDataTableConvertingIntToDouble(rawModsimTable);
                        if (i == 0)
                        {
                            total = s;
                        }
                        else
                        {
                            if (total.Count != s.Count)
                            {
                                string msg = "the node '" + nodes[i] + "' is of different length than " + nodes[i-1]+ " cannot add";
                                Console.WriteLine(msg);
                                Logger.WriteLine(msg);
                                this.Appearance.LegendText = msg;
                                throw new System.DataMisalignedException(msg);
                            }
                            total = total + s;
                        }
                        
                    }
                    // TO-DO: Use Pisces t1 & t2
                    //int timeStartIndex = total.IndexOf(t1);
                    //int timeEndIndex = total.IndexOf(t2)+1;
                    //int modelEndIndex = total.IndexOf(m_mi.TimeStepManager.dataEndDate);
                    //for (int i = 0; i < timeStartIndex; i++)
                    //{
                    //    total.RemoveAt(i);
                    //}
                    //for (int i = timeEndIndex; i <= modelEndIndex; i++)
                    //{
                    //    total.RemoveAt(i);
                    //}
                    InitTimeSeries(total.Table, Units, TimeInterval, true);
                }

                else
                {
                    Csu.Modsim.ModsimModel.TimeSeries ts = FindModsimTimeSeries(modsimName);

                    if (ts != null)
                    {
                        _modsimSeries = ts;
                       // Name = timeSeriesName;
                       // SiteName = modsimName;
                        //CheckVariesByYear();
                        DataTable rawModsimTable = ts.GetTable();
                        var s = new Reclamation.TimeSeries.Series(rawModsimTable, Units, TimeInterval);
                        s.HasFlags = false;
                        s.ReadOnly = true;
                        ModsimUtilities.FillModsimStyle(m_mi, s, m_mi.TimeStepManager.dataStartDate, 
                                                        m_mi.TimeStepManager.dataEndDate);
                        //DataTable publicTable = CopyModsimDataTableConvertingIntToDouble(rawModsimTable);
                        
                        // TO-DO: Use Pisces t1 & t2
                        //int timeStartIndex = s.LookupIndex(t1);
                        //int timeEndIndex = s.LookupIndex(t2,false) + 1;
                        //var a = m_mi.TimeStepManager.Date2Index(m_mi.TimeStepManager.dataEndDate,"TypeIndex????");
                        //int modelEndIndex = s.LookupIndex(m_mi.TimeStepManager.endingDate);
                        //for (int i = 0; i < timeStartIndex; i++)
                        //{
                        //    s.RemoveAt(i);
                        //}
                        //for (int i = timeEndIndex; i <= modelEndIndex; i++)
                        //{
                        //    s.RemoveAt(i);
                        //}
                        InitTimeSeries(s.Table, Units, TimeInterval,true);
                    }
                }
                
                ApplyDisplayScaling();
                // publicTable.RowChanged += new DataRowChangeEventHandler(tbl_RowChanged);
                this.Table.Columns[1].ColumnName = modsimName + " " + timeSeriesName;
                Appearance = new Reclamation.TimeSeries.TimeSeriesAppearance();
            }
            catch (Exception ex)
            {
                string msg = "Error reading xy file data " + Name + " " + m_mi.fname + "\n" + ex.Message + "\n"
                + ex.StackTrace;
                throw new Exception(msg);
            }
        }

        private void ApplyDisplayScaling()
        {

            string[] flowColumnNames = { "Flow", "NaturalFlow", "Surf_In", "Gw_in", "Demand", "Shortage" };

            if (Array.IndexOf(flowColumnNames, this.timeSeriesName) < 0)
                return; // don't scale if this isn't a flow

            //if (modsim.scale != 1)
            //{
            //    Reclamation.TimeSeries.Math.Multiply(this, modsim.scale);
            //}

            if (DisplayFlowInCfs)
            {
                Reclamation.TimeSeries.Math.ConvertUnits(this, "cfs");
            }
            //string lowerUnits = displayUnits.ToLower();
            //if (lowerUnits == "feet") Units = "Feet";
            //if (lowerUnits.Contains("kw")) Units = "Avg KW";
        }


        private Csu.Modsim.ModsimModel.TimeSeries FindModsimTimeSeries(string modsimName)
        {
            _isOutput = false;
            Csu.Modsim.ModsimModel.TimeSeries ts = null;

            Csu.Modsim.ModsimModel.Node n = m_mi.FindNode(modsimName);

            if (n == null)
            {
                string msg = "the node '" + modsimName + "' does not exist in " + m_mi.fname;
                Console.WriteLine(msg);
                Logger.WriteLine(msg);
                this.Appearance.LegendText = msg;
                return ts;
            }
            switch (timeSeriesName)
            {
                case "adaDemandsM":
                    ts = n.m.adaDemandsM;
                    break;
                case "adaEvaporationsM":
                    ts = n.m.adaEvaporationsM;
                    _isReservoir = true;
                    break;
                case "adaForecastsM":
                    ts = n.m.adaForecastsM;
                    _isReservoir = true;
                    break;
                case "adaGeneratingHrsM":
                    ts = n.m.adaGeneratingHrsM;
                    _isReservoir = true;
                    break;
                case "adaInfiltrationsM":
                    ts = n.m.adaInfiltrationsM;
                    break;
                case "adaInflowsM":
                    ts = n.m.adaInflowsM;
                    break;
                case "adaTargetsM":
                    ts = n.m.adaTargetsM;
                    _isReservoir = true;
                    break;

                default:
                    string msg = "the timeSeriesName '" + timeSeriesName + "' is not implemented";
                    Console.WriteLine(msg);
                    Logger.WriteLine(msg);
                    throw new NotImplementedException(msg);
            }
            return ts;
        }
        private DataTable CopyModsimDataTableConvertingIntToDouble(DataTable rawModsimTable)
        {
            DataTable t = rawModsimTable.Clone();

            for (int i = 0; i < t.Columns.Count; i++)
            {
                DataColumn c = t.Columns[i];
                if (c.DataType == typeof(int))
                {
                    c.DataType = typeof(double);
                }
            }

            for (int row = 0; row < rawModsimTable.Rows.Count; row++)
            {
                DataRow newRow = t.NewRow();
                for (int c = 0; c < t.Columns.Count; c++)
                {
                    newRow[c] = rawModsimTable.Rows[row][c];
                }
                t.Rows.Add(newRow);
            }

            return t;
        }

        //private void CheckVariesByYear()
        //{
        //    if (!_isOutput && _modsimSeries.VariesByYear == false)
        //    {
        //        string text = "you cannnot transfer data to/from " + base.Name + " " + m_mi.fname + " because VariesByYear = false";
        //        text += "\n Would you like to change VariesbyYear to True ? ";
        //        //   if (System.Windows.Forms.MessageBox.Show(text, "Change VariesByYear to True ?", System.Windows.Forms.MessageBoxButtons.YesNo)
        //        //       == System.Windows.Forms.DialogResult.Yes)
        //        {
        //            _modsimSeries.VariesByYear = true;
        //        }
        //    }
        //}

        /// <summary>
        /// when row changes propogate change to underlying Modsim DataTable
        /// using appropriate scaling factor.
        /// </summary>
        //void tbl_RowChanged(object sender, DataRowChangeEventArgs e)
        //{
        //    DataRow r = e.Row;
        //    double d = Convert.ToDouble(r[1]);
        //    DateTime date = Convert.ToDateTime(r[0]);

        //    double scale = 1.0 / ScaleFactor(date);
        //    if (e.Action == DataRowAction.Change)
        //    {
        //        //Console.WriteLine("changed value");
        //        int idxRaw = rawModsimSeries.LookupIndex(date);
        //        int idxDate = this.LookupIndex(date);
        //        rawModsimSeries[idxRaw] = this[idxDate] * scale;
        //    }
        //    else if (e.Action == DataRowAction.Add)
        //    {
        //        rawModsimSeries.Add(date, d * scale);
        //    }
        //    else
        //    {
        //        throw new Exception("The method or operation is not implemented for action " + e.Action);
        //    }
        //}

        //private double ScaleFactor(DateTime date)
        //{
        //    double rval = 1.0;

        //    string displayUnits = Units;
        //    if (DisplayFlowInCfs)
        //        displayUnits = "cfs";
        //    rval = rval * Reclamation.TimeSeries.Math.ConvertUnitsFactor(rawModsimSeries.TimeInterval, rawModsimSeries.Units, displayUnits, date);

        //    return rval;

        //}


        /// <summary>
        /// Gets the UNC path for a directory path passed to it.  empty string if folder isn't shared
        /// 
        /// </summary>
        /// <param name="directory">The local path</param>
        /// <param name="unc">The UNC path</param>
        /// <returns>True if UNC is valid, false otherwise</returns>
        public static bool GetUncFromDirectory(string directory, out string unc)
        {
            if((directory == null) || (directory==""))
            {
                unc = "";
                throw new ArgumentNullException("local");
            }


            //ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT Name FROM Win32_share WHERE path ='" + directory.Replace("\\","\\\\") + "'");
            //ManagementObjectCollection coll = searcher.Get();
            //if (coll.Count == 1)
            //{
            //    foreach (ManagementObject share in searcher.Get())
            //    {
            //        unc = share["Name"] as String;
            //        unc = "\\\\" + SystemInformation.ComputerName + "\\" + unc;
            //        return true;
            //    }
            //}
            unc = "";
            return false;
        }
    }
}
