using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Excel;
using System.Text.RegularExpressions;
using Reclamation.Core;
using System.Configuration;
using System.IO;
namespace Reclamation.TimeSeries
{
    public class UrgwomUtility
    {

        ExcelUtility xls;
        TimeSeriesDatabase m_db;
        string excelFilename;
        public UrgwomUtility(TimeSeriesDatabase db, string excelFilename )
        {
            xls = new ExcelUtility(excelFilename);
            m_db = db;
            this.excelFilename = excelFilename;
        }


        public void LoadTree()
        {
            m_db.SuspendTreeUpdates();
           var colNames = xls.ColumnNames("Run0");
           var sc = m_db.GetSeriesCatalog();

           var root = m_db.AddFolder("URGWOM");
             var sr =  m_db.GetNewSeriesRow();
             int id = sr.id;
           foreach (var item in colNames)
           {
               Series s = new UrgwomSeries(excelFilename, "Run0", item);
               s.ConnectionString = ConnectionStringUtility.MakeFileNameRelative(s.ConnectionString, m_db.DataSource);
               s.Name = item;
               s.Parameter = item;
               sc.AddSeriesCatalogRow(s, id, root.ID, "");
               id++;
           }

           m_db.Server.SaveTable(sc);
           m_db.ResumeTreeUpdates();
         
            

        }

        private static string CreateFileName(string baseFilename, string month, string percent)
        {
            string fileName = Path.GetFileName(baseFilename);
            string dir = Path.GetDirectoryName(baseFilename);

            string fn = Regex.Replace(fileName, "-[a-zA-Z]{3}-", "-" + month+"-");
            fn = Regex.Replace(fn, "-[0-9]{2}pct", "-" + percent+"pct");
            return Path.Combine(dir, fn);
        }

        public static string[] UrgwomForecastLevels = { "10", "30", "50", "70", "90" };
        public static string[] UrgwomMonths
        {
            get
            {
             return  ConfigurationManager.AppSettings["UrgwomForecastMonths"].Split(',');
            }
        }

        public static int UrgwonStartYear = 1976;
        public static int UrgwomEndYear = 2005;
        /// <summary>
        /// Scenario is defined by the workbook and worksheet names 
        /// </summary>
        public void LoadScenarios()
        {
            m_db.ClearScenarios();
           var table= m_db.GetScenarios();
      
           int i = 0;
           foreach (var pct in UrgwomForecastLevels)
           {
               foreach (var month in UrgwomMonths)
               {
                   int year = UrgwonStartYear;
                   int count = UrgwomEndYear - UrgwonStartYear + 1;
                   for (int run = 0; run < count; run++)
                   {
                       string fn = CreateFileName(excelFilename, month, pct);

                       var connectionString ="Year="+year+";Month="+month+";Percent="+pct+";FileName=" + fn + ";SheetName=" + "Run"+run;
                       connectionString =  ConnectionStringUtility.MakeFileNameRelative(connectionString, m_db.DataSource);
                       table.AddScenarioRow(month + " " + pct + "% " + year, i < 2, connectionString, 0);
                       i++;
                       year++;
                   }
               }
           }
           m_db.Server.SaveTable(table);
        }
    }
}
