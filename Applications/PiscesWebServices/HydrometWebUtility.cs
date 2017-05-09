using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Reclamation.TimeSeries;
using System.Globalization;
using Reclamation.Core;

namespace PiscesWebServices
{
    /// <summary>
    /// http://www.codeproject.com/Articles/9433/Understanding-CGI-with-C
    /// </summary>
    class HydrometWebUtility
    {

        readonly static int MAX_CONTENTLENGTH = 9000;
        
        /// <summary>
        /// Reads the query (Get or Post) and generates a query string
        /// </summary>
        /// <returns></returns>
        public static string GetQuery()
        {
            // Original Source: http://chalaki.com/8-steps-program-install-setup-call-csharp-cgi-programs-in-iis-7/321/
            //Console.Write("<html><head><title>WebCSV CGI Test Program</title></head><body><h1>CGI Environment</h1>");
            //Console.Write("The Common Gateway Interface version (env: GATEWAY_INTERFACE): " +
            //    System.Environment.GetEnvironmentVariable("GATEWAY_INTERFACE"));
            //Console.Write("<br/>The name and version of the protocol (env SERVER_PROTOCOL): " +
            //    System.Environment.GetEnvironmentVariable("SERVER_PROTOCOL"));
            //Console.Write("<br/>The request method used (env: REQUEST_METHOD): " +
            //    System.Environment.GetEnvironmentVariable("REQUEST_METHOD"));
            //Console.Write("<br/>Extra path information passed to the CGI program (env: PATH_INFO): " +
            //    System.Environment.GetEnvironmentVariable("PATH_INFO"));
            //Console.Write("<br/>The translated version of the path (env: PATH_TRANSLATED): " +
            //    System.Environment.GetEnvironmentVariable("PATH_TRANSLATED"));

            // Construct search string
            string srchString = "";
            var method = System.Environment.GetEnvironmentVariable("REQUEST_METHOD");
            if (method == null)
            {
                return "";
            }
            if (method.Equals("POST")) //POST Method
            {
                string PostedData = "";
                int PostedDataLength = Convert.ToInt32(System.Environment.GetEnvironmentVariable("CONTENT_LENGTH"));
                if (PostedDataLength > MAX_CONTENTLENGTH) /// PostedDataLength = 2048;  
                    return "";                                                          
                for (int i = 0; i < PostedDataLength; i++)
                {
                    PostedData += Convert.ToChar(Console.Read()).ToString();
                }
                srchString = "?" + PostedData;
            }
            else //GET Method
            {
                srchString = "?" + System.Environment.GetEnvironmentVariable("QUERY_STRING");
            }
          //  srchString = SanitizeQuery(srchString);

            return srchString.ToLower();
        }

        //internal static string SanitizeQuery(string srchString)
        //{
        //    srchString = srchString.Replace("%20", " "); // Replace HTML %20 with space for RegEx
        //    srchString = srchString.Replace("%2C", ","); // Replace HTML %2C+ with comma for RegEx
        //    srchString = srchString.Replace("+", " "); // Replace HTML + with space for RegEx
        //    return srchString;
        //}

        internal static bool GetDateRange(NameValueCollection c,TimeInterval interval,  out DateTime t1, out DateTime t2)
        {
             t1 = DateTime.Now.AddDays(-7);
             t2 = DateTime.Now;

             if (interval == TimeInterval.Daily)
                 t2 = DateTime.Now.AddDays(-1).Date;

            try
            {
                var back = GetIntParam(c, "back", -1);
                if (back != -1)
                {
                    if (interval == TimeInterval.Hourly || interval == TimeInterval.Irregular)
                        t1 = t2.AddHours(-back);

                    if (interval == TimeInterval.Daily)
                        t1 = t2.AddDays(-back);

                    return true;
                }

                bool startOrEnd = false;
                if( c.AllKeys.Contains("start"))
                {
                    t1 = ParseDate(c,"start");
                    startOrEnd = true;
                }
                if (c.AllKeys.Contains("end"))
                {
                    t2 = ParseDate(c,"end");
                    startOrEnd = true;
                }

                if (startOrEnd)
                    return true;

                var syer = GetIntParam(c, "syer", t1.Year);
                var smnth = GetIntParam(c, "smnth", t1.Month);
                var sdy = GetIntParam(c, "sdy", t1.Day);

                var eyer = GetIntParam(c, "eyer", t2.Year);
                var emnth = GetIntParam(c, "emnth", t2.Month);
                var edy = GetIntParam(c, "edy", t2.Day);

                t1 = new DateTime(syer, smnth, sdy);
                t2 = new DateTime(eyer, emnth, edy).EndOfDay();

            }
            catch (Exception e)
            {
                Logger.WriteLine(e.Message);
                return false;   
            }
            return true;
        }

