using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HdbApi
{
    class WebPageBuilder
    {
        private static TextReader template = new StreamReader(@"Views/template.html");
        private static string webPage = template.ReadToEnd();

        public static string BuildHomePage()
        {
            string payload = @"" +
                "<strong>" +
                "<br><li><a href=https://github.com/usbr/Pisces >Source Code (GitHub Link)</a></li>" +
                "<br><li><a href=/query>Query Interface</a></li>" +
                "<br><li>HDB API Operations</li> " +
                "</strong>" +
                "<a href=/tsquery>Time Series Data Query</a>";
                //"<a href=/query>List All Series</a>" +
                //"<br><a href=/sites>List All Sites</a>" +
                //"<br><a href=/datatypes>List All Datatypes</a>" +
                //"<br><a href=/db>List DB Instances</a>";

            return webPage.Replace("<!-- PAYLOAD -->", payload);
        }

        public static string BuildTypesPage()
        {
            string payload = @"" +
                "<strong>" +
                "<br><li><a href=/types/agrimet>Agrimet Stations</a></li>" +
                "<br><li><a href=/types/canal>Canals</a></li>" +
                "<br><li><a href=/types/reservoir>Reservoirs and Dams</a></li>" +
                "<br><li><a href=/types/snotel>Snotel Sites</a></li>" +
                "<br><li><a href=/types/stream>Stream Gages</a></li>" +
                "<br><li><a href=/types/weather>Weather Stations</a></li>" +
                "</strong>";
            return webPage.Replace("<!-- PAYLOAD -->", payload);
        }

        public static string BuildRegionsPage()
        {
            string payload = @"" +
                "<strong>" +
                "<br><li><a href=/region/PN>Pacific Northwest</a></li>" +
                "<br><li><a href=/region/GP>Great Plains</a></li>" +
                "<br><li><a href=/region/LC>Lower Colorado</a></li>" +
                "<br><li><a href=/region/UC>Upper Colorado</a></li>" +
                "<br><li><a href=/region/MP>Mid-Pacific</a></li>" +
                "</strong>";
            return webPage.Replace("<!-- PAYLOAD -->", payload);
        }


    }
}