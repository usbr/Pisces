using System;
using System.Collections.Generic;
using Reclamation.Core;
using System.IO;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;

namespace JonsTestPrograms
{
    public class AsceInstantEtCalculator
    {
        private string cbtt;

        public AsceInstantEtCalculator(string cbtt)
        { this.cbtt = cbtt; }

        // These values are used to evaluate the example data in the ASCE Standard's Appendix C
        private static bool jrTest = false;
        private static double hourtest = 1.0;
        private static double temptest = 16.5;
        private static double vaportest = 1.26;
        private static double solartest = 0.00;
        private static double windtest = 0.50;
        private static double jdaytest = 184.0;

        /// <summary>
        ///Tall-Crop ET Entry Point
        /// </summary>
        public Series ETrs(DateTime t1, DateTime t2)
        { return ET(t1, t2, "tall"); }


        /// <summary>
        /// Short-Crop ET Entry Point
        /// </summary>
        public Series ETos(DateTime t1, DateTime t2)
        { return ET(t1, t2, "short"); }
        

        /// <summary>
        /// Loops through the date range to calculate ET values
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="refCrop"></param>
        /// <returns></returns>
        private Series ET(DateTime t1, DateTime t2, string refCrop)
        {
            Series rval = new Series();
            rval.TimeInterval = TimeInterval.Irregular;

            Series dewTemp;
            Series windRun; Series avgTemp; Series R_s1All;
            ReadAllData(t1, t2, out dewTemp,
                out windRun, out avgTemp, out R_s1All);
            double z = Convert.ToDouble(Reclamation.TimeSeries.Hydromet.HydrometInfoUtility.LookupElevation(cbtt)) * .3048;
            double lat = SiteLatitude(cbtt);
            double lon = SiteLongitude(cbtt);
            
            if (jrTest) //JR Test using the Standard's Appendix C data
            { z = 1462.4; }
            
            while (t1 <= t2)
            {
                var myEt = Calculate(t1, avgTemp, dewTemp, windRun, R_s1All, lat, lon, z, refCrop);
                var num = myEt.Value;
                if (!jrTest)
                { num = System.Math.Round(myEt.Value, 4) / 4.0; }//Output is hourly, converts to 15min value
                rval.Add(t1, num);
                t1 = t1.AddMinutes(15);
            }

            return rval;
        }


        /// <summary>
        /// Refer to ASCE-EWRI Penman-Montieth Standardized Reference ET Equation
        /// This follows the calculation steps outlined in the standard
        /// </summary>
        private static Point Calculate(DateTime t, Series avgTemp, Series dewTemp, Series windRun, Series solar, 
            double latitude, double longitude, double z, string refCrop)
        {
            double avgTempData = avgTemp[t].Value;
            double windData = windRun[t].Value;
            double dewTempData = dewTemp[t].Value;
            double R_s1 = solar[t].Value;

            if (jrTest) //JR Test using the Standard's Appendix C data
            {
                windData = windtest  / 0.44704;
                avgTempData = 32.0 + temptest * 9.0 / 5.0;
                R_s1 = solartest / 0.04184;
            }

            // Site elevation required to calculate [gamma].
            double gamma = Gamma(z);

            double T = (avgTempData - 32.0) * 5.0 / 9.0;

            double e_s = SatVPressure(T);
            double delta = (2503.0 * System.Math.Exp((17.27 * T / (T + 237.3)))) /
                System.Math.Pow((T + 237.3), 2.0);

            // [e_a] calculation. Currently set to Method 2 as defined by ASCE-EWRI.
            double e_a = MeanVPressure(dewTempData);
            if (jrTest) //JR Test using the Standard's Appendix C data
            { e_a = vaportest; }

            // Daily average wind speed. [u2]
            double u2 = windData * 0.44704; // mph to m/s unit conversion
            u2 = u2 * 4.87 / (System.Math.Log(67.8 * 3.0 - 5.42, System.Math.E));// wind-profile relationship equation

            // [Rn] Net Radiation calculation.
            double siteRad = latitude * System.Math.PI / 180.0;

            double R_n = SolarRad(siteRad, T, z, e_a, t, R_s1, longitude);

            // [Cn], [Cd], & [G] varies with tall/short crop reference and nighttime/daytime periods.
            double Cd = 0.0;
            double Cn = 0.0;
            double G = 0.0;
            // ASCE Standard Table1 TALL (ETrs) Crop Reference
            // Daytime      Cd = 0.25   Cn = 66     G = 0.04 * Rn
            // Nighttime    Cd = 1.70   Cn = 66     G = 0.20 * Rn
            // ASCE Standard Table1 SHORT (ETos) Crop Reference
            // Daytime      Cd = 0.24   Cn = 37     G = 0.10 * Rn
            // Nighttime    Cd = 0.96   Cn = 37     G = 0.50 * Rn
            if (refCrop == "tall")
            {
                Cn = 66.0;
                if (R_n < 0.0) // Nighttime definition on Pg.44
                {
                    Cd = 1.70;
                    G = 0.20 * R_n;
                }
                else // Daytime definition Rn > 0.0
                {
                    Cd = 0.25;
                    G = 0.04 * R_n;
                }
            }
            else if (refCrop == "short")
            {
                Cn = 37.0;
                if (R_n < 0.0) // Nighttime definition on Pg.44
                {
                    Cd = 0.96;
                    G = 0.50 * R_n;
                }
                else // Daytime definition Rn > 0.0
                {
                    Cd = 0.24;
                    G = 0.10 * R_n;
                }
            }

            // [ET_sz] Calculation using ASCE-EWRI Equation.
            double et1 = 0.408 * delta * (R_n - G);
            double et2 = gamma * u2 * Cn * (e_s - e_a) / (T + 273.0); // Uses absolute temperature conversion.
            double et3 = delta + gamma * (1 + Cd * u2);
            double ETr = ((et1 + et2) / et3) * 0.0393700787; // Conversion from mm/day to in/day.

            if (jrTest) //JR Test using the Standard's Appendix C data
            { ETr = ETr / 0.0393700787; }
            else
            { if (ETr < 0) { ETr = 0; } }

            Point rval = new Point(t, ETr);
            return rval;
            //return ETr;
        }


