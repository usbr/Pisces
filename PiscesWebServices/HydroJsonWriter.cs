using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Reclamation.TimeSeries;
using System.Linq;
using System.Web;
using System.IO;
using System.Data;
using Reclamation.Core;
using System.Net;

namespace PiscesWebServices
{
    public class HydroJsonWriter
    {
        /// <summary>
        /// Writes output in HydroJSON Format
        /// Format adapted from sample at http://www.nwd-wc.usace.army.mil/dd/common/web_service/webexec/getjson?query=%5B%22dwr%20flow%22%5D&backward=7d
        /// and documentation at https://github.com/gunnarleffler/hydroJSON
        /// Search for [JR] within this code file to see areas that need refinement...
        /// </summary>
        /// <param name="data"></param>
        /// <param name="filename"></param>
        public static void writeHydroJsonData(DataTable data, string filename)
        {
            StreamWriter sr = new StreamWriter(filename, false);

            // [JR] need methods to get site info and metadata from query
            string siteName = "";
            string parameterName = "";
            int huc = 0;
            double lat = 0.0;
            double lon = 0.0;
            string latLonDatum = "WGS84";
            double elev = 0.0;
            string elevDatum = "NGVD29";
            string region = "USBR";
            string units = "";
            int sigfigs = 3;
            string interval = "daily";

            // Add HydroJSON Site Header
            #region
            sr.WriteLine(@"{");
            sr.WriteLine(@"   """ + siteName.Replace(" ", "") + @""": {");
            sr.WriteLine(@"      ""HUC"": """ + huc + @""", ");
            sr.WriteLine(@"      ""active_flag"": ""T"", ");
            sr.WriteLine(@"      ""coordinates"": {");
            sr.WriteLine(@"         ""datum"": """ + latLonDatum + @""", ");
            sr.WriteLine(@"         ""latitude"": " + lat + ", ");
            sr.WriteLine(@"         ""longitude"": " + lon);
            sr.WriteLine(@"      }, ");
            sr.WriteLine(@"      ""elevation"": {");
            sr.WriteLine(@"         ""accuracy"": 0.0, ");
            sr.WriteLine(@"         ""datum"": """ + elevDatum + @""", ");
            sr.WriteLine(@"         ""method"": """", ");
            sr.WriteLine(@"         ""value"": " + elev);
            sr.WriteLine(@"      }, ");
            sr.WriteLine(@"      ""location_type"": "" "", ");
            sr.WriteLine(@"      ""name"": """ + siteName + @""", ");
            sr.WriteLine(@"      ""responsibility"": """ + region + @""", ");
            sr.WriteLine(@"      ""time_format"": ""%Y-%m-%dT%H:%M:%S%z"", ");
            sr.WriteLine(@"      ""timeseries"": {");
            #endregion

            // Add HydroJSON parameter header
            #region
            sr.WriteLine(@"         ""DWR.Flow-In.Ave.~1Day.1Day.CBT-COMPUTED-REV"": {");
            sr.WriteLine(@"            ""active_flag"": 1, ");
            sr.WriteLine(@"            ""count"": " + data.Rows.Count + @", ");
            sr.WriteLine(@"            ""duration"": "" "", ");
            string colName = data.Columns[0].ColumnName;
            DateTime maxDate = Convert.ToDateTime(data.Compute("MAX(" + colName + ")", null));
            DateTime minDate = Convert.ToDateTime(data.Compute("MIN(" + colName + ")", null));
            sr.WriteLine(@"            ""end_timestamp"": """ + maxDate.ToString("s") + @""", ");
            sr.WriteLine(@"            ""hash"": ""TODO"", ");
            sr.WriteLine(@"            ""interval"": """ + interval + @""", ");
            colName = data.Columns[1].ColumnName;
            double maxVal = Convert.ToDouble(data.Compute("MAX(" + colName + ")", null));
            double minVal = Convert.ToDouble(data.Compute("MIN(" + colName + ")", null));
            sr.WriteLine(@"            ""max_value"": " + maxVal + @", ");
            sr.WriteLine(@"            ""min_value"": " + minVal + @", ");
            sr.WriteLine(@"            ""parameter"": """ + parameterName + @""", ");
            sr.WriteLine(@"            ""quality_type"": ""string"", ");
            sr.WriteLine(@"            ""sigfig"": " + sigfigs + @", ");
            sr.WriteLine(@"            ""site_quality"": [], ");
            sr.WriteLine(@"            ""start_timestamp"": """ + minDate.ToString("s") + @""", ");
            sr.WriteLine(@"            ""units"": """ + units + @""", ");
            sr.WriteLine(@"            ""values"": [");
            #endregion

            // Populate data points
            #region
            /*
            REGULAR ENTRY => ["2016-05-13T23:00:00", 10.680175, 0], 
            LAST ENTRY => ["2016-05-14T23:00:00", 10.160483, 0]
            */
            for (int i = 0; i < data.Rows.Count; i++)
            {
                sr.WriteLine("[");
                var t = DateTime.Parse(data.Rows[i][0].ToString()).ToString("s");
                sr.WriteLine(@"  """ + t + @""", ");
                var val = data.Rows[i][1].ToString();
                sr.WriteLine(@"  """ + val + @""", ");
                sr.WriteLine(@"  ""0"" ");
                sr.Write("]");
                if (i == data.Rows.Count - 1)
                {
                    sr.WriteLine("");
                }
                else
                {
                    sr.WriteLine(",");
                }
                
            }
            sr.WriteLine(@"            ]");
            sr.WriteLine(@"         },");
            #endregion

            // Add footer and close tags
            #region
            sr.WriteLine(@"      ""timezone"": ""PST"", ");
            sr.WriteLine(@"      ""tz_offset"": -8");
            sr.WriteLine(@"   }");
            sr.WriteLine(@"}");
            #endregion

            sr.Close();
        }
    }
}
