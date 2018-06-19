using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet.Operations;
using Math = Reclamation.TimeSeries.Math;
namespace FcPlot
{
    /// <summary>
    /// FloodOperation estimates targets into the future.  the primary target is based on
    /// the current forecast.  Other user specified targets are included in the output.
    /// </summary>
    public class FloodOperation
    {
        /// <summary>
        /// ComputeTargets method uses the a Rule curve, forecast, and historical average
        /// to project flood target levels through the forecast period.
        /// computes a target Seriesuses the current forecast and forecast volume period
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="waterYear"></param>
        /// <param name="start"></param>
        /// <param name="optionalPercents"></param>
        /// <returns></returns>
        public static SeriesList ComputeTargets(FloodControlPoint pt, 
            int waterYear, Point start,int[] optionalPercents, bool dashed)
        {
            string cbtt = pt.StationFC;
            
            SeriesList rval = new SeriesList();

            //calculate forecast of most recent month
            Series forecast = GetLatestForecast(cbtt, waterYear);
            int forecastMonth = MonthOfLastForecast(forecast);

            //if no forecast cannot compute target
            if (forecastMonth == 0)
            {
                return rval;
            }

            //value of forecast to use for percent of average
            double forecastValue = forecast[forecastMonth - 1].Value;
            // average runoff  month - end(typically July) volume

            if (cbtt.ToLower() == "hgh")
            {
                var avg30yrQU = Get30YearAverageSeries(pt.DailyStationQU, "qu", 5);

                // sum volume for the forecast period (may,sep)

                var t = new DateTime(start.DateTime.Year, 5, 1);
                var t2 = new DateTime(start.DateTime.Year, 9, 30);
                double historicalAverageResidual = SumResidual(avg30yrQU, t, t2);
                double percent = forecastValue / historicalAverageResidual;

                var x = HGHTarget(pt, forecastValue, start.DateTime, t);
                x.Name = "Forecast " + (100 * percent).ToString("F0") + "% " + (forecastValue / 1000.0).ToString("F0"); ;
                rval.Add(x);
                for (int i = 0; i < optionalPercents.Length; i++)
                {
                    var fc = historicalAverageResidual * optionalPercents[i] / 100.0;
                    x = HGHTarget(pt, fc, start.DateTime, t);
                    x.Name = "Target (" + optionalPercents[i].ToString("F0") + "%) " + (fc / 1000.0).ToString("F0");
                    rval.Add(x);
                }
            }
            else
            {
                rval.Add(GetTargets(pt, waterYear, start, optionalPercents, forecastMonth, forecastValue,dashed));
            }

            return rval;
        }

        private static Series HGHTarget(FloodControlPoint pt, double forecastValue, DateTime t1, DateTime t2)
        {
            HydrometRuleCurve m_ruleCurve = RuleCurveFactory.Create(pt, 7100,false);
            Series target = new Series("Target");
            
            string flag = "";
            DateTime t = t1;
            while (t < t2.AddDays(-1))
            {
                t = t.AddDays(1).Date;
                double val = -m_ruleCurve.LookupRequiredSpace(t, forecastValue, out flag) * pt.PercentSpace / 100 + pt.TotalUpstreamActiveSpace;
                target.Add(t, val);
            }
            
            return target;
        }

        /// <summary>
        /// Create a list of targts
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="waterYear"></param>
        /// <param name="start"></param>
        /// <param name="optionalPercents"></param>
        /// <param name="m_ruleCurve"></param>
        /// <param name="t2"></param>
        /// <param name="forecastMonth"></param>
        /// <param name="forecastValue"></param>
        /// <returns></returns>
        private static SeriesList GetTargets(FloodControlPoint pt, int waterYear, Point start, int[] optionalPercents,  int forecastMonth, double forecastValue, bool dashed)
        {
            SeriesList rval = new SeriesList();
            HydrometRuleCurve m_ruleCurve = RuleCurveFactory.Create(pt, 7100,dashed);
            var t1 = new DateTime(waterYear, pt.ForecastMonthStart, 1);
            var t2 = new DateTime(waterYear, pt.ForecastMonthEnd, 1).EndOfMonth();

            var avg30yrQU = Get30YearAverageSeries(pt.DailyStationQU, "qu", forecastMonth);

            // sum volume for the forecast period

            var t = new DateTime(start.DateTime.Year, start.DateTime.Month, 1);
            double historicalAverageResidual = SumResidual(avg30yrQU, t, t2);
            double percent = forecastValue / historicalAverageResidual;


            //get thirty year average QU from daily 
            //avg30yrQU = Get30YearAverageSeries(pt.DailyStationQU, "qu", forecastMonth);
            Series targetx = CalculateTarget(pt, percent, waterYear, start, m_ruleCurve, avg30yrQU, t2, forecastValue);
            targetx.Name = "Forecast (" + (100 * percent).ToString("F0") + "%)" + (forecastValue / 1000.0).ToString("F0");
            //targetx.Add(start);
            rval.Add(targetx);

            for (int i = 0; i < optionalPercents.Length; i++)
            {
                var fc = historicalAverageResidual * optionalPercents[i] / 100.0;
                targetx = CalculateTarget(pt, optionalPercents[i] / 100.0, waterYear, start,
                    m_ruleCurve, avg30yrQU
                    , t2, fc);
                targetx.Name = "Target (" + optionalPercents[i].ToString("F0") + "%) ";
                //targetx.Add(start);
                rval.Add(targetx);
            }

            return rval;
        }

