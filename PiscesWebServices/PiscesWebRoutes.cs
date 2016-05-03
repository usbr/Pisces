using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

         */

        public PiscesWebRoutes()
        {
            Get["/"] = x =>
            {
               
                return "Hello Pisces!"; // TO DO. use template feature for nice page.
            };

            Get["/site/{id}"] = parameters =>
    {
        if (((int)parameters.id) == 65)
        {
           
            return "site: #{parameters.id}! \\m/";
        }
        else
        {
            return "unknown";
        }
    };

        }
    }
}
