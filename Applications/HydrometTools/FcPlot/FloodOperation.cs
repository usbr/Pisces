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
        public static Series ComputeTargets(FloodControlPoint pt, int waterYear)
        {
            string cbtt = pt.StationQU;
            HydrometRuleCurve m_ruleCurve = RuleCurveFactory.Create(pt, 7100);
            
            Series avg30yrQU;
            Series targets = new Series();
            Series requiredContent = new Series();
            double expectedRunoff = 0;
            string flag;
            var t1 = new DateTime(waterYear, 1, 1);
            var t2 = new DateTime(waterYear, 7, 1);

            //calculate forecast of most recent month
            Series forecast = GetLatestForecast(cbtt, waterYear);
            int forecastMonth = MonthOfLastForecast(forecast);
            
            //if no forecast cannot compute target
            if (forecastMonth == 0)
            {
                return requiredContent;
            }  
          
            //value of forecast to use for percent of average
            double forecastValue = forecast[forecastMonth-1].Value;

            double averageForcastValue = HydrometMonthlySeries.AverageValue30Year(cbtt, "fc", forecastMonth, forecastMonth);
            double percent = forecastValue / averageForcastValue;

            //get thirty year average QU from either monthly or daily series which ever is available
            avg30yrQU = Get30YearAverageSeries(cbtt, "qu",forecastMonth);

            if (avg30yrQU.Count >0)
	        {
                for (int i = 0; i < avg30yrQU.Count(); i++)
                {
                    var t = avg30yrQU[i].DateTime;
                    if ( t >= t1 && t<=t2 && !avg30yrQU[i].IsMissing )
                    {
                        expectedRunoff = expectedRunoff + avg30yrQU[i].Value * 1.98347;
                        var s = forecastValue - expectedRunoff * percent;
                        targets.Add(avg30yrQU[i].DateTime, s);
                    }
                }
	        }

            for (int i = 0; i < targets.Count(); i++)
            {
                DateTime avg = targets[i].DateTime;
                DateTime s = new DateTime(waterYear, targets[i].DateTime.Month, targets[i].DateTime.Day);
                double val = -m_ruleCurve.LookupRequiredSpace(avg, targets[i].Value, out flag)*pt.PercentSpace/100 + pt.TotalUpstreamActiveSpace;
                if (val >= pt.TotalUpstreamActiveSpace)
                {
                    val = pt.TotalUpstreamActiveSpace;
                }
                requiredContent.Add(s, val);
            }

            return requiredContent;
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

        private static bool UseDailyAverage(string cbtt)
        {
            Series testDate = new HydrometDailySeries(cbtt, "QU");
            var t1 = new DateTime(7100, 1, 1);
            var t2 = new DateTime(7100, 7, 31);
            testDate.Read(t1, t2);
            testDate.RemoveMissing();
            if (testDate.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
    }
}