        private static double SumResidual(Series avg30yrQU, DateTime t1, DateTime t2)
        {
            var rval = 0.0;

            for (int i = 0; i < avg30yrQU.Count; i++)
            {
                var pt = avg30yrQU[i];

                if( pt.DateTime >=t1 && pt.DateTime <=t2 &&   !pt.IsMissing)
                {
                    rval += pt.Value*1.98347;
                }
            }
            

            return rval;
        }

        private static Series CalculateTarget(FloodControlPoint pt, double percent,
             int waterYear,
            Point start, HydrometRuleCurve m_ruleCurve, Series avg30yrQU,
             DateTime t2, double forecastValue)
        {
            //double runoffSum = 0;
            string flag = "";
            Series s = new Series();
            Series residual = new Series();
            DateTime t = start.DateTime;

            while (t <= t2)
            {
                int idx = avg30yrQU.IndexOf(t);
                var sum = SumResidual(avg30yrQU, t, t2) * percent;
                residual.Add(t, sum);
                t = t.AddDays(1);
            }



            for (int i = 0; i < residual.Count(); i++)
            { // lookup space requirement in reservoir rule curve
                DateTime avg = residual[i].DateTime;
                DateTime dt = new DateTime(waterYear, residual[i].DateTime.Month, residual[i].DateTime.Day);
                double val = -m_ruleCurve.LookupRequiredSpace(avg, residual[i].Value, out flag) * pt.PercentSpace / 100 + pt.TotalUpstreamActiveSpace;
                if (val >= pt.TotalUpstreamActiveSpace)
                {
                    val = pt.TotalUpstreamActiveSpace;
                }
                s.Add(dt, val);
            }

            return s;
        }

        private static Series Get30YearAverageSeries(string cbtt, string pcode,int forecastMonth)
        {
            var t1 = HydrometDataUtility.T1Thirty;
            var t2 = HydrometDataUtility.T2Thirty;
            var s2 = new HydrometDailySeries(cbtt, pcode, HydrometHost.PNLinux);
            s2.Read(t1, t2);

            DateTime t = new DateTime(2018, forecastMonth, 1);
            var list = Math.SummaryHydrograph(s2, new int[] { }, t, false,false,true, false); 

            return list[0];
        }
        /// <summary>
        /// Finds forecast series.  Check if a mid-month forecast exists
        /// and use that if available.  Handle special case for hungry horse (hgh)
        /// </summary>
        /// <param name="cbtt"></param>
        /// <param name="waterYear"></param>
        /// <returns></returns>
        public static Series GetLatestForecast(string cbtt, Int32 waterYear)
        {
            var pc = "fc";
            if (cbtt.ToLower() == "hgh")
                pc = "fms"; // forecast (f) may (m) to september (s) == fms
            Series fc = new HydrometMonthlySeries(cbtt,pc);
            Series fcm = new HydrometMonthlySeries(cbtt, "fcm"); // mid month
            var t1 = new DateTime(waterYear, 1, 1);
            var t2 = new DateTime(waterYear, 7, 1);
            fc.Read(t1, t2);
            fcm.Read(t1, t2);

            int i_fc = MonthOfLastForecast(fc);
            int i_fcm = MonthOfLastForecast(fcm);

            if (i_fcm > 0 && i_fcm >= i_fc)
                return fcm;

            return fc;
        }

        private static int MonthOfLastForecast(Series forecast)
        {
            int month = 0;
            for (int i = 0; i < forecast.Count(); i++)
            {
                if (forecast[i].IsMissing == false)
                {
                    month = i+1;
                }
            }
            return month;
        }

    }
}
