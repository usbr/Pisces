using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet.Operations;

namespace FcPlot
{
    public class FCOP
    {
        public static Series ComputeTargets(FloodControlPoint pt, int waterYear)
        {
            string cbtt = pt.StationQU;
            HydrometRuleCurve m_ruleCurve = RuleCurveFactory.Create(pt, 7100);
            Series forecast30yrAvg = new HydrometMonthlySeries(cbtt, "FC");
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

            //get 81-10 average forecast for that month and calculate percent of average
            t1 = new DateTime(8110, forecastMonth, 1);
            forecast30yrAvg.Read(t1, t1);
            double averageForcastValue = forecast30yrAvg[0].Value;
            double percent = forecastValue / averageForcastValue;
            
            //get thirty year average QU from either monthly or daily series which ever is available
            if (UseDailyAverage(cbtt))
	        {
                avg30yrQU = new HydrometDailySeries(cbtt, "QU");
                t1 = new DateTime(7100, forecastMonth, 1);
                t2 = new DateTime(7100, 7, 31);
                avg30yrQU.Read(t1, t2);

                for (int i = 0; i < avg30yrQU.Count(); i++)
                {
                    if (avg30yrQU[i].IsMissing == false)
                    {
                        expectedRunoff = expectedRunoff + avg30yrQU[i].Value * 1.98347;
                        var s = forecastValue - expectedRunoff * percent;
                        targets.Add(avg30yrQU[i].DateTime, s);
                    }
                }
	        }
            else
	        {
                avg30yrQU = new HydrometMonthlySeries(cbtt, "QU");
                t1 = new DateTime(7100, forecastMonth, 1);
                t2 = new DateTime(7100, 7, 1);

                avg30yrQU.Read(t1, t2);

                for (int i = 0; i < avg30yrQU.Count(); i++)
                {
                    if (avg30yrQU[i].IsMissing == false)
                    {
                        if (i==0)
                        {
                            targets.Add(avg30yrQU[i].DateTime, forecastValue);
                        }
                        else
                        {
                            expectedRunoff = expectedRunoff + avg30yrQU[i].Value;
                            var s = forecastValue - expectedRunoff * percent;
                            targets.Add(avg30yrQU[i].DateTime, s);
                        }
                        
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
