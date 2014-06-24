using Reclamation.TimeSeries;
using System.Text.RegularExpressions;
using Reclamation.TimeSeries.Excel;
using System;
using System.IO;
using Reclamation.Core;
using System.Configuration;
namespace Reclamation.TimeSeries
{
    /// <summary>
    /// A UrgwonSeries data comes from and excel file.
    /// The filename specifices the month and percent level for example
    /// SHARP_MRM_Forecast-Mar-10pct.xlsx
    /// difference traces (scenarios) are specified by the sheet name (Run0,Run1,...)
    /// </summary>
    class UrgwomSeries:Series
    {
        private string xlsFilename;
        string sheetName;
        string dateColumn;
        string valueColumn;

        public UrgwomSeries(TimeSeriesDatabase db, TimeSeriesDatabaseDataSet.SeriesCatalogRow sr)
            : base(db, sr)
        {
            TimeInterval = TimeSeries.TimeInterval.Daily;
            ExternalDataSource = true;
            ReadOnly = true;
            Provider = "UrgwomSeries";
            ScenarioName = "ScenarioName";
            xlsFilename = ConnectionStringUtility.GetFileName(ConnectionString, m_db.DataSource);
            sheetName = ConnectionStringToken("SheetName");
            dateColumn = ConnectionStringToken("DateColumn");
            valueColumn = ConnectionStringToken("ValueColumn");

        }

        public UrgwomSeries(string xlsFilename, string runName, string slotName)
        {
            TimeInterval = TimeSeries.TimeInterval.Daily;
            ExternalDataSource = true;
            ReadOnly = true;
            Provider = "UrgwomSeries";
            this.xlsFilename = xlsFilename; // "20120403_SHARP_MRM_Forecast-Mar-50pct.xlsx"
            SiteName = slotName;
            sheetName = runName;
            valueColumn = slotName;
            dateColumn  ="A"; // dateTime is in the first column

            this.ConnectionString = "FileName=" + xlsFilename
               + ";SheetName=" + sheetName + ";DateColumn=" + dateColumn
               + ";ValueColumn=" + valueColumn;

            Units = EstimateUnits(slotName);
            //ConnectionString = ConnectionStringUtility.MakeFileNameRelative(ConnectionString, m_db.Filename);
           
        }

        private string EstimateUnits(string parameterName)
        {
            //Cochiti^RioGrandeConservation.Outflow (example ParameterName)
            string slotName = "";
            var tokens = parameterName.Split('.');
            if( tokens.Length != 2)
                return "";

            string[] cfsList = {"Diversion","Diversion Request"
,"Diversion Requested"
,"Gage Outflow"
,"Inflow"
,"Outflow"
,"Total Diversion"};
string [] afList = {"Storage","UsableStorage"};

            if( Array.IndexOf(cfsList,slotName)>=0)
                return "cfs";
            if( Array.IndexOf(afList,slotName)>=0)
                return "acre-feet";
        return "";
        }

         protected override void ReadCore()
        {
            ReadFromFile(TimeSeriesDatabase.MinDateTime, TimeSeriesDatabase.MaxDateTime);
        }

        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            ReadFromFile(t1, t2);   
        }

        private void ReadFromFile(DateTime t1, DateTime t2)
        {
            this.Clear();

            if (!File.Exists(xlsFilename))
            {
                Logger.WriteLine("Missing file:" + xlsFilename, "ui");
            }
            var s = new ExcelDataReaderSeries(xlsFilename, sheetName, dateColumn, valueColumn);
            s.TimeInterval = Reclamation.TimeSeries.TimeInterval.Daily;
            s.Read(t1, t2);

            Logger.WriteLine("Read " + s.Count + " items from " + xlsFilename);
            this.Add(s);

        }
        public override Series CreateScenario(TimeSeriesDatabaseDataSet.ScenarioRow scenario)
        {
            var s = new UrgwomSeries(ConnectionStringUtility.GetFileName(scenario.Path, m_db.DataSource), 
                ConnectionStringUtility.GetToken(scenario.Path, "SheetName", ""),
                valueColumn);
            s.ScenarioName = scenario.Name;
            s.Name = "";
            s.Appearance.LegendText = scenario.Name;
            return s;
        }


        public override Series CreateBaseline() 
        {
           var fn = ConfigurationManager.AppSettings["UrgwomBaseLine"].ToString();
          var dir = Path.GetDirectoryName(xlsFilename);
          fn =  Path.Combine(dir, fn);

          var s = new UrgwomSeries(fn,"Run0",valueColumn);
          s.ScenarioName = "baseline";
          s.Appearance.LegendText = "baseline";
          return s;
        }
    }
}
