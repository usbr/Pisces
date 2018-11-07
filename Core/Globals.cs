using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
namespace Reclamation.Core
{

    public static class Globals
    {
        /// <summary>
        /// Returns Test data path, in the source code tree.
        /// </summary>
        public static string TestDataPath
        { 
            get
            {
                string dir = GetPiscesDir();

                dir = Path.Combine(dir, "PiscesTestData", "data");

                return dir;
            }
        }

        private static string GetPiscesDir()
        {
            string dir = System.AppDomain.CurrentDomain.BaseDirectory;

            int idx = dir.ToLower().IndexOf("pisces");

            if (idx >= 0)
            {
                dir = dir.Substring(0, idx + "Pisces".Length);
            }

            return dir;
        }

        /// <summary>
        /// Returns cfg data path, in the source code tree.
        /// </summary>
        public static string CfgDataPath
        {
            get
            {
                string dir = GetPiscesDir();
                dir = Path.Combine(dir, "Applications", "cfg");
                return dir;
            }
        }



        public static string LocalConfigurationDataPath {

            get
            {

                var s = ConfigurationManager.AppSettings["LocalConfigurationDataPath"];
                if (s == null || s == "")
                {
                    Logger.WriteLine("Error: LocalConfigurationDataPath is not defined in the config file");
                    return "";
                }

                return s;

            }
        }
       
       
    }
    
}
