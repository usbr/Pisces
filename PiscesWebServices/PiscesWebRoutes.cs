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
         Microsoft Windows [Version 6.1.7601]
Copyright (c) 2009 Microsoft Corporation.  All rights reserved.

C:\WINDOWS\system32>netsh http addnancy/ user=Everyone

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
               
                return "Hello Pisces!"; // TO DO. use template feature for nice page.
            };


            Get["/sites/(?<siteid>^[A-Za-z0-9]{1,40}$)"] = x =>
                { // list paramters for a site, and other stuff?
                    var fmt = this.Request.Query["format"].ToString();
                    var siteid = x.siteid.ToString();

                    var tbl = Database.GetParameters(siteid);
                    return FormatDataTable(fmt, tbl);
                };

            Get["/sites"] = x =>
                {
                    var fmt = this.Request.Query["format"].ToString();
                    var sites = Database.Sites;
                    return FormatDataTable(fmt, sites);

                };

             
    

        }


        /// <summary>
        /// Add a link to the specified column
        /// </summary>
        /// <param name="c"></param>
        /// <param name="txt"></param>
        /// <returns></returns>
        private static string FormatSiteCell(DataColumn c, string txt)
        {
            if( c.ColumnName =="siteid")
                return "<td> <a href=/sites/" + txt + ">"+txt  +"</a></td>";
            else
            return "<td>" + txt + "</td>";
        }


        private static dynamic FormatDataTable(string fmt, DataTable sites)
        {
            if (fmt == "json")
                return DataTableOutput.ToJson(sites) + " " + fmt;
            else if(fmt == "xml")
            {
                var fn = FileUtility.GetTempFileName(".xml");
                sites.WriteXml(fn, System.Data.XmlWriteMode.WriteSchema);
                return File.ReadAllText(fn) + " " + fmt;
            }
            else if ( fmt == "csv")
            {
                return "todo csv";
            }
            else // html
            {
                return DataTableOutput.ToHTML(sites,FormatSiteCell);
            }
        }
    }
}
