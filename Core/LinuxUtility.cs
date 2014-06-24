using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reclamation.Core
{
    public class LinuxUtility
    {

        public static bool IsLinux()
        {
            int p = (int)Environment.OSVersion.Platform;
            if ((p == 4) || (p == 6) || (p == 128))
            {
                return true;//Console.WriteLine("Running on Unix");
            }
            else
            {
                return false; // Console.WriteLine("NOT running on Unix");
            }
        }
    }
}
