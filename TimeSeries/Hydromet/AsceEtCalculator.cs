using System;
using System.Collections.Generic;
using Reclamation.Core;
using System.IO;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries.Parser;

namespace Reclamation.TimeSeries.Hydromet
{
    public static class AsceEtCalculator
    {
        //private string cbtt;
        //public AsceEtCalculator(string cbtt)
        //{
        //    this.cbtt = cbtt;
        //}



         private static Point CheckForMissing(Point pt, string name)
         {

             if (pt.IsMissing)
             {
                 Console.WriteLine("\nMissing data in ASCE-EWRI ET Method "+name+": " + pt.DateTime.ToShortDateString());
             }
             return pt;
             //return Point.MissingValueFlag;
         }

        /// <summary>
        /// Refer to ASCE-EWRI Penman-Montieth Standardized Reference ET Equation
         /// http://www.kimberly.uidaho.edu/water/asceewri/ASCE_Standardized_Ref_ET_Eqn_Phoenix2000.pdf
         ///  [Cn] & [Cd] varies with tall/short crop reference.
         ///  Tall (Alfalfa) Cn=1600, Cd=0.38; Short (grass) Cn=900,Cd=0.34
         /// </summary>
         /// <param name="t">Date to compute ET</param>
         /// <param name="avgTemp">mean daily air temperature degress F</param>
         /// <param name="minTemp">minimum daily air temperature degress F</param>
         /// <param name="maxTemp">maximum daily air temperature degress F</param>
         /// <param name="dewTemp">mean daiy dew point temperature degrees F</param>
         /// <param name="windRun">Wind Run  miles/day</param>
         /// <param name="solar">solar radiation Langleys</param>
         /// <param name="latitude">latitude in decimal degress</param>
         /// <param name="z">elevation in meters</param>
         /// <param name="Cd">bulk surface resistance</param>
         /// <param name="Cn">aerodynamic resistance </param>
         /// <param name="windHeight">wind height in meters (usually 2.0 m or 3.0 m)</param>
         /// <returns></returns>
         public static Point Calculate(DateTime t, Series avgTemp, Series minTemp,
             Series maxTemp, Series dewTemp, Series windRun,double windHeight, Series solar, double latitude, 
             double z,
             double Cd, double Cn)
        {

            if (minTemp.IsEmpty || maxTemp.IsEmpty || windRun.IsEmpty ||
               dewTemp.IsEmpty || solar.IsEmpty || avgTemp.IsEmpty ||
               MissingDate(minTemp, t) || MissingDate(maxTemp, t) || MissingDate(windRun, t) ||
               MissingDate(dewTemp,t) || MissingDate(solar,t) || MissingDate(avgTemp,t)   )
             
            {
                Console.WriteLine("\nMissing data in ASCE-EWRI ET Method " + t.ToShortDateString());
                return new Point(t, Point.MissingValueFlag, PointFlag.Missing);
            }

            // Define equation variables which rely on collected "Daily" data.
            double minTempData =  CheckForMissing(minTemp[t],"min temp").Value;
            double maxTempData = CheckForMissing(maxTemp[t],"max temp").Value;
            double windData = CheckForMissing(windRun[t],"wind run").Value;
            double dewTempData = CheckForMissing(dewTemp[t], "dew temp").Value;
            double R_s1 = CheckForMissing(solar[t],"solar").Value;

            if (avgTemp[t].IsMissing || minTemp[t].IsMissing || maxTemp[t].IsMissing || windRun[t].IsMissing 
                || dewTemp[t].IsMissing || solar[t].IsMissing)
            {
                Console.WriteLine("\nMissing data in ASCE-EWRI ET Method " + t.ToShortDateString());
                return new Point(t, Point.MissingValueFlag, PointFlag.Missing);
            }


            // Site elevation required to calculate [gamma].
            double gamma = Gamma(z);

            // minAirTemp and maxAirTemp required to calculate [T], [Delta], and [e_s].
            double minAirTemp = (minTempData - 32.0) * 5.0 / 9.0; // degF to degC conversion
            double maxAirTemp = (maxTempData - 32.0) * 5.0 / 9.0; // Ditto
            
            //double T = (minTemp + maxTemp) / 2.0; //Preferred ASCE-PM T calculation.
            double T = (avgTemp[t].Value - 32.0) * 5.0 / 9.0;    //Using archived AvgTemp to mimic Fortran output.
            
            double e_s = (SatVPressure(maxAirTemp) + SatVPressure(minAirTemp)) / 2.0;
            double delta = (2503.0 * System.Math.Exp((17.27 * T / (T + 237.3)))) /
                System.Math.Pow((T + 237.3), 2.0);

            // [e_a] calculation. Currently set to Method 2 as defined by ASCE-EWRI.
            double e_a = MeanVPressure(dewTempData);

            // Daily average wind speed. [u2]
            double u2 = windData * 0.0186266667; // m/day to mph to m/s unit conversion
          //  double windHeight = 3.0;
            u2 = u2 * 4.87 / (System.Math.Log(67.8 * windHeight - 5.42, System.Math.E));// wind-profile relationship equation

            // [G] = 0 for Daily time steps as defined by ASCE-EWRI.
            double G = 0;

            // [Rn] Net Radiation calculation.
            double siteRad = latitude * System.Math.PI / 180.0;

            double R_n = SolarRad(siteRad, maxAirTemp, minAirTemp, z, e_a, t, R_s1);


            // [Cn] & [Cd] varies with tall/short crop reference.
            //Tall Cn=1600, Cd=0.38; Short Cn=900,Cd=0.34

            // [ET_sz] Calculation using ASCE-EWRI Equation.
            double et1 = 0.408 * delta * (R_n - G);
            double et2 = gamma * u2 * Cn * (e_s - e_a) / (T + 273.0); // Uses absolute temperature conversion.
            double et3 = delta + gamma * (1 + Cd * u2);
            double ETr = ((et1 + et2) / et3) * 0.0393700787; // Conversion from mm/day to in/day.
            if (ETr < 0) { ETr = 0; }

            Point rval = new Point(t, ETr);
            return rval;
            //return ETr;
        }

