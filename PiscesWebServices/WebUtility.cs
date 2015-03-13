using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Reclamation.TimeSeries;

namespace PiscesWebServices
{
    /// <summary>
    /// http://www.codeproject.com/Articles/9433/Understanding-CGI-with-C
    /// </summary>
    class WebUtility
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

        internal static bool GetDateRange(NameValueCollection c, out DateTime t1, out DateTime t2)
        {
             t1 = DateTime.Now.AddDays(-7);
             t2 = DateTime.Now;

            try
            {
                var syer = GetIntParam(c, "syer", t1.Year);
                var smnth = GetIntParam(c, "smnth", t1.Month);
                var sdy = GetIntParam(c, "sdy", t1.Day);

                var eyer = GetIntParam(c, "eyer", t2.Year);
                var emnth = GetIntParam(c, "emnth", t2.Month);
                var edy = GetIntParam(c, "edy", t2.Day);

                t1 = new DateTime(syer, smnth, sdy);
                t2 = new DateTime(eyer, emnth, edy).EndOfDay();

            }
            catch (Exception)
            {
                return false;   
            }
            return true;
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
            var m = Regex.Match(query, @"\?|\&" + parameterName + @"=(<value>[a-z0-9\s]+)");

            if (m.Success)
            {
                return m.Groups["value"].Value;
            }

            return "";
        }
        public static void PrintHeader()
        {
            Console.WriteLine("USBR Pacific Northwest Region");
            Console.WriteLine("Hydromet/AgriMet System Data Access");
            Console.WriteLine(" ");
            Console.WriteLine("Although the Bureau of Reclamation makes efforts to maintain the accuracy");
            Console.WriteLine("of data found in the Hydromet system databases, the data is largely unverified");
            Console.WriteLine("and should be considered preliminary and subject to change.  Data and services");
            Console.WriteLine("are provided with the express understanding that the United States Government");
            Console.WriteLine("makes no warranties, expressed or implied, concerning the accuracy, complete-");
            Console.WriteLine("ness, usability or suitability for any particular purpose of the information");
            Console.WriteLine("or data obtained by access to this computer system, and the United States");
            Console.WriteLine("shall be under no liability whatsoever to any individual or group entity by");
            Console.WriteLine("reason of any use made thereof. ");
            Console.WriteLine(" ");
        }

    }
}
