using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.IO;
namespace Reclamation.Core
{

    public static class Globals
    {
        public static string TestDataPath
        { 
            get
            {
                var s = ConfigurationManager.AppSettings["TestDataPath"];
                if (s == null || s == "")
                    throw new Exception("Error: TestDataPath is not defined in the config file");

                return s;
            }
        }


        public static string LocalConfigurationDataPath {

            get
            {

                var s = ConfigurationManager.AppSettings["LocalConfigurationDataPath"];
                if (s == null || s == "")
                    throw new Exception("Error: LocalConfigurationDataPath is not defined in the config file");

                if (!Directory.Exists(s))
                {
                    var s2 = ConfigurationManager.AppSettings["LocalConfigurationDataPath2"];
                    if (s2 != "" && s2 != null)
                        s = s2;
                }
                return s;

            }
        }

       
       
    }
    
}
