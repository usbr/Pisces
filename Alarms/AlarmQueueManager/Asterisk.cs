using Reclamation.Core;
using Renci.SshNet;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace AlarmQueueManager
{
  
      ///  <summary>
      ///  Wrapper around command line asterisk program
      /// 
      /// Assuming a single phone call at a time to keep it simple.
      /// 
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

       private static Stopwatch stopwatch1;

        public static int MinutesElapsed
        {
            get
            {
                var a = stopwatch1.ElapsedMilliseconds;

                return (int)a / 1000 / 60; 
            }
        }

        /// <summary>
        /// originates calls on asterisk with a variable extension on the context 
        /// hydromet_groups
        /// </summary>
        internal static void Call(string siteId, string parameter, string value,string[] phoneNumbers)
        {
            stopwatch1 = new Stopwatch();
            stopwatch1.Restart();

            HangupAllChannels(); // make sure phone is clear.

            Logger.WriteLine("Making Asterisk call");
            
            Clear("hydromet");

            Set("hydromet", "siteid", siteId);
            Set("hydromet", "parameter", parameter);
            Set("hydromet", "value", value);
            Set("hydromet", "sound_file", siteId + "_" + parameter);

            for (int i = 1; i <= phoneNumbers.Length; i++)
            {
                Set("hydromet", "phone"+i, phoneNumbers[i-1]);    
            }
            

            //;asterisk -rx "channel originate local/main@hydromet extension "            
            string cmd = "channel originate local/main@hydromet extension" ;
            RunAsteriskCommand(cmd);

        }
        static void Clear(string family)
        {
            var args = "database  database deltree "+family;
            var output = RunAsteriskCommand(args);
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
                // ast_cli(a->fd, "%-50s: %-25s\n", key_s, value_s); 
                if( output[i].IndexOf("/"+family+"/"+key) >=0)
                {
                    var x = output[i].Substring(51).Trim();
                    return x;
                }
            }
            return "";
        }


//       channel request hangup all
       static void HangupAllChannels()
        {
            RunAsteriskCommand("channel request hangup all");
        }


        static string[] RunAsteriskCommand(string args)
        {
            var exe = ConfigurationManager.AppSettings["asterisk_executable"];
            Logger.WriteLine("running asterisk '" + args + "'");
            return RunExecutable(exe, "-x \""+args+"\"");
            
        }

        public static string ConfirmedBy { 
            get{
                return Get("hydromet", "confirmed_by");
            }
        }

       /// <summary>
       /// latest log message
       /// </summary>
        public static string Log
        {
            get
            {
                return Get("hydromet", "log");
            }
        }
        public static string LogTime
        {
            get
            {
                return Get("hydromet", "log_time");
            }
        }

        /// <summary>
        /// checks asterisk DB for variables to determine the status 
        /// </summary>
        public static string Status
        {
            get
            {
                return Get("hydromet", "status");
            }
        }

        public static DateTime StatusTime
        {
            get
            {
                var x = Get("hydromet", "status_time");
                Console.WriteLine(x);
                return DateTime.Parse(x);
            }
        }

       
       //todo run command line function
        private static string[] RunExecutable(string exe, string args)
        {
            if (LinuxUtility.IsLinux())
            {
                return RunLocal(exe, args);
            }
            else
            {// run remote
                var tokens = File.ReadAllLines(@"c:\utils\linux\dectalk.txt");
                SshClient ssh = new SshClient("dectalk",tokens[0],tokens[1]);
                ssh.Connect();
                var cmd = ssh.RunCommand(exe +" "+args);
                Console.WriteLine(cmd.Result);
                return cmd.Result.Split('\n');
            }
        }

        private static string[] RunLocal(string exe, string args)
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

        internal static int ActiveChannels
        {
            get
            {
                var output = RunAsteriskCommand("core show channels");
                TextFile tf = new TextFile(output);

                int idx = tf.IndexOf("active channel");
                if( idx < 0)
                {
                    throw new Exception("Error: index to active channels failed");
                }

                int idx2  = tf[idx].IndexOf("active channel");
                var s = tf[idx].Substring(0, idx2);
                return Convert.ToInt32(s);
            }
        }



         static string GetAllVariables()
        {
            Logger.WriteLine("GetAllVariable()");
            string[] output = RunAsteriskCommand("database show " );
            return String.Join("\n", output);
        }




      
    }
}
