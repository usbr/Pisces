using System;
using System.Collections.Generic;
using System.Text;

namespace Reclamation.Core
{
    public static class WindowsUtility
    {

        public static string GetUserName()
        {
         return System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
        }

        public static string GetShortUserName()
        {
            string s = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();

            var idx = s.IndexOf("\\");
            if (idx >= 0)
                return s.Substring(idx + 1).ToLower();
            return s.ToLower();
        }

        public static string GetMachineName()
        {
            return System.Environment.MachineName;
        }
    }
}
