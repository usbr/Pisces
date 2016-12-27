using Reclamation.Core;
using Reclamation.TimeSeries;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace GetUsaceDaily
{
    /// <summary>
    /// Manage reading  Corps of Engineers data 
    /// from *.dat files .  For example: http://www.nwd-wc.usace.army.mil/ftppub/project_data/daily/mcn_d.dat
    /// data is returned in Series class
    /// </summary>
    class CorpsDataFile
    {

        public static Series ReadCorpsDataFile(string url, TimeInterval interval, params string[] headers)
        {
            string units = headers[headers.Length - 1];
            if( units == "n (ft)") // hack for Banks Lake.
                units = "(ft)";

            Series rval = new Series(units, interval);
            rval.HasFlags = true;

            string fileName = FileUtility.GetTempFileName(".txt");
            Web.GetTextFile(url, fileName, true);


            int[] widths = new int[] { 9, 5, 9, 9, 9, 9, 9, 9, 9, 9, 9,9,9 };

            var tf = new FixedWidthTextFile(fileName, widths);

            int col, row;
            tf.FindIndexToRange(headers, out row, out col);
            if (row < 0 || col < 0)
            {
                Logger.WriteLine("Error: no index found for headers '" + String.Join("','", headers) + "'");
                return rval;
            }

            rval.Name = String.Join(" ", headers, 0, headers.Length - 1);
            ReadDataIntoSeries(tf, rval, row + headers.Length, col);

            return rval;
        }

        private static void ReadDataIntoSeries(FixedWidthTextFile tf, Series s, int firstRowIndex, int columnIndex)
        {

            for (int r = firstRowIndex; r < tf.RowCount; r++)
            {
                string dateStr = tf[r, 0];
                DateTime t;
                if (dateStr.Trim() == "")
                    continue;

                if (!TryParseDateTime(dateStr, out t))
                {
                    Console.WriteLine("found an invalid date '" + dateStr+"'  processing will stop");
                    return; 
                }
                int hr = 0;
                string h = tf[r, 1].Substring(0, 3);
                if (!int.TryParse(h, out hr))
                {
                    hr = 0;
                }

                if (s.TimeInterval == TimeInterval.Daily)
                {
                    if (hr == 1)
                    {
                        t = t.AddDays(-1);
                    }
                }
                if (s.TimeInterval == TimeInterval.Hourly)
                {
                        if (hr == 24)
                            t = t.AddDays(1);// use hour zero
                        else
                            t = t.AddHours(hr);
                }

                var val = tf[r, columnIndex];
                double d;
                if (!double.TryParse(val, out d))
                {
                    Logger.WriteLine("Error: parsing '" + val + "' as a number");
                    continue;
                }

                if (t.Month == 2)
                {
                    Console.WriteLine();
                }
                s.Add(t, d);
            }
        }

        /*
         * 
         * 
         #########################################################################
        NOTE: These data are furnished with the understanding that the Corps of
              Engineers makes no warrenties concerning the accuracy, reliability,
              or suitability of the data for any particular purpose.
        #########################################################################

        McNary (MCN) Daily Data

                          Total                    Average                                    
                        Station  Average  Average Generatn  Average Midnight  Average  Average
                        Service   Inflow  Outflow     Flow    Spill  Forebay  Forebay Tailwatr
           DATE   TIME     (MW)   (kcfs)   (kcfs)   (kcfs)   (kcfs)     (ft)     (ft)     (ft)
        06Jan2013 2400    96.00   175.80   183.10   166.20    15.00   339.44   339.49   265.92
        07Jan2013 2400    96.00   177.60   182.00   163.10    16.90   339.23   339.31   266.67
        08Jan2013 2400    96.00   191.90   188.30   163.10    23.30   339.41   339.33   267.25
        09Jan2013 2400    96.00   170.20   172.10   165.30     4.80   339.33   339.01   267.31
        10Jan2013 2400    96.00   143.00   165.10   163.10     0.00   338.10   338.39   266.66
        11Jan2013 2400    96.00   175.80   173.50   171.50     0.00   338.24   337.97   266.78
        12Jan2013 2400    96.00   184.30   173.00   171.00     0.00   338.83   338.45   266.71
        13Jan2013 2400    96.00   164.70   171.10   169.10     0.00   338.49   338.53   266.59
        14Jan2013 2400    96.00   181.20   161.30   157.30     2.00   339.55   338.87   265.96
        15Jan2013 2400    96.00   181.90   178.80   150.10    26.70   339.80   339.57   265.45
        16Jan2013 2400    96.00   170.60   180.30   156.40    21.90   339.23   339.47   265.12
        17Jan2013 2400    96.00   178.00   190.10   169.90    18.20   338.54   338.60   265.50
        <-- 9 --> <4-><---9---><----9--><----9--><----9--><----9--><----9--><----9--><----9-->
         */


        public static bool TryParseDateTime(string date, out DateTime t)
        {
           

            t = DateTime.Now;
            if (date.Trim() == "")
                return false;
            try
            {
                // using ParseExact instead of TryParse because of trouble in Mono...
                t = DateTime.ParseExact(date, "ddMMMyyyy", new CultureInfo("en-US"));
                return true;
            }
            catch (Exception )
            {
               // Console.WriteLine(ex.Message);
                return false;
            }
        }

    }
}
