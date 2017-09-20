using Csu.Modsim.ModsimIO;
using Csu.Modsim.ModsimModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.TimeSeries.Modsim
{
    public static class ModsimUtilities
    {
        /// <summary>
        /// Creates modsim TimeSeries and sets VariesByYear 
        /// </summary>
        /// <param name="tstype"></param>
        /// <param name="n"></param>
        public static void CreateModsimTimeSeries(TimeSeriesType tstype, Node n, bool variesByYear)
        {

            var ts = new Csu.Modsim.ModsimModel.TimeSeries(tstype);
            switch (tstype)
            {
                case TimeSeriesType.Demand:
                    ts.units = n.m.adaDemandsM.units;
                    n.m.adaDemandsM = ts;
                    break;
                case TimeSeriesType.NonStorage:
                    ts.units = n.m.adaInflowsM.units;
                    n.m.adaInflowsM = ts;
                    break;
                case TimeSeriesType.Targets:
                    ts.units = n.m.adaTargetsM.units;
                    n.m.adaTargetsM = ts;
                    break;
                case TimeSeriesType.Evaporation:
                    ts.units = n.m.adaEvaporationsM.units;
                    n.m.adaEvaporationsM = ts;
                    break;
                case TimeSeriesType.Forecast:
                    ts.units = n.m.adaForecastsM.units;
                    n.m.adaForecastsM = ts;
                    break;
                default:
                    throw new Exception("Only NonStorage, Demand, Targets, Forecast, and Evaporation supported");
            }

            ts.VariesByYear = variesByYear;
            ts.MultiColumn = !variesByYear;
        }


        /// <summary>
        /// Creates modsim TimeSeries and sets Units and VariesByYear 
        /// </summary>
        /// <param name="tstype"></param>
        /// <param name="n"></param>
        public static void CreateModsimTimeSeries(TimeSeriesType tstype, Node n, bool variesByYear, ModsimUnits units)
        {

            var ts = new Csu.Modsim.ModsimModel.TimeSeries(tstype);
            switch (tstype)
            {
                case TimeSeriesType.Demand:
                    n.m.adaDemandsM = ts;
                    break;
                case TimeSeriesType.NonStorage:
                    n.m.adaInflowsM = ts;
                    break;
                case TimeSeriesType.Targets:
                    n.m.adaTargetsM = ts;
                    break;
                case TimeSeriesType.Evaporation:
                    n.m.adaEvaporationsM = ts;
                    break;
                case TimeSeriesType.Forecast:
                    n.m.adaForecastsM = ts;
                    break;
                default:
                    throw new Exception("Only NonStorage, Demand, Targets, Forecast, and Evaporation supported");
            }

            ts.VariesByYear = variesByYear;
            ts.MultiColumn = !variesByYear;
            ts.units = units;
        }


        public static DateTime IncrementDate(Model mi, DateTime date)
        {
            switch (mi.timeStep.TSType)
            {
                case ModsimTimeStepType.Daily:
                    date = date.AddDays(1);
                    break;
                case ModsimTimeStepType.Weekly:
                    date = date.AddDays(7);
                    break;
                case ModsimTimeStepType.Monthly:
                    date = date.AddMonths(1);
                    break;
                default:
                    throw new System.Exception("Only daily, weekly, or monthly timesteps supported");
            }
            return date;
        }


        public static DateTime DecrementDate(Model mi, DateTime date)
        {
            switch (mi.timeStep.TSType)
            {
                case ModsimTimeStepType.Daily:
                    date = date.AddDays(-1);
                    break;
                case ModsimTimeStepType.Weekly:
                    date = date.AddDays(-7);
                    break;
                case ModsimTimeStepType.Monthly:
                    date = date.AddMonths(-1);
                    break;
                default:
                    throw new System.Exception("Only daily, weekly, or monthly timesteps supported");
            }
            return date;
        }


        public static long RoundAndScale(Model mi, double val)
        {
            long rval;
            double d;
            int scale = Convert.ToInt32(mi.CalcScaleFactor());
            int precision = GetPrecisionFromScale(scale);

            d = System.Math.Round(val, precision, MidpointRounding.ToEven) * scale;
            rval = Convert.ToInt64(d);
            
            return rval;
        }


        public static int GetPrecisionFromScale(int val)
        {
            int rval = 1;
            int number = System.Math.Abs(val);
            while ((number /= 10) >= 1)
                rval++;
            return rval - 1;
        }


        /// <summary>
        /// fill series MODSIM style, where first entry must have a specified startDate.
        /// missing data is input as -999
        /// </summary>
        public static void FillModsimStyle(Model mi, Series series, DateTime startDate, DateTime endDate)
        {
            if (series.Count == 0)
            {
                return;
            }

            DateTime t = startDate;
            double prevValue = -999;
            int prevIndex = -1;
            int idx = 0;
            while (t < endDate)
            {
                idx = series.IndexOf(t);

                if (idx >= 0)
                {
                    prevIndex = idx;
                    prevValue = series[idx].Value;
                }
                else
                {
                    Point pt = new Point(t, prevValue);
                    series.InsertAt(pt, prevIndex + 1);
                    prevIndex++;
                }

                t = mi.timeStep.IncrementDate(t);
            }
        }


        /// <summary>
        /// fill series, where first entry must have a specified startDate.
        /// missing data is infilled as 0
        /// </summary>
        public static void FillSeriesZeros(Model mi, Series series, DateTime startDate, DateTime endDate)
        {
            if (series.Count == 0)
            {
                return;
            }

            double missingValue = 0;
            int idx = 0;

            DateTime t = startDate;
            while (t < endDate)
            {
                if (series.IndexOf(t) < 0)
                {
                    Point pt = new Point(t, missingValue);
                    series.InsertAt(pt, idx);
                }

                idx++;
                t = mi.timeStep.IncrementDate(t);
            }
        }
    }
}
