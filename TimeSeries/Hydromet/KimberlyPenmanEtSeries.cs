using System;

namespace Reclamation.TimeSeries.Hydromet
{
    static class KimberlyPenmanEtSeries
    {

        /// <summary>
        /// Main calculation procedure. Refer to "Green Book" publication outlining 1982
        /// Kimberly-Penman ET Equations dated February 1994.
        /// </summary>
        public static Point Calculate(DateTime t, Series avgTemp,
            Series minTemp, Series maxTemp, Series dewTemp, Series wind,
            Series solar, double latitude, double z)
        {
            if (minTemp.IsEmpty || maxTemp.IsEmpty  || wind.IsEmpty ||
                dewTemp.IsEmpty || solar.IsEmpty || avgTemp.IsEmpty ||
                MissingDate(minTemp,t) || MissingDate(maxTemp,t) || MissingDate(wind,t) ||
                MissingDate(dewTemp,t) || MissingDate(solar,t) || MissingDate(avgTemp,t)   )
            {
                Console.WriteLine("Error: Missing data in Kimberly-Penman ET");
                return new Point(t, Point.MissingValueFlag, PointFlag.Missing);
            }

            // Define equation variables which rely on collected "Daily" data.
            double minTempData;
            double maxTempData;
            double windData;
            double dewTempData;
            double avgTempData;
            double R_s1;
            double ETr;
            minTempData = (minTemp[t].Value - 32.0) * 5.0 / 9.0;
            maxTempData = (maxTemp[t].Value - 32.0) * 5.0 / 9.0;
            // assumes 2m measurement height
            windData = wind[t].Value * 1.6093; // convert from mile/day to km/day
            dewTempData = (dewTemp[t].Value - 32.0) * 5.0 / 9.0;

            avgTempData = (avgTemp[t].Value - 32.0) * 5.0 / 9.0;

            R_s1 = solar[t].Value * 0.04184;
            //DateTime t = minTemp[t].DateTime.Date;

            DateTime tm1 = t.AddDays(-1);
            DateTime tm2 = t.AddDays(-2);
            DateTime tm3 = t.AddDays(-3);

            //Exceptions to terminate calculation when any of the values required is missing.
            //Added exceptions for potantially erroneous values for WR and SR rollovers.
            if (minTemp[t].IsMissing || maxTemp[t].IsMissing || wind[t].IsMissing ||
                dewTemp[t].IsMissing || solar[t].IsMissing || avgTemp[t].IsMissing ||
               MissingDate( avgTemp,tm1)|| MissingDate(avgTemp,tm2) ||
               MissingDate( avgTemp,tm3))
            {
                return new Point(t, Point.MissingValueFlag, PointFlag.Missing);
            }
            if (wind[t].Value < 0 || solar[t].Value > 1000 || solar[t].Value < 0)
            {
                return new Point(t, Point.MissingValueFlag, PointFlag.Missing);
            }

            // Calculate variables which rely on input data.
            double jDay = t.Date.DayOfYear;
            double delta = Delta(avgTempData);

            double P = Pressure(z);
            double lambda = Lambda(avgTempData);
            double gamma = Gamma(P, lambda);
            double e_a = SatVPressure(dewTempData);
            double e_s = (SatVPressure(maxTempData) + SatVPressure(minTempData)) / 2.0;
            double W_f = WindFunc(windData, jDay);
            double alpha = Albedo(jDay);
            double R_s = R_s1;
            double R_b = Rb_Calc(e_a, maxTempData, minTempData, jDay, z, R_s, latitude);
            double G = GetG(avgTemp, t);

            // ETr Calculation.
            double R_n = (1.0 - alpha) * R_s - R_b;
            double fact1 = delta / (delta + gamma);
            double fact2 = 1.0 - fact1;
            double firstTerm = fact1 * (R_n - G);
            double secondTerm = fact2 * (6.43 * W_f * (e_s - e_a));
            //double firstTerm = delta * (R_n - G) / (delta + gamma);
            //double secondTerm = gamma * 6.43 * W_f * (e_s - e_a) / (delta + gamma);
            double ETr1 = (firstTerm + secondTerm) / lambda;
            ETr = ETr1 * 0.0393700787; // Conversion from mm/day to in/day.

            if (ETr < 0.0)
            {
                ETr = 0.0;
            }
            return new Point(t, System.Math.Round(ETr, 2), PointFlag.Computed);
        }

        private static bool MissingDate(Series s, DateTime t)
        {
            return s.IndexOf(t) < 0;
        }
        
