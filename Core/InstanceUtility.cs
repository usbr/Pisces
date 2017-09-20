using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Reclamation.Core
{

    /// <summary>
    /// Instance Utility can be used to 
    /// prevent multiple instances running
    /// </summary>
    public class InstanceUtility
    {

        /// <summary>
        /// http://stackoverflow.com/questions/1545270/how-to-determine-if-a-process-id-exists
        /// </summary>
        /// <returns></returns>
        public static bool IsAnotherInstanceRunning()
        {
            if( File.Exists(FileName()))
            {
                var s = File.ReadAllText(FileName());
                var id = Convert.ToInt32(s);
               return  Process.GetProcesses().Any(x => x.Id == id);
            }

            return false;
        }


        private static string FileName()
        {
            return FileUtility.GetExecutableDirectory() + ".pid";
        }

        /// <summary>
        /// Creates a file  Executable.exe.pid
        /// the contents are the process id.
        /// </summary>
        public static void CreateProcessIdFile()
        {
            int id = Process.GetCurrentProcess().Id;
            File.WriteAllText(FileName(), id.ToString());
        }

        public static void TouchProcessFile()
        {
            System.IO.File.SetLastWriteTime(FileName(),DateTime.Now);
        }

        public static void DeleteProcessIdFile()
        {
            if (File.Exists(FileName()))
                File.Delete(FileName());
        }
    }
}
