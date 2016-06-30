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

namespace PiscesWebServices.CGI
{

    public partial class WebTimeSeriesWriter
    {

        private static string LegacyTranslation(string query, TimeInterval interval)
        {

            var rval = query;

            //http://www.usbr.gov/pn-bin/webarccsv.pl?station=cedc&pcode=mx&pcode=mn&back=10&format=2
            //station=boii&year=2016&month=4&day=21&year=2016&month=4&day=21&pcode=OB&pcode=OBX&pcode=OBN&pcode=TU

            if (query.IndexOf("station=") >= 0 && query.IndexOf("pcode") >= 0)
            {
                rval = LegacyStationQuery(query, interval);
            }
            else if( query.IndexOf("parameter") >=0 )
            {
                rval = rval.Replace("parameter", "list");
            }

            Logger.WriteLine(rval);
            return rval;

        }

        private static string LegacyStationQuery(string query, TimeInterval interval)
        {
            string rval = "";
            var c = HttpUtility.ParseQueryString(query);

            var pcodes = c["pcode"].Split(',');
            var cbtt = c["station"];
            var back = "";
            var start = "";
            var end = "";
            var keys = c.AllKeys;
            if (keys.Contains("back"))
            {
                back = c["back"];
                DateTime t1, t2;
                HydrometWebUtility.GetDateRange(c,interval,out t1, out t2);

                start = t1.ToString("yyyy-M-d");
                end = t2.ToString("yyyy-M-d");
            }
            else if (keys.Contains("year") && keys.Contains("month") && keys.Contains("day"))
            {

                var years = c["year"].Split(',');
                var months = c["month"].Split(',');
                var days = c["day"].Split(',');

                start = years[0] + "-" + months[0] + "-" + days[0];
                end = years[1] + "-" + months[1] + "-" + days[1];
            }
            rval = "list=";
            //rval = rval.Replace("station=" + cbtt + "", "list=");
            for (int i = 0; i < pcodes.Length; i++)
            {
                var pc = pcodes[i];
                rval += cbtt + " " + pc;
                if (i != pcodes.Length - 1)
                    rval += ",";
            }
            if( back == "")
               rval += "&start=" + start + "&end=" + end;

            if( c.AllKeys.Contains("print_hourly") )
                rval += "&print_hourly=true";
            if (back != "")
                rval += "&back=" + back;

            if (c.AllKeys.Contains("format"))
                rval += "&format=" + c["format"];

            return rval.ToLower();
        }



    }
}