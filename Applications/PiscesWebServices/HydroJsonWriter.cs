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
using Newtonsoft.Json;

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
            string colName = data.Columns[0].ColumnName;
            DateTime maxDate = Convert.ToDateTime(data.Compute("MAX(" + colName + ")", null));
            DateTime minDate = Convert.ToDateTime(data.Compute("MIN(" + colName + ")", null));
            colName = data.Columns[1].ColumnName;
            double maxVal = Convert.ToDouble(data.Compute("MAX(" + colName + ")", null));
            double minVal = Convert.ToDouble(data.Compute("MIN(" + colName + ")", null));

            // Initialize HydroJSON  data objects
            var jsonOut = new HydroJsonObject.Json();
            var jsonSite = new HydroJsonObject.Site();
            var jsonSiteCoordinates = new HydroJsonObject.CoordinateItems();
            var jsonSiteElevation = new HydroJsonObject.ElevationItems();
            var jsonTsItems = new HydroJsonObject.TimeSeriesItems();
            jsonTsItems.tsitem = new HydroJsonObject.TimeSeries[1]; //assumes only 1 TS object per site
            var jsonTsData = new HydroJsonObject.TsData();
            jsonTsData.point = new HydroJsonObject.TsPoint[data.Rows.Count];

            // Populate Site level data
            jsonSite.HUC = huc.ToString();
            jsonSite.active_flag = "1";
            jsonSite.location_type = "";
            jsonSite.name = siteName;
            jsonSite.responsibility = region;
            jsonSite.time_format = "%Y-%m-%dT%H:%M:%S%z";
            jsonSite.timezone = "";
            jsonSite.tz_offset = "";
            jsonSiteCoordinates.datum = latLonDatum;
            jsonSiteCoordinates.latitude = lat.ToString();
            jsonSiteCoordinates.longitude = lon.ToString();
            jsonSite.coordinates = jsonSiteCoordinates;
            jsonSiteElevation.accuracy = "0";
            jsonSiteElevation.datum = elevDatum;
            jsonSiteElevation.method = " ";
            jsonSiteElevation.value = elev.ToString();
            jsonSite.elevation = jsonSiteElevation;

            // Populate Time Series level data
            var jsonTs = new HydroJsonObject.TimeSeries();
            jsonTs.active_flag = "1";
            jsonTs.count = data.Rows.Count.ToString();
            jsonTs.duration = " ";
            jsonTs.hash = "TODO";
            jsonTs.interval = interval;
            jsonTs.max_value = maxVal.ToString();
            jsonTs.min_value = minVal.ToString();
            jsonTs.parameter = parameterName;
            jsonTs.quality_type = "string";
            jsonTs.sigfig = sigfigs.ToString();
            jsonTs.start_timestamp = minDate.ToString("s");
            jsonTs.end_timestamp = maxDate.ToString("s");
            jsonTs.units = units;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                var ithPoint = new HydroJsonObject.TsPoint();
                ithPoint.datetime = DateTime.Parse(data.Rows[i][0].ToString()).ToString("s");
                ithPoint.value = data.Rows[i][1].ToString();
                ithPoint.flag = "0";
                jsonTsData.point[i] = ithPoint;
            }
            jsonTs.values = jsonTsData;

            // Build JSON Object
            jsonTsItems.tsitem[0] = jsonTs;
            jsonSite.timeseries = jsonTsItems;
            jsonOut.site = jsonSite;

            var output = JsonConvert.SerializeObject(jsonOut);
            sr.Write(output);
            sr.Close();
        }
    }


    public class HydroJsonObject
    {
        /* Format adapted from sample at http://www.nwd-wc.usace.army.mil/dd/common/web_service/webexec/getjson?query=%5B%22dwr%20flow%22%5D&backward=7d
         * 
         *  {  
         *      "DWR":{  
         *          "HUC":"",
         *          "active_flag":"T",
         *          "coordinates":{  
         *                 "datum":"WGS84",
         *                 "latitude":46.515366,
         *                 "longitude":-116.296219
         *          },
         *          "elevation":{  
         *                 "accuracy":0.0,
         *                 "datum":"NGVD29",
         *                 "method":"",
         *                 "value":1613.0
         *          },
         *          "location_type":" ",
         *          "name":"Dworshak Dam",
         *          "responsibility":"NWW",
         *          "time_format":"%Y-%m-%dT%H:%M:%S%z",
         *          "timeseries":{        
         *                  "DWR.Flow-Gen.Ave.1Hour.1Hour.CBT-REV":{  
         *                          "active_flag":1,
         *                          "count":167,
         *                          "duration":" ",
         *                          "end_timestamp":"2017-06-02T06:00:00",
         *                          "hash":"TODO",
         *                          "interval":" ",
         *                          "max_value":4.4,
         *                          "min_value":2.1,
         *                          "parameter":"Flow-Gen",
         *                          "quality_type":"string",
         *                          "sigfig":3,
         *                          "site_quality":[  
         *                          
         *                          ],
         *                          "start_timestamp":"2017-05-26T08:00:00",
         *                          "units":"kcfs",
         *                          "values":[
         *                                  [  
         *                                      "2017-05-26T08:00:00",
         *                                      4.3,
         *                                      0
         *                                  ],
         *                                  [  
         *                                      "2017-05-26T09:00:00",
         *                                      4.3,
         *                                      0
         *                                  ], ...
         *                          ]
         *                  },
         *                  "DWR.Flow-Gen.Ave.~1Day.1Day.CBT-REV":{ ... },
         *                  "DWR.Flow-In.Ave.1Hour.1Hour.CBT-COMPUTED-REV":{ ... }
         *          },
         *          "timezone":"PST",
         *          "tz_offset":-8
         *      }
         *  } 
         */
        


        public class Json
        {
            public Site site { get; set; }
        }
        /// <summary>
        /// Data Model for the HydroJSON format
        /// </summary>
        public class Site
        {
            public string HUC { get; set; }
            public string active_flag { get; set; }
            public CoordinateItems coordinates { get; set; }
            public ElevationItems elevation { get; set; }
            public string location_type { get; set; }
            public string name { get; set; }
            public string responsibility { get; set; }
            public string time_format { get; set; }
            public TimeSeriesItems timeseries { get; set; }
            public string timezone { get; set; }
            public string tz_offset { get; set; }
        }

        public class CoordinateItems
        {
            public string datum { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
        }

        public class ElevationItems
        {
            public string accuracy { get; set; }
            public string datum { get; set; }
            public string method { get; set; }
            public string value { get; set; }
        }

        public class TimeSeriesItems
        {
            public TimeSeries[] tsitem { get; set; }
        }

        public class TimeSeries
        {
            public string active_flag { get; set; }
            public string count { get; set; }
            public string duration { get; set; }
            public string start_timestamp { get; set; }
            public string end_timestamp { get; set; }
            public string hash { get; set; }
            public string interval { get; set; }
            public string max_value { get; set; }
            public string min_value { get; set; }
            public string parameter { get; set; }
            public string quality_type { get; set; }
            public string sigfig { get; set; }
            public List<SiteQualityItems> site_quality { get; set; }
            public string units { get; set; }
            public TsData values { get; set; }
        }

        public class SiteQualityItems
        {

        }

        public class TsData
        {
            [JsonProperty("")] public TsPoint[] point { get; set; }
        }

        public class TsPoint
        {
            public string datetime { get; set; }
            public string value { get; set; }
            public string flag { get; set; }

        }

    }
}
