using System;
using Reclamation.TimeSeries;

namespace HydrometForecast
{
    public class AntecedentRunoffForecastTerm: ForecastTerm
    {
        public AntecedentRunoffForecastTerm()
        {
            ForecastTermType = "Antecedent Runoff";
        }

        public override double Evaluate(DateTime forecastDate, bool lookAhead, double estimationScaleFactor)
        {
             CreateSeriesList();

            ReadFromHydromet( forecastDate);

            MonthlyEstimation.EstimateFutureWithAverage(list,forecastDate, estimationScaleFactor);

            return WeightedSum();

        }
    }
}
