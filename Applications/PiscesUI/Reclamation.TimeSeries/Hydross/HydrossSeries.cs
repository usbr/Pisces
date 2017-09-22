using Reclamation.Core;
using System;
using System.IO;

namespace Reclamation.TimeSeries.Hydross
{
    public class HydrossSeries : Series
    {

        string fileName, stationID, varcode, extra1, extra2;

        public HydrossSeries(TimeSeriesDatabase db, Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow sr):base(db,sr)
        {
            ExternalDataSource = true;
            Init(db);

        }

        private void Init(TimeSeriesDatabase db)
        {
            fileName = ConnectionStringUtility.GetFileName(ConnectionString, db.DataSource);   
            ScenarioName = Path.GetFileNameWithoutExtension(fileName);
            stationID = ConnectionStringUtility.GetToken(ConnectionString, "stationID", "");
            varcode = ConnectionStringUtility.GetToken(ConnectionString, "varcode", "");
            extra1 = ConnectionStringUtility.GetToken(ConnectionString, "extra1", "");
            extra2 = ConnectionStringUtility.GetToken(ConnectionString, "extra2", "");

         
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
                Series s = new HydrossSeries(m_db, sr);
                s.ReadOnly = true;
                s.ScenarioName = scenario.Name;
                return s;
            }
        }

        protected override void ReadCore(DateTime t1, DateTime t2)
        {
            Clear();

            int ivarcode = int.Parse(varcode);

            // Output header info
            FileInfo fi = new FileInfo(fileName);
            string scen_name = fi.Name;

            // Open the .ods file for the current scenario.
            StreamReader istr = new StreamReader(fileName + ".ods");

            // Read the year from the first line.
            string input = istr.ReadLine();

            // The year is the last two items
            string[] tokens = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int scenStartYear = int.Parse(tokens[tokens.Length - 2]);
            int scenEndYear = int.Parse(tokens[tokens.Length - 1]);

//            err_ostr.WriteLine(string.Format("Start year = {0}, end year = {1}", scenStartYear, scenEndYear));

            // The next six lines are skipped.
            for (int iline = 0; iline < 6; iline++)
            {
                istr.ReadLine();
            }

            while (!istr.EndOfStream)
            {
                string line = istr.ReadLine();

                if (line.Trim().Length == 0) continue;

                int currentYear = int.Parse(line.Substring(0, 4));

                string currentcode = line.Substring(15, 3);
                if (currentcode != varcode)
                    continue;

                string currentStation = line.Substring(4, 6);
                if (currentStation != stationID)
                    continue;

                // Check possible diversions/IFRs
                if (ivarcode > 400 && ivarcode < 500)
                {
                    // Looking for a diversion code.
                    string divcode = line.Substring(11, 4);
                    if (divcode != extra1) continue;
                }
                else if (ivarcode > 500 && ivarcode < 600)
                {
                    // Looking for an IFR code.
                    string ifrcode = line.Substring(11, 4);
                    ifrcode = ifrcode.Trim();
                    if (ifrcode != extra1) continue;
                }
                else if (ivarcode == 113)
                {
                    string divname = "d";
                    divname += line.Substring(24, 3);
                    if (divname.Trim() != extra1) continue;

                    // Check return flow type
                    string rftype = line.Substring(27, 1);
                    //err_ostr.WriteLine(string.Format("Checking return flow type: {0} ? {1}", rftype, extra2));
                    if (rftype != extra2) continue;
                }

                //ostr << "1/" << currentYear << "," << stationID << endl;
                for (int imonth = 0; imonth < 12; imonth++)
                {
                    string val_str = line.Substring(32 + imonth * 10, 10);
                    val_str = val_str.Trim();
                    //ostr.WriteLine(string.Format("{0}/1/{1},{2}", imonth + 1, currentYear, val_str));
                    double v;
                    if (double.TryParse(val_str, out v))
                    {
                        DateTime t = new DateTime(currentYear, imonth + 1, 1);
                        Add(t, v);
                    }
                    else
                    {
                        Logger.WriteLine("Error Parsing '" + val_str + "'");
                    }
                }
            }


        }
    }
}
