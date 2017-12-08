using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiscesWebServices.CGI
{
    static class CgiUtility
    {

        public static bool IsRemoteRequest()
        {
            var REMOTE_ADDR = Environment.GetEnvironmentVariable("REMOTE_ADDR");
            return REMOTE_ADDR.IndexOf("140.215.") >= 0;
        }
    }
}
