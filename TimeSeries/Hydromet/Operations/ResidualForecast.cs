using System;
using System.Data;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries;
using System.Collections.Generic;
using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet.Operations;

namespace Reclamation.TimeSeries.Hydromet.Operations
{
    public class ResidualForecast
    {
        string[] reservoirNames;
        int[] reservoirLags;
        HydrometMonthlySeries fc, fcm;
        public HydrometDailySeries qu, qd;
        SeriesList reservoirs;

        FloodControlPoint controlPoint;
        public ResidualForecast(FloodControlPoint controlPoint)
        {
            this.controlPoint = controlPoint;
            this.reservoirLags = controlPoint.ReservoirLags;
            this.reservoirNames = controlPoint.UpstreamReservoirs;
            TotalContent = new Series();
            SpaceRequired = new Series("acre-feet", TimeInterval.Daily);;
            Space = new Series("acre-feet", TimeInterval.Daily);
            reservoirs = new SeriesList();
            Residual = new Series();
            Report = new SeriesList();
            diff = new Series();
            flags = new List<string>();
        }

       SeriesList Report;
       public Series TotalContent;
       public Series SpaceRequired;
       Series Space;
       Series Residual;
       Series diff;
        List<String> flags;

        DateTime _t1, _t2;

        public void Compute(DateTime t1, DateTime t2)
        {
            _t1 = t1;
            _t2 = t2;
            if (controlPoint.FillType == FillType.Variable)
            {
                ReadMonthlyForecast(t1, t2);
            }
            ReadDaily(t1, t2);

            TotalContent = reservoirs[0].Copy();
            for (int i = 1; i < reservoirNames.Length; i++)
            {
                TotalContent = TotalContent + reservoirs[i];
            }
            TotalContent.Name = "SYS";
            ComputeResidualAndRequiredSpace();

            Space = -TotalContent + controlPoint.TotalUpstreamActiveSpace;
            Space.Name = "Space";

            diff = Space - SpaceRequired;
            diff.Name = "Excess(+) Def(-)";

            Round(diff, 100);
            Round(SpaceRequired, 100);

            for (int i = 0; i < reservoirs.Count; i++)
            {
                Report.Add(reservoirs[i]);
            }
            
            Report.Add(TotalContent);
            if (controlPoint.FillType == FillType.Variable)
            {
                Report.Add(fc);
            }
            Report.Add(qd);
            Report.Add(qu);
            Report.Add(Residual);
            Report.Add(Space);
            Report.Add(SpaceRequired);
            Report.Add(diff);


        }

        private void Round(Series s, int roundBy )
        {
            for (int i = 0; i < s.Count; i++)
            {
                var pt = s[i];
                if (!pt.IsMissing)
                {
                    pt.Value = (int)System.Math.Round(pt.Value / roundBy) * roundBy;
                    s[i] = pt;
                }
            }
        }

        HydrometRuleCurve m_ruleCurve;

        public HydrometRuleCurve RuleCurve
        {
            get { return m_ruleCurve; }
            //set { m_ruleCurve = value; }
        }

