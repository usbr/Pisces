using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.Core;
using System.IO;
using System.Data;

namespace Reclamation.TimeSeries.BpaHydsim
{
    /// <summary>
    /// Time Series data from BPA Hydsim output in Access database
    /// </summary>
    public class BpaHydsimSeriesAccess : Series
    {
        string m_fileName, m_plantName, m_dataType;

        public BpaHydsimSeriesAccess(TimeSeriesDatabase db, Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow sr)
            : base(db, sr)
        {
            ExternalDataSource = true;
            Init(db);
        }

        public BpaHydsimSeriesAccess(string fileName, string plantName, string dataType)
        {
            m_fileName = fileName;
            m_plantName = plantName;
            m_dataType = dataType;
        }

        private void Init(TimeSeriesDatabase db)
        {
            m_fileName = ConnectionStringUtility.GetFileName(ConnectionString, db.DataSource);
            ScenarioName = Path.GetFileNameWithoutExtension(m_fileName);
            m_plantName = ConnectionStringUtility.GetToken(ConnectionString, "PlantName", "");
            m_dataType = ConnectionStringUtility.GetToken(ConnectionString, "DataType", "");
        }

        public override Series CreateScenario(TimeSeriesDatabaseDataSet.ScenarioRow scenario)
        {
            if (scenario.Name == ScenarioName)
            {
                return this;
            }
            else
            {
                string fn = ConnectionStringUtility.GetFileName(scenario.Path, m_db.DataSource);
                Logger.WriteLine("Reading series from " + fn);
                var sr = m_db.GetNewSeriesRow(false);
                sr.ItemArray = SeriesCatalogRow.ItemArray;

                sr.ConnectionString = ConnectionStringUtility.Modify(sr.ConnectionString, "FileName", fn);
                Series s = new BpaHydsimSeriesAccess( m_db, sr);
                s.ReadOnly = true;
                s.ScenarioName = scenario.Name;
                return s;
            }
        }
        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            Clear();
            ReadOnly = true;
            AccessDB mdb = new AccessDB(m_fileName);
            string[] dtypeAVG = { "QBPF", "QOUT", "FRCSPILL", "BYPSPILL" }; //flow variables
            string[] dtypeEOM = { "AVGEN", "SYSGEN", "SYSSURP", "SYSDP", 
                                    "ENDSTO", "ECC", "URC", "ENDELEV", "ECCFT", "URCFT" };
            Series s = new Series();

            if (dtypeAVG.Contains(m_dataType) == true) //get average of April & August values
            {
                s = ReadWithAverage(s, mdb, t1, t2);
            }
            else
                if (dtypeEOM.Contains(m_dataType) == true) //get end of month value for April & August
                {
                    s = ReadEndOfMonth(s, mdb, t1, t2);
                }
                else
                {
                    string msg = "Error: unsupported data type '" + m_dataType + "'";
                    Console.WriteLine(msg);
                    Logger.WriteLine(msg);
                }

            for (int i = 0; i < s.Count; i++)
            {
                Point pt = s[i];
                if (Units == "acre-feet")
                {
                    pt.Value = pt.Value * 1000.0 * (86400.0 / 43560.0); //convert ksfd to acre-feet
                }
                Add(pt);
            }
        }

        private Series ReadEndOfMonth(Series s, AccessDB mdb, DateTime t1, DateTime t2)
        {
            //string sqlEOM = "SELECT DateValue(Month(Working_Set.PeriodEnd) & "+"\""+"/"+"\""+" &"
            //    + " Day(Working_Set.PeriodEnd) & "+"\""+"/"+"\""+" & IIf(Month(Working_Set.PeriodEnd)>9,"
            //    + " Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1)) AS mmddyyyy, Working_Set.Data"
            //    + " FROM Working_Set"
            //    + " WHERE (((Working_Set.PlantName)=" + "\"" + m_plantName + "\"" + ") AND"
            //    + " ((Working_Set.DataType)=" + "\"" + m_dataType + "\"" + ") AND (Day(Working_Set.PeriodEnd)>27)"
            //    + " AND (DateValue(Month([Working_Set].[PeriodEnd]) & "+"\""+"/"+"\""+" & Day([Working_Set].[PeriodEnd])"
            //    + " & "+"\""+"/"+"\""+" & IIf(Month([Working_Set].[PeriodEnd])>9,Left([Working_Set].[WySeq],4),"
            //    + " Left([Working_Set].[WySeq],4)+1)))>=#"+t1.Date+"#"
            //    + " AND (DateValue(Month([Working_Set].[PeriodEnd]) & "+"\""+"/"+"\"" +" & Day([Working_Set].[PeriodEnd])"
            //    + " & "+"\""+"/"+"\""+" & IIf(Month([Working_Set].[PeriodEnd])>9,Left([Working_Set].[WySeq],4),"
            //    + " Left([Working_Set].[WySeq],4)+1)))<=#"+t2.Date+"#)";

            // Fixed to calculate end of month from PeriodStart rather than relying on the database (one database used
            // the 29th as the last day of February for every year, which created an error)
            string sqlEOM = "SELECT DateValue(Month(Working_Set.PeriodStart) & "+"\""+"/"+"\""+" &" 
                + " Day(DateAdd('d', -1, DateSerial(IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),"
                + " Left(Working_Set.WySeq,4)+1), Month(Working_Set.PeriodStart)+1,1))) & "+"\""+"/"+"\""+" &"
                + " IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1)) AS mmddyyyy,"
                + " Working_Set.Data"
                + " FROM Working_Set"
                + " WHERE (((Working_Set.PlantName)='"+m_plantName+"') AND ((Working_Set.DataType)='"+m_dataType+"')"
                + " AND (Day(Working_Set.PeriodEnd)>27)"
                + " AND DateValue(Month(Working_Set.PeriodStart) & "+"\""+"/"+"\""+" &"
                + " Day(DateAdd('d', -1, DateSerial(IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),"
                + " Left(Working_Set.WySeq,4)+1), Month(Working_Set.PeriodStart)+1,1))) & "+"\""+"/"+"\""+" &"
                + " IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1))>=#"+t1.Date+"#"
                + " AND DateValue(Month(Working_Set.PeriodStart) & "+"\""+"/"+"\""+" &"
                + " Day(DateAdd('d', -1, DateSerial(IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),"
                + " Left(Working_Set.WySeq,4)+1), Month(Working_Set.PeriodStart)+1,1))) & "+"\""+"/"+"\""+" &"
                + " IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1))<=#"+t2.Date+"#)";

            DataTable tbl = mdb.Table("Working_Set", sqlEOM);

            s = SeriesFromTable(tbl, 0, 1);
            return s;
        }

