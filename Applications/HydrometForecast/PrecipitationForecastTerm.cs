using System;
using Reclamation.TimeSeries;

namespace HydrometForecast
{
    public class PrecipitationForecastTerm:ForecastTerm
    {
        //http://www.wrh.noaa.gov/boi/hydro/idprecip.php


        public PrecipitationForecastTerm()
        {
            ForecastTermType = "Precipitation";
        }

        /// <summary>
        ///  computes the value for this term, estimating if necessary
        /// </summary>
        /// <param name="forecastDate">Date of forecast</param>
        /// <param name="lookAhead">set to true for historical 'perfect' forecast</param>
        /// <returns></returns>
        public override double Evaluate(DateTime forecastDate, bool lookAhead ,double estimationScaleFactor)
        {
            CreateSeriesList();

            ReadFromHydromet( forecastDate);

             MonthlyEstimation.EstimateMissingByGroup(list,forecastDate);
            if (!lookAhead)
            {
                MonthlyEstimation.EstimateFutureWithAverage(list,forecastDate,estimationScaleFactor);
            }

            ////WriteSeriesDataToConsole(list);
            
            double rval = WeightedSum();
            Console.WriteLine("Total = " + rval);
            return rval;
        }

       

        //// <summary>
        //// Determine if this term uses data from a previous water year.
        //// </summary>
        //// <returns></returns>
        ////private bool SpansWaterYears()
        ////{// if july throught december we are in two water years..
        ////    return MonthNames.IndexOf("oct") > 0;
        ////}
    }
}
