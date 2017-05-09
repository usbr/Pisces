using System;
using System.Data;
using System.Collections.Generic;
using Reclamation.Core;
using System.Text;
using System.Text.RegularExpressions;

namespace Reclamation.TimeSeries.Usgs
{
    public static class Utility
    {

      public static bool AutoUpdate = false;


      public static TimeSeriesDatabaseDataSet.RatingTableDataTable GetRatingTable(string siteNumber)
      {
          //string url = "http://waterdata.usgs.gov/nwisweb/data/exsa_rat/13236500.rdb";
          string url = "http://waterdata.usgs.gov/nwisweb/get_ratings?site_no=13081500&file_type=exsa";
          url = url.Replace("13081500", siteNumber);
          string[] data = Web.GetPage(url);

          TextFile tf = new TextFile(data);

          UsgsRDBFile rdb = new UsgsRDBFile(data);
          
          TimeSeriesDatabaseDataSet.RatingTableDataTable t = new TimeSeriesDatabaseDataSet.RatingTableDataTable();

          t.XUnits = LookupUnits(tf,"# //RATING_INDEP");
          t.YUnits = LookupUnits(tf,"# //RATING_DEP");
          
          for (int i = 0; i < rdb.Rows.Count; i++)
          {
              double x = Convert.ToDouble(rdb.Rows[i]["indep"]);
              double y = Convert.ToDouble(rdb.Rows[i]["dep"]);
              var r = t.FindByx(x);
              if (r == null)
                  t.AddRatingTableRow(x, y);
              else
              {
                  Console.WriteLine("Warning: duplicate x "+x);
              }
          }
          t.Name = "Usgs " + siteNumber;
          return t;
      }

      private static string LookupUnits(TextFile tf , string tag)
      {
          /*
          # //RATING_INDEP ROUNDING="2223456782" PARAMETER="Gage height (ft)"
          # //RATING_DEP ROUNDING="2222233332" PARAMETER="Discharge (cfs)"
          */
          int idx = tf.IndexOf(tag);
          if (idx >= 0)
          {
              string expr = ".*PARAMETER=\"(?<parmeter>)\"";
              Regex reg = new Regex(expr);
              if (!reg.IsMatch(tf[idx]))
                  return "";

               return reg.Match(tf[idx]).Groups["parameter"].Value;
          }

          return "";
      }

       public static UsgsDailyParameter DailyParameterFromString(string parm)
        {
            if (parm == UsgsDailyParameter.DailyMaxTemperature.ToString() ||
                parm == "max water temperature")
                return UsgsDailyParameter.DailyMaxTemperature;
            if (parm == UsgsDailyParameter.DailyMeanDischarge.ToString() ||
                parm == "Discharge")
                return UsgsDailyParameter.DailyMeanDischarge;
            if (parm == UsgsDailyParameter.DailyMeanTemperature.ToString() ||
                parm == "mean water temperature")
                return UsgsDailyParameter.DailyMeanTemperature;
            if (parm == UsgsDailyParameter.DailyMinTemperature.ToString() ||
                parm == "min water temperature")
                return UsgsDailyParameter.DailyMinTemperature;
            throw new ArgumentException("Unknown parameter " + parm);
        }

        //ToDO; be specific ImportUsgs /Add  or  /Update   (no fancy stuff) 
        //public static void ImportUsgs(Arguments args, SqlTimeSeriesDatabase db)
        //{
        //    bool overwrite = false;
        //    int parentID = 0;
        //    string siteNumber;
        //    DateTime t1, t2;
        //    if (!args.Contains("SiteNumber") ||
        //        !args.Contains("Paramter") || !args.Contains("T1"))
        //    {
        //        Console.WriteLine("Usage: pisces.exe /ImportUsgs /ParentID:id " +
        //            "/SiteNumber:usgsID /Parameter:usgsParameter /T1:beginDate " +
        //            "/T2:endDate /OverWrite:trueORfalse");
        //    }
        //    if (args.Contains("OverWrite"))
        //        overwrite = Convert.ToBoolean(args["OverWrite"]);
        //    if (args.Contains("ParentID"))
        //        parentID = Convert.ToInt32(args["ParentID"]);
        //    if (args.Contains("T2"))
        //        t2 = Convert.ToDateTime(args["T2"]);
        //    else t2 = DateTime.Now;
        //    siteNumber = args["SiteNumber"];
        //    UsgsDailyParameter parameter = DailyParameterFromString(args["Parameter"]);
        //    t1 = Convert.ToDateTime(args["T1"]);
        //    db.ImportUSGSDailyValues(parentID, siteNumber, parameter, t1, t2, overwrite);
        //}

        public static string SiteNumberFromConnectionString(string connectionString)
        {
            string pattern = ".*site_no=(?<site_no>[0-9]{8,10})";
           RegexOptions options = RegexOptions.IgnorePatternWhitespace 
                      | RegexOptions.Multiline  | RegexOptions.IgnoreCase;

           Regex reg = new Regex(pattern, options);
           if (!reg.IsMatch(connectionString))
               return "";
           return reg.Match(connectionString).Groups["site_no"].Value;
        }
    }
}
