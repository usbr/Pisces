using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Reclamation.Core
{
    public class NetworkUtility
    {
        private static bool s_intranet = false;
        private static bool knowMyIP = false;
        private static IPHostEntry s_IPHost;

        public static bool Intranet
        {
            get
            {
                if (!knowMyIP)
                {
                    s_IPHost = Dns.GetHostEntry(Dns.GetHostName());
                    foreach (var item in s_IPHost.AddressList)
                    {
                        string ip = item.ToString();
                        
                        

                        string prefix = System.Configuration.ConfigurationManager.AppSettings["InternalNetworkPrefix"];

                        if ( prefix != null && ip.IndexOf(prefix) == 0)
                        {
                            s_intranet = true;
                            Logger.WriteLine("We are inside the intranet");

                        }

                        Logger.WriteLine(ip);
                    }
                    knowMyIP = true;
                }
                return s_intranet;
            }
        }

        
    }
}
