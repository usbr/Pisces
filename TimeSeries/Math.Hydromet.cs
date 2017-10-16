using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet.Operations;
#if !NETCOREAPP2_0
using Reclamation.TimeSeries.Nrcs;
#endif
using Reclamation.TimeSeries.Parser;
using System;

namespace Reclamation.TimeSeries
{
    public static partial class Math
    {

        [FunctionAttribute("Hydromet daily data",
        "HydrometDaily(cbtt,pcode)")]
        public static Series HydrometDaily(string cbtt, string pcode)
        {
            return new Hydromet.HydrometDailySeries(cbtt, pcode);
        }

        [FunctionAttribute("Hydromet instant data (15 minute)",
        "HydrometInstant(cbtt,pcode)")]
        public static Series HydrometInstant(string cbtt, string pcode)
        {
            return new Hydromet.HydrometInstantSeries(cbtt, pcode);
        }


        /// <summary>
        /// Converts Monthly forecasts into a single daily series.
        /// Hydromet midmonth forecast is stored in a separate monthly series.
        /// HydrometForecastMonthlyToDaily Function combines the two monthly into a single
        /// Series on a daily time stamp.  fc is stored at first of month, fcm is stored at the 16th of the
        /// month.  The other dates of the month are entered missing.
        /// </summary>
        /// <param name="fc"></param>
        /// <param name="fcm"></param>
        /// <returns></returns>
        [FunctionAttribute("Converts Monthy Forecast points to a sparse daily series",
        "HydrometForecastMonthlyToDaily(fc,fcm)")]
        public static Series HydrometForecastMonthlyToDaily(Series fc, Series fcm=null)
        {
            var rval = new Series();
            rval.TimeInterval = TimeInterval.Daily;

            foreach (Point pt in fc)
            {
                if (!pt.IsMissing )
                    rval.Add(pt.DateTime.FirstOfMonth(), pt.Value);
            }

            if (fcm != null)
            {
                foreach (Point pt in fcm)
                {
                    if (!pt.IsMissing)
                        rval.Add(pt.DateTime.MidMonth(), pt.Value);
                }
            }

            return rval;
        }



        /// <summary>
        /// Computes Residual Forecast by starting with a known forecast such 
        /// as 100,000 acre-feet on February 1, and subtracting each day
        /// the runoff that has occured.  Required to have at least 30 days of data.
        /// </summary>
        /// <param name="forecast">Daily Forecast(acre-feet) computed from Monthly forecast</param>
        /// <param name="runoff">Daily runoff gage (cfs)</param>
        /// <param name="residual">Residual forecast - input to initilize calculations</param>
        /// <returns>residual forecaset in acre-feet</returns>
        [FunctionAttribute("Compute Residual Forecast", "HydrometResidualForecast(forecast,runoff,residual)")]
        public static Series HydrometResidualForecast(Series forecast, Series runoff, Series residual)
        {
            var rval = new Series();
            rval.TimeInterval = TimeInterval.Daily;

            if (runoff.Count < 30 )
            {
                Logger.WriteLine("Missing or not enough runoff data: HydrometResidualForecast()");
                return residual;
            }


            if (residual.Count == 0)
            {
                residual.Add(runoff[0].DateTime, 0);
            }
            double resid = residual[0].Value;

            

            for (int i = 0; i < runoff.Count; i++)
            {
                var t = runoff[i].DateTime;

                resid = ResetResidualBasedOnForecast(t, forecast, resid);

                bool missing = Point.IsMissingValue(resid);

                if (!missing && !runoff[i].IsMissing && t <=runoff.MaxDateTime)
                {

                    var quTemp = runoff[t].Value;
                    if (quTemp < 0)
                        quTemp = 0;
                    resid = resid - quTemp * 1.98347;
                    if (resid < 0)
                        resid = 0;

                    rval.Add(t, resid);
                }
                else
                {
                    rval.AddMissing(t);
                    Console.WriteLine("Missing data: incremental: " + runoff[i].ToString());
                    missing = true;
                }

            }
            return rval;
        }

