using Reclamation.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Reclamation.TimeSeries.Import
{
    /// <summary>
    /// Looks for time series data in a given directory tree
    /// manages lists of files with optional 
    /// scenario, and siteid
    /// </summary>
    public class DirectoryScanner
    {
        string[] m_files;
        string[] m_siteid;

        public string[] Siteid
        {
            get { return m_siteid; }
            set { m_siteid = value; }
        }
        string[] m_scenario;

        public string[] Scenario
        {
            get { return m_scenario; }
            set { m_scenario = value; }
        }

        public string[] UniqueScenarios()
        {
            List<string> rval = new List<string>(m_scenario);
            rval.RemoveAll(x => x == "");
            return rval.Distinct().ToArray();
        }

        Regex re = null;
        public DirectoryScanner(string path, string filter, string regexPattern)
        {
            var dirs = new List<string>{path};
            dirs.AddRange(Directory.EnumerateDirectories(path, "*"));

            var files = new List<string>();
            foreach (var dir in dirs)
            {
                try
                {
                    files.AddRange(Directory.EnumerateFiles(dir, filter, SearchOption.AllDirectories));
                }
                catch (Exception ex)
                {
                    Logger.WriteLine(ex.Message, "ui");
                    continue;
                }
            }

            m_files = files.ToArray();

            if( regexPattern != "")
            {
                FindMatches(regexPattern);
            }
        }

        public void FindMatches(string regexPattern)
        {
            var regexFiles = new List<string>();
            var regexScenarios = new List<string>();
            var regexSiteIDs = new List<string>();

            re = new Regex(regexPattern, RegexOptions.IgnoreCase);

            for (int i = 0; i < m_files.Length; i++)
            {
                var fname = m_files[i];

                if (re.IsMatch(fname))
                {
                    var m = re.Matches(fname)[0];

                    regexFiles.Add(fname);

                    if (re.GetGroupNames().Contains("scenario"))
                    {
                        regexScenarios.Add(TimeSeriesDatabase.SafeTableName(m.Groups["scenario"].Value));
                    }
                    
                    if (re.GetGroupNames().Contains("siteid"))
                    {
                        regexSiteIDs.Add(m.Groups["siteid"].Value);
                    }
                }
            }

            m_files = regexFiles.ToArray();
            m_scenario = regexScenarios.ToArray();
            m_siteid = regexSiteIDs.ToArray();
        }


        public int Count
        {
            get { return m_files.Length; }
        }

        public string[] Files
        {
            get { return m_files; }
        }


    }
}
