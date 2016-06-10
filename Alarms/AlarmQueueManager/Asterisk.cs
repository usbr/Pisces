using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmQueueManager
{
  
    class Asterisk
    {
        string alarm_def; 
        double value;
        string alarm_group;
        DateTime eventTime;
        /// <summary>
        /// Creates an asterisk object that has an alarm defined with the value and 
        /// event time
        /// </summary>
        /// <param name="alarm_def"></param>
        /// <param name="value"></param>
        /// <param name="eventTime"></param>
        public Asterisk(string alarm_group, string alarm_def, double value, DateTime eventTime)
        {
            this.alarm_group = alarm_group; 
            this.alarm_def = alarm_def;
            this.value = value;
            this.eventTime = eventTime;

        }
        /// <summary>
        /// originates calls on asterisk with a variable extension on the context 
        /// hydromet_groups
        /// </summary>
        public void Call()
        {
            Set("hydromet", "alarm_definition", alarm_def);
            Set("hydromet", "alarm_value", value.ToString());
           // Set("hydromet", "alarm_status", "busy");

        }

        private void Set(string p1, string p2, string alarm_def)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// checks asterisk DB for variables to determine the status 
        /// </summary>
        public static string Status
        {
            get
            {
                return Get("hydromet", "alarm_status");
            }
        }

        private static string Get(string p1, string p2)
        {
            return "";
        }

       //todo run command line function
        private static string[] RunExecutable(string exe, string args)
        {
            //SoiUtility.LogMessage("calling static RunExecutagle(" +exe+","+args+ " )");
            Process myProcess = new Process();
            myProcess.StartInfo.FileName = exe;
            myProcess.StartInfo.Arguments = args;
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.StartInfo.RedirectStandardOutput = true;

            var started = myProcess.Start();
            if (!started)
                Console.WriteLine("Error starting process " + exe);

            string s = myProcess.StandardOutput.ReadToEnd();
            string[] rval = s.Split(new char[] { '\n' });

            myProcess.WaitForExit();
            //SoiUtility.LogMessage("the command    RunExecutagle(" +exe+","+args+ " ) has returned");
            return rval;
        }


        public static string GetVersion()
        {
            var exe = @"c:\utils\wget.exe";
            var output = RunExecutable(exe, "--version");
            foreach (var item in output)
            {
                if (item.IndexOf("GNU Wget") == 0)
                {
                    var ver = item.Substring(8);
                    return ver.Trim();
                }
            }
            return "";
        }



        internal static bool IsBusy()
        {
            throw new NotImplementedException();
        }

        internal static string GetAllVariable()
        {
            return "";
        }

       
    }
}