        /// <summary>
        /// Residual is forecast minus QU
        /// </summary>
        private void ComputeResidualAndRequiredSpace()
        {
            SpaceRequired.Clear();
            SpaceRequired.Name = "Space Required";
            Residual.Clear();
            Residual.Name = "Residual";
            DateTime t = _t1;
            double resid = 0; // 998877;

            int wy = t.Year;
            if( t.Month >9)
                wy ++;

           

            m_ruleCurve = RuleCurveFactory.Create(controlPoint, wy);

            while (t <= _t2)
            {
                if (controlPoint.FillType == FillType.Variable)
                {
                    resid = ResetResidualBasedOnForecast(t, resid);
                    if (resid != HydrometRuleCurve.MissingValue && t <= qu.MaxDateTime)
                    {
                        var quTemp = qu[t].Value;
                        if (quTemp < 0)
                            quTemp = 0;
                        resid = resid - quTemp * 1.98347;
                        if (resid < 0)
                            resid = 0;

                        Residual.Add(t, resid);
                    }
                    else
                    {
                        Residual.AddMissing(t);
                    }
                }
                else
                {
                    resid = HydrometRuleCurve.MissingValue;
                }
                var t2 = t;
                if (t.Month == 2 && t.Day == 29)
                    t2 = t.AddDays(-1);
                string flag="";
                double req = 0;

                 req = m_ruleCurve.LookupRequiredSpace(t2, resid, out flag);

                if (req == HydrometRuleCurve.MissingValue)
                {
                    SpaceRequired.AddMissing(t);
                }
                else
                {
                    req = req * controlPoint.PercentSpace / 100.0;
                    if (req < 0)
                        req = 0;
                    SpaceRequired.Add(t, req);
                }
               flags.Add(flag);

                t = t.AddDays(1);
            }
        }

        


        private double ResetResidualBasedOnForecast(DateTime t, double resid)
        {
            if (t.Day == 1) // look for first of month forecast
            {
                if (fc.IndexOf(t) >= 0)
                {
                    if (!fc[t].IsMissing)
                        resid = fc[t].Value;
                }
            }
            else if (t.Day == 16) // check for mid month forecast
            {
                DateTime tm = t.MidMonth();
                if (fcm.IndexOf(tm) >= 0)
                {
                    if (!fcm[tm].IsMissing)
                        resid = fcm[tm].Value;
                }
            }
            return resid;
        }

       

        private void ReadDaily(DateTime t1, DateTime t2)
        {
            qu = new HydrometDailySeries(controlPoint.StationQU, "QU");
            qd = new HydrometDailySeries(controlPoint.StationQD, "QD");
            
            qu.Read(t1, t2);
            qd.Read(t1, t2);

            qu.Name = "QU";
            qd.Name = "QD";
            for (int i = 0; i < reservoirNames.Length; i++)
            {
                Series s = new HydrometDailySeries(reservoirNames[i], "AF");
                s.Read(t1.AddDays(-reservoirLags[i]), t2);
                s.Name = reservoirNames[i];
                if( reservoirLags[i] != 0)
                {
                 s  = Reclamation.TimeSeries.Math.Shift(s,reservoirLags[i]);
                }
                reservoirs.Add(s);
            }
        }

         void ReadMonthlyForecast(DateTime t1, DateTime t2)
        {

            fc = new HydrometMonthlySeries(controlPoint.StationQU, "FC");// hgh "fms"
            fc.TimePostion = TimePostion.FirstOfMonth;

            fcm = new HydrometMonthlySeries(controlPoint.StationQU, "FCM");
            fcm.TimePostion = TimePostion.MidMonth;

            fc.Read(t1, t2);
            fcm.Read(t1, t2);
        }

        public DataTable ReportTable { get
        {
            var tbl = Report.ToDataTable(true);
            // add flag column
            tbl.Columns.Add("flag",typeof(string));
            int n = System.Math.Min(tbl.Rows.Count, flags.Count);
            for (int i = 0; i < n; i++)
            {
                tbl.Rows[i]["flag"] = flags[i];
            }
            return tbl;
        }  }

        public string[] DailyCbttPcodeList()
        {
            var cbttList = new List<string>();

            cbttList.Add(controlPoint.StationQU + " QU");
            cbttList.Add(controlPoint.StationQU + " QD");
            cbttList.Add(controlPoint.StationQD + " QD");

            for (int i = 0; i < reservoirNames.Length; i++)
            {
                cbttList.Add(reservoirNames[i] + " AF");
            }

            return cbttList.ToArray();
        }

        public string[] MonthlyCbttPcodeList()
        {
            var cbttList = new List<string>();

            cbttList.Add(controlPoint.StationQU + " FC");
            cbttList.Add(controlPoint.StationQU + " FCM");

            return cbttList.ToArray();
        }
    }
}