         private static bool MissingDate(Series s, DateTime t)
         {
             return s.IndexOf(t) >= 0;
         }


        /// <summary>
        /// Solar calculation.
        /// </summary>
        private static double SolarRad(double phi, double maxTemp, double minTemp, double z, double e_a,
            DateTime t1, double R_s1)
        {
            // Constants
            double Gsc = 4.92; // Solar Constant MJ/(m^2*Hr) units
            double sigma = 4.901 * System.Math.Pow(10.0, -9.0); // Boltzmann Constant. Crazy unit on {Pg.19}
            double alpha = 0.23; // Albedo defined by ASCE-EWRI {Pg.33}

            // {Pg.17} of ASCE-EWRI Standard.[R_n] Net Radiation calculation.
            double R_nl1 = (0.34 - 0.14 * System.Math.Sqrt(e_a)) *
                ((System.Math.Pow(maxTemp + 273.16, 4.0) + System.Math.Pow(minTemp + 273.16, 4.0)) / 2.0);
            double jDay = t1.DayOfYear;
            double Dr = 1 + 0.033 * System.Math.Cos(2.0 * System.Math.PI * jDay / 365.0);
            double del = 0.409 * System.Math.Sin(2.0 * System.Math.PI * jDay / 365.0 - 1.39);
            double ws = System.Math.Acos(System.Math.Tan(del) * -System.Math.Tan(phi));
            double woah = ws * System.Math.Sin(phi) * System.Math.Sin(del) +
                System.Math.Cos(phi) * System.Math.Cos(del) * System.Math.Sin(ws);
            double R_a = Gsc * Dr * woah * 24.0 / System.Math.PI;
            double R_so = R_a * (0.75 + 2e-5 * z);
            double R_s = R_s1 * 0.04184;
            double Fcd = (1.35 * R_s / R_so) - 0.35;
            if (Fcd < 0.05) { Fcd = 0.05; }
            if (Fcd > 1.0) { Fcd = 1.0; }
            double R_nl = sigma * Fcd * R_nl1;
            double R_ns = R_s * (1 - alpha);
            double R_n = R_ns - R_nl;

            return R_n;
        }


        private static double SatVPressure(double temp)
        {
            double e_0 = 0.6108 * System.Math.Exp(((17.27 * temp) / (temp + 237.3)));
            return e_0;
        }


        private static double MeanVPressure(double dewTempData)
        {
            //// Preferred ASCE-PM e_a calculation.
            //HydrometInstantSeries humidityData = new HydrometInstantSeries(cbtt, "TU");
            //humidityData.Read(tempData.MinDateTime.Date, tempData.MaxDateTime.Date);
            //double e_a1 = 0; double limit;
            //if (humidityData.Count > tempData.Count)
            //{ limit = tempData.Count; }
            //else { limit = humidityData.Count; }
            //for (int i = 0; i < limit; i++)
            //{
            //    e_a1 = e_a1 + (SatVPressure_Obj((tempData.Values[i] - 32) * 5 / 9))
            //        * humidityData.Values[i] / 100;
            //}
            //double e_a = e_a1 / limit;

            // Method 2: Using average DewPointTemp to calculate [e_a]
            double dewTemp = (dewTempData - 32.0) * 5.0 / 9.0;
            double e_a = SatVPressure(dewTemp);

            return e_a;
        }


        private static double Gamma( double z)
        {
            double atmP = 101.3 * System.Math.Pow(((293.0 - 0.0065 * z) / 293.0), 5.26);
            double gamma = 0.000665 * atmP;
            return gamma;
        }

        /// <summary>
        /// Estimates Dew Point Temperature degF based on a NOAA equation
        /// http://www.srh.noaa.gov/images/epz/wxcalc/wetBulbTdFromRh.pdf 
        /// </summary>
        /// <param name="temp">Temperature in DegF</param>
        /// <param name="relHumid">Relative Hummidity in %</param>
        public static double DewPtTemp(double temp, double relHumid)
        {
            temp = (temp - 32.0) * 5.0 / 9.0;

            double satVapPress= 6.11* System.Math.Pow(10,7.5* temp/(237.7+temp));

            double dewTemp = 237.3 * System.Math.Log(satVapPress * relHumid / 611.0, System.Math.E) /
                (7.5 * System.Math.Log(10, System.Math.E) - System.Math.Log(satVapPress * relHumid / 611.0, System.Math.E));


            return 9.0/5.0 * dewTemp + 32.0;

        }

        
    }
}