        private static DateTime ParseDate(NameValueCollection c,string name)
        {
            DateTime rval = DateTime.Now.Date;
            Regex re = new Regex(@"\d{4}\-\d{1,2}\-\d{1,2}");
            string input = GetParameter(c, name, "");
            var m = re.Match(input);
            if (m.Success)
            {
                rval = DateTime.ParseExact(input, "yyyy-M-d", CultureInfo.InvariantCulture,
                DateTimeStyles.None);
            }
            else
                throw new InvalidCastException("bad start date");

            return rval;
        }

        public static int GetIntParam(NameValueCollection c, string parameterName, int defaultIfMissing)
        {
            int rval = defaultIfMissing;

            var p = GetParameter(c, parameterName, "");
            if (p != "")
                Int32.TryParse(p, out rval);

            return rval;
        }
        public static string GetParameter(NameValueCollection c, string parameterName, string defaultIfMissing = "")
        {
            if (c.AllKeys.Contains(parameterName))
            {
                return c[parameterName];
            }
            return defaultIfMissing;
        }
        public static string GetParameter(string query, string parameterName, string defaultIfMissing="")
        {

            var parts =  HttpUtility.ParseQueryString(query);
            
            if (query.IndexOf(parameterName) < 0)
                return defaultIfMissing;
            //[a-z]+\s+[a-z0-9]+
            var m = Regex.Match(query, @"(\?|\&)" + parameterName + @"\=(?<value>[a-z0-9\s]+)");

            if (m.Success)
            {
                return m.Groups["value"].Value;
            }

            return "";
        }


        public static void PrintHydrometTrailer(string message="")
        {
            Console.WriteLine("</pre>");
            Console.WriteLine("</body></html>\n\n\n");
        }
        /// <summary>
        /// This header is compatable with Legacy vms code.
        /// Some FORTRAN and other programs rely on this specific header,
        /// used by outside agencies and corporations.
        /// 
        /// </summary>
        public static void PrintHydrometHeader()
        {
            Console.WriteLine("<HTML>\n"
                + "<HEAD><TITLE>Hydromet/AgriMet Data Access</title></head>\n"
                + "<BODY BGCOLOR=#FFFFFF>");

            Console.WriteLine("<p><PRE>");
            Console.WriteLine("<B>USBR Pacific Northwest Region");
            Console.WriteLine("Hydromet/AgriMet Data Access</B><BR>");
            Console.WriteLine("Although the US Bureau of Reclamation makes efforts to maintain the accuracy");
            Console.WriteLine("of data found in the Hydromet system databases, the data is largely unverified");
            Console.WriteLine("and should be considered preliminary and subject to change.  Data and services");
            Console.WriteLine("are provided with the express understanding that the United States Government");
            Console.WriteLine("makes no warranties, expressed or implied, concerning the accuracy, complete-");
            Console.WriteLine("ness, usability or suitability for any particular purpose of the information");
            Console.WriteLine("or data obtained by access to this computer system, and the United States");
            Console.WriteLine("shall be under no liability whatsoever to any individual or group entity by");
            Console.WriteLine("reason of any use made thereof. ");
            Console.WriteLine("</PRE>");
            Console.WriteLine("<p>");
            Console.WriteLine("<PRE>");
        }

    }
}
