using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Reclamation.Core;

namespace HydrometTools.ssh
{
    /// <summary>
    /// c# Wrapper for pscp http://www.chiark.greenend.org.uk/~sgtatham/putty/download.html
    /// </summary>
    public class SecureCopy
    {

        public static bool CopyFrom(string user, string host, string passwd,
           string lfile, string rfile)
        {
            return Copy(user, host, passwd, lfile, rfile, ScpDirection.From);
        }

        public static bool CopyToWithPrivateKey(string user, string host, string identityFile, string lfile, string rfile)
        {
            string args = args = "-v  -batch \"" + lfile +"\" \""+ user + "@" + host + ":" + rfile + "\" ";
            return RunPscp("xxxxxxxxx", args);
        }

        public static bool CopyTo(string user, string host, string passwd,
           string lfile, string rfile)
        {
           return Copy(user, host, passwd, lfile, rfile, ScpDirection.To);
        }

        enum ScpDirection { To,From};

        static ProgramRunner pr = null;
        private static bool Copy(string user, string host, string passwd,
           string lfile, string rfile, ScpDirection direction)
        {
            //pscp -pw password -batch user@192.168.10.111:pscp.exe pscp.exe
            //ProgramRunner pr = new ProgramRunner();

            string args = "";

            if( direction == ScpDirection.From)
                args = "-v -pw \"" + passwd + "\" -batch \"" + user + "@" + host + ":" + rfile + "\" \"" + lfile+"\"";
            else
                if( direction == ScpDirection.To)
                    args = "-v -pw \"" + passwd + "\" -batch \"" + lfile +"\" \""+ user + "@" + host + ":" + rfile + "\" ";

            args = args.Replace("-batch", " ");

            Logger.WriteLine(args.Replace(passwd, "xxxxx"));

            return RunPscp(passwd, args);
             
        }

        private static bool RunPscp(string passwd, string args)
        {
            string dir = Path.GetDirectoryName(Application.ExecutablePath);
            var exe = Path.Combine(dir, "pscp.exe");
            if (!File.Exists(exe))
                exe = "pscp.exe";

            if (!File.Exists(exe))
                throw new FileNotFoundException(exe);

            pr = new ProgramRunner();
            pr.Run(exe, args);
            pr.SendCommand("y");

            int exitCode = pr.WaitForExit();


            var output = pr.Output;
            foreach (var item in pr.Output)
            {
                Logger.WriteLine(item);
            }

            if (exitCode != 0)
                throw new IOException("Error copying with PSCP " + args.Replace(passwd, "xxxxx"));

            return exitCode == 0;
            // var output = ProgramRunner.RunExecutable(exe, args);
            //TextFile tf = new TextFile(output);

            //if (tf.IndexOf("The server's host key is not cached in the registry.") >= 0)
            //{
            //    // try again.
            //     pr = new ProgramRunner();
            //   // pr.WriteLine += new ProgramRunner.InfoEventHandler(pr_WriteLine);   
            //    pr.Run("pscp", args.Replace("-batch",""));
            //    pr.SendCommand("y");
            //    exitCode =  pr.WaitForExit();

            //  //  tf = new TextFile(pr.Output);

            //}
        }

        static void pr_WriteLine(object sender, ProgramRunner.ProgramEventArgs e)
        {
            if( e.Message.IndexOf("Store key in cache? (y/n)") >=0)
            {
                pr.SendCommand("y");
            }
        }




    }
}
