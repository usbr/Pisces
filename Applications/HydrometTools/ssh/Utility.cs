using System;
using Renci.SshNet;
using System.Text.RegularExpressions;

namespace HydrometTools.ssh
{
    public class Utility
    {
        static SshClient ssh=null;

        public static string RunCommand(string hostname,string username, string passwd,
        string expectPattern, string command)
        {
            ssh = new SshClient(hostname, username, passwd);
            
            string rval = "";
            try
            {
                    Console.WriteLine("Creating new connection to "+hostname);
                    ssh = new SshClient(hostname, username, passwd);
                    ssh.Connect();
                    var cmd = ssh.RunCommand(command);
                    rval += cmd.Result;
            }
            catch (Exception e)
            {
                throw new Exception("error with ssh expect \n" + e.Message);
            }
            finally
            {
                if (ssh != null)
                {
                    ssh.Disconnect();
                    ssh.Dispose();
                    ssh = null;
                }
            }
            rval = rval.Replace('\0', ' ');
            return rval;
        }


        public static void Close()
        {
            if (ssh != null)
            {
                ssh.Disconnect();
                ssh.Dispose();
                ssh = null;
            }
        }
    }
}
