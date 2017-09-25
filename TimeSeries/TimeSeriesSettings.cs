using System;
using System.Data;

namespace Reclamation.TimeSeries
{


    public class TimeSeriesSettings
    {

        public MonthDayRange Range;
        //public bool FullPeriodOfRecord; // depends on TimeSeriesDatabase

        public TimeSeriesDatabaseList DatabaseList;
        public bool FullPeriodOfRecord;
        public bool AllowFullPeriodOfRecord;
        public AggregateType AggregateOption;
        public int WaterYearBeginningMonth;
        public int[] WaterYears;
        public int[] ExceedanceLevels;
        public bool ExceedanceAlsoPlotYear;
        public int ExceedancePlotYear;
        public bool ExceedancePlotMax;
        public bool ExceedancePlotMin;
        public AnalysisType AnalysisType;
        public string[] SelectedScenarios;
        public DataTable SelectedSites;
        public DateTime DateTime1;
        public DateTime DateTime2;

        public TimeSeriesSettings()
        {
            Defaults();
        }

        private void Defaults()
        {
            DatabaseList = new TimeSeriesDatabaseList();
            FullPeriodOfRecord = false;
            Range = new MonthDayRange(10, 1, 9, 30);
            this.AggregateOption = AggregateType.None;
            WaterYearBeginningMonth = 10;// october
            WaterYears = new int[] { 2001, 2002, 2003, 2004, 2005, 2006 };
            ExceedanceLevels = new int[] { 10, 50, 90 };
            ExceedanceAlsoPlotYear = false;
            ExceedancePlotYear = 2006;
            ExceedancePlotMax = false;
            ExceedancePlotMin = false;
            AnalysisType = AnalysisType.TimeSeries;

        }

        public SeriesList CreateSelectedSeries(string[] selectedScenarios,
    DataTable tblSelection)
        {

            TimeSeries.SeriesList rval = new TimeSeries.SeriesList();


            if (tblSelection == null || tblSelection.Rows.Count == 0)
            {
                return rval;
            }

            DataTable selected = EnumerateSelections(tblSelection, selectedScenarios);

            for (int i = 0; i < selected.Rows.Count; i++)
            {
                DataRow row = selected.Rows[i];
                string dataSource = row["DataSource"].ToString().Trim();

                if (dataSource == "")
                {
                    continue;
                }

                int idx = DatabaseList.ScenarioNames.IndexOf(dataSource);


                if (idx < 0)
                {
                    throw new NotSupportedException(dataSource + " is not defined in the config file.\nPlease check your tree.csv and connection strings in your config file");
                }

                if (!DatabaseList[idx].ValidRow(row))
                {
                    continue;
                }
                TimeSeries.Series s = DatabaseList[idx].CreateSeries(row);
                s.ScenarioName = dataSource;
                //Console.WriteLine("s.ScenarioName "+ s.ScenarioName);
                rval.Add(s);
            }
            return rval;
        }
        /// <summary>
        /// creates a single list of selections by combining scenario selections
        /// with tree selections.
        /// </summary>
        /// <param name="tblSelection"></param>
        /// <param name="selectedScenarios"></param>
        /// <returns></returns>
        private static DataTable EnumerateSelections(DataTable tblSelection,
                                string[] selectedScenarios)
        {

            DataTable tbl = tblSelection.Copy();

            for (int i = 0; i < tblSelection.Rows.Count; i++)
            {
                DataRow row = tbl.Rows[i];

                if (row["DataSource"] == DBNull.Value || row["DataSource"].ToString().Trim() == "")
                {// data source not specified in tree.  Use scenario selections
                    for (int s = 0; s < selectedScenarios.Length; s++)
                    {
                        if (s > 0)
                        {// need new row.
                            row = tbl.NewRow();
                            tbl.Rows.Add(row);
                            for (int c = 0; c < tbl.Columns.Count; c++)
                            {
                                row[c] = tbl.Rows[i][c];
                            }
                        }
                        row["DataSource"] = selectedScenarios[s];
                    }
                }
            }

            return tbl;
        }


    }
}
