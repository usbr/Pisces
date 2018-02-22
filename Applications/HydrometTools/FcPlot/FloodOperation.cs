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
    public class FloodOperation
    {
        public static SeriesList ComputeTargets(FloodControlPoint pt, 
            int waterYear, Point start,int[] optionalPercents)
        {
            string cbtt = pt.StationQU;
            HydrometRuleCurve m_ruleCurve = RuleCurveFactory.Create(pt, 7100);
            SeriesList rval = new SeriesList();
            Series avg30yrQU;
            var t1 = new DateTime(waterYear, 1, 1);
            var t2 = new DateTime(waterYear, 7, 1);

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

            double averageForcastValue = HydrometMonthlySeries.AverageValue30Year(cbtt, "fc", forecastMonth, forecastMonth);
            double percent = forecastValue / averageForcastValue;

            
            //get thirty year average QU from either monthly or daily series which ever is available
            avg30yrQU = Get30YearAverageSeries(pt.DailyStationQU, "qu", forecastMonth);
            Series targetx = CalculateTarget(pt, waterYear, start, m_ruleCurve, avg30yrQU, t2, forecastValue);
            targetx.Name = "Forecast (" + (100 * percent).ToString("F0") + "%)";
            targetx.Add(start);
            rval.Add(targetx);

            for (int i = 0; i < optionalPercents.Length; i++)
            {
                targetx = CalculateTarget(pt, waterYear, start,
                    m_ruleCurve, avg30yrQU  
                    , t2, averageForcastValue* optionalPercents[i]/100.0);
                targetx.Name = "Target (" + optionalPercents[i].ToString("F0") + "%)";
                targetx.Add(start);
                rval.Add(targetx);
            }
            return rval;
        }

        private static Series CalculateTarget(FloodControlPoint pt, int waterYear,
            Point start, HydrometRuleCurve m_ruleCurve, Series avg30yrQU, 
             DateTime t2, double forecastValue)
        {
            double runoffSum = 0;
            string flag = "";
            Series s = new Series();
            Series residual = new Series();
            for (int i = 0; i < avg30yrQU.Count(); i++)
            { // compute forecast runoff
                var t = avg30yrQU[i].DateTime;
                if (t > start.DateTime && t <= t2 && !avg30yrQU[i].IsMissing)
                {
                    runoffSum += avg30yrQU[i].Value * 1.98347;
                    var d = forecastValue - runoffSum; ;
                    residual.Add(avg30yrQU[i].DateTime, d);
                }
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
            var t1 = new DateTime(1980, 10, 1);
            var t2 = new DateTime(2010, 9, 30);
            var s2 = new HydrometDailySeries(cbtt, pcode, HydrometHost.PNLinux);
            s2.Read(t1, t2);

            DateTime t = new DateTime(2018, forecastMonth, 1);
            var list = Math.SummaryHydrograph(s2, new int[] { }, t, false,false,true, false); 

            return list[0];
        }

        public static Series GetLatestForecast(string cbtt, Int32 waterYear)
        {
            Series forecast = new HydrometMonthlySeries(cbtt,"FC");
            var t1 = new DateTime(waterYear, 1, 1);
            var t2 = new DateTime(waterYear, 7, 1);
            forecast.Read(t1, t2);
            return forecast;
        }

        private static Int32 MonthOfLastForecast(Series forecast)
        {
            Int32 month = 0;
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
