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
    /// Manage reading  Corps of Engineers Hourly data 
    /// from *.txt files .  For example:http://www.nwd-wc.usace.army.mil/ftppub/project_data/hourly/gcl_h.dat
    /// or http://www.nwd-wc.usace.army.mil/ftppub/project_data/daily/lwg_d.dat
    /// data is returned in Series class
    /// </summary>
    class CorpsHourlyFile
    {

        public static Series ReadCorpsDataFile(string url, params string[] headers)
        {
            string units = headers[headers.Length - 1];
            if( units == "n (ft)") // hack for Banks Lake.
                units = "(ft)";

            Series rval = new Series(units, TimeInterval.Irregular);
            rval.HasFlags = true;

            string fileName = FileUtility.GetTempFileName(".txt");
            Web.GetTextFile(url, fileName, true);


            //                         8,14,19,28,36,43,52,61,69
            int[] widths = new int[] { 8,6,	5,	9,	8,	7,	9,	9,	8 };

            var tf = new FixedWidthTextFile(fileName, widths);

            int col, row;
            tf.FindIndexToRange(headers, out row, out col);
            if (row < 0 || col < 0)
            {
                Logger.WriteLine("Error: no index found for headers '" + String.Join("','", headers) + "'");
                return rval;
            }

            DateTime t = ReadDate(fileName);

            rval.Name = String.Join(" ", headers, 0, headers.Length - 1);
            ReadDataIntoSeries(tf, rval, row + headers.Length, col,t);

            return rval;
        }

        private static DateTime ReadDate(string fileName)
        {
            TextFile tf = new TextFile(fileName);
            if (tf.Length < 2)
                throw new Exception("cannot read date from " + fileName);

            string a= tf[1].Substring(62);
            int idx = a.IndexOf(" ");
            a = a.Substring(idx).Trim();

            var t = DateTime.ParseExact(a,"MMMM dd, yyyy",new CultureInfo("en-US"));

            return t;

        }

        private static void ReadDataIntoSeries(FixedWidthTextFile tf, Series s, int firstRowIndex, int columnIndex,DateTime fileDate)
        {

            for (int r = firstRowIndex; r < tf.RowCount; r++)
            {
                int hr;
                if (tf[r, 0].Trim() == "" || !int.TryParse(tf[r,0].Trim(),out hr))
                    continue;

                hr = Convert.ToInt32(tf[r, 0].Trim());

                DateTime t = fileDate;

                if (hr == 24)
                    t = t.AddDays(1);// use hour zero
                else
                    t = t.AddHours(hr);

                
                var val = tf[r, columnIndex];
                double d;
                if (!double.TryParse(val, out d))
                {
                    Logger.WriteLine("Error: parsing '" + val + "' as a number");
                    continue;
                }

                s.Add(t, d);
            }
        }



    }
}
