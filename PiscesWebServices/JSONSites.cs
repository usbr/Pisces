using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Newtonsoft.Json;
using Reclamation.Core;
using Reclamation.TimeSeries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PiscesWebServices
{
    /// <summary>
    /// dump site data from Pisces meta-data in GEOJson format
    /// </summary>
    class JSONSites
    {
        TimeSeriesDatabase db;
        public JSONSites(TimeSeriesDatabase db)
        {
            this.db = db;
        }
        /// <summary>
        /// Prints GEOJson dump of Sites
        /// </summary>
        /// <param name="requiredProperties">list of properties to include (if not found in siteproperties an empty string will be inserted)</param>
        /// <param name="propertyFilter">two part filter in property table i.e.  'program:agrimet'</param>
        public void Execute(string[] requiredProperties, string propertyFilter="")
        {
            Console.Write("Content-Type:  application/json\n\n");

          var features = new List<Feature>();
          FeatureCollection fc = new FeatureCollection(features);

          var sites = db.GetSiteCatalog(propertyFilter:propertyFilter);

         var siteProp = new TimeSeriesDatabaseDataSet.sitepropertiesDataTable(db);

          foreach (var s in sites)
          {
              var pos = new GeographicPosition(s.latitude,s.longitude);
              var pt = new GeoJSON.Net.Geometry.Point(pos);

              var props = siteProp.GetDictionary(s.siteid);

              for (int i = 0; i < requiredProperties.Length; i++)
              {
                  if (requiredProperties[i].Trim() == "")
                      continue;
                  if (!props.ContainsKey(requiredProperties[i]))
                      props.Add(requiredProperties[i], "");
              }


              props.Add("cbtt", s.siteid);
              props.Add("title", s.description);
              props.Add("state", s.state);
              props.Add("type", s.type);
              props.Add("install", s.install);
              var feature = new Feature(pt,props,s.siteid);

              fc.Features.Add(feature);
          }

            var settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
          var json = Newtonsoft.Json.JsonConvert.SerializeObject(fc, 
              Newtonsoft.Json.Formatting.Indented,settings);

          Console.WriteLine(json);
         //File.WriteAllText(@"c:\temp\test.json", json);
           
        }
    }
}
