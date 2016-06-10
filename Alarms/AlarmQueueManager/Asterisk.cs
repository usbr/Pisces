using Reclamation.Core;
using Reclamation.TimeSeries.Alarms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmQueueManager
{
  
      ///  <summary>
      ///  Wrapper around command line asterisk program
      ///  makes calls and Reads and writes from Asterisk database.
      ///  asterisk -x "channel originate local/boia_emm@hydromet_groups extension"
      ///  asterisk -x "database put hydromet alarm_status busy"
      ///  asterisk -x "database show"
      ///  asterisk -x "database show hydromet alarm_status"
      ///  asterisk -x "database del hydromet alarm_status"
      ///  asterisk -x "dialplan reload"
      ///  </summary>
   static class Asterisk
    {

        /// <summary>
        /// originates calls on asterisk with a variable extension on the context 
        /// hydromet_groups
        /// </summary>
        internal static void Call(AlarmDataSet.alarm_queueRow alarm)
        {
            Logger.WriteLine("Making Asterisk call");
            Set("hydromet", "alarm_definition", alarm.siteid+"_"+alarm.parameter);
            Set("hydromet", "alarm_value", alarm.value.ToString());
           // Set("hydromet", "alarm_status", "busy");

            //asterisk -x "channel originate local/boia_emm@hydromet_groups extension"
            string context = "hydromet_groups";
            string cmd = "channel originate local/" + alarm.alarm_group + "@" + context + " extension";
            RunAsteriskCommand(cmd);

        }

        static void Set(string family, string key, string value)
        {
            var args = "database put " + family + " " + key + " " + value +"";
            var output  =RunAsteriskCommand(args);
        }

        private static string Get(string family="", string key="")
        {
            var output = RunAsteriskCommand("database show " + family + " " + key + "");
            for (int i = 0; i < output.Length; i++)
            {
                if( output[i].IndexOf("/"+family+"/"+key) >=0)
                {
                    return output[i].Split(':')[1].Trim();
                }
            }
            return "";
        }


        static string[] RunAsteriskCommand(string args)
        {
            var exe = ConfigurationManager.AppSettings["asterisk_executable"];
            return RunExecutable(exe, "-x \""+args+"\"");
            Logger.WriteLine("running asterisk '"+args+"'");
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

       
       //todo run command line function
        private static string[] RunExecutable(string exe, string args)
        {
            Logger.WriteLine("running :" + exe + " " + args);
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

            Logger.WriteLine("there are " + rval.Length + " lines of output ");
            return rval;
        }

        internal static bool IsBusy()
        {
            return Get("hydromet", "alarm_status") == "busy";
        }

        internal static string GetAllVariables()
        {
            Logger.WriteLine("GetAllVariable()");
            string[] output = RunAsteriskCommand("database show " );
            return String.Join("\n", output);
        }


        
    }
}