        private static double ResetResidualBasedOnForecast(DateTime t,Series fc, double resid)
        {
            if (fc.IndexOf(t) >= 0 && !fc[t].IsMissing )
                     resid = fc[t].Value;

            if (t == t.FirstOfMonth() &&  // residual should not keep going.
                (fc.IndexOf(t) < 0 || fc[t].IsMissing))
                return Point.MissingValueFlag;

            return resid;
        }


        /// <summary>
        /// Based on the residual compute the Curve values.
        /// </summary>
        /// <param name="residual"></param>
        /// <returns></returns>
        [FunctionAttribute("Compute Residual Forecast", "HydrometResidualForecast(cbtt,pcode)")]
        public static Series HydrometRuleCurve(string curveName,Series residual)
        {
            var rval = new Series();
            rval.TimeInterval = TimeInterval.Daily;


            if (residual.Count == 0)
                return rval;

            var m_ruleCurve = new HydrometRuleCurve(curveName);

            DateTime t = residual[0].DateTime.Date;
            DateTime t2 = residual.MaxDateTime.Date;

            if (t2 > DateTime.Now.Date.AddDays(-1))
                t2 = DateTime.Now.Date.AddDays(-1);

            var req = Point.MissingValueFlag;
            string flag = "";
            while (t <= t2)
            {
                req = m_ruleCurve.LookupRequiredSpace(t, residual[t].Value, out flag);

                rval.Add(t,req);
                t = t.AddDays(1);
            }


            return rval;
        }

#if !NETCOREAPP2_0
        [FunctionAttribute("Reads daily NRCS snowcourse data into monthly", "DailySnowCourseToMonthly(triplet)")]
        public static Series DailySnowCourseToMonthly(string triplet)
        {
            var s = new MonthlySnowCourseSeries(triplet);
            return s;
        }
#endif
        /// <summary>
        /// CipolettiWeir
        /// https://www.usbr.gov/pmts/hydraulics_lab/pubs/wmm/chap07_12.html
        /// </summary>
        /// <param name="head"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        [FunctionAttribute("CipolettiWeir equation 3.367*length*h^1.5", "CipolettiWeir(head,length)")]
        public static Series CipolettiWeir(Series head, double length)
        {
            var s = new Series();
            var h = head.Copy();
            h.RemoveMissing(true);
            s = Math.Pow(Math.Max(h, 0.0), 1.5) * length * 3.367;
            return s;
        }

        /// <summary>
        /// Standard Contracted Rectangular Weir Equation
        /// https://www.usbr.gov/pmts/hydraulics_lab/pubs/wmm/
        /// </summary>
        /// <param name="head"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        [FunctionAttribute("Fully Contracted Rectangular Weir (Francis equation) Q=3.33*h^1.5*(length-.2*h)", "RectangularContractedWeir(head,length)")]
        public static Series RectangularContractedWeir(Series head, double length)
        {
            var s = new Series();
            var h = head.Copy();
            h.RemoveMissing(true);
            s = (h * -.2 + length) * 3.33 * Math.Pow(Math.Max(h, 0.0), 1.5);
            return s;
        }


        [FunctionAttribute("Generic weir equation width_factor*(head+offset+shift)^exponent ", "GenericWeir(head,offset,width_factor,exponent)")]
        public static Series GenericWeir(Series head,double offset, double width_factor, double exponent)
        {
            double shift = Convert.ToDouble(head.Properties.Get("shift", "0"));
            var s = new Series();
            var h = head.Copy();
            h.RemoveMissing(true);
            h = h + offset + shift;
            s = Math.Pow(Math.Max(h,0), exponent) * width_factor;
            return s;
        }


        [FunctionAttribute("LookupShift from series properties ", "LookupShift(ch)")]
        public static Series LookupShift(Series ch)
        {
            double shift = Convert.ToDouble(ch.Properties.Get("shift", "0"));
            var rval = ch.Copy();
            rval.RemoveMissing();
            rval = rval * 0.0;
            rval = rval + shift;

            return rval;
        }

       
    }
}
