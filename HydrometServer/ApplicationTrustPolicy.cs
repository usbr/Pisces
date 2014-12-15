using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Security;

namespace HydrometServer
{
    /// <summary>
    /// Hack to allow downloads from Idaho Power web site
    /// </summary>
    class ApplicationTrustPolicy  
    {


        internal static void TrustAll()
        {
            ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;
        }

        public static bool ValidateServerCertificate(
      object sender,
      X509Certificate certificate,
      X509Chain chain,
      SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }


    }
}
