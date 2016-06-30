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
using System.Text;
using Reclamation.TimeSeries;
using Reclamation.TimeSeries.Hydromet;

namespace PiscesWebServices.CGI
{
    public partial class WaterYearReport
    {
        private TimeSeriesDatabase db;
        private string payload;

        public WaterYearReport(TimeSeriesDatabase db, string payload)
        {
            this.db = db;
            this.payload = payload;
        }


        internal void Run()
        {
            var query = HydrometWebUtility.GetQuery();
            var collection = HttpUtility.ParseQueryString(query);

            TimeRange r = GetDateRange(collection);

            var siteID = "";
            if (collection.AllKeys.Contains("site"))
            {
                siteID = collection["site"];
            }

            var parameter = "";
            if (collection.AllKeys.Contains("parameter"))
            {
                parameter = collection["parameter"];
            }

            var s = new HydrometDailySeries(siteID, parameter);
            s.Read(r.StartDate, r.EndDate);

            DataTable wyTable = WaterYearTable(s);
            Console.WriteLine("print header");
            var html = DataTableOutput.ToHTML(wyTable);
            Console.WriteLine(html);    
        }

        private static TimeRange GetDateRange(NameValueCollection collection)
        {
            var start = "";
            if (collection.AllKeys.Contains("start"))
            {
                start = collection["start"];
            }
            var end = "";
            if (collection.AllKeys.Contains("end"))
            {
                end = collection["end"];
            }

            int y1=0, y2=0;

            if(    !int.TryParse(start,out y1) 
                || !int.TryParse(end,out y2) )
            {
                StopWithError("Error with year range");
            }
            
            var rval = new TimeRange(
                     WaterYear.BeginningOfWaterYear( new  DateTime(y1,1,1)),
                     WaterYear.BeginningOfWaterYear( new DateTime(y2,12,31)));
            return rval;
        }

        private static void StopWithError(string message)
        {
            Console.WriteLine("Error: "+message);
            throw new Exception(message);
        }

        private DataTable WaterYearTable(HydrometDailySeries s)
        {
            return new DataTable();

        }

    }
}