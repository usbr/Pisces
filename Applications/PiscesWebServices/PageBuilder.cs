using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PiscesWebServices
{
    class WebPageBuilder
    {
        private static TextReader template = new StreamReader(@"Views/Home.html");
        private static string webPage = template.ReadToEnd();

        public static string BuildHomePage()
        {
            string payload = @"" +
                "<strong>" +
                "<br><li><a href=https://github.com/usbr/Pisces >Source Code (GitHub Link)</a></li>" +
                "<br><li><a href=/query>Query Interface</a></li>" +
                "<br><li>Pisces Database Predefined Views</li> " +
                "</strong>" +
                "<a href=/sites>List All Sites</a>" +
                "<br><a href=/series>List All Series</a>" +
                "<br><a href=/types>List By Site Type</a>" +
                "<br><a href=/region>List By USBR Regions</a>";
            
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
                
        public static string BuildQueryPage(DataTable siteList)
        {
            string payload = "" +
            #region
@"<form action='' method=""GET"" id=""piscesForm"" target=""_blank"">" +
@"<div style=""border:5px solid #244A9F;"">" +

@"<strong>Select Pisces Data from the list: (sorted alphabetically by site name)</strong>" +
@"" +
@"<blockquote>" +
@"<select id=""tableName"" size=""10"" name=""tableName"" class=""List"" style=""min-width:90%"">" +
BuildQueryPageHtmlList() +
@"</select>" +
@"</blockquote>" +

@"<strong>Select your desired date range (MM-DD-YYYY):</strong>" +
@"" +
@"<blockquote>" +
@"<strong>From: &nbsp&nbsp</strong>" +
@"<input id=""startDate"" />" +
@"<input type=""hidden"" name=""start"" id=""sDate"">" +
@"<strong>To: &nbsp&nbsp</strong>" +
@"<input id=""endDate"" />" +
@"<input type=""hidden"" name=""end"" id=""eDate"">" +
@"<br>" +
@"You may type in your own date but you have to click on the date in the calendar for the query to work" +
@"</blockquote>" +

@"<strong>Select output format:</strong>" +
@"<blockquote>" +
@"<input type=""radio"" name=""format"" value=""csv"">CSV &nbsp;&nbsp;" +
@"<input type=""radio"" name=""format"" value=""xml"">XML &nbsp;&nbsp;" +
@"<input type=""radio"" name=""format"" value=""html"" checked=""checked"">HTML &nbsp;&nbsp;" +
@"<input type=""radio"" name=""format"" value=""chart"">Chart &nbsp;&nbsp;" +
@"</blockquote>" +

@"<center><h2>" +
@"<input type=""submit"" value=""Submit Query"" style=""font-size:18px; font-weight:bold; color:#244A9F"">" +
@"</h2></center>" +
@"</form>";
#endregion          

            return webPage.Replace("<!-- PAYLOAD -->", payload);
        }

        private static string BuildQueryPageHtmlList()
        {
            var dTab = Database.GetTableProperties();
            string queryList = @"<option value="""" "+
                    @" label="""">";
            for (int i = 0; i < dTab.Rows.Count; i++)
            {
                var dRow = dTab.Rows[i];
                queryList += @"<option value=""" + dRow[0].ToString().Trim() + @""">" +
                   dRow[1] + " " + dRow[2] + @"</option>";
            }
            return queryList;
        }
       
    }
}