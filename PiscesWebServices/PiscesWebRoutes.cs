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


            Get["/sites/{id}"] = x =>
                {
                    var fmt = this.Request.Query["format"].ToString();
                    var id = x.id.ToString();
                    if (Regex.IsMatch(id, "[A-Za-z0-9]{1,20}"))
                        return "bad site id";
                    var tbl = Database.DB().GetSiteCatalog("siteid = '" + id + "'");
                    return FormatDataTable(fmt, tbl);
                };

            Get["/sites"] = x =>
                {
                    var fmt = this.Request.Query["format"].ToString();

                    var sites = Database.DB().GetSiteCatalog();

                    return FormatDataTable(fmt, sites);

                };

             
    

        }

        private static dynamic FormatDataTable(string fmt, DataTable sites)
        {
            if (fmt == "json")
                return DataTableOutput.ToJson(sites) + " " + fmt;
            else
            {
                var fn = FileUtility.GetTempFileName(".xml");
                sites.WriteXml(fn, System.Data.XmlWriteMode.WriteSchema);
                return File.ReadAllText(fn) + " " + fmt;
            }
        }
    }
}
