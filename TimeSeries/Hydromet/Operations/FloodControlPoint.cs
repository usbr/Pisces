using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.Core;
using System.Data;
using Reclamation.TimeSeries.Hydromet;

namespace Reclamation.TimeSeries.Hydromet.Operations
{
    /// <summary>
    /// FloodControlPoint contains the key properties that
    /// define flood control at a specific river location
    /// downstream of one or more reservoirs.
    /// </summary>
    public class FloodControlPoint
    {
        public string StationFC { get; set; }
        public string StationQU { get; set; }
        public string DailyStationQU { get; set; }
        public string StationQD { get; set; }
        public string[] UpstreamReservoirs {get; set;}
        public int[] ReservoirLags {get; set;}
        public double TotalUpstreamActiveSpace{get; set;}
        private bool VariableRuleCurve { get; set; }
        public string FlagLegend { get; set; }
        public double PercentSpace { get; set; }
        public string Name {get; set;}
        public string RequiredLegend { get; set; }
        public int ForecastMonthStart { get; private set; }
        public int ForecastMonthEnd { get; private set; }
        public string EspNode { get; set; }

        public FillType FillType
        {
            get {
                var m_fillType = FillType.Fixed;
                if (VariableRuleCurve)
                    m_fillType = FillType.Variable;
                return m_fillType; 
            
            }
        }

        public FloodControlPoint(string text, string lookupColumnName="Name")
        {
            FlagLegend = "";
            Init(text,lookupColumnName);
        }

        private void Init(string text, string lookupColumnName)
        {

            DataTable tbl = FcPlotDataSet.ControlPointTableFromName(text,lookupColumnName);

            if (tbl.Rows.Count != 1)
                throw new ArgumentException("Error: site not found '" + Name + "'");

            Name = tbl.Rows[0]["Name"].ToString();
            StationQU = tbl.Rows[0]["StationQU"].ToString();
            StationQD = tbl.Rows[0]["StationQD"].ToString();
            DailyStationQU = tbl.Rows[0]["DailyStationQU"].ToString();
            StationFC = tbl.Rows[0]["StationFC"].ToString();
            EspNode = tbl.Rows[0]["EspNode"].ToString();

            if (tbl.Rows[0]["FlagLegend"] != DBNull.Value)
            {
                FlagLegend = tbl.Rows[0]["FlagLegend"].ToString();
            }

            PercentSpace = Convert.ToDouble(tbl.Rows[0]["PercentSpace"]);

            string s = tbl.Rows[0]["UpstreamReservoirs"].ToString().Trim();
            UpstreamReservoirs = s.Split(',');

            ReservoirLags = new int[UpstreamReservoirs.Length];


            if (tbl.Rows[0]["ReservoirLags"] != DBNull.Value)
            {
                s = tbl.Rows[0]["ReservoirLags"].ToString().Trim();
                if (s != "")
                {
                    var tokens = s.Split(',');
                    List<int> lagList = new List<int>();
                    for (int i = 0; i < tokens.Length; i++)
                    {
                        lagList.Add(Convert.ToInt32(tokens[i]));
                    }
                    if (lagList.Count != UpstreamReservoirs.Length)
                        throw new ArgumentException("The number of lags, does not match the number of upstream stations");

                    ReservoirLags = lagList.ToArray();
                }
            }

            TotalUpstreamActiveSpace = Convert.ToDouble(tbl.Rows[0]["TotalUpstreamActiveSpace"]);

            VariableRuleCurve = Convert.ToBoolean(tbl.Rows[0]["VariableRuleCurve"]);

            RequiredLegend = tbl.Rows[0]["RequiredLegend"].ToString();

            ForecastMonthStart = Convert.ToInt32(tbl.Rows[0]["ForecastMonthStart"].ToString());
            ForecastMonthEnd = Convert.ToInt32(tbl.Rows[0]["ForecastMonthEnd"].ToString());
            
        }

        
    }
}
