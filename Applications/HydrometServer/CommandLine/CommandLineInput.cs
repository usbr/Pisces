using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Reclamation.TimeSeries;

namespace HydrometServer.CommandLine
{
    enum Command { Get,GetQ, Date, Exit ,Help};
    /// <summary>
    /// CommandLineInput represents single line of user input
    /// </summary>
    class CommandLineInput
    {
        /*
       Get/last/q   -- gets last flow for current day
       g
       g/ob           -get parameter
       g boii         -get all parameters for a site
       g/ob,pc/nmpi,boii
       g/a  [cbtt]
       get/all [cbtt]
       g/q
       day=yyyyMMMdd
       daily
       instant
       monthly
        
    */

        public DateTime T1, T2;
        public CommandLineInput(TimeInterval interval)
        {
            T1 = DateTime.Now.Date;
            T2 = DateTime.Now.EndOfDay();

            if (interval == TimeInterval.Daily)
            {
                T1 = DateTime.Now.Date.AddDays(-5);
                T2 = DateTime.Now.Date.AddDays(-1);
            }
        }

        string pattern = @"^(?<cmd>help|exit|ex|get|g|gq|getq|date)(?<separator1>/|\s+|\=)?(?<parm1>[\,a-z0-9]+)?"
                      +@"(?<separator2>/|\s+)?(?<parm2>[\,a-z0-1]+)?";
        string input = "";
        string sites= "";
        string pcodes = "";
        Command m_command = Command.Get;

        public string[] SiteList
        {
            get
            {
                return sites.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
        }
        public string[] Parameters
        {
            get
            {
                return pcodes.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            set
            {
                pcodes = String.Join(",",value);
            }
        }

        internal Command Command
        {
            get { return m_command; }
        }

        bool m_valid = false;

        public bool Valid
        {
            get { return m_valid; }
        }
        

        Match match;

        
        public override string ToString()
        {
            return Info("cmd") + Info("parm1") + Info("separator1") + Info("parm2") + Info("separator2");
        }

        /// <summary>
        /// Gets value of named Regex group
        /// in the format: name=value
        /// </summary>
        private string Info(string name)
        {
            return name + "='" + Value(name) + "'\n";
        }

        private string Value(string name)
        {
            var x = match.Groups[name].Value;
            var g = match.Groups;
            return x.Trim();
        }

        public void Read(string cmd)
        {
            pcodes = "";
            sites = "";
            input = cmd.ToLower();
            match = Regex.Match(input, pattern);
            m_valid = match.Success;
            
            if (m_valid)
            {
                if (Value("cmd") == "ex" || Value("cmd") == "exit")
                    m_command = Command.Exit;
                else if (Value("cmd") == "get" || Value("cmd") == "g"
                    || Value("cmd") == "getq" || Value("cmd") == "gq" )
                {
                    m_command = CommandLine.Command.Get;

                    if (Value("cmd") == "getq" || Value("cmd") == "gq")
                        m_command = CommandLine.Command.GetQ;

                    if (Value("separator1") == "/")
                    {//   g/ob   (get parameter, use existing cbtt)
                        pcodes = Value("parm1");
                    }
                    if (Value("separator1") == "" && Value("parm1")!= "")
                    {//   g  boii ( get all parameters, specify cbtt)
                        sites = Value("parm1");
                    }
                    if (Value("parm2")!= "" )
                    {   // get/ob,pc/nmpi,boii  
                        //g/ob,pc nmpi,boii 
                        sites = Value("parm2");
                    }
                }
                else if (Value("cmd") == "help")
                {
                    m_command = CommandLine.Command.Help;
                }
                else
                {
                    m_valid = false;
                }
            }
        }
    }
}
