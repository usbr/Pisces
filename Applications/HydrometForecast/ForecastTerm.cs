using System;
using System.Collections.Generic;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Parser;
using System.Windows.Forms;
using Reclamation.Core;
using System.Linq;

namespace HydrometForecast
{

    public enum ForecastTermType { None,AntecedentRunoff, Precipitation, Snow, Runoff };

    public abstract class ForecastTerm
    {
        public List<double> MonthlyWeights = new List<double>();
        public List<string> MonthNames = new List<string>(); // 'oct','nov','dec'
        public int Month1=0; 
        public int Month2=0;
        public List<string> cbttPodes = new List<string>();
        public List<double> siteWeights = new List<double>();
        public List<string> siteNames = new List<string>();
        protected SeriesList list = new SeriesList();
        public bool IsXterm = true;
        public string ForecastTermType = "";
        

        public abstract double Evaluate(DateTime forecastDate, bool lookAhead, double estimationScaleFactor);

        protected double WeightedSum()
        {

            double rval = 0;
            for (int siteIndex = 0; siteIndex < list.Count; siteIndex++)
            {
                Series s = list[siteIndex];
                for (int dateIndex = 0; dateIndex < s.Count; dateIndex++)
                {
                    if (s[dateIndex].IsMissing)
                    {
                        string msg = GetErrorMessage(siteIndex, dateIndex,"Error: missing data when computing weighted sum ");
                        Logger.WriteLine(msg);
                        throw new Exception(msg);
                    }

                    if (s[dateIndex].Value < 0 )
                    {
                        var msg = GetErrorMessage(siteIndex, dateIndex, "Error: negative ignored when computing weighted sum  ");
                        Logger.WriteLine(msg);
                        Logger.WriteLine(s.ToString(true));
                        // rval += 0; 
                    }
                    else
                    {
                        rval += s[dateIndex].Value * siteWeights[siteIndex] * MonthlyWeights[dateIndex];
                    }
                }
            }

            return rval;
        }

        private string GetErrorMessage(int siteIndex, int dateIndex, string prefix)
        {
            string msg = "";
            if (list[siteIndex] is HydrometMonthlySeries)
            {
                var m = list[siteIndex] as HydrometMonthlySeries;
                msg = prefix + m.Cbtt + " " + m.Pcode + " " + m[dateIndex].DateTime.ToString("MMM yyyy");
            }
            else if (list[siteIndex] is CalculationSeries)
            {
                var c = list[siteIndex] as CalculationSeries;
                msg = "Error: no data " + c.Expression + " " + c[dateIndex].DateTime.ToString("MMM yyyy");
            }

            else
            {
                msg = "Error: no data " + list[siteIndex].Name;
            }
            return msg;
        }


        /*
0X2 - OCT-MAR PRECIPITATION (INCHES)
                                 WGHT    OCT    NOV    DEC    JAN    FEB    MAR
 ANDERSON                        1.00   3.08   2.36   5.28   3.30E  2.36E  2.00E
 ARROWROCK                       2.00   5.06   4.18   8.96   5.92E  4.42E  3.88E
 CENTERVILLE                     1.00   3.40   3.97   4.70   4.27E  3.17E  2.71E
 IDAHO CITY                      1.00   3.10   2.80   6.20   3.83E  2.81E  2.51E
0                                WGHT   1.00   1.00   1.00   1.00   1.00   1.00
                               TOTALS  14.64  13.31  25.14  17.32E 12.76E 11.10E          
         **/

        public virtual List<string> Details()
        {
            var rval = new List<string>();
            if (list.Count <= 0)
                return rval;
            //rval.Add("X" + this.Number);
            // print months..
            string s = "".PadRight(25);
            s += "   wt. ";
            var series1 = list[0];
            for (int i = 0; i < series1.Count; i++)
            {
                DateTime t = series1[i].DateTime;
                s += t.ToString("MMM").PadLeft(6) + " ";
            }
            rval.Add(this.ForecastTermType);
            rval.Add(s);
            for (int i = 0; i < list.Count; i++)
            {// site weight and monthly values
                series1 = list[i];
                s = siteNames[i].PadRight(25);
                s += siteWeights[i].ToString("F2").PadLeft(6) + " ";
                for (int j = 0; j < series1.Count; j++)
                {
                    s += Format(series1[j]);
                }
                rval.Add(s);
            }
            // monthly weights
            s = "".PadRight(25);
            s += "   wt. ";
            for (int i = 0; i < series1.Count; i++)
            {
                s += MonthlyWeights[i].ToString("F2").PadLeft(6)+" ";
            }
            rval.Add(s);
          

            return rval;
        }

