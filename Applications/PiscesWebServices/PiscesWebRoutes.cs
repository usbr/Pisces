using Nancy;
using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PiscesWebServices
{
    public class PiscesWebRoutes : NancyModule
    {

        /*\

C:\WINDOWS\system32>netsh http add urlacl url="http://+:8080/" user="Everyone"
URL reservation successfully added
C:\WINDOWS\system32>


        https://github.com/NancyFx/Nancy/wiki/Self-Hosting-Nancy
        http://www.codeproject.com/Articles/694907/Lift-your-Petticoats-with-Nancy
        http://stackoverflow.com/questions/6845772/rest-uri-convention-singular-or-plural-name-of-resource-while-creating-it
         */

        public PiscesWebRoutes()
        {
            Get["/"] = x =>
            {
                return WebPageBuilder.BuildHomePage();
            };

            Get["/series/(?<tablename>^[A-Za-z0-9_]{1,40}$)"] = x =>
            {
                var fmt = this.Request.Query["format"].ToString();
                DateTime start = Convert.ToDateTime(Request.Query["start"].ToString());
                DateTime end = Convert.ToDateTime(Request.Query["end"].ToString());
                var tablename = x.tablename.ToString();

                DataTable tbl = Database.GetSeriesData(tablename,start,end);
                return FormatDataTable(fmt, tbl);
            };
            Get["/series/"] = x =>
                {
                    var fmt = this.Request.Query["format"].ToString();
                    var id = x.id.ToString();

                    DataTable tbl = Database.GetSeries();
                    return FormatDataTable(fmt, tbl);
                };

            Get["/sites/(?<siteid>^[A-Za-z0-9]{1,40}$)"] = x =>
                { // list paramters for a site, and other stuff?
                    var fmt = this.Request.Query["format"].ToString();
                    var siteid = x.siteid.ToString();

                    var tbl = Database.GetParameters(siteid);
                    DataTable tbl2 = Database.GetSiteProperties(siteid);
                    return FormatDataTable(fmt, tbl)
                        + "\n<br>"
                        + FormatDataTable(fmt, tbl2);
                };

            Get["/sites"] = x =>
                {
                    var fmt = this.Request.Query["format"].ToString();
                    var sites = Database.Sites;
                    return FormatDataTable(fmt, sites);
                };

            Get["/view/(?<viewname>^[A-Za-z0-9]{1,40}$)"] = x =>
            { // list paramters for a site, and other stuff?
                var fmt = this.Request.Query["format"].ToString();
                var viewname = x.viewname.ToString();

                var tbl = Database.GetView(viewname);
                return FormatDataTable(fmt, tbl);
            };

            Get["/types"] = x =>
                {
                    //var fmt = this.Request.Query["format"].ToString();
                    //var types = Database.Types;
                    return WebPageBuilder.BuildTypesPage();// FormatDataTable(fmt, types);
                };

            Get["/types/(?<typename>^[A-Za-z0-9]{1,40}$)"] = x =>
            { // list paramters for a site, and other stuff?
                var fmt = this.Request.Query["format"].ToString();
                var typeName = x.typename.ToString();

                var sites = Database.GetSiteByType(typeName);
                return FormatDataTable(fmt, sites);
            };

            Get["/query"] = x =>
                {
                    var fmt = this.Request.Query["format"].ToString();
                    var siteList = Database.Sites;
                    return WebPageBuilder.BuildQueryPage(siteList);
                };

            Get["/region"] = x =>
            {
                //var fmt = this.Request.Query["format"].ToString();
                //var types = Database.Types;
                return WebPageBuilder.BuildRegionsPage();// FormatDataTable(fmt, types);
            };

            Get["/region/(?<regionname>^[A-Za-z0-9]{1,40}$)"] = x =>
            { // list paramters for a site, and other stuff?
                var fmt = this.Request.Query["format"].ToString();
                var typeName = x.regionname.ToString();

                var sites = Database.GetSiteByRegion(typeName);
                return FormatDataTable(fmt, sites);
            };
        }


        /// <summary>
        /// Add a link to the specified column
        /// </summary>
        /// <param name="c"></param>
        /// <param name="txt"></param>
        /// <returns></returns>
        private static string FormatSiteCell(DataColumn c, DataRow r, string txt)
        {
            if( c.ColumnName =="siteid")
                return "<td> <a href=/sites/" + txt + ">"+txt  +"</a></td>";
            else if (c.ColumnName == "parameter")
            {
                string rng = "&start=" + r["start"].ToString() + "&end=" + r["end"].ToString();
                string x = "<a href=/series/" + r["tablename"].ToString() + "?"+rng+"&format={fmt}>{name}</a>";
                string href = x.Replace("{fmt}", "csv").Replace("{name}", "csv");
                href += " "+x.Replace("{fmt}", "xml").Replace("{name}", "xml");
                href += " " + x.Replace("{fmt}", "html").Replace("{name}", "html");
                href += " " + x.Replace("{fmt}", "waterml2").Replace("{name}", "waterml2");
                href += " " + x.Replace("{fmt}", "hydrojson").Replace("{name}", "hydrojson");
                href += " " + x.Replace("{fmt}", "chart").Replace("{name}", "chart");
                return "<td>"+txt+ " " + href + "</td>";
            }
            return "<td>" + txt + "</td>";
        }


        private static dynamic FormatDataTable(string fmt, DataTable sites)
        {
            if (fmt == "json")
                return DataTableOutput.ToJson(sites) + " " + fmt;
            else if (fmt == "xml")
            {
                var fn = FileUtility.GetTempFileName(".xml");
                sites.WriteXml(fn, System.Data.XmlWriteMode.WriteSchema);
                return File.ReadAllText(fn);
            }
            else if (fmt == "csv")
            {
                var fn = FileUtility.GetTempFileName(".csv");
                CsvFile.WriteToCSV(sites, fn, false);
                return File.ReadAllText(fn);
            }
            else if (fmt == "chart")
            {
                var fn = FileUtility.GetTempFileName(".html");
                var chartData = sites;
                chartData.Columns.Remove("flag");
                DyGraphWriter.writeHTML_dyGraphs(chartData, fn);
                return File.ReadAllText(fn);
            }
            else if (fmt == "waterml2")
            {
                var fn = FileUtility.GetTempFileName(".wml");
                var data = sites;
                data.Columns.Remove("flag");
                WaterMLWriter.writeWaterML2Data(data, fn);
                return File.ReadAllText(fn);
            }
            else if (fmt == "hydrojson")
            {
                var fn = FileUtility.GetTempFileName(".json");
                var data = sites;
                data.Columns.Remove("flag");
                HydroJsonWriter.writeHydroJsonData(data, fn);
                return File.ReadAllText(fn);
            }
            else // html
            {
                return DataTableOutput.ToHTML(sites, FormatSiteCell);
            }
        }
    }
}