        /// <summary>
        /// Imports all required Series data to satisfy required equation inputs.
        /// </summary>
        private void ReadAllData(DateTime t1, DateTime t2, out Series dewTempDataAll, out Series windDataAll,
            out Series avgTempDataAll, out Series R_s1All)
        {
            string cbtt = this.cbtt;
            dewTempDataAll = new HydrometInstantSeries(cbtt, "TP");//Dew Point Temp (DegF)
            dewTempDataAll.Read(t1, t2);
            windDataAll = new HydrometInstantSeries(cbtt, "WS");//Wind Speed (mph)
            windDataAll.Read(t1, t2);
            avgTempDataAll = new HydrometInstantSeries(cbtt, "OB");//Average Temp (DegF)
            avgTempDataAll.Read(t1, t2);
            R_s1All = new HydrometInstantSeries(cbtt, "SI");//Solar Radiation (MJ/m2/hr)
            R_s1All.Read(t1, t2);
        }


        /// <summary>
        /// Solar calculation.
        /// </summary>
        private static double SolarRad(double phi, double T, double z, double e_a,
            DateTime t1, double R_s1, double lon)
        {
            // Constants
            double Gsc = 4.92; // Solar Constant MJ/(m^2*Hr) units
            double sigma = 2.042 * System.Math.Pow(10.0, -10.0); // Boltzmann Constant. Crazy unit on {Pg.34}
            double alpha = 0.23; // Albedo defined by ASCE-EWRI {Pg.33}

            // {Pg.17} of ASCE-EWRI Standard.[R_n] Net Radiation calculation.
            double R_nl1 = (0.34 - 0.14 * System.Math.Sqrt(e_a)) * System.Math.Pow(T + 273.16, 4.0);
            double jDay = t1.DayOfYear;

            if (jrTest) //JR Test using the Standard's Appendix C data
            { jDay = jdaytest; }
            
            double Dr = 1 + 0.033 * System.Math.Cos(2.0 * System.Math.PI * jDay / 365.0);
            double del = 0.409 * System.Math.Sin(2.0 * System.Math.PI * jDay / 365.0 - 1.39);

            // Hourly considerations from Pg.40 starts here...
            double Lm = lon;
            double Lz = 0.0;
            //Time-zone estimate from longitude value
            if (Lm > 67.5 && Lm < 82.5) { Lz = 75.0; }
            else if (Lm > 67.5 && Lm < 97.5) { Lz = 90.0; }
            else if (Lm > 67.5 && Lm < 112.5) { Lz = 105.0; }
            else if (Lm > 67.5 && Lm < 127.5) { Lz = 120.0; }
            else { throw new Exception("Site longitude not supported by the ASCE equation. Refer to Pg.39 of the standard."); }
            double b = 2.0 * System.Math.PI * (jDay - 81.0) / 364.0;
            double Sc = 0.1645 * System.Math.Sin(2.0 * b) - 0.1255 * System.Math.Cos(b) - 0.025 * System.Math.Sin(b);
            double t = t1.Hour - 0.50;//this is since we are doing an hourly ET calc. See Pg.41 note on 't'
            
            if (jrTest) //JR Test using the Standard's Appendix C data
            { t = hourtest - 0.50; }

            double w = System.Math.PI * ((t + 0.06667 * (Lz - Lm) + Sc) - 12.0) / 12.0;
            double t_1 = 1.0; //t for hourly periods Pg.40
            double w1 = w - (System.Math.PI * t_1 / 24.0);
            double w2 = w + (System.Math.PI * t_1 / 24.0);
            double ws = System.Math.Acos(System.Math.Tan(del) * -System.Math.Tan(phi));
            if (w1 < -1.0 * ws) { w1 = -1.0 * ws; }
            if (w2 < -1.0 * ws) { w2 = -1.0 * ws; }
            if (w1 > ws) { w1 = ws; }
            if (w2 > ws) { w2 = ws; }
            if (w1 > w2) { w1 = w2; }

            double woah = (w2 - w1) * System.Math.Sin(phi) * System.Math.Sin(del) + System.Math.Cos(phi) *
                System.Math.Cos(del) * (System.Math.Sin(w2) - System.Math.Sin(w1));
            double R_a = Gsc * Dr * woah * 12.0 / System.Math.PI;
            double R_so = R_a * (0.75 + 2e-5 * z);
            double R_s = R_s1 * 0.04184;

            if (R_so == 0.0) { R_so = 1.0; }
            double Rs_Rso = R_s / R_so;
            if (Rs_Rso < 0.3) { Rs_Rso = 0.3; }
            else { }
            if (Rs_Rso > 1.0) { Rs_Rso = 1.0; }
            else { }

            //Fcd has additional considerations that have yet to be incorporated here; Pg.35-36 & Pg.43 of the standard 
            // has the narrative and equations
            //JR: Might not be a big deal to account for Fcd since it usually only "tunrs-on" during nighttime when there is usually
            // no solar readings and negative values are zeroed-out anyway.
            double beta = System.Math.Asin(System.Math.Sin(phi) * System.Math.Sin(del) + System.Math.Cos(phi) * System.Math.Cos(del) *
                System.Math.Cos(w));
            double Fcd = (1.35 * R_s / R_so) - 0.35;
            if (Fcd < 0.05) { Fcd = 0.05; }
            if (Fcd > 1.0) { Fcd = 1.0; }

            if (w > ws)
            { Rs_Rso = GetSunsetRsRso(phi, z, t1, R_s1, lon); }

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
            // Method 2: Using average DewPointTemp to calculate [e_a]
            double dewTemp = (dewTempData - 32.0) * 5.0 / 9.0;
            double e_a = SatVPressure(dewTemp);

            return e_a;
        }


