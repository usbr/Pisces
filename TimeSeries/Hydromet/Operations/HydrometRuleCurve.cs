using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.TimeSeries;
using System.IO;
using Reclamation.Core;


namespace Reclamation.TimeSeries.Hydromet.Operations
{
    public enum FillType { Fixed, Variable };

    /// <summary>
    /// Base Class for Rule Curves.
    /// </summary>
    public class HydrometRuleCurve
    {
        public const double MissingValue = 998877;
        private string curveName;

        public string CurveName
        {
            get { return curveName; }
            set { curveName = value; }
        }
        FillType m_fillType;

        public FillType FillType
        {
            get { return m_fillType; }
            set { m_fillType = value; }
        }

        /// <summary>
        /// constructs a RuleCurve based on the unregulated flow station name
        /// </summary>
        /// <param name="curveName"></param>
        public HydrometRuleCurve(string curveName)
        {
            this.curveName = curveName;
            FloodControlPoint pt = new FloodControlPoint(curveName, "StationQU");
            m_fillType = pt.FillType;
        }

        public HydrometRuleCurve(string curveName, FillType ftype = Hydromet.Operations.FillType.Fixed)
        {
            this.curveName = curveName;
            m_fillType = ftype;
        }
        public HydrometRuleCurve(FloodControlPoint pt)
        {
            this.curveName = pt.StationQU;
            m_fillType = pt.FillType;
        }

        private PeriodicSeries m_periodicTable;

        protected PeriodicSeries PeriodicTable
        {
            get {
                if (m_periodicTable == null)
                    m_periodicTable = FcPlotDataSet.GetPeriodicSeries(curveName);
                return m_periodicTable; 
            }
            set { m_periodicTable = value;}
        }

        public virtual double LookupRequiredSpace(DateTime t, double resid, out string flag)
        {
            flag = "";
            if( m_fillType ==  FillType.Fixed)
                  return PeriodicTable.Interpolate(t);
            return PeriodicTable.Interpolate2D(t, resid);
        }




        /// <summary>
        /// Computes rule curve in terms of 'required' storage
        /// by subracting space from the total available space
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="totalSpace"></param>
        /// <returns></returns>
        public SeriesList CalculateFixedRuleCurve(DateTime t1, DateTime t2,double totalSpace)
        {
            var rval = new SeriesList();

                Series s = CreateRuleLine(0, t1, t2);
                s = Max(-s + totalSpace,0);
                rval.Add(s);

            return rval;
        }


        /// <summary>
        /// computes the Max value 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
         static Series Max(Series s, double value)
        {
            Series rval = s.Copy();
            for (int i = 0; i < s.Count; i++)
            {
                Point pt = s[i];
                if (pt.IsMissing)
                {
                    continue;
                }

                pt.Value = System.Math.Max(pt.Value, value);
                rval[i] = pt;
            }
            return rval;
        }


        public SeriesList CalculateVariableRuleCurves(DateTime t1, DateTime t2, double totalSpace, double percent)
        {
            var rval = new SeriesList();
            if (m_fillType == FillType.Fixed)
                return rval;


            var levels = FcPlotDataSet.GetVariableForecastLevels(curveName);
            for (int i = 0; i < levels.Length; i++)
            {

                Series s = CreateRuleLine(levels[i], t1, t2) * percent;
                s = -s + totalSpace;

                s = Max(s, 0);
                rval.Add(s);
            }
            
            return rval;
        }

        private Series CreateRuleLine(double forecast, DateTime t1, DateTime t2)
        {
            Series s = new Series();
            s.Name = (forecast/1000000.0 ).ToString("F2");
            s.TimeInterval = TimeInterval.Daily;
            string flag = "";
            DateTime t = t1;
            while (t <= t2)
            {
                var t3 = t;
                if (t.Month == 2 && t.Day == 29)
                    t3 = t.AddDays(-1);

                var d = LookupRequiredSpace(t3, forecast, out flag);
                if (d == HydrometRuleCurve.MissingValue)
                    s.AddMissing(t);
                else
                    s.Add(t, d);

                t = t.Date.AddDays(1);
            }
            return s;
        }

       
    }
}
