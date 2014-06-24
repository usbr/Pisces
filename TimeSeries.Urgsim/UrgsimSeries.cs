using System;
using System.Data;
using System.IO;
using Reclamation.Core;
using System.Collections.Generic;

namespace Reclamation.TimeSeries.Urgsim
{
    public class UrgsimSeries : Reclamation.TimeSeries.Series
    {
        string m_path;
        string m_projection;
        string m_variable;
        UrgsimReferenceSeries m_baseline;
        DataView m_refTableChecked;

        /// <summary>
        /// Constructs an Urgsim series
        /// </summary>
        /// <param name="path">Root directory that contains model output</param>
        /// <param name="projection">example: bias-corrected\ukmo_hadcm3.1.sresb1</param>
        /// <param name="variableName">example: RG at Lovatos [cfs]</param>
        public UrgsimSeries(string path, string projection, string variableName)
        {
            ExternalDataSource = true;
            m_path = path;
            m_projection = projection;
            m_variable = variableName;

            Name = m_variable;
            ScenarioName = m_projection;
            Parameter = m_variable;
            SiteName = m_variable;
            Units = GetUrgsimUnits(variableName);
            Appearance.LegendText = ScenarioName;
            Provider = "UrgsimSeries";
            ConnectionString = "Path="+m_path+";Projection="+m_projection;
        }
        
        /// <summary>
        /// Creates UrgsimSeries from TimeSeriesDatabase
        /// </summary>
        /// <param name="sdi"></param>
        /// <param name="db"></param>
        /// <param name="sr"></param>
        public UrgsimSeries(int sdi, TimeSeriesDatabase db, TimeSeriesDatabaseDataSet.SeriesCatalogRow sr)
            : base(db, sr)
        {
            this.ExternalDataSource = true;

            m_path = ConnectionStringUtility.GetToken(ConnectionString, "Path", "");
            m_projection = ConnectionStringUtility.GetToken(ConnectionString, "Projection", "");
            m_variable = Parameter;

            ScenarioName = m_projection;
            Appearance.LegendText = ScenarioName;
            Units = GetUrgsimUnits(Parameter);
        }

        public override Series CreateScenario(TimeSeriesDatabaseDataSet.ScenarioRow scenario)
        {
            if (scenario.Name == m_projection)
            {
                return this;
            }
            var s = new UrgsimSeries(m_path, scenario.Name, m_variable);
            s.TimeInterval = this.TimeInterval;
            return s;
        }

        public override Series CreateBaseline()
        {
            var refTable = new TimeSeriesDatabaseDataSet.ScenarioDataTable();
            refTable.ReadLocalXml();

            DataView refTableChecked = new DataView();
            refTableChecked.Table = refTable;
            refTableChecked.RowFilter = "Checked='true'";

            bool refNotChanged;
            if (m_refTableChecked == null)
                refNotChanged = false;
            else
                refNotChanged = refTableNotChanged(m_refTableChecked, refTableChecked);

            if (m_baseline != null && refNotChanged)
            {
                return m_baseline;
            }
            else
            {
                SeriesList sl = new SeriesList();
                m_refTableChecked = refTableChecked;
                foreach (DataRowView row in m_refTableChecked)
                {
                    string path = row[3].ToString();
                    string projection = row[1].ToString();
                    string variable = this.Name;

                    var s = new UrgsimSeries(path, projection, variable);
                    sl.Add(s);
                }
                m_baseline = new UrgsimReferenceSeries(sl);
                m_baseline.ScenarioName = "reference";
                m_baseline.Appearance.LegendText = "reference";
                if (!sl.HasMultipleSites)
                {
                    m_baseline.SiteName = m_variable;
                }
                m_baseline.Units = this.Units;
                m_baseline.TimeInterval = this.TimeInterval;
                return m_baseline;
            }
        }

        private bool refTableNotChanged(DataView view1, DataView view2)
        {
            bool rval = true;
            if (view1.Count != view2.Count)
                return rval = false;
            
            List<string> list1 = new List<string>();
            List<string> list2 = new List<string>();
            for (int i = 0; i < view1.Count; i++)
            {
                list1.Add(view1[i][1].ToString());
                list2.Add(view2[i][1].ToString());
            }
            foreach (string s in list1)
            {
                int idx = list2.IndexOf(s);
                if (idx < 0)
                    return rval = false;
            }
            return rval;
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
            string filename = m_path + m_projection + "." + m_variable + ".csv";
            string[] data = Web.GetPage(filename, true);
            
            Clear();
            for (int i = 1; i < data.Length - 1; i++)
            {
                var pt = new Point();
                // Should we do checks on dates and values?
                string[] tokens = data[i].Split(',');
                pt.DateTime = DateTime.Parse(tokens[0]);
                pt.Value = Double.Parse(tokens[1]);
                if (pt.DateTime >= t1 && pt.DateTime <= t2)
                {
                    this.Add(pt);
                }
            }
            this.ReadOnly = true;
        }

        private string GetUrgsimUnits(string variableName)
        {
            string units = "";
            int unitStart = variableName.IndexOf("[") + 1;
            int unitEnd = variableName.IndexOf("]");
            if (unitStart > 0 && unitEnd > 0 && unitEnd > unitStart)
            {
                units = variableName.Substring(unitStart, unitEnd - unitStart);
            }
            if (string.Equals(units, "inch_month"))
            {
                units = "inch/month";
            }
            if (string.Equals(units, "AF"))
            {
                units = "acre-feet";
            }

            return units;
        }

        private string GetProjectionName(string proj)
        {
            //given: bias-corrected/ukmo_hadcm3.1.sresb1
            //return: ukmo_hadcm3.1.sresb1
            return proj.Substring(proj.IndexOf(@"/") + 1);
        }
    }
}