        private static double SiteLatitude(string cbtt)
        {
            string siteLatIn = Reclamation.TimeSeries.Hydromet.HydrometInfoUtility.LookupLatitude(cbtt).PadLeft(7);
            //string siteLatIn = "423246".PadLeft(7);
            char[] deg1 = new char[3];
            char[] min1 = new char[2];
            char[] sec1 = new char[2];
            StringReader sr = new StringReader(siteLatIn);
            sr.Read(deg1, 0, 3);
            sr.Read(min1, 0, 2);
            sr.Read(sec1, 0, 2);
            string deg = new string(deg1);
            string min = new string(min1);
            string sec = new string(sec1);
            double siteDMS = Convert.ToDouble(deg) + Convert.ToDouble(min) / 60.0 + Convert.ToDouble(sec) / 3600.0;
            
            if (jrTest) //JR Test using the Standard's Appendix C data
            { siteDMS = 40.41; }

            return siteDMS;
        }


        private static double SiteLongitude(string cbtt)
        {
            string siteLongIn = Reclamation.TimeSeries.Hydromet.HydrometInfoUtility.LookupLongitude(cbtt).PadLeft(7);
            char[] deg1 = new char[3];
            char[] min1 = new char[2];
            char[] sec1 = new char[2];
            StringReader sr = new StringReader(siteLongIn);
            sr.Read(deg1, 0, 3);
            sr.Read(min1, 0, 2);
            sr.Read(sec1, 0, 2);
            string deg = new string(deg1);
            string min = new string(min1);
            string sec = new string(sec1);
            double siteDMS = Convert.ToDouble(deg) + Convert.ToDouble(min) / 60.0 + Convert.ToDouble(sec) / 3600.0;

            if (jrTest) //JR Test using the Standard's Appendix C data
            { siteDMS = 104.78; }

            return siteDMS;
        }