        private Series ReadWithAverage(Series s, AccessDB mdb, DateTime t1, DateTime t2)
        {
            // gets 15th of month
            //string sqlAVG = "SELECT DateValue(Month(Working_Set.PeriodStart) & " + "\"" + "/15/" + "\"" + " &"
            //    + " IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1))"
            //    + " AS mmddyyyy, Avg(Working_Set.Data) AS AvgOfData"
            //    + " FROM Working_Set"
            //    + " GROUP BY DateValue(Month(Working_Set.PeriodStart) & " + "\"" + "/15/" + "\"" + " &"
            //    + " IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1)),"
            //    + " Working_Set.PlantName, Working_Set.DataType"
            //    + " HAVING (((DateValue(Month(Working_Set.PeriodStart) & " + "\"" + "/15/" + "\"" + " &"
            //    + " IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1)))"
            //    + ">=#" + t1.Date + "# And (DateValue(Month(Working_Set.PeriodStart) & " + "\"" + "/15/" + "\"" + " &"
            //    + " IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1)))"
            //    + "<=#" + t2.Date + "#) AND ((Working_Set.PlantName)=" + "\"" + m_plantName + "\"" + ") AND"
            //    + " ((Working_Set.DataType)=" + "\"" + m_dataType + "\"" + "))";

            // gets end of month
            string sqlAVG = "SELECT DateValue(Month(Working_Set.PeriodStart) & "+ "\"" + "/" + "\"" + " & Day(DateAdd('d', -1,"
                + " DateSerial(IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1),"
                + " Month(Working_Set.PeriodStart)+1,1))) & "+ "\"" + "/" + "\"" + " & IIf(Month(Working_Set.PeriodStart)>9,"
                + " Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1)) AS mmddyyyy, Avg(Working_Set.Data) AS AvgOfData"
                + " FROM Working_Set"
                + " GROUP BY DateValue(Month(Working_Set.PeriodStart) & "+ "\"" + "/" + "\"" + " & Day(DateAdd('d', -1,"
                + " DateSerial(IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1),"
                + " Month(Working_Set.PeriodStart)+1,1))) & "+ "\"" + "/" + "\"" + " & IIf(Month(Working_Set.PeriodStart)>9,"
                + " Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1)), Working_Set.PlantName, Working_Set.DataType"
                + " HAVING (((Working_Set.PlantName)='"+m_plantName+"') AND ((Working_Set.DataType)='"+m_dataType+"') AND"
                + " ((DateValue(Month(Working_Set.PeriodStart) & "+ "\"" + "/" + "\"" + " & Day(DateAdd('d', -1,"
                + " DateSerial(IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1),"
                + " Month(Working_Set.PeriodStart)+1,1))) & "+ "\"" + "/" + "\"" + " & IIf(Month(Working_Set.PeriodStart)>9,"
                + " Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1)))>=#"+t1.Date+"# And"
                + " (DateValue(Month([Working_Set].[PeriodStart]) & "+ "\"" + "/" + "\"" + " & Day(DateAdd('d', -1,"
                + " DateSerial(IIf(Month(Working_Set.PeriodStart)>9,Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1),"
                + " Month(Working_Set.PeriodStart)+1,1))) & "+ "\"" + "/" + "\"" + " & IIf(Month(Working_Set.PeriodStart)>9,"
                + " Left(Working_Set.WySeq,4),Left(Working_Set.WySeq,4)+1)))<=#"+t2.Date+"#))";

            DataTable tbl = mdb.Table("Working_Set", sqlAVG);

            s = SeriesFromTable(tbl, 0, 1);
            return s;
        }
    }
}
