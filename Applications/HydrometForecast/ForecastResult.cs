using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HydrometForecast
{
    public class ForecastResult
    {
        public double Forecast = 0;
        public double ResidualForecast = 0;
        public List<string> Details = new List<string>();
        public string ForecastPeriod;
        public double AverageRunoff;
        public ForecastEquation Equation;
        public double EstimationFactor=1.0;
        public ForecastResult()
        {
            ForecastPeriod = "error";
            AverageRunoff = 0;
            Equation = new ForecastEquation();
        }
        public double GetForecastForSummary(bool lookAhead)
        {
            if (lookAhead)
                return Forecast;
            return ResidualForecast;
        }
    }
}