        internal string Format(Point pt)
        {
            string s = "";
            s += pt.Value.ToString("F2").PadLeft(6);
            if (pt.Flag == PointFlag.Estimated)
                s += "E";
            else
                s += " ";

            return s;
        }


        protected SeriesList CreateSeriesList(bool getAveragePcode=false)
        {
           list = new SeriesList();

            for (int i = 0; i < cbttPodes.Count; i++)
            {

                var tokens = cbttPodes[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length != 2)
                {
                    if (cbttPodes[i].IndexOf("_") >= 0)
                    {
                        // QU or other calculation
                        var s = new CalculationSeries();
                        s.Expression = cbttPodes[i];
                        s.TimeInterval = TimeInterval.Monthly;
                        s.Parser.VariableResolver = HydrometData.GetVariableResolver();
                        list.Add(s);
                    }
                    else
                    {
                        throw new FormatException(cbttPodes[i]);
                    }
                }
                else
                {
                    var cbtt = tokens[0].Trim();
                    var pcode = tokens[1].Trim();

                    if (getAveragePcode)
                    { // used for snow/precip
                        pcode = HydrometMonthlySeries.LookupAveargePcode(pcode);
                    }
                    var s = HydrometData.GetSeries(cbtt, pcode);
                    list.Add(s);
                }
            }
            return list;
        }

        
        public int Number;


        



        /// <summary>
        /// Returns integer month based on month string in mmm format: 'jan'
        /// </summary>
        internal static int GetMonthFromString(string mmm)
        {
            string[] MonthStr = { "", "jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "oct", "nov", "dec" };
            return Array.IndexOf(MonthStr, mmm.ToLower()); ;
        }


        /// <summary>
        /// Reads as much Hydromet data as can be found
        /// in the range of the forecast definition.
        //  the potentailly 'extra' data beyond the forecast date (or missing data) 
        /// becomes a placeholder for estimates.
        /// </summary>
        protected void ReadFromHydromet( DateTime forecastDate)
        {
            DateTime t1;
            DateTime t2;
            ComputeDateRange(forecastDate, out t1, out t2);

            foreach (var s in list)
            {
                if (s is CalculationSeries)
                {
                    
                    var cs = s as CalculationSeries;
                    Logger.WriteLine("CalculationSeries.Calculate( " + cs.Expression + ")");
                    cs.Calculate(t1.AddMonths(-1), t2); // get extra month in case of [t-1] needed for QU
                    if(cs.Count != 0)
                      cs.RemoveAt(0); // remove extra month at beginning
                    Logger.WriteLine(cs.ToString(true));
                }
                else
                {
                    Logger.WriteLine("Reading data: ");
                    s.Read(t1, t2);
                    Logger.WriteLine(s.ToString(true));
                }

                HydrometMonthlySeries.ConvertFromAcreFeetToThousandAcreFeet(s);
            }

        }

        internal void ComputeDateRange(DateTime forecastDate, out DateTime t1, out DateTime t2)
        {
            Month1 = GetMonthFromString(MonthNames[0]);
            Month2 = GetMonthFromString(MonthNames[MonthNames.Count - 1]);

            bool prevYear = (this is AntecedentRunoffForecastTerm)
                                  &&  (Month1 == 7 && Month2 == 12) // island park (groundwater component)
                                  ||  (Month1 == 8 && Month2 == 9) // parker new forecast
                                  || (Month1 == 7 && Month2 == 9) // parker forecast
                                  || (Month1 == 9 && Month2 == 11) // kachess
                                  ;
            //prevYear = false;

            int y1 = forecastDate.Year;
            int y2 = forecastDate.Year;

            if (Month1 > 9 || prevYear)
            {
                y1--;
                if (Month2 > 9 || prevYear)
                    y2--;
            }

            t1 = new DateTime(y1, Month1, 1);
            t2 = new DateTime(y2, Month2, DateTime.DaysInMonth(y2, Month2));
        }



    }
}