        private static double Gamma(double z)
        {
            double atmP = 101.3 * System.Math.Pow(((293.0 - 0.0065 * z) / 293.0), 5.26);
            double gamma = 0.000665 * atmP;
            return gamma;
        }


        private static double GetSunsetRsRso(double phi, double z, DateTime t1, double R_s1, double lon)
        {
            double Rs_Rso = 0.0;

            // Constants
            double Gsc = 4.92; // Solar Constant MJ/(m^2*Hr) units

            // {Pg.17} of ASCE-EWRI Standard.[R_n] Net Radiation calculation.
            double jDay = t1.DayOfYear;

            if (jrTest) //JR Test using the Standard's Appendix C data
            { jDay = jdaytest; }

            double Dr = 1 + 0.033 * System.Math.Cos(2.0 * System.Math.PI * jDay / 365.0);
            double del = 0.409 * System.Math.Sin(2.0 * System.Math.PI * jDay / 365.0 - 1.39);

            // Hourly considerations from Pg.40 starts here...
            double Lm = lon;
            double Lz = 0.0;
            //Time-zone estimate from longitude value
            if (Lm > 67.5 && Lm < 82.5) { Lz = 75.0; }
            else if (Lm > 67.5 && Lm < 97.5) { Lz = 90.0; }
            else if (Lm > 67.5 && Lm < 112.5) { Lz = 105.0; }
            else if (Lm > 67.5 && Lm < 127.5) { Lz = 120.0; }
            else { throw new Exception("Site longitude not supported by the ASCE equation. Refer to Pg.39 of the standard."); }
            double b = 2.0 * System.Math.PI * (jDay - 81.0) / 364.0;
            double Sc = 0.1645 * System.Math.Sin(2.0 * b) - 0.1255 * System.Math.Cos(b) - 0.025 * System.Math.Sin(b);
            double t = t1.Hour - 4.50;//this is since we are doing an hourly ET calc. See Pg.41 note on 't'
                                        // This is so that we follow the convention in Appendix C of the standard Pg.C-10
            if (jrTest) //JR Test using the Standard's Appendix C data
            { t = hourtest - 0.50; }

            double w = System.Math.PI * ((t + 0.06667 * (Lz - Lm) + Sc) - 12.0) / 12.0;
            double t_1 = 1.0; //t for hourly periods Pg.40
            double w1 = w - (System.Math.PI * t_1 / 24.0);
            double w2 = w + (System.Math.PI * t_1 / 24.0);
            double ws = System.Math.Acos(System.Math.Tan(del) * -System.Math.Tan(phi));
            if (w1 < -1.0 * ws) { w1 = -1.0 * ws; }
            if (w2 < -1.0 * ws) { w2 = -1.0 * ws; }
            if (w1 > ws) { w1 = ws; }
            if (w2 > ws) { w2 = ws; }
            if (w1 > w2) { w1 = w2; }

            double woah = (w2 - w1) * System.Math.Sin(phi) * System.Math.Sin(del) + System.Math.Cos(phi) *
                System.Math.Cos(del) * (System.Math.Sin(w2) - System.Math.Sin(w1));
            double R_a = Gsc * Dr * woah * 12.0 / System.Math.PI;
            double R_so = R_a * (0.75 + 2e-5 * z);
            double R_s = R_s1 * 0.04184;

            if (R_so == 0.0) { R_so = 1.0; }
            Rs_Rso = R_s / R_so;
            if (Rs_Rso < 0.3) { Rs_Rso = 0.3; }
            else { }
            if (Rs_Rso > 1.0) { Rs_Rso = 1.0; }
            else { }

            return Rs_Rso;
        }

    }
}















