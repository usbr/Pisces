using System;
using System.Collections.Generic;
using Reclamation.Core;
using System.IO;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;
using System.Configuration;

namespace Reclamation.TimeSeries.AgriMet
{
    /// <summary>
    /// Coefficients to a crop
    /// </summary>
    public class CropCurves
    {

        static TextFile cc;

        static List<double[]> s_kc = new List<double[]>();

        internal static double[] ReadCoefficients(int cropCode)
        {
            if (cc == null)
            {
                ReadCoeficientsFromDisk();
            }

            return s_kc[cropCode];
   
        }

        private static void ReadCoeficientsFromDisk()
        {
            var fn = FileUtility.GetFileReference("cc.dat");
            //var fn = Path.Combine(Globals.LocalConfigurationDataPath, "cc.dat");

            cc = new TextFile(fn);
            s_kc.Capacity = 101;
            for (int i = 0; i <= 100; i++)
            {
                s_kc.Add(new double[] { });
            }
            for (int i = 0; i < cc.Length; i++)
            {
                string line = cc[i];
                int num = Convert.ToInt32(line.Substring(0, 2));
                List<double> kc = new List<double>();
                for (int j = 0; j < 21; j++)
                {
                    var s = line.Substring(j * 5 + 2, 5);
                    kc.Add(Convert.ToDouble(s));
                }

                s_kc[num] = kc.ToArray();
            }
        }

        //internal static Series CoefficientSeries(CropDatesDataSet.CropDatesRow row, DateTime t1, DateTime t2)
        //{
        //    var kc = ReadCoefficients(row.cropcurvenumber);

        //    var rval = new Series();
        //    rval.TimeInterval = TimeInterval.Daily;
        //    DateTime t = t1;
        //    var jpd = row.startdate.DayOfYear;
        //    var jcd = row.fullcoverdate.DayOfYear;
        //    var jtd = row.terminatedate.DayOfYear;

        //    while (t <= t2)
        //    {
        //        var juday = t.DayOfYear;
        //        rval.Add(t, CropCoefficient(kc, juday, jpd, jcd, jtd));
        //        t = t.AddDays(1);
        //    }

        //    return rval;
        //}

        internal static double CropCoefficient(double[] kc_value, int juday, int jpd, int jcd, int jtd)
        {
            /*
            c calculate the percent of growth stage.  The percent, from 0 to 100
            c determin the start to cover crop coefficient.  From 100 to 200 the
            c percent determin the cover to terminate crop coefficient.  The crop
            c coefficient is read into the array kc_value by increments of ten.
            c*/
            double r1, r2, percent, rval;
            if (juday >= jpd && juday <= jcd)
            {
                r1 = juday - jpd;
                r2 = jcd - jpd;
                percent = r1 / r2 * 100.0;
            }
            else if (juday > jcd && juday < jtd)
            {
                r1 = juday - jcd;
                r2 = jtd - jcd;
                percent = r1 / r2 * 100.0 + 100;
            }
            else
            {
                return 0;
            }

            int j = (int)(percent / 10);
            double remain = percent / 10 - j;
            //rval = (kc_value[j+2]-kc_value[j+1])*remain+kc_value[j+1];
            rval = (kc_value[j + 1] - kc_value[j]) * remain + kc_value[j];
            return rval;
        }


        internal static double WaterUse(DateTime t, double et, CropDatesDataSet.CropDatesRow row)
        {

            
            var kc_value = ReadCoefficients(row.cropcurvenumber);

            var jd = t.DayOfYear;
            var jpd = row.startdate.DayOfYear;
            var jcd = row.fullcoverdate.DayOfYear;
            var jtd = row.terminatedate.DayOfYear;

            var kc = CropCoefficient(kc_value, jd, jpd, jcd, jtd);

            return et * kc;
        }


        /// <summary>
        /// Method to calculate water use sums.
        /// </summary>
        /// <param name="n">Number of days prior to today for summation</param>
        /// <param name="et"></param>
        /// <param name="row"></param>
        /// <param name="numDaysRead"></param>
        /// <returns></returns>
        internal static double EtSummation(int n, Series et, CropDatesDataSet.CropDatesRow row, 
            int numDaysRead)
        {
            double etSum = 0;
            double dayET; 

            for (int i = 1; i <= n; i++)
            {
                if (i > numDaysRead)//Handles Charts within 14days of ETr Start Date
                { dayET = 0; }

                else
                {
                    dayET = WaterUse(et[numDaysRead - i].DateTime, et[numDaysRead - i].Value, row);
                    if (dayET < 0)//Handles missing ET values
                    { dayET = 0; }
                    else { }
                }

                etSum = etSum + dayET; 
            }
            return etSum;
        }


        /// <summary>
        /// Calculates the Daily Crop ET.
        /// </summary>
        /// <param name="numDaysRead">Number of days between Chart Generation date and the Start Date</param>
        /// <param name="n">The number of days from today for which you want the Crop ET</param>
        /// <param name="et"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        internal static string ETCropDaily(int numDaysRead, int n, HydrometDailySeries et, CropDatesDataSet.CropDatesRow row)
        {
            double val = 0.00;
            if (numDaysRead > n)
            { val = CropCurves.WaterUse(et[numDaysRead - n].DateTime, et[numDaysRead - n].Value, row); }
            else { }
            if (val < 0)
            { return "msng"; }
            else { return val.ToString("F2"); }
        }
    
    }
}
