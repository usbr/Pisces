using Reclamation.Core;
using Reclamation.TimeSeries.Analysis;
using System;
using System.Data;

namespace Reclamation.TimeSeries.Hydromet.Operations
{
    /// <summary>
    /// The Little Wood River custom rule curve.
    /// There are three criteria:
    /// 1) forecast based
    /// 2) filling schedule
    /// 3) current runoff
    /// </summary>
    class WodiRuleCurve: HydrometRuleCurve
    {

        private Series qu;
        DataTable tblQu;
        DataTable tblfc;
        private Series quAverage;
        private Series quSumJuly;
        private HydrometMonthlySeries fc;
        int waterYear;

        public WodiRuleCurve(string cbtt, int waterYear,FillType fType)
            : base("wodi.space", fType)
        {
            this.waterYear = waterYear;
            qu = new HydrometDailySeries("wodi", "qu");
            var t1 = new DateTime(waterYear - 1, 10, 1);
            var t2 = new DateTime(waterYear, 9, 30);
            qu.Read(t1, t2);
            tblQu = FcPlotDataSet.GetTable("wodi.inflow");
            tblfc = FcPlotDataSet.GetTable("wodi.forecast");
            fc = new HydrometMonthlySeries("wodi", "fc");
            fc.TimePostion = TimePostion.EndOfMonth;
            fc.Read(t1, t2);

            ReadQUAverage();
        }


        private void ReadQUAverage()
        {
            var qua = AverageSeries30Year();
            // we want january --> september (remove other months)..           
            MonthDayRange r = new MonthDayRange(1, 1, 9, 30);
            quAverage = Math.Subset(qua, r);
            quSumJuly = quAverage.Clone();
            
            for (int i = 0; i < 7; i++)
            {
                double sum = 0.0;
                for (int j = i; j < 7; j++) // sum through July
                {
                    sum += quAverage[j].Value;
                }
                var pt = quAverage[i];
                quSumJuly.Add(new DateTime(waterYear,pt.DateTime.Month,pt.DateTime.Day), sum);
            }
           quAverage = Reclamation.TimeSeries.Math.ShiftToYear(quAverage, waterYear);
        }

        static Series AverageSeries30Year()
        {
            var t1 = HydrometDataUtility.T1Thirty;
            var t2 = HydrometDataUtility.T2Thirty;
            Series qu = new HydrometDailySeries("wodi", "qu");
            qu.Read(t1, t2);
            qu = Math.MonthlySum(qu) * 0.00198347;

            var qua = MonthlySummaryAnalysis.MonthlySeries(qu, StatisticalMethods.Mean, false);
            return qua;

        }

        public override double LookupRequiredSpace(DateTime t, double resid, out string flag)
        {
            var reqfc = ComputeWodiRequired(t, resid);
           var reqfil = base.LookupRequiredSpace(t, resid, out flag);

           var t2 = t;
           if (t > qu.MaxDateTime)
               t2 = qu.MaxDateTime;

           var reqqu = 0;
           if (!qu[t2].IsMissing) 
               Reclamation.TimeSeries.Math.Interpolate(tblQu, qu[t2].Value, "inflow", "space");

           var req = reqfc;
           flag = "*";
           if (reqfil >= req)
           {
               req = reqfil;
               flag = "#";
           }
           if (reqqu > req)
           {
               req = reqqu;
               flag = "$";
           }
                return req;
        }

        private double ComputeWodiRequired(DateTime t, double resid)
        {
            // apr 1-sep 30, based on forecast
            if (t.Month >= 4 && t.Month <= 9)
            {
                double avgqu = quSumJuly[t.Month-1].Value;
                // compute percent of average
                var pt = fc[t.EndOfMonth()];
                if (pt.IsMissing )
                    return 0;

                double percent = pt.Value / avgqu;
                // add in August and September based on percent 
                var augSep = quAverage[7].Value + quAverage[8].Value;// was constant 7800 in FORTRAN
                
                var fcSep = resid + augSep * percent;

                // lookup space required
                var req = Reclamation.TimeSeries.Math.Interpolate(tblfc, fcSep,"forecast", "space");
                return req;
            }
            else
            {
                return 0;
            }
        }


    }
}
