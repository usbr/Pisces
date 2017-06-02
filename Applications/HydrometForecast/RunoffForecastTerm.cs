using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;

namespace HydrometForecast
{
    public class RunoffForecastTerm: ForecastTerm
    {
        List<string> runoffDetails;
         public RunoffForecastTerm()
        {
            ForecastTermType = "Runoff";
            IsXterm = false;
            runoffDetails = new List<string>();
        }

         public override List<string> Details()
         {
             return runoffDetails;
         }

        /// <summary>
        /// Sums runoff over the seasonal period for this forecast
        /// </summary>
        /// <returns></returns>
         public double SeasonalRunoff(DateTime forecastDate)
         {
             CreateSeriesList();

             ReadFromHydromet(forecastDate);

             DateTime t1;
             DateTime t2;
             ComputeDateRange(forecastDate, out t1, out t2);
             var s = Reclamation.TimeSeries.Math.Subset(list[0], t1, t2);
             var seasonalRunoff1 = Reclamation.TimeSeries.Math.Sum(s);


             return seasonalRunoff1;
         }

         public double RunoffToDate(DateTime forecastDate)
         {
             CreateSeriesList();

             ReadFromHydromet(forecastDate);

             DateTime t1;
             DateTime t2;
             ComputeDateRange(forecastDate, out t1, out t2);

             if (t2 > forecastDate )
             {
                 t2 = forecastDate;
             }

             bool midMonth = forecastDate.Day != 1;

             var s = Reclamation.TimeSeries.Math.Subset(list[0], t1, t2);

             double runoffSum = 0;
             runoffDetails.Add("  Date   Runoff  Sum");
             for (int i = 0; i < s.Count; i++)
             {
                 if (!midMonth && s[i].DateTime >= forecastDate)
                     break;

                 if (s[i].IsMissing)
                 {
                     runoffDetails.Add("Error: missing data for runoff term " + s[i].DateTime.ToString("MMM yyyy"));
                     runoffSum = Point.MissingValueFlag;
                     break;
                 }
                 var pt = s[i];

                 double val = 0;
                 if (pt.Value >= 0)
                     val = pt.Value;

                 runoffSum += val;
                 string dateString = pt.DateTime.ToString("   MMM yyyy");
                 if (midMonth && i == s.Count - 1)
                 {
                     dateString = forecastDate.ToString("MMM dd yyyy");
                 }

                 runoffDetails.Add(dateString
                    + " " + pt.Value.ToString("F2").PadLeft(6)
                    + " " + runoffSum.ToString("F2").PadLeft(6));
             }

             runoffDetails.Add("");
            

             return runoffSum;
         }


        public override double Evaluate(DateTime forecastDate, bool lookAhead,double estimationScaleFactor)
        {
            throw new NotImplementedException("Dont' call this...");
        }
    }
}
