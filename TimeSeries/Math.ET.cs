using Reclamation.TimeSeries.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.Core;

namespace Reclamation.TimeSeries
{
    public partial class Math
    {
        /// <summary>
        /// ASCE-EWRI Penman-Montieth Standardized Reference ET Equation
        /// http://www.kimberly.uidaho.edu/water/asceewri/ASCE_Standardized_Ref_ET_Eqn_Phoenix2000.pdf
        ///  [Cn] & [Cd] varies with tall/short crop reference.
        ///  Tall (Alfalfa) Cn=1600, Cd=0.38; Short (grass) Cn=900,Cd=0.34
        /// </summary>
        /// <param name="mm">mean daily air temperature degress F</param>
        /// <param name="mn">minimum daily air temperature degress F</param>
        /// <param name="mx">maximum daily air temperature degress F</param>
        /// <param name="ym">mean daiy dew point temperature degrees F</param>
        /// <param name="wr">Wind Run  miles/day</param>
        /// <param name="windHeight">height of wind measurement in meters</param>
        /// <param name="sr">solar radiation Langleys</param>
        /// <param name="latitude">latitude in decimal degress</param>
        /// <param name="elevation">elevation in meters</param>
        /// <param name="Cd">bulk surface resistance</param>
        /// <param name="Cn">aerodynamic resistance </param>
        /// <returns></returns>
        [FunctionAttribute("Evapotranspiration ASCE Penman-Montieth:  Tall Cd=0.38, Cn=1600; Short Cd=0.34,Cn=900",
           "DailyEtAscePenmanMontieth(mm, mn, mx, ym,  wr, sr,latitude,elevation,Cd,Cn)")]
        public static Series DailyEtAscePenmanMontieth(Series mm, Series mn, Series mx,
            Series ym, Series wr, double windHeight,Series sr, double latitude, double elevation, double Cd, double Cn)
        {
            Series rval = new Series();
            rval.TimeInterval = TimeInterval.Daily;

            if (mn.Count == 0)
            {
                Console.WriteLine("Error: DailyEtAscePenmanMontieth --  input series mn  has no data");
                return rval;
            }

            DateTime t = mn[0].DateTime.Date;
            DateTime t2 = mn[mn.Count - 1].DateTime.Date;
            if (t2 > DateTime.Now.Date.AddDays(-1))
                t2 = DateTime.Now.Date.AddDays(-1);

            while (t <= t2)
            {
                var pt = Hydromet.AsceEtCalculator.Calculate(t, mm, mn, mx, ym, wr, windHeight, sr, latitude, elevation, Cd, Cn);
                rval.Add(pt);
                t = t.AddDays(1);
            }

            return rval;
        }




        /// <summary>
        /// Reference Evapotranspiration (in/day)(1982 Kimberly-Penman equation
        /// http://www.usbr.gov/pn/agrimet/aginfo/AgriMet%20Kimberly%20Penman%20Equation.pdf
        /// </summary>
        /// <param name="mm">mean daily air temperature degress F</param>
        /// <param name="mn">minimum daily air temperature degress F</param>
        /// <param name="mx">maximum daily air temperature degress F</param>
        /// <param name="ym">mean daiy dew point temperature degrees F</param>
        /// <param name="wr">Wind Run  miles/day</param>
        /// <param name="sr">solar radiation Langleys</param>
        /// <param name="latitude">latitude in decimal degress</param>
        /// <param name="elevation">elevation in meters</param>
        /// <returns></returns>
        [FunctionAttribute("Reference Evapotranspiration (in/day)(1982 Kimberly-Penman equation)",
           "DailyEtKimberlyPenman( mm, mn, mx, ym,  wr, sr,lat,elev)")]
        public static Series DailyEtKimberlyPenman( Series mm, Series mn, Series mx,
            Series ym, Series wr, Series sr, double latitude, double elevation)
        {
            Series rval = new Series();
            rval.TimeInterval = TimeInterval.Daily;

            // we need three previous days for average temp
            // throw away and re-read mm
            if (mm.Count == 0)
            {
                Console.WriteLine("Error:DailyEtKimberlyPenman  MM (max temperature) data not found");
                return rval;
            }
            if (mn.Count == 0)
            {
                Console.WriteLine("Error:DailyEtKimberlyPenman  MN (min temperature) data not found");
                return rval;
            }


              mm.Read(mm[0].DateTime.AddDays(-3), mm[mm.Count - 1].DateTime);

            rval.TimeInterval = TimeInterval.Daily;

            DateTime t = mn[0].DateTime.Date;
            DateTime t2 = mn[mn.Count-1].DateTime.Date;
            while ( t <= t2)
            {
                var pt = Hydromet.KimberlyPenmanEtSeries.Calculate(t, mm, mn, mx, ym, wr, sr, latitude, elevation);
                rval.Add(pt);
                t = t.AddDays(1);
            }

            return rval;
        }
        [FunctionAttribute("DewPointTemperature",
           "DewPointTemperature( degF, percent)")]
        public static Series DewPointTemperature(Series ob, Series tu)
        {
            Series tp = new Series();

            if (tu.Count > 0 && ob.Count > 0)
            {
                for (int i = 0; i < ob.Count; i++)
                {
                    var pt = ob[i];
                    if (tu.IndexOf(pt.DateTime) >= 0)
                    {
                        double tpVal = AsceEtCalculator.DewPtTemp(pt.Value, tu[pt.DateTime].Value);
                        if (!double.IsNaN(tpVal))
                        {
                            tp.Add(pt.DateTime, tpVal);
                        }
                        else
                        {
                            Console.WriteLine();
                        }
                    }
                }
            }
            return tp;
        }



    }
}
