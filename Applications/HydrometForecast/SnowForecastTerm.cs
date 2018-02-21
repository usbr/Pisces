using System;
using Reclamation.TimeSeries;
using System.Collections.Generic;
using Reclamation.Core;
using Reclamation.TimeSeries.Hydromet;

namespace HydrometForecast
{
    public class SnowForecastTerm:ForecastTerm
    {

        private List<string> snowDetails=null;
        public SnowForecastTerm()
        {
            ForecastTermType = "Snow";
            snowDetails = new List<string>();
        }



        public override List<string> Details()
        {
            if (snowDetails.Count == 0)
            {
                return base.Details();
            }

            return snowDetails;


        }
        /// <summary>
        /// Computes Snow forecast term
        /// </summary>
        /// <param name="forecastDate"></param>
        /// <param name="lookAhead"></param>
        /// <returns></returns>
        public override double Evaluate(DateTime forecastDate, bool lookAhead, double estimationScaleFactor)
        {
            Logger.WriteLine("Computing Snow Term ");
            if (MonthNames.Count != 1)
                throw new ArgumentException("Snow Term should have a single month, not a range");
            
            //lower elevations peak around March 1, and higher around April 1.
            int m = GetMonthFromString(this.MonthNames[0]);
            DateTime snowPeak = new DateTime(forecastDate.Year, m, 1);

            if (forecastDate >= snowPeak || lookAhead)
            {// use observed mpoll data (no estimation needed) 
                CreateSeriesList();
                list.Read(snowPeak, snowPeak.EndOfMonth());
                MonthlyEstimation.EstimateMissingByGroup(list, forecastDate);
                return WeightedSum();
            }
            else 
            {   // estimate snow at the peak
                // a) snow at forecast date
                // b) avg snow at forecast date
                // c) avg snow at peak date
                // d) avg accumulation  = ( c - b) * estimationScaleFactor
                // e) estimate = a + d

                snowDetails.Add(this.ForecastTermType);// Snow

                if (forecastDate.Month > 9)
                    throw new ArgumentOutOfRangeException("Error: forecast month is before january");

                bool midMonth = forecastDate.Day != 1;
                int mpollMonth = forecastDate.Month;
                if (midMonth)
                    mpollMonth++; // march 16 'mid-month' value is stored in April (mpoll)
                
                var a = CreateAndRead(forecastDate.Year,mpollMonth);
                if( !midMonth)
                   MonthlyEstimation.EstimateMissingByGroup(a, forecastDate);

                var b = CreateAndRead(9999, forecastDate.Month,true);// avg at forecaset date
                if (midMonth)
                {
                    var b2 = CreateAndRead(9999, mpollMonth, true);// avg at next month..
                    var delta = AddIgnoreDates(b2, b,true);
                    double numDays = DateTime.DaysInMonth(forecastDate.Year, forecastDate.Month);
                    double proration = (forecastDate.Day) / numDays;
                    for (int i = 0; i < b.Count; i++)
                    {
                        Reclamation.TimeSeries.Math.Multiply(delta[i], proration);
                    }
                    b = AddIgnoreDates(b, delta);
                }

                var c = CreateAndRead(9999, snowPeak.Month, true);
                var d = AddIgnoreDates(c, b,true);
                if( estimationScaleFactor != 1.0 )
                     Scale(d, estimationScaleFactor);
                list = AddIgnoreDates(a ,d);

                string msg = " ".PadRight(25) + "  wt.   " + forecastDate.ToString("MMM d").PadRight(5)
                    + " Normal"
                    + " AVG ACC "
                    + snowPeak.ToString("MMM d").PadRight(6);
                Logger.WriteLine(msg);
                snowDetails.Add(msg);

                    for (int i = 0; i < siteNames.Count; i++)
			        {
                        msg = siteNames[i].PadRight(25)
                            + siteWeights[i].ToString("F2").PadLeft(6) + " "
                            + Format(a[i][0])
                            + Format(b[i][0])
                            + Format(d[i][0]) //pt.Value.ToString("F2").PadLeft(6)
                            + list[i][0].Value.ToString("F2").PadLeft(6) + "E";

                        snowDetails.Add(msg);

                        Logger.WriteLine(msg);
			        }


                    double rval = 0;
                    try
                    {
                        rval = WeightedSum();
                    }
                    catch (Exception error)
                    {
                         msg="Error in Snow Term, computing weighted sum.  Possible missing data?";
                         Logger.WriteLine(msg);
                        throw new Exception(msg, error);
                    }

                return rval;
                
            }

        }

        private void Scale(SeriesList d, double estimationScaleFactor)
        {
            for (int i = 0; i < d.Count; i++)
            {
                Reclamation.TimeSeries.Math.Multiply(d[i], estimationScaleFactor);
            }
        }

      
        

        /// <summary>
        /// Add or Subtract ignoring dates.
        /// </summary>
        /// <param name="c"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private SeriesList AddIgnoreDates(SeriesList a, SeriesList b, bool subtract=false)
        {
            if (a.Count != b.Count)
                throw new ArgumentException("Error: number of series is not the same in the two lists");

            var rval = new SeriesList();

            for (int i = 0; i < a.Count; i++)
            {
                var sa = a[i];
                var sb = b[i];
                if (sa.Count != sb.Count)
                {
                    throw new ArgumentException("Error with subtraction: series must have the same number of points");
                }
                Series s = new Series();
                s.TimeInterval = sa.TimeInterval;
                rval.Add(s);
                for (int j = 0; j < sa.Count; j++)
                {
                    if (sa[j].IsMissing || sb[j].IsMissing)
                        s.AddMissing(sa[j].DateTime);
                    else
                    {
                        if( subtract)
                            s.Add(sa[j].DateTime, sa[j].Value - sb[j].Value);
                        else
                        s.Add(sa[j].DateTime, sa[j].Value + sb[j].Value);
                    }
                }

            }

            return rval;
        }

        private SeriesList CreateAndRead(int year, int month, bool getAverageData=false)
        {
            if (year == 9999 && getAverageData)
            {
                var rval = new SeriesList();
                for (int i = 0; i < cbttPodes.Count; i++)
                {

                    var tokens = cbttPodes[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var cbtt = tokens[0].Trim();
                    var pcode = tokens[1].Trim();
                    var d = HydrometMonthlySeries.Sum30YearRunoff(cbtt, pcode, month, month);
                    Series s = new Series("", TimeInterval.Monthly);
                    var t = new DateTime(year, 1, 1);
                    s.Add(t, d);
                    rval.Add(s);
                    }
                return rval;
            }
            else
            {
                var a = CreateSeriesList(getAverageData);
                a.Read(year, month);
                return a;
            }
        }

       

    }
}
