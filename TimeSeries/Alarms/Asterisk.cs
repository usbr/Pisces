using Reclamation.Core;
using Renci.SshNet;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace Reclamation.TimeSeries.Alarms
{
  
      ///  <summary>
      ///  Wrapper around command line asterisk program
      /// 
      /// Assuming a single phone call at a time to keep it simple.
      /// 
      ///  makes calls and Reads and writes from Asterisk database.
      ///  asterisk -x "channel originate local/boia_emm@hydromet_groups extension"
      ///  asterisk -x "database put hydromet status busy"
      ///  asterisk -x "database show"
      ///  asterisk -x "database show hydromet status"
      ///  asterisk -x "database del hydromet status"
      ///  asterisk -x "dialplan reload"
      ///  </summary>
   public class Asterisk
    {

        string m_username="";
        string m_password="";
       /// <summary>
       /// Constructor to connect to remote linux server (with asterisk installed)
       /// </summary>
       /// <param name="username"></param>
       /// <param name="password"></param>
        public Asterisk(string username, string password)
        {
            m_username = username;
            m_password = password;
        }
         void Clear(string family)
        {
            var args = "database deltree "+family;
            var output = RunAsteriskCommand(args);
        }

         void Set(string family, string key, string value)
        {
            var args = "database put " + family + " " + key + " " + value +"";
            var output  =RunAsteriskCommand(args);
        }

        private  string Get(string family="", string key="")
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
        void HangupAllChannels()
        {
            RunAsteriskCommand("channel request hangup all");
        }


       /// <summary>
       /// string cmd = "channel originate local/main@hydromet extension" ;
       /// </summary>
       /// <param name="args"></param>
       /// <returns></returns>
         string[] RunAsteriskCommand(string args)
        {
            var exe = ConfigurationManager.AppSettings["asterisk_executable"];
            if (exe == null || exe == "")
                exe = "/usr/sbin/asterisk";

            Logger.WriteLine("running asterisk '" + args + "'");
            return RunRemoteExecutable(exe, "-x \""+args+"\"");
        }


        private  string[] RunRemoteExecutable(string exe, string args)
        {
            var host = ConfigurationManager.AppSettings["pbx_server"];
                SshClient ssh = new SshClient(host, m_username, m_password);
            //    var pkf = new PrivateKeyFile("C:key.key");
                ssh.Connect();
                var cmd = ssh.RunCommand(exe + " " + args);
                Console.WriteLine(cmd.Result);
                return cmd.Result.Split('\n');
        }

        private  string[] RunLocal(string exe, string args)
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

         public int ActiveChannels
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

         string GetAllVariables()
        {
            Logger.WriteLine("GetAllVariable()");
            string[] output = RunAsteriskCommand("database show " );
            return String.Join("\n", output);
        }


       /// <summary>
       /// copies call file to asterisk /var/spool/asterisk/outgoing/
       /// </summary>
       /// <param name="c"></param>
         public void OriginateFromCallFile(AsteriskCallFile c)
         {
             var src = c.SaveToTempFile();
             var host = ConfigurationManager.AppSettings["pbx_server"];
             var dest = "root@"+host+":/var/spool/asterisk/outgoing/"+Path.GetFileName(src);

             var args = src+" "+dest;

             Process.Start(@"c:\utils\pscp.exe ",args);
         }



    }
}