        /// <summary>
        /// Solar calculation using Fortran method
        /// </summary>
        private static double Rb_Calc(double e_a, double maxTempData, double minTempData, double jDay,
           double z, double R_s, double latitude)
        {
            double Rnl1 = (System.Math.Pow(maxTempData + 273.16, 4.0) +
                System.Math.Pow(minTempData + 273.16, 4.0)) / 2.0;
            double inner = System.Math.Pow(0.0154 * (jDay - 177), 2.0);
            double a1 = 0.26 + 0.1 * System.Math.Exp(-1 * inner);
            double Rnl2 = a1 - 0.139 * System.Math.Sqrt(e_a);
            double R_bo = Rnl2 * 4.903e-9 * Rnl1;
         //// Rso calc using ASCE-EWRI Equation
            double del = 0.409 * System.Math.Sin(2 * System.Math.PI * jDay / 365 - 1.39);

            double phi = latitude * System.Math.PI / 180.0;

            double ws = System.Math.Acos(System.Math.Tan(del) * -System.Math.Tan(phi));
            double woah = ws * System.Math.Sin(phi) * System.Math.Sin(del) +
                System.Math.Cos(phi) * System.Math.Cos(del) * System.Math.Sin(ws);
            double Gsc = 4.92; // Solar Constant MJ/(m^2*Hr) units
            double Dr = 1 + 0.033 * System.Math.Cos(2 * System.Math.PI * jDay / 365);
            double R_a = Gsc * Dr * woah * 24 / System.Math.PI;
            double R_so = R_a * (0.75 + 2e-5 * z);
         // Rso calc using CLIMAT.DAT
       
            //double C1; double C2; double C3; double C4; double C5;
            //GetClimatData(cbtt, out C1, out C2, out C3, out C4, out C5);


            //double R_so = (C1 + C2 * jDay + C3 * System.Math.Pow(jDay, 2.0) + C4 * System.Math.Pow(jDay, 3.0)
            //    + C5 * System.Math.Pow(jDay, 4.0)) * 0.04184;
            double a;
            double b;
            double Rs_Rso = R_s / R_so;
            if (Rs_Rso > 1.0) { Rs_Rso = 1.0; }
                if (Rs_Rso > 0.7)
                { a = 1.126; b = -0.07; }
                else { a = 1.017; b = -0.06; }
            double R_b = R_bo * ((a * Rs_Rso) + b);
            return R_b;
        }


        
        private static double Delta(double meanTempData)
        {
            double delta = 0.2*System.Math.Pow(0.00738*meanTempData+0.8072,7.0)-0.000116;
            return delta;
        }


        //private static double Elev(string cbtt)
        //{
        //    double z = Convert.ToDouble(Reclamation.TimeSeries.Hydromet.HydrometInfoUtility.LookupElevation(cbtt)) * .3048;
        //    return z;
        //}


        private static double Pressure(double z)
        {
            double P = 101.3 * (System.Math.Pow((288 - 0.0065 * z) / 288.0, 5.257));
            return P;
        }


        private static double Lambda(double meanTempData)
        {
            double lambda = 2.501 - 0.002361 * meanTempData;
            return lambda;
        }


        private static double Gamma(double P, double lambda)
        {
            double gamma = 0.001005 * P / (0.622 * lambda);
            return gamma;
        }


        private static double Albedo(double jDay)
        {
            double albedo = 0.29 + 0.06 * System.Math.Sin((jDay + 97.92) * System.Math.PI / 180.0);
            return albedo;
        }


        private static double WindFunc(double windData, double jDay)
        {
            double a_w = 0.4 + 1.4 * System.Math.Exp(-System.Math.Pow(((jDay - 173.0) / 58.0), 2.0));
            double b_w = 0.007 + 0.004 * System.Math.Exp(-System.Math.Pow(((jDay - 243.0) / 80.0), 2.0));
            double W_f = a_w + b_w * windData;
            return W_f;
            
        }


        private static double SatVPressure(double temp)
        {
            double e1 = System.Math.Pow(0.00738 * temp + 0.8072, 8.0);
            double e2 = 0.000019 * System.Math.Abs(1.8 * temp + 48.0);
            double e_o = 3.38639 * (e1 - e2 + 0.001316);
            return e_o;
        }


        private static double GetG(Series avgTemp, DateTime t)
        {
            DateTime tm1 = t.AddDays(-1);
            DateTime tm2 = t.AddDays(-2);
            DateTime tm3 = t.AddDays(-3);

            double Tpr = (avgTemp[tm1].Value + avgTemp[tm2].Value + avgTemp[tm3].Value) / 3.0;

            double G = 0.377 * (avgTemp[t].Value - Tpr) * 5.0 / 9.0; ;
            return G;
        }


    }
}